// Decompiled with JetBrains decompiler
// Type: SingleItemSelectionSideScreenBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class SingleItemSelectionSideScreenBase : SideScreenContent
{
  [Space]
  [Header("Settings")]
  [SerializeField]
  private SearchBar searchbar;
  [SerializeField]
  protected HierarchyReferences original_CategoryRow;
  [SerializeField]
  protected SingleItemSelectionRow original_ItemRow;
  protected SortedDictionary<Tag, SingleItemSelectionSideScreenBase.Category> categories = new SortedDictionary<Tag, SingleItemSelectionSideScreenBase.Category>((IComparer<Tag>) SingleItemSelectionSideScreenBase.categoryComparer);
  private Dictionary<Tag, SingleItemSelectionRow> pooledRows = new Dictionary<Tag, SingleItemSelectionRow>();
  private static TagNameComparer categoryComparer = new TagNameComparer(GameTags.Void);
  private static SingleItemSelectionSideScreenBase.ItemComparer itemRowComparer = new SingleItemSelectionSideScreenBase.ItemComparer(GameTags.Void);

  private static bool TagContainsSearchWord(Tag tag, string search)
  {
    return string.IsNullOrEmpty(search) || tag.ProperNameStripLink().ToUpper().Contains(search.ToUpper());
  }

  protected SingleItemSelectionRow CurrentSelectedItem { private set; get; }

  protected override void OnPrefabInit()
  {
    if ((UnityEngine.Object) this.searchbar != (UnityEngine.Object) null)
    {
      this.searchbar.EditingStateChanged = new Action<bool>(this.OnSearchbarEditStateChanged);
      this.searchbar.ValueChanged = new Action<string>(this.OnSearchBarValueChanged);
      this.activateOnSpawn = true;
    }
    base.OnPrefabInit();
  }

  protected virtual void OnSearchbarEditStateChanged(bool isEditing) => this.isEditing = isEditing;

  protected virtual void OnSearchBarValueChanged(string value)
  {
    foreach (Tag key in this.categories.Keys)
    {
      SingleItemSelectionSideScreenBase.Category category = this.categories[key];
      bool flag = SingleItemSelectionSideScreenBase.TagContainsSearchWord(key, value);
      int num = category.FilterItemsBySearch(flag ? (string) null : value);
      category.SetUnfoldedState(num > 0 ? SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded : SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded);
      category.SetVisibilityState(flag || num > 0);
    }
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

  public virtual void SetData(Dictionary<Tag, HashSet<Tag>> data)
  {
    this.ProhibitAllCategories();
    foreach (Tag key in data.Keys)
    {
      ICollection<Tag> items = (ICollection<Tag>) data[key];
      this.CreateCategoryWithItems(key, items);
    }
    this.SortAll();
    if (!((UnityEngine.Object) this.searchbar != (UnityEngine.Object) null) || string.IsNullOrEmpty(this.searchbar.CurrentSearchValue))
      return;
    this.searchbar.ClearSearch();
  }

  public virtual SingleItemSelectionSideScreenBase.Category CreateCategoryWithItems(
    Tag categoryTag,
    ICollection<Tag> items)
  {
    SingleItemSelectionSideScreenBase.Category emptyCategory = this.GetOrCreateEmptyCategory(categoryTag);
    if (!emptyCategory.InitializeItemList(items.Count))
      emptyCategory.RemoveAllItems();
    foreach (Tag itemTag in (IEnumerable<Tag>) items)
    {
      SingleItemSelectionRow itemRow = this.GetOrCreateItemRow(itemTag);
      emptyCategory.AddItem(itemRow);
    }
    return emptyCategory;
  }

  public virtual SingleItemSelectionSideScreenBase.Category GetOrCreateEmptyCategory(Tag categoryTag)
  {
    this.original_CategoryRow.gameObject.SetActive(false);
    SingleItemSelectionSideScreenBase.Category emptyCategory = (SingleItemSelectionSideScreenBase.Category) null;
    if (!this.categories.TryGetValue(categoryTag, out emptyCategory))
    {
      HierarchyReferences references = Util.KInstantiateUI<HierarchyReferences>(this.original_CategoryRow.gameObject, this.original_CategoryRow.transform.parent.gameObject);
      references.gameObject.SetActive(true);
      emptyCategory = new SingleItemSelectionSideScreenBase.Category(references, categoryTag);
      emptyCategory.ItemRemoved = new Action<SingleItemSelectionRow>(this.RecycleItemRow);
      emptyCategory.ToggleClicked += new Action<SingleItemSelectionSideScreenBase.Category>(this.CategoryToggleClicked);
      this.categories.Add(categoryTag, emptyCategory);
    }
    else
    {
      emptyCategory.SetProihibedState(false);
      emptyCategory.SetVisibilityState(true);
    }
    return emptyCategory;
  }

  public virtual SingleItemSelectionRow GetOrCreateItemRow(Tag itemTag)
  {
    this.original_ItemRow.gameObject.SetActive(false);
    SingleItemSelectionRow itemRow = (SingleItemSelectionRow) null;
    if (!this.pooledRows.TryGetValue(itemTag, out itemRow))
    {
      itemRow = Util.KInstantiateUI<SingleItemSelectionRow>(this.original_ItemRow.gameObject, this.original_ItemRow.transform.parent.gameObject);
      itemRow.name = "Item-" + itemTag.ToString();
    }
    else
      this.pooledRows.Remove(itemTag);
    itemRow.gameObject.SetActive(true);
    itemRow.SetTag(itemTag);
    itemRow.Clicked = new Action<SingleItemSelectionRow>(this.ItemRowClicked);
    itemRow.SetVisibleState(true);
    return itemRow;
  }

  public SingleItemSelectionSideScreenBase.Category GetCategoryWithItem(
    Tag itemTag,
    bool includeNotVisibleCategories = false)
  {
    foreach (SingleItemSelectionSideScreenBase.Category categoryWithItem in this.categories.Values)
    {
      if ((includeNotVisibleCategories || categoryWithItem.IsVisible) && (UnityEngine.Object) categoryWithItem.GetItem(itemTag) != (UnityEngine.Object) null)
        return categoryWithItem;
    }
    return (SingleItemSelectionSideScreenBase.Category) null;
  }

  public virtual void SetSelectedItem(SingleItemSelectionRow itemRow)
  {
    if ((UnityEngine.Object) this.CurrentSelectedItem != (UnityEngine.Object) null)
      this.CurrentSelectedItem.SetSelected(false);
    this.CurrentSelectedItem = itemRow;
    if (!((UnityEngine.Object) itemRow != (UnityEngine.Object) null))
      return;
    itemRow.SetSelected(true);
  }

  public virtual bool SetSelectedItem(Tag itemTag)
  {
    foreach (Tag key in this.categories.Keys)
    {
      SingleItemSelectionSideScreenBase.Category category = this.categories[key];
      if (category.IsVisible)
      {
        SingleItemSelectionRow itemRow = category.GetItem(itemTag);
        if ((UnityEngine.Object) itemRow != (UnityEngine.Object) null)
        {
          this.SetSelectedItem(itemRow);
          return true;
        }
      }
    }
    return false;
  }

  public virtual void ItemRowClicked(SingleItemSelectionRow rowClicked)
  {
    this.SetSelectedItem(rowClicked);
  }

  public virtual void CategoryToggleClicked(
    SingleItemSelectionSideScreenBase.Category categoryClicked)
  {
    categoryClicked.ToggleUnfoldedState();
  }

  private void RecycleItemRow(SingleItemSelectionRow row)
  {
    if (this.pooledRows.ContainsKey(row.tag))
      Debug.LogError((object) $"Recycling an item row with tag {row.tag} that was already in the recycle pool");
    if ((UnityEngine.Object) this.CurrentSelectedItem == (UnityEngine.Object) row)
      this.SetSelectedItem((SingleItemSelectionRow) null);
    row.Clicked = (Action<SingleItemSelectionRow>) null;
    row.SetSelected(false);
    row.transform.SetParent(this.original_ItemRow.transform.parent.parent);
    row.gameObject.SetActive(false);
    this.pooledRows.Add(row.tag, row);
  }

  private void ProhibitAllCategories()
  {
    foreach (SingleItemSelectionSideScreenBase.Category category in this.categories.Values)
      category.SetProihibedState(true);
  }

  public virtual void SortAll()
  {
    foreach (SingleItemSelectionSideScreenBase.Category category in this.categories.Values)
    {
      if (category.IsVisible)
      {
        category.Sort();
        category.SendToLastSibiling();
      }
    }
  }

  public class ItemComparer : IComparer<SingleItemSelectionRow>
  {
    private Tag firstTag;

    public ItemComparer()
    {
    }

    public ItemComparer(Tag firstTag) => this.firstTag = firstTag;

    public int Compare(SingleItemSelectionRow x, SingleItemSelectionRow y)
    {
      if ((UnityEngine.Object) x == (UnityEngine.Object) y)
        return 0;
      if (this.firstTag.IsValid)
      {
        if (x.tag == this.firstTag && y.tag != this.firstTag)
          return 1;
        if (x.tag != this.firstTag && y.tag == this.firstTag)
          return -1;
      }
      return x.tag.ProperNameStripLink().CompareTo(y.tag.ProperNameStripLink());
    }
  }

  public class Category
  {
    public Action<SingleItemSelectionRow> ItemRemoved;
    public Action<SingleItemSelectionSideScreenBase.Category> ToggleClicked;
    protected HierarchyReferences hierarchyReferences;
    protected List<SingleItemSelectionRow> items;

    public virtual void ToggleUnfoldedState()
    {
      switch ((SingleItemSelectionSideScreenBase.Category.UnfoldedStates) this.toggle.CurrentState)
      {
        case SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded:
          this.SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
          break;
        case SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded:
          this.SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded);
          break;
      }
    }

    public virtual void SetUnfoldedState(
      SingleItemSelectionSideScreenBase.Category.UnfoldedStates new_state)
    {
      this.toggle.ChangeState((int) new_state);
      this.entries.gameObject.SetActive(new_state == SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
    }

    public virtual void SetTitle(string text) => this.title.text = text;

    public Tag CategoryTag { protected set; get; }

    public bool IsProhibited { protected set; get; }

    public bool IsVisible
    {
      get
      {
        return (UnityEngine.Object) this.hierarchyReferences != (UnityEngine.Object) null && this.hierarchyReferences.gameObject.activeSelf;
      }
    }

    protected RectTransform entries
    {
      get => this.hierarchyReferences.GetReference<RectTransform>("Entries");
    }

    protected LocText title => this.hierarchyReferences.GetReference<LocText>("Label");

    protected MultiToggle toggle => this.hierarchyReferences.GetReference<MultiToggle>("Toggle");

    public Category(HierarchyReferences references, Tag categoryTag)
    {
      this.CategoryTag = categoryTag;
      this.hierarchyReferences = references;
      this.toggle.onClick = new System.Action(this.OnToggleClicked);
      this.SetTitle(categoryTag.ProperName());
    }

    public virtual void OnToggleClicked()
    {
      Action<SingleItemSelectionSideScreenBase.Category> toggleClicked = this.ToggleClicked;
      if (toggleClicked == null)
        return;
      toggleClicked(this);
    }

    public virtual void AddItems(SingleItemSelectionRow[] _items)
    {
      if (this.items == null)
      {
        this.items = new List<SingleItemSelectionRow>((IEnumerable<SingleItemSelectionRow>) _items);
      }
      else
      {
        for (int index = 0; index < _items.Length; ++index)
        {
          if (!this.items.Contains(_items[index]))
          {
            _items[index].transform.SetParent((Transform) this.entries, false);
            this.items.Add(_items[index]);
          }
        }
      }
    }

    public virtual void AddItem(SingleItemSelectionRow item)
    {
      if (this.items == null)
        this.items = new List<SingleItemSelectionRow>();
      item.transform.SetParent((Transform) this.entries, false);
      this.items.Add(item);
    }

    public virtual bool InitializeItemList(int size)
    {
      if (this.items != null)
        return false;
      this.items = new List<SingleItemSelectionRow>(size);
      return true;
    }

    public virtual void SetVisibilityState(bool isVisible)
    {
      this.hierarchyReferences.gameObject.SetActive(isVisible && !this.IsProhibited);
    }

    public virtual void RemoveAllItems()
    {
      for (int index = 0; index < this.items.Count; ++index)
      {
        SingleItemSelectionRow itemSelectionRow = this.items[index];
        Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
        if (itemRemoved != null)
          itemRemoved(itemSelectionRow);
      }
      this.items.Clear();
      this.items = (List<SingleItemSelectionRow>) null;
    }

    public virtual SingleItemSelectionRow RemoveItem(Tag itemTag)
    {
      if (this.items != null)
      {
        SingleItemSelectionRow itemSelectionRow = this.items.Find((Predicate<SingleItemSelectionRow>) (row => row.tag == itemTag));
        if ((UnityEngine.Object) itemSelectionRow != (UnityEngine.Object) null)
        {
          Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
          if (itemRemoved != null)
            itemRemoved(itemSelectionRow);
          return itemSelectionRow;
        }
      }
      return (SingleItemSelectionRow) null;
    }

    public virtual bool RemoveItem(SingleItemSelectionRow itemRow)
    {
      if (this.items == null || !this.items.Remove(itemRow))
        return false;
      Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
      if (itemRemoved != null)
        itemRemoved(itemRow);
      return true;
    }

    public SingleItemSelectionRow GetItem(Tag itemTag)
    {
      return this.items == null ? (SingleItemSelectionRow) null : this.items.Find((Predicate<SingleItemSelectionRow>) (row => row.tag == itemTag));
    }

    public int FilterItemsBySearch(string searchValue)
    {
      int num = 0;
      if (this.items != null)
      {
        foreach (SingleItemSelectionRow itemSelectionRow in this.items)
        {
          bool isVisible = SingleItemSelectionSideScreenBase.TagContainsSearchWord(itemSelectionRow.tag, searchValue);
          itemSelectionRow.SetVisibleState(isVisible);
          if (isVisible)
            ++num;
        }
      }
      return num;
    }

    public void Sort()
    {
      if (this.items == null)
        return;
      this.items.Sort((IComparer<SingleItemSelectionRow>) SingleItemSelectionSideScreenBase.itemRowComparer);
      foreach (KMonoBehaviour kmonoBehaviour in this.items)
        kmonoBehaviour.transform.SetAsLastSibling();
    }

    public void SendToLastSibiling() => this.hierarchyReferences.transform.SetAsLastSibling();

    public void SetProihibedState(bool isPohibited)
    {
      this.IsProhibited = isPohibited;
      if (!(this.IsVisible & isPohibited))
        return;
      this.SetVisibilityState(false);
    }

    public enum UnfoldedStates
    {
      Folded,
      Unfolded,
    }
  }
}
