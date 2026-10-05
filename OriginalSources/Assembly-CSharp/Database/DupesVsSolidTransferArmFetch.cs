// Decompiled with JetBrains decompiler
// Type: Database.DupesVsSolidTransferArmFetch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Database;

public class DupesVsSolidTransferArmFetch : 
  ColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  public float percentage;
  public int numCycles;
  public int currentCycleCount;
  public bool armsOutPerformingDupesThisCycle;

  public DupesVsSolidTransferArmFetch(float percentage, int numCycles)
  {
    this.percentage = percentage;
    this.numCycles = numCycles;
  }

  public override bool Success()
  {
    Dictionary<int, int> dupeChoreDeliveries = SaveGame.Instance.ColonyAchievementTracker.fetchDupeChoreDeliveries;
    Dictionary<int, int> automatedChoreDeliveries = SaveGame.Instance.ColonyAchievementTracker.fetchAutomatedChoreDeliveries;
    int val2 = 0;
    this.currentCycleCount = 0;
    for (int key = GameClock.Instance.GetCycle() - 1; key >= GameClock.Instance.GetCycle() - this.numCycles; --key)
    {
      if (automatedChoreDeliveries.ContainsKey(key))
      {
        if (!dupeChoreDeliveries.ContainsKey(key) || (double) dupeChoreDeliveries[key] < (double) automatedChoreDeliveries[key] * (double) this.percentage)
          ++val2;
        else
          break;
      }
      else if (dupeChoreDeliveries.ContainsKey(key))
      {
        val2 = 0;
        break;
      }
    }
    this.currentCycleCount = Math.Max(this.currentCycleCount, val2);
    return val2 >= this.numCycles;
  }

  public void Deserialize(IReader reader)
  {
    this.numCycles = reader.ReadInt32();
    this.percentage = reader.ReadSingle();
  }
}
