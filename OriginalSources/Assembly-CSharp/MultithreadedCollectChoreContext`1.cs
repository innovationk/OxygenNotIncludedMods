// Decompiled with JetBrains decompiler
// Type: MultithreadedCollectChoreContext`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public abstract class MultithreadedCollectChoreContext<ProviderType>
{
  public ProviderType provider;
  public ChoreConsumerState consumerState;
  public ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[] succeeded;
  public ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[] failed;
  public ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[] incomplete;

  public void Setup(ProviderType provider, ChoreConsumerState consumerState)
  {
    this.provider = provider;
    this.consumerState = consumerState;
    if (this.succeeded != null && this.succeeded.Length == GlobalJobManager.ThreadCount)
      return;
    this.SetupThreadContext();
  }

  private void SetupThreadContext()
  {
    if (this.succeeded != null)
      this.TearDownThreadContext();
    int threadCount = GlobalJobManager.ThreadCount;
    this.succeeded = new ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[threadCount];
    this.failed = new ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[threadCount];
    this.incomplete = new ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[threadCount];
    for (int index = 0; index < threadCount; ++index)
    {
      this.succeeded[index] = ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.Allocate();
      this.failed[index] = ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.Allocate();
      this.incomplete[index] = ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.Allocate();
    }
  }

  private void TearDownThreadContext()
  {
    int threadCount = GlobalJobManager.ThreadCount;
    for (int index = 0; index < threadCount; ++index)
    {
      this.succeeded[index].Recycle();
      this.failed[index].Recycle();
      this.incomplete[index].Recycle();
    }
    this.succeeded = (ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[]) null;
    this.failed = (ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[]) null;
    this.incomplete = (ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[]) null;
  }

  public void Finish(List<Chore.Precondition.Context> pass, List<Chore.Precondition.Context> fail)
  {
    int threadCount = GlobalJobManager.ThreadCount;
    for (int index = 0; index < threadCount; ++index)
    {
      pass.AddRange((IEnumerable<Chore.Precondition.Context>) this.succeeded[index]);
      this.succeeded[index].Clear();
      fail.AddRange((IEnumerable<Chore.Precondition.Context>) this.failed[index]);
      this.failed[index].Clear();
      foreach (Chore.Precondition.Context context in (List<Chore.Precondition.Context>) this.incomplete[index])
      {
        context.FinishPreconditions();
        if (context.IsSuccess())
          pass.Add(context);
        else
          fail.Add(context);
      }
      this.incomplete[index].Clear();
    }
    this.provider = default (ProviderType);
    this.consumerState = (ChoreConsumerState) null;
  }

  public abstract void CollectChore(
    int index,
    List<Chore.Precondition.Context> succeed,
    List<Chore.Precondition.Context> incomplete,
    List<Chore.Precondition.Context> failed);

  public void DefaultCollectChore(int index, int threadIndex)
  {
    this.CollectChore(index, (List<Chore.Precondition.Context>) this.succeeded[threadIndex], (List<Chore.Precondition.Context>) this.incomplete[threadIndex], (List<Chore.Precondition.Context>) this.failed[threadIndex]);
  }

  public struct WorkBlock<Parent>(int start, int end) : IWorkItem<Parent> where Parent : MultithreadedCollectChoreContext<ProviderType>
  {
    private int start = start;
    private int end = end;

    void IWorkItem<Parent>.Run(Parent shared_data, int threadIndex)
    {
      for (int start = this.start; start < this.end; ++start)
        shared_data.DefaultCollectChore(start, threadIndex);
    }
  }
}
