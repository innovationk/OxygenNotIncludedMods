// Decompiled with JetBrains decompiler
// Type: Database.TeleportDuplicant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class TeleportDuplicant : ColonyAchievementRequirement
{
  public override string GetProgress(bool complete)
  {
    return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TELEPORT_DUPLICANT;
  }

  public override bool Success()
  {
    foreach (WarpReceiver warpReceiver in Components.WarpReceivers.Items)
    {
      if (warpReceiver.Used)
        return true;
    }
    return false;
  }
}
