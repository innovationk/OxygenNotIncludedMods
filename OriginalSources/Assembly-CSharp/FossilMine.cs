// Decompiled with JetBrains decompiler
// Type: FossilMine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;

#nullable disable
public class FossilMine : ComplexFabricator
{
  [MyCmpAdd]
  protected FossilMineSM fabricatorSM;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.fabricatorSM.idleAnimationName = "idle";
    this.fabricatorSM.idleQueue_StatusItem = Db.Get().BuildingStatusItems.FossilMineIdle;
    this.fabricatorSM.waitingForMaterial_StatusItem = Db.Get().BuildingStatusItems.FossilMineEmpty;
    this.fabricatorSM.waitingForWorker_StatusItem = Db.Get().BuildingStatusItems.FossilMinePendingWork;
    this.SideScreenSubtitleLabel = (string) CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FABRICATOR_LIST_TITLE;
    this.SideScreenRecipeScreenTitle = (string) CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FABRICATOR_RECIPE_SCREEN_TITLE;
    this.choreType = Db.Get().ChoreTypes.Art;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanArtGreat.Id;
    this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Digging;
    this.workable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_fossil_dig_kanim")
    };
    this.workable.AttributeConverter = Db.Get().AttributeConverters.ArtSpeed;
    this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Art.Id;
    this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
  }

  public void SetActiveState(bool active)
  {
    if (active)
    {
      this.inStorage.showInUI = true;
      this.buildStorage.showInUI = true;
      this.outStorage.showInUI = true;
      this.fabricatorSM.Activate();
      if (this.workable is FossilMineWorkable)
        (this.workable as FossilMineWorkable).SetShouldShowSkillPerkStatusItem(true);
      this.enabled = active;
    }
    else
    {
      this.OnDisable();
      this.fabricatorSM.Deactivate();
      this.inStorage.showInUI = false;
      this.buildStorage.showInUI = false;
      this.outStorage.showInUI = false;
      if (this.workable is FossilMineWorkable)
        (this.workable as FossilMineWorkable).SetShouldShowSkillPerkStatusItem(false);
      this.enabled = false;
    }
  }
}
