// Decompiled with JetBrains decompiler
// Type: Klei.AI.GameplaySeason
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Klei.AI;

[DebuggerDisplay("{base.Id}")]
public class GameplaySeason : Resource, IHasDlcRestrictions
{
  public const float DEFAULT_PERCENTAGE_RANDOMIZED_EVENT_START = 0.0f;
  public const float PERCENTAGE_WARNING = 0.4f;
  public const float USE_DEFAULT = -1f;
  public const int INFINITE = -1;
  public float period;
  public bool synchronizedToPeriod;
  public MathUtil.MinMax randomizedEventStartTime;
  public int finishAfterNumEvents = -1;
  public bool startActive;
  public int numEventsToStartEachPeriod;
  public float minCycle;
  public float maxCycle;
  public List<GameplayEvent> events;
  private string[] requiredDlcIds;
  private string[] forbiddenDlcIds;
  public GameplaySeason.Type type;
  [Obsolete]
  public string dlcId;

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;

  public GameplaySeason(
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
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null)
    : base(id)
  {
    this.type = type;
    this.requiredDlcIds = requiredDlcIds;
    this.forbiddenDlcIds = forbiddenDlcIds;
    this.period = period;
    this.synchronizedToPeriod = synchronizedToPeriod;
    Debug.Assert((double) period > 0.0, (object) $"Season {id}'s Period cannot be 0 or negative");
    if ((double) randomizedEventStartTime == -1.0)
    {
      this.randomizedEventStartTime = new MathUtil.MinMax(-0.0f * period, 0.0f * period);
    }
    else
    {
      this.randomizedEventStartTime = new MathUtil.MinMax(-randomizedEventStartTime, randomizedEventStartTime);
      DebugUtil.DevAssert(((double) this.randomizedEventStartTime.max - (double) this.randomizedEventStartTime.min) * 0.40000000596046448 < (double) period, $"Season {id} randomizedEventStartTime is greater than {(ValueType) 0.4f}% of its period.");
    }
    this.startActive = startActive;
    this.finishAfterNumEvents = finishAfterNumEvents;
    this.minCycle = minCycle;
    this.maxCycle = maxCycle;
    this.events = new List<GameplayEvent>();
    this.numEventsToStartEachPeriod = numEventsToStartEachPeriod;
  }

  [Obsolete]
  public GameplaySeason(
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
    int numEventsToStartEachPeriod = 1)
    : this(id, type, period, (synchronizedToPeriod ? 1 : 0) != 0, randomizedEventStartTime, (startActive ? 1 : 0) != 0, finishAfterNumEvents, minCycle, maxCycle, numEventsToStartEachPeriod, new string[1]
    {
      dlcId
    })
  {
  }

  public virtual void AdditionalEventInstanceSetup(StateMachine.Instance generic_smi)
  {
  }

  public virtual float GetSeasonPeriod() => this.period;

  public GameplaySeason AddEvent(GameplayEvent evt)
  {
    this.events.Add(evt);
    return this;
  }

  public virtual GameplaySeasonInstance Instantiate(int worldId)
  {
    return new GameplaySeasonInstance(this, worldId);
  }

  public enum Type
  {
    World,
    Cluster,
  }
}
