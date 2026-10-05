// Decompiled with JetBrains decompiler
// Type: FloorSwitchConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class FloorSwitchConfig : IBuildingConfig
{
  public const string ID = "FloorSwitch";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FloorSwitch", 1, 1, "pressureswitch_kanim", 30, 30f, tieR1, refinedMetals, 1600f, BuildLocationRule.Tile, tieR0, noise);
    buildingDef.IsFoundation = true;
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.TileLayer = ObjectLayer.FoundationTile;
    buildingDef.ReplacementLayer = ObjectLayer.ReplacementTile;
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.LOGIC_PORT_INACTIVE, true)
    };
    buildingDef.AlwaysOperational = true;
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "FloorSwitch");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
    simCellOccupier.doReplaceElement = true;
    simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.BONUS_2;
    simCellOccupier.notifyOnMelt = true;
    go.GetComponent<KPrefabID>().prefabSpawnFn += new KPrefabID.PrefabFn(this.OnSpawn);
  }

  private void OnSpawn(GameObject instance)
  {
    foreach (KBatchedAnimController kbatchedAnimController in instance.GetComponentsInChildrenOnly<KBatchedAnimController>())
    {
      kbatchedAnimController.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.LiquidVisibilityLayer, false);
      kbatchedAnimController.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.WaterProof, true);
    }
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    LogicMassSensor logicMassSensor = go.AddOrGet<LogicMassSensor>();
    logicMassSensor.rangeMin = 0.0f;
    logicMassSensor.rangeMax = 2000f;
    logicMassSensor.Threshold = 10f;
    logicMassSensor.ActivateAboveThreshold = true;
    logicMassSensor.manuallyControlled = false;
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
  }
}
