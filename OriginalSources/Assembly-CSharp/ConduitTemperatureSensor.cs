// Decompiled with JetBrains decompiler
// Type: ConduitTemperatureSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitTemperatureSensor : ConduitThresholdSensor, IThresholdSwitch
{
  public float rangeMin;
  public float rangeMax = 373.15f;
  [Serialize]
  private float lastValue;

  private void GetContentsTemperature(out float temperature, out bool hasMass)
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
    {
      ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
      temperature = contents.temperature;
      hasMass = (double) contents.mass > 0.0;
    }
    else
    {
      SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
      Pickupable pickupable = flowManager.GetPickupable(flowManager.GetContents(cell).pickupableHandle);
      if ((Object) pickupable != (Object) null && (double) pickupable.PrimaryElement.Mass > 0.0)
      {
        temperature = pickupable.PrimaryElement.Temperature;
        hasMass = true;
      }
      else
      {
        temperature = 0.0f;
        hasMass = false;
      }
    }
  }

  public override float CurrentValue
  {
    get
    {
      float temperature;
      bool hasMass;
      this.GetContentsTemperature(out temperature, out hasMass);
      if (hasMass)
        this.lastValue = temperature;
      return this.lastValue;
    }
  }

  public float RangeMin => this.rangeMin;

  public float RangeMax => this.rangeMax;

  public float GetRangeMinInputField() => GameUtil.GetConvertedTemperature(this.RangeMin);

  public float GetRangeMaxInputField() => GameUtil.GetConvertedTemperature(this.RangeMax);

  public LocString Title => UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;

  public LocString ThresholdValueName
  {
    get => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE;
  }

  public string AboveToolTip
  {
    get => (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE_TOOLTIP_ABOVE;
  }

  public string BelowToolTip
  {
    get => (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE_TOOLTIP_BELOW;
  }

  public string Format(float value, bool units)
  {
    return GameUtil.GetFormattedTemperature(value, displayUnits: units);
  }

  public float ProcessedSliderValue(float input) => Mathf.Round(input);

  public float ProcessedInputValue(float input) => GameUtil.GetTemperatureConvertedToKelvin(input);

  public LocString ThresholdValueUnits()
  {
    LocString locString = (LocString) null;
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        locString = UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
        break;
      case GameUtil.TemperatureUnit.Fahrenheit:
        locString = UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
        break;
      case GameUtil.TemperatureUnit.Kelvin:
        locString = UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
        break;
    }
    return locString;
  }

  public ThresholdScreenLayoutType LayoutType => ThresholdScreenLayoutType.SliderBar;

  public int IncrementScale => 1;

  public NonLinearSlider.Range[] GetRanges
  {
    get
    {
      return new NonLinearSlider.Range[4]
      {
        new NonLinearSlider.Range(25f, 260f),
        new NonLinearSlider.Range(50f, 400f),
        new NonLinearSlider.Range(12f, 1500f),
        new NonLinearSlider.Range(13f, 10000f)
      };
    }
  }
}
