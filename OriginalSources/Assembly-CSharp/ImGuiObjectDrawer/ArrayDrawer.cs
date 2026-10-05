// Decompiled with JetBrains decompiler
// Type: ImGuiObjectDrawer.ArrayDrawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace ImGuiObjectDrawer;

public sealed class ArrayDrawer : CollectionDrawer
{
  public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
  {
    return member.type.IsArray;
  }

  public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
  {
    return ((Array) member.value).Length == 0;
  }

  protected override void VisitElements(
    CollectionDrawer.ElementVisitor visit,
    in MemberDrawContext context,
    in MemberDetails member)
  {
    Array array = (Array) member.value;
    for (int i = 0; i < array.Length; ++i)
      visit(in context, new CollectionDrawer.Element(i, (Action) (() => DrawerUtil.Tooltip(array.GetType().GetElementType())), (Func<object>) (() => (object) new
      {
        value = array.GetValue(i)
      })));
  }
}
