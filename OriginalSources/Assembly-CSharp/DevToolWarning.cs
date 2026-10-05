// Decompiled with JetBrains decompiler
// Type: DevToolWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using STRINGS;
using UnityEngine;

#nullable disable
public class DevToolWarning
{
  private bool showAgain;
  public string Name;
  public bool ShouldDrawWindow;

  public DevToolWarning() => this.Name = (string) UI.FRONTEND.DEVTOOLS.TITLE;

  public void DrawMenuBar()
  {
    if (!ImGui.BeginMainMenuBar())
      return;
    ImGui.Checkbox(this.Name, ref this.ShouldDrawWindow);
    ImGui.EndMainMenuBar();
  }

  public void DrawWindow(out bool isOpen)
  {
    ImGuiWindowFlags flags = ImGuiWindowFlags.None;
    isOpen = true;
    if (!ImGui.Begin(this.Name + "###ID_DevToolWarning", ref isOpen, flags))
      return;
    if (!isOpen)
    {
      ImGui.End();
    }
    else
    {
      ImGui.SetWindowSize(new Vector2(500f, 250f));
      ImGui.TextWrapped((string) UI.FRONTEND.DEVTOOLS.WARNING);
      ImGui.Spacing();
      ImGui.Spacing();
      ImGui.Spacing();
      ImGui.Spacing();
      ImGui.Checkbox((string) UI.FRONTEND.DEVTOOLS.DONTSHOW, ref this.showAgain);
      if (ImGui.Button((string) UI.FRONTEND.DEVTOOLS.BUTTON))
      {
        if (this.showAgain)
          KPlayerPrefs.SetInt("ShowDevtools", 1);
        DevToolManager.Instance.UserAcceptedWarning = true;
        isOpen = false;
      }
      ImGui.End();
    }
  }
}
