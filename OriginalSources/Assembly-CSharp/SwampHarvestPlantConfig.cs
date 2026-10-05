// Decompiled with JetBrains decompiler
// Type: SwampHarvestPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SwampHarvestPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SwampHarvestPlant";
  public const string SEED_ID = "SwampHarvestPlantSeed";
  public const float WATER_RATE = 0.06666667f;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.SWAMPHARVESTPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.SWAMPHARVESTPLANT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.PENALTY.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "swampcrop_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("SwampHarvestPlant", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise);
    GameObject template = placedEntity;
    string id = SwampFruitConfig.ID;
    SimHashes[] safe_elements = new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    };
    string crop_id = id;
    string name2 = placedEntity.PrefabID().Name;
    EntityTemplates.ExtendEntityToBasicPlant(template, safe_elements: safe_elements, crop_id: crop_id, max_radiation: 4600f, baseTraitId: "SwampHarvestPlantOriginal", baseTraitName: name2);
    placedEntity.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness(true);
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.DirtyWater,
        massConsumptionRate = 0.06666667f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<LoopingSounds>();
    GameObject plant = placedEntity;
    string name3 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SWAMPHARVESTPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SWAMPHARVESTPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_swampcrop_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.SWAMPHARVESTPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Harvest, "SwampHarvestPlantSeed", name3, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 2, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), "SwampHarvestPlant_preview", Assets.GetAnim((HashedString) "swampcrop_kanim"), "place", 1, 2);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
