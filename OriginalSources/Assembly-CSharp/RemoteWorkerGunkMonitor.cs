// Decompiled with JetBrains decompiler
// Type: RemoteWorkerGunkMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class RemoteWorkerGunkMonitor : StateMachineComponent<RemoteWorkerGunkMonitor.StatesInstance>
{
  [MyCmpGet]
  private Storage storage;
  public const float CAPACITY_KG = 20.0000019f;
  public const float HIGH_LEVEL = 16.0000019f;
  public const float DRAIN_AMOUNT_KG_PER_S = 3.33333373f;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public float Gunk => this.storage.GetMassAvailable(SimHashes.LiquidGunk);

  public float GunkLevel() => this.Gunk / 20.0000019f;

  public class StatesInstance(RemoteWorkerGunkMonitor master) : 
    GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor>
  {
    private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State ok;
    private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State high_gunk;
    private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State full_gunk;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      base.InitializeStates(out default_state);
      default_state = (StateMachine.BaseState) this.ok;
      this.ok.Transition(this.full_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsFullOfGunk)).Transition(this.high_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkHigh));
      this.high_gunk.Transition(this.full_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsFullOfGunk)).Transition(this.ok, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkLevelOk)).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerHighGunkLevel);
      this.full_gunk.Transition(this.high_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkHigh)).Transition(this.ok, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkLevelOk)).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerFullGunkLevel);
    }

    public static bool IsGunkLevelOk(RemoteWorkerGunkMonitor.StatesInstance smi)
    {
      return (double) smi.master.Gunk < 16.000001907348633;
    }

    public static bool IsGunkHigh(RemoteWorkerGunkMonitor.StatesInstance smi)
    {
      return (double) smi.master.Gunk >= 16.000001907348633 && (double) smi.master.Gunk < 20.000001907348633;
    }

    public static bool IsFullOfGunk(RemoteWorkerGunkMonitor.StatesInstance smi)
    {
      return (double) smi.master.Gunk >= 20.000001907348633;
    }
  }
}
