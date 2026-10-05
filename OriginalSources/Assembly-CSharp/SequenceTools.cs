// Decompiled with JetBrains decompiler
// Type: SequenceTools
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public static class SequenceTools
{
  public static WaitUntil Interpolate(
    this MonoBehaviour owner,
    Action<float> action,
    float duration,
    System.Action then = null)
  {
    return owner.Interpolate(action, duration, out Coroutine _, then);
  }

  public static WaitUntil Interpolate(
    this MonoBehaviour owner,
    Action<float> action,
    float duration,
    out Coroutine coroutineOut,
    System.Action then = null)
  {
    bool completed = false;
    System.Action then1 = (System.Action) (() =>
    {
      if (then != null)
        then();
      completed = true;
    });
    coroutineOut = owner.StartCoroutine(SequenceTools.InterpolateCoroutineLogic(action, duration, then1));
    return new WaitUntil((Func<bool>) (() => completed));
  }

  private static IEnumerator InterpolateCoroutineLogic(
    Action<float> action,
    float duration,
    System.Action then)
  {
    float timer = 0.0f;
    while ((double) timer < (double) duration)
    {
      action(timer / duration);
      timer += Time.unscaledDeltaTime;
      yield return (object) null;
    }
    action(1f);
    yield return (object) null;
    if (then != null)
      then();
  }

  public static void TextEraser(LocText label, string text, float progress)
  {
    string sourceText = text.Substring(0, Mathf.CeilToInt((float) text.Length * (1f - progress)));
    label.SetText(sourceText);
    label.ForceMeshUpdate(false, false);
  }

  public static void TextWriter(LocText label, string text, float progress)
  {
    string sourceText = (double) progress == 1.0 ? text : text.Substring(0, Mathf.CeilToInt((float) text.Length * progress));
    label.SetText(sourceText);
    label.ForceMeshUpdate(false, false);
  }
}
