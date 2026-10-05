// Decompiled with JetBrains decompiler
// Type: EasingAnimations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EasingAnimations : MonoBehaviour
{
  public EasingAnimations.AnimationScales[] scales;
  private EasingAnimations.AnimationScales currentAnimation;
  private Coroutine animationCoroutine;
  private Dictionary<string, EasingAnimations.AnimationScales> animationMap;
  public Action<string> OnAnimationDone;

  public bool IsPlaying => this.animationCoroutine != null;

  private void Start()
  {
    if (this.animationMap != null && this.animationMap.Count != 0)
      return;
    this.Initialize();
  }

  private void Initialize()
  {
    this.animationMap = new Dictionary<string, EasingAnimations.AnimationScales>();
    foreach (EasingAnimations.AnimationScales scale in this.scales)
      this.animationMap.Add(scale.name, scale);
  }

  public void PlayAnimation(string animationName, float delay = 0.0f)
  {
    if (this.animationMap == null || this.animationMap.Count == 0)
      this.Initialize();
    if (!this.animationMap.ContainsKey(animationName))
      return;
    if (this.animationCoroutine != null)
      this.StopCoroutine(this.animationCoroutine);
    this.currentAnimation = this.animationMap[animationName];
    this.currentAnimation.currentScale = this.currentAnimation.startScale;
    this.transform.localScale = Vector3.one * this.currentAnimation.currentScale;
    this.animationCoroutine = this.StartCoroutine(this.ExecuteAnimation(delay));
  }

  private IEnumerator ExecuteAnimation(float delay)
  {
    float startTime = Time.realtimeSinceStartup;
    while ((double) Time.realtimeSinceStartup < (double) startTime + (double) delay)
      yield return (object) SequenceUtil.WaitForNextFrame;
    startTime = Time.realtimeSinceStartup;
    bool keepAnimating = true;
    while (keepAnimating)
    {
      this.currentAnimation.currentScale = this.GetEasing((Time.realtimeSinceStartup - startTime) * this.currentAnimation.easingMultiplier);
      keepAnimating = (double) this.currentAnimation.endScale <= (double) this.currentAnimation.startScale ? (double) this.currentAnimation.currentScale > (double) this.currentAnimation.endScale + 0.02500000037252903 : (double) this.currentAnimation.currentScale < (double) this.currentAnimation.endScale - 0.02500000037252903;
      if (!keepAnimating)
        this.currentAnimation.currentScale = this.currentAnimation.endScale;
      this.transform.localScale = Vector3.one * this.currentAnimation.currentScale;
      yield return (object) SequenceUtil.WaitForEndOfFrame;
    }
    this.animationCoroutine = (Coroutine) null;
    if (this.OnAnimationDone != null)
      this.OnAnimationDone(this.currentAnimation.name);
  }

  private float GetEasing(float t)
  {
    switch (this.currentAnimation.type)
    {
      case EasingAnimations.AnimationScales.AnimationType.EaseOutBack:
        return this.EaseOutBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
      case EasingAnimations.AnimationScales.AnimationType.EaseInBack:
        return this.EaseInBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
      default:
        return this.EaseInOutBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
    }
  }

  public float EaseInOutBack(float start, float end, float value)
  {
    float num1 = 1.70158f;
    end -= start;
    value /= 0.5f;
    if ((double) value < 1.0)
    {
      float num2 = num1 * 1.525f;
      return (float) ((double) end * 0.5 * ((double) value * (double) value * (((double) num2 + 1.0) * (double) value - (double) num2))) + start;
    }
    value -= 2f;
    float num3 = num1 * 1.525f;
    return (float) ((double) end * 0.5 * ((double) value * (double) value * (((double) num3 + 1.0) * (double) value + (double) num3) + 2.0)) + start;
  }

  public float EaseInBack(float start, float end, float value)
  {
    end -= start;
    value /= 1f;
    float num = 1.70158f;
    return (float) ((double) end * (double) value * (double) value * (((double) num + 1.0) * (double) value - (double) num)) + start;
  }

  public float EaseOutBack(float start, float end, float value)
  {
    float num = 1.70158f;
    end -= start;
    --value;
    return end * (float) ((double) value * (double) value * (((double) num + 1.0) * (double) value + (double) num) + 1.0) + start;
  }

  [Serializable]
  public struct AnimationScales
  {
    public string name;
    public float startScale;
    public float endScale;
    public EasingAnimations.AnimationScales.AnimationType type;
    public float easingMultiplier;
    [HideInInspector]
    public float currentScale;

    public enum AnimationType
    {
      EaseInOutBack,
      EaseOutBack,
      EaseInBack,
    }
  }
}
