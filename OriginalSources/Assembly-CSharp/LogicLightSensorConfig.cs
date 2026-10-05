// Decompiled with JetBrains decompiler
// Type: LogicLightSensorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class LogicLightSensorConfig : IBuildingConfig
{
  public static string ID = "LogicLightSensor";

  public override BuildingDef CreateBuildingDef()
  {
    string id = LogicLightSensorConfig.ID;
    float[] construction_mass = new float[2]
    {
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0]
    };
    string[] construction_materials = new string[2]
    {
      "RefinedMetal",
      "Glasses"
    };
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 1, 1, "logiclightsensor_kanim", 30, 30f, construction_mass, construction_materials, 1600f, BuildLocationRule.Anywhere, tieR0, noise);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.AlwaysOperational = true;
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>();
    buildingDef.LogicOutputPorts.Add(LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICLIGHTSENSOR.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICLIGHTSENSOR.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICLIGHTSENSOR.LOGIC_PORT_INACTIVE, true));
    SoundEventVolumeCache.instance.AddVolume("logiclightsensor_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("logiclightsensor_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicLightSensorConfig.ID);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.AUTOMATION);
    return buildingDef;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    LogicLightSensor logicLightSensor = go.AddOrGet<LogicLightSensor>();
    logicLightSensor.manuallyControlled = false;
    logicLightSensor.minBrightness = 0.0f;
    logicLightSensor.maxBrightness = 15000f;
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
  }
}
