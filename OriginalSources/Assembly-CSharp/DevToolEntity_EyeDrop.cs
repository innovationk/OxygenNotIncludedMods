// Decompiled with JetBrains decompiler
// Type: DevToolEntity_EyeDrop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class DevToolEntity_EyeDrop : DevTool
{
  private Vector2 sampleAtScreenPosition;
  private Action<DevToolEntityTarget> onSelectionMadeFn;
  private Func<DevToolEntityTarget, Option<string>> getErrorForCandidateTargetFn;
  private bool requestingNavBack;
  private static Vector2 posSampler_rectBasePos = new Vector2(200f, 200f);
  private static Option<Vector2> posSampler_dragStartPos = (Option<Vector2>) Option.None;

  public DevToolEntity_EyeDrop(
    Action<DevToolEntityTarget> onSelectionMadeFn,
    Func<DevToolEntityTarget, Option<string>> getErrorForCandidateTargetFn = null)
  {
    this.onSelectionMadeFn = onSelectionMadeFn;
    this.getErrorForCandidateTargetFn = getErrorForCandidateTargetFn;
  }

  protected override void RenderTo(DevPanel panel)
  {
    if (this.requestingNavBack)
    {
      this.requestingNavBack = false;
      panel.NavGoBack();
    }
    else
    {
      if (ImGuiEx.BeginHelpMarker())
      {
        ImGui.TextWrapped("This will do a raycast check against:");
        ImGui.Bullet();
        ImGui.SameLine();
        ImGui.TextWrapped("world gameobjects that have a KCollider2D component");
        ImGui.Bullet();
        ImGui.SameLine();
        ImGui.TextWrapped("ui gameobjects with a Graphic component that also have `raycastTarget` set to true");
        ImGui.Bullet();
        ImGui.SameLine();
        ImGui.TextWrapped("world sim cells");
        ImGui.TextWrapped("This means that some gameobjects that can be seen will not show up here.");
        ImGuiEx.EndHelpMarker();
      }
      ImGui.Separator();
      DevToolEntity_EyeDrop.ImGuiInput_SampleScreenPosition(ref this.sampleAtScreenPosition);
      using (ListPool<DevToolEntityTarget, DevToolEntity_EyeDrop>.PooledList targets = PoolsFor<DevToolEntity_EyeDrop>.AllocateList<DevToolEntityTarget>())
      {
        Option<string> error1 = DevToolEntity_EyeDrop.CollectUIGameObjectHitsTo((IList<DevToolEntityTarget>) targets, (Vector3) this.sampleAtScreenPosition);
        Option<string> error2 = DevToolEntity_EyeDrop.CollectWorldGameObjectHitsTo((IList<DevToolEntityTarget>) targets, (Vector3) this.sampleAtScreenPosition);
        (Option<DevToolEntityTarget.ForSimCell> target, Option<string> error3) = DevToolEntity_EyeDrop.GetSimCellAt((Vector3) this.sampleAtScreenPosition);
        if (target.IsSome())
          targets.Add((DevToolEntityTarget) target.Unwrap());
        if (ImGui.TreeNode("Debug Info"))
        {
          DrawBullet("[UI GameObjects]", error1);
          DrawBullet("[World GameObjects]", error2);
          DrawBullet("[Sim Cell]", error3);
          ImGui.TreePop();
        }
        ImGui.Separator();
        foreach (DevToolEntityTarget toolEntityTarget in (List<DevToolEntityTarget>) targets)
        {
          Option<string> option = this.getErrorForCandidateTargetFn == null ? (Option<string>) Option.None : this.getErrorForCandidateTargetFn(toolEntityTarget);
          Option<(Vector2, Vector2)> screenRect = toolEntityTarget.GetScreenRect();
          int num = ImGuiEx.Button($"Pick target \"{toolEntityTarget.GetDebugName()}\"", option.IsNone()) ? 1 : 0;
          bool isFocused = ImGui.IsItemHovered();
          if (isFocused)
          {
            ImGui.BeginTooltip();
            if (option.IsSome())
            {
              ImGui.Text("Error:");
              ImGui.Text(option.Unwrap());
              if (screenRect.IsSome())
              {
                ImGui.Separator();
                ImGui.Separator();
              }
            }
            if (screenRect.IsNone())
              ImGui.Text("Error: Couldn't get screen rect to display.");
            ImGui.EndTooltip();
          }
          if (num != 0)
          {
            this.onSelectionMadeFn(toolEntityTarget);
            this.requestingNavBack = true;
          }
          if (screenRect.IsSome())
            DevToolEntity.DrawBoundingBox(screenRect.Unwrap(), toolEntityTarget.GetDebugName(), isFocused);
        }
      }
    }

    static void DrawBullet(string groupName, Option<string> error)
    {
      ImGui.Bullet();
      ImGui.Text(groupName);
      ImGui.SameLine();
      if (error.IsSome())
      {
        ImGui.Text("[ERROR]");
        ImGui.SameLine();
        ImGui.Text(error.Unwrap());
      }
      else
        ImGui.Text("No errors.");
    }
  }

  public static Option<string> CollectUIGameObjectHitsTo(
    IList<DevToolEntityTarget> targets,
    Vector3 screenPosition)
  {
    using (ListPool<RaycastResult, DevToolEntity_EyeDrop>.PooledList pooledList = PoolsFor<DevToolEntity_EyeDrop>.AllocateList<RaycastResult>())
    {
      UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
      if (current.IsNullOrDestroyed())
        return (Option<string>) "No EventSystem found.";
      UnityEngine.EventSystems.EventSystem eventSystem = current;
      PointerEventData eventData = new PointerEventData(current);
      eventData.position = (Vector2) screenPosition;
      ListPool<RaycastResult, DevToolEntity_EyeDrop>.PooledList raycastResults = pooledList;
      eventSystem.RaycastAll(eventData, (List<RaycastResult>) raycastResults);
      foreach (RaycastResult raycastResult in (List<RaycastResult>) pooledList)
      {
        if (!(raycastResult.gameObject.name == "ImGui Consume Input"))
          targets.Add((DevToolEntityTarget) new DevToolEntityTarget.ForUIGameObject(raycastResult.gameObject));
      }
    }
    return (Option<string>) Option.None;
  }

  public static Option<string> CollectWorldGameObjectHitsTo(
    IList<DevToolEntityTarget> targets,
    Vector3 screenPosition)
  {
    Camera main = Camera.main;
    if (main.IsNullOrDestroyed())
      return (Option<string>) "No Main Camera found.";
    (Option<DevToolEntityTarget.ForSimCell> target, Option<string> error) = DevToolEntity_EyeDrop.GetSimCellAt(screenPosition);
    if (error.IsSome())
      return error;
    if (target.IsNone())
      return (Option<string>) "Couldn't find sim cell";
    DevToolEntityTarget.ForSimCell forSimCell = target.Unwrap();
    Vector2 worldPoint = (Vector2) main.ScreenToWorldPoint(screenPosition);
    using (ListPool<InterfaceTool.Intersection, DevToolEntity_EyeDrop>.PooledList intersections = PoolsFor<DevToolEntity_EyeDrop>.AllocateList<InterfaceTool.Intersection>())
    {
      using (ListPool<ScenePartitionerEntry, DevToolEntity_EyeDrop>.PooledList gathered_entries = PoolsFor<DevToolEntity_EyeDrop>.AllocateList<ScenePartitionerEntry>())
      {
        int x;
        int y;
        Grid.CellToXY(forSimCell.cellIndex, out x, out y);
        Game.Instance.statusItemRenderer.GetIntersections(worldPoint, (List<InterfaceTool.Intersection>) intersections);
        GameScenePartitioner.Instance.GatherEntries(x, y, 1, 1, GameScenePartitioner.Instance.collisionLayer, (List<ScenePartitionerEntry>) gathered_entries);
        foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
        {
          KCollider2D kcollider2D = partitionerEntry.obj as KCollider2D;
          if (!kcollider2D.IsNullOrDestroyed() && kcollider2D.Intersects(worldPoint) && !(kcollider2D.gameObject.name == "WorldSelectionCollider"))
            targets.Add((DevToolEntityTarget) new DevToolEntityTarget.ForWorldGameObject(kcollider2D.gameObject));
        }
      }
    }
    return (Option<string>) Option.None;
  }

  public static (Option<DevToolEntityTarget.ForSimCell> target, Option<string> error) GetSimCellAt(
    Vector3 screenPosition)
  {
    if ((UnityEngine.Object) Game.Instance == (UnityEngine.Object) null)
      return ((Option<DevToolEntityTarget.ForSimCell>) Option.None, (Option<string>) "No Game instance found.");
    Camera main = Camera.main;
    if (main.IsNullOrDestroyed())
      return ((Option<DevToolEntityTarget.ForSimCell>) Option.None, (Option<string>) "No Main Camera found.");
    Ray ray = main.ScreenPointToRay(screenPosition);
    float enter;
    if (!new Plane(new Vector3(0.0f, 0.0f, -1f), new Vector3(0.0f, 0.0f, 1f)).Raycast(ray, out enter))
      return ((Option<DevToolEntityTarget.ForSimCell>) Option.None, (Option<string>) "Ray from camera did not hit game plane.");
    int cell = Grid.PosToCell(ray.GetPoint(enter));
    if (cell < 0 || Grid.CellCount <= cell)
      return ((Option<DevToolEntityTarget.ForSimCell>) Option.None, (Option<string>) $"Found cell index {cell} is out of range {cell}..{Grid.CellCount}");
    return !Grid.IsValidCell(cell) ? ((Option<DevToolEntityTarget.ForSimCell>) Option.None, (Option<string>) $"Cell index {cell} is invalid") : ((Option<DevToolEntityTarget.ForSimCell>) new DevToolEntityTarget.ForSimCell(cell), (Option<string>) Option.None);
  }

  public static void ImGuiInput_SampleScreenPosition(ref Vector2 unityScreenPosition)
  {
    float num1 = 4f;
    float num2 = 12f;
    float thickness = 4f;
    float num3 = 6f;
    float num4 = 2f;
    float num5 = num1 + num2 + thickness;
    float num6 = num1 + num2 + thickness + num4 + num3;
    float rounding = num1 + 4f;
    Vector2 vector2_1 = Vector2.one * num1 * 2f;
    Vector2 vector2_2 = Vector2.one * num5 * 2f;
    Vector2 vector2_3 = Vector2.one * (num5 + num3) * 2f;
    Vector2 size = Vector2.one * num6 * 2f;
    ImGuiWindowFlags flags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.HorizontalScrollbar;
    Vector2 mousePos = ImGui.GetMousePos();
    Vector2 samplerRectBasePos = DevToolEntity_EyeDrop.posSampler_rectBasePos;
    if (DevToolEntity_EyeDrop.posSampler_dragStartPos.IsSome())
      samplerRectBasePos += mousePos - DevToolEntity_EyeDrop.posSampler_dragStartPos.Unwrap();
    ImGui.SetNextWindowPos(samplerRectBasePos - size / 2f);
    ImGui.SetNextWindowSizeConstraints(Vector2.one, Vector2.one * -1f);
    ImGui.SetNextWindowSize(size);
    ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, Vector2.zero);
    ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.zero);
    if (ImGui.Begin("###ID_EyeDropper", flags))
    {
      bool flag = ImGui.IsWindowHovered();
      int num7 = !ImGui.IsWindowHovered() ? 0 : (ImGui.IsMouseDown(ImGuiMouseButton.Left) ? 1 : 0);
      Color col = num7 == 0 ? (!flag ? Util.ColorFromHex("EC4F71") : Util.ColorFromHex("F498AC")) : Util.ColorFromHex("C5153B");
      if (num7 != 0 && DevToolEntity_EyeDrop.posSampler_dragStartPos.IsNone())
        DevToolEntity_EyeDrop.posSampler_dragStartPos = (Option<Vector2>) mousePos;
      if (ImGui.IsMouseReleased(ImGuiMouseButton.Left) && DevToolEntity_EyeDrop.posSampler_dragStartPos.IsSome())
      {
        DevToolEntity_EyeDrop.posSampler_rectBasePos += mousePos - DevToolEntity_EyeDrop.posSampler_dragStartPos.Unwrap();
        DevToolEntity_EyeDrop.posSampler_dragStartPos = (Option<Vector2>) Option.None;
      }
      ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
      Vector2 p_min = ImGui.GetCursorScreenPos() + Vector2.one * num4;
      Vector2 vector2_4 = Vector2.one * num3;
      Vector2 vector2_5 = (vector2_2 - vector2_1) / 2f + vector2_4;
      unityScreenPosition = new Vector2(p_min.x + vector2_5.x + num1, (float) -((double) p_min.y + (double) vector2_5.y + (double) num1) + (float) Screen.height);
      windowDrawList.AddRectFilled(p_min, p_min + vector2_3, ImGui.GetColorU32(new Vector4(0.0f, 0.0f, 0.0f, 0.7f)), rounding);
      windowDrawList.AddRectFilled(p_min + vector2_5, p_min + vector2_5 + vector2_1, ImGui.GetColorU32((Vector4) col), rounding);
      windowDrawList.AddRect(p_min + vector2_4, p_min + vector2_4 + vector2_2, ImGui.GetColorU32((Vector4) col), rounding, ImDrawFlags.None, thickness);
      ImGui.End();
    }
    ImGui.PopStyleVar(2);
  }
}
