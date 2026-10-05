// Decompiled with JetBrains decompiler
// Type: SteamEngineConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SteamEngineConfig : IBuildingConfig
{
  public const string ID = "SteamEngine";

  public override string[] GetForbiddenDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR7 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SteamEngine", 7, 5, "rocket_steam_engine_kanim", 1000, 480f, tieR7, construction_materials, 9999f, BuildLocationRule.OnFloor, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.UtilityInputOffset = new CellOffset(2, 3);
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.RequiresPowerInput = false;
    buildingDef.CanMove = true;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.STEAM);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, (AttachableBuilding) null)
    };
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    RocketEngine rocketEngine = go.AddOrGet<RocketEngine>();
    rocketEngine.fuelTag = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
    rocketEngine.efficiency = TUNING.ROCKETRY.ENGINE_EFFICIENCY.WEAK;
    rocketEngine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
    rocketEngine.requireOxidizer = false;
    rocketEngine.exhaustElement = SimHashes.Steam;
    rocketEngine.exhaustTemperature = ElementLoader.FindElementByHash(SimHashes.Steam).lowTemp + 50f;
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = TUNING.BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_WET_MASS[0];
    storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    FuelTank fuelTank = go.AddOrGet<FuelTank>();
    fuelTank.consumeFuelOnLand = !DlcManager.FeatureClusterSpaceEnabled();
    fuelTank.storage = storage;
    fuelTank.FuelType = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
    fuelTank.physicalFuelCapacity = storage.capacityKg;
    go.AddOrGet<CopyBuildingSettings>();
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 10f;
    conduitConsumer.capacityTag = fuelTank.FuelType;
    conduitConsumer.capacityKG = storage.capacityKg;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_steam_engine_bg_kanim");
  }
}
