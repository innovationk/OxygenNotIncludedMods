// Decompiled with JetBrains decompiler
// Type: DevToolCavity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using ImGuiObjectDrawer;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class DevToolCavity : DevTool
{
  private Option<DevToolEntityTarget.ForSimCell> targetOpt;
  private bool shouldDrawBoundingBox = true;

  public DevToolCavity()
    : this((Option<DevToolEntityTarget.ForSimCell>) Option.None)
  {
  }

  public DevToolCavity(Option<DevToolEntityTarget.ForSimCell> target) => this.targetOpt = target;

  protected override void RenderTo(DevPanel panel)
  {
    if (ImGui.BeginMenuBar())
    {
      if (ImGui.MenuItem("Eyedrop New Target"))
        panel.PushDevTool((DevTool) new DevToolEntity_EyeDrop((Action<DevToolEntityTarget>) (target => this.targetOpt = (Option<DevToolEntityTarget.ForSimCell>) (DevToolEntityTarget.ForSimCell) target), new Func<DevToolEntityTarget, Option<string>>(DevToolCavity.GetErrorForCandidateTarget)));
      ImGui.EndMenuBar();
    }
    this.Name = "Cavity Info";
    if (this.targetOpt.IsNone())
      ImGui.TextWrapped("No Target selected");
    else if (Game.Instance.IsNullOrDestroyed())
      ImGui.TextWrapped("No Game instance");
    else if (Game.Instance.roomProber.IsNullOrDestroyed())
    {
      ImGui.TextWrapped("No RoomProber instance");
    }
    else
    {
      DevToolEntityTarget.ForSimCell uncastTarget = this.targetOpt.Unwrap();
      Option<string> forCandidateTarget = DevToolCavity.GetErrorForCandidateTarget((DevToolEntityTarget) uncastTarget);
      if (forCandidateTarget.IsSome())
      {
        ImGui.TextWrapped(forCandidateTarget.Unwrap());
      }
      else
      {
        this.Name = $"Cavity Info for: Cell {uncastTarget.cellIndex}";
        CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(uncastTarget.cellIndex);
        if (cavityForCell.IsNullOrDestroyed())
        {
          ImGui.TextWrapped("No Cavity found");
        }
        else
        {
          ImGui.Checkbox("Draw Bounding Box", ref this.shouldDrawBoundingBox);
          ImGuiEx.SimpleField("Room Type", cavityForCell.room.IsNullOrDestroyed() ? "<None>" : cavityForCell.room.GetProperName());
          ImGuiEx.SimpleField("Cell Count", (object) cavityForCell.NumCells);
          DevToolCavity.DrawKPrefabIdCollection("Creatures", (IEnumerable<KPrefabID>) cavityForCell.creatures);
          DevToolCavity.DrawKPrefabIdCollection("Buildings", (IEnumerable<KPrefabID>) cavityForCell.buildings);
          DevToolCavity.DrawKPrefabIdCollection("Plants", (IEnumerable<KPrefabID>) cavityForCell.plants);
          DevToolCavity.DrawKPrefabIdCollection("Eggs", (IEnumerable<KPrefabID>) cavityForCell.eggs);
          if (ImGui.CollapsingHeader("Full CavityInfo Object"))
            ImGuiEx.DrawObject("CavityInfo", (object) cavityForCell, new MemberDrawContext?(new MemberDrawContext(false, true)));
          if (!this.shouldDrawBoundingBox)
            return;
          Option<(Vector2 cornerA, Vector2 cornerB)> screenRect1 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(cavityForCell.minX, cavityForCell.minY)).GetScreenRect();
          Option<(Vector2 cornerA, Vector2 cornerB)> screenRect2 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(cavityForCell.maxX, cavityForCell.maxY)).GetScreenRect();
          if (!screenRect1.IsSome() || !screenRect2.IsSome())
            return;
          DevToolEntity.DrawBoundingBox((Vector2.Min(screenRect1.Unwrap().cornerA, Vector2.Min(screenRect1.Unwrap().cornerB, Vector2.Min(screenRect2.Unwrap().cornerA, screenRect2.Unwrap().cornerB))), Vector2.Max(screenRect1.Unwrap().cornerA, Vector2.Max(screenRect1.Unwrap().cornerB, Vector2.Max(screenRect2.Unwrap().cornerA, screenRect2.Unwrap().cornerB)))), cavityForCell.room.IsNullOrDestroyed() ? "<Room is null>" : cavityForCell.room.GetProperName(), ImGui.IsWindowFocused());
          Option<(Vector2, Vector2)> screenRect3 = uncastTarget.GetScreenRect();
          if (!screenRect3.IsSome())
            return;
          DevToolEntity.DrawBoundingBox(screenRect3.Unwrap(), uncastTarget.GetDebugName(), ImGui.IsWindowFocused());
        }
      }
    }
  }

  public static void DrawKPrefabIdCollection(string name, IEnumerable<KPrefabID> kprefabIds)
  {
    name += kprefabIds.IsNullOrDestroyed() ? " (0)" : $" ({kprefabIds.Count<KPrefabID>()})";
    if (!ImGui.CollapsingHeader(name))
      return;
    if (kprefabIds.IsNullOrDestroyed())
      ImGui.Text("List is null");
    else if (kprefabIds.Count<KPrefabID>() == 0)
    {
      ImGui.Text("List is empty");
    }
    else
    {
      foreach (KPrefabID kprefabId in kprefabIds)
      {
        ImGui.Text(kprefabId.ToString());
        ImGui.SameLine();
        if (ImGui.Button($"DevTool Inspect###ID_Inspect_{kprefabId.GetInstanceID()}"))
          DevToolSceneInspector.Inspect((object) kprefabId);
      }
    }
  }

  public static Option<string> GetErrorForCandidateTarget(DevToolEntityTarget uncastTarget)
  {
    if (!(uncastTarget is DevToolEntityTarget.ForSimCell))
      return (Option<string>) "Target must be a sim cell";
    DevToolEntityTarget.ForSimCell forSimCell = (DevToolEntityTarget.ForSimCell) uncastTarget;
    if (Game.Instance.IsNullOrDestroyed())
      return (Option<string>) "No Game instance found.";
    if (forSimCell.cellIndex < 0 || Grid.CellCount <= forSimCell.cellIndex)
      return (Option<string>) $"Found cell index {forSimCell.cellIndex} is out of range {forSimCell.cellIndex}..{Grid.CellCount}";
    return !Grid.IsValidCell(forSimCell.cellIndex) ? (Option<string>) $"Cell index {forSimCell.cellIndex} is invalid" : (Option<string>) Option.None;
  }
}
