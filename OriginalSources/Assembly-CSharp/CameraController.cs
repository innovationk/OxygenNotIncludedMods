// Decompiled with JetBrains decompiler
// Type: CameraController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/CameraController")]
public class CameraController : KMonoBehaviour, IInputHandler
{
  public const float DEFAULT_MAX_ORTHO_SIZE = 20f;
  public const float MAX_Y_SCALE = 1.1f;
  public LocText infoText;
  private const float FIXED_Z = -100f;
  public bool FreeCameraEnabled;
  public float zoomSpeed;
  public float minOrthographicSize;
  public float zoomFactor;
  public float keyPanningSpeed;
  public float keyPanningEasing;
  public Texture2D dayColourCube;
  public Texture2D nightColourCube;
  public Material LightBufferMaterial;
  public Material LightCircleOverlay;
  public Material LightConeOverlay;
  public Transform followTarget;
  public Vector3 followTargetPos;
  public GridVisibleArea VisibleArea = new GridVisibleArea(8);
  private float maxOrthographicSize = 20f;
  private float overrideZoomSpeed;
  private bool panning;
  private const float MaxEdgePaddingPercent = 0.33f;
  private Vector3 keyPanDelta;
  [SerializeField]
  private LayerMask timelapseCameraCullingMask;
  [SerializeField]
  private LayerMask timelapseOverlayCameraCullingMask;
  private bool userCameraControlDisabled;
  private bool panLeft;
  private bool panRight;
  private bool panUp;
  private bool panDown;
  [NonSerialized]
  public Camera baseCamera;
  [NonSerialized]
  public Camera overlayCamera;
  [NonSerialized]
  public Camera overlayNoDepthCamera;
  [NonSerialized]
  public Camera uiCamera;
  [NonSerialized]
  public Camera lightBufferCamera;
  [NonSerialized]
  public Camera simOverlayCamera;
  [NonSerialized]
  public Camera infraredCamera;
  [NonSerialized]
  public Camera timelapseFreezeCamera;
  [SerializeField]
  private List<GameScreenManager.UIRenderTarget> uiCameraTargets;
  public List<Camera> cameras = new List<Camera>();
  private MultipleRenderTarget mrt;
  private KAnimActivePostProcessingEffects kAnimPostProcessingEffects;
  private CustomActiveScreenPostProcessingEffects customActiveScreenPostProcessingEffects;
  public SoundCuller soundCuller;
  private bool cinemaCamEnabled;
  private bool cinemaToggleLock;
  private bool cinemaToggleEasing;
  private bool cinemaUnpauseNextMove;
  private bool cinemaPanLeft;
  private bool cinemaPanRight;
  private bool cinemaPanUp;
  private bool cinemaPanDown;
  private bool cinemaZoomIn;
  private bool cinemaZoomOut;
  private int cinemaZoomSpeed = 10;
  private float cinemaEasing = 0.05f;
  private float cinemaZoomVelocity;
  private float smoothDt;

  public string handlerName => this.gameObject.name;

  public float OrthographicSize
  {
    get => !((UnityEngine.Object) this.baseCamera == (UnityEngine.Object) null) ? this.baseCamera.orthographicSize : 0.0f;
    set
    {
      for (int index = 0; index < this.cameras.Count; ++index)
        this.cameras[index].orthographicSize = value;
    }
  }

  public KInputHandler inputHandler { get; set; }

  public float targetOrthographicSize { get; private set; }

  public bool isTargetPosSet { get; set; }

  public Vector3 targetPos { get; private set; }

  public bool ignoreClusterFX { get; private set; }

  public void ToggleClusterFX() => this.ignoreClusterFX = !this.ignoreClusterFX;

  protected override void OnForcedCleanUp()
  {
    Global.GetInputManager()?.usedMenus.Remove((IInputHandler) this);
  }

  public int cameraActiveCluster
  {
    get
    {
      return (UnityEngine.Object) ClusterManager.Instance == (UnityEngine.Object) null ? (int) byte.MaxValue : ClusterManager.Instance.activeWorldId;
    }
  }

  public void GetWorldCamera(out Vector2I worldOffset, out Vector2I worldSize)
  {
    WorldContainer worldContainer = (WorldContainer) null;
    if ((UnityEngine.Object) ClusterManager.Instance != (UnityEngine.Object) null)
      worldContainer = ClusterManager.Instance.activeWorld;
    if (!this.ignoreClusterFX && (UnityEngine.Object) worldContainer != (UnityEngine.Object) null)
    {
      worldOffset = worldContainer.WorldOffset;
      worldSize = worldContainer.WorldSize;
    }
    else
    {
      worldOffset = new Vector2I(0, 0);
      worldSize = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
    }
  }

  public bool DisableUserCameraControl
  {
    get => this.userCameraControlDisabled;
    set
    {
      this.userCameraControlDisabled = value;
      if (!this.userCameraControlDisabled)
        return;
      this.panning = false;
      this.panLeft = false;
      this.panRight = false;
      this.panUp = false;
      this.panDown = false;
    }
  }

  public static CameraController Instance { get; private set; }

  public static void DestroyInstance() => CameraController.Instance = (CameraController) null;

  public void ToggleColouredOverlayView(bool enabled)
  {
    this.mrt.ToggleColouredOverlayView(enabled);
  }

  public void EnableKAnimPostProcessingEffect(KAnimConverter.PostProcessingEffects effect)
  {
    this.kAnimPostProcessingEffects.EnableEffect(effect);
  }

  public void DisableKAnimPostProcessingEffect(KAnimConverter.PostProcessingEffects effect)
  {
    this.kAnimPostProcessingEffects.DisableEffect(effect);
  }

  public void RegisterCustomScreenPostProcessingEffect(Func<RenderTexture, Material> effectFn)
  {
    this.customActiveScreenPostProcessingEffects.RegisterEffect(effectFn);
  }

  public void UnregisterCustomScreenPostProcessingEffect(Func<RenderTexture, Material> effectFn)
  {
    this.customActiveScreenPostProcessingEffects.UnregisterEffect(effectFn);
  }

  protected override void OnPrefabInit()
  {
    Util.Reset(this.transform);
    this.transform.SetLocalPosition(new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, -100f));
    this.targetOrthographicSize = this.maxOrthographicSize;
    CameraController.Instance = this;
    this.DisableUserCameraControl = false;
    this.baseCamera = this.CopyCamera(Camera.main, "baseCamera");
    this.mrt = this.baseCamera.gameObject.AddComponent<MultipleRenderTarget>();
    this.mrt.onSetupComplete += new Action<Camera>(this.OnMRTSetupComplete);
    this.baseCamera.gameObject.AddComponent<LightBufferCompositor>();
    this.baseCamera.transparencySortMode = TransparencySortMode.Orthographic;
    this.baseCamera.transform.parent = this.transform;
    Util.Reset(this.baseCamera.transform);
    int mask1 = LayerMask.GetMask("PlaceWithDepth", "Overlay");
    int mask2 = LayerMask.GetMask("Construction");
    this.baseCamera.cullingMask &= ~mask1;
    this.baseCamera.cullingMask |= mask2;
    this.baseCamera.tag = "Untagged";
    this.baseCamera.gameObject.AddComponent<CameraRenderTexture>().TextureName = "_LitTex";
    this.infraredCamera = this.CopyCamera(this.baseCamera, "Infrared");
    this.infraredCamera.cullingMask = 0;
    this.infraredCamera.clearFlags = CameraClearFlags.Color;
    this.infraredCamera.depth = this.baseCamera.depth - 1f;
    this.infraredCamera.transform.parent = this.transform;
    this.infraredCamera.gameObject.AddComponent<Infrared>();
    if ((UnityEngine.Object) SimDebugView.Instance != (UnityEngine.Object) null)
    {
      this.simOverlayCamera = this.CopyCamera(this.baseCamera, "SimOverlayCamera");
      this.simOverlayCamera.cullingMask = LayerMask.GetMask("SimDebugView");
      this.simOverlayCamera.clearFlags = CameraClearFlags.Color;
      this.simOverlayCamera.depth = this.baseCamera.depth + 1f;
      this.simOverlayCamera.transform.parent = this.transform;
      this.simOverlayCamera.gameObject.AddComponent<CameraRenderTexture>().TextureName = "_SimDebugViewTex";
    }
    this.overlayCamera = Camera.main;
    this.overlayCamera.name = "Overlay";
    this.overlayCamera.cullingMask = mask1 | mask2;
    this.overlayCamera.clearFlags = CameraClearFlags.Nothing;
    this.overlayCamera.transform.parent = this.transform;
    this.overlayCamera.depth = this.baseCamera.depth + 3f;
    this.overlayCamera.transform.SetLocalPosition(Vector3.zero);
    this.overlayCamera.transform.localRotation = Quaternion.identity;
    this.overlayCamera.renderingPath = RenderingPath.Forward;
    this.overlayCamera.allowHDR = false;
    this.overlayCamera.tag = "Untagged";
    this.kAnimPostProcessingEffects = this.overlayCamera.GetComponent<KAnimActivePostProcessingEffects>();
    this.customActiveScreenPostProcessingEffects = this.overlayCamera.GetComponent<CustomActiveScreenPostProcessingEffects>();
    this.overlayCamera.gameObject.AddComponent<CameraReferenceTexture>().referenceCamera = this.baseCamera;
    ColorCorrectionLookup component = this.overlayCamera.GetComponent<ColorCorrectionLookup>();
    component.Convert(this.dayColourCube, "");
    component.Convert2(this.nightColourCube, "");
    this.cameras.Add(this.overlayCamera);
    this.lightBufferCamera = this.CopyCamera(this.overlayCamera, "Light Buffer");
    this.lightBufferCamera.clearFlags = CameraClearFlags.Color;
    this.lightBufferCamera.cullingMask = LayerMask.GetMask("Lights");
    this.lightBufferCamera.depth = this.baseCamera.depth - 1f;
    this.lightBufferCamera.transform.parent = this.transform;
    this.lightBufferCamera.transform.SetLocalPosition(Vector3.zero);
    this.lightBufferCamera.rect = new UnityEngine.Rect(0.0f, 0.0f, 1f, 1f);
    LightBuffer lightBuffer = this.lightBufferCamera.gameObject.AddComponent<LightBuffer>();
    lightBuffer.Material = this.LightBufferMaterial;
    lightBuffer.CircleMaterial = this.LightCircleOverlay;
    lightBuffer.ConeMaterial = this.LightConeOverlay;
    this.overlayNoDepthCamera = this.CopyCamera(this.overlayCamera, "overlayNoDepth");
    int mask3 = LayerMask.GetMask("Overlay", "Place");
    this.baseCamera.cullingMask &= ~mask3;
    this.overlayNoDepthCamera.clearFlags = CameraClearFlags.Depth;
    this.overlayNoDepthCamera.cullingMask = mask3;
    this.overlayNoDepthCamera.transform.parent = this.transform;
    this.overlayNoDepthCamera.transform.SetLocalPosition(Vector3.zero);
    this.overlayNoDepthCamera.depth = this.baseCamera.depth + 4f;
    this.overlayNoDepthCamera.tag = "MainCamera";
    this.overlayNoDepthCamera.gameObject.AddComponent<NavPathDrawer>();
    this.overlayNoDepthCamera.gameObject.AddComponent<RangeVisualizerEffect>();
    this.overlayNoDepthCamera.gameObject.AddComponent<SkyVisibilityVisualizerEffect>();
    this.overlayNoDepthCamera.gameObject.AddComponent<ScannerNetworkVisualizerEffect>();
    this.overlayNoDepthCamera.gameObject.AddComponent<RocketLaunchConditionVisualizerEffect>();
    if (DlcManager.IsContentSubscribed("DLC4_ID"))
      this.overlayNoDepthCamera.gameObject.AddComponent<LargeImpactorVisualizerEffect>();
    this.uiCamera = this.CopyCamera(this.overlayCamera, "uiCamera");
    this.uiCamera.clearFlags = CameraClearFlags.Depth;
    this.uiCamera.cullingMask = LayerMask.GetMask("UI");
    this.uiCamera.transform.parent = this.transform;
    this.uiCamera.transform.SetLocalPosition(Vector3.zero);
    this.uiCamera.depth = this.baseCamera.depth + 5f;
    if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
    {
      this.timelapseFreezeCamera = this.CopyCamera(this.uiCamera, "timelapseFreezeCamera");
      this.timelapseFreezeCamera.depth = this.uiCamera.depth + 3f;
      this.timelapseFreezeCamera.gameObject.AddComponent<FillRenderTargetEffect>();
      this.timelapseFreezeCamera.enabled = false;
      Camera camera = CameraController.CloneCamera(this.overlayCamera, "timelapseCamera");
      Timelapser timelapser = camera.gameObject.AddComponent<Timelapser>();
      camera.transparencySortMode = TransparencySortMode.Orthographic;
      camera.depth = this.baseCamera.depth + 2f;
      Game.Instance.timelapser = timelapser;
    }
    if ((UnityEngine.Object) GameScreenManager.Instance != (UnityEngine.Object) null)
    {
      for (int index = 0; index < this.uiCameraTargets.Count; ++index)
        GameScreenManager.Instance.SetCamera(this.uiCameraTargets[index], this.uiCamera);
      this.infoText = GameScreenManager.Instance.screenshotModeCanvas.GetComponentInChildren<LocText>();
    }
    if (!KPlayerPrefs.HasKey("CameraSpeed"))
      CameraController.SetDefaultCameraSpeed();
    this.SetSpeedFromPrefs();
    Game.Instance.Subscribe(75424175, new Action<object>(this.SetSpeedFromPrefs));
    this.VisibleArea.Update();
  }

  private void SetSpeedFromPrefs(object data = null)
  {
    this.keyPanningSpeed = Mathf.Clamp(0.1f, KPlayerPrefs.GetFloat("CameraSpeed"), 2f);
  }

  public int GetCursorCell()
  {
    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
    Vector3 rhs = Vector3.Max((Vector3) ClusterManager.Instance.activeWorld.minimumBounds, worldPoint);
    return Grid.PosToCell(Vector3.Min((Vector3) ClusterManager.Instance.activeWorld.maximumBounds, rhs));
  }

  public static Camera CloneCamera(Camera camera, string name)
  {
    GameObject gameObject = new GameObject();
    gameObject.name = name;
    Camera camera1 = gameObject.AddComponent<Camera>();
    camera1.CopyFrom(camera);
    return camera1;
  }

  private Camera CopyCamera(Camera camera, string name)
  {
    Camera camera1 = CameraController.CloneCamera(camera, name);
    this.cameras.Add(camera1);
    return camera1;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Restore();
  }

  public static void SetDefaultCameraSpeed() => KPlayerPrefs.SetFloat("CameraSpeed", 1f);

  public Coroutine activeFadeRoutine { get; private set; }

  public void FadeOut(float targetPercentage = 1f, float speed = 1f, System.Action callback = null)
  {
    if (this.activeFadeRoutine != null)
      this.StopCoroutine(this.activeFadeRoutine);
    this.activeFadeRoutine = this.StartCoroutine(this.FadeWithBlack(true, 0.0f, targetPercentage, speed, callback));
  }

  public void FadeIn(float targetPercentage = 0.0f, float speed = 1f, System.Action callback = null)
  {
    if (this.activeFadeRoutine != null)
      this.StopCoroutine(this.activeFadeRoutine);
    this.activeFadeRoutine = this.StartCoroutine(this.FadeWithBlack(true, 1f, targetPercentage, speed, callback));
  }

  public void FadeOutColor(Color color, float targetPercentage = 1f, float speed = 1f, System.Action callback = null)
  {
    this.FadeOutColor(color, 1f, targetPercentage, speed, callback);
  }

  public void FadeOutColor(
    Color color,
    float initialPercentage,
    float targetPercentage = 1f,
    float speed = 1f,
    System.Action callback = null)
  {
    if (this.activeFadeRoutine != null)
      this.StopCoroutine(this.activeFadeRoutine);
    this.activeFadeRoutine = this.StartCoroutine(this.FadeWithColor(true, initialPercentage, targetPercentage, color, speed, callback));
  }

  public void FadeInColor(Color color, float targetPercentage = 0.0f, float speed = 1f, System.Action callback = null)
  {
    if (this.activeFadeRoutine != null)
      this.StopCoroutine(this.activeFadeRoutine);
    this.activeFadeRoutine = this.StartCoroutine(this.FadeWithColor(true, 1f, targetPercentage, color, speed, callback));
  }

  public void ActiveWorldStarWipe(int id, System.Action callback = null)
  {
    this.ActiveWorldStarWipe(id, false, new Vector3(), 10f, callback);
  }

  public void ActiveWorldStarWipe(
    int id,
    Vector3 position,
    float forceOrthgraphicSize = 10f,
    System.Action callback = null)
  {
    this.ActiveWorldStarWipe(id, true, position, forceOrthgraphicSize, callback);
  }

  private void ActiveWorldStarWipe(
    int id,
    bool useForcePosition,
    Vector3 forcePosition,
    float forceOrthgraphicSize,
    System.Action callback)
  {
    if (this.activeFadeRoutine != null)
      this.StopCoroutine(this.activeFadeRoutine);
    if (ClusterManager.Instance.activeWorldId != id)
    {
      if ((UnityEngine.Object) DetailsScreen.Instance != (UnityEngine.Object) null)
        DetailsScreen.Instance.DeselectAndClose();
      this.activeFadeRoutine = this.StartCoroutine(this.SwapToWorldFade(id, useForcePosition, forcePosition, forceOrthgraphicSize, callback));
    }
    else
    {
      ManagementMenu.Instance.CloseAll();
      if (!useForcePosition)
        return;
      CameraController.Instance.SetTargetPos(forcePosition, 8f, true);
      if (callback == null)
        return;
      callback();
    }
  }

  private IEnumerator SwapToWorldFade(
    int worldId,
    bool useForcePosition,
    Vector3 forcePosition,
    float forceOrthgraphicSize,
    System.Action newWorldCallback)
  {
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().ActiveBaseChangeSnapshot);
    ClusterManager.Instance.UpdateWorldReverbSnapshot(worldId);
    yield return (object) this.StartCoroutine(this.FadeWithBlack(false, 0.0f, 1f, 3f));
    ClusterManager.Instance.SetActiveWorld(worldId);
    if (useForcePosition)
    {
      CameraController.Instance.SetTargetPos(forcePosition, forceOrthgraphicSize, false);
      CameraController.Instance.SetPosition(forcePosition);
    }
    if (newWorldCallback != null)
      newWorldCallback();
    ManagementMenu.Instance.CloseAll();
    AudioMixer.instance.Stop(AudioMixerSnapshots.Get().ActiveBaseChangeSnapshot);
    yield return (object) this.StartCoroutine(this.FadeWithBlack(false, 1f, 0.0f, 3f));
  }

  public void SetWorldInteractive(bool state)
  {
    GameScreenManager.Instance.fadePlaneFront.raycastTarget = !state;
  }

  private IEnumerator FadeWithBlack(
    bool fadeUI,
    float startBlackPercent,
    float targetBlackPercent,
    float speed = 1f,
    System.Action callback = null)
  {
    return this.FadeWithColor(fadeUI, startBlackPercent, targetBlackPercent, Color.black, speed, callback);
  }

  private IEnumerator FadeWithColor(
    bool fadeUI,
    float startPercent,
    float targetPercent,
    Color color,
    float speed = 1f,
    System.Action callback = null)
  {
    Image fadePlane = fadeUI ? GameScreenManager.Instance.fadePlaneFront : GameScreenManager.Instance.fadePlaneBack;
    float percent = 0.0f;
    while ((double) percent < 1.0)
    {
      percent += Time.unscaledDeltaTime * speed;
      color.a = MathUtil.ReRange(percent, 0.0f, 1f, startPercent, targetPercent);
      fadePlane.color = color;
      yield return (object) SequenceUtil.WaitForNextFrame;
    }
    color.a = targetPercent;
    fadePlane.color = color;
    if (callback != null)
      callback();
    this.activeFadeRoutine = (Coroutine) null;
    yield return (object) SequenceUtil.WaitForNextFrame;
  }

  public void EnableFreeCamera(bool enable)
  {
    this.FreeCameraEnabled = enable;
    this.SetInfoText("Screenshot Mode (ESC to exit)");
  }

  private static bool WithinInputField()
  {
    UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
    if ((UnityEngine.Object) current == (UnityEngine.Object) null)
      return false;
    bool flag = false;
    if ((UnityEngine.Object) current.currentSelectedGameObject != (UnityEngine.Object) null && ((UnityEngine.Object) current.currentSelectedGameObject.GetComponent<KInputTextField>() != (UnityEngine.Object) null || (UnityEngine.Object) current.currentSelectedGameObject.GetComponent<InputField>() != (UnityEngine.Object) null))
      flag = true;
    return flag;
  }

  public static bool IsMouseOverGameWindow
  {
    get
    {
      return 0.0 <= (double) Input.mousePosition.x && 0.0 <= (double) Input.mousePosition.y && (double) Screen.width >= (double) Input.mousePosition.x && (double) Screen.height >= (double) Input.mousePosition.y;
    }
  }

  private void SetInfoText(string text)
  {
    this.infoText.text = text;
    this.infoText.color = this.infoText.color with
    {
      a = 0.5f
    };
  }

  public void OnKeyDown(KButtonEvent e)
  {
    if (e.Consumed || this.DisableUserCameraControl || CameraController.WithinInputField() || (UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null && SaveGame.Instance.GetComponent<UserNavigation>().Handle(e))
      return;
    if (!this.ChangeWorldInput(e))
    {
      if (e.TryConsume(Action.TogglePause))
        SpeedControlScreen.Instance.TogglePause(false);
      else if (e.TryConsume(Action.ZoomIn) && CameraController.IsMouseOverGameWindow)
      {
        this.targetOrthographicSize = Mathf.Max(this.targetOrthographicSize * (1f / this.zoomFactor), this.minOrthographicSize);
        this.overrideZoomSpeed = 0.0f;
        this.isTargetPosSet = false;
      }
      else if (e.TryConsume(Action.ZoomOut) && CameraController.IsMouseOverGameWindow)
      {
        this.targetOrthographicSize = Mathf.Min(this.targetOrthographicSize * this.zoomFactor, this.FreeCameraEnabled ? TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug : this.maxOrthographicSize);
        this.overrideZoomSpeed = 0.0f;
        this.isTargetPosSet = false;
      }
      else if (e.TryConsume(Action.MouseMiddle) || e.IsAction(Action.MouseRight))
      {
        this.panning = true;
        this.overrideZoomSpeed = 0.0f;
        this.isTargetPosSet = false;
      }
      else if (this.FreeCameraEnabled && e.TryConsume(Action.CinemaCamEnable))
      {
        this.cinemaCamEnabled = !this.cinemaCamEnabled;
        DebugUtil.LogArgs((object) "Cinema Cam Enabled ", (object) this.cinemaCamEnabled);
        this.SetInfoText(this.cinemaCamEnabled ? "Cinema Cam Enabled" : "Cinema Cam Disabled");
      }
      else if (this.FreeCameraEnabled && this.cinemaCamEnabled)
      {
        if (e.TryConsume(Action.CinemaToggleLock))
        {
          this.cinemaToggleLock = !this.cinemaToggleLock;
          DebugUtil.LogArgs((object) "Cinema Toggle Lock ", (object) this.cinemaToggleLock);
          this.SetInfoText(this.cinemaToggleLock ? "Cinema Input Lock ON" : "Cinema Input Lock OFF");
        }
        else if (e.TryConsume(Action.CinemaToggleEasing))
        {
          this.cinemaToggleEasing = !this.cinemaToggleEasing;
          DebugUtil.LogArgs((object) "Cinema Toggle Easing ", (object) this.cinemaToggleEasing);
          this.SetInfoText(this.cinemaToggleEasing ? "Cinema Easing ON" : "Cinema Easing OFF");
        }
        else if (e.TryConsume(Action.CinemaUnpauseOnMove))
        {
          this.cinemaUnpauseNextMove = !this.cinemaUnpauseNextMove;
          DebugUtil.LogArgs((object) "Cinema Unpause Next Move ", (object) this.cinemaUnpauseNextMove);
          this.SetInfoText(this.cinemaUnpauseNextMove ? "Cinema Unpause Next Move ON" : "Cinema Unpause Next Move OFF");
        }
        else if (e.TryConsume(Action.CinemaPanLeft))
        {
          this.cinemaPanLeft = !this.cinemaToggleLock || !this.cinemaPanLeft;
          this.cinemaPanRight = false;
          this.CheckMoveUnpause();
        }
        else if (e.TryConsume(Action.CinemaPanRight))
        {
          this.cinemaPanRight = !this.cinemaToggleLock || !this.cinemaPanRight;
          this.cinemaPanLeft = false;
          this.CheckMoveUnpause();
        }
        else if (e.TryConsume(Action.CinemaPanUp))
        {
          this.cinemaPanUp = !this.cinemaToggleLock || !this.cinemaPanUp;
          this.cinemaPanDown = false;
          this.CheckMoveUnpause();
        }
        else if (e.TryConsume(Action.CinemaPanDown))
        {
          this.cinemaPanDown = !this.cinemaToggleLock || !this.cinemaPanDown;
          this.cinemaPanUp = false;
          this.CheckMoveUnpause();
        }
        else if (e.TryConsume(Action.CinemaZoomIn))
        {
          this.cinemaZoomIn = !this.cinemaToggleLock || !this.cinemaZoomIn;
          this.cinemaZoomOut = false;
          this.CheckMoveUnpause();
        }
        else if (e.TryConsume(Action.CinemaZoomOut))
        {
          this.cinemaZoomOut = !this.cinemaToggleLock || !this.cinemaZoomOut;
          this.cinemaZoomIn = false;
          this.CheckMoveUnpause();
        }
        else if (e.TryConsume(Action.CinemaZoomSpeedPlus))
        {
          ++this.cinemaZoomSpeed;
          DebugUtil.LogArgs((object) "Cinema Zoom Speed ", (object) this.cinemaZoomSpeed);
          this.SetInfoText("Cinema Zoom Speed: " + this.cinemaZoomSpeed.ToString());
        }
        else if (e.TryConsume(Action.CinemaZoomSpeedMinus))
        {
          --this.cinemaZoomSpeed;
          DebugUtil.LogArgs((object) "Cinema Zoom Speed ", (object) this.cinemaZoomSpeed);
          this.SetInfoText("Cinema Zoom Speed: " + this.cinemaZoomSpeed.ToString());
        }
      }
      else if (e.TryConsume(Action.PanLeft))
        this.panLeft = true;
      else if (e.TryConsume(Action.PanRight))
        this.panRight = true;
      else if (e.TryConsume(Action.PanUp))
        this.panUp = true;
      else if (e.TryConsume(Action.PanDown))
        this.panDown = true;
    }
    if (e.Consumed || !((UnityEngine.Object) OverlayMenu.Instance != (UnityEngine.Object) null))
      return;
    OverlayMenu.Instance.OnKeyDown(e);
  }

  public bool ChangeWorldInput(KButtonEvent e)
  {
    if (e.Consumed)
      return true;
    int index = -1;
    if (e.TryConsume(Action.SwitchActiveWorld1))
      index = 0;
    else if (e.TryConsume(Action.SwitchActiveWorld2))
      index = 1;
    else if (e.TryConsume(Action.SwitchActiveWorld3))
      index = 2;
    else if (e.TryConsume(Action.SwitchActiveWorld4))
      index = 3;
    else if (e.TryConsume(Action.SwitchActiveWorld5))
      index = 4;
    else if (e.TryConsume(Action.SwitchActiveWorld6))
      index = 5;
    else if (e.TryConsume(Action.SwitchActiveWorld7))
      index = 6;
    else if (e.TryConsume(Action.SwitchActiveWorld8))
      index = 7;
    else if (e.TryConsume(Action.SwitchActiveWorld9))
      index = 8;
    else if (e.TryConsume(Action.SwitchActiveWorld10))
      index = 9;
    if (index == -1)
      return false;
    List<int> asteroidIdsSorted = ClusterManager.Instance.GetDiscoveredAsteroidIDsSorted();
    if (index < asteroidIdsSorted.Count && index >= 0)
    {
      int id = asteroidIdsSorted[index];
      WorldContainer world = ClusterManager.Instance.GetWorld(id);
      if ((UnityEngine.Object) world != (UnityEngine.Object) null && world.IsDiscovered && ClusterManager.Instance.activeWorldId != world.id)
      {
        ManagementMenu.Instance.CloseClusterMap();
        this.ActiveWorldStarWipe(world.id);
      }
    }
    return true;
  }

  public void OnKeyUp(KButtonEvent e)
  {
    if (this.DisableUserCameraControl || CameraController.WithinInputField())
      return;
    if (e.TryConsume(Action.MouseMiddle) || e.IsAction(Action.MouseRight))
      this.panning = false;
    else if (this.FreeCameraEnabled && this.cinemaCamEnabled)
    {
      if (e.TryConsume(Action.CinemaPanLeft))
        this.cinemaPanLeft = this.cinemaToggleLock && this.cinemaPanLeft;
      else if (e.TryConsume(Action.CinemaPanRight))
        this.cinemaPanRight = this.cinemaToggleLock && this.cinemaPanRight;
      else if (e.TryConsume(Action.CinemaPanUp))
        this.cinemaPanUp = this.cinemaToggleLock && this.cinemaPanUp;
      else if (e.TryConsume(Action.CinemaPanDown))
        this.cinemaPanDown = this.cinemaToggleLock && this.cinemaPanDown;
      else if (e.TryConsume(Action.CinemaZoomIn))
      {
        this.cinemaZoomIn = this.cinemaToggleLock && this.cinemaZoomIn;
      }
      else
      {
        if (!e.TryConsume(Action.CinemaZoomOut))
          return;
        this.cinemaZoomOut = this.cinemaToggleLock && this.cinemaZoomOut;
      }
    }
    else if (e.TryConsume(Action.CameraHome))
      this.CameraGoHome(showCameraReturnButton: true);
    else if (e.TryConsume(Action.PanLeft))
      this.panLeft = false;
    else if (e.TryConsume(Action.PanRight))
      this.panRight = false;
    else if (e.TryConsume(Action.PanUp))
    {
      this.panUp = false;
    }
    else
    {
      if (!e.TryConsume(Action.PanDown))
        return;
      this.panDown = false;
    }
  }

  public void ForcePanningState(bool state) => this.panning = false;

  public void CameraGoHome(float speed = 2f, bool showCameraReturnButton = false)
  {
    GameObject activeTelepad = GameUtil.GetActiveTelepad();
    if (!((UnityEngine.Object) activeTelepad != (UnityEngine.Object) null) || !ClusterUtil.ActiveWorldHasPrinter())
      return;
    GameUtil.FocusCamera(new Vector3(activeTelepad.transform.GetPosition().x, activeTelepad.transform.GetPosition().y + 1f, this.transform.GetPosition().z), speed, show_back_button: showCameraReturnButton);
    this.SetOverrideZoomSpeed(speed);
  }

  public void CameraGoTo(Vector3 pos, float speed = 2f, bool playSound = true)
  {
    pos.z = this.transform.GetPosition().z;
    this.SetTargetPos(pos, 10f, playSound);
    this.SetOverrideZoomSpeed(speed);
  }

  public void SnapTo(Vector3 pos)
  {
    this.ClearFollowTarget();
    pos.z = -100f;
    this.targetPos = Vector3.zero;
    this.isTargetPosSet = false;
    this.transform.SetPosition(pos);
    this.keyPanDelta = Vector3.zero;
    this.OrthographicSize = this.targetOrthographicSize;
  }

  public void SnapTo(Vector3 pos, float orthographicSize)
  {
    this.targetOrthographicSize = orthographicSize;
    this.SnapTo(pos);
  }

  public void SetOverrideZoomSpeed(float tempZoomSpeed) => this.overrideZoomSpeed = tempZoomSpeed;

  public void SetTargetPos(Vector3 pos, float orthographic_size, bool playSound)
  {
    int cell = Grid.PosToCell(pos);
    if (!Grid.IsValidCell(cell) || Grid.WorldIdx[cell] == byte.MaxValue || (UnityEngine.Object) ClusterManager.Instance.GetWorld((int) Grid.WorldIdx[cell]) == (UnityEngine.Object) null)
      return;
    this.ClearFollowTarget();
    if (playSound && !this.isTargetPosSet)
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Click_Notification"));
    pos.z = -100f;
    if ((int) Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
    {
      this.targetOrthographicSize = 20f;
      this.ActiveWorldStarWipe((int) Grid.WorldIdx[cell], pos, callback: (System.Action) (() =>
      {
        this.targetPos = pos;
        this.isTargetPosSet = true;
        this.OrthographicSize = orthographic_size + 5f;
        this.targetOrthographicSize = orthographic_size;
      }));
    }
    else
    {
      this.targetPos = pos;
      this.isTargetPosSet = true;
      this.targetOrthographicSize = orthographic_size;
    }
    PlayerController.Instance.CancelDragging();
    this.CheckMoveUnpause();
  }

  public void SetTargetPosForWorldChange(Vector3 pos, float orthographic_size, bool playSound)
  {
    int cell = Grid.PosToCell(pos);
    if (!Grid.IsValidCell(cell) || Grid.WorldIdx[cell] == byte.MaxValue || (UnityEngine.Object) ClusterManager.Instance.GetWorld((int) Grid.WorldIdx[cell]) == (UnityEngine.Object) null)
      return;
    this.ClearFollowTarget();
    if (playSound && !this.isTargetPosSet)
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Click_Notification"));
    pos.z = -100f;
    this.targetPos = pos;
    this.isTargetPosSet = true;
    this.targetOrthographicSize = orthographic_size;
    PlayerController.Instance.CancelDragging();
    this.CheckMoveUnpause();
    this.SetPosition(pos);
    this.OrthographicSize = orthographic_size;
  }

  public void SetMaxOrthographicSize(float size) => this.maxOrthographicSize = size;

  public void SetPosition(Vector3 pos) => this.transform.SetPosition(pos);

  public IEnumerator DoCinematicZoom(float targetOrthographicSize)
  {
    this.cinemaCamEnabled = true;
    this.FreeCameraEnabled = true;
    this.targetOrthographicSize = targetOrthographicSize;
    while ((double) targetOrthographicSize - (double) this.OrthographicSize >= 1.0 / 1000.0)
      yield return (object) SequenceUtil.WaitForEndOfFrame;
    this.OrthographicSize = targetOrthographicSize;
    this.FreeCameraEnabled = false;
    this.cinemaCamEnabled = false;
  }

  private Vector3 PointUnderCursor(Vector3 mousePos, Camera cam)
  {
    Ray ray = cam.ScreenPointToRay(mousePos);
    Vector3 direction = ray.direction;
    Vector3 vector3 = direction * Mathf.Abs(cam.transform.GetPosition().z / direction.z);
    return ray.origin + vector3;
  }

  private void CinemaCamUpdate()
  {
    float unscaledDeltaTime = Time.unscaledDeltaTime;
    Camera main = Camera.main;
    Vector3 localPosition = this.transform.GetLocalPosition();
    float num1 = Mathf.Pow((float) this.cinemaZoomSpeed, 3f);
    if (this.cinemaZoomIn)
    {
      this.overrideZoomSpeed = -num1 / TuningData<CameraController.Tuning>.Get().cinemaZoomFactor;
      this.isTargetPosSet = false;
    }
    else if (this.cinemaZoomOut)
    {
      this.overrideZoomSpeed = num1 / TuningData<CameraController.Tuning>.Get().cinemaZoomFactor;
      this.isTargetPosSet = false;
    }
    else
      this.overrideZoomSpeed = 0.0f;
    if (this.cinemaToggleEasing)
      this.cinemaZoomVelocity += (this.overrideZoomSpeed - this.cinemaZoomVelocity) * this.cinemaEasing;
    else
      this.cinemaZoomVelocity = this.overrideZoomSpeed;
    if ((double) this.cinemaZoomVelocity != 0.0)
    {
      this.OrthographicSize = main.orthographicSize + (float) ((double) this.cinemaZoomVelocity * (double) unscaledDeltaTime * ((double) main.orthographicSize / 20.0));
      this.targetOrthographicSize = main.orthographicSize;
    }
    float num2 = num1 / TuningData<CameraController.Tuning>.Get().cinemaZoomToFactor;
    float num3 = this.keyPanningSpeed / 20f * main.orthographicSize;
    float num4 = num3 * (num1 / TuningData<CameraController.Tuning>.Get().cinemaPanToFactor);
    if (!this.isTargetPosSet && (double) this.targetOrthographicSize != (double) main.orthographicSize)
    {
      float t = Mathf.Min(num2 * unscaledDeltaTime, 0.1f);
      this.OrthographicSize = Mathf.Lerp(main.orthographicSize, this.targetOrthographicSize, t);
    }
    Vector3 vector3_1 = Vector3.zero;
    if (this.isTargetPosSet)
    {
      float num5 = this.cinemaEasing * TuningData<CameraController.Tuning>.Get().targetZoomEasingFactor;
      float num6 = this.cinemaEasing * TuningData<CameraController.Tuning>.Get().targetPanEasingFactor;
      float f1 = this.targetOrthographicSize - main.orthographicSize;
      Vector3 vector3_2 = this.targetPos - localPosition;
      float num7;
      float num8;
      if (!this.cinemaToggleEasing)
      {
        num7 = num2 * unscaledDeltaTime;
        num8 = num4 * unscaledDeltaTime;
      }
      else
      {
        DebugUtil.LogArgs((object) "Min zoom of:", (object) (float) ((double) num2 * (double) unscaledDeltaTime), (object) (float) ((double) Mathf.Abs(f1) * (double) num5 * (double) unscaledDeltaTime));
        num7 = Mathf.Min(num2 * unscaledDeltaTime, Mathf.Abs(f1) * num5 * unscaledDeltaTime);
        DebugUtil.LogArgs((object) "Min pan of:", (object) (float) ((double) num4 * (double) unscaledDeltaTime), (object) (float) ((double) vector3_2.magnitude * (double) num6 * (double) unscaledDeltaTime));
        num8 = Mathf.Min(num4 * unscaledDeltaTime, vector3_2.magnitude * num6 * unscaledDeltaTime);
      }
      float f2 = (double) Mathf.Abs(f1) >= (double) num7 ? Mathf.Sign(f1) * num7 : f1;
      vector3_1 = (double) vector3_2.magnitude >= (double) num8 ? vector3_2.normalized * num8 : vector3_2;
      if ((double) Mathf.Abs(f2) < 1.0 / 1000.0 && (double) vector3_1.magnitude < 1.0 / 1000.0)
      {
        this.isTargetPosSet = false;
        f2 = f1;
        vector3_1 = vector3_2;
      }
      this.OrthographicSize = main.orthographicSize + f2 * (main.orthographicSize / 20f);
    }
    if (!PlayerController.Instance.CanDrag())
      this.panning = false;
    Vector3 vector3_3 = Vector3.zero;
    if (this.panning)
    {
      vector3_3 = -PlayerController.Instance.GetWorldDragDelta();
      this.isTargetPosSet = false;
      if ((double) vector3_3.magnitude > 0.0)
        this.ClearFollowTarget();
      this.keyPanDelta = Vector3.zero;
    }
    else
    {
      float num9 = num1 / TuningData<CameraController.Tuning>.Get().cinemaPanFactor;
      Vector3 zero = Vector3.zero;
      if (this.cinemaPanLeft)
      {
        this.ClearFollowTarget();
        zero.x = -num3 * num9;
        this.isTargetPosSet = false;
      }
      if (this.cinemaPanRight)
      {
        this.ClearFollowTarget();
        zero.x = num3 * num9;
        this.isTargetPosSet = false;
      }
      if (this.cinemaPanUp)
      {
        this.ClearFollowTarget();
        zero.y = num3 * num9;
        this.isTargetPosSet = false;
      }
      if (this.cinemaPanDown)
      {
        this.ClearFollowTarget();
        zero.y = -num3 * num9;
        this.isTargetPosSet = false;
      }
      if (this.cinemaToggleEasing)
        this.keyPanDelta += (zero - this.keyPanDelta) * this.cinemaEasing;
      else
        this.keyPanDelta = zero;
    }
    Vector3 position = localPosition + vector3_1 + vector3_3 + this.keyPanDelta * unscaledDeltaTime;
    if ((UnityEngine.Object) this.followTarget != (UnityEngine.Object) null)
    {
      position.x = this.followTargetPos.x;
      position.y = this.followTargetPos.y;
    }
    position.z = -100f;
    if ((double) (position - this.transform.GetLocalPosition()).magnitude <= 0.001)
      return;
    this.transform.SetLocalPosition(position);
  }

  private void NormalCamUpdate()
  {
    float unscaledDeltaTime = Time.unscaledDeltaTime;
    Camera main = Camera.main;
    this.smoothDt = (float) ((double) this.smoothDt * 2.0 / 3.0 + (double) unscaledDeltaTime / 3.0);
    float num1 = (double) this.overrideZoomSpeed != 0.0 ? this.overrideZoomSpeed : this.zoomSpeed;
    Vector3 localPosition = this.transform.GetLocalPosition();
    Vector3 vector3_1 = (double) this.overrideZoomSpeed != 0.0 ? new Vector3((float) Screen.width / 2f, (float) Screen.height / 2f, 0.0f) : KInputManager.GetMousePos();
    Vector3 position1 = this.PointUnderCursor(vector3_1, main);
    Vector3 viewportPoint1 = main.ScreenToViewportPoint(vector3_1);
    float num2 = this.keyPanningSpeed / 20f * main.orthographicSize * Mathf.Min(unscaledDeltaTime / 0.0166666657f, 10f);
    float t = num1 * Mathf.Min(this.smoothDt, 0.3f);
    this.OrthographicSize = Mathf.Lerp(main.orthographicSize, this.targetOrthographicSize, t);
    this.transform.SetLocalPosition(localPosition);
    Vector3 viewportPoint2 = main.WorldToViewportPoint(position1);
    viewportPoint1.z = viewportPoint2.z;
    Vector3 vector3_2 = main.ViewportToWorldPoint(viewportPoint2) - main.ViewportToWorldPoint(viewportPoint1);
    if (this.isTargetPosSet)
    {
      vector3_2 = Vector3.Lerp(localPosition, this.targetPos, num1 * this.smoothDt) - localPosition;
      if ((double) vector3_2.magnitude < 1.0 / 1000.0)
      {
        this.isTargetPosSet = false;
        vector3_2 = this.targetPos - localPosition;
      }
    }
    if (!PlayerController.Instance.CanDrag())
      this.panning = false;
    Vector3 vector3_3 = Vector3.zero;
    if (this.panning)
    {
      vector3_3 = -PlayerController.Instance.GetWorldDragDelta();
      this.isTargetPosSet = false;
    }
    Vector3 position2 = localPosition + vector3_2 + vector3_3;
    if (this.panning)
    {
      if ((double) vector3_3.magnitude > 0.0)
        this.ClearFollowTarget();
      this.keyPanDelta = Vector3.zero;
    }
    else if (!this.DisableUserCameraControl)
    {
      if (this.panLeft)
      {
        this.ClearFollowTarget();
        this.keyPanDelta.x -= num2;
        this.isTargetPosSet = false;
        this.overrideZoomSpeed = 0.0f;
      }
      if (this.panRight)
      {
        this.ClearFollowTarget();
        this.keyPanDelta.x += num2;
        this.isTargetPosSet = false;
        this.overrideZoomSpeed = 0.0f;
      }
      if (this.panUp)
      {
        this.ClearFollowTarget();
        this.keyPanDelta.y += num2;
        this.isTargetPosSet = false;
        this.overrideZoomSpeed = 0.0f;
      }
      if (this.panDown)
      {
        this.ClearFollowTarget();
        this.keyPanDelta.y -= num2;
        this.isTargetPosSet = false;
        this.overrideZoomSpeed = 0.0f;
      }
      if (KInputManager.currentControllerIsGamepad)
      {
        Vector2 vector2 = num2 * KInputManager.steamInputInterpreter.GetSteamCameraMovement();
        if ((double) Mathf.Abs(vector2.x) > (double) Mathf.Epsilon || (double) Mathf.Abs(vector2.y) > (double) Mathf.Epsilon)
        {
          this.ClearFollowTarget();
          this.isTargetPosSet = false;
          this.overrideZoomSpeed = 0.0f;
        }
        this.keyPanDelta += new Vector3(vector2.x, vector2.y, 0.0f);
      }
      Vector3 vector3_4 = new Vector3(Mathf.Lerp(0.0f, this.keyPanDelta.x, this.smoothDt * this.keyPanningEasing), Mathf.Lerp(0.0f, this.keyPanDelta.y, this.smoothDt * this.keyPanningEasing), 0.0f);
      this.keyPanDelta -= vector3_4;
      position2.x += vector3_4.x;
      position2.y += vector3_4.y;
    }
    if ((UnityEngine.Object) this.followTarget != (UnityEngine.Object) null)
    {
      position2.x = this.followTargetPos.x;
      position2.y = this.followTargetPos.y;
    }
    position2.z = -100f;
    if ((double) (position2 - this.transform.GetLocalPosition()).magnitude <= 0.001)
      return;
    this.transform.SetLocalPosition(position2);
  }

  private void Update()
  {
    if ((UnityEngine.Object) Game.Instance == (UnityEngine.Object) null || !Game.Instance.timelapser.CapturingTimelapseScreenshot)
    {
      if (this.FreeCameraEnabled && this.cinemaCamEnabled)
        this.CinemaCamUpdate();
      else
        this.NormalCamUpdate();
    }
    if ((UnityEngine.Object) this.infoText != (UnityEngine.Object) null && (double) this.infoText.color.a > 0.0)
      this.infoText.color = this.infoText.color with
      {
        a = Mathf.Max(0.0f, this.infoText.color.a - Time.unscaledDeltaTime * 0.5f)
      };
    this.ConstrainToWorld();
    Vector3 vector3 = this.PointUnderCursor(KInputManager.GetMousePos(), Camera.main);
    Shader.SetGlobalVector("_WorldCameraPos", new Vector4(this.transform.GetPosition().x, this.transform.GetPosition().y, this.transform.GetPosition().z, Camera.main.orthographicSize));
    Shader.SetGlobalVector("_WorldCursorPos", new Vector4(vector3.x, vector3.y, 0.0f, 0.0f));
    this.VisibleArea.Update();
    this.soundCuller = SoundCuller.CreateCuller();
  }

  private Vector3 GetFollowPos()
  {
    if (!((UnityEngine.Object) this.followTarget != (UnityEngine.Object) null))
      return Vector3.zero;
    Vector3 followPos = this.followTarget.transform.GetPosition();
    KAnimControllerBase component = this.followTarget.GetComponent<KAnimControllerBase>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      followPos = component.GetWorldPivot();
    return followPos;
  }

  public static float GetHighestVisibleCell_Height(byte worldID = 255 /*0xFF*/)
  {
    Vector2 zero = Vector2.zero;
    Vector2 vector2_1 = new Vector2(Grid.WidthInMeters, Grid.HeightInMeters);
    Camera main = Camera.main;
    float orthographicSize = main.orthographicSize;
    main.orthographicSize = 20f;
    Ray ray = main.ViewportPointToRay(Vector3.one - Vector3.one * 0.33f);
    Vector3 vector3 = CameraController.Instance.transform.GetPosition() - ray.origin;
    main.orthographicSize = orthographicSize;
    if ((UnityEngine.Object) ClusterManager.Instance != (UnityEngine.Object) null)
    {
      WorldContainer worldContainer = worldID == byte.MaxValue ? ClusterManager.Instance.activeWorld : ClusterManager.Instance.GetWorld((int) worldID);
      Vector2 vector2_2 = worldContainer.minimumBounds * Grid.CellSizeInMeters;
      vector2_1 = worldContainer.maximumBounds * Grid.CellSizeInMeters;
      Vector2 vector2_3 = new Vector2((float) worldContainer.Width, (float) worldContainer.Height) * Grid.CellSizeInMeters;
    }
    return (float) ((double) vector2_1.y * 1.1000000238418579 + 20.0) + vector3.y;
  }

  private void ConstrainToWorld()
  {
    if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null && Game.Instance.IsLoading() || this.FreeCameraEnabled)
      return;
    Camera main = Camera.main;
    Ray ray1 = main.ViewportPointToRay(Vector3.zero + Vector3.one * 0.33f);
    Ray ray2 = main.ViewportPointToRay(Vector3.one - Vector3.one * 0.33f);
    float distance1 = Mathf.Abs(ray1.origin.z / ray1.direction.z);
    float distance2 = Mathf.Abs(ray2.origin.z / ray2.direction.z);
    Vector3 point1 = ray1.GetPoint(distance1);
    Vector3 point2 = ray2.GetPoint(distance2);
    Vector2 vector2_1 = Vector2.zero;
    Vector2 vector2_2 = new Vector2(Grid.WidthInMeters, Grid.HeightInMeters);
    Vector2 vector2_3 = vector2_2;
    if ((UnityEngine.Object) ClusterManager.Instance != (UnityEngine.Object) null)
    {
      WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
      vector2_1 = activeWorld.minimumBounds * Grid.CellSizeInMeters;
      vector2_2 = activeWorld.maximumBounds * Grid.CellSizeInMeters;
      vector2_3 = new Vector2((float) activeWorld.Width, (float) activeWorld.Height) * Grid.CellSizeInMeters;
    }
    if ((double) point2.x - (double) point1.x > (double) vector2_3.x || (double) point2.y - (double) point1.y > (double) vector2_3.y)
      return;
    Vector3 vector3_1 = this.transform.GetPosition() - ray1.origin;
    Vector3 vector3_2 = point1;
    vector3_2.x = Mathf.Max(vector2_1.x, vector3_2.x);
    vector3_2.y = Mathf.Max(vector2_1.y * Grid.CellSizeInMeters, vector3_2.y);
    ray1.origin = vector3_2;
    ray1.direction = -ray1.direction;
    this.transform.SetPosition(ray1.GetPoint(distance1) + vector3_1);
    Vector3 vector3_3 = this.transform.GetPosition() - ray2.origin;
    Vector3 vector3_4 = point2;
    vector3_4.x = Mathf.Min(vector2_2.x, vector3_4.x);
    vector3_4.y = Mathf.Min(vector2_2.y * 1.1f, vector3_4.y);
    ray2.origin = vector3_4;
    ray2.direction = -ray2.direction;
    vector3_4 = ray2.GetPoint(distance2);
    this.transform.SetPosition((vector3_4 + vector3_3) with
    {
      z = -100f
    });
  }

  public void Save(BinaryWriter writer)
  {
    writer.Write(this.transform.GetPosition());
    writer.Write(this.transform.localScale);
    writer.Write(this.transform.rotation);
    writer.Write(this.targetOrthographicSize);
    CameraSaveData.position = this.transform.GetPosition();
    CameraSaveData.localScale = this.transform.localScale;
    CameraSaveData.rotation = this.transform.rotation;
  }

  private void Restore()
  {
    if (!CameraSaveData.valid)
      return;
    int cell = Grid.PosToCell(CameraSaveData.position);
    if (Grid.IsValidCell(cell) && !Grid.IsVisible(cell))
    {
      Debug.LogWarning((object) "Resetting Camera Position... camera was saved in an undiscovered area of the map.");
      this.CameraGoHome();
    }
    else
    {
      this.transform.SetPosition(CameraSaveData.position);
      this.transform.localScale = CameraSaveData.localScale;
      this.transform.rotation = CameraSaveData.rotation;
      this.targetOrthographicSize = Mathf.Clamp(CameraSaveData.orthographicsSize, this.minOrthographicSize, this.FreeCameraEnabled ? TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug : this.maxOrthographicSize);
      this.SnapTo(this.transform.GetPosition());
    }
  }

  private void OnMRTSetupComplete(Camera cam)
  {
    this.cameras.Add(cam);
    cam.gameObject.AddComponent<MRTLiquidInterception>();
  }

  public bool IsAudibleSound(Vector2 pos) => this.soundCuller.IsAudible(pos);

  public bool IsAudibleSound(Vector3 pos, EventReference event_ref)
  {
    float max;
    int minMaxDistance = (int) RuntimeManager.GetEventDescription(event_ref).getMinMaxDistance(out float _, out max);
    if ((double) max == 0.0)
      max = 60f;
    return this.soundCuller.IsAudible((Vector2) pos, max * max);
  }

  public bool IsAudibleSound(Vector3 pos, HashedString sound_path)
  {
    return this.soundCuller.IsAudible((Vector2) pos, sound_path);
  }

  public Vector3 GetVerticallyScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
  {
    return this.soundCuller.GetVerticallyScaledPosition(pos, objectIsSelectedAndVisible);
  }

  public bool IsVisiblePos(Vector3 pos) => this.VisibleArea.CurrentArea.Contains(pos);

  public bool IsVisiblePosExtended(Vector3 pos)
  {
    return this.VisibleArea.CurrentAreaExtended.Contains(pos);
  }

  protected override void OnCleanUp() => CameraController.Instance = (CameraController) null;

  public void SetFollowTarget(Transform follow_target)
  {
    this.ClearFollowTarget();
    if ((UnityEngine.Object) follow_target == (UnityEngine.Object) null)
      return;
    this.followTarget = follow_target;
    this.OrthographicSize = 6f;
    this.targetOrthographicSize = 6f;
    Vector3 followPos = this.GetFollowPos();
    this.followTargetPos = new Vector3(followPos.x, followPos.y, this.transform.GetPosition().z);
    this.transform.SetPosition(this.followTargetPos);
    this.followTarget.GetComponent<KMonoBehaviour>().Trigger(-1506069671, (object) null);
  }

  public void ClearFollowTarget()
  {
    if ((UnityEngine.Object) this.followTarget == (UnityEngine.Object) null)
      return;
    this.followTarget.GetComponent<KMonoBehaviour>().Trigger(-485480405, (object) null);
    this.followTarget = (Transform) null;
  }

  public void UpdateFollowTarget()
  {
    if (!((UnityEngine.Object) this.followTarget != (UnityEngine.Object) null))
      return;
    Vector3 followPos = this.GetFollowPos();
    Vector2 a = new Vector2(this.transform.GetLocalPosition().x, this.transform.GetLocalPosition().y);
    byte worldIdx = Grid.WorldIdx[Grid.PosToCell(followPos)];
    if (ClusterManager.Instance.activeWorldId != (int) worldIdx)
    {
      Transform followTarget = this.followTarget;
      this.SetFollowTarget((Transform) null);
      ClusterManager.Instance.SetActiveWorld((int) worldIdx);
      this.SetFollowTarget(followTarget);
    }
    else
    {
      Vector2 vector2 = Vector2.Lerp(a, (Vector2) followPos, Time.unscaledDeltaTime * 25f);
      this.followTargetPos = new Vector3(vector2.x, vector2.y, this.transform.GetLocalPosition().z);
    }
  }

  public void RenderForTimelapser(ref RenderTexture tex)
  {
    this.RenderCameraForTimelapse(this.baseCamera, ref tex, this.timelapseCameraCullingMask);
    CameraClearFlags clearFlags = this.overlayCamera.clearFlags;
    this.overlayCamera.clearFlags = CameraClearFlags.Nothing;
    this.RenderCameraForTimelapse(this.overlayCamera, ref tex, this.timelapseOverlayCameraCullingMask);
    this.overlayCamera.clearFlags = clearFlags;
  }

  private void RenderCameraForTimelapse(
    Camera cam,
    ref RenderTexture tex,
    LayerMask mask,
    float overrideAspect = -1f)
  {
    int cullingMask = cam.cullingMask;
    RenderTexture targetTexture = cam.targetTexture;
    cam.targetTexture = tex;
    cam.aspect = (float) tex.width / (float) tex.height;
    if ((double) overrideAspect != -1.0)
      cam.aspect = overrideAspect;
    if ((int) mask != -1)
      cam.cullingMask = (int) mask;
    cam.Render();
    cam.ResetAspect();
    cam.cullingMask = cullingMask;
    cam.targetTexture = targetTexture;
  }

  private void CheckMoveUnpause()
  {
    if (!this.cinemaCamEnabled || !this.cinemaUnpauseNextMove)
      return;
    this.cinemaUnpauseNextMove = !this.cinemaUnpauseNextMove;
    if (!SpeedControlScreen.Instance.IsPaused)
      return;
    SpeedControlScreen.Instance.Unpause(false);
  }

  public class Tuning : TuningData<CameraController.Tuning>
  {
    public float maxOrthographicSizeDebug;
    public float cinemaZoomFactor = 100f;
    public float cinemaPanFactor = 50f;
    public float cinemaZoomToFactor = 100f;
    public float cinemaPanToFactor = 50f;
    public float targetZoomEasingFactor = 400f;
    public float targetPanEasingFactor = 100f;
  }
}
