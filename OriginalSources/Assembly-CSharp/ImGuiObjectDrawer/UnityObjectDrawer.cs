// Decompiled with JetBrains decompiler
// Type: ImGuiObjectDrawer.UnityObjectDrawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using UnityEngine;

#nullable disable
namespace ImGuiObjectDrawer;

public class UnityObjectDrawer : PlainCSharpObjectDrawer
{
  public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
  {
    return member.value is Object;
  }

  protected override void DrawCustom(
    in MemberDrawContext context,
    in MemberDetails member,
    int depth)
  {
    Object @object = (Object) member.value;
    ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.None;
    if (context.default_open && depth <= 0)
      flags |= ImGuiTreeNodeFlags.DefaultOpen;
    int num = ImGui.TreeNodeEx(member.name, flags) ? 1 : 0;
    DrawerUtil.Tooltip(member.type);
    if (num == 0)
      return;
    this.DrawContents(in context, in member, depth);
    ImGui.TreePop();
  }
}
