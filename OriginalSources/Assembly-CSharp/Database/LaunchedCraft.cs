// Decompiled with JetBrains decompiler
// Type: Database.LaunchedCraft
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class LaunchedCraft : ColonyAchievementRequirement
{
  public override string GetProgress(bool completed)
  {
    return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET;
  }

  public override bool Success()
  {
    foreach (Clustercraft clustercraft in Components.Clustercrafts)
    {
      if (clustercraft.Status == Clustercraft.CraftStatus.InFlight)
        return true;
    }
    return false;
  }
}
