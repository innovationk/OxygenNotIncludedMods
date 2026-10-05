// Decompiled with JetBrains decompiler
// Type: BionicBatteryDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Text;
using UnityEngine;

#nullable disable
public class BionicBatteryDisplayer : StandardAmountDisplayer
{
  private const float criticalIconFlashFrequency = 0.45f;

  private string GetIconForState(BionicBatteryDisplayer.ElectrobankState state)
  {
    string iconForState;
    switch (state)
    {
      case BionicBatteryDisplayer.ElectrobankState.Unexistent:
        iconForState = BionicBatteryMonitor.EmptySlotBatteryIcon;
        break;
      case BionicBatteryDisplayer.ElectrobankState.Charged:
        iconForState = BionicBatteryMonitor.ChargedBatteryIcon;
        break;
      default:
        iconForState = BionicBatteryMonitor.DischargedBatteryIcon;
        break;
    }
    return iconForState;
  }

  public override string GetTooltip(Amount master, AmountInstance instance)
  {
    StringBuilder sb = GlobalStringBuilderPool.Alloc();
    BionicBatteryMonitor.Instance smi = instance.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
    float totalDisplayValue = instance.deltaAttribute.GetTotalDisplayValue();
    if (smi != null)
    {
      float wattage = smi.Wattage;
      totalDisplayValue += wattage;
    }
    if (master.description.IndexOf("{1}") > -1)
      sb.AppendFormat(master.description, (object) this.formatter.GetFormattedValue(instance.value), (object) GameUtil.GetIdentityDescriptor(instance.gameObject, this.tense));
    else
      sb.AppendFormat(master.description, (object) this.formatter.GetFormattedValue(instance.value));
    if (smi != null)
    {
      int electrobankCount = smi.ElectrobankCount;
      int electrobankCountCapacity = smi.ElectrobankCountCapacity;
      sb.Append("\n\n");
      sb.AppendFormat((string) DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.ELECTROBANK_DETAILS_LABEL, (object) GameUtil.GetFormattedInt((float) electrobankCount), (object) GameUtil.GetFormattedInt((float) electrobankCountCapacity));
      if (electrobankCount > 0)
      {
        for (int index = 0; index < smi.storage.items.Count; ++index)
        {
          GameObject go = smi.storage.items[index];
          Electrobank component = go.GetComponent<Electrobank>();
          string iconForState = this.GetIconForState((Object) component == (Object) null ? BionicBatteryDisplayer.ElectrobankState.Damaged : ((double) component.Charge <= 0.0 ? BionicBatteryDisplayer.ElectrobankState.Depleated : BionicBatteryDisplayer.ElectrobankState.Charged));
          float joules = (Object) component == (Object) null ? 0.0f : component.Charge;
          sb.Append("\n");
          sb.Append("    • ");
          sb.AppendFormat((string) DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.ELECTROBANK_ROW, (object) iconForState, (object) go.GetProperName(), (object) GameUtil.GetFormattedJoules(joules));
        }
      }
      if (electrobankCount < electrobankCountCapacity)
      {
        for (int index = 0; index < electrobankCountCapacity - electrobankCount; ++index)
        {
          sb.Append("\n");
          sb.Append("    • ");
          sb.AppendFormat((string) DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.ELECTROBANK_EMPTY_ROW, (object) this.GetIconForState(BionicBatteryDisplayer.ElectrobankState.Unexistent));
        }
      }
    }
    sb.Append("\n\n");
    sb.AppendFormat((string) DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.CURRENT_WATTAGE_LABEL, (object) this.formatter.GetFormattedValue(totalDisplayValue, this.formatter.DeltaTimeSlice));
    if (smi != null)
    {
      StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
      stringBuilder.Append("<b>+</b>");
      GameUtil.AppendFormattedWattage(stringBuilder, smi.GetBaseWattage());
      sb.Append("\n");
      sb.Append("    • ");
      sb.AppendFormat((string) DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_ACTIVE_TEMPLATE, (object) DUPLICANTS.MODIFIERS.BIONIC_WATTS.BASE_NAME, (object) stringBuilder.ToString());
      stringBuilder.Clear();
      float num = 0.0f;
      foreach (BionicBatteryMonitor.WattageModifier modifier in smi.Modifiers)
      {
        if ((double) modifier.value != 0.0)
        {
          sb.Append("\n");
          sb.Append("    • ");
          sb.Append(modifier.name);
        }
        else if ((double) modifier.potentialValue > 0.0)
        {
          stringBuilder.Append("\n");
          stringBuilder.Append("    • ");
          stringBuilder.Append(modifier.name);
          num += modifier.potentialValue;
        }
      }
      if (stringBuilder.Length != 0)
      {
        sb.Append("\n\n");
        sb.AppendFormat((string) DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.POTENTIAL_EXTRA_WATTAGE_LABEL, (object) this.formatter.GetFormattedValue(num, this.formatter.DeltaTimeSlice));
        sb.Append(stringBuilder.ToString());
      }
      GlobalStringBuilderPool.Free(stringBuilder);
    }
    Debug.Assert(instance.deltaAttribute.Modifiers.Count <= 0, (object) "Bionic Battery Displayer has found an invalid AttributeModifier. This particular Amount should not use AttributeModifiers, instead, use BionicBatteryMonitor.Instance.Modifiers");
    float seconds = (double) totalDisplayValue == 0.0 ? 0.0f : smi.CurrentCharge / totalDisplayValue;
    sb.Append("\n\n");
    sb.AppendFormat((string) DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.ESTIMATED_LIFE_TIME_REMAINING, (object) GameUtil.GetFormattedCycles(seconds));
    return GlobalStringBuilderPool.ReturnAndFree(sb);
  }

  public override string GetValueString(Amount master, AmountInstance instance)
  {
    return base.GetValueString(master, instance);
  }

  public BionicBatteryDisplayer()
    : base(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond)
  {
    this.formatter = (StandardAttributeFormatter) new BionicBatteryDisplayer.BionicBatteryAttributeFormatter();
  }

  private enum ElectrobankState
  {
    Unexistent,
    Damaged,
    Depleated,
    Charged,
  }

  public class BionicBatteryAttributeFormatter : StandardAttributeFormatter
  {
    public BionicBatteryAttributeFormatter()
      : base(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond)
    {
    }
  }
}
