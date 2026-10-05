// Decompiled with JetBrains decompiler
// Type: LiquidLimitValveConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class LiquidLimitValveConfig : IBuildingConfig
{
  public const string ID = "LiquidLimitValve";
  private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]
    {
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
      1f
    };
    string[] construction_materials = new string[2]
    {
      "RefinedMetal",
      "BuildingGasket"
    };
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues tieR0 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LiquidLimitValve", 1, 2, "limit_valve_liquid_kanim", 30, 10f, construction_mass, construction_materials, 1600f, BuildLocationRule.Anywhere, tieR0, noise);
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.Floodable = false;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 10f;
    buildingDef.PowerInputOffset = new CellOffset(0, 1);
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      new LogicPorts.Port(LimitValve.RESET_PORT_ID, new CellOffset(0, 1), (string) STRINGS.BUILDINGS.PREFABS.LIQUIDLIMITVALVE.LOGIC_PORT_RESET, (string) STRINGS.BUILDINGS.PREFABS.LIQUIDLIMITVALVE.RESET_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LIQUIDLIMITVALVE.RESET_PORT_INACTIVE, false, LogicPortSpriteType.ResetUpdate, true)
    };
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort(LimitValve.OUTPUT_PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LIQUIDLIMITVALVE.LOGIC_PORT_OUTPUT, (string) STRINGS.BUILDINGS.PREFABS.LIQUIDLIMITVALVE.OUTPUT_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LIQUIDLIMITVALVE.OUTPUT_PORT_INACTIVE)
    };
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidLimitValve");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGetDef<PoweredActiveTransitionController.Def>();
    Object.DestroyImmediate((Object) go.GetComponent<BuildingEnabledButton>());
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<ConduitBridge>().type = ConduitType.Liquid;
    LimitValve limitValve = go.AddOrGet<LimitValve>();
    limitValve.conduitType = ConduitType.Liquid;
    limitValve.maxLimitKg = 500f;
    limitValve.Limit = 0.0f;
    limitValve.sliderRanges = LimitValveTuning.GetDefaultSlider();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Object.DestroyImmediate((Object) go.GetComponent<ConduitConsumer>());
    Object.DestroyImmediate((Object) go.GetComponent<ConduitDispenser>());
    go.GetComponent<RequireInputs>().SetRequirements(true, false);
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
  }
}
