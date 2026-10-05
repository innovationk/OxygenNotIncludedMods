// Decompiled with JetBrains decompiler
// Type: ReefGeneratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class ReefGeneratorConfig : IBuildingConfig
{
  public const string ID = "ReefGenerator";
  private const float WATTS_PRODUCED = 300f;
  private const int GASKET_UNITS = 1;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public override BuildingDef CreateBuildingDef()
  {
    float[] construction_mass = new float[2]
    {
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
      1f
    };
    string[] construction_materials = new string[2]
    {
      "Metal",
      "BuildingGasket"
    };
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues tieR2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ReefGenerator", 3, 3, "turbine_reef_kanim", 100, 60f, construction_mass, construction_materials, 2400f, BuildLocationRule.OnFloor, tieR2, noise);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingBack;
    buildingDef.GeneratorWattageRating = 300f;
    buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.RequiresPowerOutput = true;
    buildingDef.PowerOutputOffset = new CellOffset(0, 0);
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(-1, 2));
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.POWER);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.GENERATOR);
    buildingDef.AttachmentSlotTag = GameTags.ReefGenerator;
    buildingDef.BuildLocationRule = BuildLocationRule.BuildingAttachPoint;
    buildingDef.ObjectLayer = ObjectLayer.AttachableBuilding;
    buildingDef.PowerOutputOffset = new CellOffset(1, 2);
    buildingDef.Floodable = false;
    buildingDef.POIUnlockable = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    component.AddTag(RoomConstraints.ConstraintTags.PowerBuilding);
    component.AddTag(RoomConstraints.ConstraintTags.GeneratorType);
    component.AddTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType);
    go.AddOrGet<ReefGeneratorPower>().powerDistributionOrder = 9;
    go.AddOrGet<LoopingSounds>();
    Prioritizable.AddRef(go);
    go.AddOrGetDef<ReefGenerator.Def>();
    Tinkerable.MakePowerTinkerable(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
  }
}
