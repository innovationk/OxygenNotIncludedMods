// Decompiled with JetBrains decompiler
// Type: WireBridgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class WireBridgeConfig : IBuildingConfig
{
  public const string ID = "WireBridge";

  protected virtual string GetID() => "WireBridge";

  public override BuildingDef CreateBuildingDef()
  {
    string id = this.GetID();
    float[] tieR0_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0_2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 3, 1, "utilityelectricbridge_kanim", 30, 3f, tieR0_1, allMetals, 1600f, BuildLocationRule.WireBridge, tieR0_2, noise);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.ObjectLayer = ObjectLayer.WireConnectors;
    buildingDef.SceneLayer = Grid.SceneLayer.WireBridges;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireBridge");
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.POWER);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.WIRE);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
    this.AddNetworkLink(go).visualizeOnly = true;
    go.AddOrGet<BuildingCellVisualizer>();
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    this.AddNetworkLink(go).visualizeOnly = true;
    go.AddOrGet<BuildingCellVisualizer>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    this.AddNetworkLink(go).visualizeOnly = false;
    go.AddOrGet<BuildingCellVisualizer>();
  }

  protected virtual WireUtilityNetworkLink AddNetworkLink(GameObject go)
  {
    WireUtilityNetworkLink utilityNetworkLink = go.AddOrGet<WireUtilityNetworkLink>();
    utilityNetworkLink.maxWattageRating = Wire.WattageRating.Max1000;
    utilityNetworkLink.link1 = new CellOffset(-1, 0);
    utilityNetworkLink.link2 = new CellOffset(1, 0);
    return utilityNetworkLink;
  }
}
