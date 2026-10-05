// Decompiled with JetBrains decompiler
// Type: ImGuiObjectDrawer.Vector2Drawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ImGuiObjectDrawer;

public sealed class Vector2Drawer : InlineDrawer
{
  public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
  {
    return member.value is Vector2;
  }

  protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
  {
    Vector2 vector2 = (Vector2) member.value;
    ImGuiEx.SimpleField(member.name, $"( {vector2.x}, {vector2.y} )");
  }
}
