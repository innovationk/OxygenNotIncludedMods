// Decompiled with JetBrains decompiler
// Type: SpaceTreeSyrupHarvestWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;

#nullable disable
public class SpaceTreeSyrupHarvestWorkable : Workable
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.Harvesting);
    this.workAnims = new HashedString[2]
    {
      (HashedString) "syrup_harvest_trunk_pre",
      (HashedString) "syrup_harvest_trunk_loop"
    };
    this.workingPstComplete = new HashedString[1]
    {
      (HashedString) "syrup_harvest_trunk_pst"
    };
    this.workingPstFailed = new HashedString[1]
    {
      (HashedString) "syrup_harvest_trunk_loop"
    };
    this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_syrup_tree_kanim")
    };
    this.synchronizeAnims = true;
    this.shouldShowSkillPerkStatusItem = false;
    this.SetWorkTime(10f);
    this.resetProgressOnStop = true;
  }

  protected override void OnSpawn() => base.OnSpawn();

  protected override void OnStartWork(WorkerBase worker) => base.OnStartWork(worker);
}
