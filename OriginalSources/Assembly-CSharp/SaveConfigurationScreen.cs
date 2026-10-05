// Decompiled with JetBrains decompiler
// Type: SaveConfigurationScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
[Serializable]
public class SaveConfigurationScreen
{
  [SerializeField]
  private KSlider autosaveFrequencySlider;
  [SerializeField]
  private LocText timelapseDescriptionLabel;
  [SerializeField]
  private KSlider timelapseResolutionSlider;
  [SerializeField]
  private LocText autosaveDescriptionLabel;
  private int[] sliderValueToCycleCount = new int[7]
  {
    -1,
    50,
    20,
    10,
    5,
    2,
    1
  };
  private Vector2I[] sliderValueToResolution = new Vector2I[7]
  {
    new Vector2I(-1, -1),
    new Vector2I(256 /*0x0100*/, 384),
    new Vector2I(512 /*0x0200*/, 768 /*0x0300*/),
    new Vector2I(1024 /*0x0400*/, 1536 /*0x0600*/),
    new Vector2I(2048 /*0x0800*/, 3072 /*0x0C00*/),
    new Vector2I(4096 /*0x1000*/, 6144),
    new Vector2I(8192 /*0x2000*/, 12288 /*0x3000*/)
  };
  [SerializeField]
  private GameObject disabledContentPanel;
  [SerializeField]
  private GameObject disabledContentWarning;
  [SerializeField]
  private GameObject perSaveWarning;

  public void ToggleDisabledContent(bool enable)
  {
    if (enable)
    {
      this.disabledContentPanel.SetActive(true);
      this.disabledContentWarning.SetActive(false);
      this.perSaveWarning.SetActive(true);
    }
    else
    {
      this.disabledContentPanel.SetActive(false);
      this.disabledContentWarning.SetActive(true);
      this.perSaveWarning.SetActive(false);
    }
  }

  public void Init()
  {
    this.autosaveFrequencySlider.minValue = 0.0f;
    this.autosaveFrequencySlider.maxValue = (float) (this.sliderValueToCycleCount.Length - 1);
    this.autosaveFrequencySlider.onValueChanged.AddListener((UnityAction<float>) (val => this.OnAutosaveValueChanged(Mathf.FloorToInt(val))));
    this.autosaveFrequencySlider.value = (float) this.CycleCountToSlider(SaveGame.Instance.AutoSaveCycleInterval);
    this.timelapseResolutionSlider.minValue = 0.0f;
    this.timelapseResolutionSlider.maxValue = (float) (this.sliderValueToResolution.Length - 1);
    this.timelapseResolutionSlider.onValueChanged.AddListener((UnityAction<float>) (val => this.OnTimelapseValueChanged(Mathf.FloorToInt(val))));
    this.timelapseResolutionSlider.value = (float) this.ResolutionToSliderValue(SaveGame.Instance.TimelapseResolution);
    this.OnTimelapseValueChanged(Mathf.FloorToInt(this.timelapseResolutionSlider.value));
  }

  public void Show(bool show)
  {
    if (!show)
      return;
    this.autosaveFrequencySlider.value = (float) this.CycleCountToSlider(SaveGame.Instance.AutoSaveCycleInterval);
    this.timelapseResolutionSlider.value = (float) this.ResolutionToSliderValue(SaveGame.Instance.TimelapseResolution);
    this.OnAutosaveValueChanged(Mathf.FloorToInt(this.autosaveFrequencySlider.value));
    this.OnTimelapseValueChanged(Mathf.FloorToInt(this.timelapseResolutionSlider.value));
  }

  private void OnTimelapseValueChanged(int sliderValue)
  {
    Vector2I resolution = this.SliderValueToResolution(sliderValue);
    if (resolution.x <= 0)
      this.timelapseDescriptionLabel.SetText((string) UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.TIMELAPSE_DISABLED_DESCRIPTION);
    else
      this.timelapseDescriptionLabel.SetText(string.Format((string) UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.TIMELAPSE_RESOLUTION_DESCRIPTION, (object) resolution.x, (object) resolution.y));
    SaveGame.Instance.TimelapseResolution = resolution;
    Game.Instance.Trigger(75424175, (object) null);
  }

  private void OnAutosaveValueChanged(int sliderValue)
  {
    int cycleCount = this.SliderValueToCycleCount(sliderValue);
    if (sliderValue == 0)
      this.autosaveDescriptionLabel.SetText((string) UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.AUTOSAVE_NEVER);
    else
      this.autosaveDescriptionLabel.SetText(string.Format((string) UI.FRONTEND.COLONY_SAVE_OPTIONS_SCREEN.AUTOSAVE_FREQUENCY_DESCRIPTION, (object) cycleCount));
    SaveGame.Instance.AutoSaveCycleInterval = cycleCount;
  }

  private int SliderValueToCycleCount(int sliderValue) => this.sliderValueToCycleCount[sliderValue];

  private int CycleCountToSlider(int count)
  {
    for (int slider = 0; slider < this.sliderValueToCycleCount.Length; ++slider)
    {
      if (this.sliderValueToCycleCount[slider] == count)
        return slider;
    }
    return 0;
  }

  private Vector2I SliderValueToResolution(int sliderValue)
  {
    return this.sliderValueToResolution[sliderValue];
  }

  private int ResolutionToSliderValue(Vector2I resolution)
  {
    for (int sliderValue = 0; sliderValue < this.sliderValueToResolution.Length; ++sliderValue)
    {
      if (this.sliderValueToResolution[sliderValue] == resolution)
        return sliderValue;
    }
    return 0;
  }
}
