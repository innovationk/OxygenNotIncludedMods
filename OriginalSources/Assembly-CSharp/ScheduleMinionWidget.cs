// Decompiled with JetBrains decompiler
// Type: ScheduleMinionWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleMinionWidget")]
public class ScheduleMinionWidget : KMonoBehaviour
{
  [SerializeField]
  private CrewPortrait portrait;
  [SerializeField]
  private DropDown dropDown;
  [SerializeField]
  private LocText label;
  [SerializeField]
  private GameObject nightOwlIcon;
  [SerializeField]
  private GameObject earlyBirdIcon;
  [SerializeField]
  private GameObject worldContainer;

  public Schedulable schedulable { get; private set; }

  public void ChangeAssignment(Schedule targetSchedule, Schedulable schedulable)
  {
    DebugUtil.LogArgs((object) "Assigning", (object) schedulable, (object) "from", (object) ScheduleManager.Instance.GetSchedule(schedulable).name, (object) "to", (object) targetSchedule.name);
    ScheduleManager.Instance.GetSchedule(schedulable).Unassign(schedulable);
    targetSchedule.Assign(schedulable);
  }

  public void Setup(Schedulable schedulable)
  {
    this.schedulable = schedulable;
    IAssignableIdentity component1 = schedulable.GetComponent<IAssignableIdentity>();
    this.portrait.SetIdentityObject(component1);
    this.label.text = component1.GetProperName();
    MinionIdentity minionIdentity = component1 as MinionIdentity;
    StoredMinionIdentity storedMinionIdentity = component1 as StoredMinionIdentity;
    this.RefreshWidgetWorldData();
    if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
    {
      Traits component2 = minionIdentity.GetComponent<Traits>();
      if (component2.HasTrait("NightOwl"))
        this.nightOwlIcon.SetActive(true);
      else if (component2.HasTrait("EarlyBird"))
        this.earlyBirdIcon.SetActive(true);
    }
    else if ((UnityEngine.Object) storedMinionIdentity != (UnityEngine.Object) null)
    {
      if (storedMinionIdentity.traitIDs.Contains("NightOwl"))
        this.nightOwlIcon.SetActive(true);
      else if (storedMinionIdentity.traitIDs.Contains("EarlyBird"))
        this.earlyBirdIcon.SetActive(true);
    }
    this.dropDown.Initialize(ScheduleManager.Instance.GetSchedules().Cast<IListableOption>(), new Action<IListableOption, object>(this.OnDropEntryClick), refreshAction: new Action<DropDownEntry, object>(this.DropEntryRefreshAction), displaySelectedValueWhenClosed: false, targetData: (object) schedulable);
  }

  public void RefreshWidgetWorldData()
  {
    this.worldContainer.SetActive(DlcManager.IsExpansion1Active());
    MinionIdentity component = this.schedulable.GetComponent<IAssignableIdentity>() as MinionIdentity;
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.label.text = component.GetProperName();
    if (!DlcManager.IsExpansion1Active())
      return;
    WorldContainer myWorld = component.GetMyWorld();
    string sourceText = myWorld.GetComponent<ClusterGridEntity>().Name;
    Image componentInChildren = this.worldContainer.GetComponentInChildren<Image>();
    componentInChildren.sprite = myWorld.GetComponent<ClusterGridEntity>().GetUISprite();
    componentInChildren.SetAlpha((UnityEngine.Object) ClusterManager.Instance.activeWorld == (UnityEngine.Object) myWorld ? 1f : 0.7f);
    if ((UnityEngine.Object) ClusterManager.Instance.activeWorld != (UnityEngine.Object) myWorld)
      sourceText = $"<color={Constants.NEUTRAL_COLOR_STR}>{sourceText}</color>";
    this.worldContainer.GetComponentInChildren<LocText>().SetText(sourceText);
  }

  private void OnDropEntryClick(IListableOption option, object obj)
  {
    this.ChangeAssignment((Schedule) option, this.schedulable);
  }

  private void DropEntryRefreshAction(DropDownEntry entry, object obj)
  {
    Schedule entryData = (Schedule) entry.entryData;
    if (((Schedulable) obj).GetSchedule() == entryData)
    {
      entry.label.text = string.Format((string) STRINGS.UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_ASSIGNED, (object) entryData.name);
      entry.button.isInteractable = false;
    }
    else
    {
      entry.label.text = entryData.name;
      entry.button.isInteractable = true;
    }
    entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("worldContainer").gameObject.SetActive(false);
    entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("ScheduleIcon").gameObject.SetActive(true);
    entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("PortraitContainer").gameObject.SetActive(false);
  }

  public void SetupBlank(Schedule schedule)
  {
    this.label.text = (string) STRINGS.UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_BLANK;
    this.dropDown.Initialize(Components.LiveMinionIdentities.Items.Cast<IListableOption>(), new Action<IListableOption, object>(this.OnBlankDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.BlankDropEntrySort), new Action<DropDownEntry, object>(this.BlankDropEntryRefreshAction), false, (object) schedule);
    Components.LiveMinionIdentities.OnAdd += new Action<MinionIdentity>(this.OnLivingMinionsChanged);
    Components.LiveMinionIdentities.OnRemove += new Action<MinionIdentity>(this.OnLivingMinionsChanged);
  }

  private void OnLivingMinionsChanged(MinionIdentity minion)
  {
    this.dropDown.ChangeContent(Components.LiveMinionIdentities.Items.Cast<IListableOption>());
  }

  private void OnBlankDropEntryClick(IListableOption option, object obj)
  {
    Schedule targetSchedule = (Schedule) obj;
    MinionIdentity cmp = (MinionIdentity) option;
    if ((UnityEngine.Object) cmp == (UnityEngine.Object) null || cmp.HasTag(GameTags.Dead))
      return;
    this.ChangeAssignment(targetSchedule, cmp.GetComponent<Schedulable>());
  }

  private void BlankDropEntryRefreshAction(DropDownEntry entry, object obj)
  {
    Schedule schedule = (Schedule) obj;
    MinionIdentity entryData = (MinionIdentity) entry.entryData;
    WorldContainer myWorld = entryData.GetMyWorld();
    entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("worldContainer").gameObject.SetActive(DlcManager.IsExpansion1Active());
    Image reference = entry.gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("worldIcon");
    reference.sprite = myWorld.GetComponent<ClusterGridEntity>().GetUISprite();
    reference.SetAlpha((UnityEngine.Object) ClusterManager.Instance.activeWorld == (UnityEngine.Object) myWorld ? 1f : 0.7f);
    string sourceText = myWorld.GetComponent<ClusterGridEntity>().Name;
    if ((UnityEngine.Object) ClusterManager.Instance.activeWorld != (UnityEngine.Object) myWorld)
      sourceText = $"<color={Constants.NEUTRAL_COLOR_STR}>{sourceText}</color>";
    entry.gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("worldLabel").SetText(sourceText);
    if (schedule.IsAssigned(entryData.GetComponent<Schedulable>()))
    {
      entry.label.text = string.Format((string) STRINGS.UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_ASSIGNED, (object) entryData.GetProperName());
      entry.button.isInteractable = false;
    }
    else
    {
      entry.label.text = entryData.GetProperName();
      entry.button.isInteractable = true;
    }
    Traits component = entryData.GetComponent<Traits>();
    entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("NightOwlIcon").gameObject.SetActive(component.HasTrait("NightOwl"));
    entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("EarlyBirdIcon").gameObject.SetActive(component.HasTrait("EarlyBird"));
    entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("ScheduleIcon").gameObject.SetActive(false);
    entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("PortraitContainer").gameObject.SetActive(true);
  }

  private int BlankDropEntrySort(IListableOption a, IListableOption b, object obj)
  {
    Schedule schedule = (Schedule) obj;
    MinionIdentity minionIdentity1 = (MinionIdentity) a;
    MinionIdentity minionIdentity2 = (MinionIdentity) b;
    bool flag1 = schedule.IsAssigned(minionIdentity1.GetComponent<Schedulable>());
    bool flag2 = schedule.IsAssigned(minionIdentity2.GetComponent<Schedulable>());
    if (flag1 && !flag2)
      return -1;
    return !flag1 & flag2 ? 1 : 0;
  }

  protected override void OnCleanUp()
  {
    Components.LiveMinionIdentities.OnAdd -= new Action<MinionIdentity>(this.OnLivingMinionsChanged);
    Components.LiveMinionIdentities.OnRemove -= new Action<MinionIdentity>(this.OnLivingMinionsChanged);
  }
}
