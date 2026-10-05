// Decompiled with JetBrains decompiler
// Type: ImGuiObjectDrawer.CollectionDrawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System;

#nullable disable
namespace ImGuiObjectDrawer;

public abstract class CollectionDrawer : MemberDrawer
{
  public abstract bool IsEmpty(in MemberDrawContext context, in MemberDetails member);

  public override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
  {
    return this.IsEmpty(in context, in member) ? MemberDrawType.Inline : MemberDrawType.Custom;
  }

  protected sealed override void DrawInline(in MemberDrawContext context, in MemberDetails member)
  {
    Debug.Assert(this.IsEmpty(in context, in member));
    this.DrawEmpty(in context, in member);
  }

  protected sealed override void DrawCustom(
    in MemberDrawContext context,
    in MemberDetails member,
    int depth)
  {
    Debug.Assert(!this.IsEmpty(in context, in member));
    this.DrawWithContents(in context, in member, depth);
  }

  private void DrawEmpty(in MemberDrawContext context, in MemberDetails member)
  {
    ImGui.Text(member.name + "(empty)");
  }

  private void DrawWithContents(in MemberDrawContext context, in MemberDetails member, int depth)
  {
    ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.None;
    if (context.default_open && depth <= 0)
      flags |= ImGuiTreeNodeFlags.DefaultOpen;
    int num = ImGui.TreeNodeEx(member.name, flags) ? 1 : 0;
    DrawerUtil.Tooltip(member.type);
    if (num == 0)
      return;
    this.VisitElements(new CollectionDrawer.ElementVisitor(Visitor), in context, in member);
    ImGui.TreePop();

    void Visitor(in MemberDrawContext context, CollectionDrawer.Element element)
    {
      int num = ImGui.TreeNode(element.node_name) ? 1 : 0;
      element.draw_tooltip();
      if (num == 0)
        return;
      DrawerUtil.DrawObjectContents(element.get_object_to_inspect(), in context, depth + 1);
      ImGui.TreePop();
    }
  }

  protected abstract void VisitElements(
    CollectionDrawer.ElementVisitor visit,
    in MemberDrawContext context,
    in MemberDetails member);

  protected delegate void ElementVisitor(
    in MemberDrawContext context,
    CollectionDrawer.Element element);

  protected struct Element(
    string node_name,
    System.Action draw_tooltip,
    Func<object> get_object_to_inspect)
  {
    public readonly string node_name = node_name;
    public readonly System.Action draw_tooltip = draw_tooltip;
    public readonly Func<object> get_object_to_inspect = get_object_to_inspect;

    public Element(int index, System.Action draw_tooltip, Func<object> get_object_to_inspect)
      : this($"[{index}]", draw_tooltip, get_object_to_inspect)
    {
    }
  }
}
