// Decompiled with JetBrains decompiler
// Type: SafeResuscitateCellQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SafeResuscitateCellQuery : PathFinderQuery
{
  private int targetCell;
  private int targetCost;
  private OxygenBreather oxygenBreather;

  public SafeResuscitateCellQuery Reset(OxygenBreather oxygen_breather)
  {
    this.targetCell = PathFinder.InvalidCell;
    this.targetCost = int.MaxValue;
    this.oxygenBreather = oxygen_breather;
    return this;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    int index = Grid.CellAbove(cell);
    if (!Grid.IsValidCell(index) || Grid.Solid[cell] || Grid.Solid[index])
      return false;
    int num = Grid.CellBelow(cell);
    if (!Grid.IsValidCell(num) || !Grid.Solid[num] || Grid.IsSubstantialLiquid(cell) || Grid.Element[index].IsLiquid)
      return false;
    if ((Object) this.oxygenBreather != (Object) null)
    {
      if (!GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(cell, new CellOffset[2]
      {
        CellOffset.none,
        CellOffset.up
      }, this.oxygenBreather).IsBreathable)
        return false;
    }
    if (cost < this.targetCost)
    {
      this.targetCost = cost;
      this.targetCell = cell;
    }
    return false;
  }

  public override int GetResultCell() => this.targetCell;
}
