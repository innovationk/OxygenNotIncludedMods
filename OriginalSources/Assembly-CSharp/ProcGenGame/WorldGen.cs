// Decompiled with JetBrains decompiler
// Type: ProcGenGame.WorldGen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Delaunay.Geo;
using Klei;
using Klei.CustomSettings;
using KSerialization;
using LibNoiseDotNet.Graphics.Tools.Noise;
using LibNoiseDotNet.Graphics.Tools.Noise.Builder;
using ProcGen;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using VoronoiTree;

#nullable disable
namespace ProcGenGame;

[Serializable]
public class WorldGen
{
  private const string _WORLDGEN_SAVE_FILENAME = "WorldGenDataSave.worldgen";
  private const int heatScale = 2;
  private const int UNPASSABLE_EDGE_COUNT = 4;
  private const string heat_noise_name = "noise/Heat";
  private const string default_base_noise_name = "noise/Default";
  private const string default_cave_noise_name = "noise/DefaultCave";
  private const string default_density_noise_name = "noise/DefaultDensity";
  public const int WORLDGEN_SAVE_MAJOR_VERSION = 1;
  public const int WORLDGEN_SAVE_MINOR_VERSION = 1;
  private const float EXTREME_TEMPERATURE_BORDER_RANGE = 150f;
  private const float EXTREME_TEMPERATURE_BORDER_MIN_WIDTH = 2f;
  public static Element voidElement;
  public static Element vacuumElement;
  public static Element katairiteElement;
  public static Element unobtaniumElement;
  private static Diseases m_diseasesDb;
  public bool isRunningDebugGen;
  public bool skipPlacingTemplates;
  private HashSet<int> claimedCells = new HashSet<int>();
  public Dictionary<int, int> claimedPOICells = new Dictionary<int, int>();
  private HashSet<int> highPriorityClaims = new HashSet<int>();
  public List<RectInt> POIBounds = new List<RectInt>();
  public List<TemplateSpawning.TemplateSpawner> POISpawners;
  private WorldGen.OfflineCallbackFunction successCallbackFn;
  private bool running = true;
  private Action<OfflineWorldGen.ErrorInfo> errorCallback;
  private SeededRandom myRandom;
  private NoiseMapBuilderPlane heatSource;
  private bool wasLoaded;
  public int polyIndex = -1;
  public bool isStartingWorld;
  public bool isModuleInterior;
  private static Task loadSettingsTask;

  public static string WORLDGEN_SAVE_FILENAME
  {
    get => System.IO.Path.Combine(Util.RootFolder(), "WorldGenDataSave.worldgen");
  }

  public static Diseases diseaseStats
  {
    get
    {
      if (WorldGen.m_diseasesDb == null)
        WorldGen.m_diseasesDb = new Diseases((ResourceSet) null, true);
      return WorldGen.m_diseasesDb;
    }
  }

  public int BaseLeft => this.Settings.GetBaseLocation().left;

  public int BaseRight => this.Settings.GetBaseLocation().right;

  public int BaseTop => this.Settings.GetBaseLocation().top;

  public int BaseBot => this.Settings.GetBaseLocation().bottom;

  public Klei.Data data { get; private set; }

  public bool HasData => this.data != null;

  public bool HasNoiseData => this.HasData && this.data.world != null;

  public float[] DensityMap => this.data.world.density;

  public float[] HeatMap => this.data.world.heatOffset;

  public float[] OverrideMap => this.data.world.overrides;

  public float[] BaseNoiseMap => this.data.world.data;

  public float[] DefaultTendMap => this.data.world.defaultTemp;

  public float[] BackwallMap => this.data.world.backwallNoise;

  public Chunk World => this.data.world;

  public Vector2I WorldSize => this.data.world.size;

  public Vector2I WorldOffset => this.data.world.offset;

  public int HiddenYOffset => this.data.world.hiddenY;

  public WorldLayout WorldLayout => this.data.worldLayout;

  public List<TerrainCell> OverworldCells => this.data.overworldCells;

  public List<TerrainCell> TerrainCells => this.data.terrainCells;

  public List<River> Rivers => this.data.rivers;

  public GameSpawnData SpawnData => this.data.gameSpawnData;

  public int ChunkEdgeSize => this.data.chunkEdgeSize;

  public HashSet<int> ClaimedCells => this.claimedCells;

  public HashSet<int> HighPriorityClaimedCells => this.highPriorityClaims;

  public void ClearClaimedCells()
  {
    this.claimedCells.Clear();
    this.highPriorityClaims.Clear();
  }

  public void AddHighPriorityCells(HashSet<int> cells)
  {
    this.highPriorityClaims.UnionWith((IEnumerable<int>) cells);
  }

  public WorldGenSettings Settings { get; private set; }

  public WorldGen(
    string worldName,
    List<string> chosenWorldTraits,
    List<string> chosenStoryTraits,
    bool assertMissingTraits)
  {
    WorldGen.LoadSettings();
    this.Settings = new WorldGenSettings(worldName, chosenWorldTraits, chosenStoryTraits, assertMissingTraits);
    this.data = new Klei.Data();
    this.data.chunkEdgeSize = this.Settings.GetIntSetting(nameof (ChunkEdgeSize));
  }

  public WorldGen(
    string worldName,
    Klei.Data data,
    List<string> chosenTraits,
    List<string> chosenStoryTraits,
    bool assertMissingTraits)
  {
    WorldGen.LoadSettings();
    this.Settings = new WorldGenSettings(worldName, chosenTraits, chosenStoryTraits, assertMissingTraits);
    this.data = data;
  }

  public WorldGen(
    WorldPlacement world,
    int seed,
    List<string> chosenWorldTraits,
    List<string> chosenStoryTraits,
    bool assertMissingTraits)
  {
    WorldGen.LoadSettings();
    this.Settings = new WorldGenSettings(world, seed, chosenWorldTraits, chosenStoryTraits, assertMissingTraits);
    this.data = new Klei.Data();
    this.data.chunkEdgeSize = this.Settings.GetIntSetting(nameof (ChunkEdgeSize));
  }

  public static void SetupDefaultElements()
  {
    WorldGen.voidElement = ElementLoader.FindElementByHash(SimHashes.Void);
    WorldGen.vacuumElement = ElementLoader.FindElementByHash(SimHashes.Vacuum);
    WorldGen.katairiteElement = ElementLoader.FindElementByHash(SimHashes.Katairite);
    WorldGen.unobtaniumElement = ElementLoader.FindElementByHash(SimHashes.Unobtanium);
  }

  public void Reset() => this.wasLoaded = false;

  public static void LoadSettings(bool in_async_thread = false)
  {
    bool is_playing = Application.isPlaying;
    if (in_async_thread)
    {
      WorldGen.loadSettingsTask = Task.Run((System.Action) (() => WorldGen.LoadSettings_Internal(is_playing, true)));
    }
    else
    {
      if (WorldGen.loadSettingsTask != null)
      {
        WorldGen.loadSettingsTask.Wait();
        WorldGen.loadSettingsTask = (Task) null;
      }
      WorldGen.LoadSettings_Internal(is_playing);
    }
  }

  public static void WaitForPendingLoadSettings()
  {
    if (WorldGen.loadSettingsTask == null)
      return;
    WorldGen.loadSettingsTask.Wait();
    WorldGen.loadSettingsTask = (Task) null;
  }

  public static IEnumerator ListenForLoadSettingsErrorRoutine()
  {
    while (WorldGen.loadSettingsTask != null)
    {
      if (WorldGen.loadSettingsTask.Exception != null)
        throw WorldGen.loadSettingsTask.Exception;
      yield return (object) null;
    }
  }

  private static void LoadSettings_Internal(bool is_playing, bool preloadTemplates = false)
  {
    ListPool<YamlIO.Error, WorldGen>.PooledList pooledList = ListPool<YamlIO.Error, WorldGen>.Allocate();
    if (SettingsCache.LoadFiles((List<YamlIO.Error>) pooledList))
    {
      TemplateCache.Init();
      if (preloadTemplates)
      {
        foreach (ProcGen.World world in SettingsCache.worlds.worldCache.Values)
        {
          if (world.worldTemplateRules != null)
          {
            foreach (ProcGen.World.TemplateSpawnRules worldTemplateRule in world.worldTemplateRules)
            {
              foreach (string name in worldTemplateRule.names)
                TemplateCache.GetTemplate(name);
            }
          }
        }
        foreach (SubWorld subWorld in SettingsCache.subworlds.Values)
        {
          if (subWorld.subworldTemplateRules != null)
          {
            foreach (ProcGen.World.TemplateSpawnRules subworldTemplateRule in subWorld.subworldTemplateRules)
            {
              foreach (string name in subworldTemplateRule.names)
                TemplateCache.GetTemplate(name);
            }
          }
        }
        foreach (KeyValuePair<string, DlcManager.DlcInfo> keyValuePair in DlcManager.DLC_PACKS)
        {
          if (DlcManager.IsContentSubscribed(keyValuePair.Value.id))
          {
            string scopePath = keyValuePair.Value.directory + "::poi/asteroid_impacts";
            string path = TemplateCache.RewriteTemplatePath(scopePath);
            if (Directory.Exists(path))
            {
              foreach (string file in Directory.GetFiles(path, "*.yaml"))
                TemplateCache.GetTemplate(System.IO.Path.Combine(scopePath ?? "", System.IO.Path.GetFileNameWithoutExtension(file)));
            }
          }
        }
      }
      if ((UnityEngine.Object) CustomGameSettings.Instance != (UnityEngine.Object) null)
      {
        foreach (KeyValuePair<string, WorldMixingSettings> worldMixingSetting in SettingsCache.worldMixingSettings)
        {
          string key = worldMixingSetting.Key;
          if (worldMixingSetting.Value.isModded && CustomGameSettings.Instance.GetWorldMixingSettingForWorldgenFile(key) == null)
            CustomGameSettings.Instance.AddMixingSettingsConfig((SettingConfig) new WorldMixingSettingConfig(key, key, coordinate_range: -1L));
        }
        foreach (KeyValuePair<string, SubworldMixingSettings> subworldMixingSetting in SettingsCache.subworldMixingSettings)
        {
          string key = subworldMixingSetting.Key;
          if (subworldMixingSetting.Value.isModded && CustomGameSettings.Instance.GetSubworldMixingSettingForWorldgenFile(key) == null)
            CustomGameSettings.Instance.AddMixingSettingsConfig((SettingConfig) new SubworldMixingSettingConfig(key, key, coordinate_range: -1L));
        }
      }
    }
    int num = (UnityEngine.Object) CustomGameSettings.Instance != (UnityEngine.Object) null ? 1 : 0;
    if (is_playing)
    {
      Global.Instance.modManager.HandleErrors((List<YamlIO.Error>) pooledList);
    }
    else
    {
      foreach (YamlIO.Error error in (List<YamlIO.Error>) pooledList)
        YamlIO.LogError(error, false);
    }
    pooledList.Recycle();
  }

  public void InitRandom(int worldSeed, int layoutSeed, int terrainSeed, int noiseSeed)
  {
    this.data.globalWorldSeed = worldSeed;
    this.data.globalWorldLayoutSeed = layoutSeed;
    this.data.globalTerrainSeed = terrainSeed;
    this.data.globalNoiseSeed = noiseSeed;
    this.myRandom = new SeededRandom(worldSeed);
  }

  public void Initialise(
    WorldGen.OfflineCallbackFunction callbackFn,
    Action<OfflineWorldGen.ErrorInfo> error_cb,
    int worldSeed = -1,
    int layoutSeed = -1,
    int terrainSeed = -1,
    int noiseSeed = -1,
    bool debug = false,
    bool skipPlacingTemplates = false)
  {
    if (this.wasLoaded)
    {
      Debug.LogError((object) "Initialise called after load");
    }
    else
    {
      this.successCallbackFn = callbackFn;
      this.errorCallback = error_cb;
      Debug.Assert(this.successCallbackFn != null);
      this.isRunningDebugGen = debug;
      this.skipPlacingTemplates = skipPlacingTemplates;
      this.running = false;
      int num1 = UnityEngine.Random.Range(0, int.MaxValue);
      if (worldSeed == -1)
        worldSeed = num1;
      if (layoutSeed == -1)
        layoutSeed = num1;
      if (terrainSeed == -1)
        terrainSeed = num1;
      if (noiseSeed == -1)
        noiseSeed = num1;
      this.data.gameSpawnData = new GameSpawnData();
      this.InitRandom(worldSeed, layoutSeed, terrainSeed, noiseSeed);
      int num2 = this.successCallbackFn(UI.WORLDGEN.COMPLETE.key, 0.0f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
      WorldLayout.SetLayerGradient(SettingsCache.layers.LevelLayers);
    }
  }

  public bool GenerateOffline()
  {
    if (this.GenerateWorldData())
      return true;
    int num = this.successCallbackFn(UI.WORLDGEN.FAILED.key, 1f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
    return false;
  }

  private void PlaceTemplateSpawners(
    Vector2I position,
    TemplateContainer template,
    ref Dictionary<int, int> claimedCells)
  {
    this.data.gameSpawnData.AddTemplate(template, position, ref claimedCells);
  }

  public bool RenderOffline(
    bool doSettle,
    uint simSeed,
    BinaryWriter writer,
    ref WorldgenSimData simData,
    int baseId,
    ref List<WorldTrait> placedStoryTraits,
    bool isStartingWorld = false)
  {
    HashSet<int> borderCells = new HashSet<int>();
    this.POIBounds = new List<RectInt>();
    this.WriteOverWorldNoise(this.successCallbackFn);
    if (!this.RenderToMap(this.successCallbackFn, ref simData, ref borderCells, ref this.POIBounds))
    {
      int num = this.successCallbackFn(UI.WORLDGEN.FAILED.key, -100f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
      if (!this.isRunningDebugGen)
        return false;
    }
    foreach (int key in borderCells)
    {
      simData.cells[key].SetValues(WorldGen.unobtaniumElement, ElementLoader.elements);
      simData.backwallCells[key].SetValues(WorldGen.vacuumElement.idx, 0.0f, 0.0f);
      this.claimedPOICells[key] = 1;
    }
    try
    {
      if (!this.skipPlacingTemplates)
        this.POISpawners = TemplateSpawning.DetermineTemplatesForWorld(this.Settings, this.data.terrainCells, this.myRandom, ref this.POIBounds, this.isRunningDebugGen, ref placedStoryTraits, this.successCallbackFn);
    }
    catch (WorldgenException ex)
    {
      if (!this.isRunningDebugGen)
      {
        this.ReportWorldGenError((Exception) ex, ex.userMessage);
        return false;
      }
    }
    catch (Exception ex)
    {
      if (!this.isRunningDebugGen)
      {
        this.ReportWorldGenError(ex);
        return false;
      }
    }
    if (isStartingWorld)
      this.EnsureEnoughElementsInStartingBiome(ref simData);
    List<TerrainCell> terrainCellsForTag = this.GetTerrainCellsForTag(WorldGenTags.StartWorld);
    foreach (TerrainCell overworldCell in this.OverworldCells)
    {
      foreach (TerrainCell terrainCell in terrainCellsForTag)
      {
        if (overworldCell.poly.PointInPolygon(terrainCell.poly.Centroid()))
        {
          overworldCell.node.tags.Add(WorldGenTags.StartWorld);
          break;
        }
      }
    }
    if (doSettle)
      this.running = WorldGenSimUtil.DoSettleSim(this.Settings, writer, simSeed, ref simData, this.successCallbackFn, this.data, this.POISpawners, this.errorCallback, baseId);
    if (!this.skipPlacingTemplates)
    {
      foreach (TemplateSpawning.TemplateSpawner poiSpawner in this.POISpawners)
        this.PlaceTemplateSpawners(poiSpawner.position, poiSpawner.container, ref this.claimedPOICells);
    }
    if (doSettle)
      this.SpawnMobsAndTemplates(baseId, ref simData, new HashSet<int>((IEnumerable<int>) this.claimedPOICells.Keys));
    int num1 = this.successCallbackFn(UI.WORLDGEN.COMPLETE.key, 1f, WorldGenProgressStages.Stages.Complete) ? 1 : 0;
    this.running = false;
    return true;
  }

  private void SpawnMobsAndTemplates(
    int world,
    ref WorldgenSimData simData,
    HashSet<int> claimedCells)
  {
    HashSetPool<int, WorldGen>.PooledHashSet cavityCells = HashSetPool<int, WorldGen>.Allocate();
    MobSpawning.DetectNaturalCavities(world, simData.cells, (HashSet<int>) cavityCells, this.successCallbackFn);
    SeededRandom rnd = new SeededRandom(this.data.globalTerrainSeed);
    HashSet<int> alreadyOccupiedCells = new HashSet<int>();
    for (int index = 0; index < this.TerrainCells.Count; ++index)
    {
      float completePercent = (float) index / (float) this.TerrainCells.Count;
      int num = this.successCallbackFn(UI.WORLDGEN.PLACINGCREATURES.key, completePercent, WorldGenProgressStages.Stages.PlacingCreatures) ? 1 : 0;
      TerrainCell terrainCell = this.TerrainCells[index];
      Dictionary<int, string> newItems1 = MobSpawning.PlaceFeatureAmbientMobs(this.Settings, terrainCell, rnd, ref simData, (HashSet<int>) cavityCells, claimedCells, this.isRunningDebugGen, ref alreadyOccupiedCells);
      if (newItems1 != null)
        this.data.gameSpawnData.AddRange((IEnumerable<KeyValuePair<int, string>>) newItems1);
      Dictionary<int, string> newItems2 = MobSpawning.PlaceBiomeAmbientMobs(this.Settings, terrainCell, rnd, ref simData, (HashSet<int>) cavityCells, claimedCells, this.isRunningDebugGen, ref alreadyOccupiedCells);
      if (newItems2 != null)
        this.data.gameSpawnData.AddRange((IEnumerable<KeyValuePair<int, string>>) newItems2);
    }
    int num1 = this.successCallbackFn(UI.WORLDGEN.PLACINGCREATURES.key, 1f, WorldGenProgressStages.Stages.PlacingCreatures) ? 1 : 0;
    cavityCells.Recycle();
  }

  public void ReportWorldGenError(Exception e, string errorMessage = null)
  {
    if (errorMessage == null)
      errorMessage = (string) UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE;
    bool flag = FileSystem.IsModdedFile(SettingsCache.RewriteWorldgenPathYaml(this.Settings.world.filePath));
    string str = (UnityEngine.Object) CustomGameSettings.Instance != (UnityEngine.Object) null ? CustomGameSettings.Instance.GetSettingsCoordinate() : this.data.globalWorldLayoutSeed.ToString();
    Debug.LogWarning((object) $"Worldgen Failure on seed {str}, modded={flag}");
    if (this.errorCallback != null)
      this.errorCallback(new OfflineWorldGen.ErrorInfo()
      {
        errorDesc = string.Format(errorMessage, (object) str),
        exception = e
      });
    GenericGameSettings.instance.devAutoWorldGenActive = false;
    if (flag)
      return;
    KCrashReporter.ReportError("WorldgenFailure: ", e.StackTrace, (ConfirmDialogScreen) null, (GameObject) null, $"{str} - {e.Message}", false, new string[1]
    {
      KCrashReporter.CRASH_CATEGORY.WORLDGENFAILURE
    });
  }

  public void SetWorldSize(int width, int height)
  {
    this.data.world = new Chunk(0, 0, width, height);
  }

  public void SetHiddenYOffset(int offset) => this.data.world.hiddenY = offset;

  public Vector2I GetSize() => this.data.world.size;

  public void SetPosition(Vector2I position) => this.data.world.offset = position;

  public Vector2I GetPosition() => this.data.world.offset;

  public void SetClusterLocation(AxialI location) => this.data.clusterLocation = location;

  public AxialI GetClusterLocation() => this.data.clusterLocation;

  public bool GenerateNoiseData(WorldGen.OfflineCallbackFunction updateProgressFn)
  {
    try
    {
      this.running = updateProgressFn(UI.WORLDGEN.SETUPNOISE.key, 0.0f, WorldGenProgressStages.Stages.SetupNoise);
      if (!this.running)
        return false;
      this.SetupNoise(updateProgressFn);
      this.running = updateProgressFn(UI.WORLDGEN.SETUPNOISE.key, 1f, WorldGenProgressStages.Stages.SetupNoise);
      if (!this.running)
        return false;
      this.GenerateUnChunkedNoise(updateProgressFn);
    }
    catch (Exception ex)
    {
      string message = ex.Message;
      string stackTrace = ex.StackTrace;
      if (!this.isRunningDebugGen)
        this.ReportWorldGenError(ex);
      string stack = stackTrace;
      WorldGenLogger.LogException(message, stack);
      this.running = this.successCallbackFn(new StringKey("Exception in GenerateNoiseData"), -1f, WorldGenProgressStages.Stages.Failure);
      return false;
    }
    return true;
  }

  public bool GenerateLayout(WorldGen.OfflineCallbackFunction updateProgressFn)
  {
    try
    {
      this.running = updateProgressFn(UI.WORLDGEN.WORLDLAYOUT.key, 0.0f, WorldGenProgressStages.Stages.WorldLayout);
      if (!this.running)
        return false;
      Debug.Assert(this.data.world.size.x != 0 && this.data.world.size.y != 0, (object) "Map size has not been set");
      this.data.worldLayout = new WorldLayout(this, this.data.world.size.x, this.data.world.size.y, this.data.globalWorldLayoutSeed);
      this.running = updateProgressFn(UI.WORLDGEN.WORLDLAYOUT.key, 1f, WorldGenProgressStages.Stages.WorldLayout);
      this.data.voronoiTree = (VoronoiTree.Tree) null;
      try
      {
        this.data.voronoiTree = this.WorldLayout.GenerateOverworld(this.Settings.world.layoutMethod == ProcGen.World.LayoutMethod.PowerTree, this.isRunningDebugGen);
        this.WorldLayout.PopulateSubworlds();
        this.CompleteLayout(updateProgressFn);
      }
      catch (Exception ex)
      {
        WorldGenLogger.LogException(ex.Message, ex.StackTrace);
        if (!this.isRunningDebugGen)
          this.ReportWorldGenError(ex);
        this.running = updateProgressFn(new StringKey("Exception in InitVoronoiTree"), -1f, WorldGenProgressStages.Stages.Failure);
        return false;
      }
      this.data.overworldCells = new List<TerrainCell>(40);
      for (int childIndex = 0; childIndex < this.data.voronoiTree.ChildCount(); ++childIndex)
      {
        VoronoiTree.Tree child = this.data.voronoiTree.GetChild(childIndex) as VoronoiTree.Tree;
        TerrainCellLogged terrainCellLogged = new TerrainCellLogged(this.data.worldLayout.overworldGraph.FindNodeByID(child.site.id), child.site, child.minDistanceToTag);
        UnityEngine.Rect bounds = terrainCellLogged.poly.bounds;
        double xMax = (double) bounds.xMax;
        bounds = terrainCellLogged.poly.bounds;
        double xMin = (double) bounds.xMin;
        double x = (double) (int) Mathf.Floor((float) (xMax - xMin));
        bounds = terrainCellLogged.poly.bounds;
        double yMax = (double) bounds.yMax;
        bounds = terrainCellLogged.poly.bounds;
        double yMin = (double) bounds.yMin;
        double y = (double) (int) Mathf.Floor((float) (yMax - yMin));
        Vector2 vector2 = new Vector2((float) x, (float) y);
        Vector2 centerOfOverworldCell = terrainCellLogged.poly.Centroid();
        TerrainCell terrainCell = this.data.terrainCells.Find((Predicate<TerrainCell>) (tc => tc.poly.PointInPolygon(centerOfOverworldCell)));
        if (terrainCell != null)
          terrainCellLogged.biomeIdx = terrainCell.biomeIdx;
        this.data.overworldCells.Add((TerrainCell) terrainCellLogged);
      }
      this.running = updateProgressFn(UI.WORLDGEN.WORLDLAYOUT.key, 1f, WorldGenProgressStages.Stages.WorldLayout);
    }
    catch (Exception ex)
    {
      WorldGenLogger.LogException(ex.Message, ex.StackTrace);
      this.ReportWorldGenError(ex);
      int num = this.successCallbackFn(new StringKey("Exception in GenerateLayout"), -1f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
      return false;
    }
    return true;
  }

  public bool CompleteLayout(WorldGen.OfflineCallbackFunction updateProgressFn)
  {
    try
    {
      this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 0.0f, WorldGenProgressStages.Stages.CompleteLayout);
      if (!this.running)
        return false;
      this.data.terrainCells = (List<TerrainCell>) null;
      this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 0.65f, WorldGenProgressStages.Stages.CompleteLayout);
      if (!this.running)
        return false;
      this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 0.75f, WorldGenProgressStages.Stages.CompleteLayout);
      if (!this.running)
        return false;
      this.data.terrainCells = new List<TerrainCell>(4000);
      List<VoronoiTree.Node> nodes = new List<VoronoiTree.Node>();
      this.data.voronoiTree.ForceLowestToLeaf();
      this.ApplyStartNode();
      this.ApplySwapTags();
      this.data.voronoiTree.GetLeafNodes(nodes);
      WorldLayout.ResetMapGraphFromVoronoiTree(nodes, this.WorldLayout.localGraph, true);
      for (int index = 0; index < nodes.Count; ++index)
      {
        VoronoiTree.Node node = nodes[index];
        ProcGen.Map.Cell tn = this.data.worldLayout.localGraph.FindNodeByID(node.site.id);
        if (tn != null)
        {
          TerrainCell terrainCell = this.data.terrainCells.Find((Predicate<TerrainCell>) (c => c.node == tn));
          if (terrainCell == null)
            this.data.terrainCells.Add((TerrainCell) new TerrainCellLogged(tn, node.site, node.parent.minDistanceToTag));
          else
            Debug.LogWarning((object) ("Duplicate cell found" + terrainCell.node.NodeId.ToString()));
        }
      }
      for (int index1 = 0; index1 < this.data.terrainCells.Count; ++index1)
      {
        TerrainCell terrainCell1 = this.data.terrainCells[index1];
        for (int index2 = index1 + 1; index2 < this.data.terrainCells.Count; ++index2)
        {
          int edgeIdx = 0;
          TerrainCell terrainCell2 = this.data.terrainCells[index2];
          if (terrainCell2.poly.SharesEdge(terrainCell1.poly, ref edgeIdx, out LineSegment _) == Polygon.Commonality.Edge)
          {
            terrainCell1.neighbourTerrainCells.Add(index2);
            terrainCell2.neighbourTerrainCells.Add(index1);
          }
        }
      }
      this.IdentifyAllBiomes();
      this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 1f, WorldGenProgressStages.Stages.CompleteLayout);
    }
    catch (Exception ex)
    {
      WorldGenLogger.LogException(ex.Message, ex.StackTrace);
      int num = this.successCallbackFn(new StringKey("Exception in CompleteLayout"), -1f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
      return false;
    }
    return true;
  }

  public void IdentifyAllBiomes()
  {
    List<TerrainCell> collection = new List<TerrainCell>(this.data.terrainCells.Count);
    for (int index1 = 0; index1 < this.data.terrainCells.Count; ++index1)
    {
      TerrainCell terrainCell1 = this.data.terrainCells[index1];
      if (terrainCell1.biomeIdx == byte.MaxValue)
      {
        Vector4 calculatedBiomeSize = new Vector4(terrainCell1.poly.MinX, terrainCell1.poly.MinY, terrainCell1.poly.MaxX, terrainCell1.poly.MaxY);
        SubWorld.ZoneType biomeType = this.Settings.GetSubWorld(terrainCell1.node.GetSubworld()).zoneType;
        Action<TerrainCell, List<TerrainCell>> action = (Action<TerrainCell, List<TerrainCell>>) ((cell, biomeCells) =>
        {
          foreach (int neighbourTerrainCell in cell.neighbourTerrainCells)
          {
            TerrainCell terrainCell2 = this.data.terrainCells[neighbourTerrainCell];
            SubWorld.ZoneType zoneType = this.Settings.GetSubWorld(terrainCell2.node.GetSubworld()).zoneType;
            if (!biomeCells.Contains(terrainCell2) & terrainCell2.biomeIdx == byte.MaxValue & zoneType == biomeType)
            {
              UnityEngine.Rect bounds = terrainCell2.poly.bounds;
              if ((double) bounds.xMin < (double) calculatedBiomeSize.x)
              {
                ref Vector4 local = ref calculatedBiomeSize;
                bounds = terrainCell2.poly.bounds;
                double xMin = (double) bounds.xMin;
                local.x = (float) xMin;
              }
              bounds = terrainCell2.poly.bounds;
              if ((double) bounds.yMin < (double) calculatedBiomeSize.y)
              {
                ref Vector4 local = ref calculatedBiomeSize;
                bounds = terrainCell2.poly.bounds;
                double yMin = (double) bounds.yMin;
                local.y = (float) yMin;
              }
              bounds = terrainCell2.poly.bounds;
              if ((double) bounds.xMax > (double) calculatedBiomeSize.z)
              {
                ref Vector4 local = ref calculatedBiomeSize;
                bounds = terrainCell2.poly.bounds;
                double xMax = (double) bounds.xMax;
                local.z = (float) xMax;
              }
              bounds = terrainCell2.poly.bounds;
              if ((double) bounds.yMax > (double) calculatedBiomeSize.w)
              {
                ref Vector4 local = ref calculatedBiomeSize;
                bounds = terrainCell2.poly.bounds;
                double yMax = (double) bounds.yMax;
                local.w = (float) yMax;
              }
              biomeCells.Add(terrainCell2);
            }
          }
        });
        collection.Clear();
        collection.Add(terrainCell1);
        int index2 = 0;
        do
        {
          action(collection[index2], collection);
          ++index2;
        }
        while (index2 < collection.Count);
        for (int index3 = 0; index3 < collection.Count; ++index3)
          collection[index3].biomeIdx = (byte) this.data.biomes.Count;
        this.data.biomes.Add(new BiomeSizeData(biomeType, calculatedBiomeSize, new List<TerrainCell>((IEnumerable<TerrainCell>) collection)));
        Debug.Assert(this.data.biomes.Count < (int) byte.MaxValue, (object) "Error: This Seed has too many biomes");
      }
    }
  }

  public void UpdateVoronoiNodeTags(VoronoiTree.Node node)
  {
    (!node.tags.Contains(WorldGenTags.Overworld) ? (ProcGen.Node) this.WorldLayout.localGraph.FindNodeByID(node.site.id) : (ProcGen.Node) this.WorldLayout.overworldGraph.FindNodeByID(node.site.id))?.tags.Union(node.tags);
  }

  public bool GenerateWorldData()
  {
    return this.GenerateNoiseData(this.successCallbackFn) && this.GenerateLayout(this.successCallbackFn);
  }

  public void EnsureEnoughElementsInStartingBiome(ref WorldgenSimData simData)
  {
    List<StartingWorldElementSetting> startingElements = this.Settings.GetDefaultStartingElements();
    List<TerrainCell> terrainCellsForTag = this.GetTerrainCellsForTag(WorldGenTags.StartWorld);
    foreach (StartingWorldElementSetting worldElementSetting in startingElements)
    {
      float amount = worldElementSetting.amount;
      Element element = ElementLoader.GetElement(new Tag(((SimHashes) Enum.Parse(typeof (SimHashes), worldElementSetting.element, true)).ToString()));
      float num1 = 0.0f;
      int num2 = 0;
      foreach (TerrainCell terrainCell in terrainCellsForTag)
      {
        foreach (int allCell in terrainCell.GetAllCells())
        {
          if ((int) element.idx == (int) simData.cells[allCell].elementIdx)
          {
            ++num2;
            num1 += simData.cells[allCell].mass;
          }
        }
      }
      DebugUtil.DevAssert(num2 > 0, $"No {element.id} found in starting biome and trying to ensure at least {amount}. Skipping.");
      if ((double) num1 < (double) amount && num2 > 0)
      {
        float num3 = num1 / (float) num2;
        float num4 = (amount - num1) / (float) num2;
        DebugUtil.DevAssert(((double) num3 + (double) num4 <= 2.0 * (double) element.maxMass ? 1 : 0) != 0, $"Number of cells ({num2}) of {element.id} in the starting biome is insufficient, this will result in extremely dense cells. {(ValueType) (float) ((double) num3 + (double) num4)} but expecting less than {(ValueType) (float) (2.0 * (double) element.maxMass)}");
        foreach (TerrainCell terrainCell in terrainCellsForTag)
        {
          foreach (int allCell in terrainCell.GetAllCells())
          {
            if ((int) element.idx == (int) simData.cells[allCell].elementIdx)
              simData.cells[allCell].mass += num4;
          }
        }
      }
    }
  }

  public bool RenderToMap(
    WorldGen.OfflineCallbackFunction updateProgressFn,
    ref WorldgenSimData simData,
    ref HashSet<int> borderCells,
    ref List<RectInt> poiBounds)
  {
    Debug.Assert(Grid.WidthInCells == this.Settings.world.worldsize.x);
    Debug.Assert(Grid.HeightInCells == this.Settings.world.worldsize.y);
    Debug.Assert(Grid.CellCount == Grid.WidthInCells * Grid.HeightInCells);
    Debug.Assert((double) Grid.CellSizeInMeters != 0.0);
    borderCells = new HashSet<int>();
    simData.Init(Grid.CellCount);
    this.running = updateProgressFn(UI.WORLDGEN.CLEARINGLEVEL.key, 0.0f, WorldGenProgressStages.Stages.ClearingLevel);
    if (!this.running)
      return false;
    for (int index = 0; index < Grid.CellCount; ++index)
    {
      simData.cells[index].SetValues(WorldGen.katairiteElement, ElementLoader.elements);
      simData.diseaseCells[index] = new Sim.DiseaseCell();
      simData.diseaseCells[index].diseaseIdx = byte.MaxValue;
      this.running = updateProgressFn(UI.WORLDGEN.CLEARINGLEVEL.key, (float) index / (float) Grid.CellCount, WorldGenProgressStages.Stages.ClearingLevel);
      if (!this.running)
        return false;
    }
    int num1 = updateProgressFn(UI.WORLDGEN.CLEARINGLEVEL.key, 1f, WorldGenProgressStages.Stages.ClearingLevel) ? 1 : 0;
    try
    {
      this.ProcessByTerrainCell(ref simData, updateProgressFn, this.highPriorityClaims);
    }
    catch (Exception ex)
    {
      WorldGenLogger.LogException(ex.Message, ex.StackTrace);
      this.running = updateProgressFn(new StringKey("Exception in ProcessByTerrainCell"), -1f, WorldGenProgressStages.Stages.Failure);
      return false;
    }
    if (this.Settings.GetBoolSetting("DrawWorldBorder"))
    {
      SeededRandom rnd = new SeededRandom(0);
      this.DrawWorldBorder(ref simData, this.data.world, rnd, ref borderCells, ref poiBounds, updateProgressFn);
      int num2 = updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, 1f, WorldGenProgressStages.Stages.DrawWorldBorder) ? 1 : 0;
    }
    this.data.gameSpawnData.baseStartPos = this.data.worldLayout.GetStartLocation();
    foreach (ProcGen.World.ModifyLayoutTagsRule modifyLayoutTag in this.Settings.world.modifyLayoutTags)
    {
      foreach (TerrainCell terrainCell in this.data.terrainCells)
      {
        if (TemplateSpawning.DoesCellMatchFilters(terrainCell, modifyLayoutTag.allowedCellsFilter))
        {
          foreach (string addTag in modifyLayoutTag.addTags)
            terrainCell.node.tags.Add((Tag) addTag);
          foreach (string removeTag in modifyLayoutTag.removeTags)
            terrainCell.node.tags.Remove((Tag) removeTag);
        }
      }
    }
    return true;
  }

  public SubWorld GetSubWorldForNode(VoronoiTree.Tree node)
  {
    ProcGen.Node nodeById = (ProcGen.Node) this.WorldLayout.overworldGraph.FindNodeByID(node.site.id);
    if (nodeById == null)
      return (SubWorld) null;
    return !this.Settings.HasSubworld(nodeById.type) ? (SubWorld) null : this.Settings.GetSubWorld(nodeById.type);
  }

  public VoronoiTree.Tree GetOverworldForNode(Leaf leaf)
  {
    return leaf == null ? (VoronoiTree.Tree) null : this.data.worldLayout.GetVoronoiTree().GetChildContainingLeaf(leaf);
  }

  public Leaf GetLeafForTerrainCell(TerrainCell cell)
  {
    return cell == null ? (Leaf) null : this.data.worldLayout.GetVoronoiTree().GetNodeForSite(cell.site) as Leaf;
  }

  public List<TerrainCell> GetTerrainCellsForTag(Tag tag)
  {
    List<TerrainCell> terrainCellsForTag = new List<TerrainCell>();
    List<VoronoiTree.Node> leafNodesWithTag = this.WorldLayout.GetLeafNodesWithTag(tag);
    for (int index = 0; index < leafNodesWithTag.Count; ++index)
    {
      VoronoiTree.Node node = leafNodesWithTag[index];
      TerrainCell terrainCell = this.data.terrainCells.Find((Predicate<TerrainCell>) (cell => (int) cell.site.id == (int) node.site.id));
      if (terrainCell != null)
        terrainCellsForTag.Add(terrainCell);
    }
    return terrainCellsForTag;
  }

  private void GetStartCells(out int baseX, out int baseY)
  {
    Vector2I vector2I = new Vector2I(this.data.world.size.x / 2, (int) ((double) this.data.world.size.y * 0.699999988079071));
    if (this.data.worldLayout != null)
      vector2I = this.data.worldLayout.GetStartLocation();
    baseX = vector2I.x;
    baseY = vector2I.y;
  }

  public void FinalizeStartLocation()
  {
    if (string.IsNullOrEmpty(this.Settings.world.startSubworldName))
      return;
    List<VoronoiTree.Node> startNodes = this.WorldLayout.GetStartNodes();
    Debug.Assert(startNodes.Count > 0, (object) "Couldn't find a start node on a world that expects it!!");
    TagSet other = new TagSet()
    {
      WorldGenTags.StartLocation
    };
    for (int index = 1; index < startNodes.Count; ++index)
      startNodes[index].tags.Remove(other);
  }

  private void SwitchNodes(VoronoiTree.Node n1, VoronoiTree.Node n2)
  {
    if (n1 is VoronoiTree.Tree || n2 is VoronoiTree.Tree)
    {
      Debug.Log((object) "WorldGen::SwitchNodes() Skipping tree node");
    }
    else
    {
      Diagram.Site site = n1.site;
      n1.site = n2.site;
      n2.site = site;
      ProcGen.Map.Cell nodeById1 = this.data.worldLayout.localGraph.FindNodeByID(n1.site.id);
      ProcGen.Node nodeById2 = (ProcGen.Node) this.data.worldLayout.localGraph.FindNodeByID(n2.site.id);
      string type = nodeById1.type;
      nodeById1.SetType(nodeById2.type);
      nodeById2.SetType(type);
    }
  }

  private void ApplyStartNode()
  {
    List<VoronoiTree.Node> leafNodesWithTag = this.data.worldLayout.GetLeafNodesWithTag(WorldGenTags.StartLocation);
    if (leafNodesWithTag.Count == 0)
      return;
    VoronoiTree.Node node = leafNodesWithTag[0];
    VoronoiTree.Tree parent = node.parent;
    node.parent.AddTagToChildren(WorldGenTags.IgnoreCaveOverride);
    node.parent.tags.Remove(WorldGenTags.StartLocation);
  }

  private void ApplySwapTags()
  {
    List<VoronoiTree.Node> nodeList = new List<VoronoiTree.Node>();
    for (int childIndex = 0; childIndex < this.data.voronoiTree.ChildCount(); ++childIndex)
    {
      if (this.data.voronoiTree.GetChild(childIndex).tags.Contains(WorldGenTags.SwapLakesToBelow))
        nodeList.Add(this.data.voronoiTree.GetChild(childIndex));
    }
    foreach (VoronoiTree.Node node in nodeList)
    {
      if (!node.tags.Contains(WorldGenTags.CenteralFeature))
      {
        List<VoronoiTree.Node> nodes = new List<VoronoiTree.Node>();
        ((VoronoiTree.Tree) node).GetNodesWithoutTag(WorldGenTags.CenteralFeature, nodes);
        this.SwapNodesAround(WorldGenTags.Wet, true, nodes, node.site.poly.Centroid());
      }
    }
  }

  private void SwapNodesAround(Tag swapTag, bool sendTagToBottom, List<VoronoiTree.Node> nodes, Vector2 pivot)
  {
    nodes.ShuffleSeeded<VoronoiTree.Node>(this.myRandom.RandomSource());
    List<VoronoiTree.Node> nodeList1 = new List<VoronoiTree.Node>();
    List<VoronoiTree.Node> nodeList2 = new List<VoronoiTree.Node>();
    foreach (VoronoiTree.Node node in nodes)
    {
      bool flag1 = node.tags.Contains(swapTag);
      bool flag2 = (double) node.site.poly.Centroid().y > (double) pivot.y;
      bool flag3 = flag2 & sendTagToBottom || !flag2 && !sendTagToBottom;
      if (flag1 & flag3)
      {
        if (nodeList2.Count > 0)
        {
          this.SwitchNodes(node, nodeList2[0]);
          nodeList2.RemoveAt(0);
        }
        else
          nodeList1.Add(node);
      }
      else if (!flag1 && !flag3)
      {
        if (nodeList1.Count > 0)
        {
          this.SwitchNodes(node, nodeList1[0]);
          nodeList1.RemoveAt(0);
        }
        else
          nodeList2.Add(node);
      }
    }
    if (nodeList2.Count <= 0)
      return;
    for (int index = 0; index < nodeList1.Count && nodeList2.Count > 0; ++index)
    {
      this.SwitchNodes(nodeList1[index], nodeList2[0]);
      nodeList2.RemoveAt(0);
    }
  }

  public void GetElementForBiomePoint(
    Chunk chunk,
    ElementBandConfiguration elementBands,
    Vector2I pos,
    out Element element,
    out Sim.PhysicsData pd,
    out Sim.DiseaseCell dc,
    float erode)
  {
    TerrainCell.GetElementOverride(WorldGen.voidElement.tag.ToString(), (SampleDescriber.Override) null);
    TerrainCell.ElementOverride biomeElementTable = this.GetElementFromBiomeElementTable(chunk, pos, (List<ElementGradient>) elementBands, erode);
    element = biomeElementTable.element;
    pd = biomeElementTable.pdelement;
    dc = biomeElementTable.dc;
  }

  public void GetElementForBackwallBiomePoint(
    Chunk chunk,
    ElementBandConfiguration elementBands,
    Vector2I pos,
    out Element element,
    out Sim.PhysicsData pd,
    out Sim.DiseaseCell dc)
  {
    TerrainCell.ElementOverride backwallElementTable = this.GetElementFromBiomeBackwallElementTable(chunk, pos, (List<ElementGradient>) elementBands);
    element = backwallElementTable.element;
    pd = backwallElementTable.pdelement;
    dc = backwallElementTable.dc;
  }

  public void ConvertIntersectingCellsToType(MathUtil.Pair<Vector2, Vector2> segment, string type)
  {
    List<Vector2I> line = ProcGen.Util.GetLine(segment.First, segment.Second);
    for (int index1 = 0; index1 < this.data.terrainCells.Count; ++index1)
    {
      if (this.data.terrainCells[index1].node.type != type)
      {
        for (int index2 = 0; index2 < line.Count; ++index2)
        {
          if (this.data.terrainCells[index1].poly.Contains((Vector2) line[index2]))
            this.data.terrainCells[index1].node.SetType(type);
        }
      }
    }
  }

  public string GetSubWorldType(Vector2I pos)
  {
    for (int index = 0; index < this.data.overworldCells.Count; ++index)
    {
      if (this.data.overworldCells[index].poly.Contains((Vector2) pos))
        return this.data.overworldCells[index].node.type;
    }
    return (string) null;
  }

  private void ProcessByTerrainCell(
    ref WorldgenSimData simData,
    WorldGen.OfflineCallbackFunction updateProgressFn,
    HashSet<int> highPriorityCells)
  {
    int num1 = updateProgressFn(UI.WORLDGEN.PROCESSING.key, 0.0f, WorldGenProgressStages.Stages.Processing) ? 1 : 0;
    SeededRandom rnd = new SeededRandom(this.data.globalTerrainSeed);
    try
    {
      for (int index = 0; index < this.data.terrainCells.Count; ++index)
      {
        int num2 = updateProgressFn(UI.WORLDGEN.PROCESSING.key, (float) index / (float) this.data.terrainCells.Count, WorldGenProgressStages.Stages.Processing) ? 1 : 0;
        this.data.terrainCells[index].Process(this, ref simData, this.data.world, rnd);
      }
    }
    catch (Exception ex)
    {
      string message = ex.Message;
      string stackTrace = ex.StackTrace;
      int num3 = updateProgressFn(new StringKey("Exception in TerrainCell.Process"), -1f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
      Debug.LogError((object) $"Error:{message}\n{stackTrace}");
    }
    List<Border> borderList = new List<Border>();
    int num4 = updateProgressFn(UI.WORLDGEN.BORDERS.key, 0.0f, WorldGenProgressStages.Stages.Borders) ? 1 : 0;
    try
    {
      List<ProcGen.Map.Edge> edgesWithTag1 = this.data.worldLayout.overworldGraph.GetEdgesWithTag(WorldGenTags.EdgeUnpassable);
      for (int index = 0; index < edgesWithTag1.Count; ++index)
      {
        ProcGen.Map.Edge arc = edgesWithTag1[index];
        List<ProcGen.Map.Cell> cells = this.data.worldLayout.overworldGraph.GetNodes(arc);
        Debug.Assert(cells[0] != cells[1], (object) "Both nodes on an arc were the same. Allegedly this means it was a world border but I don't think we do that anymore.");
        TerrainCell a = this.data.overworldCells.Find((Predicate<TerrainCell>) (c => c.node == cells[0]));
        TerrainCell b = this.data.overworldCells.Find((Predicate<TerrainCell>) (c => c.node == cells[1]));
        Debug.Assert(a != null && b != null, (object) "NULL Terrainell nodes with EdgeUnpassable");
        a.LogInfo("BORDER WITH " + b.site.id.ToString(), "UNPASSABLE", 0.0f);
        b.LogInfo("BORDER WITH " + a.site.id.ToString(), "UNPASSABLE", 0.0f);
        borderList.Add(new Border(new Neighbors(a, b), arc.corner0.position, arc.corner1.position)
        {
          element = SettingsCache.borders["impenetrable"],
          width = (float) rnd.RandomRange(2, 3)
        });
      }
      List<ProcGen.Map.Edge> edgesWithTag2 = this.data.worldLayout.overworldGraph.GetEdgesWithTag(WorldGenTags.EdgeClosed);
      for (int index = 0; index < edgesWithTag2.Count; ++index)
      {
        ProcGen.Map.Edge arc = edgesWithTag2[index];
        if (!edgesWithTag1.Contains(arc))
        {
          List<ProcGen.Map.Cell> cells = this.data.worldLayout.overworldGraph.GetNodes(arc);
          Debug.Assert(cells[0] != cells[1], (object) "Both nodes on an arc were the same. Allegedly this means it was a world border but I don't think we do that anymore.");
          TerrainCell a = this.data.overworldCells.Find((Predicate<TerrainCell>) (c => c.node == cells[0]));
          TerrainCell b = this.data.overworldCells.Find((Predicate<TerrainCell>) (c => c.node == cells[1]));
          Debug.Assert(a != null && b != null, (object) "NULL Terraincell nodes with EdgeClosed");
          string borderOverride1 = this.Settings.GetSubWorld(a.node.type).borderOverride;
          string borderOverride2 = this.Settings.GetSubWorld(b.node.type).borderOverride;
          string key;
          if (!string.IsNullOrEmpty(borderOverride2) && !string.IsNullOrEmpty(borderOverride1))
          {
            int overridePriority1 = this.Settings.GetSubWorld(a.node.type).borderOverridePriority;
            int overridePriority2 = this.Settings.GetSubWorld(b.node.type).borderOverridePriority;
            if (overridePriority1 == overridePriority2)
            {
              key = (double) rnd.RandomValue() > 0.5 ? borderOverride2 : borderOverride1;
              a.LogInfo("BORDER WITH " + b.site.id.ToString(), "Picked Random:" + key, 0.0f);
              b.LogInfo("BORDER WITH " + a.site.id.ToString(), "Picked Random:" + key, 0.0f);
            }
            else
            {
              key = overridePriority1 > overridePriority2 ? borderOverride1 : borderOverride2;
              a.LogInfo("BORDER WITH " + b.site.id.ToString(), "Picked priority:" + key, 0.0f);
              b.LogInfo("BORDER WITH " + a.site.id.ToString(), "Picked priority:" + key, 0.0f);
            }
          }
          else if (string.IsNullOrEmpty(borderOverride2) && string.IsNullOrEmpty(borderOverride1))
          {
            key = "hardToDig";
            a.LogInfo("BORDER WITH " + b.site.id.ToString(), "Both null", 0.0f);
            b.LogInfo("BORDER WITH " + a.site.id.ToString(), "Both null", 0.0f);
          }
          else
          {
            key = !string.IsNullOrEmpty(borderOverride2) ? borderOverride2 : borderOverride1;
            a.LogInfo("BORDER WITH " + b.site.id.ToString(), "Picked specific " + key, 0.0f);
            b.LogInfo("BORDER WITH " + a.site.id.ToString(), "Picked specific " + key, 0.0f);
          }
          if (!(key == "NONE"))
          {
            Border border = new Border(new Neighbors(a, b), arc.corner0.position, arc.corner1.position);
            border.element = SettingsCache.borders[key];
            ProcGen.MinMax minMax = new ProcGen.MinMax(1.5f, 2f);
            ProcGen.MinMax borderSizeOverride1 = this.Settings.GetSubWorld(a.node.type).borderSizeOverride;
            ProcGen.MinMax borderSizeOverride2 = this.Settings.GetSubWorld(b.node.type).borderSizeOverride;
            bool flag1 = (double) borderSizeOverride1.min != 0.0 || (double) borderSizeOverride1.max != 0.0;
            bool flag2 = (double) borderSizeOverride2.min != 0.0 || (double) borderSizeOverride2.max != 0.0;
            if (flag1 & flag2)
              minMax = (double) borderSizeOverride1.max > (double) borderSizeOverride2.max ? borderSizeOverride1 : borderSizeOverride2;
            else if (flag1)
              minMax = borderSizeOverride1;
            else if (flag2)
              minMax = borderSizeOverride2;
            border.width = rnd.RandomRange(minMax.min, minMax.max);
            borderList.Add(border);
          }
        }
      }
    }
    catch (Exception ex)
    {
      string message = ex.Message;
      string stackTrace = ex.StackTrace;
      int num5 = updateProgressFn(new StringKey("Exception in Border creation"), -1f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
      Debug.LogError((object) $"Error:{message} {stackTrace}");
    }
    try
    {
      WorldGen.ProcessByTerrainCellSetter terrainCellSetter = new WorldGen.ProcessByTerrainCellSetter()
      {
        data = simData,
        blacklistCells = this.highPriorityClaims
      };
      for (int index = 0; index < borderList.Count; ++index)
      {
        Border border = borderList[index];
        SubWorld subWorld1 = this.Settings.GetSubWorld(border.neighbors.n0.node.type);
        SubWorld subWorld2 = this.Settings.GetSubWorld(border.neighbors.n1.node.type);
        float neighbour0Temperature = (float) (((double) SettingsCache.temperatures[subWorld1.temperatureRange].min + (double) SettingsCache.temperatures[subWorld1.temperatureRange].max) / 2.0);
        float neighbour1Temperature = (float) (((double) SettingsCache.temperatures[subWorld2.temperatureRange].min + (double) SettingsCache.temperatures[subWorld2.temperatureRange].max) / 2.0);
        float num6 = Mathf.Min(SettingsCache.temperatures[subWorld1.temperatureRange].min, SettingsCache.temperatures[subWorld2.temperatureRange].min);
        double num7 = (double) Mathf.Max(SettingsCache.temperatures[subWorld1.temperatureRange].max, SettingsCache.temperatures[subWorld2.temperatureRange].max);
        float midTemp = (float) (((double) neighbour0Temperature + (double) neighbour1Temperature) / 2.0);
        double num8 = (double) num6;
        double num9 = num7 - num8;
        float rangeLow = 2f;
        float rangeHigh = 5f;
        int snapLastCells = 1;
        if (num9 >= 150.0)
        {
          rangeLow = 0.0f;
          rangeHigh = border.width * 0.2f;
          snapLastCells = 2;
          border.width = Mathf.Max(border.width, 2f);
          double f1 = (double) neighbour0Temperature - 273.14999389648438;
          float f2 = neighbour1Temperature - 273.15f;
          midTemp = (double) Mathf.Abs((float) f1) >= (double) Mathf.Abs(f2) ? neighbour1Temperature : neighbour0Temperature;
        }
        border.Stagger(rnd, (float) rnd.RandomRange(8, 13), rnd.RandomRange(rangeLow, rangeHigh));
        border.ConvertToMap(this.data.world, (TerrainCell.ISimDataSetter) terrainCellSetter, neighbour0Temperature, neighbour1Temperature, midTemp, rnd, snapLastCells);
      }
      simData = terrainCellSetter.data;
    }
    catch (Exception ex)
    {
      string message = ex.Message;
      string stackTrace = ex.StackTrace;
      int num10 = updateProgressFn(new StringKey("Exception in border.ConvertToMap"), -1f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
      Debug.LogError((object) $"Error:{message} {stackTrace}");
    }
  }

  private void DrawWorldBorder(
    ref WorldgenSimData simData,
    Chunk world,
    SeededRandom rnd,
    ref HashSet<int> borderCells,
    ref List<RectInt> poiBounds,
    WorldGen.OfflineCallbackFunction updateProgressFn)
  {
    bool boolSetting = this.Settings.GetBoolSetting("DrawWorldBorderForce");
    int intSetting1 = this.Settings.GetIntSetting("WorldBorderThickness");
    int intSetting2 = this.Settings.GetIntSetting("WorldBorderRange");
    ushort idx1 = WorldGen.vacuumElement.idx;
    ushort idx2 = WorldGen.voidElement.idx;
    ushort idx3 = WorldGen.unobtaniumElement.idx;
    float temperature = WorldGen.unobtaniumElement.defaultValues.temperature;
    float mass = WorldGen.unobtaniumElement.defaultValues.mass;
    int num1 = 0;
    int num2 = 0;
    int num3 = updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, 0.0f, WorldGenProgressStages.Stages.DrawWorldBorder) ? 1 : 0;
    int num4 = world.size.y - 1;
    int a1 = 0;
    int num5 = world.size.x - 1;
    List<TerrainCell> terrainCellsForTag = this.GetTerrainCellsForTag(WorldGenTags.RemoveWorldBorderOverVacuum);
    for (int y = num4; y >= 0; y--)
    {
      int num6 = updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, (float) ((double) y / (double) num4 * 0.33000001311302185), WorldGenProgressStages.Stages.DrawWorldBorder) ? 1 : 0;
      num1 = Mathf.Max(-intSetting2, Mathf.Min(num1 + rnd.RandomRange(-2, 2), intSetting2));
      bool flag1 = terrainCellsForTag.Find((Predicate<TerrainCell>) (n => n.poly.Contains(new Vector2(0.0f, (float) y)))) != null;
      for (int index = 0; index < intSetting1 + num1; ++index)
      {
        int cell = Grid.XYToCell(index, y);
        if (boolSetting || (((int) simData.cells[cell].elementIdx == (int) idx1 ? 0 : ((int) simData.cells[cell].elementIdx != (int) idx2 ? 1 : 0)) & (flag1 ? 1 : 0)) != 0 || !flag1)
        {
          borderCells.Add(cell);
          simData.cells[cell].SetValues(idx3, temperature, mass);
          a1 = Mathf.Max(a1, index);
        }
      }
      num2 = Mathf.Max(-intSetting2, Mathf.Min(num2 + rnd.RandomRange(-2, 2), intSetting2));
      bool flag2 = terrainCellsForTag.Find((Predicate<TerrainCell>) (n => n.poly.Contains(new Vector2((float) (world.size.x - 1), (float) y)))) != null;
      for (int index = 0; index < intSetting1 + num2; ++index)
      {
        int num7 = world.size.x - 1 - index;
        int cell = Grid.XYToCell(num7, y);
        if (boolSetting || (((int) simData.cells[cell].elementIdx == (int) idx1 ? 0 : ((int) simData.cells[cell].elementIdx != (int) idx2 ? 1 : 0)) & (flag2 ? 1 : 0)) != 0 || !flag2)
        {
          borderCells.Add(cell);
          simData.cells[cell].SetValues(idx3, temperature, mass);
          num5 = Mathf.Min(num5, num7);
        }
      }
    }
    this.POIBounds.Add(new RectInt(0, 0, a1 + 1, this.World.size.y));
    this.POIBounds.Add(new RectInt(num5, 0, world.size.x - num5, this.World.size.y));
    int num8 = 0;
    int num9 = 0;
    int a2 = 0;
    int num10 = this.World.size.y - 1;
    for (int x = 0; x < world.size.x; x++)
    {
      int num11 = updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, (float) ((double) x / (double) world.size.x * 0.6600000262260437 + 0.33000001311302185), WorldGenProgressStages.Stages.DrawWorldBorder) ? 1 : 0;
      num8 = Mathf.Max(-intSetting2, Mathf.Min(num8 + rnd.RandomRange(-2, 2), intSetting2));
      bool flag3 = terrainCellsForTag.Find((Predicate<TerrainCell>) (n => n.poly.Contains(new Vector2((float) x, 0.0f)))) != null;
      for (int index = 0; index < intSetting1 + num8; ++index)
      {
        int cell = Grid.XYToCell(x, index);
        if (boolSetting || (((int) simData.cells[cell].elementIdx == (int) idx1 ? 0 : ((int) simData.cells[cell].elementIdx != (int) idx2 ? 1 : 0)) & (flag3 ? 1 : 0)) != 0 || !flag3)
        {
          borderCells.Add(cell);
          simData.cells[cell].SetValues(idx3, temperature, mass);
          a2 = Mathf.Max(a2, index);
        }
      }
      num9 = Mathf.Max(-intSetting2, Mathf.Min(num9 + rnd.RandomRange(-2, 2), intSetting2));
      bool flag4 = terrainCellsForTag.Find((Predicate<TerrainCell>) (n => n.poly.Contains(new Vector2((float) x, (float) (world.size.y - 1))))) != null;
      for (int index = 0; index < intSetting1 + num9; ++index)
      {
        int num12 = world.size.y - 1 - index;
        int cell = Grid.XYToCell(x, num12);
        if (boolSetting || (((int) simData.cells[cell].elementIdx == (int) idx1 ? 0 : ((int) simData.cells[cell].elementIdx != (int) idx2 ? 1 : 0)) & (flag4 ? 1 : 0)) != 0 || !flag4)
        {
          borderCells.Add(cell);
          simData.cells[cell].SetValues(idx3, temperature, mass);
          num10 = Mathf.Min(num10, num12);
        }
      }
    }
    this.POIBounds.Add(new RectInt(0, 0, this.World.size.x, a2 + 1));
    this.POIBounds.Add(new RectInt(0, num10, this.World.size.x, this.World.size.y - num10));
  }

  private void SetupNoise(WorldGen.OfflineCallbackFunction updateProgressFn)
  {
    int num1 = updateProgressFn(UI.WORLDGEN.BUILDNOISESOURCE.key, 0.0f, WorldGenProgressStages.Stages.SetupNoise) ? 1 : 0;
    this.heatSource = this.BuildNoiseSource(this.data.world.size.x, this.data.world.size.y, "noise/Heat");
    int num2 = updateProgressFn(UI.WORLDGEN.BUILDNOISESOURCE.key, 1f, WorldGenProgressStages.Stages.SetupNoise) ? 1 : 0;
  }

  public NoiseMapBuilderPlane BuildNoiseSource(int width, int height, string name)
  {
    ProcGen.Noise.Tree tree = SettingsCache.noise.GetTree(name);
    Debug.Assert(tree != null, (object) name);
    return this.BuildNoiseSource(width, height, tree);
  }

  public NoiseMapBuilderPlane BuildNoiseSource(int width, int height, ProcGen.Noise.Tree tree)
  {
    Vector2f lowerBound = tree.settings.lowerBound;
    Vector2f upperBound = tree.settings.upperBound;
    Debug.Assert(((double) lowerBound.x < (double) upperBound.x ? 1 : 0) != 0, (object) $"BuildNoiseSource X range broken [l: {lowerBound.x.ToString()} h: {upperBound.x.ToString()}]");
    Debug.Assert(((double) lowerBound.y < (double) upperBound.y ? 1 : 0) != 0, (object) $"BuildNoiseSource Y range broken [l: {lowerBound.y.ToString()} h: {upperBound.y.ToString()}]");
    Debug.Assert(width > 0, (object) $"BuildNoiseSource width <=0: [{width.ToString()}]");
    Debug.Assert(height > 0, (object) $"BuildNoiseSource height <=0: [{height.ToString()}]");
    NoiseMapBuilderPlane noiseMapBuilderPlane = new NoiseMapBuilderPlane(lowerBound.x, upperBound.x, lowerBound.y, upperBound.y, false);
    noiseMapBuilderPlane.SetSize(width, height);
    noiseMapBuilderPlane.SourceModule = (IModule) tree.BuildFinalModule(this.data.globalNoiseSeed);
    return noiseMapBuilderPlane;
  }

  private void GetMinMaxDataValues(float[] data, int width, int height)
  {
  }

  public static NoiseMap BuildNoiseMap(
    Vector2 offset,
    float zoom,
    NoiseMapBuilderPlane nmbp,
    int width,
    int height,
    NoiseMapBuilderCallback cb = null)
  {
    double x = (double) offset.x;
    double y = (double) offset.y;
    if ((double) zoom == 0.0)
      zoom = 0.01f;
    double lowerXBound = x * (double) zoom;
    double upperXBound = (x + (double) width) * (double) zoom;
    double lowerZBound = y * (double) zoom;
    double upperZBound = (y + (double) height) * (double) zoom;
    NoiseMap noiseMap = new NoiseMap(width, height);
    nmbp.NoiseMap = (IMap2D<float>) noiseMap;
    nmbp.SetBounds((float) lowerXBound, (float) upperXBound, (float) lowerZBound, (float) upperZBound);
    nmbp.CallBack = cb;
    nmbp.Build();
    return noiseMap;
  }

  public static float[] GenerateNoise(
    Vector2 offset,
    float zoom,
    NoiseMapBuilderPlane nmbp,
    int width,
    int height,
    NoiseMapBuilderCallback cb = null)
  {
    NoiseMap noiseMap = WorldGen.BuildNoiseMap(offset, zoom, nmbp, width, height, cb);
    float[] buffer = new float[noiseMap.Width * noiseMap.Height];
    noiseMap.CopyTo(ref buffer);
    return buffer;
  }

  public static void Normalise(float[] data)
  {
    Debug.Assert(data != null && data.Length != 0, (object) "MISSING DATA FOR NORMALIZE");
    float b1 = float.MaxValue;
    float b2 = float.MinValue;
    for (int index = 0; index < data.Length; ++index)
    {
      b1 = Mathf.Min(data[index], b1);
      b2 = Mathf.Max(data[index], b2);
    }
    float num = b2 - b1;
    for (int index = 0; index < data.Length; ++index)
      data[index] = (data[index] - b1) / num;
  }

  private void GenerateUnChunkedNoise(WorldGen.OfflineCallbackFunction updateProgressFn)
  {
    Vector2 offset = new Vector2(0.0f, 0.0f);
    int num1 = updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, 0.0f, WorldGenProgressStages.Stages.GenerateNoise) ? 1 : 0;
    int num2;
    NoiseMapBuilderCallback mapBuilderCallback = (NoiseMapBuilderCallback) (line => num2 = updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, (float) (int) (0.0 + 0.25 * ((double) line / (double) this.data.world.size.y)), WorldGenProgressStages.Stages.GenerateNoise) ? 1 : 0);
    int num3;
    NoiseMapBuilderCallback cb = (NoiseMapBuilderCallback) (line => num3 = updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, (float) (int) (0.25 + 0.25 * ((double) line / (double) this.data.world.size.y)), WorldGenProgressStages.Stages.GenerateNoise) ? 1 : 0);
    if (cb == null)
      Debug.LogError((object) "nupd is null");
    this.data.world.heatOffset = WorldGen.GenerateNoise(offset, SettingsCache.noise.GetZoomForTree("noise/Heat"), this.heatSource, this.data.world.size.x, this.data.world.size.y, cb);
    this.data.world.data = new float[this.data.world.heatOffset.Length];
    this.data.world.density = new float[this.data.world.heatOffset.Length];
    this.data.world.overrides = new float[this.data.world.heatOffset.Length];
    this.data.world.backwallNoise = new float[this.data.world.heatOffset.Length];
    int num4 = updateProgressFn(UI.WORLDGEN.NORMALISENOISE.key, 0.5f, WorldGenProgressStages.Stages.GenerateNoise) ? 1 : 0;
    if (SettingsCache.noise.ShouldNormaliseTree("noise/Heat"))
      WorldGen.Normalise(this.data.world.heatOffset);
    int num5 = updateProgressFn(UI.WORLDGEN.NORMALISENOISE.key, 1f, WorldGenProgressStages.Stages.GenerateNoise) ? 1 : 0;
  }

  public void WriteOverWorldNoise(WorldGen.OfflineCallbackFunction updateProgressFn)
  {
    Dictionary<HashedString, WorldGen.NoiseNormalizationStats> dictionary = new Dictionary<HashedString, WorldGen.NoiseNormalizationStats>();
    float perCell = 1f / (float) this.OverworldCells.Count;
    float currentProgress = 0.0f;
    foreach (TerrainCell overworldCell in this.OverworldCells)
    {
      ProcGen.Noise.Tree tree1 = SettingsCache.noise.GetTree("noise/Default");
      ProcGen.Noise.Tree tree2 = SettingsCache.noise.GetTree("noise/DefaultCave");
      ProcGen.Noise.Tree tree3 = SettingsCache.noise.GetTree("noise/DefaultDensity");
      ProcGen.Noise.Tree tree4 = (ProcGen.Noise.Tree) null;
      string key1 = "noise/Default";
      string key2 = "noise/DefaultCave";
      string key3 = "noise/DefaultDensity";
      string key4 = (string) null;
      SubWorld subWorld = this.Settings.GetSubWorld(overworldCell.node.type);
      if (subWorld == null)
      {
        Debug.Log((object) $"Couldnt find Subworld for overworld node [{overworldCell.node.type}] using defaults");
      }
      else
      {
        if (subWorld.biomeNoise != null)
        {
          ProcGen.Noise.Tree tree5 = SettingsCache.noise.GetTree(subWorld.biomeNoise);
          if (tree5 != null)
          {
            tree1 = tree5;
            key1 = subWorld.biomeNoise;
          }
        }
        if (subWorld.overrideNoise != null)
        {
          ProcGen.Noise.Tree tree6 = SettingsCache.noise.GetTree(subWorld.overrideNoise);
          if (tree6 != null)
          {
            tree2 = tree6;
            key2 = subWorld.overrideNoise;
          }
        }
        if (subWorld.densityNoise != null)
        {
          ProcGen.Noise.Tree tree7 = SettingsCache.noise.GetTree(subWorld.densityNoise);
          if (tree7 != null)
          {
            tree3 = tree7;
            key3 = subWorld.densityNoise;
          }
        }
        if (subWorld.backwallNoise != null)
        {
          ProcGen.Noise.Tree tree8 = SettingsCache.noise.GetTree(subWorld.backwallNoise);
          if (tree8 != null)
          {
            tree4 = tree8;
            key4 = subWorld.backwallNoise;
            overworldCell.spawnBackwall = true;
          }
        }
      }
      WorldGen.NoiseNormalizationStats normalizationStats1 = (WorldGen.NoiseNormalizationStats) null;
      WorldGen.NoiseNormalizationStats normalizationStats2;
      if (!dictionary.TryGetValue((HashedString) key1, out normalizationStats2))
      {
        normalizationStats2 = new WorldGen.NoiseNormalizationStats(this.BaseNoiseMap);
        dictionary.Add((HashedString) key1, normalizationStats2);
      }
      WorldGen.NoiseNormalizationStats normalizationStats3;
      if (!dictionary.TryGetValue((HashedString) key2, out normalizationStats3))
      {
        normalizationStats3 = new WorldGen.NoiseNormalizationStats(this.OverrideMap);
        dictionary.Add((HashedString) key2, normalizationStats3);
      }
      WorldGen.NoiseNormalizationStats normalizationStats4;
      if (!dictionary.TryGetValue((HashedString) key3, out normalizationStats4))
      {
        normalizationStats4 = new WorldGen.NoiseNormalizationStats(this.DensityMap);
        dictionary.Add((HashedString) key3, normalizationStats4);
      }
      if (tree4 != null && !dictionary.TryGetValue((HashedString) key4, out normalizationStats1))
      {
        normalizationStats1 = new WorldGen.NoiseNormalizationStats(this.BackwallMap);
        dictionary.Add((HashedString) key4, normalizationStats1);
      }
      UnityEngine.Rect bounds = overworldCell.poly.bounds;
      int width = (int) Mathf.Ceil(bounds.width + 2f);
      bounds = overworldCell.poly.bounds;
      int height = (int) Mathf.Ceil(bounds.height + 2f);
      bounds = overworldCell.poly.bounds;
      int x1 = (int) Mathf.Floor(bounds.xMin - 1f);
      bounds = overworldCell.poly.bounds;
      int y1 = (int) Mathf.Floor(bounds.yMin - 1f);
      Vector2 offset = new Vector2((float) x1, (float) y1);
      Vector2 point = offset;
      int num1;
      NoiseMapBuilderCallback cb = (NoiseMapBuilderCallback) (line => num1 = updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, (float) (int) ((double) currentProgress + (double) perCell * ((double) line / (double) height)), WorldGenProgressStages.Stages.NoiseMapBuilder) ? 1 : 0);
      NoiseMapBuilderPlane nmbp1 = this.BuildNoiseSource(width, height, tree1);
      NoiseMap noiseMap1 = WorldGen.BuildNoiseMap(offset, tree1.settings.zoom, nmbp1, width, height, cb);
      NoiseMapBuilderPlane nmbp2 = this.BuildNoiseSource(width, height, tree2);
      NoiseMap noiseMap2 = WorldGen.BuildNoiseMap(offset, tree2.settings.zoom, nmbp2, width, height, cb);
      NoiseMapBuilderPlane nmbp3 = this.BuildNoiseSource(width, height, tree3);
      NoiseMap noiseMap3 = WorldGen.BuildNoiseMap(offset, tree3.settings.zoom, nmbp3, width, height, cb);
      NoiseMapBuilderPlane nmbp4 = tree4 != null ? this.BuildNoiseSource(width, height, tree4) : (NoiseMapBuilderPlane) null;
      NoiseMap noiseMap4 = nmbp4 != null ? WorldGen.BuildNoiseMap(offset, tree4.settings.zoom, nmbp4, width, height, cb) : (NoiseMap) null;
      ref Vector2 local1 = ref point;
      bounds = overworldCell.poly.bounds;
      double num2 = (double) (int) Mathf.Floor(bounds.xMin);
      local1.x = (float) num2;
      while (true)
      {
        double x2 = (double) point.x;
        bounds = overworldCell.poly.bounds;
        double num3 = (double) (int) Mathf.Ceil(bounds.xMax);
        if (x2 <= num3)
        {
          ref Vector2 local2 = ref point;
          bounds = overworldCell.poly.bounds;
          double num4 = (double) (int) Mathf.Floor(bounds.yMin);
          local2.y = (float) num4;
          while (true)
          {
            double y2 = (double) point.y;
            bounds = overworldCell.poly.bounds;
            double num5 = (double) (int) Mathf.Ceil(bounds.yMax);
            if (y2 <= num5)
            {
              if (overworldCell.poly.PointInPolygon(point))
              {
                int cell = Grid.XYToCell((int) point.x, (int) point.y);
                if (tree1.settings.normalise)
                  normalizationStats2.cells.Add(cell);
                if (tree2.settings.normalise)
                  normalizationStats3.cells.Add(cell);
                if (tree3.settings.normalise)
                  normalizationStats4.cells.Add(cell);
                if (tree4 != null && tree4.settings.normalise)
                  normalizationStats1.cells.Add(cell);
                int x3 = (int) point.x - x1;
                int y3 = (int) point.y - y1;
                this.BaseNoiseMap[cell] = noiseMap1.GetValue(x3, y3);
                this.OverrideMap[cell] = noiseMap2.GetValue(x3, y3);
                this.DensityMap[cell] = noiseMap3.GetValue(x3, y3);
                if (noiseMap4 != null)
                  this.BackwallMap[cell] = noiseMap4.GetValue(x3, y3);
                normalizationStats2.min = Mathf.Min(this.BaseNoiseMap[cell], normalizationStats2.min);
                normalizationStats2.max = Mathf.Max(this.BaseNoiseMap[cell], normalizationStats2.max);
                normalizationStats3.min = Mathf.Min(this.OverrideMap[cell], normalizationStats3.min);
                normalizationStats3.max = Mathf.Max(this.OverrideMap[cell], normalizationStats3.max);
                normalizationStats4.min = Mathf.Min(this.DensityMap[cell], normalizationStats4.min);
                normalizationStats4.max = Mathf.Max(this.DensityMap[cell], normalizationStats4.max);
                if (normalizationStats1 != null)
                {
                  normalizationStats1.min = Mathf.Min(this.BackwallMap[cell], normalizationStats1.min);
                  normalizationStats1.max = Mathf.Max(this.BackwallMap[cell], normalizationStats1.max);
                }
              }
              ++point.y;
            }
            else
              break;
          }
          ++point.x;
        }
        else
          goto label_43;
      }
label_43:;
    }
    foreach (KeyValuePair<HashedString, WorldGen.NoiseNormalizationStats> keyValuePair in dictionary)
    {
      float num = keyValuePair.Value.max - keyValuePair.Value.min;
      foreach (int cell in keyValuePair.Value.cells)
        keyValuePair.Value.noise[cell] = (keyValuePair.Value.noise[cell] - keyValuePair.Value.min) / num;
    }
    foreach (TerrainCell overworldCell in this.OverworldCells)
    {
      if (overworldCell.spawnBackwall)
      {
        overworldCell.poly.Centroid();
        foreach (TerrainCell terrainCell in this.data.terrainCells)
        {
          if (overworldCell.poly.PointInPolygon(terrainCell.poly.Centroid()))
            terrainCell.spawnBackwall = true;
        }
      }
    }
  }

  private float GetValue(Chunk chunk, Vector2I pos)
  {
    int index = pos.x + this.data.world.size.x * pos.y;
    if (index < 0 || index >= chunk.data.Length)
      throw new ArgumentOutOfRangeException($"chunkDataIndex [{index.ToString()}]", $"chunk data length [{chunk.data.Length.ToString()}]");
    return chunk.data[index];
  }

  public bool InChunkRange(Chunk chunk, Vector2I pos)
  {
    int num = pos.x + this.data.world.size.x * pos.y;
    return num >= 0 && num < chunk.data.Length;
  }

  private TerrainCell.ElementOverride GetElementFromBiomeElementTable(
    Chunk chunk,
    Vector2I pos,
    List<ElementGradient> table,
    float erode)
  {
    return WorldGen.GetElementFromBiomeElementTable(this.GetValue(chunk, pos) * erode, table);
  }

  private TerrainCell.ElementOverride GetElementFromBiomeBackwallElementTable(
    Chunk chunk,
    Vector2I pos,
    List<ElementGradient> table)
  {
    int index = pos.x + this.data.world.size.x * pos.y;
    return WorldGen.GetElementFromBiomeElementTable(chunk.backwallNoise[index], table);
  }

  public static TerrainCell.ElementOverride GetElementFromBiomeElementTable(
    float value,
    List<ElementGradient> table)
  {
    TerrainCell.ElementOverride elementOverride = TerrainCell.GetElementOverride(WorldGen.voidElement.tag.ToString(), (SampleDescriber.Override) null);
    if (table.Count == 0)
      return elementOverride;
    for (int index = 0; index < table.Count; ++index)
    {
      Debug.Assert(table[index].content != null, (object) index.ToString());
      if ((double) value < (double) table[index].maxValue)
        return TerrainCell.GetElementOverride(table[index].content, table[index].overrides);
    }
    return TerrainCell.GetElementOverride(table[table.Count - 1].content, table[table.Count - 1].overrides);
  }

  public static bool CanLoad(string fileName)
  {
    if (fileName != null)
    {
      if (!(fileName == ""))
      {
        try
        {
          if (!File.Exists(fileName))
            return false;
          using (BinaryReader binaryReader = new BinaryReader((Stream) File.Open(fileName, FileMode.Open)))
            return binaryReader.BaseStream.CanRead;
        }
        catch (FileNotFoundException ex)
        {
          return false;
        }
        catch (Exception ex)
        {
          DebugUtil.LogWarningArgs((object) $"Failed to read {fileName}\n{ex.ToString()}");
          return false;
        }
      }
    }
    return false;
  }

  public static WorldGen Load(IReader reader, bool defaultDiscovered)
  {
    try
    {
      WorldGenSave worldGenSave = new WorldGenSave();
      Deserializer.Deserialize((object) worldGenSave, reader);
      WorldGen worldGen = new WorldGen(worldGenSave.worldID, worldGenSave.data, worldGenSave.traitIDs, worldGenSave.storyTraitIDs, false);
      worldGen.isStartingWorld = true;
      if (worldGenSave.version.x != 1 || worldGenSave.version.y > 1)
      {
        DebugUtil.LogErrorArgs((object) $"LoadWorldGenSim Error! Wrong save version Current: [{1.ToString()}.{1.ToString()}] File: [{worldGenSave.version.x.ToString()}.{worldGenSave.version.y.ToString()}]");
        worldGen.wasLoaded = false;
      }
      else
        worldGen.wasLoaded = true;
      return worldGen;
    }
    catch (Exception ex)
    {
      DebugUtil.LogErrorArgs((object) "WorldGen.Load Error!\n", (object) ex.Message, (object) ex.StackTrace);
      return (WorldGen) null;
    }
  }

  public void DrawDebug()
  {
  }

  public delegate bool OfflineCallbackFunction(
    StringKey stringKeyRoot,
    float completePercent,
    WorldGenProgressStages.Stages stage);

  public enum GenerateSection
  {
    SolarSystem,
    WorldNoise,
    WorldLayout,
    RenderToMap,
    CollectSpawners,
  }

  private class ProcessByTerrainCellSetter : TerrainCell.ISimDataSetter
  {
    public WorldgenSimData data;
    public HashSet<int> blacklistCells;

    public void SetSimBackwall(int cellIdx, Element element, Sim.PhysicsData pd)
    {
      throw new NotImplementedException();
    }

    public void SetSimCell(int index, Element elem, Sim.PhysicsData pd, Sim.DiseaseCell dc)
    {
      if (Grid.IsValidCell(index))
      {
        if (this.blacklistCells.Contains(index))
          return;
        if (elem.HasTag(GameTags.Special))
          pd = elem.defaultValues;
        this.data.cells[index].SetValues(elem, pd, ElementLoader.elements);
        this.data.diseaseCells[index] = dc;
      }
      else
        Debug.LogError((object) $"Process::SetValuesFunction Index [{index.ToString()}] is not valid. cells.Length [{this.data.cells.Length.ToString()}]");
    }
  }

  private class NoiseNormalizationStats
  {
    public float[] noise;
    public float min = float.MaxValue;
    public float max = float.MinValue;
    public HashSet<int> cells = new HashSet<int>();

    public NoiseNormalizationStats(float[] noise) => this.noise = noise;
  }
}
