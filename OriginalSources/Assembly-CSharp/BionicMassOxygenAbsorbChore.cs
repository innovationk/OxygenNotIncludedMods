// Decompiled with JetBrains decompiler
// Type: BionicMassOxygenAbsorbChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class BionicMassOxygenAbsorbChore : Chore<BionicMassOxygenAbsorbChore.Instance>
{
  public static CellOffset[] ABSORB_RANGE = new CellOffset[6]
  {
    new CellOffset(0, 0),
    new CellOffset(0, 1),
    new CellOffset(1, 1),
    new CellOffset(-1, 1),
    new CellOffset(1, 0),
    new CellOffset(-1, 0)
  };
  public const float ABSORB_RATE_IDEAL_CHORE_DURATION = 30f;
  public static readonly float ABSORB_RATE = BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG / 30f;
  public const int HISTORY_ROW_COUNT = 15;
  public const float LOW_OXYGEN_TRESHOLD = 2f;
  public const float GIVE_UP_DURATION_CRTICIAL_MODE = 2f;
  public const float GIVE_UP_DURATION_LOW_OXYGEN_MODE = 4f;
  public const float CRITICAL_CHORE_GIVE_UP_OXYGEN_LEVEL_TRESHOLD = 0.25f;
  public const string ABSORB_ANIM_FILE = "anim_bionic_absorb_kanim";
  public const string ABSORB_PRE_ANIM_NAME = "absorb_pre";
  public const string ABSORB_LOOP_ANIM_NAME = "absorb_loop";
  public const string ABSORB_PST_ANIM_NAME = "absorb_pst";
  public static CellOffset MouthCellOffset = new CellOffset(0, 1);

  public BionicMassOxygenAbsorbChore(IStateMachineTarget target, bool critical)
    : base(critical ? Db.Get().ChoreTypes.BionicAbsorbOxygen_Critical : Db.Get().ChoreTypes.BionicAbsorbOxygen, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: critical ? PriorityScreen.PriorityClass.compulsory : PriorityScreen.PriorityClass.personalNeeds)
  {
    this.smi = new BionicMassOxygenAbsorbChore.Instance(this, target.gameObject);
    Func<int> data = new Func<int>(this.smi.UpdateTargetCell);
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCellUntilBegun, (object) data);
  }

  public override string ResolveString(string str)
  {
    float mass = this.smi == null ? 0.0f : this.smi.GetAverageMassConsumedPerSecond();
    return string.Format(base.ResolveString(str), (object) GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerSecond));
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null)
      Debug.LogError((object) "BionicMassAbsorbOxygenChore null context.consumer");
    else if (context.consumerState.consumer.GetSMI<BionicOxygenTankMonitor.Instance>() == null)
    {
      Debug.LogError((object) "BionicMassAbsorbOxygenChore null BionicOxygenTankMonitor.Instance");
    }
    else
    {
      this.smi.ResetMassTrackHistory();
      this.smi.sm.dupe.Set((KMonoBehaviour) context.consumerState.consumer, this.smi);
      base.Begin(context);
    }
  }

  public static bool IsNotAllowedByScheduleAndChoreIsNotCritical(
    BionicMassOxygenAbsorbChore.Instance smi)
  {
    return !BionicMassOxygenAbsorbChore.IsCriticalChore(smi) && !BionicMassOxygenAbsorbChore.IsAllowedBySchedule(smi);
  }

  public static bool IsAllowedBySchedule(BionicMassOxygenAbsorbChore.Instance smi)
  {
    return BionicOxygenTankMonitor.IsAllowedToSeekOxygenBySchedule(smi.oxygenTankMonitor);
  }

  public static bool IsCriticalChore(BionicMassOxygenAbsorbChore.Instance smi)
  {
    return smi.master.choreType == Db.Get().ChoreTypes.BionicAbsorbOxygen_Critical;
  }

  public static void ResetOxygenTimer(BionicMassOxygenAbsorbChore.Instance smi)
  {
    double num = (double) smi.sm.SecondsPassedWithoutOxygen.Set(0.0f, smi);
  }

  public static void RefreshTargetSafeCell(BionicMassOxygenAbsorbChore.Instance smi)
  {
    smi.UpdateTargetCell();
  }

  public static void UpdateTargetSafeCell(BionicMassOxygenAbsorbChore.Instance smi, float dt)
  {
    BionicMassOxygenAbsorbChore.RefreshTargetSafeCell(smi);
  }

  public static bool HasSpaceInOxygenTank(BionicMassOxygenAbsorbChore.Instance smi)
  {
    return (double) smi.oxygenTankMonitor.SpaceAvailableInTank > 0.0;
  }

  public static bool ChoreIsCriticalModeAndGiveUpOxygenLevelReached(
    BionicMassOxygenAbsorbChore.Instance smi)
  {
    return BionicMassOxygenAbsorbChore.IsCriticalChore(smi) && (double) smi.oxygenTankMonitor.OxygenPercentage >= 0.25;
  }

  public static bool BreathIsFull(BionicMassOxygenAbsorbChore.Instance smi)
  {
    AmountInstance amountInstance = smi.gameObject.GetAmounts().Get(Db.Get().Amounts.Breath);
    return (double) amountInstance.value >= (double) amountInstance.GetMax();
  }

  public static void UpdateTargetSafeCellOnlyInCriticalMode(
    BionicMassOxygenAbsorbChore.Instance smi,
    float dt)
  {
    if (!BionicMassOxygenAbsorbChore.IsCriticalChore(smi))
      return;
    BionicMassOxygenAbsorbChore.RefreshTargetSafeCell(smi);
  }

  public static void AbsorbUpdate(BionicMassOxygenAbsorbChore.Instance smi, float dt)
  {
    float mass = Mathf.Min(dt * BionicMassOxygenAbsorbChore.ABSORB_RATE, smi.oxygenTankMonitor.SpaceAvailableInTank);
    BionicMassOxygenAbsorbChore.AbsorbUpdateData callback_data = new BionicMassOxygenAbsorbChore.AbsorbUpdateData(smi, dt);
    int elementCell;
    SimHashes breathableElement = BionicMassOxygenAbsorbChore.GetNearBreathableElement(elementCell = Grid.PosToCell(smi.sm.dupe.Get(smi)), BionicMassOxygenAbsorbChore.ABSORB_RANGE, out elementCell);
    HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(BionicMassOxygenAbsorbChore.OnSimConsumeCallback), (object) callback_data, nameof (BionicMassOxygenAbsorbChore));
    SimMessages.ConsumeMass(elementCell, breathableElement, mass, (byte) 6, handle.index);
  }

  private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
  {
    BionicMassOxygenAbsorbChore.AbsorbUpdateData absorbUpdateData = (BionicMassOxygenAbsorbChore.AbsorbUpdateData) data;
    absorbUpdateData.smi.OnSimConsume(mass_cb_info, absorbUpdateData.dt);
  }

  private static void ShowOxygenBar(BionicMassOxygenAbsorbChore.Instance smi)
  {
    if (!((UnityEngine.Object) NameDisplayScreen.Instance != (UnityEngine.Object) null))
      return;
    NameDisplayScreen.Instance.SetBionicOxygenTankDisplay(smi.gameObject, new Func<float>(smi.GetOxygen), true);
  }

  private static void HideOxygenBar(BionicMassOxygenAbsorbChore.Instance smi)
  {
    if (!((UnityEngine.Object) NameDisplayScreen.Instance != (UnityEngine.Object) null))
      return;
    NameDisplayScreen.Instance.SetBionicOxygenTankDisplay(smi.gameObject, (Func<float>) null, false);
  }

  public static SimHashes GetNearBreathableElement(
    int centralCell,
    CellOffset[] range,
    out int elementCell)
  {
    float num1 = 0.0f;
    int num2 = centralCell;
    SimHashes breathableElement = SimHashes.Vacuum;
    foreach (CellOffset offset in range)
    {
      int cell = Grid.OffsetCell(centralCell, offset);
      SimHashes elementID = SimHashes.Vacuum;
      float breathableMassInCell = BionicMassOxygenAbsorbChore.GetBreathableMassInCell(cell, out elementID);
      if ((double) breathableMassInCell > (double) Mathf.Epsilon && (breathableElement == SimHashes.Vacuum || (double) breathableMassInCell > (double) num1))
      {
        breathableElement = elementID;
        num1 = breathableMassInCell;
        num2 = cell;
      }
    }
    elementCell = num2;
    return breathableElement;
  }

  private static float GetBreathableMassInCell(int cell, out SimHashes elementID)
  {
    if (Grid.IsValidCell(cell))
    {
      Element element = Grid.Element[cell];
      if (element.HasTag(GameTags.Breathable))
      {
        elementID = element.id;
        return Grid.Mass[cell];
      }
    }
    elementID = SimHashes.Vacuum;
    return 0.0f;
  }

  public class States : 
    GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore>
  {
    public BionicMassOxygenAbsorbChore.States.MoveStates move;
    public BionicMassOxygenAbsorbChore.States.MassAbsorbStates absorb;
    public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State fail;
    public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State complete;
    public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.FloatParameter SecondsPassedWithoutOxygen;
    public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.TargetParameter dupe;
    public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Signal TankFilledSignal;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.move;
      this.Target(this.dupe);
      this.root.Exit((StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback) (smi => smi.ChangeCellReservation(Grid.InvalidCell)));
      this.move.DefaultState(this.move.onGoing).ScheduleChange(this.fail, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.IsNotAllowedByScheduleAndChoreIsNotCritical));
      this.move.onGoing.Enter(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.RefreshTargetSafeCell)).Update(new Action<BionicMassOxygenAbsorbChore.Instance, float>(BionicMassOxygenAbsorbChore.UpdateTargetSafeCellOnlyInCriticalMode), UpdateRate.RENDER_1000ms).MoveTo((Func<BionicMassOxygenAbsorbChore.Instance, int>) (smi => smi.targetCell), (GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State) this.absorb, this.move.fail, true);
      this.move.fail.ReturnFailure();
      this.absorb.ScheduleChange(this.fail, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.IsNotAllowedByScheduleAndChoreIsNotCritical)).ToggleTag(GameTags.RecoveringBreath).ToggleAnims("anim_bionic_absorb_kanim").Enter(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.ShowOxygenBar)).Exit(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.HideOxygenBar)).DefaultState(this.absorb.pre);
      this.absorb.pre.PlayAnim("absorb_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.absorb.loop).ScheduleGoTo(3f, (StateMachine.BaseState) this.absorb.loop).Exit(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.ResetOxygenTimer));
      this.absorb.loop.Enter(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.ResetOxygenTimer)).ParamTransition<float>((StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Parameter<float>) this.SecondsPassedWithoutOxygen, this.absorb.pst, (StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Parameter<float>.Callback) ((smi, secondsPassed) => (double) secondsPassed > (double) smi.GetGiveupTimerTimeout())).OnSignal(this.TankFilledSignal, this.absorb.pst).PlayAnim("absorb_loop", KAnim.PlayMode.Loop).Update(new Action<BionicMassOxygenAbsorbChore.Instance, float>(BionicMassOxygenAbsorbChore.AbsorbUpdate)).Transition(this.absorb.pst, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.ChoreIsCriticalModeAndGiveUpOxygenLevelReached));
      this.absorb.pst.Transition(this.absorb.criticalRecoverBreath.pre, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.IsCriticalChore)).PlayAnim("absorb_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, (StateMachine.BaseState) this.complete);
      this.absorb.criticalRecoverBreath.ToggleAnims("anim_emotes_default_kanim").DefaultState(this.absorb.criticalRecoverBreath.pre);
      this.absorb.criticalRecoverBreath.pre.PlayAnim("breathe_pre").QueueAnim("breathe_loop").OnAnimQueueComplete(this.absorb.criticalRecoverBreath.loop);
      this.absorb.criticalRecoverBreath.loop.PlayAnim("breathe_loop", KAnim.PlayMode.Loop).ToggleAttributeModifier("Recovering Breath", (Func<BionicMassOxygenAbsorbChore.Instance, AttributeModifier>) (smi => smi.recoveringbreath)).Transition(this.absorb.criticalRecoverBreath.pst, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.BreathIsFull)).Transition(this.absorb.criticalRecoverBreath.pst, (StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback) (smi => smi.UpdateTargetCell() == Grid.InvalidCell));
      this.absorb.criticalRecoverBreath.pst.PlayAnim("breathe_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, (StateMachine.BaseState) this.complete);
      this.fail.ReturnFailure();
      this.complete.ReturnSuccess();
    }

    public class MoveStates : 
      GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State
    {
      public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State onGoing;
      public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State fail;
    }

    public class MassAbsorbStates : 
      GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State
    {
      public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pre;
      public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State loop;
      public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pst;
      public BionicMassOxygenAbsorbChore.States.MassAbsorbStates.CriticalRecover criticalRecoverBreath;

      public class CriticalRecover : 
        GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State
      {
        public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pre;
        public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State loop;
        public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pst;
      }
    }
  }

  public struct AbsorbUpdateData(BionicMassOxygenAbsorbChore.Instance smi, float dt)
  {
    public BionicMassOxygenAbsorbChore.Instance smi = smi;
    public float dt = dt;
  }

  public class Instance : 
    GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.GameInstance,
    BionicOxygenTankMonitor.IChore
  {
    public AttributeModifier recoveringbreath;
    public Queue<float> massAbsorbedHistory = new Queue<float>();
    public int targetCell = Grid.InvalidCell;
    public BionicOxygenTankMonitor.Instance oxygenTankMonitor;

    public float CRITICAL_OXYGEN_MASS_GIVE_UP_TRESHOLD => this.oxygenBreather.ConsumptionRate * 8f;

    public float GetGiveupTimerTimeout()
    {
      return this.oxygenTankMonitor == null || BionicOxygenTankMonitor.AreOxygenLevelsCritical(this.oxygenTankMonitor) ? 2f : 4f;
    }

    public OxygenBreather oxygenBreather { private set; get; }

    public Instance(BionicMassOxygenAbsorbChore master, GameObject duplicant)
      : base(master)
    {
      this.sm.dupe.Set(duplicant, this.smi, false);
      this.oxygenTankMonitor = duplicant.GetSMI<BionicOxygenTankMonitor.Instance>();
      this.oxygenBreather = duplicant.GetComponent<OxygenBreather>();
      this.recoveringbreath = new AttributeModifier(Db.Get().Amounts.Breath.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.RECOVER_BREATH_DELTA, (string) DUPLICANTS.MODIFIERS.RECOVERINGBREATH.NAME);
    }

    public bool IsConsumingOxygen() => !this.IsInsideState((StateMachine.BaseState) this.sm.move);

    public void ChangeCellReservation(int newCell)
    {
      if (this.targetCell != Grid.InvalidCell && Grid.Reserved[this.targetCell])
        Grid.Reserved[this.targetCell] = false;
      if (newCell == Grid.InvalidCell || Grid.Reserved[newCell])
        return;
      Grid.Reserved[newCell] = true;
    }

    public override void StopSM(string reason)
    {
      this.ChangeCellReservation(Grid.InvalidCell);
      base.StopSM(reason);
    }

    public int UpdateTargetCell()
    {
      this.oxygenTankMonitor.UpdatePotentialCellToAbsorbOxygen(this.targetCell);
      int absorbOxygenCell = this.oxygenTankMonitor.AbsorbOxygenCell;
      this.ChangeCellReservation(absorbOxygenCell);
      this.targetCell = absorbOxygenCell;
      return absorbOxygenCell;
    }

    public void ResetMassTrackHistory()
    {
      this.massAbsorbedHistory.Clear();
      for (int index = 0; index < 15; ++index)
        this.massAbsorbedHistory.Enqueue(0.0f);
    }

    public void AddMassToHistory(float mass_rate_this_tick)
    {
      if (this.massAbsorbedHistory.Count == 15)
      {
        double num = (double) this.massAbsorbedHistory.Dequeue();
      }
      this.massAbsorbedHistory.Enqueue(mass_rate_this_tick);
    }

    public float GetAverageMassConsumedPerSecond()
    {
      float num1 = 0.0f;
      int num2 = 0;
      foreach (float num3 in this.massAbsorbedHistory)
      {
        num1 += num3;
        ++num2;
      }
      return num2 <= 0 ? 0.0f : num1 / (float) num2;
    }

    public void OnSimConsume(Sim.MassConsumedCallback mass_cb_info, float dt)
    {
      if ((UnityEngine.Object) this.oxygenBreather == (UnityEngine.Object) null || this.oxygenTankMonitor == null || this.oxygenBreather.prefabID.HasTag(GameTags.Dead))
        return;
      this.AddMassToHistory(mass_cb_info.mass / dt);
      GameObject gameObject = this.oxygenBreather.gameObject;
      int num1 = BionicOxygenTankMonitor.AreOxygenLevelsCritical(this.oxygenTankMonitor) ? 1 : 0;
      float num2 = num1 != 0 ? this.CRITICAL_OXYGEN_MASS_GIVE_UP_TRESHOLD : 2f;
      if ((double) this.GetAverageMassConsumedPerSecond() <= (double) num2)
      {
        double num3 = (double) this.sm.SecondsPassedWithoutOxygen.Set(this.sm.SecondsPassedWithoutOxygen.Get(this.smi) + dt, this.smi);
      }
      else
        BionicMassOxygenAbsorbChore.ResetOxygenTimer(this.smi);
      if (num1 != 0)
      {
        float num4 = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE * DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND;
        if ((double) mass_cb_info.mass == 0.0)
          mass_cb_info.temperature = DUPLICANTSTATS.BIONICS.Temperature.Internal.IDEAL;
        mass_cb_info.mass += (float) ((double) DUPLICANTSTATS.STANDARD.BaseStats.RECOVER_BREATH_DELTA * (double) num4 * (double) dt + (double) DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * (double) dt);
      }
      float mass = this.oxygenTankMonitor.AddGas(mass_cb_info);
      if ((double) mass > (double) Mathf.Epsilon)
        SimMessages.EmitMass(Grid.PosToCell(gameObject), mass_cb_info.elemIdx, mass, mass_cb_info.temperature, byte.MaxValue, 0);
      if (BionicMassOxygenAbsorbChore.HasSpaceInOxygenTank(this))
        return;
      this.sm.TankFilledSignal.Trigger(this);
    }

    public float GetOxygen()
    {
      return this.oxygenTankMonitor != null ? this.oxygenTankMonitor.OxygenPercentage : 0.0f;
    }
  }
}
