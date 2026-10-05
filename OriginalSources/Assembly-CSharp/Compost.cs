// Decompiled with JetBrains decompiler
// Type: Compost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Compost : StateMachineComponent<Compost.StatesInstance>, IGameObjectEffectDescriptor
{
  [MyCmpGet]
  private Operational operational;
  [MyCmpGet]
  private Storage storage;
  [MyCmpAdd]
  private ManuallySetRemoteWorkTargetComponent remoteChore;
  [SerializeField]
  public float flipInterval = 600f;
  [SerializeField]
  public float simulatedInternalTemperature = 323.15f;
  [SerializeField]
  public float simulatedInternalHeatCapacity = 400f;
  [SerializeField]
  public float simulatedThermalConductivity = 1000f;
  private SimulatedTemperatureAdjuster temperatureAdjuster;
  private static readonly EventSystem.IntraObjectHandler<Compost> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Compost>((Action<Compost, object>) ((component, data) => component.OnStorageChanged(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Compost>(-1697596308, Compost.OnStorageChangedDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<ManualDeliveryKG>().ShowStatusItem = false;
    this.temperatureAdjuster = new SimulatedTemperatureAdjuster(this.simulatedInternalTemperature, this.simulatedInternalHeatCapacity, this.simulatedThermalConductivity, this.GetComponent<Storage>());
    this.smi.StartSM();
  }

  protected override void OnCleanUp() => this.temperatureAdjuster.CleanUp();

  private void OnStorageChanged(object data)
  {
    int num = (UnityEngine.Object) data == (UnityEngine.Object) null ? 1 : 0;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature);
  }

  public class StatesInstance(Compost master) : 
    GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.GameInstance(master)
  {
    public bool CanStartConverting()
    {
      return this.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting();
    }

    public bool CanContinueConverting()
    {
      return this.master.GetComponent<ElementConverter>().CanConvertAtAll();
    }

    public bool IsEmpty() => this.master.storage.IsEmpty();

    public void ResetWorkable()
    {
      CompostWorkable component = this.master.GetComponent<CompostWorkable>();
      component.ShowProgressBar(false);
      component.WorkTimeRemaining = component.GetWorkTime();
    }
  }

  public class States : GameStateMachine<Compost.States, Compost.StatesInstance, Compost>
  {
    public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State empty;
    public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State insufficientMass;
    public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State disabled;
    public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State disabledEmpty;
    public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State inert;
    public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State composting;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.empty;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.empty.Enter("empty", (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State.Callback) (smi => smi.ResetWorkable())).EventTransition(GameHashes.OnStorageChange, this.insufficientMass, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => !smi.IsEmpty())).EventTransition(GameHashes.OperationalChanged, this.disabledEmpty, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingWaste).PlayAnim("off");
      this.insufficientMass.Enter("empty", (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State.Callback) (smi => smi.ResetWorkable())).EventTransition(GameHashes.OnStorageChange, this.empty, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => smi.IsEmpty())).EventTransition(GameHashes.OnStorageChange, this.inert, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => smi.CanStartConverting())).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingWaste).PlayAnim("idle_half");
      this.inert.EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).PlayAnim("on").ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingCompostFlip).ToggleChore(new Func<Compost.StatesInstance, Chore>(Compost.States.CreateFlipChore), new Action<Compost.StatesInstance, Chore>(Compost.States.SetRemoteChore), this.composting);
      this.composting.Enter("Composting", (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State.Callback) (smi => smi.master.operational.SetActive(true))).EventTransition(GameHashes.OnStorageChange, this.empty, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => !smi.CanContinueConverting())).EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).ScheduleGoTo((Func<Compost.StatesInstance, float>) (smi => smi.master.flipInterval), (StateMachine.BaseState) this.inert).Exit((StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State.Callback) (smi => smi.master.operational.SetActive(false)));
      this.disabled.Enter("disabledEmpty", (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State.Callback) (smi => smi.ResetWorkable())).PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.inert, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
      this.disabledEmpty.Enter("disabledEmpty", (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State.Callback) (smi => smi.ResetWorkable())).PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.empty, (StateMachine<Compost.States, Compost.StatesInstance, Compost, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
    }

    private static void SetRemoteChore(Compost.StatesInstance smi, Chore chore)
    {
      smi.master.remoteChore.SetChore(chore);
    }

    private static Chore CreateFlipChore(Compost.StatesInstance smi)
    {
      return (Chore) new WorkChore<CompostWorkable>(Db.Get().ChoreTypes.FlipCompost, (IStateMachineTarget) smi.master);
    }
  }
}
