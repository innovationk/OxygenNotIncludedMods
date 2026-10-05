// Decompiled with JetBrains decompiler
// Type: DevToolDLCManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;

#nullable disable
public class DevToolDLCManager : DevTool
{
  protected override void RenderTo(DevPanel panel)
  {
    string name = DistributionPlatform.Inst.Name;
    if (!DistributionPlatform.Initialized)
    {
      ImGui.Text("Failed to initialize " + name);
    }
    else
    {
      ImGui.Text("Active content letters: " + DlcManager.GetActiveContentLetters());
      ImGui.Separator();
      foreach (string str in DlcManager.RELEASED_VERSIONS)
      {
        if (!str.IsNullOrWhiteSpace())
        {
          ImGui.Text(str);
          ImGui.SameLine();
          bool v1 = DlcManager.IsContentSubscribed(str);
          if (ImGui.Checkbox("Enabled ", ref v1))
            DlcManager.ToggleDLC(str);
          ImGui.SameLine();
          bool v2 = DistributionPlatform.Inst.IsDLCSubscribed(str);
          if (ImGui.Checkbox("Subscribed ", ref v2))
            DistributionPlatform.Inst.ToggleDLCSubscription(str);
        }
      }
    }
  }
}
