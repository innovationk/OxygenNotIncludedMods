// Decompiled with JetBrains decompiler
// Type: GardenDecorPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GardenDecorPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "GardenDecorPlant";
  public const string SEED_ID = "GardenDecorPlantSeed";

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.GARDENDECORPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.GARDENDECORPLANT.DESC;
    EffectorValues tieR3 = TUNING.DECOR.BONUS.TIER3;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "discplant_kanim");
    EffectorValues decor = tieR3;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("GardenDecorPlant", name1, desc1, 1f, anim1, "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, decor, noise);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 263.15f, 268.15f, 313.15f, 323.15f, new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, false, can_tinker: false, baseTraitId: "GardenDecorPlantOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.GARDENDECORPLANT.NAME);
    PrickleGrass prickleGrass = placedEntity.AddOrGet<PrickleGrass>();
    placedEntity.AddOrGetDef<DecorPlantMonitor.Def>();
    prickleGrass.positive_decor_effect = TUNING.DECOR.BONUS.TIER3;
    prickleGrass.negative_decor_effect = TUNING.DECOR.PENALTY.TIER3;
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.GARDENDECORPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.GARDENDECORPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_discplant_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.DecorSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.GARDENDECORPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Hidden, "GardenDecorPlantSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 13, domesticatedDescription: domesticatedDescription), "GardenDecorPlant_preview", Assets.GetAnim((HashedString) "discplant_kanim"), "place", 1, 1);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
