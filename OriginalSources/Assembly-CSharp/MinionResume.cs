// Decompiled with JetBrains decompiler
// Type: MinionResume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TUNING;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MinionResume")]
public class MinionResume : IExperienceRecipient, ISaveLoadable, ISim200ms
{
  [MyCmpReq]
  private MinionIdentity identity;
  [Serialize]
  public Dictionary<string, bool> MasteryByRoleID = new Dictionary<string, bool>();
  [Serialize]
  public Dictionary<string, bool> MasteryBySkillID = new Dictionary<string, bool>();
  [Serialize]
  public List<string> GrantedSkillIDs = new List<string>();
  private List<HashedString> AdditionalGrantedSkillPerkIDs = new List<HashedString>();
  private List<MinionResume.HatInfo> AdditionalHats = new List<MinionResume.HatInfo>();
  [Serialize]
  public Dictionary<HashedString, float> AptitudeByRoleGroup = new Dictionary<HashedString, float>();
  [Serialize]
  public Dictionary<HashedString, float> AptitudeBySkillGroup = new Dictionary<HashedString, float>();
  [Serialize]
  private string currentRole = "NoRole";
  [Serialize]
  private string targetRole = "NoRole";
  [Serialize]
  private string currentHat;
  [Serialize]
  private string targetHat;
  private Dictionary<string, bool> ownedHats = new Dictionary<string, bool>();
  [Serialize]
  private float totalExperienceGained;
  private Notification lastSkillNotification;
  private PutOnHatChore lastHatChore;
  private AttributeModifier skillsMoraleExpectationModifier;
  private AttributeModifier skillsMoraleModifier;
  public float DEBUG_PassiveExperienceGained;
  public float DEBUG_ActiveExperienceGained;
  public float DEBUG_SecondsAlive;

  public MinionIdentity GetIdentity => this.identity;

  public float TotalExperienceGained => this.totalExperienceGained;

  public int TotalSkillPointsGained
  {
    get => MinionResume.CalculateTotalSkillPointsGained(this.TotalExperienceGained);
  }

  public static int CalculateTotalSkillPointsGained(float experience)
  {
    return Mathf.FloorToInt(Mathf.Pow((float) ((double) experience / (double) SKILLS.TARGET_SKILLS_CYCLE / 600.0), 1f / SKILLS.EXPERIENCE_LEVEL_POWER) * (float) SKILLS.TARGET_SKILLS_EARNED);
  }

  public int SkillsMastered
  {
    get
    {
      int skillsMastered = 0;
      foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
      {
        if (keyValuePair.Value)
          ++skillsMastered;
      }
      return skillsMastered;
    }
  }

  public int AvailableSkillpoints
  {
    get
    {
      return this.TotalSkillPointsGained - this.SkillsMastered + (this.GrantedSkillIDs == null ? 0 : this.GrantedSkillIDs.Count);
    }
  }

  [OnDeserialized]
  private void OnDeserializedMethod()
  {
    if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 7))
    {
      foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryByRoleID)
      {
        if (keyValuePair.Value && keyValuePair.Key != "NoRole")
          this.ForceAddSkillPoint();
      }
      foreach (KeyValuePair<HashedString, float> keyValuePair in this.AptitudeByRoleGroup)
        this.AptitudeBySkillGroup[keyValuePair.Key] = keyValuePair.Value;
    }
    if (this.TotalSkillPointsGained <= 1000 && this.TotalSkillPointsGained >= 0)
      return;
    this.ForceSetSkillPoints(100);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.MinionResumes.Add(this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GrantedSkillIDs.RemoveAll((Predicate<string>) (x => Db.Get().Skills.TryGet(x) == null));
    List<string> stringList1 = new List<string>();
    foreach (string key in this.MasteryBySkillID.Keys)
    {
      if (Db.Get().Skills.TryGet(key) == null)
        stringList1.Add(key);
    }
    foreach (string key in stringList1)
      this.MasteryBySkillID.Remove(key);
    if (this.GrantedSkillIDs == null)
      this.GrantedSkillIDs = new List<string>();
    List<string> stringList2 = new List<string>();
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
    {
      if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).deprecated)
        stringList2.Add(keyValuePair.Key);
    }
    foreach (string skillId in stringList2)
      this.UnmasterSkill(skillId);
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
    {
      if (keyValuePair.Value)
      {
        Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
        foreach (SkillPerk perk in skill.perks)
        {
          if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) perk))
          {
            if (perk.OnRemove != null)
              perk.OnRemove(this);
            if (perk.OnApply != null)
              perk.OnApply(this);
          }
        }
        if (!this.ownedHats.ContainsKey(skill.hat))
          this.ownedHats.Add(skill.hat, true);
      }
    }
    this.UpdateExpectations();
    this.UpdateMorale();
    MinionResume.ApplyHat(this.currentHat, this.GetComponent<KBatchedAnimController>());
    this.ShowNewSkillPointNotification();
  }

  public void RestoreResume(
    Dictionary<string, bool> MasteryBySkillID,
    Dictionary<HashedString, float> AptitudeBySkillGroup,
    List<string> GrantedSkillIDs,
    float totalExperienceGained)
  {
    this.MasteryBySkillID = MasteryBySkillID;
    this.GrantedSkillIDs = GrantedSkillIDs != null ? GrantedSkillIDs : new List<string>();
    this.AptitudeBySkillGroup = AptitudeBySkillGroup;
    this.totalExperienceGained = totalExperienceGained;
  }

  protected override void OnCleanUp()
  {
    Components.MinionResumes.Remove(this);
    if (this.lastSkillNotification != null)
    {
      Game.Instance.GetComponent<Notifier>().Remove(this.lastSkillNotification);
      this.lastSkillNotification = (Notification) null;
    }
    base.OnCleanUp();
  }

  public bool HasMasteredSkill(string skillId)
  {
    return this.MasteryBySkillID.ContainsKey(skillId) && this.MasteryBySkillID[skillId];
  }

  public void UpdateUrge()
  {
    if (this.targetHat != this.currentHat)
    {
      if (this.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
        return;
      this.gameObject.GetComponent<ChoreConsumer>().AddUrge(Db.Get().Urges.LearnSkill);
    }
    else
      this.gameObject.GetComponent<ChoreConsumer>().RemoveUrge(Db.Get().Urges.LearnSkill);
  }

  public string CurrentRole => this.currentRole;

  public string CurrentHat => this.currentHat;

  public string TargetHat => this.targetHat;

  public void SetHats(string current, string target)
  {
    this.currentHat = current;
    this.targetHat = target;
  }

  public void ClearAdditionalHats() => this.AdditionalHats.Clear();

  public void AddAdditionalHat(string context, string hat)
  {
    MinionResume.HatInfo hatInfo = (MinionResume.HatInfo) null;
    foreach (MinionResume.HatInfo additionalHat in this.AdditionalHats)
    {
      if (additionalHat.Source == context && additionalHat.Hat == hat)
      {
        hatInfo = additionalHat;
        break;
      }
    }
    if (hatInfo != null)
      ++hatInfo.count;
    else
      this.AdditionalHats.Add(new MinionResume.HatInfo(context, hat));
  }

  public void RemoveAdditionalHat(string context, string hat)
  {
    MinionResume.HatInfo hatInfo = (MinionResume.HatInfo) null;
    foreach (MinionResume.HatInfo additionalHat in this.AdditionalHats)
    {
      if (additionalHat.Source == context && additionalHat.Hat == hat)
      {
        --additionalHat.count;
        hatInfo = additionalHat;
        break;
      }
    }
    if (hatInfo == null || hatInfo.count > 0)
      return;
    this.AdditionalHats.Remove(hatInfo);
    if (!(this.currentHat == hat))
      return;
    this.RemoveHat();
  }

  public void SetCurrentRole(string role_id) => this.currentRole = role_id;

  public string TargetRole => this.targetRole;

  public void ApplyAdditionalSkillPerks(SkillPerk[] perks)
  {
    foreach (SkillPerk perk in perks)
    {
      if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) perk))
      {
        this.AdditionalGrantedSkillPerkIDs.Add(perk.IdHash);
        if (perk.OnApply != null)
          perk.OnApply(this);
      }
    }
    Game.Instance.Trigger(-1523247426, (object) null);
  }

  public void RemoveAdditionalSkillPerks(SkillPerk[] perks)
  {
    foreach (SkillPerk perk in perks)
    {
      if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) perk))
      {
        this.AdditionalGrantedSkillPerkIDs.Remove(perk.IdHash);
        if (perk.OnRemove != null)
          perk.OnRemove(this);
      }
    }
    Game.Instance.Trigger(-1523247426, (object) null);
  }

  private void ApplySkillPerksForSkill(string skillId)
  {
    foreach (SkillPerk perk in Db.Get().Skills.Get(skillId).perks)
    {
      if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) perk) && perk.OnApply != null)
        perk.OnApply(this);
    }
  }

  private void RemoveSkillPerksForSkill(string skillId)
  {
    foreach (SkillPerk perk in Db.Get().Skills.Get(skillId).perks)
    {
      if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) perk) && perk.OnRemove != null)
        perk.OnRemove(this);
    }
  }

  public void Sim200ms(float dt)
  {
    this.DEBUG_SecondsAlive += dt;
    if (this.GetComponent<KPrefabID>().HasTag(GameTags.Dead))
      return;
    this.DEBUG_PassiveExperienceGained += dt * SKILLS.PASSIVE_EXPERIENCE_PORTION;
    this.AddExperience(dt * SKILLS.PASSIVE_EXPERIENCE_PORTION);
  }

  public bool IsAbleToLearnSkill(string skillId)
  {
    Skill skill = Db.Get().Skills.Get(skillId);
    string choreGroupId = Db.Get().SkillGroups.Get(skill.skillGroup).choreGroupID;
    if (!string.IsNullOrEmpty(choreGroupId))
    {
      Traits component = this.GetComponent<Traits>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.IsChoreGroupDisabled((HashedString) choreGroupId))
        return false;
    }
    return true;
  }

  public bool BelowMoraleExpectation(Skill skill)
  {
    float totalValue1 = Db.Get().Attributes.QualityOfLife.Lookup((Component) this).GetTotalValue();
    double totalValue2 = (double) Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) this).GetTotalValue();
    int moraleExpectation = skill.GetMoraleExpectation();
    if (this.AptitudeBySkillGroup.ContainsKey((HashedString) skill.skillGroup) && (double) this.AptitudeBySkillGroup[(HashedString) skill.skillGroup] > 0.0)
      ++totalValue1;
    double num = (double) moraleExpectation;
    return totalValue2 + num <= (double) totalValue1;
  }

  public bool HasMasteredDirectlyRequiredSkillsForSkill(Skill skill)
  {
    for (int index = 0; index < skill.priorSkills.Count; ++index)
    {
      if (!this.HasMasteredSkill(skill.priorSkills[index]))
        return false;
    }
    return true;
  }

  public bool HasSkillPointsRequiredForSkill(Skill skill) => this.AvailableSkillpoints >= 1;

  public bool HasSkillAptitude(Skill skill)
  {
    return this.AptitudeBySkillGroup.ContainsKey((HashedString) skill.skillGroup) && (double) this.AptitudeBySkillGroup[(HashedString) skill.skillGroup] > 0.0;
  }

  public bool HasBeenGrantedSkill(Skill skill)
  {
    return this.GrantedSkillIDs != null && this.GrantedSkillIDs.Contains(skill.Id);
  }

  public bool HasBeenGrantedSkill(string id)
  {
    return this.GrantedSkillIDs != null && this.GrantedSkillIDs.Contains(id);
  }

  public MinionResume.SkillMasteryConditions[] GetSkillMasteryConditions(string skillId)
  {
    List<MinionResume.SkillMasteryConditions> masteryConditionsList = new List<MinionResume.SkillMasteryConditions>();
    Skill skill = Db.Get().Skills.Get(skillId);
    if (this.HasSkillAptitude(skill))
      masteryConditionsList.Add(MinionResume.SkillMasteryConditions.SkillAptitude);
    if (!this.BelowMoraleExpectation(skill))
      masteryConditionsList.Add(MinionResume.SkillMasteryConditions.StressWarning);
    if (!this.IsAbleToLearnSkill(skillId))
      masteryConditionsList.Add(MinionResume.SkillMasteryConditions.UnableToLearn);
    if (!this.HasSkillPointsRequiredForSkill(skill))
      masteryConditionsList.Add(MinionResume.SkillMasteryConditions.NeedsSkillPoints);
    if (!this.HasMasteredDirectlyRequiredSkillsForSkill(skill))
      masteryConditionsList.Add(MinionResume.SkillMasteryConditions.MissingPreviousSkill);
    return masteryConditionsList.ToArray();
  }

  public bool CanMasterSkill(
    MinionResume.SkillMasteryConditions[] masteryConditions)
  {
    return !Array.Exists<MinionResume.SkillMasteryConditions>(masteryConditions, (Predicate<MinionResume.SkillMasteryConditions>) (element => element == MinionResume.SkillMasteryConditions.UnableToLearn || element == MinionResume.SkillMasteryConditions.NeedsSkillPoints || element == MinionResume.SkillMasteryConditions.MissingPreviousSkill));
  }

  public bool OwnsHat(string hatId)
  {
    foreach (MinionResume.HatInfo additionalHat in this.AdditionalHats)
    {
      if (additionalHat.Hat == hatId)
        return true;
    }
    return this.ownedHats.ContainsKey(hatId) && this.ownedHats[hatId];
  }

  public List<MinionResume.HatInfo> GetAllHats()
  {
    List<MinionResume.HatInfo> allHats = new List<MinionResume.HatInfo>();
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
    {
      if (keyValuePair.Value)
      {
        Skill skill = Db.Get().Skills.TryGet(keyValuePair.Key);
        if (!skill.hat.IsNullOrWhiteSpace())
          allHats.Add(new MinionResume.HatInfo(skill.Name, skill.hat));
      }
    }
    allHats.AddRange((IEnumerable<MinionResume.HatInfo>) this.AdditionalHats);
    return allHats;
  }

  public void SkillLearned()
  {
    if (this.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
      this.gameObject.GetComponent<ChoreConsumer>().RemoveUrge(Db.Get().Urges.LearnSkill);
    foreach (string key in this.ownedHats.Keys.ToList<string>())
      this.ownedHats[key] = true;
    if (this.targetHat == null || !(this.currentHat != this.targetHat))
      return;
    this.CreateHatChangeChore();
  }

  public void CreateHatChangeChore()
  {
    if (this.lastHatChore != null)
      this.lastHatChore.Cancel("New Hat");
    this.lastHatChore = new PutOnHatChore((IStateMachineTarget) this, Db.Get().ChoreTypes.SwitchHat);
  }

  public void MasterSkill(string skillId)
  {
    if (!this.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
      this.gameObject.GetComponent<ChoreConsumer>().AddUrge(Db.Get().Urges.LearnSkill);
    this.MasteryBySkillID[skillId] = true;
    this.ApplySkillPerksForSkill(skillId);
    this.UpdateExpectations();
    this.UpdateMorale();
    this.TriggerMasterSkillEvents();
    GameScheduler.Instance.Schedule("Morale Tutorial", 2f, (Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Morale)), (object) null, (SchedulerGroup) null);
    if (!this.ownedHats.ContainsKey(Db.Get().Skills.Get(skillId).hat))
      this.ownedHats.Add(Db.Get().Skills.Get(skillId).hat, false);
    if (this.AvailableSkillpoints != 0 || this.lastSkillNotification == null)
      return;
    Game.Instance.GetComponent<Notifier>().Remove(this.lastSkillNotification);
    this.lastSkillNotification = (Notification) null;
  }

  public void UnmasterSkill(string skillId)
  {
    if (!this.MasteryBySkillID.ContainsKey(skillId))
      return;
    this.MasteryBySkillID.Remove(skillId);
    this.RemoveSkillPerksForSkill(skillId);
    this.UpdateExpectations();
    this.UpdateMorale();
    this.TriggerMasterSkillEvents();
  }

  public void GrantSkill(string skillId)
  {
    if (this.GrantedSkillIDs == null)
      this.GrantedSkillIDs = new List<string>();
    if (this.HasBeenGrantedSkill(skillId))
      return;
    this.MasteryBySkillID[skillId] = true;
    this.ApplySkillPerksForSkill(skillId);
    this.GrantedSkillIDs.Add(skillId);
    this.UpdateExpectations();
    this.UpdateMorale();
    this.TriggerMasterSkillEvents();
    if (this.ownedHats.ContainsKey(Db.Get().Skills.Get(skillId).hat))
      return;
    this.ownedHats.Add(Db.Get().Skills.Get(skillId).hat, false);
  }

  public void UngrantSkill(string skillId)
  {
    if (this.GrantedSkillIDs != null)
      this.GrantedSkillIDs.RemoveAll((Predicate<string>) (match => match == skillId));
    this.UnmasterSkill(skillId);
  }

  public Sprite GetSkillGrantSourceIcon(string skillID)
  {
    if (!this.GrantedSkillIDs.Contains(skillID))
      return (Sprite) null;
    BionicUpgradesMonitor.Instance smi = this.gameObject.GetSMI<BionicUpgradesMonitor.Instance>();
    if (smi != null)
    {
      foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in smi.upgradeComponentSlots)
      {
        if (upgradeComponentSlot.HasUpgradeInstalled)
          return Def.GetUISprite((object) upgradeComponentSlot.installedUpgradeComponent.gameObject).first;
      }
    }
    return Assets.GetSprite((HashedString) "skill_granted_trait");
  }

  private void TriggerMasterSkillEvents()
  {
    this.Trigger(540773776, (object) null);
    Game.Instance.Trigger(-1523247426, (object) this);
  }

  public void ForceSetSkillPoints(int points)
  {
    this.totalExperienceGained = MinionResume.CalculatePreviousExperienceBar(points);
    if (this.TotalSkillPointsGained == points)
      return;
    float previousExperienceBar = MinionResume.CalculatePreviousExperienceBar(points);
    float nextExperienceBar = MinionResume.CalculateNextExperienceBar(points);
    this.totalExperienceGained = previousExperienceBar + (float) (((double) nextExperienceBar - (double) previousExperienceBar) * 0.0099999997764825821);
  }

  public void ForceAddSkillPoint() => this.ForceSetSkillPoints(this.TotalSkillPointsGained + 1);

  public static float CalculateNextExperienceBar(int current_skill_points)
  {
    return (float) ((double) Mathf.Pow((float) (current_skill_points + 1) / (float) SKILLS.TARGET_SKILLS_EARNED, SKILLS.EXPERIENCE_LEVEL_POWER) * (double) SKILLS.TARGET_SKILLS_CYCLE * 600.0);
  }

  public static float CalculatePreviousExperienceBar(int current_skill_points)
  {
    return (float) ((double) Mathf.Pow((float) current_skill_points / (float) SKILLS.TARGET_SKILLS_EARNED, SKILLS.EXPERIENCE_LEVEL_POWER) * (double) SKILLS.TARGET_SKILLS_CYCLE * 600.0);
  }

  private void UpdateExpectations()
  {
    int num = 0;
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
    {
      if (keyValuePair.Value && !this.HasBeenGrantedSkill(keyValuePair.Key))
      {
        Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
        num += skill.tier + 1;
      }
    }
    AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) this);
    if (this.skillsMoraleExpectationModifier != null)
    {
      attributeInstance.Remove(this.skillsMoraleExpectationModifier);
      this.skillsMoraleExpectationModifier = (AttributeModifier) null;
    }
    if (num <= 0)
      return;
    this.skillsMoraleExpectationModifier = new AttributeModifier(attributeInstance.Id, (float) num, (string) DUPLICANTS.NEEDS.QUALITYOFLIFE.EXPECTATION_MOD_NAME);
    attributeInstance.Add(this.skillsMoraleExpectationModifier);
  }

  private void UpdateMorale()
  {
    int num1 = 0;
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
    {
      if (keyValuePair.Value && !this.HasBeenGrantedSkill(keyValuePair.Key))
      {
        Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
        float num2 = 0.0f;
        if (this.AptitudeBySkillGroup.TryGetValue(new HashedString(skill.skillGroup), out num2))
          num1 += (int) num2;
      }
    }
    AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup((Component) this);
    if (this.skillsMoraleModifier != null)
    {
      attributeInstance.Remove(this.skillsMoraleModifier);
      this.skillsMoraleModifier = (AttributeModifier) null;
    }
    if (num1 <= 0)
      return;
    this.skillsMoraleModifier = new AttributeModifier(attributeInstance.Id, (float) num1, (string) DUPLICANTS.NEEDS.QUALITYOFLIFE.APTITUDE_SKILLS_MOD_NAME);
    attributeInstance.Add(this.skillsMoraleModifier);
  }

  private void OnSkillPointGained()
  {
    Game.Instance.Trigger(1505456302, (object) this);
    this.ShowNewSkillPointNotification();
    if ((UnityEngine.Object) PopFXManager.Instance != (UnityEngine.Object) null)
    {
      string text = MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.identity.GetProperName());
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, text, this.transform, new Vector3(0.0f, 0.5f, 0.0f));
    }
    new UpgradeFX.Instance((IStateMachineTarget) this.gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0.0f, 0.0f, -0.1f)).StartSM();
  }

  private void ShowNewSkillPointNotification()
  {
    if (this.AvailableSkillpoints != 1)
      return;
    this.lastSkillNotification = (Notification) new ManagementMenuNotification(Action.ManageSkills, NotificationValence.Good, this.identity.GetSoleOwner().gameObject.GetInstanceID().ToString(), MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.identity.GetProperName()), NotificationType.Good, new Func<List<Notification>, object, string>(this.GetSkillPointGainedTooltip), (object) this.identity, custom_click_callback: (Notification.ClickCallback) (d => ManagementMenu.Instance.OpenSkills(this.identity)));
    this.GetComponent<Notifier>().Add(this.lastSkillNotification);
  }

  private string GetSkillPointGainedTooltip(List<Notification> notifications, object data)
  {
    return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP.Replace("{Duplicant}", ((MinionIdentity) data).GetProperName());
  }

  public void SetAptitude(HashedString skillGroupID, float amount)
  {
    this.AptitudeBySkillGroup[skillGroupID] = amount;
  }

  public float GetAptitudeExperienceMultiplier(
    HashedString skillGroupId,
    float buildingFrequencyMultiplier)
  {
    float num = 0.0f;
    this.AptitudeBySkillGroup.TryGetValue(skillGroupId, out num);
    return (float) (1.0 + (double) num * (double) SKILLS.APTITUDE_EXPERIENCE_MULTIPLIER * (double) buildingFrequencyMultiplier);
  }

  public void AddExperience(float amount)
  {
    float experienceGained = this.totalExperienceGained;
    float nextExperienceBar = MinionResume.CalculateNextExperienceBar(this.TotalSkillPointsGained);
    this.totalExperienceGained += amount;
    if (!this.isSpawned || (double) this.totalExperienceGained < (double) nextExperienceBar || (double) experienceGained >= (double) nextExperienceBar)
      return;
    this.OnSkillPointGained();
  }

  public override void AddExperienceWithAptitude(
    string skillGroupId,
    float amount,
    float buildingMultiplier)
  {
    float amount1 = amount * this.GetAptitudeExperienceMultiplier((HashedString) skillGroupId, buildingMultiplier) * SKILLS.ACTIVE_EXPERIENCE_PORTION;
    this.DEBUG_ActiveExperienceGained += amount1;
    this.AddExperience(amount1);
  }

  public bool HasPerk(HashedString perkId)
  {
    foreach (HashedString grantedSkillPerkId in this.AdditionalGrantedSkillPerkIDs)
    {
      if (grantedSkillPerkId == perkId)
        return true;
    }
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
    {
      if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).GivesPerk(perkId))
        return true;
    }
    return false;
  }

  public bool HasPerk(SkillPerk perk)
  {
    foreach (HashedString grantedSkillPerkId in this.AdditionalGrantedSkillPerkIDs)
    {
      if (grantedSkillPerkId == perk.IdHash)
        return true;
    }
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
    {
      if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).GivesPerk(perk))
        return true;
    }
    return false;
  }

  public void RemoveHat()
  {
    MinionResume.RemoveHat(this.GetComponent<KBatchedAnimController>());
    this.currentHat = (string) null;
    this.targetHat = (string) null;
  }

  public static void RemoveHat(KBatchedAnimController controller)
  {
    AccessorySlot hat = Db.Get().AccessorySlots.Hat;
    Accessorizer component = controller.GetComponent<Accessorizer>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      Accessory accessory = component.GetAccessory(hat);
      if (accessory != null)
        component.RemoveAccessory(accessory);
    }
    else
      controller.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride((HashedString) hat.targetSymbolId, 4);
    controller.SetSymbolVisiblity(hat.targetSymbolId, false);
    controller.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
    controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
  }

  public static void AddHat(string hat_id, KBatchedAnimController controller)
  {
    AccessorySlot hat = Db.Get().AccessorySlots.Hat;
    Accessory accessory1 = hat.Lookup(hat_id);
    if (accessory1 == null)
      Debug.LogWarning((object) ("Missing hat: " + hat_id));
    Accessorizer component1 = controller.GetComponent<Accessorizer>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      Accessory accessory2 = component1.GetAccessory(Db.Get().AccessorySlots.Hat);
      if (accessory2 != null)
        component1.RemoveAccessory(accessory2);
      if (accessory1 != null)
        component1.AddAccessory(accessory1);
    }
    else
    {
      SymbolOverrideController component2 = controller.GetComponent<SymbolOverrideController>();
      component2.TryRemoveSymbolOverride((HashedString) hat.targetSymbolId, 4);
      component2.AddSymbolOverride((HashedString) hat.targetSymbolId, accessory1.symbol, 4);
    }
    controller.SetSymbolVisiblity(hat.targetSymbolId, true);
    controller.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
    controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
  }

  public void ApplyTargetHat()
  {
    MinionResume.ApplyHat(this.targetHat, this.GetComponent<KBatchedAnimController>());
    this.currentHat = this.targetHat;
    this.targetHat = (string) null;
  }

  public static void ApplyHat(string hat_id, KBatchedAnimController controller)
  {
    if (hat_id.IsNullOrWhiteSpace())
      MinionResume.RemoveHat(controller);
    else
      MinionResume.AddHat(hat_id, controller);
  }

  public string GetSkillsSubtitle()
  {
    return string.Format((string) DUPLICANTS.NEEDS.QUALITYOFLIFE.TOTAL_SKILL_POINTS, (object) this.TotalSkillPointsGained);
  }

  public static bool AnyMinionHasPerk(string perk, int worldId = -1)
  {
    foreach (MinionResume cmp in worldId >= 0 ? Components.MinionResumes.GetWorldItems(worldId, true) : Components.MinionResumes.Items)
    {
      if (!cmp.HasTag(GameTags.Dead) && cmp.HasPerk((HashedString) perk))
        return true;
    }
    return false;
  }

  public static bool AnyMinionHasAllPerks(string[] perks, int worldId = -1)
  {
    foreach (MinionResume cmp in worldId >= 0 ? Components.MinionResumes.GetWorldItems(worldId, true) : Components.MinionResumes.Items)
    {
      if (!cmp.HasTag(GameTags.Dead))
      {
        bool flag = false;
        foreach (string perk in perks)
        {
          flag |= cmp.HasPerk((HashedString) perk);
          if (!flag)
            break;
        }
        if (flag)
          return true;
      }
    }
    return false;
  }

  public static bool AnyOtherMinionHasPerk(string perk, MinionResume me)
  {
    foreach (MinionResume minionResume in Components.MinionResumes.Items)
    {
      if (!((UnityEngine.Object) minionResume == (UnityEngine.Object) me) && minionResume.HasPerk((HashedString) perk))
        return true;
    }
    return false;
  }

  public void ResetSkillLevels(bool returnSkillPoints = true)
  {
    List<string> stringList = new List<string>();
    foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
    {
      if (keyValuePair.Value)
        stringList.Add(keyValuePair.Key);
    }
    foreach (string skillId in stringList)
      this.UnmasterSkill(skillId);
  }

  public class HatInfo
  {
    public int count;

    public string Source { get; }

    public string Hat { get; }

    public HatInfo(string source, string hat)
    {
      this.Source = source;
      this.Hat = hat;
      this.count = 1;
    }
  }

  public enum SkillMasteryConditions
  {
    SkillAptitude,
    StressWarning,
    UnableToLearn,
    NeedsSkillPoints,
    MissingPreviousSkill,
  }
}
