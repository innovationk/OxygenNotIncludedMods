// Decompiled with JetBrains decompiler
// Type: GameplayEventPreconditions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using Klei.CustomSettings;
using ProcGen;
using System;
using System.Linq;

#nullable disable
public class GameplayEventPreconditions
{
  private static GameplayEventPreconditions _instance;

  public static GameplayEventPreconditions Instance
  {
    get
    {
      if (GameplayEventPreconditions._instance == null)
        GameplayEventPreconditions._instance = new GameplayEventPreconditions();
      return GameplayEventPreconditions._instance;
    }
  }

  public GameplayEventPrecondition LiveMinions(int count = 1)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() => Components.LiveMinionIdentities.Count >= count),
      description = $"At least {count} dupes alive"
    };
  }

  public GameplayEventPrecondition BuildingExists(string buildingId, int count = 1)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() => BuildingInventory.Instance.BuildingCount(new Tag(buildingId)) >= count),
      description = $"{count} {buildingId} has been built"
    };
  }

  public GameplayEventPrecondition ResearchCompleted(string techName)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() => Research.Instance.Get(Db.Get().Techs.Get(techName)).IsComplete()),
      description = $"Has researched {techName}."
    };
  }

  public GameplayEventPrecondition AchievementUnlocked(ColonyAchievement achievement)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() => SaveGame.Instance.ColonyAchievementTracker.IsAchievementUnlocked(achievement)),
      description = $"Unlocked the {achievement.Id} achievement"
    };
  }

  public GameplayEventPrecondition RoomBuilt(RoomType roomType)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() => Game.Instance.roomProber.rooms.Exists((Predicate<Room>) (match => match.roomType == roomType))),
      description = $"Built a {roomType.Id} room"
    };
  }

  public GameplayEventPrecondition CycleRestriction(float min = 0.0f, float max = float.PositiveInfinity)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() => (double) GameUtil.GetCurrentTimeInCycles() >= (double) min && (double) GameUtil.GetCurrentTimeInCycles() <= (double) max),
      description = $"After cycle {min} and before cycle {max}"
    };
  }

  public GameplayEventPrecondition MinionsWithEffect(string effectId, int count = 1)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() => Components.LiveMinionIdentities.Items.Count<MinionIdentity>((Func<MinionIdentity, bool>) (minion => minion.GetComponent<Effects>().Get(effectId) != null)) >= count),
      description = $"At least {count} dupes have the {effectId} effect applied"
    };
  }

  public GameplayEventPrecondition MinionsWithStatusItem(StatusItem statusItem, int count = 1)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() => Components.LiveMinionIdentities.Items.Count<MinionIdentity>((Func<MinionIdentity, bool>) (minion => minion.GetComponent<KSelectable>().HasStatusItem(statusItem))) >= count),
      description = $"At least {count} dupes have the {statusItem} status item"
    };
  }

  public GameplayEventPrecondition MinionsWithChoreGroupPriorityOrGreater(
    ChoreGroup choreGroup,
    int count,
    int priority)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() => Components.LiveMinionIdentities.Items.Count<MinionIdentity>((Func<MinionIdentity, bool>) (minion =>
      {
        ChoreConsumer component = minion.GetComponent<ChoreConsumer>();
        return !component.IsChoreGroupDisabled(choreGroup) && component.GetPersonalPriority(choreGroup) >= priority;
      })) >= count),
      description = $"At least {count} dupes have their {choreGroup.Name} set to {priority} or higher."
    };
  }

  public GameplayEventPrecondition PastEventCount(string evtId, int count = 1)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() => GameplayEventManager.Instance.NumberOfPastEvents((HashedString) evtId) >= count),
      description = $"The {evtId} event has triggered {count} times."
    };
  }

  public GameplayEventPrecondition DifficultySetting(SettingConfig config, string levelId)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() => CustomGameSettings.Instance.GetCurrentQualitySetting(config).id == levelId),
      description = $"The config {config.id} is level {levelId}."
    };
  }

  public GameplayEventPrecondition ClusterHasTag(string tag)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() =>
      {
        ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
        return currentClusterLayout != null && currentClusterLayout.clusterTags.Contains(tag);
      }),
      description = $"The cluster is tagged with {tag}."
    };
  }

  public GameplayEventPrecondition PastEventCountAndNotActive(GameplayEvent evt, int count = 1)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() => GameplayEventManager.Instance.NumberOfPastEvents(evt.IdHash) >= count && !GameplayEventManager.Instance.IsGameplayEventActive(evt)),
      description = $"The {evt.Id} event has triggered {count} times and is not active."
    };
  }

  public GameplayEventPrecondition Not(GameplayEventPrecondition precondition)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() => !precondition.condition()),
      description = $"Not[{precondition.description}]"
    };
  }

  public GameplayEventPrecondition Or(
    GameplayEventPrecondition precondition1,
    GameplayEventPrecondition precondition2)
  {
    return new GameplayEventPrecondition()
    {
      condition = (GameplayEventPrecondition.PreconditionFn) (() => precondition1.condition() || precondition2.condition()),
      description = $"[{precondition1.description}]-OR-[{precondition2.description}]"
    };
  }
}
