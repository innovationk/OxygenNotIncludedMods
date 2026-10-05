// Decompiled with JetBrains decompiler
// Type: GunkMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class GunkMonitor : 
  GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>
{
  public const float BIONIC_RADS_REMOVED_WHEN_PEE = 300f;
  public static readonly float GUNK_CAPACITY = 80f;
  public const string GUNK_FULL_EFFECT_NAME = "GunkSick";
  public const string GUNK_HUNGOVER_EFFECT_NAME = "GunkHungover";
  public static SimHashes GunkElement = SimHashes.LiquidGunk;
  public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State idle;
  public GunkMonitor.MildUrgeStates mildUrge;
  public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State criticalUrge;
  public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State cantHold;
  public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State emptyRemaining;
  public StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Signal gunkValueChangedSignal;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.idle;
    this.root.Update(new System.Action<GunkMonitor.Instance, float>(GunkMonitor.GunkAmountWatcherUpdate));
    this.idle.OnSignal(this.gunkValueChangedSignal, (GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State) this.mildUrge, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Parameter<StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter>.Callback(GunkMonitor.IsGunkLevelsOverMildUrgeThreshold));
    this.mildUrge.OnSignal(this.gunkValueChangedSignal, this.criticalUrge, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Parameter<StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter>.Callback(GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold)).OnSignal(this.gunkValueChangedSignal, this.idle, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Parameter<StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter>.Callback(GunkMonitor.DoesNotWantToExpellGunk)).DefaultState(this.mildUrge.prevented);
    this.mildUrge.prevented.ScheduleChange(this.mildUrge.allowed, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.ScheduleAllowsExpelling));
    this.mildUrge.allowed.ScheduleChange(this.mildUrge.prevented, GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Not(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.ScheduleAllowsExpelling))).ToggleUrge(Db.Get().Urges.Pee).ToggleUrge(Db.Get().Urges.GunkPee);
    this.criticalUrge.OnSignal(this.gunkValueChangedSignal, this.idle, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Parameter<StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter>.Callback(GunkMonitor.DoesNotWantToExpellGunk)).OnSignal(this.gunkValueChangedSignal, (GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State) this.mildUrge, (StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Parameter<StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter>.Callback) ((smi, param) => !GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold(smi))).OnSignal(this.gunkValueChangedSignal, this.cantHold, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Parameter<StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter>.Callback(GunkMonitor.CanNotHoldGunkAnymore)).ToggleUrge(Db.Get().Urges.GunkPee).ToggleUrge(Db.Get().Urges.Pee).ToggleEffect("GunkSick").ToggleExpression(Db.Get().Expressions.FullBladder).ToggleThought(Db.Get().Thoughts.ExpellGunkDesire).ToggleAnims("anim_loco_walk_slouch_kanim").ToggleAnims("anim_idle_slouch_kanim");
    this.cantHold.ToggleUrge(Db.Get().Urges.GunkPee).ToggleThought(Db.Get().Thoughts.ExpellingGunk).ToggleChore((Func<GunkMonitor.Instance, Chore>) (smi => (Chore) new BionicGunkSpillChore(smi.master)), this.emptyRemaining);
    this.emptyRemaining.Enter(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State.Callback(GunkMonitor.ExpellAllGunk)).Enter(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State.Callback(GunkMonitor.ApplyGunkHungoverEffect)).GoTo(this.idle);
  }

  public static bool IsGunkLevelsOverCriticalUrgeThreshold(
    GunkMonitor.Instance smi,
    StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter param)
  {
    return GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold(smi);
  }

  public static bool IsGunkLevelsOverCriticalUrgeThreshold(GunkMonitor.Instance smi)
  {
    return (double) smi.CurrentGunkPercentage >= (double) smi.def.DesperetlySeekForGunkToiletTreshold;
  }

  public static bool IsGunkLevelsOverMildUrgeThreshold(
    GunkMonitor.Instance smi,
    StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter param)
  {
    return GunkMonitor.IsGunkLevelsOverMildUrgeThreshold(smi);
  }

  public static bool IsGunkLevelsOverMildUrgeThreshold(GunkMonitor.Instance smi)
  {
    return (double) smi.CurrentGunkPercentage >= (double) smi.def.SeekForGunkToiletTreshold_InSchedule;
  }

  public static bool ScheduleAllowsExpelling(GunkMonitor.Instance smi)
  {
    return smi.DoesCurrentScheduleAllowsGunkToilet;
  }

  public static bool DoesNotWantToExpellGunk(
    GunkMonitor.Instance smi,
    StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter p)
  {
    return !GunkMonitor.IsGunkLevelsOverMildUrgeThreshold(smi);
  }

  public static bool CanNotHoldGunkAnymore(
    GunkMonitor.Instance smi,
    StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.SignalParameter p)
  {
    return GunkMonitor.CanNotHoldGunkAnymore(smi);
  }

  public static bool CanNotHoldGunkAnymore(GunkMonitor.Instance smi) => smi.IsGunkBuildupAtMax;

  public static void ExpellAllGunk(GunkMonitor.Instance smi) => smi.ExpellAllGunk();

  public static void ApplyGunkHungoverEffect(GunkMonitor.Instance smi)
  {
    smi.GetComponent<Effects>().Add("GunkHungover", true);
  }

  public static void GunkAmountWatcherUpdate(GunkMonitor.Instance smi, float dt)
  {
    smi.GunkAmountWatcherUpdate(dt);
  }

  public class Def : StateMachine.BaseDef
  {
    public float SeekForGunkToiletTreshold_InSchedule = 0.6f;
    public float DesperetlySeekForGunkToiletTreshold = 0.9f;
  }

  public class MildUrgeStates : 
    GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State
  {
    public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State allowed;
    public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State prevented;
  }

  public new class Instance : 
    GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.GameInstance
  {
    private float LastAmountOfGunkObserved;
    private BionicOilMonitor.Instance oilMonitor;
    private AmountInstance gunkAmount;
    private AmountInstance bodyTemperature;
    private Schedulable schedulable;

    public bool HasGunk => (double) this.CurrentGunkMass > 0.0;

    public bool IsGunkBuildupAtMax => (double) this.CurrentGunkPercentage >= 1.0;

    public float CurrentGunkMass => this.gunkAmount != null ? this.gunkAmount.value : 0.0f;

    public float CurrentGunkPercentage => this.CurrentGunkMass / this.gunkAmount.GetMax();

    public bool DoesCurrentScheduleAllowsGunkToilet
    {
      get
      {
        return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Eat) || this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Hygiene);
      }
    }

    public Instance(IStateMachineTarget master, GunkMonitor.Def def)
      : base(master, def)
    {
      this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(this.gameObject);
      this.gunkAmount = Db.Get().Amounts.BionicGunk.Lookup(this.gameObject);
      this.schedulable = this.GetComponent<Schedulable>();
    }

    public override void StartSM()
    {
      this.oilMonitor = this.gameObject.GetSMI<BionicOilMonitor.Instance>();
      this.oilMonitor.OnOilValueChanged += new System.Action<float>(this.OnOilValueChanged);
      this.LastAmountOfGunkObserved = this.CurrentGunkMass;
      base.StartSM();
    }

    public void GunkAmountWatcherUpdate(float dt)
    {
      if ((double) this.LastAmountOfGunkObserved == (double) this.CurrentGunkMass)
        return;
      this.LastAmountOfGunkObserved = this.CurrentGunkMass;
      this.sm.gunkValueChangedSignal.Trigger(this);
    }

    protected override void OnCleanUp()
    {
      if (this.oilMonitor != null)
        this.oilMonitor.OnOilValueChanged -= new System.Action<float>(this.OnOilValueChanged);
      base.OnCleanUp();
    }

    private void OnOilValueChanged(float delta)
    {
      this.SetGunkMassValue(Mathf.Clamp(this.CurrentGunkMass + ((double) delta < 0.0 ? Mathf.Abs(delta) : 0.0f), 0.0f, this.gunkAmount.GetMax()));
    }

    public void SetGunkMassValue(float value)
    {
      double currentGunkMass = (double) this.CurrentGunkMass;
      double num = (double) this.gunkAmount.SetValue(value);
      this.LastAmountOfGunkObserved = this.CurrentGunkMass;
      this.sm.gunkValueChangedSignal.Trigger(this);
    }

    public void ExpellGunk(float mass, Storage targetStorage = null)
    {
      if (!this.HasGunk)
        return;
      double currentGunkMass = (double) this.CurrentGunkMass;
      float mass1 = Mathf.Max(Mathf.Min(mass, this.CurrentGunkMass), Mathf.Epsilon);
      int cell = Grid.PosToCell(this.transform.position);
      byte index = Db.Get().Diseases.GetIndex((HashedString) DUPLICANTSTATS.BIONICS.Secretions.PEE_DISEASE);
      float num1 = mass1 / GunkMonitor.GUNK_CAPACITY;
      if ((UnityEngine.Object) targetStorage != (UnityEngine.Object) null)
      {
        targetStorage.AddLiquid(GunkMonitor.GunkElement, mass1, this.bodyTemperature.value, index, (int) ((double) DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * (double) num1));
      }
      else
      {
        Equippable equippable = this.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
        if ((UnityEngine.Object) equippable != (UnityEngine.Object) null)
          equippable.GetComponent<Storage>().AddLiquid(GunkMonitor.GunkElement, mass1, this.bodyTemperature.value, index, (int) ((double) DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * (double) num1));
        else
          SimMessages.AddRemoveSubstance(cell, GunkMonitor.GunkElement, CellEventLogger.Instance.Vomit, mass1, this.bodyTemperature.value, index, (int) ((double) DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * (double) num1));
      }
      if (Sim.IsRadiationEnabled())
      {
        MinionIdentity component = this.transform.GetComponent<MinionIdentity>();
        AmountInstance amountInstance = Db.Get().Amounts.RadiationBalance.Lookup((Component) component);
        RadiationMonitor.Instance smi = component.GetSMI<RadiationMonitor.Instance>();
        float d = Math.Min(amountInstance.value, 300f * (DUPLICANTSTATS.STANDARD.BaseStats.BLADDER_INCREASE_PER_SECOND / DUPLICANTSTATS.BIONICS.BaseStats.BLADDER_INCREASE_PER_SECOND) * smi.difficultySettingMod * num1);
        if ((double) d >= 1.0)
          PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Math.Floor((double) d).ToString() + (string) UI.UNITSUFFIXES.RADIATION.RADS, component.transform, Vector3.up * 2f);
        double num2 = (double) amountInstance.ApplyDelta(-d);
      }
      this.SetGunkMassValue(Mathf.Clamp(this.CurrentGunkMass - mass1, 0.0f, this.gunkAmount.GetMax()));
    }

    public void ExpellAllGunk(Storage targetStorage = null)
    {
      this.ExpellGunk(this.CurrentGunkMass, targetStorage);
    }
  }
}
