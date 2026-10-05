// Decompiled with JetBrains decompiler
// Type: SnowTileConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class SnowTileConfig : IBuildingConfig
{
  public const string ID = "SnowTile";
  public static readonly int BlockTileConnectorID = Hash.SDBMLower("tiles_snow_tops");
  private SimHashes CONSTRUCTION_ELEMENT = SimHashes.Snow;
  private SimHashes STABLE_SNOW_ELEMENT = SimHashes.StableSnow;

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[1]{ 30f };
    string[] construction_materials = new string[1]
    {
      this.CONSTRUCTION_ELEMENT.ToString()
    };
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SnowTile", 1, 1, "floor_snow_kanim", 100, 3f, construction_mass, construction_materials, 1600f, BuildLocationRule.Tile, none2, noise);
    BuildingTemplates.CreateFoundationTileDef(buildingDef);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.UseStructureTemperature = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.isKAnimTile = true;
    buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_snow");
    buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_snow_place");
    buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
    buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_snow_decor_info");
    buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_snow_decor_place_info");
    buildingDef.Temperature = 263.15f;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.TILE);
    buildingDef.DragBuild = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
    simCellOccupier.doReplaceElement = true;
    simCellOccupier.strengthMultiplier = 1.5f;
    simCellOccupier.notifyOnMelt = true;
    go.AddOrGet<TileTemperature>();
    go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = SnowTileConfig.BlockTileConnectorID;
    go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.prefabInitFn += new KPrefabID.PrefabFn(this.BuildingComplete_OnInit);
    component.prefabSpawnFn += new KPrefabID.PrefabFn(this.BuildingComplete_OnSpawn);
  }

  public override string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RemoveLoopingSounds(go);
    go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles);
  }

  private void BuildingComplete_OnInit(GameObject instance)
  {
    PrimaryElement component1 = instance.GetComponent<PrimaryElement>();
    component1.SetElement(this.STABLE_SNOW_ELEMENT);
    Element element = component1.Element;
    Deconstructable component2 = instance.GetComponent<Deconstructable>();
    if (!((Object) component2 != (Object) null))
      return;
    component2.constructionElements = new Tag[1]
    {
      this.CONSTRUCTION_ELEMENT.CreateTag()
    };
  }

  private void BuildingComplete_OnSpawn(GameObject instance)
  {
    instance.GetComponent<PrimaryElement>().SetElement(this.STABLE_SNOW_ELEMENT);
    Deconstructable component = instance.GetComponent<Deconstructable>();
    if (!((Object) component != (Object) null))
      return;
    component.constructionElements = new Tag[1]
    {
      this.CONSTRUCTION_ELEMENT.CreateTag()
    };
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.AddOrGet<KAnimGridTileVisualizer>();
  }
}
