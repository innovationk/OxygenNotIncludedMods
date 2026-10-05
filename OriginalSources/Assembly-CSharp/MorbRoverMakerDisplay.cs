// Decompiled with JetBrains decompiler
// Type: MorbRoverMakerDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class MorbRoverMakerDisplay : 
  GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>
{
  public const string METER_TARGET_NAME = "meter_display_target";
  public const string OFF_IDLE_ANIM_NAME = "display_off_idle";
  public const string OFF_ENTERING_ANIM_NAME = "display_off";
  public const string OFF_EXITING_ANIM_NAME = "display_on";
  public const string GERM_ICON_ANIM_NAME = "display_germ";
  public const string NO_GERM_ANIM_NAME = "display_no_germ";
  public const string ON_IDLE_ANIM_NAME = "display_idle";
  public StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.TargetParameter monitor;
  public MorbRoverMakerDisplay.OffStates off;
  public MorbRoverMakerDisplay.OnStates on;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.Never;
    default_state = (StateMachine.BaseState) this.off.idle;
    this.root.Target(this.monitor);
    this.off.DefaultState(this.off.idle);
    this.off.entering.PlayAnim("display_off").OnAnimQueueComplete(this.off.idle);
    this.off.idle.Target(this.masterTarget).EventTransition(GameHashes.TagsChanged, this.off.exiting, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.ShouldBeOn)).Target(this.monitor).PlayAnim("display_off_idle", KAnim.PlayMode.Loop);
    this.off.exiting.PlayAnim("display_on").OnAnimQueueComplete((GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State) this.on);
    this.on.Target(this.masterTarget).TagTransition(GameTags.Operational, this.off.entering, true).Target(this.monitor).DefaultState(this.on.idle);
    this.on.idle.Transition(this.on.germ, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.HasGermsAddedAndGermsAreNeeded)).Transition(this.on.noGerm, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.NoGermsAddedAndGermsAreNeeded)).PlayAnim("display_idle", KAnim.PlayMode.Loop);
    this.on.noGerm.Transition(this.on.idle, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.GermsNoLongerNeeded)).Transition(this.on.germ, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.HasGermsAddedAndGermsAreNeeded)).PlayAnim("display_no_germ", KAnim.PlayMode.Loop);
    this.on.germ.Transition(this.on.idle, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.GermsNoLongerNeeded)).Transition(this.on.noGerm, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.NoGermsAddedAndGermsAreNeeded)).PlayAnim("display_germ", KAnim.PlayMode.Loop);
  }

  public static bool NoGermsAddedAndGermsAreNeeded(MorbRoverMakerDisplay.Instance smi)
  {
    return smi.GermsAreNeeded && !smi.HasRecentlyConsumedGerms;
  }

  public static bool HasGermsAddedAndGermsAreNeeded(MorbRoverMakerDisplay.Instance smi)
  {
    return smi.GermsAreNeeded && smi.HasRecentlyConsumedGerms;
  }

  public static bool ShouldBeOn(MorbRoverMakerDisplay.Instance smi) => smi.ShouldBeOn();

  public static bool GermsNoLongerNeeded(MorbRoverMakerDisplay.Instance smi) => !smi.GermsAreNeeded;

  public class Def : StateMachine.BaseDef
  {
    public float Timeout = 1f;
  }

  public class OffStates : 
    GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State
  {
    public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State entering;
    public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State idle;
    public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State exiting;
  }

  public class OnStates : 
    GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State
  {
    public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State idle;
    public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State shake;
    public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State noGerm;
    public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State germ;
    public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State checkmark;
  }

  public new class Instance : 
    GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.GameInstance
  {
    private float lastTimeGermsConsumed = -1f;
    [MyCmpReq]
    private Operational operational;
    private MorbRoverMaker.Instance morbRoverMaker;
    private MeterController meter;

    public bool HasRecentlyConsumedGerms
    {
      get
      {
        return (double) GameClock.Instance.GetTime() - (double) this.lastTimeGermsConsumed < (double) this.def.Timeout;
      }
    }

    public bool GermsAreNeeded => (double) this.morbRoverMaker.MorbDevelopment_Progress < 1.0;

    public Instance(IStateMachineTarget master, MorbRoverMakerDisplay.Def def)
      : base(master, def)
    {
      this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_display_target", "display_off_idle", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
      this.sm.monitor.Set(this.meter.gameObject, this.smi, false);
    }

    public override void StartSM()
    {
      this.morbRoverMaker = this.gameObject.GetSMI<MorbRoverMaker.Instance>();
      this.morbRoverMaker.GermsAdded += new System.Action<long>(this.OnGermsAdded);
      this.morbRoverMaker.OnUncovered += new System.Action(this.OnUncovered);
      base.StartSM();
    }

    private void OnGermsAdded(long amount)
    {
      this.lastTimeGermsConsumed = GameClock.Instance.GetTime();
    }

    public bool ShouldBeOn()
    {
      return this.morbRoverMaker.HasBeenRevealed && this.operational.IsOperational;
    }

    private void OnUncovered()
    {
      if (!this.IsInsideState((StateMachine.BaseState) this.sm.off.idle))
        return;
      this.GoTo((StateMachine.BaseState) this.sm.off.exiting);
    }
  }
}
