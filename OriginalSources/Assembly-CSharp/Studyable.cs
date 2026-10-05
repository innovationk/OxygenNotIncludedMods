// Decompiled with JetBrains decompiler
// Type: Studyable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Studyable")]
public class Studyable : Workable, ISidescreenButtonControl
{
  public string meterTrackerSymbol;
  public string meterAnim;
  private Chore chore;
  private const float STUDY_WORK_TIME = 3600f;
  [Serialize]
  private bool studied;
  [Serialize]
  private bool markedForStudy;
  private Guid statusItemGuid;
  private Guid additionalStatusItemGuid;
  public MeterController studiedIndicator;

  public bool Studied => this.studied;

  public bool Studying => this.chore != null && this.chore.InProgress();

  public string SidescreenTitleKey => "STRINGS.UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.TITLE";

  public string SidescreenStatusMessage
  {
    get
    {
      if (this.studied)
        return (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;
      return this.markedForStudy ? (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS : (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
    }
  }

  public string SidescreenButtonText
  {
    get
    {
      if (this.studied)
        return (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_BUTTON;
      return this.markedForStudy ? (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_BUTTON : (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_BUTTON;
    }
  }

  public string SidescreenButtonTooltip
  {
    get
    {
      if (this.studied)
        return (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;
      return this.markedForStudy ? (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS : (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
    }
  }

  public int HorizontalGroupID() => -1;

  public void SetButtonTextOverride(ButtonMenuTextOverride text)
  {
    throw new NotImplementedException();
  }

  public bool SidescreenEnabled() => true;

  public bool SidescreenButtonInteractable() => !this.studied;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_use_machine_kanim")
    };
    this.faceTargetWhenWorking = true;
    this.synchronizeAnims = false;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Studying;
    this.resetProgressOnStop = false;
    this.requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
    this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
    this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    this.SetWorkTime(3600f);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.studiedIndicator = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), this.meterTrackerSymbol, this.meterAnim, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[1]
    {
      this.meterTrackerSymbol
    });
    this.studiedIndicator.meterController.gameObject.AddComponent<LoopingSounds>();
    this.Refresh();
  }

  public void CancelChore()
  {
    if (this.chore == null)
      return;
    this.chore.Cancel("Studyable.CancelChore");
    this.chore = (Chore) null;
    this.Trigger(1488501379, (object) null);
  }

  public void Refresh()
  {
    if (KMonoBehaviour.isLoadingScene)
      return;
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.studied)
    {
      this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, Db.Get().MiscStatusItems.Studied);
      this.studiedIndicator.gameObject.SetActive(true);
      this.studiedIndicator.meterController.Play((HashedString) this.meterAnim, KAnim.PlayMode.Loop);
      this.requiredSkillPerk = (string) null;
      this.UpdateStatusItem();
    }
    else
    {
      if (this.markedForStudy)
      {
        if (this.chore == null)
          this.chore = (Chore) new WorkChore<Studyable>(Db.Get().ChoreTypes.Research, (IStateMachineTarget) this, only_when_operational: false);
        this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, Db.Get().MiscStatusItems.AwaitingStudy);
      }
      else
      {
        this.CancelChore();
        this.statusItemGuid = component.RemoveStatusItem(this.statusItemGuid);
      }
      this.studiedIndicator.gameObject.SetActive(false);
    }
  }

  private void ToggleStudyChore()
  {
    if (DebugHandler.InstantBuildMode)
    {
      this.studied = true;
      if (this.chore != null)
      {
        this.chore.Cancel("debug");
        this.chore = (Chore) null;
      }
      this.Trigger(-1436775550, (object) null);
    }
    else
      this.markedForStudy = !this.markedForStudy;
    this.Refresh();
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    base.OnCompleteWork(worker);
    this.studied = true;
    this.chore = (Chore) null;
    this.Refresh();
    this.Trigger(-1436775550, (object) null);
    if (!DlcManager.IsExpansion1Active())
      return;
    this.DropDatabanks();
  }

  private void DropDatabanks()
  {
    int num = UnityEngine.Random.Range(7, 13);
    for (int index = 0; index <= num; ++index)
    {
      GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab((Tag) "OrbitalResearchDatabank"), this.transform.position + new Vector3(0.0f, 1f, 0.0f), Grid.SceneLayer.Ore);
      gameObject.GetComponent<PrimaryElement>().Temperature = 298.15f;
      gameObject.SetActive(true);
    }
  }

  public void OnSidescreenButtonPressed() => this.ToggleStudyChore();

  public int ButtonSideScreenSortOrder() => 20;
}
