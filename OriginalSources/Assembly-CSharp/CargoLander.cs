// Decompiled with JetBrains decompiler
// Type: CargoLander
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class CargoLander : 
  GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>
{
  public StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.BoolParameter hasCargo;
  public StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.Signal emptyCargo;
  public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State init;
  public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State stored;
  public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State landing;
  public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State land;
  public CargoLander.CrashedStates grounded;
  public StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.BoolParameter isLanded = new StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.BoolParameter(false);

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.init;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.root.InitializeOperationalFlag(RocketModule.landedFlag).Enter((StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State.Callback) (smi => smi.CheckIfLoaded())).EventHandler(GameHashes.OnStorageChange, (StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State.Callback) (smi => smi.CheckIfLoaded()));
    this.init.ParamTransition<bool>((StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.Parameter<bool>) this.isLanded, (GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State) this.grounded, GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.IsTrue).GoTo(this.stored);
    this.stored.TagTransition(GameTags.Stored, this.landing, true).EventHandler(GameHashes.JettisonedLander, (StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State.Callback) (smi => smi.OnJettisoned()));
    this.landing.PlayAnim("landing", KAnim.PlayMode.Loop).Enter((StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State.Callback) (smi => smi.ShowLandingPreview(true))).Exit((StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State.Callback) (smi => smi.ShowLandingPreview(false))).Enter((StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State.Callback) (smi => smi.ResetAnimPosition())).Update((System.Action<CargoLander.StatesInstance, float>) ((smi, dt) => smi.LandingUpdate(dt)), UpdateRate.SIM_EVERY_TICK).Transition(this.land, (StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.Transition.ConditionCallback) (smi => (double) smi.flightAnimOffset <= 0.0));
    this.land.PlayAnim("grounded_pre").OnAnimQueueComplete((GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State) this.grounded);
    this.grounded.DefaultState(this.grounded.loaded).ToggleOperationalFlag(RocketModule.landedFlag).Enter((StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State.Callback) (smi => smi.CheckIfLoaded())).Enter((StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State.Callback) (smi => smi.sm.isLanded.Set(true, smi)));
    this.grounded.loaded.PlayAnim("grounded").ParamTransition<bool>((StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.Parameter<bool>) this.hasCargo, this.grounded.empty, GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.IsFalse).OnSignal(this.emptyCargo, this.grounded.emptying).Enter((StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State.Callback) (smi => smi.DoLand()));
    this.grounded.emptying.PlayAnim("deploying").TriggerOnEnter(GameHashes.JettisonCargo).OnAnimQueueComplete(this.grounded.empty);
    this.grounded.empty.PlayAnim("deployed").ParamTransition<bool>((StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.Parameter<bool>) this.hasCargo, this.grounded.loaded, GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.IsTrue);
  }

  public class Def : StateMachine.BaseDef
  {
    public Tag previewTag;
    public bool deployOnLanding = true;
  }

  public class CrashedStates : 
    GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State
  {
    public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State loaded;
    public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State emptying;
    public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State empty;
  }

  public class StatesInstance(IStateMachineTarget master, CargoLander.Def def) : 
    GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.GameInstance(master, def)
  {
    [Serialize]
    public float flightAnimOffset = 50f;
    public float exhaustEmitRate = 2f;
    public float exhaustTemperature = 1000f;
    public SimHashes exhaustElement = SimHashes.CarbonDioxide;
    public float topSpeed = 5f;
    private GameObject landingPreview;

    public void ResetAnimPosition()
    {
      this.GetComponent<KBatchedAnimController>().Offset = Vector3.up * this.flightAnimOffset;
    }

    public void OnJettisoned() => this.flightAnimOffset = 50f;

    public void ShowLandingPreview(bool show)
    {
      if (show)
      {
        this.landingPreview = Util.KInstantiate(Assets.GetPrefab(this.def.previewTag), this.transform.GetPosition(), Quaternion.identity, this.gameObject);
        this.landingPreview.SetActive(true);
      }
      else
      {
        this.landingPreview.DeleteObject();
        this.landingPreview = (GameObject) null;
      }
    }

    public void LandingUpdate(float dt)
    {
      this.flightAnimOffset = Mathf.Max(this.flightAnimOffset - dt * this.topSpeed, 0.0f);
      this.ResetAnimPosition();
      int cell = Grid.PosToCell(this.gameObject.transform.GetPosition() + new Vector3(0.0f, this.flightAnimOffset, 0.0f));
      if (!Grid.IsValidCell(cell) || (int) Grid.WorldIdx[cell] != this.gameObject.GetMyWorldId())
        return;
      SimMessages.EmitMass(cell, ElementLoader.GetElementIndex(this.exhaustElement), dt * this.exhaustEmitRate, this.exhaustTemperature, (byte) 0, 0);
    }

    public void DoLand()
    {
      this.smi.master.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
      OccupyArea component = this.smi.GetComponent<OccupyArea>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.ApplyToCells = true;
      if (this.def.deployOnLanding && this.CheckIfLoaded())
        this.sm.emptyCargo.Trigger(this);
      this.smi.master.gameObject.Trigger(1591811118, (object) this);
    }

    public bool CheckIfLoaded()
    {
      bool flag = false;
      MinionStorage component1 = this.GetComponent<MinionStorage>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        flag |= component1.GetStoredMinionInfo().Count > 0;
      Storage component2 = this.GetComponent<Storage>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && !component2.IsEmpty())
        flag = true;
      if (flag != this.sm.hasCargo.Get(this))
        this.sm.hasCargo.Set(flag, this);
      return flag;
    }
  }
}
