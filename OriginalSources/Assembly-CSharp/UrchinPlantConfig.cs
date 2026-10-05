// Decompiled with JetBrains decompiler
// Type: UrchinPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UrchinPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public static readonly SimHashes FertilizerElement = SimHashes.RefinedCarbon;
  public const float FERTILIZATION_RATE = 0.008333334f;
  public const int LIFETIME_CYCLES = 16 /*0x10*/;
  public const int UNITS_PER_HARVEST = 1;
  public const string ID = "UrchinPlant";
  public const string SEED_ID = "UrchinPlantSeed";

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.URCHINPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.URCHINPLANT.DESC;
    EffectorValues tieR2 = TUNING.DECOR.BONUS.TIER2;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "urchin_plant_kanim");
    EffectorValues decor = tieR2;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("UrchinPlant", name1, desc1, 4f, anim1, "idle_full", Grid.SceneLayer.Building, 2, 2, decor, noise, defaultTemperature: 323.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 303.15f, 313.15f, 353.15f, 383.15f, new SimHashes[5]
    {
      SimHashes.MurkyBrine,
      SimHashes.Brine,
      SimHashes.SaltWater,
      SimHashes.DirtyWater,
      SimHashes.Ink
    }, false, crop_id: "Urchin", can_drown: false, require_solid_tile: false, require_Backwall_Foundation: true, baseTraitId: "UrchinPlantOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.URCHINPLANT.NAME);
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = UrchinPlantConfig.FertilizerElement.CreateTag(),
        massConsumptionRate = 0.008333334f
      }
    });
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<PressureVulnerable>().allCellsMustBeSafe = true;
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.URCHINPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.URCHINPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_urchin_plant_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    additionalTags.Add(GameTags.BackwallSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.URCHINPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Harvest, "UrchinPlantSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 2, domesticatedDescription: domesticatedDescription), "UrchinPlant_preview", Assets.GetAnim((HashedString) "urchin_plant_kanim"), "place", 2, 2);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    EntityTemplates.ExtendPlantEntityToRequireBackwall(inst);
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
