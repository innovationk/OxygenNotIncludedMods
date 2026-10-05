// Decompiled with JetBrains decompiler
// Type: SafeCellQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SafeCellQuery : PathFinderQuery
{
  private MinionBrain brain;
  private int targetCell;
  private int targetCost;
  public SafeCellQuery.SafeFlags targetCellFlags;
  private bool avoid_light;
  private SafeCellQuery.SafeFlags ignoredFlags;

  public SafeCellQuery Reset(
    MinionBrain brain,
    bool avoid_light,
    SafeCellQuery.SafeFlags ignoredFlags = (SafeCellQuery.SafeFlags) 0)
  {
    this.brain = brain;
    this.targetCell = PathFinder.InvalidCell;
    this.targetCost = int.MaxValue;
    this.targetCellFlags = (SafeCellQuery.SafeFlags) 0;
    this.avoid_light = avoid_light;
    this.ignoredFlags = ignoredFlags;
    return this;
  }

  public static SafeCellQuery.SafeFlags GetFlags(
    int cell,
    MinionBrain brain,
    bool avoid_light = false,
    SafeCellQuery.SafeFlags ignoredFlags = (SafeCellQuery.SafeFlags) 0)
  {
    int index = Grid.CellAbove(cell);
    if (!Grid.IsValidCell(index) || (Grid.Solid[cell] ? 1 : (Grid.Solid[index] ? 1 : 0)) != 0 || (Grid.IsTileUnderConstruction[cell] ? 1 : (Grid.IsTileUnderConstruction[index] ? 1 : 0)) != 0)
      return (SafeCellQuery.SafeFlags) 0;
    bool flag1 = brain.IsCellClear(cell);
    int num1 = (ignoredFlags & SafeCellQuery.SafeFlags.IsNotLiquid) != (SafeCellQuery.SafeFlags) 0 ? 1 : (!Grid.Element[cell].IsLiquid ? 1 : 0);
    bool flag2 = (ignoredFlags & SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace) != (SafeCellQuery.SafeFlags) 0 || !Grid.Element[index].IsLiquid;
    bool flag3 = (ignoredFlags & SafeCellQuery.SafeFlags.CorrectTemperature) != (SafeCellQuery.SafeFlags) 0 || (double) Grid.Temperature[cell] > 285.14999389648438 && (double) Grid.Temperature[cell] < 303.14999389648438;
    bool flag4 = (ignoredFlags & SafeCellQuery.SafeFlags.IsNotRadiated) != (SafeCellQuery.SafeFlags) 0 || (double) Grid.Radiation[cell] < 250.0;
    bool flag5 = (ignoredFlags & SafeCellQuery.SafeFlags.IsBreathable) != (SafeCellQuery.SafeFlags) 0 || (Object) brain.OxygenBreather == (Object) null || GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(cell, Grid.DefaultOffset, brain.OxygenBreather).IsBreathable;
    int num2 = brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) ? 0 : (!brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole) ? 1 : 0);
    bool flag6 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Tube);
    bool flag7 = !avoid_light || SleepChore.IsDarkAtCell(cell);
    if (cell == Grid.PosToCell((KMonoBehaviour) brain))
      flag5 = (ignoredFlags & SafeCellQuery.SafeFlags.IsBreathable) != (SafeCellQuery.SafeFlags) 0 || (Object) brain.OxygenBreather == (Object) null || brain.OxygenBreather.HasOxygen;
    SafeCellQuery.SafeFlags flags = (SafeCellQuery.SafeFlags) 0;
    if (flag1)
      flags |= SafeCellQuery.SafeFlags.IsClear;
    if (flag3)
      flags |= SafeCellQuery.SafeFlags.CorrectTemperature;
    if (flag4)
      flags |= SafeCellQuery.SafeFlags.IsNotRadiated;
    if (flag5)
      flags |= SafeCellQuery.SafeFlags.IsBreathable;
    if (num2 != 0)
      flags |= SafeCellQuery.SafeFlags.IsNotLadder;
    if (flag6)
      flags |= SafeCellQuery.SafeFlags.IsNotTube;
    if (num1 != 0)
      flags |= SafeCellQuery.SafeFlags.IsNotLiquid;
    if (flag2)
      flags |= SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace;
    if (flag7)
      flags |= SafeCellQuery.SafeFlags.IsLightOk;
    return flags;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain, this.avoid_light, this.ignoredFlags);
    if (((flags > this.targetCellFlags ? 1 : 0) | (flags != this.targetCellFlags ? (false ? 1 : 0) : (cost < this.targetCost ? 1 : 0))) != 0)
    {
      this.targetCellFlags = flags;
      this.targetCost = cost;
      this.targetCell = cell;
    }
    return (SafeCellQuery.SafeFlags.AllSafeFlags & ~(flags | this.ignoredFlags)) == (SafeCellQuery.SafeFlags) 0;
  }

  public override int GetResultCell() => this.targetCell;

  public enum SafeFlags
  {
    IsClear = 1,
    IsLightOk = 2,
    IsNotLadder = 4,
    IsNotTube = 8,
    CorrectTemperature = 16, // 0x00000010
    IsNotRadiated = 32, // 0x00000020
    IsBreathable = 64, // 0x00000040
    IsNotLiquidOnMyFace = 128, // 0x00000080
    IsNotLiquid = 256, // 0x00000100
    AllSafeFlags = 511, // 0x000001FF
    IsNotSwimming = 512, // 0x00000200
  }
}
