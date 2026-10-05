// Decompiled with JetBrains decompiler
// Type: DevLightGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class DevLightGenerator : Light2D, IMultiSliderControl
{
  protected ISliderControl[] sliderControls;

  public DevLightGenerator()
  {
    this.sliderControls = new ISliderControl[3]
    {
      (ISliderControl) new DevLightGenerator.LuxController((Light2D) this),
      (ISliderControl) new DevLightGenerator.RangeController((Light2D) this),
      (ISliderControl) new DevLightGenerator.FalloffController((Light2D) this)
    };
  }

  string IMultiSliderControl.SidescreenTitleKey
  {
    get => "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.NAME";
  }

  ISliderControl[] IMultiSliderControl.sliderControls => this.sliderControls;

  bool IMultiSliderControl.SidescreenEnabled() => true;

  protected class LuxController : ISingleSliderControl, ISliderControl
  {
    protected Light2D target;

    public LuxController(Light2D t) => this.target = t;

    public string SliderTitleKey => "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.BRIGHTNESS_LABEL";

    public string SliderUnits => (string) UI.UNITSUFFIXES.LIGHT.LUX;

    public float GetSliderMax(int index) => 100000f;

    public float GetSliderMin(int index) => 0.0f;

    public string GetSliderTooltip(int index)
    {
      return string.Format((string) UI.GAMEOBJECTEFFECTS.EMITS_LIGHT_LUX, (object) this.target.Lux);
    }

    public string GetSliderTooltipKey(int index) => "<unused>";

    public float GetSliderValue(int index) => (float) this.target.Lux;

    public void SetSliderValue(float value, int index)
    {
      this.target.Lux = (int) value;
      this.target.FullRefresh();
    }

    public int SliderDecimalPlaces(int index) => 0;
  }

  protected class RangeController : ISingleSliderControl, ISliderControl
  {
    protected Light2D target;

    public RangeController(Light2D t) => this.target = t;

    public string SliderTitleKey => "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.RANGE_LABEL";

    public string SliderUnits => (string) UI.UNITSUFFIXES.TILES;

    public float GetSliderMax(int index) => 20f;

    public float GetSliderMin(int index) => 1f;

    public string GetSliderTooltip(int index)
    {
      return string.Format((string) UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, (object) this.target.Range);
    }

    public string GetSliderTooltipKey(int index) => "";

    public float GetSliderValue(int index) => this.target.Range;

    public void SetSliderValue(float value, int index)
    {
      this.target.Range = (float) (int) value;
      this.target.FullRefresh();
    }

    public int SliderDecimalPlaces(int index) => 0;
  }

  protected class FalloffController : ISingleSliderControl, ISliderControl
  {
    protected Light2D target;

    public FalloffController(Light2D t) => this.target = t;

    public string SliderTitleKey => "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.FALLOFF_LABEL";

    public string SliderUnits => (string) UI.UNITSUFFIXES.PERCENT;

    public float GetSliderMax(int index) => 100f;

    public float GetSliderMin(int index) => 1f;

    public string GetSliderTooltip(int index)
    {
      return $"{(ValueType) (float) ((double) this.target.FalloffRate * 100.0)}";
    }

    public string GetSliderTooltipKey(int index) => "";

    public float GetSliderValue(int index) => this.target.FalloffRate * 100f;

    public void SetSliderValue(float value, int index)
    {
      this.target.FalloffRate = value / 100f;
      this.target.FullRefresh();
    }

    public int SliderDecimalPlaces(int index) => 0;
  }
}
