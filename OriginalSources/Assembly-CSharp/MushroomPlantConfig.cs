// Decompiled with JetBrains decompiler
// Type: MushroomPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class MushroomPlantConfig : IEntityConfig
{
  public const float FERTILIZATION_RATE = 0.006666667f;
  public const string ID = "MushroomPlant";
  public const string SEED_ID = "MushroomSeed";

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "fungusplant_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("MushroomPlant", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, decor, noise);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 228.15f, 278.15f, 308.15f, safe_elements: new SimHashes[1]
    {
      SimHashes.CarbonDioxide
    }, crop_id: MushroomConfig.ID, max_radiation: 4600f, baseTraitId: "MushroomPlantOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.NAME);
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.SlimeMold,
        massConsumptionRate = 0.006666667f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness(true);
    GameObject plant = placedEntity;
    IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.MUSHROOMPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.MUSHROOMPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_fungusplant_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.MUSHROOMPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, SeedProducer.ProductionType.Harvest, "MushroomSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 3, domesticatedDescription: domesticatedDescription, width: 0.33f, height: 0.33f), "MushroomPlant_preview", Assets.GetAnim((HashedString) "fungusplant_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
