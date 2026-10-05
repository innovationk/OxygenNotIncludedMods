// Decompiled with JetBrains decompiler
// Type: PlanktonCoralConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class PlanktonCoralConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "PlanktonCoral";
  public const string SEED_ID = "PlanktonCoralSeed";
  public const float LIFETIME_CYCLES = 4f;
  public const int HARVEST_YIELD_MASS = 80 /*0x50*/;
  public const float CALCULATED_YIELD_MASS_PER_HARVEST = 80f;
  public const float CALCULATED_YIELD_MASS_PER_CYCLE = 20f;
  public const float CALCULATED_GROWTH_PER_CYCLE = 0.25f;
  public const float CALCULATED_LIFETIME_SEC = 2400f;
  public const float CALCIUM_CARBONATE_CONSUMPTION_RATE = 0.025f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.PLANKTONCORAL.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.PLANKTONCORAL.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "parrotfish_coral_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PlanktonCoral", name1, desc1, 1f, anim1, "idle_full", Grid.SceneLayer.Building, 2, 2, decor, noise, defaultTemperature: 303.15f);
    string str = SimHashes.Phosphorite.ToString();
    SimHashes[] allWaters = PLANTS.SAFE_ELEMENTS.AllWaters;
    string crop_id = str;
    string name2 = (string) STRINGS.CREATURES.SPECIES.PLANKTONCORAL.NAME;
    GameObject basicPlant = EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 273.15f, 298.15f, 318.15f, 373.15f, allWaters, false, crop_id: crop_id, can_drown: false, require_solid_tile: false, require_Backwall_Foundation: true, baseTraitId: "PlanktonCoralOriginal", baseTraitName: name2);
    basicPlant.AddOrGet<StandardCropPlant>();
    basicPlant.AddOrGet<LoopingSounds>();
    GameObject plant = basicPlant;
    string name3 = (string) STRINGS.CREATURES.SPECIES.SEEDS.PLANKTONCORAL.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.PLANKTONCORAL.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_parrotfish_coral_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    additionalTags.Add(GameTags.BackwallSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.PLANKTONCORAL.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Harvest, "PlanktonCoralSeed", name3, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 20, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), "PlanktonCoral_preview", Assets.GetAnim((HashedString) "parrotfish_coral_kanim"), "place", 2, 2);
    PlantElementAbsorber.ConsumeInfo[] fertilizers = new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Coquina.CreateTag(),
        massConsumptionRate = 0.0333333351f
      }
    };
    EntityTemplates.ExtendPlantToFertilizable(basicPlant, fertilizers);
    basicPlant.AddOrGet<PressureVulnerable>().allCellsMustBeSafe = true;
    basicPlant.AddOrGet<DirectlyEdiblePlant_Growth>();
    return basicPlant;
  }

  public void OnPrefabInit(GameObject prefab)
  {
    EntityTemplates.ExtendPlantEntityToRequireBackwall(prefab);
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
