// Decompiled with JetBrains decompiler
// Type: DropUnusedInventoryChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class DropUnusedInventoryChore : Chore<DropUnusedInventoryChore.StatesInstance>
{
  public DropUnusedInventoryChore(ChoreType chore_type, IStateMachineTarget target)
    : base(chore_type, target, target.GetComponent<ChoreProvider>(), master_priority_class: PriorityScreen.PriorityClass.compulsory)
  {
    this.smi = new DropUnusedInventoryChore.StatesInstance(this);
  }

  public class StatesInstance(DropUnusedInventoryChore master) : 
    GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore>
  {
    public GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State dropping;
    public GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State success;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.dropping;
      this.dropping.Enter((StateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State.Callback) (smi => smi.GetComponent<Storage>().DropAll())).GoTo(this.success);
      this.success.ReturnSuccess();
    }
  }
}
