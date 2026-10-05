// Decompiled with JetBrains decompiler
// Type: ToePlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ToePlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "ToePlant";
  public const string SEED_ID = "ToePlantSeed";
  public static readonly EffectorValues POSITIVE_DECOR_EFFECT = TUNING.DECOR.BONUS.TIER3;
  public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = TUNING.DECOR.PENALTY.TIER3;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.TOEPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.TOEPLANT.DESC;
    EffectorValues positiveDecorEffect = ToePlantConfig.POSITIVE_DECOR_EFFECT;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "potted_toes_kanim");
    EffectorValues decor = positiveDecorEffect;
    float freezing3 = TUNING.CREATURES.TEMPERATURE.FREEZING_3;
    EffectorValues noise = new EffectorValues();
    double defaultTemperature = (double) freezing3;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ToePlant", name1, desc1, 1f, anim1, "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, decor, noise, defaultTemperature: (float) defaultTemperature);
    GameObject template = placedEntity;
    SimHashes[] simHashesArray = new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    };
    double freezing10 = (double) TUNING.CREATURES.TEMPERATURE.FREEZING_10;
    double freezing9 = (double) TUNING.CREATURES.TEMPERATURE.FREEZING_9;
    double freezing = (double) TUNING.CREATURES.TEMPERATURE.FREEZING;
    double cool = (double) TUNING.CREATURES.TEMPERATURE.COOL;
    SimHashes[] safe_elements = simHashesArray;
    string name2 = (string) STRINGS.CREATURES.SPECIES.TOEPLANT.NAME;
    EntityTemplates.ExtendEntityToBasicPlant(template, (float) freezing10, (float) freezing9, (float) freezing, (float) cool, safe_elements, can_tinker: false, baseTraitId: "ToePlantOriginal", baseTraitName: name2);
    PrickleGrass prickleGrass = placedEntity.AddOrGet<PrickleGrass>();
    placedEntity.AddOrGetDef<DecorPlantMonitor.Def>();
    prickleGrass.positive_decor_effect = ToePlantConfig.POSITIVE_DECOR_EFFECT;
    prickleGrass.negative_decor_effect = ToePlantConfig.NEGATIVE_DECOR_EFFECT;
    GameObject plant = placedEntity;
    string name3 = (string) STRINGS.CREATURES.SPECIES.SEEDS.TOEPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.TOEPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_potted_toes_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.DecorSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.TOEPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Hidden, "ToePlantSeed", name3, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 12, domesticatedDescription: domesticatedDescription), "ToePlant_preview", Assets.GetAnim((HashedString) "potted_toes_kanim"), "place", 1, 1);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
