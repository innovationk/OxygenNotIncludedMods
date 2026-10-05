// Decompiled with JetBrains decompiler
// Type: TaskAvailabilityMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class TaskAvailabilityMonitor : 
  GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance>
{
  public GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.State satisfied;
  public GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.State unavailable;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.satisfied.EventTransition(GameHashes.NewDay, (Func<TaskAvailabilityMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) GameClock.Instance), this.unavailable, (StateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => GameClock.Instance.GetCycle() > 0));
    this.unavailable.Enter("RefreshStatusItem", (StateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.RefreshStatusItem())).EventHandler(GameHashes.ScheduleChanged, (StateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.RefreshStatusItem()));
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    public void RefreshStatusItem()
    {
      KSelectable component = this.GetComponent<KSelectable>();
      WorldContainer myWorld = this.gameObject.GetMyWorld();
      if ((UnityEngine.Object) myWorld != (UnityEngine.Object) null && myWorld.IsModuleInterior && myWorld.ParentWorldId == myWorld.id)
        component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.IdleInRockets);
      else
        component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Idle);
    }
  }
}
