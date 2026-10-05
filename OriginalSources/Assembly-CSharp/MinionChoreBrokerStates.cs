// Decompiled with JetBrains decompiler
// Type: MinionChoreBrokerStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class MinionChoreBrokerStates : 
  GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>
{
  private GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>.State hasChore;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.hasChore;
    this.root.DoNothing();
    this.hasChore.Enter((StateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>.State.Callback) (smi => { }));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(
    Chore<MinionChoreBrokerStates.Instance> chore,
    MinionChoreBrokerStates.Def def) : 
    GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>.GameInstance((IStateMachineTarget) chore, def)
  {
  }
}
