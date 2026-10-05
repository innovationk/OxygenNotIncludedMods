// Decompiled with JetBrains decompiler
// Type: LogicGateMultiplexerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
public class LogicGateMultiplexerConfig : LogicGateBaseConfig
{
  public const string ID = "LogicGateMultiplexer";

  protected override LogicGateBase.Op GetLogicOp() => LogicGateBase.Op.Multiplexer;

  protected override CellOffset[] InputPortOffsets
  {
    get
    {
      return new CellOffset[4]
      {
        new CellOffset(-1, 3),
        new CellOffset(-1, 2),
        new CellOffset(-1, 1),
        new CellOffset(-1, 0)
      };
    }
  }

  protected override CellOffset[] OutputPortOffsets
  {
    get => new CellOffset[1]{ new CellOffset(1, 3) };
  }

  protected override CellOffset[] ControlPortOffsets
  {
    get
    {
      return new CellOffset[2]
      {
        new CellOffset(0, 0),
        new CellOffset(1, 0)
      };
    }
  }

  protected override LogicGate.LogicGateDescriptions GetDescriptions()
  {
    return new LogicGate.LogicGateDescriptions()
    {
      outputOne = new LogicGate.LogicGateDescriptions.Description()
      {
        name = (string) BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_NAME,
        active = (string) BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_ACTIVE,
        inactive = (string) BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_INACTIVE
      }
    };
  }

  public override BuildingDef CreateBuildingDef()
  {
    return this.CreateBuildingDef("LogicGateMultiplexer", "logic_multiplexer_kanim", 3, 4);
  }
}
