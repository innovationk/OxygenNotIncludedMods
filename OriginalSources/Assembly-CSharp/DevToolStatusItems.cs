// Decompiled with JetBrains decompiler
// Type: DevToolStatusItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class DevToolStatusItems : DevTool
{
  private Option<DevToolEntityTarget.ForWorldGameObject> targetOpt;
  private ImGuiObjectTableDrawer<StatusItemGroup.Entry> tableDrawer;
  private StatusItemStackTraceWatcher statusItemStackTraceWatcher = new StatusItemStackTraceWatcher();
  private bool shouldDrawBoundingBox = true;

  public DevToolStatusItems()
    : this((Option<DevToolEntityTarget.ForWorldGameObject>) Option.None)
  {
  }

  public DevToolStatusItems(
    Option<DevToolEntityTarget.ForWorldGameObject> target)
  {
    this.targetOpt = target;
    this.tableDrawer = ImGuiObjectTableDrawer<StatusItemGroup.Entry>.New().RemoveFlags(ImGuiTableFlags.SizingFixedFit).AddFlags(ImGuiTableFlags.Resizable).Column("Text", (Func<StatusItemGroup.Entry, string>) (entry => entry.GetName())).Column("Id Name", (Func<StatusItemGroup.Entry, string>) (entry => entry.item.Id)).Column("Notification Type", (Func<StatusItemGroup.Entry, object>) (entry => (object) entry.item.notificationType)).Column("Category", (Func<StatusItemGroup.Entry, string>) (entry => entry.category?.Name ?? "<no category>")).Column("OnAdded Callstack", (Action<StatusItemGroup.Entry>) (entry =>
    {
      StackTrace stackTrace;
      if (this.statusItemStackTraceWatcher.GetStackTraceForEntry(entry, out stackTrace))
      {
        if (ImGui.Selectable("copy callstack"))
          ImGui.SetClipboardText(stackTrace.ToString());
        ImGuiEx.TooltipForPrevious(stackTrace.ToString());
      }
      else
        ImGui.Text("<None>");
    })).Build();
    this.OnUninit += (System.Action) (() => this.statusItemStackTraceWatcher.Dispose());
  }

  protected override void RenderTo(DevPanel panel)
  {
    this.statusItemStackTraceWatcher.SetTarget(this.targetOpt.AndThen<GameObject>((Func<DevToolEntityTarget.ForWorldGameObject, GameObject>) (t => t.gameObject)).AndThen<KSelectable>((Func<GameObject, KSelectable>) (go => go.GetComponent<KSelectable>())).AndThen<StatusItemGroup>((Func<KSelectable, StatusItemGroup>) (s => s.GetStatusItemGroup())));
    if (ImGui.BeginMenuBar())
    {
      if (ImGui.MenuItem("Eyedrop New Target"))
        panel.PushDevTool((DevTool) new DevToolEntity_EyeDrop((Action<DevToolEntityTarget>) (target => this.targetOpt = (Option<DevToolEntityTarget.ForWorldGameObject>) (DevToolEntityTarget.ForWorldGameObject) target), new Func<DevToolEntityTarget, Option<string>>(DevToolStatusItems.GetErrorForCandidateTarget)));
      string error = (string) null;
      if (this.targetOpt.IsNone())
      {
        error = "No target selected.";
      }
      else
      {
        Option<string> forCandidateTarget = DevToolStatusItems.GetErrorForCandidateTarget((DevToolEntityTarget) this.targetOpt.Unwrap());
        if (forCandidateTarget.IsSome())
          error = forCandidateTarget.Unwrap();
      }
      if (ImGuiEx.MenuItem("Debug Target", error))
        panel.PushValue<DevToolEntityTarget.ForWorldGameObject>(this.targetOpt.Unwrap());
      ImGui.EndMenuBar();
    }
    this.Name = "Status Items";
    if (this.targetOpt.IsNone())
    {
      ImGui.TextWrapped("No Target selected");
    }
    else
    {
      DevToolEntityTarget.ForWorldGameObject uncastTarget = this.targetOpt.Unwrap();
      Option<string> forCandidateTarget = DevToolStatusItems.GetErrorForCandidateTarget((DevToolEntityTarget) uncastTarget);
      if (forCandidateTarget.IsSome())
      {
        ImGui.TextWrapped(forCandidateTarget.Unwrap());
      }
      else
      {
        this.Name = "Status Items for: " + DevToolEntity.GetNameFor(uncastTarget.gameObject);
        bool shouldWatch = this.statusItemStackTraceWatcher.GetShouldWatch();
        if (ImGui.Checkbox("Should Track OnAdded Callstacks", ref shouldWatch))
          this.statusItemStackTraceWatcher.SetShouldWatch(shouldWatch);
        ImGui.Checkbox("Draw Bounding Box", ref this.shouldDrawBoundingBox);
        this.tableDrawer.Draw(uncastTarget.gameObject.GetComponent<KSelectable>().GetStatusItemGroup().GetEnumerator());
        if (!this.shouldDrawBoundingBox)
          return;
        Option<(Vector2, Vector2)> screenRect = uncastTarget.GetScreenRect();
        if (!screenRect.IsSome())
          return;
        DevToolEntity.DrawBoundingBox(screenRect.Unwrap(), uncastTarget.GetDebugName(), ImGui.IsWindowFocused());
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
    KSelectable component = forWorldGameObject.gameObject.GetComponent<KSelectable>();
    if (component.IsNullOrDestroyed())
      return (Option<string>) "Target GameObject doesn't have a KSelectable";
    return component.GetStatusItemGroup().IsNullOrDestroyed() ? (Option<string>) "Target GameObject doesn't have a StatusItemGroup" : (Option<string>) Option.None;
  }
}
