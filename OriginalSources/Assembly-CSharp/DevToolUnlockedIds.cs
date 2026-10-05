// Decompiled with JetBrains decompiler
// Type: DevToolUnlockedIds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class DevToolUnlockedIds : DevTool
{
  private string filterForUnlockIds = "";
  private string unlockIdToAdd = "";

  public DevToolUnlockedIds() => this.RequiresGameRunning = true;

  protected override void RenderTo(DevPanel panel)
  {
    (bool hasValue, DevToolUnlockedIds.UnlocksWrapper unlocksWrapper1) = this.GetUnlocks();
    int num1 = hasValue ? 1 : 0;
    DevToolUnlockedIds.UnlocksWrapper unlocksWrapper2 = unlocksWrapper1;
    if (num1 == 0)
    {
      ImGui.Text("Couldn't access global unlocks");
    }
    else
    {
      if (ImGui.TreeNode("Help"))
      {
        ImGui.TextWrapped("This is a list of global unlocks that are persistant across saves. Changes made here will be saved to disk immediately.");
        ImGui.Spacing();
        ImGui.TextWrapped("NOTE: It may be necessary to relaunch the game after modifying unlocks in order for systems to respond.");
        ImGui.TreePop();
      }
      ImGui.Spacing();
      ImGuiEx.InputFilter("Filter", ref this.filterForUnlockIds);
      if (!ImGui.BeginTable("ID_unlockIds", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.ScrollY))
        return;
      ImGui.TableSetupScrollFreeze(2, 2);
      ImGui.TableSetupColumn("Unlock ID");
      ImGui.TableSetupColumn("Actions", ImGuiTableColumnFlags.WidthFixed);
      ImGui.TableHeadersRow();
      ImGui.PushID("ID_row_add_new");
      ImGui.TableNextRow();
      ImGui.TableSetColumnIndex(0);
      ImGui.InputText("", ref this.unlockIdToAdd, 50U);
      ImGui.TableSetColumnIndex(1);
      if (ImGui.Button("Add"))
      {
        unlocksWrapper2.AddId(this.unlockIdToAdd);
        Debug.Log((object) ("[Added unlock id] " + this.unlockIdToAdd));
        this.unlockIdToAdd = "";
      }
      ImGui.PopID();
      int num2 = 0;
      foreach (string allId in unlocksWrapper2.GetAllIds())
      {
        string fmt = allId == null ? "<<null>>" : $"\"{allId}\"";
        if (fmt.ToLower().Contains(this.filterForUnlockIds.ToLower()))
        {
          ImGui.TableNextRow();
          ImGui.PushID($"ID_row_{num2++}");
          ImGui.TableSetColumnIndex(0);
          ImGui.Text(fmt);
          ImGui.TableSetColumnIndex(1);
          if (ImGui.Button("Copy"))
          {
            GUIUtility.systemCopyBuffer = allId;
            Debug.Log((object) ("[Copied to clipboard] " + allId));
          }
          ImGui.SameLine();
          if (ImGui.Button("Remove"))
          {
            unlocksWrapper2.RemoveId(allId);
            Debug.Log((object) ("[Removed unlock id] " + allId));
          }
          ImGui.PopID();
        }
      }
      ImGui.EndTable();
    }
  }

  private Option<DevToolUnlockedIds.UnlocksWrapper> GetUnlocks()
  {
    if (App.IsExiting)
      return (Option<DevToolUnlockedIds.UnlocksWrapper>) Option.None;
    if ((UnityEngine.Object) Game.Instance == (UnityEngine.Object) null || !(bool) (UnityEngine.Object) Game.Instance)
      return (Option<DevToolUnlockedIds.UnlocksWrapper>) Option.None;
    return (UnityEngine.Object) Game.Instance.unlocks == (UnityEngine.Object) null ? (Option<DevToolUnlockedIds.UnlocksWrapper>) Option.None : Option.Some<DevToolUnlockedIds.UnlocksWrapper>(new DevToolUnlockedIds.UnlocksWrapper(Game.Instance.unlocks));
  }

  public readonly struct UnlocksWrapper(Unlocks unlocks)
  {
    public readonly Unlocks unlocks = unlocks;

    public void AddId(string unlockId) => this.unlocks.Unlock(unlockId);

    public void RemoveId(string unlockId) => this.unlocks.Lock(unlockId);

    public IEnumerable<string> GetAllIds()
    {
      return (IEnumerable<string>) this.unlocks.GetAllUnlockedIds().OrderBy<string, string>((Func<string, string>) (s => s));
    }

    public int Count => this.unlocks.GetAllUnlockedIds().Count;
  }
}
