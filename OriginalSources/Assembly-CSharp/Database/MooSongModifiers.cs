// Decompiled with JetBrains decompiler
// Type: Database.MooSongModifiers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Database;

public class MooSongModifiers : ResourceSet<MooSongModifier>
{
  public List<MooSongModifier> GetForTag(Tag searchTag)
  {
    List<MooSongModifier> forTag = new List<MooSongModifier>();
    foreach (MooSongModifier resource in this.resources)
    {
      if (resource.TargetTag == searchTag)
        forTag.Add(resource);
    }
    return forTag;
  }
}
