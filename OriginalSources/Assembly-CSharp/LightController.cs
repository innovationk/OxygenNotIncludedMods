// Decompiled with JetBrains decompiler
// Type: LightController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class LightController : GameStateMachine<LightController, LightController.Instance>
{
  public GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.State off;
  public GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.State on;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (StateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
    this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (StateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight).Enter("SetActive", (StateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(true)));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, LightController.Def def) : 
    GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.GameInstance(master, (object) def)
  {
  }
}
