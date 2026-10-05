// Decompiled with JetBrains decompiler
// Type: FetchManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FoodRehydrator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/FetchManager")]
public class FetchManager : KMonoBehaviour, ISim1000ms
{
  private List<FetchManager.Pickup> pickups = new List<FetchManager.Pickup>();
  public Dictionary<Tag, FetchManager.FetchablesByPrefabId> prefabIdToFetchables = new Dictionary<Tag, FetchManager.FetchablesByPrefabId>();
  private WorkItemCollection<FetchManager.UpdateOffsetTables, object> updateOffsetTables = new WorkItemCollection<FetchManager.UpdateOffsetTables, object>();
  private WorkItemCollection<FetchManager.UpdatePickupWorkItem, object> updatePickupsWorkItems = new WorkItemCollection<FetchManager.UpdatePickupWorkItem, object>();

  private static int QuantizeRotValue(float rot_value) => (int) (4.0 * (double) rot_value);

  public HandleVector<int>.Handle Add(Pickupable pickupable)
  {
    Tag tag = pickupable.KPrefabID.PrefabID();
    FetchManager.FetchablesByPrefabId fetchablesByPrefabId = (FetchManager.FetchablesByPrefabId) null;
    if (!this.prefabIdToFetchables.TryGetValue(tag, out fetchablesByPrefabId))
    {
      fetchablesByPrefabId = new FetchManager.FetchablesByPrefabId(tag);
      this.prefabIdToFetchables[tag] = fetchablesByPrefabId;
    }
    return fetchablesByPrefabId.AddPickupable(pickupable);
  }

  public void Remove(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
  {
    FetchManager.FetchablesByPrefabId fetchablesByPrefabId;
    if (!this.prefabIdToFetchables.TryGetValue(prefab_tag, out fetchablesByPrefabId))
      return;
    fetchablesByPrefabId.RemovePickupable(fetchable_handle);
  }

  public void UpdateStorage(
    Tag prefab_tag,
    HandleVector<int>.Handle fetchable_handle,
    Storage storage)
  {
    FetchManager.FetchablesByPrefabId fetchablesByPrefabId;
    if (!this.prefabIdToFetchables.TryGetValue(prefab_tag, out fetchablesByPrefabId))
      return;
    fetchablesByPrefabId.UpdateStorage(fetchable_handle, storage);
  }

  public void UpdateTags(Tag prefab_tag, HandleVector<int>.Handle fetchable_handle)
  {
    this.prefabIdToFetchables[prefab_tag].UpdateTags(fetchable_handle);
  }

  public void Sim1000ms(float dt)
  {
    foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> prefabIdToFetchable in this.prefabIdToFetchables)
      prefabIdToFetchable.Value.Sim1000ms(dt);
  }

  public void UpdatePickups(Navigator navigator, WorkerBase worker)
  {
    this.updateOffsetTables.Reset((object) null);
    this.updatePickupsWorkItems.Reset((object) null);
    foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> prefabIdToFetchable in this.prefabIdToFetchables)
    {
      FetchManager.FetchablesByPrefabId fetchables = prefabIdToFetchable.Value;
      this.updateOffsetTables.Add(new FetchManager.UpdateOffsetTables(fetchables));
      this.updatePickupsWorkItems.Add(new FetchManager.UpdatePickupWorkItem()
      {
        fetchablesByPrefabId = fetchables,
        navigator = navigator,
        worker = worker.GetComponent<KPrefabID>().InstanceID
      });
    }
    GlobalJobManager.Run((IWorkItemCollection) this.updateOffsetTables);
    for (int idx = 0; idx < this.updateOffsetTables.Count; ++idx)
      this.updateOffsetTables.GetWorkItem(idx).Finish();
    OffsetTracker.isExecutingWithinJob = true;
    GlobalJobManager.Run((IWorkItemCollection) this.updatePickupsWorkItems);
    OffsetTracker.isExecutingWithinJob = false;
    this.pickups.Clear();
    foreach (KeyValuePair<Tag, FetchManager.FetchablesByPrefabId> prefabIdToFetchable in this.prefabIdToFetchables)
      this.pickups.AddRange((IEnumerable<FetchManager.Pickup>) prefabIdToFetchable.Value.finalPickups);
    this.pickups.Sort(FetchManager.PickupComparerNoPriority.CompareInst);
  }

  public static bool IsFetchablePickup(Pickupable pickup, FetchChore chore, Storage destination)
  {
    KPrefabID kprefabId = pickup.KPrefabID;
    Storage storage = pickup.storage;
    return (double) pickup.UnreservedFetchAmount > 0.0 && ((double) pickup.PrimaryElement.MassPerUnit <= 1.0 || (double) pickup.PrimaryElement.MassPerUnit <= (double) chore.originalAmount) && !((UnityEngine.Object) kprefabId == (UnityEngine.Object) null) && pickup.isChoreAllowedToPickup(chore.choreType) && (chore.criteria != FetchChore.MatchCriteria.MatchID || chore.tags.Contains(kprefabId.PrefabTag)) && (chore.criteria != FetchChore.MatchCriteria.MatchTags || kprefabId.HasTag(chore.tagsFirst)) && (!chore.requiredTag.IsValid || kprefabId.HasTag(chore.requiredTag)) && !kprefabId.HasAnyTags(chore.forbiddenTags) && !kprefabId.HasTag(GameTags.MarkedForMove) && (!((UnityEngine.Object) storage != (UnityEngine.Object) null) || (storage.ignoreSourcePriority || !destination.ShouldOnlyTransferFromLowerPriority || !(destination.masterPriority <= storage.masterPriority)) && (destination.storageNetworkID == -1 || destination.storageNetworkID != storage.storageNetworkID));
  }

  public static Pickupable FindFetchTarget(
    List<Pickupable> pickupables,
    Storage destination,
    FetchChore chore)
  {
    foreach (Pickupable pickupable in pickupables)
    {
      if (FetchManager.IsFetchablePickup(pickupable, chore, destination))
        return pickupable;
    }
    return (Pickupable) null;
  }

  public Pickupable FindFetchTarget(Storage destination, FetchChore chore)
  {
    foreach (FetchManager.Pickup pickup in this.pickups)
    {
      if (FetchManager.IsFetchablePickup(pickup.pickupable, chore, destination))
        return pickup.pickupable;
    }
    return (Pickupable) null;
  }

  public static bool IsFetchablePickup_Exclude(
    KPrefabID pickup_id,
    Storage source,
    float pickup_unreserved_amount,
    HashSet<Tag> exclude_tags,
    Tag required_tag,
    Storage destination)
  {
    return FetchManager.IsFetchablePickup_Exclude(pickup_id, source, pickup_unreserved_amount, exclude_tags, new Tag[1]
    {
      required_tag
    }, destination);
  }

  public static bool IsFetchablePickup_Exclude(
    KPrefabID pickup_id,
    Storage source,
    float pickup_unreserved_amount,
    HashSet<Tag> exclude_tags,
    Tag[] required_tags,
    Storage destination)
  {
    return (double) pickup_unreserved_amount > 0.0 && !((UnityEngine.Object) pickup_id == (UnityEngine.Object) null) && !exclude_tags.Contains(pickup_id.PrefabTag) && pickup_id.HasAllTags(required_tags) && (!((UnityEngine.Object) source != (UnityEngine.Object) null) || (source.ignoreSourcePriority || !destination.ShouldOnlyTransferFromLowerPriority || !(destination.masterPriority <= source.masterPriority)) && (destination.storageNetworkID == -1 || destination.storageNetworkID != source.storageNetworkID));
  }

  public Pickupable FindEdibleFetchTarget(
    Storage destination,
    HashSet<Tag> exclude_tags,
    Tag required_tag)
  {
    return this.FindEdibleFetchTarget(destination, exclude_tags, new Tag[1]
    {
      required_tag
    });
  }

  public Pickupable FindEdibleFetchTarget(
    Storage destination,
    HashSet<Tag> exclude_tags,
    Tag[] required_tags)
  {
    FetchManager.Pickup pickup1 = new FetchManager.Pickup()
    {
      PathCost = ushort.MaxValue,
      foodQuality = int.MinValue
    };
    int num1 = int.MaxValue;
    foreach (FetchManager.Pickup pickup2 in this.pickups)
    {
      Pickupable pickupable = pickup2.pickupable;
      if (FetchManager.IsFetchablePickup_Exclude(pickupable.KPrefabID, pickupable.storage, pickupable.UnreservedFetchAmount, exclude_tags, required_tags, destination))
      {
        int num2 = (int) pickup2.PathCost + (5 - pickup2.foodQuality) * 50;
        if (num2 < num1)
        {
          pickup1 = pickup2;
          num1 = num2;
        }
      }
    }
    Navigator component1 = destination.GetComponent<Navigator>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      foreach (GameObject foodRehydrator in Components.FoodRehydrators)
      {
        int cell = Grid.PosToCell(foodRehydrator);
        int navigationCost = component1.GetNavigationCost(cell);
        if (navigationCost != -1 && num1 > navigationCost + 50 + 5)
        {
          AccessabilityManager component2 = (UnityEngine.Object) foodRehydrator != (UnityEngine.Object) null ? foodRehydrator.GetComponent<AccessabilityManager>() : (AccessabilityManager) null;
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.CanAccess(destination.gameObject))
          {
            foreach (GameObject gameObject in foodRehydrator.GetComponent<Storage>().items)
            {
              Storage component3 = (UnityEngine.Object) gameObject != (UnityEngine.Object) null ? gameObject.GetComponent<Storage>() : (Storage) null;
              if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && !component3.IsEmpty())
              {
                Edible component4 = component3.items[0].GetComponent<Edible>();
                Pickupable component5 = component4.GetComponent<Pickupable>();
                if (FetchManager.IsFetchablePickup_Exclude(component5.KPrefabID, component5.storage, component5.UnreservedFetchAmount, exclude_tags, required_tags, destination))
                {
                  int num3 = navigationCost + (5 - component4.FoodInfo.Quality + 1) * 50 + 5;
                  if (num3 < num1)
                  {
                    pickup1.pickupable = component5;
                    pickup1.foodQuality = component4.FoodInfo.Quality;
                    pickup1.tagBitsHash = component4.PrefabID().GetHashCode();
                    num1 = num3;
                  }
                }
              }
            }
          }
        }
      }
    }
    return pickup1.pickupable;
  }

  public struct Fetchable
  {
    public Pickupable pickupable;
    public int tagBitsHash;
    public int masterPriority;
    public int freshness;
    public int foodQuality;
  }

  [DebuggerDisplay("{pickupable.name}")]
  public struct Pickup
  {
    public Pickupable pickupable;
    public int tagBitsHash;
    public ushort PathCost;
    public int masterPriority;
    public int freshness;
    public int foodQuality;
  }

  private static class PickupComparerIncludingPriority
  {
    public static Comparison<FetchManager.Pickup> CompareInst = new Comparison<FetchManager.Pickup>(FetchManager.PickupComparerIncludingPriority.Compare);

    private static int Compare(FetchManager.Pickup a, FetchManager.Pickup b)
    {
      int num1 = a.tagBitsHash.CompareTo(b.tagBitsHash);
      if (num1 != 0)
        return num1;
      int num2 = b.masterPriority.CompareTo(a.masterPriority);
      if (num2 != 0)
        return num2;
      int num3 = a.PathCost.CompareTo(b.PathCost);
      if (num3 != 0)
        return num3;
      int num4 = b.foodQuality.CompareTo(a.foodQuality);
      return num4 != 0 ? num4 : b.freshness.CompareTo(a.freshness);
    }
  }

  private static class PickupComparerNoPriority
  {
    public static Comparison<FetchManager.Pickup> CompareInst = new Comparison<FetchManager.Pickup>(FetchManager.PickupComparerNoPriority.Compare);

    public static int Compare(FetchManager.Pickup a, FetchManager.Pickup b)
    {
      int num1 = a.PathCost.CompareTo(b.PathCost);
      if (num1 != 0)
        return num1;
      int num2 = b.foodQuality.CompareTo(a.foodQuality);
      return num2 != 0 ? num2 : b.freshness.CompareTo(a.freshness);
    }
  }

  public class FetchablesByPrefabId
  {
    public KCompactedVector<FetchManager.Fetchable> fetchables;
    public List<FetchManager.Pickup> finalPickups = new List<FetchManager.Pickup>();
    private Dictionary<HandleVector<int>.Handle, Rottable.Instance> rotUpdaters;
    private List<FetchManager.Pickup> pickupsWhichCanBePickedUp = new List<FetchManager.Pickup>();
    private Dictionary<int, int> cellCosts = new Dictionary<int, int>();

    public Tag prefabId { get; private set; }

    public FetchablesByPrefabId(Tag prefab_id)
    {
      this.prefabId = prefab_id;
      this.fetchables = new KCompactedVector<FetchManager.Fetchable>();
      this.rotUpdaters = new Dictionary<HandleVector<int>.Handle, Rottable.Instance>();
      this.finalPickups = new List<FetchManager.Pickup>();
    }

    public HandleVector<int>.Handle AddPickupable(Pickupable pickupable)
    {
      int num1 = 5;
      Edible component = pickupable.GetComponent<Edible>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        num1 = component.GetQuality();
      int num2 = 0;
      if ((UnityEngine.Object) pickupable.storage != (UnityEngine.Object) null)
      {
        Prioritizable prioritizable = pickupable.storage.prioritizable;
        if ((UnityEngine.Object) prioritizable != (UnityEngine.Object) null)
          num2 = prioritizable.GetMasterPriority().priority_value;
      }
      Rottable.Instance smi = pickupable.GetSMI<Rottable.Instance>();
      int num3 = 0;
      if (!smi.IsNullOrStopped())
        num3 = FetchManager.QuantizeRotValue(smi.RotValue);
      KPrefabID kprefabId = pickupable.KPrefabID;
      HandleVector<int>.Handle key = this.fetchables.Allocate(new FetchManager.Fetchable()
      {
        pickupable = pickupable,
        foodQuality = num1,
        freshness = num3,
        masterPriority = num2,
        tagBitsHash = kprefabId.GetTagsHash()
      });
      if (!smi.IsNullOrStopped())
        this.rotUpdaters[key] = smi;
      return key;
    }

    public void RemovePickupable(HandleVector<int>.Handle fetchable_handle)
    {
      this.fetchables.Free(fetchable_handle);
      this.rotUpdaters.Remove(fetchable_handle);
    }

    public void UpdatePickups(Navigator worker_navigator, int worker)
    {
      this.GatherPickupablesWhichCanBePickedUp(worker);
      this.GatherReachablePickups(worker_navigator);
      this.finalPickups.Sort(FetchManager.PickupComparerIncludingPriority.CompareInst);
      if (this.finalPickups.Count <= 0)
        return;
      FetchManager.Pickup pickup = this.finalPickups[0];
      int num = pickup.tagBitsHash;
      int count = this.finalPickups.Count;
      int index1 = 0;
      for (int index2 = 1; index2 < this.finalPickups.Count; ++index2)
      {
        bool flag = false;
        FetchManager.Pickup finalPickup = this.finalPickups[index2];
        int tagBitsHash = finalPickup.tagBitsHash;
        if (pickup.masterPriority == finalPickup.masterPriority && tagBitsHash == num)
          flag = true;
        if (flag)
        {
          --count;
        }
        else
        {
          ++index1;
          pickup = finalPickup;
          num = tagBitsHash;
          if (index2 > index1)
            this.finalPickups[index1] = finalPickup;
        }
      }
      this.finalPickups.RemoveRange(count, this.finalPickups.Count - count);
    }

    private void GatherPickupablesWhichCanBePickedUp(int worker)
    {
      this.pickupsWhichCanBePickedUp.Clear();
      foreach (FetchManager.Fetchable data in this.fetchables.GetDataList())
      {
        Pickupable pickupable = data.pickupable;
        if (pickupable.CouldBePickedUpByMinion(worker))
          this.pickupsWhichCanBePickedUp.Add(new FetchManager.Pickup()
          {
            pickupable = pickupable,
            tagBitsHash = data.tagBitsHash,
            PathCost = ushort.MaxValue,
            masterPriority = data.masterPriority,
            freshness = data.freshness,
            foodQuality = data.foodQuality
          });
      }
    }

    public void UpdateOffsetTables()
    {
      foreach (FetchManager.Fetchable data in this.fetchables.GetDataList())
        data.pickupable.GetOffsets(data.pickupable.cachedCell);
    }

    private void GatherReachablePickups(Navigator navigator)
    {
      this.cellCosts.Clear();
      this.finalPickups.Clear();
      foreach (FetchManager.Pickup pickup in this.pickupsWhichCanBePickedUp)
      {
        Pickupable pickupable = pickup.pickupable;
        int num = -1;
        if (!this.cellCosts.TryGetValue(pickupable.cachedCell, out num))
        {
          num = pickupable.GetNavigationCost(navigator, pickupable.cachedCell);
          this.cellCosts[pickupable.cachedCell] = num;
        }
        if (num != -1)
          this.finalPickups.Add(new FetchManager.Pickup()
          {
            pickupable = pickupable,
            tagBitsHash = pickup.tagBitsHash,
            PathCost = (ushort) num,
            masterPriority = pickup.masterPriority,
            freshness = pickup.freshness,
            foodQuality = pickup.foodQuality
          });
      }
    }

    public void UpdateStorage(HandleVector<int>.Handle fetchable_handle, Storage storage)
    {
      FetchManager.Fetchable data = this.fetchables.GetData(fetchable_handle);
      int num = 0;
      Pickupable pickupable = data.pickupable;
      if ((UnityEngine.Object) pickupable.storage != (UnityEngine.Object) null)
      {
        Prioritizable prioritizable = pickupable.storage.prioritizable;
        if ((UnityEngine.Object) prioritizable != (UnityEngine.Object) null)
          num = prioritizable.GetMasterPriority().priority_value;
      }
      data.masterPriority = num;
      this.fetchables.SetData(fetchable_handle, data);
    }

    public void UpdateTags(HandleVector<int>.Handle fetchable_handle)
    {
      FetchManager.Fetchable data = this.fetchables.GetData(fetchable_handle);
      data.tagBitsHash = data.pickupable.KPrefabID.GetTagsHash();
      this.fetchables.SetData(fetchable_handle, data);
    }

    public void Sim1000ms(float dt)
    {
      foreach (KeyValuePair<HandleVector<int>.Handle, Rottable.Instance> rotUpdater in this.rotUpdaters)
      {
        HandleVector<int>.Handle key = rotUpdater.Key;
        Rottable.Instance instance = rotUpdater.Value;
        FetchManager.Fetchable data = this.fetchables.GetData(key) with
        {
          freshness = FetchManager.QuantizeRotValue(instance.RotValue)
        };
        this.fetchables.SetData(key, data);
      }
    }
  }

  private struct UpdateOffsetTables(FetchManager.FetchablesByPrefabId fetchables) : IWorkItem<object>
  {
    public FetchManager.FetchablesByPrefabId data = fetchables;
    private ListPool<Pickupable, FetchManager.UpdateOffsetTables>.PooledList failed = ListPool<Pickupable, FetchManager.UpdateOffsetTables>.Allocate();

    public void Run(object _, int threadIndex)
    {
      if (Game.IsOnMainThread())
      {
        this.data.UpdateOffsetTables();
      }
      else
      {
        foreach (FetchManager.Fetchable data in this.data.fetchables.GetDataList())
        {
          if (!data.pickupable.ValidateOffsets(data.pickupable.cachedCell))
            this.failed.Add(data.pickupable);
        }
      }
    }

    public void Finish()
    {
      foreach (Pickupable pickupable in (List<Pickupable>) this.failed)
        pickupable.GetOffsets(pickupable.cachedCell);
      this.failed.Recycle();
    }
  }

  private struct UpdatePickupWorkItem : IWorkItem<object>
  {
    public FetchManager.FetchablesByPrefabId fetchablesByPrefabId;
    public Navigator navigator;
    public int worker;

    public void Run(object shared_data, int threadIndex)
    {
      this.fetchablesByPrefabId.UpdatePickups(this.navigator, this.worker);
    }
  }
}
