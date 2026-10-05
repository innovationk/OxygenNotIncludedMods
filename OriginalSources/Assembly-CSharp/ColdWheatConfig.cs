// Decompiled with JetBrains decompiler
// Type: ColdWheatConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ColdWheatConfig : IEntityConfig
{
  public const string ID = "ColdWheat";
  public const string SEED_ID = "ColdWheatSeed";
  public const float FERTILIZATION_RATE = 0.008333334f;
  public const float WATER_RATE = 0.0333333351f;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.COLDWHEAT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.COLDWHEAT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "coldwheat_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ColdWheat", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 1, decor, noise, defaultTemperature: (float) byte.MaxValue);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 118.149994f, 218.15f, 278.15f, 358.15f, new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, crop_id: "ColdWheatSeed", max_radiation: 12200f, baseTraitId: "ColdWheatOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.COLDWHEAT.NAME);
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Dirt,
        massConsumptionRate = 0.008333334f
      }
    });
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Water,
        massConsumptionRate = 0.0333333351f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    GameObject plant = placedEntity;
    IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.COLDWHEAT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.COLDWHEAT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_coldwheat_kanim");
    EdiblesManager.FoodInfo coldWheatSeed = TUNING.FOOD.FOOD_TYPES.COLD_WHEAT_SEED;
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.COLDWHEAT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlantAsFood(plant, dlcRestrictions, SeedProducer.ProductionType.Crop, "ColdWheatSeed", name2, desc2, anim2, coldWheatSeed, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 3, domesticatedDescription: domesticatedDescription, width: 0.2f, height: 0.2f, ignoreDefaultSeedTag: true), "ColdWheat_preview", Assets.GetAnim((HashedString) "coldwheat_kanim"), "place", 1, 1);
    SoundEventVolumeCache.instance.AddVolume("coldwheat_kanim", "ColdWheat_grow", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("coldwheat_kanim", "ColdWheat_harvest", NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
