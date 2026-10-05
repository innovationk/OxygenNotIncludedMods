// Decompiled with JetBrains decompiler
// Type: TravelTubeWallBridgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class TravelTubeWallBridgeConfig : IBuildingConfig
{
  public const string ID = "TravelTubeWallBridge";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] plastics = TUNING.MATERIALS.PLASTICS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("TravelTubeWallBridge", 1, 1, "tube_tile_bridge_kanim", 100, 3f, tieR2, plastics, 1600f, BuildLocationRule.Tile, none2, noise);
    BuildingTemplates.CreateFoundationTileDef(buildingDef);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.AudioCategory = "Plastic";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R90;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.TRANSPORT);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
    simCellOccupier.doReplaceElement = true;
    simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.PENALTY_3;
    simCellOccupier.notifyOnMelt = true;
    go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
    go.AddOrGet<TileTemperature>();
    go.AddOrGet<TravelTubeBridge>();
    go.GetComponent<KPrefabID>().prefabInitFn += new KPrefabID.PrefabFn(this.OnPrefabInit);
  }

  private void OnPrefabInit(GameObject instance)
  {
    foreach (KBatchedAnimController kbatchedAnimController in instance.GetComponentsInChildrenOnly<KBatchedAnimController>())
    {
      kbatchedAnimController.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.LiquidVisibilityLayer, false);
      kbatchedAnimController.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.WaterProof, true);
    }
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
    go.AddOrGet<KPrefabID>().AddTag(GameTags.TravelTubeBridges);
  }

  protected virtual TravelTubeUtilityNetworkLink AddNetworkLink(GameObject go)
  {
    TravelTubeUtilityNetworkLink utilityNetworkLink = go.AddOrGet<TravelTubeUtilityNetworkLink>();
    utilityNetworkLink.link1 = new CellOffset(-1, 0);
    utilityNetworkLink.link2 = new CellOffset(1, 0);
    return utilityNetworkLink;
  }
}
