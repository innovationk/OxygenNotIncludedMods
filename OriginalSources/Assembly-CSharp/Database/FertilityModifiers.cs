// Decompiled with JetBrains decompiler
// Type: Database.FertilityModifiers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;

#nullable disable
namespace Database;

public class FertilityModifiers : ResourceSet<FertilityModifier>
{
  public List<FertilityModifier> GetForTag(Tag searchTag)
  {
    List<FertilityModifier> forTag = new List<FertilityModifier>();
    foreach (FertilityModifier resource in this.resources)
    {
      if (resource.TargetTag == searchTag)
        forTag.Add(resource);
    }
    return forTag;
  }
}
