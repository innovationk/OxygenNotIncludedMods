// Decompiled with JetBrains decompiler
// Type: MilkFatSeparatorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class MilkFatSeparatorConfig : IBuildingConfig
{
  public const string ID = "MilkFatSeparator";
  public const float INPUT_RATE = 1f;
  public const float MILK_STORED_CAPACITY = 4f;
  public const float MILK_FAT_CAPACITY = 15f;
  public const float EFFICIENCY = 0.9f;
  public const float MILKFAT_PERCENT = 0.1f;
  private const float MILK_TO_FAT_OUTPUT_RATE = 0.0899999961f;
  private const float MILK_TO_BRINE_WATER_OUTPUT_RATE = 0.809999943f;
  private const float MILK_TO_CO2_RATE = 0.100000024f;
  public const SimHashes MILK_SEPARATED_LIQUID_OUTPUT_ELEMENT = SimHashes.Brine;
  public const SimHashes FISHMILK_SEPARATED_LIQUID_OUTPUT_ELEMENT = SimHashes.Mucus;
  public static Tag MILK_SEPARATED_LIQUID_OUTPUT_TAG = SimHashes.Brine.CreateTag();
  public static Tag FISHMILK_SEPARATED_LIQUID_OUTPUT_TAG = SimHashes.Mucus.CreateTag();
  public const float FISHMILK_INPUT_RATE = 1f;
  public const float FISHMILK_EFFICIENCY = 0.9f;
  public const float CAVIAR_PERCENT = 0.1f;
  public const float FISHMILK_TO_CAVIAR_RATE = 0.0899999961f;
  private const float FISHMILK_TO_MUCUS_RATE = 0.809999943f;
  private const float FISHMILK_TO_CO2_RATE = 0.100000024f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR2 = BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MilkFatSeparator", 4, 4, "milk_separator_kanim", 100, 120f, tieR5, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.SelfHeatKilowattsWhenActive = 8f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    return buildingDef;
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Storage storage = go.AddOrGet<Storage>();
    storage.allowItemRemoval = false;
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    storage.showInUI = true;
    go.AddOrGet<Operational>();
    go.AddOrGet<EmptyMilkSeparatorWorkable>();
    ElementConverter elementConverter1 = go.AddOrGet<ElementConverter>();
    elementConverter1.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(new Tag("Milk"), 1f)
    };
    elementConverter1.outputElements = new ElementConverter.OutputElement[3]
    {
      new ElementConverter.OutputElement(0.0899999961f, SimHashes.MilkFat, 0.0f, storeOutput: true),
      new ElementConverter.OutputElement(0.809999943f, SimHashes.Brine, 0.0f, storeOutput: true, diseaseWeight: 0.0f),
      new ElementConverter.OutputElement(0.100000024f, SimHashes.CarbonDioxide, 348.15f, outputElementOffsetx: 1f, outputElementOffsety: 3f, diseaseWeight: 0.0f)
    };
    ElementConverter elementConverter2 = go.AddComponent<ElementConverter>();
    elementConverter2.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(new Tag("FishMilk"), 1f)
    };
    elementConverter2.outputElements = new ElementConverter.OutputElement[2]
    {
      new ElementConverter.OutputElement(0.809999943f, SimHashes.Mucus, 0.0f, storeOutput: true, diseaseWeight: 0.0f),
      new ElementConverter.OutputElement(0.100000024f, SimHashes.CarbonDioxide, 348.15f, outputElementOffsetx: 1f, outputElementOffsety: 3f, diseaseWeight: 0.0f)
    };
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.consumptionRate = 10f;
    conduitConsumer.capacityKG = 4f;
    conduitConsumer.capacityTag = GameTags.Liquid;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.invertElementFilter = true;
    conduitDispenser.elementFilter = new SimHashes[2]
    {
      SimHashes.Milk,
      SimHashes.FishMilk
    };
    MilkSeparator.Def def = go.AddOrGetDef<MilkSeparator.Def>();
    def.MILK_FAT_CAPACITY = 15f;
    def.CAVIAR_PRODUCTION_RATE = 0.0899999961f;
    def.MILK_SEPARATED_LIQUID_OUTPUT_TAG = MilkFatSeparatorConfig.MILK_SEPARATED_LIQUID_OUTPUT_TAG;
    def.FISHMILK_SEPARATED_LIQUID_OUTPUT_TAG = MilkFatSeparatorConfig.FISHMILK_SEPARATED_LIQUID_OUTPUT_TAG;
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    SymbolOverrideControllerUtil.AddToPrefab(go);
  }

  public override void ConfigurePost(BuildingDef def)
  {
  }
}
