// Decompiled with JetBrains decompiler
// Type: Database.SkillBranchComplete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

#nullable disable
namespace Database;

public class SkillBranchComplete : 
  ColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  private List<Skill> skillsToMaster;

  public SkillBranchComplete(List<Skill> skillsToMaster) => this.skillsToMaster = skillsToMaster;

  public override bool Success()
  {
    foreach (MinionResume minionResume in Components.MinionResumes.Items)
    {
      foreach (Skill skill1 in this.skillsToMaster)
      {
        if (minionResume.HasMasteredSkill(skill1.Id))
        {
          if (!minionResume.HasBeenGrantedSkill(skill1))
            return true;
          List<Skill> allPriorSkills = Db.Get().Skills.GetAllPriorSkills(skill1);
          bool flag = true;
          foreach (Skill skill2 in allPriorSkills)
            flag = flag && minionResume.HasMasteredSkill(skill2.Id);
          if (flag)
            return true;
        }
      }
    }
    return false;
  }

  public void Deserialize(IReader reader)
  {
    this.skillsToMaster = new List<Skill>();
    int num = reader.ReadInt32();
    for (int index = 0; index < num; ++index)
    {
      string id = reader.ReadKleiString();
      this.skillsToMaster.Add(Db.Get().Skills.Get(id));
    }
  }

  public override string GetProgress(bool complete)
  {
    return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SKILL_BRANCH;
  }
}
