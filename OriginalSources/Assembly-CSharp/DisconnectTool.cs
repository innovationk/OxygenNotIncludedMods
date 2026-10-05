// Decompiled with JetBrains decompiler
// Type: DisconnectTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DisconnectTool : FilteredDragTool
{
  [SerializeField]
  private GameObject disconnectVisSingleModePrefab;
  [SerializeField]
  private GameObject disconnectVisMultiModePrefab;
  private GameObjectPool disconnectVisPool;
  private List<DisconnectTool.VisData> visualizersInUse = new List<DisconnectTool.VisData>();
  private int lastRefreshedCell;
  private bool singleDisconnectMode = true;
  public static DisconnectTool Instance;

  public static void DestroyInstance() => DisconnectTool.Instance = (DisconnectTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DisconnectTool.Instance = this;
    this.disconnectVisPool = new GameObjectPool(new Func<GameObject>(this.InstantiateDisconnectVis), (Action<GameObject>) (_ => { }), this.singleDisconnectMode ? 1 : 10);
    if (!this.singleDisconnectMode)
      return;
    this.lineModeMaxLength = 2;
  }

  public void Activate() => PlayerController.Instance.ActivateTool((InterfaceTool) this);

  protected override DragTool.Mode GetMode()
  {
    return !this.singleDisconnectMode ? DragTool.Mode.Box : DragTool.Mode.Line;
  }

  protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
  {
    if (this.singleDisconnectMode)
      upPos = this.SnapToLine(upPos);
    this.RunOnRegion(downPos, upPos, new Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections>(this.DisconnectCellsAction));
    this.ClearVisualizers();
  }

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    this.lastRefreshedCell = -1;
  }

  private void DisconnectCellsAction(
    int cell,
    GameObject objectOnCell,
    IHaveUtilityNetworkMgr utilityComponent,
    UtilityConnections removeConnections)
  {
    Building component1 = objectOnCell.GetComponent<Building>();
    KAnimGraphTileVisualizer component2 = objectOnCell.GetComponent<KAnimGraphTileVisualizer>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      UtilityConnections new_connections = utilityComponent.GetNetworkManager().GetConnections(cell, false) & ~removeConnections;
      component2.UpdateConnections(new_connections);
      component2.Refresh();
    }
    TileVisualizer.RefreshCell(cell, component1.Def.TileLayer, component1.Def.ReplacementLayer);
    utilityComponent.GetNetworkManager().ForceRebuildNetworks();
  }

  private void RunOnRegion(
    Vector3 pos1,
    Vector3 pos2,
    Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections> action)
  {
    Vector2 regularizedPos1 = this.GetRegularizedPos(Vector2.Min((Vector2) pos1, (Vector2) pos2), true);
    Vector2 regularizedPos2 = this.GetRegularizedPos(Vector2.Max((Vector2) pos1, (Vector2) pos2), false);
    Vector2I min = new Vector2I((int) regularizedPos1.x, (int) regularizedPos1.y);
    Vector2I max = new Vector2I((int) regularizedPos2.x, (int) regularizedPos2.y);
    for (int x = min.x; x < max.x; ++x)
    {
      for (int y = min.y; y < max.y; ++y)
      {
        int cell = Grid.XYToCell(x, y);
        if (Grid.IsVisible(cell))
        {
          for (int layer = 0; layer < 45; ++layer)
          {
            GameObject input = Grid.Objects[cell, layer];
            if (!((UnityEngine.Object) input == (UnityEngine.Object) null) && this.IsActiveLayer(this.GetFilterLayerFromGameObject(input)))
            {
              Building component1 = input.GetComponent<Building>();
              if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null))
              {
                IHaveUtilityNetworkMgr component2 = component1.Def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>();
                if (!component2.IsNullOrDestroyed())
                {
                  int connections = (int) component2.GetNetworkManager().GetConnections(cell, false);
                  UtilityConnections utilityConnections = (UtilityConnections) 0;
                  if ((connections & 1) > 0 && this.IsInsideRegion(min, max, cell, -1, 0))
                    utilityConnections |= UtilityConnections.Left;
                  if ((connections & 2) > 0 && this.IsInsideRegion(min, max, cell, 1, 0))
                    utilityConnections |= UtilityConnections.Right;
                  if ((connections & 4) > 0 && this.IsInsideRegion(min, max, cell, 0, 1))
                    utilityConnections |= UtilityConnections.Up;
                  if ((connections & 8) > 0 && this.IsInsideRegion(min, max, cell, 0, -1))
                    utilityConnections |= UtilityConnections.Down;
                  if (utilityConnections > (UtilityConnections) 0)
                    action(cell, input, component2, utilityConnections);
                }
              }
            }
          }
        }
      }
    }
  }

  private bool IsInsideRegion(Vector2I min, Vector2I max, int cell, int xoff, int yoff)
  {
    int x;
    int y;
    Grid.CellToXY(Grid.OffsetCell(cell, xoff, yoff), out x, out y);
    return x >= min.x && x < max.x && y >= min.y && y < max.y;
  }

  public override void OnMouseMove(Vector3 cursorPos)
  {
    base.OnMouseMove(cursorPos);
    if (!this.Dragging)
      return;
    cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
    if (this.singleDisconnectMode)
      cursorPos = this.SnapToLine(cursorPos);
    int cell = Grid.PosToCell(cursorPos);
    if (this.lastRefreshedCell == cell)
      return;
    this.lastRefreshedCell = cell;
    this.ClearVisualizers();
    this.RunOnRegion(this.downPos, cursorPos, new Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections>(this.VisualizeAction));
  }

  private GameObject InstantiateDisconnectVis()
  {
    GameObject gameObject = GameUtil.KInstantiate(this.singleDisconnectMode ? this.disconnectVisSingleModePrefab : this.disconnectVisMultiModePrefab, Grid.SceneLayer.FXFront);
    gameObject.SetActive(false);
    return gameObject;
  }

  private void VisualizeAction(
    int cell,
    GameObject objectOnCell,
    IHaveUtilityNetworkMgr utilityComponent,
    UtilityConnections removeConnections)
  {
    if ((removeConnections & UtilityConnections.Down) != (UtilityConnections) 0)
      this.CreateVisualizer(cell, Grid.CellBelow(cell), true);
    if ((removeConnections & UtilityConnections.Right) == (UtilityConnections) 0)
      return;
    this.CreateVisualizer(cell, Grid.CellRight(cell), false);
  }

  private void CreateVisualizer(int cell1, int cell2, bool rotate)
  {
    foreach (DisconnectTool.VisData visData in this.visualizersInUse)
    {
      if (visData.Equals(cell1, cell2))
        return;
    }
    Vector3 posCcc1 = Grid.CellToPosCCC(cell1, Grid.SceneLayer.FXFront);
    Vector3 posCcc2 = Grid.CellToPosCCC(cell2, Grid.SceneLayer.FXFront);
    GameObject instance = this.disconnectVisPool.GetInstance();
    instance.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotate ? 90f : 0.0f);
    instance.transform.SetPosition(Vector3.Lerp(posCcc1, posCcc2, 0.5f));
    instance.SetActive(true);
    this.visualizersInUse.Add(new DisconnectTool.VisData(cell1, cell2, instance));
  }

  private void ClearVisualizers()
  {
    foreach (DisconnectTool.VisData visData in this.visualizersInUse)
    {
      visData.go.SetActive(false);
      this.disconnectVisPool.ReleaseInstance(visData.go);
    }
    this.visualizersInUse.Clear();
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    this.ClearVisualizers();
  }

  protected override string GetConfirmSound() => "OutletDisconnected";

  protected override string GetDragSound() => "Tile_Drag_NegativeTool";

  protected override void GetDefaultFilters(out ToolParameterMenu.ToggleData[] filters)
  {
    filters = new ToolParameterMenu.ToggleData[7]
    {
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.WIRES, ToolParameterMenu.ToggleState.Off),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.BUILDINGS, ToolParameterMenu.ToggleState.Off),
      new ToolParameterMenu.ToggleData(ToolParameterMenu.FILTERLAYERS.LOGIC, ToolParameterMenu.ToggleState.Off)
    };
  }

  public struct VisData(int cell1, int cell2, GameObject go)
  {
    public readonly int cell1 = cell1;
    public readonly int cell2 = cell2;
    public GameObject go = go;

    public bool Equals(int cell1, int cell2)
    {
      if (this.cell1 == cell1 && this.cell2 == cell2)
        return true;
      return this.cell1 == cell2 && this.cell2 == cell1;
    }
  }
}
