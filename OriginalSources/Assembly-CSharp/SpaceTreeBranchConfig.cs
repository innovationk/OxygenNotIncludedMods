// Decompiled with JetBrains decompiler
// Type: SpaceTreeBranchConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpaceTreeBranchConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SpaceTreeBranch";
  public static string[] BRANCH_NAMES = new string[5]
  {
    "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_l\">",
    "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_tl\">",
    "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_t\">",
    "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_tr\">",
    "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_syrup_tree_r\">"
  };
  public const float GROWTH_DURATION = 2700f;
  public const int WOOD_AMOUNT = 75;
  private static Dictionary<CellOffset, string> entombDefenseAnimNames;
  private static Dictionary<CellOffset, SpaceTreeBranch.AnimSet> animationSets;
  private static Dictionary<CellOffset, Vector3> animOffset;

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.SPACETREE.NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.SPACETREE.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim = Assets.GetAnim((HashedString) "syrup_tree_kanim");
    EffectorValues decor = tieR1;
    List<Tag> tagList = new List<Tag>()
    {
      GameTags.HideFromSpawnTool,
      GameTags.HideFromCodex,
      GameTags.PlantBranch
    };
    EffectorValues noise = new EffectorValues();
    List<Tag> additionalTags = tagList;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("SpaceTreeBranch", name1, desc, 8f, anim, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 1, decor, noise, additionalTags: additionalTags, defaultTemperature: (float) byte.MaxValue);
    string baseTraitId = "SpaceTreeBranchOriginal";
    string name2 = (string) STRINGS.CREATURES.SPECIES.SPACETREE.NAME;
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 173.15f, 198.15f, 258.15f, 293.15f, pressure_sensitive: false, require_solid_tile: false, max_age: 12000f, max_radiation: 12200f, baseTraitId: baseTraitId, baseTraitName: name2);
    WiltCondition component1 = placedEntity.GetComponent<WiltCondition>();
    component1.WiltDelay = 0.0f;
    component1.RecoveryDelay = 0.0f;
    Modifiers component2 = placedEntity.GetComponent<Modifiers>();
    if ((UnityEngine.Object) placedEntity.GetComponent<Traits>() == (UnityEngine.Object) null)
    {
      placedEntity.AddOrGet<Traits>();
      component2.initialTraits.Add(baseTraitId);
    }
    KPrefabID component3 = placedEntity.GetComponent<KPrefabID>();
    Crop.CropVal cropval = new Crop.CropVal("WoodLog", 2700f, 75);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, component3.PrefabID().ToString());
    component2.initialAttributes.Add(Db.Get().PlantAttributes.YieldAmount.Id);
    component2.initialAmounts.Add(Db.Get().Amounts.Maturity.Id);
    Trait trait = Db.Get().traits.Get(component2.initialTraits[0]);
    trait.Add(new AttributeModifier(Db.Get().PlantAttributes.YieldAmount.Id, (float) cropval.numProduced, name2));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute.Id, cropval.cropDuration / 600f, name2));
    trait.Add(new AttributeModifier(Db.Get().PlantAttributes.MinLightLux.Id, 300f, (string) STRINGS.CREATURES.SPECIES.SPACETREE.NAME));
    component2.initialAttributes.Add(Db.Get().PlantAttributes.MinLightLux.Id);
    placedEntity.AddOrGet<IlluminationVulnerable>().SetPrefersDarkness();
    if (DlcManager.FeaturePlantMutationsEnabled())
    {
      placedEntity.AddOrGet<MutantPlant>().SpeciesID = component3.PrefabTag;
      SymbolOverrideControllerUtil.AddToPrefab(placedEntity);
    }
    placedEntity.AddOrGet<Crop>().Configure(cropval);
    placedEntity.AddOrGet<Harvestable>();
    placedEntity.AddOrGet<HarvestDesignatable>();
    placedEntity.UpdateComponentRequirement<Uprootable>(false);
    placedEntity.AddOrGet<CodexEntryRedirector>().CodexID = "SpaceTree";
    placedEntity.AddOrGetDef<PlantBranch.Def>().animationSetupCallback = new Action<PlantBranchGrower.Instance, PlantBranch.Instance>(this.AdjustAnimation);
    placedEntity.AddOrGetDef<SpaceTreeBranch.Def>().OPTIMAL_LUX_LEVELS = 10000;
    placedEntity.AddOrGetDef<UnstableEntombDefense.Def>().Cooldown = 5f;
    placedEntity.AddOrGet<BudUprootedMonitor>().destroyOnParentLost = true;
    return placedEntity;
  }

  public void AdjustAnimation(PlantBranchGrower.Instance trunk, PlantBranch.Instance branch)
  {
    CellOffset offset = Grid.GetOffset(Grid.PosToCell((StateMachine.Instance) trunk), Grid.PosToCell((StateMachine.Instance) branch));
    SpaceTreeBranch.Instance smi = branch.GetSMI<SpaceTreeBranch.Instance>();
    KBatchedAnimController component = branch.GetComponent<KBatchedAnimController>();
    if (smi != null && (UnityEngine.Object) component != (UnityEngine.Object) null && SpaceTreeBranchConfig.animationSets.ContainsKey(offset))
    {
      SpaceTreeBranch.AnimSet animationSet = SpaceTreeBranchConfig.animationSets[offset];
      smi.Animations = animationSet;
      component.Offset = SpaceTreeBranchConfig.animOffset[offset];
      smi.RefreshAnimation();
      branch.GetSMI<UnstableEntombDefense.Instance>().UnentombAnimName = SpaceTreeBranchConfig.entombDefenseAnimNames[offset];
    }
    else
      Debug.LogWarning((object) $"Error on AdjustAnimation().SpaceTreeBranchConfig.cs, spaceBranchFound: {(smi != null).ToString()}, animControllerFound: {((UnityEngine.Object) component != (UnityEngine.Object) null).ToString()}, animationSetFound: {SpaceTreeBranchConfig.animationSets.ContainsKey(offset).ToString()}");
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.AddOrGet<Harvestable>().readyForHarvestStatusItem = Db.Get().CreatureStatusItems.ReadyForHarvest_Branch;
    inst.AddOrGet<HarvestDesignatable>().iconOffset = new Vector2(0.0f, Grid.CellSizeInMeters * 0.5f);
  }

  public void OnSpawn(GameObject inst)
  {
  }

  static SpaceTreeBranchConfig()
  {
    Dictionary<CellOffset, string> dictionary1 = new Dictionary<CellOffset, string>();
    CellOffset key1 = new CellOffset(-1, 1);
    dictionary1[key1] = "shake_branch_b";
    CellOffset key2 = new CellOffset(-1, 2);
    dictionary1[key2] = "shake_branch_c";
    CellOffset key3 = new CellOffset(0, 2);
    dictionary1[key3] = "shake_branch_d";
    CellOffset key4 = new CellOffset(1, 2);
    dictionary1[key4] = "shake_branch_e";
    CellOffset key5 = new CellOffset(1, 1);
    dictionary1[key5] = "shake_branch_f";
    SpaceTreeBranchConfig.entombDefenseAnimNames = dictionary1;
    Dictionary<CellOffset, SpaceTreeBranch.AnimSet> dictionary2 = new Dictionary<CellOffset, SpaceTreeBranch.AnimSet>();
    key5 = new CellOffset(-1, 1);
    dictionary2[key5] = new SpaceTreeBranch.AnimSet()
    {
      spawn = "branch_b_grow",
      undeveloped = "grow_b_healthy_short",
      spawn_pst = "branch_b_grow_pst",
      ready_harvest = "harvest_ready_branch_b",
      fill = "grow_fill_branch_b",
      wilted = "branch_b_wilt",
      wilted_short_trunk_healthy = "grow_b_wilt_short",
      wilted_short_trunk_wilted = "branch_b_wilt_short",
      hidden = "branch_b_hidden",
      manual_harvest_pre = "syrup_harvest_branch_b_pre",
      manual_harvest_loop = "syrup_harvest_branch_b_loop",
      manual_harvest_pst = "syrup_harvest_branch_b_pst",
      meterAnim_flowerWilted = new string[1]
      {
        "leaves_b_wilt"
      },
      die = "branch_b_harvest",
      meterTargets = new string[1]{ "leaves_b_target" },
      meterAnimNames = new string[1]{ "leaves_b_meter" }
    };
    key4 = new CellOffset(-1, 2);
    dictionary2[key4] = new SpaceTreeBranch.AnimSet()
    {
      spawn = "branch_c_grow",
      undeveloped = "grow_c_healthy_short",
      spawn_pst = "branch_c_grow_pst",
      ready_harvest = "harvest_ready_branch_c",
      fill = "grow_fill_branch_c",
      wilted = "branch_c_wilt",
      wilted_short_trunk_healthy = "grow_c_wilt_short",
      wilted_short_trunk_wilted = "branch_c_wilt_short",
      hidden = "branch_c_hidden",
      manual_harvest_pre = "syrup_harvest_branch_c_pre",
      manual_harvest_loop = "syrup_harvest_branch_c_loop",
      manual_harvest_pst = "syrup_harvest_branch_c_pst",
      meterAnim_flowerWilted = new string[1]
      {
        "leaves_c_wilt"
      },
      die = "branch_c_harvest",
      meterTargets = new string[1]{ "leaves_c_target" },
      meterAnimNames = new string[1]{ "leaves_c_meter" }
    };
    key3 = new CellOffset(0, 2);
    dictionary2[key3] = new SpaceTreeBranch.AnimSet()
    {
      spawn = "branch_d_grow",
      undeveloped = "grow_d_healthy_short",
      spawn_pst = "branch_d_grow_pst",
      ready_harvest = "harvest_ready_branch_d",
      fill = "grow_fill_branch_d",
      wilted = "branch_d_wilt",
      wilted_short_trunk_healthy = "grow_d_wilt_short",
      wilted_short_trunk_wilted = "branch_d_wilt_short",
      hidden = "branch_d_hidden",
      manual_harvest_pre = "syrup_harvest_branch_d_pre",
      manual_harvest_loop = "syrup_harvest_branch_d_loop",
      manual_harvest_pst = "syrup_harvest_branch_d_pst",
      meterAnim_flowerWilted = new string[1]
      {
        "leaves_d_wilt"
      },
      die = "branch_d_harvest",
      meterTargets = new string[1]{ "leaves_d_target" },
      meterAnimNames = new string[1]{ "leaves_d_meter" }
    };
    key2 = new CellOffset(1, 2);
    dictionary2[key2] = new SpaceTreeBranch.AnimSet()
    {
      spawn = "branch_e_grow",
      undeveloped = "grow_e_healthy_short",
      spawn_pst = "branch_e_grow_pst",
      ready_harvest = "harvest_ready_branch_e",
      fill = "grow_fill_branch_e",
      wilted = "branch_e_wilt",
      wilted_short_trunk_healthy = "grow_e_wilt_short",
      wilted_short_trunk_wilted = "branch_e_wilt_short",
      hidden = "branch_e_hidden",
      manual_harvest_pre = "syrup_harvest_branch_e_pre",
      manual_harvest_loop = "syrup_harvest_branch_e_loop",
      manual_harvest_pst = "syrup_harvest_branch_e_pst",
      meterAnim_flowerWilted = new string[1]
      {
        "leaves_e_wilt"
      },
      die = "branch_e_harvest",
      meterTargets = new string[1]{ "leaves_e_target" },
      meterAnimNames = new string[1]{ "leaves_e_meter" }
    };
    key1 = new CellOffset(1, 1);
    dictionary2[key1] = new SpaceTreeBranch.AnimSet()
    {
      spawn = "branch_f_grow",
      undeveloped = "grow_f_healthy_short",
      spawn_pst = "branch_f_grow_pst",
      ready_harvest = "harvest_ready_branch_f",
      fill = "grow_fill_branch_f",
      wilted = "branch_f_wilt",
      wilted_short_trunk_healthy = "grow_f_wilt_short",
      wilted_short_trunk_wilted = "branch_f_wilt_short",
      hidden = "branch_f_hidden",
      manual_harvest_pre = "syrup_harvest_branch_f_pre",
      manual_harvest_loop = "syrup_harvest_branch_f_loop",
      manual_harvest_pst = "syrup_harvest_branch_f_pst",
      meterAnim_flowerWilted = new string[2]
      {
        "leaves_f1_wilt",
        "leaves_f2_wilt"
      },
      die = "branch_f_harvest",
      meterTargets = new string[2]
      {
        "leaves_f1_target",
        "leaves_f2_target"
      },
      meterAnimNames = new string[2]
      {
        "leaves_f1_meter",
        "leaves_f2_meter"
      }
    };
    SpaceTreeBranchConfig.animationSets = dictionary2;
    Dictionary<CellOffset, Vector3> dictionary3 = new Dictionary<CellOffset, Vector3>();
    key1 = new CellOffset(-1, 1);
    dictionary3[key1] = new Vector3(1f, -1f, 0.0f);
    key2 = new CellOffset(-1, 2);
    dictionary3[key2] = new Vector3(1f, -2f, 0.0f);
    key3 = new CellOffset(0, 2);
    dictionary3[key3] = new Vector3(0.0f, -2f, 0.0f);
    key4 = new CellOffset(1, 2);
    dictionary3[key4] = new Vector3(-1f, -2f, 0.0f);
    key5 = new CellOffset(1, 1);
    dictionary3[key5] = new Vector3(-1f, -1f, 0.0f);
    SpaceTreeBranchConfig.animOffset = dictionary3;
  }
}
