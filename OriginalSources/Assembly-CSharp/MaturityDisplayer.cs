// Decompiled with JetBrains decompiler
// Type: MaturityDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Text;
using UnityEngine;

#nullable disable
public class MaturityDisplayer : AsPercentAmountDisplayer
{
  public MaturityDisplayer()
    : base(GameUtil.TimeSlice.PerCycle)
  {
    this.formatter = (StandardAttributeFormatter) new MaturityDisplayer.MaturityAttributeFormatter();
  }

  public override string GetTooltipDescription(Amount master, AmountInstance instance)
  {
    StringBuilder sb = GlobalStringBuilderPool.Alloc();
    sb.Append(base.GetTooltipDescription(master, instance));
    Growing component = instance.gameObject.GetComponent<Growing>();
    if (component.IsGrowing())
    {
      float seconds = (instance.GetMax() - instance.value) / instance.GetDelta();
      if ((Object) component != (Object) null && component.IsGrowing())
        sb.AppendFormat((string) CREATURES.STATS.MATURITY.TOOLTIP_GROWING_CROP, (object) GameUtil.GetFormattedCycles(seconds), (object) GameUtil.GetFormattedCycles(component.TimeUntilNextHarvest()));
      else
        sb.AppendFormat((string) CREATURES.STATS.MATURITY.TOOLTIP_GROWING, (object) GameUtil.GetFormattedCycles(seconds));
    }
    else if (component.ReachedNextHarvest())
      sb.Append((string) CREATURES.STATS.MATURITY.TOOLTIP_GROWN);
    else
      sb.Append((string) CREATURES.STATS.MATURITY.TOOLTIP_STALLED);
    return GlobalStringBuilderPool.ReturnAndFree(sb);
  }

  public override string GetDescription(Amount master, AmountInstance instance)
  {
    Growing component = instance.gameObject.GetComponent<Growing>();
    return (Object) component != (Object) null && component.IsGrowing() ? string.Format((string) CREATURES.STATS.MATURITY.AMOUNT_DESC_FMT, (object) master.Name, (object) this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance)), (object) GameUtil.GetFormattedCycles(component.TimeUntilNextHarvest())) : base.GetDescription(master, instance);
  }

  public class MaturityAttributeFormatter : StandardAttributeFormatter
  {
    public MaturityAttributeFormatter()
      : base(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None)
    {
    }

    public override string GetFormattedModifier(AttributeModifier modifier)
    {
      float num = modifier.Value;
      GameUtil.TimeSlice timeSlice = this.DeltaTimeSlice;
      if (modifier.IsMultiplier)
      {
        num *= 100f;
        timeSlice = GameUtil.TimeSlice.None;
      }
      return this.GetFormattedValue(num, timeSlice);
    }
  }
}
