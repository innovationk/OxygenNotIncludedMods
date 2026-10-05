// Decompiled with JetBrains decompiler
// Type: FetchListStatusItemUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/FetchListStatusItemUpdater")]
public class FetchListStatusItemUpdater : KMonoBehaviour, IRender200ms
{
  public static FetchListStatusItemUpdater instance;
  private List<FetchList2> fetchLists = new List<FetchList2>();
  private int[] currentIterationIndex = new int[(int) byte.MaxValue];
  private int maxIteratingCount = 100;

  public static void DestroyInstance()
  {
    FetchListStatusItemUpdater.instance = (FetchListStatusItemUpdater) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    FetchListStatusItemUpdater.instance = this;
  }

  public void AddFetchList(FetchList2 fetch_list) => this.fetchLists.Add(fetch_list);

  public void RemoveFetchList(FetchList2 fetch_list) => this.fetchLists.Remove(fetch_list);

  public void Render200ms(float dt)
  {
    foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
    {
      int id = worldContainer.id;
      DictionaryPool<int, ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList, FetchListStatusItemUpdater>.PooledDictionary pooledDictionary1 = DictionaryPool<int, ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList, FetchListStatusItemUpdater>.Allocate();
      int num1 = Math.Min(this.maxIteratingCount, this.fetchLists.Count - this.currentIterationIndex[id]);
      for (int index = 0; index < num1; ++index)
      {
        FetchList2 fetchList = this.fetchLists[index + this.currentIterationIndex[id]];
        if (!((UnityEngine.Object) fetchList.Destination == (UnityEngine.Object) null) && fetchList.Destination.gameObject.GetMyWorldId() == id)
        {
          ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList pooledList = (ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList) null;
          int instanceId = fetchList.Destination.GetInstanceID();
          if (!pooledDictionary1.TryGetValue(instanceId, out pooledList))
          {
            pooledList = ListPool<FetchList2, FetchListStatusItemUpdater>.Allocate();
            pooledDictionary1[instanceId] = pooledList;
          }
          pooledList.Add(fetchList);
        }
      }
      this.currentIterationIndex[id] += num1;
      if (this.currentIterationIndex[id] >= this.fetchLists.Count)
        this.currentIterationIndex[id] = 0;
      DictionaryPool<Tag, float, FetchListStatusItemUpdater>.PooledDictionary pooledDictionary2 = DictionaryPool<Tag, float, FetchListStatusItemUpdater>.Allocate();
      DictionaryPool<Tag, float, FetchListStatusItemUpdater>.PooledDictionary pooledDictionary3 = DictionaryPool<Tag, float, FetchListStatusItemUpdater>.Allocate();
      foreach (KeyValuePair<int, ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList> keyValuePair1 in (Dictionary<int, ListPool<FetchList2, FetchListStatusItemUpdater>.PooledList>) pooledDictionary1)
      {
        if (!((UnityEngine.Object) keyValuePair1.Value[0].Destination.GetMyWorld() == (UnityEngine.Object) null))
        {
          ListPool<Tag, FetchListStatusItemUpdater>.PooledList pooledList1 = ListPool<Tag, FetchListStatusItemUpdater>.Allocate();
          Storage destination = keyValuePair1.Value[0].Destination;
          foreach (FetchList2 fetchList2 in (List<FetchList2>) keyValuePair1.Value)
          {
            fetchList2.UpdateRemaining();
            foreach (KeyValuePair<Tag, float> keyValuePair2 in fetchList2.GetRemaining())
            {
              if (!pooledList1.Contains(keyValuePair2.Key))
                pooledList1.Add(keyValuePair2.Key);
            }
          }
          ListPool<Pickupable, FetchListStatusItemUpdater>.PooledList pooledList2 = ListPool<Pickupable, FetchListStatusItemUpdater>.Allocate();
          foreach (GameObject gameObject in destination.items)
          {
            if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
            {
              Pickupable component = gameObject.GetComponent<Pickupable>();
              if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
                pooledList2.Add(component);
            }
          }
          DictionaryPool<Tag, float, FetchListStatusItemUpdater>.PooledDictionary pooledDictionary4 = DictionaryPool<Tag, float, FetchListStatusItemUpdater>.Allocate();
          foreach (Tag tag in (List<Tag>) pooledList1)
          {
            float num2 = 0.0f;
            foreach (Pickupable pickupable in (List<Pickupable>) pooledList2)
            {
              if (pickupable.KPrefabID.HasTag(tag))
                num2 += pickupable.FetchTotalAmount;
            }
            pooledDictionary4[tag] = num2;
          }
          foreach (Tag tag in (List<Tag>) pooledList1)
          {
            if (!pooledDictionary2.ContainsKey(tag))
              pooledDictionary2[tag] = destination.GetMyWorld().worldInventory.GetTotalAmount(tag, true);
            if (!pooledDictionary3.ContainsKey(tag))
              pooledDictionary3[tag] = destination.GetMyWorld().worldInventory.GetAmount(tag, true);
          }
          foreach (FetchList2 fetchList2 in (List<FetchList2>) keyValuePair1.Value)
          {
            bool should_add1 = false;
            bool should_add2 = true;
            bool should_add3 = false;
            foreach (KeyValuePair<Tag, float> keyValuePair3 in fetchList2.GetRemaining())
            {
              Tag key = keyValuePair3.Key;
              float a = keyValuePair3.Value;
              double num3 = (double) pooledDictionary4[key];
              float b = pooledDictionary2[key];
              float num4 = pooledDictionary3[key] + Mathf.Min(a, b);
              float minimumAmount = fetchList2.GetMinimumAmount(key);
              if (num3 + (double) num4 < (double) minimumAmount)
                should_add1 = true;
              if ((double) num4 < (double) a)
                should_add2 = false;
              if (num3 + (double) num4 > (double) a && (double) a > (double) num4)
                should_add3 = true;
            }
            fetchList2.UpdateStatusItem(Db.Get().BuildingStatusItems.WaitingForMaterials, ref fetchList2.waitingForMaterialsHandle, should_add2);
            fetchList2.UpdateStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, ref fetchList2.materialsUnavailableHandle, should_add1);
            fetchList2.UpdateStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailableForRefill, ref fetchList2.materialsUnavailableForRefillHandle, should_add3);
          }
          pooledDictionary4.Recycle();
          pooledList2.Recycle();
          pooledList1.Recycle();
          keyValuePair1.Value.Recycle();
        }
      }
      pooledDictionary3.Recycle();
      pooledDictionary2.Recycle();
      pooledDictionary1.Recycle();
    }
  }
}
