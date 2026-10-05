// Decompiled with JetBrains decompiler
// Type: FoodSmoker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class FoodSmoker : 
  GameStateMachine<FoodSmoker, FoodSmoker.StatesInstance, IStateMachineTarget, FoodSmoker.Def>
{
  private static readonly Operational.Flag foodSmokerFlag = new Operational.Flag("food_smoker", Operational.Flag.Type.Requirement);
  private GameStateMachine<FoodSmoker, FoodSmoker.StatesInstance, IStateMachineTarget, FoodSmoker.Def>.State working;
  private GameStateMachine<FoodSmoker, FoodSmoker.StatesInstance, IStateMachineTarget, FoodSmoker.Def>.State requestEmpty;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.working;
    this.working.Enter((StateMachine<FoodSmoker, FoodSmoker.StatesInstance, IStateMachineTarget, FoodSmoker.Def>.State.Callback) (smi =>
    {
      smi.complexFabricator.SetQueueDirty();
      smi.operational.SetFlag(FoodSmoker.foodSmokerFlag, true);
    })).EnterTransition(this.requestEmpty, (StateMachine<FoodSmoker, FoodSmoker.StatesInstance, IStateMachineTarget, FoodSmoker.Def>.Transition.ConditionCallback) (smi => smi.RequiresEmptying())).EventHandlerTransition(GameHashes.FabricatorOrderCompleted, this.requestEmpty, (Func<FoodSmoker.StatesInstance, object, bool>) ((smi, data) => smi.RequiresEmptying()));
    this.requestEmpty.ToggleRecurringChore(new Func<FoodSmoker.StatesInstance, Chore>(this.CreateChore), new System.Action<FoodSmoker.StatesInstance, Chore>(FoodSmoker.SetRemoteChore), (Func<FoodSmoker.StatesInstance, bool>) (smi => smi.RequiresEmptying())).EventHandlerTransition(GameHashes.OnStorageChange, this.working, (Func<FoodSmoker.StatesInstance, object, bool>) ((smi, data) => !smi.RequiresEmptying())).Enter((StateMachine<FoodSmoker, FoodSmoker.StatesInstance, IStateMachineTarget, FoodSmoker.Def>.State.Callback) (smi => smi.operational.SetFlag(FoodSmoker.foodSmokerFlag, false))).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding);
  }

  private static void SetRemoteChore(FoodSmoker.StatesInstance smi, Chore chore)
  {
    smi.remoteChore.SetChore(chore);
  }

  private Chore CreateChore(FoodSmoker.StatesInstance smi)
  {
    WorkChore<FoodSmokerWorkableEmpty> chore = new WorkChore<FoodSmokerWorkableEmpty>(Db.Get().ChoreTypes.Cook, (IStateMachineTarget) smi.master.GetComponent<FoodSmokerWorkableEmpty>(), on_complete: new System.Action<Chore>(smi.OnEmptyComplete), only_when_operational: false);
    chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, (object) null);
    chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanGasRange.Id);
    return (Chore) chore;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class StatesInstance(IStateMachineTarget master, FoodSmoker.Def def) : 
    GameStateMachine<FoodSmoker, FoodSmoker.StatesInstance, IStateMachineTarget, FoodSmoker.Def>.GameInstance(master, def)
  {
    [MyCmpAdd]
    public ManuallySetRemoteWorkTargetComponent remoteChore;
    [MyCmpReq]
    public Operational operational;
    [MyCmpReq]
    public ComplexFabricator complexFabricator;

    public bool RequiresEmptying() => !this.complexFabricator.outStorage.IsEmpty();

    public void OnEmptyComplete(Chore obj)
    {
      this.complexFabricator.outStorage.DropAll(Grid.CellToPosLCC(Grid.PosToCell((StateMachine.Instance) this), Grid.SceneLayer.Ore), dump_liquid: true);
    }
  }
}
