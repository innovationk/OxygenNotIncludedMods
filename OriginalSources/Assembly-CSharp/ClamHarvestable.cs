// Decompiled with JetBrains decompiler
// Type: ClamHarvestable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using TUNING;

#nullable disable
public class ClamHarvestable : Harvestable
{
  [Serialize]
  protected bool hasBeenOpened;
  private StandardCropPlant standardCropPlant;
  private Growing growing;

  public bool IsClosedAndReadyForHarvesting => !this.hasBeenOpened && this.growing.IsGrown();

  protected override void OnPrefabInit()
  {
    this.standardCropPlant = this.GetComponent<StandardCropPlant>();
    this.growing = this.GetComponent<Growing>();
    Components.ClamHarvestables.Add(this);
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.skillsUpdateHandle != -1)
      Game.Instance.Unsubscribe(this.skillsUpdateHandle);
    this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, Workable.UpdateStatusItemDispatcher, (object) this);
    this.Subscribe(-266953818, new Action<object>(this.OnHarvestDesignationChanged));
    this.Subscribe(1272413801, new Action<object>(this.OnHarvested));
    this.SetupWorkable();
    this.UpdateHarvestReadyAnimations();
  }

  private void SetupWorkable()
  {
    if (this.hasBeenOpened)
    {
      this.SetupWorkableForHarvestClam();
      this.growing.shouldGrowOld = true;
      this.growing.smi.ModifyOldAgeGrowthRate(1f);
    }
    else
    {
      this.SetupWorkableForOpenClam();
      this.growing.shouldGrowOld = false;
      this.growing.smi.ModifyOldAgeGrowthRate(0.0f);
    }
    this.UpdateStatusItem();
  }

  public override void OnMarkedForHarvest()
  {
    base.OnMarkedForHarvest();
    this.SetupWorkable();
  }

  private void OnHarvestDesignationChanged(object data) => this.SetupWorkable();

  private void OnHarvested(object data)
  {
    if (!this.hasBeenOpened)
      return;
    this.hasBeenOpened = false;
    this.UpdateHarvestReadyAnimations(false);
    this.SetupWorkable();
  }

  private void SetupWorkableForOpenClam()
  {
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Harvesting;
    this.multitoolContext = new HashedString();
    this.multitoolHitEffectTag = (Tag) (string) null;
    this.faceTargetWhenWorking = true;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_sculpture_kanim")
    };
    this.synchronizeAnims = false;
    this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.requiredSkillPerk = Db.Get().SkillPerks.CanFarmClams.Id;
    this.shouldShowSkillPerkStatusItem = this.CanBeHarvested && this.harvestDesignatable.HarvestWhenReady;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    if (this.offsetTracker != null)
    {
      this.offsetTracker.Clear();
      this.offsetTracker = (OffsetTracker) null;
    }
    this.SetWorkTime(10f);
  }

  private void SetupWorkableForHarvestClam()
  {
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Harvesting;
    this.multitoolContext = (HashedString) "harvest";
    this.multitoolHitEffectTag = (Tag) "fx_harvest_splash";
    this.faceTargetWhenWorking = true;
    this.overrideAnims = (KAnimFile[]) null;
    this.synchronizeAnims = false;
    this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.requiredSkillPerk = (string) null;
    this.shouldShowSkillPerkStatusItem = false;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.SetWorkTime(10f);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    if (!this.hasBeenOpened)
    {
      this.hasBeenOpened = true;
      this.UpdateHarvestReadyAnimations();
      this.chore = (Chore) null;
      this.SetupWorkable();
    }
    else
      base.OnCompleteWork(worker);
  }

  public void PunchOpen()
  {
    if (this.hasBeenOpened)
      return;
    this.hasBeenOpened = true;
    if ((UnityEngine.Object) this.worker != (UnityEngine.Object) null && this.chore != null)
      this.chore.Cancel("Punched open while being worked on");
    this.UpdateHarvestReadyAnimations();
    this.chore = (Chore) null;
    this.SetupWorkable();
  }

  private void UpdateHarvestReadyAnimations(bool refresh = true)
  {
    this.standardCropPlant.anims = !this.hasBeenOpened ? ClamConfig.CROP_PLANT_CLOSED_ANIM_SET : ClamConfig.CROP_PLANT_DEFAULT_ANIM_SET;
    if ((!refresh || !this.standardCropPlant.smi.IsInsideState((StateMachine.BaseState) this.standardCropPlant.smi.sm.alive.fruiting)) && !this.standardCropPlant.smi.IsInsideState((StateMachine.BaseState) this.standardCropPlant.smi.sm.alive.pre_fruiting))
      return;
    this.standardCropPlant.smi.GoTo((StateMachine.BaseState) this.standardCropPlant.smi.sm.alive.idle);
  }

  protected override void OnCleanUp()
  {
    Components.ClamHarvestables.Remove(this);
    base.OnCleanUp();
  }
}
