// Decompiled with JetBrains decompiler
// Type: DevToolNavGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

#nullable disable
public class DevToolNavGrid : DevTool
{
  private const string INVALID_OVERLAY_MODE_STR = "None";
  private string[] navGridNames;
  private int selectedNavGrid;
  private bool drawLinks;
  private Dictionary<NavType, bool> drawLinkTypes = new Dictionary<NavType, bool>();
  public static DevToolNavGrid Instance;
  private int[] linkStats;
  private int highestLinkCell;
  private int highestLinkCount;
  private int selectedCell = Grid.InvalidCell;
  private bool follow;
  private GameObject lockObject;

  public DevToolNavGrid()
  {
    DevToolNavGrid.Instance = this;
    this.drawLinkTypes = new Dictionary<NavType, bool>(11);
    foreach (NavType key in Enum.GetValues(typeof (NavType)))
      this.drawLinkTypes.Add(key, true);
  }

  private bool Init()
  {
    if ((UnityEngine.Object) Pathfinding.Instance == (UnityEngine.Object) null)
      return false;
    if (this.navGridNames != null)
      return true;
    this.navGridNames = Pathfinding.Instance.GetNavGrids().Select<NavGrid, string>((Func<NavGrid, string>) (x => x.id)).ToArray<string>();
    return true;
  }

  protected override void RenderTo(DevPanel panel)
  {
    if (this.Init())
      this.Contents();
    else
      ImGui.Text("Game not initialized");
  }

  public void SetCell(int cell) => this.selectedCell = cell;

  private void Contents()
  {
    ImGui.Combo("Nav Grid ID", ref this.selectedNavGrid, this.navGridNames, this.navGridNames.Length);
    NavGrid navGrid = Pathfinding.Instance.GetNavGrid(this.navGridNames[this.selectedNavGrid]);
    ImGui.Text("Max Links per cell: " + navGrid.maxLinksPerCell.ToString());
    ImGui.Spacing();
    int num1 = Marshal.SizeOf<NavGrid.Link>();
    long num2 = (long) navGrid.maxLinksPerCell * (long) Grid.CellCount * (long) num1;
    long num3 = (long) Grid.CellCount * 2L;
    long num4 = (long) ((Grid.CellCount + 7) / 8);
    ImGui.Text($"Memory usage: {(num2 + num3 + num4) / 1048576L /*0x100000*/} MB (Links: {num2 / 1048576L /*0x100000*/} MB [{num1}B per link], NavTable: {num3 / 1024L /*0x0400*/} KB)");
    if (ImGui.Button("Calculate Stats"))
    {
      this.linkStats = new int[navGrid.maxLinksPerCell];
      this.highestLinkCell = 0;
      this.highestLinkCount = 0;
      for (int index1 = 0; index1 < Grid.CellCount; ++index1)
      {
        int index2 = 0;
        for (int index3 = 0; index3 < navGrid.maxLinksPerCell; ++index3)
        {
          int index4 = index1 * navGrid.maxLinksPerCell + index3;
          if (navGrid.Links[index4].link != Grid.InvalidCell)
            ++index2;
          else
            break;
        }
        if (index2 > this.highestLinkCount)
        {
          this.highestLinkCell = index1;
          this.highestLinkCount = index2;
        }
        ++this.linkStats[index2];
      }
    }
    ImGui.SameLine();
    if (ImGui.Button("Clear"))
      this.linkStats = (int[]) null;
    ImGui.SameLine();
    if (ImGui.Button("Rescan"))
      navGrid.InitializeGraph();
    ImGui.SameLine();
    if (ImGui.Button("Dirty") && this.selectedCell != Grid.InvalidCell)
      Pathfinding.Instance.AddDirtyNavGridCell(this.selectedCell);
    if (this.linkStats != null)
    {
      ImGui.Text("Highest link count: " + this.highestLinkCount.ToString());
      ImGui.Text($"Utilized percentage: {(ValueType) (float) ((double) this.highestLinkCount / (double) navGrid.maxLinksPerCell * 100.0)} %");
      ImGui.SameLine();
      if (ImGui.Button($"Select {this.highestLinkCell}"))
        this.selectedCell = this.highestLinkCell;
      for (int index = 0; index < this.linkStats.Length; ++index)
      {
        if (this.linkStats[index] > 0)
          ImGui.Text($"\t{index}: {this.linkStats[index]}");
      }
    }
    if ((UnityEngine.Object) Camera.main != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null)
    {
      GameObject gameObject = (GameObject) null;
      ImGui.Checkbox("Lock", ref this.follow);
      ImGui.Checkbox("DrawDebugPath", ref DebugHandler.DebugPathFinding);
      if (this.follow)
      {
        if ((UnityEngine.Object) this.lockObject == (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null)
          this.lockObject = SelectTool.Instance.selected.gameObject;
        gameObject = this.lockObject;
      }
      else if ((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null)
      {
        gameObject = SelectTool.Instance.selected.gameObject;
        this.lockObject = (GameObject) null;
      }
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        Navigator component = gameObject.GetComponent<Navigator>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          Vector2 positionFor = DevToolEntity.GetPositionFor(component.gameObject);
          ImGui.GetBackgroundDrawList().AddCircleFilled(positionFor, 10f, ImGui.GetColorU32((Vector4) Color.green));
          Vector2 screenPosition1 = DevToolEntity.GetScreenPosition(component.GetComponent<KBatchedAnimController>().GetPivotSymbolPosition());
          ImGui.GetBackgroundDrawList().AddCircleFilled(screenPosition1, 10f, ImGui.GetColorU32((Vector4) Color.blue));
          TransitionDriver transitionDriver = component.transitionDriver;
          if (transitionDriver.GetTransition != null)
          {
            Vector3 position = component.transform.GetPosition();
            Vector2 vector2 = gameObject.GetComponent<KBoxCollider2D>().size / 2f;
            if (transitionDriver.GetTransition.x > 0)
              position.x += vector2.x;
            else if (transitionDriver.GetTransition.x < 0)
              position.x -= vector2.x;
            Vector2 screenPosition2 = DevToolEntity.GetScreenPosition(position);
            ImGui.GetBackgroundDrawList().AddCircleFilled(screenPosition2, 10f, ImGui.GetColorU32((Vector4) Color.magenta));
          }
          if (DebugHandler.DebugPathFinding)
          {
            int mouseCell = DebugHandler.GetMouseCell();
            if (Grid.IsValidCell(mouseCell))
            {
              PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(Grid.PosToCell((KMonoBehaviour) component), component.CurrentNavType, component.flags);
              PathFinder.Path path = new PathFinder.Path();
              if (!component.PathGrid.BuildPath(component.cachedCell, mouseCell, component.CurrentNavType, ref path))
                PathFinder.UpdatePath(component.NavGrid, component.GetCurrentAbilities(), potential_path, (PathFinderQuery) PathFinderQueries.cellQuery.Reset(mouseCell), ref path);
              if (path.nodes != null)
              {
                for (int index = 0; index < path.nodes.Count - 1; ++index)
                {
                  if ((int) Grid.WorldIdx[path.nodes[index].cell] == ClusterManager.Instance.activeWorldId)
                  {
                    NavGrid.Transition transition = navGrid.transitions[(int) path.nodes[index].transitionId];
                    ImGui.Text($"   {transition.start} -> {transition.end} x:{transition.x} y:{transition.y} anim:{transition.anim} cost:{transition.cost}");
                  }
                }
              }
              else
                ImGui.Text("No valid path");
            }
          }
        }
      }
    }
    ImGui.Spacing();
    ImGui.Checkbox("Draw Links", ref this.drawLinks);
    if (this.drawLinks)
    {
      ImGui.Indent();
      foreach (NavType key in this.drawLinkTypes.Keys.ToList<NavType>())
      {
        bool drawLinkType = this.drawLinkTypes[key];
        if (key != NavType.NumNavTypes)
        {
          ImGui.PushID(key.ToString());
          if (ImGui.Checkbox(key.ToString(), ref drawLinkType))
            this.drawLinkTypes[key] = drawLinkType;
          ImGui.PopID();
        }
      }
      if (ImGui.Button("Deselect All"))
      {
        foreach (NavType key in Enum.GetValues(typeof (NavType)))
          this.drawLinkTypes[key] = false;
      }
      ImGui.SameLine();
      if (ImGui.Button("Select All"))
      {
        foreach (NavType key in Enum.GetValues(typeof (NavType)))
          this.drawLinkTypes[key] = true;
      }
      ImGui.Unindent();
      this.DebugDrawLinks(navGrid);
    }
    ImGui.Spacing();
    int x;
    int y;
    Grid.CellToXY(this.selectedCell, out x, out y);
    ImGui.Text($"Selected Cell: {this.selectedCell} ({x},{y})");
    if (!Grid.IsValidCell(this.selectedCell))
      return;
    this.DrawNavTypes(this.selectedCell, navGrid);
    if (navGrid.Links == null || navGrid.Links.Length <= navGrid.maxLinksPerCell * this.selectedCell)
      return;
    for (int idx = 0; idx < navGrid.maxLinksPerCell; ++idx)
    {
      int index = this.selectedCell * navGrid.maxLinksPerCell + idx;
      NavGrid.Link link = navGrid.Links[index];
      if (link.link == Grid.InvalidCell)
        break;
      this.DrawLink(idx, link, navGrid);
    }
  }

  private void DrawLink(int idx, NavGrid.Link l, NavGrid navGrid)
  {
    NavGrid.Transition transition = navGrid.transitions[(int) l.transitionId];
    ImGui.Text($"   {transition.start} -> {transition.end} x:{transition.x} y:{transition.y} anim:{transition.anim} cost:{transition.cost}");
  }

  private void DrawNavTypes(int cell, NavGrid navGrid)
  {
    for (byte index = 0; index < (byte) 11; ++index)
    {
      NavType nav_type = (NavType) index;
      if (navGrid.NavTable.IsValid(cell, nav_type))
        ImGui.Text($"{nav_type}");
    }
  }

  private void DebugDrawLinks(NavGrid navGrid)
  {
    if ((UnityEngine.Object) Camera.main == (UnityEngine.Object) null)
      return;
    Camera main = Camera.main;
    int pixelHeight = main.pixelHeight;
    Color white = Color.white;
    for (int cell = 0; cell < Grid.CellCount; ++cell)
    {
      int end_cell_idx = cell * navGrid.maxLinksPerCell;
      for (int link = navGrid.Links[end_cell_idx].link; link != NavGrid.InvalidCell; link = navGrid.Links[end_cell_idx].link)
      {
        if (this.DrawNavTypeLink(navGrid, end_cell_idx, ref white))
        {
          Vector3 navPos1 = NavTypeHelper.GetNavPos(cell, navGrid.Links[end_cell_idx].startNavType);
          Vector3 navPos2 = NavTypeHelper.GetNavPos(link, navGrid.Links[end_cell_idx].endNavType);
          if (this.IsInCameraView(main, navPos1) && this.IsInCameraView(main, navPos2))
          {
            Vector2 screenPoint1 = (Vector2) main.WorldToScreenPoint(navPos1);
            Vector2 screenPoint2 = (Vector2) main.WorldToScreenPoint(navPos2);
            screenPoint1.y = (float) pixelHeight - screenPoint1.y;
            screenPoint2.y = (float) pixelHeight - screenPoint2.y;
            uint colorU32 = ImGui.GetColorU32((Vector4) white);
            this.DrawArrowLink(screenPoint1, screenPoint2, colorU32);
          }
        }
        ++end_cell_idx;
      }
    }
  }

  private bool IsInCameraView(Camera camera, Vector3 pos)
  {
    Vector3 viewportPoint = camera.WorldToViewportPoint(pos);
    return (double) viewportPoint.x >= 0.0 && (double) viewportPoint.y >= 0.0 && (double) viewportPoint.x <= 1.0 && (double) viewportPoint.y <= 1.0;
  }

  private bool DrawNavTypeLink(NavGrid navGrid, int end_cell_idx, ref Color color)
  {
    for (int index = 0; index < navGrid.ValidNavTypes.Length; ++index)
    {
      if (navGrid.ValidNavTypes[index] == navGrid.Links[end_cell_idx].startNavType)
      {
        color = NavGrid.NavTypeColor(navGrid.Links[end_cell_idx].startNavType);
        return this.drawLinkTypes[navGrid.Links[end_cell_idx].startNavType] || this.drawLinkTypes[navGrid.Links[end_cell_idx].endNavType];
      }
      if (navGrid.ValidNavTypes[index] == navGrid.Links[end_cell_idx].endNavType)
      {
        color = NavGrid.NavTypeColor(navGrid.Links[end_cell_idx].endNavType);
        return this.drawLinkTypes[navGrid.Links[end_cell_idx].startNavType] || this.drawLinkTypes[navGrid.Links[end_cell_idx].endNavType];
      }
    }
    return false;
  }

  private void DrawArrowLink(Vector2 start, Vector2 end, uint color)
  {
    ImDrawListPtr backgroundDrawList = ImGui.GetBackgroundDrawList();
    Vector2 vector2 = end - start;
    float magnitude = vector2.magnitude;
    if ((double) magnitude > 0.0)
      vector2 *= 1f / Mathf.Sqrt(magnitude);
    Vector2 p2 = end - vector2 * 1f + new Vector2(-vector2.y, vector2.x) * 1f;
    Vector2 p3 = end - vector2 * 1f - new Vector2(-vector2.y, vector2.x) * 1f;
    backgroundDrawList.AddLine(start, end, color);
    backgroundDrawList.AddTriangleFilled(end, p2, p3, color);
  }
}
