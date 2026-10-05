// Decompiled with JetBrains decompiler
// Type: ResearchScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class ResearchScreen : KModalScreen
{
  private const float SCROLL_BUFFER = 250f;
  [SerializeField]
  private Image BG;
  public ResearchEntry entryPrefab;
  public ResearchTreeTitle researchTreeTitlePrefab;
  public GameObject foreground;
  public GameObject scrollContent;
  public GameObject treeTitles;
  public GameObject pointDisplayCountPrefab;
  public GameObject pointDisplayContainer;
  private Dictionary<string, LocText> pointDisplayMap;
  private Dictionary<Tech, ResearchEntry> entryMap;
  [SerializeField]
  private KButton zoomOutButton;
  [SerializeField]
  private KButton zoomInButton;
  [SerializeField]
  private ResearchScreenSideBar sideBar;
  private Tech currentResearch;
  public KButton CloseButton;
  private GraphicRaycaster m_Raycaster;
  private GraphicRaycaster scrollContentRaycaster;
  private PointerEventData m_PointerEventData;
  private Vector3 currentScrollPosition;
  private bool panUp;
  private bool panDown;
  private bool panLeft;
  private bool panRight;
  [SerializeField]
  private KChildFitter scrollContentChildFitter;
  private CanvasGroup canvasGroup;
  private bool isDragging;
  private Vector3 dragStartPosition;
  private Vector3 dragLastPosition;
  private Vector2 dragInteria;
  private Vector2 forceTargetPosition;
  private bool zoomingToTarget;
  private bool draggingJustEnded;
  private float targetZoom = 0.6f;
  private float currentZoom = 1f;
  private bool zoomCenterLock;
  private Vector2 keyPanDelta = (Vector2) Vector3.zero;
  [SerializeField]
  private float effectiveZoomSpeed = 5f;
  [SerializeField]
  private float zoomAmountPerScroll = 0.05f;
  [SerializeField]
  private float zoomAmountPerButton = 0.5f;
  [SerializeField]
  private float minZoom = 0.15f;
  [SerializeField]
  private float maxZoom = 1f;
  [SerializeField]
  private float keyboardScrollSpeed = 200f;
  [SerializeField]
  private float keyPanEasing = 1f;
  [SerializeField]
  private float edgeClampFactor = 0.5f;
  private int deactivateResearchScreenHandle = -1;

  public bool HasActiveResearch => this.currentResearch != null;

  public bool IsBeingResearched(Tech tech) => Research.Instance.IsBeingResearched(tech);

  public override float GetSortKey() => this.isEditing ? 50f : 20f;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.ConsumeMouseScroll = true;
    this.canvasGroup = this.GetComponent<CanvasGroup>();
    this.m_Raycaster = this.GetComponent<GraphicRaycaster>();
    this.scrollContentRaycaster = this.scrollContent.GetComponent<GraphicRaycaster>();
  }

  private void ZoomOut()
  {
    this.targetZoom = Mathf.Clamp(this.targetZoom - this.zoomAmountPerButton, this.minZoom, this.maxZoom);
    this.zoomCenterLock = true;
  }

  private void ZoomIn()
  {
    this.targetZoom = Mathf.Clamp(this.targetZoom + this.zoomAmountPerButton, this.minZoom, this.maxZoom);
    this.zoomCenterLock = true;
  }

  public void ZoomToTech(string techID, bool highlight = false)
  {
    Vector2 localPosition = (Vector2) this.entryMap[Db.Get().Techs.Get(techID)].rectTransform().GetLocalPosition();
    UnityEngine.Rect rect = this.foreground.rectTransform().rect;
    double x = -(double) rect.size.x / 2.0;
    rect = this.foreground.rectTransform().rect;
    double y = (double) rect.size.y / 2.0;
    Vector2 vector2 = new Vector2((float) x, (float) y);
    this.forceTargetPosition = -(localPosition + vector2);
    this.zoomingToTarget = true;
    this.targetZoom = this.maxZoom;
    if (!highlight)
      return;
    this.sideBar.SetSearch(Db.Get().Techs.Get(techID).Name);
  }

  private void Update()
  {
    if (!this.IsScreenActive())
      return;
    RectTransform component = this.scrollContent.GetComponent<RectTransform>();
    if (this.isDragging && !KInputManager.isFocused)
      this.AbortDragging();
    Vector2 anchoredPosition = component.anchoredPosition;
    this.currentZoom = Mathf.Lerp(this.currentZoom, this.targetZoom, Mathf.Min(this.effectiveZoomSpeed * Time.unscaledDeltaTime, 0.9f));
    Vector2 zero1 = Vector2.zero;
    Vector2 mousePos = (Vector2) KInputManager.GetMousePos();
    Vector2 vector2_1 = (Vector2) (this.zoomCenterLock ? component.InverseTransformPoint((Vector3) new Vector2((float) (Screen.width / 2), (float) (Screen.height / 2))) * this.currentZoom : component.InverseTransformPoint((Vector3) mousePos) * this.currentZoom);
    component.localScale = new Vector3(this.currentZoom, this.currentZoom, 1f);
    Vector2 vector2_2 = (Vector2) (this.zoomCenterLock ? component.InverseTransformPoint((Vector3) new Vector2((float) (Screen.width / 2), (float) (Screen.height / 2))) * this.currentZoom : component.InverseTransformPoint((Vector3) mousePos) * this.currentZoom) - vector2_1;
    float keyboardScrollSpeed = this.keyboardScrollSpeed;
    if (this.panUp)
      this.keyPanDelta -= Vector2.up * Time.unscaledDeltaTime * keyboardScrollSpeed;
    else if (this.panDown)
      this.keyPanDelta += Vector2.up * Time.unscaledDeltaTime * keyboardScrollSpeed;
    if (this.panLeft)
      this.keyPanDelta += Vector2.right * Time.unscaledDeltaTime * keyboardScrollSpeed;
    else if (this.panRight)
      this.keyPanDelta -= Vector2.right * Time.unscaledDeltaTime * keyboardScrollSpeed;
    if (KInputManager.currentControllerIsGamepad)
      this.keyPanDelta = KInputManager.steamInputInterpreter.GetSteamCameraMovement() * -1f * Time.unscaledDeltaTime * keyboardScrollSpeed * 2f;
    this.keyPanDelta -= new Vector2(Mathf.Lerp(0.0f, this.keyPanDelta.x, Time.unscaledDeltaTime * this.keyPanEasing), Mathf.Lerp(0.0f, this.keyPanDelta.y, Time.unscaledDeltaTime * this.keyPanEasing));
    Vector2 zero2 = Vector2.zero;
    if (this.isDragging)
    {
      Vector2 vector2_3 = (Vector2) (KInputManager.GetMousePos() - this.dragLastPosition);
      zero2 += vector2_3;
      this.dragLastPosition = KInputManager.GetMousePos();
      this.dragInteria = Vector2.ClampMagnitude(this.dragInteria + vector2_3, 400f);
    }
    this.dragInteria *= Mathf.Max(0.0f, (float) (1.0 - (double) Time.unscaledDeltaTime * 4.0));
    Vector2 vector2_4 = vector2_2;
    Vector2 a = anchoredPosition + vector2_4 + this.keyPanDelta + zero2;
    if (!this.isDragging)
    {
      Vector2 size = this.GetComponent<RectTransform>().rect.size;
      Vector2 vector2_5 = new Vector2((float) (-(double) component.rect.size.x / 2.0 - 250.0) * this.currentZoom, -250f * this.currentZoom);
      Vector2 vector2_6 = new Vector2(250f * this.currentZoom, (component.rect.size.y + 250f) * this.currentZoom - size.y);
      Vector2 vector2_7 = new Vector2(Mathf.Clamp(a.x, vector2_5.x, vector2_6.x), Mathf.Clamp(a.y, vector2_5.y, vector2_6.y));
      this.forceTargetPosition = new Vector2(Mathf.Clamp(this.forceTargetPosition.x, vector2_5.x, vector2_6.x), Mathf.Clamp(this.forceTargetPosition.y, vector2_5.y, vector2_6.y));
      Vector2 dragInteria = this.dragInteria;
      Vector2 vector2_8 = vector2_7 + dragInteria - a;
      if (!this.panLeft && !this.panRight && !this.panUp && !this.panDown)
      {
        a += vector2_8 * this.edgeClampFactor * Time.unscaledDeltaTime;
      }
      else
      {
        a += vector2_8;
        if ((double) vector2_8.x < 0.0)
          this.keyPanDelta.x = Mathf.Min(0.0f, this.keyPanDelta.x);
        if ((double) vector2_8.x > 0.0)
          this.keyPanDelta.x = Mathf.Max(0.0f, this.keyPanDelta.x);
        if ((double) vector2_8.y < 0.0)
          this.keyPanDelta.y = Mathf.Min(0.0f, this.keyPanDelta.y);
        if ((double) vector2_8.y > 0.0)
          this.keyPanDelta.y = Mathf.Max(0.0f, this.keyPanDelta.y);
      }
    }
    if (this.zoomingToTarget)
    {
      a = Vector2.Lerp(a, this.forceTargetPosition, Time.unscaledDeltaTime * 4f);
      if ((double) Vector3.Distance((Vector3) a, (Vector3) this.forceTargetPosition) < 1.0 || this.isDragging || this.panLeft || this.panRight || this.panUp || this.panDown)
        this.zoomingToTarget = false;
    }
    component.anchoredPosition = a;
  }

  protected override void OnSpawn()
  {
    this.Subscribe(Research.Instance.gameObject, -1914338957, new Action<object>(this.OnActiveResearchChanged));
    this.Subscribe(Game.Instance.gameObject, -107300940, new Action<object>(this.OnResearchComplete));
    this.deactivateResearchScreenHandle = this.Subscribe(Game.Instance.gameObject, -1974454597, (Action<object>) (o => this.Show(false)));
    this.pointDisplayMap = new Dictionary<string, LocText>();
    foreach (ResearchType type in Research.Instance.researchTypes.Types)
    {
      this.pointDisplayMap[type.id] = Util.KInstantiateUI(this.pointDisplayCountPrefab, this.pointDisplayContainer, true).GetComponentInChildren<LocText>();
      this.pointDisplayMap[type.id].text = Research.Instance.globalPointInventory.PointsByTypeID[type.id].ToString();
      this.pointDisplayMap[type.id].transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(type.description);
      this.pointDisplayMap[type.id].transform.parent.GetComponentInChildren<Image>().sprite = type.sprite;
    }
    this.pointDisplayContainer.transform.parent.gameObject.SetActive(Research.Instance.UseGlobalPointInventory);
    this.entryMap = new Dictionary<Tech, ResearchEntry>();
    List<Tech> resources1 = Db.Get().Techs.resources;
    resources1.Sort((Comparison<Tech>) ((x, y) => y.center.y.CompareTo(x.center.y)));
    List<TechTreeTitle> resources2 = Db.Get().TechTreeTitles.resources;
    resources2.Sort((Comparison<TechTreeTitle>) ((x, y) => y.center.y.CompareTo(x.center.y)));
    Vector2 vector2_1 = new Vector2(0.0f, 125f);
    for (int index = 0; index < resources2.Count; ++index)
    {
      ResearchTreeTitle researchTreeTitle = Util.KInstantiateUI<ResearchTreeTitle>(this.researchTreeTitlePrefab.gameObject, this.treeTitles);
      TechTreeTitle techTreeTitle1 = resources2[index];
      researchTreeTitle.name = techTreeTitle1.Name + " Title";
      Vector3 vector3_1 = (Vector3) (techTreeTitle1.center + vector2_1);
      researchTreeTitle.transform.rectTransform().anchoredPosition = (Vector2) vector3_1;
      float height = techTreeTitle1.height;
      float y;
      if (index + 1 < resources2.Count)
      {
        TechTreeTitle techTreeTitle2 = resources2[index + 1];
        Vector3 vector3_2 = (Vector3) (techTreeTitle2.center + vector2_1);
        y = height + (vector3_1.y - (vector3_2.y + techTreeTitle2.height));
      }
      else
        y = height + 600f;
      researchTreeTitle.transform.rectTransform().sizeDelta = new Vector2(techTreeTitle1.width, y);
      researchTreeTitle.SetLabel(techTreeTitle1.Name);
      researchTreeTitle.SetColor(index);
    }
    List<Vector2> vector2List = new List<Vector2>();
    Vector2 vector2_2 = new Vector2(0.0f, 0.0f);
    for (int index1 = 0; index1 < resources1.Count; ++index1)
    {
      ResearchEntry researchEntry = Util.KInstantiateUI<ResearchEntry>(this.entryPrefab.gameObject, this.scrollContent);
      Tech key = resources1[index1];
      researchEntry.name = key.Name + " Panel";
      Vector3 vector3 = (Vector3) (key.center + vector2_2);
      researchEntry.researchScreenReference = this;
      researchEntry.transform.rectTransform().anchoredPosition = (Vector2) vector3;
      researchEntry.transform.rectTransform().sizeDelta = new Vector2(key.width, key.height);
      this.entryMap.Add(key, researchEntry);
      if (key.edges.Count > 0)
      {
        for (int index2 = 0; index2 < key.edges.Count; ++index2)
        {
          ResourceTreeNode.Edge edge = key.edges[index2];
          if (edge.path == null)
          {
            vector2List.AddRange((IEnumerable<Vector2>) edge.SrcTarget);
          }
          else
          {
            switch (edge.edgeType)
            {
              case ResourceTreeNode.Edge.EdgeType.PolyLineEdge:
              case ResourceTreeNode.Edge.EdgeType.QuadCurveEdge:
              case ResourceTreeNode.Edge.EdgeType.BezierEdge:
              case ResourceTreeNode.Edge.EdgeType.GenericEdge:
                vector2List.Add(edge.SrcTarget[0]);
                vector2List.Add(edge.path[0]);
                for (int index3 = 1; index3 < edge.path.Count; ++index3)
                {
                  vector2List.Add(edge.path[index3 - 1]);
                  vector2List.Add(edge.path[index3]);
                }
                vector2List.Add(edge.path[edge.path.Count - 1]);
                vector2List.Add(edge.SrcTarget[1]);
                continue;
              default:
                vector2List.AddRange((IEnumerable<Vector2>) edge.path);
                continue;
            }
          }
        }
      }
    }
    for (int index = 0; index < vector2List.Count; ++index)
      vector2List[index] = new Vector2(vector2List[index].x, vector2List[index].y + this.foreground.transform.rectTransform().rect.height);
    foreach (KeyValuePair<Tech, ResearchEntry> entry in this.entryMap)
      entry.Value.SetTech(entry.Key);
    this.CloseButton.soundPlayer.Enabled = false;
    this.CloseButton.onClick += (System.Action) (() => ManagementMenu.Instance.CloseAll());
    this.StartCoroutine(this.WaitAndSetActiveResearch());
    base.OnSpawn();
    this.scrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(250f, -250f);
    this.zoomOutButton.onClick += (System.Action) (() => this.ZoomOut());
    this.zoomInButton.onClick += (System.Action) (() => this.ZoomIn());
    this.gameObject.SetActive(true);
    this.Show(false);
  }

  public override void OnBeginDrag(PointerEventData eventData)
  {
    base.OnBeginDrag(eventData);
    this.isDragging = true;
  }

  public override void OnEndDrag(PointerEventData eventData)
  {
    base.OnEndDrag(eventData);
    this.AbortDragging();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.Unsubscribe(Game.Instance.gameObject, ref this.deactivateResearchScreenHandle);
  }

  private IEnumerator WaitAndSetActiveResearch()
  {
    yield return (object) SequenceUtil.WaitForEndOfFrame;
    TechInstance targetResearch = Research.Instance.GetTargetResearch();
    if (targetResearch != null)
      this.SetActiveResearch(targetResearch.tech);
  }

  public Vector3 GetEntryPosition(Tech tech)
  {
    if (this.entryMap.ContainsKey(tech))
      return this.entryMap[tech].transform.GetPosition();
    Debug.LogError((object) "The Tech provided was not present in the dictionary");
    return Vector3.zero;
  }

  public ResearchEntry GetEntry(Tech tech)
  {
    if (this.entryMap == null)
      return (ResearchEntry) null;
    if (this.entryMap.ContainsKey(tech))
      return this.entryMap[tech];
    Debug.LogError((object) "The Tech provided was not present in the dictionary");
    return (ResearchEntry) null;
  }

  public void SetEntryPercentage(Tech tech, float percent)
  {
    ResearchEntry entry = this.GetEntry(tech);
    if (!((UnityEngine.Object) entry != (UnityEngine.Object) null))
      return;
    entry.SetPercentage(percent);
  }

  public void TurnEverythingOff()
  {
    foreach (KeyValuePair<Tech, ResearchEntry> entry in this.entryMap)
      entry.Value.SetEverythingOff();
  }

  public void TurnEverythingOn()
  {
    foreach (KeyValuePair<Tech, ResearchEntry> entry in this.entryMap)
      entry.Value.SetEverythingOn();
  }

  private void SelectAllEntries(Tech tech, bool isSelected)
  {
    ResearchEntry entry = this.GetEntry(tech);
    if ((UnityEngine.Object) entry != (UnityEngine.Object) null)
      entry.QueueStateChanged(isSelected);
    foreach (Tech tech1 in tech.requiredTech)
      this.SelectAllEntries(tech1, isSelected);
  }

  private void OnResearchComplete(object data)
  {
    if (!(data is Tech))
      return;
    ResearchEntry entry = this.GetEntry((Tech) data);
    if ((UnityEngine.Object) entry != (UnityEngine.Object) null)
      entry.ResearchCompleted();
    this.UpdateProgressBars();
    this.UpdatePointDisplay();
  }

  private void UpdatePointDisplay()
  {
    foreach (ResearchType type in Research.Instance.researchTypes.Types)
      this.pointDisplayMap[type.id].text = $"{Research.Instance.researchTypes.GetResearchType(type.id).name}: {Research.Instance.globalPointInventory.PointsByTypeID[type.id].ToString()}";
  }

  private void OnActiveResearchChanged(object data)
  {
    List<TechInstance> techInstanceList = (List<TechInstance>) data;
    foreach (TechInstance techInstance in techInstanceList)
    {
      ResearchEntry entry = this.GetEntry(techInstance.tech);
      if ((UnityEngine.Object) entry != (UnityEngine.Object) null)
        entry.QueueStateChanged(true);
    }
    this.UpdateProgressBars();
    this.UpdatePointDisplay();
    if (techInstanceList.Count <= 0)
      return;
    this.currentResearch = techInstanceList[techInstanceList.Count - 1].tech;
  }

  private void UpdateProgressBars()
  {
    foreach (KeyValuePair<Tech, ResearchEntry> entry in this.entryMap)
      entry.Value.UpdateProgressBars();
  }

  public void CancelResearch()
  {
    List<TechInstance> researchQueue = Research.Instance.GetResearchQueue();
    foreach (TechInstance techInstance in researchQueue)
    {
      ResearchEntry entry = this.GetEntry(techInstance.tech);
      if ((UnityEngine.Object) entry != (UnityEngine.Object) null)
        entry.QueueStateChanged(false);
    }
    researchQueue.Clear();
  }

  private void SetActiveResearch(Tech newResearch)
  {
    if (newResearch != this.currentResearch && this.currentResearch != null)
      this.SelectAllEntries(this.currentResearch, false);
    this.currentResearch = newResearch;
    if (this.currentResearch == null)
      return;
    this.SelectAllEntries(this.currentResearch, true);
  }

  public override bool IsScreenActive() => (double) this.canvasGroup.alpha > 0.0;

  public override void Show(bool show = true)
  {
    this.mouseOver = false;
    this.scrollContentChildFitter.enabled = show;
    this.canvasGroup.alpha = show ? 1f : 0.0f;
    this.m_Raycaster.enabled = show;
    this.scrollContentRaycaster.enabled = show;
    this.sideBar.Show(show);
    this.OnShow(show);
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (show)
      this.sideBar.ResetFilter();
    if (show)
    {
      CameraController.Instance.DisableUserCameraControl = true;
      if ((UnityEngine.Object) DetailsScreen.Instance != (UnityEngine.Object) null)
        DetailsScreen.Instance.gameObject.SetActive(false);
    }
    else
    {
      CameraController.Instance.DisableUserCameraControl = false;
      if ((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null && !DetailsScreen.Instance.gameObject.activeSelf)
      {
        DetailsScreen.Instance.gameObject.SetActive(true);
        DetailsScreen.Instance.Refresh(SelectTool.Instance.selected.gameObject);
      }
    }
    this.UpdateProgressBars();
    this.UpdatePointDisplay();
  }

  private void AbortDragging()
  {
    this.isDragging = false;
    this.draggingJustEnded = true;
  }

  private void LateUpdate() => this.draggingJustEnded = false;

  public override void OnKeyUp(KButtonEvent e)
  {
    if (!e.Consumed)
    {
      if (e.IsAction(Action.MouseRight) && !this.isDragging && !this.draggingJustEnded)
        ManagementMenu.Instance.CloseAll();
      if (e.IsAction(Action.MouseRight) || e.IsAction(Action.MouseLeft) || e.IsAction(Action.MouseMiddle))
        this.AbortDragging();
      if (this.panUp && e.TryConsume(Action.PanUp))
      {
        this.panUp = false;
        return;
      }
      if (this.panDown && e.TryConsume(Action.PanDown))
      {
        this.panDown = false;
        return;
      }
      if (this.panRight && e.TryConsume(Action.PanRight))
      {
        this.panRight = false;
        return;
      }
      if (this.panLeft && e.TryConsume(Action.PanLeft))
      {
        this.panLeft = false;
        return;
      }
    }
    base.OnKeyUp(e);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (!e.Consumed)
    {
      if (e.TryConsume(Action.MouseRight))
      {
        this.dragStartPosition = KInputManager.GetMousePos();
        this.dragLastPosition = KInputManager.GetMousePos();
        return;
      }
      if (e.TryConsume(Action.MouseLeft))
      {
        this.dragStartPosition = KInputManager.GetMousePos();
        this.dragLastPosition = KInputManager.GetMousePos();
        return;
      }
      if ((double) KInputManager.GetMousePos().x > (double) this.sideBar.rectTransform().sizeDelta.x && CameraController.IsMouseOverGameWindow)
      {
        if (e.TryConsume(Action.ZoomIn))
        {
          this.targetZoom = Mathf.Clamp(this.targetZoom + this.zoomAmountPerScroll, this.minZoom, this.maxZoom);
          this.zoomCenterLock = false;
          return;
        }
        if (e.TryConsume(Action.ZoomOut))
        {
          this.targetZoom = Mathf.Clamp(this.targetZoom - this.zoomAmountPerScroll, this.minZoom, this.maxZoom);
          this.zoomCenterLock = false;
          return;
        }
      }
      if (e.TryConsume(Action.Find))
      {
        this.sideBar.FocusSearchBox();
        return;
      }
      if (e.TryConsume(Action.Escape))
      {
        ManagementMenu.Instance.CloseAll();
        return;
      }
      if (e.TryConsume(Action.PanLeft))
      {
        this.panLeft = true;
        return;
      }
      if (e.TryConsume(Action.PanRight))
      {
        this.panRight = true;
        return;
      }
      if (e.TryConsume(Action.PanUp))
      {
        this.panUp = true;
        return;
      }
      if (e.TryConsume(Action.PanDown))
      {
        this.panDown = true;
        return;
      }
      if (DebugHandler.enabled && e.TryConsume(Action.DebugInstantBuildMode))
      {
        DebugHandler.ToggleInstantBuildMode();
        return;
      }
    }
    base.OnKeyDown(e);
  }

  public enum ResearchState
  {
    Available,
    ActiveResearch,
    ResearchComplete,
    MissingPrerequisites,
    StateCount,
  }
}
