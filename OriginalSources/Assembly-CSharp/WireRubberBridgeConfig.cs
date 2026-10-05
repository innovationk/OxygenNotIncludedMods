// Decompiled with JetBrains decompiler
// Type: WireRubberBridgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class WireRubberBridgeConfig : WireBridgeConfig
{
  public new const string ID = "WireRubberBridge";

  protected override string GetID() => "WireRubberBridge";

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = base.CreateBuildingDef();
    buildingDef.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "utilityelectricbridgerubber_kanim")
    };
    buildingDef.Mass = new float[2]
    {
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER_SMALL[0]
    };
    buildingDef.MaterialCategory = new string[2]
    {
      "RefinedMetal",
      "Rubber&Plastic"
    };
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.POWER);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.WIRE);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRubberBridge");
    return buildingDef;
  }

  protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
  {
    WireUtilityNetworkLink utilityNetworkLink = base.AddNetworkLink(go);
    utilityNetworkLink.maxWattageRating = Wire.WattageRating.Max4000;
    return utilityNetworkLink;
  }
}
