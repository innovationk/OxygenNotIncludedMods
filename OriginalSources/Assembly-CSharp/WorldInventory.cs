// Decompiled with JetBrains decompiler
// Type: WorldInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/WorldInventory")]
public class WorldInventory : KMonoBehaviour, ISaveLoadable
{
  private WorldContainer m_worldContainer;
  [Serialize]
  public List<Tag> pinnedResources = new List<Tag>();
  [Serialize]
  public List<Tag> notifyResources = new List<Tag>();
  private Dictionary<Tag, HashSet<Pickupable>> Inventory = new Dictionary<Tag, HashSet<Pickupable>>();
  private Dictionary<Tag, float> accessibleAmounts = new Dictionary<Tag, float>();
  private bool hasValidCount;
  private static readonly EventSystem.IntraObjectHandler<WorldInventory> OnNewDayDelegate = new EventSystem.IntraObjectHandler<WorldInventory>((Action<WorldInventory, object>) ((component, data) => component.GenerateInventoryReport(data)));
  private int accessibleUpdateIndex;
  private bool firstUpdate = true;
  private static Tag[] NonCritterEntitiesTags = new Tag[2]
  {
    GameTags.DupeBrain,
    GameTags.Robot
  };

  public WorldContainer WorldContainer
  {
    get
    {
      if ((UnityEngine.Object) this.m_worldContainer == (UnityEngine.Object) null)
        this.m_worldContainer = this.GetComponent<WorldContainer>();
      return this.m_worldContainer;
    }
  }

  private MinionGroupProber Prober => MinionGroupProber.Get();

  public bool HasValidCount => this.hasValidCount;

  private int worldId
  {
    get
    {
      WorldContainer worldContainer = this.WorldContainer;
      return !((UnityEngine.Object) worldContainer != (UnityEngine.Object) null) ? -1 : worldContainer.id;
    }
  }

  protected override void OnPrefabInit()
  {
    this.Subscribe(Game.Instance.gameObject, -1588644844, new Action<object>(this.OnAddedFetchable));
    this.Subscribe(Game.Instance.gameObject, -1491270284, new Action<object>(this.OnRemovedFetchable));
    this.Subscribe<WorldInventory>(631075836, WorldInventory.OnNewDayDelegate);
    this.m_worldContainer = this.GetComponent<WorldContainer>();
  }

  protected override void OnCleanUp()
  {
    this.Unsubscribe(Game.Instance.gameObject, -1588644844, new Action<object>(this.OnAddedFetchable));
    this.Unsubscribe(Game.Instance.gameObject, -1491270284, new Action<object>(this.OnRemovedFetchable));
    base.OnCleanUp();
  }

  private void GenerateInventoryReport(object data)
  {
    int num1 = 0;
    int num2 = 0;
    foreach (Brain worldItem in Components.Brains.GetWorldItems(this.worldId))
    {
      CreatureBrain cmp = worldItem as CreatureBrain;
      if ((UnityEngine.Object) cmp != (UnityEngine.Object) null)
      {
        if (cmp.HasTag(GameTags.Creatures.Wild))
        {
          ++num1;
          ReportManager.Instance.ReportValue(ReportManager.ReportType.WildCritters, 1f, cmp.GetProperName(), cmp.GetProperName());
        }
        else
        {
          ++num2;
          ReportManager.Instance.ReportValue(ReportManager.ReportType.DomesticatedCritters, 1f, cmp.GetProperName(), cmp.GetProperName());
        }
      }
    }
    if (DlcManager.IsExpansion1Active())
    {
      WorldContainer component1 = this.GetComponent<WorldContainer>();
      if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null) || !component1.IsModuleInterior)
        return;
      Clustercraft component2 = component1.GetComponent<ClusterGridEntity>() as Clustercraft;
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null) || component2.Status == Clustercraft.CraftStatus.Grounded)
        return;
      ReportManager.Instance.ReportValue(ReportManager.ReportType.RocketsInFlight, 1f, component2.Name);
    }
    else
    {
      foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
      {
        if (spacecraft.state != Spacecraft.MissionState.Grounded && spacecraft.state != Spacecraft.MissionState.Destroyed)
          ReportManager.Instance.ReportValue(ReportManager.ReportType.RocketsInFlight, 1f, spacecraft.rocketName);
      }
    }
  }

  protected override void OnSpawn() => this.StartCoroutine(this.InitialRefresh());

  private IEnumerator InitialRefresh()
  {
    for (int i = 0; i < 1; ++i)
      yield return (object) null;
    for (int idx = 0; idx < Components.Pickupables.Count; ++idx)
    {
      Pickupable pickupable = Components.Pickupables[idx];
      if ((UnityEngine.Object) pickupable != (UnityEngine.Object) null)
        pickupable.GetSMI<ReachabilityMonitor.Instance>()?.UpdateReachability();
    }
  }

  public bool IsReachable(Pickupable pickupable) => this.Prober.IsReachable((Workable) pickupable);

  public float GetTotalAmount(Tag tag, bool includeRelatedWorlds)
  {
    float totalAmount = 0.0f;
    this.accessibleAmounts.TryGetValue(tag, out totalAmount);
    return totalAmount;
  }

  public ICollection<Pickupable> GetPickupables(Tag tag, bool includeRelatedWorlds = false)
  {
    if (includeRelatedWorlds)
      return (ICollection<Pickupable>) ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
    HashSet<Pickupable> pickupables = (HashSet<Pickupable>) null;
    this.Inventory.TryGetValue(tag, out pickupables);
    return (ICollection<Pickupable>) pickupables;
  }

  public void GetPickupablesFromRelatedWorlds(Tag tag, ref List<Pickupable> pickupables)
  {
    ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag, ref pickupables);
  }

  public List<Pickupable> CreatePickupablesList(Tag tag)
  {
    HashSet<Pickupable> source = (HashSet<Pickupable>) null;
    this.Inventory.TryGetValue(tag, out source);
    return source == null ? (List<Pickupable>) null : source.ToList<Pickupable>();
  }

  public float GetAmount(Tag tag, bool includeRelatedWorlds)
  {
    return Mathf.Max(includeRelatedWorlds ? ClusterUtil.GetAmountFromRelatedWorlds(this, tag) : this.GetTotalAmount(tag, includeRelatedWorlds) - MaterialNeeds.GetAmount(tag, this.worldId, includeRelatedWorlds), 0.0f);
  }

  public int GetCountWithAdditionalTag(Tag tag, Tag additionalTag, bool includeRelatedWorlds = false)
  {
    ICollection<Pickupable> pickupables = includeRelatedWorlds ? (ICollection<Pickupable>) ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag) : this.GetPickupables(tag);
    int withAdditionalTag = 0;
    if (pickupables != null)
    {
      if (additionalTag.IsValid)
      {
        foreach (Component cmp in (IEnumerable<Pickupable>) pickupables)
        {
          if (cmp.HasTag(additionalTag))
            ++withAdditionalTag;
        }
      }
      else
        withAdditionalTag = pickupables.Count;
    }
    return withAdditionalTag;
  }

  public float GetAmountWithoutTag(Tag tag, bool includeRelatedWorlds = false, Tag[] forbiddenTags = null)
  {
    if (forbiddenTags == null)
      return this.GetAmount(tag, includeRelatedWorlds);
    float amountWithoutTag = 0.0f;
    ICollection<Pickupable> pickupables = includeRelatedWorlds ? (ICollection<Pickupable>) ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag) : this.GetPickupables(tag);
    if (pickupables != null)
    {
      foreach (Pickupable pickupable in (IEnumerable<Pickupable>) pickupables)
      {
        if ((UnityEngine.Object) pickupable != (UnityEngine.Object) null && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate) && !pickupable.KPrefabID.HasAnyTags(forbiddenTags))
          amountWithoutTag += pickupable.TotalAmount;
      }
    }
    return amountWithoutTag;
  }

  private void Update()
  {
    int num1 = 0;
    Dictionary<Tag, HashSet<Pickupable>>.Enumerator enumerator = this.Inventory.GetEnumerator();
    int worldId = this.worldId;
    while (enumerator.MoveNext())
    {
      KeyValuePair<Tag, HashSet<Pickupable>> current = enumerator.Current;
      if (num1 == this.accessibleUpdateIndex || this.firstUpdate)
      {
        Tag key = current.Key;
        HashSet<Pickupable> pickupableSet = current.Value;
        float num2 = 0.0f;
        foreach (Pickupable component in (IEnumerable<Pickupable>) pickupableSet)
        {
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.GetMyWorldId() == worldId && !component.KPrefabID.HasTag(GameTags.StoredPrivate))
            num2 += component.TotalAmount;
        }
        if (!this.hasValidCount && this.accessibleUpdateIndex + 1 >= this.Inventory.Count)
        {
          this.hasValidCount = true;
          if (this.worldId == ClusterManager.Instance.activeWorldId)
          {
            this.hasValidCount = true;
            PinnedResourcesPanel.Instance.ClearExcessiveNewItems();
            PinnedResourcesPanel.Instance.Refresh();
          }
        }
        this.accessibleAmounts[key] = num2;
        this.accessibleUpdateIndex = (this.accessibleUpdateIndex + 1) % this.Inventory.Count;
        break;
      }
      ++num1;
    }
    this.firstUpdate = false;
  }

  protected override void OnLoadLevel() => base.OnLoadLevel();

  private void OnAddedFetchable(object data)
  {
    GameObject gameObject = (GameObject) data;
    KPrefabID component1 = gameObject.GetComponent<KPrefabID>();
    if (component1.HasAnyTags(WorldInventory.NonCritterEntitiesTags))
      return;
    Pickupable component2 = gameObject.GetComponent<Pickupable>();
    if (component2.GetMyWorldId() != this.worldId)
      return;
    Tag tag1 = component1.PrefabID();
    if (!this.Inventory.ContainsKey(tag1))
    {
      Tag categoryForEntity = DiscoveredResources.GetCategoryForEntity(component1);
      DebugUtil.DevAssertArgs((categoryForEntity.IsValid ? 1 : 0) != 0, (object) component2.name, (object) "was found by worldinventory but doesn't have a category! Add it to the element definition.");
      DiscoveredResources.Instance.Discover(tag1, categoryForEntity);
    }
    HashSet<Pickupable> pickupableSet;
    if (!this.Inventory.TryGetValue(tag1, out pickupableSet))
    {
      pickupableSet = new HashSet<Pickupable>();
      this.Inventory[tag1] = pickupableSet;
    }
    pickupableSet.Add(component2);
    foreach (Tag tag2 in component1.Tags)
    {
      if (!this.Inventory.TryGetValue(tag2, out pickupableSet))
      {
        pickupableSet = new HashSet<Pickupable>();
        this.Inventory[tag2] = pickupableSet;
      }
      pickupableSet.Add(component2);
    }
  }

  private void OnRemovedFetchable(object data)
  {
    Pickupable component = ((GameObject) data).GetComponent<Pickupable>();
    KPrefabID kprefabId = component.KPrefabID;
    HashSet<Pickupable> pickupableSet;
    if (this.Inventory.TryGetValue(kprefabId.PrefabTag, out pickupableSet))
      pickupableSet.Remove(component);
    foreach (Tag tag in kprefabId.Tags)
    {
      if (this.Inventory.TryGetValue(tag, out pickupableSet))
        pickupableSet.Remove(component);
    }
  }

  public Dictionary<Tag, float> GetAccessibleAmounts() => this.accessibleAmounts;
}
