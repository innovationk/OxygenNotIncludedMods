// Decompiled with JetBrains decompiler
// Type: CarrotPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class CarrotPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "CarrotPlant";
  public const string SEED_ID = "CarrotPlantSeed";
  public const float Temperature_lethal_low = 118.149994f;
  public const float Temperature_warning_low = 218.15f;
  public const float Temperature_lethal_high = 269.15f;
  public const float Temperature_warning_high = 259.15f;
  public const float FERTILIZATION_RATE = 0.025f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.CARROTPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.CARROTPLANT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.PENALTY.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "purpleroot_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("CarrotPlant", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise, defaultTemperature: (float) byte.MaxValue);
    GameObject template = placedEntity;
    string id = CarrotConfig.ID;
    SimHashes[] safe_elements = new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    };
    string crop_id = id;
    string name2 = (string) STRINGS.CREATURES.SPECIES.CARROTPLANT.NAME;
    EntityTemplates.ExtendEntityToBasicPlant(template, 118.149994f, 218.15f, 259.15f, 269.15f, safe_elements, crop_id: crop_id, max_radiation: 4600f, baseTraitId: "CarrotPlantOriginal", baseTraitName: name2);
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<DirectlyEdiblePlant_Growth>();
    placedEntity.AddOrGet<LoopingSounds>();
    GameObject plant = placedEntity;
    string name3 = (string) STRINGS.CREATURES.SPECIES.SEEDS.CARROTPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.CARROTPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_purpleroot_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.CARROTPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    GameObject registerSeedForPlant = EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Harvest, "CarrotPlantSeed", name3, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 1, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f);
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Ethanol.CreateTag(),
        massConsumptionRate = 0.025f
      }
    });
    EntityTemplates.CreateAndRegisterPreviewForPlant(registerSeedForPlant, "CarrotPlant_preview", Assets.GetAnim((HashedString) "purpleroot_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_grow", NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
