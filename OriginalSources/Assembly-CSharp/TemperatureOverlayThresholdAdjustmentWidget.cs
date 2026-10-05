// Decompiled with JetBrains decompiler
// Type: TemperatureOverlayThresholdAdjustmentWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class TemperatureOverlayThresholdAdjustmentWidget : KMonoBehaviour
{
  public const float DEFAULT_TEMPERATURE = 294.15f;
  [SerializeField]
  private Scrollbar scrollbar;
  [SerializeField]
  private LocText scrollBarRangeLowText;
  [SerializeField]
  private LocText scrollBarRangeCenterText;
  [SerializeField]
  private LocText scrollBarRangeHighText;
  [SerializeField]
  private KButton defaultButton;
  private static float maxTemperatureRange = 700f;
  private static float temperatureWindowSize = 200f;
  private static float minimumSelectionTemperature = TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.scrollbar.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.scrollbar.size = TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange;
    this.scrollbar.value = this.KelvinToScrollPercentage(SaveGame.Instance.relativeTemperatureOverlaySliderValue);
    this.defaultButton.onClick += new System.Action(this.OnDefaultPressed);
  }

  private void OnValueChanged(float data) => this.SetUserConfig(data);

  private float KelvinToScrollPercentage(float kelvin)
  {
    kelvin -= TemperatureOverlayThresholdAdjustmentWidget.minimumSelectionTemperature;
    if ((double) kelvin < 1.0)
      kelvin = 1f;
    return Mathf.Clamp01(kelvin / TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange);
  }

  private void SetUserConfig(float scrollPercentage)
  {
    float temp = TemperatureOverlayThresholdAdjustmentWidget.minimumSelectionTemperature + TemperatureOverlayThresholdAdjustmentWidget.maxTemperatureRange * scrollPercentage;
    float f1 = temp - TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;
    float f2 = temp + TemperatureOverlayThresholdAdjustmentWidget.temperatureWindowSize / 2f;
    SimDebugView.Instance.user_temperatureThresholds[0] = f1;
    SimDebugView.Instance.user_temperatureThresholds[1] = f2;
    this.scrollBarRangeCenterText.SetText(GameUtil.GetFormattedTemperature(temp, roundInDestinationFormat: true));
    this.scrollBarRangeLowText.SetText(GameUtil.GetFormattedTemperature((float) Mathf.RoundToInt(f1), roundInDestinationFormat: true));
    this.scrollBarRangeHighText.SetText(GameUtil.GetFormattedTemperature((float) Mathf.RoundToInt(f2), roundInDestinationFormat: true));
    SaveGame.Instance.relativeTemperatureOverlaySliderValue = temp;
  }

  private void OnDefaultPressed() => this.scrollbar.value = this.KelvinToScrollPercentage(294.15f);
}
