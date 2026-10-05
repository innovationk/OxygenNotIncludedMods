// Decompiled with JetBrains decompiler
// Type: SpaceTreeSeededComet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpaceTreeSeededComet : Comet
{
  protected override void DepositTiles(
    int cell,
    Element element,
    int world,
    int prev_cell,
    float temperature)
  {
    int depthOfElement = this.GetDepthOfElement(cell, element, world);
    float num1 = 1f;
    int addTilesMinHeight = this.addTilesMinHeight;
    float f = (float) (depthOfElement - addTilesMinHeight) / (float) (this.addTilesMaxHeight - this.addTilesMinHeight);
    if (!float.IsNaN(f))
      num1 -= f;
    int num2 = Mathf.Min(this.addTiles, Mathf.Clamp(Mathf.RoundToInt((float) this.addTiles * num1), 1, this.addTiles));
    ListPool<int, Comet>.PooledList validCells = ListPool<int, Comet>.Allocate();
    FloodFill.BreadthCollect(prev_cell, new Func<int, FloodFill.BoundaryCheckResult>(condition), (List<int>) validCells, 11);
    float mass = num2 > 0 ? this.addTileMass / (float) this.addTiles : 1f;
    int disease_count = this.addDiseaseCount / num2;
    float num3 = UnityEngine.Random.value;
    float num4 = num2 == 0 ? -1f : 1f / (float) num2;
    float num5 = 0.0f;
    bool flag1 = false;
    foreach (int num6 in (List<int>) validCells)
    {
      int viable_cell = num6;
      if (num2 > 0)
      {
        num5 += num4;
        bool flag2 = !flag1 && (double) num4 >= 0.0 && (double) num3 <= (double) num5;
        int callbackIdx = flag2 ? Game.Instance.callbackManager.Add(new Game.CallbackInfo((System.Action) (() => SpaceTreeSeededComet.PlantTreeOnSolidTileCreated(viable_cell, this.addTilesMaxHeight)))).index : -1;
        SimMessages.AddRemoveSubstance(viable_cell, element.id, CellEventLogger.Instance.ElementEmitted, mass, temperature, this.diseaseIdx, disease_count, callbackIdx: callbackIdx);
        --num2;
        flag1 |= flag2;
      }
      else
        break;
    }
    validCells.Recycle();

    FloodFill.BoundaryCheckResult condition(int cell)
    {
      return !Grid.IsValidCellInWorld(cell, world) || Grid.Solid[cell] ? FloodFill.BoundaryCheckResult.Halt : FloodFill.BoundaryCheckResult.Continue;
    }
  }

  private static void PlantTreeOnSolidTileCreated(int cell, int tileMaxHeight)
  {
    byte worldIdx = Grid.WorldIdx[cell];
    int num1 = 2;
    int num2 = Grid.OffsetCell(cell, new CellOffset(0, tileMaxHeight));
    bool flag = false;
    if (!Grid.IsValidCell(cell))
      return;
    int num3;
    do
    {
      num3 = num2;
      num2 = Grid.OffsetCell(num3, 0, -1);
      if (!Grid.IsValidCell(num2))
        return;
      if (Grid.Solid[num2] && SpaceTreeSeededComet.CanGrowOnCell(num3, worldIdx))
        flag = true;
      --num1;
    }
    while (!flag && num1 > 0);
    if (!flag)
      return;
    GameObject prefab = Assets.GetPrefab((Tag) "SpaceTree");
    KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
    Util.KInstantiate(prefab, Grid.CellToPosCBC(num3, component.sceneLayer)).SetActive(true);
  }

  public static bool CanGrowOnCell(int spawnCell, byte worldIdx)
  {
    CellOffset[] occupiedCellsOffsets = Assets.GetPrefab((Tag) "SpaceTree").GetComponent<OccupyArea>().OccupiedCellsOffsets;
    bool flag = true;
    for (int index1 = 0; flag && index1 < occupiedCellsOffsets.Length; ++index1)
    {
      int index2 = Grid.OffsetCell(spawnCell, occupiedCellsOffsets[index1]);
      flag = flag && Grid.IsValidCellInWorld(index2, (int) worldIdx) && (!Grid.IsSolidCell(index2) || Grid.Element[index2].HasTag(GameTags.Unstable)) && (UnityEngine.Object) Grid.Objects[index2, 1] == (UnityEngine.Object) null && (UnityEngine.Object) Grid.Objects[index2, 5] == (UnityEngine.Object) null && !Grid.Foundation[index2];
    }
    return flag;
  }
}
