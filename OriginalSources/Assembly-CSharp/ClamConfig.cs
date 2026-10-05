// Decompiled with JetBrains decompiler
// Type: ClamConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ClamConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Clam";
  public const string SEED_ID = "ClamSeed";
  public const float LIFETIME_CYCLES = 8f;
  public const int HARVEST_YIELD_MASS = 50;
  public const float SAND_CONSUMPTION_RATE = 0.0583333336f;
  public static StandardCropPlant.AnimSet CROP_PLANT_DEFAULT_ANIM_SET = new StandardCropPlant.AnimSet(StandardCropPlant.defaultAnimSet)
  {
    wilt_recover_base = "wilt_recover"
  };
  public static StandardCropPlant.AnimSet CROP_PLANT_CLOSED_ANIM_SET = new StandardCropPlant.AnimSet(StandardCropPlant.defaultAnimSet)
  {
    grow_pst = "idle_full_closed",
    idle_full = "idle_full_closed",
    wilt_recover_base = "wilt_recover"
  };

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.CLAM.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.CLAM.DESC;
    EffectorValues tieR2 = TUNING.DECOR.BONUS.TIER2;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "clam_kanim");
    EffectorValues decor = tieR2;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("Clam", name1, desc1, 1f, anim1, "idle_full", Grid.SceneLayer.BuildingBack, 3, 3, decor, noise, defaultTemperature: 303.15f);
    placedEntity.AddOrGet<ClamHarvestable>();
    GameObject template = placedEntity;
    string str = SimHashes.Pearl.ToString();
    SimHashes[] allWaters = PLANTS.SAFE_ELEMENTS.AllWaters;
    string crop_id = str;
    string name2 = (string) STRINGS.CREATURES.SPECIES.PLANKTONCORAL.NAME;
    GameObject basicPlant = EntityTemplates.ExtendEntityToBasicPlant(template, 273.15f, 298.15f, 318.15f, 373.15f, allWaters, false, crop_id: crop_id, can_drown: false, baseTraitId: "ClamOriginal", baseTraitName: name2);
    basicPlant.AddOrGet<StandardCropPlant>();
    basicPlant.AddOrGet<LoopingSounds>();
    GameObject plant = basicPlant;
    string name3 = (string) STRINGS.CREATURES.SPECIES.SEEDS.CLAM.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.CLAM.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_clam_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.LargeSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.CLAM.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Harvest, "ClamSeed", name3, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 20, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), "Clam_preview", Assets.GetAnim((HashedString) "clam_kanim"), "place", 3, 3);
    EntityTemplates.ExtendPlantToFertilizable(basicPlant, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Sand.CreateTag(),
        massConsumptionRate = 0.0583333336f
      }
    });
    basicPlant.AddOrGet<PressureVulnerable>().allCellsMustBeSafe = true;
    basicPlant.AddOrGet<ClamPoopStation>();
    basicPlant.AddTag(GameTags.BlockBuildOverPlantFeature);
    basicPlant.GetComponent<Growing>().shouldGrowOld = false;
    basicPlant.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[3]
    {
      new CellOffset(0, -1),
      new CellOffset(-1, -1),
      new CellOffset(1, -1)
    };
    return basicPlant;
  }

  public void OnPrefabInit(GameObject prefab)
  {
    prefab.GetComponent<StandardCropPlant>().anims = ClamConfig.CROP_PLANT_DEFAULT_ANIM_SET;
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
