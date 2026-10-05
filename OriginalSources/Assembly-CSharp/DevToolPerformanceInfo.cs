// Decompiled with JetBrains decompiler
// Type: DevToolPerformanceInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DevToolPerformanceInfo : DevTool
{
  private PerformanceMonitor performanceMonitor;

  protected override void RenderTo(DevPanel panel)
  {
    if ((Object) Game.Instance == (Object) null)
    {
      ImGui.Text("No game loaded");
    }
    else
    {
      if ((Object) this.performanceMonitor == (Object) null)
        this.performanceMonitor = Global.Instance.GetComponent<PerformanceMonitor>();
      float fps = this.performanceMonitor.FPS;
      ImGui.Text($"{1000f / fps:0.00} ms ({fps:0.00} fps)");
      ImGui.NewLine();
      ImGui.Separator();
      if (ImGui.CollapsingHeader("Brains") && Game.BrainScheduler.debugGetBrainGroups() != null)
      {
        List<BrainScheduler.BrainGroup> brainGroups = Game.BrainScheduler.debugGetBrainGroups();
        for (int index = 0; index < brainGroups.Count; ++index)
        {
          ImGui.PushID(index);
          BrainScheduler.BrainGroup brainGroup = brainGroups[index];
          ImGui.Text(brainGroup.tag.ToString());
          ImGui.Indent();
          ImGui.Text("Brain count: " + brainGroup.BrainCount.ToString());
          ImGui.PushID(index);
          ImGui.Checkbox("Freeze AdjustLoad", ref brainGroup.debugFreezeLoadAdustment);
          ImGui.PopID();
          ImGui.SameLine();
          ImGui.Text("Max priority brain count seen: " + brainGroup.debugMaxPriorityBrainCountSeen.ToString());
          ImGui.SameLine();
          if (ImGui.Button("Reset"))
            brainGroup.debugMaxPriorityBrainCountSeen = 0;
          ImGui.PopID();
          ImGui.Unindent();
        }
      }
      if (!ImGui.CollapsingHeader("Camera Culling"))
        return;
      if ((Object) CameraController.Instance == (Object) null)
      {
        ImGui.Text("No camera instance");
      }
      else
      {
        GridVisibleArea visibleArea = CameraController.Instance.VisibleArea;
        ImGui.Checkbox("Freeze visible area", ref visibleArea.debugFreezeVisibleArea);
        ImGui.Checkbox("Freeze visible area extended", ref visibleArea.debugFreezeVisibleAreasExtended);
        Vector2I min1 = visibleArea.CurrentArea.Min;
        Vector2I max1 = visibleArea.CurrentArea.Max;
        Option<(Vector2, Vector2)> screenRect1 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(min1.x, min1.y)).GetScreenRect();
        Option<(Vector2, Vector2)> screenRect2 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(max1.x, max1.y)).GetScreenRect();
        if (screenRect1.IsSome() && screenRect2.IsSome())
          DevToolEntity.DrawScreenRect((Vector2.Min(screenRect1.Unwrap().Item1, Vector2.Min(screenRect1.Unwrap().Item2, Vector2.Min(screenRect2.Unwrap().Item1, screenRect2.Unwrap().Item2))), Vector2.Max(screenRect1.Unwrap().Item1, Vector2.Max(screenRect1.Unwrap().Item2, Vector2.Max(screenRect2.Unwrap().Item1, screenRect2.Unwrap().Item2)))), (Option<string>) "", new Option<Color>(Color.red));
        GridArea currentAreaExtended = visibleArea.CurrentAreaExtended;
        Vector2I min2 = currentAreaExtended.Min;
        currentAreaExtended = visibleArea.CurrentAreaExtended;
        Vector2I max2 = currentAreaExtended.Max;
        screenRect1 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(min2.x, min2.y)).GetScreenRect();
        screenRect2 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(max2.x, max2.y)).GetScreenRect();
        if (!screenRect1.IsSome() || !screenRect2.IsSome())
          return;
        DevToolEntity.DrawScreenRect((Vector2.Min(screenRect1.Unwrap().Item1, Vector2.Min(screenRect1.Unwrap().Item2, Vector2.Min(screenRect2.Unwrap().Item1, screenRect2.Unwrap().Item2))), Vector2.Max(screenRect1.Unwrap().Item1, Vector2.Max(screenRect1.Unwrap().Item2, Vector2.Max(screenRect2.Unwrap().Item1, screenRect2.Unwrap().Item2)))), (Option<string>) "", new Option<Color>(Color.cyan));
      }
    }
  }
}
