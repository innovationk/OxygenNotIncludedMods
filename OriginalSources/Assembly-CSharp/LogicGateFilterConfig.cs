// Decompiled with JetBrains decompiler
// Type: LogicGateFilterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class LogicGateFilterConfig : LogicGateBaseConfig
{
  public const string ID = "LogicGateFILTER";

  protected override LogicGateBase.Op GetLogicOp() => LogicGateBase.Op.CustomSingle;

  protected override CellOffset[] InputPortOffsets
  {
    get => new CellOffset[1]{ CellOffset.none };
  }

  protected override CellOffset[] OutputPortOffsets
  {
    get => new CellOffset[1]{ new CellOffset(1, 0) };
  }

  protected override CellOffset[] ControlPortOffsets => (CellOffset[]) null;

  protected override LogicGate.LogicGateDescriptions GetDescriptions()
  {
    return new LogicGate.LogicGateDescriptions()
    {
      outputOne = new LogicGate.LogicGateDescriptions.Description()
      {
        name = (string) BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_NAME,
        active = (string) BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_ACTIVE,
        inactive = (string) BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_INACTIVE
      }
    };
  }

  public override BuildingDef CreateBuildingDef()
  {
    return this.CreateBuildingDef("LogicGateFILTER", "logic_filter_kanim", height: 1);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    LogicGateFilter logicGateFilter = go.AddComponent<LogicGateFilter>();
    logicGateFilter.op = this.GetLogicOp();
    logicGateFilter.inputPortOffsets = this.InputPortOffsets;
    logicGateFilter.outputPortOffsets = this.OutputPortOffsets;
    logicGateFilter.controlPortOffsets = this.ControlPortOffsets;
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (game_object => game_object.GetComponent<LogicGateFilter>().SetPortDescriptions(this.GetDescriptions()));
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
  }
}
