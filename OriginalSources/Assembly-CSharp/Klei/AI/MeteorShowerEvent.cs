// Decompiled with JetBrains decompiler
// Type: Klei.AI.MeteorShowerEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Klei.AI;

public class MeteorShowerEvent : GameplayEvent<MeteorShowerEvent.StatesInstance>
{
  private List<MeteorShowerEvent.BombardmentInfo> bombardmentInfo;
  private MathUtil.MinMax secondsBombardmentOff;
  private MathUtil.MinMax secondsBombardmentOn;
  private float secondsPerMeteor = 0.33f;
  private float duration;
  private string clusterMapMeteorShowerID;
  private bool affectedByDifficulty = true;

  public bool canStarTravel
  {
    get => this.clusterMapMeteorShowerID != null && DlcManager.FeatureClusterSpaceEnabled();
  }

  public string GetClusterMapMeteorShowerID() => this.clusterMapMeteorShowerID;

  public List<MeteorShowerEvent.BombardmentInfo> GetMeteorsInfo()
  {
    return new List<MeteorShowerEvent.BombardmentInfo>((IEnumerable<MeteorShowerEvent.BombardmentInfo>) this.bombardmentInfo);
  }

  public MeteorShowerEvent(
    string id,
    float duration,
    float secondsPerMeteor,
    MathUtil.MinMax secondsBombardmentOff = default (MathUtil.MinMax),
    MathUtil.MinMax secondsBombardmentOn = default (MathUtil.MinMax),
    string clusterMapMeteorShowerID = null,
    bool affectedByDifficulty = true)
    : base(id)
  {
    this.allowMultipleEventInstances = true;
    this.clusterMapMeteorShowerID = clusterMapMeteorShowerID;
    this.duration = duration;
    this.secondsPerMeteor = secondsPerMeteor;
    this.secondsBombardmentOff = secondsBombardmentOff;
    this.secondsBombardmentOn = secondsBombardmentOn;
    this.affectedByDifficulty = affectedByDifficulty;
    this.bombardmentInfo = new List<MeteorShowerEvent.BombardmentInfo>();
    this.tags.Add(GameTags.SpaceDanger);
  }

  public MeteorShowerEvent AddMeteor(string prefab, float weight)
  {
    this.bombardmentInfo.Add(new MeteorShowerEvent.BombardmentInfo()
    {
      prefab = prefab,
      weight = weight
    });
    return this;
  }

  public override StateMachine.Instance GetSMI(
    GameplayEventManager manager,
    GameplayEventInstance eventInstance)
  {
    return (StateMachine.Instance) new MeteorShowerEvent.StatesInstance(manager, eventInstance, this);
  }

  public override bool IsAllowed()
  {
    if (!base.IsAllowed())
      return false;
    return !this.affectedByDifficulty || CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers).id != "ClearSkies";
  }

  public struct BombardmentInfo
  {
    public string prefab;
    public float weight;
  }

  public class States : 
    GameplayEventStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, MeteorShowerEvent>
  {
    public MeteorShowerEvent.States.ClusterMapStates starMap;
    public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State planning;
    public MeteorShowerEvent.States.RunningStates running;
    public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State finished;
    public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.TargetParameter clusterMapMeteorShower;
    public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter runTimeRemaining;
    public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter bombardTimeRemaining;
    public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter snoozeTimeRemaining;
    public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.Signal OnClusterMapDestinationReached;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      base.InitializeStates(out default_state);
      default_state = (StateMachine.BaseState) this.planning;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.planning.Enter((StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi =>
      {
        double num1 = (double) this.runTimeRemaining.Set(smi.gameplayEvent.duration, smi);
        double num2 = (double) this.bombardTimeRemaining.Set(smi.GetBombardOnTime(), smi);
        double num3 = (double) this.snoozeTimeRemaining.Set(smi.GetBombardOffTime(), smi);
        if (smi.gameplayEvent.canStarTravel && (double) smi.clusterTravelDuration > 0.0)
          smi.GoTo((StateMachine.BaseState) smi.sm.starMap);
        else
          smi.GoTo((StateMachine.BaseState) smi.sm.running);
      }));
      this.starMap.Enter(new StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State.Callback(MeteorShowerEvent.States.CreateClusterMapMeteorShower)).DefaultState(this.starMap.travelling);
      this.starMap.travelling.OnSignal(this.OnClusterMapDestinationReached, this.starMap.arrive);
      this.starMap.arrive.GoTo(this.running.bombarding);
      double num4;
      this.running.DefaultState(this.running.snoozing).Update((Action<MeteorShowerEvent.StatesInstance, float>) ((smi, dt) => num4 = (double) this.runTimeRemaining.Delta(-dt, smi))).ParamTransition<float>((StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.Parameter<float>) this.runTimeRemaining, this.finished, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero);
      double num5;
      double num6;
      this.running.bombarding.Enter((StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => MeteorShowerEvent.States.TriggerMeteorGlobalEvent(smi, GameHashes.MeteorShowerBombardStateBegins))).Exit((StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => MeteorShowerEvent.States.TriggerMeteorGlobalEvent(smi, GameHashes.MeteorShowerBombardStateEnds))).Enter((StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => smi.StartBackgroundEffects())).Exit((StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => smi.StopBackgroundEffects())).Exit((StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => num5 = (double) this.bombardTimeRemaining.Set(smi.GetBombardOnTime(), smi))).Update((Action<MeteorShowerEvent.StatesInstance, float>) ((smi, dt) => num6 = (double) this.bombardTimeRemaining.Delta(-dt, smi))).ParamTransition<float>((StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.Parameter<float>) this.bombardTimeRemaining, this.running.snoozing, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero).Update((Action<MeteorShowerEvent.StatesInstance, float>) ((smi, dt) => smi.Bombarding(dt)));
      double num7;
      double num8;
      this.running.snoozing.Exit((StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => num7 = (double) this.snoozeTimeRemaining.Set(smi.GetBombardOffTime(), smi))).Update((Action<MeteorShowerEvent.StatesInstance, float>) ((smi, dt) => num8 = (double) this.snoozeTimeRemaining.Delta(-dt, smi))).ParamTransition<float>((StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.Parameter<float>) this.snoozeTimeRemaining, this.running.bombarding, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero);
      this.finished.ReturnSuccess();
    }

    public static void TriggerMeteorGlobalEvent(
      MeteorShowerEvent.StatesInstance smi,
      GameHashes hash)
    {
      Game.Instance.BoxingTrigger<int>((int) hash, smi.eventInstance.worldId);
    }

    public static void CreateClusterMapMeteorShower(MeteorShowerEvent.StatesInstance smi)
    {
      if ((UnityEngine.Object) smi.sm.clusterMapMeteorShower.Get(smi) == (UnityEngine.Object) null)
      {
        GameObject prefab = Assets.GetPrefab(smi.gameplayEvent.clusterMapMeteorShowerID.ToTag());
        float num = smi.eventInstance.eventStartTime * 600f + smi.clusterTravelDuration;
        AxialI atEdgeOfUniverse = ClusterGrid.Instance.GetRandomCellAtEdgeOfUniverse();
        GameObject go = Util.KInstantiate(prefab);
        go.GetComponent<ClusterMapMeteorShowerVisualizer>().SetInitialLocation(atEdgeOfUniverse);
        ClusterMapMeteorShower.Def def = go.AddOrGetDef<ClusterMapMeteorShower.Def>();
        def.destinationWorldID = smi.eventInstance.worldId;
        def.arrivalTime = num;
        go.SetActive(true);
        smi.sm.clusterMapMeteorShower.Set(go, smi, false);
      }
      GameObject go1 = smi.sm.clusterMapMeteorShower.Get(smi);
      go1.GetDef<ClusterMapMeteorShower.Def>();
      go1.Subscribe(1796608350, new Action<object>(smi.OnClusterMapDestinationReached));
    }

    public class ClusterMapStates : 
      GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State
    {
      public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State travelling;
      public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State arrive;
    }

    public class RunningStates : 
      GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State
    {
      public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State bombarding;
      public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State snoozing;
    }
  }

  public class StatesInstance : 
    GameplayEventStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, MeteorShowerEvent>.GameplayEventStateMachineInstance
  {
    public GameObject activeMeteorBackground;
    [Serialize]
    public float clusterTravelDuration = -1f;
    [Serialize]
    private float nextMeteorTime;
    [Serialize]
    private int m_worldId;
    private WorldContainer world;
    private SettingLevel difficultyLevel;

    public float GetSleepTimerValue()
    {
      return Mathf.Clamp(GameplayEventManager.Instance.GetSleepTimer((GameplayEvent) this.gameplayEvent) - GameUtil.GetCurrentTimeInCycles(), 0.0f, float.MaxValue);
    }

    public StatesInstance(
      GameplayEventManager master,
      GameplayEventInstance eventInstance,
      MeteorShowerEvent meteorShowerEvent)
      : base(master, eventInstance, meteorShowerEvent)
    {
      this.world = ClusterManager.Instance.GetWorld(this.m_worldId);
      this.difficultyLevel = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
      this.m_worldId = eventInstance.worldId;
      Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
    }

    public void OnClusterMapDestinationReached(object obj)
    {
      this.smi.sm.OnClusterMapDestinationReached.Trigger(this);
    }

    private void OnActiveWorldChanged(object data)
    {
      int first = ((Tuple<int, int>) data).first;
      if (!((UnityEngine.Object) this.activeMeteorBackground != (UnityEngine.Object) null))
        return;
      this.activeMeteorBackground.GetComponent<ParticleSystemRenderer>().enabled = first == this.m_worldId;
    }

    public override void StopSM(string reason)
    {
      this.StopBackgroundEffects();
      base.StopSM(reason);
    }

    protected override void OnCleanUp()
    {
      Game.Instance.Unsubscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
      this.DestroyClusterMapMeteorShowerObject();
      base.OnCleanUp();
    }

    private void DestroyClusterMapMeteorShowerObject()
    {
      if (!((UnityEngine.Object) this.sm.clusterMapMeteorShower.Get(this) != (UnityEngine.Object) null))
        return;
      ClusterMapMeteorShower.Instance smi = this.sm.clusterMapMeteorShower.Get(this).GetSMI<ClusterMapMeteorShower.Instance>();
      if (smi == null)
        return;
      smi.StopSM("Event is being aborted");
      Util.KDestroyGameObject(smi.gameObject);
    }

    public void StartBackgroundEffects()
    {
      if (!((UnityEngine.Object) this.activeMeteorBackground == (UnityEngine.Object) null))
        return;
      this.activeMeteorBackground = Util.KInstantiate(EffectPrefabs.Instance.MeteorBackground);
      this.activeMeteorBackground.transform.SetPosition(new Vector3((float) (((double) this.world.maximumBounds.x + (double) this.world.minimumBounds.x) / 2.0), this.world.maximumBounds.y, 25f));
      this.activeMeteorBackground.transform.rotation = Quaternion.Euler(90f, 0.0f, 0.0f);
    }

    public void StopBackgroundEffects()
    {
      if (!((UnityEngine.Object) this.activeMeteorBackground != (UnityEngine.Object) null))
        return;
      ParticleSystem component = this.activeMeteorBackground.GetComponent<ParticleSystem>();
      component.main.stopAction = ParticleSystemStopAction.Destroy;
      component.Stop();
      if (!component.IsAlive())
        UnityEngine.Object.Destroy((UnityEngine.Object) this.activeMeteorBackground);
      this.activeMeteorBackground = (GameObject) null;
    }

    public float TimeUntilNextShower()
    {
      if (this.IsInsideState((StateMachine.BaseState) this.sm.running.bombarding))
        return 0.0f;
      if (!this.IsInsideState((StateMachine.BaseState) this.sm.starMap))
        return this.sm.snoozeTimeRemaining.Get(this);
      float num = (float) ((double) this.smi.eventInstance.eventStartTime * 600.0 + (double) this.smi.clusterTravelDuration - (double) GameUtil.GetCurrentTimeInCycles() * 600.0);
      return (double) num >= 0.0 ? num : 0.0f;
    }

    public void Bombarding(float dt)
    {
      for (this.nextMeteorTime -= dt; (double) this.nextMeteorTime < 0.0; this.nextMeteorTime += this.GetNextMeteorTime())
      {
        if ((double) this.GetSleepTimerValue() <= 0.0)
          this.DoBombardment(this.gameplayEvent.bombardmentInfo);
      }
    }

    private void DoBombardment(
      List<MeteorShowerEvent.BombardmentInfo> bombardment_info)
    {
      float maxInclusive = 0.0f;
      foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo in bombardment_info)
        maxInclusive += bombardmentInfo.weight;
      float num1 = UnityEngine.Random.Range(0.0f, maxInclusive);
      MeteorShowerEvent.BombardmentInfo bombardmentInfo1 = bombardment_info[0];
      int num2 = 0;
      for (; (double) num1 - (double) bombardmentInfo1.weight > 0.0; bombardmentInfo1 = bombardment_info[++num2])
        num1 -= bombardmentInfo1.weight;
      Game.Instance.Trigger(-84771526, (object) null);
      this.SpawnBombard(bombardmentInfo1.prefab);
    }

    private GameObject SpawnBombard(string prefab)
    {
      WorldContainer world = ClusterManager.Instance.GetWorld(this.m_worldId);
      Vector3 position = new Vector3((float) (world.Width - 1) * UnityEngine.Random.value + (float) world.WorldOffset.x, (float) (world.Height + world.WorldOffset.y - 1), Grid.GetLayerZ(Grid.SceneLayer.FXFront));
      GameObject prefab1 = Assets.GetPrefab((Tag) prefab);
      if ((UnityEngine.Object) prefab1 == (UnityEngine.Object) null)
        return (GameObject) null;
      GameObject gameObject = Util.KInstantiate(prefab1, position, Quaternion.identity);
      Comet component = gameObject.GetComponent<Comet>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.spawnWithOffset = true;
      gameObject.SetActive(true);
      return gameObject;
    }

    public float BombardTimeRemaining()
    {
      return Mathf.Min(this.sm.bombardTimeRemaining.Get(this), this.sm.runTimeRemaining.Get(this));
    }

    public float GetBombardOffTime()
    {
      float bombardOffTime = this.gameplayEvent.secondsBombardmentOff.Get();
      if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
      {
        switch (this.difficultyLevel.id)
        {
          case "Infrequent":
            bombardOffTime *= 1f;
            break;
          case "Intense":
            bombardOffTime *= 1f;
            break;
          case "Doomed":
            bombardOffTime *= 0.5f;
            break;
        }
      }
      return bombardOffTime;
    }

    public float GetBombardOnTime()
    {
      float bombardOnTime = this.gameplayEvent.secondsBombardmentOn.Get();
      if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
      {
        switch (this.difficultyLevel.id)
        {
          case "Infrequent":
            bombardOnTime *= 1f;
            break;
          case "Intense":
            bombardOnTime *= 1f;
            break;
          case "Doomed":
            bombardOnTime *= 1f;
            break;
        }
      }
      return bombardOnTime;
    }

    private float GetNextMeteorTime()
    {
      float nextMeteorTime = this.gameplayEvent.secondsPerMeteor * (256f / (float) this.world.Width);
      if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
      {
        switch (this.difficultyLevel.id)
        {
          case "Infrequent":
            nextMeteorTime *= 1.5f;
            break;
          case "Intense":
            nextMeteorTime *= 0.8f;
            break;
          case "Doomed":
            nextMeteorTime *= 0.5f;
            break;
        }
      }
      return nextMeteorTime;
    }
  }
}
