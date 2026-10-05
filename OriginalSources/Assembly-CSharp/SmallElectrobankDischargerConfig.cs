// Decompiled with JetBrains decompiler
// Type: SmallElectrobankDischargerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class SmallElectrobankDischargerConfig : IBuildingConfig
{
  public const string ID = "SmallElectrobankDischarger";
  public const float DISCHARGE_RATE = 60f;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC3;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR1_1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues tieR1_2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = tieR1_1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SmallElectrobankDischarger", 1, 1, "electrobank_discharger_small_kanim", 30, 10f, tieR2, allMetals, 2400f, BuildLocationRule.OnFoundationRotatable, tieR1_2, noise);
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.GeneratorWattageRating = 60f;
    buildingDef.GeneratorBaseCapacity = 60f;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.RequiresPowerOutput = true;
    buildingDef.PowerOutputOffset = new CellOffset(0, 0);
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "small";
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.BATTERY);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    go.AddOrGet<LoopingSounds>();
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = true;
    storage.capacityKg = 20f;
    storage.storageFilters = STORAGEFILTERS.POWER_BANKS;
    go.AddOrGet<TreeFilterable>().allResourceFilterLabelString = (string) UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON;
    go.AddOrGet<ElectrobankDischarger>().wattageRating = 60f;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    SymbolOverrideControllerUtil.AddToPrefab(go);
  }
}
