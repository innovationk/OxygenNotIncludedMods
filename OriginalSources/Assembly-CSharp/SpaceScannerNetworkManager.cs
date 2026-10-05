// Decompiled with JetBrains decompiler
// Type: SpaceScannerNetworkManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serialize]
[SerializationConfig(MemberSerialization.OptIn)]
[Serializable]
public class SpaceScannerNetworkManager : ISim1000ms
{
  [Serialize]
  private Dictionary<int, SpaceScannerWorldData> worldIdToDataMap = new Dictionary<int, SpaceScannerWorldData>();
  private static List<GameplayEventInstance> meteorShowerInstances = new List<GameplayEventInstance>();

  public Dictionary<int, SpaceScannerWorldData> DEBUG_GetWorldIdToDataMap()
  {
    return this.worldIdToDataMap;
  }

  public bool IsTargetDetectedOnWorld(int worldId, SpaceScannerTarget target)
  {
    SpaceScannerWorldData scannerWorldData;
    return this.worldIdToDataMap.TryGetValue(worldId, out scannerWorldData) && scannerWorldData.targetIdsDetected.Contains(target.id);
  }

  public MathUtil.MinMax GetDetectTimeRangeForWorld(int worldId)
  {
    return SpaceScannerNetworkManager.GetDetectTimeRange(this.GetQualityForWorld(worldId));
  }

  public float GetQualityForWorld(int worldId)
  {
    SpaceScannerWorldData scannerWorldData;
    return this.worldIdToDataMap.TryGetValue(worldId, out scannerWorldData) ? scannerWorldData.networkQuality01 : 0.0f;
  }

  private SpaceScannerWorldData GetOrCreateWorldData(int worldId)
  {
    SpaceScannerWorldData worldData;
    if (!this.worldIdToDataMap.TryGetValue(worldId, out worldData))
    {
      worldData = new SpaceScannerWorldData(worldId);
      this.worldIdToDataMap[worldId] = worldData;
    }
    return worldData;
  }

  public void Sim1000ms(float dt)
  {
    SpaceScannerNetworkManager.UpdateWorldDataScratchpads(this.worldIdToDataMap);
    foreach (int worldsId in Components.DetectorNetworks.GetWorldsIds())
    {
      WorldContainer world = ClusterManager.Instance.GetWorld(worldsId);
      if (!world.IsModuleInterior && world.IsDiscovered)
      {
        SpaceScannerWorldData worldData = this.GetOrCreateWorldData(world.id);
        SpaceScannerNetworkManager.UpdateNetworkQualityFor(worldData);
        SpaceScannerNetworkManager.UpdateDetectionOfTargetsFor(worldData);
      }
    }
  }

  private static void UpdateNetworkQualityFor(SpaceScannerWorldData worldData)
  {
    float quality01 = SpaceScannerNetworkManager.CalcWorldNetworkQuality(worldData.GetWorld());
    foreach (DetectorNetwork.Instance orGetCmp in Components.DetectorNetworks.CreateOrGetCmps(worldData.GetWorld().id))
      orGetCmp.Internal_SetNetworkQuality(quality01);
    worldData.networkQuality01 = quality01;
  }

  private static void UpdateDetectionOfTargetsFor(SpaceScannerWorldData worldData)
  {
    using (HashSetPool<string, SpaceScannerNetworkManager>.PooledHashSet pooledHashSet1 = PoolsFor<SpaceScannerNetworkManager>.AllocateHashSet<string>())
    {
      using (HashSetPool<string, SpaceScannerNetworkManager>.PooledHashSet pooledHashSet2 = PoolsFor<SpaceScannerNetworkManager>.AllocateHashSet<string>())
      {
        foreach (string str in worldData.targetIdsDetected)
        {
          pooledHashSet1.Add(str);
          pooledHashSet2.Add(str);
        }
        worldData.targetIdsDetected.Clear();
        if (SpaceScannerNetworkManager.IsDetectingAnyMeteorShower(worldData))
          worldData.targetIdsDetected.Add(SpaceScannerTarget.MeteorShower().id);
        if (SpaceScannerNetworkManager.IsDetectingAnyBallisticObject(worldData))
          worldData.targetIdsDetected.Add(SpaceScannerTarget.BallisticObject().id);
        foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
        {
          if (SpaceScannerNetworkManager.IsDetectingRocketBaseGame(worldData, spacecraft.launchConditions))
            worldData.targetIdsDetected.Add(SpaceScannerTarget.RocketBaseGame(spacecraft.launchConditions).id);
        }
        foreach (Clustercraft clustercraft in Components.Clustercrafts)
        {
          if (SpaceScannerNetworkManager.IsDetectingRocketDlc1(worldData, clustercraft))
            worldData.targetIdsDetected.Add(SpaceScannerTarget.RocketDlc1(clustercraft).id);
        }
        foreach (string str in worldData.targetIdsDetected)
          pooledHashSet2.Add(str);
        foreach (string key in (HashSet<string>) pooledHashSet2)
        {
          bool flag = pooledHashSet1.Contains(key);
          if (!worldData.targetIdsDetected.Contains(key) & flag)
            worldData.targetIdToRandomValue01Map[key] = UnityEngine.Random.value;
        }
      }
    }
  }

  private static bool IsDetectingAnyMeteorShower(SpaceScannerWorldData worldData)
  {
    SpaceScannerNetworkManager.meteorShowerInstances.Clear();
    SaveGame.Instance.GetComponent<GameplayEventManager>().GetActiveEventsOfType<MeteorShowerEvent>(worldData.GetWorld().id, ref SpaceScannerNetworkManager.meteorShowerInstances);
    float detectTime = SpaceScannerNetworkManager.GetDetectTime(worldData, SpaceScannerTarget.MeteorShower());
    MeteorShowerEvent.StatesInstance candidateTarget = (MeteorShowerEvent.StatesInstance) null;
    float num1 = float.MaxValue;
    foreach (GameplayEventInstance meteorShowerInstance in SpaceScannerNetworkManager.meteorShowerInstances)
    {
      if (meteorShowerInstance.smi is MeteorShowerEvent.StatesInstance smi)
      {
        float num2 = smi.TimeUntilNextShower();
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          candidateTarget = smi;
        }
        if ((double) num2 <= (double) detectTime)
          worldData.scratchpad.lastDetectedMeteorShowers.Add(smi);
      }
    }
    return SpaceScannerNetworkManager.IsDetectedUsingStickyCheck<MeteorShowerEvent.StatesInstance>(candidateTarget, (double) num1 <= (double) detectTime, worldData.scratchpad.lastDetectedMeteorShowers);
  }

  private static bool IsDetectingAnyBallisticObject(SpaceScannerWorldData worldData)
  {
    float a = float.MaxValue;
    foreach (ClusterTraveler ballisticObject in worldData.scratchpad.ballisticObjects)
      a = Mathf.Min(a, ballisticObject.TravelETA());
    return (double) a < (double) SpaceScannerNetworkManager.GetDetectTime(worldData, SpaceScannerTarget.BallisticObject());
  }

  private static bool IsDetectingRocketBaseGame(
    SpaceScannerWorldData worldData,
    LaunchConditionManager rocket)
  {
    Spacecraft conditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(rocket);
    return SpaceScannerNetworkManager.IsDetectedUsingStickyCheck<LaunchConditionManager>(rocket, IsDetected(worldData, conditionManager, rocket), worldData.scratchpad.lastDetectedRocketsBaseGame);

    static bool IsDetected(
      SpaceScannerWorldData worldData,
      Spacecraft spacecraft,
      LaunchConditionManager rocket)
    {
      if (spacecraft.IsNullOrDestroyed() || spacecraft.state == Spacecraft.MissionState.Destroyed)
        return false;
      switch (spacecraft.state)
      {
        case Spacecraft.MissionState.Launching:
        case Spacecraft.MissionState.WaitingToLand:
        case Spacecraft.MissionState.Landing:
          return true;
        case Spacecraft.MissionState.Underway:
          return (double) spacecraft.GetTimeLeft() <= (double) SpaceScannerNetworkManager.GetDetectTime(worldData, SpaceScannerTarget.RocketBaseGame(rocket));
        case Spacecraft.MissionState.Destroyed:
          return false;
        default:
          return false;
      }
    }
  }

  private static bool IsDetectingRocketDlc1(
    SpaceScannerWorldData worldData,
    Clustercraft clustercraft)
  {
    if (clustercraft.IsNullOrDestroyed())
      return false;
    ClusterTraveler component = clustercraft.GetComponent<ClusterTraveler>();
    bool isDetected = false;
    if (clustercraft.Status != Clustercraft.CraftStatus.Grounded)
    {
      bool flag1 = component.GetDestinationWorldID() == worldData.GetWorld().id;
      bool flag2 = component.IsTraveling();
      bool move = clustercraft.HasResourcesToMove();
      float num = component.TravelETA();
      isDetected = flag1 & flag2 & move && (double) num < (double) SpaceScannerNetworkManager.GetDetectTime(worldData, SpaceScannerTarget.RocketDlc1(clustercraft)) || !flag2 & flag1 && clustercraft.Status == Clustercraft.CraftStatus.Landing;
      if (!isDetected)
      {
        ClusterGridEntity adjacentAsteroid = clustercraft.GetAdjacentAsteroid();
        isDetected = ((UnityEngine.Object) adjacentAsteroid != (UnityEngine.Object) null ? ClusterUtil.GetAsteroidWorldIdAtLocation(adjacentAsteroid.Location) : (int) byte.MaxValue) == worldData.GetWorld().id && clustercraft.Status == Clustercraft.CraftStatus.Launching;
      }
    }
    return SpaceScannerNetworkManager.IsDetectedUsingStickyCheck<Clustercraft>(clustercraft, isDetected, worldData.scratchpad.lastDetectedRocketsDLC1);
  }

  private static bool IsDetectedUsingStickyCheck<T>(
    T candidateTarget,
    bool isDetected,
    HashSet<T> existingDetections)
  {
    if (isDetected)
      existingDetections.Add(candidateTarget);
    else if (existingDetections.Contains(candidateTarget))
      isDetected = true;
    return isDetected;
  }

  private static float GetDetectTime(SpaceScannerWorldData worldData, SpaceScannerTarget target)
  {
    float t;
    if (!worldData.targetIdToRandomValue01Map.TryGetValue(target.id, out t))
    {
      t = UnityEngine.Random.value;
      worldData.targetIdToRandomValue01Map[target.id] = t;
    }
    return SpaceScannerNetworkManager.GetDetectTimeRange(worldData.networkQuality01).Lerp(t);
  }

  private static MathUtil.MinMax GetDetectTimeRange(float networkQuality01)
  {
    return new MathUtil.MinMax(Mathf.Lerp(1f, 200f, networkQuality01), 200f);
  }

  private static float CalcWorldNetworkQuality(WorldContainer world)
  {
    int width = world.Width;
    Debug.Assert(width <= 1024 /*0x0400*/, (object) "More world columns than expected");
    bool[] flagArray = new bool[width];
    for (int index = 0; index < width; ++index)
      flagArray[index] = false;
    using (HashSetPool<int, SpaceScannerNetworkManager>.PooledHashSet visibleCells = PoolsFor<SpaceScannerNetworkManager>.AllocateHashSet<int>())
    {
      foreach (DetectorNetwork.Instance orGetCmp in Components.DetectorNetworks.CreateOrGetCmps(world.id))
      {
        if (orGetCmp.GetComponent<Operational>().IsOperational)
          CometDetectorConfig.SKY_VISIBILITY_INFO.CollectVisibleCellsTo((HashSet<int>) visibleCells, Grid.PosToCell(orGetCmp.gameObject.transform.position), world);
      }
      foreach (int cell in (HashSet<int>) visibleCells)
      {
        int index = Grid.CellToXY(cell).x - world.WorldOffset.x;
        if (index >= 0 && index < world.Width)
          flagArray[index] = true;
      }
    }
    int num = 0;
    for (int index = 0; index < width; ++index)
    {
      if (flagArray[index])
        ++num;
    }
    return Mathf.Clamp01(((float) num / (float) width).Remap((0.0f, 0.5f), (0.0f, 1f)));
  }

  private static void UpdateWorldDataScratchpads(
    Dictionary<int, SpaceScannerWorldData> worldIdToDataMap)
  {
    foreach (KeyValuePair<int, SpaceScannerWorldData> worldIdToData in worldIdToDataMap)
    {
      int num;
      SpaceScannerWorldData scannerWorldData;
      worldIdToData.Deconstruct(ref num, ref scannerWorldData);
      SpaceScannerWorldData worldData = scannerWorldData;
      if (worldData.scratchpad == null)
        worldData.scratchpad = new SpaceScannerWorldData.Scratchpad();
      worldData.scratchpad.ballisticObjects.Clear();
      worldData.scratchpad.lastDetectedMeteorShowers.RemoveWhere((Predicate<MeteorShowerEvent.StatesInstance>) (meteorShower => meteorShower.IsNullOrDestroyed() || meteorShower.IsNullOrStopped() || 200.0 < (double) meteorShower.TimeUntilNextShower()));
      worldData.scratchpad.lastDetectedRocketsBaseGame.RemoveWhere((Predicate<LaunchConditionManager>) (rocket =>
      {
        if (rocket.IsNullOrDestroyed())
          return true;
        Spacecraft conditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(rocket);
        return conditionManager.IsNullOrDestroyed() || conditionManager.state == Spacecraft.MissionState.Destroyed || conditionManager.state == Spacecraft.MissionState.Underway && 200.0 < (double) conditionManager.GetTimeLeft() || (double) conditionManager.GetTimeLeft() < 1.0;
      }));
      worldData.scratchpad.lastDetectedRocketsDLC1.RemoveWhere((Predicate<Clustercraft>) (clustercraft =>
      {
        if (clustercraft.IsNullOrDestroyed())
          return true;
        ClusterTraveler component = clustercraft.GetComponent<ClusterTraveler>();
        return component.IsNullOrDestroyed() || component.IsTraveling() && (component.GetDestinationWorldID() != worldData.worldId || 200.0 < (double) component.TravelETA()) || (double) component.TravelETA() < 1.0;
      }));
    }
    if (Components.DetectorNetworks.GetWorldsIds().Count == 0)
      return;
    foreach (ClusterTraveler clusterTraveler in Components.ClusterTravelers)
    {
      SpaceScannerWorldData scannerWorldData;
      if (clusterTraveler.IsTraveling() && clusterTraveler.GetComponent<Clustercraft>().IsNullOrDestroyed() && worldIdToDataMap.TryGetValue(clusterTraveler.GetDestinationWorldID(), out scannerWorldData))
        scannerWorldData.scratchpad.ballisticObjects.Add(clusterTraveler);
    }
  }
}
