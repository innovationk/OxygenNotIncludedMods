// Decompiled with JetBrains decompiler
// Type: PresUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public static class PresUtil
{
  public static Promise MoveAndFade(
    RectTransform rect,
    Vector2 targetAnchoredPosition,
    float targetAlpha,
    float duration,
    Easing.EasingFn easing = null)
  {
    CanvasGroup canvasGroup = rect.FindOrAddComponent<CanvasGroup>();
    return rect.FindOrAddComponent<CoroutineRunner>().Run((IEnumerator) Updater.Parallel(Updater.Ease((Action<float>) (f => canvasGroup.alpha = f), canvasGroup.alpha, targetAlpha, duration, easing), Updater.Ease((Action<Vector2>) (v2 => rect.anchoredPosition = v2), rect.anchoredPosition, targetAnchoredPosition, duration, easing)));
  }

  public static Promise OffsetFromAndFade(
    RectTransform rect,
    Vector2 offset,
    float targetAlpha,
    float duration,
    Easing.EasingFn easing = null)
  {
    Vector2 anchoredPosition = rect.anchoredPosition;
    return PresUtil.MoveAndFade(rect, offset + anchoredPosition, targetAlpha, duration, easing);
  }

  public static Promise OffsetToAndFade(
    RectTransform rect,
    Vector2 offset,
    float targetAlpha,
    float duration,
    Easing.EasingFn easing = null)
  {
    Vector2 anchoredPosition = rect.anchoredPosition;
    rect.anchoredPosition = offset + anchoredPosition;
    return PresUtil.MoveAndFade(rect, anchoredPosition, targetAlpha, duration, easing);
  }
}
