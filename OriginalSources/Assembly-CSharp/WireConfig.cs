// Decompiled with JetBrains decompiler
// Type: WireConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class WireConfig : BaseWireConfig
{
  public const string ID = "Wire";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR0_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0_2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = this.CreateBuildingDef("Wire", "utilities_electric_kanim", 3f, tieR0_1, 0.05f, tieR0_2, noise);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.POWER);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.WIRE);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    this.DoPostConfigureComplete(Wire.WattageRating.Max1000, go);
  }
}
