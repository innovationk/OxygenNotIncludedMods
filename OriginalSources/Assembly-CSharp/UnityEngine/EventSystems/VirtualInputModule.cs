// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.VirtualInputModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

#nullable disable
namespace UnityEngine.EventSystems;

[AddComponentMenu("Event/Virtual Input Module")]
public class VirtualInputModule : PointerInputModule, IInputHandler
{
  private float m_PrevActionTime;
  private Vector2 m_LastMoveVector;
  private int m_ConsecutiveMoveCount;
  private string debugName;
  private Vector2 m_LastMousePosition;
  private Vector2 m_MousePosition;
  public bool mouseMovementOnly;
  [SerializeField]
  private RectTransform m_VirtualCursor;
  [SerializeField]
  private float m_VirtualCursorSpeed = 1f;
  [SerializeField]
  private Vector2 m_VirtualCursorOffset = Vector2.zero;
  [SerializeField]
  private Camera m_canvasCamera;
  private Camera VCcam;
  public bool CursorCanvasShouldBeOverlay;
  private Canvas m_virtualCursorCanvas;
  private CanvasScaler m_virtualCursorScaler;
  private PointerEventData leftClickData;
  private PointerEventData rightClickData;
  private VirtualInputModule.ControllerButtonStates conButtonStates;
  private GameObject m_CurrentFocusedGameObject;
  private bool leftReleased;
  private bool rightReleased;
  private bool leftFirstClick;
  private bool rightFirstClick;
  [SerializeField]
  private string m_HorizontalAxis = "Horizontal";
  [SerializeField]
  private string m_VerticalAxis = "Vertical";
  [SerializeField]
  private string m_SubmitButton = "Submit";
  [SerializeField]
  private string m_CancelButton = "Cancel";
  [SerializeField]
  private float m_InputActionsPerSecond = 10f;
  [SerializeField]
  private float m_RepeatDelay = 0.5f;
  [SerializeField]
  [FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
  private bool m_ForceModuleActive;
  private readonly PointerInputModule.MouseState m_MouseState = new PointerInputModule.MouseState();

  public string handlerName => "VirtualCursorInput";

  public KInputHandler inputHandler { get; set; }

  protected VirtualInputModule()
  {
  }

  [Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
  public VirtualInputModule.InputMode inputMode => VirtualInputModule.InputMode.Mouse;

  [Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead (UnityUpgradable) -> forceModuleActive")]
  public bool allowActivationOnMobileDevice
  {
    get => this.m_ForceModuleActive;
    set => this.m_ForceModuleActive = value;
  }

  public bool forceModuleActive
  {
    get => this.m_ForceModuleActive;
    set => this.m_ForceModuleActive = value;
  }

  public float inputActionsPerSecond
  {
    get => this.m_InputActionsPerSecond;
    set => this.m_InputActionsPerSecond = value;
  }

  public float repeatDelay
  {
    get => this.m_RepeatDelay;
    set => this.m_RepeatDelay = value;
  }

  public string horizontalAxis
  {
    get => this.m_HorizontalAxis;
    set => this.m_HorizontalAxis = value;
  }

  public string verticalAxis
  {
    get => this.m_VerticalAxis;
    set => this.m_VerticalAxis = value;
  }

  public string submitButton
  {
    get => this.m_SubmitButton;
    set => this.m_SubmitButton = value;
  }

  public string cancelButton
  {
    get => this.m_CancelButton;
    set => this.m_CancelButton = value;
  }

  public void SetCursor(Texture2D tex)
  {
    this.UpdateModule();
    if (!(bool) (UnityEngine.Object) this.m_VirtualCursor)
      return;
    this.m_VirtualCursor.GetComponent<RawImage>().texture = (Texture) tex;
  }

  public override void UpdateModule()
  {
    GameInputManager inputManager = Global.GetInputManager();
    if (inputManager.GetControllerCount() <= 1)
      return;
    if (this.inputHandler == null || !this.inputHandler.UsesController((IInputHandler) this, inputManager.GetController(1)))
    {
      KInputHandler.Add((IInputHandler) inputManager.GetController(1), (IInputHandler) this, int.MaxValue);
      if (!inputManager.usedMenus.Contains((IInputHandler) this))
        inputManager.usedMenus.Add((IInputHandler) this);
      this.debugName = SceneManager.GetActiveScene().name + "-VirtualInputModule";
    }
    if ((UnityEngine.Object) this.m_VirtualCursor == (UnityEngine.Object) null)
      this.m_VirtualCursor = GameObject.Find("VirtualCursor").GetComponent<RectTransform>();
    if ((UnityEngine.Object) this.m_canvasCamera == (UnityEngine.Object) null)
    {
      this.m_canvasCamera = this.gameObject.AddComponent<Camera>();
      this.m_canvasCamera.enabled = false;
    }
    if ((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null)
      this.m_canvasCamera.CopyFrom(CameraController.Instance.overlayCamera);
    else if (this.CursorCanvasShouldBeOverlay)
      this.m_canvasCamera.CopyFrom(GameObject.Find("FrontEndCamera").GetComponent<Camera>());
    if ((UnityEngine.Object) this.m_canvasCamera != (UnityEngine.Object) null && (UnityEngine.Object) this.VCcam == (UnityEngine.Object) null)
    {
      this.VCcam = GameObject.Find("VirtualCursorCamera").GetComponent<Camera>();
      if ((UnityEngine.Object) this.VCcam != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.m_virtualCursorCanvas == (UnityEngine.Object) null)
        {
          this.m_virtualCursorCanvas = GameObject.Find("VirtualCursorCanvas").GetComponent<Canvas>();
          this.m_virtualCursorScaler = this.m_virtualCursorCanvas.GetComponent<CanvasScaler>();
        }
        if (this.CursorCanvasShouldBeOverlay)
        {
          this.m_virtualCursorCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
          this.VCcam.orthographic = false;
        }
        else
        {
          this.VCcam.orthographic = this.m_canvasCamera.orthographic;
          this.VCcam.orthographicSize = this.m_canvasCamera.orthographicSize;
          this.VCcam.transform.position = this.m_canvasCamera.transform.position;
          this.VCcam.enabled = true;
          this.m_virtualCursorCanvas.renderMode = RenderMode.ScreenSpaceCamera;
          this.m_virtualCursorCanvas.worldCamera = this.VCcam;
        }
      }
    }
    if ((UnityEngine.Object) this.m_canvasCamera != (UnityEngine.Object) null && (UnityEngine.Object) this.VCcam != (UnityEngine.Object) null)
    {
      this.VCcam.orthographic = this.m_canvasCamera.orthographic;
      this.VCcam.orthographicSize = this.m_canvasCamera.orthographicSize;
      this.VCcam.transform.position = this.m_canvasCamera.transform.position;
      this.VCcam.aspect = this.m_canvasCamera.aspect;
      this.VCcam.enabled = true;
    }
    Vector2 vector2 = new Vector2((float) Screen.width, (float) Screen.height);
    if ((UnityEngine.Object) this.m_virtualCursorScaler != (UnityEngine.Object) null && this.m_virtualCursorScaler.referenceResolution != vector2)
      this.m_virtualCursorScaler.referenceResolution = vector2;
    this.m_LastMousePosition = this.m_MousePosition;
    this.m_VirtualCursor.localScale = (Vector3) Vector2.one;
    Vector2 steamCursorMovement = KInputManager.steamInputInterpreter.GetSteamCursorMovement();
    float num = (float) (1.0 / (4500.0 / (double) vector2.x));
    steamCursorMovement.x *= num;
    steamCursorMovement.y *= num;
    this.m_VirtualCursor.anchoredPosition += steamCursorMovement * this.m_VirtualCursorSpeed;
    this.m_VirtualCursor.anchoredPosition = new Vector2(Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.x, 0.0f, vector2.x), Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.y, 0.0f, vector2.y));
    KInputManager.virtualCursorPos = new Vector3F(this.m_VirtualCursor.anchoredPosition.x, this.m_VirtualCursor.anchoredPosition.y, 0.0f);
    this.m_MousePosition = this.m_VirtualCursor.anchoredPosition;
  }

  public override bool IsModuleSupported() => this.m_ForceModuleActive || Input.mousePresent;

  public override bool ShouldActivateModule()
  {
    if (!base.ShouldActivateModule())
      return false;
    if (KInputManager.currentControllerIsGamepad)
      return true;
    int num1 = this.m_ForceModuleActive ? 1 : 0;
    Input.GetButtonDown(this.m_SubmitButton);
    int num2 = Input.GetButtonDown(this.m_CancelButton) ? 1 : 0;
    return (num1 | num2 | (!Mathf.Approximately(Input.GetAxisRaw(this.m_HorizontalAxis), 0.0f) ? 1 : 0) | (!Mathf.Approximately(Input.GetAxisRaw(this.m_VerticalAxis), 0.0f) ? 1 : 0) | ((double) (this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0.0 ? 1 : 0) | (Input.GetMouseButtonDown(0) ? 1 : 0)) != 0;
  }

  public override void ActivateModule()
  {
    base.ActivateModule();
    if ((UnityEngine.Object) this.m_canvasCamera == (UnityEngine.Object) null)
    {
      this.m_canvasCamera = this.gameObject.AddComponent<Camera>();
      this.m_canvasCamera.enabled = false;
    }
    this.m_VirtualCursor.anchoredPosition = (double) Input.mousePosition.x <= 0.0 || (double) Input.mousePosition.x >= (double) Screen.width || (double) Input.mousePosition.y <= 0.0 || (double) Input.mousePosition.y >= (double) Screen.height ? new Vector2((float) (Screen.width / 2), (float) (Screen.height / 2)) : (Vector2) Input.mousePosition;
    this.m_VirtualCursor.anchoredPosition = new Vector2(Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.x, 0.0f, (float) Screen.width), Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.y, 0.0f, (float) Screen.height));
    this.m_VirtualCursor.localScale = (Vector3) Vector2.zero;
    this.m_MousePosition = this.m_VirtualCursor.anchoredPosition;
    this.m_LastMousePosition = this.m_VirtualCursor.anchoredPosition;
    GameObject selectedGameObject = this.eventSystem.currentSelectedGameObject;
    if ((UnityEngine.Object) selectedGameObject == (UnityEngine.Object) null)
      selectedGameObject = this.eventSystem.firstSelectedGameObject;
    if ((UnityEngine.Object) this.m_VirtualCursor == (UnityEngine.Object) null)
      this.m_VirtualCursor = GameObject.Find("VirtualCursor").GetComponent<RectTransform>();
    if ((UnityEngine.Object) this.m_canvasCamera == (UnityEngine.Object) null)
      this.m_canvasCamera = GameObject.Find("FrontEndCamera").GetComponent<Camera>();
    this.eventSystem.SetSelectedGameObject(selectedGameObject, this.GetBaseEventData());
  }

  public override void DeactivateModule()
  {
    base.DeactivateModule();
    this.ClearSelection();
    this.conButtonStates.affirmativeDown = false;
    this.conButtonStates.affirmativeHoldTime = 0.0f;
    this.conButtonStates.negativeDown = false;
    this.conButtonStates.negativeHoldTime = 0.0f;
  }

  public override void Process()
  {
    bool selectedObject = this.SendUpdateEventToSelectedObject();
    if (this.eventSystem.sendNavigationEvents)
    {
      if (!selectedObject)
        selectedObject |= this.SendMoveEventToSelectedObject();
      if (!selectedObject)
        this.SendSubmitEventToSelectedObject();
    }
    this.ProcessMouseEvent();
  }

  protected bool SendSubmitEventToSelectedObject()
  {
    if ((UnityEngine.Object) this.eventSystem.currentSelectedGameObject == (UnityEngine.Object) null)
      return false;
    BaseEventData baseEventData = this.GetBaseEventData();
    if (Input.GetButtonDown(this.m_SubmitButton))
      ExecuteEvents.Execute<ISubmitHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
    if (Input.GetButtonDown(this.m_CancelButton))
      ExecuteEvents.Execute<ICancelHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
    return baseEventData.used;
  }

  private Vector2 GetRawMoveVector()
  {
    Vector2 zero = Vector2.zero with
    {
      x = Input.GetAxisRaw(this.m_HorizontalAxis),
      y = Input.GetAxisRaw(this.m_VerticalAxis)
    };
    if (Input.GetButtonDown(this.m_HorizontalAxis))
    {
      if ((double) zero.x < 0.0)
        zero.x = -1f;
      if ((double) zero.x > 0.0)
        zero.x = 1f;
    }
    if (Input.GetButtonDown(this.m_VerticalAxis))
    {
      if ((double) zero.y < 0.0)
        zero.y = -1f;
      if ((double) zero.y > 0.0)
        zero.y = 1f;
    }
    return zero;
  }

  protected bool SendMoveEventToSelectedObject()
  {
    float unscaledTime = Time.unscaledTime;
    Vector2 rawMoveVector = this.GetRawMoveVector();
    if (Mathf.Approximately(rawMoveVector.x, 0.0f) && Mathf.Approximately(rawMoveVector.y, 0.0f))
    {
      this.m_ConsecutiveMoveCount = 0;
      return false;
    }
    bool flag1 = Input.GetButtonDown(this.m_HorizontalAxis) || Input.GetButtonDown(this.m_VerticalAxis);
    bool flag2 = (double) Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0.0;
    if (!flag1)
      flag1 = !flag2 || this.m_ConsecutiveMoveCount != 1 ? (double) unscaledTime > (double) this.m_PrevActionTime + 1.0 / (double) this.m_InputActionsPerSecond : (double) unscaledTime > (double) this.m_PrevActionTime + (double) this.m_RepeatDelay;
    if (!flag1)
      return false;
    AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
    ExecuteEvents.Execute<IMoveHandler>(this.eventSystem.currentSelectedGameObject, (BaseEventData) axisEventData, ExecuteEvents.moveHandler);
    if (!flag2)
      this.m_ConsecutiveMoveCount = 0;
    ++this.m_ConsecutiveMoveCount;
    this.m_PrevActionTime = unscaledTime;
    this.m_LastMoveVector = rawMoveVector;
    return axisEventData.used;
  }

  protected void ProcessMouseEvent() => this.ProcessMouseEvent(0);

  protected void ProcessMouseEvent(int id)
  {
    if (this.mouseMovementOnly)
      return;
    PointerInputModule.MouseState pointerEventData = this.GetMousePointerEventData(id);
    PointerInputModule.MouseButtonEventData eventData = pointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
    this.m_CurrentFocusedGameObject = eventData.buttonData.pointerCurrentRaycast.gameObject;
    this.ProcessControllerPress(eventData, true);
    this.ProcessControllerPress(pointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData, false);
    this.ProcessMove(eventData.buttonData);
    this.ProcessDrag(eventData.buttonData);
    this.ProcessDrag(pointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
    this.ProcessDrag(pointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
    if (Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0.0f))
      return;
    ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), (BaseEventData) eventData.buttonData, ExecuteEvents.scrollHandler);
  }

  protected bool SendUpdateEventToSelectedObject()
  {
    if ((UnityEngine.Object) this.eventSystem.currentSelectedGameObject == (UnityEngine.Object) null)
      return false;
    BaseEventData baseEventData = this.GetBaseEventData();
    ExecuteEvents.Execute<IUpdateSelectedHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
    return baseEventData.used;
  }

  protected void ProcessMousePress(PointerInputModule.MouseButtonEventData data)
  {
    PointerEventData buttonData = data.buttonData;
    GameObject gameObject1 = buttonData.pointerCurrentRaycast.gameObject;
    if (data.PressedThisFrame())
    {
      buttonData.eligibleForClick = true;
      buttonData.delta = Vector2.zero;
      buttonData.dragging = false;
      buttonData.useDragThreshold = true;
      buttonData.pressPosition = buttonData.position;
      buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
      buttonData.position = this.m_VirtualCursor.anchoredPosition;
      this.DeselectIfSelectionChanged(gameObject1, (BaseEventData) buttonData);
      GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject1, (BaseEventData) buttonData, ExecuteEvents.pointerDownHandler);
      if ((UnityEngine.Object) gameObject2 == (UnityEngine.Object) null)
        gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
      float unscaledTime = Time.unscaledTime;
      if ((UnityEngine.Object) gameObject2 == (UnityEngine.Object) buttonData.lastPress)
      {
        if ((double) unscaledTime - (double) buttonData.clickTime < 0.30000001192092896)
          ++buttonData.clickCount;
        else
          buttonData.clickCount = 1;
        buttonData.clickTime = unscaledTime;
      }
      else
        buttonData.clickCount = 1;
      buttonData.pointerPress = gameObject2;
      buttonData.rawPointerPress = gameObject1;
      buttonData.clickTime = unscaledTime;
      buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject1);
      if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null)
        ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, (BaseEventData) buttonData, ExecuteEvents.initializePotentialDrag);
    }
    if (!data.ReleasedThisFrame())
      return;
    ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, (BaseEventData) buttonData, ExecuteEvents.pointerUpHandler);
    GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
    if ((UnityEngine.Object) buttonData.pointerPress == (UnityEngine.Object) eventHandler && buttonData.eligibleForClick)
      ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, (BaseEventData) buttonData, ExecuteEvents.pointerClickHandler);
    else if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null && buttonData.dragging)
      ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject1, (BaseEventData) buttonData, ExecuteEvents.dropHandler);
    buttonData.eligibleForClick = false;
    buttonData.pointerPress = (GameObject) null;
    buttonData.rawPointerPress = (GameObject) null;
    if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null && buttonData.dragging)
      ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, (BaseEventData) buttonData, ExecuteEvents.endDragHandler);
    buttonData.dragging = false;
    buttonData.pointerDrag = (GameObject) null;
    if (!((UnityEngine.Object) gameObject1 != (UnityEngine.Object) buttonData.pointerEnter))
      return;
    this.HandlePointerExitAndEnter(buttonData, (GameObject) null);
    this.HandlePointerExitAndEnter(buttonData, gameObject1);
  }

  public void OnKeyDown(KButtonEvent e)
  {
    if (!KInputManager.currentControllerIsGamepad)
      return;
    if (e.IsAction(Action.MouseLeft) || e.IsAction(Action.ShiftMouseLeft))
    {
      if (this.conButtonStates.affirmativeDown)
        this.conButtonStates.affirmativeHoldTime += Time.unscaledDeltaTime;
      if (!this.conButtonStates.affirmativeDown)
      {
        this.leftFirstClick = true;
        this.leftReleased = false;
      }
      this.conButtonStates.affirmativeDown = true;
    }
    else
    {
      if (!e.IsAction(Action.MouseRight))
        return;
      if (this.conButtonStates.negativeDown)
        this.conButtonStates.negativeHoldTime += Time.unscaledDeltaTime;
      if (!this.conButtonStates.negativeDown)
      {
        this.rightFirstClick = true;
        this.rightReleased = false;
      }
      this.conButtonStates.negativeDown = true;
    }
  }

  public void OnKeyUp(KButtonEvent e)
  {
    if (!KInputManager.currentControllerIsGamepad)
      return;
    if (e.IsAction(Action.MouseLeft) || e.IsAction(Action.ShiftMouseLeft))
    {
      this.conButtonStates.affirmativeHoldTime = 0.0f;
      this.leftReleased = true;
      this.leftFirstClick = false;
      this.conButtonStates.affirmativeDown = false;
    }
    else
    {
      if (!e.IsAction(Action.MouseRight))
        return;
      this.conButtonStates.negativeHoldTime = 0.0f;
      this.rightReleased = true;
      this.rightFirstClick = false;
      this.conButtonStates.negativeDown = false;
    }
  }

  protected void ProcessControllerPress(
    PointerInputModule.MouseButtonEventData data,
    bool leftClick)
  {
    if (this.leftClickData == null)
      this.leftClickData = data.buttonData;
    if (this.rightClickData == null)
      this.rightClickData = data.buttonData;
    if (leftClick)
    {
      PointerEventData buttonData = data.buttonData;
      GameObject gameObject1 = buttonData.pointerCurrentRaycast.gameObject;
      buttonData.position = this.m_VirtualCursor.anchoredPosition;
      if (this.leftFirstClick)
      {
        buttonData.button = PointerEventData.InputButton.Left;
        buttonData.eligibleForClick = true;
        buttonData.delta = Vector2.zero;
        buttonData.dragging = false;
        buttonData.useDragThreshold = true;
        buttonData.pressPosition = buttonData.position;
        buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
        buttonData.position = new Vector2(KInputManager.virtualCursorPos.x, KInputManager.virtualCursorPos.y);
        this.DeselectIfSelectionChanged(gameObject1, (BaseEventData) buttonData);
        GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject1, (BaseEventData) buttonData, ExecuteEvents.pointerDownHandler);
        if ((UnityEngine.Object) gameObject2 == (UnityEngine.Object) null)
          gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
        float unscaledTime = Time.unscaledTime;
        if ((UnityEngine.Object) gameObject2 == (UnityEngine.Object) buttonData.lastPress)
        {
          if ((double) unscaledTime - (double) buttonData.clickTime < 0.30000001192092896)
            ++buttonData.clickCount;
          else
            buttonData.clickCount = 1;
          buttonData.clickTime = unscaledTime;
        }
        else
          buttonData.clickCount = 1;
        buttonData.pointerPress = gameObject2;
        buttonData.rawPointerPress = gameObject1;
        buttonData.clickTime = unscaledTime;
        buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject1);
        if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null)
          ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, (BaseEventData) buttonData, ExecuteEvents.initializePotentialDrag);
        this.leftFirstClick = false;
      }
      else
      {
        if (!this.leftReleased)
          return;
        buttonData.button = PointerEventData.InputButton.Left;
        ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, (BaseEventData) buttonData, ExecuteEvents.pointerUpHandler);
        GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
        if ((UnityEngine.Object) buttonData.pointerPress == (UnityEngine.Object) eventHandler && buttonData.eligibleForClick)
          ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, (BaseEventData) buttonData, ExecuteEvents.pointerClickHandler);
        else if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null && buttonData.dragging)
          ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject1, (BaseEventData) buttonData, ExecuteEvents.dropHandler);
        buttonData.eligibleForClick = false;
        buttonData.pointerPress = (GameObject) null;
        buttonData.rawPointerPress = (GameObject) null;
        if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null && buttonData.dragging)
          ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, (BaseEventData) buttonData, ExecuteEvents.endDragHandler);
        buttonData.dragging = false;
        buttonData.pointerDrag = (GameObject) null;
        if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) buttonData.pointerEnter)
        {
          this.HandlePointerExitAndEnter(buttonData, (GameObject) null);
          this.HandlePointerExitAndEnter(buttonData, gameObject1);
        }
        this.leftReleased = false;
      }
    }
    else
    {
      PointerEventData buttonData = data.buttonData;
      GameObject gameObject3 = buttonData.pointerCurrentRaycast.gameObject;
      buttonData.position = this.m_VirtualCursor.anchoredPosition;
      if (this.rightFirstClick)
      {
        buttonData.button = PointerEventData.InputButton.Right;
        buttonData.eligibleForClick = true;
        buttonData.delta = Vector2.zero;
        buttonData.dragging = false;
        buttonData.useDragThreshold = true;
        buttonData.pressPosition = buttonData.position;
        buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
        buttonData.position = this.m_VirtualCursor.anchoredPosition;
        this.DeselectIfSelectionChanged(gameObject3, (BaseEventData) buttonData);
        GameObject gameObject4 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject3, (BaseEventData) buttonData, ExecuteEvents.pointerDownHandler);
        if ((UnityEngine.Object) gameObject4 == (UnityEngine.Object) null)
          gameObject4 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject3);
        float unscaledTime = Time.unscaledTime;
        if ((UnityEngine.Object) gameObject4 == (UnityEngine.Object) buttonData.lastPress)
        {
          if ((double) unscaledTime - (double) buttonData.clickTime < 0.30000001192092896)
            ++buttonData.clickCount;
          else
            buttonData.clickCount = 1;
          buttonData.clickTime = unscaledTime;
        }
        else
          buttonData.clickCount = 1;
        buttonData.pointerPress = gameObject4;
        buttonData.rawPointerPress = gameObject3;
        buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject3);
        if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null)
          ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, (BaseEventData) buttonData, ExecuteEvents.initializePotentialDrag);
        this.rightFirstClick = false;
      }
      else
      {
        if (!this.rightReleased)
          return;
        buttonData.button = PointerEventData.InputButton.Right;
        ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, (BaseEventData) buttonData, ExecuteEvents.pointerUpHandler);
        GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject3);
        if ((UnityEngine.Object) buttonData.pointerPress == (UnityEngine.Object) eventHandler && buttonData.eligibleForClick)
          ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, (BaseEventData) buttonData, ExecuteEvents.pointerClickHandler);
        else if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null && buttonData.dragging)
          ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject3, (BaseEventData) buttonData, ExecuteEvents.dropHandler);
        buttonData.eligibleForClick = false;
        buttonData.pointerPress = (GameObject) null;
        buttonData.rawPointerPress = (GameObject) null;
        if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null && buttonData.dragging)
          ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, (BaseEventData) buttonData, ExecuteEvents.endDragHandler);
        buttonData.dragging = false;
        buttonData.pointerDrag = (GameObject) null;
        if ((UnityEngine.Object) gameObject3 != (UnityEngine.Object) buttonData.pointerEnter)
        {
          this.HandlePointerExitAndEnter(buttonData, (GameObject) null);
          this.HandlePointerExitAndEnter(buttonData, gameObject3);
        }
        this.rightReleased = false;
      }
    }
  }

  protected override PointerInputModule.MouseState GetMousePointerEventData(int id)
  {
    PointerEventData data1;
    int num = this.GetPointerData(-1, out data1, true) ? 1 : 0;
    data1.Reset();
    Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(this.m_canvasCamera, this.m_VirtualCursor.position);
    if (num != 0)
      data1.position = screenPoint;
    Vector2 anchoredPosition = this.m_VirtualCursor.anchoredPosition;
    data1.delta = anchoredPosition - data1.position;
    data1.position = anchoredPosition;
    data1.scrollDelta = Input.mouseScrollDelta;
    data1.button = PointerEventData.InputButton.Left;
    this.eventSystem.RaycastAll(data1, this.m_RaycastResultCache);
    RaycastResult firstRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
    data1.pointerCurrentRaycast = firstRaycast;
    this.m_RaycastResultCache.Clear();
    PointerEventData data2;
    this.GetPointerData(-2, out data2, true);
    this.CopyFromTo(data1, data2);
    data2.button = PointerEventData.InputButton.Right;
    PointerEventData data3;
    this.GetPointerData(-3, out data3, true);
    this.CopyFromTo(data1, data3);
    data3.button = PointerEventData.InputButton.Middle;
    this.m_MouseState.SetButtonState(PointerEventData.InputButton.Left, this.StateForMouseButton(0), data1);
    this.m_MouseState.SetButtonState(PointerEventData.InputButton.Right, this.StateForMouseButton(1), data2);
    this.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, this.StateForMouseButton(2), data3);
    return this.m_MouseState;
  }

  [Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
  public enum InputMode
  {
    Mouse,
    Buttons,
  }

  private struct ControllerButtonStates
  {
    public bool affirmativeDown;
    public float affirmativeHoldTime;
    public bool negativeDown;
    public float negativeHoldTime;
  }
}
