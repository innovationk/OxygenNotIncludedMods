// Decompiled with JetBrains decompiler
// Type: BlueGrassConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BlueGrassConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "BlueGrass";
  public const string SEED_ID = "BlueGrassSeed";
  public const float CO2_RATE = 0.002f;
  public const float FERTILIZATION_RATE = 20f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.BLUE_GRASS.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.BLUE_GRASS.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "bluegrass_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("BlueGrass", name1, desc1, 2f, anim1, "idle_full", Grid.SceneLayer.BuildingFront, 1, 2, decor, noise, defaultTemperature: 240f);
    GameObject template = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.BLUE_GRASS.NAME;
    SimHashes[] safe_elements = new SimHashes[1]
    {
      SimHashes.CarbonDioxide
    };
    string baseTraitName = name2;
    EntityTemplates.ExtendEntityToBasicPlant(template, 193.15f, 193.15f, 273.15f, 273.15f, safe_elements, pressure_warning_low: 0.0f, crop_id: "OxyRock", baseTraitId: "BlueGrassOriginal", baseTraitName: baseTraitName);
    ElementConsumer elementConsumer = placedEntity.AddOrGet<ElementConsumer>();
    elementConsumer.showInStatusPanel = true;
    elementConsumer.storeOnConsume = false;
    elementConsumer.elementToConsume = SimHashes.CarbonDioxide;
    elementConsumer.configuration = ElementConsumer.Configuration.Element;
    elementConsumer.consumptionRadius = (byte) 2;
    elementConsumer.EnableConsumption(true);
    elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f);
    elementConsumer.consumptionRate = 0.0005f;
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Ice.CreateTag(),
        massConsumptionRate = 0.0333333351f
      }
    });
    placedEntity.GetComponent<UprootedMonitor>();
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<BlueGrass>();
    GameObject plant = placedEntity;
    string name3 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BLUE_GRASS.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BLUE_GRASS.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_bluegrass_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.BLUE_GRASS.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Harvest, "BlueGrassSeed", name3, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 4, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), "BlueGrass_preview", Assets.GetAnim((HashedString) "bluegrass_kanim"), "place", 1, 1);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
