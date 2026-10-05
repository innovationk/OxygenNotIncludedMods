// Decompiled with JetBrains decompiler
// Type: ArmTrapWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;

#nullable disable
public class ArmTrapWorkable : Workable
{
  public bool WorkInPstAnimation;
  public bool CanBeArmedAtLongDistance;
  public CellOffset[] initialOffsets;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (this.CanBeArmedAtLongDistance)
    {
      this.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
      this.faceTargetWhenWorking = true;
      this.multitoolContext = (HashedString) "build";
      this.multitoolHitEffectTag = (Tag) EffectConfigs.BuildSplashId;
    }
    if (this.initialOffsets != null && this.initialOffsets.Length != 0)
      this.SetOffsets(this.initialOffsets);
    this.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.ArmingTrap);
    this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
    this.attributeConverter = Db.Get().AttributeConverters.CapturableSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.SetWorkTime(5f);
    this.synchronizeAnims = true;
    this.resetProgressOnStop = true;
  }

  public override void OnPendingCompleteWork(WorkerBase worker)
  {
    base.OnPendingCompleteWork(worker);
    this.WorkInPstAnimation = true;
    this.gameObject.Trigger(-2025798095);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    base.OnCompleteWork(worker);
    this.WorkInPstAnimation = false;
  }
}
