// Decompiled with JetBrains decompiler
// Type: RemoteWorkerDockAnimSM
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
internal class RemoteWorkerDockAnimSM : StateMachineComponent<RemoteWorkerDockAnimSM.StatesInstance>
{
  [MyCmpAdd]
  private RemoteWorkerDock dock;
  [MyCmpGet]
  private Operational operational;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public class StatesInstance(RemoteWorkerDockAnimSM master) : 
    GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM>
  {
    public RemoteWorkerDockAnimSM.States.FullOrEmptyState on;
    public RemoteWorkerDockAnimSM.States.FullOrEmptyState off;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.off;
      this.off.EnterTransition(this.off.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)).EnterTransition(this.off.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored))).Transition((GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.State) this.on, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.IsOnline));
      this.off.full.QueueAnim("off_full").Transition(this.off.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)));
      this.off.empty.QueueAnim("off_empty").Transition(this.off.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored));
      this.on.EnterTransition(this.on.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)).EnterTransition(this.on.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored))).Transition((GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.State) this.off, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.IsOnline)));
      this.on.full.QueueAnim("on_full").Transition(this.off.empty, GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Not(new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored)));
      this.on.empty.QueueAnim("on_empty").Transition(this.on.full, new StateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.Transition.ConditionCallback(RemoteWorkerDockAnimSM.States.HasWorkerStored));
    }

    public static bool IsOnline(RemoteWorkerDockAnimSM.StatesInstance smi)
    {
      return smi.master.operational.IsOperational && (Object) smi.master.dock.RemoteWorker != (Object) null;
    }

    public static bool HasWorkerStored(RemoteWorkerDockAnimSM.StatesInstance smi)
    {
      return (Object) smi.master.dock.RemoteWorker != (Object) null && smi.master.dock.RemoteWorker.Docked;
    }

    public class FullOrEmptyState : 
      GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.State
    {
      public GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.State full;
      public GameStateMachine<RemoteWorkerDockAnimSM.States, RemoteWorkerDockAnimSM.StatesInstance, RemoteWorkerDockAnimSM, object>.State empty;
    }
  }
}
