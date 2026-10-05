// Decompiled with JetBrains decompiler
// Type: NavPathDrawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/NavPathDrawer")]
public class NavPathDrawer : KMonoBehaviour
{
  private PathFinder.Path path;
  public Material material;
  private Vector3 navigatorPos;
  private Navigator navigator;

  public static NavPathDrawer Instance { get; private set; }

  public static void DestroyInstance() => NavPathDrawer.Instance = (NavPathDrawer) null;

  protected override void OnPrefabInit()
  {
    this.material = new Material(Shader.Find("Lines/Colored Blended"));
    NavPathDrawer.Instance = this;
  }

  protected override void OnCleanUp() => NavPathDrawer.Instance = (NavPathDrawer) null;

  public void DrawPath(Vector3 navigator_pos, PathFinder.Path path)
  {
    this.navigatorPos = navigator_pos;
    this.navigatorPos.y += 0.5f;
    this.path = path;
  }

  public Navigator GetNavigator() => this.navigator;

  public void SetNavigator(Navigator navigator) => this.navigator = navigator;

  public void ClearNavigator() => this.navigator = (Navigator) null;

  private void DrawPath(
    PathFinder.Path path,
    Vector3 navigator_pos,
    Color color,
    bool color_by_type = false)
  {
    if (path.nodes == null || path.nodes.Count <= 1)
      return;
    GL.PushMatrix();
    this.material.SetPass(0);
    GL.Begin(1);
    GL.Color(!color_by_type ? color : NavGrid.NavTypeColor(path.nodes[0].navType));
    GL.Vertex(navigator_pos);
    GL.Vertex(NavTypeHelper.GetNavPos(path.nodes[1].cell, path.nodes[1].navType));
    for (int index = 1; index < path.nodes.Count - 1; ++index)
    {
      if ((int) Grid.WorldIdx[path.nodes[index].cell] == ClusterManager.Instance.activeWorldId && (int) Grid.WorldIdx[path.nodes[index + 1].cell] == ClusterManager.Instance.activeWorldId)
      {
        Vector3 navPos1 = NavTypeHelper.GetNavPos(path.nodes[index].cell, path.nodes[index].navType);
        Vector3 navPos2 = NavTypeHelper.GetNavPos(path.nodes[index + 1].cell, path.nodes[index + 1].navType);
        if (color_by_type)
          GL.Color(NavGrid.NavTypeColor(path.nodes[index].navType));
        GL.Vertex(navPos1);
        GL.Vertex(navPos2);
      }
    }
    GL.End();
    GL.PopMatrix();
  }

  private void OnPostRender()
  {
    this.DrawPath(this.path, this.navigatorPos, Color.white);
    this.path = new PathFinder.Path();
    this.DebugDrawSelectedNavigator();
    if (!((UnityEngine.Object) this.navigator != (UnityEngine.Object) null))
      return;
    GL.PushMatrix();
    this.material.SetPass(0);
    GL.Begin(1);
    this.navigator.RunQuery((PathFinderQuery) PathFinderQueries.drawNavGridQuery.Reset((MinionBrain) null));
    GL.End();
    GL.PopMatrix();
  }

  private void DebugDrawSelectedNavigator()
  {
    if (!DebugHandler.DebugPathFinding || (UnityEngine.Object) SelectTool.Instance == (UnityEngine.Object) null || (UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) null)
      return;
    Navigator component = SelectTool.Instance.selected.GetComponent<Navigator>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    int mouseCell = DebugHandler.GetMouseCell();
    if (!Grid.IsValidCell(mouseCell))
      return;
    PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(Grid.PosToCell((KMonoBehaviour) component), component.CurrentNavType, component.flags);
    PathFinder.Path path = new PathFinder.Path();
    if (!component.PathGrid.BuildPath(component.cachedCell, mouseCell, component.CurrentNavType, ref path))
      PathFinder.UpdatePath(component.NavGrid, component.GetCurrentAbilities(), potential_path, (PathFinderQuery) PathFinderQueries.cellQuery.Reset(mouseCell), ref path);
    string text = $"{$"{$"Source: {Grid.PosToCell((KMonoBehaviour) component).ToString()}\n"}Dest: {mouseCell.ToString()}\n"}Cost: {path.cost.ToString()}";
    if ((UnityEngine.Object) component.GetComponent<MinionBrain>() != (UnityEngine.Object) null)
      text = $"{text}\nSafeFlags: {Convert.ToString((int) SafeCellQuery.GetFlags(mouseCell, component.GetComponent<MinionBrain>()), 2)}";
    this.DrawPath(path, component.GetComponent<KAnimControllerBase>().GetPivotSymbolPosition(), Color.green, true);
    DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
  }
}
