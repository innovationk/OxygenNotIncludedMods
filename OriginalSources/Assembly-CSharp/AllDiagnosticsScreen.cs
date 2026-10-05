// Decompiled with JetBrains decompiler
// Type: AllDiagnosticsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class AllDiagnosticsScreen : ShowOptimizedKScreen, ISim4000ms, ISim1000ms
{
  private Dictionary<string, GameObject> diagnosticRows = new Dictionary<string, GameObject>();
  private Dictionary<string, Dictionary<string, GameObject>> criteriaRows = new Dictionary<string, Dictionary<string, GameObject>>();
  public GameObject rootListContainer;
  public GameObject diagnosticLinePrefab;
  public GameObject subDiagnosticLinePrefab;
  public KButton closeButton;
  public bool allowRefresh = true;
  [SerializeField]
  private KInputTextField searchInputField;
  [SerializeField]
  private KButton clearSearchButton;
  public static AllDiagnosticsScreen Instance;
  public Dictionary<Tag, bool> currentlyDisplayedRows = new Dictionary<Tag, bool>();
  public Dictionary<Tag, bool> subrowContainerOpen = new Dictionary<Tag, bool>();
  [SerializeField]
  private RectTransform debugNotificationToggleCotainer;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    AllDiagnosticsScreen.Instance = this;
    this.ConfigureDebugToggle();
  }

  protected override void OnForcedCleanUp()
  {
    AllDiagnosticsScreen.Instance = (AllDiagnosticsScreen) null;
    base.OnForcedCleanUp();
  }

  private void ConfigureDebugToggle()
  {
    Game.Instance.Subscribe(1557339983, new Action<object>(this.DebugToggleRefresh));
    MultiToggle toggle = this.debugNotificationToggleCotainer.GetComponentInChildren<MultiToggle>();
    toggle.onClick += (System.Action) (() =>
    {
      DebugHandler.ToggleDisableNotifications();
      toggle.ChangeState(DebugHandler.NotificationsDisabled ? 1 : 0);
    });
    this.DebugToggleRefresh();
    toggle.ChangeState(DebugHandler.NotificationsDisabled ? 1 : 0);
  }

  private void DebugToggleRefresh(object data = null)
  {
    this.debugNotificationToggleCotainer.gameObject.SetActive(DebugHandler.InstantBuildMode);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.ConsumeMouseScroll = true;
    this.Populate();
    Game.Instance.Subscribe(1983128072, new Action<object>(this.Populate));
    Game.Instance.Subscribe(-1280433810, new Action<object>(this.Populate));
    this.closeButton.onClick += (System.Action) (() => this.Show(false));
    this.clearSearchButton.onClick += (System.Action) (() => this.searchInputField.text = "");
    this.searchInputField.onValueChanged.AddListener((UnityAction<string>) (value => this.SearchFilter(value)));
    KInputTextField searchInputField = this.searchInputField;
    searchInputField.onFocus = searchInputField.onFocus + (System.Action) (() =>
    {
      this.isEditing = true;
      UISounds.PlaySound(UISounds.Sound.Find);
    });
    this.searchInputField.onEndEdit.AddListener((UnityAction<string>) (value => this.isEditing = false));
    this.Show(false);
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (!show)
      return;
    ManagementMenu.Instance.CloseAll();
    AllResourcesScreen.Instance.Show(false);
    this.RefreshSubrows();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape))
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close"));
      this.Show(false);
      e.Consumed = true;
    }
    if (this.isEditing)
      e.Consumed = true;
    else
      base.OnKeyDown(e);
  }

  public int GetRowCount() => this.diagnosticRows.Count;

  public override void OnKeyUp(KButtonEvent e)
  {
    if (PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight))
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close"));
      this.Show(false);
      e.Consumed = true;
    }
    if (e.Consumed)
      return;
    base.OnKeyUp(e);
  }

  public override float GetSortKey() => 50f;

  public void Populate(object _ = null)
  {
    this.SpawnRows();
    foreach (string key in this.diagnosticRows.Keys)
      this.currentlyDisplayedRows[(Tag) key] = true;
    this.SearchFilter(this.searchInputField.text);
    this.RefreshRows();
  }

  private void SpawnRows()
  {
    foreach (KeyValuePair<int, Dictionary<string, ColonyDiagnosticUtility.DisplaySetting>> diagnosticDisplaySetting in ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings)
    {
      foreach (KeyValuePair<string, ColonyDiagnosticUtility.DisplaySetting> keyValuePair in ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[diagnosticDisplaySetting.Key])
      {
        if (!this.diagnosticRows.ContainsKey(keyValuePair.Key))
        {
          ColonyDiagnostic diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic(keyValuePair.Key, diagnosticDisplaySetting.Key);
          switch (diagnostic)
          {
            case WorkTimeDiagnostic _:
            case ChoreGroupDiagnostic _:
              continue;
            default:
              this.SpawnRow(diagnostic, this.rootListContainer);
              continue;
          }
        }
      }
    }
    List<string> stringList = new List<string>();
    foreach (KeyValuePair<string, GameObject> diagnosticRow in this.diagnosticRows)
      stringList.Add(diagnosticRow.Key);
    stringList.Sort((Comparison<string>) ((a, b) => STRINGS.UI.StripLinkFormatting(ColonyDiagnosticUtility.Instance.GetDiagnosticName(a)).CompareTo(STRINGS.UI.StripLinkFormatting(ColonyDiagnosticUtility.Instance.GetDiagnosticName(b)))));
    foreach (string key in stringList)
      this.diagnosticRows[key].transform.SetAsLastSibling();
  }

  private void SpawnRow(ColonyDiagnostic diagnostic, GameObject container)
  {
    if (diagnostic == null || this.diagnosticRows.ContainsKey(diagnostic.id))
      return;
    GameObject gameObject1 = Util.KInstantiateUI(this.diagnosticLinePrefab, container, true);
    HierarchyReferences component1 = gameObject1.GetComponent<HierarchyReferences>();
    component1.GetReference<LocText>("NameLabel").SetText(diagnostic.name);
    string id1 = diagnostic.id;
    MultiToggle reference1 = component1.GetReference<MultiToggle>("PinToggle");
    string id = diagnostic.id;
    reference1.onClick += (System.Action) (() =>
    {
      if (ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(diagnostic.id))
      {
        ColonyDiagnosticUtility.Instance.ClearDiagnosticTutorialSetting(diagnostic.id);
      }
      else
      {
        int activeWorldId = ClusterManager.Instance.activeWorldId;
        int num = (int) (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[activeWorldId][id] - 1);
        if (num < 0)
          num = 2;
        ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[activeWorldId][id] = (ColonyDiagnosticUtility.DisplaySetting) num;
      }
      this.RefreshRows();
      ColonyDiagnosticScreen.Instance.RefreshAll();
    });
    GraphBase component2 = component1.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>();
    component2.axis_x.min_value = 0.0f;
    component2.axis_x.max_value = 600f;
    component2.axis_x.guide_frequency = 120f;
    component2.RefreshGuides();
    this.diagnosticRows.Add(id1, gameObject1);
    this.criteriaRows.Add(id1, new Dictionary<string, GameObject>());
    this.currentlyDisplayedRows.Add((Tag) id1, true);
    component1.GetReference<Image>("Icon").sprite = Assets.GetSprite((HashedString) diagnostic.icon);
    this.RefreshPinnedState(id1);
    RectTransform reference2 = component1.GetReference<RectTransform>("SubRows");
    foreach (DiagnosticCriterion criterion in diagnostic.GetCriteria())
    {
      DiagnosticCriterion sub = criterion;
      GameObject gameObject2 = Util.KInstantiateUI(this.subDiagnosticLinePrefab, reference2.gameObject, true);
      gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.DIAGNOSTICS_SCREEN.CRITERIA_TOOLTIP, (object) diagnostic.name, (object) sub.name));
      HierarchyReferences component3 = gameObject2.GetComponent<HierarchyReferences>();
      component3.GetReference<LocText>("Label").SetText(sub.name);
      this.criteriaRows[diagnostic.id].Add(sub.id, gameObject2);
      component3.GetReference<MultiToggle>("PinToggle").onClick += (System.Action) (() =>
      {
        int activeWorldId = ClusterManager.Instance.activeWorldId;
        bool flag = ColonyDiagnosticUtility.Instance.IsCriteriaEnabled(activeWorldId, diagnostic.id, sub.id);
        ColonyDiagnosticUtility.Instance.SetCriteriaEnabled(activeWorldId, diagnostic.id, sub.id, !flag);
        this.RefreshSubrows();
      });
    }
    this.subrowContainerOpen.Add((Tag) diagnostic.id, false);
    MultiToggle reference3 = component1.GetReference<MultiToggle>("SubrowToggle");
    reference3.onClick += (System.Action) (() =>
    {
      this.subrowContainerOpen[(Tag) diagnostic.id] = !this.subrowContainerOpen[(Tag) diagnostic.id];
      this.RefreshSubrows();
    });
    component1.GetReference<MultiToggle>("MainToggle").onClick = reference3.onClick;
  }

  private void FilterRowBySearch(Tag tag, string filter)
  {
    this.currentlyDisplayedRows[tag] = this.PassesSearchFilter(tag, filter);
  }

  private void SearchFilter(string search)
  {
    foreach (KeyValuePair<string, GameObject> diagnosticRow in this.diagnosticRows)
      this.FilterRowBySearch((Tag) diagnosticRow.Key, search);
    foreach (KeyValuePair<string, GameObject> diagnosticRow in this.diagnosticRows)
      this.currentlyDisplayedRows[(Tag) diagnosticRow.Key] = this.PassesSearchFilter((Tag) diagnosticRow.Key, search);
    this.SetRowsActive();
  }

  private bool PassesSearchFilter(Tag tag, string filter)
  {
    if (string.IsNullOrEmpty(filter))
      return true;
    filter = filter.ToUpper();
    string id = tag.ToString();
    if (ColonyDiagnosticUtility.Instance.GetDiagnosticName(id).ToUpper().Contains(filter) || tag.Name.ToUpper().Contains(filter))
      return true;
    ColonyDiagnostic diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic(id, ClusterManager.Instance.activeWorldId);
    if (diagnostic == null)
      return false;
    DiagnosticCriterion[] criteria = diagnostic.GetCriteria();
    if (criteria == null)
      return false;
    foreach (DiagnosticCriterion diagnosticCriterion in criteria)
    {
      if (diagnosticCriterion.name.ToUpper().Contains(filter))
        return true;
    }
    return false;
  }

  private void RefreshPinnedState(string diagnosticID)
  {
    if (!ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[ClusterManager.Instance.activeWorldId].ContainsKey(diagnosticID))
      return;
    MultiToggle reference = this.diagnosticRows[diagnosticID].GetComponent<HierarchyReferences>().GetReference<MultiToggle>("PinToggle");
    if (ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(diagnosticID))
    {
      reference.ChangeState(3);
    }
    else
    {
      switch (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[ClusterManager.Instance.activeWorldId][diagnosticID])
      {
        case ColonyDiagnosticUtility.DisplaySetting.Always:
          reference.ChangeState(2);
          break;
        case ColonyDiagnosticUtility.DisplaySetting.AlertOnly:
          reference.ChangeState(1);
          break;
        case ColonyDiagnosticUtility.DisplaySetting.Never:
          reference.ChangeState(0);
          break;
      }
    }
    string message = "";
    if (ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(diagnosticID))
    {
      message = (string) STRINGS.UI.DIAGNOSTICS_SCREEN.CLICK_TOGGLE_MESSAGE.TUTORIAL_DISABLED;
    }
    else
    {
      switch (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[ClusterManager.Instance.activeWorldId][diagnosticID])
      {
        case ColonyDiagnosticUtility.DisplaySetting.Always:
          message = (string) STRINGS.UI.DIAGNOSTICS_SCREEN.CLICK_TOGGLE_MESSAGE.NEVER;
          break;
        case ColonyDiagnosticUtility.DisplaySetting.AlertOnly:
          message = (string) STRINGS.UI.DIAGNOSTICS_SCREEN.CLICK_TOGGLE_MESSAGE.ALWAYS;
          break;
        case ColonyDiagnosticUtility.DisplaySetting.Never:
          message = (string) STRINGS.UI.DIAGNOSTICS_SCREEN.CLICK_TOGGLE_MESSAGE.ALERT_ONLY;
          break;
      }
    }
    reference.GetComponent<ToolTip>().SetSimpleTooltip(message);
  }

  public void RefreshRows()
  {
    WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
    if (this.allowRefresh)
    {
      foreach (KeyValuePair<string, GameObject> diagnosticRow in this.diagnosticRows)
      {
        HierarchyReferences component = diagnosticRow.Value.GetComponent<HierarchyReferences>();
        component.GetReference<LocText>("AvailableLabel").SetText(diagnosticRow.Key);
        component.GetReference<RectTransform>("SubRows").gameObject.SetActive(false);
        ColonyDiagnostic diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic(diagnosticRow.Key, ClusterManager.Instance.activeWorldId);
        if (diagnostic != null)
        {
          component.GetReference<LocText>("AvailableLabel").SetText(diagnostic.GetAverageValueString());
          component.GetReference<Image>("Indicator").color = diagnostic.colors[diagnostic.LatestResult.opinion];
          ToolTip reference = component.GetReference<ToolTip>("Tooltip");
          reference.refreshWhileHovering = true;
          reference.SetSimpleTooltip($"{(string) Strings.Get(new StringKey($"STRINGS.UI.COLONY_DIAGNOSTICS.{diagnostic.id.ToUpper()}.TOOLTIP_NAME"))}\n{diagnostic.LatestResult.GetFormattedMessage()}");
        }
        this.RefreshPinnedState(diagnosticRow.Key);
      }
    }
    this.SetRowsActive();
    this.RefreshSubrows();
  }

  private void RefreshSubrows()
  {
    foreach (KeyValuePair<string, GameObject> diagnosticRow in this.diagnosticRows)
    {
      HierarchyReferences component = diagnosticRow.Value.GetComponent<HierarchyReferences>();
      component.GetReference<MultiToggle>("SubrowToggle").ChangeState(!this.subrowContainerOpen[(Tag) diagnosticRow.Key] ? 0 : 1);
      component.GetReference<RectTransform>("SubRows").gameObject.SetActive(this.subrowContainerOpen[(Tag) diagnosticRow.Key]);
      int num = 0;
      foreach (KeyValuePair<string, GameObject> keyValuePair in this.criteriaRows[diagnosticRow.Key])
      {
        MultiToggle reference = keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("PinToggle");
        int activeWorldId = ClusterManager.Instance.activeWorldId;
        string key1 = diagnosticRow.Key;
        string key2 = keyValuePair.Key;
        bool flag = ColonyDiagnosticUtility.Instance.IsCriteriaEnabled(activeWorldId, key1, key2);
        int new_state_index = flag ? 1 : 0;
        reference.ChangeState(new_state_index);
        if (flag)
          ++num;
      }
      component.GetReference<LocText>("SubrowHeaderLabel").SetText(string.Format((string) STRINGS.UI.DIAGNOSTICS_SCREEN.CRITERIA_ENABLED_COUNT, (object) num, (object) this.criteriaRows[diagnosticRow.Key].Count));
    }
  }

  private void RefreshCharts()
  {
    foreach (KeyValuePair<string, GameObject> diagnosticRow in this.diagnosticRows)
    {
      HierarchyReferences component = diagnosticRow.Value.GetComponent<HierarchyReferences>();
      ColonyDiagnostic diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic(diagnosticRow.Key, ClusterManager.Instance.activeWorldId);
      if (diagnostic != null)
      {
        SparkLayer reference = component.GetReference<SparkLayer>("Chart");
        Tracker tracker = diagnostic.tracker;
        if (tracker != null)
        {
          float periodLength = 3000f;
          Tuple<float, float>[] data = tracker.ChartableData(periodLength);
          reference.graph.axis_x.max_value = data[data.Length - 1].first;
          reference.graph.axis_x.min_value = reference.graph.axis_x.max_value - periodLength;
          reference.RefreshLine(data, "resourceAmount");
        }
      }
    }
  }

  private void SetRowsActive()
  {
    foreach (KeyValuePair<string, GameObject> diagnosticRow in this.diagnosticRows)
    {
      if (ColonyDiagnosticUtility.Instance.GetDiagnostic(diagnosticRow.Key, ClusterManager.Instance.activeWorldId) == null)
        this.currentlyDisplayedRows[(Tag) diagnosticRow.Key] = false;
    }
    foreach (KeyValuePair<string, GameObject> diagnosticRow in this.diagnosticRows)
    {
      if (diagnosticRow.Value.activeSelf != this.currentlyDisplayedRows[(Tag) diagnosticRow.Key])
        diagnosticRow.Value.SetActive(this.currentlyDisplayedRows[(Tag) diagnosticRow.Key]);
    }
  }

  public void Sim4000ms(float dt)
  {
    if (!this.IsScreenActive())
      return;
    this.RefreshCharts();
  }

  public void Sim1000ms(float dt)
  {
    if (!this.IsScreenActive())
      return;
    this.RefreshRows();
  }
}
