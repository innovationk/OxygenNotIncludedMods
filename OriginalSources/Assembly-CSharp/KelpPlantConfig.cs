// Decompiled with JetBrains decompiler
// Type: KelpPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class KelpPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "KelpPlant";
  public const string SEED_ID = "KelpPlantSeed";
  public const int YIELD_UNITS_PER_HARVEST = 50;
  public const float LIFETIME_CYCLES = 5f;
  public const float FERTILIZATION_RATE = 0.0166666675f;
  public static SimHashes[] ALLOWED_ELEMENTS = new SimHashes[7]
  {
    SimHashes.Water,
    SimHashes.DirtyWater,
    SimHashes.SaltWater,
    SimHashes.Brine,
    SimHashes.MurkyBrine,
    SimHashes.PhytoOil,
    SimHashes.NaturalResin
  };
  public const float CALCULATED_YIELD_MASS_PER_HARVEST = 50f;
  public const float CALCULATED_YIELD_MASS_PER_CYCLE = 10f;
  public const float CALCULATED_GROWTH_PER_CYCLE = 0.2f;
  public const float CALCULATED_LIFETIME_SEC = 3000f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.KELPPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.KELPPLANT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "kelp_kanim");
    EffectorValues decor = tieR1;
    List<Tag> tagList = new List<Tag>() { GameTags.Hanging };
    EffectorValues noise = new EffectorValues();
    List<Tag> additionalTags1 = tagList;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("KelpPlant", name1, desc1, 4f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, decor, noise, additionalTags: additionalTags1, defaultTemperature: 297.15f);
    EntityTemplates.MakeHangingOffsets(placedEntity, 1, 2);
    GameObject template = placedEntity;
    string id = KelpConfig.ID;
    string name2 = (string) STRINGS.CREATURES.SPECIES.KELPPLANT.NAME;
    SimHashes[] allowedElements = KelpPlantConfig.ALLOWED_ELEMENTS;
    string crop_id = id;
    string baseTraitName = name2;
    EntityTemplates.ExtendEntityToBasicPlant(template, 253.15f, 263.15f, 358.15f, 373.15f, allowedElements, false, crop_id: crop_id, can_drown: false, max_radiation: 7400f, baseTraitId: "KelpPlantOriginal", baseTraitName: baseTraitName);
    placedEntity.AddOrGet<PressureVulnerable>().allCellsMustBeSafe = true;
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = (Tag) SimHashes.ToxicSand.ToString(),
        massConsumptionRate = 0.0166666675f
      }
    });
    placedEntity.AddOrGet<DirectlyEdiblePlant_Growth>();
    placedEntity.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[1]
    {
      new CellOffset(0, 1)
    };
    placedEntity.AddOrGet<StandardCropPlant>();
    GameObject plant = placedEntity;
    string name3 = (string) STRINGS.CREATURES.SPECIES.SEEDS.KELPPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.KELPPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_kelp_kanim");
    List<Tag> additionalTags2 = new List<Tag>();
    additionalTags2.Add(GameTags.WaterSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.KELPPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Harvest, "KelpPlantSeed", name3, desc2, anim2, additionalTags: additionalTags2, planterDirection: SingleEntityReceptacle.ReceptacleDirection.Bottom, replantGroundTag: replantGroundTag, sortOrder: 4, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), "KelpPlant_preview", Assets.GetAnim((HashedString) "kelp_kanim"), "place", 1, 2), 1, 2);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
