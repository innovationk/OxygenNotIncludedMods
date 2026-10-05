// Decompiled with JetBrains decompiler
// Type: LargeImpactorNotificationUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class LargeImpactorNotificationUI : KMonoBehaviour, ISim200ms
{
  public Image healthbar;
  public LargeImpactorNotificationUI_Clock clock;
  public KToggleSlider toggle;
  public LargeImpactorUINotificationHitEffects hitEffects;
  public LargeImpactorNotificationUI_CycleLabelEffects cyclesLabelEffects;
  public LocText numberOfCyclesLabel;
  private LargeImpactorStatus.Instance statusMonitor;
  private LargeImpactorVisualizer rangeVisualizer;
  private ParallaxBackgroundObject asteroidBackground;
  private int midSkyCell = Grid.InvalidCell;
  private const string Hit_SFX = "Notification_Imperative_hit";
  private const string Click_SFX = "HUD_Click_Open ";
  private const string Focus_SFX = "HUD_Demolior_Click_focus";
  private const string ToggleOff_SFX = "HUD_Demolior_LandingZone_toggle_off";
  private const string ToggleOn_SFX = "HUD_Demolior_LandingZone_toggle_on";

  protected override void OnSpawn()
  {
    GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance((HashedString) Db.Get().GameplayEvents.LargeImpactor.Id);
    LargeImpactorEvent.StatesInstance smi = (LargeImpactorEvent.StatesInstance) gameplayEventInstance.smi;
    this.rangeVisualizer = smi.impactorInstance.GetComponent<LargeImpactorVisualizer>();
    this.asteroidBackground = smi.impactorInstance.GetComponent<ParallaxBackgroundObject>();
    this.statusMonitor = smi.impactorInstance.GetSMI<LargeImpactorStatus.Instance>();
    this.statusMonitor.OnDamaged += new Action<int>(this.OnAsteroidDamaged);
    Game.Instance.Subscribe(445618876, new Action<object>(this.OnScreenResolutionChanged));
    Game.Instance.Subscribe(-810220474, new Action<object>(this.OnScreenResolutionChanged));
    this.cyclesLabelEffects.InitializeCycleLabelFocusMonitor();
    this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.ToggleVisibility));
    this.toggle.SetIsOnWithoutNotify((UnityEngine.Object) this.rangeVisualizer != (UnityEngine.Object) null && this.rangeVisualizer.Visible);
    this.toggle.offEffectDuration = this.rangeVisualizer.FoldEffectDuration;
    LargeImpactorCrashStamp component = smi.impactorInstance.GetComponent<LargeImpactorCrashStamp>();
    this.midSkyCell = Grid.FindMidSkyCellAlignedWithCellInWorld(Grid.XYToCell(component.stampLocation.x, component.stampLocation.y), gameplayEventInstance.worldId);
    this.RefreshTogglePositionInRangeVisualizer();
    this.RefreshValues();
  }

  private void OnScreenResolutionChanged(object data)
  {
    this.RefreshTogglePositionInRangeVisualizer();
  }

  private void RefreshTogglePositionInRangeVisualizer()
  {
    if (!((UnityEngine.Object) this.rangeVisualizer != (UnityEngine.Object) null))
      return;
    RectTransform rectTransform = this.toggle.rectTransform();
    Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint((Camera) null, rectTransform.TransformPoint((Vector3) rectTransform.rect.center));
    this.rangeVisualizer.ScreenSpaceNotificationTogglePosition = new Vector2(screenPoint.x / (float) Screen.width, screenPoint.y / (float) Screen.height);
  }

  public void Sim200ms(float dt) => this.RefreshValues();

  public void RefreshValues()
  {
    float num = (float) this.statusMonitor.Health / (float) this.statusMonitor.def.MAX_HEALTH;
    float normalizedValue = this.statusMonitor.TimeRemainingBeforeCollision / LargeImpactorEvent.GetImpactTime();
    this.healthbar.fillAmount = num;
    this.clock.SetLargeImpactorTime(normalizedValue);
    this.numberOfCyclesLabel.SetText(GameUtil.GetFormattedCycles(this.statusMonitor.TimeRemainingBeforeCollision).Split(' ', StringSplitOptions.None)[0]);
    if (!((UnityEngine.Object) this.rangeVisualizer != (UnityEngine.Object) null) || this.toggle.isOn == this.rangeVisualizer.Visible)
      return;
    this.toggle.isOn = this.rangeVisualizer.Visible;
  }

  private void OnAsteroidDamaged(int newHealth)
  {
    this.hitEffects.PlayHitEffect();
    KFMOD.PlayUISound(GlobalAssets.GetSound("Notification_Imperative_hit"));
    this.RefreshValues();
  }

  public void ToggleVisibility(bool shouldBeVisible)
  {
    if (!((UnityEngine.Object) this.rangeVisualizer != (UnityEngine.Object) null))
      return;
    KFMOD.PlayUISound(GlobalAssets.GetSound(shouldBeVisible ? "HUD_Demolior_LandingZone_toggle_on" : "HUD_Demolior_LandingZone_toggle_off"));
    this.RefreshTogglePositionInRangeVisualizer();
    this.rangeVisualizer.SetFoldedState(!shouldBeVisible);
  }

  public void OnPlayerClickedNotification()
  {
    GameUtil.FocusCamera(this.midSkyCell);
    KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click_Open "));
    KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Demolior_Click_focus"));
    if (!((UnityEngine.Object) this.asteroidBackground != (UnityEngine.Object) null))
      return;
    this.asteroidBackground.PlayPlayerClickFeedback();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.cyclesLabelEffects.AbortCycleLabelFocusMonitor();
    if (this.statusMonitor != null)
      this.statusMonitor.OnDamaged -= new Action<int>(this.OnAsteroidDamaged);
    Game.Instance.Unsubscribe(445618876, new Action<object>(this.OnScreenResolutionChanged));
    Game.Instance.Unsubscribe(-810220474, new Action<object>(this.OnScreenResolutionChanged));
  }

  protected override void OnCmpEnable()
  {
    if (!this.isSpawned)
      return;
    this.cyclesLabelEffects.InitializeCycleLabelFocusMonitor();
  }

  protected override void OnCmpDisable() => this.cyclesLabelEffects.AbortCycleLabelFocusMonitor();
}
