// Decompiled with JetBrains decompiler
// Type: BestFit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using System;
using System.Collections.Generic;
using TUNING;

#nullable disable
public class BestFit
{
  public static Vector2I BestFitWorlds(List<WorldPlacement> worldsToArrange, bool ignoreBestFitY = false)
  {
    List<BestFit.Rect> placed = new List<BestFit.Rect>();
    Vector2I vector2I = new Vector2I();
    List<WorldPlacement> worldPlacementList = new List<WorldPlacement>((IEnumerable<WorldPlacement>) worldsToArrange);
    worldPlacementList.Sort((Comparison<WorldPlacement>) ((a, b) => b.height.CompareTo(a.height)));
    int height = worldPlacementList[0].height;
    foreach (WorldPlacement worldPlacement in worldPlacementList)
    {
      Vector2I pos = new Vector2I();
      while (!BestFit.UnoccupiedSpace(new BestFit.Rect(pos.x, pos.y, worldPlacement.width, worldPlacement.height), placed))
      {
        if (ignoreBestFitY)
          ++pos.x;
        else if (pos.y + worldPlacement.height >= height + 32 /*0x20*/)
        {
          pos.y = 0;
          ++pos.x;
        }
        else
          ++pos.y;
      }
      vector2I.x = Math.Max(worldPlacement.width + pos.x, vector2I.x);
      vector2I.y = Math.Max(worldPlacement.height + pos.y, vector2I.y);
      placed.Add(new BestFit.Rect(pos.x, pos.y, worldPlacement.width, worldPlacement.height));
      worldPlacement.SetPosition(pos);
    }
    if (DlcManager.FeatureClusterSpaceEnabled())
    {
      vector2I.x += 136;
      vector2I.y = Math.Max(vector2I.y, 136);
    }
    return vector2I;
  }

  private static bool UnoccupiedSpace(BestFit.Rect RectA, List<BestFit.Rect> placed)
  {
    foreach (BestFit.Rect rect in placed)
    {
      if (RectA.X1 < rect.X2 && RectA.X2 > rect.X1 && RectA.Y1 < rect.Y2 && RectA.Y2 > rect.Y1)
        return false;
    }
    return true;
  }

  public static Vector2I GetGridOffset(
    IList<WorldContainer> existingWorlds,
    Vector2I newWorldSize,
    out Vector2I newWorldOffset)
  {
    List<BestFit.Rect> placed = new List<BestFit.Rect>();
    foreach (WorldContainer existingWorld in (IEnumerable<WorldContainer>) existingWorlds)
      placed.Add(new BestFit.Rect(existingWorld.WorldOffset.x, existingWorld.WorldOffset.y, existingWorld.WorldSize.x, existingWorld.WorldSize.y));
    Vector2I gridOffset = new Vector2I(Grid.WidthInCells, 0);
    int widthInCells = Grid.WidthInCells;
    Vector2I vector2I = new Vector2I();
    while (!BestFit.UnoccupiedSpace(new BestFit.Rect(vector2I.x, vector2I.y, newWorldSize.x, newWorldSize.y), placed))
    {
      if (vector2I.x + newWorldSize.x >= widthInCells)
      {
        vector2I.x = 0;
        ++vector2I.y;
      }
      else
        ++vector2I.x;
    }
    Debug.Assert(vector2I.x + newWorldSize.x <= Grid.WidthInCells, (object) "BestFit is trying to expand the grid width, this is unsupported and will break the SIM.");
    gridOffset.y = Math.Max(newWorldSize.y + vector2I.y, Grid.HeightInCells);
    newWorldOffset = vector2I;
    return gridOffset;
  }

  public static int CountRocketInteriors(IList<WorldContainer> existingWorlds)
  {
    int num = 0;
    List<BestFit.Rect> placedWorlds = new List<BestFit.Rect>();
    foreach (WorldContainer existingWorld in (IEnumerable<WorldContainer>) existingWorlds)
      placedWorlds.Add(new BestFit.Rect(existingWorld.WorldOffset.x, existingWorld.WorldOffset.y, existingWorld.WorldSize.x, existingWorld.WorldSize.y));
    Vector2I rocketInteriorSize = ROCKETRY.ROCKET_INTERIOR_SIZE;
    Vector2I newWorldOffset;
    while (BestFit.PlaceWorld(placedWorlds, rocketInteriorSize, out newWorldOffset))
    {
      ++num;
      placedWorlds.Add(new BestFit.Rect(newWorldOffset.x, newWorldOffset.y, rocketInteriorSize.x, rocketInteriorSize.y));
    }
    return num;
  }

  private static bool PlaceWorld(
    List<BestFit.Rect> placedWorlds,
    Vector2I newWorldSize,
    out Vector2I newWorldOffset)
  {
    Vector2I vector2I1 = new Vector2I(Grid.WidthInCells, 0);
    int widthInCells = Grid.WidthInCells;
    Vector2I vector2I2 = new Vector2I();
    while (!BestFit.UnoccupiedSpace(new BestFit.Rect(vector2I2.x, vector2I2.y, newWorldSize.x, newWorldSize.y), placedWorlds))
    {
      if (vector2I2.x + newWorldSize.x >= widthInCells)
      {
        vector2I2.x = 0;
        ++vector2I2.y;
      }
      else
        ++vector2I2.x;
    }
    vector2I1.y = Math.Max(newWorldSize.y + vector2I2.y, Grid.HeightInCells);
    newWorldOffset = vector2I2;
    return vector2I2.x + newWorldSize.x <= Grid.WidthInCells && vector2I2.y + newWorldSize.y <= Grid.HeightInCells;
  }

  private struct Rect(int x, int y, int width, int height)
  {
    private int x = x;
    private int y = y;
    private int width = width;
    private int height = height;

    public int X1 => this.x;

    public int X2 => this.x + this.width + 2;

    public int Y1 => this.y;

    public int Y2 => this.y + this.height + 2;
  }
}
