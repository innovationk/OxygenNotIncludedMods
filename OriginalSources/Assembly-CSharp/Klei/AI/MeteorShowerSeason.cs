// Decompiled with JetBrains decompiler
// Type: Klei.AI.MeteorShowerSeason
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using System;
using System.Diagnostics;

#nullable disable
namespace Klei.AI;

[DebuggerDisplay("{base.Id}")]
public class MeteorShowerSeason : GameplaySeason
{
  public bool affectedByDifficultySettings = true;
  public float clusterTravelDuration = -1f;

  public MeteorShowerSeason(
    string id,
    GameplaySeason.Type type,
    float period,
    bool synchronizedToPeriod,
    float randomizedEventStartTime = -1f,
    bool startActive = false,
    int finishAfterNumEvents = -1,
    float minCycle = 0.0f,
    float maxCycle = float.PositiveInfinity,
    int numEventsToStartEachPeriod = 1,
    bool affectedByDifficultySettings = true,
    float clusterTravelDuration = -1f,
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null)
    : base(id, type, period, synchronizedToPeriod, randomizedEventStartTime, startActive, finishAfterNumEvents, minCycle, maxCycle, numEventsToStartEachPeriod, requiredDlcIds, forbiddenDlcIds)
  {
    this.affectedByDifficultySettings = affectedByDifficultySettings;
    this.clusterTravelDuration = clusterTravelDuration;
  }

  [Obsolete]
  public MeteorShowerSeason(
    string id,
    GameplaySeason.Type type,
    string dlcId,
    float period,
    bool synchronizedToPeriod,
    float randomizedEventStartTime = -1f,
    bool startActive = false,
    int finishAfterNumEvents = -1,
    float minCycle = 0.0f,
    float maxCycle = float.PositiveInfinity,
    int numEventsToStartEachPeriod = 1,
    bool affectedByDifficultySettings = true,
    float clusterTravelDuration = -1f)
    : base(id, type, period, (synchronizedToPeriod ? 1 : 0) != 0, randomizedEventStartTime, (startActive ? 1 : 0) != 0, finishAfterNumEvents, minCycle, maxCycle, numEventsToStartEachPeriod, new string[1]
    {
      dlcId
    })
  {
  }

  public override void AdditionalEventInstanceSetup(StateMachine.Instance generic_smi)
  {
    (generic_smi as MeteorShowerEvent.StatesInstance).clusterTravelDuration = this.clusterTravelDuration;
  }

  public override float GetSeasonPeriod()
  {
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
    float seasonPeriod = base.GetSeasonPeriod();
    if (this.affectedByDifficultySettings && currentQualitySetting != null)
    {
      switch (currentQualitySetting.id)
      {
        case "Infrequent":
          seasonPeriod *= 2f;
          break;
        case "Intense":
          seasonPeriod *= 1f;
          break;
        case "Doomed":
          seasonPeriod *= 1f;
          break;
      }
    }
    return seasonPeriod;
  }
}
