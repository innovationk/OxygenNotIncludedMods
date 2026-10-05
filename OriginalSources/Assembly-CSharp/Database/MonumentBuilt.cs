// Decompiled with JetBrains decompiler
// Type: Database.MonumentBuilt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class MonumentBuilt : 
  VictoryColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  public override string Name()
  {
    return (string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT;
  }

  public override string Description()
  {
    return (string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT_DESCRIPTION;
  }

  public override bool Success()
  {
    foreach (MonumentPart monumentPart in Components.MonumentParts)
    {
      if (monumentPart.IsMonumentCompleted())
      {
        Game.Instance.unlocks.Unlock("thriving");
        return true;
      }
    }
    return false;
  }

  public void Deserialize(IReader reader)
  {
  }

  public override string GetProgress(bool complete) => this.Name();
}
