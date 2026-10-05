// Decompiled with JetBrains decompiler
// Type: PathFinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
public class PathFinder
{
  public static int InvalidHandle = -1;
  public static int InvalidIdx = -1;
  public static int InvalidCell = -1;
  public static PathGrid PathGrid;
  private static readonly Func<int, FloodFill.BoundaryCheckResult> notSolid = (Func<int, FloodFill.BoundaryCheckResult>) (cell => !Grid.Solid[cell] ? FloodFill.BoundaryCheckResult.Continue : FloodFill.BoundaryCheckResult.Halt);

  public static void Initialize()
  {
    NavType[] valid_nav_types = new NavType[11];
    for (int index = 0; index < valid_nav_types.Length; ++index)
      valid_nav_types[index] = (NavType) index;
    PathFinder.PathGrid = new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, valid_nav_types);
    for (int origin = 0; origin < Grid.CellCount; ++origin)
    {
      if (Grid.Visible[origin] > (byte) 0 && Grid.Spawnable[origin] > (byte) 0)
        FloodFill.DepthTraverse<FloodFill.PredicateCondition, PathFinder.PathTracker, FloodFill.NoMaxDepth, FloodFill.DoNothing>(origin, new FloodFill.PredicateCondition(PathFinder.notSolid), new PathFinder.PathTracker(), new FloodFill.NoMaxDepth(), new FloodFill.DoNothing());
    }
    Grid.OnReveal += new Action<int>(PathFinder.OnReveal);
  }

  private static void OnReveal(int cell)
  {
  }

  public static void UpdatePath(
    NavGrid nav_grid,
    PathFinderAbilities abilities,
    PathFinder.PotentialPath potential_path,
    PathFinderQuery query,
    ref PathFinder.Path path)
  {
    PathFinder.Run(nav_grid, abilities, potential_path, query, ref path);
  }

  public static bool ValidatePath(
    NavGrid nav_grid,
    PathFinderAbilities abilities,
    ref PathFinder.Path path,
    PathFinder.PotentialPath.Flags flags)
  {
    if (!path.IsValid())
      return false;
    for (int index1 = 0; index1 < path.nodes.Count; ++index1)
    {
      PathFinder.Path.Node node1 = path.nodes[index1];
      if (index1 < path.nodes.Count - 1)
      {
        PathFinder.Path.Node node2 = path.nodes[index1 + 1];
        int index2 = node1.cell * nav_grid.maxLinksPerCell;
        bool flag = false;
        for (NavGrid.Link link = nav_grid.Links[index2]; link.link != PathFinder.InvalidHandle; link = nav_grid.Links[index2])
        {
          if (link.link == node2.cell && node2.navType == link.endNavType && node1.navType == link.startNavType)
          {
            PathFinder.PotentialPath path1 = new PathFinder.PotentialPath(node2.cell, node1.navType, flags);
            flag = abilities.TraversePath(ref path1, node1.cell, node1.navType, 0, (int) link.transitionId, false);
            if (flag)
            {
              flags = path1.flags;
              break;
            }
          }
          ++index2;
        }
        if (!flag)
          return false;
      }
    }
    return true;
  }

  public static void Run(
    NavGrid nav_grid,
    PathFinderAbilities abilities,
    PathFinder.PotentialPath potential_path,
    PathFinderQuery query)
  {
    int invalidCell = PathFinder.InvalidCell;
    NavType result_nav_type = NavType.NumNavTypes;
    query.ClearResult();
    if (!Grid.IsValidCell(potential_path.cell))
      return;
    PathFinder.FindPaths(nav_grid, ref abilities, potential_path, query, PathFinder.Temp.Potentials, ref invalidCell, ref result_nav_type);
    if (invalidCell == PathFinder.InvalidCell)
      return;
    bool is_cell_in_range = false;
    PathFinder.Cell cell = PathFinder.PathGrid.GetCell(invalidCell, result_nav_type, out is_cell_in_range);
    query.SetResult(invalidCell, cell.cost, result_nav_type);
  }

  public static void Run(
    NavGrid nav_grid,
    PathFinderAbilities abilities,
    PathFinder.PotentialPath potential_path,
    PathFinderQuery query,
    ref PathFinder.Path path)
  {
    PathFinder.Run(nav_grid, abilities, potential_path, query);
    if (query.GetResultCell() != PathFinder.InvalidCell)
      PathFinder.BuildResultPath(query.GetResultCell(), query.GetResultNavType(), ref path);
    else
      path.Clear();
  }

  private static void BuildResultPath(
    int path_cell,
    NavType path_nav_type,
    ref PathFinder.Path path)
  {
    if (path_cell == PathFinder.InvalidCell)
      return;
    bool is_cell_in_range = false;
    PathFinder.Cell cell = PathFinder.PathGrid.GetCell(path_cell, path_nav_type, out is_cell_in_range);
    path.Clear();
    path.cost = cell.cost;
    while (path_cell != PathFinder.InvalidCell)
    {
      path.AddNode(new PathFinder.Path.Node()
      {
        cell = path_cell,
        navType = cell.navType,
        transitionId = cell.transitionId
      });
      path_cell = cell.parent;
      if (path_cell != PathFinder.InvalidCell)
        cell = PathFinder.PathGrid.GetCell(path_cell, cell.parentNavType, out is_cell_in_range);
    }
    if (path.nodes == null)
      return;
    for (int index = 0; index < path.nodes.Count / 2; ++index)
    {
      PathFinder.Path.Node node = path.nodes[index];
      path.nodes[index] = path.nodes[path.nodes.Count - index - 1];
      path.nodes[path.nodes.Count - index - 1] = node;
    }
  }

  private static void FindPaths(
    NavGrid nav_grid,
    ref PathFinderAbilities abilities,
    PathFinder.PotentialPath potential_path,
    PathFinderQuery query,
    PathFinder.PotentialList potentials,
    ref int result_cell,
    ref NavType result_nav_type)
  {
    potentials.Clear();
    ushort new_serial_no = (ushort) ((uint) PathFinder.PathGrid.SerialNo + 1U);
    if (new_serial_no == (ushort) 0)
      ++new_serial_no;
    PathFinder.PathGrid.BeginUpdate(new_serial_no, potential_path.cell);
    bool is_cell_in_range;
    PathFinder.Cell cell1 = PathFinder.PathGrid.GetCell(potential_path, out is_cell_in_range);
    PathFinder.AddPotential(potential_path, Grid.InvalidCell, NavType.NumNavTypes, 0, (byte) 0, potentials, PathFinder.PathGrid, ref cell1);
    int maxValue = int.MaxValue;
    while (potentials.Count > 0)
    {
      KeyValuePair<int, PathFinder.PotentialPath> keyValuePair = potentials.Next();
      PathFinder.Cell cell2 = PathFinder.PathGrid.GetCell(keyValuePair.Value, out is_cell_in_range);
      if (cell2.cost == keyValuePair.Key)
      {
        if ((cell2.navType == NavType.Tube || !query.IsMatch(keyValuePair.Value.cell, cell2.parent, cell2.cost) ? 0 : (cell2.cost < maxValue ? 1 : 0)) != 0)
        {
          result_cell = keyValuePair.Value.cell;
          int cost = cell2.cost;
          result_nav_type = cell2.navType;
          break;
        }
        PathFinder.AddPotentials(nav_grid.potentialScratchPad, keyValuePair.Value, cell2.cost, ref abilities, query, nav_grid.maxLinksPerCell, nav_grid.Links, potentials, PathFinder.PathGrid, cell2.parent, cell2.parentNavType);
      }
    }
    PathFinder.PathGrid.EndUpdate();
  }

  public static void AddPotential(
    PathFinder.PotentialPath potential_path,
    int parent_cell,
    NavType parent_nav_type,
    int cost,
    byte transition_id,
    PathFinder.PotentialList potentials,
    PathGrid path_grid,
    ref PathFinder.Cell cell_data)
  {
    cell_data.cost = cost;
    cell_data.parent = parent_cell;
    cell_data.SetNavTypes(potential_path.navType, parent_nav_type);
    cell_data.transitionId = transition_id;
    potentials.Add(cost, potential_path);
    path_grid.SetCell(potential_path, ref cell_data);
  }

  public static bool IsSubmerged(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return false;
    int index = Grid.CellAbove(cell);
    return Grid.IsValidCell(index) && Grid.Element[index].IsLiquid || Grid.Element[cell].IsLiquid && Grid.IsValidCell(index) && Grid.Solid[index];
  }

  public static void AddPotentials(
    PathFinder.PotentialScratchPad potential_scratch_pad,
    PathFinder.PotentialPath potential,
    int cost,
    ref PathFinderAbilities abilities,
    PathFinderQuery query,
    int max_links_per_cell,
    NavGrid.Link[] links,
    PathFinder.PotentialList potentials,
    PathGrid path_grid,
    int parent_cell,
    NavType parent_nav_type)
  {
    if (!Grid.IsValidCell(potential.cell))
      return;
    int num1 = 0;
    NavGrid.Link[] withCorrectNavType = potential_scratch_pad.linksWithCorrectNavType;
    int num2 = potential.cell * max_links_per_cell;
    int num3 = num2 + max_links_per_cell;
    for (int index = num2; index < num3; ++index)
    {
      NavGrid.Link link1 = links[index];
      int link2 = link1.link;
      if (link2 != PathFinder.InvalidHandle)
      {
        if (link1.startNavType == potential.navType && (parent_cell != link2 || parent_nav_type != link1.startNavType))
          withCorrectNavType[num1++] = link1;
      }
      else
        break;
    }
    int num4 = 0;
    PathFinder.PotentialScratchPad.PathGridCellData[] linksInCellRange = potential_scratch_pad.linksInCellRange;
    for (int index = 0; index < num1; ++index)
    {
      NavGrid.Link link3 = withCorrectNavType[index];
      int link4 = link3.link;
      bool is_cell_in_range = false;
      PathFinder.Cell cell = path_grid.GetCell(link4, link3.endNavType, out is_cell_in_range);
      if (is_cell_in_range)
      {
        int num5 = cost + (int) link3.cost;
        bool flag1 = PathFinder.IsSubmerged(link4);
        if (flag1 || link3.endNavType == NavType.Swim)
        {
          int submergedPathCostPenalty = abilities.GetSubmergedPathCostPenalty(potential, link3);
          num5 += submergedPathCostPenalty;
        }
        int num6 = cell.cost == -1 ? 1 : 0;
        bool flag2 = num5 < cell.cost;
        if (num6 != 0 || flag2)
          linksInCellRange[num4++] = new PathFinder.PotentialScratchPad.PathGridCellData()
          {
            isSubmerged = flag1,
            pathGridCell = cell,
            link = link3
          };
      }
    }
    for (int index = 0; index < num4; ++index)
    {
      PathFinder.PotentialScratchPad.PathGridCellData pathGridCellData = linksInCellRange[index];
      NavGrid.Link link5 = pathGridCellData.link;
      int link6 = link5.link;
      PathFinder.Cell pathGridCell = pathGridCellData.pathGridCell;
      int cost1 = cost + (int) link5.cost;
      PathFinder.PotentialPath path = potential with
      {
        cell = link6,
        navType = link5.endNavType
      };
      if (pathGridCellData.isSubmerged || path.navType == NavType.Swim)
      {
        int submergedPathCostPenalty = abilities.GetSubmergedPathCostPenalty(path, link5);
        cost1 += submergedPathCostPenalty;
      }
      int num7 = pathGridCell.cost == -1 ? 1 : 0;
      bool flag = cost1 < pathGridCell.cost;
      if (num7 != 0 || flag)
      {
        PathFinder.PotentialPath.Flags flags1 = path.flags;
        int num8 = abilities.TraversePath(ref path, potential.cell, potential.navType, cost1, (int) link5.transitionId, pathGridCellData.isSubmerged) ? 1 : 0;
        int flags2 = (int) path.flags;
        if (num8 != 0)
          PathFinder.AddPotential(path, potential.cell, potential.navType, cost1, link5.transitionId, potentials, path_grid, ref pathGridCell);
      }
    }
  }

  public static void DestroyStatics()
  {
    PathFinder.PathGrid.OnCleanUp();
    PathFinder.PathGrid = (PathGrid) null;
    PathFinder.Temp.Potentials.Clear();
  }

  public struct Cell
  {
    public int cost;
    public int parent;
    public ushort queryId;
    private byte navTypes;
    public byte transitionId;

    public NavType navType => (NavType) ((uint) this.navTypes & 15U);

    public NavType parentNavType => (NavType) ((uint) this.navTypes >> 4);

    public void SetNavTypes(NavType type, NavType parent_type)
    {
      this.navTypes = (byte) (type | (NavType) ((uint) parent_type << 4));
    }
  }

  public struct PotentialPath(int cell, NavType nav_type, PathFinder.PotentialPath.Flags flags)
  {
    public int cell = cell;
    public NavType navType = nav_type;

    public void SetFlags(PathFinder.PotentialPath.Flags new_flags) => this.flags |= new_flags;

    public void ClearFlags(PathFinder.PotentialPath.Flags new_flags) => this.flags &= ~new_flags;

    public bool HasFlag(PathFinder.PotentialPath.Flags flag) => this.HasAnyFlag(flag);

    public bool HasAnyFlag(PathFinder.PotentialPath.Flags mask) => (this.flags & mask) != 0;

    public PathFinder.PotentialPath.Flags flags { get; private set; } = flags;

    [Flags]
    public enum Flags : byte
    {
      None = 0,
      HasAtmoSuit = 1,
      HasJetPack = 2,
      HasOxygenMask = 4,
      PerformSuitChecks = 8,
      HasLeadSuit = 16, // 0x10
    }
  }

  public struct Path
  {
    public int cost;
    public List<PathFinder.Path.Node> nodes;

    public void AddNode(PathFinder.Path.Node node)
    {
      if (this.nodes == null)
        this.nodes = new List<PathFinder.Path.Node>();
      this.nodes.Add(node);
    }

    public bool IsValid() => this.nodes != null && this.nodes.Count > 1;

    public bool HasArrived() => this.nodes != null && this.nodes.Count > 0;

    public void Clear()
    {
      this.cost = 0;
      if (this.nodes == null)
        return;
      this.nodes.Clear();
    }

    public struct Node
    {
      public int cell;
      public NavType navType;
      public byte transitionId;
    }
  }

  public class PotentialList
  {
    private PathFinder.PotentialList.HOTQueue<PathFinder.PotentialPath> queue = new PathFinder.PotentialList.HOTQueue<PathFinder.PotentialPath>();

    public KeyValuePair<int, PathFinder.PotentialPath> Next() => this.queue.Dequeue();

    public int Count => this.queue.Count;

    public void Add(int cost, PathFinder.PotentialPath path) => this.queue.Enqueue(cost, path);

    public void Clear() => this.queue.Clear();

    public class PriorityQueue<TValue>
    {
      private List<KeyValuePair<int, TValue>> _baseHeap;

      public PriorityQueue() => this._baseHeap = new List<KeyValuePair<int, TValue>>();

      public void Enqueue(int priority, TValue value) => this.Insert(priority, value);

      public KeyValuePair<int, TValue> Dequeue()
      {
        KeyValuePair<int, TValue> keyValuePair = this._baseHeap[0];
        this.DeleteRoot();
        return keyValuePair;
      }

      public KeyValuePair<int, TValue> Peek()
      {
        if (this.Count > 0)
          return this._baseHeap[0];
        throw new InvalidOperationException("Priority queue is empty");
      }

      private void ExchangeElements(int pos1, int pos2)
      {
        KeyValuePair<int, TValue> keyValuePair = this._baseHeap[pos1];
        this._baseHeap[pos1] = this._baseHeap[pos2];
        this._baseHeap[pos2] = keyValuePair;
      }

      private void Insert(int priority, TValue value)
      {
        this._baseHeap.Add(new KeyValuePair<int, TValue>(priority, value));
        this.HeapifyFromEndToBeginning(this._baseHeap.Count - 1);
      }

      private int HeapifyFromEndToBeginning(int pos)
      {
        if (pos >= this._baseHeap.Count)
          return -1;
        int num;
        for (; pos > 0; pos = num)
        {
          num = (pos - 1) / 2;
          KeyValuePair<int, TValue> keyValuePair = this._baseHeap[num];
          int key1 = keyValuePair.Key;
          keyValuePair = this._baseHeap[pos];
          int key2 = keyValuePair.Key;
          if (key1 - key2 > 0)
            this.ExchangeElements(num, pos);
          else
            break;
        }
        return pos;
      }

      private void DeleteRoot()
      {
        if (this._baseHeap.Count <= 1)
        {
          this._baseHeap.Clear();
        }
        else
        {
          this._baseHeap[0] = this._baseHeap[this._baseHeap.Count - 1];
          this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
          this.HeapifyFromBeginningToEnd(0);
        }
      }

      private void HeapifyFromBeginningToEnd(int pos)
      {
        int count = this._baseHeap.Count;
        if (pos >= count)
          return;
        while (true)
        {
          int num = pos;
          int index1 = 2 * pos + 1;
          int index2 = 2 * pos + 2;
          if (index1 < count && this._baseHeap[num].Key - this._baseHeap[index1].Key > 0)
            num = index1;
          if (index2 < count && this._baseHeap[num].Key - this._baseHeap[index2].Key > 0)
            num = index2;
          if (num != pos)
          {
            this.ExchangeElements(num, pos);
            pos = num;
          }
          else
            break;
        }
      }

      public void Clear() => this._baseHeap.Clear();

      public int Count => this._baseHeap.Count;
    }

    private class HOTQueue<TValue>
    {
      private PathFinder.PotentialList.PriorityQueue<TValue> hotQueue = new PathFinder.PotentialList.PriorityQueue<TValue>();
      private PathFinder.PotentialList.PriorityQueue<TValue> coldQueue = new PathFinder.PotentialList.PriorityQueue<TValue>();
      private int hotThreshold = int.MinValue;
      private int coldThreshold = int.MinValue;
      private int count;

      public KeyValuePair<int, TValue> Dequeue()
      {
        if (this.hotQueue.Count == 0)
        {
          PathFinder.PotentialList.PriorityQueue<TValue> hotQueue = this.hotQueue;
          this.hotQueue = this.coldQueue;
          this.coldQueue = hotQueue;
          this.hotThreshold = this.coldThreshold;
        }
        --this.count;
        return this.hotQueue.Dequeue();
      }

      public void Enqueue(int priority, TValue value)
      {
        if (priority <= this.hotThreshold)
        {
          this.hotQueue.Enqueue(priority, value);
        }
        else
        {
          this.coldQueue.Enqueue(priority, value);
          this.coldThreshold = Math.Max(this.coldThreshold, priority);
        }
        ++this.count;
      }

      public KeyValuePair<int, TValue> Peek()
      {
        if (this.hotQueue.Count == 0)
        {
          PathFinder.PotentialList.PriorityQueue<TValue> hotQueue = this.hotQueue;
          this.hotQueue = this.coldQueue;
          this.coldQueue = hotQueue;
          this.hotThreshold = this.coldThreshold;
        }
        return this.hotQueue.Peek();
      }

      public void Clear()
      {
        this.count = 0;
        this.hotThreshold = int.MinValue;
        this.hotQueue.Clear();
        this.coldThreshold = int.MinValue;
        this.coldQueue.Clear();
      }

      public int Count => this.count;
    }
  }

  private class Temp
  {
    public static PathFinder.PotentialList Potentials = new PathFinder.PotentialList();
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  private struct PathTracker : FloodFill.IVisitTracker
  {
    public readonly bool Add(int cell)
    {
      if (this.Contains(cell))
        return false;
      Grid.AllowPathfinding[cell] = true;
      return true;
    }

    public readonly bool Contains(int cell) => Grid.AllowPathfinding[cell];
  }

  public class PotentialScratchPad
  {
    public NavGrid.Link[] linksWithCorrectNavType;
    public PathFinder.PotentialScratchPad.PathGridCellData[] linksInCellRange;

    public PotentialScratchPad(int max_links_per_cell)
    {
      this.linksWithCorrectNavType = new NavGrid.Link[max_links_per_cell];
      this.linksInCellRange = new PathFinder.PotentialScratchPad.PathGridCellData[max_links_per_cell];
    }

    public struct PathGridCellData
    {
      public PathFinder.Cell pathGridCell;
      public NavGrid.Link link;
      public bool isSubmerged;
    }
  }
}
