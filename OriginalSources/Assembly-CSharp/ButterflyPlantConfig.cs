// Decompiled with JetBrains decompiler
// Type: ButterflyPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ButterflyPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "ButterflyPlant";
  public const string SEED_ID = "ButterflyPlantSeed";

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.BUTTERFLYPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.BUTTERFLYPLANT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "pollinator_plant_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ButterflyPlant", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 233.15f, temperature_warning_high: 318.15f, temperature_lethal_high: 353.15f, safe_elements: new SimHashes[4]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide,
      SimHashes.ChlorineGas
    }, crop_id: "Butterfly", max_radiation: 7400f, baseTraitId: "ButterflyPlantOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.BUTTERFLYPLANT.NAME);
    Object.DestroyImmediate((Object) placedEntity.GetComponent<MutantPlant>());
    Object.DestroyImmediate((Object) placedEntity.GetComponent<HarvestDesignatable>());
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Dirt,
        massConsumptionRate = 0.0166666675f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<LoopingSounds>();
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BUTTERFLYPLANTSEED.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BUTTERFLYPLANTSEED.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_pollinator_plant_kanim");
    EdiblesManager.FoodInfo butterflySeed = TUNING.FOOD.FOOD_TYPES.BUTTERFLY_SEED;
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.BUTTERFLYPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlantAsFood(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Crop, "ButterflyPlantSeed", name2, desc2, anim2, butterflySeed, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 2, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f, ignoreDefaultSeedTag: true), "ButterflyPlant_preview", Assets.GetAnim((HashedString) "pollinator_plant_kanim"), "place", 1, 2);
    placedEntity.AddOrGet<Growing>().maxAge = 0.0f;
    placedEntity.AddOrGet<Crop>().cropSpawnOffset = new Vector3(-0.0365f, 1.26175f, 0.0f);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
