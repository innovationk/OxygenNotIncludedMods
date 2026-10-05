// Decompiled with JetBrains decompiler
// Type: AutoMinerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class AutoMinerConfig : IBuildingConfig
{
  public const string ID = "AutoMiner";
  private const int RANGE = 7;
  private const int X = -7;
  private const int Y = 0;
  private const int WIDTH = 16 /*0x10*/;
  private const int HEIGHT = 9;
  private const int VISION_OFFSET = 1;

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AutoMiner", 2, 2, "auto_miner_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, TUNING.MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFoundationRotatable, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0);
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "AutoMiner");
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.ROBOT);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<Operational>();
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<MiningSounds>();
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    AutoMinerConfig.AddVisualizer(go, true);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    AutoMinerConfig.AddVisualizer(go, false);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    AutoMiner autoMiner = go.AddOrGet<AutoMiner>();
    autoMiner.x = -7;
    autoMiner.y = 0;
    autoMiner.width = 16 /*0x10*/;
    autoMiner.height = 9;
    autoMiner.vision_offset = new CellOffset(0, 1);
    AutoMinerConfig.AddVisualizer(go, false);
  }

  private static void AddVisualizer(GameObject prefab, bool movable)
  {
    RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
    rangeVisualizer.RangeMin.x = -7;
    rangeVisualizer.RangeMin.y = -1;
    rangeVisualizer.RangeMax.x = 8;
    rangeVisualizer.RangeMax.y = 7;
    rangeVisualizer.OriginOffset = new Vector2I(0, 1);
    rangeVisualizer.BlockingTileVisible = false;
    prefab.GetComponent<KPrefabID>().instantiateFn += (KPrefabID.PrefabFn) (go => go.GetComponent<RangeVisualizer>().BlockingCb = AutoMiner.DigBlockingCB);
  }
}
