// Decompiled with JetBrains decompiler
// Type: LargeImpactorEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using KSerialization;
using ProcGen;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LargeImpactorEvent(string id, string[] requiredDlcIds, string[] forbiddenDlcIds) : 
  GameplayEvent<LargeImpactorEvent.StatesInstance>(id, requiredDlcIds: requiredDlcIds, forbiddenDlcIds: forbiddenDlcIds)
{
  public override StateMachine.Instance GetSMI(
    GameplayEventManager manager,
    GameplayEventInstance eventInstance)
  {
    return (StateMachine.Instance) new LargeImpactorEvent.StatesInstance(manager, eventInstance, this);
  }

  private static void SpawnIridiumShowers(LargeImpactorEvent.StatesInstance smi)
  {
    GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.IridiumShowerEvent, smi.eventInstance.worldId);
  }

  private static void PreventDemoliorFragmentsBGFromPlaying(LargeImpactorEvent.StatesInstance smi)
  {
    TerrainBG.preventLargeImpactorFragmentsFromProgressing = true;
  }

  private static void AllowDemoliorFragmentsBGFromPlaying(LargeImpactorEvent.StatesInstance smi)
  {
    TerrainBG.preventLargeImpactorFragmentsFromProgressing = false;
  }

  private static void DestroyEventInstance(LargeImpactorEvent.StatesInstance smi)
  {
    smi.eventInstance.smi.StopSM("end");
  }

  private static bool WasWinAchievementAlreadyGranted(LargeImpactorEvent.StatesInstance smi)
  {
    return SaveGame.Instance.ColonyAchievementTracker.IsAchievementUnlocked(Db.Get().ColonyAchievements.AsteroidDestroyed);
  }

  private static void UnlockWinAchievement(LargeImpactorEvent.StatesInstance smi)
  {
    SaveGame.Instance.ColonyAchievementTracker.largeImpactorState = ColonyAchievementTracker.LargeImpactorState.Defeated;
  }

  private static void RegisterDemoliorSize(LargeImpactorEvent.StatesInstance smi)
  {
    ParallaxBackgroundObject component = smi.impactorInstance.GetComponent<ParallaxBackgroundObject>();
    SaveGame.Instance.ColonyAchievementTracker.LargeImpactorBackgroundScale = component.lastScaleUsed;
  }

  private static void RegisterLandedCycle(LargeImpactorEvent.StatesInstance smi)
  {
    SaveGame.Instance.ColonyAchievementTracker.largeImpactorState = ColonyAchievementTracker.LargeImpactorState.Landed;
    SaveGame.Instance.ColonyAchievementTracker.largeImpactorLandedCycle = GameClock.Instance.GetCycle();
  }

  private static bool IsSuitablePOISpawnLocation(AxialI location)
  {
    if (!ClusterGrid.Instance.IsValidCell(location))
      return false;
    foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.GetEntitiesOnCell(location))
    {
      if (clusterGridEntity.Layer == EntityLayer.Asteroid || clusterGridEntity.Layer == EntityLayer.POI)
        return false;
    }
    return true;
  }

  private static List<AxialI> FindAvailablePOISpawnLocations(AxialI location)
  {
    List<AxialI> poiSpawnLocations = new List<AxialI>();
    if (LargeImpactorEvent.IsSuitablePOISpawnLocation(location))
      poiSpawnLocations.Add(location);
    for (int index = 1; index <= 2; ++index)
    {
      foreach (AxialI axialI in AxialI.DIRECTIONS)
      {
        AxialI location1 = location + axialI * index;
        if (LargeImpactorEvent.IsSuitablePOISpawnLocation(location1))
          poiSpawnLocations.Add(location1);
      }
    }
    return poiSpawnLocations;
  }

  private static void SpawnPOI(string id, AxialI location)
  {
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) id));
    gameObject.GetComponent<HarvestablePOIClusterGridEntity>().Init(location);
    gameObject.SetActive(true);
  }

  private static void HandleInterception(LargeImpactorEvent.StatesInstance smi)
  {
    if (DlcManager.IsExpansion1Active())
    {
      List<AxialI> poiSpawnLocations = LargeImpactorEvent.FindAvailablePOISpawnLocations(smi.impactorInstance.GetSMI<ClusterMapLargeImpactor.Instance>().ClusterGridPosition());
      if (poiSpawnLocations.Count > 0)
        LargeImpactorEvent.SpawnPOI("HarvestableSpacePOI_DLC4ImpactorDebrisField1", poiSpawnLocations[0]);
      if (poiSpawnLocations.Count > 1)
        LargeImpactorEvent.SpawnPOI("HarvestableSpacePOI_DLC4ImpactorDebrisField2", poiSpawnLocations[1]);
      if (poiSpawnLocations.Count > 2)
        LargeImpactorEvent.SpawnPOI("HarvestableSpacePOI_DLC4ImpactorDebrisField3", poiSpawnLocations[2]);
    }
    else
    {
      if (!SpacecraftManager.instance.AddDestination(Db.Get().SpaceDestinationTypes.DLC4PrehistoricDemoliorSpaceDestination.Id, SpacecraftManager.DestinationLocationSelectionType.Nearest))
        SpacecraftManager.instance.AddDestination(Db.Get().SpaceDestinationTypes.DLC4PrehistoricDemoliorSpaceDestination.Id, SpacecraftManager.DestinationLocationSelectionType.Random, maxPerDistance: 5);
      if (!SpacecraftManager.instance.AddDestination(Db.Get().SpaceDestinationTypes.DLC4PrehistoricDemoliorSpaceDestination2.Id, SpacecraftManager.DestinationLocationSelectionType.Random, 1, 5, 5))
        SpacecraftManager.instance.AddDestination(Db.Get().SpaceDestinationTypes.DLC4PrehistoricDemoliorSpaceDestination2.Id, SpacecraftManager.DestinationLocationSelectionType.Random, maxPerDistance: 5);
      if (!SpacecraftManager.instance.AddDestination(Db.Get().SpaceDestinationTypes.DLC4PrehistoricDemoliorSpaceDestination3.Id, SpacecraftManager.DestinationLocationSelectionType.Random, 1, 5, 5))
        SpacecraftManager.instance.AddDestination(Db.Get().SpaceDestinationTypes.DLC4PrehistoricDemoliorSpaceDestination3.Id, SpacecraftManager.DestinationLocationSelectionType.Random, maxPerDistance: 5);
    }
    smi.GoTo((StateMachine.BaseState) smi.sm.finished);
  }

  private static bool WasKilled(LargeImpactorEvent.StatesInstance smi, object _)
  {
    return smi.impactorInstance.GetSMI<LargeImpactorStatus.Instance>().Health <= 0;
  }

  private static void PrepareForLargeImpactorDefeatedSequence(LargeImpactorEvent.StatesInstance smi)
  {
    smi.impactorInstance.GetComponent<LargeImpactorCrashStamp>();
    LargeImpactorEvent.ToggleOffLandingZoneVisualizer(smi);
    ClusterManager.Instance.GetWorld(smi.eventInstance.worldId).RevealSurface();
  }

  private static void InitializeLandingSequence(LargeImpactorEvent.StatesInstance smi)
  {
    GameObject impactorInstance = smi.impactorInstance;
    LargeImpactorCrashStamp component1 = impactorInstance.GetComponent<LargeImpactorCrashStamp>();
    ParallaxBackgroundObject component2 = impactorInstance.GetComponent<ParallaxBackgroundObject>();
    LargeImpactorEvent.ToggleOffLandingZoneVisualizer(smi);
    WorldContainer world = ClusterManager.Instance.GetWorld(smi.eventInstance.worldId);
    world.RevealHiddenY();
    world.RevealSurface();
    component1.RevealFogOfWar(7);
    component2.SetVisibilityState(false);
    LargeComet worldFallingAsteroid = LargeImpactorEvent.CreateLargeImpactorInWorldFallingAsteroid(smi, component1, world);
    LargeImpactorLandingSequence.Start((KMonoBehaviour) component1, worldFallingAsteroid, component1, world.id);
  }

  private static void ToggleOffLandingZoneVisualizer(LargeImpactorEvent.StatesInstance smi)
  {
    LargeImpactorVisualizer component = smi.impactorInstance.GetComponent<LargeImpactorVisualizer>();
    if (!component.Active)
      return;
    component.Active = false;
  }

  private static LargeComet CreateLargeImpactorInWorldFallingAsteroid(
    LargeImpactorEvent.StatesInstance smi,
    LargeImpactorCrashStamp crashStamp,
    WorldContainer world)
  {
    TemplateContainer asteroidTemplate = crashStamp.asteroidTemplate;
    Vector2I stampLocation = crashStamp.stampLocation;
    float layerZ = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
    Vector3 position = new Vector3((float) stampLocation.X, (float) (world.Height - world.HiddenYOffset - 1), layerZ);
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) LargeImpactorCometConfig.ID), position, Quaternion.identity);
    LargeComet component = gameObject.GetComponent<LargeComet>();
    gameObject.SetActive(true);
    component.stampLocation = stampLocation;
    component.crashPosition = stampLocation;
    component.crashPosition.y += asteroidTemplate.GetTemplateBounds().yMin;
    component.asteroidTemplate = asteroidTemplate;
    component.bottomCellsOffsetOfTemplate = crashStamp.TemplateBottomCellsOffsets;
    return component;
  }

  private static GameObject CreateSpacedOutImpactorInstance(LargeImpactorEvent.StatesInstance smi)
  {
    if (!DlcManager.IsExpansion1Active() || ClusterGrid.Instance == null)
      return (GameObject) null;
    GameObject go = Util.KInstantiate(Assets.GetPrefab((Tag) "LargeImpactor"));
    float num = smi.eventInstance.eventStartTime * 600f + LargeImpactorEvent.GetImpactTime();
    AxialI location = ClusterManager.Instance.GetClusterPOIManager().GetTemporalTear().Location;
    ClusterMapMeteorShowerVisualizer component = go.GetComponent<ClusterMapMeteorShowerVisualizer>();
    component.SetInitialLocation(location);
    component.forceRevealed = true;
    ClusterMapLargeImpactor.Def def = go.AddOrGetDef<ClusterMapLargeImpactor.Def>();
    def.destinationWorldID = 0;
    def.arrivalTime = num;
    go.AddOrGet<ParallaxBackgroundObject>().worldId = new int?(smi.eventInstance.worldId);
    return go;
  }

  private static GameObject CreateVanillaImpactorInstance(LargeImpactorEvent.StatesInstance smi)
  {
    return DlcManager.IsExpansion1Active() ? (GameObject) null : Util.KInstantiate(Assets.GetPrefab((Tag) LargeImpactorVanillaConfig.ID));
  }

  public static void CreateImpactorInstance(LargeImpactorEvent.StatesInstance smi)
  {
    GameObject gameObject = !DlcManager.IsExpansion1Active() ? LargeImpactorEvent.CreateVanillaImpactorInstance(smi) : LargeImpactorEvent.CreateSpacedOutImpactorInstance(smi);
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
    {
      KCrashReporter.ReportDevNotification("Failed to create LargeImpactor Object.", Environment.StackTrace);
      smi.StopSM("No Impactor created");
    }
    else
    {
      gameObject.SetActive(true);
      smi.sm.impactorTarget.Set((KMonoBehaviour) gameObject.GetComponent<KPrefabID>(), smi);
    }
  }

  public static float GetImpactTime()
  {
    ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
    if (currentClusterLayout != null && currentClusterLayout.clusterTags.Contains("DemoliorImminentImpact"))
      return 6000f;
    float num1;
    float num2 = num1 = 200f;
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.DemoliorDifficulty);
    if (currentQualitySetting.id == "VeryHard")
      num2 = 100f;
    else if (currentQualitySetting.id == "Hard")
      num2 = 150f;
    else if (currentQualitySetting.id == "Easy")
      num2 = 300f;
    else if (currentQualitySetting.id == "VeryEasy")
      num2 = 500f;
    return num2 * 600f;
  }

  public class States : 
    GameplayEventStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, LargeImpactorEvent>
  {
    public GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State start;
    public GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State create;
    public GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State clusterMap;
    public GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State killedByPlayer;
    public GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State impacting;
    public GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State finished;
    [Serialize]
    public StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.TargetParameter impactorTarget = new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.TargetParameter();

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = StateMachine.SerializeType.ParamsOnly;
      base.InitializeStates(out default_state);
      default_state = (StateMachine.BaseState) this.start;
      this.start.ParamTransition<GameObject>((StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.Parameter<GameObject>) this.impactorTarget, this.create, GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.IsNull).ParamTransition<GameObject>((StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.Parameter<GameObject>) this.impactorTarget, this.clusterMap, GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.IsNotNull);
      this.create.ParamTransition<GameObject>((StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.Parameter<GameObject>) this.impactorTarget, this.clusterMap, GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.IsNotNull).Enter((StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => LargeImpactorEvent.CreateImpactorInstance(smi)));
      this.clusterMap.Target(this.impactorTarget).EventTransition(GameHashes.LargeImpactorArrived, this.impacting).EventTransition(GameHashes.Died, this.killedByPlayer);
      this.impacting.EnterTransition(this.finished, new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.Transition.ConditionCallback(LargeImpactorEvent.WasWinAchievementAlreadyGranted)).Enter(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.RegisterLandedCycle)).Enter(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.InitializeLandingSequence)).Target(this.impactorTarget).EventTransition(GameHashes.SequenceCompleted, this.finished);
      this.killedByPlayer.EnterTransition(this.finished, new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.Transition.ConditionCallback(LargeImpactorEvent.WasWinAchievementAlreadyGranted)).Enter(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.PrepareForLargeImpactorDefeatedSequence)).Enter(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.PreventDemoliorFragmentsBGFromPlaying)).Enter(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.UnlockWinAchievement)).Enter(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.RegisterDemoliorSize)).Exit(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.AllowDemoliorFragmentsBGFromPlaying)).Exit(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.SpawnIridiumShowers)).Target(this.impactorTarget).EventHandler(GameHashes.SequenceCompleted, new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.HandleInterception));
      this.finished.Enter((StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => Util.KDestroyGameObject(smi.sm.impactorTarget.Get(smi)))).Enter(new StateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State.Callback(LargeImpactorEvent.DestroyEventInstance)).GoTo((GameStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, object>.State) null);
    }
  }

  public class StatesInstance(
    GameplayEventManager master,
    GameplayEventInstance eventInstance,
    LargeImpactorEvent largeImpactorEvent) : 
    GameplayEventStateMachine<LargeImpactorEvent.States, LargeImpactorEvent.StatesInstance, GameplayEventManager, LargeImpactorEvent>.GameplayEventStateMachineInstance(master, eventInstance, largeImpactorEvent)
  {
    public GameObject impactorInstance => this.sm.impactorTarget.Get(this.smi);
  }
}
