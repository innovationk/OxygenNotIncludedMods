// Decompiled with JetBrains decompiler
// Type: Database.BuildALaunchPad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class BuildALaunchPad : ColonyAchievementRequirement
{
  public override string GetProgress(bool complete)
  {
    return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILD_A_LAUNCHPAD;
  }

  public override bool Success()
  {
    foreach (KMonoBehaviour component in Components.LaunchPads.Items)
    {
      WorldContainer myWorld = component.GetMyWorld();
      if (!myWorld.IsStartWorld && Components.WarpReceivers.GetWorldItems(myWorld.id).Count == 0)
        return true;
    }
    return false;
  }
}
