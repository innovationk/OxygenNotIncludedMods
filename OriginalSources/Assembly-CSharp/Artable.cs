// Decompiled with JetBrains decompiler
// Type: Artable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Artable")]
public class Artable : Workable
{
  [Serialize]
  private string currentStage;
  [Serialize]
  private string userChosenTargetStage;
  private AttributeModifier artQualityDecorModifier;
  public const string defaultArtworkId = "Default";
  public string defaultAnimName;
  public bool onlyWorkableWhenOperational = true;
  private WorkChore<Artable> chore;

  public string CurrentStage => this.currentStage;

  protected Artable()
  {
    this.faceTargetWhenWorking = true;
    if (!string.IsNullOrEmpty(this.requiredSkillPerk))
      return;
    this.requiredSkillPerk = Db.Get().SkillPerks.CanArt.Id;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Arting;
    this.attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Art.Id;
    this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    this.SetWorkTime(80f);
  }

  protected override void OnSpawn()
  {
    this.GetComponent<KPrefabID>().PrefabID();
    if (string.IsNullOrEmpty(this.currentStage) || this.currentStage == "Default")
      this.SetDefault();
    else
      this.SetStage(this.currentStage, true);
    this.shouldShowSkillPerkStatusItem = false;
    base.OnSpawn();
  }

  [System.Runtime.Serialization.OnDeserialized]
  public void OnDeserialized()
  {
    if (Db.GetArtableStages().TryGet(this.currentStage) != null || !(this.currentStage != "Default"))
      return;
    string id = $"{this.GetComponent<KPrefabID>().PrefabID().ToString()}_{this.currentStage}";
    if (Db.GetArtableStages().TryGet(id) == null)
    {
      Debug.LogWarning((object) $"Failed up to update {this.currentStage} to ArtableStages");
      this.currentStage = "Default";
    }
    else
      this.currentStage = id;
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    if (string.IsNullOrEmpty(this.userChosenTargetStage))
    {
      Db db = Db.Get();
      Tag prefab_id = this.GetComponent<KPrefabID>().PrefabID();
      List<ArtableStage> prefabStages = Db.GetArtableStages().GetPrefabStages(prefab_id);
      ArtableStatusItem artist_skill = db.ArtableStatuses.LookingUgly;
      MinionResume component = worker.GetComponent<MinionResume>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        if (component.HasPerk((HashedString) db.SkillPerks.CanArtGreat.Id))
          artist_skill = db.ArtableStatuses.LookingGreat;
        else if (component.HasPerk((HashedString) db.SkillPerks.CanArtOkay.Id))
          artist_skill = db.ArtableStatuses.LookingOkay;
      }
      prefabStages.RemoveAll((Predicate<ArtableStage>) (stage => stage.statusItem.StatusType > artist_skill.StatusType || stage.statusItem.StatusType == ArtableStatuses.ArtableStatusType.AwaitingArting));
      prefabStages.Sort((Comparison<ArtableStage>) ((x, y) => y.statusItem.StatusType.CompareTo((object) x.statusItem.StatusType)));
      ArtableStatuses.ArtableStatusType highest_type = prefabStages[0].statusItem.StatusType;
      prefabStages.RemoveAll((Predicate<ArtableStage>) (stage => stage.statusItem.StatusType < highest_type));
      prefabStages.RemoveAll((Predicate<ArtableStage>) (stage => !stage.IsUnlocked()));
      prefabStages.Shuffle<ArtableStage>();
      this.SetStage(prefabStages[0].id, false);
      if (prefabStages[0].cheerOnComplete)
      {
        EmoteChore emoteChore1 = new EmoteChore((IStateMachineTarget) worker.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer);
      }
      else
      {
        EmoteChore emoteChore2 = new EmoteChore((IStateMachineTarget) worker.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Disappointed);
      }
    }
    else
    {
      this.SetStage(this.userChosenTargetStage, false);
      this.userChosenTargetStage = (string) null;
    }
    this.shouldShowSkillPerkStatusItem = false;
    this.UpdateStatusItem();
    Prioritizable.RemoveRef(this.gameObject);
  }

  public void SetDefault()
  {
    this.currentStage = "Default";
    this.GetComponent<KBatchedAnimController>().SwapAnims(this.GetComponent<Building>().Def.AnimFiles);
    this.GetComponent<KAnimControllerBase>().Play((HashedString) this.defaultAnimName);
    KSelectable component = this.GetComponent<KSelectable>();
    component.SetName(this.GetComponent<Building>().Def.Name);
    component.SetStatusItem(Db.Get().StatusItemCategories.Main, (StatusItem) Db.Get().ArtableStatuses.AwaitingArting, (object) this);
    this.GetAttributes().Remove(this.artQualityDecorModifier);
    this.shouldShowSkillPerkStatusItem = false;
    this.UpdateStatusItem();
    if (this.currentStage == "Default")
    {
      this.shouldShowSkillPerkStatusItem = true;
      Prioritizable.AddRef(this.gameObject);
      this.chore = new WorkChore<Artable>(Db.Get().ChoreTypes.Art, (IStateMachineTarget) this, only_when_operational: this.onlyWorkableWhenOperational);
      this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) this.requiredSkillPerk);
    }
    this.RefreshDecorTag();
    this.Trigger(111068960, (object) this.currentStage);
  }

  public virtual void SetStage(string stage_id, bool skip_effect)
  {
    ArtableStage artableStage = Db.GetArtableStages().Get(stage_id);
    if (artableStage == null)
    {
      Debug.LogError((object) ("Missing stage: " + stage_id));
    }
    else
    {
      this.currentStage = artableStage.id;
      this.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) artableStage.animFile)
      });
      this.GetComponent<KAnimControllerBase>().Play((HashedString) artableStage.anim);
      this.GetAttributes().Remove(this.artQualityDecorModifier);
      if (artableStage.decor != 0)
      {
        this.artQualityDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, (float) artableStage.decor, "Art Quality");
        this.GetAttributes().Add(this.artQualityDecorModifier);
      }
      KSelectable component = this.GetComponent<KSelectable>();
      component.SetName(artableStage.Name);
      component.SetStatusItem(Db.Get().StatusItemCategories.Main, (StatusItem) artableStage.statusItem, (object) this);
      this.gameObject.GetComponent<BuildingComplete>().SetDescriptionFlavour(artableStage.Description);
      this.shouldShowSkillPerkStatusItem = false;
      this.UpdateStatusItem();
      this.RefreshDecorTag();
      this.Trigger(111068960, (object) this.currentStage);
    }
  }

  public void SetUserChosenTargetState(string stageID)
  {
    this.SetDefault();
    this.userChosenTargetStage = stageID;
  }

  public void RefreshDecorTag()
  {
    KPrefabID component = this.GetComponent<KPrefabID>();
    bool flag = component.HasTag(GameTags.Decoration);
    int num = this.CurrentStage == null ? 0 : (this.currentStage != "Default" ? 1 : 0);
    if (num != 0)
      component.AddTag(GameTags.Decoration);
    else
      component.RemoveTag(GameTags.Decoration);
    if (num == (flag ? 1 : 0))
      return;
    Game.Instance.roomProber.TriggerBuildingChangedEvent(Grid.PosToCell(this.gameObject), (object) component);
  }
}
