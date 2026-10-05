// Decompiled with JetBrains decompiler
// Type: ModularConduitPortController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class ModularConduitPortController : 
  GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>
{
  private GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State off;
  private ModularConduitPortController.OnStates on;
  public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter isUnloading;
  public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter isLoading;
  public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter hasRocket;
  private static StatusItem idleStatusItem;
  private static StatusItem unloadingStatusItem;
  private static StatusItem loadingStatusItem;
  private static StatusItem loadedStatusItem;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    ModularConduitPortController.InitializeStatusItems();
    this.off.PlayAnim("off", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, (GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State) this.on, (StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
    this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational));
    this.on.idle.PlayAnim("idle").ParamTransition<bool>((StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.Parameter<bool>) this.hasRocket, this.on.finished, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ToggleStatusItem(ModularConduitPortController.idleStatusItem);
    this.on.finished.PlayAnim("finished", KAnim.PlayMode.Loop).ParamTransition<bool>((StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.Parameter<bool>) this.hasRocket, this.on.idle, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>((StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.Parameter<bool>) this.isUnloading, this.on.unloading, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ParamTransition<bool>((StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.Parameter<bool>) this.isLoading, this.on.loading, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ToggleStatusItem(ModularConduitPortController.loadedStatusItem);
    this.on.unloading.Enter("SetActive(true)", (StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State.Callback) (smi => smi.operational.SetActive(true))).Exit("SetActive(false)", (StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State.Callback) (smi => smi.operational.SetActive(false))).PlayAnim("unloading_pre").QueueAnim("unloading_loop", true).ParamTransition<bool>((StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.Parameter<bool>) this.isUnloading, this.on.unloading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>((StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.Parameter<bool>) this.hasRocket, this.on.unloading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ToggleStatusItem(ModularConduitPortController.unloadingStatusItem);
    this.on.unloading_pst.PlayAnim("unloading_pst").OnAnimQueueComplete(this.on.finished);
    this.on.loading.Enter("SetActive(true)", (StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State.Callback) (smi => smi.operational.SetActive(true))).Exit("SetActive(false)", (StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State.Callback) (smi => smi.operational.SetActive(false))).PlayAnim("loading_pre").QueueAnim("loading_loop", true).ParamTransition<bool>((StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.Parameter<bool>) this.isLoading, this.on.loading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>((StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.Parameter<bool>) this.hasRocket, this.on.loading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ToggleStatusItem(ModularConduitPortController.loadingStatusItem);
    this.on.loading_pst.PlayAnim("loading_pst").OnAnimQueueComplete(this.on.finished);
  }

  private static void InitializeStatusItems()
  {
    if (ModularConduitPortController.idleStatusItem != null)
      return;
    ModularConduitPortController.idleStatusItem = new StatusItem("ROCKET_PORT_IDLE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    ModularConduitPortController.unloadingStatusItem = new StatusItem("ROCKET_PORT_UNLOADING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    ModularConduitPortController.loadingStatusItem = new StatusItem("ROCKET_PORT_LOADING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    ModularConduitPortController.loadedStatusItem = new StatusItem("ROCKET_PORT_LOADED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
  }

  public class Def : StateMachine.BaseDef
  {
    public ModularConduitPortController.Mode mode;
  }

  public enum Mode
  {
    Unload,
    Both,
    Load,
  }

  private class OnStates : 
    GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State
  {
    public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State idle;
    public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State unloading;
    public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State unloading_pst;
    public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State loading;
    public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State loading_pst;
    public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State finished;
  }

  public new class Instance(IStateMachineTarget master, ModularConduitPortController.Def def) : 
    GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.GameInstance(master, def)
  {
    [MyCmpGet]
    public Operational operational;

    public ModularConduitPortController.Mode SelectedMode => this.def.mode;

    public ConduitType GetConduitType() => this.GetComponent<IConduitConsumer>().ConduitType;

    public void SetUnloading(bool isUnloading) => this.sm.isUnloading.Set(isUnloading, this);

    public void SetLoading(bool isLoading) => this.sm.isLoading.Set(isLoading, this);

    public void SetRocket(bool hasRocket) => this.sm.hasRocket.Set(hasRocket, this);

    public bool IsLoading() => this.sm.isLoading.Get(this);
  }
}
