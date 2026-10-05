// Decompiled with JetBrains decompiler
// Type: ClusterManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using ProcGenGame;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ClusterManager : KMonoBehaviour, ISaveLoadable
{
  public static int MAX_ROCKET_INTERIOR_COUNT = 16 /*0x10*/;
  public static ClusterManager.RocketStatesForAudio RocketInteriorState = ClusterManager.RocketStatesForAudio.Grounded;
  public static ClusterManager Instance;
  private ClusterGrid m_grid;
  [Serialize]
  private int m_numRings = 9;
  [Serialize]
  private int activeWorldIdx;
  public const byte INVALID_WORLD_IDX = 255 /*0xFF*/;
  public static Color[] worldColors = new Color[6]
  {
    Color.HSVToRGB(0.15f, 0.3f, 0.5f),
    Color.HSVToRGB(0.3f, 0.3f, 0.5f),
    Color.HSVToRGB(0.45f, 0.3f, 0.5f),
    Color.HSVToRGB(0.6f, 0.3f, 0.5f),
    Color.HSVToRGB(0.75f, 0.3f, 0.5f),
    Color.HSVToRGB(0.9f, 0.3f, 0.5f)
  };
  private List<WorldContainer> m_worldContainers = new List<WorldContainer>();
  [MyCmpGet]
  private ClusterPOIManager m_clusterPOIsManager;
  private Dictionary<int, List<IAssignableIdentity>> minionsByWorld = new Dictionary<int, List<IAssignableIdentity>>();
  private MinionMigrationEventArgs migrationEvArg = new MinionMigrationEventArgs();
  private MigrationEventArgs critterMigrationEvArg = new MigrationEventArgs();
  private List<int> _worldIDs = new List<int>();
  private List<int> _discoveredAsteroidIds = new List<int>();
  private Tuple<int, int> activeWorldChangedData = new Tuple<int, int>(0, 0);

  public static void DestroyInstance() => ClusterManager.Instance = (ClusterManager) null;

  public int worldCount => this.m_worldContainers.Count;

  public int activeWorldId => this.activeWorldIdx;

  public List<WorldContainer> WorldContainers => this.m_worldContainers;

  public ClusterPOIManager GetClusterPOIManager() => this.m_clusterPOIsManager;

  public Dictionary<int, List<IAssignableIdentity>> MinionsByWorld
  {
    get
    {
      this.minionsByWorld.Clear();
      for (int idx = 0; idx < Components.MinionAssignablesProxy.Count; ++idx)
      {
        if (!Components.MinionAssignablesProxy[idx].GetTargetGameObject().HasTag(GameTags.Dead))
        {
          int id = Components.MinionAssignablesProxy[idx].GetTargetGameObject().GetComponent<KMonoBehaviour>().GetMyWorld().id;
          if (!this.minionsByWorld.ContainsKey(id))
            this.minionsByWorld.Add(id, new List<IAssignableIdentity>());
          this.minionsByWorld[id].Add((IAssignableIdentity) Components.MinionAssignablesProxy[idx]);
        }
      }
      return this.minionsByWorld;
    }
  }

  public void RegisterWorldContainer(WorldContainer worldContainer)
  {
    this.m_worldContainers.Add(worldContainer);
  }

  public void UnregisterWorldContainer(WorldContainer worldContainer)
  {
    this.BoxingTrigger<int>(-1078710002, worldContainer.id);
    this.m_worldContainers.Remove(worldContainer);
  }

  public List<int> GetWorldIDsSorted()
  {
    ListPool<WorldContainer, ClusterManager>.PooledList pooledList = ListPool<WorldContainer, ClusterManager>.Allocate(this.m_worldContainers);
    pooledList.Sort((Comparison<WorldContainer>) ((a, b) => a.DiscoveryTimestamp.CompareTo(b.DiscoveryTimestamp)));
    this._worldIDs.Clear();
    foreach (WorldContainer worldContainer in (List<WorldContainer>) pooledList)
      this._worldIDs.Add(worldContainer.id);
    pooledList.Recycle();
    return this._worldIDs;
  }

  public List<int> GetDiscoveredAsteroidIDsSorted()
  {
    ListPool<WorldContainer, ClusterManager>.PooledList pooledList = ListPool<WorldContainer, ClusterManager>.Allocate(this.m_worldContainers);
    pooledList.Sort((Comparison<WorldContainer>) ((a, b) => a.DiscoveryTimestamp.CompareTo(b.DiscoveryTimestamp)));
    this._discoveredAsteroidIds.Clear();
    for (int index = 0; index < pooledList.Count; ++index)
    {
      if (pooledList[index].IsDiscovered && !pooledList[index].IsModuleInterior)
        this._discoveredAsteroidIds.Add(pooledList[index].id);
    }
    pooledList.Recycle();
    return this._discoveredAsteroidIds;
  }

  public WorldContainer GetStartWorld()
  {
    foreach (WorldContainer worldContainer in this.WorldContainers)
    {
      if (worldContainer.IsStartWorld)
        return worldContainer;
    }
    return this.WorldContainers[0];
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    ClusterManager.Instance = this;
    SaveLoader.Instance.OnWorldGenComplete += new Action<Cluster>(this.OnWorldGenComplete);
  }

  protected override void OnSpawn()
  {
    if (this.m_grid == null)
      this.m_grid = new ClusterGrid(this.m_numRings);
    this.UpdateWorldReverbSnapshot(this.activeWorldId);
    base.OnSpawn();
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  public WorldContainer activeWorld => this.GetWorld(this.activeWorldId);

  private void OnWorldGenComplete(Cluster clusterLayout)
  {
    this.m_numRings = clusterLayout.numRings;
    this.m_grid = new ClusterGrid(this.m_numRings);
    AxialI location = AxialI.ZERO;
    foreach (WorldGen world in clusterLayout.worlds)
    {
      int id = this.CreateAsteroidWorldContainer(world).id;
      Vector2I position = world.GetPosition();
      Vector2I vector2I = position + world.GetSize();
      if (world.isStartingWorld)
        location = world.GetClusterLocation();
      for (int y = position.y; y < vector2I.y; ++y)
      {
        for (int x = position.x; x < vector2I.x; ++x)
        {
          int cell = Grid.XYToCell(x, y);
          Grid.WorldIdx[cell] = (byte) id;
          Pathfinding.Instance.AddDirtyNavGridCell(cell);
        }
      }
      if (world.isStartingWorld)
        this.activeWorldIdx = id;
    }
    this.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(location, 1);
    this.m_clusterPOIsManager.PopulatePOIsFromWorldGen(clusterLayout);
  }

  private int GetNextWorldId()
  {
    HashSetPool<int, ClusterManager>.PooledHashSet pooledHashSet = HashSetPool<int, ClusterManager>.Allocate();
    foreach (WorldContainer worldContainer in this.m_worldContainers)
      pooledHashSet.Add(worldContainer.id);
    Debug.Assert(this.m_worldContainers.Count < (int) byte.MaxValue, (object) "Oh no! We're trying to generate our 255th world in this save, things are going to start going badly...");
    for (int nextWorldId = 0; nextWorldId < (int) byte.MaxValue; ++nextWorldId)
    {
      if (!pooledHashSet.Contains(nextWorldId))
      {
        pooledHashSet.Recycle();
        return nextWorldId;
      }
    }
    pooledHashSet.Recycle();
    return (int) byte.MaxValue;
  }

  private WorldContainer CreateAsteroidWorldContainer(WorldGen world)
  {
    int nextWorldId = this.GetNextWorldId();
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) "Asteroid"));
    WorldContainer component1 = gameObject.GetComponent<WorldContainer>();
    component1.SetID(nextWorldId);
    component1.SetWorldDetails(world);
    AsteroidGridEntity component2 = gameObject.GetComponent<AsteroidGridEntity>();
    if (world != null)
    {
      AxialI clusterLocation = world.GetClusterLocation();
      component2.Init(component1.GetRandomName(), clusterLocation, world.Settings.world.asteroidIcon);
    }
    else
      component2.Init("", AxialI.ZERO, "");
    if (component1.IsStartWorld)
    {
      OrbitalMechanics component3 = gameObject.GetComponent<OrbitalMechanics>();
      if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
        component3.CreateOrbitalObject(Db.Get().OrbitalTypeCategories.backgroundEarth.Id);
    }
    gameObject.SetActive(true);
    return component1;
  }

  private void CreateDefaultAsteroidWorldContainer()
  {
    if (this.m_worldContainers.Count != 0)
      return;
    Debug.LogWarning((object) "Cluster manager has no world containers, create a default using Grid settings.");
    WorldContainer asteroidWorldContainer = this.CreateAsteroidWorldContainer((WorldGen) null);
    int id = asteroidWorldContainer.id;
    for (int y = (int) asteroidWorldContainer.minimumBounds.y; (double) y <= (double) asteroidWorldContainer.maximumBounds.y; ++y)
    {
      for (int x = (int) asteroidWorldContainer.minimumBounds.x; (double) x <= (double) asteroidWorldContainer.maximumBounds.x; ++x)
      {
        int cell = Grid.XYToCell(x, y);
        Grid.WorldIdx[cell] = (byte) id;
        Pathfinding.Instance.AddDirtyNavGridCell(cell);
      }
    }
  }

  public void InitializeWorldGrid()
  {
    if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
      this.CreateDefaultAsteroidWorldContainer();
    bool flag = false;
    foreach (WorldContainer worldContainer in this.m_worldContainers)
    {
      Vector2I worldOffset = worldContainer.WorldOffset;
      Vector2I vector2I = worldOffset + worldContainer.WorldSize;
      for (int y = worldOffset.y; y < vector2I.y; ++y)
      {
        for (int x = worldOffset.x; x < vector2I.x; ++x)
        {
          int cell = Grid.XYToCell(x, y);
          Grid.WorldIdx[cell] = (byte) worldContainer.id;
          Pathfinding.Instance.AddDirtyNavGridCell(cell);
        }
      }
      flag |= worldContainer.IsDiscovered;
    }
    if (flag)
      return;
    Debug.LogWarning((object) "No worlds have been discovered. Setting the active world to discovered");
    this.activeWorld.SetDiscovered();
  }

  public void SetActiveWorld(int worldIdx)
  {
    int activeWorldIdx = this.activeWorldIdx;
    if (activeWorldIdx == worldIdx)
      return;
    this.activeWorldIdx = worldIdx;
    this.activeWorldChangedData.first = this.activeWorldIdx;
    this.activeWorldChangedData.second = activeWorldIdx;
    Game.Instance.Trigger(1983128072, (object) this.activeWorldChangedData);
    this.UpdateRocketInteriorAudio();
  }

  public void TimelapseModeOverrideActiveWorld(int overrideValue)
  {
    this.activeWorldIdx = overrideValue;
  }

  public WorldContainer GetWorld(int id)
  {
    for (int index = 0; index < this.m_worldContainers.Count; ++index)
    {
      if (this.m_worldContainers[index].id == id)
        return this.m_worldContainers[index];
    }
    return (WorldContainer) null;
  }

  public WorldContainer GetWorldFromPosition(Vector3 position)
  {
    foreach (WorldContainer worldContainer in this.m_worldContainers)
    {
      if (worldContainer.ContainsPoint((Vector2) position))
        return worldContainer;
    }
    return (WorldContainer) null;
  }

  public float CountAllRations()
  {
    float num1 = 0.0f;
    foreach (WorldContainer worldContainer in this.m_worldContainers)
    {
      double num2 = (double) WorldResourceAmountTracker<RationTracker>.Get().CountAmount((Dictionary<string, float>) null, worldContainer.worldInventory);
    }
    return num1;
  }

  public Dictionary<Tag, float> GetAllWorldsAccessibleAmounts()
  {
    Dictionary<Tag, float> accessibleAmounts = new Dictionary<Tag, float>();
    foreach (WorldContainer worldContainer in this.m_worldContainers)
    {
      foreach (KeyValuePair<Tag, float> accessibleAmount in worldContainer.worldInventory.GetAccessibleAmounts())
      {
        if (accessibleAmounts.ContainsKey(accessibleAmount.Key))
          accessibleAmounts[accessibleAmount.Key] += accessibleAmount.Value;
        else
          accessibleAmounts.Add(accessibleAmount.Key, accessibleAmount.Value);
      }
    }
    return accessibleAmounts;
  }

  public void MigrateMinion(MinionIdentity minion, int targetID)
  {
    this.MigrateMinion(minion, targetID, minion.GetMyWorldId());
  }

  public void MigrateCritter(GameObject critter, int targetID)
  {
    this.MigrateCritter(critter, targetID, critter.GetMyWorldId());
  }

  public void MigrateCritter(GameObject critter, int targetID, int prevID)
  {
    this.critterMigrationEvArg.entity = critter;
    this.critterMigrationEvArg.prevWorldId = prevID;
    this.critterMigrationEvArg.targetWorldId = targetID;
    Game.Instance.Trigger(1142724171, (object) this.critterMigrationEvArg);
  }

  public void MigrateMinion(MinionIdentity minion, int targetID, int prevID)
  {
    if (!ClusterManager.Instance.GetWorld(targetID).IsDiscovered)
      ClusterManager.Instance.GetWorld(targetID).SetDiscovered();
    if (!ClusterManager.Instance.GetWorld(targetID).IsDupeVisited)
      ClusterManager.Instance.GetWorld(targetID).SetDupeVisited();
    this.migrationEvArg.minionId = minion;
    this.migrationEvArg.prevWorldId = prevID;
    this.migrationEvArg.targetWorldId = targetID;
    Game.Instance.assignmentManager.RemoveFromWorld((IAssignableIdentity) minion, this.migrationEvArg.prevWorldId);
    Game.Instance.Trigger(586301400, (object) this.migrationEvArg);
  }

  public int GetLandingBeaconLocation(int worldId)
  {
    foreach (LandingBeacon.Instance landingBeacon in Components.LandingBeacons)
    {
      if (landingBeacon.GetMyWorldId() == worldId && landingBeacon.CanBeTargeted())
        return Grid.PosToCell((StateMachine.Instance) landingBeacon);
    }
    return Grid.InvalidCell;
  }

  public int GetRandomClearCell(int worldId)
  {
    bool flag = false;
    int num1 = 0;
    while (!flag && num1 < 1000)
    {
      ++num1;
      int randomClearCell = UnityEngine.Random.Range(0, Grid.CellCount);
      if (!Grid.Solid[randomClearCell] && !Grid.IsLiquid(randomClearCell) && (int) Grid.WorldIdx[randomClearCell] == worldId)
        return randomClearCell;
    }
    int num2 = 0;
    while (!flag && num2 < 1000)
    {
      ++num2;
      int i = UnityEngine.Random.Range(0, Grid.CellCount);
      if (!Grid.Solid[i] && (int) Grid.WorldIdx[i] == worldId)
        return i;
    }
    return Grid.InvalidCell;
  }

  private bool NotObstructedCell(int x, int y)
  {
    int cell = Grid.XYToCell(x, y);
    return Grid.IsValidCell(cell) && (UnityEngine.Object) Grid.Objects[cell, 1] == (UnityEngine.Object) null;
  }

  private int LowestYThatSeesSky(int topCellYPos, int x)
  {
    int y = topCellYPos;
    while (!this.ValidSurfaceCell(x, y))
      --y;
    return y;
  }

  private bool ValidSurfaceCell(int x, int y)
  {
    int cell = Grid.XYToCell(x, y - 1);
    return Grid.Solid[cell] || Grid.Foundation[cell];
  }

  public int GetRandomSurfaceCell(int worldID, int width = 1, bool excludeTopBorderHeight = true)
  {
    WorldContainer worldContainer = this.m_worldContainers.Find((Predicate<WorldContainer>) (match => match.id == worldID));
    int num1 = Mathf.RoundToInt(UnityEngine.Random.Range(worldContainer.minimumBounds.x + (float) (worldContainer.Width / 10), worldContainer.maximumBounds.x - (float) (worldContainer.Width / 10)));
    int topCellYPos = Mathf.RoundToInt(worldContainer.maximumBounds.y);
    if (excludeTopBorderHeight)
      topCellYPos -= Grid.TopBorderHeight;
    int x = num1;
    int y1 = this.LowestYThatSeesSky(topCellYPos, x);
    int num2 = !this.NotObstructedCell(x, y1) ? 0 : 1;
    while (x + 1 != num1 && num2 < width)
    {
      ++x;
      if ((double) x > (double) worldContainer.maximumBounds.x)
      {
        num2 = 0;
        x = (int) worldContainer.minimumBounds.x;
      }
      int y2 = this.LowestYThatSeesSky(topCellYPos, x);
      bool flag = this.NotObstructedCell(x, y2);
      if (y2 == y1 & flag)
        ++num2;
      else
        num2 = !flag ? 0 : 1;
      y1 = y2;
    }
    return num2 < width ? -1 : Grid.XYToCell(x, y1);
  }

  public bool IsPositionInActiveWorld(Vector3 pos)
  {
    if ((UnityEngine.Object) this.activeWorld != (UnityEngine.Object) null && !CameraController.Instance.ignoreClusterFX)
    {
      Vector2 vector2_1 = this.activeWorld.maximumBounds * Grid.CellSizeInMeters + new Vector2(1f, 1f);
      Vector2 vector2_2 = this.activeWorld.minimumBounds * Grid.CellSizeInMeters;
      if ((double) pos.x < (double) vector2_2.x || (double) pos.x > (double) vector2_1.x || (double) pos.y < (double) vector2_2.y || (double) pos.y > (double) vector2_1.y)
        return false;
    }
    return true;
  }

  public WorldContainer CreateRocketInteriorWorld(
    GameObject craft_go,
    string interiorTemplateName,
    System.Action callback)
  {
    Vector2I rocketInteriorSize = ROCKETRY.ROCKET_INTERIOR_SIZE;
    Vector2I offset;
    if (Grid.GetFreeGridSpace(rocketInteriorSize, out offset))
    {
      int nextWorldId = this.GetNextWorldId();
      craft_go.AddComponent<WorldInventory>();
      WorldContainer rocketInteriorWorld = craft_go.AddComponent<WorldContainer>();
      rocketInteriorWorld.SetRocketInteriorWorldDetails(nextWorldId, rocketInteriorSize, offset);
      Vector2I vector2I = offset + rocketInteriorSize;
      for (int y = offset.y; y < vector2I.y; ++y)
      {
        for (int x = offset.x; x < vector2I.x; ++x)
        {
          int cell = Grid.XYToCell(x, y);
          Grid.WorldIdx[cell] = (byte) nextWorldId;
          Pathfinding.Instance.AddDirtyNavGridCell(cell);
        }
      }
      Debug.Log((object) $"Created new rocket interior id: {nextWorldId}, at {offset} with size {rocketInteriorSize}");
      rocketInteriorWorld.PlaceInteriorTemplate(interiorTemplateName, (System.Action) (() =>
      {
        if (callback != null)
          callback();
        craft_go.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.RocketInteriorComplete, (object) null);
      }));
      craft_go.AddOrGet<OrbitalMechanics>().CreateOrbitalObject(Db.Get().OrbitalTypeCategories.landed.Id);
      this.BoxingTrigger<int>(-1280433810, rocketInteriorWorld.id);
      return rocketInteriorWorld;
    }
    Debug.LogError((object) "Failed to create rocket interior.");
    return (WorldContainer) null;
  }

  public void DestoryRocketInteriorWorld(int world_id, ClustercraftExteriorDoor door)
  {
    WorldContainer world = this.GetWorld(world_id);
    if ((UnityEngine.Object) world == (UnityEngine.Object) null || !world.IsModuleInterior)
    {
      Debug.LogError((object) $"Attempting to destroy world id {world_id}. The world is not a valid rocket interior");
    }
    else
    {
      GameObject gameObject = door.GetComponent<RocketModuleCluster>().CraftInterface.gameObject;
      if (this.activeWorldId == world_id)
      {
        if (gameObject.GetComponent<WorldContainer>().ParentWorldId == world_id)
          this.SetActiveWorld(ClusterManager.Instance.GetStartWorld().id);
        else
          this.SetActiveWorld(gameObject.GetComponent<WorldContainer>().ParentWorldId);
      }
      OrbitalMechanics component = gameObject.GetComponent<OrbitalMechanics>();
      if (!component.IsNullOrDestroyed())
        UnityEngine.Object.Destroy((UnityEngine.Object) component);
      int num = gameObject.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.InFlight ? 1 : 0;
      PrimaryElement moduleElemet = door.GetComponent<PrimaryElement>();
      AxialI clusterLocation = world.GetComponent<ClusterGridEntity>().Location;
      Vector3 rocketModuleWorldPos = door.transform.position;
      if (num == 0)
        world.EjectAllDupes(rocketModuleWorldPos);
      else
        world.SpacePodAllDupes(clusterLocation, moduleElemet.ElementID);
      world.CancelChores();
      HashSet<int> noRefundTiles;
      world.DestroyWorldBuildings(out noRefundTiles);
      this.UnregisterWorldContainer(world);
      if (num == 0)
      {
        GameScheduler.Instance.ScheduleNextFrame("ClusterManager.world.TransferResourcesToParentWorld", (Action<object>) (obj => world.TransferResourcesToParentWorld(rocketModuleWorldPos + new Vector3(0.0f, 0.5f, 0.0f), noRefundTiles)));
        GameScheduler.Instance.ScheduleNextFrame("ClusterManager.DeleteWorldObjects", (Action<object>) (obj => this.DeleteWorldObjects(world)));
      }
      else
      {
        GameScheduler.Instance.ScheduleNextFrame("ClusterManager.world.TransferResourcesToDebris", (Action<object>) (obj => world.TransferResourcesToDebris(clusterLocation, noRefundTiles, moduleElemet.ElementID)));
        GameScheduler.Instance.ScheduleNextFrame("ClusterManager.DeleteWorldObjects", (Action<object>) (obj => this.DeleteWorldObjects(world)));
      }
    }
  }

  public void UpdateWorldReverbSnapshot(int worldId)
  {
    if (DlcManager.FeatureClusterSpaceEnabled())
    {
      AudioMixer.instance.Stop(AudioMixerSnapshots.Get().SmallRocketInteriorReverbSnapshot);
      AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MediumRocketInteriorReverbSnapshot);
    }
    AudioMixer.instance.PauseSpaceVisibleSnapshot(false);
    WorldContainer world = this.GetWorld(worldId);
    if (!world.IsModuleInterior)
      return;
    AudioMixer.instance.Start(world.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().interiorReverbSnapshot);
    AudioMixer.instance.PauseSpaceVisibleSnapshot(true);
    this.UpdateRocketInteriorAudio();
  }

  public void UpdateRocketInteriorAudio()
  {
    WorldContainer activeWorld = this.activeWorld;
    if (!((UnityEngine.Object) activeWorld != (UnityEngine.Object) null) || !activeWorld.IsModuleInterior)
      return;
    Vector3 vector3 = (Vector3) (activeWorld.minimumBounds + new Vector2((float) activeWorld.Width * Grid.CellSizeInMeters, (float) activeWorld.Height * Grid.CellSizeInMeters) / 2f);
    Clustercraft component = activeWorld.GetComponent<Clustercraft>();
    ClusterManager.RocketStatesForAudio rocketStatesForAudio = ClusterManager.RocketStatesForAudio.Grounded;
    switch (component.Status)
    {
      case Clustercraft.CraftStatus.Grounded:
        rocketStatesForAudio = component.LaunchRequested ? ClusterManager.RocketStatesForAudio.ReadyForLaunch : ClusterManager.RocketStatesForAudio.Grounded;
        break;
      case Clustercraft.CraftStatus.Launching:
        rocketStatesForAudio = ClusterManager.RocketStatesForAudio.Launching;
        break;
      case Clustercraft.CraftStatus.InFlight:
        rocketStatesForAudio = ClusterManager.RocketStatesForAudio.InSpace;
        break;
      case Clustercraft.CraftStatus.Landing:
        rocketStatesForAudio = ClusterManager.RocketStatesForAudio.Landing;
        break;
    }
    ClusterManager.RocketInteriorState = rocketStatesForAudio;
  }

  private void DeleteWorldObjects(WorldContainer world)
  {
    Grid.FreeGridSpace(world.WorldSize, world.WorldOffset);
    WorldInventory worldInventory = (WorldInventory) null;
    if ((UnityEngine.Object) world != (UnityEngine.Object) null)
      worldInventory = world.GetComponent<WorldInventory>();
    if ((UnityEngine.Object) worldInventory != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) worldInventory);
    if (!((UnityEngine.Object) world != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) world);
  }

  public enum RocketStatesForAudio
  {
    Grounded,
    ReadyForLaunch,
    Launching,
    InSpace,
    Landing,
  }
}
