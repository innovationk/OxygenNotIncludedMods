// Decompiled with JetBrains decompiler
// Type: IceKettleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class IceKettleConfig : IBuildingConfig
{
  public const string ID = "IceKettle";
  public const SimHashes TARGET_ELEMENT = SimHashes.Ice;
  public const float MASS_KG_PER_BATCH = 100f;
  public const float CAPACITY = 1000f;
  public const float FINAL_PRODUCT_CAPACITY = 500f;
  public static Tag TARGET_ELEMENT_TAG = SimHashes.Ice.CreateTag();
  public const float TARGET_TEMPERATURE = 298.15f;
  public const float PRODUCTION_PER_SECOND = 20f;
  public static Tag FUEL_TAG = (Tag) "BuildingWood";
  public const SimHashes EXHAUST_TAG = SimHashes.CarbonDioxide;
  public const float TOTAL_ENERGY_OF_LUMBER = 7750f;
  public const float ENERGY_OF_LUMBER_TAKEN_FOR_BUILDING_SELF_HEAT = 3750f;
  public const float ENERGY_PER_UNIT_OF_LUMBER_TAKEN_FOR_MELTING = 4000f;
  public const float FUEL_UNITS_REQUIRED_TO_MELT_ABSOLUTE_ZERO_BATCH = 15.2801876f;
  public const float FUEL_CAPACITY = 152.80188f;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("IceKettle", 2, 2, "icemelter_kettle_kanim", 100, 10f, tieR4, rawMetals, 1600f, BuildLocationRule.OnFloor, none2, noise);
    float num = 3.75000024f;
    buildingDef.SelfHeatKilowattsWhenActive = num * 0.4f;
    buildingDef.ExhaustKilowattsWhenActive = num - buildingDef.SelfHeatKilowattsWhenActive;
    buildingDef.Floodable = false;
    buildingDef.Entombable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.DefaultAnimState = "on";
    buildingDef.POIUnlockable = true;
    buildingDef.ShowInBuildMenu = true;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.WATER);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddTag(GameTags.LiquidSource);
    Storage storage1 = go.AddOrGet<Storage>();
    storage1.capacityKg = Mathf.Ceil(152.80188f);
    storage1.showInUI = true;
    storage1.allowItemRemoval = false;
    storage1.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    ManualDeliveryKG manualDeliveryKg1 = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg1.capacity = Mathf.Ceil(152.80188f);
    manualDeliveryKg1.SetStorage(storage1);
    manualDeliveryKg1.RequestedItemTag = IceKettleConfig.FUEL_TAG;
    manualDeliveryKg1.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    manualDeliveryKg1.ShowStatusItem = false;
    Storage storage2 = go.AddComponent<Storage>();
    storage2.capacityKg = 1000f;
    storage2.showInUI = true;
    storage2.allowItemRemoval = false;
    storage2.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    ManualDeliveryKG manualDeliveryKg2 = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg2.capacity = 1000f;
    manualDeliveryKg2.SetStorage(storage2);
    manualDeliveryKg2.RequestedItemTag = IceKettleConfig.TARGET_ELEMENT_TAG;
    manualDeliveryKg2.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    manualDeliveryKg2.refillMass = 100f;
    manualDeliveryKg2.ShowStatusItem = false;
    Storage storage3 = go.AddComponent<Storage>();
    storage3.capacityKg = 500f;
    storage3.showInUI = true;
    storage3.allowItemRemoval = true;
    storage3.showDescriptor = true;
    storage3.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    IceKettle.Def def = go.AddOrGetDef<IceKettle.Def>();
    def.exhaust_tag = SimHashes.CarbonDioxide;
    def.targetElementTag = IceKettleConfig.TARGET_ELEMENT_TAG;
    def.KGToMeltPerBatch = 100f;
    def.KGMeltedPerSecond = 20f;
    def.fuelElementTag = IceKettleConfig.FUEL_TAG;
    def.TargetTemperature = 298.15f;
    def.EnergyPerUnitOfLumber = 4000f;
    def.ExhaustMassPerUnitOfLumber = 0.142f;
    go.AddOrGet<IceKettleWorkable>().storage = storage3;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
