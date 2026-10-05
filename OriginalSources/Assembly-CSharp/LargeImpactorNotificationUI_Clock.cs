// Decompiled with JetBrains decompiler
// Type: LargeImpactorNotificationUI_Clock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LargeImpactorNotificationUI_Clock : KMonoBehaviour, ISim4000ms
{
  public KImage LargeNeedle;
  public RectTransform SmallNeedlePivot;
  public KImage NeedleTrailBg;
  public Image TimerOutCircleFill;
  public Image TimerOutCircleBG;
  private Color color_circleFillOriginalColor;
  private Color color_needleTrailBgOriginalColor;
  private Color color_timerOutCircleBGOriginalColor;
  private Color softRed;
  private Coroutine animationCoroutine;
  private bool hasSpawned;
  private float entryAnimationDuration = 1f;
  private float reminderAnimationDuration = 16f;
  private bool hasPlayedEntryAnimation;
  private float reminderAnimationTimer = -1f;
  private float lastLargeImpactorTime = -1f;
  private const int reminderSetting_BlinkTimes = 16 /*0x10*/;

  protected override void OnSpawn()
  {
    this.color_circleFillOriginalColor = this.TimerOutCircleFill.color;
    this.color_needleTrailBgOriginalColor = this.NeedleTrailBg.color;
    this.color_timerOutCircleBGOriginalColor = this.TimerOutCircleBG.color;
    this.softRed = new Color(1f, 0.0f, 0.0f, this.color_needleTrailBgOriginalColor.a);
    GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewCycleReached));
    this.UpdateSmallNeedlePosition();
    this.InitializeAnimationCoroutine();
    this.hasSpawned = true;
  }

  private void OnNewCycleReached(object data) => this.PlayReminderAnimation();

  public void SetLargeImpactorTime(float normalizedValue)
  {
    this.lastLargeImpactorTime = normalizedValue;
    this.SetNeedleRotation(this.LargeNeedle.rectTransform, 1f - this.lastLargeImpactorTime);
    if (!this.hasPlayedEntryAnimation)
      return;
    this.TimerOutCircleFill.fillAmount = this.lastLargeImpactorTime;
  }

  private void SetNeedleRotation(RectTransform needle, float normalizedTime)
  {
    needle.localRotation = Quaternion.Euler(0.0f, 0.0f, -360f * normalizedTime);
    if (!((UnityEngine.Object) needle.gameObject == (UnityEngine.Object) this.LargeNeedle.gameObject))
      return;
    this.NeedleTrailBg.fillAmount = normalizedTime;
  }

  public void Sim4000ms(float dt) => this.UpdateSmallNeedlePosition();

  private void UpdateSmallNeedlePosition()
  {
    this.SetNeedleRotation(this.SmallNeedlePivot, GameClock.Instance.GetCurrentCycleAsPercentage());
  }

  private void InitializeAnimationCoroutine()
  {
    this.AbortCoroutine();
    this.animationCoroutine = this.StartCoroutine(this.AnimationCoroutineLogic());
  }

  private void AbortCoroutine()
  {
    if (this.animationCoroutine != null)
      this.StopAllCoroutines();
    this.animationCoroutine = (Coroutine) null;
  }

  public void PlayReminderAnimation() => this.reminderAnimationTimer = 0.0f;

  private IEnumerator AnimationCoroutineLogic()
  {
    if (!this.hasPlayedEntryAnimation)
    {
      double cycleAsPercentage = (double) GameClock.Instance.GetCurrentCycleAsPercentage();
      double num = (double) this.entryAnimationDuration / 600.0;
      yield return (object) this.Interpolate((Action<float>) (n => this.TimerOutCircleFill.fillAmount = n * this.lastLargeImpactorTime), this.entryAnimationDuration);
      this.hasPlayedEntryAnimation = true;
    }
    while (true)
    {
      do
      {
        if ((double) this.reminderAnimationTimer < 0.0)
          yield return (object) null;
        if ((double) this.reminderAnimationTimer >= 0.0 && (double) this.reminderAnimationTimer < (double) this.reminderAnimationDuration)
        {
          float t = Mathf.Abs(Mathf.Sin((float) ((double) this.reminderAnimationTimer / (double) this.reminderAnimationDuration * 3.1415927410125732 * 16.0)));
          this.TimerOutCircleBG.color = Color.Lerp(this.color_timerOutCircleBGOriginalColor, Color.red, t);
          this.NeedleTrailBg.color = Color.Lerp(this.color_needleTrailBgOriginalColor, this.softRed, t);
          this.reminderAnimationTimer += Time.deltaTime;
          yield return (object) null;
        }
      }
      while ((double) this.reminderAnimationTimer < (double) this.reminderAnimationDuration);
      this.TimerOutCircleBG.color = this.color_timerOutCircleBGOriginalColor;
      this.NeedleTrailBg.color = this.color_needleTrailBgOriginalColor;
      this.reminderAnimationTimer = -1f;
      yield return (object) null;
    }
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    if (!this.hasSpawned)
      return;
    this.InitializeAnimationCoroutine();
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    this.AbortCoroutine();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    GameClock.Instance.Unsubscribe(631075836, new Action<object>(this.OnNewCycleReached));
  }
}
