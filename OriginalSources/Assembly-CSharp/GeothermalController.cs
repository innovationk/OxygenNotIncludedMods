// Decompiled with JetBrains decompiler
// Type: GeothermalController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GeothermalController : StateMachineComponent<GeothermalController.StatesInstance>
{
  [MyCmpGet]
  private Storage storage;
  [MyCmpGet]
  private Operational operational;
  private MeterController thermometer;
  private MeterController barometer;
  private KBatchedAnimController animController;
  public Notification dismissOnSelect;
  public static Operational.Flag allowInputFlag = new Operational.Flag(nameof (allowInputFlag), Operational.Flag.Type.Requirement);
  private GeothermalController.VentRegistrationListener listener;
  [Serialize]
  private GeothermalController.ProgressState state;
  private float fakeProgress;

  public GeothermalController.ProgressState State
  {
    get => this.state;
    protected set => this.state = value;
  }

  public List<GeothermalVent> FindVents(bool requireEnabled)
  {
    if (!requireEnabled)
      return Components.GeothermalVents.GetItems(this.gameObject.GetMyWorldId());
    List<GeothermalVent> vents = new List<GeothermalVent>();
    foreach (GeothermalVent vent in this.FindVents(false))
    {
      if (vent.IsVentConnected())
        vents.Add(vent);
    }
    return vents;
  }

  public void PushToVents(GeothermalVent.ElementInfo info)
  {
    List<GeothermalVent> vents = this.FindVents(true);
    if (vents.Count == 0)
      return;
    float[] numArray = new float[vents.Count];
    float num = 0.0f;
    for (int index = 0; index < vents.Count; ++index)
    {
      numArray[index] = GeothermalControllerConfig.OUTPUT_VENT_WEIGHT_RANGE.Get();
      num += numArray[index];
    }
    GeothermalVent.ElementInfo info1 = info;
    for (int index = 0; index < vents.Count; ++index)
    {
      info1.mass = numArray[index] * info.mass / num;
      info1.diseaseCount = (int) ((double) numArray[index] * (double) info.diseaseCount / (double) num);
      vents[index].addMaterial(info1);
    }
  }

  public bool IsFull() => (double) this.storage.MassStored() > 11999.900390625;

  public float ComputeContentTemperature()
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    foreach (GameObject gameObject in this.storage.items)
    {
      PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
      float num3 = component.Mass * component.Element.specificHeatCapacity;
      num1 += num3 * component.Temperature;
      num2 += num3;
    }
    float contentTemperature = 0.0f;
    if ((double) num2 != 0.0)
      contentTemperature = num1 / num2;
    return contentTemperature;
  }

  public List<GeothermalVent.ElementInfo> ComputeOutputs()
  {
    float contentTemperature = this.ComputeContentTemperature();
    float outputTemperature = GeothermalControllerConfig.CalculateOutputTemperature(contentTemperature);
    GeothermalController.ImpuritiesHelper impuritiesHelper = new GeothermalController.ImpuritiesHelper();
    foreach (GameObject gameObject in this.storage.items)
    {
      PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
      impuritiesHelper.AddMaterial(component.Element.idx, component.Mass * 0.92f, outputTemperature, component.DiseaseIdx, component.DiseaseCount);
    }
    foreach (GeothermalControllerConfig.Impurity impurity in GeothermalControllerConfig.GetImpurities())
    {
      if (impurity.required_temp_range.Contains(contentTemperature))
        impuritiesHelper.AddMaterial(impurity.elementIdx, impurity.mass_kg, outputTemperature, byte.MaxValue, 0);
    }
    return impuritiesHelper.results;
  }

  public void PushToVents()
  {
    SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerHasVented = true;
    List<GeothermalVent.ElementInfo> outputs = this.ComputeOutputs();
    if (!SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent && (double) outputs[0].temperature >= 602.0)
      GeothermalPlantComponent.OnVentingHotMaterial(this.GetMyWorldId());
    foreach (GeothermalVent.ElementInfo info in outputs)
      this.PushToVents(info);
    this.storage.ConsumeAllIgnoringDisease();
    this.fakeProgress = 1f;
  }

  private void TryAddConduitConsumers()
  {
    if (this.GetComponents<EntityConduitConsumer>().Length != 0)
      return;
    CellOffset[] cellOffsetArray = new CellOffset[3]
    {
      new CellOffset(0, 0),
      new CellOffset(2, 0),
      new CellOffset(-2, 0)
    };
    foreach (CellOffset cellOffset in cellOffsetArray)
    {
      EntityConduitConsumer entityConduitConsumer = this.gameObject.AddComponent<EntityConduitConsumer>();
      entityConduitConsumer.offset = cellOffset;
      entityConduitConsumer.conduitType = ConduitType.Liquid;
    }
  }

  public float GetPressure()
  {
    switch (this.state)
    {
      case GeothermalController.ProgressState.NOT_STARTED:
      case GeothermalController.ProgressState.FETCHING_STEEL:
      case GeothermalController.ProgressState.RECONNECTING_PIPES:
        return 0.0f;
      default:
        return this.storage.MassStored() / 12000f;
    }
  }

  private void FakeMeterDraining(float time)
  {
    this.fakeProgress -= time / 16f;
    if ((double) this.fakeProgress < 0.0)
      this.fakeProgress = 0.0f;
    this.barometer.SetPositionPercent(this.fakeProgress);
  }

  private void UpdatePressure()
  {
    switch (this.state)
    {
      case GeothermalController.ProgressState.NOT_STARTED:
      case GeothermalController.ProgressState.FETCHING_STEEL:
      case GeothermalController.ProgressState.RECONNECTING_PIPES:
        break;
      default:
        float pressure = this.GetPressure();
        this.barometer.SetPositionPercent(pressure);
        float contentTemperature = this.ComputeContentTemperature();
        if ((double) contentTemperature > 0.0)
          this.thermometer.SetPositionPercent((float) (((double) contentTemperature - 50.0) / 2450.0));
        int index1 = 0;
        for (int index2 = 1; index2 < GeothermalControllerConfig.PRESSURE_ANIM_THRESHOLDS.Length; ++index2)
        {
          if ((double) pressure >= (double) GeothermalControllerConfig.PRESSURE_ANIM_THRESHOLDS[index2])
            index1 = index2;
        }
        if (!((HashedString) this.animController.GetCurrentAnim()?.name != GeothermalControllerConfig.PRESSURE_ANIM_LOOPS[index1]))
          break;
        this.animController.Play(GeothermalControllerConfig.PRESSURE_ANIM_LOOPS[index1], KAnim.PlayMode.Loop);
        break;
    }
  }

  public bool IsObstructed()
  {
    if (!this.IsFull())
      return false;
    bool flag = false;
    foreach (GeothermalVent vent in this.FindVents(false))
    {
      if (vent.IsEntombed())
        return true;
      if (vent.IsVentConnected())
      {
        if (!vent.CanVent())
          return true;
        flag = true;
      }
    }
    return !flag;
  }

  public GeothermalVent FirstObstructedVent()
  {
    foreach (GeothermalVent vent in this.FindVents(false))
    {
      if (vent.IsEntombed() || vent.IsVentConnected() && !vent.CanVent())
        return vent;
    }
    return (GeothermalVent) null;
  }

  public Notification CreateFirstBatchReadyNotification()
  {
    this.dismissOnSelect = new Notification((string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_FIRST_VENT_READY, NotificationType.Event, (Func<List<Notification>, object, string>) ((_, __) => (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_FIRST_VENT_READY_TOOLTIP), expires: false, click_focus: this.transform);
    return this.dismissOnSelect;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.GeothermalControllers.Add(this.GetMyWorldId(), this);
    this.operational.SetFlag(GeothermalController.allowInputFlag, false);
    this.smi.StartSM();
    this.animController = this.GetComponent<KBatchedAnimController>();
    this.barometer = new MeterController((KAnimControllerBase) this.animController, "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalControllerConfig.BAROMETER_SYMBOLS);
    this.thermometer = new MeterController((KAnimControllerBase) this.animController, "meter_target", "meter_temp", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalControllerConfig.THERMOMETER_SYMBOLS);
    this.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelected));
  }

  protected override void OnCleanUp()
  {
    this.Unsubscribe(-1503271301, new Action<object>(this.OnBuildingSelected));
    if (this.listener != null)
      Components.GeothermalVents.Unregister(this.GetMyWorldId(), this.listener.onAdd, this.listener.onRemove);
    Components.GeothermalControllers.Remove(this.GetMyWorldId(), this);
    base.OnCleanUp();
  }

  protected void OnBuildingSelected(object clicked)
  {
    if (!((Boxed<bool>) clicked).value || this.dismissOnSelect == null)
      return;
    if (this.dismissOnSelect.customClickCallback != null)
    {
      this.dismissOnSelect.customClickCallback(this.dismissOnSelect.customClickData);
    }
    else
    {
      this.dismissOnSelect.Clear();
      this.dismissOnSelect = (Notification) null;
    }
  }

  public bool VentingCanFreeKeepsake()
  {
    List<GeothermalVent.ElementInfo> outputs = this.ComputeOutputs();
    return outputs.Count != 0 && (double) outputs[0].temperature >= 602.0;
  }

  public class ReconnectPipes : Workable
  {
    [MyCmpGet]
    private Storage storage;

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.SetWorkTime(5f);
      this.overrideAnims = new KAnimFile[1]
      {
        Assets.GetAnim(GeothermalControllerConfig.RECONNECT_PUMP_ANIM_OVERRIDE)
      };
      this.synchronizeAnims = false;
      this.faceTargetWhenWorking = true;
    }

    protected override void OnCompleteWork(WorkerBase worker)
    {
      base.OnCompleteWork(worker);
      if (!((UnityEngine.Object) this.storage != (UnityEngine.Object) null))
        return;
      this.storage.ConsumeAllIgnoringDisease();
    }
  }

  private class VentRegistrationListener
  {
    public Action<GeothermalVent> onAdd;
    public Action<GeothermalVent> onRemove;
  }

  public enum ProgressState
  {
    NOT_STARTED,
    FETCHING_STEEL,
    RECONNECTING_PIPES,
    NOTIFY_REPAIRED,
    REPAIRED,
    AT_CAPACITY,
    COMPLETE,
  }

  private class ImpuritiesHelper
  {
    public List<GeothermalVent.ElementInfo> results = new List<GeothermalVent.ElementInfo>();

    public void AddMaterial(
      ushort elementIdx,
      float mass,
      float temperature,
      byte diseaseIdx,
      int diseaseCount)
    {
      Element element = ElementLoader.elements[(int) elementIdx];
      if ((double) element.lowTemp > (double) temperature)
      {
        Element lowTempTransition = element.lowTempTransition;
        Element elementByHash = ElementLoader.FindElementByHash(element.lowTempTransitionOreID);
        this.AddMaterial(lowTempTransition.idx, mass * (1f - element.lowTempTransitionOreMassConversion), temperature, diseaseIdx, (int) ((double) diseaseCount * (1.0 - (double) element.lowTempTransitionOreMassConversion)));
        if (elementByHash == null)
          return;
        this.AddMaterial(elementByHash.idx, mass * element.lowTempTransitionOreMassConversion, temperature, diseaseIdx, (int) ((double) diseaseCount * (double) element.lowTempTransitionOreMassConversion));
      }
      else if ((double) element.highTemp < (double) temperature)
      {
        Element highTempTransition = element.highTempTransition;
        Element elementByHash = ElementLoader.FindElementByHash(element.highTempTransitionOreID);
        this.AddMaterial(highTempTransition.idx, mass * (1f - element.highTempTransitionOreMassConversion), temperature, diseaseIdx, (int) ((double) diseaseCount * (1.0 - (double) element.highTempTransitionOreMassConversion)));
        if (elementByHash == null)
          return;
        this.AddMaterial(elementByHash.idx, mass * element.highTempTransitionOreMassConversion, temperature, diseaseIdx, (int) ((double) diseaseCount * (double) element.highTempTransitionOreMassConversion));
      }
      else
      {
        GeothermalVent.ElementInfo elementInfo = new GeothermalVent.ElementInfo();
        for (int index = 0; index < this.results.Count; ++index)
        {
          if ((int) this.results[index].elementIdx == (int) elementIdx)
          {
            GeothermalVent.ElementInfo result = this.results[index];
            result.mass += mass;
            this.results[index] = result;
            return;
          }
        }
        elementInfo.elementHash = element.id;
        elementInfo.elementIdx = elementIdx;
        elementInfo.mass = mass;
        elementInfo.temperature = temperature;
        elementInfo.diseaseCount = diseaseCount;
        elementInfo.diseaseIdx = diseaseIdx;
        elementInfo.isSolid = ElementLoader.elements[(int) elementIdx].IsSolid;
        this.results.Add(elementInfo);
      }
    }
  }

  public class States : 
    GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController>
  {
    public GeothermalController.States.OfflineStates offline;
    public GeothermalController.States.OnlineStates online;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.root.EnterTransition((GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State) this.online, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.State == GeothermalController.ProgressState.COMPLETE)).EnterTransition((GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State) this.offline, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.State != GeothermalController.ProgressState.COMPLETE));
      this.offline.EnterTransition(this.offline.initial, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.State == GeothermalController.ProgressState.NOT_STARTED)).EnterTransition(this.offline.fetchSteel, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.State == GeothermalController.ProgressState.FETCHING_STEEL)).EnterTransition(this.offline.reconnectPipes, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.State == GeothermalController.ProgressState.RECONNECTING_PIPES)).EnterTransition(this.offline.notifyRepaired, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.State == GeothermalController.ProgressState.NOTIFY_REPAIRED)).EnterTransition(this.offline.filling, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.State == GeothermalController.ProgressState.REPAIRED)).EnterTransition((GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State) this.offline.filled, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.State == GeothermalController.ProgressState.AT_CAPACITY)).PlayAnim("off");
      this.offline.initial.Enter((StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State.Callback) (smi => smi.master.storage.DropAll())).Transition(this.offline.fetchSteel, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.State == GeothermalController.ProgressState.FETCHING_STEEL)).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline);
      this.offline.fetchSteel.ToggleChore((Func<GeothermalController.StatesInstance, Chore>) (smi => this.CreateRepairFetchChore(smi, GeothermalControllerConfig.STEEL_FETCH_TAGS, 1200f - smi.master.storage.MassStored())), this.offline.checkSupplies).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline).ToggleStatusItem((StatusItem) Db.Get().BuildingStatusItems.WaitingForMaterials, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.GetFetchListForStatusItem()));
      this.offline.checkSupplies.EnterTransition(this.offline.fetchSteel, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => (double) smi.master.storage.MassStored() < 1200.0)).EnterTransition(this.offline.reconnectPipes, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => (double) smi.master.storage.MassStored() >= 1200.0)).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline);
      this.offline.reconnectPipes.Enter((StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State.Callback) (smi => smi.master.state = GeothermalController.ProgressState.RECONNECTING_PIPES)).ToggleChore((Func<GeothermalController.StatesInstance, Chore>) (smi => this.CreateRepairChore(smi)), this.offline.notifyRepaired, this.offline.reconnectPipes).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoQuestPendingReconnectPipes);
      this.offline.notifyRepaired.Enter((StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State.Callback) (smi => smi.master.state = GeothermalController.ProgressState.NOTIFY_REPAIRED)).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline).ToggleNotification((Func<GeothermalController.StatesInstance, Notification>) (smi => this.CreateRepairedNotification(smi))).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
      this.offline.repaired.Exit((StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State.Callback) (smi => smi.master.State = GeothermalController.ProgressState.REPAIRED)).PlayAnim("on_pre").OnAnimQueueComplete(this.offline.filling).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master)).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master));
      this.offline.filling.PlayAnim("on").Enter((StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State.Callback) (smi => smi.master.TryAddConduitConsumers())).ToggleOperationalFlag(GeothermalController.allowInputFlag).Transition((GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State) this.offline.filled, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.IsFull())).Update((Action<GeothermalController.StatesInstance, float>) ((smi, _) => smi.master.UpdatePressure()), UpdateRate.SIM_1000ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master)).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master));
      this.offline.filled.Enter((StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State.Callback) (smi =>
      {
        smi.master.state = GeothermalController.ProgressState.AT_CAPACITY;
        smi.master.TryAddConduitConsumers();
      })).ToggleNotification((Func<GeothermalController.StatesInstance, Notification>) (smi => smi.master.CreateFirstBatchReadyNotification())).EnterTransition(this.offline.filled.ready, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => !smi.master.IsObstructed())).EnterTransition(this.offline.filled.obstructed, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.IsObstructed())).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
      this.offline.filled.ready.PlayAnim("on").Transition(this.offline.filled.obstructed, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.IsObstructed())).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master)).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master));
      this.offline.filled.obstructed.Transition(this.offline.filled.ready, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => !smi.master.IsObstructed())).PlayAnim("on").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master)).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master)).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerCantVent, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master));
      this.online.Enter((StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State.Callback) (smi => smi.master.TryAddConduitConsumers())).defaultState = (StateMachine.BaseState) this.online.active;
      this.online.active.PlayAnim("on").Transition((GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State) this.online.venting, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.IsFull() && !smi.master.IsObstructed()), UpdateRate.SIM_1000ms).Transition(this.online.obstructed, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.IsObstructed()), UpdateRate.SIM_1000ms).Update((Action<GeothermalController.StatesInstance, float>) ((smi, _) => smi.master.UpdatePressure()), UpdateRate.SIM_1000ms).ToggleOperationalFlag(GeothermalController.allowInputFlag).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master)).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master));
      this.online.venting.Transition(this.online.obstructed, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => smi.master.IsObstructed())).Enter((StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State.Callback) (smi => smi.master.PushToVents())).PlayAnim("venting_loop", KAnim.PlayMode.Loop).Update((Action<GeothermalController.StatesInstance, float>) ((smi, f) => smi.master.FakeMeterDraining(f)), UpdateRate.SIM_1000ms).ScheduleGoTo(16f, (StateMachine.BaseState) this.online.active).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master)).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master));
      this.online.obstructed.Transition(this.online.active, (StateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.Transition.ConditionCallback) (smi => !smi.master.IsObstructed()), UpdateRate.SIM_1000ms).PlayAnim("on").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master)).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master)).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerCantVent, (Func<GeothermalController.StatesInstance, object>) (smi => (object) smi.master)).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
    }

    protected Chore CreateRepairFetchChore(
      GeothermalController.StatesInstance smi,
      HashSet<Tag> tags,
      float mass_required)
    {
      return (Chore) new FetchChore(Db.Get().ChoreTypes.RepairFetch, smi.master.storage, mass_required, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, operational_requirement: Operational.State.None);
    }

    protected Chore CreateRepairChore(GeothermalController.StatesInstance smi)
    {
      return (Chore) new WorkChore<GeothermalController.ReconnectPipes>(Db.Get().ChoreTypes.Repair, (IStateMachineTarget) smi.master, only_when_operational: false, priority_class: PriorityScreen.PriorityClass.high);
    }

    protected Notification CreateRepairedNotification(GeothermalController.StatesInstance smi)
    {
      smi.master.dismissOnSelect = new Notification((string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_RECONNECTED, NotificationType.Event, (Func<List<Notification>, object, string>) ((_, __) => (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_RECONNECTED_TOOLTIP), expires: false, custom_click_callback: (Notification.ClickCallback) (_ =>
      {
        smi.master.dismissOnSelect = (Notification) null;
        this.SetProgressionToRepaired(smi);
      }), clear_on_click: true);
      return smi.master.dismissOnSelect;
    }

    protected void SetProgressionToRepaired(GeothermalController.StatesInstance smi)
    {
      SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired = true;
      GeothermalPlantComponent.DisplayPopup((string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_PLANT_REPAIRED_TITLE, (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_PLANT_REPAIRED_DESC, (HashedString) "geothermalplantonline_kanim", (System.Action) (() =>
      {
        smi.GoTo((StateMachine.BaseState) this.offline.repaired);
        SelectTool.Instance.Select(smi.master.GetComponent<KSelectable>(), true);
      }), smi.master.transform);
    }

    public class OfflineStates : 
      GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
    {
      public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State initial;
      public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State fetchSteel;
      public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State checkSupplies;
      public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State reconnectPipes;
      public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State notifyRepaired;
      public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State repaired;
      public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State filling;
      public GeothermalController.States.OfflineStates.FilledStates filled;

      public class FilledStates : 
        GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
      {
        public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State ready;
        public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State obstructed;
      }
    }

    public class OnlineStates : 
      GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
    {
      public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State active;
      public GeothermalController.States.OnlineStates.WorkingStates venting;
      public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State obstructed;

      public class WorkingStates : 
        GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
      {
        public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State pre;
        public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State loop;
        public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State post;
      }
    }
  }

  public class StatesInstance(GeothermalController smi) : 
    GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.GameInstance(smi),
    ISidescreenButtonControl
  {
    public IFetchList GetFetchListForStatusItem()
    {
      GeothermalController.StatesInstance.FakeList listForStatusItem = new GeothermalController.StatesInstance.FakeList();
      float num = 1200f - this.smi.master.storage.MassStored();
      listForStatusItem.remaining[GameTagExtensions.Create(SimHashes.Steel)] = num;
      return (IFetchList) listForStatusItem;
    }

    bool ISidescreenButtonControl.SidescreenButtonInteractable()
    {
      switch (this.smi.master.State)
      {
        case GeothermalController.ProgressState.NOT_STARTED:
        case GeothermalController.ProgressState.FETCHING_STEEL:
        case GeothermalController.ProgressState.RECONNECTING_PIPES:
          return true;
        case GeothermalController.ProgressState.NOTIFY_REPAIRED:
        case GeothermalController.ProgressState.REPAIRED:
          return false;
        case GeothermalController.ProgressState.AT_CAPACITY:
          return !this.smi.master.IsObstructed();
        case GeothermalController.ProgressState.COMPLETE:
          return false;
        default:
          return false;
      }
    }

    bool ISidescreenButtonControl.SidescreenEnabled()
    {
      return this.smi.master.State != GeothermalController.ProgressState.COMPLETE;
    }

    private string getSidescreenButtonText()
    {
      switch (this.smi.master.State)
      {
        case GeothermalController.ProgressState.NOT_STARTED:
          return (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.REPAIR_CONTROLLER_TITLE;
        case GeothermalController.ProgressState.FETCHING_STEEL:
        case GeothermalController.ProgressState.RECONNECTING_PIPES:
          return (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.CANCEL_REPAIR_CONTROLLER_TITLE;
        case GeothermalController.ProgressState.NOTIFY_REPAIRED:
        case GeothermalController.ProgressState.REPAIRED:
        case GeothermalController.ProgressState.AT_CAPACITY:
        case GeothermalController.ProgressState.COMPLETE:
          return (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_TITLE;
        default:
          return "";
      }
    }

    string ISidescreenButtonControl.SidescreenButtonText => this.getSidescreenButtonText();

    private string getSidescreenButtonTooltip()
    {
      switch (this.smi.master.State)
      {
        case GeothermalController.ProgressState.NOT_STARTED:
          return (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.REPAIR_CONTROLLER_TOOLTIP;
        case GeothermalController.ProgressState.FETCHING_STEEL:
        case GeothermalController.ProgressState.RECONNECTING_PIPES:
          return (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.CANCEL_REPAIR_CONTROLLER_TOOLTIP;
        case GeothermalController.ProgressState.NOTIFY_REPAIRED:
        case GeothermalController.ProgressState.REPAIRED:
          return (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_FILLING_TOOLTIP;
        case GeothermalController.ProgressState.AT_CAPACITY:
        case GeothermalController.ProgressState.COMPLETE:
          return this.smi.master.IsObstructed() ? (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_UNAVAILABLE_TOOLTIP : (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_READY_TOOLTIP;
        default:
          return "";
      }
    }

    string ISidescreenButtonControl.SidescreenButtonTooltip => this.getSidescreenButtonTooltip();

    void ISidescreenButtonControl.OnSidescreenButtonPressed()
    {
      switch (this.smi.master.state)
      {
        case GeothermalController.ProgressState.NOT_STARTED:
          this.smi.master.State = GeothermalController.ProgressState.FETCHING_STEEL;
          break;
        case GeothermalController.ProgressState.FETCHING_STEEL:
        case GeothermalController.ProgressState.RECONNECTING_PIPES:
          this.smi.master.State = GeothermalController.ProgressState.NOT_STARTED;
          this.smi.GoTo((StateMachine.BaseState) this.sm.offline.initial);
          break;
        case GeothermalController.ProgressState.AT_CAPACITY:
          MusicManager.instance.PlaySong("Music_Imperative_complete_DLC2");
          int num = this.smi.master.VentingCanFreeKeepsake() ? 1 : 0;
          this.smi.master.state = GeothermalController.ProgressState.COMPLETE;
          this.smi.GoTo((StateMachine.BaseState) this.sm.online.venting);
          if (num != 0)
            break;
          GeothermalFirstEmissionSequence.Start(this.smi.master);
          break;
      }
    }

    void ISidescreenButtonControl.SetButtonTextOverride(ButtonMenuTextOverride textOverride)
    {
      throw new NotImplementedException();
    }

    int ISidescreenButtonControl.HorizontalGroupID() => -1;

    int ISidescreenButtonControl.ButtonSideScreenSortOrder() => 20;

    protected class FakeList : IFetchList
    {
      public Dictionary<Tag, float> remaining = new Dictionary<Tag, float>();

      Storage IFetchList.Destination => throw new NotImplementedException();

      float IFetchList.GetMinimumAmount(Tag tag) => throw new NotImplementedException();

      Dictionary<Tag, float> IFetchList.GetRemaining() => this.remaining;

      Dictionary<Tag, float> IFetchList.GetRemainingMinimum()
      {
        throw new NotImplementedException();
      }
    }
  }
}
