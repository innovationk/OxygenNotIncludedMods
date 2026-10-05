// Decompiled with JetBrains decompiler
// Type: LogicPorts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicPorts")]
public class LogicPorts : KMonoBehaviour, IGameObjectEffectDescriptor, IRenderEveryTick
{
  [SerializeField]
  public LogicPorts.Port[] outputPortInfo;
  [SerializeField]
  public LogicPorts.Port[] inputPortInfo;
  public List<ILogicUIElement> outputPorts;
  public List<ILogicUIElement> inputPorts;
  private int cell = -1;
  private Orientation orientation = Orientation.NumRotations;
  [Serialize]
  private int[] serializedOutputValues;
  private bool isPhysical;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.autoRegisterSimRender = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Building component = this.GetComponent<Building>();
    this.isPhysical = (UnityEngine.Object) component == (UnityEngine.Object) null || component is BuildingComplete;
    if ((this.isPhysical ? 0 : (!(component is BuildingUnderConstruction) ? 1 : 0)) != 0)
    {
      OverlayScreen.Instance.OnOverlayChanged += new Action<HashedString>(this.OnOverlayChanged);
      this.OnOverlayChanged(OverlayScreen.Instance.mode);
      this.CreateVisualizers();
      SimAndRenderScheduler.instance.Add((object) this);
    }
    else if (this.isPhysical)
    {
      this.UpdateMissingWireIcon();
      this.CreatePhysicalPorts();
    }
    else
      this.CreateVisualizers();
  }

  protected override void OnCleanUp()
  {
    OverlayScreen.Instance.OnOverlayChanged -= new Action<HashedString>(this.OnOverlayChanged);
    this.DestroyVisualizers();
    if (this.isPhysical)
      this.DestroyPhysicalPorts();
    base.OnCleanUp();
  }

  public void RenderEveryTick(float dt) => this.CreateVisualizers();

  public void HackRefreshVisualizers() => this.CreateVisualizers();

  private void CreateVisualizers()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    bool flag = cell != this.cell;
    this.cell = cell;
    if (!flag)
    {
      Rotatable component = this.GetComponent<Rotatable>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        Orientation orientation = component.GetOrientation();
        flag = orientation != this.orientation;
        this.orientation = orientation;
      }
    }
    if (!flag)
      return;
    this.DestroyVisualizers();
    if (this.outputPortInfo != null)
    {
      this.outputPorts = new List<ILogicUIElement>();
      for (int index = 0; index < this.outputPortInfo.Length; ++index)
      {
        LogicPorts.Port port = this.outputPortInfo[index];
        LogicPortVisualizer elem = new LogicPortVisualizer(this.GetActualCell(port.cellOffset), port.spriteType);
        this.outputPorts.Add((ILogicUIElement) elem);
        Game.Instance.logicCircuitManager.AddVisElem((ILogicUIElement) elem);
      }
    }
    if (this.inputPortInfo == null)
      return;
    this.inputPorts = new List<ILogicUIElement>();
    for (int index = 0; index < this.inputPortInfo.Length; ++index)
    {
      LogicPorts.Port port = this.inputPortInfo[index];
      LogicPortVisualizer elem = new LogicPortVisualizer(this.GetActualCell(port.cellOffset), port.spriteType);
      this.inputPorts.Add((ILogicUIElement) elem);
      Game.Instance.logicCircuitManager.AddVisElem((ILogicUIElement) elem);
    }
  }

  private void DestroyVisualizers()
  {
    if (this.outputPorts != null)
    {
      foreach (ILogicUIElement outputPort in this.outputPorts)
        Game.Instance.logicCircuitManager.RemoveVisElem(outputPort);
    }
    if (this.inputPorts == null)
      return;
    foreach (ILogicUIElement inputPort in this.inputPorts)
      Game.Instance.logicCircuitManager.RemoveVisElem(inputPort);
  }

  private void CreatePhysicalPorts(bool forceCreate = false)
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    if (cell == this.cell && !forceCreate)
      return;
    this.cell = cell;
    this.DestroyVisualizers();
    if (this.outputPortInfo != null)
    {
      this.outputPorts = new List<ILogicUIElement>();
      for (int index = 0; index < this.outputPortInfo.Length; ++index)
      {
        LogicPorts.Port info = this.outputPortInfo[index];
        LogicEventSender elem = new LogicEventSender(info.id, this.GetActualCell(info.cellOffset), (Action<int, int>) ((new_value, prev_value) =>
        {
          if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
            return;
          this.OnLogicValueChanged(info.id, new_value, prev_value);
        }), new Action<int, bool>(this.OnLogicNetworkConnectionChanged), info.spriteType);
        this.outputPorts.Add((ILogicUIElement) elem);
        Game.Instance.logicCircuitManager.AddVisElem((ILogicUIElement) elem);
        Game.Instance.logicCircuitSystem.AddToNetworks(elem.GetLogicUICell(), (object) elem, true);
      }
      if (this.serializedOutputValues != null && this.serializedOutputValues.Length == this.outputPorts.Count)
      {
        for (int index = 0; index < this.outputPorts.Count; ++index)
          (this.outputPorts[index] as LogicEventSender).SetValue(this.serializedOutputValues[index]);
      }
      else
      {
        for (int index = 0; index < this.outputPorts.Count; ++index)
          (this.outputPorts[index] as LogicEventSender).SetValue(0);
      }
    }
    this.serializedOutputValues = (int[]) null;
    if (this.inputPortInfo == null)
      return;
    this.inputPorts = new List<ILogicUIElement>();
    for (int index = 0; index < this.inputPortInfo.Length; ++index)
    {
      LogicPorts.Port info = this.inputPortInfo[index];
      LogicEventHandler elem = new LogicEventHandler(this.GetActualCell(info.cellOffset), (Action<int, int>) ((new_value, prev_value) =>
      {
        if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
          return;
        this.OnLogicValueChanged(info.id, new_value, prev_value);
      }), new Action<int, bool>(this.OnLogicNetworkConnectionChanged), info.spriteType);
      this.inputPorts.Add((ILogicUIElement) elem);
      Game.Instance.logicCircuitManager.AddVisElem((ILogicUIElement) elem);
      Game.Instance.logicCircuitSystem.AddToNetworks(elem.GetLogicUICell(), (object) elem, true);
    }
  }

  private bool ShowMissingWireIcon()
  {
    LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
    if (this.outputPortInfo != null)
    {
      for (int index = 0; index < this.outputPortInfo.Length; ++index)
      {
        LogicPorts.Port port = this.outputPortInfo[index];
        if (port.requiresConnection)
        {
          int portCell = this.GetPortCell(port.id);
          if (logicCircuitManager.GetNetworkForCell(portCell) == null)
            return true;
        }
      }
    }
    if (this.inputPortInfo != null)
    {
      for (int index = 0; index < this.inputPortInfo.Length; ++index)
      {
        LogicPorts.Port port = this.inputPortInfo[index];
        if (port.requiresConnection)
        {
          int portCell = this.GetPortCell(port.id);
          if (logicCircuitManager.GetNetworkForCell(portCell) == null)
            return true;
        }
      }
    }
    return false;
  }

  public void OnMove()
  {
    this.DestroyPhysicalPorts();
    this.CreatePhysicalPorts();
  }

  private void OnLogicNetworkConnectionChanged(int cell, bool connected)
  {
    this.UpdateMissingWireIcon();
  }

  private void UpdateMissingWireIcon()
  {
    LogicCircuitManager.ToggleNoWireConnected(this.ShowMissingWireIcon(), this.gameObject);
  }

  private void DestroyPhysicalPorts()
  {
    if (this.outputPorts != null)
    {
      foreach (ILogicEventSender outputPort in this.outputPorts)
        Game.Instance.logicCircuitSystem.RemoveFromNetworks(outputPort.GetLogicCell(), (object) outputPort, true);
    }
    if (this.inputPorts == null)
      return;
    for (int index = 0; index < this.inputPorts.Count; ++index)
    {
      if (this.inputPorts[index] is LogicEventHandler inputPort)
        Game.Instance.logicCircuitSystem.RemoveFromNetworks(inputPort.GetLogicCell(), (object) inputPort, true);
    }
  }

  private void OnLogicValueChanged(HashedString port_id, int new_value, int prev_value)
  {
    if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
      return;
    LogicValueChanged logicValueChanged = LogicValueChanged.Pool.Get();
    logicValueChanged.portID = port_id;
    logicValueChanged.newValue = new_value;
    logicValueChanged.prevValue = prev_value;
    this.gameObject.Trigger(-801688580, (object) logicValueChanged);
    LogicValueChanged.Pool.Release(logicValueChanged);
  }

  private int GetActualCell(CellOffset offset)
  {
    Rotatable component = this.GetComponent<Rotatable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      offset = component.GetRotatedCellOffset(offset);
    return Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), offset);
  }

  public bool TryGetPortAtCell(int cell, out LogicPorts.Port port, out bool isInput)
  {
    foreach (LogicPorts.Port port1 in this.inputPortInfo)
    {
      if (this.GetActualCell(port1.cellOffset) == cell)
      {
        port = port1;
        isInput = true;
        return true;
      }
    }
    foreach (LogicPorts.Port port2 in this.outputPortInfo)
    {
      if (this.GetActualCell(port2.cellOffset) == cell)
      {
        port = port2;
        isInput = false;
        return true;
      }
    }
    port = new LogicPorts.Port();
    isInput = false;
    return false;
  }

  public void SendSignal(HashedString port_id, int new_value)
  {
    if (this.outputPortInfo != null && this.outputPorts == null)
      this.CreatePhysicalPorts(true);
    foreach (LogicEventSender outputPort in this.outputPorts)
    {
      if (outputPort.ID == port_id)
      {
        outputPort.SetValue(new_value);
        break;
      }
    }
  }

  public int GetPortCell(HashedString port_id)
  {
    foreach (LogicPorts.Port port in this.inputPortInfo)
    {
      if (port.id == port_id)
        return this.GetActualCell(port.cellOffset);
    }
    foreach (LogicPorts.Port port in this.outputPortInfo)
    {
      if (port.id == port_id)
        return this.GetActualCell(port.cellOffset);
    }
    return -1;
  }

  public int GetInputValue(HashedString port_id)
  {
    for (int index = 0; index < this.inputPortInfo.Length && this.inputPorts != null; ++index)
    {
      if (this.inputPortInfo[index].id == port_id)
        return !(this.inputPorts[index] is LogicEventHandler inputPort) ? 0 : inputPort.Value;
    }
    return 0;
  }

  public int GetOutputValue(HashedString port_id)
  {
    for (int index = 0; index < this.outputPorts.Count && this.outputPorts[index] is LogicEventSender outputPort; ++index)
    {
      if (outputPort.ID == port_id)
        return outputPort.GetLogicValue();
    }
    return 0;
  }

  public bool IsPortConnected(HashedString port_id)
  {
    return Game.Instance.logicCircuitManager.GetNetworkForCell(this.GetPortCell(port_id)) != null;
  }

  private void OnOverlayChanged(HashedString mode)
  {
    if (mode == OverlayModes.Logic.ID)
    {
      this.enabled = true;
      this.CreateVisualizers();
    }
    else
    {
      this.enabled = false;
      this.DestroyVisualizers();
    }
  }

  public LogicWire.BitDepth GetConnectedWireBitDepth(HashedString port_id)
  {
    LogicWire.BitDepth connectedWireBitDepth = LogicWire.BitDepth.NumRatings;
    int portCell = this.GetPortCell(port_id);
    GameObject gameObject = Grid.Objects[portCell, 31 /*0x1F*/];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      LogicWire component = gameObject.GetComponent<LogicWire>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        connectedWireBitDepth = component.MaxBitDepth;
    }
    return connectedWireBitDepth;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    LogicPorts component = go.GetComponent<LogicPorts>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      if (component.inputPortInfo != null && component.inputPortInfo.Length != 0)
      {
        Descriptor descriptor = new Descriptor((string) UI.LOGIC_PORTS.INPUT_PORTS, (string) UI.LOGIC_PORTS.INPUT_PORTS_TOOLTIP);
        descriptors.Add(descriptor);
        foreach (LogicPorts.Port port in component.inputPortInfo)
        {
          string tooltip = string.Format((string) UI.LOGIC_PORTS.INPUT_PORT_TOOLTIP, (object) port.activeDescription, (object) port.inactiveDescription);
          descriptor = new Descriptor(port.description, tooltip);
          descriptor.IncreaseIndent();
          descriptors.Add(descriptor);
        }
      }
      if (component.outputPortInfo != null && component.outputPortInfo.Length != 0)
      {
        Descriptor descriptor = new Descriptor((string) UI.LOGIC_PORTS.OUTPUT_PORTS, (string) UI.LOGIC_PORTS.OUTPUT_PORTS_TOOLTIP);
        descriptors.Add(descriptor);
        foreach (LogicPorts.Port port in component.outputPortInfo)
        {
          string tooltip = string.Format((string) UI.LOGIC_PORTS.OUTPUT_PORT_TOOLTIP, (object) port.activeDescription, (object) port.inactiveDescription);
          descriptor = new Descriptor(port.description, tooltip);
          descriptor.IncreaseIndent();
          descriptors.Add(descriptor);
        }
      }
    }
    return descriptors;
  }

  [System.Runtime.Serialization.OnSerializing]
  private void OnSerializing()
  {
    if (!this.isPhysical || this.outputPorts == null)
      return;
    this.serializedOutputValues = new int[this.outputPorts.Count];
    for (int index = 0; index < this.outputPorts.Count; ++index)
    {
      LogicEventSender outputPort = this.outputPorts[index] as LogicEventSender;
      this.serializedOutputValues[index] = outputPort.GetLogicValue();
    }
  }

  [System.Runtime.Serialization.OnSerialized]
  private void OnSerialized() => this.serializedOutputValues = (int[]) null;

  [Serializable]
  public struct Port(
    HashedString id,
    CellOffset cell_offset,
    string description,
    string activeDescription,
    string inactiveDescription,
    bool show_wire_missing_icon,
    LogicPortSpriteType sprite_type,
    bool display_custom_name = false)
  {
    public HashedString id = id;
    public CellOffset cellOffset = cell_offset;
    public string description = description;
    public string activeDescription = activeDescription;
    public string inactiveDescription = inactiveDescription;
    public bool requiresConnection = show_wire_missing_icon;
    public LogicPortSpriteType spriteType = sprite_type;
    public bool displayCustomName = display_custom_name;

    public static LogicPorts.Port InputPort(
      HashedString id,
      CellOffset cell_offset,
      string description,
      string activeDescription,
      string inactiveDescription,
      bool show_wire_missing_icon = false,
      bool display_custom_name = false)
    {
      return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.Input, display_custom_name);
    }

    public static LogicPorts.Port OutputPort(
      HashedString id,
      CellOffset cell_offset,
      string description,
      string activeDescription,
      string inactiveDescription,
      bool show_wire_missing_icon = false,
      bool display_custom_name = false)
    {
      return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.Output, display_custom_name);
    }

    public static LogicPorts.Port RibbonInputPort(
      HashedString id,
      CellOffset cell_offset,
      string description,
      string activeDescription,
      string inactiveDescription,
      bool show_wire_missing_icon = false,
      bool display_custom_name = false)
    {
      return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.RibbonInput, display_custom_name);
    }

    public static LogicPorts.Port RibbonOutputPort(
      HashedString id,
      CellOffset cell_offset,
      string description,
      string activeDescription,
      string inactiveDescription,
      bool show_wire_missing_icon = false,
      bool display_custom_name = false)
    {
      return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.RibbonOutput, display_custom_name);
    }
  }
}
