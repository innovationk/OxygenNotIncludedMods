// Decompiled with JetBrains decompiler
// Type: PlayerController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.Input;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/PlayerController")]
public class PlayerController : KMonoBehaviour, IInputHandler
{
  [SerializeField]
  private Action defaultConfigKey;
  [SerializeField]
  private List<InterfaceToolConfig> interfaceConfigs;
  public InterfaceTool[] tools;
  private InterfaceTool activeTool;
  public VirtualInputModule vim;
  private bool DebugHidingCursor;
  private Vector3 prevMousePos = new Vector3(float.PositiveInfinity, 0.0f, 0.0f);
  private const float MIN_DRAG_DIST_SQR = 36f;
  private const float MIN_DRAG_TIME = 0.3f;
  private Action dragAction;
  private bool draggingAllowed = true;
  private bool dragging;
  private bool queueStopDrag;
  private Vector3 startDragPos;
  private float startDragTime;
  private Vector3 dragDelta;
  private Vector3 worldDragDelta;

  public string handlerName => nameof (PlayerController);

  public KInputHandler inputHandler { get; set; }

  public InterfaceTool ActiveTool => this.activeTool;

  public static PlayerController Instance { get; private set; }

  public static void DestroyInstance() => PlayerController.Instance = (PlayerController) null;

  protected override void OnPrefabInit()
  {
    PlayerController.Instance = this;
    InterfaceTool.InitializeConfigs(this.defaultConfigKey, this.interfaceConfigs);
    this.vim = Object.FindFirstObjectByType<VirtualInputModule>(FindObjectsInactive.Include);
    for (int index = 0; index < this.tools.Length; ++index)
    {
      GameObject gameObject = Util.KInstantiate(this.tools[index].gameObject, this.gameObject);
      this.tools[index] = gameObject.GetComponent<InterfaceTool>();
      this.tools[index].gameObject.SetActive(true);
      this.tools[index].gameObject.SetActive(false);
    }
  }

  protected override void OnSpawn()
  {
    if (this.tools.Length == 0)
      return;
    this.ActivateTool(this.tools[0]);
  }

  private void InitializeConfigs()
  {
  }

  private Vector3 GetCursorPos() => PlayerController.GetCursorPos(KInputManager.GetMousePos());

  public static Vector3 GetCursorPos(Vector3 mouse_pos)
  {
    RaycastHit hitInfo;
    Vector3 cursorPos;
    if (Physics.Raycast(Camera.main.ScreenPointToRay(mouse_pos), out hitInfo, float.PositiveInfinity, Game.BlockSelectionLayerMask))
    {
      cursorPos = hitInfo.point;
    }
    else
    {
      mouse_pos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
      cursorPos = Camera.main.ScreenToWorldPoint(mouse_pos);
    }
    float x = cursorPos.x;
    float y = cursorPos.y;
    float num1 = Mathf.Min(Mathf.Max(x, 0.0f), Grid.WidthInMeters);
    float num2 = Mathf.Min(Mathf.Max(y, 0.0f), Grid.HeightInMeters);
    cursorPos.x = num1;
    cursorPos.y = num2;
    return cursorPos;
  }

  private void UpdateHover()
  {
    UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
    if (!((Object) current != (Object) null))
      return;
    this.activeTool.OnFocus(!current.IsPointerOverGameObject());
  }

  private void Update()
  {
    this.UpdateDrag();
    if ((bool) (Object) this.activeTool && this.activeTool.enabled)
    {
      this.UpdateHover();
      Vector3 cursorPos = this.GetCursorPos();
      if (cursorPos != this.prevMousePos)
      {
        this.prevMousePos = cursorPos;
        this.activeTool.OnMouseMove(cursorPos);
      }
    }
    if (!UnityEngine.Input.GetKeyDown(KeyCode.F12) || !UnityEngine.Input.GetKey(KeyCode.LeftAlt) && !UnityEngine.Input.GetKey(KeyCode.RightAlt))
      return;
    this.DebugHidingCursor = !this.DebugHidingCursor;
    Cursor.visible = !this.DebugHidingCursor;
    HoverTextScreen.Instance.Show(!this.DebugHidingCursor);
  }

  private void OnCleanup() => Global.GetInputManager().usedMenus.Remove((IInputHandler) this);

  private void LateUpdate()
  {
    if (!this.queueStopDrag)
      return;
    this.queueStopDrag = false;
    this.dragging = false;
    this.dragAction = Action.Invalid;
    this.dragDelta = Vector3.zero;
    this.worldDragDelta = Vector3.zero;
  }

  public void ActivateTool(InterfaceTool tool)
  {
    if ((Object) this.activeTool == (Object) tool)
      return;
    this.DeactivateTool(tool);
    this.activeTool = tool;
    this.activeTool.enabled = true;
    this.activeTool.gameObject.SetActive(true);
    this.activeTool.ActivateTool();
    this.UpdateHover();
  }

  public void ToolDeactivated(InterfaceTool tool)
  {
    if ((Object) this.activeTool == (Object) tool && (Object) this.activeTool != (Object) null)
      this.DeactivateTool();
    if (!((Object) this.activeTool == (Object) null))
      return;
    this.ActivateTool((InterfaceTool) SelectTool.Instance);
  }

  private void DeactivateTool(InterfaceTool new_tool = null)
  {
    if (!((Object) this.activeTool != (Object) null))
      return;
    this.activeTool.enabled = false;
    this.activeTool.gameObject.SetActive(false);
    InterfaceTool activeTool = this.activeTool;
    this.activeTool = (InterfaceTool) null;
    InterfaceTool new_tool1 = new_tool;
    activeTool.DeactivateTool(new_tool1);
  }

  public bool IsUsingDefaultTool()
  {
    return this.tools.Length != 0 && (Object) this.activeTool == (Object) this.tools[0];
  }

  private void StartDrag(Action action)
  {
    if (this.dragAction != Action.Invalid)
      return;
    this.dragAction = action;
    this.startDragPos = KInputManager.GetMousePos();
    this.startDragTime = Time.unscaledTime;
  }

  private void UpdateDrag()
  {
    this.dragDelta = (Vector3) Vector2.zero;
    Vector3 mousePos = KInputManager.GetMousePos();
    if (!this.dragging && this.CanDrag() && ((double) (mousePos - this.startDragPos).sqrMagnitude > 36.0 || (double) Time.unscaledTime - (double) this.startDragTime > 0.30000001192092896))
      this.dragging = true;
    if (DistributionPlatform.Initialized && KInputManager.currentControllerIsGamepad && this.dragging || !this.dragging)
      return;
    this.dragDelta = mousePos - this.startDragPos;
    this.worldDragDelta = Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.ScreenToWorldPoint(this.startDragPos);
    this.startDragPos = mousePos;
  }

  private void StopDrag(Action action)
  {
    if (this.dragAction != action)
      return;
    this.queueStopDrag = true;
    if (!KInputManager.currentControllerIsGamepad)
      return;
    this.dragging = false;
  }

  public void CancelDragging()
  {
    this.queueStopDrag = true;
    if (!((Object) this.activeTool != (Object) null))
      return;
    DragTool activeTool = this.activeTool as DragTool;
    if (!((Object) activeTool != (Object) null))
      return;
    activeTool.CancelDragging();
  }

  public void OnCancelInput() => this.CancelDragging();

  public void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.ToggleScreenshotMode))
      DebugHandler.ToggleScreenshotMode();
    else if (DebugHandler.HideUI && e.TryConsume(Action.Escape))
    {
      DebugHandler.ToggleScreenshotMode();
    }
    else
    {
      bool flag = true;
      if (e.IsAction(Action.MouseLeft) || e.IsAction(Action.ShiftMouseLeft))
        this.StartDrag(Action.MouseLeft);
      else if (e.IsAction(Action.MouseRight))
        this.StartDrag(Action.MouseRight);
      else if (e.IsAction(Action.MouseMiddle))
        this.StartDrag(Action.MouseMiddle);
      else
        flag = false;
      if ((Object) this.activeTool == (Object) null || !this.activeTool.enabled)
        return;
      List<RaycastResult> raycastResults = new List<RaycastResult>();
      PointerEventData eventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
      eventData.position = (Vector2) KInputManager.GetMousePos();
      UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
      if ((Object) current != (Object) null)
      {
        current.RaycastAll(eventData, raycastResults);
        if (raycastResults.Count > 0)
          return;
      }
      if (flag && !this.draggingAllowed)
        e.TryConsume(e.GetAction());
      else if (e.TryConsume(Action.MouseLeft) || e.TryConsume(Action.ShiftMouseLeft))
        this.activeTool.OnLeftClickDown(this.GetCursorPos());
      else if (e.IsAction(Action.MouseRight))
        this.activeTool.OnRightClickDown(this.GetCursorPos(), e);
      else
        this.activeTool.OnKeyDown(e);
    }
  }

  public void OnKeyUp(KButtonEvent e)
  {
    bool flag = true;
    if (e.IsAction(Action.MouseLeft) || e.IsAction(Action.ShiftMouseLeft))
      this.StopDrag(Action.MouseLeft);
    else if (e.IsAction(Action.MouseRight))
      this.StopDrag(Action.MouseRight);
    else if (e.IsAction(Action.MouseMiddle))
      this.StopDrag(Action.MouseMiddle);
    else
      flag = false;
    if ((Object) this.activeTool == (Object) null || !this.activeTool.enabled || !this.activeTool.hasFocus)
      return;
    if (flag && !this.draggingAllowed)
      e.TryConsume(e.GetAction());
    else if (!KInputManager.currentControllerIsGamepad)
    {
      if (e.TryConsume(Action.MouseLeft) || e.TryConsume(Action.ShiftMouseLeft))
        this.activeTool.OnLeftClickUp(this.GetCursorPos());
      else if (e.IsAction(Action.MouseRight))
        this.activeTool.OnRightClickUp(this.GetCursorPos());
      else
        this.activeTool.OnKeyUp(e);
    }
    else if (e.IsAction(Action.MouseLeft) || e.IsAction(Action.ShiftMouseLeft))
      this.activeTool.OnLeftClickUp(this.GetCursorPos());
    else if (e.IsAction(Action.MouseRight))
      this.activeTool.OnRightClickUp(this.GetCursorPos());
    else
      this.activeTool.OnKeyUp(e);
  }

  public bool ConsumeIfNotDragging(KButtonEvent e, Action action)
  {
    return (this.dragAction != action || !this.dragging) && e.TryConsume(action);
  }

  public bool IsDragging() => this.dragging && this.CanDrag();

  public bool CanDrag() => this.draggingAllowed && this.dragAction != 0;

  public void AllowDragging(bool allow) => this.draggingAllowed = allow;

  public Vector3 GetDragDelta() => this.dragDelta;

  public Vector3 GetWorldDragDelta() => !this.draggingAllowed ? Vector3.zero : this.worldDragDelta;
}
