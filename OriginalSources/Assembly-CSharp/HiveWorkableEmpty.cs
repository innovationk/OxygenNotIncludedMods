// Decompiled with JetBrains decompiler
// Type: HiveWorkableEmpty
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/HiveWorkableEmpty")]
public class HiveWorkableEmpty : Workable
{
  private static readonly HashedString[] WORK_ANIMS = new HashedString[2]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop"
  };
  private static readonly HashedString PST_ANIM = new HashedString("working_pst");
  public bool wasStung;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
    this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.workAnims = HiveWorkableEmpty.WORK_ANIMS;
    this.workingPstComplete = new HashedString[1]
    {
      HiveWorkableEmpty.PST_ANIM
    };
    this.workingPstFailed = new HashedString[1]
    {
      HiveWorkableEmpty.PST_ANIM
    };
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    base.OnCompleteWork(worker);
    if (this.wasStung)
      return;
    SaveGame.Instance.ColonyAchievementTracker.harvestAHiveWithoutGettingStung = true;
  }
}
