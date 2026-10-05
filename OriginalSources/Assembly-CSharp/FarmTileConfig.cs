// Decompiled with JetBrains decompiler
// Type: FarmTileConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class FarmTileConfig : IBuildingConfig
{
  public const string ID = "FarmTile";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] farmable = TUNING.MATERIALS.FARMABLE;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FarmTile", 1, 1, "farmtilerotating_kanim", 100, 30f, tieR2, farmable, 1600f, BuildLocationRule.Tile, none2, noise);
    BuildingTemplates.CreateFoundationTileDef(buildingDef);
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.Overheatable = false;
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingBack;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
    buildingDef.PermittedRotations = PermittedRotations.FlipV;
    buildingDef.DragBuild = true;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.FOOD);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.FARM);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(GameTags.CodexCategories.FarmBuilding);
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
    simCellOccupier.doReplaceElement = true;
    simCellOccupier.notifyOnMelt = true;
    go.AddOrGet<TileTemperature>();
    BuildingTemplates.CreateDefaultStorage(go).SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
    plantablePlot.occupyingObjectRelativePosition = new Vector3(0.0f, 1f, 0.0f);
    plantablePlot.AddDepositTag(GameTags.CropSeed);
    plantablePlot.AddDepositTag(GameTags.WaterSeed);
    plantablePlot.AddAdditionalCriteria(new Func<GameObject, bool>(FarmTileConfig.ForbiddenTags));
    plantablePlot.SetFertilizationFlags(true, false);
    go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
    go.AddOrGet<AnimTileable>();
    Prioritizable.AddRef(go);
    component.prefabInitFn += new KPrefabID.PrefabFn(this.OnPrefabInit);
  }

  private void OnPrefabInit(GameObject instance)
  {
    instance.AddOrGet<PlantablePlot>().AddAdditionalCriteria(new Func<GameObject, bool>(FarmTileConfig.ForbiddenTags));
  }

  public static bool ForbiddenTags(GameObject objInQuestion)
  {
    KPrefabID component = objInQuestion.GetComponent<KPrefabID>();
    return (component.HasTag(GameTags.LargeSeed) ? 1 : (component.HasTag(GameTags.BackwallSeed) ? 1 : 0)) == 0;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KBatchedAnimController>().initialBlendParameters = 4;
    GeneratedBuildings.RemoveLoopingSounds(go);
    go.GetComponent<KPrefabID>().AddTag(GameTags.FarmTiles);
    FarmTileConfig.SetUpFarmPlotTags(go);
  }

  public static void SetUpFarmPlotTags(GameObject go)
  {
    go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (inst =>
    {
      Rotatable component1 = inst.GetComponent<Rotatable>();
      PlantablePlot component2 = inst.GetComponent<PlantablePlot>();
      switch (component1.GetOrientation())
      {
        case Orientation.Neutral:
        case Orientation.FlipH:
          component2.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Top);
          break;
        case Orientation.R90:
        case Orientation.R270:
          component2.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Side);
          break;
        case Orientation.R180:
        case Orientation.FlipV:
          component2.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Bottom);
          break;
      }
    });
  }
}
