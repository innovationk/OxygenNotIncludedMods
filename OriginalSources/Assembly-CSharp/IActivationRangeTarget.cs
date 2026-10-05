// Decompiled with JetBrains decompiler
// Type: IActivationRangeTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public interface IActivationRangeTarget
{
  float ActivateValue { get; set; }

  float DeactivateValue { get; set; }

  float MinValue { get; }

  float MaxValue { get; }

  bool UseWholeNumbers { get; }

  string ActivationRangeTitleText { get; }

  string ActivateSliderLabelText { get; }

  string DeactivateSliderLabelText { get; }

  string ActivateTooltip { get; }

  string DeactivateTooltip { get; }
}
