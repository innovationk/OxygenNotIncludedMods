// Decompiled with JetBrains decompiler
// Type: MercuryLight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MercuryLight : 
  GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>
{
  private static Tag ELEMENT_TAG = SimHashes.Mercury.CreateTag();
  private const string ON_ANIM_NAME = "on";
  private const string ON_PRE_ANIM_NAME = "on_pre";
  private const string TRANSITION_TO_OFF_ANIM_NAME = "on_pst";
  private const string DEPLEATING_ANIM_NAME = "depleating";
  private const string OFF_ANIM_NAME = "off";
  private const string LIGHT_LEVEL_METER_TARGET_NAME = "meter_target";
  private const string LIGHT_LEVEL_METER_ANIM_NAME = "meter";
  public MercuryLight.Darknesstates noOperational;
  public MercuryLight.OperationalStates operational;
  public StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.FloatParameter Charge;
  public StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.BoolParameter HasEnoughFuel;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.noOperational;
    this.noOperational.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOff)).ParamTransition<float>((StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.Parameter<float>) this.Charge, this.noOperational.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero).ParamTransition<float>((StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.Parameter<float>) this.Charge, this.noOperational.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero);
    this.noOperational.depleating.TagTransition(GameTags.Operational, (GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State) this.operational).PlayAnim("depleating", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleating).ParamTransition<float>((StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.Parameter<float>) this.Charge, this.noOperational.depleated, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero).Update(new System.Action<MercuryLight.Instance, float>(MercuryLight.DepleteUpdate));
    this.noOperational.depleated.TagTransition(GameTags.Operational, (GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State) this.operational).PlayAnim("on_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.noOperational.idle);
    this.noOperational.idle.TagTransition(GameTags.Operational, this.noOperational.exit).PlayAnim("off", KAnim.PlayMode.Once).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleated);
    this.noOperational.exit.PlayAnim("on_pre", KAnim.PlayMode.Once).OnAnimQueueComplete((GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State) this.operational);
    this.operational.TagTransition(GameTags.Operational, (GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State) this.noOperational, true).DefaultState((GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State) this.operational.darkness).Update(new System.Action<MercuryLight.Instance, float>(MercuryLight.ConsumeFuelUpdate));
    this.operational.darkness.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOff)).ParamTransition<bool>((StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.Parameter<bool>) this.HasEnoughFuel, (GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State) this.operational.light, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsTrue).ParamTransition<float>((StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.Parameter<float>) this.Charge, this.operational.darkness.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero).ParamTransition<float>((StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.Parameter<float>) this.Charge, this.operational.darkness.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero);
    this.operational.darkness.depleating.PlayAnim("depleating", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleating).ParamTransition<float>((StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.Parameter<float>) this.Charge, this.operational.darkness.depleated, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTEZero).Update(new System.Action<MercuryLight.Instance, float>(MercuryLight.DepleteUpdate));
    this.operational.darkness.depleated.PlayAnim("on_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.darkness.idle);
    this.operational.darkness.idle.PlayAnim("off", KAnim.PlayMode.Once).ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Depleated).ParamTransition<float>((StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.Parameter<float>) this.Charge, this.operational.darkness.depleating, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTZero);
    this.operational.light.Enter(new StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State.Callback(MercuryLight.SetOperationalActiveFlagOn)).PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition<bool>((StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.Parameter<bool>) this.HasEnoughFuel, (GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State) this.operational.darkness, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight).DefaultState(this.operational.light.charging);
    this.operational.light.charging.ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Charging).ParamTransition<float>((StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.Parameter<float>) this.Charge, this.operational.light.idle, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsGTEOne).Update(new System.Action<MercuryLight.Instance, float>(MercuryLight.ChargeUpdate));
    this.operational.light.idle.ToggleStatusItem(Db.Get().BuildingStatusItems.MercuryLight_Charged).ParamTransition<float>((StateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.Parameter<float>) this.Charge, this.operational.light.charging, GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.IsLTOne);
  }

  public static void SetOperationalActiveFlagOn(MercuryLight.Instance smi)
  {
    smi.operational.SetActive(true);
  }

  public static void SetOperationalActiveFlagOff(MercuryLight.Instance smi)
  {
    smi.operational.SetActive(false);
  }

  public static void DepleteUpdate(MercuryLight.Instance smi, float dt) => smi.DepleteUpdate(dt);

  public static void ChargeUpdate(MercuryLight.Instance smi, float dt) => smi.ChargeUpdate(dt);

  public static void ConsumeFuelUpdate(MercuryLight.Instance smi, float dt)
  {
    smi.ConsumeFuelUpdate(dt);
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public float MAX_LUX;
    public float TURN_ON_DELAY;
    public float FUEL_MASS_PER_SECOND;

    public List<Descriptor> GetDescriptors(GameObject go)
    {
      string str = MercuryLight.ELEMENT_TAG.ProperName();
      return new List<Descriptor>()
      {
        new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTCONSUMED, (object) str, (object) GameUtil.GetFormattedMass(this.FUEL_MASS_PER_SECOND, GameUtil.TimeSlice.PerSecond)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, (object) str, (object) GameUtil.GetFormattedMass(this.FUEL_MASS_PER_SECOND, GameUtil.TimeSlice.PerSecond)), Descriptor.DescriptorType.Requirement)
      };
    }
  }

  public class LightStates : 
    GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
  {
    public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State charging;
    public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State idle;
  }

  public class Darknesstates : 
    GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
  {
    public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State depleating;
    public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State depleated;
    public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State idle;
    public GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State exit;
  }

  public class OperationalStates : 
    GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.State
  {
    public MercuryLight.LightStates light;
    public MercuryLight.Darknesstates darkness;
  }

  public new class Instance : 
    GameStateMachine<MercuryLight, MercuryLight.Instance, IStateMachineTarget, MercuryLight.Def>.GameInstance
  {
    [MyCmpGet]
    public Operational operational;
    [MyCmpGet]
    private Light2D light;
    [MyCmpGet]
    private Storage storage;
    [MyCmpGet]
    private ConduitConsumer conduitConsumer;
    private MeterController lightIntensityMeterController;

    public bool HasEnoughFuel => this.sm.HasEnoughFuel.Get(this);

    public int LuxLevel => Mathf.FloorToInt(this.smi.ChargeLevel * this.def.MAX_LUX);

    public float ChargeLevel => this.smi.sm.Charge.Get(this);

    public Instance(IStateMachineTarget master, MercuryLight.Def def)
      : base(master, def)
    {
      this.lightIntensityMeterController = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
    }

    public override void StartSM()
    {
      base.StartSM();
      this.SetChargeLevel(this.ChargeLevel);
    }

    public void DepleteUpdate(float dt)
    {
      this.SetChargeLevel(Mathf.Clamp(this.ChargeLevel - dt / this.def.TURN_ON_DELAY, 0.0f, 1f));
    }

    public void ChargeUpdate(float dt)
    {
      this.SetChargeLevel(Mathf.Clamp(this.ChargeLevel + dt / this.def.TURN_ON_DELAY, 0.0f, 1f));
    }

    public void SetChargeLevel(float value)
    {
      double num = (double) this.sm.Charge.Set(value, this);
      this.light.Lux = this.LuxLevel;
      this.light.FullRefresh();
      bool flag = (double) this.ChargeLevel > 0.0;
      if (this.light.enabled != flag)
        this.light.enabled = flag;
      this.lightIntensityMeterController.SetPositionPercent(value);
    }

    public void ConsumeFuelUpdate(float dt)
    {
      float amount = this.def.FUEL_MASS_PER_SECOND * dt;
      if ((double) this.storage.MassStored() < (double) amount)
      {
        this.sm.HasEnoughFuel.Set(false, this);
      }
      else
      {
        this.storage.ConsumeAndGetDisease(MercuryLight.ELEMENT_TAG, amount, out float _, out SimUtil.DiseaseInfo _, out float _);
        this.sm.HasEnoughFuel.Set(true, this);
      }
    }

    public bool CanRun() => true;
  }
}
