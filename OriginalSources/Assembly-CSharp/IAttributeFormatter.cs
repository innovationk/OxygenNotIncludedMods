// Decompiled with JetBrains decompiler
// Type: IAttributeFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;

#nullable disable
public interface IAttributeFormatter
{
  GameUtil.TimeSlice DeltaTimeSlice { get; set; }

  string GetFormattedAttribute(AttributeInstance instance);

  string GetFormattedModifier(AttributeModifier modifier);

  string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice);

  string GetTooltip(Attribute master, AttributeInstance instance);

  string GetTooltip(
    Attribute master,
    List<AttributeModifier> modifiers,
    AttributeConverters converters);
}
