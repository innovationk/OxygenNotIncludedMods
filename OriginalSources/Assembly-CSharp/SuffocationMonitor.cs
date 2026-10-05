// Decompiled with JetBrains decompiler
// Type: SuffocationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using TUNING;

#nullable disable
public class SuffocationMonitor : 
  GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>
{
  public StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.FloatParameter timeUntilDeath;
  public SuffocationMonitor.SatisfiedState satisfied;
  public SuffocationMonitor.NoOxygenState noOxygen;
  public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State death;
  public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State dead;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.TagTransition(GameTags.Dead, this.dead);
    this.satisfied.DefaultState(this.satisfied.normal).ToggleAttributeModifier("Breathing", (Func<SuffocationMonitor.Instance, AttributeModifier>) (smi => smi.increaseBreathModifier)).EventTransition(GameHashes.OxygenBreatherHasAirChanged, (GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State) this.noOxygen, (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.Transition.ConditionCallback) (smi => !smi.CanBreath())).Transition((GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State) this.noOxygen, (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.Transition.ConditionCallback) (smi => !smi.CanBreath()));
    this.satisfied.normal.Transition(this.satisfied.low, (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.Transition.ConditionCallback) (smi => smi.oxygenBreather.IsLowOxygen()));
    this.satisfied.low.Transition(this.satisfied.normal, (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.Transition.ConditionCallback) (smi => !smi.oxygenBreather.IsLowOxygen())).ToggleEffect("LowOxygen");
    this.noOxygen.EventTransition(GameHashes.OxygenBreatherHasAirChanged, (GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State) this.satisfied, (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.Transition.ConditionCallback) (smi => smi.CanBreath())).TagTransition(GameTags.RecoveringBreath, (GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State) this.satisfied).ToggleExpression(Db.Get().Expressions.Suffocate, (Func<SuffocationMonitor.Instance, bool>) (smi => smi.IsBreathDepletingSignificantly() || smi.IsBreathLow())).ToggleAttributeModifier("Holding Breath", (Func<SuffocationMonitor.Instance, AttributeModifier>) (smi => smi.decreaseBreathModifier)).ToggleTag(GameTags.NoOxygen).DefaultState(this.noOxygen.holdingbreath);
    this.noOxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath).Transition(this.noOxygen.suffocating, (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsSuffocating()));
    this.noOxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating).Transition(this.noOxygen.incapacitated, (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.Transition.ConditionCallback) (smi => smi.HasSuffocated()));
    double num;
    this.noOxygen.incapacitated.Enter((StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State.Callback) (smi => num = (double) smi.sm.timeUntilDeath.Set(smi.def.timeBeforeDeath, smi))).ToggleRecurringChore((Func<SuffocationMonitor.Instance, Chore>) (smi => (Chore) new BeIncapacitatedSuffocatingChore(smi.master))).ToggleUrge(Db.Get().Urges.BeIncapacitated).ToggleTag(GameTags.SuffocatingIncapacitated).Update((System.Action<SuffocationMonitor.Instance, float>) ((smi, dt) => this.UpdateTimeUntilDeath(smi, dt))).ParamTransition<float>((StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.Parameter<float>) this.timeUntilDeath, this.death, GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.IsLTZero).EventTransition(GameHashes.IncapacitationRecovery, (GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State) this.satisfied);
    this.death.Enter("SuffocationDeath", (StateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State.Callback) (smi => smi.Kill()));
    this.dead.DoNothing();
  }

  private void UpdateTimeUntilDeath(SuffocationMonitor.Instance smi, float dt)
  {
    double num = (double) smi.sm.timeUntilDeath.Delta(dt * -1f, smi);
  }

  public class Def : StateMachine.BaseDef
  {
    public float timeBeforeDeath = 120f;
  }

  public class NoOxygenState : 
    GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State
  {
    public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State holdingbreath;
    public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State suffocating;
    public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State incapacitated;
  }

  public class SatisfiedState : 
    GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State
  {
    public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State normal;
    public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State low;
  }

  public new class Instance : 
    GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.GameInstance
  {
    private AmountInstance breath;
    public AttributeModifier increaseBreathModifier;
    public AttributeModifier decreaseBreathModifier;

    public OxygenBreather oxygenBreather { get; private set; }

    public Instance(IStateMachineTarget master, SuffocationMonitor.Def def)
      : base(master, def)
    {
      this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
      Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
      float breathRate = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE;
      this.increaseBreathModifier = new AttributeModifier(deltaAttribute.Id, breathRate, (string) DUPLICANTS.MODIFIERS.BREATHING.NAME);
      this.decreaseBreathModifier = new AttributeModifier(deltaAttribute.Id, -breathRate, (string) DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME);
      this.oxygenBreather = this.GetComponent<OxygenBreather>();
    }

    public bool IsBreathDepletingSignificantly()
    {
      return (double) this.breath.deltaAttribute.GetTotalValue() <= -(double) DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE * 0.5;
    }

    public bool IsBreathLow()
    {
      return (double) this.breath.value <= (double) DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
    }

    public bool CanBreath()
    {
      return this.oxygenBreather.prefabID.HasTag(GameTags.RecoveringBreath) || this.oxygenBreather.prefabID.HasTag(GameTags.InTransitTube) || this.oxygenBreather.HasOxygen;
    }

    public bool HasSuffocated() => (double) this.breath.value <= 0.0;

    public bool IsSuffocating()
    {
      return (double) this.breath.deltaAttribute.GetTotalValue() <= 0.0 && (double) this.breath.value <= (double) DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
    }

    public float GetTimeUntilDeath(SuffocationMonitor.Instance smi)
    {
      return smi.sm.timeUntilDeath.Get(smi);
    }

    public void Kill()
    {
      this.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
    }
  }
}
