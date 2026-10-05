// Decompiled with JetBrains decompiler
// Type: ReachabilityMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ReachabilityMonitor : 
  GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable>
{
  public GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.State reachable;
  public GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.State unreachable;
  public StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.BoolParameter isReachable = new StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.BoolParameter(false);
  private static ReachabilityMonitor.UpdateReachabilityCB updateReachabilityCB = new ReachabilityMonitor.UpdateReachabilityCB();

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.unreachable;
    this.serializable = StateMachine.SerializeType.Never;
    this.root.FastUpdate("UpdateReachability", (UpdateBucketWithUpdater<ReachabilityMonitor.Instance>.IUpdater) ReachabilityMonitor.updateReachabilityCB, UpdateRate.SIM_1000ms, true);
    this.reachable.Enter((StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.State.Callback) (smi => smi.Get<KPrefabID>().AddTag(GameTags.Reachable))).Exit((StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.State.Callback) (smi => smi.Get<KPrefabID>().RemoveTag(GameTags.Reachable))).Enter("TriggerEvent", (StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.State.Callback) (smi => smi.TriggerEvent())).ParamTransition<bool>((StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.Parameter<bool>) this.isReachable, this.unreachable, GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.IsFalse);
    this.unreachable.Enter("TriggerEvent", (StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.State.Callback) (smi => smi.TriggerEvent())).ParamTransition<bool>((StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.Parameter<bool>) this.isReachable, this.reachable, GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.IsTrue);
  }

  private class UpdateReachabilityCB : UpdateBucketWithUpdater<ReachabilityMonitor.Instance>.IUpdater
  {
    public void Update(ReachabilityMonitor.Instance smi, float dt) => smi.UpdateReachability();
  }

  public new class Instance : 
    GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.GameInstance
  {
    public Instance(Workable workable)
      : base(workable)
    {
      this.UpdateReachability();
    }

    public void TriggerEvent()
    {
      this.Trigger(-1432940121, (object) BoxedBools.Box(this.sm.isReachable.Get(this.smi)));
    }

    public void UpdateReachability()
    {
      if ((Object) this.master == (Object) null)
        return;
      int cell = this.master.GetCell();
      this.sm.isReachable.Set(MinionGroupProber.Get().IsAllReachable(cell, this.master.GetOffsets(cell)), this.smi);
    }
  }
}
