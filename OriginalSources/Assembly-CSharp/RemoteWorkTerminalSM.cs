// Decompiled with JetBrains decompiler
// Type: RemoteWorkTerminalSM
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class RemoteWorkTerminalSM : StateMachineComponent<RemoteWorkTerminalSM.StatesInstance>
{
  [MyCmpGet]
  private RemoteWorkTerminal terminal;
  [MyCmpGet]
  private Operational operational;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public class StatesInstance(RemoteWorkTerminalSM master) : 
    GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM>
  {
    public GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State online;
    public RemoteWorkTerminalSM.States.OfflineStates offline;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.offline;
      this.offline.Transition(this.online, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.And(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock), new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.IsOperational))).Transition(this.offline.no_dock, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Not(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock)));
      this.offline.no_dock.Transition((GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State) this.offline, new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock)).ToggleStatusItem(Db.Get().BuildingStatusItems.RemoteWorkTerminalNoDock);
      this.online.ToggleRecurringChore(new Func<RemoteWorkTerminalSM.StatesInstance, Chore>(RemoteWorkTerminalSM.States.CreateChore)).Transition((GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State) this.offline, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Not(GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.And(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock), new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.IsOperational))));
    }

    public static bool IsOperational(RemoteWorkTerminalSM.StatesInstance smi)
    {
      return smi.master.operational.IsOperational;
    }

    public static bool HasAssignedDock(RemoteWorkTerminalSM.StatesInstance smi)
    {
      return (UnityEngine.Object) smi.master.terminal.CurrentDock != (UnityEngine.Object) null;
    }

    public static Chore CreateChore(RemoteWorkTerminalSM.StatesInstance smi)
    {
      return (Chore) new RemoteChore(smi.master.terminal);
    }

    public class OfflineStates : 
      GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State
    {
      public GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State no_dock;
    }
  }
}
