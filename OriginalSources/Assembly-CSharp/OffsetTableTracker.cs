// Decompiled with JetBrains decompiler
// Type: OffsetTableTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class OffsetTableTracker : OffsetTracker
{
  private readonly CellOffset[][] table;
  public HandleVector<int>.Handle solidPartitionerEntry;
  public HandleVector<int>.Handle validNavCellChangedPartitionerEntry;
  private static NavGrid navGridImpl;
  private KMonoBehaviour cmp;
  private Action<object> OnCellChangedClosure;
  private int[] DEBUG_rowValidIdx;

  private static NavGrid navGrid
  {
    get
    {
      if (OffsetTableTracker.navGridImpl == null)
        OffsetTableTracker.navGridImpl = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
      return OffsetTableTracker.navGridImpl;
    }
  }

  public OffsetTableTracker(CellOffset[][] table, KMonoBehaviour cmp)
  {
    this.table = table;
    this.cmp = cmp;
    this.OnCellChangedClosure = new Action<object>(this.OnCellChanged);
  }

  protected override void UpdateCell(int previous_cell, int current_cell)
  {
    if (previous_cell == current_cell)
      return;
    base.UpdateCell(previous_cell, current_cell);
    Extents extents = new Extents(current_cell, this.table);
    extents.height += 2;
    --extents.y;
    if (!this.solidPartitionerEntry.IsValid())
    {
      this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("OffsetTableTracker.UpdateCell", (object) this.cmp.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, this.OnCellChangedClosure);
      this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("OffsetTableTracker.UpdateCell", (object) this.cmp.gameObject, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, this.OnCellChangedClosure);
    }
    else
    {
      GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, extents);
      GameScenePartitioner.Instance.UpdatePosition(this.validNavCellChangedPartitionerEntry, extents);
    }
    this.offsets = (CellOffset[]) null;
  }

  private static bool IsValidRow(int current_cell, CellOffset[] row, int rowIdx, int[] debugIdxs)
  {
    for (int index = 1; index < row.Length; ++index)
    {
      int num = Grid.OffsetCell(current_cell, row[index]);
      if (!Grid.IsValidCell(num) || Grid.Solid[num])
        return false;
    }
    return true;
  }

  private void UpdateOffsets(int cell, CellOffset[][] table)
  {
    HashSetPool<CellOffset, OffsetTableTracker>.PooledHashSet pooledHashSet = HashSetPool<CellOffset, OffsetTableTracker>.Allocate();
    if (Grid.IsValidCell(cell))
    {
      for (int rowIdx = 0; rowIdx < table.Length; ++rowIdx)
      {
        CellOffset[] row = table[rowIdx];
        if (!pooledHashSet.Contains(row[0]))
        {
          int cell1 = Grid.OffsetCell(cell, row[0]);
          for (int index = 0; index < OffsetTableTracker.navGrid.ValidNavTypes.Length; ++index)
          {
            NavType validNavType = OffsetTableTracker.navGrid.ValidNavTypes[index];
            if (validNavType != NavType.Tube && OffsetTableTracker.navGrid.NavTable.IsValid(cell1, validNavType) && OffsetTableTracker.IsValidRow(cell, row, rowIdx, this.DEBUG_rowValidIdx))
            {
              pooledHashSet.Add(row[0]);
              break;
            }
          }
        }
      }
    }
    if (this.offsets == null || this.offsets.Length != pooledHashSet.Count)
      this.offsets = new CellOffset[pooledHashSet.Count];
    pooledHashSet.CopyTo(this.offsets);
    pooledHashSet.Recycle();
  }

  protected override void UpdateOffsets(int current_cell)
  {
    base.UpdateOffsets(current_cell);
    this.UpdateOffsets(current_cell, this.table);
  }

  private void OnCellChanged(object data) => this.offsets = (CellOffset[]) null;

  public override void Clear()
  {
    GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
  }

  public static void OnPathfindingInvalidated() => OffsetTableTracker.navGridImpl = (NavGrid) null;
}
