// Decompiled with JetBrains decompiler
// Type: CaloriesDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

#nullable disable
public class CaloriesDisplayer : StandardAmountDisplayer
{
  public CaloriesDisplayer()
    : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle)
  {
    this.formatter = (StandardAttributeFormatter) new CaloriesDisplayer.CaloriesAttributeFormatter();
  }

  public class CaloriesAttributeFormatter : StandardAttributeFormatter
  {
    public CaloriesAttributeFormatter()
      : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle)
    {
    }

    public override string GetFormattedModifier(AttributeModifier modifier)
    {
      return modifier.IsMultiplier ? GameUtil.GetFormattedPercent((float) (-(double) modifier.Value * 100.0)) : base.GetFormattedModifier(modifier);
    }
  }
}
