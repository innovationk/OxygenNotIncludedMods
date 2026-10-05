// Decompiled with JetBrains decompiler
// Type: RoboPilotCommandModuleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class RoboPilotCommandModuleConfig : IBuildingConfig
{
  public const string ID = "RoboPilotCommandModule";
  public static float DATABANKCONSUMPTION = 2f;
  public static float DATABANKRANGE = 10000f / RoboPilotCommandModuleConfig.DATABANKCONSUMPTION;
  private const string TRIGGER_LAUNCH_PORT_ID = "TriggerLaunch";
  private const string LAUNCH_READY_PORT_ID = "LaunchReady";

  public override string[] GetRequiredDlcIds() => DlcManager.DLC3;

  public override string[] GetForbiddenDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] commandModuleMass = TUNING.BUILDINGS.ROCKETRY_MASS_KG.COMMAND_MODULE_MASS;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("RoboPilotCommandModule", 5, 5, "robo_command_capsule_kanim", 1000, 60f, commandModuleMass, construction_materials, 9999f, BuildLocationRule.BuildingAttachPoint, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.RequiresPowerInput = false;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.CanMove = true;
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort((HashedString) "TriggerLaunch", new CellOffset(0, 1), (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_LAUNCH, (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_LAUNCH_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_LAUNCH_INACTIVE)
    };
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort((HashedString) "LaunchReady", new CellOffset(0, 2), (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_READY, (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_READY_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.COMMANDMODULE.LOGIC_PORT_READY_INACTIVE)
    };
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.ROBOT);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    LaunchConditionManager conditionManager = go.AddOrGet<LaunchConditionManager>();
    conditionManager.triggerPort = (HashedString) "TriggerLaunch";
    conditionManager.statusPort = (HashedString) "LaunchReady";
    RoboPilotModule roboPilotModule = go.AddOrGet<RoboPilotModule>();
    roboPilotModule.consumeDataBanksOnLand = true;
    roboPilotModule.dataBankConsumption = 1;
    Storage storage = go.AddComponent<Storage>();
    storage.showInUI = true;
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    storage.capacityKg = 100f;
    ManualDeliveryKG manualDeliveryKg = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    manualDeliveryKg.capacity = storage.capacityKg;
    manualDeliveryKg.refillMass = 20f;
    manualDeliveryKg.RequestedItemTag = DatabankHelper.TAG;
    manualDeliveryKg.MinimumMass = 1f;
    go.AddOrGet<CommandModule>().robotPilotControlled = true;
    go.AddOrGet<RobotCommandConditions>();
    go.AddOrGet<LaunchableRocket>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_command_module_bg_kanim");
  }
}
