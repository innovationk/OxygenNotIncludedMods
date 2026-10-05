// Decompiled with JetBrains decompiler
// Type: LogicDuplicantSensorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class LogicDuplicantSensorConfig : IBuildingConfig
{
  public const string ID = "LogicDuplicantSensor";
  private const int RANGE = 4;

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicDuplicantSensor", 1, 1, "presence_sensor_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, TUNING.MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFoundationRotatable, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.AlwaysOperational = true;
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT_INACTIVE, true)
    };
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicDuplicantSensor");
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.AUTOMATION);
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    LogicDuplicantSensorConfig.AddVisualizer(go, true);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    LogicDuplicantSensor logicDuplicantSensor = go.AddOrGet<LogicDuplicantSensor>();
    logicDuplicantSensor.defaultState = false;
    logicDuplicantSensor.manuallyControlled = false;
    logicDuplicantSensor.pickupRange = 4;
    LogicDuplicantSensorConfig.AddVisualizer(go, false);
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
  }

  private static void AddVisualizer(GameObject prefab, bool movable)
  {
    RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
    rangeVisualizer.OriginOffset = new Vector2I(0, 0);
    rangeVisualizer.RangeMin.x = -2;
    rangeVisualizer.RangeMin.y = 0;
    rangeVisualizer.RangeMax.x = 2;
    rangeVisualizer.RangeMax.y = 4;
    rangeVisualizer.BlockingTileVisible = true;
  }
}
