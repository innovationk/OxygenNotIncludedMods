// Decompiled with JetBrains decompiler
// Type: DevToolDebugModeToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;

#nullable disable
public class DevToolDebugModeToggle : DevTool
{
  public DevToolDebugModeToggle() => this.RequiresGameRunning = true;

  protected override void RenderTo(DevPanel panel)
  {
    bool instantBuildMode = DebugHandler.InstantBuildMode;
    if (!ImGui.Checkbox("Instant Build Mode (Ctrl+F4)", ref instantBuildMode))
      return;
    DebugHandler.ToggleInstantBuildMode();
  }
}
