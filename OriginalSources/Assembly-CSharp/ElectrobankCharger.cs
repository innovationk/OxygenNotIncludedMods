// Decompiled with JetBrains decompiler
// Type: ElectrobankCharger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ElectrobankCharger : 
  GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>
{
  public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State noBattery;
  public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State inoperational;
  public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State charging;
  public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State full;
  public StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.FloatParameter internalChargeAmount;
  public StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.BoolParameter hasElectrobank;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.noBattery;
    this.noBattery.PlayAnim("off").EventHandler(GameHashes.OnStorageChange, (GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.GameEvent.Callback) ((smi, data) => smi.QueueElectrobank())).ParamTransition<bool>((StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.Parameter<bool>) this.hasElectrobank, this.charging, GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.IsTrue).Enter((StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State.Callback) (smi => smi.QueueElectrobank()));
    this.inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.charging, (StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.Transition.ConditionCallback) (smi => smi.master.GetComponent<Operational>().IsOperational));
    this.charging.QueueAnim("working_pre").QueueAnim("working_loop", true).Enter((StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State.Callback) (smi =>
    {
      smi.QueueElectrobank();
      smi.master.GetComponent<Operational>().SetActive(true);
    })).Exit((StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State.Callback) (smi => smi.master.GetComponent<Operational>().SetActive(false))).ToggleStatusItem(Db.Get().BuildingStatusItems.PowerBankChargerInProgress).Update((System.Action<ElectrobankCharger.Instance, float>) ((smi, dt) => smi.ChargeInternal(smi, dt)), UpdateRate.SIM_EVERY_TICK).EventTransition(GameHashes.OperationalChanged, this.inoperational, (StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.Transition.ConditionCallback) (smi => !smi.master.GetComponent<Operational>().IsOperational)).ParamTransition<float>((StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.Parameter<float>) this.internalChargeAmount, this.full, (StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.Parameter<float>.Callback) ((smi, dt) => (double) this.internalChargeAmount.Get(smi) >= 120000.0));
    this.full.PlayAnim("working_pst").Enter((StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State.Callback) (smi => smi.TransferChargeToElectrobank())).OnAnimQueueComplete(this.noBattery);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.GameInstance
  {
    private Storage storage;
    public GameObject targetElectrobank;
    private MeterController meterController;

    public Storage Storage
    {
      get
      {
        if ((UnityEngine.Object) this.storage == (UnityEngine.Object) null)
          this.storage = this.GetComponent<Storage>();
        return this.storage;
      }
    }

    public Instance(IStateMachineTarget master, ElectrobankCharger.Def def)
      : base(master, def)
    {
      this.meterController = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
    }

    public void ChargeInternal(ElectrobankCharger.Instance smi, float dt)
    {
      double num = (double) smi.sm.internalChargeAmount.Delta(dt * 400f, smi);
      this.UpdateMeter();
    }

    public void UpdateMeter()
    {
      this.meterController.SetPositionPercent(this.sm.internalChargeAmount.Get(this.smi) / 120000f);
    }

    public void TransferChargeToElectrobank()
    {
      this.targetElectrobank = Electrobank.ReplaceEmptyWithCharged(this.targetElectrobank, true);
      this.DequeueElectrobank();
    }

    public void DequeueElectrobank()
    {
      this.targetElectrobank = (GameObject) null;
      this.smi.sm.hasElectrobank.Set(false, this.smi);
      double num = (double) this.smi.sm.internalChargeAmount.Set(0.0f, this.smi);
      this.UpdateMeter();
    }

    public void QueueElectrobank(object data = null)
    {
      if ((UnityEngine.Object) this.targetElectrobank == (UnityEngine.Object) null)
      {
        for (int index = 0; index < this.Storage.items.Count; ++index)
        {
          GameObject go = this.Storage.items[index];
          if ((UnityEngine.Object) go != (UnityEngine.Object) null && go.HasTag(GameTags.EmptyPortableBattery))
          {
            this.targetElectrobank = go;
            this.smi.sm.hasElectrobank.Set(true, this.smi);
            break;
          }
        }
      }
      this.UpdateMeter();
    }
  }
}
