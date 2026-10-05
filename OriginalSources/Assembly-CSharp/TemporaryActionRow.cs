// Decompiled with JetBrains decompiler
// Type: TemporaryActionRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class TemporaryActionRow : KMonoBehaviour, IRender200ms
{
  public const float ROW_HEIGHT_ANIM_ENTRY_DURATION = 0.5f;
  public const float ROW_HEIGHT_ANIM_EXIT_DURATION = 0.3f;
  public const float SLIDE_ENTER_ANIM_DURATION = 0.4f;
  public const float SLIDE_EXIT_ANIM_DURATION = 0.4f;
  public RectTransform Content;
  public RectTransform IconSection;
  public RectTransform TimeoutBarSection;
  public KImage Image;
  public UnityEngine.UI.Image TimeoutImage;
  public LocText Label;
  public ToolTip Tooltip;
  public Action<TemporaryActionRow> OnRowClicked;
  public Action<TemporaryActionRow> OnRowHidden;
  private LayoutElement layoutElement;
  private Coroutine layoutCoroutine;
  private UnityEngine.UI.Button button;
  private bool HasBeenShown;
  private float lastSpecifiedLifetime = -1f;

  public float MaxHeight { private set; get; }

  public bool IsVisible { private set; get; }

  public bool ShouldProgressBarBeEnabled
  {
    get
    {
      return this.ShowTimeout && (double) this.Lifetime > 0.0 && (double) this.lastSpecifiedLifetime > 0.0;
    }
  }

  public float Lifetime { private set; get; } = -1f;

  public bool ShowTimeout { set; get; } = true;

  public bool ShowOnSpawn { set; get; } = true;

  public bool HideOnClick { set; get; } = true;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.layoutElement = this.GetComponent<LayoutElement>();
    this.button = this.GetComponent<UnityEngine.UI.Button>();
    this.button.onClick.AddListener(new UnityAction(this._OnRowClicked));
    this.MaxHeight = this.layoutElement.minHeight;
    this.HideImmediatly();
  }

  private void Update()
  {
    if (this.HasBeenShown || !this.ShowOnSpawn)
      return;
    this.RefreshContentWidth();
    if ((double) this.Content.sizeDelta.x <= 0.0)
      return;
    this.Show();
  }

  private void _OnRowClicked()
  {
    Action<TemporaryActionRow> onRowClicked = this.OnRowClicked;
    if (onRowClicked != null)
      onRowClicked(this);
    if (!this.HideOnClick)
      return;
    this.Hide();
  }

  private void _OnRowHidden()
  {
    Action<TemporaryActionRow> onRowHidden = this.OnRowHidden;
    if (onRowHidden == null)
      return;
    onRowHidden(this);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    if (!this.isSpawned)
      return;
    this.RefreshContentWidth();
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    this.HideImmediatly();
    this._OnRowHidden();
  }

  public void SetLifetime(float lifetime)
  {
    this.Lifetime = lifetime;
    this.lastSpecifiedLifetime = lifetime;
    this.UpdateTimeout();
  }

  private void UpdateTimeout()
  {
    bool progressBarBeEnabled = this.ShouldProgressBarBeEnabled;
    if (progressBarBeEnabled != this.TimeoutBarSection.gameObject.activeInHierarchy)
      this.TimeoutBarSection.gameObject.SetActive(progressBarBeEnabled);
    if (!progressBarBeEnabled)
      return;
    this.TimeoutImage.fillAmount = Mathf.Clamp(this.Lifetime / this.lastSpecifiedLifetime, 0.0f, 1f);
  }

  public void Render200ms(float dt)
  {
    if (!this.HasBeenShown || (double) this.Lifetime <= 0.0 || !this.IsVisible)
      return;
    this.Lifetime -= dt;
    if ((double) this.Lifetime <= 0.0)
      this.Hide();
    this.UpdateTimeout();
  }

  public void Setup(string text, string tooltip, Sprite icon = null)
  {
    this.Label.SetText(text);
    this.Tooltip.SetSimpleTooltip(tooltip);
    this.Image.sprite = icon;
    this.IconSection.gameObject.SetActive((UnityEngine.Object) icon != (UnityEngine.Object) null);
  }

  public void Show()
  {
    this.AbortCoroutine();
    this.IsVisible = true;
    this.HasBeenShown = true;
    this.button.interactable = true;
    if (!this.gameObject.activeInHierarchy)
      return;
    this.SetContentToHiddenPosition();
    this.layoutCoroutine = this.RunEnterHeightAnimation((System.Action) (() => this.layoutCoroutine = this.RunEnterSlideAnimation()));
  }

  public void HideImmediatly()
  {
    this.AbortCoroutine();
    this.IsVisible = false;
    this.Content.localPosition = new Vector3(-(this.transform as RectTransform).sizeDelta.x, this.Content.localPosition.y, this.Content.localPosition.z);
    this.layoutElement.minHeight = 0.0f;
    this.button.interactable = false;
  }

  public void Hide()
  {
    this.AbortCoroutine();
    this.IsVisible = false;
    this.button.interactable = false;
    if (!this.gameObject.activeInHierarchy)
      return;
    this.layoutCoroutine = this.RunExitSlideAnimation((System.Action) (() => this.layoutCoroutine = this.RunExitHeightAnimation(new System.Action(this._OnRowHidden))));
  }

  private void AbortCoroutine()
  {
    if (this.layoutCoroutine == null)
      return;
    this.StopCoroutine(this.layoutCoroutine);
    this.layoutCoroutine = (Coroutine) null;
  }

  private void RefreshContentWidth()
  {
    RectTransform transform = this.transform as RectTransform;
    if ((double) transform.sizeDelta.x == (double) this.Content.sizeDelta.x)
      return;
    this.Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, transform.sizeDelta.x);
  }

  private void SetContentToHiddenPosition()
  {
    this.RefreshContentWidth();
    this.Content.anchoredPosition = (Vector2) ((Vector3) this.Content.anchoredPosition with
    {
      x = -(this.transform as RectTransform).sizeDelta.x
    });
  }

  private Coroutine RunEnterSlideAnimation(System.Action onAnimationEnds = null)
  {
    return this.StartCoroutine(this.SlideTransitionAnimation(0.4f, true, (Func<float, float>) (n => Mathf.Sqrt(n)), onAnimationEnds));
  }

  private Coroutine RunExitSlideAnimation(System.Action onAnimationEnds = null)
  {
    return this.StartCoroutine(this.SlideTransitionAnimation(0.4f, false, (Func<float, float>) (n => Mathf.Pow(n, 2f)), onAnimationEnds));
  }

  private Coroutine RunEnterHeightAnimation(System.Action onAnimationEnds = null)
  {
    return this.StartCoroutine(this.HeightTransitionAnimation(0.5f, true, (Func<float, float>) (n => Mathf.Sqrt(n)), onAnimationEnds));
  }

  private Coroutine RunExitHeightAnimation(System.Action onAnimationEnds = null)
  {
    return this.StartCoroutine(this.HeightTransitionAnimation(0.3f, false, (Func<float, float>) (n => Mathf.Pow(n, 2f)), onAnimationEnds));
  }

  private IEnumerator SlideTransitionAnimation(
    float duration,
    bool show,
    Func<float, float> curveModifier = null,
    System.Action onAnimationEnds = null)
  {
    float num1 = -(this.transform as RectTransform).sizeDelta.x;
    float num2 = 0.0f;
    float contentInitialXPosition = show ? num1 : this.Content.anchoredPosition.x;
    float targetPosition = show ? num2 : num1;
    float timePassed = 0.0f;
    Vector3 anchoredPosition = (Vector3) this.Content.anchoredPosition;
    while ((double) timePassed < (double) duration)
    {
      this.RefreshContentWidth();
      float t = timePassed / duration;
      if (curveModifier != null)
        t = curveModifier(t);
      this.Content.anchoredPosition = (Vector2) ((Vector3) this.Content.anchoredPosition with
      {
        x = Mathf.Lerp(contentInitialXPosition, targetPosition, t)
      });
      timePassed += Time.unscaledDeltaTime;
      yield return (object) null;
    }
    this.RefreshContentWidth();
    this.Content.anchoredPosition = (Vector2) ((Vector3) this.Content.anchoredPosition with
    {
      x = targetPosition
    });
    yield return (object) null;
    if (onAnimationEnds != null)
      onAnimationEnds();
  }

  private IEnumerator HeightTransitionAnimation(
    float duration,
    bool show,
    Func<float, float> curveModifier = null,
    System.Action onAnimationEnds = null)
  {
    Transform transform = this.transform;
    float initialHeight = this.layoutElement.minHeight;
    float targetHeight = show ? this.MaxHeight : 0.0f;
    float timePassed = 0.0f;
    float minHeight = this.layoutElement.minHeight;
    while ((double) timePassed < (double) duration)
    {
      this.RefreshContentWidth();
      float t = timePassed / duration;
      if (curveModifier != null)
        t = curveModifier(t);
      this.layoutElement.minHeight = Mathf.Lerp(initialHeight, targetHeight, t);
      timePassed += Time.unscaledDeltaTime;
      yield return (object) null;
    }
    this.RefreshContentWidth();
    this.layoutElement.minHeight = targetHeight;
    yield return (object) null;
    if (onAnimationEnds != null)
      onAnimationEnds();
  }
}
