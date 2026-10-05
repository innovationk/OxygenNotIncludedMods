// Decompiled with JetBrains decompiler
// Type: Promise
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;

#nullable disable
public class Promise : IEnumerator
{
  private System.Action on_complete;
  private bool m_is_resolved;
  private static Promise m_instant = new Promise();

  public bool IsResolved => this.m_is_resolved;

  public Promise(Action<System.Action> fn) => fn((System.Action) (() => this.Resolve()));

  public Promise()
  {
  }

  public void EnsureResolved()
  {
    if (this.IsResolved)
      return;
    this.Resolve();
  }

  public void Resolve()
  {
    DebugUtil.Assert(!this.m_is_resolved, "Can only resolve a promise once");
    this.m_is_resolved = true;
    if (this.on_complete == null)
      return;
    this.on_complete();
    this.on_complete = (System.Action) null;
  }

  public Promise Then(System.Action callback)
  {
    if (this.m_is_resolved)
      callback();
    else
      this.on_complete += callback;
    return this;
  }

  public Promise ThenWait(Func<Promise> callback)
  {
    return this.m_is_resolved ? callback() : new Promise((Action<System.Action>) (resolve => this.on_complete += (System.Action) (() => callback().Then(resolve))));
  }

  public Promise<T> ThenWait<T>(Func<Promise<T>> callback)
  {
    return this.m_is_resolved ? callback() : new Promise<T>((Action<Action<T>>) (resolve => this.on_complete += (System.Action) (() => callback().Then(resolve))));
  }

  object IEnumerator.Current => (object) null;

  bool IEnumerator.MoveNext() => !this.IsResolved;

  void IEnumerator.Reset()
  {
  }

  static Promise() => Promise.m_instant.Resolve();

  public static Promise Instant => Promise.m_instant;

  public static Promise Fail => new Promise();

  public static Promise All(params Promise[] promises)
  {
    if (promises == null || promises.Length == 0)
      return Promise.Instant;
    Promise all_resolved_promise = new Promise();
    foreach (Promise promise in promises)
      promise.Then(new System.Action(TryResolve));
    return all_resolved_promise;

    void TryResolve()
    {
      if (all_resolved_promise.IsResolved)
        return;
      foreach (Promise promise in promises)
      {
        if (!promise.IsResolved)
          return;
      }
      all_resolved_promise.Resolve();
    }
  }

  public static Promise Chain(params Func<Promise>[] make_promise_fns)
  {
    Promise all_resolve_promise = new Promise();
    int current_promise_fn_index = 0;
    TryNext();
    return all_resolve_promise;

    void TryNext()
    {
      if (DidAll())
      {
        all_resolve_promise.Resolve();
      }
      else
      {
        Promise promise = make_promise_fns[current_promise_fn_index]();
        ++current_promise_fn_index;
        System.Action callback = new System.Action(TryNext);
        promise.Then(callback);
      }
    }

    bool DidAll()
    {
      return make_promise_fns == null || make_promise_fns.Length == 0 || make_promise_fns.Length <= current_promise_fn_index;
    }
  }
}
