// Decompiled with JetBrains decompiler
// Type: IceKettle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class IceKettle : 
  GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>
{
  public static string LIQUID_METER_TARGET_NAME = "kettle_meter_target";
  public static string LIQUID_METER_ANIM_NAME = "meter_kettle";
  public static string IDEL_ANIM_STATE = "on";
  public static string BOILING_PRE_ANIM_NAME = "boiling_pre";
  public static string BOILING_LOOP_ANIM_NAME = "boiling_loop";
  public static string BOILING_PST_ANIM_NAME = "boiling_pst";
  private const float InUseTimeout = 5f;
  public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State noOperational;
  public IceKettle.OperationalStates operational;
  public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State inUse;
  public StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.FloatParameter MeltingTimer;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.noOperational;
    this.root.EventHandlerTransition(GameHashes.WorkableStartWork, this.inUse, (Func<IceKettle.Instance, object, bool>) ((smi, obj) => true)).EventHandler(GameHashes.OnStorageChange, (StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback) (smi => smi.UpdateMeter()));
    this.noOperational.TagTransition(GameTags.Operational, (GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State) this.operational);
    this.operational.TagTransition(GameTags.Operational, this.noOperational, true).DefaultState((GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State) this.operational.idle);
    this.operational.idle.PlayAnim(IceKettle.IDEL_ANIM_STATE).DefaultState(this.operational.idle.waitingForSolids);
    this.operational.idle.waitingForSolids.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientSolids).EventTransition(GameHashes.OnStorageChange, this.operational.idle.waitingForSpaceInLiquidTank, (StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback) (smi => IceKettle.HasEnoughSolidsToMelt(smi)));
    this.operational.idle.waitingForSpaceInLiquidTank.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientLiquidSpace).EventTransition(GameHashes.OnStorageChange, this.operational.idle.notEnoughFuel, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.LiquidTankHasCapacityForNextBatch));
    this.operational.idle.notEnoughFuel.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleInsuficientFuel).EventTransition(GameHashes.OnStorageChange, (GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State) this.operational.melting, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch));
    this.operational.melting.Toggle("Operational Active State", new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.SetOperationalActiveStatesTrue), new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.SetOperationalActiveStatesFalse)).DefaultState(this.operational.melting.entering);
    this.operational.melting.entering.PlayAnim(IceKettle.BOILING_PRE_ANIM_NAME, KAnim.PlayMode.Once).OnAnimQueueComplete((GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State) this.operational.melting.working);
    this.operational.melting.working.ToggleStatusItem(Db.Get().BuildingStatusItems.KettleMelting).DefaultState(this.operational.melting.working.idle).PlayAnim(IceKettle.BOILING_LOOP_ANIM_NAME, KAnim.PlayMode.Loop);
    this.operational.melting.working.idle.ParamTransition<float>((StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Parameter<float>) this.MeltingTimer, this.operational.melting.working.complete, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Parameter<float>.Callback(IceKettle.IsDoneMelting)).Update(new System.Action<IceKettle.Instance, float>(IceKettle.MeltingTimerUpdate));
    this.operational.melting.working.complete.Enter(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.ResetMeltingTimer)).Enter(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State.Callback(IceKettle.MeltNextBatch)).EnterTransition(this.operational.melting.working.idle, new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch)).EnterTransition(this.operational.melting.exit, GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Not(new StateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.Transition.ConditionCallback(IceKettle.CanMeltNextBatch)));
    this.operational.melting.exit.PlayAnim(IceKettle.BOILING_PST_ANIM_NAME, KAnim.PlayMode.Once).OnAnimQueueComplete((GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State) this.operational.idle);
    this.inUse.EventHandlerTransition(GameHashes.WorkableStopWork, this.noOperational, (Func<IceKettle.Instance, object, bool>) ((smi, obj) => true)).ScheduleGoTo(new Func<IceKettle.Instance, float>(IceKettle.GetInUseTimeout), (StateMachine.BaseState) this.noOperational);
  }

  public static void SetOperationalActiveStatesTrue(IceKettle.Instance smi)
  {
    smi.operational.SetActive(true);
  }

  public static void SetOperationalActiveStatesFalse(IceKettle.Instance smi)
  {
    smi.operational.SetActive(false);
  }

  public static float GetInUseTimeout(IceKettle.Instance smi) => smi.InUseWorkableDuration + 1f;

  public static void ResetMeltingTimer(IceKettle.Instance smi)
  {
    double num = (double) smi.sm.MeltingTimer.Set(0.0f, smi);
  }

  public static bool HasEnoughSolidsToMelt(IceKettle.Instance smi)
  {
    return smi.HasAtLeastOneBatchOfSolidsWaitingToMelt;
  }

  public static bool LiquidTankHasCapacityForNextBatch(IceKettle.Instance smi)
  {
    return smi.LiquidTankHasCapacityForNextBatch;
  }

  public static bool HasEnoughFuelForNextBacth(IceKettle.Instance smi)
  {
    return smi.HasEnoughFuelUnitsToMeltNextBatch;
  }

  public static bool CanMeltNextBatch(IceKettle.Instance smi)
  {
    return smi.HasAtLeastOneBatchOfSolidsWaitingToMelt && IceKettle.LiquidTankHasCapacityForNextBatch(smi) && IceKettle.HasEnoughFuelForNextBacth(smi);
  }

  public static bool IsDoneMelting(IceKettle.Instance smi, float timePassed)
  {
    return (double) timePassed >= (double) smi.MeltDurationPerBatch;
  }

  public static void MeltingTimerUpdate(IceKettle.Instance smi, float dt)
  {
    float num1 = smi.sm.MeltingTimer.Get(smi);
    double num2 = (double) smi.sm.MeltingTimer.Set(num1 + dt, smi);
  }

  public static void MeltNextBatch(IceKettle.Instance smi) => smi.MeltNextBatch();

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public SimHashes exhaust_tag;
    public Tag targetElementTag;
    public Tag fuelElementTag;
    public float KGToMeltPerBatch;
    public float KGMeltedPerSecond;
    public float TargetTemperature;
    public float EnergyPerUnitOfLumber;
    public float ExhaustMassPerUnitOfLumber;

    public List<Descriptor> GetDescriptors(GameObject go)
    {
      return new List<Descriptor>()
      {
        new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.KETTLE_MELT_RATE, (object) GameUtil.GetFormattedMass(this.KGMeltedPerSecond, GameUtil.TimeSlice.PerSecond)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.KETTLE_MELT_RATE, (object) GameUtil.GetFormattedMass(this.KGToMeltPerBatch), (object) GameUtil.GetFormattedTemperature(this.TargetTemperature)))
      };
    }
  }

  public class WorkingStates : 
    GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
  {
    public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State idle;
    public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State complete;
  }

  public class MeltingStates : 
    GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
  {
    public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State entering;
    public IceKettle.WorkingStates working;
    public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State exit;
  }

  public class IdleStates : 
    GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
  {
    public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State notEnoughFuel;
    public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State waitingForSolids;
    public GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State waitingForSpaceInLiquidTank;
  }

  public class OperationalStates : 
    GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.State
  {
    public IceKettle.MeltingStates melting;
    public IceKettle.IdleStates idle;
  }

  public new class Instance : 
    GameStateMachine<IceKettle, IceKettle.Instance, IStateMachineTarget, IceKettle.Def>.GameInstance
  {
    private Storage fuelStorage;
    private Storage kettleStorage;
    private Storage outputStorage;
    private Element elementToMelt;
    private MeterController LiquidMeter;
    [MyCmpGet]
    public Operational operational;
    [MyCmpGet]
    private IceKettleWorkable dupeWorkable;
    [MyCmpGet]
    private KBatchedAnimController animController;

    public float CurrentTemperatureOfSolidsStored
    {
      get
      {
        return (double) this.kettleStorage.MassStored() <= 0.0 ? 0.0f : this.kettleStorage.items[0].GetComponent<PrimaryElement>().Temperature;
      }
    }

    public float MeltDurationPerBatch => this.def.KGToMeltPerBatch / this.def.KGMeltedPerSecond;

    public float FuelUnitsAvailable => this.fuelStorage.MassStored();

    public bool HasAtLeastOneBatchOfSolidsWaitingToMelt
    {
      get => (double) this.kettleStorage.MassStored() >= (double) this.def.KGToMeltPerBatch;
    }

    public bool HasEnoughFuelUnitsToMeltNextBatch
    {
      get
      {
        return (double) this.kettleStorage.MassStored() <= 0.0 || (double) this.FuelUnitsAvailable >= (double) this.FuelRequiredForNextBratch;
      }
    }

    public bool LiquidTankHasCapacityForNextBatch
    {
      get => (double) this.outputStorage.RemainingCapacity() >= (double) this.def.KGToMeltPerBatch;
    }

    public float LiquidTankCapacity => this.outputStorage.capacityKg;

    public float LiquidStored => this.outputStorage.MassStored();

    public float FuelRequiredForNextBratch
    {
      get
      {
        return this.GetUnitsOfFuelRequiredToMelt(this.elementToMelt, this.def.KGToMeltPerBatch, this.CurrentTemperatureOfSolidsStored);
      }
    }

    public float InUseWorkableDuration => this.dupeWorkable.workTime;

    public Instance(IStateMachineTarget master, IceKettle.Def def)
      : base(master, def)
    {
      this.elementToMelt = ElementLoader.GetElement(def.targetElementTag);
      this.LiquidMeter = new MeterController((KAnimControllerBase) this.animController, IceKettle.LIQUID_METER_TARGET_NAME, IceKettle.LIQUID_METER_ANIM_NAME, Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
      Storage[] components = this.gameObject.GetComponents<Storage>();
      this.fuelStorage = components[0];
      this.kettleStorage = components[1];
      this.outputStorage = components[2];
    }

    public override void StartSM()
    {
      base.StartSM();
      this.UpdateMeter();
    }

    public void UpdateMeter()
    {
      this.LiquidMeter.SetPositionPercent(this.outputStorage.MassStored() / this.outputStorage.capacityKg);
    }

    public void MeltNextBatch()
    {
      if (!this.HasAtLeastOneBatchOfSolidsWaitingToMelt)
        return;
      float amount = Mathf.Min(this.GetUnitsOfFuelRequiredToMelt(this.elementToMelt, this.def.KGToMeltPerBatch, this.kettleStorage.FindFirst(this.def.targetElementTag).GetComponent<PrimaryElement>().Temperature), this.FuelUnitsAvailable);
      float amount_consumed = 0.0f;
      float aggregate_temperature = 0.0f;
      SimUtil.DiseaseInfo disease_info;
      this.kettleStorage.ConsumeAndGetDisease(this.elementToMelt.id.CreateTag(), this.def.KGToMeltPerBatch, out amount_consumed, out disease_info, out aggregate_temperature);
      this.outputStorage.AddElement(this.elementToMelt.highTempTransitionTarget, amount_consumed, this.def.TargetTemperature, disease_info.idx, disease_info.count);
      float temperature = this.fuelStorage.FindFirst(this.def.fuelElementTag).GetComponent<PrimaryElement>().Temperature;
      this.fuelStorage.ConsumeIgnoringDisease(this.def.fuelElementTag, amount);
      float mass = amount * this.def.ExhaustMassPerUnitOfLumber;
      Element elementByHash = ElementLoader.FindElementByHash(this.def.exhaust_tag);
      SimMessages.AddRemoveSubstance(Grid.PosToCell(this.gameObject), elementByHash.id, (CellAddRemoveSubstanceEvent) null, mass, temperature, byte.MaxValue, 0);
    }

    public float GetUnitsOfFuelRequiredToMelt(
      Element elementToMelt,
      float massToMelt_KG,
      float elementToMelt_initialTemperature)
    {
      if (!elementToMelt.IsSolid)
        return -1f;
      float num = massToMelt_KG * elementToMelt.specificHeatCapacity * elementToMelt_initialTemperature;
      float targetTemperature = this.def.TargetTemperature;
      return (massToMelt_KG * elementToMelt.specificHeatCapacity * targetTemperature - num) / this.def.EnergyPerUnitOfLumber;
    }
  }
}
