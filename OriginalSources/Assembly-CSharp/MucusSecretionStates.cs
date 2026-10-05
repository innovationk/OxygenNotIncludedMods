// Decompiled with JetBrains decompiler
// Type: MucusSecretionStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MucusSecretionStates : 
  GameStateMachine<MucusSecretionStates, MucusSecretionStates.Instance, IStateMachineTarget, MucusSecretionStates.Def>
{
  public GameStateMachine<MucusSecretionStates, MucusSecretionStates.Instance, IStateMachineTarget, MucusSecretionStates.Def>.State secretePre;
  public GameStateMachine<MucusSecretionStates, MucusSecretionStates.Instance, IStateMachineTarget, MucusSecretionStates.Def>.State behaviourComplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.secretePre;
    this.secretePre.QueueAnim("poop").Exit(new StateMachine<MucusSecretionStates, MucusSecretionStates.Instance, IStateMachineTarget, MucusSecretionStates.Def>.State.Callback(MucusSecretionStates.Secrete)).OnAnimQueueComplete(this.behaviourComplete);
    this.behaviourComplete.BehaviourComplete(GameTags.Creatures.Behaviours.SecretingMucusBehavior);
  }

  private static void Secrete(MucusSecretionStates.Instance smi)
  {
    smi.position = smi.transform.GetPosition();
    smi.GetSMI<MoistureMonitor.Instance>()?.ProduceLubricant();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<MucusSecretionStates, MucusSecretionStates.Instance, IStateMachineTarget, MucusSecretionStates.Def>.GameInstance
  {
    public Vector3 position;

    public Instance(Chore<MucusSecretionStates.Instance> chore, MucusSecretionStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Behaviours.SecretingMucusBehavior);
    }
  }
}
