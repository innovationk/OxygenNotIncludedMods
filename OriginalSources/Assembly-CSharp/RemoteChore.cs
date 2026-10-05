// Decompiled with JetBrains decompiler
// Type: RemoteChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RemoteChore : WorkChore<RemoteWorkTerminal>
{
  private static Chore.Precondition RemoteTerminalHasDock = new Chore.Precondition()
  {
    id = "RemoteDockAssigned",
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_NO_REMOTE_DOCK,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (Object) ((RemoteWorkTerminal) data).CurrentDock != (Object) null),
    canExecuteOnAnyThread = true
  };
  private static Chore.Precondition RemoteDockOperational = new Chore.Precondition()
  {
    id = nameof (RemoteDockOperational),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_DOCK_INOPERABLE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      RemoteWorkTerminal remoteWorkTerminal = (RemoteWorkTerminal) data;
      return (Object) remoteWorkTerminal.CurrentDock != (Object) null && remoteWorkTerminal.CurrentDock.IsOperational;
    }),
    canExecuteOnAnyThread = true
  };
  private static Chore.Precondition RemoteDockHasWorker = new Chore.Precondition()
  {
    id = "RemoteDockHasAvailableWorker",
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_NO_REMOTE_WORKER,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      RemoteWorkerDock currentDock = ((RemoteWorkTerminal) data).CurrentDock;
      return !((Object) currentDock == (Object) null) && currentDock.HasWorker() && currentDock.RemoteWorker.Available && !currentDock.RemoteWorker.RequiresMaintnence;
    }),
    canExecuteOnAnyThread = true
  };
  private static Chore.Precondition RemoteDockAvailable = new Chore.Precondition()
  {
    id = nameof (RemoteDockAvailable),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_DOCK_UNAVAILABLE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      RemoteWorkTerminal terminal = (RemoteWorkTerminal) data;
      RemoteWorkerDock currentDock = terminal.CurrentDock;
      return !((Object) currentDock == (Object) null) && currentDock.AvailableForWorkBy(terminal);
    }),
    canExecuteOnAnyThread = true
  };
  private static Chore.Precondition RemoteChoreSubchorePreconditions = new Chore.Precondition()
  {
    id = "RemoteChorePreconditionsMet",
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_SUBCHORE_PRECONDITIONS,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if (context.data == null)
        return true;
      Chore.Precondition.Context data1 = (Chore.Precondition.Context) context.data;
      if (data1.failedPreconditionId != -1)
        return false;
      data1.RunPreconditions();
      return data1.failedPreconditionId == -1;
    }),
    canExecuteOnAnyThread = false
  };
  private RemoteWorkTerminal terminal;
  private Chore active_subchore;

  public RemoteChore(RemoteWorkTerminal terminal)
    : base(Db.Get().ChoreTypes.RemoteOperate, (IStateMachineTarget) terminal)
  {
    this.terminal = terminal;
    this.AddPrecondition(RemoteChore.RemoteTerminalHasDock, (object) terminal);
    this.AddPrecondition(RemoteChore.RemoteDockHasWorker, (object) terminal);
    this.AddPrecondition(RemoteChore.RemoteDockAvailable, (object) terminal);
    this.AddPrecondition(RemoteChore.RemoteChoreSubchorePreconditions, (object) terminal);
    this.AddPrecondition(RemoteChore.RemoteDockOperational, (object) terminal);
  }

  public override void CollectChores(
    ChoreConsumerState duplicantState,
    List<Chore.Precondition.Context> succeeded_contexts,
    List<Chore.Precondition.Context> incomplete_contexts,
    List<Chore.Precondition.Context> failed_contexts,
    bool is_attempting_override)
  {
    Chore.Precondition.Context context1 = new Chore.Precondition.Context((Chore) this, duplicantState, is_attempting_override);
    context1.RunPreconditions();
    if (!context1.IsComplete())
    {
      ListPool<Chore.Precondition.Context, Chore.Precondition>.PooledList succeeded_contexts1 = ListPool<Chore.Precondition.Context, Chore.Precondition>.Allocate();
      ListPool<Chore.Precondition.Context, Chore.Precondition>.PooledList failed_contexts1 = ListPool<Chore.Precondition.Context, Chore.Precondition>.Allocate();
      ListPool<Chore.Precondition.Context, Chore.Precondition>.PooledList incomplete_contexts1 = ListPool<Chore.Precondition.Context, Chore.Precondition>.Allocate();
      this.terminal.CurrentDock?.CollectChores(duplicantState, (List<Chore.Precondition.Context>) succeeded_contexts1, (List<Chore.Precondition.Context>) incomplete_contexts1, (List<Chore.Precondition.Context>) failed_contexts1, is_attempting_override);
      foreach (Chore.Precondition.Context context2 in (List<Chore.Precondition.Context>) succeeded_contexts1)
      {
        context1.data = (object) context2;
        context1.SetPriority(context2.chore);
        incomplete_contexts.Add(context1);
      }
      foreach (Chore.Precondition.Context context3 in (List<Chore.Precondition.Context>) incomplete_contexts1)
      {
        context1.data = (object) context3;
        context1.SetPriority(context3.chore);
        incomplete_contexts.Add(context1);
      }
      List<Chore.PreconditionInstance> preconditions = context1.chore.GetPreconditions();
      for (int index = 0; index < preconditions.Count; ++index)
      {
        if (preconditions[index].condition.id == RemoteChore.RemoteChoreSubchorePreconditions.id)
        {
          context1.failedPreconditionId = index;
          break;
        }
      }
      foreach (Chore.Precondition.Context context4 in (List<Chore.Precondition.Context>) failed_contexts1)
      {
        context1.data = (object) context4;
        context1.SetPriority(context4.chore);
        failed_contexts.Add(context1);
      }
      succeeded_contexts1.Recycle();
      failed_contexts1.Recycle();
      incomplete_contexts1.Recycle();
    }
    else if (context1.IsSuccess())
    {
      ListPool<Chore.Precondition.Context, Chore.Precondition>.PooledList succeeded_contexts2 = ListPool<Chore.Precondition.Context, Chore.Precondition>.Allocate();
      ListPool<Chore.Precondition.Context, Chore.Precondition>.PooledList failed_contexts2 = ListPool<Chore.Precondition.Context, Chore.Precondition>.Allocate();
      this.terminal.CurrentDock?.CollectChores(duplicantState, (List<Chore.Precondition.Context>) succeeded_contexts2, (List<Chore.Precondition.Context>) null, (List<Chore.Precondition.Context>) failed_contexts2, is_attempting_override);
      foreach (Chore.Precondition.Context context5 in (List<Chore.Precondition.Context>) succeeded_contexts2)
      {
        context1.data = (object) context5;
        context1.SetPriority(context5.chore);
        succeeded_contexts.Add(context1);
      }
      foreach (Chore.Precondition.Context context6 in (List<Chore.Precondition.Context>) failed_contexts2)
      {
        context1.data = (object) context6;
        context1.SetPriority(context6.chore);
        failed_contexts.Add(context1);
      }
      succeeded_contexts2.Recycle();
      failed_contexts2.Recycle();
    }
    else
      failed_contexts.Add(context1);
  }

  public override void PrepareChore(ref Chore.Precondition.Context context)
  {
    base.PrepareChore(ref context);
    DebugUtil.Assert(this.active_subchore == null);
    this.active_subchore = ((Chore.Precondition.Context) context.data).chore;
    this.terminal.CurrentDock?.SetNextChore(this.terminal, (Chore.Precondition.Context) context.data);
  }

  protected override void End(string reason)
  {
    if (this.active_subchore != null && (Object) this.active_subchore.driver != (Object) null && !this.active_subchore.driver.HasChore())
      this.active_subchore.Reserve((ChoreDriver) null);
    this.active_subchore = (Chore) null;
    base.End(reason);
    if (!((Object) this.terminal.worker != (Object) null))
      return;
    this.terminal.StopWork(this.terminal.worker, true);
  }
}
