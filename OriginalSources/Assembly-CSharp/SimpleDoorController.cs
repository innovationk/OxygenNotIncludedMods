// Decompiled with JetBrains decompiler
// Type: SimpleDoorController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class SimpleDoorController : 
  GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>
{
  public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State inactive;
  public SimpleDoorController.ActiveStates active;
  public StateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.IntParameter numOpens;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.inactive;
    this.inactive.TagTransition(GameTags.RocketOnGround, (GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State) this.active);
    this.active.DefaultState(this.active.closed).TagTransition(GameTags.RocketOnGround, this.inactive, true).Enter((StateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State.Callback) (smi => smi.Register())).Exit((StateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State.Callback) (smi => smi.Unregister()));
    this.active.closed.PlayAnim((Func<SimpleDoorController.StatesInstance, string>) (smi => smi.GetDefaultAnim()), KAnim.PlayMode.Loop).ParamTransition<int>((StateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.Parameter<int>) this.numOpens, this.active.opening, (StateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.Parameter<int>.Callback) ((smi, p) => p > 0));
    this.active.opening.PlayAnim("enter_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.active.open);
    this.active.open.PlayAnim("enter_loop", KAnim.PlayMode.Loop).ParamTransition<int>((StateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.Parameter<int>) this.numOpens, this.active.closedelay, (StateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.Parameter<int>.Callback) ((smi, p) => p == 0));
    this.active.closedelay.ParamTransition<int>((StateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.Parameter<int>) this.numOpens, this.active.open, (StateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.Parameter<int>.Callback) ((smi, p) => p > 0)).ScheduleGoTo(0.5f, (StateMachine.BaseState) this.active.closing);
    this.active.closing.PlayAnim("enter_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.active.closed);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class ActiveStates : 
    GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State
  {
    public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closed;
    public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State opening;
    public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State open;
    public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closedelay;
    public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closing;
  }

  public class StatesInstance(IStateMachineTarget master, SimpleDoorController.Def def) : 
    GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.GameInstance(master, def),
    INavDoor
  {
    public string GetDefaultAnim()
    {
      KBatchedAnimController component = this.master.GetComponent<KBatchedAnimController>();
      return (UnityEngine.Object) component != (UnityEngine.Object) null && !component.initialAnim.IsNullOrWhiteSpace() ? component.initialAnim : "idle_loop";
    }

    public void Register()
    {
      int cell = Grid.PosToCell(this.gameObject.transform.GetPosition());
      Grid.HasDoor[cell] = true;
    }

    public void Unregister()
    {
      int cell = Grid.PosToCell(this.gameObject.transform.GetPosition());
      Grid.HasDoor[cell] = false;
    }

    public bool isSpawned => this.master.gameObject.GetComponent<KMonoBehaviour>().isSpawned;

    public void Close() => this.sm.numOpens.Delta(-1, this.smi);

    public bool IsOpen()
    {
      return this.IsInsideState((StateMachine.BaseState) this.sm.active.open) || this.IsInsideState((StateMachine.BaseState) this.sm.active.closedelay);
    }

    public void Open() => this.sm.numOpens.Delta(1, this.smi);
  }
}
