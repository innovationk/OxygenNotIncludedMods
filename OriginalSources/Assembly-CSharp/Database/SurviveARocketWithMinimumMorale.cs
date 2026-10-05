// Decompiled with JetBrains decompiler
// Type: Database.SurviveARocketWithMinimumMorale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

#nullable disable
namespace Database;

public class SurviveARocketWithMinimumMorale : ColonyAchievementRequirement
{
  public float minimumMorale;
  public int numberOfCycles;

  public SurviveARocketWithMinimumMorale(float minimumMorale, int numberOfCycles)
  {
    this.minimumMorale = minimumMorale;
    this.numberOfCycles = numberOfCycles;
  }

  public override string GetProgress(bool complete)
  {
    return complete ? string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SURVIVE_SPACE_COMPLETE, (object) this.minimumMorale, (object) this.numberOfCycles) : base.GetProgress(complete);
  }

  public override bool Success()
  {
    foreach (KeyValuePair<int, int> keyValuePair in SaveGame.Instance.ColonyAchievementTracker.cyclesRocketDupeMoraleAboveRequirement)
    {
      if (keyValuePair.Value >= this.numberOfCycles)
        return true;
    }
    return false;
  }
}
