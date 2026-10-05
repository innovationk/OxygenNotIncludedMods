// Decompiled with JetBrains decompiler
// Type: Database.RepairGeothermalController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class RepairGeothermalController : VictoryColonyAchievementRequirement
{
  public override string Description() => this.GetProgress(this.Success());

  public override string GetProgress(bool complete)
  {
    return (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.REPAIR_CONTROLLER_DESCRIPTION;
  }

  public override string Name()
  {
    return (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.REQUIREMENTS.REPAIR_CONTROLLER_TITLE;
  }

  public override bool Success()
  {
    return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired;
  }
}
