// Decompiled with JetBrains decompiler
// Type: StorageUnloadMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class StorageUnloadMonitor : 
  GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>
{
  public StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage> internalStorage = new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage>();
  public StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage> sweepLocker;
  public GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.State notFull;
  public GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.State full;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.notFull;
    this.notFull.Transition(this.full, new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Transition.ConditionCallback(StorageUnloadMonitor.WantsToUnload));
    this.full.ToggleStatusItem(Db.Get().RobotStatusItems.DustBinFull, (Func<StorageUnloadMonitor.Instance, object>) (smi => (object) smi.gameObject)).ToggleBehaviour(GameTags.Robots.Behaviours.UnloadBehaviour, (StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Transition.ConditionCallback) (data => true)).Transition(this.notFull, GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Not(new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Transition.ConditionCallback(StorageUnloadMonitor.WantsToUnload)), UpdateRate.SIM_1000ms).Enter((StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.State.Callback) (smi =>
    {
      if ((double) smi.master.gameObject.GetComponents<Storage>()[1].RemainingCapacity() > 0.0)
        return;
      smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim((HashedString) "react_full");
    }));
  }

  public static bool WantsToUnload(StorageUnloadMonitor.Instance smi)
  {
    Storage cmp = smi.sm.sweepLocker.Get(smi);
    return !((UnityEngine.Object) cmp == (UnityEngine.Object) null) && !((UnityEngine.Object) smi.sm.internalStorage.Get(smi) == (UnityEngine.Object) null) && !smi.HasTag(GameTags.Robots.Behaviours.RechargeBehaviour) && (smi.sm.internalStorage.Get(smi).IsFull() || (UnityEngine.Object) cmp != (UnityEngine.Object) null && !smi.sm.internalStorage.Get(smi).IsEmpty() && Grid.PosToCell((KMonoBehaviour) cmp) == Grid.PosToCell((StateMachine.Instance) smi));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, StorageUnloadMonitor.Def def) : 
    GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.GameInstance(master, def)
  {
  }
}
