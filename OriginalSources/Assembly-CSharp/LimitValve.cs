// Decompiled with JetBrains decompiler
// Type: LimitValve
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class LimitValve : KMonoBehaviour, ISaveLoadable
{
  public static readonly HashedString RESET_PORT_ID = new HashedString("LimitValveReset");
  public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LimitValveOutput");
  public static readonly Operational.Flag limitNotReached = new Operational.Flag(nameof (limitNotReached), Operational.Flag.Type.Requirement);
  public ConduitType conduitType;
  public float maxLimitKg = 100f;
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private LogicPorts ports;
  [MyCmpGet]
  private KBatchedAnimController controller;
  private KBatchedAnimController fg_Controller;
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpGet]
  private ConduitBridge conduitBridge;
  [MyCmpGet]
  private SolidConduitBridge solidConduitBridge;
  [Serialize]
  [SerializeField]
  private float m_limit;
  [Serialize]
  private float m_amount;
  [Serialize]
  private bool m_resetRequested;
  private MeterController limitMeter;
  public bool displayUnitsInsteadOfMass;
  public NonLinearSlider.Range[] sliderRanges;
  private SimHashes lastElemenet = SimHashes.Void;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<LimitValve> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LimitValve>((Action<LimitValve, object>) ((component, data) => component.OnLogicValueChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<LimitValve> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LimitValve>((Action<LimitValve, object>) ((component, data) => component.OnCopySettings(data)));

  public float RemainingCapacity => Mathf.Max(0.0f, this.m_limit - this.m_amount);

  public NonLinearSlider.Range[] GetRanges()
  {
    return this.sliderRanges != null && this.sliderRanges.Length != 0 ? this.sliderRanges : NonLinearSlider.GetDefaultRange(this.maxLimitKg);
  }

  public float Limit
  {
    get => this.m_limit;
    set
    {
      this.m_limit = value;
      this.Refresh();
    }
  }

  public float Amount
  {
    get => this.m_amount;
    set
    {
      this.m_amount = value;
      this.BoxingTrigger<float>(-1722241721, this.m_amount);
      this.Refresh();
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LimitValve>(-905833192, LimitValve.OnCopySettingsDelegate);
    foreach (KBatchedAnimController componentsInChild in this.GetComponentsInChildren<KBatchedAnimController>())
    {
      if (componentsInChild.name.Contains("_fg"))
      {
        this.fg_Controller = componentsInChild;
        break;
      }
    }
  }

  protected override void OnSpawn()
  {
    Game.Instance.logicCircuitManager.onLogicTick += new System.Action(this.LogicTick);
    this.Subscribe<LimitValve>(-801688580, LimitValve.OnLogicValueChangedDelegate);
    if (this.conduitType == ConduitType.Gas || this.conduitType == ConduitType.Liquid)
    {
      ConduitBridge conduitBridge1 = this.conduitBridge;
      conduitBridge1.desiredMassTransfer = conduitBridge1.desiredMassTransfer + new ConduitBridgeBase.DesiredMassTransfer(this.DesiredMassTransfer);
      ConduitBridge conduitBridge2 = this.conduitBridge;
      conduitBridge2.OnMassTransfer = conduitBridge2.OnMassTransfer + new ConduitBridgeBase.ConduitBridgeEvent(this.OnMassTransfer);
    }
    else if (this.conduitType == ConduitType.Solid)
    {
      SolidConduitBridge solidConduitBridge1 = this.solidConduitBridge;
      solidConduitBridge1.desiredMassTransfer = solidConduitBridge1.desiredMassTransfer + new ConduitBridgeBase.DesiredMassTransfer(this.DesiredMassTransfer);
      SolidConduitBridge solidConduitBridge2 = this.solidConduitBridge;
      solidConduitBridge2.OnMassTransfer = solidConduitBridge2.OnMassTransfer + new ConduitBridgeBase.ConduitBridgeEvent(this.OnMassTransfer);
    }
    if (this.limitMeter == null)
      this.limitMeter = new MeterController((KAnimControllerBase) this.controller, "meter_target_counter", "meter_counter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[1]
      {
        "meter_target_counter"
      });
    this.Refresh();
    base.OnSpawn();
  }

  protected override void OnCleanUp()
  {
    Game.Instance.logicCircuitManager.onLogicTick -= new System.Action(this.LogicTick);
    base.OnCleanUp();
  }

  private void LogicTick()
  {
    if (!this.m_resetRequested)
      return;
    this.ResetAmount();
  }

  public void ResetAmount()
  {
    this.m_resetRequested = false;
    this.Amount = 0.0f;
  }

  private float DesiredMassTransfer(
    float dt,
    SimHashes element,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count,
    Pickupable pickupable)
  {
    if (!this.operational.IsOperational)
      return 0.0f;
    if (this.conduitType != ConduitType.Solid || !((UnityEngine.Object) pickupable != (UnityEngine.Object) null) || !GameTags.DisplayAsUnits.Contains(pickupable.KPrefabID.PrefabID()))
      return Mathf.Min(mass, this.RemainingCapacity);
    float units = pickupable.PrimaryElement.Units;
    if ((double) this.RemainingCapacity < (double) units)
      units = (float) Mathf.FloorToInt(this.RemainingCapacity);
    return units * pickupable.PrimaryElement.MassPerUnit;
  }

  private void OnMassTransfer(
    SimHashes element,
    float transferredMass,
    float temperature,
    byte disease_idx,
    int disease_count,
    Pickupable pickupable)
  {
    if (!LogicCircuitNetwork.IsBitActive(0, this.ports.GetInputValue(LimitValve.RESET_PORT_ID)))
    {
      if (this.conduitType == ConduitType.Gas || this.conduitType == ConduitType.Liquid)
        this.Amount += transferredMass;
      else if (this.conduitType == ConduitType.Solid && (UnityEngine.Object) pickupable != (UnityEngine.Object) null)
        this.Amount += transferredMass / pickupable.PrimaryElement.MassPerUnit;
    }
    if (this.lastElemenet != element && this.conduitType == ConduitType.Liquid)
    {
      Element elementByHash = ElementLoader.FindElementByHash(element);
      if (elementByHash != null)
      {
        GameUtil.TintLiquidSymbolOnBuilding("gradient", this.controller, elementByHash);
        GameUtil.TintLiquidSymbolOnBuilding("water_color_fg", this.fg_Controller, elementByHash);
      }
    }
    this.lastElemenet = element;
    this.operational.SetActive(this.operational.IsOperational && (double) transferredMass > 0.0);
    this.Refresh();
  }

  private void Refresh()
  {
    if ((UnityEngine.Object) this.operational == (UnityEngine.Object) null)
      return;
    this.ports.SendSignal(LimitValve.OUTPUT_PORT_ID, (double) this.RemainingCapacity <= 0.0 ? 1 : 0);
    this.operational.SetFlag(LimitValve.limitNotReached, (double) this.RemainingCapacity > 0.0);
    if ((double) this.RemainingCapacity > 0.0)
    {
      this.limitMeter.meterController.Play((HashedString) "meter_counter", KAnim.PlayMode.Paused);
      this.limitMeter.SetPositionPercent(this.Amount / this.Limit);
      this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.LimitValveLimitNotReached, (object) this);
    }
    else
    {
      this.limitMeter.meterController.Play((HashedString) "meter_on", KAnim.PlayMode.Paused);
      this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.LimitValveLimitReached, (object) this);
    }
  }

  public void OnLogicValueChanged(object data)
  {
    LogicValueChanged logicValueChanged = (LogicValueChanged) data;
    if (!(logicValueChanged.portID == LimitValve.RESET_PORT_ID) || !LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue))
      return;
    this.ResetAmount();
  }

  private void OnCopySettings(object data)
  {
    LimitValve component = ((GameObject) data).GetComponent<LimitValve>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Limit = component.Limit;
  }
}
