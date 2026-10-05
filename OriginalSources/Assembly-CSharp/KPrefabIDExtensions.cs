// Decompiled with JetBrains decompiler
// Type: KPrefabIDExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class KPrefabIDExtensions
{
  public static Tag PrefabID(this Component cmp) => cmp.GetComponent<KPrefabID>().PrefabID();

  public static Tag PrefabID(this GameObject go) => go.GetComponent<KPrefabID>().PrefabID();

  public static Tag PrefabID(this StateMachine.Instance smi)
  {
    return smi.GetComponent<KPrefabID>().PrefabID();
  }

  public static bool IsPrefabID(this Component cmp, Tag id)
  {
    return cmp.GetComponent<KPrefabID>().IsPrefabID(id);
  }

  public static bool IsPrefabID(this GameObject go, Tag id)
  {
    return go.GetComponent<KPrefabID>().IsPrefabID(id);
  }

  public static bool HasTag(this Component cmp, Tag tag)
  {
    return cmp.GetComponent<KPrefabID>().HasTag(tag);
  }

  public static bool HasTag(this GameObject go, Tag tag)
  {
    return go.GetComponent<KPrefabID>().HasTag(tag);
  }

  public static bool HasAnyTags(this Component cmp, Tag[] tags)
  {
    return cmp.GetComponent<KPrefabID>().HasAnyTags(tags);
  }

  public static bool HasAnyTags(this Component cmp, List<Tag> tags)
  {
    return cmp.GetComponent<KPrefabID>().HasAnyTags(tags);
  }

  public static bool HasAnyTags(this GameObject go, Tag[] tags)
  {
    return go.GetComponent<KPrefabID>().HasAnyTags(tags);
  }

  public static bool HasAnyTags(this GameObject go, List<Tag> tags)
  {
    return go.GetComponent<KPrefabID>().HasAnyTags(tags);
  }

  public static bool HasAllTags(this Component cmp, Tag[] tags)
  {
    return cmp.GetComponent<KPrefabID>().HasAllTags(tags);
  }

  public static bool HasAllTags(this GameObject go, Tag[] tags)
  {
    return go.GetComponent<KPrefabID>().HasAllTags(tags);
  }

  public static void AddTag(this GameObject go, Tag tag)
  {
    go.GetComponent<KPrefabID>().AddTag(tag);
  }

  public static void AddTag(this Component cmp, Tag tag)
  {
    cmp.GetComponent<KPrefabID>().AddTag(tag);
  }

  public static void RemoveTag(this GameObject go, Tag tag)
  {
    go.GetComponent<KPrefabID>().RemoveTag(tag);
  }

  public static void RemoveTag(this Component cmp, Tag tag)
  {
    cmp.GetComponent<KPrefabID>().RemoveTag(tag);
  }
}
