// Decompiled with JetBrains decompiler
// Type: DrawNavGridQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DrawNavGridQuery : PathFinderQuery
{
  public DrawNavGridQuery Reset(MinionBrain brain) => this;

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    if (parent_cell == Grid.InvalidCell || (int) Grid.WorldIdx[parent_cell] != ClusterManager.Instance.activeWorldId || (int) Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
      return false;
    GL.Color(Color.white);
    GL.Vertex(Grid.CellToPosCCC(parent_cell, Grid.SceneLayer.Move));
    GL.Vertex(Grid.CellToPosCCC(cell, Grid.SceneLayer.Move));
    return false;
  }
}
