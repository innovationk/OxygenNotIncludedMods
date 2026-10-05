// Decompiled with JetBrains decompiler
// Type: FilteredDragTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class FilteredDragTool : DragTool
{
  protected ToolParameterMenu.ToggleData[] currentFilters = new ToolParameterMenu.ToggleData[0];
  private ToolParameterMenu.ToggleData[] userSelectedFilters;
  private bool active;
  private HashedString lastAppliedOverlay;
  private bool isOverlayDriven;

  protected bool IsActive => this.active;

  private bool IsFilterOn(string name)
  {
    for (int index = 0; index < this.currentFilters.Length; ++index)
    {
      if (this.currentFilters[index].name == name)
        return this.currentFilters[index].IsOn;
    }
    return false;
  }

  public bool IsActiveLayer(string layer)
  {
    return this.IsFilterOn(ToolParameterMenu.FILTERLAYERS.ALL) || this.IsFilterOn(layer.ToUpper());
  }

  public bool IsActiveLayer(ObjectLayer layer)
  {
    if (this.IsFilterOn(ToolParameterMenu.FILTERLAYERS.ALL))
      return true;
    for (int index = 0; index < this.currentFilters.Length; ++index)
    {
      if (this.currentFilters[index].IsOn && this.GetObjectLayerFromFilterLayer(this.currentFilters[index].name) == layer)
        return true;
    }
    return false;
  }

  protected virtual void GetDefaultFilters(out ToolParameterMenu.ToggleData[] filters)
  {
    filters = new ToolParameterMenu.ToggleData[8]
    {
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.WIRES, ToolParameterMenu.ToggleState.Off),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.BUILDINGS, ToolParameterMenu.ToggleState.Off),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.LOGIC, ToolParameterMenu.ToggleState.Off),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.BACKWALL, ToolParameterMenu.ToggleState.Off)
    };
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.ResetFilter();
    this.userSelectedFilters = this.CloneFilters(this.currentFilters);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    OverlayScreen.Instance.OnOverlayChanged += new Action<HashedString>(this.OnOverlayChanged);
  }

  protected override void OnCleanUp()
  {
    OverlayScreen.Instance.OnOverlayChanged -= new Action<HashedString>(this.OnOverlayChanged);
    base.OnCleanUp();
  }

  public void ResetFilter() => this.GetDefaultFilters(out this.currentFilters);

  private ToolParameterMenu.ToggleData[] CloneFilters(ToolParameterMenu.ToggleData[] source)
  {
    ToolParameterMenu.ToggleData[] toggleDataArray = new ToolParameterMenu.ToggleData[source.Length];
    for (int index = 0; index < source.Length; ++index)
      toggleDataArray[index] = new ToolParameterMenu.ToggleData(source[index].name, source[index].state, source[index].isToggleInclusive);
    return toggleDataArray;
  }

  private void SaveUserFilters()
  {
    this.userSelectedFilters = this.CloneFilters(this.currentFilters);
  }

  private void RestoreUserFilters()
  {
    if (this.userSelectedFilters != null)
      this.currentFilters = this.CloneFilters(this.userSelectedFilters);
    else
      this.ResetFilter();
  }

  private void OnParametersChanged()
  {
    if (this.isOverlayDriven)
      return;
    this.SaveUserFilters();
  }

  protected override void OnActivateTool()
  {
    this.active = true;
    base.OnActivateTool();
    ToolMenu.Instance.toolParameterMenu.onParametersChanged += new System.Action(this.OnParametersChanged);
    HashedString mode = OverlayScreen.Instance.mode;
    if (mode != this.lastAppliedOverlay)
      this.OnOverlayChanged(mode);
    else
      ToolMenu.Instance.toolParameterMenu.PopulateMenu(this.currentFilters);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    this.active = false;
    ToolMenu.Instance.toolParameterMenu.onParametersChanged -= new System.Action(this.OnParametersChanged);
    ToolMenu.Instance.toolParameterMenu.ClearMenu();
    base.OnDeactivateTool(new_tool);
  }

  public virtual string GetFilterLayerFromGameObject(GameObject input)
  {
    BuildingComplete component1 = input.GetComponent<BuildingComplete>();
    BuildingUnderConstruction component2 = input.GetComponent<BuildingUnderConstruction>();
    if ((bool) (UnityEngine.Object) component1)
      return this.GetFilterLayerFromObjectLayer(component1.Def.ObjectLayer);
    if ((bool) (UnityEngine.Object) component2)
      return this.GetFilterLayerFromObjectLayer(component2.Def.ObjectLayer);
    if ((UnityEngine.Object) input.GetComponent<Clearable>() != (UnityEngine.Object) null || (UnityEngine.Object) input.GetComponent<Moppable>() != (UnityEngine.Object) null)
      return "CleanAndClear";
    return (UnityEngine.Object) input.GetComponent<Diggable>() != (UnityEngine.Object) null ? "DigPlacer" : "Default";
  }

  public string GetFilterLayerFromObjectLayer(ObjectLayer gamer_layer)
  {
    switch (gamer_layer)
    {
      case ObjectLayer.Building:
      case ObjectLayer.Gantry:
        return "Buildings";
      case ObjectLayer.Backwall:
        return "BackWall";
      case ObjectLayer.FoundationTile:
        return "Tiles";
      case ObjectLayer.GasConduit:
      case ObjectLayer.GasConduitConnection:
        return "GasPipes";
      case ObjectLayer.LiquidConduit:
      case ObjectLayer.LiquidConduitConnection:
        return "LiquidPipes";
      case ObjectLayer.SolidConduit:
      case ObjectLayer.SolidConduitConnection:
        return "SolidConduits";
      case ObjectLayer.Wire:
      case ObjectLayer.WireConnectors:
        return "Wires";
      case ObjectLayer.LogicGate:
      case ObjectLayer.LogicWire:
        return "Logic";
      default:
        return "Default";
    }
  }

  private ObjectLayer GetObjectLayerFromFilterLayer(string filter_layer)
  {
    switch (filter_layer.ToLower())
    {
      case "backwall":
        return ObjectLayer.Backwall;
      case "buildings":
        return ObjectLayer.Building;
      case "gaspipes":
        return ObjectLayer.GasConduit;
      case "liquidpipes":
        return ObjectLayer.LiquidConduit;
      case "logic":
        return ObjectLayer.LogicWire;
      case "solidconduits":
        return ObjectLayer.SolidConduit;
      case "tiles":
        return ObjectLayer.FoundationTile;
      case "wires":
        return ObjectLayer.Wire;
      default:
        throw new ArgumentException("Invalid filter layer: " + filter_layer);
    }
  }

  protected virtual void OnOverlayChanged(HashedString overlay)
  {
    if (!this.active || GameUtil.IsCapturingTimeLapse())
      return;
    this.lastAppliedOverlay = overlay;
    string str = (string) null;
    if (overlay == OverlayModes.Power.ID)
      str = ToolParameterMenu.FILTERLAYERS.WIRES;
    else if (overlay == OverlayModes.LiquidConduits.ID)
      str = ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT;
    else if (overlay == OverlayModes.GasConduits.ID)
      str = ToolParameterMenu.FILTERLAYERS.GASCONDUIT;
    else if (overlay == OverlayModes.SolidConveyor.ID)
      str = ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT;
    else if (overlay == OverlayModes.Logic.ID)
      str = ToolParameterMenu.FILTERLAYERS.LOGIC;
    if (str != null)
    {
      if (!this.isOverlayDriven)
        this.SaveUserFilters();
      this.isOverlayDriven = true;
      this.GetDefaultFilters(out this.currentFilters);
      for (int index = 0; index < this.currentFilters.Length; ++index)
        this.currentFilters[index].state = this.currentFilters[index].name == str ? ToolParameterMenu.ToggleState.On : ToolParameterMenu.ToggleState.Disabled;
    }
    else if (this.isOverlayDriven)
    {
      this.RestoreUserFilters();
      this.isOverlayDriven = false;
    }
    ToolMenu.Instance.toolParameterMenu.PopulateMenu(this.currentFilters);
  }
}
