// Decompiled with JetBrains decompiler
// Type: MethaneGeneratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class MethaneGeneratorConfig : IBuildingConfig
{
  public const string ID = "MethaneGenerator";
  public const float FUEL_CONSUMPTION_RATE = 0.09f;
  private const float CO2_RATIO = 0.25f;
  public const float WATER_OUTPUT_TEMPERATURE = 313.15f;
  private const int WIDTH = 4;
  private const int HEIGHT = 3;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues tieR5_2 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR5_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MethaneGenerator", 4, 3, "generatormethane_kanim", 100, 120f, tieR5_1, rawMetals, 2400f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.GeneratorWattageRating = 800f;
    buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
    buildingDef.ExhaustKilowattsWhenActive = 2f;
    buildingDef.SelfHeatKilowattsWhenActive = 8f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
    buildingDef.RequiresPowerOutput = true;
    buildingDef.PowerOutputOffset = new CellOffset(0, 0);
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.OutputConduitType = ConduitType.Gas;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.POWER);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.GENERATOR);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType);
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<Storage>().capacityKg = 50f;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 0.900000036f;
    conduitConsumer.capacityTag = GameTags.CombustibleGas;
    conduitConsumer.capacityKG = 0.900000036f;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    EnergyGenerator energyGenerator = go.AddOrGet<EnergyGenerator>();
    energyGenerator.powerDistributionOrder = 8;
    energyGenerator.ignoreBatteryRefillPercent = true;
    energyGenerator.formula = new EnergyGenerator.Formula()
    {
      inputs = new EnergyGenerator.InputItem[1]
      {
        new EnergyGenerator.InputItem(GameTags.Methane, 0.09f, 0.900000036f)
      },
      outputs = new EnergyGenerator.OutputItem[2]
      {
        new EnergyGenerator.OutputItem(SimHashes.DirtyWater, 0.0675f, false, new CellOffset(1, 1), 313.15f),
        new EnergyGenerator.OutputItem(SimHashes.CarbonDioxide, 0.0225f, true, new CellOffset(0, 2), 383.15f)
      }
    };
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Gas;
    conduitDispenser.invertElementFilter = true;
    conduitDispenser.elementFilter = new SimHashes[2]
    {
      SimHashes.Methane,
      SimHashes.Syngas
    };
    Tinkerable.MakePowerTinkerable(go);
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
