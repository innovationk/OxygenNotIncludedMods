// Decompiled with JetBrains decompiler
// Type: RemoteWorkerOilMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class RemoteWorkerOilMonitor : StateMachineComponent<RemoteWorkerOilMonitor.StatesInstance>
{
  [MyCmpGet]
  private Storage storage;
  public const float CAPACITY_KG = 20.0000019f;
  public const float LOW_LEVEL = 4.00000048f;
  public const float FILL_RATE_KG_PER_S = 2.50000024f;
  public const float CONSUMPTION_RATE_KG_PER_S = 0.0333333351f;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public float Oil => this.storage.GetMassAvailable(GameTags.LubricatingOil);

  public float OilLevel() => this.Oil / 20.0000019f;

  public class StatesInstance(RemoteWorkerOilMonitor master) : 
    GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor>
  {
    private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State ok;
    private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State low_oil;
    private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State out_of_oil;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      base.InitializeStates(out default_state);
      default_state = (StateMachine.BaseState) this.ok;
      this.ok.Transition(this.out_of_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOutOfOil)).Transition(this.low_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsLowOnOil));
      this.low_oil.Transition(this.out_of_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOutOfOil)).Transition(this.ok, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOkForOil)).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerLowOil);
      this.out_of_oil.Transition(this.low_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsLowOnOil)).Transition(this.ok, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOkForOil)).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerOutOfOil);
    }

    public static bool IsOkForOil(RemoteWorkerOilMonitor.StatesInstance smi)
    {
      return (double) smi.master.Oil > 4.0000004768371582;
    }

    public static bool IsLowOnOil(RemoteWorkerOilMonitor.StatesInstance smi)
    {
      return (double) smi.master.Oil >= 1.4012984643248171E-45 && (double) smi.master.Oil < 4.0000004768371582;
    }

    public static bool IsOutOfOil(RemoteWorkerOilMonitor.StatesInstance smi)
    {
      return (double) smi.master.Oil < 1.4012984643248171E-45;
    }
  }
}
