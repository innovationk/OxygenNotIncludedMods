// Decompiled with JetBrains decompiler
// Type: IceFlowerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class IceFlowerConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "IceFlower";
  public const string SEED_ID = "IceFlowerSeed";
  public readonly EffectorValues POSITIVE_DECOR_EFFECT = TUNING.DECOR.BONUS.TIER3;
  public readonly EffectorValues NEGATIVE_DECOR_EFFECT = TUNING.DECOR.PENALTY.TIER3;

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.ICEFLOWER.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.ICEFLOWER.DESC;
    EffectorValues positiveDecorEffect = this.POSITIVE_DECOR_EFFECT;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "potted_ice_flower_kanim");
    EffectorValues decor = positiveDecorEffect;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("IceFlower", name1, desc1, 1f, anim1, "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, decor, noise, defaultTemperature: 243.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 173.15f, 203.15f, 278.15f, 318.15f, new SimHashes[5]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide,
      SimHashes.ChlorineGas,
      SimHashes.Hydrogen
    }, can_tinker: false, baseTraitId: "IceFlowerOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.ICEFLOWER.NAME);
    PrickleGrass prickleGrass = placedEntity.AddOrGet<PrickleGrass>();
    placedEntity.AddOrGetDef<DecorPlantMonitor.Def>();
    prickleGrass.positive_decor_effect = this.POSITIVE_DECOR_EFFECT;
    prickleGrass.negative_decor_effect = this.NEGATIVE_DECOR_EFFECT;
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.ICEFLOWER.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.ICEFLOWER.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_ice_flower_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.DecorSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.ICEFLOWER.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Hidden, "IceFlowerSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 12, domesticatedDescription: domesticatedDescription, collisionShape: EntityTemplates.CollisionShape.RECTANGLE, width: 0.8f, height: 0.6f), "IceFlower_preview", Assets.GetAnim((HashedString) "potted_ice_flower_kanim"), "place", 1, 1);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
