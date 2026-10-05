// Decompiled with JetBrains decompiler
// Type: CritterRoarStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class CritterRoarStates : 
  GameStateMachine<CritterRoarStates, CritterRoarStates.Instance, IStateMachineTarget, CritterRoarStates.Def>
{
  private readonly GameStateMachine<CritterRoarStates, CritterRoarStates.Instance, IStateMachineTarget, CritterRoarStates.Def>.State roar;
  private readonly GameStateMachine<CritterRoarStates, CritterRoarStates.Instance, IStateMachineTarget, CritterRoarStates.Def>.State behaviourComplete;
  private const float FALLBACK_TIMEOUT = 10f;
  private static HashedString ANIM = (HashedString) nameof (roar);
  private static readonly HashedString[] ANIM_SEQUENCE = new HashedString[1]
  {
    CritterRoarStates.ANIM
  };
  private static Tag TAG = CritterRoarMonitor.TAG;

  public override void InitializeStates(out StateMachine.BaseState defaultState)
  {
    defaultState = (StateMachine.BaseState) this.roar;
    this.roar.PlayAnims((Func<CritterRoarStates.Instance, HashedString[]>) (smi => CritterRoarStates.ANIM_SEQUENCE)).ScheduleGoTo(10f, (StateMachine.BaseState) this.behaviourComplete).OnAnimQueueComplete(this.behaviourComplete);
    this.behaviourComplete.BehaviourComplete(CritterRoarStates.TAG);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<CritterRoarStates, CritterRoarStates.Instance, IStateMachineTarget, CritterRoarStates.Def>.GameInstance
  {
    public Instance(Chore<CritterRoarStates.Instance> chore, CritterRoarStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) CritterRoarStates.TAG);
    }
  }
}
