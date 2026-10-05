// Decompiled with JetBrains decompiler
// Type: LogicInterasteroidReceiverConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class LogicInterasteroidReceiverConfig : IBuildingConfig
{
  public const string ID = "LogicInterasteroidReceiver";
  public const string OUTPUT_PORT_ID = "OutputPort";

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicInterasteroidReceiver", 1, 1, "inter_asteroid_automation_signal_receiver_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, TUNING.MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
    buildingDef.AlwaysOperational = false;
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort((HashedString) "OutputPort", new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT_INACTIVE, true)
    };
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicInterasteroidReceiver");
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.AUTOMATION);
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    LogicInterasteroidReceiverConfig.AddVisualizer(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicBroadcastReceiver>().PORT_ID = "OutputPort";
    LogicInterasteroidReceiverConfig.AddVisualizer(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    LogicInterasteroidReceiverConfig.AddVisualizer(go);
  }

  private static void AddVisualizer(GameObject prefab)
  {
    SkyVisibilityVisualizer visibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
    visibilityVisualizer.RangeMin = 0;
    visibilityVisualizer.RangeMax = 0;
    visibilityVisualizer.SkipOnModuleInteriors = true;
  }
}
