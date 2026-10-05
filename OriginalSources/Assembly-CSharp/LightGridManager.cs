// Decompiled with JetBrains decompiler
// Type: LightGridManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class LightGridManager
{
  public const float DEFAULT_FALLOFF_RATE = 0.5f;
  public static List<Tuple<int, int>> previewLightCells = new List<Tuple<int, int>>();
  public static int[] previewLux;

  public static int ComputeFalloff(
    float fallOffRate,
    int cell,
    int originCell,
    LightShape lightShape,
    DiscreteShadowCaster.Direction lightDirection)
  {
    int num = originCell;
    if (lightShape == LightShape.Quad)
    {
      Vector2I xy1 = Grid.CellToXY(num);
      Vector2I xy2 = Grid.CellToXY(cell);
      switch (lightDirection)
      {
        case DiscreteShadowCaster.Direction.North:
        case DiscreteShadowCaster.Direction.South:
          Vector2I vector2I1 = new Vector2I(xy2.x, xy1.y);
          num = Grid.XYToCell(vector2I1.x, vector2I1.y);
          break;
        case DiscreteShadowCaster.Direction.East:
        case DiscreteShadowCaster.Direction.West:
          Vector2I vector2I2 = new Vector2I(xy1.x, xy2.y);
          num = Grid.XYToCell(vector2I2.x, vector2I2.y);
          break;
      }
    }
    return LightGridManager.CalculateFalloff(fallOffRate, cell, num);
  }

  private static int CalculateFalloff(float falloffRate, int cell, int origin)
  {
    return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float) Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
  }

  public static void Initialise() => LightGridManager.previewLux = new int[Grid.CellCount];

  public static void Shutdown()
  {
    LightGridManager.previewLux = (int[]) null;
    LightGridManager.previewLightCells.Clear();
  }

  public static void DestroyPreview()
  {
    foreach (Tuple<int, int> previewLightCell in LightGridManager.previewLightCells)
      LightGridManager.previewLux[previewLightCell.first] = 0;
    LightGridManager.previewLightCells.Clear();
  }

  public static void CreatePreview(int origin_cell, float radius, LightShape shape, int lux)
  {
    LightGridManager.CreatePreview(origin_cell, radius, shape, lux, 0, DiscreteShadowCaster.Direction.South);
  }

  public static void CreatePreview(
    int origin_cell,
    float radius,
    LightShape shape,
    int lux,
    int width,
    DiscreteShadowCaster.Direction direction)
  {
    LightGridManager.previewLightCells.Clear();
    ListPool<int, LightGridManager.LightGridEmitter>.PooledList visiblePoints = ListPool<int, LightGridManager.LightGridEmitter>.Allocate();
    visiblePoints.Add(origin_cell);
    DiscreteShadowCaster.GetVisibleCells(origin_cell, (List<int>) visiblePoints, (int) radius, width, direction, shape);
    foreach (int index in (List<int>) visiblePoints)
    {
      if (Grid.IsValidCell(index))
      {
        int b = lux / LightGridManager.ComputeFalloff(0.5f, index, origin_cell, shape, direction);
        LightGridManager.previewLightCells.Add(new Tuple<int, int>(index, b));
        LightGridManager.previewLux[index] = b;
      }
    }
    visiblePoints.Recycle();
  }

  public class LightGridEmitter
  {
    private LightGridManager.LightGridEmitter.State state = LightGridManager.LightGridEmitter.State.DEFAULT;
    private List<int> litCells = new List<int>();

    public void UpdateLitCells()
    {
      DiscreteShadowCaster.GetVisibleCells(this.state.origin, this.litCells, (int) this.state.radius, this.state.width, this.state.direction, this.state.shape);
    }

    public void AddToGrid(bool update_lit_cells)
    {
      DebugUtil.DevAssert(!update_lit_cells || this.litCells.Count == 0, "adding an already added emitter");
      if (update_lit_cells)
        this.UpdateLitCells();
      foreach (int litCell in this.litCells)
      {
        if (Grid.IsValidCell(litCell))
        {
          int num = Mathf.Max(0, Grid.LightCount[litCell] + this.ComputeLux(litCell));
          Grid.LightCount[litCell] = num;
          LightGridManager.previewLux[litCell] = num;
        }
      }
    }

    public void RemoveFromGrid()
    {
      foreach (int litCell in this.litCells)
      {
        if (Grid.IsValidCell(litCell))
        {
          Grid.LightCount[litCell] = Mathf.Max(0, Grid.LightCount[litCell] - this.ComputeLux(litCell));
          LightGridManager.previewLux[litCell] = 0;
        }
      }
      this.litCells.Clear();
    }

    public bool Refresh(LightGridManager.LightGridEmitter.State state, bool force = false)
    {
      if (!force && EqualityComparer<LightGridManager.LightGridEmitter.State>.Default.Equals(this.state, state))
        return false;
      this.RemoveFromGrid();
      this.state = state;
      this.AddToGrid(true);
      return true;
    }

    private int ComputeLux(int cell) => this.state.intensity / this.ComputeFalloff(cell);

    private int ComputeFalloff(int cell)
    {
      return LightGridManager.ComputeFalloff(this.state.falloffRate, cell, this.state.origin, this.state.shape, this.state.direction);
    }

    [Serializable]
    public struct State : IEquatable<LightGridManager.LightGridEmitter.State>
    {
      public int origin;
      public LightShape shape;
      public int width;
      public DiscreteShadowCaster.Direction direction;
      public float radius;
      public int intensity;
      public float falloffRate;
      public Color colour;
      public static readonly LightGridManager.LightGridEmitter.State DEFAULT = new LightGridManager.LightGridEmitter.State()
      {
        origin = Grid.InvalidCell,
        shape = LightShape.Circle,
        radius = 4f,
        intensity = 1,
        falloffRate = 0.5f,
        colour = Color.white,
        direction = DiscreteShadowCaster.Direction.South,
        width = 4
      };

      public bool Equals(LightGridManager.LightGridEmitter.State rhs)
      {
        return this.origin == rhs.origin && this.shape == rhs.shape && (double) this.radius == (double) rhs.radius && this.intensity == rhs.intensity && (double) this.falloffRate == (double) rhs.falloffRate && this.colour == rhs.colour && this.width == rhs.width && this.direction == rhs.direction;
      }
    }
  }
}
