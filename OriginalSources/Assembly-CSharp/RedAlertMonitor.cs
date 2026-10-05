// Decompiled with JetBrains decompiler
// Type: RedAlertMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class RedAlertMonitor : GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance>
{
  public GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.State off;
  public GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.State on;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
    this.off.EventTransition(GameHashes.EnteredRedAlert, (Func<RedAlertMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.on, (StateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi =>
    {
      WorldContainer myWorld = smi.master.gameObject.GetMyWorld();
      return !((UnityEngine.Object) myWorld == (UnityEngine.Object) null) && myWorld.AlertManager.IsRedAlert();
    }));
    this.on.EventTransition(GameHashes.ExitedRedAlert, (Func<RedAlertMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.off, (StateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi =>
    {
      WorldContainer myWorld = smi.master.gameObject.GetMyWorld();
      return !((UnityEngine.Object) myWorld == (UnityEngine.Object) null) && !myWorld.AlertManager.IsRedAlert();
    })).Enter("EnableRedAlert", (StateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.EnableRedAlert())).ToggleEffect("RedAlert").ToggleExpression(Db.Get().Expressions.RedAlert);
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    public void EnableRedAlert()
    {
      ChoreDriver component = this.GetComponent<ChoreDriver>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      Chore currentChore = component.GetCurrentChore();
      if (currentChore == null)
        return;
      bool flag = false;
      for (int index = 0; index < currentChore.GetPreconditions().Count; ++index)
      {
        if (currentChore.GetPreconditions()[index].condition.id == ChorePreconditions.instance.IsNotRedAlert.id)
          flag = true;
      }
      if (!flag)
        return;
      component.StopChore();
    }
  }
}
