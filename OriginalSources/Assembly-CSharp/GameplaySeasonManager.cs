// Decompiled with JetBrains decompiler
// Type: GameplaySeasonManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
public class GameplaySeasonManager : 
  GameStateMachine<GameplaySeasonManager, GameplaySeasonManager.Instance, IStateMachineTarget, GameplaySeasonManager.Def>
{
  public override void InitializeStates(out StateMachine.BaseState defaultState)
  {
    defaultState = (StateMachine.BaseState) this.root;
    this.root.Enter((StateMachine<GameplaySeasonManager, GameplaySeasonManager.Instance, IStateMachineTarget, GameplaySeasonManager.Def>.State.Callback) (smi => smi.Initialize())).Update((System.Action<GameplaySeasonManager.Instance, float>) ((smi, dt) => smi.Update(dt)), UpdateRate.SIM_4000ms);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<GameplaySeasonManager, GameplaySeasonManager.Instance, IStateMachineTarget, GameplaySeasonManager.Def>.GameInstance
  {
    [Serialize]
    public List<GameplaySeasonInstance> activeSeasons;
    [MyCmpGet]
    private WorldContainer m_worldContainer;

    public Instance(IStateMachineTarget master, GameplaySeasonManager.Def def)
      : base(master, def)
    {
      this.activeSeasons = new List<GameplaySeasonInstance>();
    }

    public void Initialize()
    {
      this.activeSeasons.RemoveAll((Predicate<GameplaySeasonInstance>) (item => item.Season == null));
      List<GameplaySeason> gameplaySeasonList = new List<GameplaySeason>();
      if ((UnityEngine.Object) this.m_worldContainer != (UnityEngine.Object) null)
      {
        ClusterGridEntity component = this.GetComponent<ClusterGridEntity>();
        foreach (string seasonId in this.m_worldContainer.GetSeasonIds())
        {
          GameplaySeason gameplaySeason = Db.Get().GameplaySeasons.TryGet(seasonId);
          if (gameplaySeason == null)
          {
            Debug.LogWarning((object) $"world {component.name} has invalid season {seasonId}");
          }
          else
          {
            if (gameplaySeason.type != GameplaySeason.Type.World)
              Debug.LogWarning((object) $"world {component.name} has specified season {seasonId}, which is not a world type season");
            gameplaySeasonList.Add(gameplaySeason);
          }
        }
      }
      else
      {
        Debug.Assert((UnityEngine.Object) this.GetComponent<SaveGame>() != (UnityEngine.Object) null);
        gameplaySeasonList = Db.Get().GameplaySeasons.resources.Where<GameplaySeason>((Func<GameplaySeason, bool>) (season => season.type == GameplaySeason.Type.Cluster)).ToList<GameplaySeason>();
      }
      foreach (GameplaySeason gameplaySeason in gameplaySeasonList)
      {
        if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) gameplaySeason) && gameplaySeason.startActive && !this.SeasonExists(gameplaySeason) && gameplaySeason.events.Count > 0)
          this.activeSeasons.Add(gameplaySeason.Instantiate(this.GetWorldId()));
      }
      foreach (GameplaySeasonInstance gameplaySeasonInstance in new List<GameplaySeasonInstance>((IEnumerable<GameplaySeasonInstance>) this.activeSeasons))
      {
        if (!gameplaySeasonList.Contains(gameplaySeasonInstance.Season) || !Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) gameplaySeasonInstance.Season))
          this.activeSeasons.Remove(gameplaySeasonInstance);
      }
    }

    private int GetWorldId()
    {
      return (UnityEngine.Object) this.m_worldContainer != (UnityEngine.Object) null ? this.m_worldContainer.id : -1;
    }

    public void Update(float dt)
    {
      foreach (GameplaySeasonInstance activeSeason in this.activeSeasons)
      {
        if (activeSeason.ShouldGenerateEvents() && (double) GameUtil.GetCurrentTimeInCycles() > (double) activeSeason.NextEventTime)
        {
          int num = 0;
          while (true)
          {
            if (num < activeSeason.Season.numEventsToStartEachPeriod && activeSeason.StartEvent())
              ++num;
            else
              goto label_6;
          }
        }
        else
          continue;
label_6:;
      }
    }

    public void StartNewSeason(GameplaySeason seasonType)
    {
      if (!Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) seasonType))
        return;
      this.activeSeasons.Add(seasonType.Instantiate(this.GetWorldId()));
    }

    public bool SeasonExists(GameplaySeason seasonType)
    {
      return this.activeSeasons.Find((Predicate<GameplaySeasonInstance>) (e => e.Season.IdHash == seasonType.IdHash)) != null;
    }
  }
}
