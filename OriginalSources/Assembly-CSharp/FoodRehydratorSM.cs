// Decompiled with JetBrains decompiler
// Type: FoodRehydratorSM
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class FoodRehydratorSM : 
  GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>
{
  private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State off;
  private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State on;
  private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State active;
  private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State postactive;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.EnterTransition(this.off, (StateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.Transition.ConditionCallback) (smi => !smi.operational.IsFunctional)).EnterTransition(this.on, (StateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.Transition.ConditionCallback) (smi => smi.operational.IsFunctional));
    this.off.PlayAnim("off", KAnim.PlayMode.Loop).EnterTransition(this.on, (StateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.Transition.ConditionCallback) (smi => smi.operational.IsFunctional)).EventTransition(GameHashes.FunctionalChanged, this.on, (StateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.Transition.ConditionCallback) (smi => smi.operational.IsFunctional));
    this.on.PlayAnim("on", KAnim.PlayMode.Loop).EnterTransition(this.off, (StateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.Transition.ConditionCallback) (smi => !smi.operational.IsFunctional)).EnterTransition(this.active, (StateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.Transition.ConditionCallback) (smi => smi.operational.IsActive)).EventTransition(GameHashes.FunctionalChanged, this.off, (StateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.Transition.ConditionCallback) (smi => !smi.operational.IsFunctional)).EventTransition(GameHashes.ActiveChanged, this.active, (StateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.Transition.ConditionCallback) (smi => smi.operational.IsActive));
    this.active.EnterTransition(this.off, (StateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.Transition.ConditionCallback) (smi => !smi.operational.IsFunctional)).EnterTransition(this.on, (StateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.Transition.ConditionCallback) (smi => !smi.operational.IsActive)).EventTransition(GameHashes.FunctionalChanged, this.off, (StateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.Transition.ConditionCallback) (smi => !smi.operational.IsFunctional)).EventTransition(GameHashes.ActiveChanged, this.postactive, (StateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.Transition.ConditionCallback) (smi => !smi.operational.IsActive));
    this.postactive.OnAnimQueueComplete(this.on);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class StatesInstance(IStateMachineTarget master, FoodRehydratorSM.Def def) : 
    GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.GameInstance(master, def)
  {
    [MyCmpReq]
    public Operational operational;
  }
}
