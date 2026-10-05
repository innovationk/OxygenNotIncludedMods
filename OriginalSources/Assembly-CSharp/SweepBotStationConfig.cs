// Decompiled with JetBrains decompiler
// Type: SweepBotStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class SweepBotStationConfig : IBuildingConfig
{
  public const string ID = "SweepBotStation";
  public const float POWER_USAGE = 240f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[1]
    {
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0] - SweepBotConfig.MASS
    };
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SweepBotStation", 2, 2, "sweep_bot_base_station_kanim", 30, 30f, construction_mass, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.ROBOT);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    Storage botMaterialStorage = go.AddComponent<Storage>();
    botMaterialStorage.showInUI = true;
    botMaterialStorage.allowItemRemoval = false;
    botMaterialStorage.ignoreSourcePriority = true;
    botMaterialStorage.showDescriptor = false;
    botMaterialStorage.storageFilters = STORAGEFILTERS.STORAGE_LOCKERS_STANDARD;
    botMaterialStorage.storageFullMargin = TUNING.STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
    botMaterialStorage.fetchCategory = Storage.FetchCategory.Building;
    botMaterialStorage.capacityKg = 25f;
    botMaterialStorage.allowClearable = false;
    Storage sweepStorage = go.AddComponent<Storage>();
    sweepStorage.showInUI = true;
    sweepStorage.allowItemRemoval = true;
    sweepStorage.ignoreSourcePriority = true;
    sweepStorage.showDescriptor = true;
    sweepStorage.storageFilters = STORAGEFILTERS.STORAGE_LOCKERS_STANDARD;
    sweepStorage.storageFullMargin = TUNING.STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
    sweepStorage.fetchCategory = Storage.FetchCategory.StorageSweepOnly;
    sweepStorage.capacityKg = 1000f;
    sweepStorage.allowClearable = true;
    sweepStorage.showCapacityStatusItem = true;
    go.AddOrGet<CharacterOverlay>().shouldShowName = true;
    go.AddOrGet<SweepBotStation>().SetStorages(botMaterialStorage, sweepStorage);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<StorageController.Def>();
  }
}
