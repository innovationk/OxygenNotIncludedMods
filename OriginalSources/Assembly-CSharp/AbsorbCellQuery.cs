// Decompiled with JetBrains decompiler
// Type: AbsorbCellQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class AbsorbCellQuery : PathFinderQuery
{
  private MinionBrain brain;
  private float scaldingTreshold = -1f;
  private int targetCell;
  private int targetCost;
  private float targetOxygenScore;
  private bool criticalMode;
  private float bionicOxygenRemaining;
  private float breathPercentage;
  private float targetBreathableMassAvailable;
  public AbsorbCellQuery.AbsorbOxygenSafeCellFlags targetCellSafetyFlags;
  public float targetCellBreathabilityScore;
  private int allowCellEvenIfReserved = -1;
  private SafetyChecker checker;
  private SafetyChecker.Context context;
  private bool isRecoveringFromSuffocation;

  public AbsorbCellQuery() => this.checker = Game.Instance.safetyConditions.AbsorbCellCellChecker;

  public AbsorbCellQuery Reset(
    MinionBrain brain,
    bool criticalMode,
    float currentOxygenTankMass,
    float breathPercentage,
    int allowCellEvenIfReserved,
    bool isRecoveringFromSuffocation)
  {
    this.brain = brain;
    this.targetCell = PathFinder.InvalidCell;
    this.targetCost = int.MaxValue;
    this.targetOxygenScore = float.MinValue;
    this.targetCellSafetyFlags = (AbsorbCellQuery.AbsorbOxygenSafeCellFlags) 0;
    this.targetBreathableMassAvailable = 0.0f;
    this.criticalMode = criticalMode;
    this.bionicOxygenRemaining = currentOxygenTankMass;
    this.breathPercentage = breathPercentage;
    this.allowCellEvenIfReserved = allowCellEvenIfReserved;
    this.context = new SafetyChecker.Context((KMonoBehaviour) brain);
    ScaldingMonitor.Instance smi = (Object) brain == (Object) null ? (ScaldingMonitor.Instance) null : brain.GetSMI<ScaldingMonitor.Instance>();
    this.scaldingTreshold = smi == null ? -1f : smi.GetScaldingThreshold();
    this.isRecoveringFromSuffocation = isRecoveringFromSuffocation;
    return this;
  }

  public static AbsorbCellQuery.AbsorbOxygenSafeCellFlags GetAbsorbOxygenFlags(
    int cell,
    MinionBrain brain,
    float scaldingTreshold,
    out float totalBreathableMassAroundCell,
    out float breathableCellRatioInSample,
    int allowCellEvenIfReserved)
  {
    totalBreathableMassAroundCell = 0.0f;
    breathableCellRatioInSample = 0.0f;
    int index1 = Grid.CellAbove(cell);
    if (!Grid.IsValidCell(index1) || (Grid.Solid[cell] ? 1 : (Grid.Solid[index1] ? 1 : 0)) != 0 || (Grid.IsTileUnderConstruction[cell] ? 1 : (Grid.IsTileUnderConstruction[index1] ? 1 : 0)) != 0)
      return (AbsorbCellQuery.AbsorbOxygenSafeCellFlags) 0;
    bool flag1 = cell == allowCellEvenIfReserved || brain.IsCellClear(cell);
    bool flag2 = !Grid.Element[cell].IsLiquid;
    bool flag3 = !Grid.Element[index1].IsLiquid;
    bool flag4 = (double) scaldingTreshold < 0.0 || (double) Grid.Temperature[cell] < (double) scaldingTreshold;
    bool flag5 = (double) Grid.Radiation[cell] < 250.0;
    bool flag6 = false;
    if ((Object) brain.OxygenBreather != (Object) null)
    {
      for (int index2 = 0; index2 < GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length; ++index2)
      {
        int index3 = Grid.OffsetCell(cell, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS[index2]);
        if (Grid.IsValidCell(index3) && Grid.AreCellsInSameWorld(cell, index3) && Grid.Element[index3].HasTag(GameTags.Breathable))
          breathableCellRatioInSample += 1f / (float) GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length;
      }
      flag6 = GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(cell, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, brain.OxygenBreather, out totalBreathableMassAroundCell).IsBreathable;
    }
    int num = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Tube) ? 1 : 0;
    AbsorbCellQuery.AbsorbOxygenSafeCellFlags absorbOxygenFlags = (AbsorbCellQuery.AbsorbOxygenSafeCellFlags) 0;
    if (flag4)
      absorbOxygenFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotScaldingTemperatures;
    if (flag5)
      absorbOxygenFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotRadiated;
    if (flag6)
      absorbOxygenFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsBreathable;
    if (flag1)
      absorbOxygenFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsClear;
    if (num != 0)
      absorbOxygenFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotTube;
    if (flag2)
      absorbOxygenFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotLiquid;
    if (flag3)
      absorbOxygenFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotLiquidOnMyFace;
    return absorbOxygenFlags;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    float num1 = 0.1f * (float) GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length;
    float a = 2.5f * (float) GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length;
    float max = (float) (54 / GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length);
    float num2 = max / 2.8f;
    float num3 = (float) cost;
    bool all_conditions_met;
    this.checker.GetSafetyConditions(cell, cost, this.context, out all_conditions_met);
    if (all_conditions_met)
    {
      float num4 = 0.03f * (num3 / 10f);
      float totalBreathableMassAroundCell = 0.0f;
      float breathableCellRatioInSample = 0.0f;
      AbsorbCellQuery.AbsorbOxygenSafeCellFlags absorbOxygenFlags = AbsorbCellQuery.GetAbsorbOxygenFlags(cell, this.brain, this.scaldingTreshold, out totalBreathableMassAroundCell, out breathableCellRatioInSample, this.allowCellEvenIfReserved);
      float num5 = Mathf.Clamp(totalBreathableMassAroundCell, 0.0f, max);
      if (this.criticalMode)
      {
        if (!this.isRecoveringFromSuffocation && (double) this.breathPercentage > (double) DUPLICANTSTATS.BIONICS.Breath.SUFFOCATE_AMOUNT && (double) num5 < (double) num1)
          num5 = 0.0f;
      }
      else if ((double) num5 < (double) num2)
        num5 = 0.0f;
      double num6 = (double) absorbOxygenFlags;
      float num7 = 10f * breathableCellRatioInSample;
      float num8 = num5 * num7 - num4;
      bool flag1 = false;
      if (this.targetCell == Grid.InvalidCell)
        flag1 = true;
      bool flag2 = (double) this.targetBreathableMassAvailable > 0.0;
      bool flag3 = (double) num3 < (double) this.targetCost;
      bool flag4 = (double) this.targetOxygenScore >= (double) a;
      double targetCellSafetyFlags = (double) this.targetCellSafetyFlags;
      bool flag5 = num6 >= targetCellSafetyFlags || !flag2;
      float b = this.targetOxygenScore;
      if (this.criticalMode)
        b = Mathf.Min(a, b);
      if ((double) num8 >= (double) b & flag5)
      {
        if (this.criticalMode)
        {
          if (flag3 || !flag4)
            flag1 = true;
        }
        else
          flag1 = true;
      }
      if (flag1 && (double) num5 > (double) DUPLICANTSTATS.BIONICS.BaseStats.NO_OXYGEN_THRESHOLD)
      {
        this.targetBreathableMassAvailable = num5;
        this.targetCellSafetyFlags = absorbOxygenFlags;
        this.targetCost = cost;
        this.targetCell = cell;
        this.targetOxygenScore = num8;
      }
    }
    return false;
  }

  public override int GetResultCell() => this.targetCell;

  public enum AbsorbOxygenSafeCellFlags
  {
    IsNotTube = 1,
    IsNotRadiated = 2,
    IsBreathable = 4,
    IsNotScaldingTemperatures = 8,
    IsClear = 16, // 0x00000010
    IsNotLiquidOnMyFace = 32, // 0x00000020
    IsNotLiquid = 64, // 0x00000040
  }
}
