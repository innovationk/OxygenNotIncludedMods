// Decompiled with JetBrains decompiler
// Type: Database.SentCraftIntoTemporalTear
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class SentCraftIntoTemporalTear : VictoryColonyAchievementRequirement
{
  public override string Name()
  {
    return string.Format((string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION, (object) UI.SPACEDESTINATIONS.WORMHOLE.NAME);
  }

  public override string Description()
  {
    return string.Format((string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION_DESCRIPTION, (object) UI.SPACEDESTINATIONS.WORMHOLE.NAME);
  }

  public override string GetProgress(bool completed)
  {
    return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET_TO_WORMHOLE;
  }

  public override bool Success()
  {
    return ClusterManager.Instance.GetClusterPOIManager().HasTemporalTearConsumedCraft();
  }
}
