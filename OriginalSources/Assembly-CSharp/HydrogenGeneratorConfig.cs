// Decompiled with JetBrains decompiler
// Type: HydrogenGeneratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class HydrogenGeneratorConfig : IBuildingConfig
{
  public const string ID = "HydrogenGenerator";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues tieR5_2 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR5_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("HydrogenGenerator", 4, 3, "generatormerc_kanim", 100, 120f, tieR5_1, rawMetals, 2400f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.GeneratorWattageRating = 800f;
    buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
    buildingDef.ExhaustKilowattsWhenActive = 2f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
    buildingDef.RequiresPowerOutput = true;
    buildingDef.PowerOutputOffset = new CellOffset(1, 0);
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(-1, 0));
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
    go.AddOrGet<Storage>();
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 1f;
    conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Hydrogen);
    conduitConsumer.capacityKG = 2f;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    EnergyGenerator energyGenerator = go.AddOrGet<EnergyGenerator>();
    energyGenerator.formula = EnergyGenerator.CreateSimpleFormula(SimHashes.Hydrogen.CreateTag(), 0.1f, 2f);
    energyGenerator.powerDistributionOrder = 8;
    energyGenerator.ignoreBatteryRefillPercent = true;
    energyGenerator.meterOffset = Meter.Offset.Behind;
    Tinkerable.MakePowerTinkerable(go);
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
