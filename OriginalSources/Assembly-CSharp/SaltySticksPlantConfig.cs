// Decompiled with JetBrains decompiler
// Type: SaltySticksPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SaltySticksPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SaltySticksPlant";
  public const string SEED_ID = "SaltySticksPlantSeed";
  public const float FERTILIZER_RATE = 0.0166666675f;
  public const int GROWTH_CYCLES = 4;
  public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 2f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.SALTYSTICKSPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.SALTYSTICKSPLANT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.PENALTY.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "salty_sticks_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("SaltySticksPlant", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 273.15f, temperature_warning_high: 313.15f, temperature_lethal_high: 318.15f, safe_elements: new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, crop_id: "SaltySticksFood", can_tinker: false, max_radiation: 9800f, baseTraitId: "SaltySticksPlantOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.SALTYSTICKSPLANT.NAME);
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGet<DirectlyEdiblePlant_Growth>();
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SALTYSTICKSPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SALTYSTICKSPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_salty_sticks_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.SALTYSTICKSPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    GameObject registerSeedForPlant = EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Harvest, "SaltySticksPlantSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 1, domesticatedDescription: domesticatedDescription);
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Salt.CreateTag(),
        massConsumptionRate = 0.0166666675f
      }
    });
    EntityTemplates.CreateAndRegisterPreviewForPlant(registerSeedForPlant, "SaltySticksPlant_preview", Assets.GetAnim((HashedString) "salty_sticks_kanim"), "place", 1, 2);
    placedEntity.AddOrGet<PlantFiberProducer>().amount = 2f;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
