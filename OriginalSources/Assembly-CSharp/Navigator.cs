// Decompiled with JetBrains decompiler
// Type: Navigator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

#nullable disable
public class Navigator : StateMachineComponent<Navigator.StatesInstance>, ISaveLoadableDetails
{
  public bool DebugDrawPath;
  [MyCmpAdd]
  public Facing facing;
  public float defaultSpeed = 1f;
  public TransitionDriver transitionDriver;
  public string NavGridName;
  public bool updateProber;
  public int maxProbeRadiusX;
  public int maxProbeRadiusY;
  public PathFinder.PotentialPath.Flags flags;
  private LoggerFSS log;
  public Dictionary<NavType, int> distanceTravelledByNavType;
  public Grid.SceneLayer sceneLayer = Grid.SceneLayer.Move;
  private PathFinderAbilities abilities;
  [MyCmpReq]
  public KBatchedAnimController animController;
  [NonSerialized]
  public PathFinder.Path path;
  public NavType CurrentNavType;
  private int AnchorCell;
  private KPrefabID targetLocator;
  public int cachedCell;
  private int reservedCell = NavigationReservations.InvalidReservation;
  private NavTactic tactic;
  public bool reportOccupation;
  public List<int> occupiedCells = new List<int>();
  private Action<int, object> OnBuildingTileChangedAction;
  private static readonly EventSystem.IntraObjectHandler<Navigator> OnDefeatedDelegate = new EventSystem.IntraObjectHandler<Navigator>((Action<Navigator, object>) ((component, data) => component.OnDefeated(data)));
  private static readonly EventSystem.IntraObjectHandler<Navigator> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Navigator>((Action<Navigator, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<Navigator> OnSelectObjectDelegate = new EventSystem.IntraObjectHandler<Navigator>((Action<Navigator, object>) ((component, data) => component.OnSelectObject(data)));
  private static readonly EventSystem.IntraObjectHandler<Navigator> OnStoreDelegate = new EventSystem.IntraObjectHandler<Navigator>((Action<Navigator, object>) ((component, data) => component.OnStore(data)));
  private static readonly EventSystem.IntraObjectHandler<Navigator> OnQueueDestroyDelegate = new EventSystem.IntraObjectHandler<Navigator>((Action<Navigator, object>) ((component, data) => component.OnQueueDestroy()));
  public bool executePathProbeTaskAsync;

  public KMonoBehaviour target { get; set; }

  public CellOffset[] targetOffsets { get; private set; }

  public NavGrid NavGrid { get; private set; }

  public PathGrid PathGrid { get; set; }

  public void Serialize(BinaryWriter writer)
  {
    byte currentNavType = (byte) this.CurrentNavType;
    writer.Write(currentNavType);
    writer.Write(this.distanceTravelledByNavType.Count);
    foreach (KeyValuePair<NavType, int> keyValuePair in this.distanceTravelledByNavType)
    {
      byte key = (byte) keyValuePair.Key;
      writer.Write(key);
      writer.Write(keyValuePair.Value);
    }
  }

  public void Deserialize(IReader reader)
  {
    NavType navType = (NavType) reader.ReadByte();
    if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 11))
    {
      int num1 = reader.ReadInt32();
      for (int index = 0; index < num1; ++index)
      {
        NavType key = (NavType) reader.ReadByte();
        int num2 = reader.ReadInt32();
        if (this.distanceTravelledByNavType.ContainsKey(key))
          this.distanceTravelledByNavType[key] = num2;
      }
    }
    bool flag = false;
    foreach (NavType validNavType in this.NavGrid.ValidNavTypes)
    {
      if (validNavType == navType)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return;
    this.CurrentNavType = navType;
  }

  protected override void OnPrefabInit()
  {
    this.transitionDriver = new TransitionDriver(this);
    this.targetLocator = Util.KInstantiate(Assets.GetPrefab((Tag) TargetLocator.ID)).GetComponent<KPrefabID>();
    this.targetLocator.gameObject.SetActive(true);
    this.log = new LoggerFSS(nameof (Navigator));
    this.simRenderLoadBalance = true;
    this.autoRegisterSimRender = false;
    this.NavGrid = Pathfinding.Instance.GetNavGrid(this.NavGridName);
    this.PathGrid = this.maxProbeRadiusX != 0 || this.maxProbeRadiusY != 0 ? new PathGrid(this.maxProbeRadiusX * 2 + 1, this.maxProbeRadiusY * 2 + 1, true, this.NavGrid.ValidNavTypes) : new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, this.NavGrid.ValidNavTypes);
    this.distanceTravelledByNavType = new Dictionary<NavType, int>();
    for (int key = 0; key < 11; ++key)
      this.distanceTravelledByNavType.Add((NavType) key, 0);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Navigator>(1623392196, Navigator.OnDefeatedDelegate);
    this.Subscribe<Navigator>(-1506500077, Navigator.OnDefeatedDelegate);
    this.Subscribe<Navigator>(493375141, Navigator.OnRefreshUserMenuDelegate);
    this.Subscribe<Navigator>(-1503271301, Navigator.OnSelectObjectDelegate);
    this.Subscribe<Navigator>(856640610, Navigator.OnStoreDelegate);
    this.Subscribe<Navigator>(1502190696, Navigator.OnQueueDestroyDelegate);
    if (this.updateProber)
      SimAndRenderScheduler.instance.Add((object) this);
    if (this.executePathProbeTaskAsync)
      AsyncPathProber.Instance.Register(this);
    this.cachedCell = Grid.PosToCell((KMonoBehaviour) this);
    this.SetCurrentNavType(this.CurrentNavType);
    this.OnBuildingTileChangedAction = new Action<int, object>(this.OnBuildingTileChanged);
    this.SubscribeUnstuckFunctions();
  }

  private void SubscribeUnstuckFunctions()
  {
    if (this.CurrentNavType != NavType.Tube)
      return;
    GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], this.OnBuildingTileChangedAction);
  }

  private void UnsubscribeUnstuckFunctions()
  {
    GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], this.OnBuildingTileChangedAction);
  }

  private void OnBuildingTileChanged(int cell, object building)
  {
    if (this.CurrentNavType != NavType.Tube || building != null || !(this.smi != null & cell == Grid.PosToCell((KMonoBehaviour) this)))
      return;
    this.SetCurrentNavType(NavType.Floor);
    this.UnsubscribeUnstuckFunctions();
  }

  protected override void OnCleanUp()
  {
    this.UnsubscribeUnstuckFunctions();
    base.OnCleanUp();
  }

  protected void OnQueueDestroy()
  {
    if (this.executePathProbeTaskAsync)
      AsyncPathProber.Instance.Unregister(this);
    if (!this.reportOccupation)
      return;
    MinionGroupProber.Get().Vacate(this.occupiedCells);
  }

  public PathGrid TakeResult(ref AsyncPathProber.WorkResult result)
  {
    PathGrid pathGrid = this.PathGrid;
    this.PathGrid = result.pathGrid;
    if (!this.reportOccupation)
      return pathGrid;
    List<int> occupiedCells = this.occupiedCells;
    MinionGroupProber.Get().OccupyST(result.newlyReachableCells);
    MinionGroupProber.Get().VacateST(result.noLongerReachableCells);
    this.occupiedCells = result.reachableCells;
    result.reachableCells = occupiedCells;
    return pathGrid;
  }

  public bool IsMoving()
  {
    return this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.normal.moving);
  }

  public bool IsSwimming() => this.CurrentNavType == NavType.Swim;

  public bool GoTo(int cell, CellOffset[] offsets = null)
  {
    if (offsets == null)
      offsets = new CellOffset[1];
    this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
    return this.GoTo((KMonoBehaviour) this.targetLocator, offsets, NavigationTactics.ReduceTravelDistance);
  }

  public bool GoTo(int cell, CellOffset[] offsets, NavTactic tactic)
  {
    if (offsets == null)
      offsets = new CellOffset[1];
    this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
    return this.GoTo((KMonoBehaviour) this.targetLocator, offsets, tactic);
  }

  public void UpdateTarget(int cell)
  {
    this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
  }

  public bool GoTo(KMonoBehaviour target, CellOffset[] offsets, NavTactic tactic)
  {
    if (tactic == null)
      tactic = NavigationTactics.ReduceTravelDistance;
    this.smi.GoTo((StateMachine.BaseState) this.smi.sm.normal.moving);
    this.smi.sm.moveTarget.Set(target.gameObject, this.smi, false);
    this.tactic = tactic;
    this.target = target;
    this.targetOffsets = offsets;
    this.ClearReservedCell();
    this.AdvancePath();
    return this.IsMoving();
  }

  public void BeginTransition(NavGrid.Transition transition)
  {
    this.transitionDriver.EndTransition();
    this.smi.GoTo((StateMachine.BaseState) this.smi.sm.normal.moving);
    this.transitionDriver.BeginTransition(this, transition, this.defaultSpeed);
  }

  private bool ValidatePath(ref PathFinder.Path path, out bool atNextNode)
  {
    atNextNode = false;
    bool flag = false;
    if (path.IsValid())
    {
      int cell = Grid.PosToCell(this.target);
      flag = (this.reservedCell != NavigationReservations.InvalidReservation && this.CanReach(this.reservedCell)) & Grid.IsCellOffsetOf(this.reservedCell, cell, this.targetOffsets);
    }
    if (flag)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) this);
      flag = ((cell != path.nodes[0].cell ? (false ? 1 : 0) : (this.CurrentNavType == path.nodes[0].navType ? 1 : 0)) | ((atNextNode = cell == path.nodes[1].cell && this.CurrentNavType == path.nodes[1].navType) ? 1 : 0)) != 0;
    }
    return flag && PathFinder.ValidatePath(this.NavGrid, this.GetCurrentAbilities(), ref path, this.flags);
  }

  private bool TryBuildPathFromCache(int cachedCell, int reservedCell, ref PathFinder.Path path)
  {
    return this.PathGrid.BuildPath(cachedCell, reservedCell, this.CurrentNavType, ref path) && this.ValidatePath(ref path, out bool _);
  }

  public void AdvancePath(bool trigger_advance = true)
  {
    this.cachedCell = Grid.PosToCell((KMonoBehaviour) this);
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
    {
      if (!this.Stop())
        this.Trigger(-766531887, (object) null);
    }
    else if (this.cachedCell == this.reservedCell && this.CurrentNavType != NavType.Tube)
    {
      this.Stop(true);
    }
    else
    {
      bool atNextNode;
      if (!this.ValidatePath(ref this.path, out atNextNode))
      {
        this.SetReservedCell(this.tactic.GetCellPreferences(Grid.PosToCell(this.target), this.targetOffsets, this));
        if (this.reservedCell == NavigationReservations.InvalidReservation)
          this.path.Clear();
        else if (!this.TryBuildPathFromCache(this.cachedCell, this.reservedCell, ref this.path))
        {
          PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(this.cachedCell, this.CurrentNavType, this.flags);
          PathFinder.UpdatePath(this.NavGrid, this.GetCurrentAbilities(), potential_path, (PathFinderQuery) PathFinderQueries.cellQuery.Reset(this.reservedCell), ref this.path);
          if (this.executePathProbeTaskAsync)
            AsyncPathProber.Instance.ApplyNavigationFailedPenalty(this);
        }
      }
      else if (atNextNode)
        this.path.nodes.RemoveAt(0);
      if (this.path.IsValid())
      {
        this.BeginTransition(this.NavGrid.transitions[(int) this.path.nodes[1].transitionId]);
        this.distanceTravelledByNavType[this.CurrentNavType] = Mathf.Max(this.distanceTravelledByNavType[this.CurrentNavType] + 1, this.distanceTravelledByNavType[this.CurrentNavType]);
      }
      else if (this.path.HasArrived())
      {
        this.Stop(true);
      }
      else
      {
        this.ClearReservedCell();
        this.Stop();
      }
    }
    if (!trigger_advance)
      return;
    this.Trigger(1347184327, (object) null);
  }

  public NavGrid.Transition GetNextTransition()
  {
    return this.NavGrid.transitions[(int) this.path.nodes[1].transitionId];
  }

  public bool Stop(bool arrived_at_destination = false, bool play_idle = true)
  {
    this.target = (KMonoBehaviour) null;
    this.targetOffsets = (CellOffset[]) null;
    this.path.Clear();
    this.smi.sm.moveTarget.Set((KMonoBehaviour) null, this.smi);
    this.transitionDriver.EndTransition();
    if (play_idle)
      this.animController.Play(this.NavGrid.GetIdleAnim(this.CurrentNavType), KAnim.PlayMode.Loop);
    if (arrived_at_destination)
    {
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.normal.arrived);
      return true;
    }
    if (this.smi.GetCurrentState() != this.smi.sm.normal.moving)
      return false;
    this.ClearReservedCell();
    this.smi.GoTo((StateMachine.BaseState) this.smi.sm.normal.failed);
    return true;
  }

  private void SimEveryTick(float dt)
  {
    if (!this.IsMoving())
      return;
    this.transitionDriver.UpdateTransition(dt);
  }

  public void UpdateProbe(bool forceUpdate = false)
  {
    if (!forceUpdate && this.executePathProbeTaskAsync)
      return;
    if (this.reportOccupation)
    {
      ListPool<int, Navigator>.PooledList pooledList = ListPool<int, Navigator>.Allocate();
      PathProber.Run(this, (List<int>) pooledList);
      MinionGroupProber.Get().Occupy((List<int>) pooledList);
      MinionGroupProber.Get().Vacate(this.occupiedCells);
      this.occupiedCells.Clear();
      this.occupiedCells.AddRange((IEnumerable<int>) pooledList);
      pooledList.Recycle();
    }
    else
      PathProber.Run(this);
  }

  public void DrawPath()
  {
    if (!this.gameObject.activeInHierarchy || !this.IsMoving())
      return;
    NavPathDrawer.Instance.DrawPath(this.animController.GetPivotSymbolPosition(), this.path);
  }

  public void Pause(string reason) => this.smi.sm.isPaused.Set(true, this.smi);

  public void Unpause(string reason) => this.smi.sm.isPaused.Set(false, this.smi);

  private void OnDefeated(object data)
  {
    this.ClearReservedCell();
    this.Stop(play_idle: false);
  }

  private void ClearReservedCell()
  {
    if (this.reservedCell == NavigationReservations.InvalidReservation)
      return;
    NavigationReservations.Instance.RemoveOccupancy(this.reservedCell);
    this.reservedCell = NavigationReservations.InvalidReservation;
  }

  private void SetReservedCell(int cell)
  {
    this.ClearReservedCell();
    this.reservedCell = cell;
    NavigationReservations.Instance.AddOccupancy(cell);
  }

  public int GetReservedCell() => this.reservedCell;

  public int GetAnchorCell() => this.AnchorCell;

  public bool IsValidNavType(NavType nav_type) => this.NavGrid.HasNavTypeData(nav_type);

  public void SetCurrentNavType(NavType nav_type)
  {
    this.CurrentNavType = nav_type;
    this.AnchorCell = NavTypeHelper.GetAnchorCell(nav_type, Grid.PosToCell((KMonoBehaviour) this));
    NavGrid.NavTypeData navTypeData = this.NavGrid.GetNavTypeData(this.CurrentNavType);
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    Vector2 one = Vector2.one;
    if (navTypeData.flipX)
      one.x = -1f;
    if (navTypeData.flipY)
      one.y = -1f;
    Matrix2x3 matrix2x3 = Matrix2x3.Translate(navTypeData.animControllerOffset * 200f) * Matrix2x3.Rotate(navTypeData.rotation) * Matrix2x3.Scale(one);
    component.navMatrix = matrix2x3;
  }

  private void OnRefreshUserMenu(object data)
  {
    if (this.gameObject.HasTag(GameTags.Dead))
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, (UnityEngine.Object) NavPathDrawer.Instance.GetNavigator() != (UnityEngine.Object) this ? new KIconButtonMenu.ButtonInfo("action_navigable_regions", (string) UI.USERMENUACTIONS.DRAWPATHS.NAME, new System.Action(this.OnDrawPaths), tooltipText: (string) UI.USERMENUACTIONS.DRAWPATHS.TOOLTIP) : new KIconButtonMenu.ButtonInfo("action_navigable_regions", (string) UI.USERMENUACTIONS.DRAWPATHS.NAME_OFF, new System.Action(this.OnDrawPaths), tooltipText: (string) UI.USERMENUACTIONS.DRAWPATHS.TOOLTIP_OFF), 0.1f);
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_follow_cam", (string) UI.USERMENUACTIONS.FOLLOWCAM.NAME, new System.Action(this.OnFollowCam), tooltipText: (string) UI.USERMENUACTIONS.FOLLOWCAM.TOOLTIP), 0.3f);
  }

  private void OnFollowCam()
  {
    if ((UnityEngine.Object) CameraController.Instance.followTarget == (UnityEngine.Object) this.transform)
      CameraController.Instance.ClearFollowTarget();
    else
      CameraController.Instance.SetFollowTarget(this.transform);
  }

  private void OnDrawPaths()
  {
    if ((UnityEngine.Object) NavPathDrawer.Instance.GetNavigator() != (UnityEngine.Object) this)
      NavPathDrawer.Instance.SetNavigator(this);
    else
      NavPathDrawer.Instance.ClearNavigator();
  }

  private void OnSelectObject(object data) => NavPathDrawer.Instance.ClearNavigator();

  public void OnStore(object data)
  {
    if ((data is Storage ? 1 : (data != null ? (((Boxed<bool>) data).value ? 1 : 0) : 0)) == 0)
      return;
    this.Stop();
  }

  public PathFinderAbilities GetCurrentAbilities()
  {
    this.abilities.Refresh();
    return this.abilities;
  }

  public void SetAbilities(PathFinderAbilities abilities) => this.abilities = abilities;

  public bool CanReach(IApproachable approachable)
  {
    return this.CanReach(approachable.GetCell(), approachable.GetOffsets());
  }

  public bool CanReach(int cell, CellOffset[] offsets)
  {
    foreach (CellOffset offset in offsets)
    {
      if (this.CanReach(Grid.OffsetCell(cell, offset)))
        return true;
    }
    return false;
  }

  public bool CanReach(int cell) => this.GetNavigationCost(cell) != -1;

  public int GetNavigationCost(int cell)
  {
    return Grid.IsValidCell(cell) ? this.PathGrid.GetCost(cell) : -1;
  }

  public int GetNavigationCostIgnoreProberOffset(int cell, CellOffset[] offsets)
  {
    return this.PathGrid.GetCostIgnoreProberOffset(cell, offsets);
  }

  public int GetNavigationCost(int cell, CellOffset[] offsets)
  {
    int navigationCost1 = -1;
    int length = offsets.Length;
    for (int index = 0; index < length; ++index)
    {
      int navigationCost2 = this.GetNavigationCost(Grid.OffsetCell(cell, offsets[index]));
      if (navigationCost2 != -1 && (navigationCost1 == -1 || navigationCost2 < navigationCost1))
        navigationCost1 = navigationCost2;
    }
    return navigationCost1;
  }

  public int GetNavigationCost(int cell, IReadOnlyList<CellOffset> offsets)
  {
    int navigationCost1 = -1;
    int count = offsets.Count;
    for (int index = 0; index < count; ++index)
    {
      int navigationCost2 = this.GetNavigationCost(Grid.OffsetCell(cell, offsets[index]));
      if (navigationCost2 != -1 && (navigationCost1 == -1 || navigationCost2 < navigationCost1))
        navigationCost1 = navigationCost2;
    }
    return navigationCost1;
  }

  public int GetNavigationCost(IApproachable approachable)
  {
    return this.GetNavigationCost(approachable.GetCell(), approachable.GetOffsets());
  }

  public void RunQuery(PathFinderQuery query)
  {
    PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(Grid.PosToCell((KMonoBehaviour) this), this.CurrentNavType, this.flags);
    PathFinder.Run(this.NavGrid, this.GetCurrentAbilities(), potential_path, query);
  }

  public void SetFlags(PathFinder.PotentialPath.Flags new_flags) => this.flags |= new_flags;

  public void ClearFlags(PathFinder.PotentialPath.Flags new_flags) => this.flags &= ~new_flags;

  [Conditional("ENABLE_DETAILED_NAVIGATOR_PROFILE_INFO")]
  public static void BeginDetailedSample(string region_name)
  {
  }

  [Conditional("ENABLE_DETAILED_NAVIGATOR_PROFILE_INFO")]
  public static void EndDetailedSample(string region_name)
  {
  }

  public class ActiveTransition
  {
    public int x;
    public int y;
    public bool isLooping;
    public NavType start;
    public NavType end;
    public HashedString preAnim;
    public HashedString anim;
    public float speed;
    public float animSpeed = 1f;
    public NavGrid.Transition navGridTransition;

    public void Init(NavGrid.Transition transition, float default_speed)
    {
      this.x = transition.x;
      this.y = transition.y;
      this.isLooping = transition.isLooping;
      this.start = transition.start;
      this.end = transition.end;
      this.preAnim = (HashedString) transition.preAnim;
      this.anim = (HashedString) transition.anim;
      this.speed = default_speed;
      this.animSpeed = transition.animSpeed;
      this.navGridTransition = transition;
    }

    public void Copy(Navigator.ActiveTransition other)
    {
      this.x = other.x;
      this.y = other.y;
      this.isLooping = other.isLooping;
      this.start = other.start;
      this.end = other.end;
      this.preAnim = other.preAnim;
      this.anim = other.anim;
      this.speed = other.speed;
      this.animSpeed = other.animSpeed;
      this.navGridTransition = other.navGridTransition;
    }
  }

  public class StatesInstance(Navigator master) : 
    GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.GameInstance(master)
  {
  }

  public class States : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator>
  {
    public StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.TargetParameter moveTarget;
    public StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.BoolParameter isPaused = new StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.BoolParameter(false);
    public Navigator.States.NormalStates normal;
    public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State paused;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.normal.stopped;
      this.saveHistory = true;
      this.normal.ParamTransition<bool>((StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.Parameter<bool>) this.isPaused, this.paused, GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.IsTrue);
      this.normal.moving.Enter((StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State.Callback) (smi => smi.BoxingTrigger<GameHashes>(1027377649, GameHashes.ObjectMovementWakeUp))).Update("UpdateNavigator", (Action<Navigator.StatesInstance, float>) ((smi, dt) => smi.master.SimEveryTick(dt)), UpdateRate.SIM_EVERY_TICK, true).Exit((StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State.Callback) (smi => smi.BoxingTrigger<GameHashes>(1027377649, GameHashes.ObjectMovementSleep)));
      this.normal.arrived.TriggerOnEnter(GameHashes.DestinationReached).GoTo(this.normal.stopped);
      this.normal.failed.TriggerOnEnter(GameHashes.NavigationFailed).GoTo(this.normal.stopped);
      this.normal.stopped.Enter((StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State.Callback) (smi => smi.master.SubscribeUnstuckFunctions())).DoNothing().Exit((StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State.Callback) (smi => smi.master.UnsubscribeUnstuckFunctions()));
      this.paused.ParamTransition<bool>((StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.Parameter<bool>) this.isPaused, (GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State) this.normal, GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.IsFalse);
    }

    public class NormalStates : 
      GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State
    {
      public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State moving;
      public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State arrived;
      public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State failed;
      public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State stopped;
    }
  }

  public class Scanner<T> where T : KMonoBehaviour
  {
    private static readonly CellOffset[] NO_OFFSETS = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    private readonly int radius;
    private readonly ScenePartitionerLayer layer;
    private readonly Func<T, bool> filterFn;
    private CellOffset[] offsets;
    private Action<T, List<CellOffset>> offsetsFn;
    private int? early_out_threshold;

    public Scanner(int radius, ScenePartitionerLayer layer, Func<T, bool> filterFn)
    {
      this.radius = radius;
      this.layer = layer;
      this.filterFn = filterFn;
      this.offsets = Navigator.Scanner<T>.NO_OFFSETS;
      this.offsetsFn = (Action<T, List<CellOffset>>) null;
      this.early_out_threshold = new int?();
    }

    public void SetConstantOffsets(CellOffset[] offsets) => this.offsets = offsets;

    public void SetDynamicOffsetsFn(Action<T, List<CellOffset>> offsetsFn)
    {
      this.offsetsFn = offsetsFn;
    }

    public void SetEarlyOutThreshold(int early_out_threshold)
    {
      this.early_out_threshold = new int?(early_out_threshold);
    }

    private int NavCostFromConstantOffsets(
      Navigator navigator,
      T destinationObject,
      CellOffset[] offsets)
    {
      return navigator.GetNavigationCost(Grid.PosToCell(destinationObject.gameObject), offsets);
    }

    private int NavCostFromDynamicOffsets(
      Navigator navigator,
      T destinationObject,
      Action<T, List<CellOffset>> offsetsFn)
    {
      ListPool<CellOffset, Navigator>.PooledList offsets = ListPool<CellOffset, Navigator>.Allocate();
      offsetsFn(destinationObject, (List<CellOffset>) offsets);
      int navigationCost = navigator.GetNavigationCost(Grid.PosToCell(destinationObject.gameObject), (IReadOnlyList<CellOffset>) offsets);
      offsets.Recycle();
      return navigationCost;
    }

    public T Scan(Vector2I gridPos, Navigator navigator)
    {
      ListPool<ScenePartitionerEntry, Navigator>.PooledList pooledList = ListPool<ScenePartitionerEntry, Navigator>.Allocate();
      GameScenePartitioner.Instance.GatherEntries(gridPos.x - this.radius, gridPos.y - this.radius, this.radius * 2, this.radius * 2, this.layer, (List<ScenePartitionerEntry>) pooledList);
      T obj = default (T);
      int num1 = -1;
      if (this.early_out_threshold.HasValue)
      {
        pooledList.Shuffle<ScenePartitionerEntry>();
        if (this.offsetsFn != null)
        {
          for (int index = 0; index < pooledList.Count; ++index)
          {
            T destinationObject = pooledList[index].obj as T;
            if (this.filterFn(destinationObject))
            {
              int num2 = this.NavCostFromDynamicOffsets(navigator, destinationObject, this.offsetsFn);
              if (num2 != -1 && ((UnityEngine.Object) obj == (UnityEngine.Object) null || num2 < num1))
              {
                obj = destinationObject;
                num1 = num2;
                if (num2 <= this.early_out_threshold.Value)
                  break;
              }
            }
          }
        }
        else
        {
          for (int index = 0; index < pooledList.Count; ++index)
          {
            T destinationObject = pooledList[index].obj as T;
            if (this.filterFn(destinationObject))
            {
              int num3 = this.NavCostFromConstantOffsets(navigator, destinationObject, this.offsets);
              if (num3 != -1 && ((UnityEngine.Object) obj == (UnityEngine.Object) null || num3 < num1))
              {
                obj = destinationObject;
                num1 = num3;
                if (num3 <= this.early_out_threshold.Value)
                  break;
              }
            }
          }
        }
      }
      else if (this.offsetsFn != null)
      {
        for (int index = 0; index < pooledList.Count; ++index)
        {
          T destinationObject = pooledList[index].obj as T;
          if (this.filterFn(destinationObject))
          {
            int num4 = this.NavCostFromDynamicOffsets(navigator, destinationObject, this.offsetsFn);
            if (num4 != -1 && ((UnityEngine.Object) obj == (UnityEngine.Object) null || num4 < num1))
            {
              obj = destinationObject;
              num1 = num4;
            }
          }
        }
      }
      else
      {
        for (int index = 0; index < pooledList.Count; ++index)
        {
          T destinationObject = pooledList[index].obj as T;
          if (this.filterFn(destinationObject))
          {
            int num5 = this.NavCostFromConstantOffsets(navigator, destinationObject, this.offsets);
            if (num5 != -1 && ((UnityEngine.Object) obj == (UnityEngine.Object) null || num5 < num1))
            {
              obj = destinationObject;
              num1 = num5;
            }
          }
        }
      }
      pooledList.Recycle();
      return obj;
    }
  }
}
