// Decompiled with JetBrains decompiler
// Type: CritterPickUpConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class CritterPickUpConfig : IBuildingConfig
{
  public const string ID = "CritterPickUp";
  public const string INPUT_PORT = "CritterPickUpInput";

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CritterPickUp", 1, 3, "relocator_pickup_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, TUNING.MATERIALS.RAW_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0);
    buildingDef.AudioCategory = "Metal";
    buildingDef.ViewMode = OverlayModes.Rooms.ID;
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort((HashedString) "CritterPickUpInput", new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.CRITTERPICKUP.LOGIC_INPUT.DESC, (string) STRINGS.BUILDINGS.PREFABS.CRITTERPICKUP.LOGIC_INPUT.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.CRITTERPICKUP.LOGIC_INPUT.LOGIC_PORT_INACTIVE)
    };
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.CRITTER);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.RANCHING);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(GameTags.CodexCategories.CreatureRelocator);
    Storage storage = go.AddOrGet<Storage>();
    storage.allowItemRemoval = false;
    storage.showDescriptor = true;
    storage.storageFilters = STORAGEFILTERS.BAGABLE_CREATURES;
    storage.workAnims = new HashedString[2]
    {
      (HashedString) "place",
      (HashedString) "release"
    };
    storage.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_restrain_creature_kanim")
    };
    storage.workAnimPlayMode = KAnim.PlayMode.Once;
    storage.synchronizeAnims = false;
    storage.useGunForDelivery = false;
    storage.allowSettingOnlyFetchMarkedItems = false;
    go.AddOrGet<TreeFilterable>();
    go.AddOrGet<BaggableCritterCapacityTracker>().filteredCount = true;
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    FixedCapturePoint.Def def = go.AddOrGetDef<FixedCapturePoint.Def>();
    def.isAmountStoredOverCapacity = (Func<FixedCapturePoint.Instance, FixedCapturableMonitor.Instance, bool>) ((smi, capturable) =>
    {
      TreeFilterable component1 = smi.GetComponent<TreeFilterable>();
      IUserControlledCapacity component2 = smi.GetComponent<IUserControlledCapacity>();
      return (double) component2.AmountStored > (double) component2.UserMaxCapacity && component1.ContainsTag(capturable.PrefabTag);
    });
    def.allowBabies = true;
  }
}
