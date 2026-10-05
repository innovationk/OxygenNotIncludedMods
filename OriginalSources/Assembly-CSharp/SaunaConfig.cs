// Decompiled with JetBrains decompiler
// Type: SaunaConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class SaunaConfig : IBuildingConfig
{
  public const string ID = "Sauna";
  public const string COLD_IMMUNITY_EFFECT_NAME = "WarmTouch";
  public const float COLD_IMMUNITY_DURATION = 1800f;
  private const float STEAM_PER_USE_KG = 25f;
  private const float WATER_OUTPUT_TEMP = 353.15f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]{ 100f, 100f };
    string[] construction_materials = new string[2]
    {
      "Metal",
      "BuildingWood"
    };
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Sauna", 3, 3, "sauna_kanim", 30, 60f, construction_mass, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.ViewMode = OverlayModes.GasConduits.ID;
    buildingDef.Floodable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = true;
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(0, 2);
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.STEAM);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding);
    go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
    conduitConsumer.capacityKG = 50f;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    conduitConsumer.alwaysConsume = true;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.elementFilter = new SimHashes[1]
    {
      SimHashes.Water
    };
    go.AddOrGet<SaunaWorkable>().basePriority = RELAXATION.PRIORITY.TIER3;
    Sauna sauna = go.AddOrGet<Sauna>();
    sauna.steamPerUseKG = 25f;
    sauna.waterOutputTemp = 353.15f;
    sauna.specificEffect = "Sauna";
    sauna.trackingEffect = "RecentlySauna";
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
