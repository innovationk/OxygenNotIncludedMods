// Decompiled with JetBrains decompiler
// Type: LogicGateBufferConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class LogicGateBufferConfig : LogicGateBaseConfig
{
  public const string ID = "LogicGateBUFFER";

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
        name = (string) BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_NAME,
        active = (string) BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_ACTIVE,
        inactive = (string) BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_INACTIVE
      }
    };
  }

  public override BuildingDef CreateBuildingDef()
  {
    return this.CreateBuildingDef("LogicGateBUFFER", "logic_buffer_kanim", height: 1);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    LogicGateBuffer logicGateBuffer = go.AddComponent<LogicGateBuffer>();
    logicGateBuffer.op = this.GetLogicOp();
    logicGateBuffer.inputPortOffsets = this.InputPortOffsets;
    logicGateBuffer.outputPortOffsets = this.OutputPortOffsets;
    logicGateBuffer.controlPortOffsets = this.ControlPortOffsets;
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (game_object => game_object.GetComponent<LogicGateBuffer>().SetPortDescriptions(this.GetDescriptions()));
    go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
  }
}
