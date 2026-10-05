// Decompiled with JetBrains decompiler
// Type: RoomProber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RoomProber : ISim1000ms
{
  private RoomProber.Generation generation;
  public List<Room> rooms = new List<Room>();
  private readonly KCompactedVector<CavityInfo> cavityInfos = new KCompactedVector<CavityInfo>(1024 /*0x0400*/);
  private readonly RoomProber.Cell[] grid;
  private readonly RoomProber.RefreshModule refresh;
  private readonly HashSet<int> solidChanges = new HashSet<int>();
  private bool dirty = true;

  public RoomProber()
  {
    CavityInfo newCavity = this.CreateNewCavity();
    newCavity.cells = new List<int>()
    {
      Capacity = Grid.CellCount
    };
    for (int index = 0; index < Grid.CellCount; ++index)
      newCavity.cells.Add(index);
    this.grid = new RoomProber.Cell[Grid.CellCount];
    Array.Fill<RoomProber.Cell>(this.grid, new RoomProber.Cell()
    {
      cavityID = newCavity.handle,
      generation = 0U
    });
    this.generation = new RoomProber.Generation(this.grid);
    this.solidChanges.Add(Grid.XYToCell(1, 1));
    this.refresh = new RoomProber.RefreshModule(this);
    this.refresh.Initialize();
    this.Refresh();
    Game.Instance.OnSpawnComplete += new System.Action(this.Refresh);
    World.Instance.OnSolidChanged += new Action<int>(this.SolidChangedEvent);
    GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingsChanged));
    GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[2], new Action<int, object>(this.OnBuildingsChanged));
  }

  private void SolidChangedEvent(int cell) => this.SolidChangedEvent(cell, true);

  private void OnBuildingsChanged(int cell, object building)
  {
    if (this.GetCavityForCell(cell) == null)
      return;
    this.solidChanges.Add(cell);
    this.dirty = true;
  }

  public void TriggerBuildingChangedEvent(int cell, object building)
  {
    this.OnBuildingsChanged(cell, building);
  }

  public void SolidChangedEvent(int cell, bool ignoreDoors)
  {
    if (ignoreDoors && Grid.HasDoor[cell])
      return;
    this.solidChanges.Add(cell);
    this.dirty = true;
  }

  private CavityInfo CreateNewCavity()
  {
    CavityInfo initial_data = new CavityInfo();
    initial_data.handle = this.cavityInfos.Allocate(initial_data);
    return initial_data;
  }

  private static bool IsCavityBoundary(int cell)
  {
    return (Grid.BuildMasks[cell] & (Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation)) != 0 || Grid.HasDoor[cell];
  }

  public void Refresh() => this.refresh.Run();

  public void Sim1000ms(float dt)
  {
    if (!this.dirty)
      return;
    this.Refresh();
  }

  private void CreateRoom(CavityInfo cavity)
  {
    Debug.Assert(cavity.room == null);
    Room room = new Room() { cavity = cavity };
    cavity.room = room;
    this.rooms.Add(room);
    room.roomType = Db.Get().RoomTypes.GetRoomType(room);
    this.AssignBuildingsToRoom(room);
  }

  private void ClearRoom(Room room)
  {
    this.UnassignBuildingsToRoom(room);
    room.CleanUp();
    this.rooms.Remove(room);
  }

  private void RefreshRooms(List<KPrefabID> dirtyEntities)
  {
    int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
    foreach (CavityInfo data in this.cavityInfos.GetDataList())
    {
      if (data.dirty)
      {
        Debug.Assert(data.room == null, (object) "I expected info.room to always be null by this point");
        if (data.NumCells > 0)
        {
          if (data.NumCells <= maxRoomSize)
            this.CreateRoom(data);
          foreach (KMonoBehaviour building in data.buildings)
            building.Trigger(144050788, (object) data.room);
          foreach (KMonoBehaviour plant in data.plants)
            plant.Trigger(144050788, (object) data.room);
        }
        data.dirty = false;
      }
    }
    foreach (KPrefabID dirtyEntity in dirtyEntities)
    {
      if ((UnityEngine.Object) dirtyEntity != (UnityEngine.Object) null)
        dirtyEntity.Trigger(144050788, (object) null);
    }
    this.dirty = false;
  }

  private void AssignBuildingsToRoom(Room room)
  {
    Debug.Assert(room != null);
    RoomType roomType = room.roomType;
    if (roomType == Db.Get().RoomTypes.Neutral)
      return;
    foreach (KPrefabID building in room.buildings)
    {
      Assignable component;
      if (!((UnityEngine.Object) building == (UnityEngine.Object) null) && !building.HasTag(GameTags.NotRoomAssignable) && building.TryGetComponent<Assignable>(out component) && (roomType.primary_constraint == null || !roomType.primary_constraint.building_criteria(building)))
        component.Assign((IAssignableIdentity) room);
    }
  }

  private void UnassignKPrefabIDs(Room room, List<KPrefabID> buildings)
  {
    foreach (KPrefabID building in buildings)
    {
      if (!((UnityEngine.Object) building == (UnityEngine.Object) null))
      {
        building.Trigger(144050788, (object) null);
        Assignable component;
        if (building.TryGetComponent<Assignable>(out component) && component.assignee == room)
          component.Unassign();
      }
    }
  }

  private void UnassignBuildingsToRoom(Room room)
  {
    Debug.Assert(room != null);
    this.UnassignKPrefabIDs(room, room.buildings);
    this.UnassignKPrefabIDs(room, room.plants);
  }

  public void UpdateRoom(CavityInfo cavity)
  {
    if (cavity == null)
      return;
    if (cavity.room != null)
    {
      this.ClearRoom(cavity.room);
      cavity.room = (Room) null;
    }
    this.CreateRoom(cavity);
    foreach (KPrefabID building in cavity.buildings)
    {
      if ((UnityEngine.Object) building != (UnityEngine.Object) null)
        building.Trigger(144050788, (object) cavity.room);
    }
    foreach (KPrefabID plant in cavity.plants)
    {
      if ((UnityEngine.Object) plant != (UnityEngine.Object) null)
        plant.Trigger(144050788, (object) cavity.room);
    }
  }

  public Room GetRoomOfGameObject(GameObject go)
  {
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      return (Room) null;
    int cell = Grid.PosToCell(go);
    if (!Grid.IsValidCell(cell))
      return (Room) null;
    return this.GetCavityForCell(cell)?.room;
  }

  public bool IsInRoomType(GameObject go, RoomType checkType)
  {
    Room roomOfGameObject = this.GetRoomOfGameObject(go);
    if (roomOfGameObject == null)
      return false;
    RoomType roomType = roomOfGameObject.roomType;
    return checkType == roomType;
  }

  private CavityInfo GetCavityInfo(HandleVector<int>.Handle id)
  {
    return !id.IsValid() ? (CavityInfo) null : this.cavityInfos.GetData(id);
  }

  public CavityInfo GetCavityForCell(int cell)
  {
    return !Grid.IsValidCell(cell) ? (CavityInfo) null : this.GetCavityInfo(this.grid[cell].cavityID);
  }

  public class Tuning : TuningData<RoomProber.Tuning>
  {
    public int maxRoomSize;
  }

  private struct Cell
  {
    public HandleVector<int>.Handle cavityID;
    public uint generation;
    public static RoomProber.Cell INVALID = new RoomProber.Cell()
    {
      cavityID = HandleVector<int>.InvalidHandle,
      generation = 0
    };
  }

  private struct Generation(RoomProber.Cell[] grid)
  {
    private uint value = 1;
    private readonly RoomProber.Cell[] grid = grid;

    public uint Next()
    {
      uint num = this.value++;
      if (num == 0U)
      {
        Array.Fill<RoomProber.Cell>(this.grid, RoomProber.Cell.INVALID);
        num = this.value++;
      }
      return num;
    }
  }

  private struct RefreshModule(RoomProber roomProber)
  {
    private readonly RoomProber.RefreshModule.CavityBuilder cavityBuilder = new RoomProber.RefreshModule.CavityBuilder();
    private readonly RoomProber roomProber = roomProber;
    private readonly List<int> dirtyCells = new List<int>();
    private readonly List<HandleVector<int>.Handle> condemnedCavities = new List<HandleVector<int>.Handle>();
    private readonly List<CavityInfo> newCavities = new List<CavityInfo>();
    private readonly List<KPrefabID> dirtyEntities = new List<KPrefabID>();
    private readonly HashSet<HandleVector<int>.Handle> visitedCavities = new HashSet<HandleVector<int>.Handle>();
    private readonly HashSet<RoomProber.RefreshModule.BuildingId> visitedBuildings = new HashSet<RoomProber.RefreshModule.BuildingId>();
    private Func<int, FloodFill.BoundaryCheckResult> cavityBoundary = (Func<int, FloodFill.BoundaryCheckResult>) null;

    public void Initialize()
    {
      this.cavityBoundary = (Func<int, FloodFill.BoundaryCheckResult>) (cell => !RoomProber.IsCavityBoundary(cell) ? FloodFill.BoundaryCheckResult.Continue : FloodFill.BoundaryCheckResult.Halt);
    }

    public void Run()
    {
      this.CollectDirtyCells();
      this.CollectCondemnedCavities();
      this.BuildNewCavities();
      foreach (HandleVector<int>.Handle condemnedCavity in this.condemnedCavities)
      {
        CavityInfo data = this.roomProber.cavityInfos.GetData(condemnedCavity);
        this.dirtyEntities.Capacity = Math.Max(this.dirtyEntities.Capacity, this.dirtyEntities.Count + data.creatures.Count + data.otherEntities.Count);
        foreach (KPrefabID creature in data.creatures)
          this.dirtyEntities.Add(creature);
        foreach (KPrefabID otherEntity in data.otherEntities)
          this.dirtyEntities.Add(otherEntity);
        if (data.room != null)
          this.roomProber.ClearRoom(data.room);
        this.roomProber.cavityInfos.Free(condemnedCavity);
      }
      this.AddRoomContentsToCavities();
      this.roomProber.RefreshRooms(this.dirtyEntities);
      this.Recycle();
    }

    private readonly void Recycle()
    {
      this.dirtyCells.Clear();
      this.condemnedCavities.Clear();
      this.newCavities.Clear();
      this.dirtyEntities.Clear();
    }

    private readonly unsafe void CollectDirtyCells()
    {
      int* numPtr = stackalloc int[5]
      {
        0,
        -Grid.WidthInCells,
        -1,
        1,
        Grid.WidthInCells
      };
      uint num = this.roomProber.generation.Next();
      foreach (int solidChange in this.roomProber.solidChanges)
      {
        for (int index = 0; index < 5; ++index)
        {
          int cell1 = solidChange + numPtr[index];
          if (Grid.IsValidCell(cell1))
          {
            RoomProber.Cell cell2 = this.roomProber.grid[cell1];
            if ((int) cell2.generation != (int) num)
            {
              this.roomProber.grid[cell1] = new RoomProber.Cell()
              {
                cavityID = cell2.cavityID,
                generation = num
              };
              this.dirtyCells.Add(cell1);
            }
          }
        }
      }
      this.roomProber.solidChanges.Clear();
    }

    private readonly void CollectCondemnedCavities()
    {
      uint num = this.roomProber.generation.Next();
      foreach (int dirtyCell in this.dirtyCells)
      {
        RoomProber.Cell cell1 = this.roomProber.grid[dirtyCell];
        if ((int) cell1.generation != (int) num)
        {
          this.roomProber.grid[dirtyCell] = new RoomProber.Cell()
          {
            cavityID = cell1.cavityID,
            generation = num
          };
          if (cell1.cavityID.IsValid())
          {
            if (this.visitedCavities.Add(cell1.cavityID))
              this.condemnedCavities.Add(cell1.cavityID);
            foreach (int cell2 in this.roomProber.cavityInfos.GetData(cell1.cavityID).cells)
              this.roomProber.grid[cell2] = RoomProber.Cell.INVALID;
          }
        }
      }
      this.visitedCavities.Clear();
    }

    private readonly void BuildNewCavities()
    {
      int num = 0;
      foreach (HandleVector<int>.Handle condemnedCavity in this.condemnedCavities)
        num += this.roomProber.cavityInfos.GetData(condemnedCavity).NumCells;
      this.dirtyCells.Capacity = Math.Max(this.dirtyCells.Capacity, this.dirtyCells.Count + num);
      foreach (HandleVector<int>.Handle condemnedCavity in this.condemnedCavities)
      {
        foreach (int cell in this.roomProber.cavityInfos.GetData(condemnedCavity).cells)
          this.dirtyCells.Add(cell);
      }
      int index = this.condemnedCavities.Count > 0 ? 0 : -1;
      RoomProber.RefreshModule.VisitTracker visited = new RoomProber.RefreshModule.VisitTracker(this.roomProber.grid, this.roomProber.generation.Next());
      foreach (int dirtyCell in this.dirtyCells)
      {
        if ((int) this.roomProber.grid[dirtyCell].generation != (int) visited.Generation)
        {
          if (RoomProber.IsCavityBoundary(dirtyCell))
          {
            this.roomProber.grid[dirtyCell] = new RoomProber.Cell()
            {
              cavityID = HandleVector<int>.InvalidHandle,
              generation = visited.Generation
            };
          }
          else
          {
            CavityInfo newCavity = this.roomProber.CreateNewCavity();
            if (index >= 0)
            {
              CavityInfo data = this.roomProber.cavityInfos.GetData(this.condemnedCavities[index]);
              newCavity.cells = data.cells;
              newCavity.cells.Clear();
              data.cells = (List<int>) null;
              ++index;
              if (index >= this.condemnedCavities.Count)
                index = -1;
            }
            else
              newCavity.cells = new List<int>();
            this.cavityBuilder.Reset(newCavity.handle);
            FloodFill.DepthTraverse<FloodFill.PredicateCondition, RoomProber.RefreshModule.VisitTracker, FloodFill.NoMaxDepth, RoomProber.RefreshModule.Visitor>(dirtyCell, new FloodFill.PredicateCondition(this.cavityBoundary), visited, new FloodFill.NoMaxDepth(), new RoomProber.RefreshModule.Visitor(this.roomProber.grid, newCavity.cells, newCavity.handle, this.cavityBuilder));
            DebugUtil.DevAssert(this.cavityBuilder.NumCells > 0, "Degenerate cavities should have been detected and rejected prior to this point");
            newCavity.minX = this.cavityBuilder.MinX;
            newCavity.minY = this.cavityBuilder.MinY;
            newCavity.maxX = this.cavityBuilder.MaxX;
            newCavity.maxY = this.cavityBuilder.MaxY;
            this.newCavities.Add(newCavity);
          }
        }
      }
    }

    private void AddRoomContentsToCavities()
    {
      int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
      foreach (CavityInfo newCavity in this.newCavities)
      {
        if (newCavity.NumCells <= maxRoomSize)
        {
          foreach (int cell in newCavity.cells)
          {
            GameObject gameObject = Grid.Objects[cell, 1];
            if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
              gameObject = Grid.Objects[cell, 38];
            if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
            {
              KPrefabID component = gameObject.GetComponent<KPrefabID>();
              if (this.visitedBuildings.Add(new RoomProber.RefreshModule.BuildingId()
              {
                prefab = component.GetHashCode(),
                instance = component.InstanceID
              }))
              {
                if (component.HasTag(GameTags.RoomProberBuilding))
                  newCavity.AddBuilding(component);
                else if (component.HasTag(GameTags.Plant))
                  newCavity.AddPlants(component);
              }
            }
          }
        }
      }
      this.visitedBuildings.Clear();
    }

    private class CavityBuilder
    {
      public HandleVector<int>.Handle CavityID { get; private set; }

      public int MinX { get; private set; }

      public int MinY { get; private set; }

      public int MaxX { get; private set; }

      public int MaxY { get; private set; }

      public int NumCells { get; private set; }

      public void Reset(HandleVector<int>.Handle search_id)
      {
        this.CavityID = search_id;
        this.NumCells = 0;
        this.MinX = int.MaxValue;
        this.MinY = int.MaxValue;
        this.MaxX = 0;
        this.MaxY = 0;
      }

      public void AddCell(int flood_cell)
      {
        int x;
        int y;
        Grid.CellToXY(flood_cell, out x, out y);
        this.MinX = Math.Min(x, this.MinX);
        this.MinY = Math.Min(y, this.MinY);
        this.MaxX = Math.Max(x, this.MaxX);
        this.MaxY = Math.Max(y, this.MaxY);
        ++this.NumCells;
      }
    }

    private struct BuildingId
    {
      public int prefab;
      public int instance;
    }

    private struct VisitTracker(RoomProber.Cell[] grid, uint generation) : FloodFill.IVisitTracker
    {
      private readonly RoomProber.Cell[] grid = grid;

      public uint Generation { get; private set; } = generation;

      public bool Add(int cell)
      {
        RoomProber.Cell cell1 = this.grid[cell];
        if ((int) cell1.generation == (int) this.Generation)
          return false;
        this.grid[cell] = new RoomProber.Cell()
        {
          cavityID = cell1.cavityID,
          generation = this.Generation
        };
        return true;
      }

      public readonly bool Contains(int cell)
      {
        return (int) this.grid[cell].generation == (int) this.Generation;
      }
    }

    private struct Visitor(
      RoomProber.Cell[] grid,
      List<int> cavityCells,
      HandleVector<int>.Handle cavityID,
      RoomProber.RefreshModule.CavityBuilder cavityBuilder) : FloodFill.IVisitor
    {
      private readonly RoomProber.Cell[] grid = grid;
      private readonly List<int> cavityCells = cavityCells;
      private HandleVector<int>.Handle cavityID = cavityID;
      private readonly RoomProber.RefreshModule.CavityBuilder cavityBuilder = cavityBuilder;

      public readonly bool EarlyOut => false;

      public void VisitBoundary(int cell)
      {
        RoomProber.Cell cell1 = this.grid[cell];
        this.grid[cell] = new RoomProber.Cell()
        {
          cavityID = HandleVector<int>.InvalidHandle,
          generation = cell1.generation
        };
      }

      public void VisitCell(int cell)
      {
        RoomProber.Cell cell1 = this.grid[cell];
        this.grid[cell] = new RoomProber.Cell()
        {
          cavityID = this.cavityID,
          generation = cell1.generation
        };
        this.cavityCells.Add(cell);
        this.cavityBuilder.AddCell(cell);
      }
    }
  }
}
