// Decompiled with JetBrains decompiler
// Type: QuarantineFeedableMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class QuarantineFeedableMonitor : 
  GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance>
{
  public GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.State hungry;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
    this.satisfied.EventTransition(GameHashes.AddUrge, this.hungry, (StateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsHungry()));
    this.hungry.EventTransition(GameHashes.RemoveUrge, this.satisfied, (StateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsHungry()));
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<QuarantineFeedableMonitor, QuarantineFeedableMonitor.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    public bool IsHungry() => this.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Eat);
  }
}
