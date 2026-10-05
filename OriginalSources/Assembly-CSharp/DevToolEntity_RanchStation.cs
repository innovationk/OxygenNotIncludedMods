// Decompiled with JetBrains decompiler
// Type: DevToolEntity_RanchStation
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
public class DevToolEntity_RanchStation : DevTool
{
  private Option<DevToolEntityTarget.ForWorldGameObject> targetOpt;
  private bool shouldDrawBoundingBox = true;

  public DevToolEntity_RanchStation()
    : this((Option<DevToolEntityTarget.ForWorldGameObject>) Option.None)
  {
  }

  public DevToolEntity_RanchStation(
    Option<DevToolEntityTarget.ForWorldGameObject> target)
  {
    this.targetOpt = target;
  }

  protected override void RenderTo(DevPanel panel)
  {
    if (ImGui.BeginMenuBar())
    {
      if (ImGui.MenuItem("Eyedrop New Target"))
        panel.PushDevTool((DevTool) new DevToolEntity_EyeDrop((Action<DevToolEntityTarget>) (target => this.targetOpt = (Option<DevToolEntityTarget.ForWorldGameObject>) (DevToolEntityTarget.ForWorldGameObject) target), new Func<DevToolEntityTarget, Option<string>>(DevToolEntity_RanchStation.GetErrorForCandidateTarget)));
      ImGui.EndMenuBar();
    }
    this.Name = "RanchStation debug";
    if (this.targetOpt.IsNone())
    {
      ImGui.TextWrapped("No Target selected");
    }
    else
    {
      DevToolEntityTarget.ForWorldGameObject uncastTarget = this.targetOpt.Unwrap();
      Option<string> forCandidateTarget = DevToolEntity_RanchStation.GetErrorForCandidateTarget((DevToolEntityTarget) uncastTarget);
      if (forCandidateTarget.IsSome())
      {
        ImGui.TextWrapped(forCandidateTarget.Unwrap());
      }
      else
      {
        this.Name = "RanchStation debug for: " + DevToolEntity.GetNameFor(uncastTarget.gameObject);
        RanchStation.Instance smi = uncastTarget.gameObject.GetSMI<RanchStation.Instance>();
        RanchStation.Def def = uncastTarget.gameObject.GetDef<RanchStation.Def>();
        StateMachine stateMachine = smi.GetStateMachine();
        DevToolEntity_RanchStation.DrawRanchableCollection("Target Ranchables", (IEnumerable<RanchableMonitor.Instance>) smi.DEBUG_GetTargetRanchables());
        if (ImGui.CollapsingHeader("Full Debug Info"))
        {
          ImGuiEx.DrawObject("State Machine Instance", (object) smi, new MemberDrawContext?(new MemberDrawContext(false, false)));
          ImGuiEx.DrawObject("State Machine Def", (object) def, new MemberDrawContext?(new MemberDrawContext(false, false)));
          ImGuiEx.DrawObject("State Machine", (object) stateMachine, new MemberDrawContext?(new MemberDrawContext(false, false)));
        }
        if (!this.shouldDrawBoundingBox)
          return;
        Option<(Vector2, Vector2)> screenRect1 = uncastTarget.GetScreenRect();
        if (screenRect1.IsSome())
          DevToolEntity.DrawBoundingBox(screenRect1.Unwrap(), "[Ranching Station]", ImGui.IsWindowFocused());
        List<RanchableMonitor.Instance> targetRanchables = smi.DEBUG_GetTargetRanchables();
        for (int index = 0; index < targetRanchables.Count; ++index)
        {
          RanchableMonitor.Instance instance = targetRanchables[index];
          if (!instance.gameObject.IsNullOrDestroyed())
          {
            Option<(Vector2, Vector2)> screenRect2 = new DevToolEntityTarget.ForWorldGameObject(instance.gameObject).GetScreenRect();
            if (screenRect2.IsSome())
              DevToolEntity.DrawBoundingBox(screenRect2.Unwrap(), $"[Target Ranchable @ Index {index}]", ImGui.IsWindowFocused());
          }
        }
      }
    }
  }

  public static void DrawRanchableCollection(
    string name,
    IEnumerable<RanchableMonitor.Instance> ranchables)
  {
    if (!ImGui.CollapsingHeader(name))
      return;
    if (ranchables.IsNullOrDestroyed())
      ImGui.Text("List is null");
    else if (ranchables.Count<RanchableMonitor.Instance>() == 0)
    {
      ImGui.Text("List is empty");
    }
    else
    {
      int num = 0;
      foreach (RanchableMonitor.Instance ranchable in ranchables)
      {
        ImGui.Text(ranchable.IsNullOrDestroyed() ? "<null RanchableMonitor>" : DevToolEntity.GetNameFor(ranchable.gameObject));
        ImGui.SameLine();
        if (ImGui.Button($"DevTool Inspect###ID_Inspect_{num}"))
          DevToolSceneInspector.Inspect((object) ranchable);
        ++num;
      }
    }
  }

  public static Option<string> GetErrorForCandidateTarget(DevToolEntityTarget uncastTarget)
  {
    if (!(uncastTarget is DevToolEntityTarget.ForWorldGameObject))
      return (Option<string>) "Target must be a world GameObject";
    DevToolEntityTarget.ForWorldGameObject forWorldGameObject = (DevToolEntityTarget.ForWorldGameObject) uncastTarget;
    if (forWorldGameObject.gameObject.IsNullOrDestroyed())
      return (Option<string>) "Target GameObject is null or destroyed";
    return forWorldGameObject.gameObject.GetDef<RanchStation.Def>().IsNullOrDestroyed() ? (Option<string>) "Target GameObject doesn't have a RanchStation.Def" : (Option<string>) Option.None;
  }
}
