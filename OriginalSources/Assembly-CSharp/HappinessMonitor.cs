// Decompiled with JetBrains decompiler
// Type: HappinessMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

#nullable disable
public class HappinessMonitor : 
  GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>
{
  private const float REPRODUCTION_HAPPINESS_MULTIPLIER = 2.25f;
  private GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State satisfied;
  private HappinessMonitor.HappyState happy;
  private HappinessMonitor.NeutralState neutral;
  private HappinessMonitor.UnhappyState glum;
  private HappinessMonitor.MiserableState miserable;
  private Effect happyWildEffect;
  private Effect happyTameEffect;
  private Effect neutralTameEffect;
  private Effect neutralWildEffect;
  private Effect glumWildEffect;
  private Effect glumTameEffect;
  private Effect miserableWildEffect;
  private Effect miserableTameEffect;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.satisfied.Transition((GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State) this.happy, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy), UpdateRate.SIM_1000ms).Transition((GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State) this.neutral, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsNeutral), UpdateRate.SIM_1000ms).Transition((GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State) this.glum, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsGlum), UpdateRate.SIM_1000ms).Transition((GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State) this.miserable, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsMisirable), UpdateRate.SIM_1000ms);
    this.happy.DefaultState(this.happy.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy)), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Happy).ToggleCritterEmotion(Db.Get().CritterEmotions.Happy);
    this.happy.wild.ToggleEffect((Func<HappinessMonitor.Instance, Effect>) (smi => this.happyWildEffect)).TagTransition(GameTags.Creatures.Wild, this.happy.tame, true);
    this.happy.tame.ToggleEffect((Func<HappinessMonitor.Instance, Effect>) (smi => this.happyTameEffect)).TagTransition(GameTags.Creatures.Wild, this.happy.wild);
    this.neutral.DefaultState(this.neutral.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsNeutral)), UpdateRate.SIM_1000ms);
    this.neutral.wild.ToggleEffect((Func<HappinessMonitor.Instance, Effect>) (smi => this.neutralWildEffect)).TagTransition(GameTags.Creatures.Wild, this.neutral.tame, true);
    this.neutral.tame.ToggleEffect((Func<HappinessMonitor.Instance, Effect>) (smi => this.neutralTameEffect)).TagTransition(GameTags.Creatures.Wild, this.neutral.wild);
    this.glum.DefaultState(this.glum.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsGlum)), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Unhappy);
    this.glum.wild.ToggleEffect((Func<HappinessMonitor.Instance, Effect>) (smi => this.glumWildEffect)).TagTransition(GameTags.Creatures.Wild, this.glum.tame, true);
    this.glum.tame.ToggleEffect((Func<HappinessMonitor.Instance, Effect>) (smi => this.glumTameEffect)).TagTransition(GameTags.Creatures.Wild, this.glum.wild);
    this.miserable.DefaultState(this.miserable.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsMisirable)), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Unhappy);
    this.miserable.wild.ToggleEffect((Func<HappinessMonitor.Instance, Effect>) (smi => this.miserableWildEffect)).TagTransition(GameTags.Creatures.Wild, this.miserable.tame, true);
    this.miserable.tame.ToggleEffect((Func<HappinessMonitor.Instance, Effect>) (smi => this.miserableTameEffect)).TagTransition(GameTags.Creatures.Wild, this.miserable.wild);
    this.happyWildEffect = new Effect("Happy", (string) CREATURES.MODIFIERS.HAPPY_WILD.NAME, (string) CREATURES.MODIFIERS.HAPPY_WILD.TOOLTIP, 0.0f, true, false, false);
    this.happyTameEffect = new Effect("Happy", (string) CREATURES.MODIFIERS.HAPPY_TAME.NAME, (string) CREATURES.MODIFIERS.HAPPY_TAME.TOOLTIP, 0.0f, true, false, false);
    this.neutralWildEffect = new Effect("Neutral", (string) CREATURES.MODIFIERS.NEUTRAL.NAME, (string) CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0.0f, true, false, false);
    this.neutralTameEffect = new Effect("Neutral", (string) CREATURES.MODIFIERS.NEUTRAL.NAME, (string) CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0.0f, true, false, false);
    this.glumWildEffect = new Effect("Glum", (string) CREATURES.MODIFIERS.GLUM.NAME, (string) CREATURES.MODIFIERS.GLUM.TOOLTIP, 0.0f, true, false, true);
    this.glumTameEffect = new Effect("Glum", (string) CREATURES.MODIFIERS.GLUM.NAME, (string) CREATURES.MODIFIERS.GLUM.TOOLTIP, 0.0f, true, false, true);
    this.miserableWildEffect = new Effect("Miserable", (string) CREATURES.MODIFIERS.MISERABLE.NAME, (string) CREATURES.MODIFIERS.MISERABLE.TOOLTIP, 0.0f, true, false, true);
    this.miserableTameEffect = new Effect("Miserable", (string) CREATURES.MODIFIERS.MISERABLE.NAME, (string) CREATURES.MODIFIERS.MISERABLE.TOOLTIP, 0.0f, true, false, true);
    this.glumWildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -15f, (string) CREATURES.MODIFIERS.GLUM.NAME));
    this.glumTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -80f, (string) CREATURES.MODIFIERS.GLUM.NAME));
    this.miserableTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -80f, (string) CREATURES.MODIFIERS.MISERABLE.NAME));
    this.miserableTameEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, (string) CREATURES.MODIFIERS.MISERABLE.NAME, true));
    this.miserableTameEffect.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, -1f, (string) CREATURES.MODIFIERS.MISERABLE.NAME, true));
    this.miserableWildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -15f, (string) CREATURES.MODIFIERS.MISERABLE.NAME));
    this.miserableWildEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, (string) CREATURES.MODIFIERS.MISERABLE.NAME, true));
    this.miserableWildEffect.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, -1f, (string) CREATURES.MODIFIERS.MISERABLE.NAME, true));
  }

  private static bool IsHappy(HappinessMonitor.Instance smi)
  {
    return (double) smi.happiness.GetTotalValue() >= (double) smi.def.happyThreshold;
  }

  private static bool IsNeutral(HappinessMonitor.Instance smi)
  {
    float totalValue = smi.happiness.GetTotalValue();
    return (double) totalValue > (double) smi.def.glumThreshold && (double) totalValue < (double) smi.def.happyThreshold;
  }

  private static bool IsGlum(HappinessMonitor.Instance smi)
  {
    float totalValue = smi.happiness.GetTotalValue();
    return (double) totalValue > (double) smi.def.miserableThreshold && (double) totalValue <= (double) smi.def.glumThreshold;
  }

  private static bool IsMisirable(HappinessMonitor.Instance smi)
  {
    return (double) smi.happiness.GetTotalValue() <= (double) smi.def.miserableThreshold;
  }

  public class Def : StateMachine.BaseDef
  {
    public float happyThreshold = 1f;
    public float glumThreshold = -1f;
    public float miserableThreshold = -10f;
  }

  public class MiserableState : 
    GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
  {
    public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;
    public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
  }

  public class NeutralState : 
    GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
  {
    public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;
    public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
  }

  public class UnhappyState : 
    GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
  {
    public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;
    public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
  }

  public class HappyState : 
    GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
  {
    public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;
    public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
  }

  public new class Instance : 
    GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.GameInstance
  {
    public AttributeInstance happiness;
    private AttributeModifier fertilityFromHappiness;
    private bool fertilityModifierActive;

    public Instance(IStateMachineTarget master, HappinessMonitor.Def def)
      : base(master, def)
    {
      this.happiness = this.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Happiness);
      this.fertilityFromHappiness = new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, 0.0f, (string) CREATURES.MODIFIERS.HAPPY_TAME.NAME, true, is_readonly: false);
      this.happiness.OnDirty += new System.Action(this.UpdateFertilityModifier);
      this.gameObject.Subscribe(-1582839653, new System.Action<object>(this.OnTagsChanged));
      this.UpdateFertilityModifier();
    }

    private void OnTagsChanged(object data) => this.UpdateFertilityModifier();

    private void UpdateFertilityModifier()
    {
      float fertilityMultiplier = this.GetFertilityMultiplier();
      if ((double) fertilityMultiplier > 0.0)
      {
        this.fertilityFromHappiness.SetValue(fertilityMultiplier);
        if (this.fertilityModifierActive)
          return;
        this.gameObject.GetAttributes().Add(this.fertilityFromHappiness);
        this.fertilityModifierActive = true;
      }
      else
      {
        if (!this.fertilityModifierActive)
          return;
        this.gameObject.GetAttributes().Remove(this.fertilityFromHappiness);
        this.fertilityModifierActive = false;
      }
    }

    private float GetFertilityMultiplier()
    {
      if (this.gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Creatures.Wild))
        return 0.0f;
      float totalValue = this.happiness.GetTotalValue();
      return (double) totalValue <= 0.0 ? 0.0f : totalValue * 2.25f;
    }
  }
}
