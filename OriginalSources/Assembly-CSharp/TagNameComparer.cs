// Decompiled with JetBrains decompiler
// Type: TagNameComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class TagNameComparer : IComparer<Tag>
{
  private Tag firstTag;

  public TagNameComparer()
  {
  }

  public TagNameComparer(Tag firstTag) => this.firstTag = firstTag;

  public int Compare(Tag x, Tag y)
  {
    if (x == y)
      return 0;
    if (this.firstTag.IsValid)
    {
      if (x == this.firstTag && y != this.firstTag)
        return 1;
      if (x != this.firstTag && y == this.firstTag)
        return -1;
    }
    return x.ProperNameStripLink().CompareTo(y.ProperNameStripLink());
  }
}
