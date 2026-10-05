// Decompiled with JetBrains decompiler
// Type: PlantableCellQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PlantableCellQuery : PathFinderQuery
{
  public List<int> result_cells = new List<int>();
  private PlantableSeed seed;
  private int max_results;
  private int plantDetectionRadius = 6;
  private int maxPlantsInRadius = 2;

  public PlantableCellQuery Reset(PlantableSeed seed, int max_results)
  {
    this.seed = seed;
    this.max_results = max_results;
    this.result_cells.Clear();
    return this;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    if (!this.result_cells.Contains(cell) && this.CheckValidPlotCell(this.seed, cell))
      this.result_cells.Add(cell);
    return this.result_cells.Count >= this.max_results;
  }

  private bool CheckValidPlotCell(PlantableSeed seed, int plant_cell)
  {
    if (!Grid.IsValidCell(plant_cell))
      return false;
    int num = seed.Direction != SingleEntityReceptacle.ReceptacleDirection.Bottom ? Grid.CellBelow(plant_cell) : Grid.CellAbove(plant_cell);
    if (!Grid.IsValidCell(num) || !Grid.Solid[num] || (bool) (Object) Grid.Objects[plant_cell, 5] || (bool) (Object) Grid.Objects[plant_cell, 1])
      return false;
    GameObject gameObject = Grid.Objects[num, 1];
    if ((bool) (Object) gameObject)
    {
      PlantablePlot component1 = gameObject.GetComponent<PlantablePlot>();
      if ((Object) component1 == (Object) null || component1.Direction != seed.Direction || (Object) component1.Occupant != (Object) null)
        return false;
      SingleEntityReceptacle component2 = gameObject.GetComponent<SingleEntityReceptacle>();
      if ((Object) component2 != (Object) null && !component2.IsValidEntity(seed.gameObject))
        return false;
    }
    else if (!seed.TestSuitableGround(plant_cell) || PlantableCellQuery.CountNearbyPlants(num, this.plantDetectionRadius) > this.maxPlantsInRadius)
      return false;
    return true;
  }

  private static int CountNearbyPlants(int cell, int radius)
  {
    int x = 0;
    int y = 0;
    Grid.PosToXY(Grid.CellToPos(cell), out x, out y);
    int num1 = radius * 2;
    int x_bottomLeft = x - radius;
    int y_bottomLeft = y - radius;
    ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
    GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, num1, num1, GameScenePartitioner.Instance.plants, (List<ScenePartitionerEntry>) gathered_entries);
    int num2 = 0;
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
    {
      if (!(bool) (Object) ((Component) partitionerEntry.obj).GetComponent<TreeBud>())
        ++num2;
    }
    gathered_entries.Recycle();
    return num2;
  }
}
