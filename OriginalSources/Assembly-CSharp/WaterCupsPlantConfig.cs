// Decompiled with JetBrains decompiler
// Type: WaterCupsPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class WaterCupsPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "WaterCups";
  public static string SEED_ID = "WaterCupsSeed";
  public static string BASETRAIT_ID = "WaterCupsOriginal";
  public static string PREVIEW_ID = "WaterCupsPreview";
  public static EffectorValues POSITIVE_DECOR_EFFECT = TUNING.DECOR.BONUS.TIER3;
  public static EffectorValues NEGATIVE_DECOR_EFFECT = TUNING.DECOR.PENALTY.TIER3;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(WaterCupsPlantConfig.ID, (string) STRINGS.CREATURES.SPECIES.WATERCUPS.NAME, (string) STRINGS.CREATURES.SPECIES.WATERCUPS.DESC, 1f, Assets.GetAnim((HashedString) "watercups_kanim"), "idle", Grid.SceneLayer.BuildingFront, 1, 1, WaterCupsPlantConfig.POSITIVE_DECOR_EFFECT, NOISE_POLLUTION.NONE, defaultTemperature: 298.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 288.15f, 293.15f, 323.15f, 373.15f, new SimHashes[3]
    {
      SimHashes.Oxygen,
      SimHashes.ContaminatedOxygen,
      SimHashes.CarbonDioxide
    }, can_tinker: false, baseTraitId: WaterCupsPlantConfig.BASETRAIT_ID, baseTraitName: (string) STRINGS.CREATURES.SPECIES.WATERCUPS.NAME);
    PrickleGrass prickleGrass = placedEntity.AddOrGet<PrickleGrass>();
    prickleGrass.positive_decor_effect = WaterCupsPlantConfig.POSITIVE_DECOR_EFFECT;
    prickleGrass.negative_decor_effect = WaterCupsPlantConfig.NEGATIVE_DECOR_EFFECT;
    placedEntity.AddOrGetDef<DecorPlantMonitor.Def>();
    GameObject plant = placedEntity;
    string seedId = WaterCupsPlantConfig.SEED_ID;
    string name = (string) STRINGS.CREATURES.SPECIES.SEEDS.WATERCUPS.NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.SEEDS.WATERCUPS.DESC;
    KAnimFile anim = Assets.GetAnim((HashedString) "seed_watercups_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.DecorSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.WATERCUPS.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Hidden, seedId, name, desc, anim, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 12, domesticatedDescription: domesticatedDescription, width: 0.22f, height: 0.22f), WaterCupsPlantConfig.PREVIEW_ID, Assets.GetAnim((HashedString) "watercups_kanim"), "place", 1, 1);
    return placedEntity;
  }

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
