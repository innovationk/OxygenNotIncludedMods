// Decompiled with JetBrains decompiler
// Type: BatteryModuleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class BatteryModuleConfig : IBuildingConfig
{
  public const string ID = "BatteryModule";
  public const float NUM_CAPSULES = 3f;
  private static readonly CellOffset PLUG_OFFSET = new CellOffset(-1, 0);

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] hollowTieR2 = TUNING.BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER2;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("BatteryModule", 3, 2, "rocket_battery_pack_kanim", 1000, 30f, hollowTieR2, rawMetals, 9999f, BuildLocationRule.Anywhere, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.DefaultAnimState = "grounded";
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.ForegroundLayer = Grid.SceneLayer.Front;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.PowerInputOffset = BatteryModuleConfig.PLUG_OFFSET;
    buildingDef.PowerOutputOffset = BatteryModuleConfig.PLUG_OFFSET;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.RequiresPowerOutput = true;
    buildingDef.UseWhitePowerOutputConnectorColour = true;
    buildingDef.CanMove = true;
    buildingDef.Cancellable = false;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.BATTERY);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddComponent<RequireInputs>();
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 2), GameTags.Rocket, (AttachableBuilding) null)
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Prioritizable.AddRef(go);
    ModuleBattery moduleBattery = go.AddOrGet<ModuleBattery>();
    moduleBattery.capacity = 100000f;
    moduleBattery.joulesLostPerSecond = 0.6666667f;
    WireUtilitySemiVirtualNetworkLink virtualNetworkLink = go.AddOrGet<WireUtilitySemiVirtualNetworkLink>();
    virtualNetworkLink.link1 = BatteryModuleConfig.PLUG_OFFSET;
    virtualNetworkLink.visualizeOnly = true;
    BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, (string) null, TUNING.ROCKETRY.BURDEN.MINOR);
  }
}
