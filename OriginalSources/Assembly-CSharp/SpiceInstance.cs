// Decompiled with JetBrains decompiler
// Type: SpiceInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;

#nullable disable
[Serializable]
public struct SpiceInstance
{
  public Tag Id;
  public float TotalKG;

  public AttributeModifier CalorieModifier
  {
    get => SpiceGrinder.SettingOptions[this.Id].Spice.CalorieModifier;
  }

  public AttributeModifier FoodModifier => SpiceGrinder.SettingOptions[this.Id].Spice.FoodModifier;

  public Effect StatBonus => SpiceGrinder.SettingOptions[this.Id].StatBonus;
}
