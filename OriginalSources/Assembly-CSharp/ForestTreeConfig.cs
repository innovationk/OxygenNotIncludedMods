// Decompiled with JetBrains decompiler
// Type: ForestTreeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ForestTreeConfig : IEntityConfig
{
  public const string ID = "ForestTree";
  public const string SEED_ID = "ForestTreeSeed";
  public const float FERTILIZATION_RATE = 0.0166666675f;
  public const float WATER_RATE = 0.116666667f;
  public const float BRANCH_GROWTH_TIME = 2100f;
  public const int NUM_BRANCHES = 7;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "tree_kanim");
    EffectorValues decor = tieR1;
    List<Tag> tagList = new List<Tag>();
    EffectorValues noise = new EffectorValues();
    List<Tag> additionalTags1 = tagList;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ForestTree", name1, desc1, 2f, anim1, "idle_empty", Grid.SceneLayer.Building, 1, 2, decor, noise, additionalTags: additionalTags1, defaultTemperature: 298.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 258.15f, 288.15f, 313.15f, 448.15f, crop_id: "WoodLog", should_grow_old: false, max_radiation: 9800f, baseTraitId: "ForestTreeOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME);
    PlantBranchGrower.Def def = placedEntity.AddOrGetDef<PlantBranchGrower.Def>();
    def.preventStartSMIOnSpawn = true;
    def.onBranchSpawned = new Action<PlantBranch.Instance, PlantBranchGrower.Instance>(this.RollChancesForSeed);
    def.onBranchHarvested = new Action<PlantBranch.Instance, PlantBranchGrower.Instance>(this.RollChancesForSeed);
    def.onEarlySpawn = new Action<PlantBranchGrower.Instance>(this.TranslateOldBranchesToNewSystem);
    def.BRANCH_PREFAB_NAME = "ForestTreeBranch";
    def.harvestOnDrown = true;
    def.MAX_BRANCH_COUNT = 5;
    def.BRANCH_OFFSETS = new CellOffset[7]
    {
      new CellOffset(-1, 0),
      new CellOffset(-1, 1),
      new CellOffset(-1, 2),
      new CellOffset(0, 2),
      new CellOffset(1, 2),
      new CellOffset(1, 1),
      new CellOffset(1, 0)
    };
    placedEntity.AddOrGet<BuddingTrunk>();
    placedEntity.AddOrGet<DirectlyEdiblePlant_TreeBranches>();
    placedEntity.UpdateComponentRequirement<Harvestable>(false);
    Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = tag,
        massConsumptionRate = 0.116666667f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Dirt,
        massConsumptionRate = 0.0166666675f
      }
    });
    placedEntity.AddComponent<StandardCropPlant>().wiltsOnReadyToHarvest = true;
    placedEntity.AddComponent<ForestTreeSeedMonitor>();
    GameObject plant = placedEntity;
    IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.WOOD_TREE.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.WOOD_TREE.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_tree_kanim");
    List<Tag> additionalTags2 = new List<Tag>();
    additionalTags2.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, SeedProducer.ProductionType.Hidden, "ForestTreeSeed", name2, desc2, anim2, additionalTags: additionalTags2, replantGroundTag: replantGroundTag, sortOrder: 4, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), "ForestTree_preview", Assets.GetAnim((HashedString) "tree_kanim"), "place", 3, 3);
    return placedEntity;
  }

  public void RollChancesForSeed(
    PlantBranch.Instance branch_smi,
    PlantBranchGrower.Instance trunk_smi)
  {
    trunk_smi.GetComponent<ForestTreeSeedMonitor>().TryRollNewSeed();
  }

  public void TranslateOldBranchesToNewSystem(PlantBranchGrower.Instance smi)
  {
    KPrefabID[] serializedBranches = smi.GetComponent<BuddingTrunk>().GetAndForgetOldSerializedBranches();
    if (serializedBranches == null)
      return;
    smi.ManuallyDefineBranchArray(serializedBranches);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
