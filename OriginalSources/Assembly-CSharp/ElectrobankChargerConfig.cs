// Decompiled with JetBrains decompiler
// Type: ElectrobankChargerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class ElectrobankChargerConfig : IBuildingConfig
{
  public const string ID = "ElectrobankCharger";
  public const float BUILDING_WATTAGE_COST = 480f;
  public const float CHARGE_RATE = 400f;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC3;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues tieR1_1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues tieR1_2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = tieR1_1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ElectrobankCharger", 2, 2, "electrobank_charger_small_kanim", 30, 30f, tieR2, refinedMetals, 2400f, BuildLocationRule.OnFloor, tieR1_2, noise);
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(0, 0);
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "small";
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.BATTERY);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 1f;
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = GameTags.EmptyPortableBattery;
    manualDeliveryKg.capacity = storage.capacityKg;
    manualDeliveryKg.refillMass = 20f;
    manualDeliveryKg.MinimumMass = 20f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.PowerFetch.IdHash;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<ElectrobankCharger.Def>();
  }
}
