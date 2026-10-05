// Decompiled with JetBrains decompiler
// Type: TreeFilterable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterable")]
public class TreeFilterable : KMonoBehaviour, ISaveLoadable
{
  [MyCmpReq]
  private Storage storage;
  public Tag storageToFilterTag = Tag.Invalid;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  public static readonly Color32 FILTER_TINT = (Color32) Color.white;
  public static readonly Color32 NO_FILTER_TINT = (Color32) new Color(0.5019608f, 0.5019608f, 0.5019608f, 1f);
  public Color32 filterTint = TreeFilterable.FILTER_TINT;
  public Color32 noFilterTint = TreeFilterable.NO_FILTER_TINT;
  [SerializeField]
  public bool dropIncorrectOnFilterChange = true;
  [SerializeField]
  public bool autoSelectStoredOnLoad = true;
  public bool showUserMenu = true;
  public bool copySettingsEnabled = true;
  public bool preventAutoAddOnDiscovery;
  public string allResourceFilterLabelString = (string) UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON;
  public bool filterAllStoragesOnBuilding;
  public bool tintOnNoFiltersSet = true;
  public TreeFilterable.UISideScreenHeight uiHeight = TreeFilterable.UISideScreenHeight.Tall;
  public bool filterByStorageCategoriesOnSpawn = true;
  [SerializeField]
  [Serialize]
  [Obsolete("Deprecated, use acceptedTagSet")]
  private List<Tag> acceptedTags = new List<Tag>();
  [SerializeField]
  [Serialize]
  private HashSet<Tag> acceptedTagSet = new HashSet<Tag>();
  public HashSet<Tag> ForbiddenTags = new HashSet<Tag>();
  public Action<HashSet<Tag>> OnFilterChanged;
  private static readonly EventSystem.IntraObjectHandler<TreeFilterable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<TreeFilterable>((Action<TreeFilterable, object>) ((component, data) => component.OnCopySettings(data)));

  public HashSet<Tag> AcceptedTags => this.acceptedTagSet;

  [System.Runtime.Serialization.OnDeserialized]
  [Obsolete]
  private void OnDeserialized()
  {
    if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
      this.filterByStorageCategoriesOnSpawn = false;
    if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
      return;
    this.acceptedTagSet.UnionWith((IEnumerable<Tag>) this.acceptedTags);
    this.acceptedTagSet.ExceptWith((IEnumerable<Tag>) this.ForbiddenTags);
    this.acceptedTags = (List<Tag>) null;
  }

  private void OnDiscover(Tag category_tag, Tag tag)
  {
    if (this.preventAutoAddOnDiscovery || !this.storage.storageFilters.Contains(category_tag))
      return;
    bool flag = false;
    if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category_tag).Count <= 1)
    {
      foreach (Tag storageFilter in this.storage.storageFilters)
      {
        if (!(storageFilter == category_tag) && DiscoveredResources.Instance.IsDiscovered(storageFilter))
        {
          flag = true;
          foreach (Tag tag1 in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(storageFilter))
          {
            if (!this.acceptedTagSet.Contains(tag1))
              return;
          }
        }
      }
      if (!flag)
        return;
    }
    foreach (Tag tag2 in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category_tag))
    {
      if (!(tag2 == tag) && !this.acceptedTagSet.Contains(tag2))
        return;
    }
    this.AddTagToFilter(tag);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<TreeFilterable>(-905833192, TreeFilterable.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    DiscoveredResources.Instance.OnDiscover += new Action<Tag, Tag>(this.OnDiscover);
    if (this.storageToFilterTag != Tag.Invalid)
    {
      foreach (Storage component in this.GetComponents<Storage>())
      {
        if (component.storageID == this.storageToFilterTag)
        {
          this.storage = component;
          break;
        }
      }
    }
    if (this.autoSelectStoredOnLoad && (UnityEngine.Object) this.storage != (UnityEngine.Object) null)
    {
      HashSet<Tag> filters = new HashSet<Tag>((IEnumerable<Tag>) this.acceptedTagSet);
      filters.UnionWith((IEnumerable<Tag>) this.storage.GetAllIDsInStorage());
      this.UpdateFilters(filters);
    }
    if (this.OnFilterChanged != null)
      this.OnFilterChanged(this.acceptedTagSet);
    this.RefreshTint();
    if (!this.filterByStorageCategoriesOnSpawn)
      return;
    this.RemoveIncorrectAcceptedTags();
  }

  private void RemoveIncorrectAcceptedTags()
  {
    List<Tag> tagList = new List<Tag>();
    foreach (Tag acceptedTag in this.acceptedTagSet)
    {
      bool flag = false;
      foreach (Tag storageFilter in this.storage.storageFilters)
      {
        if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(storageFilter).Contains(acceptedTag))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        tagList.Add(acceptedTag);
    }
    foreach (Tag t in tagList)
      this.RemoveTagFromFilter(t);
  }

  protected override void OnCleanUp()
  {
    DiscoveredResources.Instance.OnDiscover -= new Action<Tag, Tag>(this.OnDiscover);
    base.OnCleanUp();
  }

  private void OnCopySettings(object data)
  {
    if (!this.copySettingsEnabled)
      return;
    TreeFilterable component = ((GameObject) data).GetComponent<TreeFilterable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.UpdateFilters(component.GetTags());
  }

  public Storage GetFilterStorage() => this.storage;

  public HashSet<Tag> GetTags() => this.acceptedTagSet;

  public bool ContainsTag(Tag t) => this.acceptedTagSet.Contains(t);

  public void AddTagToFilter(Tag t)
  {
    if (this.ContainsTag(t))
      return;
    this.UpdateFilters(new HashSet<Tag>((IEnumerable<Tag>) this.acceptedTagSet)
    {
      t
    });
  }

  public void RemoveTagFromFilter(Tag t)
  {
    if (!this.ContainsTag(t))
      return;
    HashSet<Tag> filters = new HashSet<Tag>((IEnumerable<Tag>) this.acceptedTagSet);
    filters.Remove(t);
    this.UpdateFilters(filters);
  }

  public void UpdateFilters(HashSet<Tag> filters)
  {
    this.acceptedTagSet.Clear();
    this.acceptedTagSet.UnionWith((IEnumerable<Tag>) filters);
    this.acceptedTagSet.ExceptWith((IEnumerable<Tag>) this.ForbiddenTags);
    if (this.OnFilterChanged != null)
      this.OnFilterChanged(this.acceptedTagSet);
    this.RefreshTint();
    if (!this.dropIncorrectOnFilterChange || (UnityEngine.Object) this.storage == (UnityEngine.Object) null || this.storage.items == null)
      return;
    if (!this.filterAllStoragesOnBuilding)
    {
      this.DropFilteredItemsFromTargetStorage(this.storage);
    }
    else
    {
      foreach (Storage component in this.GetComponents<Storage>())
        this.DropFilteredItemsFromTargetStorage(component);
    }
  }

  private void DropFilteredItemsFromTargetStorage(Storage targetStorage)
  {
    for (int index = targetStorage.items.Count - 1; index >= 0; --index)
    {
      GameObject go = targetStorage.items[index];
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && !this.acceptedTagSet.Contains(go.GetComponent<KPrefabID>().PrefabTag))
        targetStorage.Drop(go, true);
    }
  }

  public string GetTagsAsStatus(int maxDisplays = 6)
  {
    string tagsAsStatus = "Tags:\n";
    List<Tag> first = new List<Tag>((IEnumerable<Tag>) this.storage.storageFilters);
    first.Intersect<Tag>((IEnumerable<Tag>) this.acceptedTagSet);
    for (int index = 0; index < Mathf.Min(first.Count, maxDisplays); ++index)
    {
      tagsAsStatus += first[index].ProperName();
      if (index < Mathf.Min(first.Count, maxDisplays) - 1)
        tagsAsStatus += "\n";
      if (index == maxDisplays - 1 && first.Count > maxDisplays)
      {
        tagsAsStatus += "\n...";
        break;
      }
    }
    if (this.tag.Length == 0)
      tagsAsStatus = "No tags selected";
    return tagsAsStatus;
  }

  private void RefreshTint()
  {
    bool flag = this.acceptedTagSet != null && this.acceptedTagSet.Count != 0;
    if (this.tintOnNoFiltersSet)
      this.GetComponent<KBatchedAnimController>().TintColour = flag ? this.filterTint : this.noFilterTint;
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoStorageFilterSet, !flag, (object) this);
  }

  public enum UISideScreenHeight
  {
    Short,
    Tall,
  }
}
