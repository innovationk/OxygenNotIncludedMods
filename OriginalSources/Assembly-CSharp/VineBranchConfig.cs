// Decompiled with JetBrains decompiler
// Type: VineBranchConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class VineBranchConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "VineBranch";
  public const float GROWING_DURATION_CYCLES = 3f;
  public const float GROWING_DURATION = 1800f;
  public const float FRUIT_GROWING_DURATION = 1800f;
  public const int FRUIT_COUNT_PER_HARVEST = 1;
  public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 6f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.CREATURES.SPECIES.VINEBRANCH.NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.VINEBRANCH.DESC;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "vine_kanim");
    EffectorValues decor = tieR0;
    List<Tag> tagList = new List<Tag>()
    {
      GameTags.HideFromSpawnTool,
      GameTags.HideFromCodex,
      GameTags.PlantBranch,
      GameTags.ExcludeFromTemplate
    };
    EffectorValues noise = new EffectorValues();
    List<Tag> additionalTags = tagList;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("VineBranch", name, desc, 1f, anim, "line_idle", Grid.SceneLayer.BuildingFront, 1, 1, decor, noise, additionalTags: additionalTags, defaultTemperature: 308.15f);
    string baseTraitId = "VineBranchOriginal";
    bool should_grow_old = false;
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 273.15f, 298.15f, 318.15f, 378.15f, pressure_sensitive: false, require_solid_tile: false, should_grow_old: should_grow_old, baseTraitId: baseTraitId, baseTraitName: (string) STRINGS.CREATURES.SPECIES.VINEBRANCH.NAME);
    placedEntity.AddOrGet<HarvestDesignatable>();
    placedEntity.AddOrGet<CodexEntryRedirector>().CodexID = "VineMother";
    placedEntity.AddOrGet<PlantFiberProducer>().amount = 6f;
    placedEntity.AddOrGet<UprootedMonitor>();
    Crop.CropVal cropval = TUNING.CROPS.CROP_TYPES.Find((Predicate<Crop.CropVal>) (m => m.cropId == VineFruitConfig.ID));
    placedEntity.AddOrGet<Crop>().Configure(cropval);
    Modifiers component = placedEntity.GetComponent<Modifiers>();
    if ((UnityEngine.Object) placedEntity.GetComponent<Traits>() == (UnityEngine.Object) null)
    {
      placedEntity.AddOrGet<Traits>();
      component.initialTraits.Add(baseTraitId);
    }
    component.initialAmounts.Add(Db.Get().Amounts.Maturity.Id);
    component.initialAmounts.Add(Db.Get().Amounts.Maturity2.Id);
    component.initialAttributes.Add(Db.Get().PlantAttributes.YieldAmount.Id);
    Trait trait = Db.Get().traits.Get(component.initialTraits[0]);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute.Id, 3f, (string) STRINGS.CREATURES.SPECIES.VINEBRANCH.NAME));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Maturity2.maxAttribute.Id, 3f, (string) STRINGS.CREATURES.SPECIES.VINEBRANCH.NAME));
    trait.Add(new AttributeModifier(Db.Get().PlantAttributes.YieldAmount.Id, (float) cropval.numProduced, (string) STRINGS.CREATURES.SPECIES.VINEBRANCH.NAME));
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, "VineBranch");
    placedEntity.AddOrGetDef<VineBranch.Def>().BRANCH_PREFAB_NAME = "VineBranch";
    placedEntity.AddOrGet<Harvestable>();
    placedEntity.AddOrGet<HarvestDesignatable>();
    WiltCondition wiltCondition = placedEntity.AddOrGet<WiltCondition>();
    wiltCondition.WiltDelay = 0.0f;
    wiltCondition.RecoveryDelay = 0.0f;
    SeedProducer seedProducer = placedEntity.AddOrGet<SeedProducer>();
    seedProducer.Configure("VineMotherSeed", SeedProducer.ProductionType.HarvestOnly);
    seedProducer.seedDropChanceMultiplier = 0.166666672f;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.AddOrGet<UprootedMonitor>().monitorCells = new CellOffset[0];
    inst.AddOrGet<HarvestDesignatable>().iconOffset = new Vector2(0.0f, Grid.CellSizeInMeters * 0.75f);
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
