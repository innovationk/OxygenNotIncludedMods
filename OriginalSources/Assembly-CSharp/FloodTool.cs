// Decompiled with JetBrains decompiler
// Type: FloodTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FloodTool : InterfaceTool
{
  public Func<int, FloodFill.BoundaryCheckResult> floodCriteria;
  public Action<List<int>> paintArea;
  protected Color32 areaColour = (Color32) new Color(0.5f, 0.7f, 0.5f, 0.2f);
  protected int mouseCell = -1;

  public List<int> Flood(int startCell)
  {
    List<int> validCells = new List<int>();
    FloodFill.DepthCollect(startCell, this.floodCriteria, validCells);
    return validCells;
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    base.OnLeftClickDown(cursor_pos);
    this.paintArea(this.Flood(Grid.PosToCell(cursor_pos)));
  }

  public override void OnMouseMove(Vector3 cursor_pos)
  {
    base.OnMouseMove(cursor_pos);
    this.mouseCell = Grid.PosToCell(cursor_pos);
  }
}
