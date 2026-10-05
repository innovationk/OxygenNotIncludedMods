// Decompiled with JetBrains decompiler
// Type: RoverChoreMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

#nullable disable
public class RoverChoreMonitor : 
  GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>
{
  public GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>.State loop;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.loop;
    this.loop.ToggleBehaviour(GameTags.Creatures.Tunnel, (StateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>.Transition.ConditionCallback) (smi => true)).ToggleBehaviour(GameTags.Creatures.Builder, (StateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>.Transition.ConditionCallback) (smi => true));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, RoverChoreMonitor.Def def) : 
    GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>.GameInstance(master, def)
  {
    [Serialize]
    public int lastDigCell = -1;
    private System.Action<object> OnDestinationReachedDelegate;

    protected override void OnCleanUp() => base.OnCleanUp();
  }
}
