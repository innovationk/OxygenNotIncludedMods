// Decompiled with JetBrains decompiler
// Type: ProcGenGame.Cluster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using ProcGen;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

#nullable disable
namespace ProcGenGame;

[Serializable]
public class Cluster
{
  public List<WorldGen> worlds = new List<WorldGen>();
  public WorldGen currentWorld;
  public Vector2I size;
  public string Id;
  public int numRings = 5;
  public bool worldTraitsEnabled;
  public bool assertMissingTraits;
  public Dictionary<ClusterLayoutSave.POIType, List<AxialI>> poiLocations = new Dictionary<ClusterLayoutSave.POIType, List<AxialI>>();
  public Dictionary<AxialI, string> poiPlacements = new Dictionary<AxialI, string>();
  private int seed;
  private SeededRandom myRandom;
  private bool doSimSettle = true;
  [NonSerialized]
  public Action<int, WorldGen> PerWorldGenBeginCallback;
  [NonSerialized]
  public Action<int, WorldGen, WorldgenSimData> PerWorldGenCompleteCallback;
  [NonSerialized]
  public Func<int, WorldGen, bool> ShouldSkipWorldCallback;
  [NonSerialized]
  public List<WorldTrait> unplacedStoryTraits;
  [NonSerialized]
  public List<string> chosenStoryTraitIds;
  private MutatedClusterLayout mutatedClusterLayout;
  [NonSerialized]
  private Stopwatch worldgenDebugTimer = new Stopwatch();
  private Thread thread;
  private bool ApplicationIsPlaying;

  public ClusterLayout clusterLayout => this.mutatedClusterLayout.layout;

  public Cluster.WorldGenStatus Status { get; private set; }

  public bool HasGenerationStopped
  {
    get
    {
      return this.Status == Cluster.WorldGenStatus.Failed || this.Status == Cluster.WorldGenStatus.Complete;
    }
  }

  private Cluster()
  {
  }

  public Cluster(
    string clusterName,
    int seed,
    List<string> chosenStoryTraitIds,
    bool assertMissingTraits,
    bool skipWorldTraits,
    bool isRunningWorldgenDebug = false)
  {
    DebugUtil.Assert(!string.IsNullOrEmpty(clusterName), "Cluster file is missing");
    this.seed = seed;
    this.Id = clusterName;
    this.assertMissingTraits = assertMissingTraits;
    this.worldTraitsEnabled = seed > 0 && !skipWorldTraits;
    WorldGen.LoadSettings();
    this.InitializeWorlds(isRunningWorldgenDebug: isRunningWorldgenDebug);
    this.unplacedStoryTraits = new List<WorldTrait>();
    if (!this.clusterLayout.disableStoryTraits)
    {
      this.chosenStoryTraitIds = chosenStoryTraitIds;
      foreach (string chosenStoryTraitId in chosenStoryTraitIds)
      {
        WorldTrait cachedStoryTrait = SettingsCache.GetCachedStoryTrait(chosenStoryTraitId, assertMissingTraits);
        if (cachedStoryTrait != null)
          this.unplacedStoryTraits.Add(cachedStoryTrait);
      }
    }
    else
      this.chosenStoryTraitIds = new List<string>();
    if ((UnityEngine.Object) CustomGameSettings.Instance != (UnityEngine.Object) null)
    {
      foreach (string currentDlcMixingId in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
      {
        DlcMixingSettings dlcMixingSettings = SettingsCache.GetCachedDlcMixingSettings(currentDlcMixingId);
        if (dlcMixingSettings != null && this.clusterLayout.poiPlacements != null)
          this.clusterLayout.poiPlacements.AddRange((IEnumerable<SpaceMapPOIPlacement>) dlcMixingSettings.spacePois);
      }
    }
    if (this.clusterLayout.numRings <= 0)
      return;
    this.numRings = this.clusterLayout.numRings;
  }

  public void InitializeWorlds(bool reuseWorldgen = false, bool isRunningWorldgenDebug = false)
  {
    this.mutatedClusterLayout = WorldgenMixing.DoWorldMixing(SettingsCache.clusterLayouts.clusterCache[this.Id], this.seed, isRunningWorldgenDebug, false);
    for (int index = 0; index < this.clusterLayout.worldPlacements.Count; ++index)
    {
      ProcGen.World worldData = SettingsCache.worlds.GetWorldData(this.clusterLayout.worldPlacements[index], this.seed);
      if (worldData != null)
      {
        this.clusterLayout.worldPlacements[index].SetSize(worldData.worldsize);
        if (index == this.clusterLayout.startWorldIndex)
          this.clusterLayout.worldPlacements[index].startWorld = true;
      }
    }
    this.size = BestFit.BestFitWorlds(this.clusterLayout.worldPlacements);
    int seed = this.seed;
    for (int index = 0; index < this.clusterLayout.worldPlacements.Count; ++index)
    {
      WorldPlacement worldPlacement = this.clusterLayout.worldPlacements[index];
      List<string> chosenWorldTraits = new List<string>();
      ProcGen.World worldData = SettingsCache.worlds.GetWorldData(worldPlacement, seed);
      if (this.worldTraitsEnabled)
      {
        chosenWorldTraits = SettingsCache.GetRandomTraits(seed, worldData);
        ++seed;
      }
      WorldGen worldGen;
      if (reuseWorldgen)
      {
        if (worldData.name == this.worlds[index].Settings.world.name)
        {
          worldGen = this.worlds[index];
        }
        else
        {
          worldGen = new WorldGen(worldPlacement, seed, chosenWorldTraits, (List<string>) null, this.assertMissingTraits);
          this.worlds[index] = worldGen;
        }
      }
      else
      {
        worldGen = new WorldGen(worldPlacement, seed, chosenWorldTraits, (List<string>) null, this.assertMissingTraits);
        this.worlds.Add(worldGen);
      }
      Vector2I worldsize = worldGen.Settings.world.worldsize;
      worldGen.SetWorldSize(worldsize.x, worldsize.y);
      worldGen.SetPosition(new Vector2I(worldPlacement.x, worldPlacement.y));
      worldGen.SetHiddenYOffset(worldGen.Settings.world.hiddenY);
      if (!reuseWorldgen && worldPlacement.worldMixing.mixingWasApplied)
      {
        worldGen.Settings.world.worldTemplateRules.AddRange((IEnumerable<ProcGen.World.TemplateSpawnRules>) worldPlacement.worldMixing.additionalWorldTemplateRules);
        worldGen.Settings.world.subworldFiles.AddRange((IEnumerable<WeightedSubworldName>) worldPlacement.worldMixing.additionalSubworldFiles);
        worldGen.Settings.world.AddUnknownCellsAllowedSubworlds(worldPlacement.worldMixing.additionalUnknownCellFilters);
        worldGen.Settings.world.AddSeasons(worldPlacement.worldMixing.additionalSeasons);
      }
      if (worldPlacement.startWorld)
      {
        this.currentWorld = worldGen;
        worldGen.isStartingWorld = true;
      }
    }
    if (this.currentWorld != null)
      return;
    DebugUtil.DevLogErrorFormat("Start world not set. Defaulting to first world {0}", (object) this.worlds[0].Settings.world.name);
    this.currentWorld = this.worlds[0];
  }

  public void Reset() => this.worlds.Clear();

  private void LogBeginGeneration()
  {
    string str1 = (UnityEngine.Object) CustomGameSettings.Instance != (UnityEngine.Object) null ? CustomGameSettings.Instance.GetSettingsCoordinate() : this.seed.ToString();
    if (this.chosenStoryTraitIds.Count > 0)
    {
      string str2 = "storytraits:";
      foreach (string chosenStoryTraitId in this.chosenStoryTraitIds)
        str2 = $"{str2}\n  - {chosenStoryTraitId}";
      DebugUtil.LogArgs((object) str2);
    }
    Console.WriteLine("\n\n");
    DebugUtil.LogArgs((object) $"WORLDGEN START seed={str1}, cluster={this.clusterLayout.filePath}");
    this.worldgenDebugTimer.Restart();
    this.worldgenDebugTimer.Start();
  }

  public void Generate(
    WorldGen.OfflineCallbackFunction callbackFn,
    Action<OfflineWorldGen.ErrorInfo> error_cb,
    int worldSeed = -1,
    int layoutSeed = -1,
    int terrainSeed = -1,
    int noiseSeed = -1,
    bool doSimSettle = true,
    bool debug = false,
    bool skipPlacingTemplates = false)
  {
    this.doSimSettle = doSimSettle;
    for (int index = 0; index != this.worlds.Count; ++index)
    {
      if (this.ShouldSkipWorldCallback == null || !this.ShouldSkipWorldCallback(index, this.worlds[index]))
        this.worlds[index].Initialise(callbackFn, error_cb, worldSeed + index, layoutSeed + index, terrainSeed + index, noiseSeed + index, debug, skipPlacingTemplates);
    }
    this.ApplicationIsPlaying = Application.isPlaying;
    this.thread = new Thread(new ThreadStart(this.ThreadMain));
    Util.ApplyInvariantCultureToThread(this.thread);
    this.thread.Start();
  }

  private bool IsRunningDebugGen() => !this.ApplicationIsPlaying;

  private void BeginGeneration()
  {
    this.Status = Cluster.WorldGenStatus.Generating;
    this.LogBeginGeneration();
    try
    {
      WorldgenMixing.DoSubworldMixing(this, this.seed, this.ShouldSkipWorldCallback, this.IsRunningDebugGen());
    }
    catch (WorldgenException ex)
    {
      if (!this.IsRunningDebugGen())
        this.currentWorld.ReportWorldGenError((Exception) ex, ex.userMessage);
      this.Status = Cluster.WorldGenStatus.Failed;
      return;
    }
    WorldgenSimData simData = new WorldgenSimData();
    int baseId = 0;
    AxialI startLoc = this.worlds[0].GetClusterLocation();
    foreach (WorldGen world in this.worlds)
    {
      if (world.isStartingWorld)
        startLoc = world.GetClusterLocation();
    }
    List<WorldGen> worldGenList = new List<WorldGen>((IEnumerable<WorldGen>) this.worlds);
    worldGenList.Sort((Comparison<WorldGen>) ((a, b) =>
    {
      int distance1 = AxialUtil.GetDistance(startLoc, a.GetClusterLocation());
      int distance2 = AxialUtil.GetDistance(startLoc, b.GetClusterLocation());
      if (distance1 == distance2)
        return 0;
      return distance1 >= distance2 ? 1 : -1;
    }));
    MemoryStream output = new MemoryStream();
    BinaryWriter writer = new BinaryWriter((Stream) output);
    for (int index = 0; index < worldGenList.Count; ++index)
    {
      WorldGen worldGen = worldGenList[index];
      if (this.ShouldSkipWorldCallback == null || !this.ShouldSkipWorldCallback(index, worldGen))
      {
        DebugUtil.Separator();
        DebugUtil.LogArgs((object) ("Generating world: " + worldGen.Settings.world.filePath));
        if (worldGen.Settings.GetWorldTraitIDs().Length != 0)
          DebugUtil.LogArgs((object) (" - worldtraits: " + string.Join(", ", ((IEnumerable<string>) worldGen.Settings.GetWorldTraitIDs()).ToArray<string>())));
        if (this.PerWorldGenBeginCallback != null)
          this.PerWorldGenBeginCallback(index, worldGen);
        List<WorldTrait> storyTraits = new List<WorldTrait>();
        storyTraits.AddRange((IEnumerable<WorldTrait>) this.unplacedStoryTraits);
        worldGen.Settings.SetStoryTraitCandidates(storyTraits);
        GridSettings.Reset(worldGen.GetSize().x, worldGen.GetSize().y);
        BackwallManager.Clear();
        if (!worldGen.GenerateOffline())
        {
          this.Status = Cluster.WorldGenStatus.Failed;
          return;
        }
        worldGen.FinalizeStartLocation();
        List<WorldTrait> placedStoryTraits = new List<WorldTrait>();
        uint seed = (uint) this.seed;
        if (!worldGen.RenderOffline(this.doSimSettle, seed, writer, ref simData, baseId, ref placedStoryTraits, worldGen.isStartingWorld))
        {
          this.Status = Cluster.WorldGenStatus.Failed;
          return;
        }
        if (this.PerWorldGenCompleteCallback != null)
          this.PerWorldGenCompleteCallback(index, worldGen, simData);
        foreach (WorldTrait worldTrait in placedStoryTraits)
          this.unplacedStoryTraits.Remove(worldTrait);
        ++baseId;
      }
    }
    if (this.unplacedStoryTraits.Count > 0)
    {
      List<string> stringList = new List<string>();
      foreach (WorldTrait unplacedStoryTrait in this.unplacedStoryTraits)
        stringList.Add(unplacedStoryTrait.filePath);
      string message = "Story trait failure, unable to place on any world: " + string.Join(", ", stringList.ToArray());
      if (!this.worlds[0].isRunningDebugGen)
        this.worlds[0].ReportWorldGenError(new Exception(message), (string) UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE_STORY);
      this.Status = Cluster.WorldGenStatus.Failed;
    }
    else
    {
      DebugUtil.Separator();
      if (!this.AssignClusterLocations())
      {
        this.Status = Cluster.WorldGenStatus.Failed;
      }
      else
      {
        DebugUtil.Separator();
        this.worldgenDebugTimer.Stop();
        DebugUtil.LogArgs((object) $"WORLDGEN COMPLETE (took {this.worldgenDebugTimer.Elapsed.TotalSeconds:F2}s)\n\n\n");
        BinaryWriter fileWriter = new BinaryWriter((Stream) File.Open(WorldGen.WORLDGEN_SAVE_FILENAME, FileMode.Create));
        this.Save(fileWriter);
        fileWriter.Write(output.ToArray());
        fileWriter.Flush();
        fileWriter.Close();
        this.Status = Cluster.WorldGenStatus.Complete;
      }
    }
  }

  public void Join()
  {
    if (this.thread == null)
      return;
    this.thread.Join();
    this.thread = (Thread) null;
  }

  private bool IsValidHex(AxialI location)
  {
    return location.IsWithinRadius(AxialI.ZERO, this.numRings - 1);
  }

  public bool AssignClusterLocations()
  {
    this.myRandom = new SeededRandom(this.seed);
    List<WorldPlacement> worldPlacementList = new List<WorldPlacement>((IEnumerable<WorldPlacement>) SettingsCache.clusterLayouts.clusterCache[this.Id].worldPlacements);
    List<SpaceMapPOIPlacement> spaceMapPoiPlacementList = this.clusterLayout.poiPlacements == null ? new List<SpaceMapPOIPlacement>() : new List<SpaceMapPOIPlacement>((IEnumerable<SpaceMapPOIPlacement>) this.clusterLayout.poiPlacements);
    this.currentWorld.SetClusterLocation(AxialI.ZERO);
    HashSet<AxialI> assignedLocations = new HashSet<AxialI>();
    HashSet<AxialI> worldForbiddenLocations = new HashSet<AxialI>();
    HashSet<AxialI> axialISet = new HashSet<AxialI>();
    HashSet<AxialI> poiWorldAvoidance = new HashSet<AxialI>();
    int maxRadius1 = 2;
    for (int index = 0; index < this.worlds.Count; ++index)
    {
      WorldGen world = this.worlds[index];
      WorldPlacement worldPlacement = worldPlacementList[index];
      DebugUtil.Assert(worldPlacement != null, "Somehow we're trying to generate a cluster with a world that isn't the cluster .yaml's world list!", world.Settings.world.filePath);
      HashSet<AxialI> antiBuffer = new HashSet<AxialI>();
      foreach (AxialI center in assignedLocations)
        antiBuffer.UnionWith((IEnumerable<AxialI>) AxialUtil.GetRings(center, 1, worldPlacement.buffer));
      List<AxialI> list1 = AxialUtil.GetRings(AxialI.ZERO, worldPlacement.allowedRings.min, Mathf.Min(worldPlacement.allowedRings.max, this.numRings - 1)).Where<AxialI>((Func<AxialI, bool>) (location => !assignedLocations.Contains(location) && !worldForbiddenLocations.Contains(location) && !antiBuffer.Contains(location))).ToList<AxialI>();
      if (list1.Count > 0)
      {
        AxialI axialI = list1[this.myRandom.RandomRange(0, list1.Count)];
        world.SetClusterLocation(axialI);
        assignedLocations.Add(axialI);
        worldForbiddenLocations.UnionWith((IEnumerable<AxialI>) AxialUtil.GetRings(axialI, 1, worldPlacement.buffer));
        poiWorldAvoidance.UnionWith((IEnumerable<AxialI>) AxialUtil.GetRings(axialI, 1, maxRadius1));
      }
      else
      {
        DebugUtil.DevLogError($"Could not find a spot in the cluster for {world.Settings.world.filePath}. Check the placement settings in {this.Id}.yaml to ensure there are no conflicts.");
        HashSet<AxialI> minBuffers = new HashSet<AxialI>();
        foreach (AxialI center in assignedLocations)
          minBuffers.UnionWith((IEnumerable<AxialI>) AxialUtil.GetRings(center, 1, 2));
        List<AxialI> list2 = AxialUtil.GetRings(AxialI.ZERO, worldPlacement.allowedRings.min, Mathf.Min(worldPlacement.allowedRings.max, this.numRings - 1)).Where<AxialI>((Func<AxialI, bool>) (location => !assignedLocations.Contains(location) && !minBuffers.Contains(location))).ToList<AxialI>();
        if (list2.Count > 0)
        {
          AxialI axialI = list2[this.myRandom.RandomRange(0, list2.Count)];
          world.SetClusterLocation(axialI);
          assignedLocations.Add(axialI);
          worldForbiddenLocations.UnionWith((IEnumerable<AxialI>) AxialUtil.GetRings(axialI, 1, worldPlacement.buffer));
          poiWorldAvoidance.UnionWith((IEnumerable<AxialI>) AxialUtil.GetRings(axialI, 1, maxRadius1));
        }
        else
        {
          string message = $"Could not find a spot in the cluster for {world.Settings.world.filePath} EVEN AFTER REDUCING BUFFERS. Check the placement settings in {this.Id}.yaml to ensure there are no conflicts.";
          DebugUtil.LogErrorArgs((object) message);
          if (!world.isRunningDebugGen)
            this.currentWorld.ReportWorldGenError(new Exception(message));
          return false;
        }
      }
    }
    if (DlcManager.FeatureClusterSpaceEnabled() && spaceMapPoiPlacementList != null)
    {
      HashSet<AxialI> poiClumpLocations = new HashSet<AxialI>();
      HashSet<AxialI> poiForbiddenLocations = new HashSet<AxialI>();
      float num1 = 0.5f;
      int num2 = 3;
      int num3 = 0;
      foreach (SpaceMapPOIPlacement spaceMapPoiPlacement in spaceMapPoiPlacementList)
      {
        List<string> stringList = new List<string>((IEnumerable<string>) spaceMapPoiPlacement.pois);
        for (int index = 0; index < spaceMapPoiPlacement.numToSpawn; ++index)
        {
          int num4 = (double) this.myRandom.RandomRange(0.0f, 1f) <= (double) num1 ? 1 : 0;
          List<AxialI> axialIList = (List<AxialI>) null;
          MinMaxI allowedRings;
          if (num4 != 0 && num3 < num2 && !spaceMapPoiPlacement.avoidClumping)
          {
            ++num3;
            AxialI zero = AxialI.ZERO;
            allowedRings = spaceMapPoiPlacement.allowedRings;
            int min = allowedRings.min;
            allowedRings = spaceMapPoiPlacement.allowedRings;
            int maxRadius2 = Mathf.Min(allowedRings.max, this.numRings - 1);
            axialIList = AxialUtil.GetRings(zero, min, maxRadius2).Where<AxialI>((Func<AxialI, bool>) (location => !assignedLocations.Contains(location) && poiClumpLocations.Contains(location) && !poiWorldAvoidance.Contains(location))).ToList<AxialI>();
          }
          if (axialIList == null || axialIList.Count <= 0)
          {
            num3 = 0;
            poiClumpLocations.Clear();
            AxialI zero = AxialI.ZERO;
            allowedRings = spaceMapPoiPlacement.allowedRings;
            int min = allowedRings.min;
            allowedRings = spaceMapPoiPlacement.allowedRings;
            int maxRadius3 = Mathf.Min(allowedRings.max, this.numRings - 1);
            axialIList = AxialUtil.GetRings(zero, min, maxRadius3).Where<AxialI>((Func<AxialI, bool>) (location => !assignedLocations.Contains(location) && !poiWorldAvoidance.Contains(location) && !poiForbiddenLocations.Contains(location))).ToList<AxialI>();
          }
          if (spaceMapPoiPlacement.guarantee && (axialIList == null || axialIList.Count <= 0))
          {
            num3 = 0;
            poiClumpLocations.Clear();
            AxialI zero = AxialI.ZERO;
            allowedRings = spaceMapPoiPlacement.allowedRings;
            int min = allowedRings.min;
            allowedRings = spaceMapPoiPlacement.allowedRings;
            int maxRadius4 = Mathf.Min(allowedRings.max, this.numRings - 1);
            axialIList = AxialUtil.GetRings(zero, min, maxRadius4).Where<AxialI>((Func<AxialI, bool>) (location => !assignedLocations.Contains(location) && !poiWorldAvoidance.Contains(location))).ToList<AxialI>();
          }
          if (axialIList != null && axialIList.Count > 0)
          {
            AxialI axialI = axialIList[this.myRandom.RandomRange(0, axialIList.Count)];
            string str = stringList[this.myRandom.RandomRange(0, stringList.Count)];
            if (!spaceMapPoiPlacement.canSpawnDuplicates)
              stringList.Remove(str);
            this.poiPlacements[axialI] = str;
            poiForbiddenLocations.UnionWith((IEnumerable<AxialI>) AxialUtil.GetRings(axialI, 1, 3));
            poiClumpLocations.UnionWith((IEnumerable<AxialI>) AxialUtil.GetRings(axialI, 1, 1));
            assignedLocations.Add(axialI);
          }
          else
          {
            allowedRings = spaceMapPoiPlacement.allowedRings;
            // ISSUE: variable of a boxed type
            __Boxed<int> min = (ValueType) allowedRings.min;
            allowedRings = spaceMapPoiPlacement.allowedRings;
            // ISSUE: variable of a boxed type
            __Boxed<int> max = (ValueType) allowedRings.max;
            string str = string.Join("\n - ", spaceMapPoiPlacement.pois.ToArray());
            Debug.LogWarning((object) $"There is no room for a Space POI in ring range [{min}, {max}] with pois: {str}");
          }
        }
      }
    }
    return true;
  }

  public void AbortGeneration()
  {
    if (this.thread == null || !this.thread.IsAlive)
      return;
    this.thread.Abort();
    this.thread.Join();
    this.thread = (Thread) null;
  }

  private void ThreadMain() => this.BeginGeneration();

  private void Save(BinaryWriter fileWriter)
  {
    try
    {
      using (MemoryStream output = new MemoryStream())
      {
        using (BinaryWriter writer = new BinaryWriter((Stream) output))
        {
          try
          {
            KSerialization.Manager.Clear();
            ClusterLayoutSave clusterLayoutSave = new ClusterLayoutSave();
            clusterLayoutSave.version = new Vector2I(1, 1);
            clusterLayoutSave.size = this.size;
            clusterLayoutSave.ID = this.Id;
            clusterLayoutSave.numRings = this.numRings;
            clusterLayoutSave.poiLocations = this.poiLocations;
            clusterLayoutSave.poiPlacements = this.poiPlacements;
            for (int index = 0; index != this.worlds.Count; ++index)
            {
              WorldGen world = this.worlds[index];
              if (this.ShouldSkipWorldCallback == null || !this.ShouldSkipWorldCallback(index, world))
              {
                HashSet<string> source = new HashSet<string>();
                foreach (TerrainCell terrainCell in world.TerrainCells)
                  source.Add(terrainCell.node.GetSubworld());
                clusterLayoutSave.worlds.Add(new ClusterLayoutSave.World()
                {
                  data = world.data,
                  name = world.Settings.world.filePath,
                  isDiscovered = world.isStartingWorld,
                  traits = ((IEnumerable<string>) world.Settings.GetWorldTraitIDs()).ToList<string>(),
                  storyTraits = ((IEnumerable<string>) world.Settings.GetStoryTraitIDs()).ToList<string>(),
                  seasons = world.Settings.world.seasons,
                  generatedSubworlds = source.ToList<string>()
                });
                if (world == this.currentWorld)
                  clusterLayoutSave.currentWorldIdx = index;
              }
            }
            Serializer.Serialize((object) clusterLayoutSave, writer);
          }
          catch (Exception ex)
          {
            DebugUtil.LogErrorArgs((object) "Couldn't serialize", (object) ex.Message, (object) ex.StackTrace);
          }
        }
        KSerialization.Manager.SerializeDirectory(fileWriter);
        fileWriter.Write(output.ToArray());
      }
    }
    catch (Exception ex)
    {
      DebugUtil.LogErrorArgs((object) "Couldn't write", (object) ex.Message, (object) ex.StackTrace);
    }
  }

  public static Cluster Load(FastReader reader)
  {
    Cluster cluster = new Cluster();
    try
    {
      KSerialization.Manager.DeserializeDirectory((IReader) reader);
      int position = reader.Position;
      ClusterLayoutSave clusterLayoutSave = new ClusterLayoutSave();
      if (!Deserializer.Deserialize((object) clusterLayoutSave, (IReader) reader))
      {
        reader.Position = position;
        WorldGen worldGen = WorldGen.Load((IReader) reader, true);
        cluster.worlds.Add(worldGen);
        cluster.size = worldGen.GetSize();
        cluster.currentWorld = cluster.worlds[0] ?? (WorldGen) null;
      }
      else
      {
        for (int index = 0; index != clusterLayoutSave.worlds.Count; ++index)
        {
          ClusterLayoutSave.World world = clusterLayoutSave.worlds[index];
          WorldGen worldGen = new WorldGen(world.name, world.data, world.traits, world.storyTraits, false);
          worldGen.Settings.world.ReplaceSeasons(world.seasons);
          worldGen.Settings.world.generatedSubworlds = world.generatedSubworlds;
          cluster.worlds.Add(worldGen);
          if (index == clusterLayoutSave.currentWorldIdx)
          {
            cluster.currentWorld = worldGen;
            cluster.worlds[index].isStartingWorld = true;
          }
        }
        cluster.size = clusterLayoutSave.size;
        cluster.Id = clusterLayoutSave.ID;
        cluster.numRings = clusterLayoutSave.numRings;
        cluster.poiLocations = clusterLayoutSave.poiLocations;
        cluster.poiPlacements = clusterLayoutSave.poiPlacements;
      }
      DebugUtil.Assert(cluster.currentWorld != null);
      if (cluster.currentWorld == null)
      {
        DebugUtil.Assert(0 < cluster.worlds.Count);
        cluster.currentWorld = cluster.worlds[0];
      }
    }
    catch (Exception ex)
    {
      DebugUtil.LogErrorArgs((object) "SolarSystem.Load Error!\n", (object) ex.Message, (object) ex.StackTrace);
      cluster = (Cluster) null;
    }
    return cluster;
  }

  public void LoadClusterSim(List<SimSaveFileStructure> loadedWorlds, FastReader reader)
  {
    try
    {
      for (int index = 0; index != this.worlds.Count; ++index)
      {
        SimSaveFileStructure saveFileStructure = new SimSaveFileStructure();
        KSerialization.Manager.DeserializeDirectory((IReader) reader);
        Deserializer.Deserialize((object) saveFileStructure, (IReader) reader);
        if (saveFileStructure.worldDetail == null)
        {
          if (!GenericGameSettings.instance.devAutoWorldGenActive)
            Debug.LogError((object) ("Detail is null for world " + index.ToString()));
        }
        else
          loadedWorlds.Add(saveFileStructure);
      }
    }
    catch (Exception ex)
    {
      if (GenericGameSettings.instance.devAutoWorldGenActive)
        return;
      DebugUtil.LogErrorArgs((object) "LoadSim Error!\n", (object) ex.Message, (object) ex.StackTrace);
    }
  }

  public void SetIsRunningDebug(bool isDebug)
  {
    foreach (WorldGen world in this.worlds)
      world.isRunningDebugGen = isDebug;
  }

  public void DEBUG_UpdateSeed(int seed)
  {
    this.seed = seed;
    this.InitializeWorlds(true, true);
  }

  public int MaxSupportedSubworldMixings()
  {
    int num = 0;
    foreach (WorldGen world in this.worlds)
      num += world.Settings.world.subworldMixingRules.Count;
    return num;
  }

  public int MaxSupportedWorldMixings()
  {
    int num = 0;
    foreach (WorldPlacement worldPlacement in this.clusterLayout.worldPlacements)
    {
      if (worldPlacement.worldMixing != null && (worldPlacement.worldMixing.requiredTags.Count != 0 || worldPlacement.worldMixing.forbiddenTags.Count != 0))
        ++num;
    }
    return num;
  }

  public enum WorldGenStatus
  {
    NotStarted,
    Generating,
    Failed,
    Complete,
  }
}
