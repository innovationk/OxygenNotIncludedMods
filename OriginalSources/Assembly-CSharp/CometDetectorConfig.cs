// Decompiled with JetBrains decompiler
// Type: CometDetectorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class CometDetectorConfig : IBuildingConfig
{
  public static string ID = "CometDetector";
  public const float COVERAGE_REQUIRED_01 = 0.5f;
  public const float BEST_WARNING_TIME_IN_SECONDS = 200f;
  public const float WORST_WARNING_TIME_IN_SECONDS = 1f;
  public const int SCAN_RADIUS = 15;
  public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 0), 15, new CellOffset(0, 0), 15, 1);
  public const float LOGIC_SIGNAL_DELAY_ON_LOAD = 3f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = CometDetectorConfig.ID;
    float[] tieR0_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0_2 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 2, 4, "meteor_detector_kanim", 30, 30f, tieR0_1, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR0_2, noise);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = true;
    buildingDef.Entombable = true;
    buildingDef.RequiresPowerInput = true;
    buildingDef.AddLogicPowerPort = false;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT_INACTIVE, true)
    };
    SoundEventVolumeCache.instance.AddVolume("world_element_sensor_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("world_element_sensor_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, CometDetectorConfig.ID);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    if (DlcManager.IsExpansion1Active())
      go.AddOrGetDef<ClusterCometDetector.Def>();
    else
      go.AddOrGetDef<CometDetector.Def>();
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
    CometDetectorConfig.AddVisualizer(go);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    CometDetectorConfig.AddVisualizer(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    CometDetectorConfig.AddVisualizer(go);
  }

  private static void AddVisualizer(GameObject prefab)
  {
    ScannerNetworkVisualizer networkVisualizer = prefab.AddOrGet<ScannerNetworkVisualizer>();
    networkVisualizer.RangeMin = -15;
    networkVisualizer.RangeMax = 15;
  }
}
