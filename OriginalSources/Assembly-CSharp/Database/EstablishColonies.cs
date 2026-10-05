// Decompiled with JetBrains decompiler
// Type: Database.EstablishColonies
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
namespace Database;

public class EstablishColonies : VictoryColonyAchievementRequirement
{
  public static int BASE_COUNT = 5;

  public override string GetProgress(bool complete)
  {
    return ((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ESTABLISH_COLONIES).Replace("{goalBaseCount}", EstablishColonies.BASE_COUNT.ToString()).Replace("{baseCount}", this.GetColonyCount().ToString()).Replace("{neededCount}", EstablishColonies.BASE_COUNT.ToString());
  }

  public override string Description() => this.GetProgress(this.Success());

  public override bool Success() => this.GetColonyCount() >= EstablishColonies.BASE_COUNT;

  public override string Name()
  {
    return (string) COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.SEVERAL_COLONIES;
  }

  private int GetColonyCount()
  {
    int colonyCount = 0;
    for (int idx = 0; idx < Components.Telepads.Count; ++idx)
    {
      Activatable component = Components.Telepads[idx].GetComponent<Activatable>();
      if ((Object) component == (Object) null || component.IsActivated)
        ++colonyCount;
    }
    return colonyCount;
  }
}
