// Decompiled with JetBrains decompiler
// Type: MinionGroupProber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Threading;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/MinionGroupProber")]
public class MinionGroupProber : KMonoBehaviour
{
  private static MinionGroupProber Instance;
  private int[] cells;

  public static void DestroyInstance() => MinionGroupProber.Instance = (MinionGroupProber) null;

  public static MinionGroupProber Get() => MinionGroupProber.Instance;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    MinionGroupProber.Instance = this;
    this.cells = new int[Grid.CellCount];
  }

  public bool IsReachable(int cell) => Grid.IsValidCell(cell) && this.cells[cell] > 0;

  public bool IsReachable(int cell, CellOffset[] offsets)
  {
    if (!Grid.IsValidCell(cell))
      return false;
    foreach (CellOffset offset in offsets)
    {
      if (this.IsReachable(Grid.OffsetCell(cell, offset)))
        return true;
    }
    return false;
  }

  public bool IsAllReachable(int cell, CellOffset[] offsets)
  {
    if (this.IsReachable(cell))
      return true;
    foreach (CellOffset offset in offsets)
    {
      if (this.IsReachable(Grid.OffsetCell(cell, offset)))
        return true;
    }
    return false;
  }

  public bool IsReachable(Workable workable)
  {
    return this.IsReachable(Grid.PosToCell((KMonoBehaviour) workable), workable.GetOffsets());
  }

  public void Occupy(List<int> cells)
  {
    foreach (int cell in cells)
      Interlocked.Increment(ref this.cells[cell]);
  }

  public void OccupyST(List<int> cells)
  {
    foreach (int cell in cells)
      ++this.cells[cell];
  }

  public void Occupy(int cell) => Interlocked.Increment(ref this.cells[cell]);

  public void Vacate(List<int> cells)
  {
    foreach (int cell in cells)
      Interlocked.Decrement(ref this.cells[cell]);
  }

  public void VacateST(List<int> cells)
  {
    foreach (int cell in cells)
      --this.cells[cell];
  }

  public void Vacate(int cell) => Interlocked.Decrement(ref this.cells[cell]);
}
