// Decompiled with JetBrains decompiler
// Type: ChoreProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ChoreProvider")]
public class ChoreProvider : KMonoBehaviour
{
  public Dictionary<int, List<Chore>> choreWorldMap = new Dictionary<int, List<Chore>>();
  private int worldParentChangedHandle = -1;
  private int minionMigrationHandle = -1;
  private int enitityMigrationHandle = -1;
  private int worldRemovedHandle = -1;
  private static ChoreProvider.ChoreProviderCollectContext batch_context = new ChoreProvider.ChoreProviderCollectContext();
  private static WorkItemCollection<MultithreadedCollectChoreContext<List<Chore>>.WorkBlock<ChoreProvider.ChoreProviderCollectContext>, ChoreProvider.ChoreProviderCollectContext> batch_work_items = new WorkItemCollection<MultithreadedCollectChoreContext<List<Chore>>.WorkBlock<ChoreProvider.ChoreProviderCollectContext>, ChoreProvider.ChoreProviderCollectContext>();

  public string Name { get; private set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.worldParentChangedHandle = Game.Instance.Subscribe(880851192, new Action<object>(this.OnWorldParentChanged));
    this.minionMigrationHandle = Game.Instance.Subscribe(586301400, new Action<object>(this.OnMinionMigrated));
    this.enitityMigrationHandle = Game.Instance.Subscribe(1142724171, new Action<object>(this.OnEntityMigrated));
  }

  protected override void OnSpawn()
  {
    if ((UnityEngine.Object) ClusterManager.Instance != (UnityEngine.Object) null)
      this.worldRemovedHandle = ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
    base.OnSpawn();
    this.Name = this.name;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Game.Instance.Unsubscribe(ref this.worldParentChangedHandle);
    Game.Instance.Unsubscribe(ref this.minionMigrationHandle);
    Game.Instance.Unsubscribe(ref this.enitityMigrationHandle);
    if (!((UnityEngine.Object) ClusterManager.Instance != (UnityEngine.Object) null))
      return;
    ClusterManager.Instance.Unsubscribe(ref this.worldRemovedHandle);
  }

  protected virtual void OnWorldRemoved(object data)
  {
    int num = ((Boxed<int>) data).value;
    List<Chore> chores;
    if (!this.choreWorldMap.TryGetValue(ClusterManager.Instance.GetWorld(num).ParentWorldId, out chores))
      return;
    this.ClearWorldChores<Chore>(chores, num);
  }

  protected virtual void OnWorldParentChanged(object data)
  {
    List<Chore> oldChores;
    if (((!(data is WorldParentChangedEventArgs changedEventArgs) ? 0 : (changedEventArgs.lastParentId != (int) byte.MaxValue ? 1 : 0)) == 0 ? 0 : (changedEventArgs.lastParentId != changedEventArgs.world.ParentWorldId ? 1 : 0)) == 0 || !this.choreWorldMap.TryGetValue(changedEventArgs.lastParentId, out oldChores))
      return;
    List<Chore> newChores;
    if (!this.choreWorldMap.TryGetValue(changedEventArgs.world.ParentWorldId, out newChores))
      this.choreWorldMap[changedEventArgs.world.ParentWorldId] = newChores = new List<Chore>();
    this.TransferChores<Chore>(oldChores, newChores, changedEventArgs.world.ParentWorldId);
  }

  protected virtual void OnEntityMigrated(object data)
  {
    List<Chore> oldChores;
    if (((!(data is MigrationEventArgs migrationEventArgs) ? 0 : ((UnityEngine.Object) migrationEventArgs.entity == (UnityEngine.Object) this.gameObject ? 1 : 0)) == 0 ? 0 : (migrationEventArgs.prevWorldId != migrationEventArgs.targetWorldId ? 1 : 0)) == 0 || !this.choreWorldMap.TryGetValue(migrationEventArgs.prevWorldId, out oldChores))
      return;
    List<Chore> newChores;
    if (!this.choreWorldMap.TryGetValue(migrationEventArgs.targetWorldId, out newChores))
      this.choreWorldMap[migrationEventArgs.targetWorldId] = newChores = new List<Chore>();
    this.TransferChores<Chore>(oldChores, newChores, migrationEventArgs.targetWorldId);
  }

  protected virtual void OnMinionMigrated(object data)
  {
    List<Chore> oldChores;
    if (((!(data is MinionMigrationEventArgs migrationEventArgs) ? 0 : ((UnityEngine.Object) migrationEventArgs.minionId.gameObject == (UnityEngine.Object) this.gameObject ? 1 : 0)) == 0 ? 0 : (migrationEventArgs.prevWorldId != migrationEventArgs.targetWorldId ? 1 : 0)) == 0 || !this.choreWorldMap.TryGetValue(migrationEventArgs.prevWorldId, out oldChores))
      return;
    List<Chore> newChores;
    if (!this.choreWorldMap.TryGetValue(migrationEventArgs.targetWorldId, out newChores))
      this.choreWorldMap[migrationEventArgs.targetWorldId] = newChores = new List<Chore>();
    this.TransferChores<Chore>(oldChores, newChores, migrationEventArgs.targetWorldId);
  }

  protected void TransferChores<T>(List<T> oldChores, List<T> newChores, int transferId) where T : Chore
  {
    int index1 = oldChores.Count - 1;
    for (int index2 = index1; index2 >= 0; --index2)
    {
      T oldChore = oldChores[index2];
      if (oldChore.isNull)
        DebugUtil.DevLogError($"[{oldChore.GetType().Name}] {oldChore.GetReportName((string) null)} has no target");
      else if (oldChore.gameObject.GetMyParentWorldId() == transferId)
      {
        newChores.Add(oldChore);
        oldChores[index2] = oldChores[index1];
        oldChores.RemoveAt(index1--);
      }
    }
  }

  protected void ClearWorldChores<T>(List<T> chores, int worldId) where T : Chore
  {
    int index1 = chores.Count - 1;
    for (int index2 = index1; index2 >= 0; --index2)
    {
      if (chores[index2].gameObject.GetMyWorldId() == worldId)
      {
        chores[index2] = chores[index1];
        chores.RemoveAt(index1--);
      }
    }
  }

  public virtual void AddChore(Chore chore)
  {
    chore.provider = this;
    List<Chore> choreList = (List<Chore>) null;
    int myParentWorldId = chore.gameObject.GetMyParentWorldId();
    if (!this.choreWorldMap.TryGetValue(myParentWorldId, out choreList))
      this.choreWorldMap[myParentWorldId] = choreList = new List<Chore>();
    choreList.Add(chore);
  }

  public virtual void RemoveChore(Chore chore)
  {
    if (chore == null)
      return;
    chore.provider = (ChoreProvider) null;
    List<Chore> choreList = (List<Chore>) null;
    if (!this.choreWorldMap.TryGetValue(chore.gameObject.GetMyParentWorldId(), out choreList))
      return;
    choreList.Remove(chore);
  }

  public virtual void CollectChores(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded,
    List<Chore.Precondition.Context> failed_contexts)
  {
    List<Chore> provider = (List<Chore>) null;
    if (!this.choreWorldMap.TryGetValue(consumer_state.gameObject.GetMyParentWorldId(), out provider))
      return;
    for (int index = provider.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) provider[index].provider == (UnityEngine.Object) null)
      {
        provider[index].Cancel("no provider");
        provider[index] = provider[provider.Count - 1];
        provider.RemoveAt(provider.Count - 1);
      }
    }
    int num = 48 /*0x30*/;
    if (provider.Count > num)
    {
      ChoreProvider.batch_context.Setup(provider, consumer_state);
      ChoreProvider.batch_work_items.Reset(ChoreProvider.batch_context);
      for (int start = 0; start < provider.Count; start += 16 /*0x10*/)
        ChoreProvider.batch_work_items.Add(new MultithreadedCollectChoreContext<List<Chore>>.WorkBlock<ChoreProvider.ChoreProviderCollectContext>(start, Math.Min(start + 16 /*0x10*/, provider.Count)));
      GlobalJobManager.Run((IWorkItemCollection) ChoreProvider.batch_work_items);
      ChoreProvider.batch_context.Finish(succeeded, failed_contexts);
    }
    else
    {
      foreach (Chore chore in provider)
        chore.CollectChores(consumer_state, succeeded, failed_contexts, false);
    }
  }

  private class ChoreProviderCollectContext : MultithreadedCollectChoreContext<List<Chore>>
  {
    public override void CollectChore(
      int index,
      List<Chore.Precondition.Context> succeed,
      List<Chore.Precondition.Context> incomplete,
      List<Chore.Precondition.Context> failed)
    {
      this.provider[index].CollectChores(this.consumerState, succeed, incomplete, failed, false);
    }
  }
}
