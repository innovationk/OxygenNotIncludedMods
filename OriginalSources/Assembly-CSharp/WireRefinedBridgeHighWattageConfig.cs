// Decompiled with JetBrains decompiler
// Type: WireRefinedBridgeHighWattageConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class WireRefinedBridgeHighWattageConfig : WireBridgeHighWattageConfig
{
  public new const string ID = "WireRefinedBridgeHighWattage";

  protected override string GetID() => "WireRefinedBridgeHighWattage";

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = base.CreateBuildingDef();
    buildingDef.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "heavywatttile_conductive_kanim")
    };
    buildingDef.Mass = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    buildingDef.MaterialCategory = TUNING.MATERIALS.REFINED_METALS;
    buildingDef.SceneLayer = Grid.SceneLayer.WireBridges;
    buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridgeHighWattage");
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.POWER);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.WIRE);
    return buildingDef;
  }

  protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
  {
    WireUtilityNetworkLink utilityNetworkLink = base.AddNetworkLink(go);
    utilityNetworkLink.maxWattageRating = Wire.WattageRating.Max50000;
    return utilityNetworkLink;
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
  }
}
