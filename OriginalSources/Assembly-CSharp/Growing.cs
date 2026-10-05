// Decompiled with JetBrains decompiler
// Type: Growing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using TemplateClasses;
using UnityEngine;

#nullable disable
public class Growing : 
  StateMachineComponent<Growing.StatesInstance>,
  IGameObjectEffectDescriptor,
  IManageGrowingStates
{
  public Func<GameObject, bool> CustomGrowStallCondition_IsStalled;
  public float MaxMaturityValuePercentageToSpawnWith = 1f;
  public float GROWTH_RATE = 1f / 600f;
  public float WILD_GROWTH_RATE = 0.000416666677f;
  public bool shouldGrowOld = true;
  public float maxAge = 2400f;
  private AmountInstance maturity;
  private AmountInstance oldAge;
  [MyCmpGet]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpReq]
  private Modifiers modifiers;
  [MyCmpReq]
  private ReceptacleMonitor rm;
  private static readonly EventSystem.IntraObjectHandler<Growing> OnNewGameSpawnDelegate = new EventSystem.IntraObjectHandler<Growing>((Action<Growing, object>) ((component, data) => component.OnNewGameSpawn(data)));
  private static readonly EventSystem.IntraObjectHandler<Growing> ResetGrowthDelegate = new EventSystem.IntraObjectHandler<Growing>((Action<Growing, object>) ((component, data) => component.ResetGrowth(data)));

  protected override void OnPrefabInit()
  {
    Amounts amounts = this.gameObject.GetAmounts();
    this.maturity = amounts.Get(Db.Get().Amounts.Maturity);
    this.oldAge = amounts.Add(new AmountInstance(Db.Get().Amounts.OldAge, this.gameObject));
    this.oldAge.maxAttribute.ClearModifiers();
    this.oldAge.maxAttribute.Add(new AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute.Id, this.maxAge));
    base.OnPrefabInit();
    this.Subscribe<Growing>(1119167081, Growing.OnNewGameSpawnDelegate);
    this.Subscribe<Growing>(1272413801, Growing.ResetGrowthDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    this.gameObject.AddTag(GameTags.GrowingPlant);
  }

  private void OnNewGameSpawn(object data)
  {
    Prefab prefab = (Prefab) data;
    if (prefab != null && prefab.amounts != null)
    {
      foreach (Prefab.template_amount_value amount in prefab.amounts)
      {
        if (amount.id == this.maturity.amount.Id && (double) amount.value == (double) this.GetMaxMaturity())
          return;
      }
    }
    if (this.maturity == null)
      KCrashReporter.ReportDevNotification("Maturity.OnNewGameSpawn", Environment.StackTrace);
    double num = (double) this.maturity.SetValue(this.maturity.maxAttribute.GetTotalValue() * this.MaxMaturityValuePercentageToSpawnWith * UnityEngine.Random.Range(0.0f, 1f));
  }

  public void OverrideMaturityLevel(float percent)
  {
    double num = (double) this.maturity.SetValue(this.maturity.GetMax() * percent);
  }

  public bool ReachedNextHarvest() => (double) this.PercentOfCurrentHarvest() >= 1.0;

  public bool IsGrown() => (double) this.maturity.value == (double) this.maturity.GetMax();

  public bool CanGrow() => !this.IsGrown();

  public bool IsGrowing() => (double) this.maturity.GetDelta() > 0.0;

  public void ClampGrowthToHarvest() => this.maturity.value = this.maturity.GetMax();

  public float GetMaxMaturity() => this.maturity.GetMax();

  public float PercentOfCurrentHarvest() => this.maturity.value / this.maturity.GetMax();

  public float TimeUntilNextHarvest()
  {
    return (this.maturity.GetMax() - this.maturity.value) / this.maturity.GetDelta();
  }

  public float DomesticGrowthTime() => this.maturity.GetMax() / this.smi.baseGrowingRate.Value;

  public float WildGrowthTime() => this.maturity.GetMax() / this.smi.wildGrowingRate.Value;

  public float PercentGrown() => this.maturity.value / this.maturity.GetMax();

  public void ResetGrowth(object data = null) => this.maturity.value = 0.0f;

  public float PercentOldAge()
  {
    return !this.shouldGrowOld ? 0.0f : this.oldAge.value / this.oldAge.GetMax();
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    Klei.AI.Attribute maxAttribute = Db.Get().Amounts.Maturity.maxAttribute;
    descriptors.Add(new Descriptor(go.GetComponent<Modifiers>().GetPreModifiedAttributeDescription(maxAttribute), go.GetComponent<Modifiers>().GetPreModifiedAttributeToolTip(maxAttribute), Descriptor.DescriptorType.Requirement));
    return descriptors;
  }

  public void ConsumeMass(float mass_to_consume)
  {
    float b = this.maturity.value;
    mass_to_consume = Mathf.Min(mass_to_consume, b);
    this.maturity.value -= mass_to_consume;
    this.gameObject.Trigger(-1793167409);
  }

  public void ConsumeGrowthUnits(float units_to_consume, float unit_maturity_ratio)
  {
    float num = units_to_consume / unit_maturity_ratio;
    DebugUtil.DevAssert(((double) num <= (double) this.maturity.value ? 1 : 0) != 0, $"A critter consuming a plant ({this.gameObject.GetProperName()}) tried to consumed more maturity units than there were available, available: {this.maturity.value.ToString()}, attempted amount: {num.ToString()}");
    this.maturity.value -= Mathf.Clamp(num, 0.0f, this.maturity.value);
    this.gameObject.Trigger(-1793167409);
  }

  public Crop GetCropComponent() => this.GetComponent<Crop>();

  public bool IsWildPlanted() => !this.rm.Replanted;

  public class StatesInstance : 
    GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.GameInstance
  {
    public AttributeModifier baseGrowingRate;
    public AttributeModifier wildGrowingRate;
    public AttributeModifier getOldRate;
    public Harvestable harvestable;

    public StatesInstance(Growing master)
      : base(master)
    {
      this.baseGrowingRate = new AttributeModifier(master.maturity.deltaAttribute.Id, master.GROWTH_RATE, (string) CREATURES.STATS.MATURITY.GROWING);
      this.wildGrowingRate = new AttributeModifier(master.maturity.deltaAttribute.Id, master.WILD_GROWTH_RATE, (string) CREATURES.STATS.MATURITY.GROWINGWILD);
      this.getOldRate = new AttributeModifier(master.oldAge.deltaAttribute.Id, master.shouldGrowOld ? 1f : 0.0f);
      this.harvestable = this.GetComponent<Harvestable>();
    }

    public void ModifyOldAgeGrowthRate(float newValue)
    {
      Attributes attributes = this.gameObject.GetAttributes();
      attributes.Remove(this.getOldRate);
      this.getOldRate = new AttributeModifier(this.master.oldAge.deltaAttribute.Id, newValue);
      if (!this.smi.IsInsideState((StateMachine.BaseState) this.sm.grown))
        return;
      attributes.Add(this.getOldRate);
    }

    public bool IsGrown() => this.master.IsGrown();

    public bool ReachedNextHarvest() => this.master.ReachedNextHarvest();

    public void ClampGrowthToHarvest() => this.master.ClampGrowthToHarvest();

    public bool IsWilting()
    {
      return (UnityEngine.Object) this.master.wiltCondition != (UnityEngine.Object) null && this.master.wiltCondition.IsWilting();
    }

    public bool IsStalledByCustomCondition()
    {
      bool flag = false;
      if (this.master.CustomGrowStallCondition_IsStalled != null)
        flag = this.master.CustomGrowStallCondition_IsStalled(this.master.gameObject);
      return flag;
    }

    public bool CanExitStalled()
    {
      if (this.IsWilting())
        return false;
      return this.master.CustomGrowStallCondition_IsStalled == null || !this.master.CustomGrowStallCondition_IsStalled(this.master.gameObject);
    }
  }

  public class States : GameStateMachine<Growing.States, Growing.StatesInstance, Growing>
  {
    public Growing.States.GrowingStates growing;
    public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State stalled;
    public Growing.States.GrownStates grown;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.growing;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.growing.EventTransition(GameHashes.Wilt, this.stalled, (StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.Transition.ConditionCallback) (smi => smi.IsWilting())).EventTransition(GameHashes.CropSleep, this.stalled, (StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.Transition.ConditionCallback) (smi => smi.IsStalledByCustomCondition())).EventTransition(GameHashes.ReceptacleMonitorChange, this.growing.planted, (StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.Transition.ConditionCallback) (smi => !smi.master.IsWildPlanted())).EventTransition(GameHashes.ReceptacleMonitorChange, this.growing.wild, (StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.Transition.ConditionCallback) (smi => smi.master.IsWildPlanted())).EventTransition(GameHashes.PlanterStorage, this.growing.planted, (StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.Transition.ConditionCallback) (smi => !smi.master.IsWildPlanted())).EventTransition(GameHashes.PlanterStorage, this.growing.wild, (StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.Transition.ConditionCallback) (smi => smi.master.IsWildPlanted())).TriggerOnEnter(GameHashes.Grow).Update("CheckGrown", (Action<Growing.StatesInstance, float>) ((smi, dt) =>
      {
        if (!smi.ReachedNextHarvest())
          return;
        smi.GoTo((StateMachine.BaseState) this.grown);
      }), UpdateRate.SIM_4000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.Growing, (Func<Growing.StatesInstance, object>) (smi => (object) smi.master.GetComponent<IManageGrowingStates>())).Enter((StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State.Callback) (smi =>
      {
        GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State state = smi.master.IsWildPlanted() ? this.growing.wild : this.growing.planted;
        smi.GoTo((StateMachine.BaseState) state);
      }));
      this.growing.wild.ToggleAttributeModifier("GrowingWild", (Func<Growing.StatesInstance, AttributeModifier>) (smi => smi.wildGrowingRate));
      this.growing.planted.ToggleAttributeModifier(nameof (Growing), (Func<Growing.StatesInstance, AttributeModifier>) (smi => smi.baseGrowingRate));
      this.stalled.EventTransition(GameHashes.WiltRecover, (GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State) this.growing, (StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.Transition.ConditionCallback) (smi => smi.CanExitStalled())).EventTransition(GameHashes.CropWakeUp, (GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State) this.growing, (StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.Transition.ConditionCallback) (smi => smi.CanExitStalled()));
      double num1;
      this.grown.DefaultState(this.grown.idle).TriggerOnEnter(GameHashes.Grow).Update("CheckNotGrown", (Action<Growing.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.ReachedNextHarvest())
          return;
        smi.GoTo((StateMachine.BaseState) this.growing);
      }), UpdateRate.SIM_4000ms).ToggleAttributeModifier("GettingOld", (Func<Growing.StatesInstance, AttributeModifier>) (smi => smi.getOldRate)).Enter((StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State.Callback) (smi => smi.ClampGrowthToHarvest())).Exit((StateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State.Callback) (smi => num1 = (double) smi.master.oldAge.SetValue(0.0f)));
      this.grown.idle.Update("CheckNotGrown", (Action<Growing.StatesInstance, float>) ((smi, dt) =>
      {
        if (!smi.master.shouldGrowOld || (double) smi.master.oldAge.value < (double) smi.master.oldAge.GetMax() || !(bool) (UnityEngine.Object) smi.harvestable || !smi.harvestable.CanBeHarvested)
          return;
        if ((UnityEngine.Object) smi.harvestable.harvestDesignatable != (UnityEngine.Object) null)
        {
          int num2 = smi.harvestable.harvestDesignatable.HarvestWhenReady ? 1 : 0;
          smi.harvestable.ForceCancelHarvest();
          smi.harvestable.Harvest();
          if (num2 != 0 && (UnityEngine.Object) smi.harvestable != (UnityEngine.Object) null)
            smi.harvestable.harvestDesignatable.SetHarvestWhenReady(true);
        }
        else
        {
          smi.harvestable.ForceCancelHarvest();
          smi.harvestable.Harvest();
        }
        double num3 = (double) smi.master.maturity.SetValue(0.0f);
        double num4 = (double) smi.master.oldAge.SetValue(0.0f);
      }), UpdateRate.SIM_4000ms);
    }

    public class GrowingStates : 
      GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State
    {
      public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State wild;
      public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State planted;
    }

    public class GrownStates : 
      GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State
    {
      public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State idle;
    }
  }
}
