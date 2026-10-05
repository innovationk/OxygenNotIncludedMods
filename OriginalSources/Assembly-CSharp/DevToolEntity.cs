// Decompiled with JetBrains decompiler
// Type: DevToolEntity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class DevToolEntity : DevTool
{
  private Option<DevToolEntityTarget> currentTargetOpt;
  private bool shouldDrawBoundingBox = true;

  protected override void RenderTo(DevPanel panel)
  {
    if (ImGui.BeginMenuBar())
    {
      if (ImGui.MenuItem("New Window"))
        DevToolUtil.Open((DevTool) new DevToolEntity());
      ImGui.EndMenuBar();
    }
    ImGui.Text(this.currentTargetOpt.IsNone() ? "Pick target:" : "Change target:");
    ImGui.SameLine();
    if (ImGui.Button("Eyedrop"))
      panel.PushDevTool((DevTool) new DevToolEntity_EyeDrop((Action<DevToolEntityTarget>) (result => this.currentTargetOpt = (Option<DevToolEntityTarget>) result)));
    ImGui.SameLine();
    if (ImGui.Button("Search GameObjects (NOT implemented)"))
      panel.PushDevTool((DevTool) new DevToolEntity_SearchGameObjects((Action<DevToolEntityTarget>) (result => this.currentTargetOpt = (Option<DevToolEntityTarget>) result)));
    if (this.GetInGameSelectedEntity().IsSome())
    {
      ImGui.SameLine();
      if (ImGui.Button($"\"{this.GetInGameSelectedEntity().Unwrap().name}\""))
        this.currentTargetOpt = (Option<DevToolEntityTarget>) (DevToolEntityTarget) new DevToolEntityTarget.ForWorldGameObject(this.GetInGameSelectedEntity().Unwrap());
    }
    ImGui.Separator();
    ImGui.Spacing();
    if (this.currentTargetOpt.IsNone())
    {
      this.Name = "Entity";
      ImGui.Text("<nothing selected>");
    }
    else
    {
      this.Name = "Entity: " + this.currentTargetOpt.Unwrap().ToString();
      this.Name = "EntityType: " + this.currentTargetOpt.Unwrap().GetType().FullName.Substring("For".Length);
      ImGuiEx.SimpleField("Entity Name", this.currentTargetOpt.Unwrap().ToString());
    }
    ImGui.Spacing();
    ImGui.Separator();
    ImGui.Spacing();
    if (this.currentTargetOpt.IsNone())
      return;
    DevToolEntityTarget toolEntityTarget = this.currentTargetOpt.Unwrap();
    Option<GameObject> option;
    switch (toolEntityTarget)
    {
      case DevToolEntityTarget.ForUIGameObject forUiGameObject:
        option = (Option<GameObject>) forUiGameObject.gameObject;
        break;
      case DevToolEntityTarget.ForWorldGameObject forWorldGameObject:
        option = (Option<GameObject>) forWorldGameObject.gameObject;
        break;
      default:
        option = (Option<GameObject>) Option.None;
        break;
    }
    if (ImGui.CollapsingHeader("Actions", ImGuiTreeNodeFlags.DefaultOpen))
    {
      ImGui.Indent();
      ImGui.Checkbox("Draw Bounding Box", ref this.shouldDrawBoundingBox);
      if (option.IsSome())
      {
        GameObject go = option.Unwrap();
        if (ImGui.Button($"Inspect GameObject in DevTools###ID_InspectInGame_{go.GetInstanceID()}"))
          DevToolSceneInspector.Inspect((object) go);
        JoyBehaviourMonitor.Instance smi1 = go.GetSMI<JoyBehaviourMonitor.Instance>();
        if (smi1.IsNullOrDestroyed())
          ImGuiEx.Button("Duplicant: Make Overjoyed", "No JoyBehaviourMonitor.Instance found on the selected GameObject");
        else if (ImGui.Button("Duplicant: Make Overjoyed"))
          smi1.GoToOverjoyed();
        WildnessMonitor.Instance smi2 = go.GetSMI<WildnessMonitor.Instance>();
        if (smi2.IsNullOrDestroyed())
        {
          ImGuiEx.Button("Taming: Covert to Tamed", "No WildnessMonitor.Instance found on the selected GameObject");
        }
        else
        {
          WildnessMonitor stateMachine = (WildnessMonitor) smi2.GetStateMachine();
          if (smi2.GetCurrentState() != stateMachine.tame)
          {
            if (ImGui.Button("Taming: Convert to Tamed"))
            {
              double num = (double) smi2.wildness.SetValue(0.0f);
              smi2.GoTo((StateMachine.BaseState) stateMachine.tame);
            }
          }
          else if (ImGui.Button("Taming: Convert to Untamed"))
          {
            smi2.wildness.value = smi2.wildness.GetMax();
            smi2.GoTo((StateMachine.BaseState) stateMachine.wild);
          }
        }
      }
      ImGui.Unindent();
    }
    ImGui.Spacing();
    if (ImGui.CollapsingHeader("Related DevTools", ImGuiTreeNodeFlags.DefaultOpen))
    {
      ImGui.Indent();
      if (ImGuiEx.Button("Debug Status Items", DevToolStatusItems.GetErrorForCandidateTarget(toolEntityTarget).UnwrapOrDefault()))
        panel.PushDevTool((DevTool) new DevToolStatusItems((Option<DevToolEntityTarget.ForWorldGameObject>) (DevToolEntityTarget.ForWorldGameObject) toolEntityTarget));
      Option<string> forCandidateTarget = DevToolCavity.GetErrorForCandidateTarget(toolEntityTarget);
      if (ImGuiEx.Button("Debug Cavity", forCandidateTarget.UnwrapOrDefault()))
        panel.PushDevTool((DevTool) new DevToolCavity((Option<DevToolEntityTarget.ForSimCell>) (DevToolEntityTarget.ForSimCell) toolEntityTarget));
      forCandidateTarget = DevToolEntity_DebugGoTo.GetErrorForCandidateTarget(toolEntityTarget);
      if (ImGuiEx.Button("Debug GoTo", forCandidateTarget.UnwrapOrDefault()))
        panel.PushDevTool((DevTool) new DevToolEntity_DebugGoTo((Option<DevToolEntityTarget.ForWorldGameObject>) (DevToolEntityTarget.ForWorldGameObject) toolEntityTarget));
      forCandidateTarget = DevToolEntity_RanchStation.GetErrorForCandidateTarget(toolEntityTarget);
      if (ImGuiEx.Button("Debug RanchStation", forCandidateTarget.UnwrapOrDefault()))
        panel.PushDevTool((DevTool) new DevToolEntity_RanchStation((Option<DevToolEntityTarget.ForWorldGameObject>) (DevToolEntityTarget.ForWorldGameObject) toolEntityTarget));
      ImGui.Unindent();
    }
    if (!this.shouldDrawBoundingBox)
      return;
    Option<(Vector2, Vector2)> screenRect = toolEntityTarget.GetScreenRect();
    if (!screenRect.IsSome())
      return;
    DevToolEntity.DrawBoundingBox(screenRect.Unwrap(), toolEntityTarget.GetDebugName(), ImGui.IsWindowFocused());
  }

  public Option<GameObject> GetInGameSelectedEntity()
  {
    if ((UnityEngine.Object) SelectTool.Instance == (UnityEngine.Object) null)
      return (Option<GameObject>) Option.None;
    KSelectable selected = SelectTool.Instance.selected;
    return selected.IsNullOrDestroyed() ? (Option<GameObject>) Option.None : (Option<GameObject>) selected.gameObject;
  }

  public static string GetNameFor(GameObject gameObject)
  {
    if (gameObject.IsNullOrDestroyed())
      return "<null or destroyed GameObject>";
    return $"\"{UI.StripLinkFormatting(gameObject.name)}\" [0x{gameObject.GetInstanceID().ToString("X")}]";
  }

  public static Vector2 GetPositionFor(GameObject gameObject)
  {
    if (!((UnityEngine.Object) Camera.main != (UnityEngine.Object) null))
      return Vector2.zero;
    Camera main = Camera.main;
    Vector2 screenPoint = (Vector2) main.WorldToScreenPoint(gameObject.transform.position);
    screenPoint.y = (float) main.pixelHeight - screenPoint.y;
    return screenPoint;
  }

  public static Vector2 GetScreenPosition(Vector3 pos)
  {
    if (!((UnityEngine.Object) Camera.main != (UnityEngine.Object) null))
      return Vector2.zero;
    Camera main = Camera.main;
    Vector2 screenPoint = (Vector2) main.WorldToScreenPoint(pos);
    screenPoint.y = (float) main.pixelHeight - screenPoint.y;
    return screenPoint;
  }

  public static void DrawBoundingBox(
    (Vector2 cornerA, Vector2 cornerB) screenRect,
    string name,
    bool isFocused)
  {
    if (isFocused)
      DevToolEntity.DrawScreenRect(screenRect, (Option<string>) name, (Option<Color>) new Color(1f, 0.0f, 0.0f, 1f), (Option<Color>) new Color(1f, 0.0f, 0.0f, 0.3f));
    else
      DevToolEntity.DrawScreenRect(screenRect, (Option<string>) Option.None, (Option<Color>) new Color(0.9f, 0.0f, 0.0f, 0.6f));
  }

  public static void DrawScreenRect(
    (Vector2 cornerA, Vector2 cornerB) screenRect,
    Option<string> text = default (Option<string>),
    Option<Color> outlineColor = default (Option<Color>),
    Option<Color> fillColor = default (Option<Color>),
    Option<DevToolUtil.TextAlignment> alignment = default (Option<DevToolUtil.TextAlignment>))
  {
    Vector2 p_min = Vector2.Min(screenRect.cornerA, screenRect.cornerB);
    Vector2 p_max = Vector2.Max(screenRect.cornerA, screenRect.cornerB);
    ImGui.GetBackgroundDrawList().AddRect(p_min, p_max, ImGui.GetColorU32((Vector4) outlineColor.UnwrapOr(Color.red)), 0.0f, ImDrawFlags.None, 4f);
    ImGui.GetBackgroundDrawList().AddRectFilled(p_min, p_max, ImGui.GetColorU32((Vector4) fillColor.UnwrapOr(Color.clear)));
    float font_size = 30f;
    if (!text.IsSome())
      return;
    Vector2 pos = new Vector2(p_max.x, p_min.y) + new Vector2(15f, 0.0f);
    if (alignment.HasValue)
    {
      font_size = ImGui.GetFont().FontSize;
      Vector2 vector2_1 = ImGui.CalcTextSize(text.Unwrap());
      if (alignment == DevToolUtil.TextAlignment.Center)
      {
        Vector2 vector2_2 = p_max - p_min;
        pos.x = p_min.x + (float) (((double) vector2_2.x - (double) vector2_1.x) * 0.5);
        pos.y = p_min.y + (float) (((double) vector2_2.y - (double) vector2_1.y) * 0.5);
      }
    }
    ImGui.GetBackgroundDrawList().AddText(ImGui.GetFont(), font_size, pos, ImGui.GetColorU32((Vector4) Color.white), text.Unwrap());
  }
}
