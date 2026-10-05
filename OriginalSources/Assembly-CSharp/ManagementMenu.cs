// Decompiled with JetBrains decompiler
// Type: ManagementMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class ManagementMenu : KIconToggleMenu
{
  private const float UI_WIDTH_COMPRESS_THRESHOLD = 1300f;
  [MyCmpReq]
  public ManagementMenuNotificationDisplayer notificationDisplayer;
  public static ManagementMenu Instance;
  [Header("Management Menu Specific")]
  [SerializeField]
  private KToggle smallPrefab;
  [SerializeField]
  private KToggle researchButtonPrefab;
  public KToggle PauseMenuButton;
  [Header("Top Right Screen References")]
  public JobsTableScreen jobsScreen;
  public VitalsTableScreen vitalsScreen;
  public ScheduleScreen scheduleScreen;
  public ReportScreen reportsScreen;
  public CodexScreen codexScreen;
  public ConsumablesTableScreen consumablesScreen;
  private StarmapScreen starmapScreen;
  private ClusterMapScreen clusterMapScreen;
  private SkillsScreen skillsScreen;
  private ResearchScreen researchScreen;
  [Header("Notification Styles")]
  public ColorStyleSetting noAlertColorStyle;
  public List<ColorStyleSetting> alertColorStyle;
  public List<TextStyleSetting> alertTextStyle;
  private ManagementMenu.ManagementMenuToggleInfo jobsInfo;
  private ManagementMenu.ManagementMenuToggleInfo consumablesInfo;
  private ManagementMenu.ManagementMenuToggleInfo scheduleInfo;
  private ManagementMenu.ManagementMenuToggleInfo vitalsInfo;
  private ManagementMenu.ManagementMenuToggleInfo reportsInfo;
  private ManagementMenu.ManagementMenuToggleInfo researchInfo;
  private ManagementMenu.ManagementMenuToggleInfo codexInfo;
  private ManagementMenu.ManagementMenuToggleInfo starmapInfo;
  private ManagementMenu.ManagementMenuToggleInfo clusterMapInfo;
  private ManagementMenu.ManagementMenuToggleInfo skillsInfo;
  private ManagementMenu.ManagementMenuToggleInfo[] fullscreenUIs;
  private Dictionary<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData> ScreenInfoMatch = new Dictionary<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData>();
  private ManagementMenu.ScreenData activeScreen;
  private KButton activeButton;
  private string skillsTooltip;
  private string skillsTooltipDisabled;
  private string researchTooltip;
  private string researchTooltipDisabled;
  private string starmapTooltip;
  private string starmapTooltipDisabled;
  private string clusterMapTooltip;
  private string clusterMapTooltipDisabled;
  private List<KScreen> mutuallyExclusiveScreens = new List<KScreen>();

  public static void DestroyInstance() => ManagementMenu.Instance = (ManagementMenu) null;

  public bool HasActiveResearch => this.researchScreen.HasActiveResearch;

  public override float GetSortKey() => 21f;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    ManagementMenu.Instance = this;
    this.notificationDisplayer.onNotificationsChanged += new System.Action(this.OnNotificationsChanged);
    CodexCache.CodexCacheInit();
    ScheduledUIInstantiation component = GameScreenManager.Instance.GetComponent<ScheduledUIInstantiation>();
    this.starmapScreen = component.GetInstantiatedObject<StarmapScreen>();
    this.clusterMapScreen = component.GetInstantiatedObject<ClusterMapScreen>();
    this.skillsScreen = component.GetInstantiatedObject<SkillsScreen>();
    this.researchScreen = component.GetInstantiatedObject<ResearchScreen>();
    this.fullscreenUIs = new ManagementMenu.ManagementMenuToggleInfo[4]
    {
      this.researchInfo,
      this.skillsInfo,
      this.starmapInfo,
      this.clusterMapInfo
    };
    this.Subscribe(Game.Instance.gameObject, 288942073, new Action<object>(this.OnUIClear));
    this.consumablesInfo = new ManagementMenu.ManagementMenuToggleInfo((string) UI.CONSUMABLES, "OverviewUI_consumables_icon", hotkey: Action.ManageConsumables, tooltip: (string) UI.TOOLTIPS.MANAGEMENTMENU_CONSUMABLES);
    this.AddToggleTooltip(this.consumablesInfo);
    this.vitalsInfo = new ManagementMenu.ManagementMenuToggleInfo((string) UI.VITALS, "OverviewUI_vitals_icon", hotkey: Action.ManageVitals, tooltip: (string) UI.TOOLTIPS.MANAGEMENTMENU_VITALS);
    this.AddToggleTooltip(this.vitalsInfo);
    this.researchInfo = new ManagementMenu.ManagementMenuToggleInfo((string) UI.RESEARCH, "OverviewUI_research_nav_icon", hotkey: Action.ManageResearch, tooltip: (string) UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH);
    this.AddToggleTooltipForResearch(this.researchInfo, (string) UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_RESEARCH);
    this.researchInfo.prefabOverride = this.researchButtonPrefab;
    this.jobsInfo = new ManagementMenu.ManagementMenuToggleInfo((string) UI.JOBS, "OverviewUI_priority_icon", hotkey: Action.ManagePriorities, tooltip: (string) UI.TOOLTIPS.MANAGEMENTMENU_JOBS);
    this.AddToggleTooltip(this.jobsInfo);
    this.skillsInfo = new ManagementMenu.ManagementMenuToggleInfo((string) UI.SKILLS, "OverviewUI_jobs_icon", hotkey: Action.ManageSkills, tooltip: (string) UI.TOOLTIPS.MANAGEMENTMENU_SKILLS);
    this.AddToggleTooltip(this.skillsInfo, (string) UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_SKILL_STATION);
    this.starmapInfo = new ManagementMenu.ManagementMenuToggleInfo((string) UI.STARMAP.MANAGEMENT_BUTTON, "OverviewUI_starmap_icon", hotkey: Action.ManageStarmap, tooltip: (string) UI.TOOLTIPS.MANAGEMENTMENU_STARMAP);
    this.AddToggleTooltip(this.starmapInfo, (string) UI.TOOLTIPS.MANAGEMENTMENU_REQUIRES_TELESCOPE);
    this.clusterMapInfo = new ManagementMenu.ManagementMenuToggleInfo((string) UI.STARMAP.MANAGEMENT_BUTTON, "OverviewUI_starmap_icon", hotkey: Action.ManageStarmap, tooltip: (string) UI.TOOLTIPS.MANAGEMENTMENU_STARMAP);
    this.AddToggleTooltip(this.clusterMapInfo);
    this.scheduleInfo = new ManagementMenu.ManagementMenuToggleInfo((string) UI.SCHEDULE, "OverviewUI_schedule2_icon", hotkey: Action.ManageSchedule, tooltip: (string) UI.TOOLTIPS.MANAGEMENTMENU_SCHEDULE);
    this.AddToggleTooltip(this.scheduleInfo);
    this.reportsInfo = new ManagementMenu.ManagementMenuToggleInfo((string) UI.REPORT, "OverviewUI_reports_icon", hotkey: Action.ManageReport, tooltip: (string) UI.TOOLTIPS.MANAGEMENTMENU_DAILYREPORT);
    this.AddToggleTooltip(this.reportsInfo);
    this.reportsInfo.prefabOverride = this.smallPrefab;
    this.codexInfo = new ManagementMenu.ManagementMenuToggleInfo((string) UI.CODEX.MANAGEMENT_BUTTON, "OverviewUI_database_icon", hotkey: Action.ManageDatabase, tooltip: (string) UI.TOOLTIPS.MANAGEMENTMENU_CODEX);
    this.AddToggleTooltip(this.codexInfo);
    this.codexInfo.prefabOverride = this.smallPrefab;
    this.ScreenInfoMatch.Add(this.consumablesInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.consumablesScreen,
      tabIdx = 3,
      toggleInfo = this.consumablesInfo,
      cancelHandler = (Func<bool>) null
    });
    this.ScreenInfoMatch.Add(this.vitalsInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.vitalsScreen,
      tabIdx = 2,
      toggleInfo = this.vitalsInfo,
      cancelHandler = (Func<bool>) null
    });
    this.ScreenInfoMatch.Add(this.reportsInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.reportsScreen,
      tabIdx = 4,
      toggleInfo = this.reportsInfo,
      cancelHandler = (Func<bool>) null
    });
    this.ScreenInfoMatch.Add(this.jobsInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.jobsScreen,
      tabIdx = 1,
      toggleInfo = this.jobsInfo,
      cancelHandler = (Func<bool>) null
    });
    this.ScreenInfoMatch.Add(this.skillsInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.skillsScreen,
      tabIdx = 0,
      toggleInfo = this.skillsInfo,
      cancelHandler = (Func<bool>) null
    });
    this.ScreenInfoMatch.Add(this.codexInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.codexScreen,
      tabIdx = 6,
      toggleInfo = this.codexInfo,
      cancelHandler = (Func<bool>) null
    });
    this.ScreenInfoMatch.Add(this.scheduleInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.scheduleScreen,
      tabIdx = 7,
      toggleInfo = this.scheduleInfo,
      cancelHandler = (Func<bool>) null
    });
    if (DlcManager.FeatureClusterSpaceEnabled())
      this.ScreenInfoMatch.Add(this.clusterMapInfo, new ManagementMenu.ScreenData()
      {
        screen = (KScreen) this.clusterMapScreen,
        tabIdx = 7,
        toggleInfo = this.clusterMapInfo,
        cancelHandler = new Func<bool>(this.clusterMapScreen.TryHandleCancel)
      });
    else
      this.ScreenInfoMatch.Add(this.starmapInfo, new ManagementMenu.ScreenData()
      {
        screen = (KScreen) this.starmapScreen,
        tabIdx = 7,
        toggleInfo = this.starmapInfo,
        cancelHandler = (Func<bool>) null
      });
    this.ScreenInfoMatch.Add(this.researchInfo, new ManagementMenu.ScreenData()
    {
      screen = (KScreen) this.researchScreen,
      tabIdx = 5,
      toggleInfo = this.researchInfo,
      cancelHandler = (Func<bool>) null
    });
    List<KIconToggleMenu.ToggleInfo> toggleInfo = new List<KIconToggleMenu.ToggleInfo>();
    toggleInfo.Add((KIconToggleMenu.ToggleInfo) this.vitalsInfo);
    toggleInfo.Add((KIconToggleMenu.ToggleInfo) this.consumablesInfo);
    toggleInfo.Add((KIconToggleMenu.ToggleInfo) this.jobsInfo);
    toggleInfo.Add((KIconToggleMenu.ToggleInfo) this.scheduleInfo);
    toggleInfo.Add((KIconToggleMenu.ToggleInfo) this.skillsInfo);
    toggleInfo.Add((KIconToggleMenu.ToggleInfo) this.researchInfo);
    if (DlcManager.FeatureClusterSpaceEnabled())
      toggleInfo.Add((KIconToggleMenu.ToggleInfo) this.clusterMapInfo);
    else
      toggleInfo.Add((KIconToggleMenu.ToggleInfo) this.starmapInfo);
    toggleInfo.Add((KIconToggleMenu.ToggleInfo) this.reportsInfo);
    toggleInfo.Add((KIconToggleMenu.ToggleInfo) this.codexInfo);
    this.Setup((IList<KIconToggleMenu.ToggleInfo>) toggleInfo);
    this.onSelect += new KIconToggleMenu.OnSelect(this.OnButtonClick);
    this.PauseMenuButton.onClick += new System.Action(this.OnPauseMenuClicked);
    this.PauseMenuButton.transform.SetAsLastSibling();
    this.PauseMenuButton.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_PAUSEMENU, Action.Escape);
    KInputManager.InputChange.AddListener(new UnityAction(this.OnInputChanged));
    Components.ResearchCenters.OnAdd += new Action<IResearchCenter>(this.CheckResearch);
    Components.ResearchCenters.OnRemove += new Action<IResearchCenter>(this.CheckResearch);
    Components.RoleStations.OnAdd += new Action<RoleStation>(this.CheckSkills);
    Components.RoleStations.OnRemove += new Action<RoleStation>(this.CheckSkills);
    Game.Instance.Subscribe(-809948329, new Action<object>(this.CheckResearch));
    Game.Instance.Subscribe(-809948329, new Action<object>(this.CheckSkills));
    Game.Instance.Subscribe(445618876, new Action<object>(this.OnResolutionChanged));
    if (!DlcManager.FeatureClusterSpaceEnabled())
    {
      Components.Telescopes.OnAdd += new Action<Telescope>(this.CheckStarmap);
      Components.Telescopes.OnRemove += new Action<Telescope>(this.CheckStarmap);
    }
    this.CheckResearch((object) null);
    this.CheckSkills();
    if (!DlcManager.FeatureClusterSpaceEnabled())
      this.CheckStarmap();
    this.researchInfo.toggle.soundPlayer.AcceptClickCondition = (Func<bool>) (() => this.ResearchAvailable() || this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
    foreach (KToggle toggle in this.toggles)
    {
      toggle.soundPlayer.toggle_widget_sound_events[0].PlaySound = false;
      toggle.soundPlayer.toggle_widget_sound_events[1].PlaySound = false;
    }
    this.OnResolutionChanged();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.mutuallyExclusiveScreens.Add((KScreen) AllResourcesScreen.Instance);
    this.mutuallyExclusiveScreens.Add((KScreen) AllDiagnosticsScreen.Instance);
    this.OnNotificationsChanged();
  }

  protected override void OnForcedCleanUp()
  {
    KInputManager.InputChange.RemoveListener(new UnityAction(this.OnInputChanged));
    base.OnForcedCleanUp();
  }

  private void OnInputChanged()
  {
    this.PauseMenuButton.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_PAUSEMENU, Action.Escape);
    this.consumablesInfo.tooltip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_CONSUMABLES, this.consumablesInfo.hotKey);
    this.vitalsInfo.tooltip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_VITALS, this.vitalsInfo.hotKey);
    this.researchInfo.tooltip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH, this.researchInfo.hotKey);
    this.jobsInfo.tooltip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_JOBS, this.jobsInfo.hotKey);
    this.skillsInfo.tooltip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_SKILLS, this.skillsInfo.hotKey);
    this.starmapInfo.tooltip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, this.starmapInfo.hotKey);
    this.clusterMapInfo.tooltip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_STARMAP, this.clusterMapInfo.hotKey);
    this.scheduleInfo.tooltip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_SCHEDULE, this.scheduleInfo.hotKey);
    this.reportsInfo.tooltip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_DAILYREPORT, this.reportsInfo.hotKey);
    this.codexInfo.tooltip = GameUtil.ReplaceHotkeyString((string) UI.TOOLTIPS.MANAGEMENTMENU_CODEX, this.codexInfo.hotKey);
  }

  private void OnResolutionChanged(object data = null)
  {
    bool flag = (double) Screen.width < 1300.0;
    foreach (Component toggle in this.toggles)
    {
      HierarchyReferences component = toggle.GetComponent<HierarchyReferences>();
      if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
      {
        RectTransform reference = component.GetReference<RectTransform>("TextContainer");
        if (!((UnityEngine.Object) reference == (UnityEngine.Object) null))
          reference.gameObject.SetActive(!flag);
      }
    }
  }

  private void OnNotificationsChanged()
  {
    foreach (KeyValuePair<ManagementMenu.ManagementMenuToggleInfo, ManagementMenu.ScreenData> keyValuePair in this.ScreenInfoMatch)
      keyValuePair.Key.SetNotificationDisplay(false, false, (ColorStyleSetting) null, this.noAlertColorStyle);
  }

  private ToolTip.ComplexTooltipDelegate CreateToggleTooltip(
    ManagementMenu.ManagementMenuToggleInfo toggleInfo,
    string disabledTooltip = null)
  {
    return (ToolTip.ComplexTooltipDelegate) (() =>
    {
      List<Tuple<string, TextStyleSetting>> toggleTooltip = new List<Tuple<string, TextStyleSetting>>();
      if (disabledTooltip != null && !toggleInfo.toggle.interactable)
      {
        toggleTooltip.Add(new Tuple<string, TextStyleSetting>(disabledTooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
        return toggleTooltip;
      }
      if (toggleInfo.tooltipHeader != null)
        toggleTooltip.Add(new Tuple<string, TextStyleSetting>(toggleInfo.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
      toggleTooltip.Add(new Tuple<string, TextStyleSetting>(toggleInfo.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
      return toggleTooltip;
    });
  }

  private void AddToggleTooltip(
    ManagementMenu.ManagementMenuToggleInfo toggleInfo,
    string disabledTooltip = null)
  {
    toggleInfo.getTooltipText = this.CreateToggleTooltip(toggleInfo, disabledTooltip);
  }

  private void AddToggleTooltipForResearch(
    ManagementMenu.ManagementMenuToggleInfo toggleInfo,
    string disabledTooltip = null)
  {
    toggleInfo.getTooltipText = (ToolTip.ComplexTooltipDelegate) (() =>
    {
      List<Tuple<string, TextStyleSetting>> tupleList = new List<Tuple<string, TextStyleSetting>>();
      TechInstance activeResearch = Research.Instance.GetActiveResearch();
      string a1 = activeResearch == null ? (string) UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_NO_RESEARCH : string.Format((string) UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_CARD_NAME, (object) activeResearch.tech.Name);
      tupleList.Add(new Tuple<string, TextStyleSetting>(a1, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
      if (activeResearch != null)
      {
        string a2 = "";
        for (int index = 0; index < activeResearch.tech.unlockedItems.Count; ++index)
        {
          TechItem unlockedItem = activeResearch.tech.unlockedItems[index];
          a2 = $"{a2}\n{string.Format((string) UI.TOOLTIPS.MANAGEMENTMENU_RESEARCH_ITEM_LINE, (object) unlockedItem.Name)}";
        }
        tupleList.Add(new Tuple<string, TextStyleSetting>(a2, ToolTipScreen.Instance.defaultTooltipBodyStyle));
      }
      if (disabledTooltip != null && !toggleInfo.toggle.interactable)
      {
        tupleList.Add(new Tuple<string, TextStyleSetting>(disabledTooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
        return tupleList;
      }
      if (toggleInfo.tooltipHeader != null)
        tupleList.Add(new Tuple<string, TextStyleSetting>(toggleInfo.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
      tupleList.Add(new Tuple<string, TextStyleSetting>("\n" + toggleInfo.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
      return tupleList;
    });
  }

  public bool IsFullscreenUIActive()
  {
    if (this.activeScreen == null)
      return false;
    foreach (ManagementMenu.ManagementMenuToggleInfo fullscreenUi in this.fullscreenUIs)
    {
      if (this.activeScreen.toggleInfo == fullscreenUi)
        return true;
    }
    return false;
  }

  private void OnPauseMenuClicked()
  {
    PauseScreen.Instance.Show();
    this.PauseMenuButton.isOn = false;
  }

  public void Refresh()
  {
    this.CheckResearch((object) null);
    this.CheckSkills();
    this.CheckStarmap();
  }

  public bool CheckHasResearchCenter()
  {
    return Components.ResearchCenters.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
  }

  public void CheckResearch(object o)
  {
    if ((UnityEngine.Object) this.researchInfo.toggle == (UnityEngine.Object) null)
      return;
    bool disabled = !this.CheckHasResearchCenter();
    bool active = !disabled && this.activeScreen != null && this.activeScreen.toggleInfo == this.researchInfo;
    this.ConfigureToggle(this.researchInfo.toggle, disabled, active);
  }

  public void CheckSkills(object o = null)
  {
    if ((UnityEngine.Object) this.skillsInfo.toggle == (UnityEngine.Object) null)
      return;
    this.ConfigureToggle(this.skillsInfo.toggle, Components.RoleStations.Count <= 0 && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive, this.activeScreen != null && this.activeScreen.toggleInfo == this.skillsInfo);
  }

  public void CheckStarmap(object o = null)
  {
    if ((UnityEngine.Object) this.starmapInfo.toggle == (UnityEngine.Object) null)
      return;
    this.ConfigureToggle(this.starmapInfo.toggle, Components.Telescopes.Count <= 0 && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive, this.activeScreen != null && this.activeScreen.toggleInfo == this.starmapInfo);
  }

  private void ConfigureToggle(KToggle toggle, bool disabled, bool active)
  {
    toggle.interactable = !disabled;
    if (disabled)
      toggle.GetComponentInChildren<ImageToggleState>().SetDisabled();
    else
      toggle.GetComponentInChildren<ImageToggleState>().SetActiveState(active);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.activeScreen != null && e.TryConsume(Action.Escape))
      this.ToggleIfCancelUnhandled(this.activeScreen);
    if (e.Consumed)
      return;
    base.OnKeyDown(e);
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (this.activeScreen != null && PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
      this.ToggleIfCancelUnhandled(this.activeScreen);
    if (e.Consumed)
      return;
    base.OnKeyUp(e);
  }

  private void ToggleIfCancelUnhandled(ManagementMenu.ScreenData screenData)
  {
    if (screenData.cancelHandler != null && screenData.cancelHandler())
      return;
    this.ToggleScreen(screenData);
  }

  private bool ResearchAvailable()
  {
    return Components.ResearchCenters.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
  }

  private bool SkillsAvailable()
  {
    return Components.RoleStations.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
  }

  public static bool StarmapAvailable()
  {
    return Components.Telescopes.Count > 0 || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
  }

  public void CloseAll()
  {
    if (this.activeScreen == null)
      return;
    if (this.activeScreen.toggleInfo != null)
      this.ToggleScreen(this.activeScreen);
    this.CloseActive();
    this.ClearSelection();
  }

  private void OnUIClear(object data) => this.CloseAll();

  public void ToggleScreen(ManagementMenu.ScreenData screenData)
  {
    if (screenData == null)
      return;
    if (screenData.toggleInfo == this.researchInfo && !this.ResearchAvailable())
    {
      this.CheckResearch((object) null);
      this.CloseActive();
    }
    else if (screenData.toggleInfo == this.skillsInfo && !this.SkillsAvailable())
    {
      this.CheckSkills();
      this.CloseActive();
    }
    else if (screenData.toggleInfo == this.starmapInfo && !ManagementMenu.StarmapAvailable())
    {
      this.CheckStarmap();
      this.CloseActive();
    }
    else
    {
      if (screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().IsDisabled)
        return;
      if (this.activeScreen != null)
      {
        this.activeScreen.toggleInfo.toggle.isOn = false;
        this.activeScreen.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetInactive();
      }
      if (this.activeScreen != screenData)
      {
        OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID);
        if (this.activeScreen != null)
          this.activeScreen.toggleInfo.toggle.ActivateFlourish(false);
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open"));
        AudioMixer.instance.Start(AudioMixerSnapshots.Get().MenuOpenMigrated);
        screenData.toggleInfo.toggle.ActivateFlourish(true);
        screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetActive();
        this.CloseActive();
        this.activeScreen = screenData;
        if (!this.activeScreen.screen.IsActive())
          this.activeScreen.screen.Activate();
        this.activeScreen.screen.Show();
        foreach (ManagementMenuNotification menuNotification in this.notificationDisplayer.GetNotificationsForAction(screenData.toggleInfo.hotKey))
        {
          if (menuNotification.customClickCallback != null)
          {
            menuNotification.customClickCallback(menuNotification.customClickData);
            break;
          }
        }
        foreach (KScreen mutuallyExclusiveScreen in this.mutuallyExclusiveScreens)
          mutuallyExclusiveScreen.Show(false);
      }
      else
      {
        this.activeScreen.screen.Show(false);
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close"));
        AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MenuOpenMigrated);
        this.activeScreen.toggleInfo.toggle.ActivateFlourish(false);
        this.activeScreen = (ManagementMenu.ScreenData) null;
        screenData.toggleInfo.toggle.gameObject.GetComponentInChildren<ImageToggleState>().SetInactive();
      }
    }
  }

  public void OnButtonClick(KIconToggleMenu.ToggleInfo toggle_info)
  {
    this.ToggleScreen(this.ScreenInfoMatch[(ManagementMenu.ManagementMenuToggleInfo) toggle_info]);
  }

  private void CloseActive()
  {
    if (this.activeScreen == null)
      return;
    this.activeScreen.toggleInfo.toggle.isOn = false;
    this.activeScreen.screen.Show(false);
    this.activeScreen = (ManagementMenu.ScreenData) null;
  }

  public void ToggleResearch()
  {
    if (!this.ResearchAvailable() && this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo] || this.researchInfo == null)
      return;
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
  }

  public void ToggleCodex()
  {
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.codexInfo]);
  }

  public void OpenCodexToLockId(string lockId, bool focusContent = false)
  {
    string entryForLock = CodexCache.GetEntryForLock(lockId);
    if (entryForLock == null)
    {
      DebugUtil.LogWarningArgs((object) $"Could not open codex to lockId \"{lockId}\", couldn't find an entry that contained that lockId");
    }
    else
    {
      ContentContainer targetContainer = (ContentContainer) null;
      if (focusContent)
      {
        CodexEntry entry = CodexCache.FindEntry(entryForLock);
        for (int index = 0; targetContainer == null && index < entry.contentContainers.Count; ++index)
        {
          if (!(entry.contentContainers[index].lockID != lockId))
            targetContainer = entry.contentContainers[index];
        }
      }
      this.OpenCodexToEntry(entryForLock, targetContainer);
    }
  }

  public void OpenCodexToEntry(string id, ContentContainer targetContainer = null)
  {
    if (!this.codexScreen.gameObject.activeInHierarchy)
      this.ToggleCodex();
    this.codexScreen.ChangeArticle(id);
    this.codexScreen.FocusContainer(targetContainer);
  }

  public void ToggleSkills()
  {
    if (!this.SkillsAvailable() && this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo] || this.skillsInfo == null)
      return;
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]);
  }

  public void ToggleStarmap()
  {
    if (this.starmapInfo == null)
      return;
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo]);
  }

  public void ToggleClusterMap()
  {
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
  }

  public void TogglePriorities()
  {
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.jobsInfo]);
  }

  public void OpenReports(int day)
  {
    if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.reportsInfo])
      this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.reportsInfo]);
    ReportScreen.Instance.ShowReport(day);
  }

  public void OpenResearch(string zoomToTech = null)
  {
    if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo])
      this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.researchInfo]);
    if (zoomToTech == null)
      return;
    UIScheduler.Instance.Schedule("ResearchCameraFocus", 0.25f, (Action<object>) (data => this.researchScreen.ZoomToTech(zoomToTech, true)), (object) null, (SchedulerGroup) null);
  }

  public void OpenStarmap()
  {
    if (this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo])
      return;
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.starmapInfo]);
  }

  public void OpenClusterMap()
  {
    if (this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo])
      return;
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
  }

  public void CloseClusterMap()
  {
    if (this.activeScreen != this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo])
      return;
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.clusterMapInfo]);
  }

  public void OpenSkills(MinionIdentity minionIdentity)
  {
    this.skillsScreen.CurrentlySelectedMinion = (IAssignableIdentity) minionIdentity;
    if (this.activeScreen == this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo])
      return;
    this.ToggleScreen(this.ScreenInfoMatch[ManagementMenu.Instance.skillsInfo]);
  }

  public bool IsScreenOpen(KScreen screen)
  {
    return this.activeScreen != null && (UnityEngine.Object) this.activeScreen.screen == (UnityEngine.Object) screen;
  }

  public class ScreenData
  {
    public KScreen screen;
    public ManagementMenu.ManagementMenuToggleInfo toggleInfo;
    public Func<bool> cancelHandler;
    public int tabIdx;
  }

  public class ManagementMenuToggleInfo : KIconToggleMenu.ToggleInfo
  {
    public ImageToggleState alertImage;
    public ImageToggleState glowImage;
    private ColorStyleSetting originalButtonSetting;

    public ManagementMenuToggleInfo(
      string text,
      string icon,
      object user_data = null,
      Action hotkey = Action.NumActions,
      string tooltip = "",
      string tooltip_header = "")
      : base(text, icon, user_data, hotkey, tooltip, tooltip_header)
    {
      this.tooltip = GameUtil.ReplaceHotkeyString(this.tooltip, this.hotKey);
    }

    public void SetNotificationDisplay(
      bool showAlertImage,
      bool showGlow,
      ColorStyleSetting buttonColorStyle,
      ColorStyleSetting alertColorStyle)
    {
      ImageToggleState component = this.toggle.GetComponent<ImageToggleState>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) buttonColorStyle != (UnityEngine.Object) null)
          component.SetColorStyle(buttonColorStyle);
        else
          component.SetColorStyle(this.originalButtonSetting);
      }
      if ((UnityEngine.Object) this.alertImage != (UnityEngine.Object) null)
      {
        this.alertImage.gameObject.SetActive(showAlertImage);
        this.alertImage.SetColorStyle(alertColorStyle);
      }
      if (!((UnityEngine.Object) this.glowImage != (UnityEngine.Object) null))
        return;
      this.glowImage.gameObject.SetActive(showGlow);
      if (!((UnityEngine.Object) buttonColorStyle != (UnityEngine.Object) null))
        return;
      this.glowImage.SetColorStyle(buttonColorStyle);
    }

    public override void SetToggle(KToggle toggle)
    {
      base.SetToggle(toggle);
      ImageToggleState component1 = toggle.GetComponent<ImageToggleState>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        this.originalButtonSetting = component1.colorStyleSetting;
      HierarchyReferences component2 = toggle.GetComponent<HierarchyReferences>();
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
        return;
      this.alertImage = component2.GetReference<ImageToggleState>("AlertImage");
      this.glowImage = component2.GetReference<ImageToggleState>("GlowImage");
    }
  }
}
