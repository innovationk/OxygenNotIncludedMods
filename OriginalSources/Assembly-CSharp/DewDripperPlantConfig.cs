// Decompiled with JetBrains decompiler
// Type: DewDripperPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DewDripperPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "DewDripperPlant";
  public const string SEED_ID = "DewDripperPlantSeed";
  public const float GROWTH_TIME = 1200f;
  public const float FERTILIZER_RATE = 0.0166666675f;
  public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 2f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.DEWDRIPPERPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.DEWDRIPPERPLANT.DESC;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "brackwood_kanim");
    EffectorValues decor = tieR0;
    List<Tag> tagList = new List<Tag>() { GameTags.Hanging };
    EffectorValues noise = new EffectorValues();
    List<Tag> additionalTags1 = tagList;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("DewDripperPlant", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, decor, noise, additionalTags: additionalTags1, defaultTemperature: 253.15f);
    EntityTemplates.MakeHangingOffsets(placedEntity, 1, 2);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, temperature_warning_low: 238.15f, temperature_warning_high: 278.15f, temperature_lethal_high: 308.15f, pressure_warning_low: 0.25f, crop_id: DewDripConfig.ID, can_tinker: false, max_radiation: 4600f, baseTraitId: "DewDripperPlantOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.DEWDRIPPERPLANT.NAME);
    PressureVulnerable pressureVulnerable = placedEntity.AddOrGet<PressureVulnerable>();
    pressureVulnerable.pressureWarning_High = 2f;
    pressureVulnerable.pressureLethal_High = 10f;
    placedEntity.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[1]
    {
      new CellOffset(0, 1)
    };
    placedEntity.AddOrGet<PlantFiberProducer>().amount = 2f;
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<LoopingSounds>();
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.BrineIce.CreateTag(),
        massConsumptionRate = 0.0166666675f
      }
    });
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.DEWDRIPPERPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.DEWDRIPPERPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_brackwood_kanim");
    List<Tag> additionalTags2 = new List<Tag>();
    additionalTags2.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.DEWDRIPPERPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Harvest, "DewDripperPlantSeed", name2, desc2, anim2, additionalTags: additionalTags2, planterDirection: SingleEntityReceptacle.ReceptacleDirection.Bottom, replantGroundTag: replantGroundTag, sortOrder: 5, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), "DewDripperPlant_preview", Assets.GetAnim((HashedString) "brackwood_kanim"), "place", 1, 2), 1, 2);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
