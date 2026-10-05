// Decompiled with JetBrains decompiler
// Type: SolidLimitValveConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SolidLimitValveConfig : IBuildingConfig
{
  public const string ID = "SolidLimitValve";
  private const ConduitType CONDUIT_TYPE = ConduitType.Solid;

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
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SolidLimitValve", 1, 2, "limit_valve_solid_kanim", 30, 10f, construction_mass, construction_materials, 1600f, BuildLocationRule.Anywhere, tieR0, noise);
    buildingDef.InputConduitType = ConduitType.Solid;
    buildingDef.OutputConduitType = ConduitType.Solid;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 10f;
    buildingDef.PowerInputOffset = new CellOffset(0, 1);
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      new LogicPorts.Port(LimitValve.RESET_PORT_ID, new CellOffset(0, 1), (string) STRINGS.BUILDINGS.PREFABS.SOLIDLIMITVALVE.LOGIC_PORT_RESET, (string) STRINGS.BUILDINGS.PREFABS.SOLIDLIMITVALVE.RESET_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.SOLIDLIMITVALVE.RESET_PORT_INACTIVE, false, LogicPortSpriteType.ResetUpdate, true)
    };
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort(LimitValve.OUTPUT_PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.SOLIDLIMITVALVE.LOGIC_PORT_OUTPUT, (string) STRINGS.BUILDINGS.PREFABS.SOLIDLIMITVALVE.OUTPUT_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.SOLIDLIMITVALVE.OUTPUT_PORT_INACTIVE)
    };
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidLimitValve");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Object.DestroyImmediate((Object) go.GetComponent<BuildingEnabledButton>());
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<PoweredActiveTransitionController.Def>();
    go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
    go.AddOrGet<SolidConduitBridge>();
    LimitValve limitValve = go.AddOrGet<LimitValve>();
    limitValve.conduitType = ConduitType.Solid;
    limitValve.displayUnitsInsteadOfMass = true;
    limitValve.Limit = 0.0f;
    limitValve.maxLimitKg = 500f;
    limitValve.sliderRanges = new NonLinearSlider.Range[3]
    {
      new NonLinearSlider.Range(50f, 50f),
      new NonLinearSlider.Range(30f, 200f),
      new NonLinearSlider.Range(20f, limitValve.maxLimitKg)
    };
  }
}
