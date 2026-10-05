// Decompiled with JetBrains decompiler
// Type: ResearchButtonImageToggleStateUtilityFunctions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public static class ResearchButtonImageToggleStateUtilityFunctions
{
  public static void Opacity(this Graphic graphic, float opacity)
  {
    Color color = graphic.color with { a = opacity };
    graphic.color = color;
  }

  public static WaitUntil FadeAway(
    this Graphic graphic,
    float duration,
    Func<bool> assertCondition = null)
  {
    float timer = 0.0f;
    float startingOpacity = graphic.color.a;
    return new WaitUntil((Func<bool>) (() =>
    {
      if ((double) timer >= (double) duration || assertCondition != null && !assertCondition())
      {
        graphic.Opacity(0.0f);
        return true;
      }
      graphic.Opacity(startingOpacity * (1f - timer / duration));
      timer += Time.unscaledDeltaTime;
      return false;
    }));
  }

  public static WaitUntil FadeToVisible(
    this Graphic graphic,
    float duration,
    Func<bool> assertCondition = null)
  {
    float timer = 0.0f;
    float startingOpacity = graphic.color.a;
    float remainingOpacity = 1f - graphic.color.a;
    return new WaitUntil((Func<bool>) (() =>
    {
      if ((double) timer >= (double) duration || assertCondition != null && !assertCondition())
      {
        graphic.Opacity(1f);
        return true;
      }
      graphic.Opacity(startingOpacity + remainingOpacity * (timer / duration));
      timer += Time.unscaledDeltaTime;
      return false;
    }));
  }
}
