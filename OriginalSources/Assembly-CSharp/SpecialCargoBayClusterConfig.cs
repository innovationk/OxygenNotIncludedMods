// Decompiled with JetBrains decompiler
// Type: SpecialCargoBayClusterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SpecialCargoBayClusterConfig : IBuildingConfig
{
  public const string ID = "SpecialCargoBayCluster";
  private static readonly List<Storage.StoredItemModifier> StoredCrittersModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Insulate,
    Storage.StoredItemModifier.Hide
  };
  private static readonly List<Storage.StoredItemModifier> StoredLootModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Seal
  };

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] hollowTieR1 = TUNING.BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SpecialCargoBayCluster", 3, 1, "rocket_storage_live_small_kanim", 1000, 60f, hollowTieR1, refinedMetals, 9999f, BuildLocationRule.Anywhere, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.RequiresPowerInput = false;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.CanMove = true;
    buildingDef.Cancellable = false;
    buildingDef.ShowInBuildMenu = false;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.TRANSPORT);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 1), GameTags.Rocket, (AttachableBuilding) null)
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Storage storage1 = go.AddOrGet<Storage>();
    storage1.SetDefaultStoredItemModifiers(SpecialCargoBayClusterConfig.StoredCrittersModifiers);
    storage1.showCapacityStatusItem = false;
    storage1.showInUI = false;
    storage1.storageFilters = new List<Tag>()
    {
      GameTags.BagableCreature
    };
    storage1.allowSettingOnlyFetchMarkedItems = false;
    storage1.allowItemRemoval = false;
    Storage storage2 = go.AddComponent<Storage>();
    storage2.SetDefaultStoredItemModifiers(SpecialCargoBayClusterConfig.StoredLootModifiers);
    storage2.showCapacityStatusItem = false;
    storage2.showInUI = false;
    storage2.allowSettingOnlyFetchMarkedItems = false;
    storage2.allowItemRemoval = false;
    go.AddOrGet<CopyBuildingSettings>();
    go.AddOrGetDef<SpecialCargoBayCluster.Def>();
    SpecialCargoBayClusterReceptacle clusterReceptacle = go.AddOrGet<SpecialCargoBayClusterReceptacle>();
    clusterReceptacle.AddDepositTag(GameTags.BagableCreature);
    clusterReceptacle.AddAdditionalCriteria((Func<GameObject, bool>) (obj => obj.HasTag(GameTags.Creatures.Deliverable)));
    clusterReceptacle.sideProductStorage = storage2;
    BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, (string) null, TUNING.ROCKETRY.BURDEN.INSIGNIFICANT);
    SymbolOverrideControllerUtil.AddToPrefab(go);
    go.AddOrGet<Prioritizable>();
    Prioritizable.AddRef(go);
  }
}
