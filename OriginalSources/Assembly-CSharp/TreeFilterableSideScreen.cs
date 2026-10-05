// Decompiled with JetBrains decompiler
// Type: TreeFilterableSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using TUNING;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class TreeFilterableSideScreen : SideScreenContent
{
  [SerializeField]
  private MultiToggle allCheckBox;
  [SerializeField]
  private LocText allCheckBoxLabel;
  [SerializeField]
  private GameObject specialItemsHeader;
  [SerializeField]
  private MultiToggle onlyAllowTransportItemsCheckBox;
  [SerializeField]
  private GameObject onlyallowTransportItemsRow;
  [SerializeField]
  private MultiToggle onlyAllowSpicedItemsCheckBox;
  [SerializeField]
  private GameObject onlyallowSpicedItemsRow;
  [SerializeField]
  private TreeFilterableSideScreenRow rowPrefab;
  [SerializeField]
  private GameObject rowGroup;
  [SerializeField]
  private TreeFilterableSideScreenElement elementPrefab;
  [SerializeField]
  private GameObject titlebar;
  [SerializeField]
  private GameObject contentMask;
  [SerializeField]
  private KInputTextField inputField;
  [SerializeField]
  private KButton clearButton;
  [SerializeField]
  private GameObject configurationRowsContainer;
  private GameObject target;
  private bool visualDirty;
  private bool initialized;
  private KImage onlyAllowTransportItemsImg;
  public UIPool<TreeFilterableSideScreenElement> elementPool;
  private UIPool<TreeFilterableSideScreenRow> rowPool;
  private TreeFilterable targetFilterable;
  private Dictionary<Tag, TreeFilterableSideScreenRow> tagRowMap = new Dictionary<Tag, TreeFilterableSideScreenRow>();
  private Dictionary<Tag, bool> rowExpandedStatusMemory = new Dictionary<Tag, bool>();
  private Storage storage;

  private bool InputFieldEmpty => this.inputField.text == "";

  public bool IsStorage => (UnityEngine.Object) this.storage != (UnityEngine.Object) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Initialize();
  }

  private void Initialize()
  {
    if (this.initialized)
      return;
    this.rowPool = new UIPool<TreeFilterableSideScreenRow>(this.rowPrefab);
    this.elementPool = new UIPool<TreeFilterableSideScreenElement>(this.elementPrefab);
    this.allCheckBox.onClick += (System.Action) (() =>
    {
      switch (this.GetAllCheckboxState())
      {
        case TreeFilterableSideScreenRow.State.Off:
        case TreeFilterableSideScreenRow.State.Mixed:
          this.SetAllCheckboxState(TreeFilterableSideScreenRow.State.On);
          break;
        case TreeFilterableSideScreenRow.State.On:
          this.SetAllCheckboxState(TreeFilterableSideScreenRow.State.Off);
          break;
      }
    });
    this.onlyAllowTransportItemsCheckBox.onClick = new System.Action(this.OnlyAllowTransportItemsClicked);
    this.onlyAllowSpicedItemsCheckBox.onClick = new System.Action(this.OnlyAllowSpicedItemsClicked);
    this.initialized = true;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.allCheckBox.transform.parent.parent.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTONTOOLTIP);
    this.onlyAllowTransportItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWTRANSPORTITEMSBUTTONTOOLTIP);
    this.onlyAllowSpicedItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWSPICEDITEMSBUTTONTOOLTIP);
    this.inputField.ActivateInputField();
    this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = (string) STRINGS.UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER;
    this.InitSearch();
  }

  public override float GetSortKey() => this.isEditing ? 50f : base.GetSortKey();

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.Consumed || !this.isEditing)
      return;
    e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (e.Consumed || !this.isEditing)
      return;
    e.Consumed = true;
  }

  public override int GetSideScreenSortOrder() => 1;

  private void UpdateAllCheckBoxVisualState()
  {
    switch (this.GetAllCheckboxState())
    {
      case TreeFilterableSideScreenRow.State.Off:
        this.allCheckBox.ChangeState(0);
        break;
      case TreeFilterableSideScreenRow.State.Mixed:
        this.allCheckBox.ChangeState(1);
        break;
      case TreeFilterableSideScreenRow.State.On:
        this.allCheckBox.ChangeState(2);
        break;
    }
  }

  public void Update()
  {
    foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> tagRow in this.tagRowMap)
    {
      if (tagRow.Value.visualDirty)
      {
        this.visualDirty = true;
        break;
      }
    }
    if (!this.visualDirty)
      return;
    foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> tagRow in this.tagRowMap)
    {
      tagRow.Value.RefreshRowElements();
      tagRow.Value.UpdateCheckBoxVisualState();
    }
    this.UpdateAllCheckBoxVisualState();
    this.visualDirty = false;
  }

  private void OnlyAllowTransportItemsClicked()
  {
    this.storage.SetOnlyFetchMarkedItems(!this.storage.GetOnlyFetchMarkedItems());
  }

  private void OnlyAllowSpicedItemsClicked()
  {
    FoodStorage component = this.storage.GetComponent<FoodStorage>();
    component.SpicedFoodOnly = !component.SpicedFoodOnly;
  }

  private TreeFilterableSideScreenRow.State GetAllCheckboxState()
  {
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> tagRow in this.tagRowMap)
    {
      if (tagRow.Value.standardCommodity && tagRow.Value.gameObject.activeSelf)
      {
        switch (tagRow.Value.GetState())
        {
          case TreeFilterableSideScreenRow.State.Off:
            flag2 = true;
            continue;
          case TreeFilterableSideScreenRow.State.Mixed:
            flag3 = true;
            continue;
          case TreeFilterableSideScreenRow.State.On:
            flag1 = true;
            continue;
          default:
            continue;
        }
      }
    }
    if (flag3)
      return TreeFilterableSideScreenRow.State.Mixed;
    if (flag1 && !flag2)
      return TreeFilterableSideScreenRow.State.On;
    return !flag1 & flag2 || !(flag1 & flag2) ? TreeFilterableSideScreenRow.State.Off : TreeFilterableSideScreenRow.State.Mixed;
  }

  private void SetAllCheckboxState(TreeFilterableSideScreenRow.State newState)
  {
    switch (newState)
    {
      case TreeFilterableSideScreenRow.State.Off:
        using (Dictionary<Tag, TreeFilterableSideScreenRow>.Enumerator enumerator = this.tagRowMap.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<Tag, TreeFilterableSideScreenRow> current = enumerator.Current;
            if (current.Value.standardCommodity)
              current.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
          }
          break;
        }
      case TreeFilterableSideScreenRow.State.On:
        using (Dictionary<Tag, TreeFilterableSideScreenRow>.Enumerator enumerator = this.tagRowMap.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<Tag, TreeFilterableSideScreenRow> current = enumerator.Current;
            if (current.Value.standardCommodity)
              current.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
          }
          break;
        }
    }
    this.visualDirty = true;
  }

  public bool GetElementTagAcceptedState(Tag t) => this.targetFilterable.ContainsTag(t);

  public override bool IsValidForTarget(GameObject target)
  {
    TreeFilterable component1 = target.GetComponent<TreeFilterable>();
    Storage component2 = target.GetComponent<Storage>();
    return (UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) target.GetComponent<FlatTagFilterable>() == (UnityEngine.Object) null && component1.showUserMenu && ((UnityEngine.Object) component2 == (UnityEngine.Object) null || component2.showInUI) && target.GetSMI<StorageTile.Instance>() == null;
  }

  private void ReconfigureForPreviousTarget()
  {
    Debug.Assert((UnityEngine.Object) this.target != (UnityEngine.Object) null, (object) "TreeFilterableSideScreen trying to restore null target.");
    this.SetTarget(this.target);
  }

  public override void SetTarget(GameObject target)
  {
    bool flag = true;
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) target)
      flag = false;
    this.Initialize();
    this.target = target;
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "The target object provided was null");
    }
    else
    {
      this.targetFilterable = target.GetComponent<TreeFilterable>();
      if ((UnityEngine.Object) this.targetFilterable == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "The target provided does not have a Tree Filterable component");
      }
      else
      {
        this.contentMask.GetComponent<LayoutElement>().minHeight = this.targetFilterable.uiHeight == TreeFilterable.UISideScreenHeight.Tall ? 380f : 256f;
        this.storage = this.targetFilterable.GetFilterStorage();
        this.storage.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
        this.storage.Subscribe(1163645216, new Action<object>(this.OnOnlySpicedItemsSettingChanged));
        this.OnOnlyFetchMarkedItemsSettingChanged((object) null);
        this.OnOnlySpicedItemsSettingChanged((object) null);
        this.allCheckBoxLabel.SetText(this.targetFilterable.allResourceFilterLabelString);
        this.CreateCategories();
        this.CreateSpecialItemRows();
        this.titlebar.SetActive(false);
        if (this.storage.showSideScreenTitleBar)
        {
          this.titlebar.SetActive(true);
          this.titlebar.GetComponentInChildren<LocText>().SetText(this.storage.GetProperName());
        }
        if (flag)
        {
          if (!this.InputFieldEmpty)
            this.ClearSearch();
        }
        else
          this.UpdateSearchFilter();
        this.ToggleSearchConfiguration(!this.InputFieldEmpty);
      }
    }
  }

  private void OnOnlyFetchMarkedItemsSettingChanged(object _)
  {
    this.onlyAllowTransportItemsCheckBox.ChangeState(this.storage.GetOnlyFetchMarkedItems() ? 1 : 0);
    if (this.storage.allowSettingOnlyFetchMarkedItems)
      this.onlyallowTransportItemsRow.SetActive(true);
    else
      this.onlyallowTransportItemsRow.SetActive(false);
  }

  private void OnOnlySpicedItemsSettingChanged(object _)
  {
    FoodStorage component = this.storage.GetComponent<FoodStorage>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      this.onlyallowSpicedItemsRow.SetActive(true);
      this.onlyAllowSpicedItemsCheckBox.ChangeState(component.SpicedFoodOnly ? 1 : 0);
    }
    else
      this.onlyallowSpicedItemsRow.SetActive(false);
  }

  public bool IsTagAllowed(Tag tag) => this.targetFilterable.AcceptedTags.Contains(tag);

  public void AddTag(Tag tag)
  {
    if ((UnityEngine.Object) this.targetFilterable == (UnityEngine.Object) null)
      return;
    this.targetFilterable.AddTagToFilter(tag);
  }

  public void RemoveTag(Tag tag)
  {
    if ((UnityEngine.Object) this.targetFilterable == (UnityEngine.Object) null)
      return;
    this.targetFilterable.RemoveTagFromFilter(tag);
  }

  private List<TreeFilterableSideScreen.TagOrderInfo> GetTagsSortedAlphabetically(
    ICollection<Tag> tags)
  {
    List<TreeFilterableSideScreen.TagOrderInfo> sortedAlphabetically = new List<TreeFilterableSideScreen.TagOrderInfo>();
    foreach (Tag tag in (IEnumerable<Tag>) tags)
      sortedAlphabetically.Add(new TreeFilterableSideScreen.TagOrderInfo()
      {
        tag = tag,
        strippedName = tag.ProperNameStripLink()
      });
    sortedAlphabetically.Sort((Comparison<TreeFilterableSideScreen.TagOrderInfo>) ((a, b) => a.strippedName.CompareTo(b.strippedName)));
    return sortedAlphabetically;
  }

  private TreeFilterableSideScreenRow AddRow(Tag rowTag)
  {
    if (this.tagRowMap.ContainsKey(rowTag))
      return this.tagRowMap[rowTag];
    TreeFilterableSideScreenRow freeElement = this.rowPool.GetFreeElement(this.rowGroup, true);
    freeElement.Parent = this;
    freeElement.standardCommodity = !STORAGEFILTERS.SPECIAL_STORAGE.Contains(rowTag);
    this.tagRowMap.Add(rowTag, freeElement);
    Dictionary<Tag, bool> filterMap = new Dictionary<Tag, bool>();
    foreach (TreeFilterableSideScreen.TagOrderInfo tagOrderInfo in this.GetTagsSortedAlphabetically((ICollection<Tag>) DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(rowTag)).FindAll((Predicate<TreeFilterableSideScreen.TagOrderInfo>) (t => !this.targetFilterable.ForbiddenTags.Contains(t.tag))))
      filterMap.Add(tagOrderInfo.tag, this.targetFilterable.ContainsTag(tagOrderInfo.tag) || this.targetFilterable.ContainsTag(rowTag));
    freeElement.SetElement(rowTag, this.targetFilterable.ContainsTag(rowTag), filterMap);
    freeElement.transform.SetAsLastSibling();
    return freeElement;
  }

  public float GetAmountInStorage(Tag tag)
  {
    return !this.IsStorage ? 0.0f : this.storage.GetMassAvailable(tag);
  }

  private void CreateCategories()
  {
    if (this.storage.storageFilters != null && this.storage.storageFilters.Count >= 1)
    {
      bool flag = (UnityEngine.Object) this.target.GetComponent<CreatureDeliveryPoint>() != (UnityEngine.Object) null;
      foreach (TreeFilterableSideScreen.TagOrderInfo tagOrderInfo in this.GetTagsSortedAlphabetically((ICollection<Tag>) this.storage.storageFilters))
      {
        Tag tag = tagOrderInfo.tag;
        if ((flag ? 1 : (DiscoveredResources.Instance.IsDiscovered(tag) ? 1 : 0)) != 0)
          this.AddRow(tag);
      }
      this.visualDirty = true;
    }
    else
      Debug.LogError((object) "If you're filtering, your storage filter should have the filters set on it");
  }

  private void CreateSpecialItemRows()
  {
    this.specialItemsHeader.transform.SetAsLastSibling();
    foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> tagRow in this.tagRowMap)
    {
      if (!tagRow.Value.standardCommodity)
        tagRow.Value.transform.transform.SetAsLastSibling();
    }
    this.RefreshSpecialItemsHeader();
  }

  private void RefreshSpecialItemsHeader()
  {
    bool flag = false;
    foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> tagRow in this.tagRowMap)
    {
      if (!tagRow.Value.standardCommodity)
      {
        flag = true;
        break;
      }
    }
    this.specialItemsHeader.gameObject.SetActive(flag);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    if (!((UnityEngine.Object) this.target != (UnityEngine.Object) null) || this.tagRowMap != null && this.tagRowMap.Count != 0)
      return;
    this.ReconfigureForPreviousTarget();
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if ((UnityEngine.Object) this.storage != (UnityEngine.Object) null)
    {
      this.storage.Unsubscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
      this.storage.Unsubscribe(1163645216, new Action<object>(this.OnOnlySpicedItemsSettingChanged));
    }
    this.rowPool.ClearAll();
    this.elementPool.ClearAll();
    this.tagRowMap.Clear();
  }

  private void RecordRowExpandedStatus()
  {
    this.rowExpandedStatusMemory.Clear();
    foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> tagRow in this.tagRowMap)
      this.rowExpandedStatusMemory.Add(tagRow.Key, tagRow.Value.ArrowExpanded);
  }

  private void RestoreRowExpandedStatus()
  {
    foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> tagRow in this.tagRowMap)
    {
      if (this.rowExpandedStatusMemory.ContainsKey(tagRow.Key))
        tagRow.Value.SetArrowToggleState(this.rowExpandedStatusMemory[tagRow.Key]);
    }
  }

  private void InitSearch()
  {
    KInputTextField inputField = this.inputField;
    inputField.onFocus = inputField.onFocus + (System.Action) (() =>
    {
      this.isEditing = true;
      KScreenManager.Instance.RefreshStack();
      UISounds.PlaySound(UISounds.Sound.Find);
      this.RecordRowExpandedStatus();
    });
    this.inputField.onEndEdit.AddListener((UnityAction<string>) (value =>
    {
      this.isEditing = false;
      KScreenManager.Instance.RefreshStack();
    }));
    this.inputField.onValueChanged.AddListener((UnityAction<string>) (value =>
    {
      if (this.InputFieldEmpty)
        this.RestoreRowExpandedStatus();
      this.ToggleSearchConfiguration(!this.InputFieldEmpty);
      this.UpdateSearchFilter();
    }));
    this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = (string) STRINGS.UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER;
    this.clearButton.onClick += (System.Action) (() =>
    {
      if (this.InputFieldEmpty)
        return;
      this.ClearSearch();
    });
  }

  private void ToggleSearchConfiguration(bool searching)
  {
    this.configurationRowsContainer.gameObject.SetActive(!searching);
    foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> tagRow in this.tagRowMap)
      tagRow.Value.ShowToggleBox(!searching);
    if (searching)
      this.specialItemsHeader.gameObject.SetActive(false);
    else
      this.RefreshSpecialItemsHeader();
  }

  private void ClearSearch()
  {
    this.inputField.text = "";
    this.RestoreRowExpandedStatus();
    this.ToggleSearchConfiguration(false);
  }

  public string CurrentSearchValue => this.inputField.text == null ? "" : this.inputField.text;

  private void UpdateSearchFilter()
  {
    foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> tagRow in this.tagRowMap)
      tagRow.Value.FilterAgainstSearch(tagRow.Key, this.CurrentSearchValue);
  }

  private struct TagOrderInfo
  {
    public Tag tag;
    public string strippedName;
  }
}
