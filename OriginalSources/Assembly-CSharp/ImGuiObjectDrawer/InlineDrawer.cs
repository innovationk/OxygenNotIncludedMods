// Decompiled with JetBrains decompiler
// Type: ImGuiObjectDrawer.InlineDrawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace ImGuiObjectDrawer;

public abstract class InlineDrawer : MemberDrawer
{
  public sealed override MemberDrawType GetDrawType(
    in MemberDrawContext context,
    in MemberDetails member)
  {
    return MemberDrawType.Inline;
  }

  protected sealed override void DrawCustom(
    in MemberDrawContext context,
    in MemberDetails member,
    int depth)
  {
    this.DrawInline(in context, in member);
  }
}
