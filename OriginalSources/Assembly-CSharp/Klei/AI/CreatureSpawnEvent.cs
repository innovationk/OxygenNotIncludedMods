// Decompiled with JetBrains decompiler
// Type: Klei.AI.CreatureSpawnEvent
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

public class CreatureSpawnEvent : GameplayEvent<CreatureSpawnEvent.StatesInstance>
{
  public const string ID = "HatchSpawnEvent";
  public const float UPDATE_TIME = 4f;
  public const float NUM_TO_SPAWN = 10f;
  public const float duration = 40f;
  public static List<string> CreatureSpawnEventIDs = new List<string>()
  {
    "Hatch",
    "Squirrel",
    "Puft",
    "Crab",
    "Drecko",
    "Mole",
    "LightBug",
    "Pacu"
  };

  public CreatureSpawnEvent()
    : base("HatchSpawnEvent")
  {
    this.title = (string) GAMEPLAY_EVENTS.EVENT_TYPES.CREATURE_SPAWN.NAME;
    this.description = (string) GAMEPLAY_EVENTS.EVENT_TYPES.CREATURE_SPAWN.DESCRIPTION;
  }

  public override StateMachine.Instance GetSMI(
    GameplayEventManager manager,
    GameplayEventInstance eventInstance)
  {
    return (StateMachine.Instance) new CreatureSpawnEvent.StatesInstance(manager, eventInstance, this);
  }

  public class StatesInstance(
    GameplayEventManager master,
    GameplayEventInstance eventInstance,
    CreatureSpawnEvent creatureEvent) : 
    GameplayEventStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, CreatureSpawnEvent>.GameplayEventStateMachineInstance(master, eventInstance, creatureEvent)
  {
    [Serialize]
    private List<Vector3> spawnPositions = new List<Vector3>();
    [Serialize]
    private string creatureID;

    private void PickCreatureToSpawn()
    {
      this.creatureID = CreatureSpawnEvent.CreatureSpawnEventIDs.GetRandom<string>();
    }

    private void PickSpawnLocations()
    {
      Vector3 position = Components.Telepads.Items.GetRandom<Telepad>().transform.GetPosition();
      int num = 100;
      ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
      GameScenePartitioner.Instance.GatherEntries((int) position.x - num / 2, (int) position.y - num / 2, num, num, GameScenePartitioner.Instance.plants, (List<ScenePartitionerEntry>) gathered_entries);
      foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
      {
        KPrefabID kprefabId = (KPrefabID) partitionerEntry.obj;
        if (!(bool) (UnityEngine.Object) kprefabId.GetComponent<TreeBud>())
          this.smi.spawnPositions.Add(kprefabId.transform.GetPosition());
      }
      gathered_entries.Recycle();
    }

    public void InitializeEvent()
    {
      this.PickCreatureToSpawn();
      this.PickSpawnLocations();
    }

    public void EndEvent()
    {
      this.creatureID = (string) null;
      this.spawnPositions.Clear();
    }

    public void SpawnCreature()
    {
      if (this.spawnPositions.Count <= 0)
        return;
      Vector3 random = this.spawnPositions.GetRandom<Vector3>();
      Util.KInstantiate(Assets.GetPrefab((Tag) this.creatureID), random).SetActive(true);
    }
  }

  public class States : 
    GameplayEventStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, CreatureSpawnEvent>
  {
    public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State initialize_event;
    public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State spawn_season;
    public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State start;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.initialize_event;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.initialize_event.Enter((StateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi =>
      {
        smi.InitializeEvent();
        smi.GoTo((StateMachine.BaseState) this.spawn_season);
      }));
      this.start.DoNothing();
      this.spawn_season.Update((Action<CreatureSpawnEvent.StatesInstance, float>) ((smi, dt) => smi.SpawnCreature()), UpdateRate.SIM_4000ms).Exit((StateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => smi.EndEvent()));
    }

    public override EventInfoData GenerateEventPopupData(CreatureSpawnEvent.StatesInstance smi)
    {
      return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
      {
        location = (string) GAMEPLAY_EVENTS.LOCATIONS.PRINTING_POD,
        whenDescription = (string) GAMEPLAY_EVENTS.TIMES.NOW
      };
    }
  }
}
