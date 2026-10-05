// Decompiled with JetBrains decompiler
// Type: SkillsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SkillsScreen : KModalScreen
{
  [SerializeField]
  private KButton CloseButton;
  [Header("Prefabs")]
  [SerializeField]
  private GameObject Prefab_skillWidget;
  [SerializeField]
  private GameObject Prefab_skillColumn;
  [SerializeField]
  private GameObject Prefab_minion;
  [SerializeField]
  private GameObject Prefab_minionLayout;
  [SerializeField]
  private GameObject Prefab_tableLayout;
  [SerializeField]
  private GameObject Prefab_worldDivider;
  [Header("Sort Toggles")]
  [SerializeField]
  private MultiToggle dupeSortingToggle;
  [SerializeField]
  private MultiToggle experienceSortingToggle;
  [SerializeField]
  private MultiToggle moraleSortingToggle;
  private MultiToggle activeSortToggle;
  private bool sortReversed;
  private Comparison<IAssignableIdentity> active_sort_method;
  [Header("Duplicant Animation")]
  [SerializeField]
  private FullBodyUIMinionWidget minionAnimWidget;
  [Header("Progress Bars")]
  [SerializeField]
  private ToolTip expectationsTooltip;
  [SerializeField]
  private LocText moraleProgressLabel;
  [SerializeField]
  private GameObject moraleWarning;
  [SerializeField]
  private GameObject moraleNotch;
  [SerializeField]
  private Color moraleNotchColor;
  private List<GameObject> moraleNotches = new List<GameObject>();
  [SerializeField]
  private LocText expectationsProgressLabel;
  [SerializeField]
  private GameObject expectationWarning;
  [SerializeField]
  private GameObject expectationNotch;
  [SerializeField]
  private Color expectationNotchColor;
  [SerializeField]
  private Color expectationNotchProspectColor;
  private List<GameObject> expectationNotches = new List<GameObject>();
  [SerializeField]
  private ToolTip experienceBarTooltip;
  [SerializeField]
  private Image experienceProgressFill;
  [SerializeField]
  private LocText EXPCount;
  [SerializeField]
  private LocText duplicantLevelIndicator;
  [SerializeField]
  private KScrollRect scrollRect;
  [SerializeField]
  private float scrollSpeed = 7f;
  [SerializeField]
  private DropDown hatDropDown;
  [SerializeField]
  public Image selectedHat;
  [SerializeField]
  private GameObject skillsContainer;
  [SerializeField]
  private GameObject boosterPanel;
  [SerializeField]
  private GameObject boosterHeader;
  [SerializeField]
  private GameObject boosterContentGrid;
  [SerializeField]
  private GameObject boosterPrefab;
  private Dictionary<Tag, HierarchyReferences> boosterWidgets = new Dictionary<Tag, HierarchyReferences>();
  [SerializeField]
  private LocText equippedBoostersHeaderLabel;
  [SerializeField]
  private LocText assignedBoostersCountLabel;
  [SerializeField]
  private GameObject boosterSlotIconPrefab;
  private List<GameObject> boosterSlotIcons = new List<GameObject>();
  private IAssignableIdentity currentlySelectedMinion;
  private List<GameObject> rows = new List<GameObject>();
  private List<SkillMinionWidget> sortableRows = new List<SkillMinionWidget>();
  private Dictionary<int, GameObject> worldDividers = new Dictionary<int, GameObject>();
  private string hoveredSkillID = "";
  private Dictionary<string, GameObject> skillWidgets = new Dictionary<string, GameObject>();
  private Dictionary<string, int> skillGroupRow = new Dictionary<string, int>();
  private List<GameObject> skillColumns = new List<GameObject>();
  private bool dirty;
  private bool linesPending;
  private int layoutRowHeight = 80 /*0x50*/;
  private Coroutine delayRefreshRoutine;
  protected Comparison<IAssignableIdentity> compareByExperience = (Comparison<IAssignableIdentity>) ((a, b) =>
  {
    GameObject targetGameObject1 = ((MinionAssignablesProxy) a).GetTargetGameObject();
    GameObject targetGameObject2 = ((MinionAssignablesProxy) b).GetTargetGameObject();
    if ((UnityEngine.Object) targetGameObject1 == (UnityEngine.Object) null && (UnityEngine.Object) targetGameObject2 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) targetGameObject1 == (UnityEngine.Object) null)
      return -1;
    if ((UnityEngine.Object) targetGameObject2 == (UnityEngine.Object) null)
      return 1;
    MinionResume component1 = targetGameObject1.GetComponent<MinionResume>();
    MinionResume component2 = targetGameObject2.GetComponent<MinionResume>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null && (UnityEngine.Object) component2 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return -1;
    return (UnityEngine.Object) component2 == (UnityEngine.Object) null ? 1 : ((float) component1.AvailableSkillpoints).CompareTo((float) component2.AvailableSkillpoints);
  });
  protected Comparison<IAssignableIdentity> compareByMinion = (Comparison<IAssignableIdentity>) ((a, b) => a.GetProperName().CompareTo(b.GetProperName()));
  protected Comparison<IAssignableIdentity> compareByMorale = (Comparison<IAssignableIdentity>) ((a, b) =>
  {
    GameObject targetGameObject3 = ((MinionAssignablesProxy) a).GetTargetGameObject();
    GameObject targetGameObject4 = ((MinionAssignablesProxy) b).GetTargetGameObject();
    if ((UnityEngine.Object) targetGameObject3 == (UnityEngine.Object) null && (UnityEngine.Object) targetGameObject4 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) targetGameObject3 == (UnityEngine.Object) null)
      return -1;
    if ((UnityEngine.Object) targetGameObject4 == (UnityEngine.Object) null)
      return 1;
    MinionResume component3 = targetGameObject3.GetComponent<MinionResume>();
    MinionResume component4 = targetGameObject4.GetComponent<MinionResume>();
    if ((UnityEngine.Object) component3 == (UnityEngine.Object) null && (UnityEngine.Object) component4 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) component3 == (UnityEngine.Object) null)
      return -1;
    if ((UnityEngine.Object) component4 == (UnityEngine.Object) null)
      return 1;
    AttributeInstance attributeInstance1 = Db.Get().Attributes.QualityOfLife.Lookup((Component) component3);
    Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) component3);
    AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLife.Lookup((Component) component4);
    Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) component4);
    return attributeInstance1.GetTotalValue().CompareTo(attributeInstance2.GetTotalValue());
  });

  public override float GetSortKey() => this.isEditing ? 50f : 20f;

  public IAssignableIdentity CurrentlySelectedMinion
  {
    get
    {
      return this.currentlySelectedMinion == null || this.currentlySelectedMinion.IsNull() ? (IAssignableIdentity) null : this.currentlySelectedMinion;
    }
    set
    {
      this.currentlySelectedMinion = value;
      if (!this.IsActive())
        return;
      this.RefreshSelectedMinion();
      this.RefreshSkillWidgets();
      this.RefreshBoosters();
    }
  }

  protected override void OnSpawn()
  {
    ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.WorldRemoved));
  }

  protected override void OnActivate()
  {
    this.ConsumeMouseScroll = true;
    base.OnActivate();
    this.BuildMinions();
    this.RefreshAll();
    this.SortRows(this.active_sort_method == null ? this.compareByMinion : this.active_sort_method);
    Components.LiveMinionIdentities.OnAdd += new Action<MinionIdentity>(this.OnAddMinionIdentity);
    Components.LiveMinionIdentities.OnRemove += new Action<MinionIdentity>(this.OnRemoveMinionIdentity);
    this.CloseButton.onClick += (System.Action) (() => ManagementMenu.Instance.CloseAll());
    this.dupeSortingToggle.onClick += (System.Action) (() => this.SortRows(this.compareByMinion));
    this.moraleSortingToggle.onClick += (System.Action) (() => this.SortRows(this.compareByMorale));
    this.experienceSortingToggle.onClick += (System.Action) (() => this.SortRows(this.compareByExperience));
  }

  protected override void OnShow(bool show)
  {
    if (show)
    {
      if (this.CurrentlySelectedMinion == null && Components.LiveMinionIdentities.Count > 0)
        this.CurrentlySelectedMinion = (IAssignableIdentity) Components.LiveMinionIdentities.Items[0];
      this.BuildMinions();
      if (this.boosterWidgets.Count == 0)
        this.PopulateBoosters();
      this.RefreshAll();
      this.SortRows(this.active_sort_method == null ? this.compareByMinion : this.active_sort_method);
    }
    base.OnShow(show);
  }

  public void RefreshAll()
  {
    this.dirty = false;
    this.RefreshSkillWidgets();
    this.RefreshSelectedMinion();
    this.RefreshBoosters();
    this.linesPending = true;
  }

  private void RefreshSelectedMinion()
  {
    this.minionAnimWidget.SetPortraitAnimator(this.currentlySelectedMinion);
    this.RefreshProgressBars();
    this.RefreshHat();
  }

  public void GetMinionIdentity(
    IAssignableIdentity assignableIdentity,
    out MinionIdentity minionIdentity,
    out StoredMinionIdentity storedMinionIdentity)
  {
    if (assignableIdentity is MinionAssignablesProxy)
    {
      minionIdentity = ((MinionAssignablesProxy) assignableIdentity).GetTargetGameObject().GetComponent<MinionIdentity>();
      storedMinionIdentity = ((MinionAssignablesProxy) assignableIdentity).GetTargetGameObject().GetComponent<StoredMinionIdentity>();
    }
    else
    {
      minionIdentity = assignableIdentity as MinionIdentity;
      storedMinionIdentity = assignableIdentity as StoredMinionIdentity;
    }
  }

  private void RefreshProgressBars()
  {
    if (this.currentlySelectedMinion == null || this.currentlySelectedMinion.IsNull())
      return;
    MinionIdentity minionIdentity;
    StoredMinionIdentity storedMinionIdentity;
    this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
    HierarchyReferences component1 = this.expectationsTooltip.GetComponent<HierarchyReferences>();
    component1.GetReference("Labels").gameObject.SetActive((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null);
    component1.GetReference("MoraleBar").gameObject.SetActive((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null);
    component1.GetReference("ExpectationBar").gameObject.SetActive((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null);
    component1.GetReference("StoredMinion").gameObject.SetActive((UnityEngine.Object) minionIdentity == (UnityEngine.Object) null);
    this.experienceProgressFill.gameObject.SetActive((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null);
    if ((UnityEngine.Object) minionIdentity == (UnityEngine.Object) null)
    {
      this.expectationsTooltip.SetSimpleTooltip(string.Format((string) STRINGS.UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, (object) storedMinionIdentity.GetStorageReason(), (object) this.currentlySelectedMinion.GetProperName()));
      this.experienceBarTooltip.SetSimpleTooltip(string.Format((string) STRINGS.UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, (object) storedMinionIdentity.GetStorageReason(), (object) this.currentlySelectedMinion.GetProperName()));
      this.EXPCount.text = "";
      this.duplicantLevelIndicator.text = (string) STRINGS.UI.TABLESCREENS.NA;
    }
    else
    {
      MinionResume component2 = minionIdentity.GetComponent<MinionResume>();
      float previousExperienceBar = MinionResume.CalculatePreviousExperienceBar(component2.TotalSkillPointsGained);
      float nextExperienceBar = MinionResume.CalculateNextExperienceBar(component2.TotalSkillPointsGained);
      float num1 = (float) (((double) component2.TotalExperienceGained - (double) previousExperienceBar) / ((double) nextExperienceBar - (double) previousExperienceBar));
      this.EXPCount.text = $"{Mathf.RoundToInt(component2.TotalExperienceGained - previousExperienceBar).ToString()} / {Mathf.RoundToInt(nextExperienceBar - previousExperienceBar).ToString()}";
      this.duplicantLevelIndicator.text = component2.AvailableSkillpoints.ToString();
      this.experienceProgressFill.fillAmount = num1;
      this.experienceBarTooltip.SetSimpleTooltip(string.Format((string) STRINGS.UI.SKILLS_SCREEN.EXPERIENCE_TOOLTIP, (object) (Mathf.RoundToInt(nextExperienceBar - previousExperienceBar) - Mathf.RoundToInt(component2.TotalExperienceGained - previousExperienceBar))));
      AttributeInstance attributeInstance1 = Db.Get().Attributes.QualityOfLife.Lookup((Component) component2);
      AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) component2);
      float num2 = 0.0f;
      float num3 = 0.0f;
      if (!string.IsNullOrEmpty(this.hoveredSkillID) && !component2.HasMasteredSkill(this.hoveredSkillID))
      {
        List<string> stringList = new List<string>();
        List<string> collection = new List<string>();
        stringList.Add(this.hoveredSkillID);
        while (stringList.Count > 0)
        {
          for (int index = stringList.Count - 1; index >= 0; --index)
          {
            if (!component2.HasMasteredSkill(stringList[index]))
            {
              num2 += (float) (Db.Get().Skills.Get(stringList[index]).tier + 1);
              if (component2.AptitudeBySkillGroup.ContainsKey((HashedString) Db.Get().Skills.Get(stringList[index]).skillGroup) && (double) component2.AptitudeBySkillGroup[(HashedString) Db.Get().Skills.Get(stringList[index]).skillGroup] > 0.0)
                ++num3;
              foreach (string priorSkill in Db.Get().Skills.Get(stringList[index]).priorSkills)
                collection.Add(priorSkill);
            }
          }
          stringList.Clear();
          stringList.AddRange((IEnumerable<string>) collection);
          collection.Clear();
        }
      }
      float num4 = attributeInstance1.GetTotalValue() + num3 / (attributeInstance2.GetTotalValue() + num2);
      float f = Mathf.Max(attributeInstance1.GetTotalValue() + num3, attributeInstance2.GetTotalValue() + num2);
      while (this.moraleNotches.Count < Mathf.RoundToInt(f))
      {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.moraleNotch, this.moraleNotch.transform.parent);
        gameObject.SetActive(true);
        this.moraleNotches.Add(gameObject);
      }
      while (this.moraleNotches.Count > Mathf.RoundToInt(f))
      {
        GameObject moraleNotch = this.moraleNotches[this.moraleNotches.Count - 1];
        this.moraleNotches.Remove(moraleNotch);
        UnityEngine.Object.Destroy((UnityEngine.Object) moraleNotch);
      }
      for (int index = 0; index < this.moraleNotches.Count; ++index)
      {
        if ((double) index < (double) attributeInstance1.GetTotalValue() + (double) num3)
          this.moraleNotches[index].GetComponentsInChildren<Image>()[1].color = this.moraleNotchColor;
        else
          this.moraleNotches[index].GetComponentsInChildren<Image>()[1].color = Color.clear;
      }
      this.moraleProgressLabel.text = $"{(string) STRINGS.UI.SKILLS_SCREEN.MORALE}: {attributeInstance1.GetTotalValue().ToString()}";
      if ((double) num3 > 0.0)
      {
        LocText moraleProgressLabel = this.moraleProgressLabel;
        moraleProgressLabel.text = $"{moraleProgressLabel.text} + {GameUtil.ApplyBoldString(GameUtil.ColourizeString((Color32) this.moraleNotchColor, num3.ToString()))}";
      }
      while (this.expectationNotches.Count < Mathf.RoundToInt(f))
      {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.expectationNotch, this.expectationNotch.transform.parent);
        gameObject.SetActive(true);
        this.expectationNotches.Add(gameObject);
      }
      while (this.expectationNotches.Count > Mathf.RoundToInt(f))
      {
        GameObject expectationNotch = this.expectationNotches[this.expectationNotches.Count - 1];
        this.expectationNotches.Remove(expectationNotch);
        UnityEngine.Object.Destroy((UnityEngine.Object) expectationNotch);
      }
      for (int index = 0; index < this.expectationNotches.Count; ++index)
      {
        if ((double) index < (double) attributeInstance2.GetTotalValue() + (double) num2)
        {
          if ((double) index < (double) attributeInstance2.GetTotalValue())
            this.expectationNotches[index].GetComponentsInChildren<Image>()[1].color = this.expectationNotchColor;
          else
            this.expectationNotches[index].GetComponentsInChildren<Image>()[1].color = this.expectationNotchProspectColor;
        }
        else
          this.expectationNotches[index].GetComponentsInChildren<Image>()[1].color = Color.clear;
      }
      this.expectationsProgressLabel.text = $"{(string) STRINGS.UI.SKILLS_SCREEN.MORALE_EXPECTATION}: {attributeInstance2.GetTotalValue().ToString()}";
      if ((double) num2 > 0.0)
      {
        LocText expectationsProgressLabel = this.expectationsProgressLabel;
        expectationsProgressLabel.text = $"{expectationsProgressLabel.text} + {GameUtil.ApplyBoldString(GameUtil.ColourizeString((Color32) this.expectationNotchColor, num2.ToString()))}";
      }
      if ((double) num4 < 1.0)
      {
        this.expectationWarning.SetActive(true);
        this.moraleWarning.SetActive(false);
      }
      else
      {
        this.expectationWarning.SetActive(false);
        this.moraleWarning.SetActive(true);
      }
      string str1 = "";
      List<Tuple<string, float>> source = new List<Tuple<string, float>>();
      string str2 = $"{str1}{GameUtil.ApplyBoldString((string) STRINGS.UI.SKILLS_SCREEN.MORALE)}: {attributeInstance1.GetTotalValue().ToString()}\n";
      for (int i = 0; i < attributeInstance1.Modifiers.Count; ++i)
        source.Add(new Tuple<string, float>(attributeInstance1.Modifiers[i].GetDescription(), attributeInstance1.Modifiers[i].Value));
      List<Tuple<string, float>> list = source.ToList<Tuple<string, float>>();
      list.Sort((Comparison<Tuple<string, float>>) ((pair1, pair2) => pair2.second.CompareTo(pair1.second)));
      foreach (Tuple<string, float> tuple in list)
        str2 = $"{str2}    • {tuple.first}: {((double) tuple.second > 0.0 ? UIConstants.ColorPrefixGreen : UIConstants.ColorPrefixRed)}{tuple.second.ToString()}{UIConstants.ColorSuffix}\n";
      string message = $"{str2 + "\n"}{GameUtil.ApplyBoldString((string) STRINGS.UI.SKILLS_SCREEN.MORALE_EXPECTATION)}: {attributeInstance2.GetTotalValue().ToString()}\n";
      for (int i = 0; i < attributeInstance2.Modifiers.Count; ++i)
        message = $"{message}    • {attributeInstance2.Modifiers[i].GetDescription()}: {((double) attributeInstance2.Modifiers[i].Value > 0.0 ? UIConstants.ColorPrefixRed : UIConstants.ColorPrefixGreen)}{attributeInstance2.Modifiers[i].GetFormattedString()}{UIConstants.ColorSuffix}\n";
      this.expectationsTooltip.SetSimpleTooltip(message);
    }
  }

  private Tag SelectedMinionModel()
  {
    MinionIdentity minionIdentity;
    StoredMinionIdentity storedMinionIdentity;
    this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
    if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
      return Db.Get().Personalities.Get(minionIdentity.personalityResourceId).model;
    return (UnityEngine.Object) storedMinionIdentity != (UnityEngine.Object) null ? Db.Get().Personalities.Get(storedMinionIdentity.personalityResourceId).model : (Tag) (string) null;
  }

  private void RefreshHat()
  {
    if (this.currentlySelectedMinion == null || this.currentlySelectedMinion.IsNull())
      return;
    List<IListableOption> contentKeys = new List<IListableOption>();
    MinionIdentity minionIdentity;
    StoredMinionIdentity storedMinionIdentity;
    this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
    string str;
    if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
    {
      MinionResume component = minionIdentity.GetComponent<MinionResume>();
      str = string.IsNullOrEmpty(component.TargetHat) ? component.CurrentHat : component.TargetHat;
      foreach (MinionResume.HatInfo allHat in component.GetAllHats())
        contentKeys.Add((IListableOption) new HatListable(allHat.Source, allHat.Hat));
      this.hatDropDown.Initialize((IEnumerable<IListableOption>) contentKeys, new Action<IListableOption, object>(this.OnHatDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.hatDropDownSort), new Action<DropDownEntry, object>(this.hatDropEntryRefreshAction), false, (object) this.currentlySelectedMinion);
    }
    else
      str = string.IsNullOrEmpty(storedMinionIdentity.targetHat) ? storedMinionIdentity.currentHat : storedMinionIdentity.targetHat;
    this.hatDropDown.openButton.enabled = (UnityEngine.Object) minionIdentity != (UnityEngine.Object) null;
    this.selectedHat.transform.Find("Arrow").gameObject.SetActive((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null);
    this.selectedHat.sprite = Assets.GetSprite((HashedString) (string.IsNullOrEmpty(str) ? "hat_role_none" : str));
  }

  private void OnHatDropEntryClick(IListableOption skill, object data)
  {
    MinionIdentity minionIdentity;
    this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out StoredMinionIdentity _);
    if ((UnityEngine.Object) minionIdentity == (UnityEngine.Object) null)
      return;
    MinionResume component = minionIdentity.GetComponent<MinionResume>();
    string name = "hat_role_none";
    if (skill != null)
    {
      this.selectedHat.sprite = Assets.GetSprite((HashedString) (skill as HatListable).hat);
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        string hat = (skill as HatListable).hat;
        component.SetHats(component.CurrentHat, hat);
        if (component.OwnsHat(hat))
        {
          PutOnHatChore putOnHatChore = new PutOnHatChore((IStateMachineTarget) component, Db.Get().ChoreTypes.SwitchHat);
        }
      }
    }
    else
    {
      this.selectedHat.sprite = Assets.GetSprite((HashedString) name);
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        component.SetHats(component.CurrentHat, (string) null);
        component.ApplyTargetHat();
      }
    }
    IAssignableIdentity assignableIdentity = (IAssignableIdentity) minionIdentity.assignableProxy.Get();
    foreach (SkillMinionWidget sortableRow in this.sortableRows)
    {
      if (sortableRow.assignableIdentity == assignableIdentity)
        sortableRow.RefreshHat(component.TargetHat);
    }
  }

  private void hatDropEntryRefreshAction(DropDownEntry entry, object targetData)
  {
    if (entry.entryData == null)
      return;
    HatListable entryData = entry.entryData as HatListable;
    entry.image.sprite = Assets.GetSprite((HashedString) entryData.hat);
  }

  private int hatDropDownSort(IListableOption a, IListableOption b, object targetData) => 0;

  public override void OnKeyDown(KButtonEvent e)
  {
    if (!e.Consumed && DebugHandler.enabled && e.TryConsume(Action.DebugInstantBuildMode))
      DebugHandler.ToggleInstantBuildMode();
    else
      base.OnKeyDown(e);
  }

  private void Update()
  {
    if (this.dirty)
      this.RefreshAll();
    if (this.linesPending)
    {
      foreach (GameObject gameObject in this.skillWidgets.Values)
        gameObject.GetComponent<SkillWidget>().RefreshLines();
      this.linesPending = false;
    }
    if (!KInputManager.currentControllerIsGamepad)
      return;
    this.scrollRect.AnalogUpdate(KInputManager.steamInputInterpreter.GetSteamCameraMovement() * this.scrollSpeed);
  }

  private void PopulateBoosters()
  {
    foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.BionicUpgrade))
    {
      Tag id = go.GetComponent<KPrefabID>().PrefabID();
      GameObject gameObject1 = Util.KInstantiate(this.boosterPrefab, this.boosterContentGrid, go.name);
      gameObject1.transform.localScale = Vector3.one;
      gameObject1.SetActive(true);
      HierarchyReferences component = gameObject1.GetComponent<HierarchyReferences>();
      this.boosterWidgets.Add(go.PrefabID(), component);
      component.GetReference<Image>("Icon").sprite = Def.GetUISprite((object) go).first;
      gameObject1.GetComponentInChildren<LocText>().SetText(go.GetProperName());
      KButton reference1 = component.GetReference<KButton>("AssignmentIncrementButton");
      reference1.ClearOnClick();
      reference1.onClick += (System.Action) (() => this.IncrementBoosterAssignment(id));
      KButton reference2 = component.GetReference<KButton>("AssignmentDecrementButton");
      reference2.ClearOnClick();
      reference2.onClick += (System.Action) (() => this.DecrementBoosterAssignment(id));
      foreach (GameObject boosterSlotIcon in this.boosterSlotIcons)
        Util.KDestroyGameObject(boosterSlotIcon);
      this.boosterSlotIcons.Clear();
      for (int index = 0; index < 8; ++index)
      {
        GameObject gameObject2 = Util.KInstantiateUI(this.boosterSlotIconPrefab, this.boosterSlotIconPrefab.transform.parent.gameObject);
        this.boosterSlotIcons.Add(gameObject2);
        int slotIdx = index;
        gameObject2.transform.GetChild(0).GetComponent<MultiToggle>().onClick = (System.Action) (() =>
        {
          MinionIdentity minionIdentity;
          this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out StoredMinionIdentity _);
          if ((UnityEngine.Object) minionIdentity == (UnityEngine.Object) null)
            return;
          minionIdentity.GetSMI<BionicUpgradesMonitor.Instance>().upgradeComponentSlots[slotIdx].GetAssignableSlotInstance().Unassign();
          this.RefreshBoosters();
        });
      }
    }
  }

  private void IncrementBoosterAssignment(Tag boosterType)
  {
    BionicUpgradeComponent availableBoosterOfType = this.FindAvailableBoosterOfType(boosterType);
    if ((UnityEngine.Object) availableBoosterOfType != (UnityEngine.Object) null)
      availableBoosterOfType.Assign(this.CurrentlySelectedMinion);
    this.RefreshBoosters();
  }

  private void DecrementBoosterAssignment(Tag boosterType)
  {
    MinionIdentity minionIdentity;
    this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out StoredMinionIdentity _);
    BionicUpgradesMonitor.Instance smi = minionIdentity.GetSMI<BionicUpgradesMonitor.Instance>();
    if (smi == null)
    {
      bool flag = false;
      for (int index = smi.upgradeComponentSlots.Length - 1; index >= 0; --index)
      {
        BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = smi.upgradeComponentSlots[index];
        if ((UnityEngine.Object) upgradeComponentSlot.assignedUpgradeComponent != (UnityEngine.Object) null && upgradeComponentSlot.assignedUpgradeComponent.PrefabID() == boosterType && upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.AssignedUpgradeMatchesInstalledUpgrade)
        {
          upgradeComponentSlot.GetAssignableSlotInstance().Unassign();
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        for (int index = smi.upgradeComponentSlots.Length - 1; index >= 0; --index)
        {
          BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = smi.upgradeComponentSlots[index];
          if ((UnityEngine.Object) upgradeComponentSlot.assignedUpgradeComponent != (UnityEngine.Object) null && upgradeComponentSlot.assignedUpgradeComponent.PrefabID() == boosterType)
          {
            upgradeComponentSlot.GetAssignableSlotInstance().Unassign();
            break;
          }
        }
      }
    }
    this.RefreshBoosters();
  }

  private BionicUpgradeComponent FindAvailableBoosterOfType(Tag boosterType)
  {
    MinionIdentity minionIdentity;
    this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out StoredMinionIdentity _);
    if ((UnityEngine.Object) minionIdentity == (UnityEngine.Object) null)
      return (BionicUpgradeComponent) null;
    List<Pickupable> pickupablesList = ClusterManager.Instance.GetWorld(minionIdentity.GetMyWorldId()).worldInventory.CreatePickupablesList(boosterType);
    if (pickupablesList == null || pickupablesList.Count == 0)
      return (BionicUpgradeComponent) null;
    List<Pickupable> all = pickupablesList.FindAll((Predicate<Pickupable>) (match => match.GetComponent<BionicUpgradeComponent>().assignee == null));
    if (all == null || all.Count == 0)
      return (BionicUpgradeComponent) null;
    using (List<Pickupable>.Enumerator enumerator = all.GetEnumerator())
    {
      if (enumerator.MoveNext())
        return enumerator.Current.GetComponent<BionicUpgradeComponent>();
    }
    return (BionicUpgradeComponent) null;
  }

  private void RefreshBoosters()
  {
    BionicUpgradesMonitor.Instance instance = (BionicUpgradesMonitor.Instance) null;
    MinionIdentity minionIdentity;
    this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out StoredMinionIdentity _);
    bool flag1 = this.SelectedMinionModel() == GameTags.Minions.Models.Bionic && (UnityEngine.Object) minionIdentity != (UnityEngine.Object) null;
    if (flag1)
    {
      instance = minionIdentity.GetSMI<BionicUpgradesMonitor.Instance>();
      if (instance == null)
        flag1 = false;
    }
    if (flag1)
    {
      this.equippedBoostersHeaderLabel.SetText(GameUtil.SafeStringFormat((string) STRINGS.UI.SKILLS_SCREEN.ASSIGNED_BOOSTERS_HEADER, (object) this.CurrentlySelectedMinion.GetProperName()));
      this.assignedBoostersCountLabel.SetText(GameUtil.SafeStringFormat((string) STRINGS.UI.SKILLS_SCREEN.ASSIGNED_BOOSTERS_COUNT_LABEL, (object) instance.AssignedSlotCount, (object) instance.UnlockedSlotCount));
      this.boosterPanel.SetActive(true);
      this.boosterHeader.SetActive(true);
      float y = (float) ((double) Screen.height / (double) GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetCanvasScale() * 0.40000000596046448);
      float num1 = 96f;
      this.skillsContainer.rectTransform().sizeDelta = new Vector2(0.0f, (float) (-1.0 * ((double) y + (double) num1)));
      this.boosterPanel.rectTransform().sizeDelta = new Vector2(0.0f, y);
      this.boosterHeader.rectTransform().anchoredPosition = new Vector2(0.0f, y);
      for (int index = 0; index < this.boosterSlotIcons.Count; ++index)
      {
        BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = instance.upgradeComponentSlots[7 - index];
        this.boosterSlotIcons[index].SetActive(true);
        if (index >= instance.upgradeComponentSlots.Length || instance.upgradeComponentSlots[index].IsLocked)
        {
          this.boosterSlotIcons[index].GetComponent<Image>().sprite = Assets.GetSprite((HashedString) "bionicUpgradeSlotLocked");
          this.boosterSlotIcons[index].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
          this.boosterSlotIcons[index].GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.SKILLS_SCREEN.BIONIC_UPGRADE_SLOT_LOCKED);
          this.boosterSlotIcons[index].transform.GetChild(0).gameObject.SetActive(false);
        }
        else if ((UnityEngine.Object) instance.upgradeComponentSlots[index].assignedUpgradeComponent != (UnityEngine.Object) null)
        {
          this.boosterSlotIcons[index].GetComponent<Image>().sprite = Def.GetUISprite((object) instance.upgradeComponentSlots[index].assignedUpgradeComponent.PrefabID()).first;
          this.boosterSlotIcons[index].GetComponent<ToolTip>().SetSimpleTooltip($"{instance.upgradeComponentSlots[index].assignedUpgradeComponent.GetProperName()}\n\n{(string) STRINGS.UI.SKILLS_SCREEN.BIONIC_UPGRADE_SLOT_UNASSIGN}");
          this.boosterSlotIcons[index].GetComponent<Image>().color = Color.white;
          this.boosterSlotIcons[index].transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
          this.boosterSlotIcons[index].GetComponent<Image>().sprite = Assets.GetSprite((HashedString) "bionicUpgradeSlot");
          this.boosterSlotIcons[index].GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.SKILLS_SCREEN.BIONIC_UPGRADE_SLOT_AVAILABLE);
          this.boosterSlotIcons[index].GetComponent<Image>().color = Color.white;
          this.boosterSlotIcons[index].transform.GetChild(0).gameObject.SetActive(false);
        }
      }
      foreach (KeyValuePair<Tag, HierarchyReferences> boosterWidget in this.boosterWidgets)
      {
        KeyValuePair<Tag, HierarchyReferences> widget = boosterWidget;
        int num2 = 0;
        if (instance != null && instance.upgradeComponentSlots != null)
        {
          foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in instance.upgradeComponentSlots)
          {
            if ((UnityEngine.Object) upgradeComponentSlot.assignedUpgradeComponent != (UnityEngine.Object) null && upgradeComponentSlot.assignedUpgradeComponent.PrefabID() == widget.Key)
              ++num2;
          }
        }
        GameObject prefab = Assets.GetPrefab(widget.Key);
        widget.Value.GetReference<LocText>("Label").SetText(prefab.GetProperName());
        float num3 = 0.0f;
        List<Pickupable> pickupablesList = ClusterManager.Instance.GetWorld(minionIdentity.GetMyWorldId()).worldInventory.CreatePickupablesList(widget.Key);
        if (pickupablesList != null && pickupablesList.Count > 0)
          num3 = (float) pickupablesList.FindAll((Predicate<Pickupable>) (match => match.GetComponent<Assignable>().assignee == null)).Count;
        if ((double) num3 > 0.0)
        {
          widget.Value.GetReference<Image>("Icon").material = GlobalResources.Instance().AnimUIMaterial;
          widget.Value.GetReference<Image>("Icon").color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
          widget.Value.GetReference<Image>("Icon").material = GlobalResources.Instance().AnimMaterialUIDesaturated;
          widget.Value.GetReference<Image>("Icon").color = new Color(1f, 1f, 1f, 0.5f);
        }
        string sourceText1 = GameUtil.SafeStringFormat((string) STRINGS.UI.SKILLS_SCREEN.AVAILABLE_BOOSTERS_LABEL, (object) num3.ToString());
        LocText reference = widget.Value.GetReference<LocText>("AvailableLabel");
        reference.SetText(sourceText1);
        reference.color = (double) num3 > 0.0 ? new Color(0.53f, 0.83f, 0.53f) : new Color(0.65f, 0.65f, 0.65f);
        string sourceText2 = GameUtil.SafeStringFormat((string) STRINGS.UI.SKILLS_SCREEN.ASSIGNED_BOOSTERS_LABEL, (object) num2);
        widget.Value.GetReference<LocText>("EquipCountLabel").SetText(sourceText2);
        widget.Value.GetReference<ToolTip>("Tooltip").SetSimpleTooltip($"<b>{prefab.GetProperName()}</b>\n\n{BionicUpgradeComponentConfig.UpgradesData[widget.Key].stateMachineDescription}\n\n{BionicUpgradeComponentConfig.GetColonyBoosterAssignmentString(widget.Key.Name)}");
        bool flag2 = instance.AssignedSlotCount < instance.UnlockedSlotCount;
        int num4 = (double) num3 > 0.0 ? 1 : 0;
        MultiToggle component = widget.Value.gameObject.GetComponent<MultiToggle>();
        component.onClick = (System.Action) null;
        int num5 = flag2 ? 1 : 0;
        if ((num4 & num5) != 0)
          component.onClick += (System.Action) (() => this.IncrementBoosterAssignment(widget.Key));
      }
    }
    else
    {
      this.boosterPanel.SetActive(false);
      this.boosterHeader.SetActive(false);
      this.skillsContainer.rectTransform().sizeDelta = new Vector2(0.0f, 0.0f);
    }
  }

  private void RefreshSkillWidgets()
  {
    int num1 = 1;
    foreach (SkillGroup resource in Db.Get().SkillGroups.resources)
    {
      List<Skill> skillsBySkillGroup = this.GetSkillsBySkillGroup(resource.Id);
      if (skillsBySkillGroup.Count > 0)
      {
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        for (int index = 0; index < skillsBySkillGroup.Count; ++index)
        {
          Skill restrictions = skillsBySkillGroup[index];
          if (!restrictions.deprecated && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) restrictions))
          {
            if (!this.skillWidgets.ContainsKey(restrictions.Id))
            {
              while (restrictions.tier >= this.skillColumns.Count)
              {
                GameObject gameObject = Util.KInstantiateUI(this.Prefab_skillColumn, this.Prefab_tableLayout, true);
                this.skillColumns.Add(gameObject);
                HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
                if (this.skillColumns.Count % 2 == 0)
                  component.GetReference("BG").gameObject.SetActive(false);
              }
              int num2 = 0;
              dictionary.TryGetValue(restrictions.tier, out num2);
              dictionary[restrictions.tier] = num2 + 1;
              GameObject gameObject1 = Util.KInstantiateUI(this.Prefab_skillWidget, this.skillColumns[restrictions.tier], true);
              this.skillWidgets.Add(restrictions.Id, gameObject1);
            }
            this.skillWidgets[restrictions.Id].GetComponent<SkillWidget>().Refresh(restrictions.Id);
          }
        }
        if (!this.skillGroupRow.ContainsKey(resource.Id))
        {
          int a = 1;
          foreach (KeyValuePair<int, int> keyValuePair in dictionary)
            a = Mathf.Max(a, keyValuePair.Value);
          this.skillGroupRow.Add(resource.Id, num1);
          num1 += a;
        }
      }
    }
    foreach (KeyValuePair<string, GameObject> skillWidget in this.skillWidgets)
    {
      if (Db.Get().Skills.Get(skillWidget.Key).requiredDuplicantModel != null)
        skillWidget.Value.SetActive((Tag) Db.Get().Skills.Get(skillWidget.Key).requiredDuplicantModel == this.SelectedMinionModel());
    }
    foreach (SkillMinionWidget sortableRow in this.sortableRows)
      sortableRow.Refresh();
    this.RefreshWidgetPositions();
  }

  public void HoverSkill(string skillID)
  {
    this.hoveredSkillID = skillID;
    if (this.delayRefreshRoutine != null)
    {
      this.StopCoroutine(this.delayRefreshRoutine);
      this.delayRefreshRoutine = (Coroutine) null;
    }
    if (string.IsNullOrEmpty(this.hoveredSkillID))
      this.delayRefreshRoutine = this.StartCoroutine(this.DelayRefreshProgressBars());
    else
      this.RefreshProgressBars();
  }

  private IEnumerator DelayRefreshProgressBars()
  {
    yield return (object) SequenceUtil.WaitForSecondsRealtime(0.1f);
    this.RefreshProgressBars();
  }

  public void RefreshWidgetPositions()
  {
    float num1 = 0.0f;
    foreach (KeyValuePair<string, GameObject> skillWidget in this.skillWidgets)
    {
      if (!((Tag) Db.Get().Skills.Get(skillWidget.Key).requiredDuplicantModel != this.SelectedMinionModel()))
      {
        float rowPosition = this.GetRowPosition(skillWidget.Key);
        num1 = Mathf.Max(rowPosition, num1);
        skillWidget.Value.rectTransform().anchoredPosition = Vector2.down * rowPosition;
      }
    }
    float num2 = Mathf.Max(num1, (float) this.layoutRowHeight);
    float layoutRowHeight = (float) this.layoutRowHeight;
    foreach (GameObject skillColumn in this.skillColumns)
      skillColumn.GetComponent<LayoutElement>().minHeight = num2 + layoutRowHeight;
    this.linesPending = true;
  }

  public float GetRowPosition(string skillID)
  {
    Skill skill1 = Db.Get().Skills.Get(skillID);
    int num1 = this.skillGroupRow[skill1.skillGroup];
    int num2 = num1;
    foreach (KeyValuePair<string, int> keyValuePair in this.skillGroupRow)
    {
      if (keyValuePair.Value <= num1 && this.SelectedMinionModel() != (Tag) this.GetSkillsBySkillGroup(keyValuePair.Key)[0].requiredDuplicantModel)
        --num2;
    }
    int num3 = num2;
    List<Skill> skillsBySkillGroup = this.GetSkillsBySkillGroup(skill1.skillGroup);
    int num4 = 0;
    foreach (Skill skill2 in skillsBySkillGroup)
    {
      if (skill2 != skill1)
      {
        if (skill2.tier == skill1.tier)
          ++num4;
      }
      else
        break;
    }
    return (float) (this.layoutRowHeight * (num4 + num3 - 1));
  }

  private void OnAddMinionIdentity(MinionIdentity add)
  {
    this.BuildMinions();
    this.RefreshAll();
  }

  private void OnRemoveMinionIdentity(MinionIdentity remove)
  {
    if ((UnityEngine.Object) remove != (UnityEngine.Object) null)
    {
      if (this.CurrentlySelectedMinion == (IAssignableIdentity) remove)
        this.CurrentlySelectedMinion = (IAssignableIdentity) null;
      if (remove.assignableProxy.Get() == this.CurrentlySelectedMinion)
        this.CurrentlySelectedMinion = (IAssignableIdentity) null;
    }
    this.BuildMinions();
    this.RefreshAll();
  }

  private void BuildMinions()
  {
    for (int index = this.sortableRows.Count - 1; index >= 0; --index)
      this.sortableRows[index].DeleteObject();
    this.sortableRows.Clear();
    foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
    {
      GameObject gameObject = Util.KInstantiateUI(this.Prefab_minion, this.Prefab_minionLayout, true);
      gameObject.GetComponent<SkillMinionWidget>().SetMinon((IAssignableIdentity) minionIdentity.assignableProxy.Get());
      this.sortableRows.Add(gameObject.GetComponent<SkillMinionWidget>());
    }
    foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
    {
      foreach (MinionStorage.Info info in minionStorage.GetStoredMinionInfo())
      {
        if (info.serializedMinion != null)
        {
          StoredMinionIdentity storedMinionIdentity = info.serializedMinion.Get<StoredMinionIdentity>();
          GameObject gameObject = Util.KInstantiateUI(this.Prefab_minion, this.Prefab_minionLayout, true);
          gameObject.GetComponent<SkillMinionWidget>().SetMinon((IAssignableIdentity) storedMinionIdentity.assignableProxy.Get());
          this.sortableRows.Add(gameObject.GetComponent<SkillMinionWidget>());
        }
      }
    }
    foreach (int num in ClusterManager.Instance.GetWorldIDsSorted())
    {
      if (ClusterManager.Instance.GetWorld(num).IsDiscovered)
        this.AddWorldDivider(num);
    }
    foreach (KeyValuePair<int, GameObject> worldDivider in this.worldDividers)
    {
      worldDivider.Value.SetActive(ClusterManager.Instance.GetWorld(worldDivider.Key).IsDiscovered && DlcManager.FeatureClusterSpaceEnabled());
      Component reference = worldDivider.Value.GetComponent<HierarchyReferences>().GetReference("NobodyRow");
      reference.gameObject.SetActive(true);
      foreach (MinionAssignablesProxy assignablesProxy in Components.MinionAssignablesProxy)
      {
        if (assignablesProxy.GetTargetGameObject().GetComponent<KMonoBehaviour>().GetMyWorld().id == worldDivider.Key)
        {
          reference.gameObject.SetActive(false);
          break;
        }
      }
    }
    if (this.CurrentlySelectedMinion != null || Components.LiveMinionIdentities.Count <= 0)
      return;
    this.CurrentlySelectedMinion = (IAssignableIdentity) Components.LiveMinionIdentities.Items[0];
  }

  protected void AddWorldDivider(int worldId)
  {
    if (this.worldDividers.ContainsKey(worldId))
      return;
    GameObject gameObject = Util.KInstantiateUI(this.Prefab_worldDivider, this.Prefab_minionLayout, true);
    gameObject.GetComponentInChildren<Image>().color = ClusterManager.worldColors[worldId % ClusterManager.worldColors.Length];
    ClusterGridEntity component = ClusterManager.Instance.GetWorld(worldId).GetComponent<ClusterGridEntity>();
    gameObject.GetComponentInChildren<LocText>().SetText(component.Name);
    gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = component.GetUISprite();
    this.worldDividers.Add(worldId, gameObject);
  }

  private void WorldRemoved(object worldId)
  {
    int key = ((Boxed<int>) worldId).value;
    GameObject gameObject;
    if (!this.worldDividers.TryGetValue(key, out gameObject))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) gameObject);
    this.worldDividers.Remove(key);
  }

  public Vector2 GetSkillWidgetLineTargetPosition(string skillID)
  {
    return (Vector2) this.skillWidgets[skillID].GetComponent<SkillWidget>().lines_right.GetPosition();
  }

  public SkillWidget GetSkillWidget(string skill)
  {
    return this.skillWidgets[skill].GetComponent<SkillWidget>();
  }

  public List<Skill> GetSkillsBySkillGroup(string skillGrp)
  {
    List<Skill> skillsBySkillGroup = new List<Skill>();
    foreach (Skill resource in Db.Get().Skills.resources)
    {
      if (resource.skillGroup == skillGrp && !resource.deprecated)
        skillsBySkillGroup.Add(resource);
    }
    return skillsBySkillGroup;
  }

  private void SelectSortToggle(MultiToggle toggle)
  {
    this.dupeSortingToggle.ChangeState(0);
    this.experienceSortingToggle.ChangeState(0);
    this.moraleSortingToggle.ChangeState(0);
    if ((UnityEngine.Object) toggle != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) this.activeSortToggle == (UnityEngine.Object) toggle)
        this.sortReversed = !this.sortReversed;
      this.activeSortToggle = toggle;
    }
    this.activeSortToggle.ChangeState(this.sortReversed ? 2 : 1);
  }

  private void SortRows(Comparison<IAssignableIdentity> comparison)
  {
    this.active_sort_method = comparison;
    Dictionary<IAssignableIdentity, SkillMinionWidget> dictionary1 = new Dictionary<IAssignableIdentity, SkillMinionWidget>();
    foreach (SkillMinionWidget sortableRow in this.sortableRows)
      dictionary1.Add(sortableRow.assignableIdentity, sortableRow);
    Dictionary<int, List<IAssignableIdentity>> minionsByWorld = ClusterManager.Instance.MinionsByWorld;
    this.sortableRows.Clear();
    Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
    int num1 = 0;
    int num2 = 0;
    foreach (KeyValuePair<int, List<IAssignableIdentity>> keyValuePair in minionsByWorld)
    {
      dictionary2.Add(keyValuePair.Key, num1);
      int num3 = num1 + 1;
      List<IAssignableIdentity> assignableIdentityList = new List<IAssignableIdentity>();
      foreach (IAssignableIdentity assignableIdentity in keyValuePair.Value)
        assignableIdentityList.Add(assignableIdentity);
      if (comparison != null)
      {
        assignableIdentityList.Sort(comparison);
        if (this.sortReversed)
          assignableIdentityList.Reverse();
      }
      num1 = num3 + assignableIdentityList.Count;
      num2 += assignableIdentityList.Count;
      for (int index = 0; index < assignableIdentityList.Count; ++index)
      {
        IAssignableIdentity key = assignableIdentityList[index];
        this.sortableRows.Add(dictionary1[key]);
      }
    }
    for (int index = 0; index < this.sortableRows.Count; ++index)
      this.sortableRows[index].gameObject.transform.SetSiblingIndex(index);
    foreach (KeyValuePair<int, int> keyValuePair in dictionary2)
      this.worldDividers[keyValuePair.Key].transform.SetSiblingIndex(keyValuePair.Value);
  }
}
