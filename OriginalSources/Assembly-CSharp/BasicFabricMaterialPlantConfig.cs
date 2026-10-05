// Decompiled with JetBrains decompiler
// Type: BasicFabricMaterialPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class BasicFabricMaterialPlantConfig : IEntityConfig
{
  public static string ID = "BasicFabricPlant";
  public static string SEED_ID = "BasicFabricMaterialPlantSeed";
  public const float WATER_RATE = 0.266666681f;

  public GameObject CreatePrefab()
  {
    string id1 = BasicFabricMaterialPlantConfig.ID;
    string name1 = (string) STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.DESC;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "swampreed_kanim");
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingBack, 1, 3, decor, noise);
    GameObject template = placedEntity;
    string id2 = BasicFabricConfig.ID;
    SimHashes[] safe_elements = new SimHashes[5]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide,
      SimHashes.DirtyWater,
      SimHashes.Water
    };
    string crop_id = id2;
    string baseTraitId = BasicFabricMaterialPlantConfig.ID + "Original";
    string name2 = (string) STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.NAME;
    EntityTemplates.ExtendEntityToBasicPlant(template, 248.15f, 295.15f, 310.15f, safe_elements: safe_elements, pressure_sensitive: false, crop_id: crop_id, can_drown: false, max_radiation: 4600f, baseTraitId: baseTraitId, baseTraitName: name2);
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.DirtyWater,
        massConsumptionRate = 0.266666681f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<DirectlyEdiblePlant_Growth>();
    placedEntity.AddOrGet<LoopingSounds>();
    GameObject plant = placedEntity;
    IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
    string seedId = BasicFabricMaterialPlantConfig.SEED_ID;
    string name3 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BASICFABRICMATERIALPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BASICFABRICMATERIALPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_swampreed_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.WaterSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    GameObject registerSeedForPlant = EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, SeedProducer.ProductionType.Harvest, seedId, name3, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 20, domesticatedDescription: domesticatedDescription);
    Assets.GetPrefab((Tag) BasicFabricMaterialPlantConfig.SEED_ID).AddOrGet<CodexEntryRedirector>().CodexID = "BASICFABRICPLANT";
    string id3 = BasicFabricMaterialPlantConfig.ID + "_preview";
    KAnimFile anim3 = Assets.GetAnim((HashedString) "swampreed_kanim");
    EntityTemplates.CreateAndRegisterPreviewForPlant(registerSeedForPlant, id3, anim3, "place", 1, 3);
    SoundEventVolumeCache.instance.AddVolume("swampreed_kanim", "FabricPlant_grow", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("swampreed_kanim", "FabricPlant_harvest", NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
