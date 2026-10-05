// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.StateGrowthRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Klei.AI.DiseaseGrowthRules;

public class StateGrowthRule : GrowthRule
{
  public Element.State state;

  public StateGrowthRule(Element.State state) => this.state = state;

  public override bool Test(Element e) => e.IsState(this.state);

  public override string Name() => Element.GetStateString(this.state);
}
