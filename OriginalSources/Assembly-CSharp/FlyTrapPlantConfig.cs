// Decompiled with JetBrains decompiler
// Type: FlyTrapPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FlyTrapPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "FlyTrapPlant";
  public const string SEED_ID = "FlyTrapPlantSeed";
  public static readonly StandardCropPlant.AnimSet Default_StandardCropAnimSet = new StandardCropPlant.AnimSet()
  {
    pre_grow = "grow_pre",
    grow = "grow",
    grow_pst = "grow_pst",
    idle_full = "idle_full",
    wilt_base = "wilt",
    harvest = "harvest",
    waning = "waning",
    grow_playmode = KAnim.PlayMode.Paused
  };
  public const int DIGESTION_DURATION_CYCLES = 12;
  public const float DIGESTION_DURATION = 7200f;
  public const int AMBER_PER_HARVEST_KG = 264;

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.FLYTRAPPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.FLYTRAPPLANT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "ceiling_carnie_kanim");
    EffectorValues decor = tieR1;
    List<Tag> tagList = new List<Tag>() { GameTags.Hanging };
    EffectorValues noise = new EffectorValues();
    List<Tag> additionalTags1 = tagList;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("FlyTrapPlant", name1, desc1, 1f, anim1, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 2, decor, noise, additionalTags: additionalTags1, defaultTemperature: 291.15f);
    EntityTemplates.MakeHangingOffsets(placedEntity, 1, 2);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 273.15f, temperature_warning_high: 328.15f, temperature_lethal_high: 348.15f, crop_id: SimHashes.Amber.ToString(), max_radiation: 7400f, baseTraitId: "FlyTrapPlantOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.FLYTRAPPLANT.NAME);
    placedEntity.GetComponent<UprootedMonitor>().monitorCells = new CellOffset[1]
    {
      new CellOffset(0, 1)
    };
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<FlytrapConsumptionMonitor>();
    placedEntity.AddOrGet<Growing>().MaxMaturityValuePercentageToSpawnWith = 0.0f;
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.FLYTRAPPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.FLYTRAPPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_ceiling_carnie_kanim");
    List<Tag> additionalTags2 = new List<Tag>();
    additionalTags2.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.FLYTRAPPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Harvest, "FlyTrapPlantSeed", name2, desc2, anim2, additionalTags: additionalTags2, planterDirection: SingleEntityReceptacle.ReceptacleDirection.Bottom, replantGroundTag: replantGroundTag, sortOrder: 4, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), "FlyTrapPlant_preview", Assets.GetAnim((HashedString) "ceiling_carnie_kanim"), "place", 1, 2), 1, 2);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.AddOrGet<StandardCropPlant>().anims = FlyTrapPlantConfig.Default_StandardCropAnimSet;
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
