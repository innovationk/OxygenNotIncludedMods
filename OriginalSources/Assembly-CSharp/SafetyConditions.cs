// Decompiled with JetBrains decompiler
// Type: SafetyConditions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SafetyConditions
{
  public SafetyChecker.Condition IsNotLiquid;
  public SafetyChecker.Condition IsNotCoveredInLiquid;
  public SafetyChecker.Condition IsNotLadder;
  public SafetyChecker.Condition IsCorrectTemperature;
  public SafetyChecker.Condition IsWarming;
  public SafetyChecker.Condition IsCooling;
  public SafetyChecker.Condition HasSomeOxygen;
  public SafetyChecker.Condition HasSomeOxygenAround;
  public SafetyChecker.Condition IsClear;
  public SafetyChecker.Condition IsNotFoundation;
  public SafetyChecker.Condition IsNotDoor;
  public SafetyChecker.Condition IsNotLedge;
  public SafetyChecker.Condition IsNearby;
  public SafetyChecker WarmUpChecker;
  public SafetyChecker CoolDownChecker;
  public SafetyChecker RecoverBreathChecker;
  public SafetyChecker AbsorbCellCellChecker;
  public SafetyChecker VomitCellChecker;
  public SafetyChecker SafeCellChecker;
  public SafetyChecker IdleCellChecker;

  public SafetyConditions()
  {
    int num1 = 1;
    int num2;
    this.IsNearby = new SafetyChecker.Condition(nameof (IsNearby), num2 = num1 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) => cost > 5));
    int num3;
    this.IsNotLedge = new SafetyChecker.Condition(nameof (IsNotLedge), num3 = num2 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) =>
    {
      int i1 = Grid.CellBelow(Grid.CellLeft(cell));
      if (Grid.Solid[i1])
        return false;
      int i2 = Grid.CellBelow(Grid.CellRight(cell));
      return Grid.Solid[i2];
    }));
    int num4;
    this.IsNotLiquid = new SafetyChecker.Condition(nameof (IsNotLiquid), num4 = num3 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) => !Grid.Element[cell].IsLiquid));
    int num5;
    this.IsNotCoveredInLiquid = new SafetyChecker.Condition(nameof (IsNotCoveredInLiquid), num5 = num4 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) =>
    {
      int cell1 = Grid.CellAbove(cell);
      if (!Grid.IsValidCell(cell1))
        return false;
      return !Grid.Element[cell].IsLiquid || !Grid.Element[cell1].IsLiquid;
    }));
    int num6;
    this.IsNotLadder = new SafetyChecker.Condition(nameof (IsNotLadder), num6 = num5 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) => !context.navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !context.navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole)));
    int num7;
    this.IsNotDoor = new SafetyChecker.Condition(nameof (IsNotDoor), num7 = num6 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) =>
    {
      int num8 = Grid.CellAbove(cell);
      return !Grid.HasDoor[cell] && Grid.IsValidCell(num8) && !Grid.HasDoor[num8];
    }));
    int num9;
    this.IsCorrectTemperature = new SafetyChecker.Condition(nameof (IsCorrectTemperature), num9 = num7 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) => (double) Grid.Temperature[cell] > 285.14999389648438 && (double) Grid.Temperature[cell] < 303.14999389648438));
    int num10;
    this.IsWarming = new SafetyChecker.Condition(nameof (IsWarming), num10 = num9 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) => WarmthProvider.IsWarmCell(cell)));
    int num11;
    this.IsCooling = new SafetyChecker.Condition(nameof (IsCooling), num11 = num10 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) => false));
    int num12;
    this.HasSomeOxygen = new SafetyChecker.Condition(nameof (HasSomeOxygen), num12 = num11 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) =>
    {
      if ((Object) context.oxygenBreather == (Object) null)
        return true;
      return GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(cell, new CellOffset[2]
      {
        CellOffset.none,
        CellOffset.up
      }, context.oxygenBreather).IsBreathable;
    }));
    int num13;
    this.HasSomeOxygenAround = new SafetyChecker.Condition(nameof (HasSomeOxygenAround), num13 = num12 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) => (Object) context.oxygenBreather == (Object) null || GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(cell, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, context.oxygenBreather).IsBreathable));
    int num14;
    this.IsClear = new SafetyChecker.Condition(nameof (IsClear), num14 = num13 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) => context.minionBrain.IsCellClear(cell)));
    this.WarmUpChecker = new SafetyChecker(new List<SafetyChecker.Condition>()
    {
      this.IsWarming
    }.ToArray());
    this.CoolDownChecker = new SafetyChecker(new List<SafetyChecker.Condition>()
    {
      this.IsCooling
    }.ToArray());
    this.AbsorbCellCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>()
    {
      this.IsNotCoveredInLiquid,
      this.IsNotDoor,
      this.HasSomeOxygenAround
    }.ToArray());
    List<SafetyChecker.Condition> collection1 = new List<SafetyChecker.Condition>();
    collection1.Add(this.HasSomeOxygen);
    collection1.Add(this.IsNotDoor);
    this.RecoverBreathChecker = new SafetyChecker(collection1.ToArray());
    List<SafetyChecker.Condition> collection2 = new List<SafetyChecker.Condition>((IEnumerable<SafetyChecker.Condition>) collection1);
    collection2.Add(this.IsNotLiquid);
    collection2.Add(this.IsCorrectTemperature);
    this.SafeCellChecker = new SafetyChecker(collection2.ToArray());
    this.IdleCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>((IEnumerable<SafetyChecker.Condition>) collection2)
    {
      this.IsClear,
      this.IsNotLadder
    }.ToArray());
    this.VomitCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>()
    {
      this.IsNotLiquid,
      this.IsNotLedge,
      this.IsNearby
    }.ToArray());
  }
}
