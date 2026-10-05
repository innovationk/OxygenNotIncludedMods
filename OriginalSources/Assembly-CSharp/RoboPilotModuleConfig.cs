// Decompiled with JetBrains decompiler
// Type: RoboPilotModuleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class RoboPilotModuleConfig : IBuildingConfig
{
  public const string ID = "RoboPilotModule";

  public override string[] GetRequiredDlcIds()
  {
    return new string[2]{ "EXPANSION1_ID", "DLC3_ID" };
  }

  public override BuildingDef CreateBuildingDef()
  {
    float[] hollowTieR1 = TUNING.BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("RoboPilotModule", 3, 4, "robot_rocket_control_station_kanim", 1000, 30f, hollowTieR1, construction_materials, 9999f, BuildLocationRule.Anywhere, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.DefaultAnimState = "grounded";
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.RequiresPowerInput = false;
    buildingDef.CanMove = true;
    buildingDef.Cancellable = false;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.ROBOT);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 4), GameTags.Rocket, (AttachableBuilding) null)
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Prioritizable.AddRef(go);
    go.GetComponent<KPrefabID>().AddTag(GameTags.LaunchButtonRocketModule);
    go.AddOrGet<RoboPilotModule>();
    go.AddOrGet<LaunchableRocketCluster>();
    go.AddOrGet<RobotCommandConditions>();
    go.AddOrGet<RocketProcessConditionDisplayTarget>();
    go.AddOrGet<RocketLaunchConditionVisualizer>();
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
    BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, (string) null, TUNING.ROCKETRY.BURDEN.MODERATE);
    go.GetComponent<ReorderableBuilding>().buildConditions.Add((SelectModuleCondition) new LimitOneRoboPilotModule());
  }
}
