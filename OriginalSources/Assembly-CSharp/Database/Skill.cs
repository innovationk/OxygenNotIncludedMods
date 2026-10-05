// Decompiled with JetBrains decompiler
// Type: Database.Skill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TUNING;

#nullable disable
namespace Database;

public class Skill : Resource, IHasDlcRestrictions
{
  public string description;
  public string[] requiredDlcIds;
  public string[] forbiddenDlcIds;
  public string skillGroup;
  public string hat;
  public string badge;
  public int tier;
  public bool deprecated;
  public List<SkillPerk> perks;
  public List<string> priorSkills;
  public string requiredDuplicantModel;

  public Skill(
    string id,
    string name,
    string description,
    int tier,
    string hat,
    string badge,
    string skillGroup,
    List<SkillPerk> perks = null,
    List<string> priorSkills = null,
    string requiredDuplicantModel = "Minion",
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null)
    : base(id, name)
  {
    this.description = description;
    this.requiredDlcIds = requiredDlcIds;
    this.forbiddenDlcIds = forbiddenDlcIds;
    this.tier = tier;
    this.hat = hat;
    this.badge = badge;
    this.skillGroup = skillGroup;
    this.perks = perks;
    if (this.perks == null)
      this.perks = new List<SkillPerk>();
    this.priorSkills = priorSkills;
    if (this.priorSkills == null)
      this.priorSkills = new List<string>();
    this.requiredDuplicantModel = requiredDuplicantModel;
  }

  [Obsolete]
  public Skill(
    string id,
    string name,
    string description,
    string dlcId,
    int tier,
    string hat,
    string badge,
    string skillGroup,
    List<SkillPerk> perks = null,
    List<string> priorSkills = null,
    string requiredDuplicantModel = "Minion")
    : this(id, name, description, tier, hat, badge, skillGroup, perks, priorSkills, requiredDuplicantModel)
  {
  }

  public int GetMoraleExpectation() => SKILLS.SKILL_TIER_MORALE_COST[this.tier];

  public bool GivesPerk(SkillPerk perk) => this.perks.Contains(perk);

  public bool GivesPerk(HashedString perkId)
  {
    foreach (Resource perk in this.perks)
    {
      if (perk.IdHash == perkId)
        return true;
    }
    return false;
  }

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;
}
