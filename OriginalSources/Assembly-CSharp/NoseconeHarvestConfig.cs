// Decompiled with JetBrains decompiler
// Type: NoseconeHarvestConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class NoseconeHarvestConfig : IBuildingConfig
{
  public const string ID = "NoseconeHarvest";
  private float timeToFill = 3600f;
  private float solidCapacity = ROCKETRY.SOLID_CARGO_BAY_CLUSTER_CAPACITY * ROCKETRY.CARGO_CAPACITY_SCALE;
  public const float DIAMOND_CONSUMED_PER_HARVEST_KG = 0.05f;
  public const float DIAMOND_STORAGE_CAPACITY_KG = 1000f;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]
    {
      BUILDINGS.ROCKETRY_MASS_KG.NOSE_CONE_TIER2[0],
      4f
    };
    string[] construction_materials = new string[2]
    {
      "RefinedMetal",
      "BuildingGasket"
    };
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("NoseconeHarvest", 5, 4, "rocket_nosecone_gathering_kanim", 1000, 60f, construction_mass, construction_materials, 9999f, BuildLocationRule.Anywhere, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.ForegroundLayer = Grid.SceneLayer.Front;
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
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.GetComponent<KPrefabID>().AddTag(GameTags.NoseRocketModule);
    go.AddOrGetDef<ResourceHarvestModule.Def>().harvestSpeed = this.solidCapacity / this.timeToFill;
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 1000f;
    storage.useWideOffsets = true;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = SimHashes.Diamond.CreateTag();
    manualDeliveryKg.MinimumMass = storage.capacityKg;
    manualDeliveryKg.capacity = storage.capacityKg;
    manualDeliveryKg.refillMass = storage.capacityKg;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, (string) null, ROCKETRY.BURDEN.MINOR);
    go.GetComponent<ReorderableBuilding>().buildConditions.Add((SelectModuleCondition) new TopOnly());
  }
}
