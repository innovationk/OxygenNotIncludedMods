// Decompiled with JetBrains decompiler
// Type: DevToolAllThingsCritter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System;
using UnityEngine;

#nullable disable
public class DevToolAllThingsCritter : DevTool
{
  private bool follow;
  private GameObject lockObject;
  private bool drawNavDots = true;
  private bool drawOccupyArea;
  private bool drawCollider;

  protected override void RenderTo(DevPanel panel)
  {
    if ((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null || (UnityEngine.Object) this.lockObject != (UnityEngine.Object) null)
      this.Contents();
    else
      ImGui.Text("No Critter Selected");
  }

  private void Contents()
  {
    ImGui.Spacing();
    if (!((UnityEngine.Object) Camera.main != (UnityEngine.Object) null) || !((UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null))
      return;
    GameObject gameObject = (GameObject) null;
    ImGui.Checkbox("Lock", ref this.follow);
    if (this.follow)
    {
      if ((UnityEngine.Object) this.lockObject == (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected.GetComponent<KPrefabID>() != (UnityEngine.Object) null && SelectTool.Instance.selected.HasTag(GameTags.Creature))
        this.lockObject = SelectTool.Instance.selected.gameObject;
      gameObject = this.lockObject;
    }
    else if ((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) SelectTool.Instance.selected.GetComponent<KPrefabID>() != (UnityEngine.Object) null && SelectTool.Instance.selected.HasTag(GameTags.Creature))
        gameObject = SelectTool.Instance.selected.gameObject;
      this.lockObject = (GameObject) null;
    }
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
      return;
    ImGuiEx.SimpleField("Name", DevToolEntity.GetNameFor(gameObject));
    Vector3 position = gameObject.transform.GetPosition();
    ImGuiEx.SimpleField("Position", $"X={position.x:F2}, Y={position.y:F2}, Z={position.z:F2}");
    ImGuiEx.SimpleField("Cell", (object) Grid.PosToCell(gameObject));
    this.NavigatorContents(position, gameObject);
    this.OccupyAreaContents(gameObject);
    this.ColliderContents(gameObject);
    this.CritterTemperatureMonitorContents(gameObject);
  }

  private void NavigatorContents(Vector3 pos, GameObject go)
  {
    Navigator component = go.GetComponent<Navigator>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !ImGui.CollapsingHeader("Navigator", ImGuiTreeNodeFlags.DefaultOpen))
      return;
    ImGui.Checkbox("Draw", ref this.drawNavDots);
    Vector2 positionFor = DevToolEntity.GetPositionFor(component.gameObject);
    ImGui.TextColored((Vector4) Color.green, "World: " + $"X={pos.x:F2}, Y={pos.y:F2}");
    ImDrawListPtr backgroundDrawList;
    if (this.drawNavDots)
    {
      backgroundDrawList = ImGui.GetBackgroundDrawList();
      backgroundDrawList.AddCircleFilled(positionFor, 10f, ImGui.GetColorU32((Vector4) Color.green));
    }
    Vector2 pivotSymbolPosition = (Vector2) component.GetComponent<KBatchedAnimController>().GetPivotSymbolPosition();
    Vector2 screenPosition1 = DevToolEntity.GetScreenPosition((Vector3) pivotSymbolPosition);
    ImGui.TextColored((Vector4) Color.blue, "Pivot: " + $"X={pivotSymbolPosition.x:F2}, Y={pivotSymbolPosition.y:F2}");
    if (this.drawNavDots)
    {
      backgroundDrawList = ImGui.GetBackgroundDrawList();
      backgroundDrawList.AddCircleFilled(screenPosition1, 10f, ImGui.GetColorU32((Vector4) Color.blue));
    }
    TransitionDriver transitionDriver = component.transitionDriver;
    if (transitionDriver.GetTransition == null)
      return;
    if (transitionDriver.GetTransition.navGridTransition.useXOffset)
    {
      Vector2 vector2 = go.GetComponent<KBoxCollider2D>().size / 2f;
      if (transitionDriver.GetTransition.x > 0)
        pos.x += vector2.x;
      else if (transitionDriver.GetTransition.x < 0)
        pos.x -= vector2.x;
      Vector2 screenPosition2 = DevToolEntity.GetScreenPosition(pos);
      ImGui.TextColored((Vector4) Color.magenta, "Nav Transition: " + $"X={pos.x:F2}, Y={pos.y:F2}");
      if (this.drawNavDots)
      {
        backgroundDrawList = ImGui.GetBackgroundDrawList();
        backgroundDrawList.AddCircleFilled(screenPosition2, 10f, ImGui.GetColorU32((Vector4) Color.magenta));
      }
    }
    ImGuiEx.SimpleField("Transition", transitionDriver.GetTransition.navGridTransition.ToString());
  }

  private void OccupyAreaContents(GameObject go)
  {
    OccupyArea component = go.GetComponent<OccupyArea>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !ImGui.CollapsingHeader("Occupy Area", ImGuiTreeNodeFlags.DefaultOpen))
      return;
    Extents extents = component.GetExtents();
    ImGui.Checkbox("Draw Occupy Area", ref this.drawOccupyArea);
    if (this.drawOccupyArea)
    {
      Vector2 screenPosition = DevToolEntity.GetScreenPosition(Grid.CellToPos(Grid.OffsetCell(Grid.PosToCell(go), extents.width, extents.height)));
      DevToolEntity.DrawScreenRect((DevToolEntity.GetScreenPosition(Grid.CellToPos(Grid.XYToCell(extents.x, extents.y))), screenPosition), (Option<string>) go.name, (Option<Color>) Color.cyan, (Option<Color>) new Color(0.0f, 1f, 1f, 0.33f));
    }
    ImGui.Text($"X={extents.x:F2}, Y={extents.y:F2}");
    ImGui.Text($"Width={extents.width:F2}, Height={extents.height:F2}");
  }

  private void ColliderContents(GameObject go)
  {
    KCollider2D component = go.GetComponent<KCollider2D>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !ImGui.CollapsingHeader("Collider", ImGuiTreeNodeFlags.DefaultOpen))
      return;
    ImGui.Checkbox("Draw Collider", ref this.drawCollider);
    Bounds bounds;
    if (this.drawCollider)
    {
      Vector2 screenPosition1 = DevToolEntity.GetScreenPosition(component.bounds.min);
      bounds = component.bounds;
      Vector2 screenPosition2 = DevToolEntity.GetScreenPosition(bounds.max);
      DevToolEntity.DrawScreenRect((screenPosition1, screenPosition2), (Option<string>) go.name, (Option<Color>) Color.green, (Option<Color>) new Color(0.0f, 1f, 0.0f, 0.33f));
    }
    string field_value = $"X={component.offset.x:F2}, Y={component.offset.y:F2}";
    ImGuiEx.SimpleField("Offset", field_value);
    ImGui.Text("Bounds");
    ImGuiEx.SimpleField("Offset", field_value);
    bounds = component.bounds;
    // ISSUE: variable of a boxed type
    __Boxed<Vector3> min = (ValueType) bounds.min;
    bounds = component.bounds;
    // ISSUE: variable of a boxed type
    __Boxed<int> cell1 = (ValueType) Grid.PosToCell(bounds.min);
    ImGuiEx.SimpleField("Min", $"{min} Cell: {cell1}");
    bounds = component.bounds;
    // ISSUE: variable of a boxed type
    __Boxed<Vector3> max = (ValueType) bounds.max;
    bounds = component.bounds;
    // ISSUE: variable of a boxed type
    __Boxed<int> cell2 = (ValueType) Grid.PosToCell(bounds.max);
    ImGuiEx.SimpleField("Max", $"{max} Cell: {cell2}");
    bounds = component.bounds;
    ImGuiEx.SimpleField("Center", (object) bounds.center);
  }

  private void CritterTemperatureMonitorContents(GameObject go)
  {
    CritterTemperatureMonitor.Instance smi = go.GetSMI<CritterTemperatureMonitor.Instance>();
    if (smi == null || !ImGui.CollapsingHeader("Temperature Monitor", ImGuiTreeNodeFlags.DefaultOpen))
      return;
    ImGuiEx.SimpleField("Current State", smi.GetCurrentState().name.Replace("root.", ""));
    ImGuiEx.SimpleField("External", (object) smi.GetTemperatureExternal());
    ImGuiEx.SimpleField("Internal", (object) smi.GetTemperatureInternal());
  }
}
