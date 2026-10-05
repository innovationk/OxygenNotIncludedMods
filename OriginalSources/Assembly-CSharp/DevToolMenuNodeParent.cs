// Decompiled with JetBrains decompiler
// Type: DevToolMenuNodeParent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System.Collections.Generic;

#nullable disable
public class DevToolMenuNodeParent : IMenuNode
{
  public string name;
  public List<IMenuNode> children;

  public DevToolMenuNodeParent(string name)
  {
    this.name = name;
    this.children = new List<IMenuNode>();
  }

  public void AddChild(IMenuNode menuNode) => this.children.Add(menuNode);

  public string GetName() => this.name;

  public void Draw()
  {
    if (!ImGui.BeginMenu(this.name))
      return;
    foreach (IMenuNode child in this.children)
      child.Draw();
    ImGui.EndMenu();
  }
}
