// Decompiled with JetBrains decompiler
// Type: LeadSuitLocker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TUNING;
using UnityEngine;

#nullable disable
public class LeadSuitLocker : StateMachineComponent<LeadSuitLocker.StatesInstance>
{
  [MyCmpReq]
  private Building building;
  [MyCmpReq]
  private Storage storage;
  [MyCmpReq]
  private SuitLocker suit_locker;
  [MyCmpReq]
  private KBatchedAnimController anim_controller;
  private MeterController o2_meter;
  private MeterController battery_meter;
  private float batteryChargeTime = 60f;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.o2_meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target_top", "meter_oxygen", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[1]
    {
      "meter_target_top"
    });
    this.battery_meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target_side", "meter_petrol", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[1]
    {
      "meter_target_side"
    });
    this.smi.StartSM();
  }

  public bool IsSuitFullyCharged() => this.suit_locker.IsSuitFullyCharged();

  public KPrefabID GetStoredOutfit() => this.suit_locker.GetStoredOutfit();

  private void FillBattery(float dt)
  {
    KPrefabID storedOutfit = this.suit_locker.GetStoredOutfit();
    if ((UnityEngine.Object) storedOutfit == (UnityEngine.Object) null)
      return;
    LeadSuitTank component = storedOutfit.GetComponent<LeadSuitTank>();
    if (component.IsFull())
      return;
    component.batteryCharge += dt / this.batteryChargeTime;
  }

  private void RefreshMeter()
  {
    this.o2_meter.SetPositionPercent(this.suit_locker.OxygenAvailable);
    this.battery_meter.SetPositionPercent(this.suit_locker.BatteryAvailable);
    this.anim_controller.SetSymbolVisiblity((KAnimHashedString) "oxygen_yes_bloom", this.IsOxygenTankAboveMinimumLevel());
    this.anim_controller.SetSymbolVisiblity((KAnimHashedString) "petrol_yes_bloom", this.IsBatteryAboveMinimumLevel());
  }

  public bool IsOxygenTankAboveMinimumLevel()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!((UnityEngine.Object) storedOutfit != (UnityEngine.Object) null))
      return false;
    SuitTank component = storedOutfit.GetComponent<SuitTank>();
    return (UnityEngine.Object) component == (UnityEngine.Object) null || (double) component.PercentFull() >= (double) EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
  }

  public bool IsBatteryAboveMinimumLevel()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!((UnityEngine.Object) storedOutfit != (UnityEngine.Object) null))
      return false;
    LeadSuitTank component = storedOutfit.GetComponent<LeadSuitTank>();
    return (UnityEngine.Object) component == (UnityEngine.Object) null || (double) component.PercentFull() >= (double) EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE;
  }

  public class States : 
    GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker>
  {
    public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State empty;
    public LeadSuitLocker.States.ChargingStates charging;
    public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State charged;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.empty;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.root.Update("RefreshMeter", (Action<LeadSuitLocker.StatesInstance, float>) ((smi, dt) => smi.master.RefreshMeter()), UpdateRate.RENDER_200ms);
      this.empty.EventTransition(GameHashes.OnStorageChange, (GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State) this.charging, (StateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() != (UnityEngine.Object) null));
      this.charging.DefaultState(this.charging.notoperational).EventTransition(GameHashes.OnStorageChange, this.empty, (StateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() == (UnityEngine.Object) null)).Transition(this.charged, (StateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.IsSuitFullyCharged()));
      this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational);
      this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Update("FillBattery", (Action<LeadSuitLocker.StatesInstance, float>) ((smi, dt) => smi.master.FillBattery(dt)), UpdateRate.SIM_1000ms);
      this.charged.EventTransition(GameHashes.OnStorageChange, this.empty, (StateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() == (UnityEngine.Object) null));
    }

    public class ChargingStates : 
      GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State
    {
      public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State notoperational;
      public GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.State operational;
    }
  }

  public class StatesInstance(LeadSuitLocker lead_suit_locker) : 
    GameStateMachine<LeadSuitLocker.States, LeadSuitLocker.StatesInstance, LeadSuitLocker, object>.GameInstance(lead_suit_locker)
  {
  }
}
