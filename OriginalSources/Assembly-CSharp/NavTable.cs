// Decompiled with JetBrains decompiler
// Type: NavTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class NavTable
{
  public Action<int, NavType> OnValidCellChanged;
  private short[] NavTypeMasks;
  public string NavGridId;
  private short[] ValidCells;

  public NavTable(int cell_count, string nav_grid_id = null)
  {
    this.ValidCells = new short[cell_count];
    this.NavTypeMasks = new short[11];
    this.NavGridId = nav_grid_id;
    for (short index = 0; index < (short) 11; ++index)
      this.NavTypeMasks[(int) index] = (short) (1 << (int) index);
  }

  public bool IsValid(int cell, NavType nav_type = NavType.Floor)
  {
    return Grid.IsValidCell(cell) && ((uint) this.NavTypeMasks[(int) nav_type] & (uint) this.ValidCells[cell]) > 0U;
  }

  public void SetValid(int cell, NavType nav_type, bool is_valid)
  {
    short navTypeMask = this.NavTypeMasks[(int) nav_type];
    short validCell = this.ValidCells[cell];
    if (((uint) validCell & (uint) navTypeMask) > 0U == is_valid)
      return;
    this.ValidCells[cell] = !is_valid ? (short) ((int) ~navTypeMask & (int) validCell) : (short) ((int) navTypeMask | (int) validCell);
    if (this.OnValidCellChanged == null)
      return;
    this.OnValidCellChanged(cell, nav_type);
  }
}
