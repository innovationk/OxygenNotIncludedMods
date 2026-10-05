// Decompiled with JetBrains decompiler
// Type: HardSkinBerryPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class HardSkinBerryPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "HardSkinBerryPlant";
  public const string SEED_ID = "HardSkinBerryPlantSeed";
  public const float Temperature_lethal_low = 118.149994f;
  public const float Temperature_warning_low = 218.15f;
  public const float Temperature_lethal_high = 269.15f;
  public const float Temperature_warning_high = 259.15f;
  public const float FERTILIZATION_RATE = 0.008333334f;
  public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 12f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.HARDSKINBERRYPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.HARDSKINBERRYPLANT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.PENALTY.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "ice_berry_bush_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("HardSkinBerryPlant", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise, defaultTemperature: (float) byte.MaxValue);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 118.149994f, 218.15f, 259.15f, 269.15f, new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, crop_id: "HardSkinBerry", max_radiation: 4600f, baseTraitId: "HardSkinBerryPlantOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.HARDSKINBERRYPLANT.NAME);
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<DirectlyEdiblePlant_Growth>();
    placedEntity.AddOrGet<LoopingSounds>();
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.HARDSKINBERRYPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.HARDSKINBERRYPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_ice_berry_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.HARDSKINBERRYPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    GameObject registerSeedForPlant = EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Harvest, "HardSkinBerryPlantSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 1, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f);
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Phosphorite.CreateTag(),
        massConsumptionRate = 0.008333334f
      }
    });
    EntityTemplates.CreateAndRegisterPreviewForPlant(registerSeedForPlant, "HardSkinBerryPlant_preview", Assets.GetAnim((HashedString) "ice_berry_bush_kanim"), "place", 1, 2);
    placedEntity.AddOrGet<PlantFiberProducer>().amount = 12f;
    SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_harvest", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("meallice_kanim", "MealLice_LP", NOISE_POLLUTION.CREATURES.TIER4);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
