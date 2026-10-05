// Decompiled with JetBrains decompiler
// Type: UnderwaterVentDrillConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class UnderwaterVentDrillConfig : IBuildingConfig
{
  public const string ID = "UnderwaterVentDrill";
  public const float DRILL_DURATION = 100f;
  public const float DIAMOND_USAGE_PER_DRILL = 100f;
  public const float DIAMOND_STORAGE_CAPACITY = 200f;
  public const float DIAMOND_CONSUMPTION_RATE = 1f;
  public static readonly Vector3 PROGRESS_BAR_OFFSET = new Vector3(0.0f, 1.75f, 0.0f);

  public override string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("UnderwaterVentDrill", 4, 4, "underwater_vent_drill_kanim", 100, 120f, new float[2]
    {
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER4[0],
      1f
    }, new string[2]{ "RefinedMetal", "BuildingGasket" }, 1600f, BuildLocationRule.BuildingAttachPoint, DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER2);
    buildingDef.AttachmentSlotTag = GameTags.UnderwaterVentDrill;
    buildingDef.BuildLocationRule = BuildLocationRule.BuildingAttachPoint;
    buildingDef.ObjectLayer = ObjectLayer.AttachableBuilding;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingUse;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(0, 3);
    buildingDef.EnergyConsumptionWhenActive = 960f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    buildingDef.Floodable = false;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 3));
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    Tag tag = SimHashes.Diamond.CreateTag();
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    storage.storageFilters = new List<Tag>() { tag };
    storage.allowItemRemoval = false;
    storage.showInUI = true;
    storage.capacityKg = 200f;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = tag;
    manualDeliveryKg.capacity = 200f;
    manualDeliveryKg.refillMass = 80f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    UnderwaterVentDrill.Def def = go.AddOrGetDef<UnderwaterVentDrill.Def>();
    def.DiamondTag = tag;
    def.DiamondConsumptionRate = 1f;
    def.WorkDuration = 100f;
    def.ProgressBarOffset = UnderwaterVentDrillConfig.PROGRESS_BAR_OFFSET;
    go.AddOrGet<LoopingSounds>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
