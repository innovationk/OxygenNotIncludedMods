// Decompiled with JetBrains decompiler
// Type: GameplayEventManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;

#nullable disable
public class GameplayEventManager : KMonoBehaviour
{
  public static GameplayEventManager Instance;
  public Notifier notifier;
  [Serialize]
  private List<GameplayEventInstance> activeEvents = new List<GameplayEventInstance>();
  [Serialize]
  private Dictionary<HashedString, int> pastEvents = new Dictionary<HashedString, int>();
  [Serialize]
  private Dictionary<HashedString, float> sleepTimers = new Dictionary<HashedString, float>();

  public static void DestroyInstance()
  {
    GameplayEventManager.Instance = (GameplayEventManager) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    GameplayEventManager.Instance = this;
    this.notifier = this.GetComponent<Notifier>();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RestoreEvents();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    GameplayEventManager.Instance = (GameplayEventManager) null;
  }

  private void RestoreEvents()
  {
    this.activeEvents.RemoveAll((Predicate<GameplayEventInstance>) (x => Db.Get().GameplayEvents.TryGet(x.eventID) == null));
    for (int index = this.activeEvents.Count - 1; index >= 0; --index)
    {
      GameplayEventInstance activeEvent = this.activeEvents[index];
      if (activeEvent.smi == null)
        this.StartEventInstance(activeEvent);
    }
  }

  public void SetSleepTimerForEvent(GameplayEvent eventType, float time)
  {
    this.sleepTimers[eventType.IdHash] = time;
  }

  public float GetSleepTimer(GameplayEvent eventType)
  {
    float sleepTimer = 0.0f;
    this.sleepTimers.TryGetValue(eventType.IdHash, out sleepTimer);
    this.sleepTimers[eventType.IdHash] = sleepTimer;
    return sleepTimer;
  }

  public bool IsGameplayEventActive(GameplayEvent eventType)
  {
    return this.activeEvents.Find((Predicate<GameplayEventInstance>) (e => e.eventID == eventType.IdHash)) != null;
  }

  public bool IsGameplayEventRunningWithTag(Tag tag)
  {
    foreach (GameplayEventInstance activeEvent in this.activeEvents)
    {
      if (activeEvent.tags.Contains(tag))
        return true;
    }
    return false;
  }

  public void GetActiveEventsOfType<T>(int worldID, ref List<GameplayEventInstance> results) where T : GameplayEvent
  {
    foreach (GameplayEventInstance activeEvent in this.activeEvents)
    {
      if (activeEvent.worldId == worldID && (object) (activeEvent.gameplayEvent as T) != null)
        results.Add(activeEvent);
    }
  }

  public void GetActiveEventsOfType<T>(ref List<GameplayEventInstance> results) where T : GameplayEvent
  {
    foreach (GameplayEventInstance activeEvent in this.activeEvents)
    {
      if ((object) (activeEvent.gameplayEvent as T) != null)
        results.Add(activeEvent);
    }
  }

  private GameplayEventInstance CreateGameplayEvent(GameplayEvent gameplayEvent, int worldId)
  {
    return gameplayEvent.CreateInstance(worldId);
  }

  public GameplayEventInstance GetGameplayEventInstance(HashedString eventID, int worldId = -1)
  {
    return this.activeEvents.Find((Predicate<GameplayEventInstance>) (e =>
    {
      if (!(e.eventID == eventID))
        return false;
      return worldId == -1 || e.worldId == worldId;
    }));
  }

  public GameplayEventInstance CreateOrGetEventInstance(GameplayEvent eventType, int worldId = -1)
  {
    return this.GetGameplayEventInstance((HashedString) eventType.Id, worldId) ?? this.StartNewEvent(eventType, worldId);
  }

  public void RemoveActiveEvent(GameplayEventInstance eventInstance, string reason = "RemoveActiveEvent() called")
  {
    GameplayEventInstance gameplayEventInstance = this.activeEvents.Find((Predicate<GameplayEventInstance>) (x => x == eventInstance));
    if (gameplayEventInstance == null)
      return;
    if (gameplayEventInstance.smi != null)
      gameplayEventInstance.smi.StopSM(reason);
    else
      this.activeEvents.Remove(gameplayEventInstance);
  }

  public GameplayEventInstance StartNewEvent(
    GameplayEvent eventType,
    int worldId = -1,
    Action<StateMachine.Instance> setupActionsBeforeStart = null)
  {
    GameplayEventInstance gameplayEvent = this.CreateGameplayEvent(eventType, worldId);
    this.StartEventInstance(gameplayEvent, setupActionsBeforeStart);
    this.activeEvents.Add(gameplayEvent);
    int num;
    this.pastEvents.TryGetValue(gameplayEvent.eventID, out num);
    this.pastEvents[gameplayEvent.eventID] = num + 1;
    return gameplayEvent;
  }

  private void StartEventInstance(
    GameplayEventInstance gameplayEventInstance,
    Action<StateMachine.Instance> setupActionsBeforeStart = null)
  {
    StateMachine.Instance instance = gameplayEventInstance.PrepareEvent(this);
    instance.OnStop += (Action<string, StateMachine.Status>) ((reason, status) => this.activeEvents.Remove(gameplayEventInstance));
    if (setupActionsBeforeStart != null)
      setupActionsBeforeStart(instance);
    gameplayEventInstance.StartEvent();
  }

  public int NumberOfPastEvents(HashedString eventID)
  {
    int num;
    this.pastEvents.TryGetValue(eventID, out num);
    return num;
  }

  public static Notification CreateStandardCancelledNotification(EventInfoData eventInfoData)
  {
    if (eventInfoData == null)
    {
      DebugUtil.LogWarningArgs((object) "eventPopup is null in CreateStandardCancelledNotification");
      return (Notification) null;
    }
    eventInfoData.FinalizeText();
    return new Notification(string.Format((string) GAMEPLAY_EVENTS.CANCELED, (object) eventInfoData.title), NotificationType.Event, (Func<List<Notification>, object, string>) ((list, data) => string.Format((string) GAMEPLAY_EVENTS.CANCELED_TOOLTIP, (object) eventInfoData.title)));
  }
}
