// Decompiled with JetBrains decompiler
// Type: Database.CycleNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class CycleNumber : 
  VictoryColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  private int cycleNumber;

  public override string Name()
  {
    return string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE, (object) this.cycleNumber);
  }

  public override string Description()
  {
    return string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_CYCLE_DESCRIPTION, (object) this.cycleNumber);
  }

  public CycleNumber(int cycleNumber = 100) => this.cycleNumber = cycleNumber;

  public override bool Success() => GameClock.Instance.GetCycle() + 1 >= this.cycleNumber;

  public void Deserialize(IReader reader) => this.cycleNumber = reader.ReadInt32();

  public override string GetProgress(bool complete)
  {
    return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CYCLE_NUMBER, (object) (complete ? this.cycleNumber : GameClock.Instance.GetCycle() + 1), (object) this.cycleNumber);
  }
}
