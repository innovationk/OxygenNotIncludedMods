// Decompiled with JetBrains decompiler
// Type: OxyCoralConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class OxyCoralConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "OxyCoral";
  public const string SEED_ID = "OxyCoralSeed";
  public const int MIN_LUX_REQUIRED = 2500;
  public const float MINIONS_SUPPORTED_PER_PLANT = 1.5f;
  public const float LIME_CONSUMPTION_RATE = 0.008333334f;
  public const float WATER_CONSUMPTION_RATE = 0.0333333351f;

  public static float OXYGEN_PER_SECOND
  {
    get => DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * 1.5f;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.OXYCORAL.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.OXYCORAL.DESC;
    EffectorValues tieR2 = TUNING.DECOR.BONUS.TIER2;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "thalassaire_coral_kanim");
    EffectorValues decor = tieR2;
    EffectorValues noise = new EffectorValues();
    GameObject basicPlant = EntityTemplates.ExtendEntityToBasicPlant(EntityTemplates.CreatePlacedEntity("OxyCoral", name1, desc1, 1f, anim1, "grow", Grid.SceneLayer.BuildingBack, 3, 2, decor, noise, defaultTemperature: 303.15f), 253.15f, 298.15f, 318.15f, 373.15f, new SimHashes[5]
    {
      SimHashes.Water,
      SimHashes.SaltWater,
      SimHashes.DirtyWater,
      SimHashes.Brine,
      SimHashes.MurkyBrine
    }, false, can_drown: false, can_tinker: false, baseTraitId: "OxyCoralOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.OXYCORAL.NAME);
    basicPlant.AddOrGet<LoopingSounds>();
    GameObject plant = basicPlant;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.OXYCORAL.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.OXYCORAL.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_thalassaire_coral_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.LargeSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.OXYCORAL.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Hidden, "OxyCoralSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 20, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), "OxyCoral_preview", Assets.GetAnim((HashedString) "thalassaire_coral_kanim"), "place", 3, 2);
    EntityTemplates.ExtendPlantToIrrigated(basicPlant, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = ElementLoader.FindElementByHash(SimHashes.SaltWater).tag,
        massConsumptionRate = 0.0333333351f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(basicPlant, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Lime.CreateTag(),
        massConsumptionRate = 0.008333334f
      }
    });
    basicPlant.AddTag(GameTags.BlockBuildOverPlantFeature);
    OxyCoral.Def def = basicPlant.AddOrGetDef<OxyCoral.Def>();
    def.OxygenProductionRate = OxyCoralConfig.OXYGEN_PER_SECOND;
    def.MinLuxRequired = 2500;
    def.OutputBubbleCells = new CellOffset[3]
    {
      new CellOffset(-1, 1),
      new CellOffset(0, 1),
      new CellOffset(1, 1)
    };
    basicPlant.AddOrGet<PressureVulnerable>().allCellsMustBeSafe = true;
    SoundEventVolumeCache.instance.AddVolume("oxy_fern_kanim", "MealLice_harvest", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("oxy_fern_kanim", "MealLice_LP", NOISE_POLLUTION.CREATURES.TIER4);
    return basicPlant;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
