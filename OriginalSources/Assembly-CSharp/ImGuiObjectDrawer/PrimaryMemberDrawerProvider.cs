// Decompiled with JetBrains decompiler
// Type: ImGuiObjectDrawer.PrimaryMemberDrawerProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ImGuiObjectDrawer;

public class PrimaryMemberDrawerProvider : IMemberDrawerProvider
{
  public int Priority => 100;

  public void AppendDrawersTo(List<MemberDrawer> drawers)
  {
    drawers.AddRange((IEnumerable<MemberDrawer>) new MemberDrawer[15]
    {
      (MemberDrawer) new NullDrawer(),
      (MemberDrawer) new SimpleDrawer(),
      (MemberDrawer) new LocStringDrawer(),
      (MemberDrawer) new EnumDrawer(),
      (MemberDrawer) new HashedStringDrawer(),
      (MemberDrawer) new KAnimHashedStringDrawer(),
      (MemberDrawer) new Vector2Drawer(),
      (MemberDrawer) new Vector3Drawer(),
      (MemberDrawer) new Vector4Drawer(),
      (MemberDrawer) new UnityObjectDrawer(),
      (MemberDrawer) new ArrayDrawer(),
      (MemberDrawer) new IDictionaryDrawer(),
      (MemberDrawer) new IEnumerableDrawer(),
      (MemberDrawer) new PlainCSharpObjectDrawer(),
      (MemberDrawer) new FallbackDrawer()
    });
  }
}
