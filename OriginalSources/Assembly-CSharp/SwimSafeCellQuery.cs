// Decompiled with JetBrains decompiler
// Type: SwimSafeCellQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class SwimSafeCellQuery : PathFinderQuery
{
  private const float SECONDS_PER_COST = 0.1f;
  private const float BREATH_SAFETY_MARGIN_SECONDS = 15f;
  private MinionBrain brain;
  private int targetCell;
  private int targetCost;
  public SafeCellQuery.SafeFlags targetCellFlags;
  private bool avoid_light;
  private SafeCellQuery.SafeFlags ignoredFlags;
  private float maxSubmergedCost;
  private int[] routeSubmergedCost;
  private int targetRouteSubmergedCost;

  public SwimSafeCellQuery Reset(
    MinionBrain brain,
    bool avoid_light,
    SafeCellQuery.SafeFlags ignoredFlags,
    float currentBreathValue)
  {
    this.brain = brain;
    this.targetCell = PathFinder.InvalidCell;
    this.targetCost = int.MaxValue;
    this.targetCellFlags = (SafeCellQuery.SafeFlags) 0;
    this.avoid_light = avoid_light;
    this.targetRouteSubmergedCost = int.MaxValue;
    this.ignoredFlags = ignoredFlags | SafeCellQuery.SafeFlags.IsNotLiquid | SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace | SafeCellQuery.SafeFlags.IsNotSwimming;
    float breathRate = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE;
    this.maxSubmergedCost = Mathf.Max(0.0f, ((double) breathRate > 0.0 ? currentBreathValue / breathRate : 0.0f) - 15f) / 0.1f;
    if (this.routeSubmergedCost == null || this.routeSubmergedCost.Length != Grid.CellCount)
      this.routeSubmergedCost = new int[Grid.CellCount];
    int cell = Grid.PosToCell((KMonoBehaviour) brain);
    if (Grid.IsValidCell(cell))
      this.routeSubmergedCost[cell] = 0;
    return this;
  }

  private static bool IsCellSubmerged(int cell)
  {
    if (!Grid.Element[cell].IsLiquid)
      return false;
    int cell1 = Grid.CellAbove(cell);
    return Grid.IsValidCell(cell1) && Grid.Element[cell1].IsLiquid;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain, this.avoid_light, this.ignoredFlags);
    int num1 = 0;
    if (Grid.IsValidCell(parent_cell))
      num1 = this.routeSubmergedCost[parent_cell];
    int num2;
    if (SwimSafeCellQuery.IsCellSubmerged(cell))
    {
      int num3 = 9;
      num2 = num1 + num3;
    }
    else
      num2 = 0;
    this.routeSubmergedCost[cell] = num2;
    if ((double) num2 > (double) this.maxSubmergedCost)
      return false;
    int num4 = flags > this.targetCellFlags ? 1 : 0;
    int num5 = flags == this.targetCellFlags ? 1 : 0;
    bool flag1 = num5 != 0 && num2 < this.targetRouteSubmergedCost;
    bool flag2 = (num5 == 0 ? 0 : (num2 == this.targetRouteSubmergedCost ? 1 : 0)) != 0 && cost < this.targetCost;
    int num6 = flag1 ? 1 : 0;
    if ((num4 | num6 | (flag2 ? 1 : 0)) != 0)
    {
      this.targetCellFlags = flags;
      this.targetRouteSubmergedCost = num2;
      this.targetCost = cost;
      this.targetCell = cell;
    }
    return (SafeCellQuery.SafeFlags.AllSafeFlags & ~(flags | this.ignoredFlags)) == (SafeCellQuery.SafeFlags) 0;
  }

  public override int GetResultCell() => this.targetCell;
}
