// Decompiled with JetBrains decompiler
// Type: BeanPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BeanPlantConfig : IEntityConfig
{
  public const string ID = "BeanPlant";
  public const string SEED_ID = "BeanPlantSeed";
  public const float FERTILIZATION_RATE = 0.008333334f;
  public const float WATER_RATE = 0.0333333351f;
  public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 42f;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.BEAN_PLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.BEAN_PLANT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "beanplant_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("BeanPlant", name1, desc1, 2f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, decor, noise, defaultTemperature: 258.15f);
    GameObject template = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.BEAN_PLANT.NAME;
    SimHashes[] safe_elements = new SimHashes[1]
    {
      SimHashes.CarbonDioxide
    };
    string baseTraitName = name2;
    EntityTemplates.ExtendEntityToBasicPlant(template, 198.15f, 248.15f, 273.15f, 323.15f, safe_elements, pressure_warning_low: 0.025f, crop_id: "BeanPlantSeed", max_radiation: 9800f, baseTraitId: "BeanPlantOriginal", baseTraitName: baseTraitName);
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Ethanol.CreateTag(),
        massConsumptionRate = 0.0333333351f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Dirt.CreateTag(),
        massConsumptionRate = 0.008333334f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<DirectlyEdiblePlant_Growth>();
    placedEntity.AddOrGet<PlantFiberProducer>().amount = 42f;
    GameObject plant = placedEntity;
    IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
    string name3 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BEAN_PLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BEAN_PLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_beanplant_kanim");
    EdiblesManager.FoodInfo bean = TUNING.FOOD.FOOD_TYPES.BEAN;
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.BEAN_PLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlantAsFood(plant, dlcRestrictions, SeedProducer.ProductionType.Crop, "BeanPlantSeed", name3, desc2, anim2, bean, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 3, domesticatedDescription: domesticatedDescription, collisionShape: EntityTemplates.CollisionShape.RECTANGLE, width: 0.6f, height: 0.3f, ignoreDefaultSeedTag: true), "BeanPlant_preview", Assets.GetAnim((HashedString) "beanplant_kanim"), "place", 1, 2);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
