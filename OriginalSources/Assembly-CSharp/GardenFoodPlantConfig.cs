// Decompiled with JetBrains decompiler
// Type: GardenFoodPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class GardenFoodPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "GardenFoodPlant";
  public const string SEED_ID = "GardenFoodPlantSeed";

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.GARDENFOODPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.GARDENFOODPLANT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.PENALTY.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "spike_fruit_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("GardenFoodPlant", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 263.15f, 268.15f, 313.15f, 323.15f, new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, crop_id: "GardenFoodPlantFood", max_radiation: 4600f, baseTraitId: "GardenFoodPlantOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.GARDENFOODPLANT.NAME);
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<DirectlyEdiblePlant_Growth>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<PollinationMonitor.Def>();
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.GARDENFOODPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.GARDENFOODPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_spikefruit_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.GARDENFOODPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    GameObject registerSeedForPlant = EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Harvest, "GardenFoodPlantSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 1, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f);
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Peat.CreateTag(),
        massConsumptionRate = 0.0166666675f
      }
    });
    EntityTemplates.CreateAndRegisterPreviewForPlant(registerSeedForPlant, "GardenFoodPlant_preview", Assets.GetAnim((HashedString) "spike_fruit_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("spike_fruit_kanim", "spike_fruit_harvest", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("spike_fruit_kanim", "spike_fruit_LP", NOISE_POLLUTION.CREATURES.TIER4);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
