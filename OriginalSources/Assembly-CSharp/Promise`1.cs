// Decompiled with JetBrains decompiler
// Type: Promise`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;

#nullable disable
public class Promise<T> : IEnumerator
{
  private Promise promise = new Promise();
  private T result;

  public bool IsResolved => this.promise.IsResolved;

  public Promise(Action<Action<T>> fn) => fn((Action<T>) (value => this.Resolve(value)));

  public Promise()
  {
  }

  public void EnsureResolved(T value)
  {
    this.result = value;
    this.promise.EnsureResolved();
  }

  public void Resolve(T value)
  {
    this.result = value;
    this.promise.Resolve();
  }

  public Promise<T> Then(Action<T> fn)
  {
    this.promise.Then((System.Action) (() => fn(this.result)));
    return this;
  }

  public Promise ThenWait(Func<Promise> fn) => this.promise.ThenWait(fn);

  public Promise<T> ThenWait(Func<Promise<T>> fn) => this.promise.ThenWait<T>(fn);

  object IEnumerator.Current => (object) null;

  bool IEnumerator.MoveNext() => !this.promise.IsResolved;

  void IEnumerator.Reset()
  {
  }
}
