// Decompiled with JetBrains decompiler
// Type: GroundTrapConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class GroundTrapConfig : IBuildingConfig
{
  public const string ID = "CreatureGroundTrap";
  public const string OUTPUT_LOGIC_PORT_ID = "TRAP_HAS_PREY_STATUS_PORT";
  private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CreatureGroundTrap", 2, 2, "critter_trap_ground_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, TUNING.MATERIALS.RAW_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0);
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT_INACTIVE)
    };
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort((HashedString) "TRAP_HAS_PREY_STATUS_PORT", new CellOffset(1, 0), (string) STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT_INACTIVE)
    };
    buildingDef.AudioCategory = "Metal";
    buildingDef.Floodable = false;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.RANCHING);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.CRITTER);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<Prioritizable>();
    Prioritizable.AddRef(go);
    go.AddOrGet<ArmTrapWorkable>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_critter_trap_ground_kanim")
    };
    go.AddOrGet<Operational>();
    Storage storage = go.AddOrGet<Storage>();
    storage.allowItemRemoval = true;
    storage.SetDefaultStoredItemModifiers(GroundTrapConfig.StoredItemModifiers);
    storage.sendOnStoreOnSpawn = true;
    TrapTrigger trapTrigger = go.AddOrGet<TrapTrigger>();
    trapTrigger.trappableCreatures = new Tag[3]
    {
      GameTags.Creatures.Walker,
      GameTags.Creatures.Hoverer,
      GameTags.Creatures.Swimmer
    };
    trapTrigger.trappedOffset = new Vector2(0.5f, 0.0f);
    go.AddOrGetDef<ReusableTrap.Def>().OUTPUT_LOGIC_PORT_ID = "TRAP_HAS_PREY_STATUS_PORT";
    go.AddOrGet<LogicPorts>();
    go.AddOrGet<LogicOperationalController>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
