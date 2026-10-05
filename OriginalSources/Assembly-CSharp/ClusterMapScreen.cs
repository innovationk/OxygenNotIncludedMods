// Decompiled with JetBrains decompiler
// Type: ClusterMapScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class ClusterMapScreen : KScreen
{
  public static ClusterMapScreen Instance;
  public GameObject cellVisContainer;
  public GameObject terrainVisContainer;
  public GameObject mobileVisContainer;
  public GameObject telescopeVisContainer;
  public GameObject POIVisContainer;
  public GameObject DebriVisContainer;
  public GameObject FXVisContainer;
  public ClusterMapVisualizer cellVisPrefab;
  public ClusterMapVisualizer terrainVisPrefab;
  public ClusterMapVisualizer mobileVisPrefab;
  public ClusterMapVisualizer staticVisPrefab;
  public Color rocketPathColor;
  public Color rocketSelectedPathColor;
  public Color rocketPreviewPathColor;
  private ClusterMapHex m_selectedHex;
  private ClusterMapHex m_hoveredHex;
  private ClusterGridEntity m_selectedEntity;
  public KButton closeButton;
  private const float ZOOM_SCALE_MIN = 50f;
  private const float ZOOM_SCALE_MAX = 150f;
  private const float ZOOM_SCALE_INCREMENT = 25f;
  private const float ZOOM_SCALE_SPEED = 4f;
  private const float ZOOM_NAME_THRESHOLD = 115f;
  private float m_currentZoomScale = 75f;
  private float m_targetZoomScale = 75f;
  private ClusterMapPath m_previewMapPath;
  private Dictionary<ClusterGridEntity, ClusterMapVisualizer> m_gridEntityVis = new Dictionary<ClusterGridEntity, ClusterMapVisualizer>();
  private Dictionary<ClusterGridEntity, ClusterMapVisualizer> m_gridEntityAnims = new Dictionary<ClusterGridEntity, ClusterMapVisualizer>();
  private Dictionary<AxialI, ClusterMapVisualizer> m_cellVisByLocation = new Dictionary<AxialI, ClusterMapVisualizer>();
  private Action<object> m_onDestinationChangedDelegate;
  private Action<object> m_onSelectObjectDelegate;
  [SerializeField]
  private KScrollRect mapScrollRect;
  [SerializeField]
  private float scrollSpeed = 15f;
  public GameObject selectMarkerPrefab;
  public ClusterMapPathDrawer pathDrawer;
  private SelectMarker m_selectMarker;
  private bool movingToTargetNISPosition;
  private Vector3 targetNISPosition;
  private float targetNISZoom;
  private AxialI selectOnMoveNISComplete;
  private ClusterMapScreen.Mode m_mode;
  private ClusterDestinationSelector m_destinationSelector;
  private bool m_closeOnSelect;
  private Coroutine activeMoveToTargetRoutine;
  public float floatCycleScale = 4f;
  public float floatCycleOffset = 0.75f;
  public float floatCycleSpeed = 0.75f;

  public static void DestroyInstance() => ClusterMapScreen.Instance = (ClusterMapScreen) null;

  public ClusterMapVisualizer GetEntityVisAnim(ClusterGridEntity entity)
  {
    return this.m_gridEntityAnims.ContainsKey(entity) ? this.m_gridEntityAnims[entity] : (ClusterMapVisualizer) null;
  }

  public override float GetSortKey() => this.isEditing ? 50f : 20f;

  public float CurrentZoomPercentage()
  {
    return (float) (((double) this.m_currentZoomScale - 50.0) / 100.0);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.m_selectMarker = Util.KInstantiateUI<SelectMarker>(this.selectMarkerPrefab, this.gameObject);
    this.m_selectMarker.gameObject.SetActive(false);
    ClusterMapScreen.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Debug.Assert(this.cellVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), (object) "The radius of the cellVisPrefab hex must be 1");
    Debug.Assert(this.terrainVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), (object) "The radius of the terrainVisPrefab hex must be 1");
    Debug.Assert(this.mobileVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), (object) "The radius of the mobileVisPrefab hex must be 1");
    Debug.Assert(this.staticVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), (object) "The radius of the staticVisPrefab hex must be 1");
    int maxR;
    int maxQ;
    this.GenerateGridVis(out int _, out maxR, out int _, out maxQ);
    this.Show(false);
    this.mapScrollRect.content.sizeDelta = new Vector2((float) (maxR * 4), (float) (maxQ * 4));
    this.mapScrollRect.content.localScale = new Vector3(this.m_currentZoomScale, this.m_currentZoomScale, 1f);
    this.m_onDestinationChangedDelegate = new Action<object>(this.OnDestinationChanged);
    this.m_onSelectObjectDelegate = new Action<object>(this.OnSelectObject);
    this.Subscribe(1980521255, (Action<object>) (_ => this.UpdateVis()));
  }

  private void RefreshHexCellsInventoryVisuals()
  {
    foreach (Component component in StarmapHexCellInventory.AllInventories.Values)
      component.GetComponent<StarmapHexCellInventoryVisuals>().RefreshVisuals();
  }

  protected void MoveToNISPosition()
  {
    if (!this.movingToTargetNISPosition)
      return;
    Vector3 b = new Vector3(-this.targetNISPosition.x * this.mapScrollRect.content.localScale.x, -this.targetNISPosition.y * this.mapScrollRect.content.localScale.y, this.targetNISPosition.z);
    this.m_targetZoomScale = Mathf.Lerp(this.m_targetZoomScale, this.targetNISZoom, Time.unscaledDeltaTime * 2f);
    this.mapScrollRect.content.SetLocalPosition(Vector3.Lerp(this.mapScrollRect.content.GetLocalPosition(), b, Time.unscaledDeltaTime * 2.5f));
    float num = Vector3.Distance(this.mapScrollRect.content.GetLocalPosition(), b);
    if ((double) num >= 100.0)
      return;
    ClusterMapHex component = this.m_cellVisByLocation[this.selectOnMoveNISComplete].GetComponent<ClusterMapHex>();
    if ((UnityEngine.Object) this.m_selectedHex != (UnityEngine.Object) component)
      this.SelectHex(component);
    if ((double) num >= 10.0)
      return;
    this.movingToTargetNISPosition = false;
  }

  public void SetTargetFocusPosition(AxialI targetPosition, float delayBeforeMove = 0.5f)
  {
    if (this.activeMoveToTargetRoutine != null)
      this.StopCoroutine(this.activeMoveToTargetRoutine);
    this.activeMoveToTargetRoutine = this.StartCoroutine(this.MoveToTargetRoutine(targetPosition, delayBeforeMove));
  }

  private IEnumerator MoveToTargetRoutine(AxialI targetPosition, float delayBeforeMove)
  {
    delayBeforeMove = Mathf.Max(delayBeforeMove, 0.0f);
    yield return (object) SequenceUtil.WaitForSecondsRealtime(delayBeforeMove);
    this.targetNISPosition = AxialUtil.AxialToWorld((float) targetPosition.r, (float) targetPosition.q);
    this.targetNISZoom = 150f;
    this.movingToTargetNISPosition = true;
    this.selectOnMoveNISComplete = targetPosition;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (!e.Consumed && (e.IsAction(Action.ZoomIn) || e.IsAction(Action.ZoomOut)) && CameraController.IsMouseOverGameWindow)
    {
      List<RaycastResult> raycastResults = new List<RaycastResult>();
      PointerEventData eventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
      eventData.position = (Vector2) KInputManager.GetMousePos();
      UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
      if ((UnityEngine.Object) current != (UnityEngine.Object) null)
      {
        current.RaycastAll(eventData, raycastResults);
        bool flag = false;
        foreach (RaycastResult raycastResult in raycastResults)
        {
          if (!raycastResult.gameObject.transform.IsChildOf(this.transform))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          this.m_targetZoomScale = Mathf.Clamp(this.m_targetZoomScale + (!KInputManager.currentControllerIsGamepad ? Input.mouseScrollDelta.y * 25f : (float) (25.0 * (e.IsAction(Action.ZoomIn) ? 1.0 : -1.0))), 50f, 150f);
          e.TryConsume(Action.ZoomIn);
          if (!e.Consumed)
            e.TryConsume(Action.ZoomOut);
        }
      }
    }
    CameraController.Instance.ChangeWorldInput(e);
    base.OnKeyDown(e);
  }

  public bool TryHandleCancel()
  {
    if (this.m_mode != ClusterMapScreen.Mode.SelectDestination || this.m_closeOnSelect)
      return false;
    ClusterDestinationSelector destinationSelector = this.m_destinationSelector;
    this.SetMode(ClusterMapScreen.Mode.Default);
    if ((UnityEngine.Object) destinationSelector != (UnityEngine.Object) null)
      destinationSelector.Trigger(94158097, (object) null);
    return true;
  }

  public void ShowInSelectDestinationMode(ClusterDestinationSelector destination_selector)
  {
    this.m_destinationSelector = destination_selector;
    if (!this.IsScreenActive())
    {
      ManagementMenu.Instance.ToggleClusterMap();
      this.m_closeOnSelect = true;
    }
    this.SetSelectedEntity(destination_selector.GetComponent<ClusterGridEntity>());
    this.m_selectedHex = !((UnityEngine.Object) this.m_selectedEntity != (UnityEngine.Object) null) ? this.m_cellVisByLocation[destination_selector.GetMyWorldLocation()].GetComponent<ClusterMapHex>() : this.m_cellVisByLocation[this.m_selectedEntity.Location].GetComponent<ClusterMapHex>();
    this.SetMode(ClusterMapScreen.Mode.SelectDestination);
  }

  private void SetMode(ClusterMapScreen.Mode mode)
  {
    this.m_mode = mode;
    if (this.m_mode == ClusterMapScreen.Mode.Default)
      this.m_destinationSelector = (ClusterDestinationSelector) null;
    this.UpdateVis();
  }

  public ClusterMapScreen.Mode GetMode() => this.m_mode;

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (show)
    {
      this.MoveToNISPosition();
      this.UpdateVis(true);
      Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
      Game.Instance.Subscribe(-1554423969, new Action<object>(this.OnNewTelescopeTarget));
      Game.Instance.Subscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
      ClusterMapSelectTool.Instance.Activate();
      this.SetShowingNonClusterMapHud(false);
      CameraController.Instance.DisableUserCameraControl = true;
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().MENUStarmapNotPausedSnapshot);
      MusicManager.instance.PlaySong("Music_Starmap");
      this.UpdateTearStatus();
      this.RefreshHexCellsInventoryVisuals();
    }
    else
    {
      Game.Instance.Unsubscribe(-1554423969, new Action<object>(this.OnNewTelescopeTarget));
      Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
      Game.Instance.Unsubscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
      this.m_mode = ClusterMapScreen.Mode.Default;
      this.m_closeOnSelect = false;
      this.m_destinationSelector = (ClusterDestinationSelector) null;
      SelectTool.Instance.Activate();
      this.SetShowingNonClusterMapHud(true);
      CameraController.Instance.DisableUserCameraControl = false;
      AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUStarmapNotPausedSnapshot);
      if (!MusicManager.instance.SongIsPlaying("Music_Starmap"))
        return;
      MusicManager.instance.StopSong("Music_Starmap");
    }
  }

  private void SetShowingNonClusterMapHud(bool show)
  {
    PlanScreen.Instance.gameObject.SetActive(show);
    ToolMenu.Instance.gameObject.SetActive(show);
    OverlayScreen.Instance.gameObject.SetActive(show);
  }

  private void SetSelectedEntity(ClusterGridEntity entity, bool frameDelay = false)
  {
    if ((UnityEngine.Object) this.m_selectedEntity != (UnityEngine.Object) null)
    {
      this.m_selectedEntity.Unsubscribe(543433792, this.m_onDestinationChangedDelegate);
      this.m_selectedEntity.Unsubscribe(-1503271301, this.m_onSelectObjectDelegate);
    }
    this.m_selectedEntity = entity;
    if ((UnityEngine.Object) this.m_selectedEntity != (UnityEngine.Object) null)
    {
      this.m_selectedEntity.Subscribe(543433792, this.m_onDestinationChangedDelegate);
      this.m_selectedEntity.Subscribe(-1503271301, this.m_onSelectObjectDelegate);
    }
    KSelectable component = (UnityEngine.Object) this.m_selectedEntity != (UnityEngine.Object) null ? this.m_selectedEntity.GetComponent<KSelectable>() : (KSelectable) null;
    if (frameDelay)
      ClusterMapSelectTool.Instance.SelectNextFrame(component);
    else
      ClusterMapSelectTool.Instance.Select(component);
  }

  private void OnDestinationChanged(object _) => this.UpdateVis();

  private void OnSelectObject(object _)
  {
    if ((UnityEngine.Object) this.m_selectedEntity == (UnityEngine.Object) null)
      return;
    KSelectable component = this.m_selectedEntity.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || component.IsSelected)
      return;
    this.SetSelectedEntity((ClusterGridEntity) null);
    if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
    {
      if (this.m_closeOnSelect)
        ManagementMenu.Instance.CloseAll();
      else
        this.SetMode(ClusterMapScreen.Mode.Default);
    }
    this.UpdateVis();
  }

  private void OnFogOfWarRevealed(object _ = null) => this.UpdateVis();

  private void OnNewTelescopeTarget(object _ = null) => this.UpdateVis();

  private void Update()
  {
    if (!KInputManager.currentControllerIsGamepad || !this.IsScreenActive())
      return;
    this.mapScrollRect.AnalogUpdate(KInputManager.steamInputInterpreter.GetSteamCameraMovement() * this.scrollSpeed);
  }

  private void GenerateGridVis(out int minR, out int maxR, out int minQ, out int maxQ)
  {
    minR = int.MaxValue;
    maxR = int.MinValue;
    minQ = int.MaxValue;
    maxQ = int.MinValue;
    foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> cellContent in ClusterGrid.Instance.cellContents)
    {
      ClusterMapVisualizer cmp = UnityEngine.Object.Instantiate<ClusterMapVisualizer>(this.cellVisPrefab, Vector3.zero, Quaternion.identity, this.cellVisContainer.transform);
      RectTransform transform = cmp.rectTransform();
      AxialI axialI = cellContent.Key;
      Vector3 world = axialI.ToWorld();
      transform.SetLocalPosition(world);
      cmp.gameObject.SetActive(true);
      ClusterMapHex component = cmp.GetComponent<ClusterMapHex>();
      component.SetLocation(cellContent.Key);
      this.m_cellVisByLocation.Add(cellContent.Key, cmp);
      ref int local1 = ref minR;
      int a1 = minR;
      axialI = component.location;
      int r1 = axialI.R;
      int num1 = Mathf.Min(a1, r1);
      local1 = num1;
      ref int local2 = ref maxR;
      int a2 = maxR;
      axialI = component.location;
      int r2 = axialI.R;
      int num2 = Mathf.Max(a2, r2);
      local2 = num2;
      ref int local3 = ref minQ;
      int a3 = minQ;
      axialI = component.location;
      int q1 = axialI.Q;
      int num3 = Mathf.Min(a3, q1);
      local3 = num3;
      ref int local4 = ref maxQ;
      int a4 = maxQ;
      axialI = component.location;
      int q2 = axialI.Q;
      int num4 = Mathf.Max(a4, q2);
      local4 = num4;
    }
    this.SetupVisGameObjects();
    this.UpdateVis();
  }

  public Transform GetGridEntityNameTarget(ClusterGridEntity entity)
  {
    ClusterMapVisualizer clusterMapVisualizer;
    return (double) this.m_currentZoomScale >= 115.0 && this.m_gridEntityVis.TryGetValue(entity, out clusterMapVisualizer) ? clusterMapVisualizer.nameTarget : (Transform) null;
  }

  public override void ScreenUpdate(bool topLevel)
  {
    this.m_currentZoomScale = Mathf.Lerp(this.m_currentZoomScale, this.m_targetZoomScale, Mathf.Min(4f * Time.unscaledDeltaTime, 0.9f));
    Vector2 mousePos = (Vector2) KInputManager.GetMousePos();
    Vector3 vector3_1 = this.mapScrollRect.content.InverseTransformPoint((Vector3) mousePos);
    this.mapScrollRect.content.localScale = new Vector3(this.m_currentZoomScale, this.m_currentZoomScale, 1f);
    Vector3 vector3_2 = this.mapScrollRect.content.InverseTransformPoint((Vector3) mousePos);
    RectTransform content = this.mapScrollRect.content;
    content.localPosition = content.localPosition + (vector3_2 - vector3_1) * this.m_currentZoomScale;
    this.MoveToNISPosition();
    this.FloatyAsteroidAnimation();
  }

  private void FloatyAsteroidAnimation()
  {
    float num = 0.0f;
    foreach (Component worldContainer in ClusterManager.Instance.WorldContainers)
    {
      AsteroidGridEntity component = worldContainer.GetComponent<AsteroidGridEntity>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && this.m_gridEntityVis.ContainsKey((ClusterGridEntity) component) && ClusterMapScreen.GetRevealLevel((ClusterGridEntity) component) == ClusterRevealLevel.Visible)
        this.m_gridEntityVis[(ClusterGridEntity) component].GetFirstAnimController().Offset = (Vector3) new Vector2(0.0f, this.floatCycleOffset + this.floatCycleScale * Mathf.Sin(this.floatCycleSpeed * (num + GameClock.Instance.GetTime())));
      ++num;
    }
  }

  private void SetupVisGameObjects()
  {
    foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> cellContent in ClusterGrid.Instance.cellContents)
    {
      foreach (ClusterGridEntity clusterGridEntity in cellContent.Value)
      {
        int cellRevealLevel = (int) ClusterGrid.Instance.GetCellRevealLevel(cellContent.Key);
        int isVisibleInFow = (int) clusterGridEntity.IsVisibleInFOW;
        ClusterRevealLevel revealLevel = ClusterMapScreen.GetRevealLevel(clusterGridEntity);
        if (clusterGridEntity.IsVisible && revealLevel != ClusterRevealLevel.Hidden && !this.m_gridEntityVis.ContainsKey(clusterGridEntity))
        {
          ClusterMapVisualizer original = (ClusterMapVisualizer) null;
          GameObject gameObject = (GameObject) null;
          switch (clusterGridEntity.Layer)
          {
            case EntityLayer.Asteroid:
              original = this.terrainVisPrefab;
              gameObject = this.terrainVisContainer;
              break;
            case EntityLayer.Craft:
              original = this.mobileVisPrefab;
              gameObject = this.mobileVisContainer;
              break;
            case EntityLayer.POI:
              original = this.staticVisPrefab;
              gameObject = this.POIVisContainer;
              break;
            case EntityLayer.Telescope:
              original = this.staticVisPrefab;
              gameObject = this.telescopeVisContainer;
              break;
            case EntityLayer.Payload:
            case EntityLayer.Meteor:
              original = this.mobileVisPrefab;
              gameObject = this.mobileVisContainer;
              break;
            case EntityLayer.FX:
              original = this.staticVisPrefab;
              gameObject = this.FXVisContainer;
              break;
            case EntityLayer.Debri:
              original = this.staticVisPrefab;
              gameObject = this.DebriVisContainer;
              break;
          }
          ClusterNameDisplayScreen.Instance.AddNewEntry(clusterGridEntity);
          ClusterMapVisualizer clusterMapVisualizer = UnityEngine.Object.Instantiate<ClusterMapVisualizer>(original, gameObject.transform);
          clusterMapVisualizer.Init(clusterGridEntity, this.pathDrawer);
          clusterMapVisualizer.gameObject.SetActive(true);
          this.m_gridEntityAnims.Add(clusterGridEntity, clusterMapVisualizer);
          this.m_gridEntityVis.Add(clusterGridEntity, clusterMapVisualizer);
          clusterGridEntity.positionDirty = false;
          clusterGridEntity.Subscribe(1502190696, new Action<object>(this.RemoveDeletedEntities));
        }
      }
    }
    this.RemoveDeletedEntities();
    foreach (KeyValuePair<ClusterGridEntity, ClusterMapVisualizer> gridEntityVi in this.m_gridEntityVis)
    {
      ClusterGridEntity key = gridEntityVi.Key;
      if (key.Layer == EntityLayer.Asteroid)
      {
        int id = key.GetComponent<WorldContainer>().id;
        gridEntityVi.Value.alertVignette.worldID = id;
      }
    }
  }

  private void RemoveDeletedEntities(object obj = null)
  {
    foreach (ClusterGridEntity key in this.m_gridEntityVis.Keys.Where<ClusterGridEntity>((Func<ClusterGridEntity, bool>) (x => (UnityEngine.Object) x == (UnityEngine.Object) null || (UnityEngine.Object) x.gameObject == (UnityEngine.Object) obj)).ToList<ClusterGridEntity>())
    {
      Util.KDestroyGameObject((Component) this.m_gridEntityVis[key]);
      this.m_gridEntityVis.Remove(key);
      this.m_gridEntityAnims.Remove(key);
    }
  }

  private void OnClusterLocationChanged(object data) => this.UpdateVis();

  public static ClusterRevealLevel GetRevealLevel(ClusterGridEntity entity)
  {
    ClusterRevealLevel cellRevealLevel = ClusterGrid.Instance.GetCellRevealLevel(entity.Location);
    ClusterRevealLevel isVisibleInFow = entity.IsVisibleInFOW;
    if (cellRevealLevel == ClusterRevealLevel.Visible || isVisibleInFow == ClusterRevealLevel.Visible)
      return ClusterRevealLevel.Visible;
    return cellRevealLevel == ClusterRevealLevel.Peeked && isVisibleInFow == ClusterRevealLevel.Peeked ? ClusterRevealLevel.Peeked : ClusterRevealLevel.Hidden;
  }

  private void UpdateVis(bool onShow = false)
  {
    this.SetupVisGameObjects();
    this.UpdatePaths();
    foreach (KeyValuePair<ClusterGridEntity, ClusterMapVisualizer> gridEntityAnim in this.m_gridEntityAnims)
    {
      ClusterRevealLevel revealLevel = ClusterMapScreen.GetRevealLevel(gridEntityAnim.Key);
      gridEntityAnim.Value.Show(revealLevel);
      bool selected = (UnityEngine.Object) this.m_selectedEntity == (UnityEngine.Object) gridEntityAnim.Key;
      gridEntityAnim.Value.Select(selected);
      if (gridEntityAnim.Key.positionDirty | onShow)
      {
        Vector3 position = ClusterGrid.Instance.GetPosition(gridEntityAnim.Key);
        gridEntityAnim.Value.rectTransform().SetLocalPosition(position);
        gridEntityAnim.Key.positionDirty = false;
      }
    }
    if ((UnityEngine.Object) this.m_selectedEntity != (UnityEngine.Object) null && this.m_gridEntityVis.ContainsKey(this.m_selectedEntity))
    {
      ClusterMapVisualizer gridEntityVi = this.m_gridEntityVis[this.m_selectedEntity];
      this.m_selectMarker.SetTargetTransform(gridEntityVi.transform);
      this.m_selectMarker.gameObject.SetActive(true);
      gridEntityVi.transform.SetAsLastSibling();
    }
    else
      this.m_selectMarker.gameObject.SetActive(false);
    foreach (KeyValuePair<AxialI, ClusterMapVisualizer> keyValuePair in this.m_cellVisByLocation)
    {
      ClusterMapHex component = keyValuePair.Value.GetComponent<ClusterMapHex>();
      AxialI key = keyValuePair.Key;
      int cellRevealLevel = (int) ClusterGrid.Instance.GetCellRevealLevel(key);
      component.SetRevealed((ClusterRevealLevel) cellRevealLevel);
    }
    this.UpdateHexToggleStates();
    this.FloatyAsteroidAnimation();
  }

  private void OnEntityDestroyed(object obj) => this.RemoveDeletedEntities();

  private void UpdateHexToggleStates()
  {
    bool flag = (UnityEngine.Object) this.m_hoveredHex != (UnityEngine.Object) null && (bool) (UnityEngine.Object) ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_hoveredHex.location, EntityLayer.Asteroid);
    foreach (KeyValuePair<AxialI, ClusterMapVisualizer> keyValuePair in this.m_cellVisByLocation)
    {
      ClusterMapHex component = keyValuePair.Value.GetComponent<ClusterMapHex>();
      AxialI key = keyValuePair.Key;
      int state = !((UnityEngine.Object) this.m_selectedHex != (UnityEngine.Object) null) || !(this.m_selectedHex.location == key) ? (!flag || !this.m_hoveredHex.location.IsAdjacent(key) ? 0 : 2) : 1;
      component.UpdateToggleState((ClusterMapHex.ToggleState) state);
    }
  }

  public void SelectEntity(ClusterGridEntity entity, bool frameDelay = false)
  {
    if ((UnityEngine.Object) entity != (UnityEngine.Object) null)
    {
      this.SetSelectedEntity(entity, frameDelay);
      this.m_selectedHex = this.m_cellVisByLocation[entity.Location].GetComponent<ClusterMapHex>();
    }
    this.UpdateVis();
  }

  public void SelectHex(ClusterMapHex newSelectionHex)
  {
    if (this.m_mode == ClusterMapScreen.Mode.Default)
    {
      List<ClusterGridEntity> visibleEntitiesAtCell = ClusterGrid.Instance.GetVisibleEntitiesAtCell(newSelectionHex.location);
      for (int index = visibleEntitiesAtCell.Count - 1; index >= 0; --index)
      {
        KSelectable component = visibleEntitiesAtCell[index].GetComponent<KSelectable>();
        if ((UnityEngine.Object) component == (UnityEngine.Object) null || !component.IsSelectable)
          visibleEntitiesAtCell.RemoveAt(index);
      }
      if (visibleEntitiesAtCell.Count == 0)
      {
        this.SetSelectedEntity((ClusterGridEntity) null);
      }
      else
      {
        int num = visibleEntitiesAtCell.IndexOf(this.m_selectedEntity);
        int index = 0;
        if (num >= 0)
          index = (num + 1) % visibleEntitiesAtCell.Count;
        this.SetSelectedEntity(visibleEntitiesAtCell[index]);
      }
      this.m_selectedHex = newSelectionHex;
    }
    else if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
    {
      Debug.Assert((UnityEngine.Object) this.m_destinationSelector != (UnityEngine.Object) null, (object) "Selected a hex in SelectDestination mode with no ClusterDestinationSelector");
      if (ClusterGrid.Instance.GetPath(this.m_selectedHex.location, newSelectionHex.location, this.m_destinationSelector) != null)
      {
        this.SetMode(ClusterMapScreen.Mode.FinishingSelectDestination);
        this.m_destinationSelector.SetDestination(newSelectionHex.location);
        if (this.m_closeOnSelect)
          ManagementMenu.Instance.CloseAll();
        else
          this.SetMode(ClusterMapScreen.Mode.Default);
      }
    }
    this.UpdateVis();
  }

  public bool HasCurrentHover() => (UnityEngine.Object) this.m_hoveredHex != (UnityEngine.Object) null;

  public AxialI GetCurrentHoverLocation() => this.m_hoveredHex.location;

  public void OnHoverHex(ClusterMapHex newHoverHex)
  {
    this.m_hoveredHex = newHoverHex;
    if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
      this.UpdateVis();
    this.UpdateHexToggleStates();
  }

  public void OnUnhoverHex(ClusterMapHex unhoveredHex)
  {
    if (!((UnityEngine.Object) this.m_hoveredHex == (UnityEngine.Object) unhoveredHex))
      return;
    this.m_hoveredHex = (ClusterMapHex) null;
    this.UpdateHexToggleStates();
  }

  public void SetLocationHighlight(AxialI location, bool highlight)
  {
    this.m_cellVisByLocation[location].GetComponent<ClusterMapHex>().ChangeState(highlight ? 1 : 0);
  }

  public ClusterMapHex GetClusterMapHexAtLocation(AxialI location)
  {
    return this.m_cellVisByLocation[location].GetComponent<ClusterMapHex>();
  }

  private void UpdatePaths()
  {
    ClusterDestinationSelector component1 = (UnityEngine.Object) this.m_selectedEntity != (UnityEngine.Object) null ? this.m_selectedEntity.GetComponent<ClusterDestinationSelector>() : (ClusterDestinationSelector) null;
    if (this.m_mode == ClusterMapScreen.Mode.SelectDestination && (UnityEngine.Object) this.m_hoveredHex != (UnityEngine.Object) null)
    {
      Debug.Assert((UnityEngine.Object) this.m_destinationSelector != (UnityEngine.Object) null, (object) "In SelectDestination mode without a destination selector");
      AxialI myWorldLocation = this.m_destinationSelector.GetMyWorldLocation();
      string fail_reason;
      List<AxialI> path = ClusterGrid.Instance.GetPath(myWorldLocation, this.m_hoveredHex.location, this.m_destinationSelector, out fail_reason);
      if (path != null)
      {
        if ((UnityEngine.Object) this.m_previewMapPath == (UnityEngine.Object) null)
          this.m_previewMapPath = this.pathDrawer.AddPath();
        this.m_previewMapPath.SetPoints(ClusterMapPathDrawer.GetDrawPathList((Vector2) this.m_gridEntityVis[this.GetSelectorGridEntity(this.m_destinationSelector)].transform.localPosition, path));
        this.m_previewMapPath.SetColor(this.rocketPreviewPathColor);
      }
      else if ((UnityEngine.Object) this.m_previewMapPath != (UnityEngine.Object) null)
      {
        Util.KDestroyGameObject((Component) this.m_previewMapPath);
        this.m_previewMapPath = (ClusterMapPath) null;
      }
      // ISSUE: explicit non-virtual call
      int pathLength = path != null ? __nonvirtual (path.Count) : -1;
      if ((UnityEngine.Object) this.m_selectedEntity != (UnityEngine.Object) null)
      {
        IClusterRange component2 = this.m_selectedEntity.GetComponent<IClusterRange>();
        int rangeInTiles = component2.GetRangeInTiles();
        int maxRangeInTiles = component2.GetMaxRangeInTiles();
        if (pathLength > rangeInTiles && string.IsNullOrEmpty(fail_reason))
          fail_reason = GameUtil.SafeStringFormat((string) UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_OUT_OF_RANGE, (object) rangeInTiles, (object) maxRangeInTiles);
        bool repeat = component1.GetComponent<RocketClusterDestinationSelector>().Repeat;
        this.m_hoveredHex.SetDestinationStatus(fail_reason, pathLength, rangeInTiles, repeat);
      }
      else
        this.m_hoveredHex.SetDestinationStatus(fail_reason);
    }
    else
    {
      if (!((UnityEngine.Object) this.m_previewMapPath != (UnityEngine.Object) null))
        return;
      Util.KDestroyGameObject((Component) this.m_previewMapPath);
      this.m_previewMapPath = (ClusterMapPath) null;
    }
  }

  private ClusterGridEntity GetSelectorGridEntity(ClusterDestinationSelector selector)
  {
    ClusterGridEntity component1 = selector.GetComponent<ClusterGridEntity>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
    {
      RocketModuleCluster component2 = selector.GetComponent<RocketModuleCluster>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        component1 = component2.CraftInterface.GetComponent<ClusterGridEntity>();
    }
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && ClusterGrid.Instance.IsVisible(component1))
      return component1;
    ClusterGridEntity entityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(selector.GetMyWorldLocation(), EntityLayer.Asteroid);
    Debug.Assert((UnityEngine.Object) component1 != (UnityEngine.Object) null || (UnityEngine.Object) entityOfLayerAtCell != (UnityEngine.Object) null, (object) $"{selector} has no grid entity and isn't located at a visible asteroid at {selector.GetMyWorldLocation()}");
    return (bool) (UnityEngine.Object) entityOfLayerAtCell ? entityOfLayerAtCell : component1;
  }

  private void UpdateTearStatus()
  {
    ClusterPOIManager clusterPoiManager = (ClusterPOIManager) null;
    if ((UnityEngine.Object) ClusterManager.Instance != (UnityEngine.Object) null)
      clusterPoiManager = ClusterManager.Instance.GetComponent<ClusterPOIManager>();
    if (!((UnityEngine.Object) clusterPoiManager != (UnityEngine.Object) null))
      return;
    TemporalTear temporalTear = clusterPoiManager.GetTemporalTear();
    if (!((UnityEngine.Object) temporalTear != (UnityEngine.Object) null))
      return;
    temporalTear.UpdateStatus();
  }

  public enum Mode
  {
    Default,
    SelectDestination,
    FinishingSelectDestination,
  }
}
