// Decompiled with JetBrains decompiler
// Type: InterfaceTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/InterfaceTool")]
public class InterfaceTool : KMonoBehaviour
{
  private static Dictionary<Action, InterfaceToolConfig> interfaceConfigMap = (Dictionary<Action, InterfaceToolConfig>) null;
  private static List<InterfaceToolConfig> activeConfigs = new List<InterfaceToolConfig>();
  public const float MaxClickDistance = 0.02f;
  public const float DepthBias = -0.15f;
  public GameObject visualizer;
  public Grid.SceneLayer visualizerLayer = Grid.SceneLayer.Move;
  public string placeSound;
  protected bool populateHitsList;
  [NonSerialized]
  public bool hasFocus;
  [SerializeField]
  protected Texture2D cursor;
  public Vector2 cursorOffset = new Vector2(2f, 2f);
  public System.Action OnDeactivate;
  private static Texture2D activeCursor = (Texture2D) null;
  private static HashedString toolActivatedViewMode = OverlayModes.None.ID;
  protected HashedString viewMode = OverlayModes.None.ID;
  private HoverTextConfiguration hoverTextConfiguration;
  private KSelectable hoverOverride;
  public KSelectable hover;
  protected int layerMask;
  protected SelectMarker selectMarker;
  private List<RaycastResult> castResults = new List<RaycastResult>();
  private bool isAppFocused = true;
  private List<KSelectable> hits = new List<KSelectable>();
  protected bool playedSoundThisFrame;
  private List<InterfaceTool.Intersection> intersections = new List<InterfaceTool.Intersection>();

  public static InterfaceToolConfig ActiveConfig
  {
    get
    {
      if (InterfaceTool.interfaceConfigMap == null)
        InterfaceTool.InitializeConfigs(Action.Invalid, (List<InterfaceToolConfig>) null);
      return InterfaceTool.activeConfigs[InterfaceTool.activeConfigs.Count - 1];
    }
  }

  public static void ToggleConfig(Action configKey)
  {
    if (InterfaceTool.interfaceConfigMap == null)
      InterfaceTool.InitializeConfigs(Action.Invalid, (List<InterfaceToolConfig>) null);
    InterfaceToolConfig interfaceToolConfig;
    if (!InterfaceTool.interfaceConfigMap.TryGetValue(configKey, out interfaceToolConfig))
      Debug.LogWarning((object) ($"[InterfaceTool] No config is associated with Key: {configKey}!" + " Are you sure the configs were initialized properly!"));
    else if (InterfaceTool.activeConfigs.BinarySearch(interfaceToolConfig, (IComparer<InterfaceToolConfig>) InterfaceToolConfig.ConfigComparer) <= 0)
    {
      Debug.Log((object) $"[InterfaceTool] Pushing config with key: {configKey}");
      InterfaceTool.activeConfigs.Add(interfaceToolConfig);
      InterfaceTool.activeConfigs.Sort((IComparer<InterfaceToolConfig>) InterfaceToolConfig.ConfigComparer);
    }
    else
    {
      Debug.Log((object) $"[InterfaceTool] Popping config with key: {configKey}");
      InterfaceTool.activeConfigs.Remove(interfaceToolConfig);
    }
  }

  public static void InitializeConfigs(Action defaultKey, List<InterfaceToolConfig> configs)
  {
    string str = configs == null ? "null" : configs.Count.ToString();
    Debug.Log((object) $"[InterfaceTool] Initializing configs with values of DefaultKey: {defaultKey} Configs: {str}");
    if (configs == null || configs.Count == 0)
    {
      InterfaceToolConfig instance = ScriptableObject.CreateInstance<InterfaceToolConfig>();
      InterfaceTool.interfaceConfigMap = new Dictionary<Action, InterfaceToolConfig>();
      InterfaceTool.interfaceConfigMap[instance.InputAction] = instance;
    }
    else
    {
      InterfaceTool.interfaceConfigMap = configs.ToDictionary<InterfaceToolConfig, Action>((Func<InterfaceToolConfig, Action>) (x => x.InputAction));
      InterfaceTool.ToggleConfig(defaultKey);
    }
  }

  public HashedString ViewMode => this.viewMode;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.hoverTextConfiguration = this.GetComponent<HoverTextConfiguration>();
  }

  public void ActivateTool()
  {
    this.OnActivateTool();
    this.OnMouseMove(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
    Game.Instance.Trigger(1174281782, (object) this);
  }

  public virtual bool ShowHoverUI()
  {
    if ((UnityEngine.Object) ManagementMenu.Instance == (UnityEngine.Object) null || ManagementMenu.Instance.IsFullscreenUIActive())
      return false;
    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
    if ((UnityEngine.Object) OverlayScreen.Instance == (UnityEngine.Object) null || !ClusterManager.Instance.IsPositionInActiveWorld(worldPoint) || (double) worldPoint.x < 0.0 || (double) worldPoint.x > (double) Grid.WidthInMeters || (double) worldPoint.y < 0.0 || (double) worldPoint.y > (double) Grid.HeightInMeters)
      return false;
    UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
    return (UnityEngine.Object) current != (UnityEngine.Object) null && !current.IsPointerOverGameObject();
  }

  protected virtual void OnActivateTool()
  {
    if ((UnityEngine.Object) OverlayScreen.Instance != (UnityEngine.Object) null && this.viewMode != OverlayModes.None.ID && OverlayScreen.Instance.mode != this.viewMode)
    {
      OverlayScreen.Instance.ToggleOverlay(this.viewMode);
      InterfaceTool.toolActivatedViewMode = this.viewMode;
    }
    this.SetCursor(this.cursor, this.cursorOffset, CursorMode.Auto);
  }

  public void SetCurrentVirtualInputModuleMousMovementMode(
    bool mouseMovementOnly,
    Action<VirtualInputModule> extraActions = null)
  {
    UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
    if (!((UnityEngine.Object) current != (UnityEngine.Object) null) || !((UnityEngine.Object) current.currentInputModule != (UnityEngine.Object) null))
      return;
    VirtualInputModule currentInputModule = current.currentInputModule as VirtualInputModule;
    if (!((UnityEngine.Object) currentInputModule != (UnityEngine.Object) null))
      return;
    currentInputModule.mouseMovementOnly = mouseMovementOnly;
    if (extraActions == null)
      return;
    extraActions(currentInputModule);
  }

  public void DeactivateTool(InterfaceTool new_tool = null)
  {
    this.OnDeactivateTool(new_tool);
    if (!((UnityEngine.Object) new_tool == (UnityEngine.Object) null) && !((UnityEngine.Object) new_tool == (UnityEngine.Object) SelectTool.Instance) || !(InterfaceTool.toolActivatedViewMode != OverlayModes.None.ID) || !(InterfaceTool.toolActivatedViewMode == SimDebugView.Instance.GetMode()))
      return;
    OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID);
    InterfaceTool.toolActivatedViewMode = OverlayModes.None.ID;
  }

  public virtual void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
  {
    colors = (HashSet<ToolMenu.CellColorData>) null;
  }

  protected virtual void OnDeactivateTool(InterfaceTool new_tool)
  {
  }

  private void OnApplicationFocus(bool focusStatus) => this.isAppFocused = focusStatus;

  public virtual string GetDeactivateSound() => "Tile_Cancel";

  public virtual void OnMouseMove(Vector3 cursor_pos)
  {
    if ((UnityEngine.Object) this.visualizer == (UnityEngine.Object) null || !this.isAppFocused)
      return;
    cursor_pos = Grid.CellToPosCBC(Grid.PosToCell(cursor_pos), this.visualizerLayer);
    cursor_pos.z += -0.15f;
    this.visualizer.transform.SetLocalPosition(cursor_pos);
  }

  public virtual void OnKeyDown(KButtonEvent e)
  {
  }

  public virtual void OnKeyUp(KButtonEvent e)
  {
  }

  public virtual void OnLeftClickDown(Vector3 cursor_pos)
  {
  }

  public virtual void OnLeftClickUp(Vector3 cursor_pos)
  {
  }

  public virtual void OnRightClickDown(Vector3 cursor_pos, KButtonEvent e)
  {
  }

  public virtual void OnRightClickUp(Vector3 cursor_pos)
  {
  }

  public virtual void OnFocus(bool focus)
  {
    if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
      this.visualizer.SetActive(focus);
    this.hasFocus = focus;
  }

  protected Vector2 GetRegularizedPos(Vector2 input, bool minimize)
  {
    Vector3 vector3 = new Vector3(Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, 0.0f);
    return (Vector2) (Grid.CellToPosCCC(Grid.PosToCell(input), Grid.SceneLayer.Background) + (minimize ? -vector3 : vector3));
  }

  protected Vector2 GetWorldRestrictedPosition(Vector2 input)
  {
    input.x = Mathf.Clamp(input.x, ClusterManager.Instance.activeWorld.minimumBounds.x, ClusterManager.Instance.activeWorld.maximumBounds.x);
    input.y = Mathf.Clamp(input.y, ClusterManager.Instance.activeWorld.minimumBounds.y, ClusterManager.Instance.activeWorld.maximumBounds.y);
    return input;
  }

  protected void SetCursor(Texture2D new_cursor, Vector2 offset, CursorMode mode)
  {
    if (!((UnityEngine.Object) new_cursor != (UnityEngine.Object) InterfaceTool.activeCursor) || !((UnityEngine.Object) new_cursor != (UnityEngine.Object) null))
      return;
    InterfaceTool.activeCursor = new_cursor;
    try
    {
      Cursor.SetCursor(new_cursor, offset, mode);
      if (!((UnityEngine.Object) PlayerController.Instance.vim != (UnityEngine.Object) null))
        return;
      PlayerController.Instance.vim.SetCursor(new_cursor);
    }
    catch (Exception ex)
    {
      KCrashReporter.ReportDevNotification("SetCursor Failed", ex.StackTrace, $"SetCursor Failed new_cursor={new_cursor} offset={offset} mode={mode}");
    }
  }

  protected void UpdateHoverElements(List<KSelectable> hits)
  {
    if (!((UnityEngine.Object) this.hoverTextConfiguration != (UnityEngine.Object) null))
      return;
    this.hoverTextConfiguration.UpdateHoverElements(hits);
  }

  public virtual void LateUpdate()
  {
    if (this.populateHitsList)
    {
      if (!this.isAppFocused || !Grid.IsValidCell(Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()))))
        return;
      this.hits.Clear();
      this.GetSelectablesUnderCursor(this.hits);
      KSelectable objectUnderCursor = this.GetObjectUnderCursor<KSelectable>(false, (Func<KSelectable, bool>) (s => s.GetComponent<KSelectable>().IsSelectable));
      this.UpdateHoverElements(this.hits);
      if (!this.hasFocus && (UnityEngine.Object) this.hoverOverride == (UnityEngine.Object) null)
        this.ClearHover();
      else if ((UnityEngine.Object) objectUnderCursor != (UnityEngine.Object) this.hover)
      {
        this.ClearHover();
        this.hover = objectUnderCursor;
        if ((UnityEngine.Object) objectUnderCursor != (UnityEngine.Object) null)
        {
          Game.Instance.Trigger(2095258329, (object) objectUnderCursor.gameObject);
          objectUnderCursor.Hover(!this.playedSoundThisFrame);
          this.playedSoundThisFrame = true;
        }
      }
      this.playedSoundThisFrame = false;
    }
    else
      this.UpdateHoverElements((List<KSelectable>) null);
  }

  public void GetSelectablesUnderCursor(List<KSelectable> hits)
  {
    if ((UnityEngine.Object) this.hoverOverride != (UnityEngine.Object) null)
      hits.Add(this.hoverOverride);
    Camera main = Camera.main;
    Vector3 position = new Vector3(KInputManager.GetMousePos().x, KInputManager.GetMousePos().y, -main.transform.GetPosition().z);
    Vector3 worldPoint = main.ScreenToWorldPoint(position);
    Vector2 pos = new Vector2(worldPoint.x, worldPoint.y);
    int cell = Grid.PosToCell(worldPoint);
    if (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell))
      return;
    Game.Instance.statusItemRenderer.GetIntersections(pos, hits);
    ListPool<ScenePartitionerEntry, SelectTool>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, SelectTool>.Allocate();
    GameScenePartitioner.Instance.GatherEntries((int) pos.x, (int) pos.y, 1, 1, GameScenePartitioner.Instance.collisionLayer, (List<ScenePartitionerEntry>) gathered_entries);
    gathered_entries.Sort((Comparison<ScenePartitionerEntry>) ((x, y) => this.SortHoverCards(x, y)));
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
    {
      KCollider2D kcollider2D = partitionerEntry.obj as KCollider2D;
      if (!((UnityEngine.Object) kcollider2D == (UnityEngine.Object) null) && kcollider2D.enabled && kcollider2D.Intersects(new Vector2(pos.x, pos.y)))
      {
        KSelectable kselectable = kcollider2D.GetComponent<KSelectable>();
        if ((UnityEngine.Object) kselectable == (UnityEngine.Object) null)
          kselectable = kcollider2D.GetComponentInParent<KSelectable>();
        if (!((UnityEngine.Object) kselectable == (UnityEngine.Object) null) && kselectable.isActiveAndEnabled && !hits.Contains(kselectable) && kselectable.IsSelectable)
          hits.Add(kselectable);
      }
    }
    gathered_entries.Recycle();
  }

  public void SetLinkCursor(bool set)
  {
    this.SetCursor(set ? Assets.GetTexture("cursor_hand") : this.cursor, set ? Vector2.zero : this.cursorOffset, CursorMode.Auto);
  }

  protected T GetObjectUnderCursor<T>(
    bool cycleSelection,
    Func<T, bool> condition = null,
    Component previous_selection = null)
    where T : MonoBehaviour
  {
    this.intersections.Clear();
    this.GetObjectUnderCursor2D<T>(this.intersections, condition, this.layerMask);
    this.intersections.RemoveAll(new Predicate<InterfaceTool.Intersection>(InterfaceTool.is_component_null));
    if (this.intersections.Count <= 0)
      return default (T);
    this.intersections.Sort((Comparison<InterfaceTool.Intersection>) ((a, b) => this.SortSelectables(a.component as KMonoBehaviour, b.component as KMonoBehaviour)));
    int index1 = 0;
    if (cycleSelection && (UnityEngine.Object) previous_selection != (UnityEngine.Object) null)
    {
      for (int index2 = 0; index2 < this.intersections.Count; ++index2)
      {
        if ((UnityEngine.Object) this.intersections[index2].component == (UnityEngine.Object) previous_selection)
        {
          index1 = (index2 + 1) % this.intersections.Count;
          break;
        }
      }
    }
    return this.intersections[index1].component as T;
  }

  private void GetObjectUnderCursor2D<T>(
    List<InterfaceTool.Intersection> intersections,
    Func<T, bool> condition,
    int layer_mask)
    where T : MonoBehaviour
  {
    Camera main = Camera.main;
    Vector3 position = new Vector3(KInputManager.GetMousePos().x, KInputManager.GetMousePos().y, -main.transform.GetPosition().z);
    Vector3 worldPoint = main.ScreenToWorldPoint(position);
    Vector2 pos = new Vector2(worldPoint.x, worldPoint.y);
    InterfaceTool.Intersection intersection1;
    if ((UnityEngine.Object) this.hoverOverride != (UnityEngine.Object) null)
    {
      List<InterfaceTool.Intersection> intersectionList = intersections;
      intersection1 = new InterfaceTool.Intersection();
      intersection1.component = (MonoBehaviour) this.hoverOverride;
      intersection1.distance = -100f;
      InterfaceTool.Intersection intersection2 = intersection1;
      intersectionList.Add(intersection2);
    }
    int cell = Grid.PosToCell(worldPoint);
    if (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell))
      return;
    Game.Instance.statusItemRenderer.GetIntersections(pos, intersections);
    ListPool<ScenePartitionerEntry, SelectTool>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, SelectTool>.Allocate();
    int x = 0;
    int y = 0;
    Grid.CellToXY(cell, out x, out y);
    GameScenePartitioner.Instance.GatherEntries(x, y, 1, 1, GameScenePartitioner.Instance.collisionLayer, (List<ScenePartitionerEntry>) gathered_entries);
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
    {
      KCollider2D kcollider2D = partitionerEntry.obj as KCollider2D;
      if (!((UnityEngine.Object) kcollider2D == (UnityEngine.Object) null) && kcollider2D.enabled && kcollider2D.Intersects(new Vector2(worldPoint.x, worldPoint.y)))
      {
        T obj = kcollider2D.GetComponent<T>();
        if ((UnityEngine.Object) obj == (UnityEngine.Object) null)
          obj = kcollider2D.GetComponentInParent<T>();
        if (!((UnityEngine.Object) obj == (UnityEngine.Object) null) && (1 << obj.gameObject.layer & layer_mask) != 0 && !((UnityEngine.Object) obj == (UnityEngine.Object) null) && (condition == null || condition(obj)))
        {
          float b = obj.transform.GetPosition().z - worldPoint.z;
          bool flag = false;
          for (int index = 0; index < intersections.Count; ++index)
          {
            InterfaceTool.Intersection intersection3 = intersections[index];
            if ((UnityEngine.Object) intersection3.component.gameObject == (UnityEngine.Object) obj.gameObject)
            {
              intersection3.distance = Mathf.Min(intersection3.distance, b);
              intersections[index] = intersection3;
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            List<InterfaceTool.Intersection> intersectionList = intersections;
            intersection1 = new InterfaceTool.Intersection();
            intersection1.component = (MonoBehaviour) obj;
            intersection1.distance = b;
            InterfaceTool.Intersection intersection4 = intersection1;
            intersectionList.Add(intersection4);
          }
        }
      }
    }
    gathered_entries.Recycle();
  }

  private int SortSelectables(KMonoBehaviour x, KMonoBehaviour y)
  {
    if ((UnityEngine.Object) x == (UnityEngine.Object) null && (UnityEngine.Object) y == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) x == (UnityEngine.Object) null)
      return -1;
    if ((UnityEngine.Object) y == (UnityEngine.Object) null)
      return 1;
    int num = x.transform.GetPosition().z.CompareTo(y.transform.GetPosition().z);
    return num != 0 ? num : x.GetInstanceID().CompareTo(y.GetInstanceID());
  }

  public void SetHoverOverride(KSelectable hover_override) => this.hoverOverride = hover_override;

  private int SortHoverCards(ScenePartitionerEntry x, ScenePartitionerEntry y)
  {
    return this.SortSelectables(x.obj as KMonoBehaviour, y.obj as KMonoBehaviour);
  }

  private static bool is_component_null(InterfaceTool.Intersection intersection)
  {
    return !(bool) (UnityEngine.Object) intersection.component;
  }

  protected void ClearHover()
  {
    if (!((UnityEngine.Object) this.hover != (UnityEngine.Object) null))
      return;
    KSelectable hover = this.hover;
    this.hover = (KSelectable) null;
    hover.Unhover();
    Game.Instance.Trigger(-1201923725, (object) null);
  }

  public struct Intersection
  {
    public MonoBehaviour component;
    public float distance;
  }
}
