// Decompiled with JetBrains decompiler
// Type: DevToolMenuNodeList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System;

#nullable disable
public class DevToolMenuNodeList
{
  private DevToolMenuNodeParent root = new DevToolMenuNodeParent("<root>");

  public DevToolMenuNodeParent AddOrGetParentFor(string childPath)
  {
    string[] strArray = System.IO.Path.GetDirectoryName(childPath).Split('/', StringSplitOptions.None);
    string str1 = "";
    DevToolMenuNodeParent parentFor = this.root;
    foreach (string str2 in strArray)
    {
      string split = str2;
      str1 += parentFor.GetName();
      IMenuNode menuNode = parentFor.children.Find((Predicate<IMenuNode>) (x => x.GetName() == split));
      DevToolMenuNodeParent toolMenuNodeParent1;
      if (menuNode != null)
      {
        toolMenuNodeParent1 = menuNode is DevToolMenuNodeParent toolMenuNodeParent2 ? toolMenuNodeParent2 : throw new Exception("Conflict! Both a leaf and parent node exist at path: " + str1);
      }
      else
      {
        toolMenuNodeParent1 = new DevToolMenuNodeParent(split);
        parentFor.AddChild((IMenuNode) toolMenuNodeParent1);
      }
      parentFor = toolMenuNodeParent1;
    }
    return parentFor;
  }

  public DevToolMenuNodeAction AddAction(string path, System.Action onClickFn)
  {
    DevToolMenuNodeAction toolMenuNodeAction = new DevToolMenuNodeAction(System.IO.Path.GetFileName(path), onClickFn);
    this.AddOrGetParentFor(path).AddChild((IMenuNode) toolMenuNodeAction);
    return toolMenuNodeAction;
  }

  public void Draw()
  {
    foreach (IMenuNode child in this.root.children)
      child.Draw();
  }

  public void DrawFull()
  {
    if (!ImGui.BeginMainMenuBar())
      return;
    this.Draw();
    ImGui.EndMainMenuBar();
  }
}
