// Decompiled with JetBrains decompiler
// Type: Klei.AI.GameplaySeasonInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Klei.AI;

[SerializationConfig(MemberSerialization.OptIn)]
public class GameplaySeasonInstance : ISaveLoadable
{
  public const int LIMIT_SELECTION = 5;
  [Serialize]
  public int numStartEvents;
  [Serialize]
  public int worldId;
  [Serialize]
  private readonly string seasonId;
  [Serialize]
  private float nextPeriodTime;
  [Serialize]
  private float randomizedNextTime;
  private bool allEventWillNotRunAgain;
  private GameplaySeason _season;

  public float NextEventTime => this.nextPeriodTime + this.randomizedNextTime;

  public GameplaySeason Season
  {
    get
    {
      if (this._season == null)
        this._season = Db.Get().GameplaySeasons.TryGet(this.seasonId);
      return this._season;
    }
  }

  public GameplaySeasonInstance(GameplaySeason season, int worldId)
  {
    this.seasonId = season.Id;
    this.worldId = worldId;
    float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
    if (season.synchronizedToPeriod)
    {
      float seasonPeriod = this.Season.GetSeasonPeriod();
      this.nextPeriodTime = (Mathf.Floor(currentTimeInCycles / seasonPeriod) + 1f) * seasonPeriod;
    }
    else
      this.nextPeriodTime = currentTimeInCycles;
    this.CalculateNextEventTime();
  }

  private void CalculateNextEventTime()
  {
    float seasonPeriod = this.Season.GetSeasonPeriod();
    this.randomizedNextTime = UnityEngine.Random.Range(this.Season.randomizedEventStartTime.min, this.Season.randomizedEventStartTime.max);
    float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
    for (float num = this.nextPeriodTime + this.randomizedNextTime; (double) num < (double) currentTimeInCycles || (double) num < (double) this.Season.minCycle; num = this.nextPeriodTime + this.randomizedNextTime)
      this.nextPeriodTime += seasonPeriod;
  }

  public bool StartEvent(bool ignorePreconditions = false)
  {
    bool flag = false;
    this.CalculateNextEventTime();
    ++this.numStartEvents;
    List<GameplayEvent> gameplayEventList = ignorePreconditions ? this.Season.events : this.Season.events.Where<GameplayEvent>((Func<GameplayEvent, bool>) (x => x.IsAllowed())).ToList<GameplayEvent>();
    if (gameplayEventList.Count > 0)
    {
      gameplayEventList.ForEach((Action<GameplayEvent>) (x => x.CalculatePriority()));
      gameplayEventList.Sort();
      int maxExclusive = Mathf.Min(gameplayEventList.Count, 5);
      GameplayEvent eventType = gameplayEventList[UnityEngine.Random.Range(0, maxExclusive)];
      GameplayEventManager.Instance.StartNewEvent(eventType, this.worldId, new Action<StateMachine.Instance>(this.Season.AdditionalEventInstanceSetup));
      flag = true;
    }
    this.allEventWillNotRunAgain = true;
    foreach (GameplayEvent gameplayEvent in this.Season.events)
    {
      if (!gameplayEvent.WillNeverRunAgain())
      {
        this.allEventWillNotRunAgain = false;
        break;
      }
    }
    return flag;
  }

  public bool ShouldGenerateEvents()
  {
    WorldContainer world = ClusterManager.Instance.GetWorld(this.worldId);
    if ((world.IsDupeVisited ? 1 : (world.IsRoverVisted ? 1 : 0)) == 0 || this.Season.finishAfterNumEvents != -1 && this.numStartEvents >= this.Season.finishAfterNumEvents || this.allEventWillNotRunAgain)
      return false;
    float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
    return (double) currentTimeInCycles > (double) this.Season.minCycle && (double) currentTimeInCycles < (double) this.Season.maxCycle;
  }
}
