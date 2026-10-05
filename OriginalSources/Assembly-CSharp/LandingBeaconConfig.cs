// Decompiled with JetBrains decompiler
// Type: LandingBeaconConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TUNING;
using UnityEngine;

#nullable disable
public class LandingBeaconConfig : IBuildingConfig
{
  public const string ID = "LandingBeacon";
  public const int LANDING_ACCURACY = 3;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR2_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues tieR2_2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues tieR1 = BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = tieR2_2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LandingBeacon", 1, 3, "landing_beacon_kanim", 1000, 30f, tieR2_1, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.DefaultAnimState = "off";
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.OverheatTemperature = 398.15f;
    buildingDef.Floodable = false;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.RequiresPowerInput = false;
    buildingDef.CanMove = false;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGetDef<LandingBeacon.Def>();
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    LandingBeaconConfig.AddVisualizer(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    LandingBeaconConfig.AddVisualizer(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    LandingBeaconConfig.AddVisualizer(go);
  }

  private static void AddVisualizer(GameObject prefab)
  {
    SkyVisibilityVisualizer visibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
    visibilityVisualizer.RangeMin = 0;
    visibilityVisualizer.RangeMax = 0;
    prefab.GetComponent<KPrefabID>().instantiateFn += (KPrefabID.PrefabFn) (go => go.GetComponent<SkyVisibilityVisualizer>().SkyVisibilityCb = new Func<int, bool>(LandingBeaconConfig.BeaconSkyVisibility));
  }

  private static bool BeaconSkyVisibility(int cell)
  {
    DebugUtil.DevAssert((UnityEngine.Object) ClusterManager.Instance != (UnityEngine.Object) null, "beacon assumes DLC");
    if (!Grid.IsValidCell(cell) || Grid.WorldIdx[cell] == byte.MaxValue)
      return false;
    int y = (int) ClusterManager.Instance.GetWorld((int) Grid.WorldIdx[cell]).maximumBounds.y;
    for (int index = cell; Grid.CellRow(index) <= y; index = Grid.CellAbove(index))
    {
      if (!Grid.IsValidCell(index) || Grid.Solid[index])
        return false;
    }
    return true;
  }
}
