// Decompiled with JetBrains decompiler
// Type: TableScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class TableScreen : ShowOptimizedKScreen
{
  protected string title;
  protected bool has_default_duplicant_row = true;
  protected bool useWorldDividers = true;
  private bool rows_dirty;
  protected Comparison<IAssignableIdentity> active_sort_method;
  protected TableColumn active_sort_column;
  protected bool sort_is_reversed;
  private int active_cascade_coroutine_count;
  private HandleVector<int>.Handle current_looping_sound = HandleVector<int>.InvalidHandle;
  private bool incubating;
  private int removeWorldHandle = -1;
  protected Dictionary<string, TableColumn> columns = new Dictionary<string, TableColumn>();
  public List<TableRow> rows = new List<TableRow>();
  public List<TableRow> all_sortable_rows = new List<TableRow>();
  public List<string> column_scrollers = new List<string>();
  private Dictionary<GameObject, TableRow> known_widget_rows = new Dictionary<GameObject, TableRow>();
  private Dictionary<GameObject, TableColumn> known_widget_columns = new Dictionary<GameObject, TableColumn>();
  public GameObject prefab_row_empty;
  public GameObject prefab_row_header;
  public GameObject prefab_world_divider;
  public GameObject prefab_scroller_border;
  private string cascade_sound_path = GlobalAssets.GetSound("Placers_Unfurl_LP");
  public KButton CloseButton;
  [MyCmpGet]
  private VerticalLayoutGroup VLG;
  protected GameObject header_row;
  protected GameObject default_row;
  public LocText title_bar;
  public Transform header_content_transform;
  public Transform scroll_content_transform;
  public Transform scroller_borders_transform;
  public Dictionary<int, GameObject> worldDividers = new Dictionary<int, GameObject>();
  private bool scrollersDirty;
  private float targetScrollerPosition;
  private Dictionary<IAssignableIdentity, bool> obsoleteMinionRowStatus = new Dictionary<IAssignableIdentity, bool>();
  private Dictionary<int, bool> obsoleteWorldDividerStatus = new Dictionary<int, bool>();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.removeWorldHandle = ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.RemoveWorldDivider));
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    this.title_bar.text = this.title;
    this.ConsumeMouseScroll = true;
    this.CloseButton.onClick += (System.Action) (() => ManagementMenu.Instance.CloseAll());
    this.incubating = true;
    this.transform.rectTransform().localScale = Vector3.zero;
    Components.LiveMinionIdentities.OnAdd += (Action<MinionIdentity>) (param => this.MarkRowsDirty());
    Components.LiveMinionIdentities.OnRemove += (Action<MinionIdentity>) (param => this.MarkRowsDirty());
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (this.removeWorldHandle == -1)
      return;
    ClusterManager.Instance.Unsubscribe(this.removeWorldHandle);
  }

  protected override void OnShow(bool show)
  {
    if (!show)
    {
      this.active_cascade_coroutine_count = 0;
      this.StopAllCoroutines();
      this.StopLoopingCascadeSound();
    }
    this.ZeroScrollers();
    base.OnShow(show);
    if (!show)
      return;
    this.MarkRowsDirty();
  }

  private void ZeroScrollers()
  {
    if (this.rows.Count <= 0)
      return;
    foreach (string columnScroller in this.column_scrollers)
    {
      foreach (TableRow row in this.rows)
      {
        ScrollRect component = row.GetScroller(columnScroller).transform.parent.GetComponent<ScrollRect>();
        if ((double) component.horizontalNormalizedPosition > 1.0 / 1000.0)
          component.horizontalNormalizedPosition = 0.0f;
      }
    }
    foreach (KeyValuePair<int, GameObject> worldDivider in this.worldDividers)
    {
      ScrollRect reference = worldDivider.Value.GetComponent<HierarchyReferences>().GetReference<ScrollRect>("ScrollerScrollRect");
      if ((UnityEngine.Object) reference != (UnityEngine.Object) null && (double) reference.horizontalNormalizedPosition > 1.0 / 1000.0)
        reference.horizontalNormalizedPosition = 0.0f;
    }
  }

  public bool CheckScrollersDirty() => this.scrollersDirty;

  public void SetScrollersDirty(float position)
  {
    this.targetScrollerPosition = position;
    this.scrollersDirty = true;
    this.PositionScrollers();
  }

  public void PositionScrollers()
  {
    foreach (TableRow row in this.rows)
    {
      ScrollRect scrollRect = row.scroll_rect;
      if ((UnityEngine.Object) scrollRect != (UnityEngine.Object) null)
        scrollRect.horizontalNormalizedPosition = this.targetScrollerPosition;
    }
    foreach (KeyValuePair<int, GameObject> worldDivider in this.worldDividers)
    {
      if (worldDivider.Value.activeInHierarchy)
      {
        ScrollRect reference = worldDivider.Value.GetComponent<HierarchyReferences>().GetReference<ScrollRect>("ScrollerScrollRect");
        if ((UnityEngine.Object) reference != (UnityEngine.Object) null)
          reference.horizontalNormalizedPosition = this.targetScrollerPosition;
      }
    }
    this.scrollersDirty = false;
  }

  public override void ScreenUpdate(bool topLevel)
  {
    base.ScreenUpdate(topLevel);
    if (this.incubating)
    {
      this.ZeroScrollers();
      this.transform.rectTransform().localScale = Vector3.one;
      this.incubating = false;
    }
    if (this.rows_dirty)
      this.RefreshRows();
    foreach (TableRow row in this.rows)
      row.RefreshScrollers();
    foreach (TableColumn tableColumn in this.columns.Values)
    {
      if (tableColumn.isDirty)
      {
        foreach (KeyValuePair<TableRow, GameObject> keyValuePair in tableColumn.widgets_by_row)
        {
          tableColumn.on_load_action(keyValuePair.Key.GetIdentity(), keyValuePair.Value);
          tableColumn.MarkClean();
        }
      }
    }
  }

  protected void MarkRowsDirty() => this.rows_dirty = true;

  protected virtual void RefreshRows()
  {
    this.ObsoleteRows();
    this.AddRow((IAssignableIdentity) null);
    if (this.has_default_duplicant_row)
      this.AddDefaultRow();
    for (int idx = 0; idx < Components.LiveMinionIdentities.Count; ++idx)
    {
      if ((UnityEngine.Object) Components.LiveMinionIdentities[idx] != (UnityEngine.Object) null)
        this.AddRow((IAssignableIdentity) Components.LiveMinionIdentities[idx]);
    }
    foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
    {
      foreach (MinionStorage.Info info in minionStorage.GetStoredMinionInfo())
      {
        if (info.serializedMinion != null)
          this.AddRow((IAssignableIdentity) info.serializedMinion.Get<StoredMinionIdentity>());
      }
    }
    foreach (int worldId in ClusterManager.Instance.GetWorldIDsSorted())
      this.AddWorldDivider(worldId);
    this.AddWorldDivider((int) byte.MaxValue);
    foreach (KeyValuePair<int, bool> worldDividerStatu in this.obsoleteWorldDividerStatus)
    {
      if (worldDividerStatu.Value)
      {
        Boxed<int> worldId = Boxed<int>.Get(worldDividerStatu.Key);
        this.RemoveWorldDivider((object) worldId);
        worldId.Release();
      }
    }
    this.obsoleteWorldDividerStatus.Clear();
    foreach (KeyValuePair<int, GameObject> worldDivider in this.worldDividers)
    {
      Component reference = worldDivider.Value.GetComponent<HierarchyReferences>().GetReference("NobodyRow");
      bool flag1 = true;
      foreach (MinionAssignablesProxy assignablesProxy in Components.MinionAssignablesProxy)
      {
        if ((UnityEngine.Object) assignablesProxy != (UnityEngine.Object) null && (UnityEngine.Object) assignablesProxy.GetTargetGameObject() != (UnityEngine.Object) null)
        {
          WorldContainer myWorld = assignablesProxy.GetTargetGameObject().GetMyWorld();
          if ((UnityEngine.Object) myWorld != (UnityEngine.Object) null && myWorld.id == worldDivider.Key)
          {
            flag1 = false;
            break;
          }
          if ((UnityEngine.Object) myWorld == (UnityEngine.Object) null && worldDivider.Key == (int) byte.MaxValue)
          {
            flag1 = false;
            break;
          }
        }
      }
      reference.gameObject.SetActive(flag1);
      WorldContainer world = ClusterManager.Instance.GetWorld(worldDivider.Key);
      bool flag2 = DlcManager.FeatureClusterSpaceEnabled() && ((UnityEngine.Object) world == (UnityEngine.Object) null || ClusterManager.Instance.GetWorld(worldDivider.Key).IsDiscovered);
      if ((UnityEngine.Object) world == (UnityEngine.Object) null && flag1)
        flag2 = false;
      if (worldDivider.Value.activeSelf != flag2)
        worldDivider.Value.SetActive(flag2);
    }
    foreach (KeyValuePair<IAssignableIdentity, bool> obsoleteMinionRowStatu in this.obsoleteMinionRowStatus)
    {
      KeyValuePair<IAssignableIdentity, bool> kvp = obsoleteMinionRowStatu;
      if (kvp.Value)
      {
        int index = this.rows.FindIndex((Predicate<TableRow>) (match => match.GetIdentity() == kvp.Key));
        TableRow row = this.rows[index];
        this.rows[index].Clear();
        this.rows.RemoveAt(index);
        this.all_sortable_rows.Remove(row);
      }
    }
    this.obsoleteMinionRowStatus.Clear();
    this.SortRows();
    this.rows_dirty = false;
    LayoutRebuilder.ForceRebuildLayoutImmediate(this.gameObject.rectTransform());
    foreach (TableRow row in this.rows)
      row.PositionScrollerBorders();
  }

  public virtual void SetSortComparison(
    Comparison<IAssignableIdentity> comparison,
    TableColumn sort_column)
  {
    if (comparison == null)
      return;
    if (this.active_sort_column == sort_column)
    {
      if (this.sort_is_reversed)
      {
        this.sort_is_reversed = false;
        this.active_sort_method = (Comparison<IAssignableIdentity>) null;
        this.active_sort_column = (TableColumn) null;
      }
      else
        this.sort_is_reversed = true;
    }
    else
    {
      this.active_sort_column = sort_column;
      this.active_sort_method = comparison;
      this.sort_is_reversed = false;
    }
  }

  public void SortRows()
  {
    foreach (TableColumn tableColumn in this.columns.Values)
    {
      if (!((UnityEngine.Object) tableColumn.column_sort_toggle == (UnityEngine.Object) null))
      {
        if (tableColumn == this.active_sort_column)
        {
          if (this.sort_is_reversed)
            tableColumn.column_sort_toggle.ChangeState(2);
          else
            tableColumn.column_sort_toggle.ChangeState(1);
        }
        else
          tableColumn.column_sort_toggle.ChangeState(0);
      }
    }
    Dictionary<IAssignableIdentity, TableRow> dictionary1 = new Dictionary<IAssignableIdentity, TableRow>();
    foreach (TableRow allSortableRow in this.all_sortable_rows)
      dictionary1.Add(allSortableRow.GetIdentity(), allSortableRow);
    Dictionary<int, List<IAssignableIdentity>> dictionary2 = new Dictionary<int, List<IAssignableIdentity>>();
    foreach (KeyValuePair<IAssignableIdentity, TableRow> keyValuePair in dictionary1)
    {
      WorldContainer myWorld = keyValuePair.Key.GetSoleOwner().GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<KMonoBehaviour>().GetMyWorld();
      int key = (int) byte.MaxValue;
      if ((UnityEngine.Object) myWorld != (UnityEngine.Object) null)
        key = myWorld.id;
      if (!dictionary2.ContainsKey(key))
        dictionary2.Add(key, new List<IAssignableIdentity>());
      dictionary2[key].Add(keyValuePair.Key);
    }
    this.all_sortable_rows.Clear();
    Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
    int num1 = 0;
    int num2 = 0;
    foreach (KeyValuePair<int, List<IAssignableIdentity>> keyValuePair in dictionary2)
    {
      dictionary3.Add(keyValuePair.Key, num1);
      int num3 = num1 + 1;
      List<IAssignableIdentity> assignableIdentityList = new List<IAssignableIdentity>();
      foreach (IAssignableIdentity assignableIdentity in keyValuePair.Value)
        assignableIdentityList.Add(assignableIdentity);
      if (this.active_sort_method != null)
      {
        assignableIdentityList.Sort(this.active_sort_method);
        if (this.sort_is_reversed)
          assignableIdentityList.Reverse();
      }
      num1 = num3 + assignableIdentityList.Count;
      num2 += assignableIdentityList.Count;
      for (int index = 0; index < assignableIdentityList.Count; ++index)
        this.all_sortable_rows.Add(dictionary1[assignableIdentityList[index]]);
    }
    for (int index = 0; index < this.all_sortable_rows.Count; ++index)
      this.all_sortable_rows[index].gameObject.transform.SetSiblingIndex(index);
    foreach (KeyValuePair<int, int> keyValuePair in dictionary3)
      this.worldDividers[keyValuePair.Key].transform.SetSiblingIndex(keyValuePair.Value);
    if (!this.has_default_duplicant_row)
      return;
    this.default_row.transform.SetAsFirstSibling();
  }

  protected int compare_rows_alphabetical(IAssignableIdentity a, IAssignableIdentity b)
  {
    if (a == null && b == null)
      return 0;
    if (a == null)
      return -1;
    return b == null ? 1 : a.GetProperName().CompareTo(b.GetProperName());
  }

  protected int default_sort(TableRow a, TableRow b) => 0;

  protected void ObsoleteRows()
  {
    for (int index = this.rows.Count - 1; index >= 0; --index)
    {
      IAssignableIdentity identity = this.rows[index].GetIdentity();
      if (identity != null)
        this.obsoleteMinionRowStatus.Add(identity, true);
    }
    foreach (KeyValuePair<int, GameObject> worldDivider in this.worldDividers)
      this.obsoleteWorldDividerStatus.Add(worldDivider.Key, true);
  }

  protected void AddRow(IAssignableIdentity minion)
  {
    bool flag = minion == null;
    if (!flag && this.obsoleteMinionRowStatus.ContainsKey(minion))
    {
      this.obsoleteMinionRowStatus[minion] = false;
      this.rows.Find((Predicate<TableRow>) (match => match.GetIdentity() == minion)).RefreshColumns(this.columns);
    }
    else if (flag && (UnityEngine.Object) this.header_row != (UnityEngine.Object) null)
    {
      this.header_row.GetComponent<TableRow>().RefreshColumns(this.columns);
    }
    else
    {
      GameObject gameObject = Util.KInstantiateUI(flag ? this.prefab_row_header : this.prefab_row_empty, minion == null ? this.header_content_transform.gameObject : this.scroll_content_transform.gameObject, true);
      TableRow component = gameObject.GetComponent<TableRow>();
      component.rowType = flag ? TableRow.RowType.Header : ((UnityEngine.Object) (minion as MinionIdentity) != (UnityEngine.Object) null ? TableRow.RowType.Minion : TableRow.RowType.StoredMinon);
      this.rows.Add(component);
      component.ConfigureContent(minion, this.columns, this);
      if (!flag)
        this.all_sortable_rows.Add(component);
      else
        this.header_row = gameObject;
    }
  }

  protected void AddDefaultRow()
  {
    if ((UnityEngine.Object) this.default_row != (UnityEngine.Object) null)
    {
      this.default_row.GetComponent<TableRow>().RefreshColumns(this.columns);
    }
    else
    {
      GameObject gameObject = Util.KInstantiateUI(this.prefab_row_empty, this.scroll_content_transform.gameObject, true);
      this.default_row = gameObject;
      TableRow component = gameObject.GetComponent<TableRow>();
      component.rowType = TableRow.RowType.Default;
      component.isDefault = true;
      this.rows.Add(component);
      component.ConfigureContent((IAssignableIdentity) null, this.columns, this);
    }
  }

  protected void AddWorldDivider(int worldId)
  {
    if (this.obsoleteWorldDividerStatus.ContainsKey(worldId) && this.obsoleteWorldDividerStatus[worldId])
    {
      this.obsoleteWorldDividerStatus[worldId] = false;
    }
    else
    {
      GameObject gameObject = Util.KInstantiateUI(this.prefab_world_divider, this.scroll_content_transform.gameObject, true);
      HierarchyReferences component1 = gameObject.GetComponent<HierarchyReferences>();
      component1.GetReference<Image>("RowBG").color = ClusterManager.worldColors[worldId % ClusterManager.worldColors.Length];
      LocText reference = component1.GetReference<LocText>("NameLabel");
      RectTransform rectTransform = reference.rectTransform();
      rectTransform.sizeDelta = new Vector2(150f, rectTransform.sizeDelta.y);
      WorldContainer world = ClusterManager.Instance.GetWorld(worldId);
      if ((UnityEngine.Object) world != (UnityEngine.Object) null)
      {
        ClusterGridEntity component2 = world.GetComponent<ClusterGridEntity>();
        string str = (string) (component2 is Clustercraft ? NAMEGEN.WORLD.SPACECRAFT_PREFIX : NAMEGEN.WORLD.PLANETOID_PREFIX);
        reference.SetText(str + component2.Name);
        reference.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) NAMEGEN.WORLD.WORLDDIVIDER_TOOLTIP, (object) component2.Name));
        gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = component2.GetUISprite();
      }
      else
      {
        reference.SetText((string) NAMEGEN.WORLD.UNKNOWN_WORLD);
        reference.GetComponent<ToolTip>().SetSimpleTooltip("");
        gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Assets.GetSprite((HashedString) "hex_unknown");
      }
      this.worldDividers.Add(worldId, gameObject);
      gameObject.GetComponent<TableRow>().ConfigureAsWorldDivider(this.columns, this);
    }
  }

  protected void RemoveWorldDivider(object worldId)
  {
    int key = ((Boxed<int>) worldId).value;
    if (!this.worldDividers.ContainsKey(key))
      return;
    this.rows.Remove(this.worldDividers[key].GetComponent<TableRow>());
    Util.KDestroyGameObject(this.worldDividers[key]);
    this.worldDividers.Remove(key);
  }

  protected TableRow GetWidgetRow(GameObject widget_go)
  {
    if ((UnityEngine.Object) widget_go == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "Widget is null");
      return (TableRow) null;
    }
    if (this.known_widget_rows.ContainsKey(widget_go))
      return this.known_widget_rows[widget_go];
    foreach (TableRow row in this.rows)
    {
      if (row.rowType != TableRow.RowType.WorldDivider && row.ContainsWidget(widget_go))
      {
        this.known_widget_rows.Add(widget_go, row);
        return row;
      }
    }
    Debug.LogWarning((object) $"Row is null for widget: {widget_go.name} parent is {widget_go.transform.parent.name}");
    return (TableRow) null;
  }

  protected void StartScrollableContent(string scrollablePanelID)
  {
    if (this.column_scrollers.Contains(scrollablePanelID))
      return;
    DividerColumn new_column = new DividerColumn((Func<bool>) (() => true));
    this.RegisterColumn("scroller_spacer_" + scrollablePanelID, (TableColumn) new_column);
    this.column_scrollers.Add(scrollablePanelID);
  }

  protected PortraitTableColumn AddPortraitColumn(
    string id,
    Action<IAssignableIdentity, GameObject> on_load_action,
    Comparison<IAssignableIdentity> sort_comparison,
    bool double_click_to_target = true)
  {
    PortraitTableColumn new_column = new PortraitTableColumn(on_load_action, sort_comparison, double_click_to_target);
    return this.RegisterColumn(id, (TableColumn) new_column) ? new_column : (PortraitTableColumn) null;
  }

  protected ButtonLabelColumn AddButtonLabelColumn(
    string id,
    Action<IAssignableIdentity, GameObject> on_load_action,
    Func<IAssignableIdentity, GameObject, string> get_value_action,
    Action<GameObject> on_click_action,
    Action<GameObject> on_double_click_action,
    Comparison<IAssignableIdentity> sort_comparison,
    Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip,
    Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip,
    bool whiteText = false)
  {
    ButtonLabelColumn new_column = new ButtonLabelColumn(on_load_action, get_value_action, on_click_action, on_double_click_action, sort_comparison, on_tooltip, on_sort_tooltip, whiteText);
    return this.RegisterColumn(id, (TableColumn) new_column) ? new_column : (ButtonLabelColumn) null;
  }

  protected LabelTableColumn AddLabelColumn(
    string id,
    Action<IAssignableIdentity, GameObject> on_load_action,
    Func<IAssignableIdentity, GameObject, string> get_value_action,
    Comparison<IAssignableIdentity> sort_comparison,
    Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip,
    Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip,
    int widget_width = 128 /*0x80*/,
    bool should_refresh_columns = false)
  {
    LabelTableColumn new_column = new LabelTableColumn(on_load_action, get_value_action, sort_comparison, on_tooltip, on_sort_tooltip, widget_width, should_refresh_columns);
    return this.RegisterColumn(id, (TableColumn) new_column) ? new_column : (LabelTableColumn) null;
  }

  protected CheckboxTableColumn AddCheckboxColumn(
    string id,
    Action<IAssignableIdentity, GameObject> on_load_action,
    Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action,
    Action<GameObject> on_press_action,
    Action<GameObject, TableScreen.ResultValues> set_value_function,
    Comparison<IAssignableIdentity> sort_comparison,
    Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip,
    Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip)
  {
    CheckboxTableColumn new_column = new CheckboxTableColumn(on_load_action, get_value_action, on_press_action, set_value_function, sort_comparison, on_tooltip, on_sort_tooltip);
    return this.RegisterColumn(id, (TableColumn) new_column) ? new_column : (CheckboxTableColumn) null;
  }

  protected SuperCheckboxTableColumn AddSuperCheckboxColumn(
    string id,
    CheckboxTableColumn[] columns_affected,
    Action<IAssignableIdentity, GameObject> on_load_action,
    Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action,
    Action<GameObject> on_press_action,
    Action<GameObject, TableScreen.ResultValues> set_value_action,
    Comparison<IAssignableIdentity> sort_comparison,
    Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip)
  {
    SuperCheckboxTableColumn new_column = new SuperCheckboxTableColumn(columns_affected, on_load_action, get_value_action, on_press_action, set_value_action, sort_comparison, on_tooltip);
    if (this.RegisterColumn(id, (TableColumn) new_column))
    {
      foreach (CheckboxTableColumn checkboxTableColumn in columns_affected)
        checkboxTableColumn.on_set_action += new Action<GameObject, TableScreen.ResultValues>(((TableColumn) new_column).MarkDirty);
      new_column.MarkDirty();
      return new_column;
    }
    Debug.LogWarning((object) "SuperCheckbox column registration failed");
    return (SuperCheckboxTableColumn) null;
  }

  protected NumericDropDownTableColumn AddNumericDropDownColumn(
    string id,
    object user_data,
    List<TMP_Dropdown.OptionData> options,
    Action<IAssignableIdentity, GameObject> on_load_action,
    Action<GameObject, int> set_value_action,
    Comparison<IAssignableIdentity> sort_comparison,
    NumericDropDownTableColumn.ToolTipCallbacks tooltip_callbacks)
  {
    NumericDropDownTableColumn new_column = new NumericDropDownTableColumn(user_data, options, on_load_action, set_value_action, sort_comparison, tooltip_callbacks);
    return this.RegisterColumn(id, (TableColumn) new_column) ? new_column : (NumericDropDownTableColumn) null;
  }

  protected bool RegisterColumn(string id, TableColumn new_column)
  {
    if (this.columns.ContainsKey(id))
    {
      Debug.LogWarning((object) $"Column with id {id} already in dictionary");
      return false;
    }
    new_column.screen = this;
    this.columns.Add(id, new_column);
    this.MarkRowsDirty();
    return true;
  }

  protected TableColumn GetWidgetColumn(GameObject widget_go)
  {
    if (this.known_widget_columns.ContainsKey(widget_go))
      return this.known_widget_columns[widget_go];
    foreach (KeyValuePair<string, TableColumn> column in this.columns)
    {
      if (column.Value.ContainsWidget(widget_go))
      {
        this.known_widget_columns.Add(widget_go, column.Value);
        return column.Value;
      }
    }
    Debug.LogWarning((object) ("No column found for widget gameobject " + widget_go.name));
    return (TableColumn) null;
  }

  protected void on_load_portrait(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    CrewPortrait component = widget_go.GetComponent<CrewPortrait>();
    if (minion != null)
    {
      component.SetIdentityObject(minion, false);
      component.ForceRefresh();
    }
    else
      component.targetImage.enabled = widgetRow.rowType == TableRow.RowType.Default;
  }

  protected void on_load_name_label(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText locText = (LocText) null;
    HierarchyReferences component = widget_go.GetComponent<HierarchyReferences>();
    LocText reference = component.GetReference("Label") as LocText;
    if (component.HasReference("SubLabel"))
      locText = component.GetReference("SubLabel") as LocText;
    if (minion != null)
    {
      reference.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
      if (!((UnityEngine.Object) locText != (UnityEngine.Object) null))
        return;
      MinionIdentity minionIdentity = minion as MinionIdentity;
      if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
        locText.text = minionIdentity.gameObject.GetComponent<MinionResume>().GetSkillsSubtitle();
      else
        locText.text = "";
      locText.textWrappingMode = TextWrappingModes.Normal;
    }
    else
    {
      if (widgetRow.isDefault)
      {
        reference.text = (string) STRINGS.UI.JOBSCREEN_DEFAULT;
        if ((UnityEngine.Object) locText != (UnityEngine.Object) null && locText.gameObject.activeSelf)
          locText.gameObject.SetActive(false);
      }
      else
        reference.text = (string) STRINGS.UI.JOBSCREEN_EVERYONE;
      if (!((UnityEngine.Object) locText != (UnityEngine.Object) null))
        return;
      locText.text = "";
    }
  }

  protected string get_value_name_label(IAssignableIdentity minion, GameObject widget_go)
  {
    return minion.GetProperName();
  }

  protected void on_load_value_checkbox_column_super(
    IAssignableIdentity minion,
    GameObject widget_go)
  {
    MultiToggle component = widget_go.GetComponent<MultiToggle>();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
      case TableRow.RowType.Default:
      case TableRow.RowType.Minion:
        component.ChangeState((int) this.get_value_checkbox_column_super(minion, widget_go));
        break;
    }
  }

  public virtual TableScreen.ResultValues get_value_checkbox_column_super(
    IAssignableIdentity minion,
    GameObject widget_go)
  {
    SuperCheckboxTableColumn widgetColumn = this.GetWidgetColumn(widget_go) as SuperCheckboxTableColumn;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    bool flag1 = true;
    bool flag2 = true;
    bool flag3 = false;
    bool flag4 = false;
    bool flag5 = false;
    foreach (CheckboxTableColumn column in widgetColumn.columns_affected)
    {
      if (column.isRevealed)
      {
        switch (column.get_value_action(widgetRow.GetIdentity(), widgetRow.GetWidget((TableColumn) column)))
        {
          case TableScreen.ResultValues.False:
            flag2 = false;
            if (!flag1)
            {
              flag5 = true;
              break;
            }
            break;
          case TableScreen.ResultValues.Partial:
            flag4 = true;
            flag5 = true;
            break;
          case TableScreen.ResultValues.True:
            flag4 = true;
            flag1 = false;
            if (!flag2)
            {
              flag5 = true;
              break;
            }
            break;
          case TableScreen.ResultValues.ConditionalGroup:
            flag3 = true;
            flag2 = false;
            flag1 = false;
            break;
        }
        int num = flag5 ? 1 : 0;
      }
    }
    TableScreen.ResultValues checkboxColumnSuper = TableScreen.ResultValues.Partial;
    if (flag3 && !flag4 && !flag2 && !flag1)
      checkboxColumnSuper = TableScreen.ResultValues.ConditionalGroup;
    else if (flag2)
      checkboxColumnSuper = TableScreen.ResultValues.True;
    else if (flag1)
      checkboxColumnSuper = TableScreen.ResultValues.False;
    else if (flag4)
      checkboxColumnSuper = TableScreen.ResultValues.Partial;
    return checkboxColumnSuper;
  }

  protected void set_value_checkbox_column_super(
    GameObject widget_go,
    TableScreen.ResultValues new_value)
  {
    SuperCheckboxTableColumn widgetColumn = this.GetWidgetColumn(widget_go) as SuperCheckboxTableColumn;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    switch (widgetRow.rowType)
    {
      case TableRow.RowType.Header:
        this.StartCoroutine(this.CascadeSetRowCheckBoxes(widgetColumn.columns_affected, this.default_row.GetComponent<TableRow>(), new_value, widget_go));
        this.StartCoroutine(this.CascadeSetColumnCheckBoxes(this.all_sortable_rows, (CheckboxTableColumn) widgetColumn, new_value, widget_go));
        break;
      case TableRow.RowType.Default:
        this.StartCoroutine(this.CascadeSetRowCheckBoxes(widgetColumn.columns_affected, widgetRow, new_value, widget_go));
        break;
      case TableRow.RowType.Minion:
        this.StartCoroutine(this.CascadeSetRowCheckBoxes(widgetColumn.columns_affected, widgetRow, new_value, widget_go));
        break;
    }
  }

  protected IEnumerator CascadeSetRowCheckBoxes(
    CheckboxTableColumn[] checkBoxToggleColumns,
    TableRow row,
    TableScreen.ResultValues state,
    GameObject ignore_widget = null)
  {
    if (this.active_cascade_coroutine_count == 0)
      this.current_looping_sound = LoopingSoundManager.StartSound(this.cascade_sound_path, Vector3.zero, false, false);
    ++this.active_cascade_coroutine_count;
    for (int i = 0; i < checkBoxToggleColumns.Length; ++i)
    {
      if (checkBoxToggleColumns[i].widgets_by_row.ContainsKey(row))
      {
        GameObject widget_go = checkBoxToggleColumns[i].widgets_by_row[row];
        if (!((UnityEngine.Object) widget_go == (UnityEngine.Object) ignore_widget) && checkBoxToggleColumns[i].isRevealed)
        {
          bool flag = false;
          switch ((this.GetWidgetColumn(widget_go) as CheckboxTableColumn).get_value_action(row.GetIdentity(), widget_go))
          {
            case TableScreen.ResultValues.False:
              flag = state != TableScreen.ResultValues.False;
              break;
            case TableScreen.ResultValues.Partial:
            case TableScreen.ResultValues.ConditionalGroup:
              flag = true;
              break;
            case TableScreen.ResultValues.True:
              flag = state != TableScreen.ResultValues.True;
              break;
          }
          if (flag)
          {
            (this.GetWidgetColumn(widget_go) as CheckboxTableColumn).on_set_action(widget_go, state);
            yield return (object) null;
          }
        }
      }
    }
    --this.active_cascade_coroutine_count;
    if (this.active_cascade_coroutine_count <= 0)
      this.StopLoopingCascadeSound();
  }

  protected IEnumerator CascadeSetColumnCheckBoxes(
    List<TableRow> rows,
    CheckboxTableColumn checkBoxToggleColumn,
    TableScreen.ResultValues state,
    GameObject header_widget_go = null)
  {
    if (this.active_cascade_coroutine_count == 0)
      this.current_looping_sound = LoopingSoundManager.StartSound(this.cascade_sound_path, Vector3.zero, false);
    ++this.active_cascade_coroutine_count;
    for (int i = 0; i < rows.Count; ++i)
    {
      GameObject widget = rows[i].GetWidget((TableColumn) checkBoxToggleColumn);
      if (!((UnityEngine.Object) widget == (UnityEngine.Object) header_widget_go))
      {
        bool flag = false;
        switch ((this.GetWidgetColumn(widget) as CheckboxTableColumn).get_value_action(rows[i].GetIdentity(), widget))
        {
          case TableScreen.ResultValues.False:
            flag = state != TableScreen.ResultValues.False;
            break;
          case TableScreen.ResultValues.Partial:
          case TableScreen.ResultValues.ConditionalGroup:
            flag = true;
            break;
          case TableScreen.ResultValues.True:
            flag = state != TableScreen.ResultValues.True;
            break;
        }
        if (flag)
        {
          (this.GetWidgetColumn(widget) as CheckboxTableColumn).on_set_action(widget, state);
          yield return (object) null;
        }
      }
    }
    if ((UnityEngine.Object) header_widget_go != (UnityEngine.Object) null)
      (this.GetWidgetColumn(header_widget_go) as CheckboxTableColumn).on_load_action((IAssignableIdentity) null, header_widget_go);
    --this.active_cascade_coroutine_count;
    if (this.active_cascade_coroutine_count <= 0)
      this.StopLoopingCascadeSound();
  }

  private void StopLoopingCascadeSound()
  {
    if (!this.current_looping_sound.IsValid())
      return;
    LoopingSoundManager.StopSound(this.current_looping_sound);
    this.current_looping_sound.Clear();
  }

  protected void on_press_checkbox_column_super(GameObject widget_go)
  {
    SuperCheckboxTableColumn widgetColumn = this.GetWidgetColumn(widget_go) as SuperCheckboxTableColumn;
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    switch (this.get_value_checkbox_column_super(widgetRow.GetIdentity(), widget_go))
    {
      case TableScreen.ResultValues.False:
        widgetColumn.on_set_action(widget_go, TableScreen.ResultValues.True);
        break;
      case TableScreen.ResultValues.Partial:
      case TableScreen.ResultValues.ConditionalGroup:
        widgetColumn.on_set_action(widget_go, TableScreen.ResultValues.True);
        break;
      case TableScreen.ResultValues.True:
        widgetColumn.on_set_action(widget_go, TableScreen.ResultValues.False);
        break;
    }
    widgetColumn.on_load_action(widgetRow.GetIdentity(), widget_go);
  }

  protected void on_tooltip_sort_alphabetically(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) STRINGS.UI.TABLESCREENS.COLUMN_SORT_BY_NAME, (TextStyleSetting) null);
        break;
    }
  }

  public enum ResultValues
  {
    False,
    Partial,
    True,
    ConditionalGroup,
    NotApplicable,
  }
}
