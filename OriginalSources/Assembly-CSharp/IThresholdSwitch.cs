// Decompiled with JetBrains decompiler
// Type: IThresholdSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public interface IThresholdSwitch
{
  float Threshold { get; set; }

  bool ActivateAboveThreshold { get; set; }

  float CurrentValue { get; }

  float RangeMin { get; }

  float RangeMax { get; }

  float GetRangeMinInputField();

  float GetRangeMaxInputField();

  LocString Title { get; }

  LocString ThresholdValueName { get; }

  LocString ThresholdValueUnits();

  string Format(float value, bool units);

  string AboveToolTip { get; }

  string BelowToolTip { get; }

  float ProcessedSliderValue(float input);

  float ProcessedInputValue(float input);

  ThresholdScreenLayoutType LayoutType { get; }

  int IncrementScale { get; }

  NonLinearSlider.Range[] GetRanges { get; }
}
