// Decompiled with JetBrains decompiler
// Type: SeaTreeBranchConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SeaTreeBranchConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SeaTreeBranch";
  public const float GROWING_DURATION_CYCLES = 3f;
  public const float GROWING_DURATION = 1800f;
  public const float BULB_GROWING_DURATION = 1800f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.SEATREEBRANCH.NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.SEATREEBRANCH.DESC;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "sea_fairy_plant_kanim");
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
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("SeaTreeBranch", name1, desc, 1f, anim, "branch_idle", Grid.SceneLayer.BuildingFront, 1, 1, decor, noise, additionalTags: additionalTags, defaultTemperature: 302.65f);
    string str = "SeaTreeBranchOriginal";
    bool flag1 = false;
    GameObject template = placedEntity;
    bool flag2 = flag1;
    SimHashes[] allWaters = PLANTS.SAFE_ELEMENTS.AllWaters;
    int num = flag2 ? 1 : 0;
    string baseTraitId = str;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEATREEBRANCH.NAME;
    EntityTemplates.ExtendEntityToBasicPlant(template, 248.15f, 295.15f, 310.15f, safe_elements: allWaters, pressure_sensitive: false, can_drown: false, require_solid_tile: false, should_grow_old: num != 0, baseTraitId: baseTraitId, baseTraitName: name2);
    placedEntity.AddOrGet<PressureVulnerable>().allCellsMustBeSafe = true;
    placedEntity.AddOrGet<HarvestDesignatable>();
    placedEntity.AddOrGet<CodexEntryRedirector>().CodexID = "SeaTree";
    placedEntity.AddOrGet<UprootedMonitor>();
    Crop.CropVal cropval = TUNING.CROPS.CROP_TYPES.Find((Predicate<Crop.CropVal>) (m => m.cropId == "SeaFairy"));
    placedEntity.AddOrGet<Crop>().Configure(cropval);
    Modifiers component = placedEntity.GetComponent<Modifiers>();
    if ((UnityEngine.Object) placedEntity.GetComponent<Traits>() == (UnityEngine.Object) null)
    {
      placedEntity.AddOrGet<Traits>();
      component.initialTraits.Add(str);
    }
    component.initialAmounts.Add(Db.Get().Amounts.Maturity.Id);
    component.initialAmounts.Add(Db.Get().Amounts.Maturity2.Id);
    component.initialAttributes.Add(Db.Get().PlantAttributes.YieldAmount.Id);
    Trait trait = Db.Get().traits.Get(component.initialTraits[0]);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute.Id, 3f, (string) STRINGS.CREATURES.SPECIES.SEATREEBRANCH.NAME));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Maturity2.maxAttribute.Id, 3f, (string) STRINGS.CREATURES.SPECIES.SEATREEBRANCH.NAME));
    trait.Add(new AttributeModifier(Db.Get().PlantAttributes.YieldAmount.Id, (float) cropval.numProduced, (string) STRINGS.CREATURES.SPECIES.SEATREEBRANCH.NAME));
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, "SeaTreeBranch");
    SeaTreeBranch.Def def = placedEntity.AddOrGetDef<SeaTreeBranch.Def>();
    def.MAX_BRANCH_COUNT = 8;
    def.BRANCH_PREFAB_NAME = "SeaTreeBranch";
    placedEntity.AddOrGet<Harvestable>();
    placedEntity.AddOrGet<HarvestDesignatable>();
    WiltCondition wiltCondition = placedEntity.AddOrGet<WiltCondition>();
    wiltCondition.WiltDelay = 0.0f;
    wiltCondition.RecoveryDelay = 0.0f;
    SeedProducer seedProducer = placedEntity.AddOrGet<SeedProducer>();
    seedProducer.Configure("SeaTreeSeed", SeedProducer.ProductionType.HarvestOnly);
    seedProducer.seedDropChanceMultiplier = 0.125f;
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
