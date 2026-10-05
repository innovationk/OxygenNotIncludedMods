// Decompiled with JetBrains decompiler
// Type: LogicGateXorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
public class LogicGateXorConfig : LogicGateBaseConfig
{
  public const string ID = "LogicGateXOR";

  protected override LogicGateBase.Op GetLogicOp() => LogicGateBase.Op.Xor;

  protected override CellOffset[] InputPortOffsets
  {
    get
    {
      return new CellOffset[2]
      {
        CellOffset.none,
        new CellOffset(0, 1)
      };
    }
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
        name = (string) BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_NAME,
        active = (string) BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_ACTIVE,
        inactive = (string) BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_INACTIVE
      }
    };
  }

  public override BuildingDef CreateBuildingDef()
  {
    return this.CreateBuildingDef("LogicGateXOR", "logic_xor_kanim");
  }
}
