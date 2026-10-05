// Decompiled with JetBrains decompiler
// Type: GameplayEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
[DebuggerDisplay("{base.Id}")]
public abstract class GameplayEvent : Resource, IComparable<GameplayEvent>, IHasDlcRestrictions
{
  public const int INFINITE = -1;
  public int numTimesAllowed = -1;
  public bool allowMultipleEventInstances;
  protected int basePriority;
  protected int calculatedPriority;
  public List<GameplayEventPrecondition> preconditions;
  public List<GameplayEventMinionFilter> minionFilters;
  public List<HashedString> successEvents;
  public List<HashedString> failureEvents;
  public string title;
  public string description;
  public HashedString animFileName;
  public List<Tag> tags;
  private string[] requiredDlcIds;
  private string[] forbiddenDlcIds;

  public int importance { get; private set; }

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;

  public virtual bool IsAllowed()
  {
    if (this.WillNeverRunAgain() || !this.allowMultipleEventInstances && GameplayEventManager.Instance.IsGameplayEventActive(this))
      return false;
    foreach (GameplayEventPrecondition precondition in this.preconditions)
    {
      if (precondition.required && !precondition.condition())
        return false;
    }
    float sleepTimer = GameplayEventManager.Instance.GetSleepTimer(this);
    return (double) GameUtil.GetCurrentTimeInCycles() >= (double) sleepTimer;
  }

  public void SetSleepTimer(float timeToSleepUntil)
  {
    GameplayEventManager.Instance.SetSleepTimerForEvent(this, timeToSleepUntil);
  }

  public virtual bool WillNeverRunAgain()
  {
    if (!Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) this))
      return true;
    return this.numTimesAllowed != -1 && GameplayEventManager.Instance.NumberOfPastEvents((HashedString) this.Id) >= this.numTimesAllowed;
  }

  public int GetCashedPriority() => this.calculatedPriority;

  public virtual int CalculatePriority()
  {
    this.calculatedPriority = this.basePriority + this.CalculateBoost();
    return this.calculatedPriority;
  }

  public int CalculateBoost()
  {
    int boost = 0;
    foreach (GameplayEventPrecondition precondition in this.preconditions)
    {
      if (!precondition.required && precondition.condition())
        boost += precondition.priorityModifier;
    }
    return boost;
  }

  public GameplayEvent AddPrecondition(GameplayEventPrecondition precondition)
  {
    precondition.required = true;
    this.preconditions.Add(precondition);
    return this;
  }

  public GameplayEvent AddPriorityBoost(GameplayEventPrecondition precondition, int priorityBoost)
  {
    precondition.required = false;
    precondition.priorityModifier = priorityBoost;
    this.preconditions.Add(precondition);
    return this;
  }

  public GameplayEvent AddMinionFilter(GameplayEventMinionFilter filter)
  {
    this.minionFilters.Add(filter);
    return this;
  }

  public GameplayEvent TrySpawnEventOnSuccess(HashedString evt)
  {
    this.successEvents.Add(evt);
    return this;
  }

  public GameplayEvent TrySpawnEventOnFailure(HashedString evt)
  {
    this.failureEvents.Add(evt);
    return this;
  }

  public GameplayEvent SetVisuals(HashedString animFileName)
  {
    this.animFileName = animFileName;
    return this;
  }

  public virtual Sprite GetDisplaySprite() => (Sprite) null;

  public virtual string GetDisplayString() => (string) null;

  public MinionIdentity GetRandomFilteredMinion()
  {
    List<MinionIdentity> minionIdentityList = new List<MinionIdentity>((IEnumerable<MinionIdentity>) Components.LiveMinionIdentities.Items);
    foreach (GameplayEventMinionFilter minionFilter in this.minionFilters)
    {
      GameplayEventMinionFilter filter = minionFilter;
      minionIdentityList.RemoveAll((Predicate<MinionIdentity>) (x => !filter.filter(x)));
    }
    return minionIdentityList.Count != 0 ? minionIdentityList[UnityEngine.Random.Range(0, minionIdentityList.Count)] : (MinionIdentity) null;
  }

  public MinionIdentity GetRandomMinionPrioritizeFiltered()
  {
    MinionIdentity randomFilteredMinion = this.GetRandomFilteredMinion();
    return !((UnityEngine.Object) randomFilteredMinion == (UnityEngine.Object) null) ? randomFilteredMinion : Components.LiveMinionIdentities.Items[UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Items.Count)];
  }

  public int CompareTo(GameplayEvent other)
  {
    return -this.GetCashedPriority().CompareTo(other.GetCashedPriority());
  }

  public GameplayEvent(string id, int priority, int importance)
    : base(id)
  {
    this.tags = new List<Tag>();
    this.basePriority = priority;
    this.preconditions = new List<GameplayEventPrecondition>();
    this.minionFilters = new List<GameplayEventMinionFilter>();
    this.successEvents = new List<HashedString>();
    this.failureEvents = new List<HashedString>();
    this.importance = importance;
    this.animFileName = (HashedString) id;
  }

  public GameplayEvent(
    string id,
    int priority,
    int importance,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds = null)
    : this(id, priority, importance)
  {
    this.requiredDlcIds = requiredDlcIds;
    this.forbiddenDlcIds = forbiddenDlcIds;
  }

  public abstract StateMachine.Instance GetSMI(
    GameplayEventManager manager,
    GameplayEventInstance eventInstance);

  public GameplayEventInstance CreateInstance(int worldId)
  {
    GameplayEventInstance instance = new GameplayEventInstance(this, worldId);
    if (this.tags != null)
      instance.tags.AddRange((IEnumerable<Tag>) this.tags);
    return instance;
  }
}
