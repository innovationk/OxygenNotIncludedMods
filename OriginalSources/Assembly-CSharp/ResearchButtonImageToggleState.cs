// Decompiled with JetBrains decompiler
// Type: ResearchButtonImageToggleState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ResearchButtonImageToggleState : ImageToggleState
{
  public Image progressBar;
  private KToggle toggle;
  [Header("Scroll Options")]
  public float researchLogoDuration = 5f;
  public float durationPerResearchItemIcon = 0.6f;
  public float fadingDuration = 0.2f;
  private Coroutine scrollIconCoroutine;
  private Sprite[] currentResearchIcons;
  private float mainIconScreenTime;
  private float itemScreenTime;
  private int item_idx = -1;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Research.Instance.Subscribe(-1914338957, new Action<object>(this.UpdateActiveResearch));
    Research.Instance.Subscribe(-125623018, new Action<object>(this.RefreshProgressBar));
    this.toggle = this.GetComponent<KToggle>();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.UpdateActiveResearch((object) null);
    this.RestartCoroutine();
  }

  protected override void OnCleanUp()
  {
    this.AbortCoroutine();
    Research.Instance.Unsubscribe(-1914338957, new Action<object>(this.UpdateActiveResearch));
    Research.Instance.Unsubscribe(-125623018, new Action<object>(this.RefreshProgressBar));
    base.OnCleanUp();
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.RestartCoroutine();
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    this.AbortCoroutine();
  }

  private void AbortCoroutine()
  {
    if (this.scrollIconCoroutine != null)
      this.StopCoroutine(this.scrollIconCoroutine);
    this.scrollIconCoroutine = (Coroutine) null;
  }

  private void RestartCoroutine()
  {
    this.AbortCoroutine();
    if (!this.gameObject.activeInHierarchy)
      return;
    this.scrollIconCoroutine = this.StartCoroutine(this.ScrollIcon());
  }

  private void UpdateActiveResearch(object o)
  {
    TechInstance activeResearch = Research.Instance.GetActiveResearch();
    if (activeResearch == null)
    {
      this.currentResearchIcons = (Sprite[]) null;
    }
    else
    {
      this.currentResearchIcons = new Sprite[activeResearch.tech.unlockedItems.Count];
      for (int index = 0; index < activeResearch.tech.unlockedItems.Count; ++index)
      {
        TechItem unlockedItem = activeResearch.tech.unlockedItems[index];
        this.currentResearchIcons[index] = unlockedItem.UISprite();
      }
    }
    this.ResetCoroutineTimers();
    this.RefreshProgressBar(o);
  }

  public void RefreshProgressBar(object o)
  {
    TechInstance activeResearch = Research.Instance.GetActiveResearch();
    if (activeResearch == null)
      this.progressBar.fillAmount = 0.0f;
    else
      this.progressBar.fillAmount = activeResearch.GetTotalPercentageComplete();
  }

  public void SetProgressBarVisibility(bool viisble) => this.progressBar.enabled = viisble;

  public override void SetActive()
  {
    base.SetActive();
    this.SetProgressBarVisibility(false);
  }

  public override void SetDisabledActive()
  {
    base.SetDisabledActive();
    this.SetProgressBarVisibility(false);
  }

  public override void SetDisabled()
  {
    base.SetDisabled();
    this.SetProgressBarVisibility(false);
  }

  public override void SetInactive()
  {
    base.SetInactive();
    this.SetProgressBarVisibility(true);
    this.RefreshProgressBar((object) null);
  }

  private void ResetCoroutineTimers()
  {
    this.mainIconScreenTime = 0.0f;
    this.itemScreenTime = 0.0f;
    this.item_idx = -1;
  }

  private bool ReadyToDisplayIcons
  {
    get
    {
      return this.progressBar.enabled && this.currentResearchIcons != null && this.item_idx >= 0 && this.item_idx < this.currentResearchIcons.Length;
    }
  }

  private IEnumerator ScrollIcon()
  {
    while (Application.isPlaying)
    {
      if ((double) this.mainIconScreenTime < (double) this.researchLogoDuration)
      {
        this.toggle.fgImage.Opacity(1f);
        if ((UnityEngine.Object) this.toggle.fgImage.overrideSprite != (UnityEngine.Object) null)
          this.toggle.fgImage.overrideSprite = (Sprite) null;
        this.item_idx = 0;
        this.itemScreenTime = 0.0f;
        this.mainIconScreenTime += Time.unscaledDeltaTime;
        if (this.progressBar.enabled && (double) this.mainIconScreenTime >= (double) this.researchLogoDuration && this.ReadyToDisplayIcons)
          yield return (object) this.toggle.fgImage.FadeAway(this.fadingDuration, (Func<bool>) (() => this.progressBar.enabled && (double) this.mainIconScreenTime >= (double) this.researchLogoDuration && this.ReadyToDisplayIcons));
        yield return (object) null;
      }
      else if (this.ReadyToDisplayIcons)
      {
        if ((UnityEngine.Object) this.toggle.fgImage.overrideSprite != (UnityEngine.Object) this.currentResearchIcons[this.item_idx])
          this.toggle.fgImage.overrideSprite = this.currentResearchIcons[this.item_idx];
        yield return (object) this.toggle.fgImage.FadeToVisible(this.fadingDuration, (Func<bool>) (() => this.ReadyToDisplayIcons));
        while ((double) this.itemScreenTime < (double) this.durationPerResearchItemIcon && this.ReadyToDisplayIcons)
        {
          this.itemScreenTime += Time.unscaledDeltaTime;
          yield return (object) null;
        }
        yield return (object) this.toggle.fgImage.FadeAway(this.fadingDuration, (Func<bool>) (() => this.ReadyToDisplayIcons));
        if (this.ReadyToDisplayIcons)
        {
          this.itemScreenTime = 0.0f;
          ++this.item_idx;
        }
        yield return (object) null;
      }
      else
      {
        this.mainIconScreenTime = 0.0f;
        yield return (object) null;
      }
    }
  }
}
