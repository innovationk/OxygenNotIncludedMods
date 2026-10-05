// Decompiled with JetBrains decompiler
// Type: MorbRoverMakerWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;

#nullable disable
public class MorbRoverMakerWorkable : Workable
{
  public const float DOCTOR_WORKING_TIME = 90f;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workingStatusItem = Db.Get().BuildingStatusItems.MorbRoverMakerDoctorWorking;
    this.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.MorbRoverMakerDoctorWorking);
    this.requiredSkillPerk = Db.Get().SkillPerks.CanAdvancedMedicine.Id;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_gravitas_morb_tank_kanim")
    };
    this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.lightEfficiencyBonus = true;
    this.synchronizeAnims = true;
    this.shouldShowSkillPerkStatusItem = true;
    this.SetWorkTime(90f);
    this.resetProgressOnStop = true;
  }

  protected override void OnSpawn() => base.OnSpawn();

  protected override void OnStartWork(WorkerBase worker) => base.OnStartWork(worker);
}
