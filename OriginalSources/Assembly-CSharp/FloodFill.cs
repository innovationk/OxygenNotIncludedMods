// Decompiled with JetBrains decompiler
// Type: FloodFill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

#nullable disable
public static class FloodFill
{
  private static readonly ThreadLocal<FloodFill.RetainedQueue> tlOpen = new ThreadLocal<FloodFill.RetainedQueue>((Func<FloodFill.RetainedQueue>) (() => new FloodFill.RetainedQueue()));
  private static readonly ThreadLocal<HashSet<int>> tlVisited = new ThreadLocal<HashSet<int>>((Func<HashSet<int>>) (() => new HashSet<int>()));
  private static readonly ThreadLocal<byte[]> tlGrid = new ThreadLocal<byte[]>((Func<byte[]>) (() => new byte[Grid.CellCount]));
  private static readonly ThreadLocal<byte> tlGeneration = new ThreadLocal<byte>((Func<byte>) (() => (byte) 0));
  private const ulong LEFT_BITS = 2305843009213693952 /*0x2000000000000000*/;
  private const ulong RIGHT_BITS = 4611686018427387904 /*0x4000000000000000*/;
  private const ulong UP_BITS = 6917529027641081856 /*0x6000000000000000*/;
  private const ulong DOWN_BITS = 9223372036854775808 /*0x8000000000000000*/;
  private const ulong INDEX_MASK = 4294967295 /*0xFFFFFFFF*/;
  private const int INDEX_SHIFT = 0;
  private const int INDEX_MAX = 2147483647 /*0x7FFFFFFF*/;
  private const ulong DEPTH_MASK = 2305843004918726656;
  private const int DEPTH_SHIFT = 32 /*0x20*/;
  private const uint DEPTH_MAX = 536870911 /*0x1FFFFFFF*/;
  private const ulong QUEUED_FROM_MASK = 16140901064495857664 /*0xE000000000000000*/;
  private const int QUEUED_FROM_SHIFT = 61;
  private static readonly ThreadLocal<FloodFill.RetainedQueue> tlAbove = new ThreadLocal<FloodFill.RetainedQueue>((Func<FloodFill.RetainedQueue>) (() => new FloodFill.RetainedQueue()));
  private static readonly ThreadLocal<FloodFill.RetainedQueue> tlBelow = new ThreadLocal<FloodFill.RetainedQueue>((Func<FloodFill.RetainedQueue>) (() => new FloodFill.RetainedQueue()));
  private static readonly ThreadLocal<FloodFill.RetainedQueue> tlLeft = new ThreadLocal<FloodFill.RetainedQueue>((Func<FloodFill.RetainedQueue>) (() => new FloodFill.RetainedQueue()));
  private static readonly ThreadLocal<FloodFill.RetainedQueue> tlRight = new ThreadLocal<FloodFill.RetainedQueue>((Func<FloodFill.RetainedQueue>) (() => new FloodFill.RetainedQueue()));

  public static void BreadthTraverse<BoundaryCondition, VisitTracker, MyMaxDepth, Visitor>(
    int startCell,
    BoundaryCondition boundaryCondition,
    VisitTracker visited,
    MyMaxDepth maxDepth,
    Visitor visitor)
    where BoundaryCondition : FloodFill.IBoundaryCondition
    where VisitTracker : FloodFill.IVisitTracker
    where MyMaxDepth : FloodFill.IMaxDepth
    where Visitor : FloodFill.IVisitor
  {
    FloodFill.VerifyConstants();
    if (!maxDepth.Check(0))
      return;
    FloodFill.RetainedQueue retainedQueue = FloodFill.tlOpen.Value;
    retainedQueue.Clear();
    retainedQueue.Enqueue((ulong) startCell);
    while (retainedQueue.Count > 0)
    {
      ulong num1 = retainedQueue.Dequeue();
      int cell = (int) ((long) num1 & (long) uint.MaxValue);
      if (Grid.IsValidCell(cell) && visited.Add(cell))
      {
        if (boundaryCondition.Check(cell) == FloodFill.BoundaryCheckResult.Halt)
        {
          visitor.VisitBoundary(cell);
        }
        else
        {
          visitor.VisitCell(cell);
          if (visitor.EarlyOut)
            break;
          uint cellDepth = (uint) ((num1 & 2305843004918726656UL) >> 32 /*0x20*/) + 1U;
          DebugUtil.DevAssert(cellDepth <= 536870911U /*0x1FFFFFFF*/, "nextDepth overflowed allocated bitfield");
          DebugUtil.DevAssert(cellDepth <= (uint) int.MaxValue, "nextDepth cannot be cast to int");
          if (maxDepth.Check((int) cellDepth))
          {
            ulong num2 = (ulong) cellDepth << 32 /*0x20*/;
            retainedQueue.Enqueue((ulong) Grid.CellLeft(cell) | num2);
            retainedQueue.Enqueue((ulong) Grid.CellRight(cell) | num2);
            retainedQueue.Enqueue((ulong) Grid.CellAbove(cell) | num2);
            retainedQueue.Enqueue((ulong) Grid.CellBelow(cell) | num2);
          }
        }
      }
    }
  }

  public static void BreadthVisit(
    int startCell,
    Func<int, FloodFill.BoundaryCheckResult> boundaryCondition,
    HashSet<int> visitedCells)
  {
    FloodFill.BreadthTraverse<FloodFill.PredicateCondition, FloodFill.HashSetVisitTracker, FloodFill.NoMaxDepth, FloodFill.DoNothing>(startCell, new FloodFill.PredicateCondition(boundaryCondition), new FloodFill.HashSetVisitTracker()
    {
      visited = visitedCells
    }, new FloodFill.NoMaxDepth(), new FloodFill.DoNothing());
  }

  public static void BreadthVisit(
    int startCell,
    Func<int, FloodFill.BoundaryCheckResult> boundaryCondition,
    int maxDepth)
  {
    FloodFill.BreadthTraverse<FloodFill.PredicateCondition, FloodFill.GenerationGrid, FloodFill.MaxDepth, FloodFill.DoNothing>(startCell, new FloodFill.PredicateCondition(boundaryCondition), FloodFill.GenerationGrid.Default(), new FloodFill.MaxDepth(maxDepth), new FloodFill.DoNothing());
  }

  public static void BreadthVisit(
    int startCell,
    Func<int, FloodFill.BoundaryCheckResult> boundaryCondition)
  {
    FloodFill.BreadthTraverse<FloodFill.PredicateCondition, FloodFill.GenerationGrid, FloodFill.NoMaxDepth, FloodFill.DoNothing>(startCell, new FloodFill.PredicateCondition(boundaryCondition), FloodFill.GenerationGrid.Default(), new FloodFill.NoMaxDepth(), new FloodFill.DoNothing());
  }

  public static void BreadthCollect(
    int startCell,
    Func<int, FloodFill.BoundaryCheckResult> boundaryCondition,
    HashSet<int> visitedCells,
    List<int> validCells)
  {
    FloodFill.BreadthTraverse<FloodFill.PredicateCondition, FloodFill.HashSetVisitTracker, FloodFill.NoMaxDepth, FloodFill.Collector>(startCell, new FloodFill.PredicateCondition(boundaryCondition), new FloodFill.HashSetVisitTracker()
    {
      visited = visitedCells
    }, new FloodFill.NoMaxDepth(), new FloodFill.Collector(validCells));
  }

  public static void BreadthCollect(
    int startCell,
    Func<int, FloodFill.BoundaryCheckResult> boundaryCondition,
    HashSet<int> visitedCells,
    List<int> validCells,
    int maxDepth)
  {
    FloodFill.BreadthTraverse<FloodFill.PredicateCondition, FloodFill.HashSetVisitTracker, FloodFill.MaxDepth, FloodFill.Collector>(startCell, new FloodFill.PredicateCondition(boundaryCondition), new FloodFill.HashSetVisitTracker()
    {
      visited = visitedCells
    }, new FloodFill.MaxDepth(maxDepth), new FloodFill.Collector(validCells));
  }

  public static void BreadthCollect(
    int startCell,
    Func<int, FloodFill.BoundaryCheckResult> boundaryCondition,
    List<int> validCells,
    int maxDepth)
  {
    FloodFill.BreadthTraverse<FloodFill.PredicateCondition, FloodFill.GenerationGrid, FloodFill.MaxDepth, FloodFill.Collector>(startCell, new FloodFill.PredicateCondition(boundaryCondition), FloodFill.GenerationGrid.Default(), new FloodFill.MaxDepth(maxDepth), new FloodFill.Collector(validCells));
  }

  public static int Find<MaxDepth>(
    Func<int, bool> criteria,
    int startCell,
    MaxDepth maxDepth,
    bool stopAtSolid,
    bool stopAtLiquid)
    where MaxDepth : FloodFill.IMaxDepth
  {
    FloodFill.Finder visitor = new FloodFill.Finder(criteria);
    FloodFill.ElementCheck boundaryCondition = new FloodFill.ElementCheck(stopAtSolid, stopAtLiquid);
    if (maxDepth.Check(10))
      FloodFill.BreadthTraverse<FloodFill.ElementCheck, FloodFill.GenerationGrid, MaxDepth, FloodFill.Finder>(startCell, boundaryCondition, FloodFill.GenerationGrid.Default(), maxDepth, visitor);
    else
      FloodFill.BreadthTraverse<FloodFill.ElementCheck, FloodFill.HashSetVisitTracker, MaxDepth, FloodFill.Finder>(startCell, boundaryCondition, FloodFill.HashSetVisitTracker.Default(), maxDepth, visitor);
    return visitor.Cell;
  }

  public static bool Any<MaxDepth>(
    Func<int, bool> fn,
    int start_cell,
    MaxDepth max_depth,
    bool stop_at_solid,
    bool stop_at_liquid)
    where MaxDepth : FloodFill.IMaxDepth
  {
    return FloodFill.Find<MaxDepth>(fn, start_cell, max_depth, stop_at_solid, stop_at_liquid) != -1;
  }

  public static int FindBest(
    Func<int, float> rateCell,
    Func<int, FloodFill.BoundaryCheckResult> boundaryCondition,
    int startCell,
    int maxCellEvaluations = -1)
  {
    if (maxCellEvaluations == 0)
      return Grid.InvalidCell;
    FloodFill.Scorer visitor = new FloodFill.Scorer(rateCell, maxCellEvaluations);
    FloodFill.BreadthTraverse<FloodFill.PredicateCondition, FloodFill.GenerationGrid, FloodFill.NoMaxDepth, FloodFill.Scorer>(startCell, new FloodFill.PredicateCondition(boundaryCondition), FloodFill.GenerationGrid.Default(), new FloodFill.NoMaxDepth(), visitor);
    return visitor.BestCell;
  }

  public static void BreadthTraverseNoBacktrack<BoundaryCondition, VisitTracker, MyMaxDepth, Visitor>(
    int startCell,
    BoundaryCondition boundaryCondition,
    VisitTracker visited,
    MyMaxDepth maxDepth,
    Visitor visitor)
    where BoundaryCondition : FloodFill.IBoundaryCondition
    where VisitTracker : FloodFill.IVisitTracker
    where MyMaxDepth : FloodFill.IMaxDepth
    where Visitor : FloodFill.IVisitor
  {
    FloodFill.VerifyConstants();
    if (!maxDepth.Check(0))
      return;
    FloodFill.RetainedQueue retainedQueue = FloodFill.tlOpen.Value;
    retainedQueue.Clear();
    retainedQueue.Enqueue((ulong) startCell);
    while (retainedQueue.Count > 0)
    {
      ulong num1 = retainedQueue.Dequeue();
      int cell = (int) ((long) num1 & (long) uint.MaxValue);
      if (Grid.IsValidCell(cell) && visited.Add(cell))
      {
        if (boundaryCondition.Check(cell) == FloodFill.BoundaryCheckResult.Halt)
        {
          visitor.VisitBoundary(cell);
        }
        else
        {
          visitor.VisitCell(cell);
          if (visitor.EarlyOut)
            break;
          uint cellDepth = (uint) ((num1 & 2305843004918726656UL) >> 32 /*0x20*/) + 1U;
          DebugUtil.DevAssert(cellDepth <= 536870911U /*0x1FFFFFFF*/, "nextDepth overflowed allocated bitfield");
          DebugUtil.DevAssert(cellDepth <= (uint) int.MaxValue, "nextDepth cannot be cast to int");
          if (maxDepth.Check((int) cellDepth))
          {
            int num2 = (int) (byte) ((num1 & 16140901064495857664UL /*0xE000000000000000*/) >> 61);
            ulong num3 = (ulong) cellDepth << 32 /*0x20*/;
            if (num2 != 1)
              retainedQueue.Enqueue((ulong) ((long) Grid.CellLeft(cell) | (long) num3 | 4611686018427387904L /*0x4000000000000000*/));
            if (num2 != 2)
              retainedQueue.Enqueue((ulong) ((long) Grid.CellRight(cell) | (long) num3 | 2305843009213693952L /*0x2000000000000000*/));
            if (num2 != 3)
              retainedQueue.Enqueue((ulong) ((long) Grid.CellAbove(cell) | (long) num3 | long.MinValue));
            if (num2 != 4)
              retainedQueue.Enqueue((ulong) ((long) Grid.CellBelow(cell) | (long) num3 | 6917529027641081856L /*0x6000000000000000*/));
          }
        }
      }
    }
  }

  public static void DepthTraverse<BoundaryCondition, VisitTracker, MyMaxDepth, Visitor>(
    int origin,
    BoundaryCondition boundaryCondition,
    VisitTracker visited,
    MyMaxDepth maxDepth,
    Visitor visitor)
    where BoundaryCondition : FloodFill.IBoundaryCondition
    where VisitTracker : FloodFill.IVisitTracker
    where MyMaxDepth : FloodFill.IMaxDepth
    where Visitor : FloodFill.IVisitor
  {
    FloodFill.VerifyConstants();
    if (!maxDepth.Check(0) || !Grid.IsValidCell(origin) || !visited.Add(origin))
      return;
    FloodFill.BoundaryCheckResult boundaryCheckResult = boundaryCondition.Check(origin);
    if (boundaryCheckResult == FloodFill.BoundaryCheckResult.Continue)
    {
      visitor.VisitCell(origin);
      if (visitor.EarlyOut)
        return;
      FloodFill.RetainedQueue rays1 = FloodFill.tlAbove.Value;
      FloodFill.RetainedQueue rays2 = FloodFill.tlBelow.Value;
      FloodFill.RetainedQueue rays3 = FloodFill.tlLeft.Value;
      FloodFill.RetainedQueue rays4 = FloodFill.tlRight.Value;
      rays1.Clear();
      rays2.Clear();
      rays3.Clear();
      rays4.Clear();
      ulong originCell = (ulong) origin | 0UL;
      SeedDirection(rays1);
      SeedDirection(rays2);
      SeedDirection(rays3);
      SeedDirection(rays4);
      FloodFill.NorthRay ray1 = FloodFill.NorthRay.Default();
      FloodFill.SouthRay ray2 = FloodFill.SouthRay.Default();
      FloodFill.WestRay ray3 = FloodFill.WestRay.Default();
      FloodFill.EastRay ray4 = FloodFill.EastRay.Default();
      while (rays3.Count + rays4.Count + rays1.Count + rays2.Count > 0)
      {
        bool flag = false;
        if (ray1.ForwardQueue.Count > 0)
          flag = !FloodFill.CastRay<FloodFill.NorthRay, BoundaryCondition, VisitTracker, MyMaxDepth, Visitor>(ray1, boundaryCondition, visited, maxDepth, visitor);
        else if (ray2.ForwardQueue.Count > 0)
          flag = !FloodFill.CastRay<FloodFill.SouthRay, BoundaryCondition, VisitTracker, MyMaxDepth, Visitor>(ray2, boundaryCondition, visited, maxDepth, visitor);
        if (flag)
          break;
        while (ray3.ForwardQueue.Count > 0)
        {
          flag = !FloodFill.CastRay<FloodFill.WestRay, BoundaryCondition, VisitTracker, MyMaxDepth, Visitor>(ray3, boundaryCondition, visited, maxDepth, visitor);
          if (flag)
            break;
        }
        while (ray4.ForwardQueue.Count > 0)
        {
          flag = !FloodFill.CastRay<FloodFill.EastRay, BoundaryCondition, VisitTracker, MyMaxDepth, Visitor>(ray4, boundaryCondition, visited, maxDepth, visitor);
          if (flag)
            break;
        }
        if (flag)
          break;
      }

      void SeedDirection(FloodFill.RetainedQueue rays) => rays.Enqueue(originCell);
    }
    else
    {
      DebugUtil.DevAssert(boundaryCheckResult == FloodFill.BoundaryCheckResult.Halt, "unexpected CheckResult value");
      visitor.VisitBoundary(origin);
    }
  }

  public static void DepthVisit(
    int startCell,
    Func<int, FloodFill.BoundaryCheckResult> boundaryCondition,
    HashSet<int> visitedCells)
  {
    FloodFill.DepthTraverse<FloodFill.PredicateCondition, FloodFill.HashSetVisitTracker, FloodFill.NoMaxDepth, FloodFill.DoNothing>(startCell, new FloodFill.PredicateCondition(boundaryCondition), new FloodFill.HashSetVisitTracker()
    {
      visited = visitedCells
    }, new FloodFill.NoMaxDepth(), new FloodFill.DoNothing());
  }

  public static void DepthVisit(
    int startCell,
    Func<int, FloodFill.BoundaryCheckResult> boundaryCondition)
  {
    FloodFill.DepthTraverse<FloodFill.PredicateCondition, FloodFill.GenerationGrid, FloodFill.NoMaxDepth, FloodFill.DoNothing>(startCell, new FloodFill.PredicateCondition(boundaryCondition), FloodFill.GenerationGrid.Default(), new FloodFill.NoMaxDepth(), new FloodFill.DoNothing());
  }

  public static void DepthCollect(
    int startCell,
    Func<int, FloodFill.BoundaryCheckResult> boundaryCondition,
    HashSet<int> visitedCells,
    List<int> validCells,
    int maxDepth)
  {
    FloodFill.DepthTraverse<FloodFill.PredicateCondition, FloodFill.HashSetVisitTracker, FloodFill.MaxDepth, FloodFill.Collector>(startCell, new FloodFill.PredicateCondition(boundaryCondition), new FloodFill.HashSetVisitTracker()
    {
      visited = visitedCells
    }, new FloodFill.MaxDepth(maxDepth), new FloodFill.Collector(validCells));
  }

  public static void DepthCollect(
    int startCell,
    Func<int, FloodFill.BoundaryCheckResult> boundaryCondition,
    HashSet<int> visitedCells,
    List<int> validCells)
  {
    FloodFill.DepthTraverse<FloodFill.PredicateCondition, FloodFill.HashSetVisitTracker, FloodFill.NoMaxDepth, FloodFill.Collector>(startCell, new FloodFill.PredicateCondition(boundaryCondition), new FloodFill.HashSetVisitTracker()
    {
      visited = visitedCells
    }, new FloodFill.NoMaxDepth(), new FloodFill.Collector(validCells));
  }

  public static void DepthCollect(
    int startCell,
    Func<int, FloodFill.BoundaryCheckResult> boundaryCondition,
    List<int> validCells,
    int maxDepth)
  {
    FloodFill.DepthTraverse<FloodFill.PredicateCondition, FloodFill.GenerationGrid, FloodFill.MaxDepth, FloodFill.Collector>(startCell, new FloodFill.PredicateCondition(boundaryCondition), FloodFill.GenerationGrid.Default(), new FloodFill.MaxDepth(maxDepth), new FloodFill.Collector(validCells));
  }

  public static void DepthCollect(
    int startCell,
    Func<int, FloodFill.BoundaryCheckResult> boundaryCondition,
    List<int> validCells)
  {
    FloodFill.DepthTraverse<FloodFill.PredicateCondition, FloodFill.GenerationGrid, FloodFill.NoMaxDepth, FloodFill.Collector>(startCell, new FloodFill.PredicateCondition(boundaryCondition), FloodFill.GenerationGrid.Default(), new FloodFill.NoMaxDepth(), new FloodFill.Collector(validCells));
  }

  private static void VerifyConstants()
  {
    DebugUtil.DevAssert(Grid.CellCount <= int.MaxValue, "Too few bits allocated to INDEX to handle all grid cells.");
    DebugUtil.DevAssert(true, "Unsigned DEPTH_MAX must not be larger than the maximum Int32 as that is the user-facing type for depth");
  }

  private static bool CastLateralRay<Ray, LateralRay, BoundaryCondition, VisitTracker, Visitor>(
    int originIndex,
    uint originDepth,
    int cellCount,
    Ray ray,
    LateralRay lateralRay,
    BoundaryCondition boundaryCondition,
    VisitTracker visited,
    Visitor visitor)
    where Ray : FloodFill.IRay
    where LateralRay : FloodFill.ILateralRay
    where BoundaryCondition : FloodFill.IBoundaryCondition
    where VisitTracker : FloodFill.IVisitTracker
    where Visitor : FloodFill.IVisitor
  {
    int cell = lateralRay.FromSourceCell(originIndex);
    if (!Grid.IsValidCell(cell))
      return true;
    bool flag = false;
    for (int index = 0; index != cellCount; ++index)
    {
      cell = ray.Forward(cell);
      if (visited.Add(cell))
      {
        flag = boundaryCondition.Check(cell) == FloodFill.BoundaryCheckResult.Continue;
        if (flag)
        {
          visitor.VisitCell(cell);
          if (visitor.EarlyOut)
            return false;
          ulong num = (ulong) originDepth + (ulong) index + 2UL;
          DebugUtil.DevAssert(num <= 536870911UL /*0x1FFFFFFF*/, "cellDepth overflowed allocated bitfield");
          lateralRay.Enqueue((ulong) cell | num << 32 /*0x20*/);
        }
        else
          visitor.VisitBoundary(cell);
      }
    }
    if (flag)
    {
      ulong num = (ulong) originDepth + (ulong) cellCount + 1UL;
      DebugUtil.DevAssert(num <= 536870911UL /*0x1FFFFFFF*/, "cellDepth overflowed allocated bitfield");
      ray.ForwardQueue.Enqueue((ulong) cell | num << 32 /*0x20*/);
    }
    return true;
  }

  private static bool CastRay<Ray, BoundaryCondition, VisitTracker, MyMaxDepth, Visitor>(
    Ray ray,
    BoundaryCondition boundaryCondition,
    VisitTracker visited,
    MyMaxDepth maxDepth,
    Visitor visitor)
    where Ray : FloodFill.IRay
    where BoundaryCondition : FloodFill.IBoundaryCondition
    where VisitTracker : FloodFill.IVisitTracker
    where MyMaxDepth : FloodFill.IMaxDepth
    where Visitor : FloodFill.IVisitor
  {
    long num = (long) ray.ForwardQueue.Dequeue();
    int originIndex = (int) (num & (long) uint.MaxValue);
    uint originDepth = (uint) ((num & 2305843004918726656L) >>> 32 /*0x20*/);
    uint cellDepth = originDepth;
    int cell = originIndex;
    for (; maxDepth.Check((int) cellDepth); ++cellDepth)
    {
      cell = ray.Forward(cell);
      if (Grid.IsValidCell(cell) && visited.Add(cell))
      {
        if (boundaryCondition.Check(cell) == FloodFill.BoundaryCheckResult.Halt)
        {
          visitor.VisitBoundary(cell);
          break;
        }
        visitor.VisitCell(cell);
        if (visitor.EarlyOut)
          return false;
      }
      else
        break;
    }
    DebugUtil.DevAssert(cellDepth <= (uint) int.MaxValue, "depth cannot be cast to int");
    int cellCount = (int) cellDepth - (int) originDepth;
    return cellCount == 0 || FloodFill.CastLateralRay<Ray, FloodFill.PortRay<Ray>, BoundaryCondition, VisitTracker, Visitor>(originIndex, originDepth, cellCount, ray, new FloodFill.PortRay<Ray>(ray), boundaryCondition, visited, visitor) && FloodFill.CastLateralRay<Ray, FloodFill.StarboardRay<Ray>, BoundaryCondition, VisitTracker, Visitor>(originIndex, originDepth, cellCount, ray, new FloodFill.StarboardRay<Ray>(ray), boundaryCondition, visited, visitor);
  }

  public interface IVisitTracker
  {
    bool Contains(int cell);

    bool Add(int cell);
  }

  public struct HashSetVisitTracker : FloodFill.IVisitTracker
  {
    public HashSet<int> visited;

    public readonly bool Contains(int cell) => this.visited.Contains(cell);

    public readonly bool Add(int cell) => this.visited.Add(cell);

    public static FloodFill.HashSetVisitTracker Default()
    {
      HashSet<int> intSet = FloodFill.tlVisited.Value;
      intSet.Clear();
      return new FloodFill.HashSetVisitTracker()
      {
        visited = intSet
      };
    }
  }

  public readonly struct GenerationGrid(byte generation, byte[] grid) : FloodFill.IVisitTracker
  {
    private readonly byte generation = generation;
    private readonly byte[] grid = grid;

    public bool Contains(int cell) => (int) this.grid[cell] == (int) this.generation;

    public bool Add(int cell)
    {
      if (this.Contains(cell))
        return false;
      this.grid[cell] = this.generation;
      return true;
    }

    public static FloodFill.GenerationGrid Default()
    {
      byte[] grid = FloodFill.tlGrid.Value;
      if (grid.Length != Grid.CellCount)
      {
        Debug.Log((object) "Resize FloodFill.GenerationGrid");
        FloodFill.tlGrid.Value = new byte[Grid.CellCount];
        grid = FloodFill.tlGrid.Value;
        FloodFill.tlGeneration.Value = (byte) 0;
      }
      byte generation = (byte) ((uint) FloodFill.tlGeneration.Value + 1U);
      if (generation == (byte) 0)
      {
        Debug.Log((object) "Reset FloodFill.GenerationGrid");
        Array.Clear((Array) grid, 0, grid.Length);
        FloodFill.tlGeneration.Value = (byte) 1;
        generation = (byte) 1;
      }
      else
        FloodFill.tlGeneration.Value = generation;
      return new FloodFill.GenerationGrid(generation, grid);
    }
  }

  public enum BoundaryCheckResult
  {
    Continue,
    Halt,
  }

  public interface IBoundaryCondition
  {
    FloodFill.BoundaryCheckResult Check(int cell);
  }

  public readonly struct PredicateCondition(Func<int, FloodFill.BoundaryCheckResult> predicate) : 
    FloodFill.IBoundaryCondition
  {
    private readonly Func<int, FloodFill.BoundaryCheckResult> predicate = predicate;

    public FloodFill.BoundaryCheckResult Check(int cell) => this.predicate(cell);
  }

  public readonly struct ElementCheck : FloodFill.IBoundaryCondition
  {
    private readonly bool stop_at_solid;
    private readonly bool stop_at_liquid;

    public ElementCheck(bool stop_at_solid, bool stop_at_liquid)
    {
      DebugUtil.DevAssert(stop_at_solid | stop_at_liquid, "No sense in running this if it never does anything");
      this.stop_at_solid = stop_at_solid;
      this.stop_at_liquid = stop_at_liquid;
    }

    public FloodFill.BoundaryCheckResult Check(int cell)
    {
      Element element = Grid.Element[cell];
      return this.stop_at_solid && element.IsSolid || this.stop_at_liquid && element.IsLiquid ? FloodFill.BoundaryCheckResult.Halt : FloodFill.BoundaryCheckResult.Continue;
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public readonly struct NoBoundary : FloodFill.IBoundaryCondition
  {
    public FloodFill.BoundaryCheckResult Check(int cell) => FloodFill.BoundaryCheckResult.Continue;
  }

  public interface IMaxDepth
  {
    bool Check(int cellDepth);
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct NoMaxDepth : FloodFill.IMaxDepth
  {
    public readonly bool Check(int cellDepth) => true;
  }

  public readonly struct MaxDepth(int maxDepth) : FloodFill.IMaxDepth
  {
    private readonly int value = maxDepth;

    public bool Check(int cellDepth) => cellDepth < this.value;
  }

  public interface IVisitor
  {
    void VisitCell(int cell);

    void VisitBoundary(int cell);

    bool EarlyOut { get; }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct DoNothing : FloodFill.IVisitor
  {
    public readonly void VisitCell(int cell)
    {
    }

    public readonly void VisitBoundary(int cell)
    {
    }

    public readonly bool EarlyOut => false;
  }

  public readonly struct Collector(List<int> cells) : FloodFill.IVisitor
  {
    private readonly List<int> cells = cells;

    public void VisitCell(int cell) => this.cells.Add(cell);

    public void VisitBoundary(int cell)
    {
    }

    public bool EarlyOut => false;
  }

  public class Finder : FloodFill.IVisitor
  {
    private readonly Func<int, bool> criteria;
    private int foundCell = Grid.InvalidCell;

    public int Cell => this.foundCell;

    public bool EarlyOut => this.foundCell != Grid.InvalidCell;

    public void VisitCell(int cell)
    {
      if (!this.criteria(cell))
        return;
      this.foundCell = cell;
    }

    public void VisitBoundary(int cell)
    {
    }

    public Finder(Func<int, bool> criteria) => this.criteria = criteria;
  }

  public class Scorer : FloodFill.IVisitor
  {
    private readonly Func<int, float> rateCell;
    private int threshold;
    private float bestScore;
    private int bestCell;

    public int BestCell => this.bestCell;

    public bool EarlyOut => this.threshold == 0;

    public Scorer(Func<int, float> rateCell, int threshold)
    {
      this.rateCell = rateCell;
      this.threshold = threshold;
      this.bestScore = float.NegativeInfinity;
      this.bestCell = Grid.InvalidCell;
    }

    public void VisitCell(int cell)
    {
      float num = this.rateCell(cell);
      if ((double) num > (double) this.bestScore)
      {
        this.bestScore = num;
        this.bestCell = cell;
      }
      if (this.threshold <= 0)
        return;
      --this.threshold;
    }

    public void VisitBoundary(int cell)
    {
    }
  }

  private class RetainedQueue
  {
    private ulong[] buffer;
    private int head;
    private int tail;
    private int count;

    public int Count => this.count;

    public RetainedQueue(int initialCapacity = 1024 /*0x0400*/)
    {
      this.buffer = new ulong[initialCapacity];
    }

    public void Enqueue(ulong item)
    {
      if (this.count == this.buffer.Length)
        this.Grow();
      this.buffer[this.tail] = item;
      this.tail = (this.tail + 1) % this.buffer.Length;
      ++this.count;
    }

    public ulong Dequeue()
    {
      long num = (long) this.buffer[this.head];
      this.head = (this.head + 1) % this.buffer.Length;
      --this.count;
      return (ulong) num;
    }

    public void Clear()
    {
      this.head = 0;
      this.tail = 0;
      this.count = 0;
    }

    public void EnsureCapacity(int capacity)
    {
      if (this.buffer.Length >= capacity)
        return;
      this.Resize(capacity);
    }

    private void Grow() => this.Resize(this.buffer.Length * 2);

    private void Resize(int newCapacity)
    {
      ulong[] destinationArray = new ulong[newCapacity];
      if (this.head < this.tail)
      {
        Array.Copy((Array) this.buffer, this.head, (Array) destinationArray, 0, this.count);
      }
      else
      {
        Array.Copy((Array) this.buffer, this.head, (Array) destinationArray, 0, this.buffer.Length - this.head);
        Array.Copy((Array) this.buffer, 0, (Array) destinationArray, this.buffer.Length - this.head, this.tail);
      }
      this.buffer = destinationArray;
      this.head = 0;
      this.tail = this.count;
    }
  }

  private enum QueuedFrom : byte
  {
    None,
    Left,
    Right,
    Up,
    Down,
  }

  private interface IRay
  {
    int Forward(int cell);

    int Port(int cell);

    int Starboard(int cell);

    FloodFill.RetainedQueue ForwardQueue { get; }

    FloodFill.RetainedQueue PortQueue { get; }

    FloodFill.RetainedQueue StarboardQueue { get; }
  }

  private struct NorthRay : FloodFill.IRay
  {
    private FloodFill.RetainedQueue above;
    private FloodFill.RetainedQueue left;
    private FloodFill.RetainedQueue right;

    public readonly int Forward(int cell) => Grid.CellAbove(cell);

    public readonly int Port(int cell) => Grid.CellLeft(cell);

    public readonly int Starboard(int cell) => Grid.CellRight(cell);

    public readonly FloodFill.RetainedQueue ForwardQueue => this.above;

    public readonly FloodFill.RetainedQueue PortQueue => this.left;

    public readonly FloodFill.RetainedQueue StarboardQueue => this.right;

    public static FloodFill.NorthRay Default()
    {
      return new FloodFill.NorthRay()
      {
        above = FloodFill.tlAbove.Value,
        left = FloodFill.tlLeft.Value,
        right = FloodFill.tlRight.Value
      };
    }
  }

  private struct SouthRay : FloodFill.IRay
  {
    private FloodFill.RetainedQueue below;
    private FloodFill.RetainedQueue right;
    private FloodFill.RetainedQueue left;

    public readonly int Forward(int cell) => Grid.CellBelow(cell);

    public readonly int Port(int cell) => Grid.CellRight(cell);

    public readonly int Starboard(int cell) => Grid.CellLeft(cell);

    public readonly FloodFill.RetainedQueue ForwardQueue => this.below;

    public readonly FloodFill.RetainedQueue PortQueue => this.right;

    public readonly FloodFill.RetainedQueue StarboardQueue => this.left;

    public static FloodFill.SouthRay Default()
    {
      return new FloodFill.SouthRay()
      {
        below = FloodFill.tlBelow.Value,
        right = FloodFill.tlRight.Value,
        left = FloodFill.tlLeft.Value
      };
    }
  }

  private struct EastRay : FloodFill.IRay
  {
    private FloodFill.RetainedQueue right;
    private FloodFill.RetainedQueue above;
    private FloodFill.RetainedQueue below;

    public readonly int Forward(int cell) => Grid.CellRight(cell);

    public readonly int Port(int cell) => Grid.CellAbove(cell);

    public readonly int Starboard(int cell) => Grid.CellBelow(cell);

    public readonly FloodFill.RetainedQueue ForwardQueue => this.right;

    public readonly FloodFill.RetainedQueue PortQueue => this.above;

    public readonly FloodFill.RetainedQueue StarboardQueue => this.below;

    public static FloodFill.EastRay Default()
    {
      return new FloodFill.EastRay()
      {
        right = FloodFill.tlRight.Value,
        above = FloodFill.tlAbove.Value,
        below = FloodFill.tlBelow.Value
      };
    }
  }

  private struct WestRay : FloodFill.IRay
  {
    private FloodFill.RetainedQueue left;
    private FloodFill.RetainedQueue below;
    private FloodFill.RetainedQueue above;

    public readonly int Forward(int cell) => Grid.CellLeft(cell);

    public readonly int Port(int cell) => Grid.CellBelow(cell);

    public readonly int Starboard(int cell) => Grid.CellAbove(cell);

    public readonly FloodFill.RetainedQueue ForwardQueue => this.left;

    public readonly FloodFill.RetainedQueue PortQueue => this.below;

    public readonly FloodFill.RetainedQueue StarboardQueue => this.above;

    public static FloodFill.WestRay Default()
    {
      return new FloodFill.WestRay()
      {
        left = FloodFill.tlLeft.Value,
        below = FloodFill.tlBelow.Value,
        above = FloodFill.tlAbove.Value
      };
    }
  }

  private interface ILateralRay
  {
    int FromSourceCell(int sourceCellIndex);

    void Enqueue(ulong cell);
  }

  private readonly struct PortRay<Ray>(Ray ray) : FloodFill.ILateralRay where Ray : FloodFill.IRay
  {
    private readonly Ray ray = ray;

    public int FromSourceCell(int sourceCellIndex) => this.ray.Port(sourceCellIndex);

    public void Enqueue(ulong cell) => this.ray.PortQueue.Enqueue(cell);
  }

  private readonly struct StarboardRay<Ray>(Ray ray) : FloodFill.ILateralRay where Ray : FloodFill.IRay
  {
    private readonly Ray ray = ray;

    public int FromSourceCell(int sourceCellIndex) => this.ray.Starboard(sourceCellIndex);

    public void Enqueue(ulong cell) => this.ray.StarboardQueue.Enqueue(cell);
  }
}
