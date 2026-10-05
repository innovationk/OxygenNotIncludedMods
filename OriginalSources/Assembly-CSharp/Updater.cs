// Decompiled with JetBrains decompiler
// Type: Updater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public readonly struct Updater(Func<float, UpdaterResult> fn) : IEnumerator
{
  public readonly Func<float, UpdaterResult> fn = fn;

  public UpdaterResult Internal_Update(float deltaTime) => this.fn(deltaTime);

  object IEnumerator.Current => (object) null;

  bool IEnumerator.MoveNext() => this.fn(Updater.GetDeltaTime()) == UpdaterResult.NotComplete;

  void IEnumerator.Reset()
  {
  }

  public static implicit operator Updater(Promise promise)
  {
    return Updater.Until((Func<bool>) (() => promise.IsResolved));
  }

  public static Updater Until(Func<bool> fn)
  {
    return new Updater((Func<float, UpdaterResult>) (dt => !fn() ? UpdaterResult.NotComplete : UpdaterResult.Complete));
  }

  public static Updater While(Func<bool> isTrueFn)
  {
    return new Updater((Func<float, UpdaterResult>) (dt => !isTrueFn() ? UpdaterResult.Complete : UpdaterResult.NotComplete));
  }

  public static Updater While(Func<bool> isTrueFn, Func<Updater> getUpdaterWhileNotTrueFn)
  {
    Updater whileNotTrueUpdater = Updater.None();
    return new Updater((Func<float, UpdaterResult>) (dt =>
    {
      if (whileNotTrueUpdater.Internal_Update(dt) == UpdaterResult.Complete)
      {
        if (!isTrueFn())
          return UpdaterResult.Complete;
        whileNotTrueUpdater = getUpdaterWhileNotTrueFn();
      }
      return UpdaterResult.NotComplete;
    }));
  }

  public static Updater None()
  {
    return new Updater((Func<float, UpdaterResult>) (dt => UpdaterResult.Complete));
  }

  public static Updater WaitOneFrame() => Updater.WaitFrames(1);

  public static Updater WaitFrames(int framesToWait)
  {
    int frame = 0;
    return new Updater((Func<float, UpdaterResult>) (dt =>
    {
      ++frame;
      return framesToWait <= frame ? UpdaterResult.Complete : UpdaterResult.NotComplete;
    }));
  }

  public static Updater WaitForSeconds(float secondsToWait)
  {
    float currentSeconds = 0.0f;
    return new Updater((Func<float, UpdaterResult>) (dt =>
    {
      currentSeconds += dt;
      return (double) secondsToWait <= (double) currentSeconds ? UpdaterResult.Complete : UpdaterResult.NotComplete;
    }));
  }

  public static Updater Ease(
    Action<float> fn,
    float from,
    float to,
    float duration,
    Easing.EasingFn easing = null,
    float delay = -1f)
  {
    return Updater.GenericEase<float>(fn, new Func<float, float, float, float>(Mathf.LerpUnclamped), easing, from, to, duration, delay);
  }

  public static Updater Ease(
    Action<Vector2> fn,
    Vector2 from,
    Vector2 to,
    float duration,
    Easing.EasingFn easing = null,
    float delay = -1f)
  {
    return Updater.GenericEase<Vector2>(fn, new Func<Vector2, Vector2, float, Vector2>(Vector2.LerpUnclamped), easing, from, to, duration, delay);
  }

  public static Updater Ease(
    Action<Vector3> fn,
    Vector3 from,
    Vector3 to,
    float duration,
    Easing.EasingFn easing = null,
    float delay = -1f)
  {
    return Updater.GenericEase<Vector3>(fn, new Func<Vector3, Vector3, float, Vector3>(Vector3.LerpUnclamped), easing, from, to, duration, delay);
  }

  public static Updater GenericEase<T>(
    Action<T> useFn,
    Func<T, T, float, T> interpolateFn,
    Easing.EasingFn easingFn,
    T from,
    T to,
    float duration,
    float delay)
  {
    if (easingFn == null)
      easingFn = Easing.SmoothStep;
    float currentSeconds = 0.0f;
    UseKeyframeAt(0.0f);
    Updater updater = new Updater((Func<float, UpdaterResult>) (dt =>
    {
      currentSeconds += dt;
      if ((double) currentSeconds < (double) duration)
      {
        UseKeyframeAt(currentSeconds / duration);
        return UpdaterResult.NotComplete;
      }
      UseKeyframeAt(1f);
      return UpdaterResult.Complete;
    }));
    if ((double) delay <= 0.0)
      return updater;
    return Updater.Series(Updater.WaitForSeconds(delay), updater);

    void UseKeyframeAt(float progress01) => useFn(interpolateFn(from, to, easingFn(progress01)));
  }

  public static Updater Do(System.Action fn)
  {
    return new Updater((Func<float, UpdaterResult>) (dt =>
    {
      fn();
      return UpdaterResult.Complete;
    }));
  }

  public static Updater Do(Func<Updater> fn)
  {
    bool didInitalize = false;
    Updater target = new Updater();
    return new Updater((Func<float, UpdaterResult>) (dt =>
    {
      if (!didInitalize)
      {
        target = fn();
        didInitalize = true;
      }
      return target.Internal_Update(dt);
    }));
  }

  public static Updater Loop(params Func<Updater>[] makeUpdaterFns)
  {
    return Updater.Internal_Loop((Option<int>) Option.None, makeUpdaterFns);
  }

  public static Updater Loop(int loopCount, params Func<Updater>[] makeUpdaterFns)
  {
    return Updater.Internal_Loop((Option<int>) loopCount, makeUpdaterFns);
  }

  public static Updater Internal_Loop(Option<int> limitLoopCount, Func<Updater>[] makeUpdaterFns)
  {
    if (makeUpdaterFns == null || makeUpdaterFns.Length == 0)
      return Updater.None();
    int completedLoopCount = 0;
    int currentIndex = 0;
    Updater currentUpdater = makeUpdaterFns[currentIndex]();
    return new Updater((Func<float, UpdaterResult>) (dt =>
    {
      if (currentUpdater.Internal_Update(dt) == UpdaterResult.Complete)
      {
        ++currentIndex;
        if (currentIndex >= makeUpdaterFns.Length)
        {
          currentIndex -= makeUpdaterFns.Length;
          ++completedLoopCount;
          if (limitLoopCount.IsSome() && completedLoopCount >= limitLoopCount.Unwrap())
            return UpdaterResult.Complete;
        }
        currentUpdater = makeUpdaterFns[currentIndex]();
      }
      return UpdaterResult.NotComplete;
    }));
  }

  public static Updater Parallel(params Updater[] updaters)
  {
    bool[] isCompleted = new bool[updaters.Length];
    return new Updater((Func<float, UpdaterResult>) (dt =>
    {
      bool flag = false;
      for (int index = 0; index < updaters.Length; ++index)
      {
        if (!isCompleted[index])
        {
          if (updaters[index].Internal_Update(dt) == UpdaterResult.Complete)
            isCompleted[index] = true;
          else
            flag = true;
        }
      }
      return !flag ? UpdaterResult.Complete : UpdaterResult.NotComplete;
    }));
  }

  public static Updater Series(params Updater[] updaters)
  {
    int i = 0;
    return new Updater((Func<float, UpdaterResult>) (dt =>
    {
      if (i == updaters.Length)
        return UpdaterResult.Complete;
      if (updaters[i].Internal_Update(dt) == UpdaterResult.Complete)
        ++i;
      return i == updaters.Length ? UpdaterResult.Complete : UpdaterResult.NotComplete;
    }));
  }

  public static Promise RunRoutine(MonoBehaviour monoBehaviour, IEnumerator coroutine)
  {
    Promise willComplete = new Promise();
    monoBehaviour.StartCoroutine(Routine());
    return willComplete;

    IEnumerator Routine()
    {
      yield return (object) coroutine;
      willComplete.Resolve();
    }
  }

  public static Promise Run(MonoBehaviour monoBehaviour, params Updater[] updaters)
  {
    return Updater.Run(monoBehaviour, Updater.Series(updaters));
  }

  public static Promise Run(MonoBehaviour monoBehaviour, Updater updater)
  {
    Promise willComplete = new Promise();
    monoBehaviour.StartCoroutine(Routine());
    return willComplete;

    IEnumerator Routine()
    {
      while (updater.Internal_Update(Updater.GetDeltaTime()) == UpdaterResult.NotComplete)
        yield return (object) null;
      willComplete.Resolve();
    }
  }

  public static float GetDeltaTime() => Time.unscaledDeltaTime;
}
