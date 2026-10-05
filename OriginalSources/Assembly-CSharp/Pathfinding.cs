// Decompiled with JetBrains decompiler
// Type: Pathfinding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Pathfinding")]
public class Pathfinding : KMonoBehaviour
{
  private List<NavGrid> NavGrids = new List<NavGrid>();
  private int UpdateIdx;
  private bool navGridsHaveBeenFlushedOnLoad;
  public static Pathfinding Instance;

  public static void DestroyInstance()
  {
    Pathfinding.Instance = (Pathfinding) null;
    OffsetTableTracker.OnPathfindingInvalidated();
  }

  protected override void OnPrefabInit() => Pathfinding.Instance = this;

  public int MaxLinksPerCell()
  {
    int a = 0;
    foreach (NavGrid navGrid in this.NavGrids)
      a = Mathf.Max(a, navGrid.maxLinksPerCell);
    return a;
  }

  public void AddNavGrid(NavGrid nav_grid) => this.NavGrids.Add(nav_grid);

  public NavGrid GetNavGrid(string id)
  {
    foreach (NavGrid navGrid in this.NavGrids)
    {
      if (navGrid.id == id)
        return navGrid;
    }
    Debug.LogError((object) ("Could not find nav grid: " + id));
    return (NavGrid) null;
  }

  public List<NavGrid> GetNavGrids() => this.NavGrids;

  public void ResetNavGrids()
  {
    foreach (NavGrid navGrid in this.NavGrids)
      navGrid.InitializeGraph();
  }

  public void FlushNavGridsOnLoad()
  {
    if (this.navGridsHaveBeenFlushedOnLoad)
      return;
    this.navGridsHaveBeenFlushedOnLoad = true;
    this.UpdateNavGrids(true);
  }

  public void UpdateNavGrids(bool update_all = false)
  {
    update_all = true;
    if (update_all)
    {
      foreach (NavGrid navGrid in this.NavGrids)
        navGrid.UpdateGraph();
    }
    else
    {
      foreach (NavGrid navGrid in this.NavGrids)
      {
        if (navGrid.updateEveryFrame)
          navGrid.UpdateGraph();
      }
      this.NavGrids[this.UpdateIdx].UpdateGraph();
      this.UpdateIdx = (this.UpdateIdx + 1) % this.NavGrids.Count;
    }
  }

  public void RenderEveryTick()
  {
    foreach (NavGrid navGrid in this.NavGrids)
      navGrid.DebugUpdate();
  }

  public void AddDirtyNavGridCell(int cell)
  {
    foreach (NavGrid navGrid in this.NavGrids)
      navGrid.AddDirtyCell(cell);
  }

  public void RefreshNavCell(int cell)
  {
    ListPool<int, PathFinder>.PooledList dirty_nav_cells = ListPool<int, PathFinder>.Allocate();
    dirty_nav_cells.Add(cell);
    foreach (NavGrid navGrid in this.NavGrids)
      navGrid.UpdateGraph((List<int>) dirty_nav_cells);
    dirty_nav_cells.Recycle();
  }

  protected override void OnCleanUp()
  {
    this.NavGrids.Clear();
    OffsetTableTracker.OnPathfindingInvalidated();
  }
}
