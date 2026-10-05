// Decompiled with JetBrains decompiler
// Type: FishOvercrowingManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/FishOvercrowingManager")]
public class FishOvercrowingManager : KMonoBehaviour, ISim1000ms
{
  public static FishOvercrowingManager Instance;
  private readonly List<KPrefabID> allAquaticEntities = new List<KPrefabID>();
  private readonly List<FishOvercrowingManager.Pond> ponds = new List<FishOvercrowingManager.Pond>();
  private FishOvercrowingManager.Cell[] grid;
  private int nextGeneration = 2;
  private static readonly Func<int, FloodFill.BoundaryCheckResult> isLiquidCell = (Func<int, FloodFill.BoundaryCheckResult>) (cell => !Grid.IsNavigatableLiquidUnsafe(cell) ? FloodFill.BoundaryCheckResult.Halt : FloodFill.BoundaryCheckResult.Continue);

  public static void DestroyInstance()
  {
    FishOvercrowingManager.Instance = (FishOvercrowingManager) null;
  }

  protected override void OnPrefabInit()
  {
    FishOvercrowingManager.Instance = this;
    this.grid = new FishOvercrowingManager.Cell[Grid.CellCount];
  }

  public void Add(KPrefabID aquaticEntity) => this.allAquaticEntities.Add(aquaticEntity);

  public void Remove(KPrefabID aquaticEntity)
  {
    if (aquaticEntity.IsNullOrDestroyed())
      return;
    for (int index = this.allAquaticEntities.Count - 1; index >= 0; --index)
    {
      KPrefabID allAquaticEntity = this.allAquaticEntities[index];
      if (!allAquaticEntity.IsNullOrDestroyed() && allAquaticEntity.InstanceID == aquaticEntity.InstanceID)
      {
        this.allAquaticEntities.RemoveAt(index);
        break;
      }
    }
  }

  public void Sim1000ms(float dt)
  {
    int generation = this.nextGeneration++;
    if (generation == 0)
    {
      Array.Fill<FishOvercrowingManager.Cell>(this.grid, new FishOvercrowingManager.Cell()
      {
        generation = 0,
        pondIndex = -1
      });
      generation = this.nextGeneration++;
    }
    for (int index = 0; index != this.ponds.Count; ++index)
    {
      FishOvercrowingManager.Pond pond = this.ponds[index];
      pond.fishes.Clear();
      pond.eggs.Clear();
      pond.cellCount = 0;
      pond.occupancy.dirty = true;
    }
    int index1 = this.ponds.Count == 0 ? -1 : 0;
    foreach (KPrefabID allAquaticEntity in this.allAquaticEntities)
    {
      if (!allAquaticEntity.IsNullOrDestroyed())
      {
        int cell1 = Grid.PosToCell((KMonoBehaviour) allAquaticEntity);
        if (Grid.IsValidCell(cell1))
        {
          FishOvercrowingManager.Cell cell2 = this.grid[cell1];
          int num1 = cell2.generation != generation ? 1 : (cell2.pondIndex == -1 ? 1 : 0);
          int num2;
          if (num1 == 0)
            num2 = cell2.pondIndex;
          else if (index1 != -1 && index1 < this.ponds.Count)
          {
            num2 = index1;
            ++index1;
            if (index1 == this.ponds.Count)
              index1 = -1;
          }
          else
          {
            this.ponds.Add(new FishOvercrowingManager.Pond()
            {
              fishes = new List<KPrefabID>(),
              eggs = new List<KPrefabID>()
            });
            num2 = this.ponds.Count - 1;
          }
          FishOvercrowingManager.Pond pond = this.ponds[num2];
          if (allAquaticEntity.HasTag(GameTags.Egg))
            pond.eggs.Add(allAquaticEntity);
          else
            pond.fishes.Add(allAquaticEntity);
          if (num1 != 0)
            FloodFill.DepthTraverse<FloodFill.PredicateCondition, FishOvercrowingManager.VisitTracker, FloodFill.NoMaxDepth, FishOvercrowingManager.Visitor>(cell1, new FloodFill.PredicateCondition(FishOvercrowingManager.isLiquidCell), new FishOvercrowingManager.VisitTracker(this.grid, generation), new FloodFill.NoMaxDepth(), new FishOvercrowingManager.Visitor(this.grid, this.ponds, num2));
        }
      }
    }
    if (index1 != -1)
    {
      int count = this.ponds.Count - index1;
      if (count > 0)
        this.ponds.RemoveRange(index1, count);
    }
    this.allAquaticEntities.RemoveAll(new Predicate<KPrefabID>(Util.IsNullOrDestroyed));
  }

  public FishOvercrowingManager.Pond GetPond(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return (FishOvercrowingManager.Pond) null;
    FishOvercrowingManager.Cell cell1 = this.grid[cell];
    return cell1.generation != this.nextGeneration - 1 || cell1.pondIndex == -1 ? (FishOvercrowingManager.Pond) null : this.ponds[cell1.pondIndex];
  }

  public int GetFishInPondCount(int cell, HashSet<Tag> accepted_tags)
  {
    int fishInPondCount = 0;
    FishOvercrowingManager.Pond pond = this.GetPond(cell);
    if (pond == null)
      return 0;
    foreach (KPrefabID fish in pond.fishes)
    {
      if (!fish.HasTag(GameTags.Creatures.Bagged) && !fish.HasTag(GameTags.Trapped) && accepted_tags.Contains(fish.PrefabTag))
        ++fishInPondCount;
    }
    return fishInPondCount;
  }

  private struct Cell
  {
    public int generation;
    public int pondIndex;
  }

  public class Pond
  {
    public List<KPrefabID> fishes;
    public List<KPrefabID> eggs;
    public int cellCount;
    public OvercrowdingMonitor.Occupancy occupancy = new OvercrowdingMonitor.Occupancy();

    public int FishCount => this.fishes.Count;

    public int EggCount => this.eggs.Count;
  }

  private readonly struct VisitTracker(FishOvercrowingManager.Cell[] grid, int generation) : 
    FloodFill.IVisitTracker
  {
    private readonly FishOvercrowingManager.Cell[] grid = grid;
    private readonly int generation = generation;

    public bool Add(int cellIndex)
    {
      if (this.Contains(cellIndex))
        return false;
      this.grid[cellIndex].generation = this.generation;
      return true;
    }

    public bool Contains(int cellIndex) => this.grid[cellIndex].generation == this.generation;
  }

  private readonly struct Visitor(
    FishOvercrowingManager.Cell[] grid,
    List<FishOvercrowingManager.Pond> ponds,
    int pondIndex) : FloodFill.IVisitor
  {
    private readonly FishOvercrowingManager.Cell[] grid = grid;
    private readonly List<FishOvercrowingManager.Pond> ponds = ponds;
    private readonly int pondIndex = pondIndex;

    public bool EarlyOut => false;

    public void VisitCell(int cell)
    {
      ++this.ponds[this.pondIndex].cellCount;
      this.grid[cell].pondIndex = this.pondIndex;
    }

    public void VisitBoundary(int cell) => this.grid[cell].pondIndex = -1;
  }
}
