// Decompiled with JetBrains decompiler
// Type: ImGuiObjectDrawer.LocStringDrawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace ImGuiObjectDrawer;

public sealed class LocStringDrawer : InlineDrawer
{
  public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
  {
    return member.CanAssignToType<LocString>();
  }

  protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
  {
    ImGuiEx.SimpleField(member.name, $"{member.value}({((LocString) member.value).text})");
  }
}
