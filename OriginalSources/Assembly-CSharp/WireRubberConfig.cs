// Decompiled with JetBrains decompiler
// Type: WireRubberConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class WireRubberConfig : BaseWireConfig
{
  public const string ID = "WireRubber";

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]
    {
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER_SMALL[0]
    };
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = this.CreateBuildingDef("WireRubber", "utilities_electric_rubber_kanim", 3f, construction_mass, 0.05f, none2, noise);
    buildingDef.MaterialCategory = new string[2]
    {
      "RefinedMetal",
      "Rubber&Plastic"
    };
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    this.DoPostConfigureComplete(Wire.WattageRating.Max4000, go);
  }
}
