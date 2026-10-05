// Decompiled with JetBrains decompiler
// Type: PathGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class PathGrid
{
  private PathFinder.Cell[] Cells;
  private PathGrid.ProberCell[] ProberCells;
  private List<int> freshlyOccupiedCells;
  public NavType[] ValidNavTypes;
  public int[] NavTypeTable;
  public int widthInCells;
  public int heightInCells;
  public bool applyOffset;
  private int rootX;
  private int rootY;
  private ushort serialNo;
  public static readonly PathFinder.Cell InvalidCell = new PathFinder.Cell()
  {
    cost = -1,
    parent = -1
  };
  private static readonly PathGrid.ProberCell InvalidProberCell = new PathGrid.ProberCell()
  {
    cost = -1,
    queryId = 0,
    navType = NavType.Floor
  };

  public ulong AllocatedClassification
  {
    get
    {
      DebugUtil.Assert(this.widthInCells < (int) ushort.MaxValue);
      DebugUtil.Assert(this.heightInCells < (int) ushort.MaxValue);
      DebugUtil.Assert(this.ValidNavTypes.Length < 256 /*0x0100*/);
      return (ulong) (((long) this.widthInCells << 16 /*0x10*/) + (long) this.heightInCells << 8) + (ulong) this.ValidNavTypes.Length;
    }
  }

  public ushort SerialNo => this.serialNo;

  public PathGrid(PathGrid other)
    : this(other.widthInCells, other.heightInCells, other.applyOffset, other.ValidNavTypes)
  {
  }

  public PathGrid(
    int width_in_cells,
    int height_in_cells,
    bool apply_offset,
    NavType[] valid_nav_types)
  {
    this.applyOffset = apply_offset;
    this.widthInCells = width_in_cells;
    this.heightInCells = height_in_cells;
    this.ValidNavTypes = valid_nav_types;
    int num = 0;
    this.NavTypeTable = new int[11];
    for (int index1 = 0; index1 < this.NavTypeTable.Length; ++index1)
    {
      this.NavTypeTable[index1] = -1;
      for (int index2 = 0; index2 < this.ValidNavTypes.Length; ++index2)
      {
        if (this.ValidNavTypes[index2] == (NavType) index1)
        {
          this.NavTypeTable[index1] = num++;
          break;
        }
      }
    }
    DebugUtil.DevAssert(true, "Cell packs nav type into 4 bits!");
    this.Cells = new PathFinder.Cell[width_in_cells * height_in_cells * this.ValidNavTypes.Length];
    this.ProberCells = new PathGrid.ProberCell[width_in_cells * height_in_cells];
  }

  public void CloneNavTypes(PathGrid other)
  {
    DebugUtil.Assert(other.ValidNavTypes.Length == this.ValidNavTypes.Length);
    other.ValidNavTypes.CopyTo((Array) this.ValidNavTypes, 0);
    int num = 0;
    for (int index1 = 0; index1 < this.NavTypeTable.Length; ++index1)
    {
      this.NavTypeTable[index1] = -1;
      for (int index2 = 0; index2 < this.ValidNavTypes.Length; ++index2)
      {
        if (this.ValidNavTypes[index2] == (NavType) index1)
        {
          this.NavTypeTable[index1] = num++;
          break;
        }
      }
    }
  }

  public void ResetProberCells()
  {
    for (int index = 0; index < this.ProberCells.Length; ++index)
      this.ProberCells[index] = new PathGrid.ProberCell();
  }

  public void OnCleanUp()
  {
  }

  public void BeginUpdate(ushort new_serial_no, int root_cell, List<int> found_cells_list = null)
  {
    this.freshlyOccupiedCells = found_cells_list;
    if (this.applyOffset)
    {
      Grid.CellToXY(root_cell, out this.rootX, out this.rootY);
      this.rootX -= this.widthInCells / 2;
      this.rootY -= this.heightInCells / 2;
    }
    this.serialNo = new_serial_no;
  }

  public void EndUpdate() => this.freshlyOccupiedCells = (List<int>) null;

  private bool IsValidSerialNo(ushort serialNo)
  {
    return (int) serialNo == (int) this.serialNo && serialNo > (ushort) 0;
  }

  public PathFinder.Cell GetCell(PathFinder.PotentialPath potential_path, out bool is_cell_in_range)
  {
    return this.GetCell(potential_path.cell, potential_path.navType, out is_cell_in_range);
  }

  public PathFinder.Cell GetCell(int cell, NavType nav_type, out bool is_cell_in_range)
  {
    int num = this.OffsetCell(cell);
    is_cell_in_range = -1 != num;
    if (!is_cell_in_range || nav_type >= (NavType) this.NavTypeTable.Length || num * this.ValidNavTypes.Length + this.NavTypeTable[(int) nav_type] >= this.Cells.Length)
      return PathGrid.InvalidCell;
    PathFinder.Cell cell1 = this.Cells[num * this.ValidNavTypes.Length + this.NavTypeTable[(int) nav_type]];
    return !this.IsValidSerialNo(cell1.queryId) ? PathGrid.InvalidCell : cell1;
  }

  private PathGrid.ProberCell GetProberCell(int cell)
  {
    int index = this.OffsetCell(cell);
    return index == -1 ? PathGrid.InvalidProberCell : this.ProberCells[index];
  }

  public void SetCell(PathFinder.PotentialPath potential_path, ref PathFinder.Cell cell_data)
  {
    int index = this.OffsetCell(potential_path.cell);
    if (-1 == index)
      return;
    cell_data.queryId = this.serialNo;
    int num = this.NavTypeTable[(int) potential_path.navType];
    this.Cells[index * this.ValidNavTypes.Length + num] = cell_data;
    if (potential_path.navType == NavType.Tube)
      return;
    PathGrid.ProberCell proberCell = this.ProberCells[index];
    if ((int) cell_data.queryId != (int) proberCell.queryId && this.freshlyOccupiedCells != null)
      this.freshlyOccupiedCells.Add(potential_path.cell);
    if ((int) cell_data.queryId == (int) proberCell.queryId && cell_data.cost >= proberCell.cost)
      return;
    proberCell.queryId = cell_data.queryId;
    proberCell.cost = cell_data.cost;
    proberCell.navType = potential_path.navType;
    this.ProberCells[index] = proberCell;
  }

  public int GetCostIgnoreProberOffset(int cell, CellOffset[] offsets)
  {
    int ignoreProberOffset = -1;
    foreach (CellOffset offset in offsets)
    {
      int cell1 = Grid.OffsetCell(cell, offset);
      if (Grid.IsValidCell(cell1))
      {
        PathGrid.ProberCell proberCell = this.ProberCells[cell1];
        if (this.IsValidSerialNo(proberCell.queryId) && (ignoreProberOffset == -1 || proberCell.cost < ignoreProberOffset))
          ignoreProberOffset = proberCell.cost;
      }
    }
    return ignoreProberOffset;
  }

  public int GetCost(int cell)
  {
    int index = this.OffsetCell(cell);
    if (-1 == index)
      return -1;
    PathGrid.ProberCell proberCell = this.ProberCells[index];
    return !this.IsValidSerialNo(proberCell.queryId) ? -1 : proberCell.cost;
  }

  private int OffsetCell(int cell)
  {
    if (!this.applyOffset)
      return cell;
    int x;
    int y;
    Grid.CellToXY(cell, out x, out y);
    if (x < this.rootX || x >= this.rootX + this.widthInCells || y < this.rootY || y >= this.rootY + this.heightInCells)
      return -1;
    int num = x - this.rootX;
    return (y - this.rootY) * this.widthInCells + num;
  }

  public bool BuildPath(
    int source_cell,
    int target_cell,
    NavType current_nav_type,
    ref PathFinder.Path path)
  {
    if (path.nodes != null)
      path.nodes.Clear();
    path.cost = -1;
    if (target_cell == PathFinder.InvalidCell || this.GetCost(target_cell) == -1 || this.GetCost(source_cell) == -1)
      return false;
    bool is_cell_in_range = false;
    PathGrid.ProberCell proberCell = this.GetProberCell(target_cell);
    PathFinder.Cell cell = this.GetCell(target_cell, proberCell.navType, out is_cell_in_range);
    path.Clear();
    path.cost = cell.cost;
    int cost = path.cost;
    while (target_cell != PathFinder.InvalidCell)
    {
      path.AddNode(new PathFinder.Path.Node()
      {
        cell = target_cell,
        navType = cell.navType,
        transitionId = cell.transitionId
      });
      if (target_cell == source_cell && cell.navType == current_nav_type)
      {
        path.nodes.Reverse();
        return true;
      }
      if (target_cell != PathFinder.InvalidCell)
      {
        target_cell = cell.parent;
        cell = this.GetCell(target_cell, cell.parentNavType, out is_cell_in_range);
      }
      if (cell.cost >= cost && target_cell != PathFinder.InvalidCell)
      {
        KCrashReporter.ReportDevNotification("Invalid Cost Progression", Environment.StackTrace, $"{source_cell}x{current_nav_type} -> {target_cell} via path of length {path.nodes.Count} cell_data.cost: {cell.cost} previousCost: {cost} cell_data.navType: {cell.navType}");
        break;
      }
      cost = cell.cost;
    }
    path.Clear();
    return false;
  }

  private struct ProberCell
  {
    public int cost;
    public ushort queryId;
    public NavType navType;
  }
}
