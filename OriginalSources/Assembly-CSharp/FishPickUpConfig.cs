// Decompiled with JetBrains decompiler
// Type: FishPickUpConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class FishPickUpConfig : IBuildingConfig
{
  public const string ID = "FishPickUp";
  public const string INPUT_PORT = "FishPickUpInput";

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FishPickUp", 1, 3, "fishrelocator2_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, TUNING.MATERIALS.RAW_METALS, 1600f, BuildLocationRule.Anywhere, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0);
    buildingDef.AudioCategory = "Metal";
    buildingDef.Entombable = true;
    buildingDef.Floodable = false;
    buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
    buildingDef.ViewMode = OverlayModes.Rooms.ID;
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort((HashedString) "FishPickUpInput", new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.FISHPICKUP.LOGIC_INPUT.DESC, (string) STRINGS.BUILDINGS.PREFABS.FISHPICKUP.LOGIC_INPUT.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.FISHPICKUP.LOGIC_INPUT.LOGIC_PORT_INACTIVE)
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
    go.AddOrGet<TreeFilterable>();
    BaggableCritterCapacityTracker critterCapacityTracker = go.AddOrGet<BaggableCritterCapacityTracker>();
    critterCapacityTracker.maximumCreatures = 20;
    critterCapacityTracker.cavityOffset = CellOffset.down;
    critterCapacityTracker.requireLiquidOffset = true;
    BuildingPointStraw buildingPointStraw = go.AddOrGet<BuildingPointStraw>();
    buildingPointStraw.canControlAnimStates = false;
    buildingPointStraw.usesSymbols = false;
    Prioritizable.AddRef(go);
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
    FixedCapturePoint.Def def = go.AddOrGetDef<FixedCapturePoint.Def>();
    def.onAnimName = "on";
    def.offAnimName = "off";
    def.isAmountStoredOverCapacity = (Func<FixedCapturePoint.Instance, FixedCapturableMonitor.Instance, bool>) ((smi, capturable) =>
    {
      TreeFilterable component1 = smi.GetComponent<TreeFilterable>();
      IUserControlledCapacity component2 = smi.GetComponent<IUserControlledCapacity>();
      return (double) component2.AmountStored > (double) component2.UserMaxCapacity && component1.ContainsTag(capturable.PrefabTag);
    });
    def.allowBabies = true;
    def.captureCellOffset = new CellOffset(0, -1);
    def.rancherInteractOffset = new CellOffset(0, 1);
    def.postCaptureOffset = new CellOffset?(new CellOffset(0, 1));
    def.logicPortId = (HashedString) "FishPickUpInput";
    def.preCaptureAnimName = "working_pst";
    def.getPreCaptureAnimSuffix = (Func<FixedCapturePoint.Instance, string>) (smi =>
    {
      BuildingPointStraw component = smi.GetComponent<BuildingPointStraw>();
      return !((UnityEngine.Object) component != (UnityEngine.Object) null) ? "_1" : component.GetAnimSuffix();
    });
    def.getTargetCapturePoint = (Func<FixedCapturePoint.Instance, int>) (smi =>
    {
      int cell1 = Grid.PosToCell((StateMachine.Instance) smi);
      BuildingPointStraw component = smi.GetComponent<BuildingPointStraw>();
      int y = (UnityEngine.Object) component != (UnityEngine.Object) null ? component.GetDepthOffset() : -1;
      int cell2 = Grid.OffsetCell(cell1, 0, y);
      return Grid.IsValidCell(cell2) && smi.targetCapturable.Navigator.CanReach(cell2) ? cell2 : cell1;
    });
  }
}
