// Decompiled with JetBrains decompiler
// Type: SteamEngineClusterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SteamEngineClusterConfig : IBuildingConfig
{
  public const string ID = "SteamEngineCluster";

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] denseTieR0 = TUNING.BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER0;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SteamEngineCluster", 7, 5, "rocket_cluster_steam_engine_kanim", 1000, 60f, denseTieR0, refinedMetals, 9999f, BuildLocationRule.Anywhere, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.UtilityInputOffset = new CellOffset(2, 3);
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.GeneratorWattageRating = 600f;
    buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
    buildingDef.RequiresPowerInput = false;
    buildingDef.RequiresPowerOutput = false;
    buildingDef.CanMove = true;
    buildingDef.Cancellable = false;
    buildingDef.ShowInBuildMenu = false;
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
    RocketEngineCluster rocketEngineCluster = go.AddOrGet<RocketEngineCluster>();
    rocketEngineCluster.maxModules = 6;
    rocketEngineCluster.maxHeight = TUNING.ROCKETRY.ROCKET_HEIGHT.TALL;
    rocketEngineCluster.fuelTag = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
    rocketEngineCluster.efficiency = TUNING.ROCKETRY.ENGINE_EFFICIENCY.WEAK;
    rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
    rocketEngineCluster.requireOxidizer = false;
    rocketEngineCluster.exhaustElement = SimHashes.Steam;
    rocketEngineCluster.exhaustTemperature = ElementLoader.FindElementByHash(SimHashes.Steam).lowTemp + 50f;
    go.AddOrGet<ModuleGenerator>();
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = TUNING.BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_WET_MASS_GAS_LARGE[0];
    storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    FuelTank fuelTank = go.AddOrGet<FuelTank>();
    fuelTank.consumeFuelOnLand = false;
    fuelTank.storage = storage;
    fuelTank.FuelType = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
    fuelTank.targetFillMass = storage.capacityKg;
    fuelTank.physicalFuelCapacity = storage.capacityKg;
    go.AddOrGet<CopyBuildingSettings>();
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 10f;
    conduitConsumer.capacityTag = fuelTank.FuelType;
    conduitConsumer.capacityKG = storage.capacityKg;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, (string) null, TUNING.ROCKETRY.BURDEN.MONUMENTAL, (float) TUNING.ROCKETRY.ENGINE_POWER.MID_WEAK, TUNING.ROCKETRY.FUEL_COST_PER_DISTANCE.GAS_VERY_LOW);
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst => { });
  }
}
