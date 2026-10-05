// Decompiled with JetBrains decompiler
// Type: Klei.AI.BonusEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Klei.AI;

public class BonusEvent : GameplayEvent<BonusEvent.StatesInstance>
{
  public const int PRE_SELECT_MINION_TIMEOUT = 5;
  public string effect;
  public bool preSelectMinion;
  public int numTimesToTrigger;
  public BonusEvent.TriggerType triggerType;
  public HashSet<Tag> buildingTrigger;
  public HashSet<System.Type> workableType;
  public HashSet<RoomType> roomRestrictions;
  public BonusEvent.ConditionFn extraCondition;
  public bool roomHasOwnable;

  public BonusEvent(
    string id,
    string overrideEffect = null,
    int numTimesAllowed = 1,
    bool preSelectMinion = false,
    int priority = 0)
    : base(id, priority)
  {
    this.title = (string) Strings.Get($"STRINGS.GAMEPLAY_EVENTS.BONUS.{id.ToUpper()}.NAME");
    this.description = (string) Strings.Get($"STRINGS.GAMEPLAY_EVENTS.BONUS.{id.ToUpper()}.DESCRIPTION");
    this.effect = overrideEffect != null ? overrideEffect : id;
    this.numTimesAllowed = numTimesAllowed;
    this.preSelectMinion = preSelectMinion;
    this.animFileName = (HashedString) (id.ToLower() + "_kanim");
    this.AddPrecondition(GameplayEventPreconditions.Instance.LiveMinions());
  }

  public override StateMachine.Instance GetSMI(
    GameplayEventManager manager,
    GameplayEventInstance eventInstance)
  {
    return (StateMachine.Instance) new BonusEvent.StatesInstance(manager, eventInstance, this);
  }

  public BonusEvent TriggerOnNewBuilding(int triggerCount, params string[] buildings)
  {
    DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event");
    this.triggerType = BonusEvent.TriggerType.NewBuilding;
    this.buildingTrigger = new HashSet<Tag>((IEnumerable<Tag>) buildings.ToTagList());
    this.numTimesToTrigger = triggerCount;
    return this;
  }

  public BonusEvent TriggerOnUseBuilding(int triggerCount, params string[] buildings)
  {
    DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event");
    this.triggerType = BonusEvent.TriggerType.UseBuilding;
    this.buildingTrigger = new HashSet<Tag>((IEnumerable<Tag>) buildings.ToTagList());
    this.numTimesToTrigger = triggerCount;
    return this;
  }

  public BonusEvent TriggerOnWorkableComplete(int triggerCount, params System.Type[] types)
  {
    DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event");
    this.triggerType = BonusEvent.TriggerType.WorkableComplete;
    this.workableType = new HashSet<System.Type>((IEnumerable<System.Type>) types);
    this.numTimesToTrigger = triggerCount;
    return this;
  }

  public BonusEvent SetExtraCondition(BonusEvent.ConditionFn extraCondition)
  {
    this.extraCondition = extraCondition;
    return this;
  }

  public BonusEvent SetRoomConstraints(bool hasOwnableInRoom, params RoomType[] types)
  {
    this.roomHasOwnable = hasOwnableInRoom;
    this.roomRestrictions = types == null ? (HashSet<RoomType>) null : new HashSet<RoomType>((IEnumerable<RoomType>) types);
    return this;
  }

  public string GetEffectTooltip(Effect effect)
  {
    return $"{effect.Name}\n\n{Effect.CreateTooltip(effect, true)}";
  }

  public override Sprite GetDisplaySprite()
  {
    Effect effect = Db.Get().effects.Get(this.effect);
    return effect.SelfModifiers.Count > 0 ? Assets.GetSprite((HashedString) Db.Get().Attributes.TryGet(effect.SelfModifiers[0].AttributeId).uiFullColourSprite) : (Sprite) null;
  }

  public override string GetDisplayString()
  {
    Effect effect = Db.Get().effects.Get(this.effect);
    return effect.SelfModifiers.Count > 0 ? Db.Get().Attributes.TryGet(effect.SelfModifiers[0].AttributeId).Name : (string) null;
  }

  public enum TriggerType
  {
    None,
    NewBuilding,
    UseBuilding,
    WorkableComplete,
    AchievementUnlocked,
  }

  public delegate bool ConditionFn(BonusEvent.GameplayEventData data);

  public class GameplayEventData
  {
    public GameHashes eventTrigger;
    public BuildingComplete building;
    public Workable workable;
    public WorkerBase worker;
  }

  public class States : 
    GameplayEventStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, BonusEvent>
  {
    public StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.TargetParameter chosen;
    public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State load;
    public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitNewBuilding;
    public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitUseBuilding;
    public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitForAchievement;
    public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitforWorkables;
    public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State immediate;
    public BonusEvent.States.ActiveStates active;
    public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State ending;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.load;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.load.Enter(new StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State.Callback(this.AssignPreSelectedMinionIfNeeded)).Transition(this.waitNewBuilding, (StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.Transition.ConditionCallback) (smi => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.NewBuilding)).Transition(this.waitUseBuilding, (StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.Transition.ConditionCallback) (smi => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.UseBuilding)).Transition(this.waitforWorkables, (StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.Transition.ConditionCallback) (smi => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.WorkableComplete)).Transition(this.waitForAchievement, (StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.Transition.ConditionCallback) (smi => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.AchievementUnlocked)).Transition(this.immediate, (StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.Transition.ConditionCallback) (smi => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.None));
      this.waitNewBuilding.EventHandlerTransition(GameHashes.NewBuilding, (GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State) this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.BuildingEventTrigger));
      this.waitUseBuilding.EventHandlerTransition(GameHashes.UseBuilding, (GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State) this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.BuildingEventTrigger));
      this.waitforWorkables.EventHandlerTransition(GameHashes.UseBuilding, (GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State) this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.WorkableEventTrigger));
      this.immediate.Enter((StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi =>
      {
        if (!((UnityEngine.Object) smi.sm.chosen.Get(smi) == (UnityEngine.Object) null))
          return;
        GameObject gameObject = smi.gameplayEvent.GetRandomMinionPrioritizeFiltered().gameObject;
        smi.sm.chosen.Set(gameObject, smi, false);
      })).GoTo((GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State) this.active);
      this.active.Enter((StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => smi.sm.chosen.Get(smi).GetComponent<Effects>().Add(smi.gameplayEvent.effect, true))).Enter((StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => this.MonitorStart(this.chosen, smi))).Exit((StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => this.MonitorStop(this.chosen, smi))).ScheduleGoTo((Func<BonusEvent.StatesInstance, float>) (smi =>
      {
        Effect effect = this.GetEffect(smi);
        return effect != null ? effect.duration : 0.0f;
      }), (StateMachine.BaseState) this.ending).DefaultState(this.active.notify).OnTargetLost(this.chosen, this.ending).Target(this.chosen).TagTransition(GameTags.Dead, this.ending);
      this.active.notify.ToggleNotification((Func<BonusEvent.StatesInstance, Notification>) (smi => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi))));
      this.active.seenNotification.Enter((StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => smi.eventInstance.seenNotification = true));
      this.ending.ReturnSuccess();
    }

    public override EventInfoData GenerateEventPopupData(BonusEvent.StatesInstance smi)
    {
      EventInfoData eventPopupData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
      GameObject go = smi.sm.chosen.Get(smi);
      if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      {
        DebugUtil.LogErrorArgs((object) ("Minion not set for " + smi.gameplayEvent.Id));
        return (EventInfoData) null;
      }
      Effect effect = this.GetEffect(smi);
      if (effect == null)
        return (EventInfoData) null;
      eventPopupData.clickFocus = go.transform;
      eventPopupData.minions = new GameObject[1]{ go };
      eventPopupData.SetTextParameter("dupe", go.GetProperName());
      if ((UnityEngine.Object) smi.building != (UnityEngine.Object) null)
        eventPopupData.SetTextParameter("building", UI.FormatAsLink(smi.building.GetProperName(), smi.building.GetProperName().ToUpper()));
      EventInfoData.Option option = eventPopupData.AddDefaultOption((System.Action) (() => smi.GoTo((StateMachine.BaseState) smi.sm.active.seenNotification)));
      ((string) GAMEPLAY_EVENTS.BONUS_EVENT_DESCRIPTION).Replace("{effects}", Effect.CreateTooltip(effect, false, " ", false)).Replace("{durration}", GameUtil.GetFormattedCycles(effect.duration));
      foreach (AttributeModifier selfModifier in effect.SelfModifiers)
      {
        Attribute attribute = Db.Get().Attributes.TryGet(selfModifier.AttributeId);
        string tooltip = $"{string.Format((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) attribute.Name, (object) selfModifier.GetFormattedString())}\n{string.Format((string) DUPLICANTS.MODIFIERS.TIME_TOTAL, (object) GameUtil.GetFormattedCycles(effect.duration))}";
        Sprite sprite = Assets.GetSprite((HashedString) attribute.uiFullColourSprite);
        option.AddPositiveIcon(sprite, tooltip, 1.75f);
      }
      return eventPopupData;
    }

    private void AssignPreSelectedMinionIfNeeded(BonusEvent.StatesInstance smi)
    {
      if (!smi.gameplayEvent.preSelectMinion || !((UnityEngine.Object) smi.sm.chosen.Get(smi) == (UnityEngine.Object) null))
        return;
      smi.sm.chosen.Set(smi.gameplayEvent.GetRandomMinionPrioritizeFiltered().gameObject, smi, false);
      smi.timesTriggered = 0;
    }

    private bool IsCorrectMinion(
      BonusEvent.StatesInstance smi,
      BonusEvent.GameplayEventData gameplayEventData)
    {
      if (!smi.gameplayEvent.preSelectMinion || !((UnityEngine.Object) smi.sm.chosen.Get(smi) != (UnityEngine.Object) gameplayEventData.worker.gameObject))
        return true;
      if ((double) GameUtil.GetCurrentTimeInCycles() - (double) smi.lastTriggered <= 5.0 || (double) smi.PercentageUntilTriggered() >= 0.5)
        return false;
      smi.sm.chosen.Set(gameplayEventData.worker.gameObject, smi, false);
      smi.timesTriggered = 0;
      return true;
    }

    private bool OtherConditionsAreSatisfied(
      BonusEvent.StatesInstance smi,
      BonusEvent.GameplayEventData gameplayEventData)
    {
      if (smi.gameplayEvent.roomRestrictions != null)
      {
        Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(gameplayEventData.worker.gameObject);
        if (roomOfGameObject == null || !smi.gameplayEvent.roomRestrictions.Contains(roomOfGameObject.roomType))
          return false;
        if (smi.gameplayEvent.roomHasOwnable)
        {
          bool flag = false;
          foreach (Component owner in roomOfGameObject.GetOwners())
          {
            if ((UnityEngine.Object) owner.gameObject == (UnityEngine.Object) gameplayEventData.worker.gameObject)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            return false;
        }
      }
      return smi.gameplayEvent.extraCondition == null || smi.gameplayEvent.extraCondition(gameplayEventData);
    }

    private bool IncrementAndTrigger(
      BonusEvent.StatesInstance smi,
      BonusEvent.GameplayEventData gameplayEventData)
    {
      ++smi.timesTriggered;
      smi.lastTriggered = GameUtil.GetCurrentTimeInCycles();
      if (smi.timesTriggered < smi.gameplayEvent.numTimesToTrigger)
        return false;
      smi.building = gameplayEventData.building;
      smi.sm.chosen.Set(gameplayEventData.worker.gameObject, smi, false);
      return true;
    }

    private bool BuildingEventTrigger(BonusEvent.StatesInstance smi, object data)
    {
      if (!(data is BonusEvent.GameplayEventData gameplayEventData))
        return false;
      this.AssignPreSelectedMinionIfNeeded(smi);
      return !((UnityEngine.Object) gameplayEventData.building == (UnityEngine.Object) null) && (smi.gameplayEvent.buildingTrigger.Count <= 0 || smi.gameplayEvent.buildingTrigger.Contains(gameplayEventData.building.prefabid.PrefabID())) && this.OtherConditionsAreSatisfied(smi, gameplayEventData) && this.IsCorrectMinion(smi, gameplayEventData) && this.IncrementAndTrigger(smi, gameplayEventData);
    }

    private bool WorkableEventTrigger(BonusEvent.StatesInstance smi, object data)
    {
      if (!(data is BonusEvent.GameplayEventData gameplayEventData))
        return false;
      this.AssignPreSelectedMinionIfNeeded(smi);
      return (smi.gameplayEvent.workableType.Count <= 0 || smi.gameplayEvent.workableType.Contains(gameplayEventData.workable.GetType())) && this.OtherConditionsAreSatisfied(smi, gameplayEventData) && this.IsCorrectMinion(smi, gameplayEventData) && this.IncrementAndTrigger(smi, gameplayEventData);
    }

    private bool ChosenMinionDied(BonusEvent.StatesInstance smi, object data)
    {
      return (UnityEngine.Object) smi.sm.chosen.Get(smi) == (UnityEngine.Object) (data as GameObject);
    }

    private Effect GetEffect(BonusEvent.StatesInstance smi)
    {
      GameObject gameObject = smi.sm.chosen.Get(smi);
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        return (Effect) null;
      EffectInstance effectInstance = gameObject.GetComponent<Effects>().Get(smi.gameplayEvent.effect);
      if (effectInstance != null)
        return effectInstance.effect;
      Debug.LogWarning((object) $"Effect {smi.gameplayEvent.effect} not found on {gameObject} in BonusEvent");
      return (Effect) null;
    }

    public class ActiveStates : 
      GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State
    {
      public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State notify;
      public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State seenNotification;
    }
  }

  public class StatesInstance : 
    GameplayEventStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, BonusEvent>.GameplayEventStateMachineInstance
  {
    [Serialize]
    public int timesTriggered;
    [Serialize]
    public float lastTriggered;
    public BuildingComplete building;

    public StatesInstance(
      GameplayEventManager master,
      GameplayEventInstance eventInstance,
      BonusEvent bonusEvent)
      : base(master, eventInstance, bonusEvent)
    {
      this.lastTriggered = GameUtil.GetCurrentTimeInCycles();
    }

    public float PercentageUntilTriggered()
    {
      return (float) this.timesTriggered / (float) this.smi.gameplayEvent.numTimesToTrigger;
    }
  }
}
