// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.ExposureRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Klei.AI.DiseaseGrowthRules;

public class ExposureRule
{
  public float? populationHalfLife;

  public void Apply(ElemExposureInfo[] infoList)
  {
    List<Element> elements = ElementLoader.elements;
    for (int index = 0; index < elements.Count; ++index)
    {
      if (this.Test(elements[index]))
      {
        ElemExposureInfo info = infoList[index];
        if (this.populationHalfLife.HasValue)
          info.populationHalfLife = this.populationHalfLife.Value;
        infoList[index] = info;
      }
    }
  }

  public virtual bool Test(Element e) => true;

  public virtual string Name() => (string) null;
}
