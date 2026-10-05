// Decompiled with JetBrains decompiler
// Type: MilkingStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TUNING;
using UnityEngine;

#nullable disable
public class MilkingStationConfig : IBuildingConfig
{
  public const string ID = "MilkingStation";

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]
    {
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER4[0],
      4f
    };
    string[] construction_materials = new string[2]
    {
      "RefinedMetal",
      "BuildingGasket"
    };
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues tieR2 = BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MilkingStation", 2, 4, "milking_station_kanim", 30, 60f, construction_mass, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.ViewMode = OverlayModes.Rooms.ID;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanUseMilkingStation.Id;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType);
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = Mathf.Max(MooTuning.MILK_AMOUNT_AT_MILKING, MooTuning.DIESEL_PER_CYCLE) * 2f;
    storage.showInUI = true;
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
    go.AddOrGet<SkillPerkMissingComplainer>().requiredSkillPerk = Db.Get().SkillPerks.CanUseMilkingStation.Id;
    RanchStation.Def def = go.AddOrGetDef<RanchStation.Def>();
    def.IsCritterEligibleToBeRanchedCb = (Func<GameObject, RanchStation.Instance, bool>) ((creature_go, ranch_station_smi) =>
    {
      IMilkable smi = creature_go.GetSMI<IMilkable>();
      return smi != null && smi.IsReadyToBeMilked();
    });
    def.RancherInteractAnim = (HashedString) "anim_interacts_milking_station_kanim";
    def.RanchedPreAnim = (HashedString) "mooshake_pre";
    def.RanchedLoopAnim = (HashedString) "mooshake_loop";
    def.RanchedPstAnim = (HashedString) "mooshake_pst";
    def.WorkTime = 20f;
    def.CreatureRanchingStatusItem = Db.Get().CreatureStatusItems.GettingMilked;
    def.RancherWipesBrowAnim = false;
    def.GetTargetRanchCell = (Func<RanchStation.Instance, int>) (smi =>
    {
      int num = Grid.InvalidCell;
      if (!smi.IsNullOrStopped())
        num = Grid.PosToCell(smi.transform.GetPosition());
      return num;
    });
    def.OnRanchCompleteCb = (Action<GameObject, WorkerBase>) ((creature_go, rancher_wb) =>
    {
      RanchStation.Instance targetRanchStation = creature_go.GetSMI<RanchableMonitor.Instance>().TargetRanchStation;
      creature_go.GetSMI<IMilkable>().MilkingComplete(targetRanchStation.GetComponent<Storage>());
    });
    def.OnRanchWorkBegins = (Action<RanchedStates.Instance, Workable>) ((creature, workable) =>
    {
      IMilkable smi = creature.gameObject.GetSMI<IMilkable>();
      if (smi == null)
        return;
      Color colour = (Color) ElementLoader.FindElementByHash(smi.GetMilkElement()).substance.colour with
      {
        a = 1f
      };
      workable.GetComponent<KBatchedAnimController>().SetSymbolTint(new KAnimHashedString("gushfx"), colour);
    });
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.alwaysDispense = true;
    conduitDispenser.elementFilter = (SimHashes[]) null;
  }
}
