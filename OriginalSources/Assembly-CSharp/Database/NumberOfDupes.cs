// Decompiled with JetBrains decompiler
// Type: Database.NumberOfDupes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class NumberOfDupes : 
  VictoryColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  private int numDupes;

  public override string Name()
  {
    return string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS, (object) this.numDupes);
  }

  public override string Description()
  {
    return string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS_DESCRIPTION, (object) this.numDupes);
  }

  public NumberOfDupes(int num) => this.numDupes = num;

  public override bool Success() => Components.LiveMinionIdentities.Items.Count >= this.numDupes;

  public void Deserialize(IReader reader) => this.numDupes = reader.ReadInt32();

  public override string GetProgress(bool complete)
  {
    return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.POPULATION, (object) (complete ? this.numDupes : Components.LiveMinionIdentities.Items.Count), (object) this.numDupes);
  }
}
