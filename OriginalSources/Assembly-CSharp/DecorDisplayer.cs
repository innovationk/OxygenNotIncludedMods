// Decompiled with JetBrains decompiler
// Type: DecorDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Text;

#nullable disable
public class DecorDisplayer : StandardAmountDisplayer
{
  public DecorDisplayer()
    : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
  {
    this.formatter = (StandardAttributeFormatter) new DecorDisplayer.DecorAttributeFormatter();
  }

  public override string GetTooltip(Amount master, AmountInstance instance)
  {
    string text = LocText.ParseText(master.description);
    StringBuilder sb = GlobalStringBuilderPool.Alloc();
    sb.AppendFormat(text, (object) this.formatter.GetFormattedValue(instance.value));
    int cell = Grid.PosToCell(instance.gameObject);
    if (Grid.IsValidCell(cell))
      sb.Append(string.Format((string) DUPLICANTS.STATS.DECOR.TOOLTIP_CURRENT, (object) GameUtil.GetDecorAtCell(cell)));
    sb.Append("\n");
    DecorMonitor.Instance smi = instance.gameObject.GetSMI<DecorMonitor.Instance>();
    if (smi != null)
    {
      sb.AppendFormat((string) DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_TODAY, (object) this.formatter.GetFormattedValue(smi.GetTodaysAverageDecor()));
      sb.AppendFormat((string) DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_YESTERDAY, (object) this.formatter.GetFormattedValue(smi.GetYesterdaysAverageDecor()));
    }
    return GlobalStringBuilderPool.ReturnAndFree(sb);
  }

  public class DecorAttributeFormatter : StandardAttributeFormatter
  {
    public DecorAttributeFormatter()
      : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
    {
    }
  }
}
