// Decompiled with JetBrains decompiler
// Type: CritterTrapPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CritterTrapPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "CritterTrapPlant";
  public const float WATER_RATE = 0.0166666675f;
  public const float GAS_RATE = 0.0416666679f;
  public const float GAS_VENT_THRESHOLD = 33.25f;
  private static Tag[] AllowedPreyTags = new Tag[2]
  {
    GameTags.Creatures.Walker,
    GameTags.Creatures.Hoverer
  };

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "venus_critter_trap_kanim");
    EffectorValues decor = tieR1;
    float freezing3 = TUNING.CREATURES.TEMPERATURE.FREEZING_3;
    EffectorValues noise = new EffectorValues();
    double defaultTemperature = (double) freezing3;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("CritterTrapPlant", name1, desc1, 4f, anim1, "idle_open", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise, defaultTemperature: (float) defaultTemperature);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, TUNING.CREATURES.TEMPERATURE.FREEZING_10, TUNING.CREATURES.TEMPERATURE.FREEZING_9, TUNING.CREATURES.TEMPERATURE.FREEZING, TUNING.CREATURES.TEMPERATURE.COOL, pressure_sensitive: false, crop_id: "PlantMeat", should_grow_old: false, baseTraitId: "CritterTrapPlantOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.NAME);
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) placedEntity.GetComponent<MutantPlant>());
    CritterTrapPlant critterTrapPlant = placedEntity.AddOrGet<CritterTrapPlant>();
    critterTrapPlant.CONSUMABLE_TAGs = CritterTrapPlantConfig.AllowedPreyTags;
    critterTrapPlant.gasOutputRate = 0.0416666679f;
    critterTrapPlant.outputElement = SimHashes.Hydrogen;
    critterTrapPlant.gasVentThreshold = 33.25f;
    TrapTrigger trapTrigger = placedEntity.AddOrGet<TrapTrigger>();
    trapTrigger.trappableCreatures = CritterTrapPlantConfig.AllowedPreyTags;
    trapTrigger.trappedOffset = new Vector2(0.5f, 0.0f);
    trapTrigger.enabled = false;
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGet<Storage>();
    Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = tag,
        massConsumptionRate = 0.0166666675f
      }
    });
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.CRITTERTRAPPLANT.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.CRITTERTRAPPLANT.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_critter_trap_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.CRITTERTRAPPLANT.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Hidden, "CritterTrapPlantSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 21, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), "CritterTrapPlant_preview", Assets.GetAnim((HashedString) "venus_critter_trap_kanim"), "place", 1, 2);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    CritterTrapPlant component = inst.GetComponent<CritterTrapPlant>();
    inst.GetComponent<TrapTrigger>().customConditionsToTrap = new Func<GameObject, bool>(component.IsEntityEdible);
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
