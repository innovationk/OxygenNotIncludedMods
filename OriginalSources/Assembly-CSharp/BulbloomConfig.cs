// Decompiled with JetBrains decompiler
// Type: BulbloomConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class BulbloomConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Bulbloom";
  public const string SEED_ID = "BulbloomSeed";
  public static readonly EffectorValues POSITIVE_DECOR_EFFECT = TUNING.DECOR.BONUS.TIER3;
  public static readonly EffectorValues NEGATIVE_DECOR_EFFECT = TUNING.DECOR.PENALTY.TIER3;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.BULBLOOM.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.BULBLOOM.DESC;
    EffectorValues positiveDecorEffect = BulbloomConfig.POSITIVE_DECOR_EFFECT;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "bulbloom_kanim");
    EffectorValues decor = positiveDecorEffect;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("Bulbloom", name1, desc1, 1f, anim1, "grow_seed", Grid.SceneLayer.BuildingFront, 1, 1, decor, noise, defaultTemperature: 333.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 293.15f, 313.15f, 353.15f, 383.15f, PLANTS.SAFE_ELEMENTS.MurkyWaters, false, can_drown: false, can_tinker: false, baseTraitId: "BulbloomOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.BULBLOOM.NAME);
    PrickleGrass prickleGrass = placedEntity.AddOrGet<PrickleGrass>();
    placedEntity.AddOrGetDef<DecorPlantMonitor.Def>();
    prickleGrass.positive_decor_effect = BulbloomConfig.POSITIVE_DECOR_EFFECT;
    prickleGrass.negative_decor_effect = BulbloomConfig.NEGATIVE_DECOR_EFFECT;
    Light2D light2D = placedEntity.AddOrGet<Light2D>();
    light2D.Color = new Color(0.4f, 0.5f, 1f, 0.15f);
    light2D.overlayColour = LIGHT2D.LIGHT_OVERLAY;
    light2D.Range = 2f;
    light2D.Direction = LIGHT2D.DEFAULT_DIRECTION;
    light2D.Offset = new Vector2(0.05f, 0.5f);
    light2D.shape = LightShape.Circle;
    light2D.drawOverlay = true;
    light2D.Lux = DUPLICANTSTATS.STANDARD.Light.LOW_LIGHT;
    placedEntity.AddComponent<PlantGlowController>();
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BULBLOOM.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.BULBLOOM.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_bulbloom_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.DecorSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.BULBLOOM.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(placedEntity, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Hidden, "BulbloomSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 13, domesticatedDescription: domesticatedDescription), "Bulbloom_preview", Assets.GetAnim((HashedString) "bulbloom_kanim"), "place", 1, 1);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
