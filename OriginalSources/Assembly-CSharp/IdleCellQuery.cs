// Decompiled with JetBrains decompiler
// Type: IdleCellQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class IdleCellQuery : PathFinderQuery
{
  private MinionBrain brain;
  private int targetCell;
  private int maxCost;
  private bool canSwim;

  public IdleCellQuery Reset(MinionBrain brain, int max_cost, bool can_swim = false)
  {
    this.brain = brain;
    this.maxCost = max_cost;
    this.targetCell = Grid.InvalidCell;
    this.canSwim = can_swim;
    return this;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain);
    if (((flags & SafeCellQuery.SafeFlags.IsClear) == (SafeCellQuery.SafeFlags) 0 || (flags & SafeCellQuery.SafeFlags.IsNotLadder) == (SafeCellQuery.SafeFlags) 0 || (flags & SafeCellQuery.SafeFlags.IsNotTube) == (SafeCellQuery.SafeFlags) 0 ? 0 : ((flags & SafeCellQuery.SafeFlags.IsBreathable) != 0 ? 1 : 0)) != 0)
    {
      if ((flags & SafeCellQuery.SafeFlags.IsNotLiquid) != (SafeCellQuery.SafeFlags) 0)
        this.targetCell = cell;
      else if (this.canSwim && (flags & SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace) != (SafeCellQuery.SafeFlags) 0)
        this.targetCell = cell;
    }
    return cost > this.maxCost;
  }

  public override int GetResultCell() => this.targetCell;
}
