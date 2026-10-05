// Decompiled with JetBrains decompiler
// Type: Database.TuneUpGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
namespace Database;

public class TuneUpGenerator : 
  ColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  private float numChoreseToComplete;
  private float choresCompleted;

  public TuneUpGenerator(float numChoreseToComplete)
  {
    this.numChoreseToComplete = numChoreseToComplete;
  }

  public override bool Success()
  {
    float num = 0.0f;
    ReportManager.ReportEntry entry1 = ReportManager.Instance.TodaysReport.GetEntry(ReportManager.ReportType.ChoreStatus);
    for (int i = 0; i < entry1.contextEntries.Count; ++i)
    {
      ReportManager.ReportEntry contextEntry = entry1.contextEntries[i];
      if (contextEntry.context == Db.Get().ChoreTypes.PowerTinker.Name)
        num += contextEntry.Negative;
    }
    string name = Db.Get().ChoreTypes.PowerTinker.Name;
    int count1 = ReportManager.Instance.reports.Count;
    for (int index = 0; index < count1; ++index)
    {
      ReportManager.ReportEntry entry2 = ReportManager.Instance.reports[index].GetEntry(ReportManager.ReportType.ChoreStatus);
      int count2 = entry2.contextEntries.Count;
      for (int i = 0; i < count2; ++i)
      {
        ReportManager.ReportEntry contextEntry = entry2.contextEntries[i];
        if (contextEntry.context == name)
          num += contextEntry.Negative;
      }
    }
    this.choresCompleted = Math.Abs(num);
    return (double) Math.Abs(num) >= (double) this.numChoreseToComplete;
  }

  public void Deserialize(IReader reader) => this.numChoreseToComplete = reader.ReadSingle();

  public override string GetProgress(bool complete)
  {
    return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CHORES_OF_TYPE, (object) (float) (complete ? (double) this.numChoreseToComplete : (double) this.choresCompleted), (object) this.numChoreseToComplete, (object) Db.Get().ChoreTypes.PowerTinker.Name);
  }
}
