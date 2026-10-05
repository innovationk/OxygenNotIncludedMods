// Decompiled with JetBrains decompiler
// Type: DevToolAnimEventManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DevToolAnimEventManager : DevTool
{
  protected override void RenderTo(DevPanel panel)
  {
    (Option<AnimEventManager.DevTools_DebugInfo> value, string error) managerDebugInfo = this.GetAnimEventManagerDebugInfo();
    (bool hasValue, AnimEventManager.DevTools_DebugInfo devToolsDebugInfo1) = managerDebugInfo.value;
    int num = hasValue ? 1 : 0;
    AnimEventManager.DevTools_DebugInfo devToolsDebugInfo2 = devToolsDebugInfo1;
    string error = managerDebugInfo.error;
    if (num == 0)
    {
      ImGui.Text(error);
    }
    else
    {
      if (ImGui.CollapsingHeader("World space animations", ImGuiTreeNodeFlags.DefaultOpen))
        this.DrawFor("ID_world_space_anims", devToolsDebugInfo2.eventData.GetDataList(), devToolsDebugInfo2.animData.GetDataList());
      if (ImGui.CollapsingHeader("UI space animations", ImGuiTreeNodeFlags.DefaultOpen))
        this.DrawFor("ID_ui_space_anims", devToolsDebugInfo2.uiEventData.GetDataList(), devToolsDebugInfo2.uiAnimData.GetDataList());
      if (!ImGui.CollapsingHeader("Raw AnimEventManger", ImGuiTreeNodeFlags.DefaultOpen))
        return;
      ImGuiEx.DrawObject("Anim Event Manager", (object) devToolsDebugInfo2.eventManager);
    }
  }

  public void DrawFor(
    string uniqueTableId,
    List<AnimEventManager.EventPlayerData> eventDataList,
    List<AnimEventManager.AnimData> animDataList)
  {
    if (eventDataList == null)
      ImGui.Text("Can't draw table: eventData is null");
    else if (animDataList == null)
      ImGui.Text("Can't draw table: animData is null");
    else if (eventDataList.Count != animDataList.Count)
    {
      ImGui.Text($"Can't draw table: eventData.Count ({eventDataList.Count}) != animData.Count ({animDataList.Count})");
    }
    else
    {
      int count = eventDataList.Count;
      ImGui.PushID(uniqueTableId);
      ImGuiStoragePtr stateStorage = ImGui.GetStateStorage();
      uint id = ImGui.GetID("ID_should_expand_full_height");
      bool val = stateStorage.GetBool(id);
      if (ImGui.Button(val ? "Unexpand Height" : "Expand Height"))
      {
        val = !val;
        stateStorage.SetBool(id, val);
      }
      if (ImGui.BeginTable("ID_table_contents", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable | ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.ScrollY, new Vector2(-1f, val ? -1f : 400f)))
      {
        ImGui.TableSetupScrollFreeze(4, 1);
        ImGui.TableSetupColumn("Game Object Name");
        ImGui.TableSetupColumn("Event Frame");
        ImGui.TableSetupColumn("Animation Frame");
        ImGui.TableSetupColumn("Event - Animation Frame Diff");
        ImGui.TableHeadersRow();
        int num1;
        for (int index = 0; index < count; index = num1 + 1)
        {
          AnimEventManager.EventPlayerData eventData = eventDataList[index];
          AnimEventManager.AnimData animData = animDataList[index];
          ImGui.TableNextRow();
          int num2 = index;
          num1 = num2 + 1;
          ImGui.PushID($"ID_row_{num2}");
          ImGui.TableNextColumn();
          if (ImGuiEx.Button("Focus", DevToolUtil.CanRevealAndFocus(eventData.controller.gameObject)))
            DevToolUtil.RevealAndFocus(eventData.controller.gameObject);
          ImGuiEx.TooltipForPrevious("Will move the in-game camera to this gameobject");
          ImGui.SameLine();
          ImGui.Text(UI.StripLinkFormatting(eventData.controller.gameObject.name));
          ImGui.TableNextColumn();
          int num3 = eventData.currentFrame;
          ImGui.Text(num3.ToString());
          ImGui.TableNextColumn();
          num3 = eventData.controller.currentFrame;
          ImGui.Text(num3.ToString());
          ImGui.TableNextColumn();
          num3 = eventData.currentFrame - eventData.controller.currentFrame;
          ImGui.Text(num3.ToString());
          ImGui.PopID();
        }
        ImGui.EndTable();
      }
      ImGui.PopID();
    }
  }

  public (Option<AnimEventManager.DevTools_DebugInfo> value, string error) GetAnimEventManagerDebugInfo()
  {
    return Singleton<AnimEventManager>.Instance == null ? ((Option<AnimEventManager.DevTools_DebugInfo>) Option.None, "AnimEventManager is null") : (Option.Some<AnimEventManager.DevTools_DebugInfo>(Singleton<AnimEventManager>.Instance.DevTools_GetDebugInfo()), (string) null);
  }
}
