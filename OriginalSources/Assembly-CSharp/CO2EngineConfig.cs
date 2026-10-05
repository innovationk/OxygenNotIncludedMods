// Decompiled with JetBrains decompiler
// Type: CO2EngineConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class CO2EngineConfig : IBuildingConfig
{
  public const string ID = "CO2Engine";
  public const SimHashes FUEL = SimHashes.CarbonDioxide;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] denseTieR1 = BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER1;
    string[] rawMetals = MATERIALS.RAW_METALS;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CO2Engine", 3, 2, "rocket_co2_engine_kanim", 1000, 30f, denseTieR1, rawMetals, 9999f, BuildLocationRule.Anywhere, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.UtilityInputOffset = new CellOffset(0, 1);
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.RequiresPowerInput = false;
    buildingDef.RequiresPowerOutput = false;
    buildingDef.CanMove = true;
    buildingDef.Cancellable = false;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 2), GameTags.Rocket, (AttachableBuilding) null)
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
    rocketEngineCluster.maxModules = 3;
    rocketEngineCluster.maxHeight = ROCKETRY.ROCKET_HEIGHT.VERY_SHORT;
    rocketEngineCluster.fuelTag = SimHashes.CarbonDioxide.CreateTag();
    rocketEngineCluster.efficiency = ROCKETRY.ENGINE_EFFICIENCY.WEAK;
    rocketEngineCluster.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
    rocketEngineCluster.requireOxidizer = false;
    rocketEngineCluster.exhaustElement = SimHashes.CarbonDioxide;
    rocketEngineCluster.exhaustTemperature = ElementLoader.FindElementByHash(SimHashes.CarbonDioxide).lowTemp + 20f;
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_WET_MASS_GAS[0];
    storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    FuelTank fuelTank = go.AddOrGet<FuelTank>();
    fuelTank.consumeFuelOnLand = false;
    fuelTank.storage = storage;
    fuelTank.FuelType = SimHashes.CarbonDioxide.CreateTag();
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
    BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, (string) null, ROCKETRY.BURDEN.MINOR_PLUS, (float) ROCKETRY.ENGINE_POWER.EARLY_STRONG, ROCKETRY.FUEL_COST_PER_DISTANCE.GAS_LOW);
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst => { });
  }
}
