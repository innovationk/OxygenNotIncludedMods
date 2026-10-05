// Decompiled with JetBrains decompiler
// Type: Database.EatXCalories
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class EatXCalories : 
  ColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  private int numCalories;

  public EatXCalories(int numCalories) => this.numCalories = numCalories;

  public override bool Success()
  {
    return (double) WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumed() / 1000.0 > (double) this.numCalories;
  }

  public void Deserialize(IReader reader) => this.numCalories = reader.ReadInt32();

  public override string GetProgress(bool complete)
  {
    return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CONSUME_CALORIES, (object) GameUtil.GetFormattedCalories(complete ? (float) this.numCalories * 1000f : WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumed()), (object) GameUtil.GetFormattedCalories((float) this.numCalories * 1000f));
  }
}
