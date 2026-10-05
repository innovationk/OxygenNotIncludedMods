// Decompiled with JetBrains decompiler
// Type: DinofernConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class DinofernConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Dinofern";
  public const string SEED_ID = "DinofernSeed";
  public const float CHLORINE_CONSUMPTION_RATE = 0.09f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.DINOFERN.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.DINOFERN.DESC;
    EffectorValues tieR1 = TUNING.DECOR.PENALTY.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "prehistoric_fern_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("Dinofern", name1, desc1, 1f, anim1, "idle_full", Grid.SceneLayer.BuildingBack, 3, 3, decor, noise, defaultTemperature: 253.15f);
    string id = FernFoodConfig.ID;
    SimHashes[] safe_elements = new SimHashes[1]
    {
      SimHashes.ChlorineGas
    };
    string crop_id = id;
    string name2 = (string) STRINGS.CREATURES.SPECIES.DINOFERN.NAME;
    GameObject basicPlant = EntityTemplates.ExtendEntityToBasicPlant(placedEntity, temperature_warning_low: 228.15f, temperature_warning_high: 288.15f, temperature_lethal_high: 308.15f, safe_elements: safe_elements, pressure_warning_low: 0.5f, crop_id: crop_id, can_tinker: false, baseTraitId: "DinofernOriginal", baseTraitName: name2);
    basicPlant.AddOrGet<LoopingSounds>();
    basicPlant.AddOrGet<StandardCropPlant>();
    basicPlant.AddOrGet<Dinofern>();
    Storage storage = basicPlant.AddOrGet<Storage>();
    storage.showInUI = false;
    storage.capacityKg = 1f;
    ElementConsumer elementConsumer = basicPlant.AddOrGet<ElementConsumer>();
    elementConsumer.showInStatusPanel = true;
    elementConsumer.storeOnConsume = false;
    elementConsumer.elementToConsume = SimHashes.ChlorineGas;
    elementConsumer.configuration = ElementConsumer.Configuration.Element;
    elementConsumer.consumptionRadius = (byte) 4;
    elementConsumer.EnableConsumption(false);
    elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f);
    elementConsumer.consumptionRate = 0.09f;
    string name3 = (string) STRINGS.CREATURES.SPECIES.SEEDS.DINOFERN.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.DINOFERN.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_megafrond_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.DINOFERN.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(basicPlant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Hidden, "DinofernSeed", name3, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 20, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), "Dinofern_preview", Assets.GetAnim((HashedString) "prehistoric_fern_kanim"), "place", 3, 3);
    basicPlant.AddTag(GameTags.BlockBuildOverPlantFeature);
    SoundEventVolumeCache.instance.AddVolume("oxy_fern_kanim", "MealLice_harvest", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("oxy_fern_kanim", "MealLice_LP", NOISE_POLLUTION.CREATURES.TIER4);
    return basicPlant;
  }

  public void OnPrefabInit(GameObject prefab)
  {
    prefab.AddOrGet<StandardCropPlant>().anims = new StandardCropPlant.AnimSet()
    {
      pre_grow = "expand",
      grow = "grow",
      grow_pst = "grow_pst",
      idle_full = "idle_full",
      wilt_base = "wilt",
      harvest = "harvest",
      waning = "waning"
    };
  }

  public void OnSpawn(GameObject inst) => inst.GetComponent<Dinofern>().SetConsumptionRate();
}
