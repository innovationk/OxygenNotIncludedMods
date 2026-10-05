// Decompiled with JetBrains decompiler
// Type: DevToolEntity_DebugGoTo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System;
using UnityEngine;

#nullable disable
public class DevToolEntity_DebugGoTo : DevTool
{
  private Option<DevToolEntityTarget.ForWorldGameObject> targetOpt;
  private Option<DevToolEntityTarget.ForSimCell> destinationSimCellTarget;
  private bool shouldDrawBoundingBox = true;
  private bool shouldContinouslyRequest;

  public DevToolEntity_DebugGoTo()
    : this((Option<DevToolEntityTarget.ForWorldGameObject>) Option.None)
  {
  }

  public DevToolEntity_DebugGoTo(
    Option<DevToolEntityTarget.ForWorldGameObject> target)
  {
    this.targetOpt = target;
  }

  protected override void RenderTo(DevPanel panel)
  {
    if (ImGui.BeginMenuBar())
    {
      if (ImGui.MenuItem("Eyedrop New Target"))
        panel.PushDevTool((DevTool) new DevToolEntity_EyeDrop((Action<DevToolEntityTarget>) (target => this.targetOpt = (Option<DevToolEntityTarget.ForWorldGameObject>) (DevToolEntityTarget.ForWorldGameObject) target), new Func<DevToolEntityTarget, Option<string>>(DevToolEntity_DebugGoTo.GetErrorForCandidateTarget)));
      ImGui.EndMenuBar();
    }
    this.Name = "Debug Go To";
    if (this.targetOpt.IsNone())
    {
      ImGui.TextWrapped("No Target selected");
    }
    else
    {
      DevToolEntityTarget.ForWorldGameObject uncastTarget1 = this.targetOpt.Unwrap();
      Option<string> forCandidateTarget = DevToolEntity_DebugGoTo.GetErrorForCandidateTarget((DevToolEntityTarget) uncastTarget1);
      if (forCandidateTarget.IsSome())
      {
        ImGui.TextWrapped(forCandidateTarget.Unwrap());
      }
      else
      {
        this.Name = "Debug Go To for: " + DevToolEntity.GetNameFor(uncastTarget1.gameObject);
        if (uncastTarget1.gameObject.IsNullOrDestroyed())
        {
          ImGui.TextWrapped("Target GameObject is null");
        }
        else
        {
          ImGui.Checkbox("Draw Bounding Box", ref this.shouldDrawBoundingBox);
          ImGuiEx.SimpleField("Target GameObject", DevToolEntity.GetNameFor(uncastTarget1.gameObject));
          ImGuiEx.SimpleField("Destination Cell Index", GetCellName(this.destinationSimCellTarget));
          if (ImGui.Button("Select New Destination Cell"))
            panel.PushDevTool((DevTool) new DevToolEntity_EyeDrop((Action<DevToolEntityTarget>) (target => this.destinationSimCellTarget = (Option<DevToolEntityTarget.ForSimCell>) (DevToolEntityTarget.ForSimCell) target), (Func<DevToolEntityTarget, Option<string>>) (uncastTarget => !(uncastTarget is DevToolEntityTarget.ForSimCell) ? (Option<string>) "Target is not a sim cell" : (Option<string>) Option.None)));
          ImGui.Separator();
          ImGui.Checkbox("Should Continously Request", ref this.shouldContinouslyRequest);
          if (ImGuiEx.Button("Request Target go to Destination", this.shouldContinouslyRequest ? "Disable continous requests" : (this.destinationSimCellTarget.IsNone() ? "No destination target." : (string) null)) || this.shouldContinouslyRequest && this.destinationSimCellTarget.IsSome())
          {
            DebugGoToMonitor.Instance smi1 = uncastTarget1.gameObject.GetSMI<DebugGoToMonitor.Instance>();
            CreatureDebugGoToMonitor.Instance smi2 = uncastTarget1.gameObject.GetSMI<CreatureDebugGoToMonitor.Instance>();
            if (!smi1.IsNullOrDestroyed())
              smi1.GoToCell(this.destinationSimCellTarget.Unwrap().cellIndex);
            else if (!smi2.IsNullOrDestroyed())
              smi2.GoToCell(this.destinationSimCellTarget.Unwrap().cellIndex);
            else
              DebugUtil.DevLogError("No debug goto SMI found");
          }
          if (!this.shouldDrawBoundingBox)
            return;
          Option<(Vector2, Vector2)> screenRect1 = uncastTarget1.GetScreenRect();
          if (screenRect1.IsSome())
            DevToolEntity.DrawBoundingBox(screenRect1.Unwrap(), "[Target]", ImGui.IsWindowFocused());
          if (!this.destinationSimCellTarget.IsSome())
            return;
          Option<(Vector2, Vector2)> screenRect2 = this.destinationSimCellTarget.Unwrap().GetScreenRect();
          if (!screenRect2.IsSome())
            return;
          DevToolEntity.DrawBoundingBox(screenRect2.Unwrap(), "[Destination]", ImGui.IsWindowFocused());
        }
      }
    }

    static string GetCellName(Option<DevToolEntityTarget.ForSimCell> target)
    {
      return !target.IsNone() ? target.Unwrap().cellIndex.ToString() : "<None>";
    }
  }

  public static Option<string> GetErrorForCandidateTarget(DevToolEntityTarget uncastTarget)
  {
    if (!(uncastTarget is DevToolEntityTarget.ForWorldGameObject))
      return (Option<string>) "Target must be a world GameObject";
    DevToolEntityTarget.ForWorldGameObject forWorldGameObject = (DevToolEntityTarget.ForWorldGameObject) uncastTarget;
    if (forWorldGameObject.gameObject.IsNullOrDestroyed())
      return (Option<string>) "Target GameObject is null or destroyed";
    return forWorldGameObject.gameObject.GetSMI<DebugGoToMonitor.Instance>().IsNullOrDestroyed() && forWorldGameObject.gameObject.GetSMI<CreatureDebugGoToMonitor.Instance>().IsNullOrDestroyed() ? (Option<string>) "Target GameObject doesn't have either a DebugGoToMonitor or CreatureDebugGoToMonitor" : (Option<string>) Option.None;
  }
}
