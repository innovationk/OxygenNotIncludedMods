// Decompiled with JetBrains decompiler
// Type: PartyCakeWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;

#nullable disable
public class PartyCakeWorkable : Workable
{
  private static readonly HashedString[] WORK_ANIMS = new HashedString[2]
  {
    (HashedString) "salt_pre",
    (HashedString) "salt_loop"
  };
  private static readonly HashedString PST_ANIM = new HashedString("salt_pst");

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
    this.alwaysShowProgressBar = true;
    this.resetProgressOnStop = false;
    this.attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_desalinator_kanim")
    };
    this.workAnims = PartyCakeWorkable.WORK_ANIMS;
    this.workingPstComplete = new HashedString[1]
    {
      PartyCakeWorkable.PST_ANIM
    };
    this.workingPstFailed = new HashedString[1]
    {
      PartyCakeWorkable.PST_ANIM
    };
    this.synchronizeAnims = false;
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    base.OnWorkTick(worker, dt);
    this.GetComponent<KBatchedAnimController>().SetPositionPercent(this.GetPercentComplete());
    return false;
  }
}
