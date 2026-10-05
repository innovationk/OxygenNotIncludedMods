// Decompiled with JetBrains decompiler
// Type: SpiceVineConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpiceVineConfig : IEntityConfig
{
  public const string ID = "SpiceVine";
  public const string SEED_ID = "SpiceVineSeed";
  public const float FERTILIZATION_RATE = 0.00166666671f;
  public const float WATER_RATE = 0.0583333336f;
  public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 16f;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.SPICE_VINE.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.SPICE_VINE.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "vinespicenut_kanim");
    EffectorValues decor = tieR1;
    List<Tag> tagList = new List<Tag>() { GameTags.Hanging };
    EffectorValues noise = new EffectorValues();
    List<Tag> additionalTags1 = tagList;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("SpiceVine", name1, desc1, 2f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 3, decor, noise, additionalTags: additionalTags1, defaultTemperature: 320f);
    EntityTemplates.MakeHangingOffsets(placedEntity, 1, 3);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 258.15f, 308.15f, 358.15f, 448.15f, crop_id: SpiceNutConfig.ID, max_radiation: 9800f, baseTraitId: "SpiceVineOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.SPICE_VINE.NAME);
    Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = tag,
        massConsumptionRate = 0.0583333336f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Phosphorite,
        massConsumptionRate = 1f / 600f
      }
    });
    placedEntity.AddOrGet<DirectlyEdiblePlant_Growth>();
    placedEntity.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[1]
    {
      new CellOffset(0, 1)
    };
    placedEntity.AddOrGet<StandardCropPlant>();
    GameObject plant = placedEntity;
    IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SPICE_VINE.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SPICE_VINE.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_spicenut_kanim");
    List<Tag> additionalTags2 = new List<Tag>();
    additionalTags2.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.SPICE_VINE.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    GameObject registerPreviewForPlant = EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, SeedProducer.ProductionType.Harvest, "SpiceVineSeed", name2, desc2, anim2, additionalTags: additionalTags2, planterDirection: SingleEntityReceptacle.ReceptacleDirection.Bottom, replantGroundTag: replantGroundTag, sortOrder: 4, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), "SpiceVine_preview", Assets.GetAnim((HashedString) "vinespicenut_kanim"), "place", 1, 3);
    placedEntity.AddOrGet<PlantFiberProducer>().amount = 16f;
    EntityTemplates.MakeHangingOffsets(registerPreviewForPlant, 1, 3);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
