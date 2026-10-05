// Decompiled with JetBrains decompiler
// Type: RadiationBalanceDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Text;
using UnityEngine;

#nullable disable
public class RadiationBalanceDisplayer : StandardAmountDisplayer
{
  public RadiationBalanceDisplayer()
    : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
  {
    this.formatter = (StandardAttributeFormatter) new RadiationBalanceDisplayer.RadiationAttributeFormatter();
  }

  public override string GetValueString(Amount master, AmountInstance instance)
  {
    return base.GetValueString(master, instance) + (string) UI.UNITSUFFIXES.RADIATION.RADS;
  }

  public override string GetTooltip(Amount master, AmountInstance instance)
  {
    StringBuilder sb = GlobalStringBuilderPool.Alloc();
    if (instance.gameObject.GetSMI<RadiationMonitor.Instance>() != null)
    {
      int cell = Grid.PosToCell(instance.gameObject);
      if (Grid.IsValidCell(cell))
        sb.Append((string) DUPLICANTS.STATS.RADIATIONBALANCE.TOOLTIP_CURRENT_BALANCE);
      sb.Append("\n\n");
      float num = Mathf.Clamp01(1f - Db.Get().Attributes.RadiationResistance.Lookup(instance.gameObject).GetTotalValue());
      sb.AppendFormat((string) DUPLICANTS.STATS.RADIATIONBALANCE.CURRENT_EXPOSURE, (object) Mathf.RoundToInt(Grid.Radiation[cell] * num));
      sb.Append("\n");
      sb.AppendFormat((string) DUPLICANTS.STATS.RADIATIONBALANCE.CURRENT_REJUVENATION, (object) Mathf.RoundToInt(Db.Get().Attributes.RadiationRecovery.Lookup(instance.gameObject).GetTotalValue() * 600f));
    }
    return GlobalStringBuilderPool.ReturnAndFree(sb);
  }

  public class RadiationAttributeFormatter : StandardAttributeFormatter
  {
    public RadiationAttributeFormatter()
      : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
    {
    }
  }
}
