// Decompiled with JetBrains decompiler
// Type: ImGuiObjectDrawer.HashedStringDrawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace ImGuiObjectDrawer;

public sealed class HashedStringDrawer : InlineDrawer
{
  public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
  {
    return member.value is HashedString;
  }

  protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
  {
    HashedString hashedString = (HashedString) member.value;
    string str1 = hashedString.ToString();
    string str2 = "0x" + hashedString.HashValue.ToString("X");
    ImGuiEx.SimpleField(member.name, $"{str1} ({str2})");
  }
}
