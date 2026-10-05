// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.ElementGrowthRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Klei.AI.DiseaseGrowthRules;

public class ElementGrowthRule : GrowthRule
{
  public SimHashes element;

  public ElementGrowthRule(SimHashes element) => this.element = element;

  public override bool Test(Element e) => e.id == this.element;

  public override string Name() => ElementLoader.FindElementByHash(this.element).name;
}
