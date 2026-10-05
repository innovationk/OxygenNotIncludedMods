// Decompiled with JetBrains decompiler
// Type: BionicGunkDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Text;
using UnityEngine;

#nullable disable
public class BionicGunkDisplayer(GameUtil.TimeSlice deltaTimeSlice) : AsPercentAmountDisplayer(deltaTimeSlice)
{
  public override string GetTooltip(Amount master, AmountInstance instance)
  {
    StringBuilder sb = GlobalStringBuilderPool.Alloc();
    BionicOilMonitor.Instance smi = instance.gameObject.GetSMI<BionicOilMonitor.Instance>();
    AmountInstance oilAmount = smi == null ? (AmountInstance) null : smi.oilAmount;
    sb.AppendFormat(master.description, (object) this.formatter.GetFormattedValue(instance.value));
    sb.Append("\n\n");
    float totalDisplayValue1 = instance.deltaAttribute.GetTotalDisplayValue();
    if (smi != null)
    {
      float totalDisplayValue2 = oilAmount.deltaAttribute.GetTotalDisplayValue();
      if ((double) totalDisplayValue2 < 0.0)
        totalDisplayValue1 += Mathf.Abs(totalDisplayValue2);
    }
    if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
      sb.AppendFormat((string) UI.CHANGEPERCYCLE, (object) this.formatter.GetFormattedValue(this.ToPercent(totalDisplayValue1, instance), GameUtil.TimeSlice.PerCycle));
    else
      sb.AppendFormat((string) UI.CHANGEPERSECOND, (object) this.formatter.GetFormattedValue(this.ToPercent(totalDisplayValue1, instance), GameUtil.TimeSlice.PerSecond));
    if (smi != null)
    {
      for (int i = 0; i != oilAmount.deltaAttribute.Modifiers.Count; ++i)
      {
        AttributeModifier modifier = oilAmount.deltaAttribute.Modifiers[i];
        float modifierContribution = oilAmount.deltaAttribute.GetModifierContribution(modifier);
        if ((double) modifierContribution < 0.0)
        {
          float num = Mathf.Abs(modifierContribution);
          sb.Append("\n");
          sb.AppendFormat((string) UI.MODIFIER_ITEM_TEMPLATE, (object) modifier.GetDescription(), (object) this.formatter.GetFormattedValue(this.ToPercent(num, instance), this.formatter.DeltaTimeSlice));
        }
      }
    }
    for (int i = 0; i != instance.deltaAttribute.Modifiers.Count; ++i)
    {
      AttributeModifier modifier = instance.deltaAttribute.Modifiers[i];
      float modifierContribution = instance.deltaAttribute.GetModifierContribution(modifier);
      sb.Append("\n");
      sb.AppendFormat((string) UI.MODIFIER_ITEM_TEMPLATE, (object) modifier.GetDescription(), (object) this.formatter.GetFormattedValue(this.ToPercent(modifierContribution, instance), this.formatter.DeltaTimeSlice));
    }
    return GlobalStringBuilderPool.ReturnAndFree(sb);
  }
}
