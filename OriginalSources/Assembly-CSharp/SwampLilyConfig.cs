// Decompiled with JetBrains decompiler
// Type: SwampLilyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SwampLilyConfig : IEntityConfig
{
  public static string ID = "SwampLily";
  public const string SEED_ID = "SwampLilySeed";
  public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 24f;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.SWAMPLILY.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.SWAMPLILY.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "swamplily_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("SwampLily", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise, defaultTemperature: 328.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 258.15f, 308.15f, 358.15f, 448.15f, new SimHashes[1]
    {
      SimHashes.ChlorineGas
    }, crop_id: SwampLilyFlowerConfig.ID, max_radiation: 4600f, baseTraitId: SwampLilyConfig.ID + "Original", baseTraitName: (string) STRINGS.CREATURES.SPECIES.SWAMPLILY.NAME);
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<DirectlyEdiblePlant_Growth>();
    placedEntity.AddOrGet<PlantFiberProducer>().amount = 24f;
    GameObject plant = placedEntity;
    IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SWAMPLILY.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SWAMPLILY.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_swampLily_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.SWAMPLILY.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, SeedProducer.ProductionType.Harvest, "SwampLilySeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 21, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), SwampLilyConfig.ID + "_preview", Assets.GetAnim((HashedString) "swamplily_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_grow", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_harvest", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_death", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("swamplily_kanim", "SwampLily_death_bloom", NOISE_POLLUTION.CREATURES.TIER3);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, SwampLilyConfig.ID);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
