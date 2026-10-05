// Decompiled with JetBrains decompiler
// Type: FilamentPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class FilamentPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public static readonly string ID = "FilamentPlant";
  public static readonly string SEED_ID = "FilamentPlantSeed";
  public static readonly EffectorValues POSITIVE_DECOR_EFFECT = TUNING.DECOR.BONUS.TIER3;
  public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = TUNING.DECOR.PENALTY.TIER3;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string id = FilamentPlantConfig.ID;
    string name1 = (string) STRINGS.CREATURES.SPECIES.FILAMENTPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.FILAMENTPLANT.DESC;
    EffectorValues tieR3 = TUNING.DECOR.BONUS.TIER3;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "potted_petta_pouf_kanim");
    EffectorValues decor = tieR3;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, name1, desc1, 1f, anim1, "idle", Grid.SceneLayer.BuildingFront, 1, 1, decor, noise, defaultTemperature: 303.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 295.15f, 299.15f, 315.15f, 311.15f, PLANTS.SAFE_ELEMENTS.AllWaters, false, can_drown: false, can_tinker: false, baseTraitId: FilamentPlantConfig.ID + "Original", baseTraitName: (string) STRINGS.CREATURES.SPECIES.FILAMENTPLANT.NAME);
    placedEntity.AddOrGetDef<DecorPlantMonitor.Def>();
    PrickleGrass prickleGrass = placedEntity.AddOrGet<PrickleGrass>();
    prickleGrass.positive_decor_effect = TUNING.DECOR.BONUS.TIER3;
    prickleGrass.negative_decor_effect = TUNING.DECOR.PENALTY.TIER3;
    string seedId = FilamentPlantConfig.SEED_ID;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.FILAMENTPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.FILAMENTPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_potted_petta_pouf_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.DecorSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.FILAMENTPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Hidden, seedId, name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 13, domesticatedDescription: domesticatedDescription), FilamentPlantConfig.ID + "_preview", Assets.GetAnim((HashedString) "filament_plant_kanim"), "place", 1, 1);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
