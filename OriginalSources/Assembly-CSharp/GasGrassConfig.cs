// Decompiled with JetBrains decompiler
// Type: GasGrassConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class GasGrassConfig : IEntityConfig
{
  public const string ID = "GasGrass";
  public const string SEED_ID = "GasGrassSeed";
  public const float CHLORINE_FERTILIZATION_RATE = 0.000833333354f;
  public const float DIRT_FERTILIZATION_RATE = 0.0416666679f;
  public const int PLANT_FIBER_KG_PER_HARVEST = 400;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.GASGRASS.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.GASGRASS.DESC;
    EffectorValues tieR3 = TUNING.DECOR.BONUS.TIER3;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "gassygrass_kanim");
    EffectorValues decor = tieR3;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("GasGrass", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 3, decor, noise, defaultTemperature: (float) byte.MaxValue);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, temperature_warning_low: 0.0f, temperature_warning_high: 348.15f, temperature_lethal_high: 373.15f, pressure_sensitive: false, crop_id: "PlantFiber", max_radiation: 12200f, baseTraitId: "GasGrassOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.GASGRASS.NAME);
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Chlorine,
        massConsumptionRate = 0.000833333354f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Dirt.CreateTag(),
        massConsumptionRate = 0.0416666679f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<DirectlyEdiblePlant_Growth>();
    Modifiers component = placedEntity.GetComponent<Modifiers>();
    Db.Get().traits.Get(component.initialTraits[0]).Add(new AttributeModifier(Db.Get().PlantAttributes.MinLightLux.Id, 10000f, (string) STRINGS.CREATURES.SPECIES.GASGRASS.NAME));
    component.initialAttributes.Add(Db.Get().PlantAttributes.MinLightLux.Id);
    placedEntity.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness();
    GameObject plant = placedEntity;
    IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
    int num = DlcManager.FeaturePlantMutationsEnabled() ? 2 : 0;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.GASGRASS.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.GASGRASS.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_gassygrass_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.GASGRASS.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, (SeedProducer.ProductionType) num, "GasGrassSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 22, domesticatedDescription: domesticatedDescription, width: 0.2f, height: 0.2f), "GasGrass_preview", Assets.GetAnim((HashedString) "gassygrass_kanim"), "place", 1, 1);
    SoundEventVolumeCache.instance.AddVolume("gassygrass_kanim", "GasGrass_grow", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("gassygrass_kanim", "GasGrass_harvest", NOISE_POLLUTION.CREATURES.TIER3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
