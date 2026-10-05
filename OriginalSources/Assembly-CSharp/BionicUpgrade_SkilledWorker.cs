// Decompiled with JetBrains decompiler
// Type: BionicUpgrade_SkilledWorker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;

#nullable disable
public class BionicUpgrade_SkilledWorker : 
  BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.Inactive;
    this.root.Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ApplySkillPerks)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.RemoveSkillPerks)).Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ApplyModifiers)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.RemoveModifiers)).Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ApplyHats)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.RemoveHats));
    this.Inactive.EventTransition(GameHashes.ScheduleBlocksChanged, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.ScheduleChanged, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.BionicOnline, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.StartWork, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged);
    this.Active.EventTransition(GameHashes.ScheduleBlocksChanged, this.Inactive, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBedTimeChore)).EventTransition(GameHashes.ScheduleChanged, this.Inactive, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBedTimeChore)).EventTransition(GameHashes.BionicOffline, this.Inactive).EventTransition(GameHashes.StopWork, this.Inactive).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged).Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.CreateFX)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ClearFX));
  }

  public static void ApplySkillPerks(BionicUpgrade_SkilledWorker.Instance smi)
  {
    smi.resume.ApplyAdditionalSkillPerks(((BionicUpgrade_SkilledWorker.Def) smi.def).SkillPerksIds);
  }

  public static void RemoveSkillPerks(BionicUpgrade_SkilledWorker.Instance smi)
  {
    smi.resume.RemoveAdditionalSkillPerks(((BionicUpgrade_SkilledWorker.Def) smi.def).SkillPerksIds);
  }

  public static void ApplyModifiers(BionicUpgrade_SkilledWorker.Instance smi)
  {
    smi.ApplyModifiers();
  }

  public static void RemoveModifiers(BionicUpgrade_SkilledWorker.Instance smi)
  {
    smi.RemoveModifiers();
  }

  public static void ApplyHats(BionicUpgrade_SkilledWorker.Instance smi) => smi.ApplyHats();

  public static void RemoveHats(BionicUpgrade_SkilledWorker.Instance smi) => smi.RemoveHats();

  public static bool IsMinionWorkingOnlineAndNotInBatterySaveMode(
    BionicUpgrade_SkilledWorker.Instance smi)
  {
    return BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsOnline((BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.BaseInstance) smi) && !BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBedTimeChore((BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.BaseInstance) smi) && BionicUpgrade_SkilledWorker.IsMinionWorkingWithAttribute(smi);
  }

  public static bool IsMinionWorkingWithAttribute(BionicUpgrade_SkilledWorker.Instance smi)
  {
    Workable workable = smi.worker.GetWorkable();
    return (Object) workable != (Object) null && smi.worker.GetState() == WorkerBase.State.Working && workable.GetWorkAttribute() != null && workable.GetWorkAttribute().Id == ((BionicUpgrade_SkilledWorker.Def) smi.def).AttributeId;
  }

  public static void CreateFX(BionicUpgrade_SkilledWorker.Instance smi)
  {
    BionicUpgrade_SkilledWorker.CreateAndReturnFX(smi);
  }

  public static BionicAttributeUseFx.Instance CreateAndReturnFX(
    BionicUpgrade_SkilledWorker.Instance smi)
  {
    if (smi.isMasterNull)
      return (BionicAttributeUseFx.Instance) null;
    smi.fx = new BionicAttributeUseFx.Instance((IStateMachineTarget) smi.GetComponent<KMonoBehaviour>(), new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.FXFront)));
    smi.fx.StartSM();
    return smi.fx;
  }

  public static void ClearFX(BionicUpgrade_SkilledWorker.Instance smi)
  {
    smi.fx.sm.destroyFX.Trigger(smi.fx);
    smi.fx = (BionicAttributeUseFx.Instance) null;
  }

  public new class Def : 
    BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def
  {
    public SkillPerk[] SkillPerksIds;
    public string AttributeId;
    public AttributeModifier[] modifiers;
    public string[] hats;

    public Def(
      string upgradeID,
      string attributeID,
      AttributeModifier[] modifiers = null,
      SkillPerk[] skillPerks = null,
      string[] hats = null)
      : base(upgradeID)
    {
      this.AttributeId = attributeID;
      this.modifiers = modifiers;
      this.SkillPerksIds = skillPerks;
      this.hats = hats;
    }

    public override string GetDescription()
    {
      string description = "";
      if (this.SkillPerksIds.Length != 0)
      {
        description += (string) UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.HEADER_PERKS;
        for (int index = 0; index < this.SkillPerksIds.Length; ++index)
          description = description + "\n" + SkillPerk.GetDescription(this.SkillPerksIds[index].Id);
        if (this.modifiers.Length != 0)
          description += "\n\n";
      }
      if (this.modifiers.Length != 0)
      {
        description += (string) UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.HEADER_ATTRIBUTES;
        for (int index = 0; index < this.modifiers.Length; ++index)
          description = $"{description + "\n"}{this.modifiers[index].GetName()}: {this.modifiers[index].GetFormattedString()}";
      }
      return description;
    }
  }

  public new class Instance(IStateMachineTarget master, BionicUpgrade_SkilledWorker.Def def) : 
    BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.BaseInstance(master, (BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def) def)
  {
    [MyCmpGet]
    public WorkerBase worker;
    [MyCmpGet]
    public MinionResume resume;
    public BionicAttributeUseFx.Instance fx;

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

    public void ApplyModifiers()
    {
      Klei.AI.Attributes attributes = this.resume.GetIdentity.GetAttributes();
      foreach (AttributeModifier modifier in ((BionicUpgrade_SkilledWorker.Def) this.smi.def).modifiers)
        attributes.Add(modifier);
    }

    public void RemoveModifiers()
    {
      Klei.AI.Attributes attributes = this.resume.GetIdentity.GetAttributes();
      foreach (AttributeModifier modifier in ((BionicUpgrade_SkilledWorker.Def) this.smi.def).modifiers)
        attributes.Remove(modifier);
    }

    public void ApplyHats()
    {
      string[] hats = ((BionicUpgrade_SkilledWorker.Def) this.smi.def).hats;
      if (hats == null)
        return;
      MinionResume component = this.GetComponent<MinionResume>();
      string properName = Assets.GetPrefab((Tag) this.smi.def.UpgradeID).GetProperName();
      foreach (string hat in hats)
        component.AddAdditionalHat(properName, hat);
    }

    public void RemoveHats()
    {
      string[] hats = ((BionicUpgrade_SkilledWorker.Def) this.smi.def).hats;
      if (hats == null)
        return;
      MinionResume component = this.GetComponent<MinionResume>();
      string properName = Assets.GetPrefab((Tag) this.smi.def.UpgradeID).GetProperName();
      foreach (string hat in hats)
        component.RemoveAdditionalHat(properName, hat);
    }
  }
}
