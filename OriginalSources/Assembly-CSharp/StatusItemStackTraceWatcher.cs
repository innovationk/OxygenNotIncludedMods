// Decompiled with JetBrains decompiler
// Type: StatusItemStackTraceWatcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class StatusItemStackTraceWatcher : IDisposable
{
  private Dictionary<Guid, StackTrace> entryIdToStackTraceMap = new Dictionary<Guid, StackTrace>();
  private Option<StatusItemGroup> currentTarget;
  private bool shouldWatch;
  private System.Action onCleanup;

  public bool GetShouldWatch() => this.shouldWatch;

  public void SetShouldWatch(bool shouldWatch)
  {
    if (this.shouldWatch == shouldWatch)
      return;
    this.shouldWatch = shouldWatch;
    this.Refresh();
  }

  public Option<StatusItemGroup> GetTarget() => this.currentTarget;

  public void SetTarget(Option<StatusItemGroup> nextTarget)
  {
    if (this.currentTarget.IsNone() && nextTarget.IsNone() || this.currentTarget.IsSome() && nextTarget.IsSome() && this.currentTarget.Unwrap() == nextTarget.Unwrap())
      return;
    this.currentTarget = nextTarget;
    this.Refresh();
  }

  private void Refresh()
  {
    if (this.onCleanup != null)
    {
      System.Action onCleanup = this.onCleanup;
      if (onCleanup != null)
        onCleanup();
      this.onCleanup = (System.Action) null;
    }
    if (!this.shouldWatch || !this.currentTarget.IsSome())
      return;
    StatusItemGroup target = this.currentTarget.Unwrap();
    Action<StatusItemGroup.Entry, StatusItemCategory> onAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>) ((entry, category) => this.entryIdToStackTraceMap[entry.id] = new StackTrace(true));
    target.OnAddStatusItem += onAddStatusItem;
    this.onCleanup += (System.Action) (() => target.OnAddStatusItem -= onAddStatusItem);
    StatusItemStackTraceWatcher.StatusItemStackTraceWatcher_OnDestroyListenerMB destroyListener = this.currentTarget.Unwrap().gameObject.AddOrGet<StatusItemStackTraceWatcher.StatusItemStackTraceWatcher_OnDestroyListenerMB>();
    destroyListener.owner = this;
    this.onCleanup += (System.Action) (() =>
    {
      if (destroyListener.IsNullOrDestroyed())
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) destroyListener);
    });
    this.onCleanup += (System.Action) (() => this.entryIdToStackTraceMap.Clear());
  }

  public bool GetStackTraceForEntry(StatusItemGroup.Entry entry, out StackTrace stackTrace)
  {
    return this.entryIdToStackTraceMap.TryGetValue(entry.id, out stackTrace);
  }

  public void Dispose()
  {
    if (this.onCleanup == null)
      return;
    System.Action onCleanup = this.onCleanup;
    if (onCleanup != null)
      onCleanup();
    this.onCleanup = (System.Action) null;
  }

  public class StatusItemStackTraceWatcher_OnDestroyListenerMB : MonoBehaviour
  {
    public StatusItemStackTraceWatcher owner;

    private void OnDestroy()
    {
      if (((this.owner != null ? 1 : 0) & (!this.owner.currentTarget.IsSome() ? (false ? 1 : 0) : ((UnityEngine.Object) this.owner.currentTarget.Unwrap().gameObject == (UnityEngine.Object) this.gameObject ? 1 : 0))) == 0)
        return;
      this.owner.SetTarget((Option<StatusItemGroup>) Option.None);
    }
  }
}
