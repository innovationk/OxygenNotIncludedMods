// Decompiled with JetBrains decompiler
// Type: DetailsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DetailsScreen : KTabMenu
{
  public static DetailsScreen Instance;
  [SerializeField]
  private KButton CodexEntryButton;
  [SerializeField]
  private KButton PinResourceButton;
  [Header("Panels")]
  public Transform UserMenuPanel;
  [Header("Name Editing (disabled)")]
  [SerializeField]
  private KButton CloseButton;
  [Header("Tabs")]
  [SerializeField]
  private DetailTabHeader tabHeader;
  [SerializeField]
  private EditableTitleBar TabTitle;
  [SerializeField]
  private DetailsScreen.Screens[] screens;
  [SerializeField]
  private GameObject tabHeaderContainer;
  [Header("Side Screen Tabs")]
  [SerializeField]
  private DetailsScreen.SidescreenTab[] sidescreenTabs;
  [SerializeField]
  private GameObject sidescreenTabHeader;
  [SerializeField]
  private GameObject original_tab;
  [SerializeField]
  private GameObject original_tab_body;
  [Header("Side Screens")]
  [SerializeField]
  private GameObject sideScreen;
  [SerializeField]
  private List<DetailsScreen.SideScreenRef> sideScreens;
  [SerializeField]
  private LayoutElement tabBodyLayoutElement;
  [Header("Secondary Side Screens")]
  [SerializeField]
  private GameObject sideScreen2ContentBody;
  [SerializeField]
  private GameObject sideScreen2;
  [SerializeField]
  private LocText sideScreen2Title;
  private KScreen activeSideScreen2;
  private Tag previousTargetID = (Tag) (string) null;
  private bool HasActivated;
  private DetailsScreen.SidescreenTabTypes selectedSidescreenTabID;
  private Dictionary<KScreen, KScreen> instantiatedSecondarySideScreens = new Dictionary<KScreen, KScreen>();
  private static readonly EventSystem.IntraObjectHandler<DetailsScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<DetailsScreen>((Action<DetailsScreen, object>) ((component, data) => component.OnRefreshData(data)));
  private List<KeyValuePair<DetailsScreen.SideScreenRef, int>> sortedSideScreens = new List<KeyValuePair<DetailsScreen.SideScreenRef, int>>();
  private int setRocketTitleHandle = -1;

  public static void DestroyInstance() => DetailsScreen.Instance = (DetailsScreen) null;

  public GameObject target { get; private set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SortScreenOrder();
    this.ConsumeMouseScroll = true;
    Debug.Assert((UnityEngine.Object) DetailsScreen.Instance == (UnityEngine.Object) null);
    DetailsScreen.Instance = this;
    this.InitiateSidescreenTabs();
    this.DeactivateSideContent();
    this.Show(false);
    this.Subscribe(Game.Instance.gameObject, -1503271301, new Action<object>(this.OnSelectObject));
    this.tabHeader.Init();
  }

  public bool CanObjectDisplayTabOfType(GameObject obj, DetailsScreen.SidescreenTabTypes type)
  {
    for (int index = 0; index < this.sidescreenTabs.Length; ++index)
    {
      DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[index];
      if (sidescreenTab.type == type)
        return sidescreenTab.ValidateTarget(obj);
    }
    return false;
  }

  public DetailsScreen.SidescreenTab GetTabOfType(DetailsScreen.SidescreenTabTypes type)
  {
    for (int index = 0; index < this.sidescreenTabs.Length; ++index)
    {
      DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[index];
      if (sidescreenTab.type == type)
        return sidescreenTab;
    }
    return (DetailsScreen.SidescreenTab) null;
  }

  public void InitiateSidescreenTabs()
  {
    for (int index = 0; index < this.sidescreenTabs.Length; ++index)
    {
      DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[index];
      sidescreenTab.Initiate(this.original_tab, this.original_tab_body, (Action<DetailsScreen.SidescreenTab>) (_tab => this.SelectSideScreenTab(_tab.type)));
      switch (sidescreenTab.type)
      {
        case DetailsScreen.SidescreenTabTypes.Errands:
          sidescreenTab.ValidateTargetCallback = (Func<GameObject, DetailsScreen.SidescreenTab, bool>) ((target, _tab) => (UnityEngine.Object) target.GetComponent<MinionIdentity>() != (UnityEngine.Object) null);
          break;
        case DetailsScreen.SidescreenTabTypes.Material:
          sidescreenTab.ValidateTargetCallback = (Func<GameObject, DetailsScreen.SidescreenTab, bool>) ((target, _tab) =>
          {
            Reconstructable component = target.GetComponent<Reconstructable>();
            return (UnityEngine.Object) component != (UnityEngine.Object) null && component.AllowReconstruct;
          });
          break;
        case DetailsScreen.SidescreenTabTypes.Blueprints:
          sidescreenTab.ValidateTargetCallback = (Func<GameObject, DetailsScreen.SidescreenTab, bool>) ((target, _tab) =>
          {
            MinionIdentity component1 = target.GetComponent<MinionIdentity>();
            BuildingFacade component2 = target.GetComponent<BuildingFacade>();
            return (UnityEngine.Object) component1 != (UnityEngine.Object) null || (UnityEngine.Object) component2 != (UnityEngine.Object) null;
          });
          break;
      }
    }
  }

  private void OnSelectObject(object data)
  {
    if (data == null)
    {
      this.previouslyActiveTab = -1;
      this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Config);
    }
    else
    {
      KPrefabID component = ((GameObject) data).GetComponent<KPrefabID>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null || this.previousTargetID != component.PrefabID())
      {
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && (bool) (UnityEngine.Object) component.GetComponent<MinionIdentity>())
          this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Errands);
        else
          this.SelectSideScreenTab(DetailsScreen.SidescreenTabTypes.Config);
      }
      else
        this.SelectSideScreenTab(this.selectedSidescreenTabID);
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.CodexEntryButton.onClick += new System.Action(this.CodexEntryButton_OnClick);
    this.PinResourceButton.onClick += new System.Action(this.PinResourceButton_OnClick);
    this.CloseButton.onClick += new System.Action(this.DeselectAndClose);
    this.TabTitle.OnNameChanged += new Action<string>(this.OnNameChanged);
    this.TabTitle.OnStartedEditing += new System.Action(this.OnStartedEditing);
    this.sideScreen2.SetActive(false);
    this.Subscribe<DetailsScreen>(-1514841199, DetailsScreen.OnRefreshDataDelegate);
  }

  private void OnStartedEditing()
  {
    this.isEditing = true;
    KScreenManager.Instance.RefreshStack();
  }

  private void OnNameChanged(string newName)
  {
    this.isEditing = false;
    if (string.IsNullOrEmpty(newName))
      return;
    MinionIdentity component1 = this.target.GetComponent<MinionIdentity>();
    UserNameable component2 = this.target.GetComponent<UserNameable>();
    ClustercraftExteriorDoor component3 = this.target.GetComponent<ClustercraftExteriorDoor>();
    CommandModule component4 = this.target.GetComponent<CommandModule>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      component1.SetName(newName);
      if ((UnityEngine.Object) ScheduleScreen.Instance != (UnityEngine.Object) null)
        ScheduleScreen.Instance.Trigger(1980521255, (object) null);
    }
    else if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
      SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(component4.GetComponent<LaunchConditionManager>()).SetRocketName(newName);
    else if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      component3.GetTargetWorld().GetComponent<UserNameable>().SetName(newName);
    else if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      component2.SetName(newName);
    this.TabTitle.UpdateRenameTooltip(this.target);
  }

  protected override void OnDeactivate()
  {
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null && this.setRocketTitleHandle != -1)
      this.target.Unsubscribe(this.setRocketTitleHandle);
    this.setRocketTitleHandle = -1;
    this.DeactivateSideContent();
    base.OnDeactivate();
  }

  protected override void OnShow(bool show)
  {
    if (!show)
    {
      this.DeactivateSideContent();
    }
    else
    {
      this.MaskSideContent(false);
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenHalfEffect);
    }
    base.OnShow(show);
  }

  protected override void OnCmpDisable()
  {
    this.DeactivateSideContent();
    base.OnCmpDisable();
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (this.isEditing || !((UnityEngine.Object) this.target != (UnityEngine.Object) null) || !PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
      return;
    this.DeselectAndClose();
  }

  private static Component GetComponent(GameObject go, string name)
  {
    System.Type type = System.Type.GetType(name);
    return !(type != (System.Type) null) ? go.GetComponent(name) : go.GetComponent(type);
  }

  private static bool IsExcludedPrefabTag(GameObject go, Tag[] excluded_tags)
  {
    if (excluded_tags == null || excluded_tags.Length == 0)
      return false;
    bool flag = false;
    KPrefabID component = go.GetComponent<KPrefabID>();
    for (int index = 0; index < excluded_tags.Length; ++index)
    {
      Tag excludedTag = excluded_tags[index];
      if (component.PrefabTag == excludedTag)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private string CodexEntryButton_GetCodexId()
  {
    Debug.Assert((UnityEngine.Object) this.target != (UnityEngine.Object) null, (object) "Details Screen has no target");
    KSelectable component1 = this.target.GetComponent<KSelectable>();
    DebugUtil.AssertArgs(((UnityEngine.Object) component1 != (UnityEngine.Object) null ? 1 : 0) != 0, (object) "Details Screen target is not a KSelectable", (object) this.target);
    CodexEntryRedirector component2 = component1.GetComponent<CodexEntryRedirector>();
    BuildingUnderConstruction component3 = component1.GetComponent<BuildingUnderConstruction>();
    CreatureBrain component4 = component1.GetComponent<CreatureBrain>();
    PlantableSeed component5 = component1.GetComponent<PlantableSeed>();
    ICellSelectionProxy component6 = component1.GetComponent<ICellSelectionProxy>();
    string str;
    if (component6 != null && component6.Element != null)
      str = CodexCache.FormatLinkID(component6.Element.id.ToString());
    else if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && !string.IsNullOrEmpty(component2.CodexID))
      str = CodexCache.FormatLinkID(component2.CodexID);
    else if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      str = CodexCache.FormatLinkID(component3.Def.PrefabID);
    else if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
      str = CodexCache.FormatLinkID(component1.PrefabID().ToString()).Replace("BABY", "");
    else if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
    {
      str = CodexCache.FormatLinkID(component5.PrefabID().ToString());
    }
    else
    {
      str = STRINGS.UI.ExtractLinkID(component1.GetProperName());
      if (string.IsNullOrEmpty(str))
        str = CodexCache.FormatLinkID(component1.PrefabID().ToString());
    }
    return CodexCache.entries.ContainsKey(str) || CodexCache.FindSubEntry(str) != null ? str : "";
  }

  private void CodexEntryButton_Refresh()
  {
    this.CodexEntryButton.isInteractable = this.CodexEntryButton_GetCodexId() != "";
    this.CodexEntryButton.GetComponent<ToolTip>().SetSimpleTooltip((string) (this.CodexEntryButton.isInteractable ? STRINGS.UI.TOOLTIPS.OPEN_CODEX_ENTRY : STRINGS.UI.TOOLTIPS.NO_CODEX_ENTRY));
  }

  public void CodexEntryButton_OnClick()
  {
    string codexId = this.CodexEntryButton_GetCodexId();
    if (!(codexId != ""))
      return;
    ManagementMenu.Instance.OpenCodexToEntry(codexId);
  }

  private bool PinResourceButton_TryGetResourceTagAndProperName(
    out Tag targetTag,
    out string targetProperName)
  {
    KPrefabID component1 = this.target.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && ShouldUse(component1.PrefabTag))
    {
      targetTag = component1.PrefabTag;
      targetProperName = component1.GetProperName();
      return true;
    }
    ICellSelectionProxy component2 = this.target.GetComponent<ICellSelectionProxy>();
    if (component2 != null && component2.Element != null && ShouldUse(component2.Element.tag))
    {
      targetTag = component2.Element.tag;
      targetProperName = component2.Element.name;
      return true;
    }
    targetTag = (Tag) (string) null;
    targetProperName = (string) null;
    return false;

    static bool ShouldUse(Tag targetTag)
    {
      foreach (Tag materialCategory in GameTags.MaterialCategories)
      {
        if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(materialCategory).Contains(targetTag))
          return true;
      }
      foreach (Tag calorieCategory in GameTags.CalorieCategories)
      {
        if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(calorieCategory).Contains(targetTag))
          return true;
      }
      foreach (Tag unitCategory in GameTags.UnitCategories)
      {
        if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(unitCategory).Contains(targetTag))
          return true;
      }
      return false;
    }
  }

  private void PinResourceButton_Refresh()
  {
    Tag targetTag;
    string targetProperName;
    if (this.PinResourceButton_TryGetResourceTagAndProperName(out targetTag, out targetProperName))
    {
      ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(targetTag);
      GameUtil.MeasureUnit measureUnit;
      if (!AllResourcesScreen.Instance.units.TryGetValue(targetTag, out measureUnit))
        measureUnit = GameUtil.MeasureUnit.quantity;
      string str;
      switch (measureUnit)
      {
        case GameUtil.MeasureUnit.mass:
          str = GameUtil.GetFormattedMass(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(targetTag, false));
          break;
        case GameUtil.MeasureUnit.kcal:
          str = GameUtil.GetFormattedCalories(WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(targetTag.Name, ClusterManager.Instance.activeWorld.worldInventory));
          break;
        case GameUtil.MeasureUnit.quantity:
          str = GameUtil.GetFormattedUnits(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(targetTag, false));
          break;
        default:
          str = "";
          break;
      }
      this.PinResourceButton.gameObject.SetActive(true);
      this.PinResourceButton.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.TOOLTIPS.OPEN_RESOURCE_INFO, (object) str, (object) targetProperName));
    }
    else
      this.PinResourceButton.gameObject.SetActive(false);
  }

  public void PinResourceButton_OnClick()
  {
    string targetProperName;
    if (!this.PinResourceButton_TryGetResourceTagAndProperName(out Tag _, out targetProperName))
      return;
    AllResourcesScreen.Instance.SetFilter(STRINGS.UI.StripLinkFormatting(targetProperName));
    AllResourcesScreen.Instance.Show(true);
  }

  public void OnRefreshData(object obj)
  {
    this.RefreshTitle();
    for (int index = 0; index < this.tabs.Count; ++index)
    {
      if (this.tabs[index].gameObject.activeInHierarchy)
        this.tabs[index].Trigger(-1514841199, obj);
    }
  }

  public void Refresh(GameObject go)
  {
    if (this.screens == null)
      return;
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) go)
    {
      if (this.setRocketTitleHandle != -1)
      {
        this.target.Unsubscribe(this.setRocketTitleHandle);
        this.setRocketTitleHandle = -1;
      }
      if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
      {
        KPrefabID component = this.target.GetComponent<KPrefabID>();
        this.previousTargetID = !((UnityEngine.Object) component != (UnityEngine.Object) null) ? (Tag) (string) null : component.PrefabID();
      }
    }
    this.target = go;
    this.sortedSideScreens.Clear();
    this.target.GetComponent<ICellSelectionProxy>()?.OnObjectSelected((object) null);
    this.UpdateTitle();
    this.tabHeader.RefreshTabDisplayForTarget(this.target);
    if (this.sideScreens != null && this.sideScreens.Count > 0)
    {
      bool flag = false;
      foreach (DetailsScreen.SideScreenRef sideScreen in this.sideScreens)
      {
        if (!sideScreen.screenPrefab.IsValidForTarget(this.target))
        {
          if ((UnityEngine.Object) sideScreen.screenInstance != (UnityEngine.Object) null && sideScreen.screenInstance.gameObject.activeSelf)
            sideScreen.screenInstance.gameObject.SetActive(false);
        }
        else
        {
          flag = true;
          if ((UnityEngine.Object) sideScreen.screenInstance == (UnityEngine.Object) null)
          {
            DetailsScreen.SidescreenTab tabOfType = this.GetTabOfType(sideScreen.tab);
            sideScreen.screenInstance = Util.KInstantiateUI<SideScreenContent>(sideScreen.screenPrefab.gameObject, tabOfType.bodyInstance);
          }
          if (!this.sideScreen.activeSelf)
            this.sideScreen.SetActive(true);
          sideScreen.screenInstance.SetTarget(this.target);
          sideScreen.screenInstance.Show();
          int sideScreenSortOrder = sideScreen.screenInstance.GetSideScreenSortOrder();
          this.sortedSideScreens.Add(new KeyValuePair<DetailsScreen.SideScreenRef, int>(sideScreen, sideScreenSortOrder));
        }
      }
      if (!flag)
      {
        if ((this.CanObjectDisplayTabOfType(this.target, DetailsScreen.SidescreenTabTypes.Material) ? 1 : (this.CanObjectDisplayTabOfType(this.target, DetailsScreen.SidescreenTabTypes.Blueprints) ? 1 : 0)) == 0)
          this.sideScreen.SetActive(false);
        else
          this.sideScreen.SetActive(true);
      }
    }
    this.sortedSideScreens.Sort((Comparison<KeyValuePair<DetailsScreen.SideScreenRef, int>>) ((x, y) => x.Value <= y.Value ? 1 : -1));
    for (int index = 0; index < this.sortedSideScreens.Count; ++index)
      this.sortedSideScreens[index].Key.screenInstance.transform.SetSiblingIndex(index);
    for (int index = 0; index < this.sidescreenTabs.Length; ++index)
    {
      DetailsScreen.SidescreenTab tab = this.sidescreenTabs[index];
      tab.RepositionTitle();
      KeyValuePair<DetailsScreen.SideScreenRef, int> keyValuePair = this.sortedSideScreens.Find((Predicate<KeyValuePair<DetailsScreen.SideScreenRef, int>>) (t => t.Key.tab == tab.type));
      tab.SetNoConfigMessageVisibility(keyValuePair.Key == null);
    }
    this.RefreshTitle();
  }

  public void RefreshTitle()
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    this.TabTitle.SetTitle(this.target.GetProperName());
    for (int index = 0; index < this.sidescreenTabs.Length; ++index)
    {
      DetailsScreen.SidescreenTab tab = this.sidescreenTabs[index];
      if (tab.IsVisible)
      {
        KeyValuePair<DetailsScreen.SideScreenRef, int> keyValuePair = this.sortedSideScreens.Find((Predicate<KeyValuePair<DetailsScreen.SideScreenRef, int>>) (match => match.Key.tab == tab.type));
        if (keyValuePair.Key != null)
        {
          tab.SetTitleVisibility(keyValuePair.Key.screenInstance.CheckShouldShowTopTitle == null || keyValuePair.Key.screenInstance.CheckShouldShowTopTitle());
          tab.SetTitle(keyValuePair.Key.screenInstance.GetTitle());
        }
        else
        {
          tab.SetTitle((string) STRINGS.UI.UISIDESCREENS.NOCONFIG.TITLE);
          tab.SetTitleVisibility(tab.type == DetailsScreen.SidescreenTabTypes.Config || tab.type == DetailsScreen.SidescreenTabTypes.Errands);
        }
      }
      if (tab.type == DetailsScreen.SidescreenTabTypes.Config)
      {
        tab.Tooltip_Key = !((UnityEngine.Object) this.target.GetComponent<MinionIdentity>() == (UnityEngine.Object) null) ? "STRINGS.UI.DETAILTABS.CONFIGURATION.TOOLTIP_DUPLICANT" : "STRINGS.UI.DETAILTABS.CONFIGURATION.TOOLTIP";
        tab.tabInstance.GetComponent<ToolTip>().SetSimpleTooltip((string) Strings.Get(tab.Tooltip_Key));
      }
    }
  }

  private void SelectSideScreenTab(DetailsScreen.SidescreenTabTypes tabID)
  {
    this.selectedSidescreenTabID = tabID;
    this.RefreshSideScreenTabs();
  }

  private void RefreshSideScreenTabs()
  {
    int num1 = 1;
    for (int index = 0; index < this.sidescreenTabs.Length; ++index)
    {
      DetailsScreen.SidescreenTab sidescreenTab = this.sidescreenTabs[index];
      bool visible = sidescreenTab.ValidateTarget(this.target);
      sidescreenTab.SetVisible(visible);
      sidescreenTab.SetSelected(this.selectedSidescreenTabID == sidescreenTab.type);
      num1 += visible ? 1 : 0;
    }
    this.RefreshTitle();
    switch (this.selectedSidescreenTabID)
    {
      case DetailsScreen.SidescreenTabTypes.Material:
        this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Material).bodyInstance.GetComponentInChildren<DetailsScreenMaterialPanel>().SetTarget(this.target);
        break;
      case DetailsScreen.SidescreenTabTypes.Blueprints:
        DetailsScreen.SidescreenTab tabOfType = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Blueprints);
        CosmeticsPanel reference1 = tabOfType.bodyInstance.GetComponent<HierarchyReferences>().GetReference<CosmeticsPanel>("CosmeticsPanel");
        reference1.SetTarget(this.target);
        reference1.Refresh();
        CosmeticsPanel reference2 = tabOfType.OverrideBody.GetComponent<HierarchyReferences>().GetReference<CosmeticsPanel>("CosmeticsPanel");
        LayoutRebuilder.ForceRebuildLayoutImmediate(reference2.GetComponent<RectTransform>());
        float num2 = Mathf.Min(384f, reference2.GetComponent<RectTransform>().sizeDelta.y + 16f);
        tabOfType.OverrideBody.GetComponent<LayoutElement>().minHeight = num2;
        tabOfType.OverrideBody.GetComponent<LayoutElement>().preferredHeight = num2;
        break;
    }
    this.sidescreenTabHeader.SetActive(num1 > 1);
  }

  public KScreen SetSecondarySideScreen(KScreen secondaryPrefab, string title)
  {
    this.ClearSecondarySideScreen();
    if (this.instantiatedSecondarySideScreens.ContainsKey(secondaryPrefab))
    {
      this.activeSideScreen2 = this.instantiatedSecondarySideScreens[secondaryPrefab];
      this.activeSideScreen2.gameObject.SetActive(true);
    }
    else
    {
      this.activeSideScreen2 = KScreenManager.Instance.InstantiateScreen(secondaryPrefab.gameObject, this.sideScreen2ContentBody);
      this.activeSideScreen2.Activate();
      this.instantiatedSecondarySideScreens.Add(secondaryPrefab, this.activeSideScreen2);
    }
    this.sideScreen2Title.text = title;
    this.sideScreen2.SetActive(true);
    return this.activeSideScreen2;
  }

  public void ClearSecondarySideScreen()
  {
    if ((UnityEngine.Object) this.activeSideScreen2 != (UnityEngine.Object) null)
    {
      this.activeSideScreen2.gameObject.SetActive(false);
      this.activeSideScreen2 = (KScreen) null;
    }
    this.sideScreen2.SetActive(false);
  }

  public void DeactivateSideContent()
  {
    if ((UnityEngine.Object) SideDetailsScreen.Instance != (UnityEngine.Object) null && SideDetailsScreen.Instance.gameObject.activeInHierarchy)
      SideDetailsScreen.Instance.Show(false);
    if (this.sideScreens != null && this.sideScreens.Count > 0)
      this.sideScreens.ForEach((Action<DetailsScreen.SideScreenRef>) (scn =>
      {
        if (!((UnityEngine.Object) scn.screenInstance != (UnityEngine.Object) null))
          return;
        scn.screenInstance.ClearTarget();
        scn.screenInstance.Show(false);
      }));
    DetailsScreen.SidescreenTab tabOfType1 = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Material);
    DetailsScreen.SidescreenTab tabOfType2 = this.GetTabOfType(DetailsScreen.SidescreenTabTypes.Blueprints);
    tabOfType1.bodyInstance.GetComponentInChildren<DetailsScreenMaterialPanel>().SetTarget((GameObject) null);
    tabOfType2.bodyInstance.GetComponentInChildren<CosmeticsPanel>().SetTarget((GameObject) null);
    AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MenuOpenHalfEffect);
    this.sideScreen.SetActive(false);
  }

  public void MaskSideContent(bool hide)
  {
    if (hide)
      this.sideScreen.transform.localScale = Vector3.zero;
    else
      this.sideScreen.transform.localScale = Vector3.one;
  }

  public void DeselectAndClose()
  {
    if (this.gameObject.activeInHierarchy)
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back"));
    if ((UnityEngine.Object) this.GetActiveTab() != (UnityEngine.Object) null)
      this.GetActiveTab().SetTarget((GameObject) null);
    SelectTool.Instance.Select((KSelectable) null);
    ClusterMapSelectTool.Instance.Select((KSelectable) null);
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      return;
    this.target = (GameObject) null;
    this.previousTargetID = (Tag) (string) null;
    this.DeactivateSideContent();
    this.Show(false);
  }

  private void SortScreenOrder()
  {
    Array.Sort<DetailsScreen.Screens>(this.screens, (Comparison<DetailsScreen.Screens>) ((x, y) => x.displayOrderPriority.CompareTo(y.displayOrderPriority)));
  }

  public void UpdatePortrait(GameObject target)
  {
    KSelectable component1 = target.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    this.TabTitle.portrait.ClearPortrait();
    Building component2 = component1.GetComponent<Building>();
    if ((bool) (UnityEngine.Object) component2)
    {
      Sprite uiSprite = component2.Def.GetUISprite();
      if ((UnityEngine.Object) uiSprite != (UnityEngine.Object) null)
      {
        this.TabTitle.portrait.SetPortrait(uiSprite);
        return;
      }
    }
    if ((bool) (UnityEngine.Object) target.GetComponent<MinionIdentity>())
    {
      this.TabTitle.SetPortrait(component1.gameObject);
    }
    else
    {
      Edible component3 = target.GetComponent<Edible>();
      if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      {
        this.TabTitle.portrait.SetPortrait(Def.GetUISpriteFromMultiObjectAnim(component3.GetComponent<KBatchedAnimController>().AnimFiles[0]));
      }
      else
      {
        PrimaryElement component4 = target.GetComponent<PrimaryElement>();
        if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
        {
          this.TabTitle.portrait.SetPortrait(Def.GetUISpriteFromMultiObjectAnim(ElementLoader.FindElementByHash(component4.ElementID).substance.anim));
        }
        else
        {
          CellSelectionObject component5 = target.GetComponent<CellSelectionObject>();
          if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
          {
            string animName = component5.element.IsSolid ? "ui" : component5.element.substance.name;
            this.TabTitle.portrait.SetPortrait(Def.GetUISpriteFromMultiObjectAnim(component5.element.substance.anim, animName));
          }
          else
          {
            ICellSelectionProxy component6 = target.GetComponent<ICellSelectionProxy>();
            if (component6 == null || component6.Element == null || component6.Element.substance == null)
              return;
            string animName = component6.Element.IsSolid ? "ui" : component6.Element.substance.name;
            this.TabTitle.portrait.SetPortrait(Def.GetUISpriteFromMultiObjectAnim(component6.Element.substance.anim, animName));
          }
        }
      }
    }
  }

  public bool CompareTargetWith(GameObject compare) => (UnityEngine.Object) this.target == (UnityEngine.Object) compare;

  public void UpdateTitle()
  {
    this.CodexEntryButton_Refresh();
    this.PinResourceButton_Refresh();
    this.TabTitle.SetTitle(this.target.GetProperName());
    if (!((UnityEngine.Object) this.TabTitle != (UnityEngine.Object) null))
      return;
    this.TabTitle.SetTitle(this.target.GetProperName());
    MinionIdentity minionIdentity = (MinionIdentity) null;
    UserNameable userNameable = (UserNameable) null;
    ClustercraftExteriorDoor clusterCraftDoor = (ClustercraftExteriorDoor) null;
    CommandModule commandModule = (CommandModule) null;
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
    {
      minionIdentity = this.target.gameObject.GetComponent<MinionIdentity>();
      userNameable = this.target.gameObject.GetComponent<UserNameable>();
      clusterCraftDoor = this.target.gameObject.GetComponent<ClustercraftExteriorDoor>();
      commandModule = this.target.gameObject.GetComponent<CommandModule>();
    }
    if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
    {
      this.TabTitle.SetSubText(minionIdentity.GetComponent<MinionResume>().GetSkillsSubtitle());
      this.TabTitle.SetUserEditable(true);
    }
    else if ((UnityEngine.Object) userNameable != (UnityEngine.Object) null)
    {
      this.TabTitle.SetSubText("");
      this.TabTitle.SetUserEditable(true);
    }
    else if ((UnityEngine.Object) commandModule != (UnityEngine.Object) null)
    {
      this.TrySetRocketTitle(commandModule);
      this.TabTitle.SetSubText(commandModule.GetComponent<BuildingComplete>().Def.Name);
    }
    else if ((UnityEngine.Object) clusterCraftDoor != (UnityEngine.Object) null)
    {
      this.TrySetRocketTitle(clusterCraftDoor);
      this.TabTitle.SetSubText(clusterCraftDoor.GetComponent<BuildingComplete>().Def.Name);
    }
    else
    {
      this.TabTitle.SetSubText("");
      this.TabTitle.SetUserEditable(false);
    }
    this.TabTitle.UpdateRenameTooltip(this.target);
  }

  private void TrySetRocketTitle(ClustercraftExteriorDoor clusterCraftDoor1)
  {
    if (clusterCraftDoor1.HasTargetWorld())
    {
      WorldContainer targetWorld = clusterCraftDoor1.GetTargetWorld();
      this.TabTitle.SetTitle(targetWorld.GetComponent<ClusterGridEntity>().Name);
      this.TabTitle.SetUserEditable(true);
      clusterCraftDoor1.GetComponent<KSelectable>().SetName(targetWorld.GetComponent<ClusterGridEntity>().Name);
      this.setRocketTitleHandle = -1;
    }
    else
    {
      if (this.setRocketTitleHandle != -1)
        return;
      this.setRocketTitleHandle = this.target.Subscribe(-71801987, (Action<object>) (clusterCraftDoor2 =>
      {
        this.OnRefreshData((object) null);
        this.target.Unsubscribe(this.setRocketTitleHandle);
        this.setRocketTitleHandle = -1;
      }));
    }
  }

  private void TrySetRocketTitle(CommandModule commandModule)
  {
    if (!((UnityEngine.Object) commandModule != (UnityEngine.Object) null))
      return;
    string rocketName = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(commandModule.GetComponent<LaunchConditionManager>()).GetRocketName();
    commandModule.GetComponent<KSelectable>().SetName(rocketName);
    this.TabTitle.SetTitle(rocketName);
    this.TabTitle.SetUserEditable(true);
  }

  public TargetPanel GetActiveTab() => this.tabHeader.ActivePanel;

  [Serializable]
  private struct Screens
  {
    public string name;
    public string displayName;
    public string tooltip;
    public Sprite icon;
    public TargetPanel screen;
    public int displayOrderPriority;
    public bool hideWhenDead;
    public HashedString focusInViewMode;
    [HideInInspector]
    public int tabIdx;
  }

  public enum SidescreenTabTypes
  {
    Config,
    Errands,
    Material,
    Blueprints,
  }

  [Serializable]
  public class SidescreenTab
  {
    public DetailsScreen.SidescreenTabTypes type;
    public string Title_Key;
    public string Tooltip_Key;
    public Sprite Icon;
    public GameObject OverrideBody;
    public Func<GameObject, DetailsScreen.SidescreenTab, bool> ValidateTargetCallback;
    public System.Action OnClicked;
    [NonSerialized]
    public MultiToggle tabInstance;
    [NonSerialized]
    public GameObject bodyInstance;
    private HierarchyReferences bodyReferences;
    private const string bodyRef_Title = "Title";
    private const string bodyRef_TitleLabel = "TitleLabel";
    private const string bodyRef_NoConfigMessage = "NoConfigMessage";

    private void OnTabClicked()
    {
      System.Action onClicked = this.OnClicked;
      if (onClicked == null)
        return;
      onClicked();
    }

    public bool IsVisible { private set; get; }

    public bool IsSelected { private set; get; }

    public void Initiate(
      GameObject originalTabInstance,
      GameObject originalBodyInstance,
      Action<DetailsScreen.SidescreenTab> on_tab_clicked_callback)
    {
      if (on_tab_clicked_callback != null)
        this.OnClicked = (System.Action) (() => on_tab_clicked_callback(this));
      originalBodyInstance.gameObject.SetActive(false);
      if ((UnityEngine.Object) this.OverrideBody == (UnityEngine.Object) null)
      {
        this.bodyInstance = UnityEngine.Object.Instantiate<GameObject>(originalBodyInstance);
        this.bodyInstance.name = this.type.ToString() + " Tab - body instance";
        this.bodyInstance.SetActive(true);
        this.bodyInstance.transform.SetParent(originalBodyInstance.transform.parent, false);
      }
      else
        this.bodyInstance = this.OverrideBody;
      this.bodyReferences = this.bodyInstance.GetComponent<HierarchyReferences>();
      originalTabInstance.gameObject.SetActive(false);
      if (!((UnityEngine.Object) this.tabInstance == (UnityEngine.Object) null))
        return;
      this.tabInstance = UnityEngine.Object.Instantiate<GameObject>(originalTabInstance.gameObject).GetComponent<MultiToggle>();
      this.tabInstance.name = this.type.ToString() + " Tab Instance";
      this.tabInstance.gameObject.SetActive(true);
      this.tabInstance.transform.SetParent(originalTabInstance.transform.parent, false);
      this.tabInstance.onClick += new System.Action(this.OnTabClicked);
      HierarchyReferences component = this.tabInstance.GetComponent<HierarchyReferences>();
      component.GetReference<LocText>("label").SetText((string) Strings.Get(this.Title_Key));
      component.GetReference<Image>("icon").sprite = this.Icon;
      this.tabInstance.GetComponent<ToolTip>().SetSimpleTooltip((string) Strings.Get(this.Tooltip_Key));
    }

    public void SetSelected(bool isSelected)
    {
      this.IsSelected = isSelected;
      this.tabInstance.ChangeState(isSelected ? 1 : 0);
      this.bodyInstance.SetActive(isSelected);
    }

    public void SetTitle(string title)
    {
      if (!((UnityEngine.Object) this.bodyReferences != (UnityEngine.Object) null) || !this.bodyReferences.HasReference("TitleLabel"))
        return;
      this.bodyReferences.GetReference<LocText>("TitleLabel").SetText(title);
    }

    public void SetTitleVisibility(bool visible)
    {
      if (!((UnityEngine.Object) this.bodyReferences != (UnityEngine.Object) null) || !this.bodyReferences.HasReference("Title"))
        return;
      Component reference = this.bodyReferences.GetReference("Title");
      reference.gameObject.SetActive(visible);
      reference.transform.parent.GetComponent<LayoutGroup>().padding.top = visible ? 24 : 0;
    }

    public void SetNoConfigMessageVisibility(bool visible)
    {
      if (!((UnityEngine.Object) this.bodyReferences != (UnityEngine.Object) null) || !this.bodyReferences.HasReference("NoConfigMessage"))
        return;
      this.bodyReferences.GetReference("NoConfigMessage").gameObject.SetActive(visible);
    }

    public void RepositionTitle()
    {
      if (!((UnityEngine.Object) this.bodyReferences != (UnityEngine.Object) null) || !((UnityEngine.Object) this.bodyReferences.GetReference("Title") != (UnityEngine.Object) null))
        return;
      this.bodyReferences.GetReference("Title").transform.SetSiblingIndex(0);
    }

    public void SetVisible(bool visible)
    {
      this.IsVisible = visible;
      this.tabInstance.gameObject.SetActive(visible);
      this.bodyInstance.SetActive(this.IsSelected && this.IsVisible);
    }

    public bool ValidateTarget(GameObject target)
    {
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return false;
      return this.ValidateTargetCallback == null || this.ValidateTargetCallback(target, this);
    }
  }

  [Serializable]
  public class SideScreenRef
  {
    public string name;
    public SideScreenContent screenPrefab;
    public Vector2 offset;
    public DetailsScreen.SidescreenTabTypes tab;
    [HideInInspector]
    public SideScreenContent screenInstance;
  }
}
