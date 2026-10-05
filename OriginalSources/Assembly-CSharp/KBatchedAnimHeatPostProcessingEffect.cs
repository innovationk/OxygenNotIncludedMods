// Decompiled with JetBrains decompiler
// Type: KBatchedAnimHeatPostProcessingEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class KBatchedAnimHeatPostProcessingEffect : KMonoBehaviour
{
  public const float SHOW_EFFECT_HEAT_TRESHOLD = 1f;
  private const float DISABLING_VALUE = 0.0f;
  private const float ENABLING_VALUE = 1f;
  private float heatProduction;
  public const float ANIM_DURATION = 1f;
  private int loopsPlayed;
  [MyCmpGet]
  private KBatchedAnimController animController;

  public float HeatProduction => this.heatProduction;

  public bool IsHeatProductionEnoughToShowEffect => (double) this.HeatProduction >= 1.0;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.animController.postProcessingEffectsAllowed |= KAnimConverter.PostProcessingEffects.TemperatureOverlay;
  }

  public void SetHeatBeingProducedValue(float heat)
  {
    this.heatProduction = heat;
    this.RefreshEffectVisualState();
  }

  public void RefreshEffectVisualState()
  {
    if (this.enabled && this.IsHeatProductionEnoughToShowEffect)
      this.SetParameterValue(1f);
    else
      this.SetParameterValue(0.0f);
  }

  private void SetParameterValue(float value)
  {
    if (!((Object) this.animController != (Object) null))
      return;
    this.animController.postProcessingParameters = value;
  }

  protected override void OnCmpEnable() => this.RefreshEffectVisualState();

  protected override void OnCmpDisable() => this.RefreshEffectVisualState();

  private void Update()
  {
    int num = Mathf.FloorToInt(Time.timeSinceLevelLoad / 1f);
    if (num == this.loopsPlayed)
      return;
    this.loopsPlayed = num;
    this.OnNewLoopReached();
  }

  private void OnNewLoopReached()
  {
    if (!((Object) OverlayScreen.Instance != (Object) null) || !(OverlayScreen.Instance.mode == OverlayModes.Temperature.ID) || !this.IsHeatProductionEnoughToShowEffect)
      return;
    Vector3 position = this.transform.GetPosition();
    string sound = GlobalAssets.GetSound("Temperature_Heat_Emission");
    position.z = 0.0f;
    Vector3 pos = position;
    SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, pos));
  }
}
