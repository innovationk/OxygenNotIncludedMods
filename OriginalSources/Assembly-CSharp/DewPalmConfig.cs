// Decompiled with JetBrains decompiler
// Type: DewPalmConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DewPalmConfig : IEntityConfig, IHasDlcRestrictions
{
  public static readonly string ID = "DewPalm";
  public static readonly string SEED_ID = "DewPalmSeed";
  public static readonly string PREVIEW_ID = "DewPalmPreview";
  public static readonly string BASE_TRAIT_ID = "DewPalmOriginal";
  public const float GROWTH_CYCLES = 10f;
  public const int WOOD_HARVEST_YIELD = 700;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string id = DewPalmConfig.ID;
    string name1 = (string) STRINGS.CREATURES.SPECIES.DEWPALM.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.DEWPALM.DESC;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "rubber_tree_kanim");
    EffectorValues tieR2 = TUNING.DECOR.BONUS.TIER2;
    float hot = TUNING.CREATURES.TEMPERATURE.HOT;
    EffectorValues noise = new EffectorValues();
    double defaultTemperature = (double) hot;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, name1, desc1, 100f, anim1, "idle_empty", Grid.SceneLayer.BuildingBack, 3, 4, tieR2, noise, defaultTemperature: (float) defaultTemperature);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, GameUtil.GetTemperatureConvertedToKelvin(16f, GameUtil.TemperatureUnit.Celsius), GameUtil.GetTemperatureConvertedToKelvin(24f, GameUtil.TemperatureUnit.Celsius), GameUtil.GetTemperatureConvertedToKelvin(54f, GameUtil.TemperatureUnit.Celsius), GameUtil.GetTemperatureConvertedToKelvin(56f, GameUtil.TemperatureUnit.Celsius), crop_id: SimHashes.PalmWood.ToString(), max_age: 12000f, baseTraitId: DewPalmConfig.BASE_TRAIT_ID, baseTraitName: (string) STRINGS.CREATURES.SPECIES.DEWPALM.NAME);
    PlantElementAbsorber.ConsumeInfo[] fertilizers = new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Sulfur.CreateTag(),
        massConsumptionRate = 0.0333333351f
      }
    };
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, fertilizers);
    placedEntity.AddOrGet<StandardCropPlant>();
    GameObject plant = placedEntity;
    string seedId = DewPalmConfig.SEED_ID;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.DEWPALM.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.DEWPALM.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_rubbertree_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.LargeSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.DEWPALM.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) null, SeedProducer.ProductionType.Harvest, seedId, name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 3, domesticatedDescription: domesticatedDescription, width: 0.33f, height: 0.33f), DewPalmConfig.PREVIEW_ID, Assets.GetAnim((HashedString) "rubber_tree_kanim"), "place", 3, 4);
    placedEntity.AddOrGet<DirectlyEdiblePlant_Growth>();
    placedEntity.AddTag(GameTags.BlockBuildOverPlantFeature);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
