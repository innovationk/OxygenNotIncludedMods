// Decompiled with JetBrains decompiler
// Type: SpaceHeater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class SpaceHeater : 
  StateMachineComponent<SpaceHeater.StatesInstance>,
  IGameObjectEffectDescriptor,
  ISingleSliderControl,
  ISliderControl
{
  public float targetTemperature = 308.15f;
  public float minimumCellMass;
  public int radius = 2;
  [SerializeField]
  public bool heatLiquid;
  public bool hasTargetTemperature = true;
  [Serialize]
  public float UserSliderSetting;
  public bool produceHeat;
  public float maxPower;
  public float minPower;
  public float maxSelfHeatKWs;
  public float maxExhaustedKWs;
  private StatusItem heatStatusItem;
  private StatusItem turboModeStatusItem;
  private HandleVector<int>.Handle structureTemperature;
  private Extents extents;
  private float overheatTemperature;
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private PrimaryElement primaryElement;
  [MyCmpGet]
  private KBatchedAnimHeatPostProcessingEffect heatEffect;
  [MyCmpGet]
  private KBatchedAnimController animController;
  [MyCmpGet]
  private EnergyConsumer energyConsumer;
  [MyCmpGet]
  private LiquidHeaterBubbleEmitter bubbleEmitter;
  private List<int> monitorCells = new List<int>();

  public float TargetTemperature => this.targetTemperature;

  public float MinSelfHeatKWs => this.maxSelfHeatKWs * (this.minPower / this.maxPower);

  public float MinExhaustedKWs => this.maxExhaustedKWs * (this.minPower / this.maxPower);

  public float CurrentSelfHeatKW
  {
    get => Mathf.Lerp(this.MinSelfHeatKWs, this.maxSelfHeatKWs, this.UserSliderSetting);
  }

  public float CurrentExhaustedKW
  {
    get => Mathf.Lerp(this.MinExhaustedKWs, this.maxExhaustedKWs, this.UserSliderSetting);
  }

  public float CurrentPowerConsumption
  {
    get => Mathf.Lerp(this.minPower, this.maxPower, this.UserSliderSetting);
  }

  public static void GenerateHeat(SpaceHeater.StatesInstance smi, float dt)
  {
    if (!smi.master.produceHeat)
      return;
    double num1 = (double) SpaceHeater.AddExhaustHeat(smi, dt);
    double num2 = (double) SpaceHeater.AddSelfHeat(smi, dt);
  }

  private static float AddExhaustHeat(SpaceHeater.StatesInstance smi, float dt)
  {
    float currentExhaustedKw = smi.master.CurrentExhaustedKW;
    float num1 = (UnityEngine.Object) smi.master.bubbleEmitter != (UnityEngine.Object) null ? smi.master.bubbleEmitter.DivertEnergy(currentExhaustedKw, dt) : 0.0f;
    float kw = Mathf.Max(currentExhaustedKw - num1, 0.0f);
    float num2 = smi.master.heatLiquid ? 358.15f : smi.master.overheatTemperature;
    float maxTemperature = smi.master.IsTurboModeActive ? 10000f : num2;
    StructureTemperatureComponents.ExhaustHeat(smi.master.extents, kw, maxTemperature, dt);
    return kw;
  }

  public static void RefreshHeatEffect(SpaceHeater.StatesInstance smi)
  {
    if (!((UnityEngine.Object) smi.master.heatEffect != (UnityEngine.Object) null) || !smi.master.produceHeat)
      return;
    float heat = smi.IsInsideState((StateMachine.BaseState) smi.sm.online.heating) ? smi.master.CurrentExhaustedKW + smi.master.CurrentSelfHeatKW : 0.0f;
    smi.master.heatEffect.SetHeatBeingProducedValue(heat);
  }

  private static float AddSelfHeat(SpaceHeater.StatesInstance smi, float dt)
  {
    float currentSelfHeatKw = smi.master.CurrentSelfHeatKW;
    GameComps.StructureTemperatures.ProduceEnergy(smi.master.structureTemperature, currentSelfHeatKw * dt, (string) BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, dt);
    return currentSelfHeatKw;
  }

  public bool IsTurboModeActive => this.heatLiquid && (double) this.UserSliderSetting > 0.0;

  public static void RefreshTepidizerAnim(SpaceHeater.StatesInstance smi)
  {
    if (!smi.master.heatLiquid)
      return;
    if (smi.IsInsideState((StateMachine.BaseState) smi.sm.offline) || smi.IsInsideState((StateMachine.BaseState) smi.sm.online.undermassliquid) || smi.IsInsideState((StateMachine.BaseState) smi.sm.online.undermassgas) || smi.IsInsideState((StateMachine.BaseState) smi.sm.online.overtemp))
      smi.master.animController.Play((HashedString) "off");
    else if (smi.IsInsideState((StateMachine.BaseState) smi.sm.online.heating))
    {
      if (smi.master.IsTurboModeActive)
        smi.master.animController.Play((HashedString) "working_loop_turbo", KAnim.PlayMode.Loop);
      else
        smi.master.animController.Play((HashedString) "working_loop", KAnim.PlayMode.Loop);
    }
    else
      smi.master.animController.Play((HashedString) "on");
  }

  public void SetUserSpecifiedPowerConsumptionValue(float value)
  {
    if (!this.produceHeat)
      return;
    this.UserSliderSetting = (float) (((double) value - (double) this.minPower) / ((double) this.maxPower - (double) this.minPower));
    SpaceHeater.RefreshHeatEffect(this.smi);
    this.energyConsumer.BaseWattageRating = this.CurrentPowerConsumption;
    this.RefreshTurboModeStatusItem();
    if (this.IsTurboModeActive && this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.online.overtemp))
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.online.heating);
    if (!this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.online.heating))
      return;
    SpaceHeater.RefreshTepidizerAnim(this.smi);
  }

  private void RefreshTurboModeStatusItem()
  {
    if (!this.heatLiquid || this.turboModeStatusItem == null)
      return;
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.IsTurboModeActive)
      component.SetStatusItem(Db.Get().StatusItemCategories.OperatingEnergy, this.turboModeStatusItem, (object) this);
    else
      component.SetStatusItem(Db.Get().StatusItemCategories.OperatingEnergy, (StatusItem) null);
  }

  protected override void OnPrefabInit()
  {
    if (this.produceHeat)
    {
      this.heatStatusItem = new StatusItem("OperatingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.heatStatusItem.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance) data;
        float num = statesInstance.master.CurrentSelfHeatKW + statesInstance.master.CurrentExhaustedKW;
        str = string.Format(str, (object) GameUtil.GetFormattedHeatEnergy(num * 1000f));
        return str;
      });
      this.heatStatusItem.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance) data;
        float num = statesInstance.master.CurrentSelfHeatKW + statesInstance.master.CurrentExhaustedKW;
        str = str.Replace("{0}", GameUtil.GetFormattedHeatEnergy(num * 1000f));
        string newValue = string.Format((string) BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, (object) BUILDING.STATUSITEMS.OPERATINGENERGY.OPERATING, (object) GameUtil.GetFormattedHeatEnergy(statesInstance.master.CurrentSelfHeatKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S)) + string.Format((string) BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, (object) BUILDING.STATUSITEMS.OPERATINGENERGY.EXHAUSTING, (object) GameUtil.GetFormattedHeatEnergy(statesInstance.master.CurrentExhaustedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
        str = str.Replace("{1}", newValue);
        return str;
      });
    }
    if (this.heatLiquid)
    {
      this.turboModeStatusItem = new StatusItem("TurboMode", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      this.turboModeStatusItem.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        SpaceHeater spaceHeater = (SpaceHeater) data;
        str = string.Format(str, (object) GameUtil.GetFormattedWattage(spaceHeater.CurrentPowerConsumption), (object) GameUtil.GetFormattedHeatEnergyRate((float) (((double) spaceHeater.CurrentSelfHeatKW + (double) spaceHeater.CurrentExhaustedKW) * 1000.0)));
        return str;
      });
      this.turboModeStatusItem.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
      {
        SpaceHeater spaceHeater = (SpaceHeater) data;
        return string.Format(str, (object) GameUtil.GetFormattedWattage(spaceHeater.CurrentPowerConsumption), (object) GameUtil.GetFormattedHeatEnergyRate((float) (((double) spaceHeater.CurrentSelfHeatKW + (double) spaceHeater.CurrentExhaustedKW) * 1000.0)));
      });
    }
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    GameScheduler.Instance.Schedule("InsulationTutorial", 2f, (Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation)), (object) null, (SchedulerGroup) null);
    this.extents = this.GetComponent<OccupyArea>().GetExtents();
    this.overheatTemperature = this.GetComponent<BuildingComplete>().Def.OverheatTemperature;
    this.structureTemperature = GameComps.StructureTemperatures.GetHandle(this.gameObject);
    this.smi.StartSM();
    this.SetUserSpecifiedPowerConsumptionValue(this.CurrentPowerConsumption);
  }

  public void SetLiquidHeater() => this.heatLiquid = true;

  private SpaceHeater.MonitorState MonitorHeating(float dt)
  {
    this.monitorCells.Clear();
    GameUtil.GetNonSolidCells(Grid.PosToCell(this.transform.GetPosition()), this.radius, this.monitorCells);
    int num1 = 0;
    float num2 = 0.0f;
    foreach (int monitorCell in this.monitorCells)
    {
      if ((double) Grid.Mass[monitorCell] > (double) this.minimumCellMass && (Grid.Element[monitorCell].IsGas && !this.heatLiquid || Grid.Element[monitorCell].IsLiquid && this.heatLiquid))
      {
        ++num1;
        num2 += Grid.Temperature[monitorCell];
      }
    }
    return num1 == 0 ? (!this.heatLiquid ? SpaceHeater.MonitorState.NotEnoughGas : SpaceHeater.MonitorState.NotEnoughLiquid) : (this.hasTargetTemperature && (double) num2 / (double) num1 >= (double) this.targetTemperature || this.heatLiquid && !this.IsTurboModeActive && (double) num2 / (double) num1 >= 358.14999389648438 ? SpaceHeater.MonitorState.TooHot : SpaceHeater.MonitorState.ReadyToHeat);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    if (this.hasTargetTemperature)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.HEATER_TARGETTEMPERATURE, (object) GameUtil.GetFormattedTemperature(this.targetTemperature)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.HEATER_TARGETTEMPERATURE, (object) GameUtil.GetFormattedTemperature(this.targetTemperature)));
      descriptors.Add(descriptor);
    }
    return descriptors;
  }

  public string SliderTitleKey => "STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TITLE";

  public string SliderUnits => (string) UI.UNITSUFFIXES.ELECTRICAL.WATT;

  public int SliderDecimalPlaces(int index) => 0;

  public float GetSliderMin(int index)
  {
    return !this.produceHeat || this.heatLiquid ? 0.0f : this.minPower;
  }

  public float GetSliderMax(int index)
  {
    return !this.produceHeat || this.heatLiquid ? 0.0f : this.maxPower;
  }

  public float GetSliderValue(int index) => this.CurrentPowerConsumption;

  public void SetSliderValue(float value, int index)
  {
    this.SetUserSpecifiedPowerConsumptionValue(value);
  }

  public string GetSliderTooltipKey(int index)
  {
    return "STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TOOLTIP";
  }

  string ISliderControl.GetSliderTooltip(int index)
  {
    return string.Format((string) Strings.Get("STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TOOLTIP"), (object) GameUtil.GetFormattedHeatEnergyRate((float) (((double) this.CurrentSelfHeatKW + (double) this.CurrentExhaustedKW) * 1000.0)));
  }

  public class StatesInstance(SpaceHeater master) : 
    GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.GameInstance(master)
  {
  }

  public class States : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater>
  {
    public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State offline;
    public SpaceHeater.States.OnlineStates online;
    private StatusItem statusItemUnderMassLiquid;
    private StatusItem statusItemUnderMassGas;
    private StatusItem statusItemOverTemp;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.offline;
      this.serializable = StateMachine.SerializeType.Never;
      this.statusItemUnderMassLiquid = new StatusItem("statusItemUnderMassLiquid", (string) BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.NAME, (string) BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.statusItemUnderMassGas = new StatusItem("statusItemUnderMassGas", (string) BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.NAME, (string) BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.statusItemOverTemp = new StatusItem("statusItemOverTemp", (string) BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.NAME, (string) BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
      this.statusItemOverTemp.resolveStringCallback = (Func<string, object, string>) ((str, obj) =>
      {
        SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance) obj;
        float temp = statesInstance.master.heatLiquid ? 358.15f : statesInstance.master.TargetTemperature;
        return string.Format(str, (object) GameUtil.GetFormattedTemperature(temp));
      });
      this.offline.Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect)).Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshTepidizerAnim)).EventTransition(GameHashes.OperationalChanged, (GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State) this.online, (StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.online.EventTransition(GameHashes.OperationalChanged, this.offline, (StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshTepidizerAnim)).Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect)).Update("spaceheater_online", (Action<SpaceHeater.StatesInstance, float>) ((smi, dt) =>
      {
        switch (smi.master.MonitorHeating(dt))
        {
          case SpaceHeater.MonitorState.ReadyToHeat:
            smi.GoTo((StateMachine.BaseState) this.online.heating);
            break;
          case SpaceHeater.MonitorState.TooHot:
            smi.GoTo((StateMachine.BaseState) this.online.overtemp);
            break;
          case SpaceHeater.MonitorState.NotEnoughLiquid:
            smi.GoTo((StateMachine.BaseState) this.online.undermassliquid);
            break;
          case SpaceHeater.MonitorState.NotEnoughGas:
            smi.GoTo((StateMachine.BaseState) this.online.undermassgas);
            break;
        }
      }), UpdateRate.SIM_1000ms);
      this.online.heating.Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect)).Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshTepidizerAnim)).Enter((StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback) (smi => smi.master.operational.SetActive(true))).ToggleStatusItem((Func<SpaceHeater.StatesInstance, StatusItem>) (smi => smi.master.heatStatusItem), (Func<SpaceHeater.StatesInstance, object>) (smi => (object) smi)).Update(new Action<SpaceHeater.StatesInstance, float>(SpaceHeater.GenerateHeat)).Exit((StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback) (smi => smi.master.operational.SetActive(false))).Exit(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect));
      this.online.undermassliquid.Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshTepidizerAnim)).ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemUnderMassLiquid);
      this.online.undermassgas.Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshTepidizerAnim)).ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemUnderMassGas);
      this.online.overtemp.Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshTepidizerAnim)).ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemOverTemp);
    }

    public class OnlineStates : 
      GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State
    {
      public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State heating;
      public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State overtemp;
      public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State undermassliquid;
      public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State undermassgas;
    }
  }

  private enum MonitorState
  {
    ReadyToHeat,
    TooHot,
    NotEnoughLiquid,
    NotEnoughGas,
  }
}
