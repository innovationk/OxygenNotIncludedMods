// Decompiled with JetBrains decompiler
// Type: EmptyLitterboxWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;

#nullable disable
public class EmptyLitterboxWorkable : Workable
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workingStatusItem = Db.Get().BuildingStatusItems.LitterBoxBeingEmptied;
    this.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.EmptyingLitterBox);
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_compost_kanim")
    };
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.lightEfficiencyBonus = true;
    this.synchronizeAnims = true;
    this.shouldShowSkillPerkStatusItem = false;
    this.SetWorkTime(15f);
  }
}
