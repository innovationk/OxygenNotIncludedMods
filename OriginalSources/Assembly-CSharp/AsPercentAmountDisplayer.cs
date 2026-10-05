// Decompiled with JetBrains decompiler
// Type: AsPercentAmountDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Text;

#nullable disable
public class AsPercentAmountDisplayer : IAmountDisplayer
{
  protected StandardAttributeFormatter formatter;

  public IAttributeFormatter Formatter => (IAttributeFormatter) this.formatter;

  public GameUtil.TimeSlice DeltaTimeSlice
  {
    get => this.formatter.DeltaTimeSlice;
    set => this.formatter.DeltaTimeSlice = value;
  }

  public AsPercentAmountDisplayer(GameUtil.TimeSlice deltaTimeSlice)
  {
    this.formatter = new StandardAttributeFormatter(GameUtil.UnitClass.Percent, deltaTimeSlice);
  }

  public string GetValueString(Amount master, AmountInstance instance)
  {
    return this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance));
  }

  public virtual string GetDescription(Amount master, AmountInstance instance)
  {
    return $"{master.Name}: {this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance))}";
  }

  public virtual string GetTooltipDescription(Amount master, AmountInstance instance)
  {
    return string.Format(master.description, (object) this.formatter.GetFormattedValue(instance.value));
  }

  public virtual string GetTooltip(Amount master, AmountInstance instance)
  {
    StringBuilder sb = GlobalStringBuilderPool.Alloc();
    sb.Append(this.GetTooltipDescription(master, instance));
    sb.Append("\n\n");
    if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
      sb.AppendFormat((string) UI.CHANGEPERCYCLE, (object) this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerCycle));
    else
      sb.AppendFormat((string) UI.CHANGEPERSECOND, (object) this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerSecond));
    for (int i = 0; i != instance.deltaAttribute.Modifiers.Count; ++i)
    {
      AttributeModifier modifier = instance.deltaAttribute.Modifiers[i];
      float modifierContribution = instance.deltaAttribute.GetModifierContribution(modifier);
      sb.Append("\n");
      sb.AppendFormat((string) UI.MODIFIER_ITEM_TEMPLATE, (object) modifier.GetDescription(), (object) this.formatter.GetFormattedValue(this.ToPercent(modifierContribution, instance), this.formatter.DeltaTimeSlice));
    }
    return GlobalStringBuilderPool.ReturnAndFree(sb);
  }

  public string GetFormattedAttribute(AttributeInstance instance)
  {
    return this.formatter.GetFormattedAttribute(instance);
  }

  public string GetFormattedModifier(AttributeModifier modifier)
  {
    return modifier.IsMultiplier ? GameUtil.GetFormattedPercent(modifier.Value * 100f) : this.formatter.GetFormattedModifier(modifier);
  }

  public string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
  {
    return this.formatter.GetFormattedValue(value, timeSlice);
  }

  protected float ToPercent(float value, AmountInstance instance)
  {
    return 100f * value / instance.GetMax();
  }
}
