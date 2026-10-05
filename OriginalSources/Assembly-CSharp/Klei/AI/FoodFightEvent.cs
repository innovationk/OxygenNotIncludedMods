// Decompiled with JetBrains decompiler
// Type: Klei.AI.FoodFightEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Klei.AI;

public class FoodFightEvent : GameplayEvent<FoodFightEvent.StatesInstance>
{
  public const float FUTURE_TIME = 60f;
  public const float DURATION = 60f;

  public FoodFightEvent()
    : base("FoodFight")
  {
    this.title = (string) GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.NAME;
    this.description = (string) GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.DESCRIPTION;
  }

  public override StateMachine.Instance GetSMI(
    GameplayEventManager manager,
    GameplayEventInstance eventInstance)
  {
    return (StateMachine.Instance) new FoodFightEvent.StatesInstance(manager, eventInstance, this);
  }

  public class StatesInstance(
    GameplayEventManager master,
    GameplayEventInstance eventInstance,
    FoodFightEvent foodEvent) : 
    GameplayEventStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, FoodFightEvent>.GameplayEventStateMachineInstance(master, eventInstance, foodEvent)
  {
    public List<FoodFightChore> chores;

    public void CreateChores(FoodFightEvent.StatesInstance smi)
    {
      this.chores = new List<FoodFightChore>();
      List<Room> all = Game.Instance.roomProber.rooms.FindAll((Predicate<Room>) (match => match.roomType == Db.Get().RoomTypes.MessHall || match.roomType == Db.Get().RoomTypes.GreatHall));
      if (all == null || all.Count == 0)
        return;
      List<GameObject> buildingsOnFloor = all[UnityEngine.Random.Range(0, all.Count)].GetBuildingsOnFloor();
      for (int idx = 0; idx < Math.Min(Components.LiveMinionIdentities.Count, buildingsOnFloor.Count); ++idx)
      {
        MinionIdentity liveMinionIdentity = Components.LiveMinionIdentities[idx];
        GameObject gameObject = buildingsOnFloor[UnityEngine.Random.Range(0, buildingsOnFloor.Count)];
        GameObject locator = ChoreHelpers.CreateLocator("FoodFightLocator", gameObject.transform.position);
        GameObject locator1 = locator;
        FoodFightChore foodFightChore1 = new FoodFightChore((IStateMachineTarget) liveMinionIdentity, locator1);
        buildingsOnFloor.Remove(gameObject);
        FoodFightChore foodFightChore2 = foodFightChore1;
        foodFightChore2.onExit = foodFightChore2.onExit + (Action<Chore>) (data => Util.KDestroyGameObject(locator));
        this.chores.Add(foodFightChore1);
      }
    }

    public void ClearChores()
    {
      if (this.chores != null)
      {
        for (int index = this.chores.Count - 1; index >= 0; --index)
        {
          if (this.chores[index] != null)
            this.chores[index].Cancel("end");
        }
      }
      this.chores = (List<FoodFightChore>) null;
    }
  }

  public class States : 
    GameplayEventStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, FoodFightEvent>
  {
    public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State planning;
    public FoodFightEvent.States.WarmupStates warmup;
    public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State partying;
    public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State ending;
    public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State canceled;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      base.InitializeStates(out default_state);
      default_state = (StateMachine.BaseState) this.planning;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.root.Exit((StateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => smi.ClearChores()));
      this.planning.ToggleNotification((Func<FoodFightEvent.StatesInstance, Notification>) (smi => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi))));
      this.warmup.ToggleNotification((Func<FoodFightEvent.StatesInstance, Notification>) (smi => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi))));
      this.warmup.wait.ScheduleGoTo(60f, (StateMachine.BaseState) this.warmup.start);
      this.warmup.start.Enter((StateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => smi.CreateChores(smi))).Update((Action<FoodFightEvent.StatesInstance, float>) ((smi, data) =>
      {
        int num = 0;
        foreach (FoodFightChore chore in smi.chores)
        {
          if (chore.smi.IsInsideState((StateMachine.BaseState) chore.smi.sm.waitForParticipants))
            ++num;
        }
        if (num < smi.chores.Count && (double) smi.timeinstate <= 30.0)
          return;
        foreach (Chore chore in smi.chores)
          chore.gameObject.Trigger(-2043101269);
        smi.GoTo((StateMachine.BaseState) this.partying);
      }), UpdateRate.RENDER_1000ms);
      this.partying.ToggleNotification((Func<FoodFightEvent.StatesInstance, Notification>) (smi => new Notification((string) GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.UNDERWAY, NotificationType.Good, (Func<List<Notification>, object, string>) ((a, b) => (string) GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.UNDERWAY_TOOLTIP)))).ScheduleGoTo(60f, (StateMachine.BaseState) this.ending);
      this.ending.ReturnSuccess();
      this.canceled.DoNotification((Func<FoodFightEvent.StatesInstance, Notification>) (smi => GameplayEventManager.CreateStandardCancelledNotification(this.GenerateEventPopupData(smi)))).Enter((StateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi =>
      {
        foreach (Component liveMinionIdentity in Components.LiveMinionIdentities)
          liveMinionIdentity.GetComponent<Effects>().Add("NoFunAllowed", true);
      })).ReturnFailure();
    }

    public override EventInfoData GenerateEventPopupData(FoodFightEvent.StatesInstance smi)
    {
      EventInfoData eventPopupData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
      eventPopupData.location = (string) GAMEPLAY_EVENTS.LOCATIONS.PRINTING_POD;
      eventPopupData.whenDescription = string.Format((string) GAMEPLAY_EVENTS.TIMES.IN_CYCLES, (object) 0.1f);
      eventPopupData.AddOption((string) GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.ACCEPT_OPTION_NAME).callback = (System.Action) (() => smi.GoTo((StateMachine.BaseState) smi.sm.warmup.wait));
      eventPopupData.AddOption((string) GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.REJECT_OPTION_NAME).callback = (System.Action) (() => smi.GoTo((StateMachine.BaseState) smi.sm.canceled));
      return eventPopupData;
    }

    public class WarmupStates : 
      GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State
    {
      public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State wait;
      public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State start;
    }
  }
}
