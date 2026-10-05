// Decompiled with JetBrains decompiler
// Type: StandardAttributeFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class StandardAttributeFormatter : IAttributeFormatter
{
  public GameUtil.UnitClass unitClass;

  public GameUtil.TimeSlice DeltaTimeSlice { get; set; }

  public StandardAttributeFormatter(GameUtil.UnitClass unitClass, GameUtil.TimeSlice deltaTimeSlice)
  {
    this.unitClass = unitClass;
    this.DeltaTimeSlice = deltaTimeSlice;
  }

  public virtual string GetFormattedAttribute(AttributeInstance instance)
  {
    return this.GetFormattedValue(instance.GetTotalDisplayValue());
  }

  public virtual string GetFormattedModifier(AttributeModifier modifier)
  {
    return this.GetFormattedValue(modifier.Value, modifier.OverrideTimeSlice.HasValue ? modifier.OverrideTimeSlice.Value : this.DeltaTimeSlice);
  }

  public virtual string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    switch (this.unitClass)
    {
      case GameUtil.UnitClass.SimpleInteger:
        return GameUtil.GetFormattedInt(value, timeSlice);
      case GameUtil.UnitClass.Temperature:
        return GameUtil.GetFormattedTemperature(value, timeSlice, timeSlice == GameUtil.TimeSlice.None ? GameUtil.TemperatureInterpretation.Absolute : GameUtil.TemperatureInterpretation.Relative);
      case GameUtil.UnitClass.Mass:
        return GameUtil.GetFormattedMass(value, timeSlice);
      case GameUtil.UnitClass.Calories:
        return GameUtil.GetFormattedCalories(value, timeSlice);
      case GameUtil.UnitClass.Percent:
        return GameUtil.GetFormattedPercent(value, timeSlice);
      case GameUtil.UnitClass.Distance:
        return GameUtil.GetFormattedDistance(value);
      case GameUtil.UnitClass.Disease:
        return GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(value));
      case GameUtil.UnitClass.Radiation:
        return GameUtil.GetFormattedRads(value, timeSlice);
      case GameUtil.UnitClass.Energy:
        return GameUtil.GetFormattedJoules(value, timeSlice: timeSlice);
      case GameUtil.UnitClass.Power:
        return GameUtil.GetFormattedWattage(value);
      case GameUtil.UnitClass.Lux:
        return GameUtil.GetFormattedLux(Mathf.FloorToInt(value));
      case GameUtil.UnitClass.Time:
        return GameUtil.GetFormattedCycles(value);
      case GameUtil.UnitClass.Seconds:
        return GameUtil.GetFormattedTime(value);
      case GameUtil.UnitClass.Cycles:
        return GameUtil.GetFormattedCycles(value * 600f);
      default:
        return GameUtil.GetFormattedSimple(value, timeSlice);
    }
  }

  public virtual string GetTooltipDescription(Klei.AI.Attribute master) => master.Description;

  public virtual string GetTooltip(Klei.AI.Attribute master, AttributeInstance instance)
  {
    List<AttributeModifier> modifiers = new List<AttributeModifier>();
    for (int i = 0; i < instance.Modifiers.Count; ++i)
      modifiers.Add(instance.Modifiers[i]);
    return this.GetTooltip(master, modifiers, instance.GetComponent<AttributeConverters>());
  }

  public string GetTooltip(
    Klei.AI.Attribute master,
    List<AttributeModifier> modifiers,
    AttributeConverters converters)
  {
    StringBuilder sb = GlobalStringBuilderPool.Alloc();
    sb.Append(this.GetTooltipDescription(master));
    sb.AppendFormat((string) DUPLICANTS.ATTRIBUTES.TOTAL_VALUE, (object) this.GetFormattedValue(AttributeInstance.GetTotalDisplayValue(master, modifiers)), (object) master.Name);
    if ((double) master.BaseValue != 0.0)
      sb.AppendFormat((string) DUPLICANTS.ATTRIBUTES.BASE_VALUE, (object) master.BaseValue);
    List<AttributeModifier> attributeModifierList = new List<AttributeModifier>((IEnumerable<AttributeModifier>) modifiers);
    attributeModifierList.Sort((Comparison<AttributeModifier>) ((p1, p2) => p2.Value.CompareTo(p1.Value)));
    for (int index = 0; index != attributeModifierList.Count; ++index)
    {
      AttributeModifier attributeModifier = attributeModifierList[index];
      string formattedString = attributeModifier.GetFormattedString();
      if (formattedString != null)
        sb.AppendFormat((string) DUPLICANTS.ATTRIBUTES.MODIFIER_ENTRY, (object) attributeModifier.GetDescription(), (object) formattedString);
    }
    bool flag = true;
    if ((UnityEngine.Object) converters != (UnityEngine.Object) null && master.converters.Count > 0)
    {
      foreach (AttributeConverterInstance converter in converters.converters)
      {
        if (converter.converter.attribute == master)
        {
          string str = converter.DescriptionFromAttribute(converter.Evaluate(), converter.gameObject);
          if (str != null)
          {
            if (flag)
            {
              sb.AppendLine();
              flag = false;
            }
            sb.AppendLine();
            sb.Append(str);
          }
        }
      }
    }
    return GlobalStringBuilderPool.ReturnAndFree(sb);
  }
}
