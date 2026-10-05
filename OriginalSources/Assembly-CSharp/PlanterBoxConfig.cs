// Decompiled with JetBrains decompiler
// Type: PlanterBoxConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class PlanterBoxConfig : IBuildingConfig
{
  public const string ID = "PlanterBox";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] farmable = TUNING.MATERIALS.FARMABLE;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("PlanterBox", 1, 1, "planterbox_kanim", 10, 3f, tieR2, farmable, 800f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingBack;
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "large";
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.FOOD);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.FARM);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(GameTags.CodexCategories.FarmBuilding);
    Storage storage = go.AddOrGet<Storage>();
    PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
    plantablePlot.IsOffGround = true;
    plantablePlot.tagOnPlanted = GameTags.PlantedOnFloorVessel;
    plantablePlot.AddDepositTag(GameTags.CropSeed);
    plantablePlot.AddAdditionalCriteria(new Func<GameObject, bool>(FarmTileConfig.ForbiddenTags));
    plantablePlot.SetFertilizationFlags(true, false);
    go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
    BuildingTemplates.CreateDefaultStorage(go);
    List<Storage.StoredItemModifier> standardSealedStorage = Storage.StandardSealedStorage;
    storage.SetDefaultStoredItemModifiers(standardSealedStorage);
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<PlanterBox>();
    go.AddOrGet<AnimTileable>();
    Prioritizable.AddRef(go);
    component.prefabInitFn += new KPrefabID.PrefabFn(this.OnPrefabInit);
  }

  private void OnPrefabInit(GameObject instance)
  {
    instance.AddOrGet<PlantablePlot>().AddAdditionalCriteria(new Func<GameObject, bool>(FarmTileConfig.ForbiddenTags));
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
