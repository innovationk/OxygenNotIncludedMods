// Decompiled with JetBrains decompiler
// Type: BionicOxygenTankMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class BionicOxygenTankMonitor : 
  GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>
{
  public const SimHashes INITIAL_TANK_ELEMENT = SimHashes.Oxygen;
  public static readonly Tag INITIAL_TANK_ELEMENT_TAG = SimHashes.Oxygen.CreateTag();
  public const float SAFE_TRESHOLD = 0.85f;
  public const float CRITICAL_TRESHOLD = 0.0f;
  public const float OXYGEN_TANK_CAPACITY_IN_SECONDS = 2400f;
  public static readonly float OXYGEN_TANK_CAPACITY_KG = 2400f * DUPLICANTSTATS.BIONICS.BaseStats.OXYGEN_USED_PER_SECOND;
  public static float INITIAL_OXYGEN_TEMP = DUPLICANTSTATS.BIONICS.Temperature.Internal.IDEAL;
  public static float SECONDS_PER_PATH_COST_UNIT = 0.3f;
  public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State fistSpawn;
  public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State safe;
  public BionicOxygenTankMonitor.LowOxygenStates low;
  public BionicOxygenTankMonitor.SeekOxygenStates critical;
  private StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.BoolParameter HasSpawnedBefore;
  public StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Signal AbsorbCellChangedSignal;
  public StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Signal OxygenSourceItemLostSignal;
  public StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Signal ClosestOxygenSourceChanged;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.fistSpawn;
    this.fistSpawn.ParamTransition<bool>((StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Parameter<bool>) this.HasSpawnedBefore, this.safe, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.IsTrue).Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.StartWithFullTank));
    this.safe.Transition((GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.low, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe)));
    this.low.DefaultState(this.low.idle);
    this.low.idle.Transition((GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.critical, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical)).Transition(this.safe, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe)).ScheduleChange((GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.low.schedule, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsAllowedToSeekOxygenBySchedule));
    this.low.schedule.ToggleUrge(Db.Get().Urges.FindOxygenRefill).DefaultState(this.low.schedule.enableSensors).Transition((GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.critical, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCriticalAndNotConsumingOxygen)).Exit(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.DisableOxygenSourceSensors));
    this.low.schedule.enableSensors.Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.EnableOxygenSourceSensors)).GoTo((GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.low.schedule.oxygenCanisterMode);
    this.low.schedule.oxygenCanisterMode.DefaultState(this.low.schedule.oxygenCanisterMode.running);
    this.low.schedule.oxygenCanisterMode.running.ScheduleChange(this.low.idle, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsNotAllowedToSeekOxygenSourceItemsByScheduleAndSeekChoreHasNotBegun)).OnSignal(this.OxygenSourceItemLostSignal, (GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.low.schedule.environmentAbsorbMode, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Parameter<StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter>.Callback(BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable)).OnSignal(this.AbsorbCellChangedSignal, (GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.low.schedule.environmentAbsorbMode, (StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Parameter<StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter>.Callback) ((smi, param) => !BionicOxygenTankMonitor.FindOxygenSourceChoreIsRunning(smi) && BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable(smi))).Update(new System.Action<BionicOxygenTankMonitor.Instance, float>(BionicOxygenTankMonitor.UpdateAbsorbCellIfNoOxygenSourceAvailable)).ToggleChore((Func<BionicOxygenTankMonitor.Instance, Chore>) (smi => (Chore) new FindAndConsumeOxygenSourceChore(smi.master, false)), this.low.schedule.oxygenCanisterMode.ends, this.low.schedule.oxygenCanisterMode.ends);
    this.low.schedule.oxygenCanisterMode.ends.EnterTransition(this.safe, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe)).GoTo(this.low.idle);
    this.low.schedule.environmentAbsorbMode.DefaultState(this.low.schedule.environmentAbsorbMode.running);
    this.low.schedule.environmentAbsorbMode.running.ScheduleChange(this.low.idle, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsNotAllowedToSeekOxygenSourceItemsByScheduleAndAbsorbChoreHasNotBegun)).OnSignal(this.ClosestOxygenSourceChanged, (GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.low.schedule.oxygenCanisterMode, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Parameter<StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter>.Callback(BionicOxygenTankMonitor.OxygenSourceItemAvailableAndAbsorbChoreNotStarted)).ToggleChore((Func<BionicOxygenTankMonitor.Instance, Chore>) (smi => (Chore) new BionicMassOxygenAbsorbChore(smi.master, false)), this.low.schedule.environmentAbsorbMode.ends, this.low.schedule.environmentAbsorbMode.ends);
    this.low.schedule.environmentAbsorbMode.ends.EnterTransition(this.safe, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe)).GoTo(this.low.idle);
    this.critical.ToggleUrge(Db.Get().Urges.FindOxygenRefill).Exit(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.DisableOxygenSourceSensors)).DefaultState(this.critical.enableSensors).ToggleExpression(Db.Get().Expressions.RecoverBreath).Update((System.Action<BionicOxygenTankMonitor.Instance, float>) ((smi, dt) =>
    {
      if ((double) smi.master.gameObject.GetAmounts().Get("Breath").value > (double) DUPLICANTSTATS.BIONICS.Breath.SUFFOCATE_AMOUNT)
        return;
      smi.isRecoveringFromSuffocation = true;
    })).Exit((StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback) (smi => smi.isRecoveringFromSuffocation = false));
    this.critical.enableSensors.Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.EnableOxygenSourceSensors)).GoTo((GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.critical.oxygenCanisterMode);
    this.critical.oxygenCanisterMode.DefaultState(this.critical.oxygenCanisterMode.running);
    this.critical.oxygenCanisterMode.running.OnSignal(this.ClosestOxygenSourceChanged, (GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.critical.environmentAbsorbMode, (StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Parameter<StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter>.Callback) ((smi, param) => !BionicOxygenTankMonitor.FindOxygenSourceChoreIsRunning(smi) && BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable(smi))).OnSignal(this.OxygenSourceItemLostSignal, (GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.critical.environmentAbsorbMode, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Parameter<StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter>.Callback(BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable)).OnSignal(this.AbsorbCellChangedSignal, (GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.critical.environmentAbsorbMode, (StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Parameter<StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter>.Callback) ((smi, param) => !BionicOxygenTankMonitor.FindOxygenSourceChoreIsRunning(smi) && BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable(smi))).Update(new System.Action<BionicOxygenTankMonitor.Instance, float>(BionicOxygenTankMonitor.UpdateAbsorbCellIfNoOxygenSourceAvailable)).ToggleChore((Func<BionicOxygenTankMonitor.Instance, Chore>) (smi => (Chore) new FindAndConsumeOxygenSourceChore(smi.master, true)), this.critical.oxygenCanisterMode.ends, this.critical.oxygenCanisterMode.ends);
    this.critical.oxygenCanisterMode.ends.EnterTransition((GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.low, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical))).GoTo(this.critical.oxygenCanisterMode.running);
    this.critical.environmentAbsorbMode.DefaultState(this.critical.environmentAbsorbMode.running);
    this.critical.environmentAbsorbMode.running.OnSignal(this.ClosestOxygenSourceChanged, (GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.critical.oxygenCanisterMode, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Parameter<StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter>.Callback(BionicOxygenTankMonitor.OxygenSourceItemAvailableAndAbsorbChoreNotStarted)).ToggleChore((Func<BionicOxygenTankMonitor.Instance, Chore>) (smi => (Chore) new BionicMassOxygenAbsorbChore(smi.master, true)), this.critical.environmentAbsorbMode.ends, this.critical.environmentAbsorbMode.ends);
    this.critical.environmentAbsorbMode.ends.EnterTransition((GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.low, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical))).GoTo((GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State) this.critical.oxygenCanisterMode);
  }

  public static bool IsAllowedToSeekOxygenBySchedule(BionicOxygenTankMonitor.Instance smi)
  {
    return smi.IsAllowedToSeekOxygenBySchedule;
  }

  public static bool IsNotAllowedToSeekOxygenSourceItemsByScheduleAndSeekChoreHasNotBegun(
    BionicOxygenTankMonitor.Instance smi)
  {
    return !BionicOxygenTankMonitor.IsAllowedToSeekOxygenBySchedule(smi) && !BionicOxygenTankMonitor.FindOxygenSourceChoreIsRunning(smi);
  }

  public static bool IsNotAllowedToSeekOxygenSourceItemsByScheduleAndAbsorbChoreHasNotBegun(
    BionicOxygenTankMonitor.Instance smi)
  {
    return !BionicOxygenTankMonitor.IsAllowedToSeekOxygenBySchedule(smi) && !BionicOxygenTankMonitor.AbsorbChoreIsRunning(smi);
  }

  public static bool AreOxygenLevelsSafe(BionicOxygenTankMonitor.Instance smi)
  {
    return (double) smi.OxygenPercentage >= 0.85000002384185791;
  }

  public static bool AreOxygenLevelsCritical(BionicOxygenTankMonitor.Instance smi)
  {
    return (double) smi.OxygenPercentage <= 0.0;
  }

  public static bool AreOxygenLevelsCriticalAndNotConsumingOxygen(
    BionicOxygenTankMonitor.Instance smi)
  {
    return BionicOxygenTankMonitor.AreOxygenLevelsCritical(smi) && !BionicOxygenTankMonitor.IsConsumingOxygen(smi);
  }

  public static bool IsThereAnOxygenSourceItemAvailable(BionicOxygenTankMonitor.Instance smi)
  {
    return (UnityEngine.Object) smi.GetClosestOxygenSource() != (UnityEngine.Object) null;
  }

  public static bool AbsorbCellUnavailable(BionicOxygenTankMonitor.Instance smi)
  {
    return smi.AbsorbOxygenCell == Grid.InvalidCell;
  }

  public static bool AbsorbCellAvailable(BionicOxygenTankMonitor.Instance smi)
  {
    return smi.AbsorbOxygenCell != Grid.InvalidCell;
  }

  public static bool NoOxygenSourceAvailable(BionicOxygenTankMonitor.Instance smi)
  {
    return (UnityEngine.Object) smi.GetClosestOxygenSource() == (UnityEngine.Object) null;
  }

  public static bool NoOxygenSourceAvailableButAbsorbCellAvailable(
    BionicOxygenTankMonitor.Instance smi)
  {
    return BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable(smi, (StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter) null);
  }

  public static bool NoOxygenSourceAvailableButAbsorbCellAvailable(
    BionicOxygenTankMonitor.Instance smi,
    StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter param)
  {
    return BionicOxygenTankMonitor.NoOxygenSourceAvailable(smi) && BionicOxygenTankMonitor.AbsorbCellAvailable(smi);
  }

  public static bool OxygenSourceItemAvailableAndAbsorbChoreNotStarted(
    BionicOxygenTankMonitor.Instance smi,
    StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.SignalParameter param)
  {
    return BionicOxygenTankMonitor.IsThereAnOxygenSourceItemAvailable(smi) && !BionicOxygenTankMonitor.AbsorbChoreIsRunning(smi);
  }

  public static bool AbsorbChoreIsRunning(BionicOxygenTankMonitor.Instance smi)
  {
    return BionicOxygenTankMonitor.ChoreIsRunning(smi, Db.Get().ChoreTypes.BionicAbsorbOxygen) || BionicOxygenTankMonitor.ChoreIsRunning(smi, Db.Get().ChoreTypes.BionicAbsorbOxygen_Critical);
  }

  public static bool FindOxygenSourceChoreIsRunning(BionicOxygenTankMonitor.Instance smi)
  {
    return BionicOxygenTankMonitor.ChoreIsRunning(smi, Db.Get().ChoreTypes.FindOxygenSourceItem) || BionicOxygenTankMonitor.ChoreIsRunning(smi, Db.Get().ChoreTypes.FindOxygenSourceItem_Critical);
  }

  public static bool ChoreIsRunning(BionicOxygenTankMonitor.Instance smi, ChoreType type)
  {
    return smi.ChoreIsRunning(type);
  }

  public static bool IsConsumingOxygen(BionicOxygenTankMonitor.Instance smi)
  {
    return smi.IsConsumingOxygen();
  }

  public static void StartWithFullTank(BionicOxygenTankMonitor.Instance smi)
  {
    smi.AddFirstTimeSpawnedOxygen();
  }

  public static void EnableOxygenSourceSensors(BionicOxygenTankMonitor.Instance smi)
  {
    smi.SetOxygenSourceSensorsActiveState(true);
  }

  public static void DisableOxygenSourceSensors(BionicOxygenTankMonitor.Instance smi)
  {
    smi.SetOxygenSourceSensorsActiveState(false);
  }

  public static void UpdateAbsorbCellIfNoOxygenSourceAvailable(
    BionicOxygenTankMonitor.Instance smi,
    float dt)
  {
    if (!BionicOxygenTankMonitor.NoOxygenSourceAvailable(smi))
      return;
    smi.UpdatePotentialCellToAbsorbOxygen(Grid.InvalidCell);
  }

  public interface IChore
  {
    bool IsConsumingOxygen();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class ChoreState : 
    GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
  {
    public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State running;
    public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State ends;
  }

  public class SeekOxygenStates : 
    GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
  {
    public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State enableSensors;
    public BionicOxygenTankMonitor.ChoreState oxygenCanisterMode;
    public BionicOxygenTankMonitor.ChoreState environmentAbsorbMode;
  }

  public class LowOxygenStates : 
    GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
  {
    public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State idle;
    public BionicOxygenTankMonitor.SeekOxygenStates schedule;
  }

  public new class Instance : 
    GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.GameInstance,
    OxygenBreather.IGasProvider
  {
    public AttributeInstance airConsumptionRate;
    private Schedulable schedulable;
    private AmountInstance oxygenTankAmountInstance;
    private ClosestPickupableSensor<Pickupable>[] oxygenSourceSensors;
    private Pickupable closestOxygenSource;
    private Navigator navigator;
    private float movementRate;
    private AbsorbCellQuery query;
    private OxygenBreather oxygenBreather;
    private MinionBrain brain;
    private MinionStorageDataHolder dataHolder;
    private ChoreDriver choreDriver;
    public bool isRecoveringFromSuffocation;

    public bool IsAllowedToSeekOxygenBySchedule
    {
      get => ScheduleManager.Instance.IsAllowed(this.schedulable, Db.Get().ScheduleBlockTypes.Eat);
    }

    public bool IsEmpty => (double) this.AvailableOxygen == 0.0;

    public float OxygenPercentage => this.AvailableOxygen / this.storage.capacityKg;

    public float AvailableOxygen => this.storage.GetMassAvailable(GameTags.Breathable);

    public float SpaceAvailableInTank => this.storage.capacityKg - this.AvailableOxygen;

    public int AbsorbOxygenCell { private set; get; } = Grid.InvalidCell;

    public Storage storage { private set; get; }

    public Instance(IStateMachineTarget master, BionicOxygenTankMonitor.Def def)
      : base(master, def)
    {
      this.query = new AbsorbCellQuery();
      NameDisplayScreen.Instance.RegisterComponent(this.gameObject, (object) this);
      Sensors component = this.GetComponent<Sensors>();
      this.schedulable = this.GetComponent<Schedulable>();
      this.navigator = this.GetComponent<Navigator>();
      this.movementRate = BipedTransitionLayer.GetMovementSpeedMultiplier(Db.Get().AttributeConverters.MovementSpeed.Lookup(this.navigator.gameObject)) / BionicOxygenTankMonitor.SECONDS_PER_PATH_COST_UNIT;
      this.oxygenBreather = this.GetComponent<OxygenBreather>();
      this.brain = this.GetComponent<MinionBrain>();
      this.dataHolder = this.GetComponent<MinionStorageDataHolder>();
      this.dataHolder.OnCopyBegins += new System.Action<StoredMinionIdentity>(this.OnCopyMinionBegins);
      this.oxygenSourceSensors = new ClosestPickupableSensor<Pickupable>[1]
      {
        (ClosestPickupableSensor<Pickupable>) component.GetSensor<ClosestOxygenCanisterSensor>()
      };
      for (int index = 0; index < this.oxygenSourceSensors.Length; ++index)
        this.oxygenSourceSensors[index].OnItemChanged += new System.Action<Pickupable>(this.OnOxygenSourceSensorItemChanged);
      this.storage = this.gameObject.GetComponents<Storage>().FindFirst<Storage>((Func<Storage, bool>) (s => s.storageID == GameTags.StoragesIds.BionicOxygenTankStorage));
      this.oxygenTankAmountInstance = Db.Get().Amounts.BionicOxygenTank.Lookup(this.gameObject);
      this.airConsumptionRate = Db.Get().Attributes.AirConsumptionRate.Lookup(this.gameObject);
      this.storage.OnStorageChange += new System.Action<GameObject>(this.OnOxygenTankStorageChanged);
      this.choreDriver = this.gameObject.GetComponent<ChoreDriver>();
    }

    public bool ChoreIsRunning(ChoreType type)
    {
      if ((UnityEngine.Object) this.choreDriver == (UnityEngine.Object) null)
        return false;
      Chore currentChore = this.choreDriver.GetCurrentChore();
      return currentChore != null && currentChore.choreType == type;
    }

    public bool IsConsumingOxygen()
    {
      this.choreDriver = this.smi.GetComponent<ChoreDriver>();
      return !((UnityEngine.Object) this.choreDriver == (UnityEngine.Object) null) && this.choreDriver.GetCurrentChore() is BionicOxygenTankMonitor.IChore currentChore && currentChore.IsConsumingOxygen();
    }

    public Pickupable GetClosestOxygenSource() => this.closestOxygenSource;

    private void OnOxygenSourceSensorItemChanged(object o) => this.CompareOxygenSources();

    private void OnOxygenTankStorageChanged(object o) => this.RefreshAmountInstance();

    public void RefreshAmountInstance()
    {
      double num = (double) this.oxygenTankAmountInstance.SetValue(this.AvailableOxygen);
    }

    public void AddFirstTimeSpawnedOxygen()
    {
      this.storage.AddElement(SimHashes.Oxygen, this.storage.capacityKg - this.AvailableOxygen, BionicOxygenTankMonitor.INITIAL_OXYGEN_TEMP, byte.MaxValue, 0);
      this.sm.HasSpawnedBefore.Set(true, this);
    }

    private void OnCopyMinionBegins(StoredMinionIdentity destination)
    {
      this.dataHolder.UpdateData<BionicOxygenTankMonitor.Instance>(new MinionStorageDataHolder.DataPackData()
      {
        Bools = new bool[1]
        {
          this.sm.HasSpawnedBefore.Get(this)
        }
      });
    }

    public override void StartSM()
    {
      base.StartSM();
      this.RefreshAmountInstance();
    }

    public override void PostParamsInitialized()
    {
      MinionStorageDataHolder.DataPack dataPack = this.dataHolder.GetDataPack<BionicOxygenTankMonitor.Instance>();
      if (dataPack != null && dataPack.IsStoringNewData)
      {
        MinionStorageDataHolder.DataPackData dataPackData = dataPack.ReadData();
        if (dataPackData != null)
          this.sm.HasSpawnedBefore.Set(dataPackData.Bools[0], this);
      }
      base.PostParamsInitialized();
    }

    private void CompareOxygenSources()
    {
      Pickupable pickupable = (Pickupable) null;
      float num1 = (float) int.MaxValue;
      for (int index = 0; index < this.oxygenSourceSensors.Length; ++index)
      {
        ClosestPickupableSensor<Pickupable> oxygenSourceSensor = this.oxygenSourceSensors[index];
        int itemNavCost = oxygenSourceSensor.GetItemNavCost();
        if ((double) itemNavCost < (double) num1)
        {
          num1 = (float) itemNavCost;
          pickupable = oxygenSourceSensor.GetItem();
        }
      }
      if ((UnityEngine.Object) pickupable != (UnityEngine.Object) null && this.IsInsideState((StateMachine.BaseState) this.sm.critical))
      {
        float num2 = num1 / this.movementRate * this.oxygenBreather.ConsumptionRate;
        if ((double) this.oxygenBreather.GetAmounts().Get(Db.Get().Amounts.Breath).value < (double) num2)
          pickupable = (Pickupable) null;
      }
      if (!((UnityEngine.Object) this.closestOxygenSource != (UnityEngine.Object) pickupable))
        return;
      this.closestOxygenSource = pickupable;
      this.sm.ClosestOxygenSourceChanged.Trigger(this);
    }

    public void UpdatePotentialCellToAbsorbOxygen(int previouslyReservedCell)
    {
      float breathPercentage = this.brain.GetAmounts().Get(Db.Get().Amounts.Breath).value / this.brain.GetAmounts().Get(Db.Get().Amounts.Breath).GetMax();
      this.query.Reset(this.brain, BionicOxygenTankMonitor.AreOxygenLevelsCritical(this), this.AvailableOxygen, breathPercentage, previouslyReservedCell, this.isRecoveringFromSuffocation);
      this.navigator.RunQuery((PathFinderQuery) this.smi.query);
      int theSpecificCell = this.smi.query.GetResultCell();
      if (theSpecificCell == Grid.PosToCell(this.gameObject) && !GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(theSpecificCell, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, this.oxygenBreather).IsBreathable)
        theSpecificCell = PathFinder.InvalidCell;
      int num = this.AbsorbOxygenCell != theSpecificCell ? 1 : 0;
      this.AbsorbOxygenCell = theSpecificCell;
      if (num == 0)
        return;
      this.sm.AbsorbCellChangedSignal.Trigger(this);
    }

    public float AddGas(Sim.MassConsumedCallback mass_cb_info)
    {
      return this.AddGas(ElementLoader.elements[(int) mass_cb_info.elemIdx].id, mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
    }

    public float AddGas(
      SimHashes element,
      float mass,
      float temperature,
      byte disseaseIDX = 255 /*0xFF*/,
      int _disseaseCount = 0)
    {
      float mass1 = Mathf.Min(mass, this.SpaceAvailableInTank);
      double num1 = (double) mass - (double) mass1;
      float num2 = mass1 / mass;
      int disease_count = Mathf.CeilToInt((float) _disseaseCount * num2);
      this.storage.AddElement(element, mass1, temperature, disseaseIDX, disease_count);
      return (float) num1;
    }

    public void SetOxygenSourceSensorsActiveState(bool shouldItBeActive)
    {
      for (int index = 0; index < this.oxygenSourceSensors.Length; ++index)
      {
        ClosestPickupableSensor<Pickupable> oxygenSourceSensor = this.oxygenSourceSensors[index];
        oxygenSourceSensor.SetActive(shouldItBeActive);
        if (shouldItBeActive)
          oxygenSourceSensor.Update();
      }
    }

    public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
    {
      if (this.IsEmpty)
        return false;
      SimHashes mostRelevantItemElement = SimHashes.Vacuum;
      float aggregate_temperature = 0.0f;
      SimUtil.DiseaseInfo disease_info;
      this.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out float _, out disease_info, out aggregate_temperature, out mostRelevantItemElement);
      OxygenBreather.BreathableGasConsumed(oxygen_breather, mostRelevantItemElement, amount, aggregate_temperature, disease_info.idx, disease_info.count);
      return true;
    }

    public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
    {
    }

    public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
    {
    }

    public bool IsLowOxygen() => (double) this.OxygenPercentage <= 0.0;

    public bool HasOxygen() => !this.IsEmpty;

    public bool IsBlocked() => false;

    public bool ShouldEmitCO2() => false;

    public bool ShouldStoreCO2() => false;

    protected override void OnCleanUp()
    {
      if ((UnityEngine.Object) this.dataHolder != (UnityEngine.Object) null)
        this.dataHolder.OnCopyBegins -= new System.Action<StoredMinionIdentity>(this.OnCopyMinionBegins);
      if ((UnityEngine.Object) this.storage != (UnityEngine.Object) null)
        this.storage.OnStorageChange -= new System.Action<GameObject>(this.OnOxygenTankStorageChanged);
      base.OnCleanUp();
    }
  }
}
