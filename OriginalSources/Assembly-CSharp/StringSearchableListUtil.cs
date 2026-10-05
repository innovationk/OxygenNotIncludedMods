// Decompiled with JetBrains decompiler
// Type: StringSearchableListUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
public static class StringSearchableListUtil
{
  public static bool DoAnyTagsMatchFilter(string[] lowercaseTags, in string filter)
  {
    string filter1 = filter.Trim().ToLowerInvariant();
    string[] source = filter1.Split(' ', StringSplitOptions.None);
    foreach (string lowercaseTag in lowercaseTags)
    {
      string tag = lowercaseTag;
      if (StringSearchableListUtil.DoesTagMatchFilter(tag, in filter1) || ((IEnumerable<string>) source).Select<string, bool>((Func<string, bool>) (f => StringSearchableListUtil.DoesTagMatchFilter(tag, in f))).All<bool>((Func<bool, bool>) (result => result)))
        return true;
    }
    return false;
  }

  public static bool DoesTagMatchFilter(string lowercaseTag, in string filter)
  {
    return string.IsNullOrWhiteSpace(filter) || lowercaseTag.Contains(filter);
  }

  public static bool ShouldUseFilter(string filter) => !string.IsNullOrWhiteSpace(filter);
}
