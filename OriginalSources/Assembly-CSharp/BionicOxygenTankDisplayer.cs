// Decompiled with JetBrains decompiler
// Type: BionicOxygenTankDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Text;
using UnityEngine;

#nullable disable
public class BionicOxygenTankDisplayer(
  GameUtil.UnitClass unitClass,
  GameUtil.TimeSlice deltaTimeSlice) : StandardAmountDisplayer(unitClass, deltaTimeSlice)
{
  public override string GetTooltip(Amount master, AmountInstance instance)
  {
    StringBuilder sb = GlobalStringBuilderPool.Alloc();
    BionicOxygenTankMonitor.Instance smi = instance.gameObject.GetSMI<BionicOxygenTankMonitor.Instance>();
    sb.AppendFormat(master.description, (object) this.formatter.GetFormattedValue(instance.value));
    sb.Append("\n\n");
    sb.AppendFormat((string) DUPLICANTS.STATS.BIONICOXYGENTANK.TOOLTIP_MASS_LINE, (object) GameUtil.GetFormattedMass(instance.value), (object) GameUtil.GetFormattedMass(instance.GetMax()));
    if (smi != null)
    {
      foreach (GameObject gameObject in smi.storage.items)
      {
        if ((Object) gameObject != (Object) null)
        {
          PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
          if ((Object) component != (Object) null && (double) component.Mass > 0.0)
          {
            string str = (component.DiseaseIdx == byte.MaxValue ? 0 : (component.DiseaseCount > 0 ? 1 : 0)) != 0 ? string.Format((string) DUPLICANTS.STATS.BIONICOXYGENTANK.TOOLTIP_GERM_DETAIL, (object) GameUtil.GetFormattedDisease(component.DiseaseIdx, component.DiseaseCount)) : "";
            sb.Append("\n");
            sb.AppendFormat((string) DUPLICANTS.STATS.BIONICOXYGENTANK.TOOLTIP_MASS_ROW_DETAIL, (object) component.Element.name, (object) GameUtil.GetFormattedMass(component.Mass), (object) str);
          }
        }
      }
    }
    sb.Append("\n\n");
    float totalDisplayValue = instance.deltaAttribute.GetTotalDisplayValue();
    if (smi != null)
    {
      float totalValue = smi.airConsumptionRate.GetTotalValue();
      totalDisplayValue += totalValue;
    }
    sb.AppendFormat((string) UI.CHANGEPERSECOND, (object) this.formatter.GetFormattedValue(totalDisplayValue, GameUtil.TimeSlice.PerSecond));
    Debug.Assert(instance.deltaAttribute.Modifiers.Count <= 0, (object) "BionicOxygenTankDisplayer has found an invalid AttributeModifier. This particular Amount should not use AttributeModifiers, the rate of breathing is defined by  Db.Get().Attributes.AirConsumptionRate");
    return GlobalStringBuilderPool.ReturnAndFree(sb);
  }
}
