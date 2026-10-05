// Decompiled with JetBrains decompiler
// Type: WarmBlooded
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class WarmBlooded : StateMachineComponent<WarmBlooded.StatesInstance>
{
  [MyCmpAdd]
  private Notifier notifier;
  public AmountInstance temperature;
  private PrimaryElement primaryElement;
  public WarmBlooded.ComplexityType complexity = WarmBlooded.ComplexityType.FullHomeostasis;
  public string TemperatureAmountName = "Temperature";
  public float IdealTemperature = DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL;
  public float BaseGenerationKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_BASE_GENERATION_KILOWATTS;
  public string BaseTemperatureModifierDescription = (string) DUPLICANTS.MODEL.STANDARD.NAME;
  public float KCal2Joules = DUPLICANTSTATS.STANDARD.BaseStats.KCAL2JOULES;
  public float WarmingKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_WARMING_KILOWATTS;
  public float CoolingKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_COOLING_KILOWATTS;
  public string CaloriesModifierDescription = (string) DUPLICANTS.MODIFIERS.BURNINGCALORIES.NAME;
  public string BodyRegulatorModifierDescription = (string) DUPLICANTS.MODIFIERS.HOMEOSTASIS.NAME;
  public const float TRANSITION_DELAY_HOT = 3f;
  public const float TRANSITION_DELAY_COLD = 3f;

  public static bool IsCold(WarmBlooded.StatesInstance smi)
  {
    return !smi.IsSimpleHeatProducer() && smi.IsCold();
  }

  public static bool IsHot(WarmBlooded.StatesInstance smi)
  {
    return !smi.IsSimpleHeatProducer() && smi.IsHot();
  }

  public static void WarmingRegulator(WarmBlooded.StatesInstance smi, float dt)
  {
    PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
    float temperatureDelta = SimUtil.EnergyFlowToTemperatureDelta(smi.master.CoolingKW, component.Element.specificHeatCapacity, component.Mass);
    float num1 = smi.IdealTemperature - smi.BodyTemperature;
    float num2 = 1f;
    if (((double) temperatureDelta - (double) smi.baseTemperatureModification.Value) * (double) dt < (double) num1)
      num2 = Mathf.Clamp(num1 / ((temperatureDelta - smi.baseTemperatureModification.Value) * dt), 0.0f, 1f);
    smi.bodyRegulator.SetValue(-temperatureDelta * num2);
    if (smi.master.complexity != WarmBlooded.ComplexityType.FullHomeostasis)
      return;
    smi.burningCalories.SetValue(-smi.master.CoolingKW * num2 / smi.master.KCal2Joules);
  }

  public static void CoolingRegulator(WarmBlooded.StatesInstance smi, float dt)
  {
    PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
    float temperatureDelta1 = SimUtil.EnergyFlowToTemperatureDelta(smi.master.BaseGenerationKW, component.Element.specificHeatCapacity, component.Mass);
    float temperatureDelta2 = SimUtil.EnergyFlowToTemperatureDelta(smi.master.WarmingKW, component.Element.specificHeatCapacity, component.Mass);
    float num1 = smi.IdealTemperature - smi.BodyTemperature;
    float num2 = 1f;
    if ((double) temperatureDelta2 + (double) temperatureDelta1 > (double) num1)
      num2 = Mathf.Max(0.0f, num1 - temperatureDelta1) / temperatureDelta2;
    smi.bodyRegulator.SetValue(temperatureDelta2 * num2);
    if (smi.master.complexity != WarmBlooded.ComplexityType.FullHomeostasis)
      return;
    smi.burningCalories.SetValue((float) (-(double) smi.master.WarmingKW * (double) num2 * 1000.0) / smi.master.KCal2Joules);
  }

  protected override void OnPrefabInit()
  {
    this.temperature = Db.Get().Amounts.Get(this.TemperatureAmountName).Lookup(this.gameObject);
    this.primaryElement = this.GetComponent<PrimaryElement>();
  }

  protected override void OnSpawn() => this.smi.StartSM();

  public void SetTemperatureImmediate(float t) => this.temperature.value = t;

  public enum ComplexityType
  {
    SimpleHeatProduction,
    HomeostasisWithoutCaloriesImpact,
    FullHomeostasis,
  }

  public class StatesInstance : 
    GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.GameInstance
  {
    public AttributeModifier baseTemperatureModification;
    public AttributeModifier bodyRegulator;
    public AttributeModifier burningCalories;

    public StatesInstance(WarmBlooded smi)
      : base(smi)
    {
      this.baseTemperatureModification = new AttributeModifier(this.master.TemperatureAmountName + "Delta", 0.0f, this.master.BaseTemperatureModifierDescription, uiOnly: true, is_readonly: false);
      this.master.GetAttributes().Add(this.baseTemperatureModification);
      if (this.master.complexity != WarmBlooded.ComplexityType.SimpleHeatProduction)
      {
        this.bodyRegulator = new AttributeModifier(this.master.TemperatureAmountName + "Delta", 0.0f, this.master.BodyRegulatorModifierDescription, uiOnly: true, is_readonly: false);
        this.master.GetAttributes().Add(this.bodyRegulator);
      }
      if (this.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
      {
        this.burningCalories = new AttributeModifier("CaloriesDelta", 0.0f, this.master.CaloriesModifierDescription, is_readonly: false);
        this.master.GetAttributes().Add(this.burningCalories);
      }
      this.master.SetTemperatureImmediate(this.IdealTemperature);
    }

    public float IdealTemperature => this.master.IdealTemperature;

    public float TemperatureDelta => this.bodyRegulator.Value;

    public float BodyTemperature => this.master.primaryElement.Temperature;

    public bool IsSimpleHeatProducer()
    {
      return this.master.complexity == WarmBlooded.ComplexityType.SimpleHeatProduction;
    }

    public bool IsHot() => (double) this.BodyTemperature > (double) this.IdealTemperature;

    public bool IsCold() => (double) this.BodyTemperature < (double) this.IdealTemperature;
  }

  public class States : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded>
  {
    public WarmBlooded.States.AliveState alive;
    public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State dead;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.alive.normal;
      this.root.TagTransition(GameTags.Dead, this.dead).Enter((StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State.Callback) (smi =>
      {
        PrimaryElement component1 = smi.master.GetComponent<PrimaryElement>();
        float temperatureDelta = SimUtil.EnergyFlowToTemperatureDelta(smi.master.BaseGenerationKW, component1.Element.specificHeatCapacity, component1.Mass);
        smi.baseTemperatureModification.SetValue(temperatureDelta);
        CreatureSimTemperatureTransfer component2 = smi.master.GetComponent<CreatureSimTemperatureTransfer>();
        component2.NonSimTemperatureModifiers.Add(smi.baseTemperatureModification);
        if (smi.IsSimpleHeatProducer())
          return;
        component2.NonSimTemperatureModifiers.Add(smi.bodyRegulator);
      }));
      this.alive.normal.Transition(this.alive.cold.transition, new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold)).Transition(this.alive.hot.transition, new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot));
      this.alive.cold.transition.ScheduleGoTo(3f, (StateMachine.BaseState) this.alive.cold.regulating).Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold)));
      this.alive.cold.regulating.Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold))).Update("ColdRegulating", new Action<WarmBlooded.StatesInstance, float>(WarmBlooded.CoolingRegulator)).Exit((StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State.Callback) (smi =>
      {
        smi.bodyRegulator.SetValue(0.0f);
        if (smi.master.complexity != WarmBlooded.ComplexityType.FullHomeostasis)
          return;
        smi.burningCalories.SetValue(0.0f);
      }));
      this.alive.hot.transition.ScheduleGoTo(3f, (StateMachine.BaseState) this.alive.hot.regulating).Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot)));
      this.alive.hot.regulating.Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot))).Update("WarmRegulating", new Action<WarmBlooded.StatesInstance, float>(WarmBlooded.WarmingRegulator)).Exit((StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State.Callback) (smi => smi.bodyRegulator.SetValue(0.0f)));
      this.dead.Enter((StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State.Callback) (smi => smi.master.enabled = false));
    }

    public class RegulatingState : 
      GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State
    {
      public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State transition;
      public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State regulating;
    }

    public class AliveState : 
      GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State
    {
      public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State normal;
      public WarmBlooded.States.RegulatingState cold;
      public WarmBlooded.States.RegulatingState hot;
    }
  }
}
