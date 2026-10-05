// Decompiled with JetBrains decompiler
// Type: LargeElectrobankDischargerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class LargeElectrobankDischargerConfig : IBuildingConfig
{
  public const string ID = "LargeElectrobankDischarger";
  public const float DISCHARGE_RATE = 480f;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC3;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LargeElectrobankDischarger", 2, 2, "electrobank_discharger_large_kanim", 30, 60f, tieR4, refinedMetals, 2400f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.GeneratorWattageRating = 480f;
    buildingDef.GeneratorBaseCapacity = 480f;
    buildingDef.ExhaustKilowattsWhenActive = 0.25f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.RequiresPowerOutput = true;
    buildingDef.PowerOutputOffset = new CellOffset(0, 0);
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.BATTERY);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = true;
    storage.capacityKg = 20f;
    storage.storageFilters = STORAGEFILTERS.POWER_BANKS;
    go.AddOrGet<TreeFilterable>().allResourceFilterLabelString = (string) UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON;
    go.AddOrGet<ElectrobankDischarger>().wattageRating = 480f;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    SymbolOverrideControllerUtil.AddToPrefab(go);
  }
}
