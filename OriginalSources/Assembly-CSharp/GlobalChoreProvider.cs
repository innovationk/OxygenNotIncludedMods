// Decompiled with JetBrains decompiler
// Type: GlobalChoreProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class GlobalChoreProvider : ChoreProvider, IRender200ms
{
  public static GlobalChoreProvider Instance;
  public Dictionary<int, List<FetchChore>> fetchMap = new Dictionary<int, List<FetchChore>>();
  public List<GlobalChoreProvider.Fetch> fetches = new List<GlobalChoreProvider.Fetch>();
  private static readonly GlobalChoreProvider.FetchComparer Comparer = new GlobalChoreProvider.FetchComparer();
  private ClearableManager clearableManager;
  private HashSet<Tag> storageFetchableTags = new HashSet<Tag>();
  private static GlobalChoreProvider.GlobalChoreProviderMultithreader batch_context = new GlobalChoreProvider.GlobalChoreProviderMultithreader();
  private static WorkItemCollection<MultithreadedCollectChoreContext<GlobalChoreProvider>.WorkBlock<GlobalChoreProvider.GlobalChoreProviderMultithreader>, GlobalChoreProvider.GlobalChoreProviderMultithreader> batch_work_items = new WorkItemCollection<MultithreadedCollectChoreContext<GlobalChoreProvider>.WorkBlock<GlobalChoreProvider.GlobalChoreProviderMultithreader>, GlobalChoreProvider.GlobalChoreProviderMultithreader>();

  public static void DestroyInstance() => GlobalChoreProvider.Instance = (GlobalChoreProvider) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    GlobalChoreProvider.Instance = this;
    this.clearableManager = new ClearableManager();
  }

  protected override void OnWorldRemoved(object data)
  {
    int num = ((Boxed<int>) data).value;
    List<FetchChore> chores;
    if (this.fetchMap.TryGetValue(ClusterManager.Instance.GetWorld(num).ParentWorldId, out chores))
      this.ClearWorldChores<FetchChore>(chores, num);
    base.OnWorldRemoved(data);
  }

  protected override void OnWorldParentChanged(object data)
  {
    if (!(data is WorldParentChangedEventArgs changedEventArgs) || changedEventArgs.lastParentId == (int) byte.MaxValue)
      return;
    base.OnWorldParentChanged(data);
    List<FetchChore> oldChores;
    if (!this.fetchMap.TryGetValue(changedEventArgs.lastParentId, out oldChores))
      return;
    List<FetchChore> newChores;
    if (!this.fetchMap.TryGetValue(changedEventArgs.world.ParentWorldId, out newChores))
      this.fetchMap[changedEventArgs.world.ParentWorldId] = newChores = new List<FetchChore>();
    this.TransferChores<FetchChore>(oldChores, newChores, changedEventArgs.world.ParentWorldId);
  }

  public override void AddChore(Chore chore)
  {
    if (chore is FetchChore fetchChore)
    {
      int myParentWorldId = fetchChore.gameObject.GetMyParentWorldId();
      List<FetchChore> fetchChoreList;
      if (!this.fetchMap.TryGetValue(myParentWorldId, out fetchChoreList))
        this.fetchMap[myParentWorldId] = fetchChoreList = new List<FetchChore>();
      chore.provider = (ChoreProvider) this;
      fetchChoreList.Add(fetchChore);
    }
    else
      base.AddChore(chore);
  }

  public override void RemoveChore(Chore chore)
  {
    if (chore is FetchChore fetchChore)
    {
      List<FetchChore> fetchChoreList;
      if (this.fetchMap.TryGetValue(fetchChore.gameObject.GetMyParentWorldId(), out fetchChoreList))
        fetchChoreList.Remove(fetchChore);
      chore.provider = (ChoreProvider) null;
    }
    else
      base.RemoveChore(chore);
  }

  public void UpdateFetches(Navigator navigator)
  {
    List<FetchChore> fetchChoreList = (List<FetchChore>) null;
    if (!this.fetchMap.TryGetValue(navigator.gameObject.GetMyParentWorldId(), out fetchChoreList))
      return;
    this.fetches.Clear();
    GlobalChoreProvider.Fetch fetch1;
    for (int index = fetchChoreList.Count - 1; index >= 0; --index)
    {
      FetchChore fetchChore = fetchChoreList[index];
      if (!((UnityEngine.Object) fetchChore.driver != (UnityEngine.Object) null) && (!((UnityEngine.Object) fetchChore.automatable != (UnityEngine.Object) null) || !fetchChore.automatable.GetAutomationOnly()))
      {
        if ((UnityEngine.Object) fetchChore.provider == (UnityEngine.Object) null)
        {
          fetchChore.Cancel("no provider");
          fetchChoreList[index] = fetchChoreList[fetchChoreList.Count - 1];
          fetchChoreList.RemoveAt(fetchChoreList.Count - 1);
        }
        else
        {
          Storage destination = fetchChore.destination;
          if (!((UnityEngine.Object) destination == (UnityEngine.Object) null))
          {
            int navigationCost = navigator.GetNavigationCost((IApproachable) destination);
            if (navigationCost != -1)
            {
              List<GlobalChoreProvider.Fetch> fetches = this.fetches;
              fetch1 = new GlobalChoreProvider.Fetch();
              fetch1.chore = fetchChore;
              fetch1.idsHash = fetchChore.tagsHash;
              fetch1.cost = navigationCost;
              fetch1.priority = fetchChore.masterPriority;
              fetch1.category = destination.fetchCategory;
              GlobalChoreProvider.Fetch fetch2 = fetch1;
              fetches.Add(fetch2);
            }
          }
        }
      }
    }
    if (this.fetches.Count > 0)
    {
      this.fetches.Sort((IComparer<GlobalChoreProvider.Fetch>) GlobalChoreProvider.Comparer);
      int index1 = 1;
      int index2 = 0;
      for (; index1 < this.fetches.Count; ++index1)
      {
        fetch1 = this.fetches[index2];
        if (!fetch1.IsBetterThan(this.fetches[index1]))
        {
          ++index2;
          this.fetches[index2] = this.fetches[index1];
        }
      }
      this.fetches.RemoveRange(index2 + 1, this.fetches.Count - index2 - 1);
    }
    this.clearableManager.CollectAndSortClearables(navigator);
  }

  public override void CollectChores(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded,
    List<Chore.Precondition.Context> failed_contexts)
  {
    base.CollectChores(consumer_state, succeeded, failed_contexts);
    this.clearableManager.CollectChores(this.fetches, consumer_state, succeeded, failed_contexts);
    if (this.fetches.Count > 48 /*0x30*/)
    {
      GlobalChoreProvider.batch_context.Setup(this, consumer_state);
      GlobalChoreProvider.batch_work_items.Reset(GlobalChoreProvider.batch_context);
      for (int start = 0; start < this.fetches.Count; start += 16 /*0x10*/)
        GlobalChoreProvider.batch_work_items.Add(new MultithreadedCollectChoreContext<GlobalChoreProvider>.WorkBlock<GlobalChoreProvider.GlobalChoreProviderMultithreader>(start, Math.Min(start + 16 /*0x10*/, this.fetches.Count)));
      GlobalJobManager.Run((IWorkItemCollection) GlobalChoreProvider.batch_work_items);
      GlobalChoreProvider.batch_context.Finish(succeeded, failed_contexts);
    }
    else
    {
      for (int index = 0; index < this.fetches.Count; ++index)
        this.fetches[index].chore.CollectChoresFromGlobalChoreProvider(consumer_state, succeeded, failed_contexts, false);
    }
  }

  public HandleVector<int>.Handle RegisterClearable(Clearable clearable)
  {
    return this.clearableManager.RegisterClearable(clearable);
  }

  public void UnregisterClearable(HandleVector<int>.Handle handle)
  {
    this.clearableManager.UnregisterClearable(handle);
  }

  protected override void OnLoadLevel()
  {
    base.OnLoadLevel();
    GlobalChoreProvider.Instance = (GlobalChoreProvider) null;
  }

  public void Render200ms(float dt) => this.UpdateStorageFetchableBits();

  private void UpdateStorageFetchableBits()
  {
    ChoreType storageFetch = Db.Get().ChoreTypes.StorageFetch;
    ChoreType foodFetch = Db.Get().ChoreTypes.FoodFetch;
    this.storageFetchableTags.Clear();
    List<int> worldIdsSorted = ClusterManager.Instance.GetWorldIDsSorted();
    for (int index1 = 0; index1 < worldIdsSorted.Count; ++index1)
    {
      List<FetchChore> fetchChoreList;
      if (this.fetchMap.TryGetValue(worldIdsSorted[index1], out fetchChoreList))
      {
        for (int index2 = 0; index2 < fetchChoreList.Count; ++index2)
        {
          FetchChore fetchChore = fetchChoreList[index2];
          if ((fetchChore.choreType == storageFetch || fetchChore.choreType == foodFetch) && (bool) (UnityEngine.Object) fetchChore.destination)
          {
            int cell = Grid.PosToCell((KMonoBehaviour) fetchChore.destination);
            if (MinionGroupProber.Get().IsReachable(cell, fetchChore.destination.GetOffsets(cell)))
              this.storageFetchableTags.UnionWith((IEnumerable<Tag>) fetchChore.tags);
          }
        }
      }
    }
  }

  public bool ClearableHasDestination(Pickupable pickupable)
  {
    return this.storageFetchableTags.Contains(pickupable.KPrefabID.PrefabTag);
  }

  public struct Fetch
  {
    public FetchChore chore;
    public int idsHash;
    public int cost;
    public PrioritySetting priority;
    public Storage.FetchCategory category;

    public bool IsBetterThan(GlobalChoreProvider.Fetch fetch)
    {
      if (this.category != fetch.category || this.idsHash != fetch.idsHash || this.chore.choreType != fetch.chore.choreType)
        return false;
      if (this.priority.priority_class > fetch.priority.priority_class)
        return true;
      if (this.priority.priority_class == fetch.priority.priority_class)
      {
        if (this.priority.priority_value > fetch.priority.priority_value)
          return true;
        if (this.priority.priority_value == fetch.priority.priority_value)
          return this.cost <= fetch.cost;
      }
      return false;
    }
  }

  private class GlobalChoreProviderMultithreader : 
    MultithreadedCollectChoreContext<GlobalChoreProvider>
  {
    public override void CollectChore(
      int index,
      List<Chore.Precondition.Context> succeed,
      List<Chore.Precondition.Context> incomplete,
      List<Chore.Precondition.Context> failed)
    {
      this.provider.fetches[index].chore.CollectChoresFromGlobalChoreProvider(this.consumerState, succeed, incomplete, failed, false);
    }
  }

  private class FetchComparer : IComparer<GlobalChoreProvider.Fetch>
  {
    public int Compare(GlobalChoreProvider.Fetch a, GlobalChoreProvider.Fetch b)
    {
      int num1 = b.priority.priority_class - a.priority.priority_class;
      if (num1 != 0)
        return num1;
      int num2 = b.priority.priority_value - a.priority.priority_value;
      return num2 != 0 ? num2 : a.cost - b.cost;
    }
  }

  private struct FindTopPriorityTask(int start, int end, List<Prioritizable> worldCollection) : 
    IWorkItem<object>
  {
    private int start = start;
    private int end = end;
    private List<Prioritizable> worldCollection = worldCollection;
    public bool found = false;
    public static bool abort;

    public void Run(object context, int threadIndex)
    {
      if (GlobalChoreProvider.FindTopPriorityTask.abort)
        return;
      for (int start = this.start; start != this.end && this.worldCollection.Count > start; ++start)
      {
        if (!((UnityEngine.Object) this.worldCollection[start] == (UnityEngine.Object) null) && this.worldCollection[start].IsTopPriority())
        {
          this.found = true;
          break;
        }
      }
      if (!this.found)
        return;
      GlobalChoreProvider.FindTopPriorityTask.abort = true;
    }
  }
}
