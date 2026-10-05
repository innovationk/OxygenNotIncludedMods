// Decompiled with JetBrains decompiler
// Type: BionicOilMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;

#nullable disable
public class BionicOilMonitor : 
  GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>
{
  public static Dictionary<SimHashes, Effect> LUBRICANT_TYPE_EFFECT = new Dictionary<SimHashes, Effect>()
  {
    [SimHashes.Tallow] = BionicOilMonitor.CreateFreshOilEffectVariation(SimHashes.Tallow.ToString(), -0.0166666675f, 3f),
    [SimHashes.CrudeOil] = BionicOilMonitor.CreateFreshOilEffectVariation(SimHashes.CrudeOil.ToString(), -0.0166666675f, 3f),
    [SimHashes.PhytoOil] = BionicOilMonitor.CreateFreshOilEffectVariation(SimHashes.PhytoOil.ToString(), -0.008333334f, 2f)
  };
  public const float OIL_CAPACITY = 200f;
  public const float OIL_TANK_DURATION = 6000f;
  public const float OIL_REFILL_TRESHOLD = 0.2f;
  public const string NO_OIL_EFFECT_NAME_MINOR = "NoLubricationMinor";
  public const string NO_OIL_EFFECT_NAME_MAJOR = "NoLubricationMajor";
  public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State offline;
  public BionicOilMonitor.OnlineStates online;
  public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilFilledSignal;
  public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilRanOutSignal;
  public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OilValueChanged;
  public StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Signal OnClosestSolidLubricantChangedSignal;

  private static Effect CreateFreshOilEffectVariation(
    string id,
    float stressBonus,
    float moralBonus)
  {
    Effect oilEffectVariation = new Effect("FreshOil_" + id, (string) DUPLICANTS.MODIFIERS.FRESHOIL.NAME, (string) DUPLICANTS.MODIFIERS.FRESHOIL.TOOLTIP, 4800f, true, true, false);
    oilEffectVariation.Add(new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, moralBonus, (string) DUPLICANTS.MODIFIERS.FRESHOIL.NAME));
    oilEffectVariation.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, stressBonus, (string) DUPLICANTS.MODIFIERS.FRESHOIL.NAME));
    return oilEffectVariation;
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.offline;
    this.root.Update(new System.Action<BionicOilMonitor.Instance, float>(BionicOilMonitor.OilAmountInstanceWatcherUpdate)).Exit(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.RemoveBaseOilDeltaModifier));
    this.offline.EventTransition(GameHashes.BionicOnline, (GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State) this.online, new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.IsBionicOnline)).Enter(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.RemoveBaseOilDeltaModifier));
    this.online.EventTransition(GameHashes.BionicOffline, this.offline, GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Not(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.IsBionicOnline))).Enter(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.AddBaseOilDeltaModifier)).DefaultState(this.online.idle).Enter(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.EnableSolidLubricationSensor)).Exit(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback(BionicOilMonitor.DisableSolidLubricationSensor));
    this.online.idle.EnterTransition((GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State) this.online.seeking, new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.WantsOilChange)).OnSignal(this.OilValueChanged, (GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State) this.online.seeking, new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Parameter<StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.SignalParameter>.Callback(BionicOilMonitor.WantsOilChange));
    this.online.seeking.OnSignal(this.OilFilledSignal, this.online.idle).OnSignal(this.OilValueChanged, this.online.idle, new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Parameter<StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.SignalParameter>.Callback(BionicOilMonitor.HasDecentAmountOfOil)).DefaultState(this.online.seeking.hasOil).ToggleThought(Db.Get().Thoughts.RefillOilDesire).ToggleUrge(Db.Get().Urges.OilRefill).ToggleChore((Func<BionicOilMonitor.Instance, Chore>) (smi => (Chore) new UseSolidLubricantChore(smi.master)), this.online.idle);
    this.online.seeking.hasOil.EnterTransition(this.online.seeking.noOil, GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Not(new StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.Transition.ConditionCallback(BionicOilMonitor.HasAnyAmountOfOil))).OnSignal(this.OilRanOutSignal, this.online.seeking.noOil).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicWantsOilChange);
    this.online.seeking.noOil.Enter((StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback) (smi => smi.currentNoLubricationEffectApplied = smi.effects.Add(smi.GetEffect(), false).effect.IdHash)).Exit((StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State.Callback) (smi => smi.effects.Remove(smi.currentNoLubricationEffectApplied))).ToggleReactable(new Func<BionicOilMonitor.Instance, Reactable>(BionicOilMonitor.GrindingGearsReactable)).EventTransition(GameHashes.AssignedRoleChanged, this.online.seeking.hasOil);
  }

  public static bool IsBionicOnline(BionicOilMonitor.Instance smi) => smi.IsOnline;

  public static bool HasAnyAmountOfOil(BionicOilMonitor.Instance smi)
  {
    return (double) smi.CurrentOilMass > 0.0;
  }

  public static bool HasDecentAmountOfOil(BionicOilMonitor.Instance smi)
  {
    return BionicOilMonitor.HasDecentAmountOfOil(smi, (StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.SignalParameter) null);
  }

  public static bool HasDecentAmountOfOil(
    BionicOilMonitor.Instance smi,
    StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.SignalParameter param)
  {
    return (double) smi.CurrentOilPercentage > 0.20000000298023224;
  }

  public static bool WantsOilChange(BionicOilMonitor.Instance smi)
  {
    return BionicOilMonitor.WantsOilChange(smi, (StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.SignalParameter) null);
  }

  public static bool WantsOilChange(
    BionicOilMonitor.Instance smi,
    StateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.SignalParameter param)
  {
    return (double) smi.CurrentOilPercentage <= 0.20000000298023224;
  }

  public static void AddBaseOilDeltaModifier(BionicOilMonitor.Instance smi)
  {
    smi.SetBaseDeltaModifierActiveState(true);
  }

  public static void RemoveBaseOilDeltaModifier(BionicOilMonitor.Instance smi)
  {
    smi.SetBaseDeltaModifierActiveState(false);
  }

  public static void OilAmountInstanceWatcherUpdate(BionicOilMonitor.Instance smi, float dt)
  {
    float amountMassRecorded = smi.LastOilAmountMassRecorded;
    float delta = smi.CurrentOilMass - amountMassRecorded;
    if ((double) delta == 0.0)
      return;
    smi.LastOilAmountMassRecorded = smi.CurrentOilMass;
    if (!smi.HasOil)
      smi.ReportOilRanOut();
    smi.ReportOilValueChanged(delta);
  }

  public static void EnableSolidLubricationSensor(BionicOilMonitor.Instance smi)
  {
    smi.SetSolidLubricationSensorActiveState(true);
  }

  public static void DisableSolidLubricationSensor(BionicOilMonitor.Instance smi)
  {
    smi.SetSolidLubricationSensorActiveState(false);
  }

  private static Reactable GrindingGearsReactable(BionicOilMonitor.Instance smi)
  {
    return smi.GetGrindingGearReactable();
  }

  public static void ApplyLubricationEffects(Effects targetBionicEffects, SimHashes lubricant)
  {
    foreach (SimHashes key in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Keys)
    {
      if (BionicOilMonitor.LUBRICANT_TYPE_EFFECT.ContainsKey(key))
      {
        Effect effect = BionicOilMonitor.LUBRICANT_TYPE_EFFECT[key];
        if (lubricant == key)
          targetBionicEffects.Add(effect, true);
        else
          targetBionicEffects.Remove(effect);
      }
    }
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class WantsOilChangeState : 
    GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State
  {
    public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State hasOil;
    public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State noOil;
  }

  public class OnlineStates : 
    GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State
  {
    public GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.State idle;
    public BionicOilMonitor.WantsOilChangeState seeking;
  }

  public new class Instance : 
    GameStateMachine<BionicOilMonitor, BionicOilMonitor.Instance, IStateMachineTarget, BionicOilMonitor.Def>.GameInstance
  {
    public float LastOilAmountMassRecorded = -1f;
    public System.Action<float> OnOilValueChanged;
    private BionicBatteryMonitor.Instance batterySMI;
    [MyCmpGet]
    private MinionResume resume;
    [MyCmpGet]
    public Effects effects;
    public HashedString currentNoLubricationEffectApplied;
    private AttributeModifier BaseOilDeltaModifier = new AttributeModifier(Db.Get().Amounts.BionicOil.deltaAttribute.Id, -0.0333333351f, BionicMinionConfig.NAME);
    private ClosestLubricantSensor closestSolidLubricantSensor;

    public bool IsOnline => this.batterySMI != null && this.batterySMI.IsOnline;

    public bool HasOil => (double) this.CurrentOilMass > 0.0;

    public float CurrentOilPercentage => this.CurrentOilMass / this.oilAmount.GetMax();

    public float CurrentOilMass => this.oilAmount != null ? this.oilAmount.value : 0.0f;

    public AmountInstance oilAmount { private set; get; }

    public Instance(IStateMachineTarget master, BionicOilMonitor.Def def)
      : base(master, def)
    {
      this.oilAmount = Db.Get().Amounts.BionicOil.Lookup(this.gameObject);
      this.batterySMI = this.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
    }

    public override void StartSM()
    {
      this.closestSolidLubricantSensor = this.GetComponent<Sensors>().GetSensor<ClosestLubricantSensor>();
      ClosestLubricantSensor solidLubricantSensor = this.closestSolidLubricantSensor;
      solidLubricantSensor.OnItemChanged = solidLubricantSensor.OnItemChanged + new System.Action<Pickupable>(this.OnClosestSolidLubricantChanged);
      this.LastOilAmountMassRecorded = this.CurrentOilMass;
      base.StartSM();
    }

    public string GetEffect()
    {
      return !this.resume.HasPerk(Db.Get().SkillPerks.EfficientBionicGears) ? "NoLubricationMajor" : "NoLubricationMinor";
    }

    private void ReportOilTankFilled() => this.sm.OilFilledSignal.Trigger(this);

    public void ReportOilRanOut() => this.sm.OilRanOutSignal.Trigger(this);

    public void ReportOilValueChanged(float delta)
    {
      this.sm.OilValueChanged.Trigger(this);
      System.Action<float> onOilValueChanged = this.OnOilValueChanged;
      if (onOilValueChanged == null)
        return;
      onOilValueChanged(delta);
    }

    public void SetOilMassValue(float value)
    {
      double num = (double) this.oilAmount.SetValue(value);
    }

    public void SetBaseDeltaModifierActiveState(bool isActive)
    {
      MinionModifiers component = this.GetComponent<MinionModifiers>();
      if (isActive)
      {
        bool flag = false;
        int count = component.attributes.Get(this.BaseOilDeltaModifier.AttributeId).Modifiers.Count;
        for (int i = 0; i < count; ++i)
        {
          if (component.attributes.Get(this.BaseOilDeltaModifier.AttributeId).Modifiers[i] == this.BaseOilDeltaModifier)
          {
            flag = true;
            break;
          }
        }
        if (flag)
          return;
        component.attributes.Add(this.BaseOilDeltaModifier);
      }
      else
        component.attributes.Remove(this.BaseOilDeltaModifier);
    }

    public void RefillOil(float amount)
    {
      double num = (double) this.oilAmount.SetValue(this.CurrentOilMass + amount);
      this.ReportOilTankFilled();
    }

    private void OnClosestSolidLubricantChanged(Pickupable newItem)
    {
      this.sm.OnClosestSolidLubricantChangedSignal.Trigger(this);
    }

    public Pickupable GetClosestSolidLubricant() => this.closestSolidLubricantSensor.GetItem();

    public void SetSolidLubricationSensorActiveState(bool shouldItBeActive)
    {
      this.closestSolidLubricantSensor.SetActive(shouldItBeActive);
      if (!shouldItBeActive)
        return;
      this.closestSolidLubricantSensor.Update();
    }

    public Reactable GetGrindingGearReactable()
    {
      SelfEmoteReactable grindingGearReactable = new SelfEmoteReactable(this.master.gameObject, (HashedString) Db.Get().Emotes.Minion.GrindingGears.Id, Db.Get().ChoreTypes.EmoteHighPriority, localCooldown: 10f);
      grindingGearReactable.SetEmote(Db.Get().Emotes.Minion.GrindingGears);
      grindingGearReactable.SetThought(Db.Get().Thoughts.RefillOilDesire);
      grindingGearReactable.preventChoreInterruption = true;
      return (Reactable) grindingGearReactable;
    }
  }
}
