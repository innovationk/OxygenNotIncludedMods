// Decompiled with JetBrains decompiler
// Type: BionicUpgrade_OnGoingEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class BionicUpgrade_OnGoingEffect : 
  BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.Inactive;
    this.Inactive.EventTransition(GameHashes.ScheduleBlocksChanged, this.Active, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_OnGoingEffect.IsOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.ScheduleChanged, this.Active, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_OnGoingEffect.IsOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.BionicOnline, this.Active, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_OnGoingEffect.IsOnlineAndNotInBatterySaveMode)).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged);
    this.Active.ToggleEffect(new Func<BionicUpgrade_OnGoingEffect.Instance, string>(BionicUpgrade_OnGoingEffect.GetEffectName)).Enter(new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.State.Callback(BionicUpgrade_OnGoingEffect.ApplySkills)).Exit(new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.State.Callback(BionicUpgrade_OnGoingEffect.RemoveSkills)).EventTransition(GameHashes.ScheduleBlocksChanged, this.Inactive, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsInBedTimeChore)).EventTransition(GameHashes.ScheduleChanged, this.Inactive, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsInBedTimeChore)).EventTransition(GameHashes.BionicOffline, this.Inactive, GameStateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Not(new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsOnline))).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged);
  }

  public static string GetEffectName(BionicUpgrade_OnGoingEffect.Instance smi)
  {
    return ((BionicUpgrade_OnGoingEffect.Def) smi.def).EFFECT_NAME;
  }

  public static void ApplySkills(BionicUpgrade_OnGoingEffect.Instance smi) => smi.ApplySkills();

  public static void RemoveSkills(BionicUpgrade_OnGoingEffect.Instance smi) => smi.RemoveSkills();

  public static bool IsOnlineAndNotInBatterySaveMode(BionicUpgrade_OnGoingEffect.Instance smi)
  {
    return BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsOnline((BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.BaseInstance) smi) && !BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsInBedTimeChore((BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.BaseInstance) smi);
  }

  public new class Def : 
    BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def
  {
    public string EFFECT_NAME;
    public string[] SKILLS_IDS;

    public Def(string upgradeID, string effectID, string[] skills = null)
      : base(upgradeID)
    {
      this.EFFECT_NAME = effectID;
      this.SKILLS_IDS = skills;
    }

    public override string GetDescription()
    {
      return "BionicUpgrade_OnGoingEffect.Def description not implemented";
    }
  }

  public new class Instance : 
    BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.BaseInstance
  {
    private MinionResume resume;

    public Instance(IStateMachineTarget master, BionicUpgrade_OnGoingEffect.Def def)
      : base(master, (BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def) def)
    {
      this.resume = this.GetComponent<MinionResume>();
    }

    public override float GetCurrentWattageCost()
    {
      return this.IsInsideState((StateMachine.BaseState) this.sm.Active) ? this.Data.WattageCost : 0.0f;
    }

    public override string GetCurrentWattageCostName()
    {
      float currentWattageCost = this.GetCurrentWattageCost();
      if (!this.IsInsideState((StateMachine.BaseState) this.sm.Active))
        return string.Format((string) DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_INACTIVE_TEMPLATE, (object) this.upgradeComponent.GetProperName(), (object) GameUtil.GetFormattedWattage(this.upgradeComponent.PotentialWattage));
      string str = $"<b>{((double) currentWattageCost >= 0.0 ? "+" : "-")}</b>";
      return string.Format((string) DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_ACTIVE_TEMPLATE, (object) this.upgradeComponent.GetProperName(), (object) (str + GameUtil.GetFormattedWattage(currentWattageCost)));
    }

    public void ApplySkills()
    {
      BionicUpgrade_OnGoingEffect.Def def = (BionicUpgrade_OnGoingEffect.Def) this.def;
      if (def.SKILLS_IDS == null)
        return;
      for (int index = 0; index < def.SKILLS_IDS.Length; ++index)
        this.resume.GrantSkill(def.SKILLS_IDS[index]);
    }

    public void RemoveSkills()
    {
      BionicUpgrade_OnGoingEffect.Def def = (BionicUpgrade_OnGoingEffect.Def) this.def;
      if (def.SKILLS_IDS == null)
        return;
      for (int index = 0; index < def.SKILLS_IDS.Length; ++index)
        this.resume.UngrantSkill(def.SKILLS_IDS[index]);
    }
  }
}
