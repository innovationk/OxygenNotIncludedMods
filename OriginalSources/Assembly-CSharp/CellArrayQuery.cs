// Decompiled with JetBrains decompiler
// Type: CellArrayQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class CellArrayQuery : PathFinderQuery
{
  private int[] targetCells;

  public CellArrayQuery Reset(int[] target_cells)
  {
    this.targetCells = target_cells;
    return this;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    for (int index = 0; index < this.targetCells.Length; ++index)
    {
      if (this.targetCells[index] == cell)
        return true;
    }
    return false;
  }
}
