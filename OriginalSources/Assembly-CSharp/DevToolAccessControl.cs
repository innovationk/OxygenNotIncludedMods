// Decompiled with JetBrains decompiler
// Type: DevToolAccessControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class DevToolAccessControl : DevTool
{
  public static DevToolAccessControl Instance;
  private bool initialized;
  private Dictionary<Tag, List<MinionAssignablesProxy>> minionsByType = new Dictionary<Tag, List<MinionAssignablesProxy>>();
  private AccessControl selectedAccessControl;
  private bool lockSelected;
  private List<Tag> robotTypes;

  public DevToolAccessControl() => DevToolAccessControl.Instance = this;

  private bool Init()
  {
    if ((UnityEngine.Object) Game.Instance == (UnityEngine.Object) null)
      return false;
    if (!this.initialized)
    {
      this.initialized = true;
      foreach (Tag key in Assets.GetPrefabsWithTag(GameTags.BaseMinion).Select<GameObject, Tag>((Func<GameObject, Tag>) (e => e.GetComponent<KPrefabID>().PrefabTag)).ToList<Tag>())
        this.minionsByType.Add(key, new List<MinionAssignablesProxy>());
      this.robotTypes = Assets.GetPrefabsWithTag(GameTags.Robots.Behaviours.HasDoorPermissions).Select<GameObject, Tag>((Func<GameObject, Tag>) (e => e.GetComponent<KPrefabID>().PrefabTag)).ToList<Tag>();
    }
    return true;
  }

  protected override void RenderTo(DevPanel panel)
  {
    if (this.Init())
    {
      if (this.DoorSelected())
      {
        ImGui.Checkbox("Lock Selection", ref this.lockSelected);
        this.MinionContents();
        this.RobotContents();
      }
      this.GridRestrictionSerializerContents();
    }
    else
      ImGui.Text("No Access Control selected");
  }

  private bool DoorSelected()
  {
    return (UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null && (UnityEngine.Object) this.SetDoorAccessControl() != (UnityEngine.Object) null;
  }

  private AccessControl SetDoorAccessControl()
  {
    AccessControl component = SelectTool.Instance.selected.GetComponent<AccessControl>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) this.selectedAccessControl && !this.lockSelected)
      this.selectedAccessControl = component;
    return this.selectedAccessControl;
  }

  private void MinionContents()
  {
    foreach (MinionAssignablesProxy assignablesProxy in Components.MinionAssignablesProxy.Items)
    {
      Tag minionModel = assignablesProxy.GetMinionModel();
      if (!this.minionsByType[minionModel].Contains(assignablesProxy))
        this.minionsByType[minionModel].Add(assignablesProxy);
    }
    foreach (Tag key in this.minionsByType.Keys)
    {
      ImGui.PushID(key.Name);
      ImGui.Text(key.Name);
      AccessControl.Permission setPermission1 = this.selectedAccessControl.GetSetPermission(Tag.Invalid.GetHashCode(), key);
      bool v1 = setPermission1 == AccessControl.Permission.GoLeft || setPermission1 == AccessControl.Permission.Both;
      bool v2 = setPermission1 == AccessControl.Permission.GoRight || setPermission1 == AccessControl.Permission.Both;
      ImGui.SameLine();
      if (ImGui.Checkbox("Left", ref v1))
        this.UpdateAccess(key, v1, v2);
      ImGui.SameLine();
      if (ImGui.Checkbox("Right", ref v2))
        this.UpdateAccess(key, v1, v2);
      ImGui.PopID();
      ImGui.Indent();
      ImGui.PushStyleVar(ImGuiStyleVar.Alpha, 0.75f);
      foreach (MinionAssignablesProxy assignablesProxy in this.minionsByType[key])
      {
        ImGui.PushID(assignablesProxy.TargetInstanceID);
        ImGui.Text(assignablesProxy.target.GetProperName());
        AccessControl.Permission setPermission2 = this.selectedAccessControl.GetSetPermission(assignablesProxy);
        ImGui.SameLine();
        bool v3 = setPermission2 == AccessControl.Permission.GoLeft || setPermission2 == AccessControl.Permission.Both;
        bool v4 = setPermission2 == AccessControl.Permission.GoRight || setPermission2 == AccessControl.Permission.Both;
        if (ImGui.Checkbox("Left", ref v3))
          this.UpdateMinionAccess(assignablesProxy, v3, v4);
        ImGui.SameLine();
        if (ImGui.Checkbox("Right", ref v4))
          this.UpdateMinionAccess(assignablesProxy, v3, v4);
        ImGui.PopID();
      }
      ImGui.PopStyleVar();
      ImGui.Unindent();
    }
  }

  private void RobotContents()
  {
    ImGui.PushID(GameTags.Robot.Name);
    ImGui.Text(GameTags.Robot.Name);
    AccessControl.Permission setPermission1 = this.selectedAccessControl.GetSetPermission(Tag.Invalid.GetHashCode(), GameTags.Robot);
    bool v1 = setPermission1 == AccessControl.Permission.GoLeft || setPermission1 == AccessControl.Permission.Both;
    bool v2 = setPermission1 == AccessControl.Permission.GoRight || setPermission1 == AccessControl.Permission.Both;
    ImGui.SameLine();
    if (ImGui.Checkbox("Left", ref v1))
      this.UpdateAccess(GameTags.Robot, v1, v2);
    ImGui.SameLine();
    if (ImGui.Checkbox("Right", ref v2))
      this.UpdateAccess(GameTags.Robot, v1, v2);
    ImGui.PopID();
    ImGui.Indent();
    ImGui.PushStyleVar(ImGuiStyleVar.Alpha, 0.75f);
    foreach (Tag robotType in this.robotTypes)
    {
      ImGui.PushID(robotType.Name);
      ImGui.Text(robotType.Name);
      AccessControl.Permission setPermission2 = this.selectedAccessControl.GetSetPermission(robotType);
      bool v3 = setPermission2 == AccessControl.Permission.GoLeft || setPermission2 == AccessControl.Permission.Both;
      bool v4 = setPermission2 == AccessControl.Permission.GoRight || setPermission2 == AccessControl.Permission.Both;
      ImGui.SameLine();
      if (ImGui.Checkbox("Left", ref v3))
        this.UpdateAccess(robotType, v3, v4);
      ImGui.SameLine();
      if (ImGui.Checkbox("Right", ref v4))
        this.UpdateAccess(robotType, v3, v4);
      ImGui.PopID();
    }
    ImGui.PopStyleVar();
    ImGui.Unindent();
  }

  private void GridRestrictionSerializerContents()
  {
  }

  private void UpdateMinionAccess(MinionAssignablesProxy proxy, bool left, bool right)
  {
    AccessControl.Permission permission = !left ? (!right ? AccessControl.Permission.Neither : AccessControl.Permission.GoRight) : (!right ? AccessControl.Permission.GoLeft : AccessControl.Permission.Both);
    this.selectedAccessControl.SetPermission(proxy, permission);
  }

  private void UpdateAccess(Tag id, bool left, bool right)
  {
    AccessControl.Permission permission = !left ? (!right ? AccessControl.Permission.Neither : AccessControl.Permission.GoRight) : (!right ? AccessControl.Permission.GoLeft : AccessControl.Permission.Both);
    this.selectedAccessControl.SetPermission(id, permission);
  }

  private struct MinionPermissions
  {
    public bool Left;
    public bool Right;
    public MinionAssignablesProxy Proxy;
  }
}
