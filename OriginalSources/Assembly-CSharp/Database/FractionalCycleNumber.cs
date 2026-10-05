// Decompiled with JetBrains decompiler
// Type: Database.FractionalCycleNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class FractionalCycleNumber : 
  ColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  private float fractionalCycleNumber;

  public FractionalCycleNumber(float fractionalCycleNumber)
  {
    this.fractionalCycleNumber = fractionalCycleNumber;
  }

  public override bool Success()
  {
    int fractionalCycleNumber = (int) this.fractionalCycleNumber;
    float num = this.fractionalCycleNumber - (float) fractionalCycleNumber;
    if ((double) (GameClock.Instance.GetCycle() + 1) > (double) this.fractionalCycleNumber)
      return true;
    return GameClock.Instance.GetCycle() + 1 == fractionalCycleNumber && (double) GameClock.Instance.GetCurrentCycleAsPercentage() >= (double) num;
  }

  public void Deserialize(IReader reader) => this.fractionalCycleNumber = reader.ReadSingle();

  public override string GetProgress(bool complete)
  {
    float num = (float) GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage();
    return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.FRACTIONAL_CYCLE, (object) (float) (complete ? (double) this.fractionalCycleNumber : (double) num), (object) this.fractionalCycleNumber);
  }
}
