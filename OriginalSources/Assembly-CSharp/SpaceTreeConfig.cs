// Decompiled with JetBrains decompiler
// Type: SpaceTreeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SpaceTreeConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SpaceTree";
  public const string SEED_ID = "SpaceTreeSeed";
  public const float Temperature_lethal_low = 173.15f;
  public const float Temperature_warning_low = 198.15f;
  public const float Temperature_warning_high = 258.15f;
  public const float Temperature_lethal_high = 293.15f;
  public const float SNOW_RATE = 0.166666672f;
  public const float ENTOMB_DEFENSE_COOLDOWN = 5f;
  public static CellOffset OUTPUT_CONDUIT_CELL_OFFSET = new CellOffset(0, 1);
  public const float TRUNK_GROWTH_DURATION = 2700f;
  public const int MAX_BRANCH_NUMBER = 5;
  public const int OPTIMAL_LUX = 10000;
  public const float MIN_REQUIRED_LIGHT_TO_GROW_BRANCHES = 300f;
  public const float SUGAR_WATER_PRODUCTION_DURATION = 150f;
  public const float SUGAR_WATER_CAPACITY = 20f;
  public const string MANUAL_HARVEST_PRE_ANIM_NAME = "syrup_harvest_trunk_pre";
  public const string MANUAL_HARVEST_LOOP_ANIM_NAME = "syrup_harvest_trunk_loop";
  public const string MANUAL_HARVEST_PST_ANIM_NAME = "syrup_harvest_trunk_pst";
  public const string MANUAL_HARVEST_INTERRUPT_ANIM_NAME = "syrup_harvest_trunk_loop";
  private static readonly List<Storage.StoredItemModifier> storedItemModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Preserve,
    Storage.StoredItemModifier.Insulate,
    Storage.StoredItemModifier.Seal
  };

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.SPACETREE.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.SPACETREE.DESC;
    EffectorValues tieR1 = TUNING.DECOR.PENALTY.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "syrup_tree_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("SpaceTree", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise, defaultTemperature: (float) byte.MaxValue);
    string baseTraitId = "SpaceTreeOriginal";
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 173.15f, 198.15f, 258.15f, 293.15f, new SimHashes[5]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide,
      SimHashes.Snow,
      SimHashes.Vacuum
    }, false, can_tinker: false, should_grow_old: false, max_radiation: 12200f, baseTraitId: baseTraitId, baseTraitName: (string) STRINGS.CREATURES.SPECIES.SPACETREE.NAME);
    WiltCondition component1 = placedEntity.GetComponent<WiltCondition>();
    component1.WiltDelay = 0.0f;
    component1.RecoveryDelay = 0.0f;
    Modifiers component2 = placedEntity.GetComponent<Modifiers>();
    if ((UnityEngine.Object) placedEntity.GetComponent<Traits>() == (UnityEngine.Object) null)
    {
      placedEntity.AddOrGet<Traits>();
      component2.initialTraits.Add(baseTraitId);
    }
    Crop.CropVal cropval = TUNING.CROPS.CROP_TYPES.Find((Predicate<Crop.CropVal>) (m => (Tag) m.cropId == SimHashes.SugarWater.CreateTag()));
    Trait trait = Db.Get().traits.Get(component2.initialTraits[0]);
    component2.initialAmounts.Add(Db.Get().Amounts.Maturity.Id);
    AttributeModifier modifier = new AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute.Id, 4.5f, (string) STRINGS.CREATURES.SPECIES.SPACETREE.NAME);
    trait.Add(modifier);
    placedEntity.AddOrGet<Crop>().Configure(cropval);
    KPrefabID component3 = placedEntity.GetComponent<KPrefabID>();
    HashSet<Tag> harvestableIds = OverlayScreen.HarvestableIDs;
    Tag tag = component3.PrefabID();
    string id = tag.ToString();
    GeneratedBuildings.RegisterWithOverlay(harvestableIds, id);
    if (DlcManager.FeaturePlantMutationsEnabled())
    {
      placedEntity.AddOrGet<MutantPlant>().SpeciesID = component3.PrefabTag;
      SymbolOverrideControllerUtil.AddToPrefab(placedEntity);
    }
    Growing growing = placedEntity.AddOrGet<Growing>();
    growing.shouldGrowOld = false;
    growing.maxAge = 2400f;
    placedEntity.AddOrGet<HarvestDesignatable>();
    placedEntity.AddOrGet<LoopingSounds>();
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SPACETREE.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SPACETREE.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_syrup_tree_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.SPACETREE.DOMESTICATEDDESC;
    tag = new Tag();
    Tag replantGroundTag = tag;
    string domesticatedDescription = domesticateddesc;
    GameObject registerSeedForPlant = EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Harvest, "SpaceTreeSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 1, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f);
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Snow.CreateTag(),
        massConsumptionRate = 0.166666672f
      }
    });
    EntityTemplates.CreateAndRegisterPreviewForPlant(registerSeedForPlant, "SpaceTree_preview", Assets.GetAnim((HashedString) "syrup_tree_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_harvest", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_LP", NOISE_POLLUTION.CREATURES.TIER4);
    DirectlyEdiblePlant_StorageElement plantStorageElement = placedEntity.AddOrGet<DirectlyEdiblePlant_StorageElement>();
    plantStorageElement.tagToConsume = SimHashes.SugarWater.CreateTag();
    plantStorageElement.rateProducedPerCycle = 4f;
    plantStorageElement.storageCapacity = 20f;
    plantStorageElement.edibleCellOffsets = new CellOffset[4]
    {
      new CellOffset(-1, 0),
      new CellOffset(1, 0),
      new CellOffset(-1, 1),
      new CellOffset(1, 1)
    };
    DirectlyEdiblePlant_TreeBranches plantTreeBranches = placedEntity.AddOrGet<DirectlyEdiblePlant_TreeBranches>();
    plantTreeBranches.overrideCropID = "SpaceTreeBranch";
    plantTreeBranches.MinimumEdibleMaturity = 1f;
    Storage storage = placedEntity.AddOrGet<Storage>();
    storage.allowItemRemoval = false;
    storage.showInUI = true;
    storage.capacityKg = 20f;
    storage.SetDefaultStoredItemModifiers(SpaceTreeConfig.storedItemModifiers);
    ConduitDispenser conduitDispenser = placedEntity.AddOrGet<ConduitDispenser>();
    conduitDispenser.noBuildingOutputCellOffset = SpaceTreeConfig.OUTPUT_CONDUIT_CELL_OFFSET;
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.alwaysDispense = true;
    conduitDispenser.SetOnState(false);
    placedEntity.AddOrGet<SpaceTreeSyrupHarvestWorkable>();
    UnstableEntombDefense.Def def1 = placedEntity.AddOrGetDef<UnstableEntombDefense.Def>();
    def1.defaultAnimName = "shake_trunk";
    def1.Cooldown = 5f;
    PlantBranchGrower.Def def2 = placedEntity.AddOrGetDef<PlantBranchGrower.Def>();
    def2.BRANCH_OFFSETS = new CellOffset[5]
    {
      new CellOffset(-1, 1),
      new CellOffset(-1, 2),
      new CellOffset(0, 2),
      new CellOffset(1, 2),
      new CellOffset(1, 1)
    };
    def2.BRANCH_PREFAB_NAME = "SpaceTreeBranch";
    def2.harvestOnDrown = true;
    def2.propagateHarvestDesignation = false;
    def2.MAX_BRANCH_COUNT = 5;
    SpaceTreePlant.Def def3 = placedEntity.AddOrGetDef<SpaceTreePlant.Def>();
    def3.OptimalProductionDuration = 150f;
    def3.OptimalAmountOfBranches = 5;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    EntityCellVisualizer entityCellVisualizer = inst.AddOrGet<EntityCellVisualizer>();
    entityCellVisualizer.AddPort(EntityCellVisualizer.Ports.LiquidOut, SpaceTreeConfig.OUTPUT_CONDUIT_CELL_OFFSET, (Color) entityCellVisualizer.Resources.liquidIOColours.output.connected);
  }
}
