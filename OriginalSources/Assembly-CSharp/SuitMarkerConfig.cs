// Decompiled with JetBrains decompiler
// Type: SuitMarkerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class SuitMarkerConfig : IBuildingConfig
{
  public const string ID = "SuitMarker";

  public override BuildingDef CreateBuildingDef()
  {
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] construction_materials = refinedMetals;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SuitMarker", 1, 3, "changingarea_arrow_kanim", 30, 30f, tieR2, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.PreventIdleTraversalPastBuilding = true;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "SuitMarker");
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.ATMOSUIT);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
    suitMarker.LockerTags = new Tag[1]
    {
      new Tag("SuitLocker")
    };
    suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasAtmoSuit;
    go.AddOrGet<AnimTileable>().tags = new Tag[2]
    {
      new Tag("SuitMarker"),
      new Tag("SuitLocker")
    };
    go.AddTag(GameTags.JetSuitBlocker);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
  }
}
