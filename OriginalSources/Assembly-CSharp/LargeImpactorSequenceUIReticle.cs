// Decompiled with JetBrains decompiler
// Type: LargeImpactorSequenceUIReticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LargeImpactorSequenceUIReticle : KMonoBehaviour
{
  private const float reticleEnterDuration = 0.4f;
  private const float flashDuration = 0.4f;
  private const int flashTimes = 3;
  private const float reticleZoomOutDuration = 0.8f;
  private const float labelRevealDuration = 1f;
  private const float sidePanel_TitleRevealDuration = 0.5f;
  private const float sidePanel_DescriptionRevealDuration = 1.5f;
  private const float exitToCalculationDuration = 0.5f;
  private const float expandReticleHorizontallyDuration = 0.8f;
  private const float calculateImpactZoneTextRevealDuration = 0.5f;
  private const float exitDuration = 0.8f;
  public const float RevealPOI_LandingZone_Duration = 3.5f;
  private const string Sound_LockTarget = "HUD_Imperative_analysis_start";
  private const string Sound_BracketSquareExpand = "HUD_Imperative_bracket_open_first";
  private const string Sound_BracketExpandsForCalculatingLandingZone = "HUD_Imperative_bracket_open_second";
  private const string Sound_CalculatingLandingZoneTextAppears = "HUD_Imperative_calculating_beep";
  private const string Sound_TypeHeader = "HUD_Imperative_Text_typing_header";
  private const string Sound_TypeBody = "HUD_Imperative_Text_typing_body";
  public Vector2 initialSize = new Vector2(100f, 100f);
  public Vector2 zoomedOutSize = new Vector2(180f, 180f);
  public Vector2 calculatingImpactSize = new Vector2(500f, 120f);
  [Space]
  public LocText label;
  public LocText sidePanelTitleLabel;
  public LocText sidePanelDescriptionLabel;
  public LocText calculateImpactLabel;
  public Image bg;
  public Image border;
  public Image sidePanelIcon;
  private RectTransform transform;
  private LargeImpactorStatus.Instance largeImpactorStatus;
  private LoopingSounds loopingSounds;
  private bool isVisible;
  private Color bgOriginalColor;
  private Color calculatingImpactLabelOriginalColor;
  private System.Action onPhase1Completed;
  private System.Action onComplete;
  private Coroutine coroutine;

  protected override void OnPrefabInit()
  {
    this.transform = this.transform as RectTransform;
    this.bgOriginalColor = this.bg.color;
    this.calculatingImpactLabelOriginalColor = this.calculateImpactLabel.color;
    base.OnPrefabInit();
  }

  protected override void OnSpawn() => this.ResetGraphics();

  public void Run(System.Action onPhase1Completed = null, System.Action onComplete = null)
  {
    this.SetVisibility(true);
    this.AbortCoroutine();
    this.ResetGraphics();
    this.onPhase1Completed = onPhase1Completed;
    this.onComplete = onComplete;
    this.InitializeAndRunCoroutine();
  }

  public void Hide()
  {
    this.AbortCoroutine();
    this.ResetGraphics();
    this.SetVisibility(false);
  }

  private void SetVisibility(bool visible)
  {
    this.isVisible = visible;
    this.label.enabled = visible;
    this.bg.enabled = visible;
    this.border.enabled = visible;
  }

  private void InitializeAndRunCoroutine()
  {
    this.coroutine = this.StartCoroutine(this.EnterSequence());
  }

  private void AbortCoroutine()
  {
    this.StopAllCoroutines();
    this.coroutine = (Coroutine) null;
  }

  public void SetTarget(LargeImpactorStatus.Instance largeImpactorStatus)
  {
    this.largeImpactorStatus = largeImpactorStatus;
    this.loopingSounds = largeImpactorStatus.GetComponent<LoopingSounds>();
  }

  private void ResetGraphics()
  {
    this.label.SetText("");
    this.border.Opacity(0.0f);
    this.bg.color = this.bgOriginalColor;
    this.bg.Opacity(0.0f);
    this.sidePanelIcon.Opacity(0.0f);
    this.sidePanelTitleLabel.SetText("");
    this.calculateImpactLabel.SetText("");
    this.sidePanelDescriptionLabel.SetText("");
    this.calculateImpactLabel.color = this.calculatingImpactLabelOriginalColor;
  }

  private void PlayLoopingSound(string soundName)
  {
    this.loopingSounds.StartSound(GlobalAssets.GetSound(soundName), false, false, false);
  }

  private IEnumerator EnterSequence()
  {
    KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Imperative_analysis_start"));
    yield return (object) this.Interpolate((Action<float>) (n =>
    {
      this.transform.sizeDelta = Vector2.Lerp(this.initialSize * 2f, this.initialSize, n);
      this.border.Opacity(n);
    }), 0.4f);
    if (this.bg.color != this.border.color)
      this.bg.color = this.border.color;
    yield return (object) this.Interpolate((Action<float>) (n => this.bg.Opacity(Mathf.Abs(Mathf.Sin((float) ((double) n * 3.1415927410125732 * 3.0))))), 0.4f, (System.Action) (() => this.bg.color = this.bgOriginalColor));
    KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Imperative_bracket_open_first"));
    yield return (object) this.Interpolate((Action<float>) (n =>
    {
      this.bg.Opacity(n * this.bgOriginalColor.a);
      this.transform.sizeDelta = Vector2.Lerp(this.initialSize, this.zoomedOutSize, n);
    }), 0.8f);
    this.PlayLoopingSound("HUD_Imperative_Text_typing_header");
    string titleText = (string) MISC.NOTIFICATIONS.LARGEIMPACTORREVEALSEQUENCE.RETICLE.LARGE_IMPACTOR_NAME;
    yield return (object) this.Interpolate((Action<float>) (n => SequenceTools.TextWriter(this.label, titleText, n)), 1f);
    this.loopingSounds.StopSound(GlobalAssets.GetSound("HUD_Imperative_Text_typing_header"));
    yield return (object) null;
    this.PlayLoopingSound("HUD_Imperative_Text_typing_header");
    this.sidePanelIcon.color = Color.white;
    string sidePanelTitleText = (string) MISC.NOTIFICATIONS.LARGEIMPACTORREVEALSEQUENCE.RETICLE.SIDE_PANEL_TITLE;
    yield return (object) this.Interpolate((Action<float>) (n =>
    {
      this.sidePanelIcon.Opacity(n);
      this.sidePanelIcon.transform.localRotation = Quaternion.Euler(0.0f, Mathf.Lerp(90f, 0.0f, n), 0.0f);
      SequenceTools.TextWriter(this.sidePanelTitleLabel, sidePanelTitleText, n);
    }), 0.5f);
    this.loopingSounds.StopSound(GlobalAssets.GetSound("HUD_Imperative_Text_typing_header"));
    this.PlayLoopingSound("HUD_Imperative_Text_typing_body");
    string sidePanelDescriptionText = GameUtil.SafeStringFormat((string) MISC.NOTIFICATIONS.LARGEIMPACTORREVEALSEQUENCE.RETICLE.SIDE_PANEL_DESCRIPTION, (object) GameUtil.GetFormattedCycles(this.largeImpactorStatus.TimeRemainingBeforeCollision).Split(' ', StringSplitOptions.None)[0]);
    yield return (object) this.Interpolate((Action<float>) (n => SequenceTools.TextWriter(this.sidePanelDescriptionLabel, sidePanelDescriptionText, n)), 1.5f);
    this.loopingSounds.StopSound(GlobalAssets.GetSound("HUD_Imperative_Text_typing_body"));
    yield return (object) new WaitForSecondsRealtime(2f);
    if (this.onPhase1Completed != null)
    {
      this.onPhase1Completed();
      this.onPhase1Completed = (System.Action) null;
    }
    yield return (object) this.Interpolate((Action<float>) (n =>
    {
      SequenceTools.TextEraser(this.label, titleText, n);
      SequenceTools.TextEraser(this.sidePanelTitleLabel, sidePanelTitleText, n);
      SequenceTools.TextEraser(this.sidePanelDescriptionLabel, sidePanelDescriptionText, n);
      this.sidePanelIcon.color = Color.Lerp(Color.white, Color.red, n);
      this.sidePanelIcon.Opacity(1f - n);
    }), 0.5f);
    Color bgColor = this.bg.color;
    Color targetBgColor = Color.Lerp(this.bgOriginalColor, Color.black, 0.5f) with
    {
      a = this.bgOriginalColor.a * 0.8f
    };
    KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Imperative_bracket_open_second"));
    yield return (object) this.Interpolate((Action<float>) (n =>
    {
      this.bg.color = Color.Lerp(bgColor, targetBgColor, n);
      this.transform.sizeDelta = Vector2.Lerp(this.zoomedOutSize, this.calculatingImpactSize, n);
    }), 0.8f);
    KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Imperative_calculating_beep"));
    Coroutine flashLabelCoroutine = (Coroutine) null;
    this.Interpolate((Action<float>) (n => this.calculateImpactLabel.color = Color.Lerp(Color.white, Color.red, Mathf.Abs(Mathf.Sin((float) ((double) n * 3.1415927410125732 * 999.0))))), 999f, out flashLabelCoroutine);
    yield return (object) this.Interpolate((Action<float>) (n => SequenceTools.TextWriter(this.calculateImpactLabel, (string) MISC.NOTIFICATIONS.LARGEIMPACTORREVEALSEQUENCE.RETICLE.CALCULATING_IMPACT_ZONE_TEXT, n)), 0.5f);
    yield return (object) new WaitForSecondsRealtime(3.5f);
    this.StopCoroutine(flashLabelCoroutine);
    flashLabelCoroutine = (Coroutine) null;
    Color impactLabelColor = this.calculateImpactLabel.color;
    yield return (object) this.Interpolate((Action<float>) (n =>
    {
      float opacity = Mathf.Sqrt(1f - n);
      float t = Mathf.Sqrt(n);
      this.bg.Opacity(this.bgOriginalColor.a * opacity);
      this.border.Opacity(opacity);
      this.calculateImpactLabel.Opacity(impactLabelColor.a * opacity);
      this.transform.sizeDelta = Vector2.Lerp(this.calculatingImpactSize, this.calculatingImpactSize * 1.3f, t);
    }), 0.8f);
    if (this.onComplete != null)
    {
      this.onComplete();
      this.onComplete = (System.Action) null;
    }
  }

  protected override void OnCmpDisable() => this.AbortCoroutine();

  protected override void OnCmpEnable()
  {
    if (!this.isVisible)
      return;
    this.AbortCoroutine();
    this.ResetGraphics();
    this.InitializeAndRunCoroutine();
  }
}
