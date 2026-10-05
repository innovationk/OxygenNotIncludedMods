// Decompiled with JetBrains decompiler
// Type: PeatGeneratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class PeatGeneratorConfig : IBuildingConfig
{
  public const string ID = "PeatGenerator";
  private const float PEAT_BURN_RATE = 1f;
  public const float EXHAUST_LIQUID_RATE = 0.2f;
  public const float EXHAUST_GAS_RATE = 0.04f;
  private const float PEAT_CAPACITY = 600f;
  public const float CO2_OUTPUT_TEMPERATURE = 383.15f;
  public const float LIQUID_OUTPUT_TEMPERATURE = 313.15f;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR5_2 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR5_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("PeatGenerator", 3, 2, "generatorpeat_kanim", 100, 120f, tieR5_1, allMetals, 2400f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.GeneratorWattageRating = 480f;
    buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
    buildingDef.ExhaustKilowattsWhenActive = 4f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.RequiresPowerOutput = true;
    buildingDef.PowerOutputOffset = new CellOffset(0, 0);
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.POWER);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.GENERATOR);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType);
    EnergyGenerator energyGenerator = go.AddOrGet<EnergyGenerator>();
    energyGenerator.formula = new EnergyGenerator.Formula()
    {
      inputs = new EnergyGenerator.InputItem[1]
      {
        new EnergyGenerator.InputItem(SimHashes.Peat.CreateTag(), 1f, 600f)
      },
      outputs = new EnergyGenerator.OutputItem[2]
      {
        new EnergyGenerator.OutputItem(SimHashes.CarbonDioxide, 0.04f, false, new CellOffset(0, 1), 383.15f),
        new EnergyGenerator.OutputItem(SimHashes.DirtyWater, 0.2f, false, new CellOffset(1, 1), 313.15f)
      }
    };
    energyGenerator.meterOffset = Meter.Offset.Infront;
    energyGenerator.SetSliderValue(50f, 0);
    energyGenerator.powerDistributionOrder = 9;
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 600f;
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = SimHashes.Peat.CreateTag();
    manualDeliveryKg.capacity = storage.capacityKg;
    manualDeliveryKg.refillMass = 100f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.PowerFetch.IdHash;
    Tinkerable.MakePowerTinkerable(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
