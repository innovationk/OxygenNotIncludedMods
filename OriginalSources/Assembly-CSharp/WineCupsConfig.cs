// Decompiled with JetBrains decompiler
// Type: WineCupsConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WineCupsConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "WineCups";
  public const string SEED_ID = "WineCupsSeed";
  public static readonly EffectorValues POSITIVE_DECOR_EFFECT = TUNING.DECOR.BONUS.TIER3;
  public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = TUNING.DECOR.PENALTY.TIER3;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.WINECUPS.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.WINECUPS.DESC;
    EffectorValues positiveDecorEffect = WineCupsConfig.POSITIVE_DECOR_EFFECT;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "potted_cups_kanim");
    EffectorValues decor = positiveDecorEffect;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("WineCups", name1, desc1, 1f, anim1, "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, decor, noise);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, safe_elements: new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, can_tinker: false, max_radiation: 900f, baseTraitId: "WineCupsOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.WINECUPS.NAME);
    PrickleGrass prickleGrass = placedEntity.AddOrGet<PrickleGrass>();
    placedEntity.AddOrGetDef<DecorPlantMonitor.Def>();
    prickleGrass.positive_decor_effect = WineCupsConfig.POSITIVE_DECOR_EFFECT;
    prickleGrass.negative_decor_effect = WineCupsConfig.NEGATIVE_DECOR_EFFECT;
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.WINECUPS.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.WINECUPS.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_potted_cups_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.DecorSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.WINECUPS.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Hidden, "WineCupsSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 11, domesticatedDescription: domesticatedDescription), "WineCups_preview", Assets.GetAnim((HashedString) "potted_cups_kanim"), "place", 1, 1);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
