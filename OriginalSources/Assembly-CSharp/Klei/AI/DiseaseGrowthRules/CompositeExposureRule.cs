// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.CompositeExposureRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Klei.AI.DiseaseGrowthRules;

public class CompositeExposureRule
{
  public string name;
  public float populationHalfLife;

  public string Name() => this.name;

  public void Overlay(ExposureRule rule)
  {
    if (rule.populationHalfLife.HasValue)
      this.populationHalfLife = rule.populationHalfLife.Value;
    this.name = rule.Name();
  }

  public float GetHalfLifeForCount(int count) => this.populationHalfLife;
}
