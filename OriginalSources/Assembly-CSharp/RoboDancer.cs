// Decompiled with JetBrains decompiler
// Type: RoboDancer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TUNING;

#nullable disable
public class RoboDancer : GameStateMachine<RoboDancer, RoboDancer.Instance>
{
  public StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.FloatParameter timeSpentDancing;
  public StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.BoolParameter hasAudience;
  public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State neutral;
  public RoboDancer.OverjoyedStates overjoyed;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.neutral;
    this.root.TagTransition(GameTags.Dead, (GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State) null);
    this.neutral.TagTransition(GameTags.Overjoyed, (GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State) this.overjoyed);
    double num;
    this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<float>((StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.Parameter<float>) this.timeSpentDancing, this.overjoyed.exitEarly, (StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.Parameter<float>.Callback) ((smi, p) => (double) p >= (double) TRAITS.JOY_REACTIONS.ROBO_DANCER.DANCE_DURATION && !this.hasAudience.Get(smi))).Exit((StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State.Callback) (smi => num = (double) this.timeSpentDancing.Set(0.0f, smi)));
    this.overjoyed.idle.Enter((StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if (!smi.IsRecTime())
        return;
      smi.GoTo((StateMachine.BaseState) this.overjoyed.dancing);
    })).ToggleStatusItem(Db.Get().DuplicantStatusItems.RoboDancerPlanning).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.dancing, (StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsRecTime()));
    this.overjoyed.dancing.ToggleStatusItem(Db.Get().DuplicantStatusItems.RoboDancerDancing).EventTransition(GameHashes.ScheduleBlocksTick, this.overjoyed.idle, (StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsRecTime())).ToggleChore((Func<RoboDancer.Instance, Chore>) (smi => (Chore) new RoboDancerChore(smi.master)), this.overjoyed.idle);
    this.overjoyed.exitEarly.Enter((StateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.ExitJoyReactionEarly()));
  }

  public class OverjoyedStates : 
    GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State idle;
    public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State dancing;
    public GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.State exitEarly;
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<RoboDancer, RoboDancer.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    public bool IsRecTime()
    {
      return this.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
    }

    public void ExitJoyReactionEarly()
    {
      JoyBehaviourMonitor.Instance smi = this.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
      smi.sm.exitEarly.Trigger(smi);
    }
  }
}
