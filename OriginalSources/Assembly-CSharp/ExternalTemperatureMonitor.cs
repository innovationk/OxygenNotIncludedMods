// Decompiled with JetBrains decompiler
// Type: ExternalTemperatureMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class ExternalTemperatureMonitor : 
  GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance>
{
  public const float EXTERNAL_WARM_THRESHOLD = 0.008f;
  public const float EXTERNAL_COLD_THRESHOLD = -0.039f;
  public const float EXTERNAL_COLD_THRESHOLD_RESISTANCE_DURATION = 5f;
  public const float EXTERNAL_COLD_THRESHOLD_RESISTANCE_MULTIPLIER = 10f;
  public const string CHILLY_SURROUNDINGS_EFFECT_NAME = "ColdAir";
  public const string TOASTY_SURROUNDINGS_EFFECT_NAME = "WarmAir";
  public static readonly float BASE_STRESS_TOLERANCE_COLD = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_WARMING_KILOWATTS * 0.2f;
  public static readonly float BASE_STRESS_TOLERANCE_WARM = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_COOLING_KILOWATTS * 0.2f;
  private const float START_GAME_AVERAGING_DELAY = 6f;
  private const float TRANSITION_TO_DELAY = 1f;
  private const float TRANSITION_OUT_DELAY = 6f;
  public ExternalTemperatureMonitor.AliveStates alive;
  public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State dead;
  private StateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.FloatParameter _ColdResistanceDurationRemaining;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.alive;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.alive.TagTransition(GameTags.Dead, this.dead).Update(new System.Action<ExternalTemperatureMonitor.Instance, float>(ExternalTemperatureMonitor.UpdateTemperatureTresholdModifiers), UpdateRate.SIM_1000ms).DefaultState(this.alive.comfortable);
    this.alive.comfortable.Transition(this.alive.transitionToTooWarm, (StateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsTooHot() && (double) smi.timeinstate > 6.0)).Transition(this.alive.transitionToTooCool, (StateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsTooCold() && (double) smi.timeinstate > 6.0));
    this.alive.transitionToTooWarm.Transition(this.alive.comfortable, (StateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsTooHot())).Transition(this.alive.tooWarm, (StateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsTooHot() && (double) smi.timeinstate > 1.0));
    this.alive.transitionToTooCool.Transition(this.alive.comfortable, (StateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsTooCold())).Transition(this.alive.tooCool, (StateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsTooCold() && (double) smi.timeinstate > 1.0));
    this.alive.tooWarm.ToggleTag(GameTags.FeelingWarm).Transition(this.alive.comfortable, (StateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsTooHot() && (double) smi.timeinstate > 6.0)).EventHandlerTransition(GameHashes.EffectAdded, this.alive.comfortable, (Func<ExternalTemperatureMonitor.Instance, object, bool>) ((smi, obj) => !smi.IsTooHot())).Enter((StateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort)));
    this.alive.tooCool.ToggleTag(GameTags.FeelingCold).Transition(this.alive.comfortable, (StateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsTooCold() && (double) smi.timeinstate > 6.0)).EventHandlerTransition(GameHashes.EffectAdded, this.alive.comfortable, (Func<ExternalTemperatureMonitor.Instance, object, bool>) ((smi, obj) => !smi.IsTooCold())).Enter((StateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort)));
    this.dead.DoNothing();
  }

  public static void UpdateTemperatureTresholdModifiers(
    ExternalTemperatureMonitor.Instance smi,
    float dt)
  {
    smi.UpdateTemperatureTresholdModifiers(dt);
  }

  public static float GetExternalColdThreshold(ExternalTemperatureMonitor.Instance smi)
  {
    return smi == null ? -0.039f : -0.039f * smi.CurrentColdResistanceModifier;
  }

  public class AliveStates : 
    GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State comfortable;
    public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State transitionToTooWarm;
    public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State tooWarm;
    public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State transitionToTooCool;
    public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State tooCool;
  }

  public new class Instance : 
    GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Effects effects;
    public Traits traits;
    public Attributes attributes;
    public AmountInstance internalTemperature;
    public CreatureSimTemperatureTransfer temperatureTransferer;
    public PrimaryElement primaryElement;
    public MinionResume minionResume;
    private Effect coldAirEffect = Db.Get().effects.Get("ColdAir");
    private Effect[] immunityToColdEffects = new Effect[2]
    {
      Db.Get().effects.Get("WarmTouch"),
      Db.Get().effects.Get("WarmTouchFood")
    };
    private Effect warmAirEffect = Db.Get().effects.Get("WarmAir");

    public float CurrentColdResistanceModifier => !this.HasColdResistance ? 1f : 10f;

    public bool HasColdResistance => (double) this.ColdResistanceDurationRemaining > 0.0;

    public float ColdResistanceDurationRemaining
    {
      get => this.sm._ColdResistanceDurationRemaining.Get(this);
    }

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.attributes = this.gameObject.GetAttributes();
      this.minionResume = this.gameObject.GetComponent<MinionResume>();
      this.internalTemperature = Db.Get().Amounts.Temperature.Lookup(this.gameObject);
      this.temperatureTransferer = this.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
      this.primaryElement = this.gameObject.GetComponent<PrimaryElement>();
      this.effects = this.gameObject.GetComponent<Effects>();
      this.traits = this.gameObject.GetComponent<Traits>();
    }

    public bool IsTooHot()
    {
      return !this.effects.HasEffect("RefreshingTouch") && !this.effects.HasImmunityTo(this.warmAirEffect) && this.temperatureTransferer.LastTemperatureRecordIsReliable && (double) this.smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage > 0.00800000037997961;
    }

    public bool IsTooCold()
    {
      for (int index = 0; index < this.immunityToColdEffects.Length; ++index)
      {
        if (this.effects.HasEffect(this.immunityToColdEffects[index]))
          return false;
      }
      return !this.effects.HasImmunityTo(this.coldAirEffect) && (!((UnityEngine.Object) this.traits != (UnityEngine.Object) null) || !this.traits.IsEffectIgnored(this.coldAirEffect)) && !WarmthProvider.IsWarmCell(Grid.PosToCell((StateMachine.Instance) this)) && this.temperatureTransferer.LastTemperatureRecordIsReliable && (double) this.smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage < (double) ExternalTemperatureMonitor.GetExternalColdThreshold(this);
    }

    public void UpdateTemperatureTresholdModifiers(float dt)
    {
      int cell = Grid.PosToCell((StateMachine.Instance) this);
      bool flag = (UnityEngine.Object) this.minionResume != (UnityEngine.Object) null && this.minionResume.HasPerk((HashedString) Db.Get().SkillPerks.ImprovedLiquidTemperatureTolerance.Id);
      if (((!Grid.IsValidCell(cell) ? 0 : (Grid.Element[cell].IsLiquid ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      {
        double num1 = (double) this.sm._ColdResistanceDurationRemaining.Set(5f, this);
      }
      else
      {
        double num2 = (double) this.sm._ColdResistanceDurationRemaining.Set(Mathf.Max(0.0f, this.ColdResistanceDurationRemaining - dt), this);
      }
    }
  }
}
