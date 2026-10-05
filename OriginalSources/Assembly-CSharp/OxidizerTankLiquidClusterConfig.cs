// Decompiled with JetBrains decompiler
// Type: OxidizerTankLiquidClusterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class OxidizerTankLiquidClusterConfig : IBuildingConfig
{
  public const string ID = "OxidizerTankLiquidCluster";
  public const float FuelCapacity = 450f;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] fuelTankDryMass = BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_DRY_MASS;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("OxidizerTankLiquidCluster", 5, 2, "rocket_cluster_oxidizer_tank_liquid_kanim", 1000, 60f, fuelTankDryMass, construction_materials, 9999f, BuildLocationRule.Anywhere, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.DefaultAnimState = "grounded";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.UtilityInputOffset = new CellOffset(1, 1);
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.RequiresPowerInput = false;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.CanMove = true;
    buildingDef.Cancellable = false;
    buildingDef.ShowInBuildMenu = false;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 2), GameTags.Rocket, (AttachableBuilding) null)
    };
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 450f;
    storage.storageFilters = new List<Tag>()
    {
      SimHashes.LiquidOxygen.CreateTag()
    };
    storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    OxidizerTank oxidizerTank = go.AddOrGet<OxidizerTank>();
    oxidizerTank.supportsMultipleOxidizers = false;
    oxidizerTank.consumeOnLand = false;
    oxidizerTank.storage = storage;
    oxidizerTank.targetFillMass = 450f;
    oxidizerTank.maxFillMass = 450f;
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGet<DropToUserCapacity>();
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.consumptionRate = 10f;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.LiquidOxygen).tag;
    conduitConsumer.capacityKG = storage.capacityKg;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, (string) null, ROCKETRY.BURDEN.MODERATE_PLUS);
    storage.showUnreachableStatus = false;
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst =>
    {
      Element elementByHash = ElementLoader.FindElementByHash(SimHashes.LiquidOxygen);
      if (DiscoveredResources.Instance.IsDiscovered(elementByHash.tag))
        return;
      DiscoveredResources.Instance.Discover(elementByHash.tag, elementByHash.GetMaterialCategoryTag());
    });
  }
}
