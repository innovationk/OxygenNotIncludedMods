// Decompiled with JetBrains decompiler
// Type: DevToolSaveGameInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using UnityEngine;

#nullable disable
public class DevToolSaveGameInfo : DevTool
{
  private string clSearch = "";

  protected override void RenderTo(DevPanel panel)
  {
    if ((Object) Game.Instance == (Object) null)
    {
      ImGui.Text("No game loaded");
    }
    else
    {
      ImGui.Text("Seed: " + CustomGameSettings.Instance.GetSettingsCoordinate());
      ImGui.Text("Generated: " + Game.Instance.dateGenerated);
      ImGui.Text("DebugWasUsed: " + Game.Instance.debugWasUsed.ToString());
      ImGui.Text("Content Enabled: ");
      foreach (string dlcId in SaveLoader.Instance.GameInfo.dlcIds)
        ImGui.Text(" - " + (dlcId == "" ? "VANILLA_ID" : dlcId));
      ImGui.PushItemWidth(100f);
      ImGui.NewLine();
      ImGui.Text("Changelists played on");
      ImGui.InputText("Search", ref this.clSearch, 10U);
      ImGui.PopItemWidth();
      foreach (uint num in Game.Instance.changelistsPlayedOn)
      {
        if (this.clSearch.IsNullOrWhiteSpace() || num.ToString().Contains(this.clSearch))
          ImGui.Text(num.ToString());
      }
      ImGui.NewLine();
    }
  }
}
