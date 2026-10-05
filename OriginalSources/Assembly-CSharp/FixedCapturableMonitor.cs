// Decompiled with JetBrains decompiler
// Type: FixedCapturableMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class FixedCapturableMonitor : 
  GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleBehaviour(GameTags.Creatures.WantsToGetCaptured, (StateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>.Transition.ConditionCallback) (smi => smi.ShouldGoGetCaptured())).Enter((StateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>.State.Callback) (smi => Components.FixedCapturableMonitors.Add(smi))).Exit((StateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>.State.Callback) (smi => Components.FixedCapturableMonitors.Remove(smi)));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>.GameInstance
  {
    public FixedCapturePoint.Instance targetCapturePoint;
    public ChoreConsumer ChoreConsumer;
    public Navigator Navigator;
    public Tag PrefabTag;
    public bool isBaby;

    public Instance(IStateMachineTarget master, FixedCapturableMonitor.Def def)
      : base(master, def)
    {
      this.ChoreConsumer = this.GetComponent<ChoreConsumer>();
      this.Navigator = this.GetComponent<Navigator>();
      this.PrefabTag = this.GetComponent<KPrefabID>().PrefabTag;
      this.isBaby = master.gameObject.GetDef<BabyMonitor.Def>() != null;
    }

    public bool ShouldGoGetCaptured()
    {
      if (this.targetCapturePoint == null || !this.targetCapturePoint.IsRunning() || !this.targetCapturePoint.shouldCreatureGoGetCaptured)
        return false;
      return !this.isBaby || this.targetCapturePoint.def.allowBabies;
    }
  }
}
