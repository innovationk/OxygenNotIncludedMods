// Decompiled with JetBrains decompiler
// Type: LeadSuitMarkerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class LeadSuitMarkerConfig : IBuildingConfig
{
  public const string ID = "LeadSuitMarker";

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] construction_materials = refinedMetals;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LeadSuitMarker", 2, 4, "changingarea_radiation_arrow_kanim", 30, 30f, tieR2, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.PreventIdleTraversalPastBuilding = true;
    buildingDef.Deprecated = !Sim.IsRadiationEnabled();
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "LeadSuitMarker");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
    suitMarker.LockerTags = new Tag[1]
    {
      new Tag("LeadSuitLocker")
    };
    suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasLeadSuit;
    go.AddOrGet<AnimTileable>().tags = new Tag[2]
    {
      new Tag("LeadSuitMarker"),
      new Tag("LeadSuitLocker")
    };
    go.AddTag(GameTags.JetSuitBlocker);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
  }
}
