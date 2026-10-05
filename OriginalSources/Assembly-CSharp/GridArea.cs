// Decompiled with JetBrains decompiler
// Type: GridArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public struct GridArea
{
  private Vector2I min;
  private Vector2I max;
  private int MinCell;
  private int MaxCell;

  public Vector2I Min => this.min;

  public Vector2I Max => this.max;

  public void SetArea(int cell, int width, int height)
  {
    Vector2I xy = Grid.CellToXY(cell);
    Vector2I vector2I = new Vector2I(xy.x + width, xy.y + height);
    this.SetExtents(xy.x, xy.y, vector2I.x, vector2I.y);
  }

  public void SetExtents(int min_x, int min_y, int max_x, int max_y)
  {
    this.min.x = Math.Max(min_x, 0);
    this.min.y = Math.Max(min_y, 0);
    this.max.x = Math.Min(max_x, Grid.WidthInCells);
    this.max.y = Math.Min(max_y, Grid.HeightInCells);
    this.MinCell = Grid.XYToCell(this.min.x, this.min.y);
    this.MaxCell = Grid.XYToCell(this.max.x, this.max.y);
  }

  public bool Contains(int cell)
  {
    if (cell < this.MinCell || cell >= this.MaxCell)
      return false;
    int num = cell % Grid.WidthInCells;
    return num >= this.Min.x && num < this.Max.x;
  }

  public bool Contains(int x, int y)
  {
    return x >= this.min.x && x < this.max.x && y >= this.min.y && y < this.max.y;
  }

  public bool Contains(Vector3 pos)
  {
    return (double) this.min.x <= (double) pos.x && (double) pos.x < (double) this.max.x && (double) this.min.y <= (double) pos.y && (double) pos.y <= (double) this.max.y;
  }

  public void RunIfInside(int cell, Action<int> action)
  {
    if (!this.Contains(cell))
      return;
    action(cell);
  }

  public void Run(Action<int> action)
  {
    for (int y = this.min.y; y < this.max.y; ++y)
    {
      for (int x = this.min.x; x < this.max.x; ++x)
      {
        int cell = Grid.XYToCell(x, y);
        action(cell);
      }
    }
  }

  public void RunOnDifference(GridArea subtract_area, Action<int> action)
  {
    for (int y = this.min.y; y < this.max.y; ++y)
    {
      for (int x = this.min.x; x < this.max.x; ++x)
      {
        if (!subtract_area.Contains(x, y))
        {
          int cell = Grid.XYToCell(x, y);
          action(cell);
        }
      }
    }
  }

  public int GetCellCount() => (this.max.x - this.min.x) * (this.max.y - this.min.y);
}
