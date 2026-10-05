// Decompiled with JetBrains decompiler
// Type: HotTubConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class HotTubConfig : IBuildingConfig
{
  public const string ID = "HotTub";
  public const string COLD_IMMUNITY_EFFECT_NAME = "WarmTouch";
  public const float COLD_IMMUNITY_DURATION = 1800f;
  private float WATER_AMOUNT = 100f;
  private const float KDTU_TRANSFER_RATE = 15f;
  private float MINIMUM_WATER_TEMPERATURE = 310.85f;
  private float MAXIMUM_TUB_TEMPERATURE = 310.85f;
  private float BLEACH_STONE_CONSUMPTION_RATE = 0.116666667f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]{ 200f, 200f };
    string[] construction_materials = new string[2]
    {
      "Metal",
      "BuildingWood"
    };
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.BONUS.TIER2;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("HotTub", 5, 2, "hottub_kanim", 30, 10f, construction_mass, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.Floodable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = true;
    buildingDef.OverheatTemperature = this.MINIMUM_WATER_TEMPERATURE;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(2, 0);
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(-2, 0);
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.ExhaustKilowattsWhenActive = 1f;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding);
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
    conduitConsumer.capacityKG = this.WATER_AMOUNT;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    conduitConsumer.storage = storage;
    conduitConsumer.SetOnState(false);
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.storage = storage;
    conduitDispenser.SetOnState(false);
    ManualDeliveryKG manualDeliveryKg = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = new Tag("BleachStone");
    manualDeliveryKg.capacity = 100f;
    manualDeliveryKg.refillMass = 10f;
    manualDeliveryKg.MinimumMass = 1f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    HotTub hotTub = go.AddOrGet<HotTub>();
    hotTub.waterStorage = storage;
    hotTub.hotTubCapacity = this.WATER_AMOUNT;
    hotTub.waterCoolingRate = 15f;
    hotTub.minimumWaterTemperature = this.MINIMUM_WATER_TEMPERATURE;
    hotTub.bleachStoneConsumption = this.BLEACH_STONE_CONSUMPTION_RATE;
    hotTub.maxOperatingTemperature = this.MAXIMUM_TUB_TEMPERATURE;
    hotTub.specificEffect = "HotTub";
    hotTub.trackingEffect = "RecentlyHotTub";
    hotTub.basePriority = RELAXATION.PRIORITY.TIER4;
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
    roomTracker.requirement = RoomTracker.Requirement.Recommended;
    go.AddOrGetDef<RocketUsageRestriction.Def>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<RequireInputs>().requireConduitHasMass = false;
  }
}
