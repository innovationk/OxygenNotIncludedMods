// Decompiled with JetBrains decompiler
// Type: Database.DefeatPrehistoricAsteroid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
namespace Database;

public class DefeatPrehistoricAsteroid : VictoryColonyAchievementRequirement
{
  public override string GetProgress(bool complete)
  {
    int num1 = 1000;
    int num2 = complete ? num1 : 0;
    GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance((HashedString) Db.Get().GameplayEvents.LargeImpactor.Id);
    if (gameplayEventInstance != null)
    {
      LargeImpactorEvent.StatesInstance smi1 = (LargeImpactorEvent.StatesInstance) gameplayEventInstance.smi;
      if (smi1 != null && (Object) smi1.impactorInstance != (Object) null)
      {
        LargeImpactorStatus.Instance smi2 = smi1.impactorInstance.GetSMI<LargeImpactorStatus.Instance>();
        num1 = smi2.def.MAX_HEALTH;
        num2 = num1 - smi2.Health;
      }
    }
    return GameUtil.SafeStringFormat((string) COLONY_ACHIEVEMENTS.ASTEROID_DESTROYED.REQUIREMENT_DESCRIPTION, (object) GameUtil.GetFormattedInt((float) num2), (object) GameUtil.GetFormattedInt((float) num1));
  }

  public override string Description()
  {
    return (string) COLONY_ACHIEVEMENTS.ASTEROID_DESTROYED.DESCRIPTION;
  }

  public override bool Success()
  {
    return SaveGame.Instance.ColonyAchievementTracker.largeImpactorState == ColonyAchievementTracker.LargeImpactorState.Defeated;
  }

  public override bool Fail()
  {
    return SaveGame.Instance.ColonyAchievementTracker.largeImpactorState == ColonyAchievementTracker.LargeImpactorState.Landed;
  }

  public override string Name() => (string) COLONY_ACHIEVEMENTS.ASTEROID_DESTROYED.REQUIREMENT_NAME;
}
