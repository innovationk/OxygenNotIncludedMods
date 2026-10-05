// Decompiled with JetBrains decompiler
// Type: ISliderControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public interface ISliderControl
{
  string SliderTitleKey { get; }

  string SliderUnits { get; }

  int SliderDecimalPlaces(int index);

  float GetSliderMin(int index);

  float GetSliderMax(int index);

  float GetSliderValue(int index);

  void SetSliderValue(float percent, int index);

  string GetSliderTooltipKey(int index);

  string GetSliderTooltip(int index);
}
