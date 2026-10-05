// Decompiled with JetBrains decompiler
// Type: RobotElectroBankMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RobotElectroBankMonitor : 
  GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>
{
  public static readonly HashedString BATTER_SYMBOL = (HashedString) "meter_target";
  public static readonly HashedString BATTER_FULL_SYMBOL = (HashedString) "battery_full";
  public static readonly HashedString BATTER_LOW_SYMBOL = (HashedString) "battery_low";
  public static readonly HashedString BATTER_DEAD_SYMBOL = (HashedString) "battery_dead";
  public RobotElectroBankMonitor.PoweredState powered;
  public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State deceased;
  public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State powerdown;
  public StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.BoolParameter hasElectrobank;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.powered;
    this.root.Enter((StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State.Callback) (smi => smi.ElectroBankStorageChange())).TagTransition(GameTags.Dead, this.deceased).TagTransition(GameTags.Creatures.Die, this.deceased);
    this.powered.DefaultState(this.powered.highBattery).ParamTransition<bool>((StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Parameter<bool>) this.hasElectrobank, this.powerdown, GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.IsFalse).Update((System.Action<RobotElectroBankMonitor.Instance, float>) ((smi, dt) => RobotElectroBankMonitor.ConsumePower(smi, dt)));
    this.powered.highBattery.Transition(this.powered.lowBattery, GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Not(new StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Transition.ConditionCallback(RobotElectroBankMonitor.ChargeDecent))).Enter((StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State.Callback) (smi => this.UpdateBatteryMeter(smi, RobotElectroBankMonitor.BATTER_FULL_SYMBOL)));
    this.powered.lowBattery.Enter((StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State.Callback) (smi =>
    {
      RobotElectroBankMonitor.RequestBattery(smi);
      this.UpdateBatteryMeter(smi, RobotElectroBankMonitor.BATTER_LOW_SYMBOL);
    })).Transition(this.powered.highBattery, new StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Transition.ConditionCallback(RobotElectroBankMonitor.ChargeDecent)).ToggleStatusItem((Func<RobotElectroBankMonitor.Instance, StatusItem>) (smi => Db.Get().RobotStatusItems.LowBatteryNoCharge));
    this.powerdown.Enter((StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State.Callback) (smi => RobotElectroBankMonitor.RequestBattery(smi))).ToggleBehaviour(GameTags.Robots.Behaviours.NoElectroBank, (StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Transition.ConditionCallback) (smi => true), (System.Action<RobotElectroBankMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.powered)));
    this.deceased.DoNothing();
  }

  private void UpdateBatteryMeter(RobotElectroBankMonitor.Instance smi, HashedString symbol)
  {
    smi.UpdateBatteryState(symbol);
  }

  public static bool ChargeDecent(RobotElectroBankMonitor.Instance smi)
  {
    float num = 0.0f;
    foreach (GameObject gameObject in smi.electroBankStorage.items)
    {
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
        num += gameObject.GetComponent<Electrobank>().Charge;
    }
    return (double) num >= (double) smi.def.lowBatteryWarningPercent * 120000.0;
  }

  public static void ConsumePower(RobotElectroBankMonitor.Instance smi, float dt)
  {
    if ((UnityEngine.Object) smi.electrobank == (UnityEngine.Object) null)
    {
      RobotElectroBankMonitor.RequestBattery(smi);
    }
    else
    {
      float joules = Mathf.Min(dt * Mathf.Abs(smi.bankAmount.GetDelta()), smi.electrobank.Charge);
      double num = (double) smi.electrobank.RemovePower(joules, true);
      if (!((UnityEngine.Object) smi.electrobank != (UnityEngine.Object) null))
        return;
      smi.bankAmount.value = smi.electrobank.Charge;
    }
  }

  public static void RequestBattery(RobotElectroBankMonitor.Instance smi)
  {
    if (!smi.fetchBatteryChore.IsPaused)
      return;
    smi.fetchBatteryChore.Pause((UnityEngine.Object) smi.electrobank != (UnityEngine.Object) null && RobotElectroBankMonitor.ChargeDecent(smi), "FlydoBattery");
  }

  public class Def : StateMachine.BaseDef
  {
    public float lowBatteryWarningPercent;
  }

  public class PoweredState : 
    GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State
  {
    public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State highBattery;
    public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State lowBattery;
  }

  public new class Instance : 
    GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.GameInstance
  {
    public Storage electroBankStorage;
    public Electrobank electrobank;
    public ManualDeliveryKG fetchBatteryChore;
    public AmountInstance bankAmount;
    [MyCmpReq]
    private SymbolOverrideController symbolOverrideController;
    [MyCmpReq]
    private KBatchedAnimController animController;
    private HashedString currentSymbolSwap;
    private HashSet<Tag> batteryTags = new HashSet<Tag>();

    public Instance(IStateMachineTarget master, RobotElectroBankMonitor.Def def)
      : base(master, def)
    {
      this.fetchBatteryChore = this.GetComponent<ManualDeliveryKG>();
      foreach (Storage component in master.gameObject.GetComponents<Storage>())
      {
        if (component.storageID == GameTags.ChargedPortableBattery)
        {
          this.electroBankStorage = component;
          break;
        }
      }
      foreach (GameObject gameObject in Assets.GetPrefabsWithTag(GameTags.ChargedPortableBattery))
        this.batteryTags.Add(gameObject.GetComponent<KPrefabID>().PrefabTag);
      this.bankAmount = Db.Get().Amounts.InternalElectroBank.Lookup(master.gameObject);
      this.electroBankStorage.Subscribe(-1697596308, new System.Action<object>(this.ElectroBankStorageChange));
      this.ElectroBankStorageChange();
      this.GetComponent<TreeFilterable>().OnFilterChanged += new System.Action<HashSet<Tag>>(this.OnFilterChanged);
    }

    public void ElectroBankStorageChange(object data = null)
    {
      GameObject go = (GameObject) data;
      if ((UnityEngine.Object) go != (UnityEngine.Object) null)
      {
        Pickupable component = go.GetComponent<Pickupable>();
        if ((UnityEngine.Object) component.storage != (UnityEngine.Object) null && component.storage.storageID == GameTags.ChargedPortableBattery)
        {
          if (this.electroBankStorage.Count > 0 && (UnityEngine.Object) this.electroBankStorage.items[0] != (UnityEngine.Object) null)
          {
            this.electrobank = this.electroBankStorage.items[0].GetComponent<Electrobank>();
            this.bankAmount.value = this.electrobank.Charge;
          }
          else
            this.electrobank = (Electrobank) null;
        }
        else if (this.electroBankStorage.Count <= 0)
        {
          this.electrobank = (Electrobank) null;
          this.bankAmount.value = 0.0f;
          this.DropDischargedElectroBank(go);
        }
        this.fetchBatteryChore.Pause((UnityEngine.Object) this.electrobank != (UnityEngine.Object) null && RobotElectroBankMonitor.ChargeDecent(this), "Robot has sufficienct electrobank");
        this.sm.hasElectrobank.Set((UnityEngine.Object) this.electrobank != (UnityEngine.Object) null, this);
      }
      else
      {
        if (!((UnityEngine.Object) this.electrobank == (UnityEngine.Object) null))
          return;
        if (this.electroBankStorage.Count > 0 && (UnityEngine.Object) this.electroBankStorage.items[0] != (UnityEngine.Object) null)
        {
          this.electrobank = this.electroBankStorage.items[0].GetComponent<Electrobank>();
          this.bankAmount.value = this.electrobank.Charge;
        }
        else
        {
          this.electrobank = (Electrobank) null;
          this.bankAmount.value = 0.0f;
        }
        this.fetchBatteryChore.Pause((UnityEngine.Object) this.electrobank != (UnityEngine.Object) null && RobotElectroBankMonitor.ChargeDecent(this), "Robot has sufficienct electrobank");
        this.sm.hasElectrobank.Set((UnityEngine.Object) this.electrobank != (UnityEngine.Object) null, this);
      }
    }

    private void DropDischargedElectroBank(GameObject go)
    {
      Electrobank component = go.GetComponent<Electrobank>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.HasTag(GameTags.ChargedPortableBattery) || component.IsFullyCharged)
        return;
      double num = (double) component.RemovePower(component.Charge, true);
    }

    public void UpdateBatteryState(HashedString newState)
    {
      if (this.currentSymbolSwap.IsValid)
        this.symbolOverrideController.RemoveSymbolOverride(this.currentSymbolSwap);
      KAnim.Build.Symbol symbol = this.animController.AnimFiles[0].GetData().build.GetSymbol((KAnimHashedString) newState);
      this.symbolOverrideController.AddSymbolOverride(RobotElectroBankMonitor.BATTER_SYMBOL, symbol);
      this.currentSymbolSwap = newState;
    }

    private void OnFilterChanged(HashSet<Tag> allowed_tags)
    {
      if (!((UnityEngine.Object) this.fetchBatteryChore != (UnityEngine.Object) null))
        return;
      List<Tag> tagList = new List<Tag>();
      foreach (Tag batteryTag in this.batteryTags)
      {
        if (!allowed_tags.Contains(batteryTag))
          tagList.Add(batteryTag);
      }
      this.fetchBatteryChore.ForbiddenTags = tagList.ToArray();
    }
  }
}
