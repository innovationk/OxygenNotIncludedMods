// Decompiled with JetBrains decompiler
// Type: ImGuiObjectDrawer.IDictionaryDrawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;

#nullable disable
namespace ImGuiObjectDrawer;

public sealed class IDictionaryDrawer : CollectionDrawer
{
  public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
  {
    return member.CanAssignToType<IDictionary>();
  }

  public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
  {
    return ((ICollection) member.value).Count == 0;
  }

  protected override void VisitElements(
    CollectionDrawer.ElementVisitor visit,
    in MemberDrawContext context,
    in MemberDetails member)
  {
    IDictionary dictionary = (IDictionary) member.value;
    int index = 0;
    foreach (DictionaryEntry dictionaryEntry in dictionary)
    {
      DictionaryEntry kvp = dictionaryEntry;
      visit(in context, new CollectionDrawer.Element(index, (Action) (() => DrawerUtil.Tooltip($"{kvp.Key.GetType()} -> {kvp.Value.GetType()}")), (Func<object>) (() => (object) new
      {
        key = kvp.Key,
        value = kvp.Value
      })));
      ++index;
    }
  }
}
