// Decompiled with JetBrains decompiler
// Type: StandardAmountDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Text;

#nullable disable
public class StandardAmountDisplayer : IAmountDisplayer
{
  protected StandardAttributeFormatter formatter;
  protected StandardAttributeFormatter deltaFormatter;
  public GameUtil.IdentityDescriptorTense tense;

  public IAttributeFormatter Formatter => (IAttributeFormatter) this.formatter;

  public GameUtil.TimeSlice DeltaTimeSlice
  {
    get => this.formatter.DeltaTimeSlice;
    set => this.formatter.DeltaTimeSlice = value;
  }

  public StandardAmountDisplayer(
    GameUtil.UnitClass unitClass,
    GameUtil.TimeSlice deltaTimeSlice,
    StandardAttributeFormatter formatter = null,
    GameUtil.IdentityDescriptorTense tense = GameUtil.IdentityDescriptorTense.Normal)
  {
    this.tense = tense;
    this.formatter = formatter == null ? new StandardAttributeFormatter(unitClass, deltaTimeSlice) : formatter;
    this.deltaFormatter = this.formatter;
  }

  public void SetDeltaFormatter(StandardAttributeFormatter deltaFormatter)
  {
    this.deltaFormatter = deltaFormatter;
  }

  public virtual string GetValueString(Amount master, AmountInstance instance)
  {
    return !master.showMax ? this.formatter.GetFormattedValue(instance.value) : $"{this.formatter.GetFormattedValue(instance.value)} / {this.formatter.GetFormattedValue(instance.GetMax())}";
  }

  public virtual string GetDescription(Amount master, AmountInstance instance)
  {
    return $"{master.Name}: {this.GetValueString(master, instance)}";
  }

  public virtual string GetTooltip(Amount master, AmountInstance instance)
  {
    StringBuilder sb = GlobalStringBuilderPool.Alloc();
    if (master.description.IndexOf("{1}") > -1)
      sb.AppendFormat(master.description, (object) this.formatter.GetFormattedValue(instance.value), (object) GameUtil.GetIdentityDescriptor(instance.gameObject, this.tense));
    else
      sb.AppendFormat(master.description, (object) this.formatter.GetFormattedValue(instance.value));
    sb.Append("\n\n");
    if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
      sb.AppendFormat((string) UI.CHANGEPERCYCLE, (object) this.deltaFormatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle));
    else if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerSecond)
      sb.AppendFormat((string) UI.CHANGEPERSECOND, (object) this.deltaFormatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerSecond));
    for (int i = 0; i != instance.deltaAttribute.Modifiers.Count; ++i)
    {
      AttributeModifier modifier = instance.deltaAttribute.Modifiers[i];
      sb.Append("\n");
      sb.AppendFormat((string) UI.MODIFIER_ITEM_TEMPLATE, (object) modifier.GetDescription(), (object) this.deltaFormatter.GetFormattedModifier(modifier));
    }
    return GlobalStringBuilderPool.ReturnAndFree(sb);
  }

  public string GetFormattedAttribute(AttributeInstance instance)
  {
    return this.formatter.GetFormattedAttribute(instance);
  }

  public string GetFormattedModifier(AttributeModifier modifier)
  {
    return this.formatter.GetFormattedModifier(modifier);
  }

  public string GetFormattedValue(float value, GameUtil.TimeSlice time_slice)
  {
    return this.formatter.GetFormattedValue(value, time_slice);
  }
}
