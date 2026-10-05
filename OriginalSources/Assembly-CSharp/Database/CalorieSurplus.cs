// Decompiled with JetBrains decompiler
// Type: Database.CalorieSurplus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class CalorieSurplus : 
  ColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  private double surplusAmount;

  public CalorieSurplus(float surplusAmount) => this.surplusAmount = (double) surplusAmount;

  public override bool Success()
  {
    return (double) ClusterManager.Instance.CountAllRations() / 1000.0 >= this.surplusAmount;
  }

  public override bool Fail() => !this.Success();

  public void Deserialize(IReader reader) => this.surplusAmount = reader.ReadDouble();

  public override string GetProgress(bool complete)
  {
    return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CALORIE_SURPLUS, (object) GameUtil.GetFormattedCalories(complete ? (float) this.surplusAmount : ClusterManager.Instance.CountAllRations()), (object) GameUtil.GetFormattedCalories((float) this.surplusAmount));
  }
}
