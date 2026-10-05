// Decompiled with JetBrains decompiler
// Type: IdleSuitMarkerCellQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class IdleSuitMarkerCellQuery : PathFinderQuery
{
  private int targetCell;
  private bool isRotated;
  private int markerX;

  public IdleSuitMarkerCellQuery(bool is_rotated, int marker_x)
  {
    this.targetCell = Grid.InvalidCell;
    this.isRotated = is_rotated;
    this.markerX = marker_x;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    if (!Grid.PreventIdleTraversal[cell] && Grid.CellToXY(cell).x < this.markerX != this.isRotated)
      this.targetCell = cell;
    return this.targetCell != Grid.InvalidCell;
  }

  public override int GetResultCell() => this.targetCell;
}
