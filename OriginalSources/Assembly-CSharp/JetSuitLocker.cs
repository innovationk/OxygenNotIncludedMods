// Decompiled with JetBrains decompiler
// Type: JetSuitLocker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class JetSuitLocker : StateMachineComponent<JetSuitLocker.StatesInstance>, ISecondaryInput
{
  [MyCmpReq]
  private Building building;
  [MyCmpReq]
  private Storage storage;
  [MyCmpReq]
  private SuitLocker suit_locker;
  [MyCmpReq]
  private KBatchedAnimController anim_controller;
  public const float FUEL_CAPACITY = 100f;
  [SerializeField]
  public ConduitPortInfo portInfo;
  private int secondaryInputCell = -1;
  private FlowUtilityNetwork.NetworkItem flowNetworkItem;
  private ConduitConsumer fuel_consumer;
  private Tag fuel_tag;
  private MeterController o2_meter;
  private MeterController fuel_meter;

  public float FuelAvailable
  {
    get
    {
      return Math.Min((float) ((0.0 + (double) this.storage.GetMassAvailable(this.fuel_tag)) / 100.0), 1f);
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.fuel_tag = GameTags.CombustibleLiquid;
    this.fuel_consumer = this.gameObject.AddComponent<ConduitConsumer>();
    this.fuel_consumer.conduitType = this.portInfo.conduitType;
    this.fuel_consumer.consumptionRate = 10f;
    this.fuel_consumer.capacityTag = GameTags.CombustibleLiquid;
    this.fuel_consumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    this.fuel_consumer.forceAlwaysSatisfied = true;
    this.fuel_consumer.capacityKG = 100f;
    this.fuel_consumer.useSecondaryInput = true;
    RequireInputs requireInputs = this.gameObject.AddComponent<RequireInputs>();
    requireInputs.conduitConsumer = this.fuel_consumer;
    requireInputs.SetRequirements(false, true);
    this.secondaryInputCell = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), this.building.GetRotatedOffset(this.portInfo.offset));
    IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
    this.flowNetworkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, this.secondaryInputCell, this.gameObject);
    int secondaryInputCell = this.secondaryInputCell;
    FlowUtilityNetwork.NetworkItem flowNetworkItem = this.flowNetworkItem;
    networkManager.AddToNetworks(secondaryInputCell, (object) flowNetworkItem, true);
    this.fuel_meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target_1", "meter_petrol", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[1]
    {
      "meter_target_1"
    });
    this.o2_meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target_2", "meter_oxygen", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[1]
    {
      "meter_target_2"
    });
    this.smi.StartSM();
  }

  protected override void OnCleanUp()
  {
    Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryInputCell, (object) this.flowNetworkItem, true);
    base.OnCleanUp();
  }

  public bool IsSuitFullyCharged() => this.suit_locker.IsSuitFullyCharged();

  public KPrefabID GetStoredOutfit() => this.suit_locker.GetStoredOutfit();

  private void FuelSuit(float dt)
  {
    KPrefabID storedOutfit = this.suit_locker.GetStoredOutfit();
    if ((UnityEngine.Object) storedOutfit == (UnityEngine.Object) null || !this.HasFuel())
      return;
    JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
    float b = Mathf.Min(dt * 10f, 100f - component.amount);
    while ((double) b > 0.0 && this.HasFuel())
    {
      float amount = Mathf.Min(this.storage.GetMassAvailable(this.fuel_tag), b);
      component.amount += amount;
      b -= amount;
      SimHashes mostRelevantItemElement = SimHashes.Petroleum;
      this.storage.ConsumeAndGetDisease(this.fuel_tag, amount, out float _, out SimUtil.DiseaseInfo _, out float _, out mostRelevantItemElement);
      component.lastFuelUsed = mostRelevantItemElement;
    }
  }

  bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
  {
    return this.portInfo.conduitType == type;
  }

  public CellOffset GetSecondaryConduitOffset(ConduitType type)
  {
    return this.portInfo.conduitType == type ? this.portInfo.offset : CellOffset.none;
  }

  public bool HasFuel()
  {
    return this.storage.Has(this.fuel_tag) && (double) this.storage.GetMassAvailable(this.fuel_tag) > 0.0;
  }

  private void RefreshMeter()
  {
    this.o2_meter.SetPositionPercent(this.suit_locker.OxygenAvailable);
    this.fuel_meter.SetPositionPercent(this.FuelAvailable);
    this.anim_controller.SetSymbolVisiblity((KAnimHashedString) "oxygen_yes_bloom", this.IsOxygenTankAboveMinimumLevel());
    this.anim_controller.SetSymbolVisiblity((KAnimHashedString) "petrol_yes_bloom", this.IsFuelTankAboveMinimumLevel());
  }

  public bool IsOxygenTankAboveMinimumLevel()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!((UnityEngine.Object) storedOutfit != (UnityEngine.Object) null))
      return false;
    SuitTank component = storedOutfit.GetComponent<SuitTank>();
    return (UnityEngine.Object) component == (UnityEngine.Object) null || (double) component.PercentFull() >= (double) TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
  }

  public bool IsFuelTankAboveMinimumLevel()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!((UnityEngine.Object) storedOutfit != (UnityEngine.Object) null))
      return false;
    JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
    return (UnityEngine.Object) component == (UnityEngine.Object) null || (double) component.PercentFull() >= (double) TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
  }

  public class States : 
    GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker>
  {
    public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State empty;
    public JetSuitLocker.States.ChargingStates charging;
    public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State charged;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.empty;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.root.Update("RefreshMeter", (Action<JetSuitLocker.StatesInstance, float>) ((smi, dt) => smi.master.RefreshMeter()), UpdateRate.RENDER_200ms);
      this.empty.EventTransition(GameHashes.OnStorageChange, (GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State) this.charging, (StateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() != (UnityEngine.Object) null));
      this.charging.DefaultState(this.charging.notoperational).EventTransition(GameHashes.OnStorageChange, this.empty, (StateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() == (UnityEngine.Object) null)).Transition(this.charged, (StateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.IsSuitFullyCharged()));
      this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational);
      this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.nofuel, (StateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.Transition.ConditionCallback) (smi => !smi.master.HasFuel())).Update("FuelSuit", (Action<JetSuitLocker.StatesInstance, float>) ((smi, dt) => smi.master.FuelSuit(dt)), UpdateRate.SIM_1000ms);
      this.charging.nofuel.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.operational, (StateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.HasFuel())).ToggleStatusItem((string) BUILDING.STATUSITEMS.SUIT_LOCKER.NO_FUEL.NAME, (string) BUILDING.STATUSITEMS.SUIT_LOCKER.NO_FUEL.TOOLTIP, "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor);
      this.charged.Transition((GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State) this.charging, (StateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.Transition.ConditionCallback) (smi => !smi.master.IsSuitFullyCharged())).EventTransition(GameHashes.OnStorageChange, this.empty, (StateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() == (UnityEngine.Object) null));
    }

    public class ChargingStates : 
      GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State
    {
      public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State notoperational;
      public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State operational;
      public GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.State nofuel;
    }
  }

  public class StatesInstance(JetSuitLocker jet_suit_locker) : 
    GameStateMachine<JetSuitLocker.States, JetSuitLocker.StatesInstance, JetSuitLocker, object>.GameInstance(jet_suit_locker)
  {
  }
}
