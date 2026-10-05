// Decompiled with JetBrains decompiler
// Type: CoroutineRunner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class CoroutineRunner : MonoBehaviour
{
  public Promise Run(IEnumerator routine)
  {
    return new Promise((Action<System.Action>) (resolve => this.StartCoroutine(this.RunRoutine(routine, resolve))));
  }

  public (Promise, System.Action) RunCancellable(IEnumerator routine)
  {
    Promise promise = new Promise();
    Coroutine coroutine = this.StartCoroutine(this.RunRoutine(routine, new System.Action(promise.Resolve)));
    System.Action action = (System.Action) (() => this.StopCoroutine(coroutine));
    return (promise, action);
  }

  private IEnumerator RunRoutine(IEnumerator routine, System.Action completedCallback)
  {
    yield return (object) routine;
    completedCallback();
  }

  public static CoroutineRunner Create()
  {
    return new GameObject(nameof (CoroutineRunner)).AddComponent<CoroutineRunner>();
  }

  public static Promise RunOne(IEnumerator routine)
  {
    CoroutineRunner runner = CoroutineRunner.Create();
    return runner.Run(routine).Then((System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) runner.gameObject)));
  }
}
