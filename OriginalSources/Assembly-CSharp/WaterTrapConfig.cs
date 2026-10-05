// Decompiled with JetBrains decompiler
// Type: WaterTrapConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class WaterTrapConfig : IBuildingConfig
{
  public const string ID = "WaterTrap";
  public const string OUTPUT_LOGIC_PORT_ID = "TRAP_HAS_PREY_STATUS_PORT";
  public const int TRAIL_LENGTH = 4;
  private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("WaterTrap", 1, 2, "critter_trap_water_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, TUNING.MATERIALS.RAW_METALS, 1600f, BuildLocationRule.Anywhere, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0);
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT_INACTIVE)
    };
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort((HashedString) "TRAP_HAS_PREY_STATUS_PORT", new CellOffset(0, 1), (string) STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT_INACTIVE)
    };
    buildingDef.AudioCategory = "Metal";
    buildingDef.Floodable = false;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.RANCHING);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.CRITTER);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<Prioritizable>();
    Prioritizable.AddRef(go);
    ArmTrapWorkable armTrapWorkable = go.AddOrGet<ArmTrapWorkable>();
    armTrapWorkable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_critter_trap_water_kanim")
    };
    armTrapWorkable.initialOffsets = new CellOffset[1]
    {
      new CellOffset(0, 1)
    };
    go.AddOrGet<Operational>();
    RangeVisualizer rangeVisualizer = go.AddOrGet<RangeVisualizer>();
    rangeVisualizer.OriginOffset = new Vector2I(0, 0);
    rangeVisualizer.BlockingTileVisible = false;
    Storage storage = go.AddOrGet<Storage>();
    storage.allowItemRemoval = true;
    storage.SetDefaultStoredItemModifiers(WaterTrapConfig.StoredItemModifiers);
    storage.sendOnStoreOnSpawn = true;
    TrapTrigger trapTrigger = go.AddOrGet<TrapTrigger>();
    trapTrigger.trappableCreatures = new Tag[1]
    {
      GameTags.Creatures.Swimmer
    };
    trapTrigger.trappedOffset = new Vector2(0.0f, 1f);
    go.AddOrGetDef<WaterTrapTrail.Def>();
    ReusableTrap.Def def = go.AddOrGetDef<ReusableTrap.Def>();
    def.releaseCellOffset = new CellOffset(0, 1);
    def.OUTPUT_LOGIC_PORT_ID = "TRAP_HAS_PREY_STATUS_PORT";
    def.lures = new Tag[1]
    {
      GameTags.Creatures.FishTrapLure
    };
    def.usingSymbolChaseCapturingAnimations = true;
    def.getTrappedAnimationNameCallback = (Func<string>) (() => "trapped");
    go.AddOrGet<LogicPorts>();
    go.AddOrGet<LogicOperationalController>();
  }

  private static void AddGuide(GameObject go, bool occupy_tiles)
  {
    GameObject gameObject = new GameObject();
    gameObject.transform.parent = go.transform;
    gameObject.transform.SetLocalPosition(Vector3.zero);
    KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
    kbatchedAnimController.Offset = go.GetComponent<Building>().Def.GetVisualizerOffset();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim(new HashedString("critter_trap_water_kanim"))
    };
    kbatchedAnimController.initialAnim = "place_guide";
    kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
    kbatchedAnimController.isMovable = true;
    WaterTrapGuide waterTrapGuide = gameObject.AddComponent<WaterTrapGuide>();
    waterTrapGuide.parent = go;
    waterTrapGuide.occupyTiles = occupy_tiles;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    WaterTrapConfig.AddGuide(go.GetComponent<Building>().Def.BuildingPreview, false);
    WaterTrapConfig.AddGuide(go.GetComponent<Building>().Def.BuildingUnderConstruction, false);
    Lure.Def def = go.AddOrGetDef<Lure.Def>();
    def.defaultLurePoints = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    def.radius = 32 /*0x20*/;
    go.AddOrGet<FakeFloorAdder>().floorOffsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
  }
}
