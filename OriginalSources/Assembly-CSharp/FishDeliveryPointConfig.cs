// Decompiled with JetBrains decompiler
// Type: FishDeliveryPointConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class FishDeliveryPointConfig : IBuildingConfig
{
  public const string ID = "FishDeliveryPoint";
  public const int STRAW_LENGTH = 4;
  public const string INPUT_PORT = "FishDeliveryPointInput";

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FishDeliveryPoint", 1, 3, "fishrelocator_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, TUNING.MATERIALS.RAW_METALS, 1600f, BuildLocationRule.Anywhere, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0);
    buildingDef.AudioCategory = "Metal";
    buildingDef.Entombable = true;
    buildingDef.Floodable = false;
    buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
    buildingDef.ViewMode = OverlayModes.Rooms.ID;
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort((HashedString) "CritterDropOffInput", new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.FISHDELIVERYPOINT.LOGIC_INPUT.DESC, (string) STRINGS.BUILDINGS.PREFABS.FISHDELIVERYPOINT.LOGIC_INPUT.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.FISHDELIVERYPOINT.LOGIC_INPUT.LOGIC_PORT_INACTIVE)
    };
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.RANCHING);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.CRITTER);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(GameTags.CodexCategories.CreatureRelocator);
    Storage storage = go.AddOrGet<Storage>();
    storage.allowItemRemoval = false;
    storage.showDescriptor = true;
    storage.storageFilters = STORAGEFILTERS.SWIMMING_CREATURES;
    storage.workAnims = new HashedString[1]
    {
      new HashedString("working_pre")
    };
    storage.workAnimPlayMode = KAnim.PlayMode.Once;
    storage.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_fishrelocator_kanim")
    };
    storage.synchronizeAnims = false;
    storage.useGunForDelivery = false;
    storage.allowSettingOnlyFetchMarkedItems = false;
    storage.faceTargetWhenWorking = false;
    CreatureDeliveryPoint creatureDeliveryPoint = go.AddOrGet<CreatureDeliveryPoint>();
    creatureDeliveryPoint.deliveryOffsets = new CellOffset[1]
    {
      new CellOffset(0, 1)
    };
    creatureDeliveryPoint.spawnOffset = new CellOffset(0, -1);
    creatureDeliveryPoint.largeCritterSpawnOffset = new CellOffset(0, -2);
    creatureDeliveryPoint.playAnimsOnFetch = true;
    BaggableCritterCapacityTracker critterCapacityTracker = go.AddOrGet<BaggableCritterCapacityTracker>();
    critterCapacityTracker.maximumCreatures = 20;
    critterCapacityTracker.cavityOffset = CellOffset.down;
    critterCapacityTracker.requireLiquidOffset = true;
    go.AddOrGet<TreeFilterable>();
    BuildingPointStraw buildingPointStraw = go.AddOrGet<BuildingPointStraw>();
    buildingPointStraw.canControlAnimStates = false;
    buildingPointStraw.usesSymbols = false;
    buildingPointStraw.maxDepth = 4;
    component.prefabInitFn += new KPrefabID.PrefabFn(this.OnPrefabInit);
  }

  private void OnPrefabInit(GameObject instance)
  {
    foreach (KBatchedAnimController kbatchedAnimController in instance.GetComponentsInChildrenOnly<KBatchedAnimController>())
    {
      kbatchedAnimController.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.LiquidVisibilityLayer, false);
      kbatchedAnimController.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.WaterProof, true);
    }
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<MakeBaseSolid.Def>().solidOffsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
  }
}
