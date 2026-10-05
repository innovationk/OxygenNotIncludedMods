// Decompiled with JetBrains decompiler
// Type: DevToolEntity_SearchGameObjects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System;

#nullable disable
public class DevToolEntity_SearchGameObjects : DevTool
{
  private Action<DevToolEntityTarget> onSelectionMadeFn;

  public DevToolEntity_SearchGameObjects(Action<DevToolEntityTarget> onSelectionMadeFn)
  {
    this.onSelectionMadeFn = onSelectionMadeFn;
  }

  protected override void RenderTo(DevPanel panel) => ImGui.Text("Not implemented yet");
}
