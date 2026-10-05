// Decompiled with JetBrains decompiler
// Type: RocketSelfDestructMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class RocketSelfDestructMonitor : 
  GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance>
{
  public GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.State idle;
  public GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.State exploding;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.EventTransition(GameHashes.RocketSelfDestructRequested, this.exploding);
    this.exploding.Update((System.Action<RocketSelfDestructMonitor.Instance, float>) ((smi, dt) =>
    {
      if ((double) smi.timeinstate < 3.0)
        return;
      smi.master.Trigger(-1311384361);
      smi.GoTo((StateMachine.BaseState) this.idle);
    }));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, RocketSelfDestructMonitor.Def def) : 
    GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    public KBatchedAnimController eyes;
  }
}
