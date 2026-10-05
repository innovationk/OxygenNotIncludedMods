// Decompiled with JetBrains decompiler
// Type: Database.CritterTypeExists
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Database;

public class CritterTypeExists : 
  ColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  private List<Tag> critterTypes = new List<Tag>();

  public CritterTypeExists(List<Tag> critterTypes) => this.critterTypes = critterTypes;

  public override bool Success()
  {
    foreach (Component cmp in Components.Capturables.Items)
    {
      if (this.critterTypes.Contains(cmp.PrefabID()))
        return true;
    }
    return false;
  }

  public void Deserialize(IReader reader)
  {
    int capacity = reader.ReadInt32();
    this.critterTypes = new List<Tag>(capacity);
    for (int index = 0; index < capacity; ++index)
      this.critterTypes.Add(new Tag(reader.ReadKleiString()));
  }

  public override string GetProgress(bool complete)
  {
    return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.HATCH_A_MORPH;
  }
}
