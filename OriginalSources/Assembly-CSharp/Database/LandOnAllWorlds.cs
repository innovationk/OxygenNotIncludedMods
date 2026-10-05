// Decompiled with JetBrains decompiler
// Type: Database.LandOnAllWorlds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class LandOnAllWorlds : ColonyAchievementRequirement
{
  public override string GetProgress(bool complete)
  {
    int num1 = 0;
    int num2 = 0;
    foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
    {
      if (!worldContainer.IsModuleInterior)
      {
        ++num1;
        if (worldContainer.IsDupeVisited || worldContainer.IsRoverVisted)
          ++num2;
      }
    }
    return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAND_DUPES_ON_ALL_WORLDS, (object) num2, (object) num1);
  }

  public override bool Success()
  {
    foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
    {
      if (!worldContainer.IsModuleInterior && !worldContainer.IsDupeVisited && !worldContainer.IsRoverVisted)
        return false;
    }
    return true;
  }
}
