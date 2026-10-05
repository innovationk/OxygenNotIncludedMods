// Decompiled with JetBrains decompiler
// Type: CrewRationsEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public class CrewRationsEntry : CrewListEntry
{
  public KButton incRationPerDayButton;
  public KButton decRationPerDayButton;
  public LocText rationPerDayText;
  public LocText rationsEatenToday;
  public LocText currentCaloriesText;
  public LocText currentStressText;
  public LocText currentHealthText;
  public ValueTrendImageToggle stressTrendImage;
  private RationMonitor.Instance rationMonitor;

  public override void Populate(MinionIdentity _identity)
  {
    base.Populate(_identity);
    this.rationMonitor = _identity.GetSMI<RationMonitor.Instance>();
    this.Refresh();
  }

  public override void Refresh()
  {
    base.Refresh();
    this.rationsEatenToday.text = GameUtil.GetFormattedCalories(this.rationMonitor.GetRationsAteToday());
    if ((Object) this.identity == (Object) null)
      return;
    foreach (AmountInstance modifier in this.identity.GetAmounts().ModifierList)
    {
      float min = modifier.GetMin();
      float max = modifier.GetMax();
      float num = max - min;
      string str = Mathf.RoundToInt((float) (((double) num - ((double) max - (double) modifier.value)) / (double) num * 100.0)).ToString();
      if (modifier.amount == Db.Get().Amounts.Stress)
      {
        this.currentStressText.text = modifier.GetValueString();
        this.currentStressText.GetComponent<ToolTip>().toolTip = modifier.GetTooltip();
        this.stressTrendImage.SetValue(modifier);
      }
      else if (modifier.amount == Db.Get().Amounts.Calories)
      {
        this.currentCaloriesText.text = str + "%";
        this.currentCaloriesText.GetComponent<ToolTip>().toolTip = modifier.GetTooltip();
      }
      else if (modifier.amount == Db.Get().Amounts.HitPoints)
      {
        this.currentHealthText.text = str + "%";
        this.currentHealthText.GetComponent<ToolTip>().toolTip = modifier.GetTooltip();
      }
    }
  }
}
