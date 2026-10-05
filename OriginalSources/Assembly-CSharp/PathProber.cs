// Decompiled with JetBrains decompiler
// Type: PathProber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine.Pool;

#nullable disable
public static class PathProber
{
  public const int InvalidHandle = -1;
  public const int InvalidIdx = -1;
  public const int InvalidCell = -1;
  public const int InvalidCost = -1;
  public const ushort InvalidSerialNo = 0;
  private static ObjectPool<PathFinder.PotentialScratchPad> ScratchPadPool = new ObjectPool<PathFinder.PotentialScratchPad>((Func<PathFinder.PotentialScratchPad>) (() => new PathFinder.PotentialScratchPad(Pathfinding.Instance.MaxLinksPerCell())), collectionCheck: false, defaultCapacity: 1, maxSize: 4);
  private static ObjectPool<PathFinder.PotentialList> PotentialListPool = new ObjectPool<PathFinder.PotentialList>((Func<PathFinder.PotentialList>) (() => new PathFinder.PotentialList()), actionOnRelease: (Action<PathFinder.PotentialList>) (list => list.Clear()), collectionCheck: false, defaultCapacity: 1, maxSize: 4);

  public static void Run(
    int root_cell,
    PathFinderAbilities abilities,
    NavGrid nav_grid,
    NavType starting_nav_type,
    PathGrid path_grid,
    ushort serialNo,
    PathFinder.PotentialScratchPad scratchPad,
    PathFinder.PotentialList potentials,
    PathFinder.PotentialPath.Flags flags,
    List<int> found_cells = null)
  {
    path_grid.BeginUpdate(serialNo, root_cell, found_cells);
    NavType nav_type = starting_nav_type;
    bool is_cell_in_range;
    PathFinder.Cell cell1 = path_grid.GetCell(root_cell, starting_nav_type, out is_cell_in_range);
    PathFinder.AddPotential(new PathFinder.PotentialPath(root_cell, nav_type, flags), Grid.InvalidCell, NavType.NumNavTypes, 0, (byte) 0, potentials, path_grid, ref cell1);
    while (potentials.Count > 0)
    {
      KeyValuePair<int, PathFinder.PotentialPath> keyValuePair = potentials.Next();
      PathFinder.Cell cell2 = path_grid.GetCell(keyValuePair.Value, out is_cell_in_range);
      if (cell2.cost == keyValuePair.Key)
        PathFinder.AddPotentials(scratchPad, keyValuePair.Value, cell2.cost, ref abilities, (PathFinderQuery) null, nav_grid.maxLinksPerCell, nav_grid.Links, potentials, path_grid, cell2.parent, cell2.parentNavType);
    }
    path_grid.EndUpdate();
  }

  public static void Run(Navigator navigator, List<int> found_cells = null)
  {
    PathFinder.PotentialScratchPad potentialScratchPad = PathProber.ScratchPadPool.Get();
    PathFinder.PotentialList potentialList = PathProber.PotentialListPool.Get();
    ushort serialNo = (ushort) ((uint) navigator.PathGrid.SerialNo + 1U);
    if (serialNo == (ushort) 0)
    {
      ++serialNo;
      navigator.PathGrid.ResetProberCells();
    }
    PathFinderAbilities currentAbilities = navigator.GetCurrentAbilities();
    PathProber.Run(navigator.cachedCell, currentAbilities, navigator.NavGrid, navigator.CurrentNavType, navigator.PathGrid, serialNo, potentialScratchPad, potentialList, navigator.flags, found_cells);
    PathProber.ScratchPadPool.Release(potentialScratchPad);
    PathProber.PotentialListPool.Release(potentialList);
  }
}
