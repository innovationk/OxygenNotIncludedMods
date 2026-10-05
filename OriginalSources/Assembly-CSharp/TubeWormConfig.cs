// Decompiled with JetBrains decompiler
// Type: TubeWormConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class TubeWormConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "TubeWorm";
  public const string SEED_ID = "TubeWormSeed";
  public const float LIFETIME_CYCLES = 8f;
  public const int HARVEST_YIELD = 200;
  public const float SULFUR_CONSUMPTION_RATE = 0.0333333351f;
  public const float MURKY_BRINE_CONSUMPTION_RATE = 0.05f;
  public const SimHashes PRODUCT_ELEMENT = SimHashes.Polypropylene;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.TUBEWORM.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.TUBEWORM.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "tube_worm_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("TubeWorm", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, decor, noise, defaultTemperature: 348.15f);
    string str = SimHashes.Polypropylene.ToString();
    SimHashes[] murkyWaters = PLANTS.SAFE_ELEMENTS.MurkyWaters;
    string crop_id = str;
    string name2 = (string) STRINGS.CREATURES.SPECIES.TUBEWORM.NAME;
    GameObject basicPlant = EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 303.15f, 323.15f, 383.15f, 403.15f, murkyWaters, false, crop_id: crop_id, can_drown: false, can_tinker: false, baseTraitId: "TubeWormOriginal", baseTraitName: name2);
    basicPlant.AddOrGet<StandardCropPlant>();
    basicPlant.AddOrGet<DirectlyEdiblePlant_Growth>();
    basicPlant.AddOrGet<LoopingSounds>();
    EntityTemplates.ExtendPlantToFertilizable(basicPlant, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Sulfur.CreateTag(),
        massConsumptionRate = 0.0333333351f
      }
    });
    EntityTemplates.ExtendPlantToIrrigated(basicPlant, new PlantElementAbsorber.ConsumeInfo[2]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.MurkyBrine.CreateTag(),
        massConsumptionRate = 0.05f
      },
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Brine.CreateTag(),
        massConsumptionRate = 0.05f
      }
    });
    basicPlant.AddOrGet<PressureVulnerable>().allCellsMustBeSafe = true;
    GameObject plant = basicPlant;
    string name3 = (string) STRINGS.CREATURES.SPECIES.SEEDS.TUBEWORM.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.TUBEWORM.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_tube_worm_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.TUBEWORM.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Harvest, "TubeWormSeed", name3, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 21, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), "TubeWorm_preview", Assets.GetAnim((HashedString) "tube_worm_kanim"), "place", 1, 2);
    return basicPlant;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
