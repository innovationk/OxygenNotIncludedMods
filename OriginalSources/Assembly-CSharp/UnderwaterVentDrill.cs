// Decompiled with JetBrains decompiler
// Type: UnderwaterVentDrill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UnderwaterVentDrill : 
  GameStateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>
{
  private const string OFF_ANIM_NAME = "idle";
  private const string IDLE_ANIM_NAME = "idle";
  private const string PRE_ANIM_NAME = "working_pre";
  private const string LOOP_ANIM_NAME = "working_loop";
  private const string PST_ANIM_NAME = "working_pst";
  private const string VENT_PRE_ANIM_NAME = "drill_pre";
  private const string VENT_LOOP_ANIM_NAME = "drill_loop";
  private const string VENT_PST_ANIM_NAME = "drill_pst";
  private const string METER_TARGET_NAME = "target_meter";
  private const string METER_ANIM_NAME = "meter";
  public GameStateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State noOperational;
  public UnderwaterVentDrill.OperationalStates operational;
  public StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.FloatParameter DrillProgress = new StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.FloatParameter(0.0f);
  public StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.TargetParameter Vent;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.noOperational;
    this.noOperational.TagTransition(GameTags.Operational, (GameStateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State) this.operational).Enter(new StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State.Callback(UnderwaterVentDrill.UpdateDiamondMeter)).PlayAnim("idle");
    this.operational.Enter(new StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State.Callback(UnderwaterVentDrill.UpdateDiamondMeter)).DefaultState(this.operational.idle);
    this.operational.idle.Target(this.Vent).EventTransition(GameHashes.VentBlocked, (GameStateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State) this.operational.working, new StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.Transition.ConditionCallback(UnderwaterVentDrill.CanWork)).Target(this.masterTarget).TagTransition(GameTags.Operational, this.noOperational, true).EventTransition(GameHashes.OnStorageChange, this.operational.missingDiamonds, GameStateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.Not(new StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.Transition.ConditionCallback(UnderwaterVentDrill.HasAnyDiamond))).PlayAnim("idle").ToggleStatusItem(Db.Get().BuildingStatusItems.UnderwaterDrillIdle);
    this.operational.missingDiamonds.TagTransition(GameTags.Operational, this.noOperational, true).EventTransition(GameHashes.OnStorageChange, this.operational.idle, new StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.Transition.ConditionCallback(UnderwaterVentDrill.HasAnyDiamond)).EventHandler(GameHashes.OnStorageChange, new StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State.Callback(UnderwaterVentDrill.UpdateDiamondMeter)).PlayAnim("idle");
    this.operational.working.ToggleStatusItem(Db.Get().BuildingStatusItems.UnderwaterDrillActive).Toggle("HeatProduction", new StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State.Callback(UnderwaterVentDrill.EnableHeatProduction), new StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State.Callback(UnderwaterVentDrill.DisableHeatProduction)).DefaultState(this.operational.working.pre);
    this.operational.working.pre.Target(this.Vent).PlayAnim("drill_pre").Target(this.masterTarget).PlayAnim("working_pre").OnAnimQueueComplete(this.operational.working.loop);
    this.operational.working.loop.Target(this.Vent).PlayAnim("drill_loop", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().MiscStatusItems.UnderwaterVentBeingDrilled).Target(this.masterTarget).TagTransition(GameTags.Operational, this.operational.working.pst, true).UpdateTransition(this.operational.working.pst, new Func<UnderwaterVentDrill.Instance, float, bool>(UnderwaterVentDrill.DrillUpdate)).Toggle("ToggleProgressBar", new StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State.Callback(UnderwaterVentDrill.CreateProgressBar), new StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State.Callback(UnderwaterVentDrill.ClearProgressBar)).PlayAnim("working_loop", KAnim.PlayMode.Loop);
    this.operational.working.pst.Target(this.Vent).PlayAnim("drill_pst").Target(this.masterTarget).PlayAnim("working_pst").OnAnimQueueComplete(this.operational.workEnded);
    this.operational.workEnded.ParamTransition<float>((StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.Parameter<float>) this.DrillProgress, this.operational.completed, GameStateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.IsGTEOne).GoTo(this.operational.missingDiamonds);
    this.operational.completed.Enter(new StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State.Callback(UnderwaterVentDrill.ResetDrillProgress)).Enter(new StateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State.Callback(UnderwaterVentDrill.UnblockVent)).EnterGoTo(this.operational.idle);
  }

  private static void EnableHeatProduction(UnderwaterVentDrill.Instance smi)
  {
    smi.SetOperationalActiveFlag(true);
  }

  private static void DisableHeatProduction(UnderwaterVentDrill.Instance smi)
  {
    smi.SetOperationalActiveFlag(false);
  }

  private static void ResetDrillProgress(UnderwaterVentDrill.Instance smi)
  {
    double num = (double) smi.sm.DrillProgress.Set(0.0f, smi);
  }

  private static void UnblockVent(UnderwaterVentDrill.Instance smi) => smi.UnblockVent();

  private static void CreateProgressBar(UnderwaterVentDrill.Instance smi)
  {
    smi.CreateProgressBar();
  }

  private static void ClearProgressBar(UnderwaterVentDrill.Instance smi) => smi.ClearProgressBar();

  private static bool DrillUpdate(UnderwaterVentDrill.Instance smi, float dt)
  {
    return smi.DrillUpdate(dt);
  }

  private static bool CanWork(UnderwaterVentDrill.Instance smi) => smi.CanWork;

  private static bool HasAnyDiamond(UnderwaterVentDrill.Instance smi) => smi.HasAnyDiamond;

  private static void UpdateDiamondMeter(UnderwaterVentDrill.Instance smi)
  {
    smi.UpdateDiamondMeter();
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public Tag DiamondTag = SimHashes.Diamond.CreateTag();
    public float DiamondConsumptionRate;
    public float WorkDuration;
    public Vector3 ProgressBarOffset = Vector3.zero;

    public List<Descriptor> GetDescriptors(GameObject go)
    {
      List<Descriptor> descriptors = new List<Descriptor>();
      string formattedMass = GameUtil.GetFormattedMass(this.DiamondConsumptionRate, GameUtil.TimeSlice.PerSecond);
      descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.UNDERWATER_DRILL_DIAMOND_CONSUMPTION.Replace("{Rate}", formattedMass), UI.BUILDINGEFFECTS.TOOLTIPS.UNDERWATER_DRILL_DIAMOND_CONSUMPTION.Replace("{Rate}", formattedMass), Descriptor.DescriptorType.Requirement));
      return descriptors;
    }
  }

  public class OperationalStates : 
    GameStateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State
  {
    public GameStateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State idle;
    public GameStateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State missingDiamonds;
    public GameStateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.PreLoopPostState working;
    public GameStateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State workEnded;
    public GameStateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.State completed;
  }

  public new class Instance : 
    GameStateMachine<UnderwaterVentDrill, UnderwaterVentDrill.Instance, IStateMachineTarget, UnderwaterVentDrill.Def>.GameInstance
  {
    private Storage storage;
    private UnderwaterVent.Instance vent;
    private Operational operational;
    private ProgressBar progressBar;
    private MeterController diamondMeter;

    public float DrillProgress => this.sm.DrillProgress.Get(this);

    public bool CanWork => this.HasAnyDiamond && this.IsVentBlocked;

    public bool HasAnyDiamond => (double) this.storage.GetMassAvailable(this.def.DiamondTag) > 0.0;

    public bool IsVentBlocked => this.vent != null && this.vent.IsBlocked;

    public bool IsOff => this.IsInsideState((StateMachine.BaseState) this.sm.noOperational);

    public Instance(IStateMachineTarget master, UnderwaterVentDrill.Def def)
      : base(master, def)
    {
      this.storage = this.GetComponent<Storage>();
      this.operational = this.GetComponent<Operational>();
      this.diamondMeter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "target_meter", "meter", Meter.Offset.Infront, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
    }

    public override void StartSM()
    {
      int cell = Grid.PosToCell(this.gameObject);
      GameObject go = Grid.Objects[cell, 1];
      this.vent = (UnityEngine.Object) go == (UnityEngine.Object) null ? (UnderwaterVent.Instance) null : go.GetSMI<UnderwaterVent.Instance>();
      this.sm.Vent.Set(this.vent == null ? (GameObject) null : this.vent.gameObject, this, false);
      base.StartSM();
      this.UpdateDiamondMeter();
    }

    public void SetOperationalActiveFlag(bool active) => this.operational.SetActive(active);

    public bool DrillUpdate(float dt)
    {
      if ((double) dt == 0.0)
        return false;
      float a = dt * this.def.DiamondConsumptionRate;
      float massAvailable = this.storage.GetMassAvailable(this.def.DiamondTag);
      float amount = Mathf.Min(a, massAvailable);
      float num1 = amount / a;
      float num2 = dt / this.def.WorkDuration * num1;
      this.storage.ConsumeIgnoringDisease(this.def.DiamondTag, amount);
      double num3 = (double) this.sm.DrillProgress.Set(this.DrillProgress + num2, this);
      int num4 = !this.HasAnyDiamond ? 1 : ((double) this.DrillProgress >= 1.0 ? 1 : 0);
      this.UpdateDiamondMeter();
      return num4 != 0;
    }

    public void UpdateDiamondMeter()
    {
      if (this.diamondMeter == null)
        return;
      this.diamondMeter.SetPositionPercent(this.IsOff ? 0.0f : this.storage.MassStored() / this.storage.Capacity());
    }

    public void UnblockVent()
    {
      if (this.vent == null)
        return;
      this.vent.Unblock();
    }

    public void CreateProgressBar()
    {
      this.progressBar = ProgressBar.CreateProgressBar(this.gameObject, (Func<float>) (() => this.DrillProgress), this.def.ProgressBarOffset);
      this.progressBar.SetVisibility(true);
    }

    public void ClearProgressBar()
    {
      if (!((UnityEngine.Object) this.progressBar != (UnityEngine.Object) null))
        return;
      Util.KDestroyGameObject(this.progressBar.gameObject);
      this.progressBar = (ProgressBar) null;
    }
  }
}
