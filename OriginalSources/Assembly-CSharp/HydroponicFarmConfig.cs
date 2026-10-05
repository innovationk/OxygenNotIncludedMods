// Decompiled with JetBrains decompiler
// Type: HydroponicFarmConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class HydroponicFarmConfig : IBuildingConfig
{
  public const string ID = "HydroponicFarm";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("HydroponicFarm", 1, 1, "farmtilehydroponicrotating_kanim", 100, 30f, tieR2, allMetals, 1600f, BuildLocationRule.Tile, tieR0, noise);
    BuildingTemplates.CreateFoundationTileDef(buildingDef);
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.Overheatable = false;
    buildingDef.UseStructureTemperature = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
    buildingDef.PermittedRotations = PermittedRotations.FlipV;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.FARM);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.FOOD);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(GameTags.CodexCategories.FarmBuilding);
    SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
    simCellOccupier.doReplaceElement = true;
    simCellOccupier.notifyOnMelt = true;
    go.AddOrGet<TileTemperature>();
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.consumptionRate = 1f;
    conduitConsumer.capacityKG = 5f;
    conduitConsumer.capacityTag = GameTags.Liquid;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    go.AddOrGet<Storage>();
    PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
    plantablePlot.AddDepositTag(GameTags.CropSeed);
    plantablePlot.AddDepositTag(GameTags.WaterSeed);
    plantablePlot.AddAdditionalCriteria(new Func<GameObject, bool>(FarmTileConfig.ForbiddenTags));
    plantablePlot.occupyingObjectRelativePosition.y = 1f;
    plantablePlot.SetFertilizationFlags(true, true);
    go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
    BuildingTemplates.CreateDefaultStorage(go).SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    go.AddOrGet<PlanterBox>();
    go.AddOrGet<AnimTileable>();
    go.AddOrGet<DropAllWorkable>();
    Prioritizable.AddRef(go);
    component.prefabInitFn += new KPrefabID.PrefabFn(this.OnPrefabInit);
  }

  private void OnPrefabInit(GameObject instance)
  {
    instance.AddOrGet<PlantablePlot>().AddAdditionalCriteria(new Func<GameObject, bool>(FarmTileConfig.ForbiddenTags));
    KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
    component.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.LiquidVisibilityLayer, false);
    component.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.WaterProof, true);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    FarmTileConfig.SetUpFarmPlotTags(go);
    go.GetComponent<KPrefabID>().AddTag(GameTags.FarmTiles);
    go.GetComponent<RequireInputs>().requireConduitHasMass = false;
  }
}
