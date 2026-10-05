// Decompiled with JetBrains decompiler
// Type: MetalTileConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class MetalTileConfig : IBuildingConfig
{
  public const string ID = "MetalTile";
  public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_metal_tops");

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR2_2 = TUNING.BUILDINGS.DECOR.BONUS.TIER2;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MetalTile", 1, 1, "floor_metal_kanim", 100, 30f, tieR2_1, refinedMetals, 800f, BuildLocationRule.Tile, tieR2_2, noise);
    BuildingTemplates.CreateFoundationTileDef(buildingDef);
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.Overheatable = false;
    buildingDef.UseStructureTemperature = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.isKAnimTile = true;
    buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
    buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_metal");
    buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_metal_place");
    buildingDef.BlockTileShineAtlas = Assets.GetTextureAtlas("tiles_metal_spec");
    buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_metal_tops_decor_info");
    buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_metal_tops_decor_place_info");
    buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.TILE);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.METAL);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
    simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.BONUS_3;
    simCellOccupier.notifyOnMelt = true;
    go.AddOrGet<TileTemperature>();
    go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = MetalTileConfig.BlockTileConnectorID;
    go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RemoveLoopingSounds(go);
    go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.AddOrGet<KAnimGridTileVisualizer>();
  }
}
