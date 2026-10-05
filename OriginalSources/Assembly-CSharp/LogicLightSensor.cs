// Decompiled with JetBrains decompiler
// Type: LogicLightSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicLightSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
  private int simUpdateCounter;
  [Serialize]
  public float thresholdBrightness = 280f;
  [Serialize]
  public bool activateOnBrighterThan = true;
  public float minBrightness;
  public float maxBrightness = 15000f;
  private const int NumFrameDelay = 4;
  private float[] levels = new float[4];
  private float averageBrightness;
  private bool wasOn;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<LogicLightSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicLightSensor>((Action<LogicLightSensor, object>) ((component, data) => component.OnCopySettings(data)));

  private void OnCopySettings(object data)
  {
    LogicLightSensor component = ((GameObject) data).GetComponent<LogicLightSensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Threshold = component.Threshold;
    this.ActivateAboveThreshold = component.ActivateAboveThreshold;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicLightSensor>(-905833192, LogicLightSensor.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnToggle += new Action<bool>(this.OnSwitchToggled);
    this.UpdateVisualState(true);
    this.UpdateLogicCircuit();
    this.wasOn = this.switchedOn;
  }

  public void Sim200ms(float dt)
  {
    if (this.simUpdateCounter < 4)
    {
      this.levels[this.simUpdateCounter] = (float) Grid.LightIntensity[Grid.PosToCell((KMonoBehaviour) this)];
      ++this.simUpdateCounter;
    }
    else
    {
      this.simUpdateCounter = 0;
      this.averageBrightness = 0.0f;
      for (int index = 0; index < 4; ++index)
        this.averageBrightness += this.levels[index];
      this.averageBrightness /= 4f;
      if (this.activateOnBrighterThan)
      {
        if (((double) this.averageBrightness <= (double) this.thresholdBrightness || this.IsSwitchedOn) && ((double) this.averageBrightness >= (double) this.thresholdBrightness || !this.IsSwitchedOn))
          return;
        this.Toggle();
      }
      else
      {
        if (((double) this.averageBrightness <= (double) this.thresholdBrightness || !this.IsSwitchedOn) && ((double) this.averageBrightness >= (double) this.thresholdBrightness || this.IsSwitchedOn))
          return;
        this.Toggle();
      }
    }
  }

  private void OnSwitchToggled(bool toggled_on)
  {
    this.UpdateVisualState();
    this.UpdateLogicCircuit();
  }

  private void UpdateLogicCircuit()
  {
    this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
  }

  private void UpdateVisualState(bool force = false)
  {
    if (!(this.wasOn != this.switchedOn | force))
      return;
    this.wasOn = this.switchedOn;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) (this.switchedOn ? "on_pre" : "on_pst"));
    component.Queue((HashedString) (this.switchedOn ? "on" : "off"));
  }

  public float Threshold
  {
    get => this.thresholdBrightness;
    set => this.thresholdBrightness = value;
  }

  public bool ActivateAboveThreshold
  {
    get => this.activateOnBrighterThan;
    set => this.activateOnBrighterThan = value;
  }

  public float CurrentValue => this.averageBrightness;

  public float RangeMin => this.minBrightness;

  public float RangeMax => this.maxBrightness;

  public float GetRangeMinInputField() => this.RangeMin;

  public float GetRangeMaxInputField() => this.RangeMax;

  public LocString Title => UI.UISIDESCREENS.BRIGHTNESSSWITCHSIDESCREEN.TITLE;

  public LocString ThresholdValueName => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS;

  public string AboveToolTip
  {
    get => (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS_TOOLTIP_ABOVE;
  }

  public string BelowToolTip
  {
    get => (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS_TOOLTIP_BELOW;
  }

  public string Format(float value, bool units)
  {
    return units ? GameUtil.GetFormattedLux((int) value) : $"{(int) value}";
  }

  public float ProcessedSliderValue(float input) => Mathf.Round(input);

  public float ProcessedInputValue(float input) => input;

  public LocString ThresholdValueUnits() => UI.UNITSUFFIXES.LIGHT.LUX;

  public ThresholdScreenLayoutType LayoutType => ThresholdScreenLayoutType.SliderBar;

  public int IncrementScale => 1;

  public NonLinearSlider.Range[] GetRanges => NonLinearSlider.GetDefaultRange(this.RangeMax);

  protected override void UpdateSwitchStatus()
  {
    StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item);
  }
}
