// Decompiled with JetBrains decompiler
// Type: BionicUpgrade_SM`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class BionicUpgrade_SM<SMType, StateMachineInstanceType> : 
  GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>
  where SMType : GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>
  where StateMachineInstanceType : BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance
{
  public GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.State Active;
  public GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.State Inactive;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.Inactive;
  }

  public static bool IsOnline(
    BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance smi)
  {
    return smi.IsOnline;
  }

  public static bool IsInBedTimeChore(
    BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance smi)
  {
    return smi.IsInBedTimeChore;
  }

  public class Def : StateMachine.BaseDef
  {
    public string UpgradeID;
    public Func<StateMachine.Instance, StateMachine.Instance>[] StateMachinesWhenActive;

    public Def(string upgradeID) => this.UpgradeID = upgradeID;

    public virtual string GetDescription() => "";
  }

  public abstract class BaseInstance : 
    GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.GameInstance,
    BionicUpgradeComponent.IWattageController
  {
    protected BionicBedTimeMonitor.Instance bedTimeMonitor;
    protected BionicBatteryMonitor.Instance batteryMonitor;
    protected BionicUpgradeComponent upgradeComponent;

    public bool IsInBedTimeChore
    {
      get => this.bedTimeMonitor != null && this.bedTimeMonitor.IsBedTimeChoreRunning;
    }

    public bool IsOnline => this.batteryMonitor != null && this.batteryMonitor.IsOnline;

    public BionicUpgradeComponentConfig.BionicUpgradeData Data
    {
      get => BionicUpgradeComponentConfig.UpgradesData[(Tag) this.def.UpgradeID];
    }

    public BaseInstance(
      IStateMachineTarget master,
      BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def def)
      : base(master, def)
    {
      this.batteryMonitor = this.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
      this.bedTimeMonitor = this.gameObject.GetSMI<BionicBedTimeMonitor.Instance>();
      this.RegisterMonitorToUpgradeComponent();
    }

    private void RegisterMonitorToUpgradeComponent()
    {
      foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in this.gameObject.GetSMI<BionicUpgradesMonitor.Instance>().upgradeComponentSlots)
      {
        if (upgradeComponentSlot.HasUpgradeInstalled)
        {
          BionicUpgradeComponent upgradeComponent = upgradeComponentSlot.installedUpgradeComponent;
          if ((UnityEngine.Object) upgradeComponent != (UnityEngine.Object) null && !upgradeComponent.HasWattageController)
          {
            this.upgradeComponent = upgradeComponent;
            upgradeComponent.SetWattageController((BionicUpgradeComponent.IWattageController) this);
            break;
          }
        }
      }
    }

    private void UnregisterMonitorToUpgradeComponent()
    {
      if (!((UnityEngine.Object) this.upgradeComponent != (UnityEngine.Object) null))
        return;
      this.upgradeComponent.SetWattageController((BionicUpgradeComponent.IWattageController) null);
    }

    public abstract float GetCurrentWattageCost();

    public abstract string GetCurrentWattageCostName();

    protected override void OnCleanUp()
    {
      this.UnregisterMonitorToUpgradeComponent();
      base.OnCleanUp();
    }
  }
}
