// Decompiled with JetBrains decompiler
// Type: BionicSuffocationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using TUNING;

#nullable disable
public class BionicSuffocationMonitor : 
  GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>
{
  public BionicSuffocationMonitor.NoOxygenState noOxygen;
  public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State normal;
  public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State death;
  public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State dead;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.normal;
    this.root.TagTransition(GameTags.Dead, this.dead);
    this.normal.ToggleAttributeModifier("Breathing", (Func<BionicSuffocationMonitor.Instance, AttributeModifier>) (smi => smi.breathing)).EventTransition(GameHashes.OxygenBreatherHasAirChanged, (GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State) this.noOxygen, (StateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.Transition.ConditionCallback) (smi => !smi.IsBreathing()));
    this.noOxygen.EventTransition(GameHashes.OxygenBreatherHasAirChanged, this.normal, (StateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsBreathing())).TagTransition(GameTags.RecoveringBreath, this.normal).ToggleExpression(Db.Get().Expressions.Suffocate).ToggleAttributeModifier("Holding Breath", (Func<BionicSuffocationMonitor.Instance, AttributeModifier>) (smi => smi.holdingbreath)).ToggleTag(GameTags.NoOxygen).DefaultState(this.noOxygen.holdingbreath);
    this.noOxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath).Transition(this.noOxygen.suffocating, (StateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsSuffocating()));
    this.noOxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating).Transition(this.death, (StateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.Transition.ConditionCallback) (smi => smi.HasSuffocated()));
    this.death.Enter("SuffocationDeath", (StateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State.Callback) (smi => smi.Kill()));
    this.dead.DoNothing();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class NoOxygenState : 
    GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State
  {
    public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State holdingbreath;
    public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State suffocating;
  }

  public new class Instance : 
    GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.GameInstance
  {
    private AmountInstance breath;
    public AttributeModifier breathing;
    public AttributeModifier holdingbreath;

    public OxygenBreather oxygenBreather { get; private set; }

    public Instance(IStateMachineTarget master, BionicSuffocationMonitor.Def def)
      : base(master, def)
    {
      this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
      Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
      float breathRate = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE;
      this.breathing = new AttributeModifier(deltaAttribute.Id, breathRate, (string) DUPLICANTS.MODIFIERS.BREATHING.NAME);
      this.holdingbreath = new AttributeModifier(deltaAttribute.Id, -breathRate, (string) DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME);
      this.oxygenBreather = this.GetComponent<OxygenBreather>();
    }

    public bool IsBreathing()
    {
      return this.oxygenBreather.HasOxygen || this.master.GetComponent<KPrefabID>().HasTag(GameTags.RecoveringBreath) || this.oxygenBreather.HasTag(GameTags.InTransitTube);
    }

    public bool HasSuffocated() => (double) this.breath.value <= 0.0;

    public bool IsSuffocating()
    {
      return (double) this.breath.deltaAttribute.GetTotalValue() <= 0.0 && (double) this.breath.value <= (double) DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
    }

    public void Kill()
    {
      this.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
    }
  }
}
