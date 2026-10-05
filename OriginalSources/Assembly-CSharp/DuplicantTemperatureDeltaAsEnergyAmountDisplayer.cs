// Decompiled with JetBrains decompiler
// Type: DuplicantTemperatureDeltaAsEnergyAmountDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Text;
using TUNING;

#nullable disable
public class DuplicantTemperatureDeltaAsEnergyAmountDisplayer(
  GameUtil.UnitClass unitClass,
  GameUtil.TimeSlice timeSlice) : StandardAmountDisplayer(unitClass, timeSlice)
{
  public override string GetTooltip(Amount master, AmountInstance instance)
  {
    StringBuilder sb = GlobalStringBuilderPool.Alloc();
    sb.AppendFormat(master.description, (object) this.formatter.GetFormattedValue(instance.value), (object) this.formatter.GetFormattedValue(DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL));
    float num1 = (float) ((double) ElementLoader.FindElementByHash(SimHashes.Creature).specificHeatCapacity * (double) DUPLICANTSTATS.STANDARD.BaseStats.DEFAULT_MASS * 1000.0);
    float num2 = 0.0f;
    float num3 = 0.0f;
    for (int i = 0; i != instance.deltaAttribute.Modifiers.Count; ++i)
    {
      AttributeModifier modifier = instance.deltaAttribute.Modifiers[i];
      float num4 = modifier.Value;
      if (modifier.GetDescription() == CreatureSimTemperatureTransfer.RESULT_MODIFIER_NAME)
      {
        num2 = num4 * 5f;
        num3 += num4 * 5f;
      }
      else
        num3 += num4;
    }
    sb.Append("\n\n");
    if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
    {
      sb.AppendFormat((string) UI.CHANGEPERCYCLE, (object) this.formatter.GetFormattedValue(num3, GameUtil.TimeSlice.PerCycle));
    }
    else
    {
      sb.AppendFormat((string) UI.CHANGEPERSECOND, (object) this.formatter.GetFormattedValue(num3, GameUtil.TimeSlice.PerSecond));
      sb.Append("\n");
      sb.AppendFormat((string) UI.CHANGEPERSECOND, (object) GameUtil.GetFormattedJoules(num3 * num1));
    }
    for (int i = 0; i != instance.deltaAttribute.Modifiers.Count; ++i)
    {
      AttributeModifier modifier = instance.deltaAttribute.Modifiers[i];
      float num5 = modifier.Value;
      string description = modifier.GetDescription();
      if (description == CreatureSimTemperatureTransfer.RESULT_MODIFIER_NAME)
        num5 = num2;
      sb.Append("\n");
      sb.AppendFormat((string) UI.MODIFIER_ITEM_TEMPLATE, (object) description, (object) GameUtil.GetFormattedHeatEnergyRate((float) ((double) num5 * (double) num1 * 1.0)));
    }
    return GlobalStringBuilderPool.ReturnAndFree(sb);
  }
}
