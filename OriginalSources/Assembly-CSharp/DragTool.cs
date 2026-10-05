// Decompiled with JetBrains decompiler
// Type: DragTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using STRINGS;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class DragTool : InterfaceTool
{
  [SerializeField]
  private Texture2D boxCursor;
  [SerializeField]
  private GameObject areaVisualizer;
  [SerializeField]
  private GameObject areaVisualizerTextPrefab;
  [SerializeField]
  private Color32 areaColour = (Color32) new Color(1f, 1f, 1f, 0.5f);
  protected SpriteRenderer areaVisualizerSpriteRenderer;
  protected Guid areaVisualizerText;
  protected Vector3 placementPivot;
  protected bool interceptNumberKeysForPriority;
  private bool dragging;
  private Vector3 previousCursorPos;
  private DragTool.Mode mode = DragTool.Mode.Box;
  private DragTool.DragAxis dragAxis = DragTool.DragAxis.Invalid;
  protected bool canChangeDragAxis = true;
  protected int lineModeMaxLength = -1;
  protected Vector3 downPos;
  private bool cellChangedSinceDown;
  private VirtualInputModule currentVirtualInputInUse;

  public bool Dragging => this.dragging;

  protected virtual DragTool.Mode GetMode() => this.mode;

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    this.dragging = false;
    this.SetMode(this.mode);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    if ((UnityEngine.Object) KScreenManager.Instance != (UnityEngine.Object) null)
      KScreenManager.Instance.SetEventSystemEnabled(true);
    if (KInputManager.currentControllerIsGamepad)
      this.SetCurrentVirtualInputModuleMousMovementMode(false);
    this.RemoveCurrentAreaText();
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
    this.areaVisualizerSpriteRenderer = this.areaVisualizer.GetComponent<SpriteRenderer>();
    this.areaVisualizer.transform.SetParent(this.transform);
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
    cursor_pos = this.ClampPositionToWorld(cursor_pos, ClusterManager.Instance.activeWorld);
    this.dragging = true;
    this.downPos = cursor_pos;
    this.cellChangedSinceDown = false;
    this.previousCursorPos = cursor_pos;
    if ((UnityEngine.Object) this.currentVirtualInputInUse != (UnityEngine.Object) null)
    {
      this.currentVirtualInputInUse.mouseMovementOnly = false;
      this.currentVirtualInputInUse = (VirtualInputModule) null;
    }
    if (!KInputManager.currentControllerIsGamepad)
    {
      KScreenManager.Instance.SetEventSystemEnabled(false);
    }
    else
    {
      UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
      this.SetCurrentVirtualInputModuleMousMovementMode(true, (Action<VirtualInputModule>) (module => this.currentVirtualInputInUse = module));
    }
    this.hasFocus = true;
    this.RemoveCurrentAreaText();
    if ((UnityEngine.Object) this.areaVisualizerTextPrefab != (UnityEngine.Object) null)
    {
      this.areaVisualizerText = NameDisplayScreen.Instance.AddAreaText("", this.areaVisualizerTextPrefab);
      NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>().color = (Color) this.areaColour;
    }
    switch (this.GetMode())
    {
      case DragTool.Mode.Brush:
        if (!((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null))
          break;
        this.AddDragPoint(cursor_pos);
        break;
      case DragTool.Mode.Box:
      case DragTool.Mode.Line:
        if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
          this.visualizer.SetActive(false);
        if (!((UnityEngine.Object) this.areaVisualizer != (UnityEngine.Object) null))
          break;
        this.areaVisualizer.SetActive(true);
        this.areaVisualizer.transform.SetPosition(cursor_pos);
        this.areaVisualizerSpriteRenderer.size = new Vector2(0.01f, 0.01f);
        break;
    }
  }

  public void RemoveCurrentAreaText()
  {
    if (!(this.areaVisualizerText != Guid.Empty))
      return;
    NameDisplayScreen.Instance.RemoveWorldText(this.areaVisualizerText);
    this.areaVisualizerText = Guid.Empty;
  }

  public void CancelDragging()
  {
    KScreenManager.Instance.SetEventSystemEnabled(true);
    if ((UnityEngine.Object) this.currentVirtualInputInUse != (UnityEngine.Object) null)
    {
      this.currentVirtualInputInUse.mouseMovementOnly = false;
      this.currentVirtualInputInUse = (VirtualInputModule) null;
    }
    if (KInputManager.currentControllerIsGamepad)
      this.SetCurrentVirtualInputModuleMousMovementMode(false);
    this.dragAxis = DragTool.DragAxis.Invalid;
    if (!this.dragging)
      return;
    this.dragging = false;
    this.RemoveCurrentAreaText();
    switch (this.GetMode())
    {
      case DragTool.Mode.Box:
      case DragTool.Mode.Line:
        if (!((UnityEngine.Object) this.areaVisualizer != (UnityEngine.Object) null))
          break;
        this.areaVisualizer.SetActive(false);
        break;
    }
  }

  public override void OnLeftClickUp(Vector3 cursor_pos)
  {
    KScreenManager.Instance.SetEventSystemEnabled(true);
    if ((UnityEngine.Object) this.currentVirtualInputInUse != (UnityEngine.Object) null)
    {
      this.currentVirtualInputInUse.mouseMovementOnly = false;
      this.currentVirtualInputInUse = (VirtualInputModule) null;
    }
    if (KInputManager.currentControllerIsGamepad)
      this.SetCurrentVirtualInputModuleMousMovementMode(false);
    this.dragAxis = DragTool.DragAxis.Invalid;
    if (!this.dragging)
      return;
    this.dragging = false;
    cursor_pos = this.ClampPositionToWorld(cursor_pos, ClusterManager.Instance.activeWorld);
    this.RemoveCurrentAreaText();
    DragTool.Mode mode = this.GetMode();
    if (mode == DragTool.Mode.Line || Input.GetKey((KeyCode) Global.GetInputManager().GetDefaultController().GetInputForAction(Action.DragStraight)))
      cursor_pos = this.SnapToLine(cursor_pos);
    if (mode != DragTool.Mode.Box && mode != DragTool.Mode.Line || !((UnityEngine.Object) this.areaVisualizer != (UnityEngine.Object) null))
      return;
    this.areaVisualizer.SetActive(false);
    int x1;
    int y1;
    Grid.PosToXY(this.downPos, out x1, out y1);
    int num1 = x1;
    int num2 = y1;
    int x2;
    int y2;
    Grid.PosToXY(cursor_pos, out x2, out y2);
    if (x2 < x1)
      Util.Swap<int>(ref x1, ref x2);
    if (y2 < y1)
      Util.Swap<int>(ref y1, ref y2);
    for (int y3 = y1; y3 <= y2; ++y3)
    {
      for (int x3 = x1; x3 <= x2; ++x3)
      {
        int cell = Grid.XYToCell(x3, y3);
        if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
        {
          int num3 = y3 - num2;
          int num4 = x3 - num1;
          int num5 = Mathf.Abs(num3);
          int num6 = Mathf.Abs(num4);
          this.OnDragTool(cell, num5 + num6);
        }
      }
    }
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.GetConfirmSound()));
    this.OnDragComplete(this.downPos, cursor_pos);
  }

  protected virtual string GetConfirmSound() => "Tile_Confirm";

  protected virtual string GetDragSound() => "Tile_Drag";

  public override string GetDeactivateSound() => "Tile_Cancel";

  protected Vector3 ClampPositionToWorld(Vector3 position, WorldContainer world)
  {
    position.x = Mathf.Clamp(position.x, world.minimumBounds.x, world.maximumBounds.x);
    position.y = Mathf.Clamp(position.y, world.minimumBounds.y, world.maximumBounds.y);
    return position;
  }

  protected Vector3 SnapToLine(Vector3 cursorPos)
  {
    Vector3 vector3 = cursorPos - this.downPos;
    if (this.canChangeDragAxis || !this.canChangeDragAxis && !this.cellChangedSinceDown || this.dragAxis == DragTool.DragAxis.Invalid)
    {
      this.dragAxis = DragTool.DragAxis.Invalid;
      this.dragAxis = (double) Mathf.Abs(vector3.x) >= (double) Mathf.Abs(vector3.y) ? DragTool.DragAxis.Horizontal : DragTool.DragAxis.Vertical;
    }
    switch (this.dragAxis)
    {
      case DragTool.DragAxis.Horizontal:
        cursorPos.y = this.downPos.y;
        if (this.lineModeMaxLength != -1 && (double) Mathf.Abs(vector3.x) > (double) (this.lineModeMaxLength - 1))
        {
          cursorPos.x = this.downPos.x + Mathf.Sign(vector3.x) * (float) (this.lineModeMaxLength - 1);
          break;
        }
        break;
      case DragTool.DragAxis.Vertical:
        cursorPos.x = this.downPos.x;
        if (this.lineModeMaxLength != -1 && (double) Mathf.Abs(vector3.y) > (double) (this.lineModeMaxLength - 1))
        {
          cursorPos.y = this.downPos.y + Mathf.Sign(vector3.y) * (float) (this.lineModeMaxLength - 1);
          break;
        }
        break;
    }
    return cursorPos;
  }

  public override void OnMouseMove(Vector3 cursorPos)
  {
    cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
    if (this.dragging && (Input.GetKey((KeyCode) Global.GetInputManager().GetDefaultController().GetInputForAction(Action.DragStraight)) || this.GetMode() == DragTool.Mode.Line))
      cursorPos = this.SnapToLine(cursorPos);
    else
      this.dragAxis = DragTool.DragAxis.Invalid;
    base.OnMouseMove(cursorPos);
    if (!this.dragging)
      return;
    if (Grid.PosToCell(cursorPos) != Grid.PosToCell(this.downPos))
      this.cellChangedSinceDown = true;
    switch (this.GetMode())
    {
      case DragTool.Mode.Brush:
        this.AddDragPoints(cursorPos, this.previousCursorPos);
        if (this.areaVisualizerText != Guid.Empty)
        {
          int dragLength = this.GetDragLength();
          LocText component = NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>();
          component.text = string.Format((string) UI.TOOLS.TOOL_LENGTH_FMT, (object) dragLength);
          component.transform.SetPosition(Grid.CellToPos(Grid.PosToCell(cursorPos)) + new Vector3(0.0f, 1f, 0.0f));
          break;
        }
        break;
      case DragTool.Mode.Box:
      case DragTool.Mode.Line:
        Vector2 input1 = (Vector2) Vector3.Max(this.downPos, cursorPos);
        Vector2 input2 = (Vector2) Vector3.Min(this.downPos, cursorPos);
        Vector2 restrictedPosition1 = this.GetWorldRestrictedPosition(input1);
        Vector2 restrictedPosition2 = this.GetWorldRestrictedPosition(input2);
        Vector2 regularizedPos1 = this.GetRegularizedPos(restrictedPosition1, false);
        Vector2 regularizedPos2 = this.GetRegularizedPos(restrictedPosition2, true);
        Vector2 vector2 = regularizedPos1 - regularizedPos2;
        Vector2 position1 = (regularizedPos1 + regularizedPos2) * 0.5f;
        this.areaVisualizer.transform.SetPosition((Vector3) new Vector2(position1.x, position1.y));
        int num1 = (int) ((double) regularizedPos1.x - (double) regularizedPos2.x + ((double) regularizedPos1.y - (double) regularizedPos2.y) - 1.0);
        if (this.areaVisualizerSpriteRenderer.size != vector2)
        {
          string sound = GlobalAssets.GetSound(this.GetDragSound());
          if (sound != null)
          {
            Vector3 position2 = this.areaVisualizer.transform.GetPosition() with
            {
              z = 0.0f
            };
            EventInstance instance = SoundEvent.BeginOneShot(sound, position2);
            int num2 = (int) instance.setParameterByName("tileCount", (float) num1);
            SoundEvent.EndOneShot(instance);
          }
        }
        this.areaVisualizerSpriteRenderer.size = vector2;
        if (this.areaVisualizerText != Guid.Empty)
        {
          Vector2I vector2I = new Vector2I(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
          LocText component = NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>();
          component.text = string.Format((string) UI.TOOLS.TOOL_AREA_FMT, (object) vector2I.x, (object) vector2I.y, (object) (vector2I.x * vector2I.y));
          component.transform.SetPosition((Vector3) position1);
          break;
        }
        break;
    }
    this.previousCursorPos = cursorPos;
  }

  protected virtual void OnDragTool(int cell, int distFromOrigin)
  {
  }

  protected virtual void OnDragComplete(Vector3 cursorDown, Vector3 cursorUp)
  {
  }

  protected virtual int GetDragLength() => 0;

  private void AddDragPoint(Vector3 cursorPos)
  {
    cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
    int cell = Grid.PosToCell(cursorPos);
    if (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell))
      return;
    this.OnDragTool(cell, 0);
  }

  private void AddDragPoints(Vector3 cursorPos, Vector3 previousCursorPos)
  {
    cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
    Vector3 vector3 = cursorPos - previousCursorPos;
    float magnitude = vector3.magnitude;
    float num1 = Grid.CellSizeInMeters * 0.25f;
    int num2 = 1 + (int) ((double) magnitude / (double) num1);
    vector3.Normalize();
    for (int index = 0; index < num2; ++index)
      this.AddDragPoint(previousCursorPos + vector3 * ((float) index * num1));
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.interceptNumberKeysForPriority)
      this.HandlePriortyKeysDown(e);
    if (e.Consumed)
      return;
    base.OnKeyDown(e);
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (this.interceptNumberKeysForPriority)
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

  protected void SetMode(DragTool.Mode newMode)
  {
    this.mode = newMode;
    switch (this.mode)
    {
      case DragTool.Mode.Brush:
        if ((UnityEngine.Object) this.areaVisualizer != (UnityEngine.Object) null)
          this.areaVisualizer.SetActive(false);
        if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
          this.visualizer.SetActive(true);
        this.SetCursor(this.cursor, this.cursorOffset, CursorMode.Auto);
        break;
      case DragTool.Mode.Box:
        if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
          this.visualizer.SetActive(true);
        this.mode = DragTool.Mode.Box;
        this.SetCursor(this.boxCursor, this.cursorOffset, CursorMode.Auto);
        break;
      case DragTool.Mode.Line:
        if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
          this.visualizer.SetActive(true);
        this.mode = DragTool.Mode.Line;
        this.SetCursor(this.boxCursor, this.cursorOffset, CursorMode.Auto);
        break;
    }
  }

  public override void OnFocus(bool focus)
  {
    switch (this.GetMode())
    {
      case DragTool.Mode.Brush:
        if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
          this.visualizer.SetActive(focus);
        this.hasFocus = focus;
        break;
      case DragTool.Mode.Box:
      case DragTool.Mode.Line:
        if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null && !this.dragging)
          this.visualizer.SetActive(focus);
        this.hasFocus = focus || this.dragging;
        break;
    }
  }

  private void OnTutorialOpened(object data) => this.dragging = false;

  public override bool ShowHoverUI() => this.dragging || base.ShowHoverUI();

  private enum DragAxis
  {
    Invalid = -1, // 0xFFFFFFFF
    None = 0,
    Horizontal = 1,
    Vertical = 2,
  }

  public enum Mode
  {
    Brush,
    Box,
    Line,
  }
}
