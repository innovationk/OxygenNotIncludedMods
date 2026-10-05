// Decompiled with JetBrains decompiler
// Type: LargeImpactorUINotificationHitEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LargeImpactorUINotificationHitEffects : KMonoBehaviour, IRenderEveryTick
{
  public Image hitBackgorund;
  public Image heartIcon;
  public Image healthBarFill;
  public UIShake shake;
  public Color HighlightedColor = Color.yellow;
  private Color heartIconOriginalColor;
  private Color healthBarOriginalColor;
  private float duration = 0.4f;
  private float lastTimerValue;
  private float timer;

  private float Intensity => this.timer / this.duration;

  public void PlayHitEffect() => this.timer = this.duration;

  public void RenderEveryTick(float dt)
  {
    if ((double) this.lastTimerValue != (double) this.timer)
    {
      this.lastTimerValue = this.timer;
      this.hitBackgorund.Opacity(this.Intensity);
      this.heartIcon.color = Color.Lerp(this.heartIconOriginalColor, this.HighlightedColor, this.Intensity);
      this.healthBarFill.color = Color.Lerp(this.healthBarOriginalColor, this.HighlightedColor, this.Intensity);
      this.shake.SetIntensity(this.Intensity);
    }
    this.timer = Mathf.Clamp(this.timer - dt, 0.0f, this.duration);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.heartIconOriginalColor = this.heartIcon.color;
    this.healthBarOriginalColor = this.healthBarFill.color;
  }
}
