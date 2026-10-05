// Decompiled with JetBrains decompiler
// Type: ForestTreeBranchConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ForestTreeBranchConfig : IEntityConfig
{
  public const string ID = "ForestTreeBranch";
  public const float WOOD_AMOUNT = 300f;
  private static Dictionary<CellOffset, StandardCropPlant.AnimSet> animationSets;
  private static Dictionary<CellOffset, Vector3> animOffset;

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim = Assets.GetAnim((HashedString) "tree_kanim");
    EffectorValues decor = tieR1;
    List<Tag> tagList = new List<Tag>()
    {
      GameTags.HideFromSpawnTool,
      GameTags.HideFromCodex,
      GameTags.PlantBranch
    };
    EffectorValues noise = new EffectorValues();
    List<Tag> additionalTags = tagList;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ForestTreeBranch", name, desc, 8f, anim, "idle_empty", Grid.SceneLayer.BuildingFront, 1, 1, decor, noise, additionalTags: additionalTags, defaultTemperature: 298.15f);
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 258.15f, 288.15f, 313.15f, 448.15f, crop_id: "WoodLog", require_solid_tile: false, max_age: 12000f, max_radiation: 9800f, baseTraitId: "ForestTreeBranchOriginal", baseTraitName: (string) STRINGS.CREATURES.SPECIES.WOOD_TREE.NAME);
    placedEntity.AddOrGet<TreeBud>();
    placedEntity.AddOrGet<StandardCropPlant>();
    placedEntity.AddOrGet<BudUprootedMonitor>();
    placedEntity.AddOrGet<CodexEntryRedirector>().CodexID = "ForestTree";
    PlantBranch.Def def = placedEntity.AddOrGetDef<PlantBranch.Def>();
    def.preventStartSMIOnSpawn = true;
    def.onEarlySpawn = new Action<PlantBranch.Instance>(this.TranslateOldTrunkToNewSystem);
    def.animationSetupCallback = new Action<PlantBranchGrower.Instance, PlantBranch.Instance>(this.AdjustAnimation);
    return placedEntity;
  }

  public void AdjustAnimation(PlantBranchGrower.Instance trunk, PlantBranch.Instance branch)
  {
    CellOffset offset = Grid.GetOffset(Grid.PosToCell((StateMachine.Instance) trunk), Grid.PosToCell((StateMachine.Instance) branch));
    StandardCropPlant component1 = branch.GetComponent<StandardCropPlant>();
    KBatchedAnimController component2 = branch.GetComponent<KBatchedAnimController>();
    component1.anims = ForestTreeBranchConfig.animationSets[offset];
    component2.Offset = ForestTreeBranchConfig.animOffset[offset];
    component2.Play((HashedString) component1.anims.grow, KAnim.PlayMode.Paused);
    component1.RefreshPositionPercent();
  }

  public void TranslateOldTrunkToNewSystem(PlantBranch.Instance smi)
  {
    BuddingTrunk andForgetOldTrunk = smi.GetComponent<TreeBud>().GetAndForgetOldTrunk();
    if (!((UnityEngine.Object) andForgetOldTrunk != (UnityEngine.Object) null))
      return;
    PlantBranchGrower.Instance smi1 = andForgetOldTrunk.GetSMI<PlantBranchGrower.Instance>();
    smi.SetTrunk(smi1);
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.AddOrGet<Harvestable>().readyForHarvestStatusItem = Db.Get().CreatureStatusItems.ReadyForHarvest_Branch;
  }

  public void OnSpawn(GameObject inst)
  {
  }

  static ForestTreeBranchConfig()
  {
    Dictionary<CellOffset, StandardCropPlant.AnimSet> dictionary1 = new Dictionary<CellOffset, StandardCropPlant.AnimSet>();
    CellOffset key1 = new CellOffset(-1, 0);
    dictionary1[key1] = new StandardCropPlant.AnimSet()
    {
      grow = "branch_a_grow",
      grow_pst = "branch_a_grow_pst",
      idle_full = "branch_a_idle_full",
      wilt_base = "branch_a_wilt",
      harvest = "branch_a_harvest"
    };
    CellOffset key2 = new CellOffset(-1, 1);
    dictionary1[key2] = new StandardCropPlant.AnimSet()
    {
      grow = "branch_b_grow",
      grow_pst = "branch_b_grow_pst",
      idle_full = "branch_b_idle_full",
      wilt_base = "branch_b_wilt",
      harvest = "branch_b_harvest"
    };
    CellOffset key3 = new CellOffset(-1, 2);
    dictionary1[key3] = new StandardCropPlant.AnimSet()
    {
      grow = "branch_c_grow",
      grow_pst = "branch_c_grow_pst",
      idle_full = "branch_c_idle_full",
      wilt_base = "branch_c_wilt",
      harvest = "branch_c_harvest"
    };
    CellOffset key4 = new CellOffset(0, 2);
    dictionary1[key4] = new StandardCropPlant.AnimSet()
    {
      grow = "branch_d_grow",
      grow_pst = "branch_d_grow_pst",
      idle_full = "branch_d_idle_full",
      wilt_base = "branch_d_wilt",
      harvest = "branch_d_harvest"
    };
    CellOffset key5 = new CellOffset(1, 2);
    dictionary1[key5] = new StandardCropPlant.AnimSet()
    {
      grow = "branch_e_grow",
      grow_pst = "branch_e_grow_pst",
      idle_full = "branch_e_idle_full",
      wilt_base = "branch_e_wilt",
      harvest = "branch_e_harvest"
    };
    CellOffset key6 = new CellOffset(1, 1);
    dictionary1[key6] = new StandardCropPlant.AnimSet()
    {
      grow = "branch_f_grow",
      grow_pst = "branch_f_grow_pst",
      idle_full = "branch_f_idle_full",
      wilt_base = "branch_f_wilt",
      harvest = "branch_f_harvest"
    };
    CellOffset key7 = new CellOffset(1, 0);
    dictionary1[key7] = new StandardCropPlant.AnimSet()
    {
      grow = "branch_g_grow",
      grow_pst = "branch_g_grow_pst",
      idle_full = "branch_g_idle_full",
      wilt_base = "branch_g_wilt",
      harvest = "branch_g_harvest"
    };
    ForestTreeBranchConfig.animationSets = dictionary1;
    Dictionary<CellOffset, Vector3> dictionary2 = new Dictionary<CellOffset, Vector3>();
    key7 = new CellOffset(-1, 0);
    dictionary2[key7] = new Vector3(1f, 0.0f, 0.0f);
    key6 = new CellOffset(-1, 1);
    dictionary2[key6] = new Vector3(1f, -1f, 0.0f);
    key5 = new CellOffset(-1, 2);
    dictionary2[key5] = new Vector3(1f, -2f, 0.0f);
    key4 = new CellOffset(0, 2);
    dictionary2[key4] = new Vector3(0.0f, -2f, 0.0f);
    key3 = new CellOffset(1, 2);
    dictionary2[key3] = new Vector3(-1f, -2f, 0.0f);
    key2 = new CellOffset(1, 1);
    dictionary2[key2] = new Vector3(-1f, -1f, 0.0f);
    key1 = new CellOffset(1, 0);
    dictionary2[key1] = new Vector3(-1f, 0.0f, 0.0f);
    ForestTreeBranchConfig.animOffset = dictionary2;
  }
}
