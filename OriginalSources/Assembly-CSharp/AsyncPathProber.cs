// Decompiled with JetBrains decompiler
// Type: AsyncPathProber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using UnityEngine.Pool;

#nullable disable
public static class AsyncPathProber
{
  public const int kMaxProbersPerFrame = 4;

  public static AsyncPathProber.Manager Instance { get; private set; }

  public static void CreateInstance(int count)
  {
    DebugUtil.Assert(AsyncPathProber.Instance == null);
    AsyncPathProber.Instance = new AsyncPathProber.Manager();
    AsyncPathProber.Instance.Start(count);
  }

  public static void DestroyInstance()
  {
    AsyncPathProber.Instance.Shutdown();
    AsyncPathProber.Instance = (AsyncPathProber.Manager) null;
  }

  public struct WorkResult
  {
    public Navigator navigator;
    public PathGrid pathGrid;
    public List<int> reachableCells;
    public List<int> newlyReachableCells;
    public List<int> noLongerReachableCells;
    public PathFinderAbilities abilitiesInstance;
  }

  public struct WorkOrder
  {
    public Navigator navigator;
    public NavGrid navGrid;
    public ulong gridClassification;
    public PathFinderAbilities abilities;
    public int originCell;
    public NavType startingNavType;
    public PathFinder.PotentialPath.Flags startingFlags;
    public ushort serialNo;
    public bool computeReachables;
    private static Comparison<int> WorkOrderIntSorter = (Comparison<int>) ((lhs, rhs) => lhs.CompareTo(rhs));

    public void Execute(
      PathFinder.PotentialList potentials,
      PathFinder.PotentialScratchPad scratch,
      ref AsyncPathProber.WorkResult result)
    {
      if ((int) result.pathGrid.SerialNo >= (int) this.serialNo)
        result.pathGrid.ResetProberCells();
      PathProber.Run(this.originCell, this.abilities, this.navGrid, this.startingNavType, result.pathGrid, this.serialNo, scratch, potentials, this.startingFlags, result.reachableCells);
      result.abilitiesInstance = this.abilities;
      this.abilities = (PathFinderAbilities) null;
      if (!this.computeReachables)
        return;
      result.reachableCells.Sort(AsyncPathProber.WorkOrder.WorkOrderIntSorter);
      int index1 = 0;
      int index2 = 0;
      while (index1 < this.navigator.occupiedCells.Count && index2 < result.reachableCells.Count)
      {
        if (this.navigator.occupiedCells[index1] < result.reachableCells[index2])
        {
          result.noLongerReachableCells.Add(this.navigator.occupiedCells[index1]);
          ++index1;
        }
        else if (result.reachableCells[index2] < this.navigator.occupiedCells[index1])
        {
          result.newlyReachableCells.Add(result.reachableCells[index2]);
          ++index2;
        }
        else
        {
          ++index1;
          ++index2;
        }
      }
      for (; index1 < this.navigator.occupiedCells.Count; ++index1)
        result.noLongerReachableCells.Add(this.navigator.occupiedCells[index1]);
      for (; index2 < result.reachableCells.Count; ++index2)
        result.newlyReachableCells.Add(result.reachableCells[index2]);
    }
  }

  private static class AsyncPathProbeWorker
  {
    public static void main(object _)
    {
      PathFinder.PotentialList potentials = new PathFinder.PotentialList();
      PathFinder.PotentialScratchPad scratch = new PathFinder.PotentialScratchPad(Pathfinding.Instance.MaxLinksPerCell());
      try
      {
        while (!AsyncPathProber.Instance.Halting())
        {
          AsyncPathProber.WorkOrder order;
          AsyncPathProber.WorkResult result;
          if (AsyncPathProber.Instance.NextTask(out order, out result))
          {
            order.Execute(potentials, scratch, ref result);
            AsyncPathProber.Instance.WorkCompleted(result);
          }
          else
            Thread.Sleep(1);
        }
      }
      catch (Exception ex)
      {
        AsyncPathProber.Instance.SetException(ExceptionDispatchInfo.Capture(ex));
      }
    }
  }

  public class Manager
  {
    private const int kNovelProberPenalty = 10000;
    private const int kFailedNavigationPenalty = 10;
    private const int kNavigatorInFlightValue = -1;
    private Dictionary<Navigator, int> navigators = new Dictionary<Navigator, int>();
    private List<Navigator> navigatorOrdering = new List<Navigator>();
    private Comparison<Navigator> navigatorOrderer;
    private ushort activeSerialNo;
    private Thread[] agents;
    private bool halting;
    private ExceptionDispatchInfo agentException;
    private List<AsyncPathProber.WorkOrder> workQueue = new List<AsyncPathProber.WorkOrder>();
    private List<AsyncPathProber.WorkResult> finishedWork = new List<AsyncPathProber.WorkResult>();
    private ConcurrentDictionary<ulong, ObjectPool<PathGrid>> gridPool = new ConcurrentDictionary<ulong, ObjectPool<PathGrid>>();
    private ObjectPool<List<int>> indexListPool = new ObjectPool<List<int>>((Func<List<int>>) (() => new List<int>(Grid.CellCount / 8)), actionOnRelease: (Action<List<int>>) (list => list.Clear()), collectionCheck: false, defaultCapacity: 12);
    private static int NavFailures;

    public bool Halting() => this.halting;

    public Manager()
    {
      this.navigatorOrderer = (Comparison<Navigator>) ((lhs, rhs) => CollectionExtensions.GetValueOrDefault<Navigator, int>((IReadOnlyDictionary<Navigator, int>) this.navigators, rhs, 0).CompareTo(CollectionExtensions.GetValueOrDefault<Navigator, int>((IReadOnlyDictionary<Navigator, int>) this.navigators, lhs, 0)));
    }

    public void SetException(ExceptionDispatchInfo ex)
    {
      lock (this)
        this.agentException = ex;
    }

    public void Register(Navigator nav)
    {
      lock (this)
      {
        if (this.navigators.ContainsKey(nav))
          Debug.LogWarning((object) ("Double registration of navigator to AsyncManager: " + nav.ToString()));
        if (!this.gridPool.ContainsKey(nav.PathGrid.AllocatedClassification))
        {
          lock (this)
          {
            int width = nav.PathGrid.widthInCells;
            int height = nav.PathGrid.heightInCells;
            bool applyOffset = nav.PathGrid.applyOffset;
            NavType[] navTypes = new NavType[nav.PathGrid.ValidNavTypes.Length];
            nav.PathGrid.ValidNavTypes.CopyTo((Array) navTypes, 0);
            this.gridPool[nav.PathGrid.AllocatedClassification] = new ObjectPool<PathGrid>((Func<PathGrid>) (() => new PathGrid(width, height, applyOffset, navTypes)), collectionCheck: false, defaultCapacity: 4 + this.agents.Length, maxSize: 4 + this.agents.Length);
          }
        }
        this.navigators[nav] = 10000;
      }
    }

    public void Unregister(Navigator nav)
    {
      lock (this)
      {
        if (this.navigators.Remove(nav))
          return;
        Debug.LogWarning((object) ("Unregister of unknown navigator from AsyncManager: " + nav.ToString()));
      }
    }

    public void WorkCompleted(AsyncPathProber.WorkResult result)
    {
      lock (this)
        this.finishedWork.Add(result);
    }

    public bool NextTask(out AsyncPathProber.WorkOrder order, out AsyncPathProber.WorkResult result)
    {
      lock (this)
      {
        if (this.workQueue.Count > 0)
        {
          order = this.workQueue[0];
          this.workQueue.RemoveAt(0);
          if (this.navigators.ContainsKey(order.navigator))
          {
            result = new AsyncPathProber.WorkResult()
            {
              navigator = order.navigator,
              pathGrid = this.gridPool[order.gridClassification].Get(),
              newlyReachableCells = this.indexListPool.Get(),
              noLongerReachableCells = this.indexListPool.Get(),
              reachableCells = this.indexListPool.Get()
            };
            this.navigators[order.navigator] = -1;
            return true;
          }
        }
      }
      order = new AsyncPathProber.WorkOrder();
      result = new AsyncPathProber.WorkResult();
      return false;
    }

    private AsyncPathProber.WorkOrder makeWorkOrder(Navigator nav)
    {
      PathFinderAbilities currentAbilities = nav.GetCurrentAbilities();
      return new AsyncPathProber.WorkOrder()
      {
        navigator = nav,
        navGrid = nav.NavGrid,
        gridClassification = nav.PathGrid.AllocatedClassification,
        abilities = currentAbilities.Clone(),
        originCell = nav.cachedCell,
        startingNavType = nav.CurrentNavType,
        startingFlags = nav.flags,
        serialNo = this.activeSerialNo,
        computeReachables = nav.reportOccupation
      };
    }

    public void TickFrame()
    {
      lock (this)
      {
        if (this.agentException != null)
        {
          this.agentException.Throw();
          this.agentException = (ExceptionDispatchInfo) null;
        }
        ++this.activeSerialNo;
        if (this.activeSerialNo == (ushort) 0)
          ++this.activeSerialNo;
        for (int index = 0; index < this.finishedWork.Count; ++index)
        {
          AsyncPathProber.WorkResult result = this.finishedWork[index];
          PathGrid element = result.pathGrid;
          if (this.navigators.ContainsKey(result.navigator))
          {
            element = result.navigator.TakeResult(ref result);
            this.navigators[result.navigator] = 0;
          }
          if (element != null)
            this.gridPool[element.AllocatedClassification].Release(element);
          this.indexListPool.Release(result.reachableCells);
          this.indexListPool.Release(result.newlyReachableCells);
          this.indexListPool.Release(result.noLongerReachableCells);
          result.abilitiesInstance.RecycleClone();
        }
        this.finishedWork.Clear();
        foreach (KeyValuePair<Navigator, int> navigator in this.navigators)
          this.navigatorOrdering.Add(navigator.Key);
        for (int index = this.navigatorOrdering.Count - 1; index >= 0; --index)
        {
          Navigator key = this.navigatorOrdering[index];
          int navigator = this.navigators[key];
          if (navigator == -1)
            this.navigatorOrdering.RemoveAtSwap<Navigator>(index);
          else
            this.navigators[key] = navigator + 1;
        }
        this.navigatorOrdering.Sort(this.navigatorOrderer);
        for (int index = 0; index < this.workQueue.Count; ++index)
          this.workQueue[index].abilities.RecycleClone();
        this.workQueue.Clear();
        for (int index = 0; index < this.navigatorOrdering.Count && this.workQueue.Count < 4; ++index)
        {
          AsyncPathProber.WorkOrder workOrder = this.makeWorkOrder(this.navigatorOrdering[index]);
          if (Grid.IsValidCell(workOrder.originCell))
            this.workQueue.Add(workOrder);
          else
            workOrder.abilities.RecycleClone();
        }
        this.navigatorOrdering.Clear();
      }
    }

    public void ApplyNavigationFailedPenalty(Navigator nav)
    {
      ++AsyncPathProber.Manager.NavFailures;
      lock (this)
      {
        int num;
        if (!this.navigators.TryGetValue(nav, out num) || num < 0)
          return;
        this.navigators[nav] = num + 10;
      }
    }

    public void Start(int agentCount)
    {
      this.agents = new Thread[agentCount];
      this.halting = false;
      for (int index = 0; index < agentCount; ++index)
      {
        this.agents[index] = new Thread(new ParameterizedThreadStart(AsyncPathProber.AsyncPathProbeWorker.main));
        this.agents[index].Start();
      }
    }

    public void Shutdown()
    {
      lock (this)
        this.halting = true;
      foreach (Thread agent in this.agents)
        agent.Join();
    }
  }
}
