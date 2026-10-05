// Decompiled with JetBrains decompiler
// Type: BrushTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BrushTool : InterfaceTool
{
  [SerializeField]
  private Texture2D brushCursor;
  [SerializeField]
  private GameObject areaVisualizer;
  [SerializeField]
  private Color32 areaColour = (Color32) new Color(1f, 1f, 1f, 0.5f);
  protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);
  protected Vector3 placementPivot;
  protected bool interceptNumberKeysForPriority;
  protected List<Vector2> brushOffsets = new List<Vector2>();
  protected bool affectFoundation;
  private bool dragging;
  protected int brushRadius = -1;
  private BrushTool.DragAxis dragAxis = BrushTool.DragAxis.Invalid;
  protected Vector3 downPos;
  protected int currentCell;
  protected int lastCell;
  protected List<int> visitedCells = new List<int>();
  protected HashSet<int> cellsInRadius = new HashSet<int>();

  public bool Dragging => this.dragging;

  protected virtual void PlaySound()
  {
  }

  protected virtual void clearVisitedCells() => this.visitedCells.Clear();

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    this.dragging = false;
  }

  public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
  {
    colors = new HashSet<ToolMenu.CellColorData>();
    foreach (int cellsInRadiu in this.cellsInRadius)
      colors.Add(new ToolMenu.CellColorData(cellsInRadiu, this.radiusIndicatorColor));
  }

  public virtual void SetBrushSize(int radius)
  {
    if (radius == this.brushRadius)
      return;
    this.brushRadius = radius;
    this.brushOffsets.Clear();
    for (int x = 0; x < this.brushRadius * 2; ++x)
    {
      for (int y = 0; y < this.brushRadius * 2; ++y)
      {
        if ((double) Vector2.Distance(new Vector2((float) x, (float) y), new Vector2((float) this.brushRadius, (float) this.brushRadius)) < (double) this.brushRadius - 0.800000011920929)
          this.brushOffsets.Add(new Vector2((float) (x - this.brushRadius), (float) (y - this.brushRadius)));
      }
    }
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    KScreenManager.Instance.SetEventSystemEnabled(true);
    if (KInputManager.currentControllerIsGamepad)
      this.SetCurrentVirtualInputModuleMousMovementMode(false);
    base.OnDeactivateTool(new_tool);
  }

  protected override void OnPrefabInit()
  {
    Game.Instance.Subscribe(1634669191, new Action<object>(this.OnTutorialOpened));
    base.OnPrefabInit();
    if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
      this.visualizer = Util.KInstantiate(this.visualizer);
    if (!((UnityEngine.Object) this.areaVisualizer != (UnityEngine.Object) null))
      return;
    this.areaVisualizer = Util.KInstantiate(this.areaVisualizer);
    this.areaVisualizer.SetActive(false);
    this.areaVisualizer.GetComponent<RectTransform>().SetParent(this.transform);
    this.areaVisualizer.GetComponent<Renderer>().material.color = (Color) this.areaColour;
  }

  protected override void OnCmpEnable() => this.dragging = false;

  protected override void OnCmpDisable()
  {
    if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
      this.visualizer.SetActive(false);
    if (!((UnityEngine.Object) this.areaVisualizer != (UnityEngine.Object) null))
      return;
    this.areaVisualizer.SetActive(false);
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    cursor_pos -= this.placementPivot;
    this.dragging = true;
    this.downPos = cursor_pos;
    if (!KInputManager.currentControllerIsGamepad)
      KScreenManager.Instance.SetEventSystemEnabled(false);
    else
      this.SetCurrentVirtualInputModuleMousMovementMode(true);
    this.Paint();
  }

  public override void OnLeftClickUp(Vector3 cursor_pos)
  {
    cursor_pos -= this.placementPivot;
    KScreenManager.Instance.SetEventSystemEnabled(true);
    if (KInputManager.currentControllerIsGamepad)
      this.SetCurrentVirtualInputModuleMousMovementMode(false);
    if (!this.dragging)
      return;
    this.dragging = false;
    switch (this.dragAxis)
    {
      case BrushTool.DragAxis.Horizontal:
        cursor_pos.y = this.downPos.y;
        this.dragAxis = BrushTool.DragAxis.None;
        break;
      case BrushTool.DragAxis.Vertical:
        cursor_pos.x = this.downPos.x;
        this.dragAxis = BrushTool.DragAxis.None;
        break;
    }
  }

  protected virtual string GetConfirmSound() => "Tile_Confirm";

  protected virtual string GetDragSound() => "Tile_Drag";

  public override string GetDeactivateSound() => "Tile_Cancel";

  private static int GetGridDistance(int cell, int center_cell)
  {
    Vector2I vector2I = Grid.CellToXY(cell) - Grid.CellToXY(center_cell);
    return Math.Abs(vector2I.x) + Math.Abs(vector2I.y);
  }

  private void Paint()
  {
    int count = this.visitedCells.Count;
    foreach (int cellsInRadiu in this.cellsInRadius)
    {
      if (Grid.IsValidCell(cellsInRadiu) && (int) Grid.WorldIdx[cellsInRadiu] == ClusterManager.Instance.activeWorldId && (!Grid.Foundation[cellsInRadiu] || this.affectFoundation))
        this.OnPaintCell(cellsInRadiu, Grid.GetCellDistance(this.currentCell, cellsInRadiu));
    }
    if (this.lastCell != this.currentCell)
      this.PlayDragSound();
    if (count >= this.visitedCells.Count)
      return;
    this.PlaySound();
  }

  protected virtual void PlayDragSound()
  {
    string dragSound = this.GetDragSound();
    if (string.IsNullOrEmpty(dragSound))
      return;
    string sound = GlobalAssets.GetSound(dragSound);
    if (sound == null)
      return;
    Vector3 pos = Grid.CellToPos(this.currentCell) with
    {
      z = 0.0f
    };
    int cellDistance = Grid.GetCellDistance(Grid.PosToCell(this.downPos), this.currentCell);
    EventInstance instance = SoundEvent.BeginOneShot(sound, pos);
    int num = (int) instance.setParameterByName("tileCount", (float) cellDistance);
    SoundEvent.EndOneShot(instance);
  }

  public override void OnMouseMove(Vector3 cursorPos)
  {
    this.currentCell = Grid.PosToCell(cursorPos);
    base.OnMouseMove(cursorPos);
    this.cellsInRadius.Clear();
    foreach (Vector2 brushOffset in this.brushOffsets)
    {
      int cell = Grid.OffsetCell(Grid.PosToCell(cursorPos), new CellOffset((int) brushOffset.x, (int) brushOffset.y));
      if (Grid.IsValidCell(cell) && (int) Grid.WorldIdx[cell] == ClusterManager.Instance.activeWorldId)
        this.cellsInRadius.Add(Grid.OffsetCell(Grid.PosToCell(cursorPos), new CellOffset((int) brushOffset.x, (int) brushOffset.y)));
    }
    if (!this.dragging)
      return;
    this.Paint();
    this.lastCell = this.currentCell;
  }

  protected virtual void OnPaintCell(int cell, int distFromOrigin)
  {
    if (this.visitedCells.Contains(cell))
      return;
    this.visitedCells.Add(cell);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.DragStraight))
      this.dragAxis = BrushTool.DragAxis.None;
    else if (this.interceptNumberKeysForPriority)
      this.HandlePriortyKeysDown(e);
    if (e.Consumed)
      return;
    base.OnKeyDown(e);
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (e.TryConsume(Action.DragStraight))
      this.dragAxis = BrushTool.DragAxis.Invalid;
    else if (this.interceptNumberKeysForPriority)
      this.HandlePriorityKeysUp(e);
    if (e.Consumed)
      return;
    base.OnKeyUp(e);
  }

  private void HandlePriortyKeysDown(KButtonEvent e)
  {
    Action action = e.GetAction();
    if (Action.Plan1 > action || action > Action.Plan10 || !e.TryConsume(action))
      return;
    int priority_value = (int) (action - 36 + 1);
    if (priority_value <= 9)
      ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, priority_value), true);
    else
      ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1), true);
  }

  private void HandlePriorityKeysUp(KButtonEvent e)
  {
    Action action = e.GetAction();
    if (Action.Plan1 > action || action > Action.Plan10)
      return;
    e.TryConsume(action);
  }

  public override void OnFocus(bool focus)
  {
    if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
      this.visualizer.SetActive(focus);
    this.hasFocus = focus;
    base.OnFocus(focus);
  }

  private void OnTutorialOpened(object data) => this.dragging = false;

  public override bool ShowHoverUI() => this.dragging || base.ShowHoverUI();

  public override void LateUpdate() => base.LateUpdate();

  private enum DragAxis
  {
    Invalid = -1, // 0xFFFFFFFF
    None = 0,
    Horizontal = 1,
    Vertical = 2,
  }
}
