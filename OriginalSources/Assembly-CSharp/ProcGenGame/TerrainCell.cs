// Decompiled with JetBrains decompiler
// Type: ProcGenGame.TerrainCell
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Delaunay.Geo;
using KSerialization;
using ProcGen;
using System;
using System.Collections.Generic;
using UnityEngine;
using VoronoiTree;

#nullable disable
namespace ProcGenGame;

[SerializationConfig(MemberSerialization.OptIn)]
public class TerrainCell
{
  private const float MASS_VARIATION = 0.2f;
  public List<KeyValuePair<int, Tag>> terrainPositions;
  public List<KeyValuePair<int, Tag>> poi;
  public List<int> neighbourTerrainCells = new List<int>();
  public byte biomeIdx = byte.MaxValue;
  public bool spawnBackwall;
  private float finalSize;
  private bool debugMode;
  private List<int> allCells;
  private HashSet<int> availableTerrainPoints;
  private HashSet<int> featureSpawnPoints;
  private HashSet<int> availableSpawnPoints;
  public const int DONT_SET_TEMPERATURE_DEFAULTS = -1;
  private static readonly Tag[] noPOISpawnTags = new Tag[5]
  {
    WorldGenTags.StartLocation,
    WorldGenTags.AtStart,
    WorldGenTags.NearStartLocation,
    WorldGenTags.POI,
    WorldGenTags.Feature
  };
  private static readonly TagSet noPOISpawnTagSet = new TagSet(TerrainCell.noPOISpawnTags);
  private static readonly Tag[] relaxedNoPOISpawnTags = new Tag[4]
  {
    WorldGenTags.StartLocation,
    WorldGenTags.AtStart,
    WorldGenTags.NearStartLocation,
    WorldGenTags.POI
  };
  private static readonly TagSet relaxedNoPOISpawnTagSet = new TagSet(TerrainCell.relaxedNoPOISpawnTags);
  private static readonly Tag[] noPOISpawnTagsNearStart = new Tag[2]
  {
    WorldGenTags.POI,
    WorldGenTags.Feature
  };
  private static readonly TagSet noPOISpawnNearStartTagSet = new TagSet(TerrainCell.noPOISpawnTagsNearStart);
  private static readonly Tag[] relaxedNoPOISpawnTagsAllowNearStart = new Tag[1]
  {
    WorldGenTags.POI
  };
  private static readonly TagSet relaxedNoPOISpawnAllowNearStartTagSet = new TagSet(TerrainCell.relaxedNoPOISpawnTagsAllowNearStart);

  public Polygon poly => this.site.poly;

  public ProcGen.Map.Cell node { get; private set; }

  public Diagram.Site site { get; private set; }

  public Dictionary<Tag, int> distancesToTags { get; private set; }

  public bool HasMobs => this.mobs != null && this.mobs.Count > 0;

  public List<KeyValuePair<int, Tag>> mobs { get; private set; }

  protected TerrainCell()
  {
  }

  protected TerrainCell(ProcGen.Map.Cell node, Diagram.Site site, Dictionary<Tag, int> distancesToTags)
  {
    this.node = node;
    this.site = site;
    this.distancesToTags = distancesToTags;
  }

  public virtual void LogInfo(string evt, string param, float value)
  {
    Debug.Log((object) $"{evt}:{param}={value.ToString()}");
  }

  public void InitializeCells(HashSet<int> claimedCells)
  {
    if (this.allCells != null)
      return;
    this.allCells = new List<int>();
    this.availableTerrainPoints = new HashSet<int>();
    this.availableSpawnPoints = new HashSet<int>();
    int y1 = (int) this.poly.bounds.y;
    while (true)
    {
      double num1 = (double) y1;
      UnityEngine.Rect bounds = this.poly.bounds;
      double y2 = (double) bounds.y;
      bounds = this.poly.bounds;
      double height = (double) bounds.height;
      double num2 = y2 + height;
      if (num1 < num2)
      {
        bounds = this.poly.bounds;
        int x1 = (int) bounds.x;
        while (true)
        {
          double num3 = (double) x1;
          bounds = this.poly.bounds;
          double x2 = (double) bounds.x;
          bounds = this.poly.bounds;
          double width = (double) bounds.width;
          double num4 = x2 + width;
          if (num3 < num4)
          {
            if (this.poly.Contains(new Vector2((float) x1, (float) y1)))
            {
              int cell = Grid.XYToCell(x1, y1);
              this.availableTerrainPoints.Add(cell);
              this.availableSpawnPoints.Add(cell);
              if (claimedCells.Add(cell))
                this.allCells.Add(cell);
            }
            ++x1;
          }
          else
            break;
        }
        ++y1;
      }
      else
        break;
    }
    this.LogInfo("Initialise cells", "", (float) this.allCells.Count);
  }

  public List<int> GetAllCells() => new List<int>((IEnumerable<int>) this.allCells);

  public List<int> GetAvailableSpawnCellsAll()
  {
    List<int> availableSpawnCellsAll = new List<int>();
    foreach (int availableSpawnPoint in this.availableSpawnPoints)
      availableSpawnCellsAll.Add(availableSpawnPoint);
    return availableSpawnCellsAll;
  }

  public List<int> GetAvailableSpawnCellsFeature()
  {
    List<int> spawnCellsFeature = new List<int>();
    HashSet<int> intSet = new HashSet<int>((IEnumerable<int>) this.availableSpawnPoints);
    intSet.ExceptWith((IEnumerable<int>) this.availableTerrainPoints);
    foreach (int num in intSet)
      spawnCellsFeature.Add(num);
    return spawnCellsFeature;
  }

  public List<int> GetAvailableSpawnCellsBiome()
  {
    List<int> availableSpawnCellsBiome = new List<int>();
    HashSet<int> intSet = new HashSet<int>((IEnumerable<int>) this.availableSpawnPoints);
    intSet.ExceptWith((IEnumerable<int>) this.featureSpawnPoints);
    foreach (int num in intSet)
      availableSpawnCellsBiome.Add(num);
    return availableSpawnCellsBiome;
  }

  private bool RemoveFromAvailableSpawnCells(int cell) => this.availableSpawnPoints.Remove(cell);

  public void AddMobs(IEnumerable<KeyValuePair<int, Tag>> newMobs)
  {
    foreach (KeyValuePair<int, Tag> newMob in newMobs)
      this.AddMob(newMob);
  }

  private void AddMob(int cellIdx, string tag)
  {
    this.AddMob(new KeyValuePair<int, Tag>(cellIdx, new Tag(tag)));
  }

  public void AddMob(KeyValuePair<int, Tag> mob)
  {
    if (this.mobs == null)
      this.mobs = new List<KeyValuePair<int, Tag>>();
    this.mobs.Add(mob);
    bool flag = this.RemoveFromAvailableSpawnCells(mob.Key);
    Tag tag = mob.Value;
    this.LogInfo("\t\t\tRemoveFromAvailableCells", $"{tag.Name}: {(flag ? "success" : "failed")}", (float) mob.Key);
    if (flag)
      return;
    if (!this.allCells.Contains(mob.Key))
    {
      string[] strArray = new string[5]
      {
        "Couldnt find cell [",
        mob.Key.ToString(),
        "] we dont own, to remove for mob [",
        null,
        null
      };
      tag = mob.Value;
      strArray[3] = tag.Name;
      strArray[4] = "]";
      Debug.Assert(false, (object) string.Concat(strArray));
    }
    else
    {
      string[] strArray = new string[5]
      {
        "Couldnt find cell [",
        mob.Key.ToString(),
        "] to remove for mob [",
        null,
        null
      };
      tag = mob.Value;
      strArray[3] = tag.Name;
      strArray[4] = "]";
      Debug.Assert(false, (object) string.Concat(strArray));
    }
  }

  protected string GetSubWorldType(WorldGen worldGen)
  {
    Vector2I pos = new Vector2I((int) this.site.poly.Centroid().x, (int) this.site.poly.Centroid().y);
    return worldGen.GetSubWorldType(pos);
  }

  protected ProcGen.Temperature.Range GetTemperatureRange(WorldGen worldGen)
  {
    string subWorldType = this.GetSubWorldType(worldGen);
    return subWorldType == null || !worldGen.Settings.HasSubworld(subWorldType) ? ProcGen.Temperature.Range.Mild : worldGen.Settings.GetSubWorld(subWorldType).temperatureRange;
  }

  protected void GetTemperatureRange(WorldGen worldGen, ref float min, ref float range)
  {
    ProcGen.Temperature.Range temperatureRange = this.GetTemperatureRange(worldGen);
    min = SettingsCache.temperatures[temperatureRange].min;
    range = SettingsCache.temperatures[temperatureRange].max - min;
  }

  protected float GetDensityMassForCell(Chunk world, int cellIdx, float mass)
  {
    if (!Grid.IsValidCell(cellIdx))
      return 0.0f;
    Debug.Assert((double) world.density[cellIdx] >= 0.0 && (double) world.density[cellIdx] <= 1.0, (object) $"Density [{world.density[cellIdx].ToString()}] out of range [0-1]");
    float num = (float) (0.20000000298023224 * ((double) world.density[cellIdx] - 0.5));
    float densityMassForCell = mass + mass * num;
    if ((double) densityMassForCell > 10000.0)
      densityMassForCell = 10000f;
    return densityMassForCell;
  }

  private void HandleSprinkleOfElement(
    WorldGenSettings settings,
    Tag targetTag,
    Chunk world,
    TerrainCell.ISimDataSetter simDataSetter,
    float temperatureMin,
    float temperatureRange,
    SeededRandom rnd)
  {
    Element elementByName = ElementLoader.FindElementByName(settings.GetFeature(targetTag.Name).GetOneWeightedSimHash("SprinkleOfElementChoices", rnd).element);
    ProcGen.Room room = (ProcGen.Room) null;
    SettingsCache.rooms.TryGetValue(targetTag.Name, out room);
    SampleDescriber sampleDescriber = (SampleDescriber) room;
    Sim.PhysicsData defaultValues = elementByName.defaultValues;
    Sim.DiseaseCell invalid = Sim.DiseaseCell.Invalid;
    for (int index1 = 0; index1 < this.terrainPositions.Count; ++index1)
    {
      if (!(this.terrainPositions[index1].Value != targetTag))
      {
        SeededRandom seededRandom = rnd;
        ProcGen.MinMax blobSize = sampleDescriber.blobSize;
        double min = (double) blobSize.min;
        blobSize = sampleDescriber.blobSize;
        double max = (double) blobSize.max;
        float radius = seededRandom.RandomRange((float) min, (float) max);
        List<Vector2I> filledCircle = ProcGen.Util.GetFilledCircle((Vector2) Grid.CellToPos2D(this.terrainPositions[index1].Key), radius);
        for (int index2 = 0; index2 < filledCircle.Count; ++index2)
        {
          int cell = Grid.XYToCell(filledCircle[index2].x, filledCircle[index2].y);
          if (Grid.IsValidCell(cell))
          {
            defaultValues.mass = this.GetDensityMassForCell(world, cell, elementByName.defaultValues.mass);
            defaultValues.temperature = temperatureMin + world.heatOffset[cell] * temperatureRange;
            simDataSetter.SetSimCell(cell, elementByName, defaultValues, invalid);
          }
        }
      }
    }
  }

  private HashSet<Vector2I> DigFeature(
    ProcGen.Room.Shape shape,
    float size,
    List<int> bordersWidths,
    SeededRandom rnd,
    out List<Vector2I> featureCenterPoints,
    out List<List<Vector2I>> featureBorders)
  {
    HashSet<Vector2I> sourcePoints = new HashSet<Vector2I>();
    featureCenterPoints = new List<Vector2I>();
    featureBorders = new List<List<Vector2I>>();
    if ((double) size < 1.0)
      return sourcePoints;
    Vector2 center = this.site.poly.Centroid();
    this.finalSize = size;
    switch (shape)
    {
      case ProcGen.Room.Shape.Circle:
        featureCenterPoints = ProcGen.Util.GetFilledCircle(center, this.finalSize);
        break;
      case ProcGen.Room.Shape.Blob:
        featureCenterPoints = ProcGen.Util.GetBlob(center, this.finalSize, rnd.RandomSource());
        break;
      case ProcGen.Room.Shape.Square:
        featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, this.finalSize, this.finalSize, rnd);
        break;
      case ProcGen.Room.Shape.TallThin:
        featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, this.finalSize / 4f, this.finalSize, rnd);
        break;
      case ProcGen.Room.Shape.ShortWide:
        featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, this.finalSize, this.finalSize / 4f, rnd);
        break;
      case ProcGen.Room.Shape.Splat:
        featureCenterPoints = ProcGen.Util.GetSplat(center, this.finalSize, rnd.RandomSource());
        break;
    }
    sourcePoints.UnionWith((IEnumerable<Vector2I>) featureCenterPoints);
    if (featureCenterPoints.Count != 0 && bordersWidths != null && bordersWidths.Count > 0 && bordersWidths[0] > 0)
    {
      for (int index = 0; index < bordersWidths.Count && bordersWidths[index] > 0; ++index)
      {
        featureBorders.Add(ProcGen.Util.GetBorder(sourcePoints, bordersWidths[index]));
        sourcePoints.UnionWith((IEnumerable<Vector2I>) featureBorders[index]);
      }
    }
    return sourcePoints;
  }

  public static TerrainCell.ElementOverride GetElementOverride(
    string element,
    SampleDescriber.Override overrides)
  {
    Debug.Assert(element != null && element.Length > 0);
    TerrainCell.ElementOverride elementOverride = new TerrainCell.ElementOverride();
    elementOverride.element = ElementLoader.FindElementByName(element);
    Debug.Assert(elementOverride.element != null, (object) ("Couldn't find an element called " + element));
    elementOverride.pdelement = elementOverride.element.defaultValues;
    elementOverride.dc = Sim.DiseaseCell.Invalid;
    elementOverride.mass = elementOverride.element.defaultValues.mass;
    elementOverride.temperature = elementOverride.element.defaultValues.temperature;
    if (overrides == null)
      return elementOverride;
    elementOverride.overrideMass = false;
    elementOverride.overrideTemperature = false;
    elementOverride.overrideDiseaseIdx = false;
    elementOverride.overrideDiseaseAmount = false;
    float? nullable = overrides.massOverride;
    if (nullable.HasValue)
    {
      ref TerrainCell.ElementOverride local = ref elementOverride;
      nullable = overrides.massOverride;
      double num = (double) nullable.Value;
      local.mass = (float) num;
      elementOverride.overrideMass = true;
    }
    nullable = overrides.massMultiplier;
    if (nullable.HasValue)
    {
      ref float local = ref elementOverride.mass;
      double num1 = (double) local;
      nullable = overrides.massMultiplier;
      double num2 = (double) nullable.Value;
      local = (float) (num1 * num2);
      elementOverride.overrideMass = true;
    }
    nullable = overrides.temperatureOverride;
    if (nullable.HasValue)
    {
      ref TerrainCell.ElementOverride local = ref elementOverride;
      nullable = overrides.temperatureOverride;
      double num = (double) nullable.Value;
      local.temperature = (float) num;
      elementOverride.overrideTemperature = true;
    }
    nullable = overrides.temperatureMultiplier;
    if (nullable.HasValue)
    {
      ref float local = ref elementOverride.temperature;
      double num3 = (double) local;
      nullable = overrides.temperatureMultiplier;
      double num4 = (double) nullable.Value;
      local = (float) (num3 * num4);
      elementOverride.overrideTemperature = true;
    }
    if (overrides.diseaseOverride != null)
    {
      elementOverride.diseaseIdx = WorldGen.diseaseStats.GetIndex((HashedString) overrides.diseaseOverride);
      elementOverride.overrideDiseaseIdx = true;
    }
    int? diseaseAmountOverride = overrides.diseaseAmountOverride;
    if (diseaseAmountOverride.HasValue)
    {
      ref TerrainCell.ElementOverride local = ref elementOverride;
      diseaseAmountOverride = overrides.diseaseAmountOverride;
      int num = diseaseAmountOverride.Value;
      local.diseaseAmount = num;
      elementOverride.overrideDiseaseAmount = true;
    }
    if (elementOverride.overrideTemperature)
      elementOverride.pdelement.temperature = elementOverride.temperature;
    if (elementOverride.overrideMass)
      elementOverride.pdelement.mass = elementOverride.mass;
    if (elementOverride.overrideDiseaseIdx)
      elementOverride.dc.diseaseIdx = elementOverride.diseaseIdx;
    if (elementOverride.overrideDiseaseAmount)
      elementOverride.dc.elementCount = elementOverride.diseaseAmount;
    return elementOverride;
  }

  private bool IsFeaturePointContainedInBorder(Vector2 point, WorldGen worldGen)
  {
    if (!this.node.tags.Contains(WorldGenTags.AllowExceedNodeBorders) || this.poly.Contains(point))
      return true;
    TerrainCell terrainCell = worldGen.TerrainCells.Find((Predicate<TerrainCell>) (x => x.poly.Contains(point)));
    return terrainCell == null || worldGen.Settings.GetSubWorld(this.node.GetSubworld()).zoneType == worldGen.Settings.GetSubWorld(terrainCell.node.GetSubworld()).zoneType;
  }

  private void ApplyPlaceElementForRoom(
    FeatureSettings feature,
    string group,
    List<Vector2I> cells,
    WorldGen worldGen,
    Chunk world,
    TerrainCell.ISimDataSetter simSetter,
    float temperatureMin,
    float temperatureRange,
    SeededRandom rnd,
    HashSet<int> highPriorityClaims)
  {
    if (cells == null || cells.Count == 0 || !feature.HasGroup(group))
      return;
    switch (feature.ElementChoiceGroups[group].selectionMethod)
    {
      case ProcGen.Room.Selection.Weighted:
      case ProcGen.Room.Selection.WeightedResample:
        for (int index = 0; index < cells.Count; ++index)
        {
          int cell = Grid.XYToCell(cells[index].x, cells[index].y);
          if (Grid.IsValidCell(cell) && !highPriorityClaims.Contains(cell) && this.IsFeaturePointContainedInBorder((Vector2) cells[index], worldGen))
          {
            WeightedSimHash oneWeightedSimHash = feature.GetOneWeightedSimHash(group, rnd);
            TerrainCell.ElementOverride elementOverride = TerrainCell.GetElementOverride(oneWeightedSimHash.element, oneWeightedSimHash.overrides);
            if (!elementOverride.overrideTemperature)
              elementOverride.pdelement.temperature = temperatureMin + world.heatOffset[cell] * temperatureRange;
            if (!elementOverride.overrideMass)
              elementOverride.pdelement.mass = this.GetDensityMassForCell(world, cell, elementOverride.mass);
            simSetter.SetSimCell(cell, elementOverride.element, elementOverride.pdelement, elementOverride.dc);
          }
        }
        break;
      case ProcGen.Room.Selection.HorizontalSlice:
        int b1 = int.MaxValue;
        int b2 = int.MinValue;
        for (int index = 0; index < cells.Count; ++index)
        {
          b1 = Mathf.Min(cells[index].y, b1);
          b2 = Mathf.Max(cells[index].y, b2);
        }
        int num = b2 - b1;
        for (int index = 0; index < cells.Count; ++index)
        {
          int cell = Grid.XYToCell(cells[index].x, cells[index].y);
          if (Grid.IsValidCell(cell) && !highPriorityClaims.Contains(cell) && this.IsFeaturePointContainedInBorder((Vector2) cells[index], worldGen))
          {
            float percentage = (float) (1.0 - (double) (cells[index].y - b1) / (double) num);
            WeightedSimHash weightedSimHashAtChoice = feature.GetWeightedSimHashAtChoice(group, percentage);
            TerrainCell.ElementOverride elementOverride = TerrainCell.GetElementOverride(weightedSimHashAtChoice.element, weightedSimHashAtChoice.overrides);
            if (!elementOverride.overrideTemperature)
              elementOverride.pdelement.temperature = temperatureMin + world.heatOffset[cell] * temperatureRange;
            if (!elementOverride.overrideMass)
              elementOverride.pdelement.mass = this.GetDensityMassForCell(world, cell, elementOverride.mass);
            simSetter.SetSimCell(cell, elementOverride.element, elementOverride.pdelement, elementOverride.dc);
          }
        }
        break;
      default:
        WeightedSimHash oneWeightedSimHash1 = feature.GetOneWeightedSimHash(group, rnd);
        for (int index = 0; index < cells.Count; ++index)
        {
          int cell = Grid.XYToCell(cells[index].x, cells[index].y);
          if (Grid.IsValidCell(cell) && !highPriorityClaims.Contains(cell) && this.IsFeaturePointContainedInBorder((Vector2) cells[index], worldGen))
          {
            TerrainCell.ElementOverride elementOverride = TerrainCell.GetElementOverride(oneWeightedSimHash1.element, oneWeightedSimHash1.overrides);
            if (!elementOverride.overrideTemperature)
              elementOverride.pdelement.temperature = temperatureMin + world.heatOffset[cell] * temperatureRange;
            if (!elementOverride.overrideMass)
              elementOverride.pdelement.mass = this.GetDensityMassForCell(world, cell, elementOverride.mass);
            simSetter.SetSimCell(cell, elementOverride.element, elementOverride.pdelement, elementOverride.dc);
          }
        }
        break;
    }
  }

  private int GetIndexForLocation(List<Vector2I> points, Mob.Location location, SeededRandom rnd)
  {
    int index1 = -1;
    if (points == null || points.Count == 0)
      return index1;
    if (location == Mob.Location.Air || location == Mob.Location.Solid)
      return rnd.RandomRange(0, points.Count);
    for (int index2 = 0; index2 < points.Count; ++index2)
    {
      if (Grid.IsValidCell(Grid.XYToCell(points[index2].x, points[index2].y)))
      {
        if (index1 == -1)
        {
          index1 = index2;
        }
        else
        {
          switch (location)
          {
            case Mob.Location.Floor:
              if (points[index2].y < points[index1].y)
              {
                index1 = index2;
                continue;
              }
              continue;
            case Mob.Location.Ceiling:
              if (points[index2].y > points[index1].y)
              {
                index1 = index2;
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
    }
    return index1;
  }

  private void PlaceMobsInRoom(
    WorldGenSettings settings,
    List<MobReference> mobTags,
    List<Vector2I> points,
    SeededRandom rnd)
  {
    if (points == null)
      return;
    if (this.mobs == null)
      this.mobs = new List<KeyValuePair<int, Tag>>();
    for (int index1 = 0; index1 < mobTags.Count; ++index1)
    {
      if (!settings.HasMob(mobTags[index1].type))
      {
        Debug.LogError((object) $"Missing sample description for tag [{mobTags[index1].type}]");
      }
      else
      {
        Mob mob = settings.GetMob(mobTags[index1].type);
        int num = Mathf.RoundToInt(mobTags[index1].count.GetRandomValueWithinRange(rnd));
        for (int index2 = 0; index2 < num; ++index2)
        {
          int indexForLocation = this.GetIndexForLocation(points, mob.location, rnd);
          if (indexForLocation != -1)
          {
            if (points.Count <= indexForLocation)
              return;
            int cell = Grid.XYToCell(points[indexForLocation].x, points[indexForLocation].y);
            points.RemoveAt(indexForLocation);
            this.AddMob(cell, mobTags[index1].type);
          }
          else
            break;
        }
      }
    }
  }

  private int[] ConvertNoiseToPoints(float[] basenoise, float minThreshold = 0.9f, float maxThreshold = 1f)
  {
    if (basenoise == null)
      return (int[]) null;
    List<int> intList = new List<int>();
    float width = this.site.poly.bounds.width;
    float height = this.site.poly.bounds.height;
    for (float y = this.site.position.y - height / 2f; (double) y < (double) this.site.position.y + (double) height / 2.0; ++y)
    {
      for (float x = this.site.position.x - width / 2f; (double) x < (double) this.site.position.x + (double) width / 2.0; ++x)
      {
        int cell = Grid.PosToCell(new Vector2(x, y));
        if (this.site.poly.Contains(new Vector2(x, y)))
        {
          float num = (float) (int) basenoise[cell];
          if ((double) num >= (double) minThreshold && (double) num <= (double) maxThreshold && !intList.Contains(cell))
            intList.Add(Grid.PosToCell(new Vector2(x, y)));
        }
      }
    }
    return intList.ToArray();
  }

  private void ApplyForeground(
    WorldGen worldGen,
    Chunk world,
    TerrainCell.ISimDataSetter simDataSetter,
    float temperatureMin,
    float temperatureRange,
    SeededRandom rnd)
  {
    bool flag = this.node.tags != null;
    this.LogInfo("Apply foregreound", flag.ToString(), (float) (this.node.tags != null ? (double) this.node.tags.Count : 0.0));
    if (this.node.tags == null)
      return;
    FeatureSettings feature = worldGen.Settings.TryGetFeature(this.node.type);
    flag = feature != null;
    this.LogInfo("\tFeature?", flag.ToString(), 0.0f);
    if (feature == null && this.node.tags != null)
    {
      List<Tag> tagList = new List<Tag>();
      foreach (Tag tag in this.node.tags)
      {
        if (worldGen.Settings.HasFeature(tag.Name))
          tagList.Add(tag);
      }
      this.LogInfo("\tNo feature, checking possible feature tags, found", "", (float) tagList.Count);
      if (tagList.Count > 0)
      {
        Tag tag = tagList[rnd.RandomSource().Next(tagList.Count)];
        feature = worldGen.Settings.GetFeature(tag.Name);
        this.LogInfo("\tPicked feature", tag.Name, 0.0f);
      }
    }
    if (feature == null)
      return;
    this.LogInfo("APPLY FOREGROUND", this.node.type, 0.0f);
    float size = feature.blobSize.GetRandomValueWithinRange(rnd);
    float closestEdge = this.poly.DistanceToClosestEdge();
    if (!this.node.tags.Contains(WorldGenTags.AllowExceedNodeBorders) && (double) closestEdge < (double) size)
    {
      if (this.debugMode)
        Debug.LogWarning((object) $"{this.node.type} {feature.shape.ToString()}  blob size too large to fit in node. Size reduced. {size.ToString()}->{(closestEdge - 6f).ToString()}");
      size = closestEdge - 6f;
    }
    if ((double) size <= 0.0)
      return;
    List<Vector2I> featureCenterPoints;
    List<List<Vector2I>> featureBorders;
    HashSet<Vector2I> vector2ISet = this.DigFeature(feature.shape, size, feature.borders, rnd, out featureCenterPoints, out featureBorders);
    this.featureSpawnPoints = new HashSet<int>();
    foreach (Vector2I vector2I in vector2ISet)
      this.featureSpawnPoints.Add(Grid.XYToCell(vector2I.x, vector2I.y));
    this.LogInfo("\t\t", "claimed points", (float) this.featureSpawnPoints.Count);
    this.availableTerrainPoints.ExceptWith((IEnumerable<int>) this.featureSpawnPoints);
    this.ApplyPlaceElementForRoom(feature, "RoomCenterElements", featureCenterPoints, worldGen, world, simDataSetter, temperatureMin, temperatureRange, rnd, worldGen.HighPriorityClaimedCells);
    if (featureBorders != null)
    {
      for (int index = 0; index < featureBorders.Count; ++index)
        this.ApplyPlaceElementForRoom(feature, "RoomBorderChoices" + index.ToString(), featureBorders[index], worldGen, world, simDataSetter, temperatureMin, temperatureRange, rnd, worldGen.HighPriorityClaimedCells);
    }
    if (!feature.tags.Contains(WorldGenTags.HighPriorityFeature.Name))
      return;
    this.featureSpawnPoints.RemoveWhere((Predicate<int>) (cellIdx =>
    {
      if (!Grid.IsValidCell(cellIdx))
        return true;
      Vector2I xy = Grid.CellToXY(cellIdx);
      return !this.IsFeaturePointContainedInBorder(new Vector2((float) xy.x, (float) xy.y), worldGen);
    }));
    worldGen.AddHighPriorityCells(this.featureSpawnPoints);
  }

  private void ApplyBackground(
    WorldGen worldGen,
    Chunk world,
    TerrainCell.ISimDataSetter simDataSetter,
    float temperatureMin,
    float temperatureRange,
    SeededRandom rnd)
  {
    this.LogInfo("Apply Background", this.node.type, 0.0f);
    float floatSetting1 = worldGen.Settings.GetFloatSetting("CaveOverrideMaxValue");
    float floatSetting2 = worldGen.Settings.GetFloatSetting("CaveOverrideSliverValue");
    Leaf leafForTerrainCell = worldGen.GetLeafForTerrainCell(this);
    bool flag1 = leafForTerrainCell.tags.Contains(WorldGenTags.IgnoreCaveOverride);
    bool flag2 = leafForTerrainCell.tags.Contains(WorldGenTags.CaveVoidSliver);
    bool flag3 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToCentroid);
    bool flag4 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToCentroidInv);
    bool flag5 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToEdge);
    bool flag6 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToEdgeInv);
    bool flag7 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToBorder);
    bool flag8 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToBorderWeak);
    bool flag9 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToBorderInv);
    bool flag10 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToWorldTop);
    bool flag11 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToWorldTopOrSide);
    bool flag12 = leafForTerrainCell.tags.Contains(WorldGenTags.DistFunctionPointCentroid);
    bool flag13 = leafForTerrainCell.tags.Contains(WorldGenTags.DistFunctionPointEdge);
    this.LogInfo("Getting Element Bands", this.node.type, 0.0f);
    ElementBandConfiguration elementBandForBiome1 = worldGen.Settings.GetElementBandForBiome(this.node.type);
    if (elementBandForBiome1 == null && this.node.biomeSpecificTags != null)
    {
      this.LogInfo("\tType is not a biome, checking tags", "", (float) this.node.tags.Count);
      List<ElementBandConfiguration> bandConfigurationList = new List<ElementBandConfiguration>();
      foreach (Tag biomeSpecificTag in this.node.biomeSpecificTags)
      {
        ElementBandConfiguration elementBandForBiome2 = worldGen.Settings.GetElementBandForBiome(biomeSpecificTag.Name);
        if (elementBandForBiome2 != null)
        {
          bandConfigurationList.Add(elementBandForBiome2);
          this.LogInfo("\tFound biome", biomeSpecificTag.Name, 0.0f);
        }
      }
      if (bandConfigurationList.Count > 0)
      {
        int index = rnd.RandomSource().Next(bandConfigurationList.Count);
        elementBandForBiome1 = bandConfigurationList[index];
        this.LogInfo("\tPicked biome", "", (float) index);
      }
    }
    ElementBandConfiguration elementBands = worldGen.Settings.GetElementBandForBiome(this.node.GetBiome() + "_backwall") ?? elementBandForBiome1;
    DebugUtil.Assert(elementBandForBiome1 != null, "A node didn't get assigned a biome! ", this.node.type);
    foreach (int availableTerrainPoint in this.availableTerrainPoints)
    {
      Vector2I xy = Grid.CellToXY(availableTerrainPoint);
      if (!worldGen.HighPriorityClaimedCells.Contains(availableTerrainPoint))
      {
        float num1 = world.overrides[availableTerrainPoint];
        if (!flag1 && (double) num1 >= 100.0)
        {
          if ((double) num1 >= 300.0)
            simDataSetter.SetSimCell(availableTerrainPoint, WorldGen.voidElement, WorldGen.voidElement.defaultValues, Sim.DiseaseCell.Invalid);
          else if ((double) num1 >= 200.0)
            simDataSetter.SetSimCell(availableTerrainPoint, WorldGen.unobtaniumElement, WorldGen.unobtaniumElement.defaultValues, Sim.DiseaseCell.Invalid);
          else
            simDataSetter.SetSimCell(availableTerrainPoint, WorldGen.katairiteElement, WorldGen.katairiteElement.defaultValues, Sim.DiseaseCell.Invalid);
        }
        else
        {
          float erode = 1f;
          Vector2 vector2 = new Vector2((float) xy.x, (float) xy.y);
          if (flag3 | flag4)
          {
            float num2 = 15f;
            if (flag13)
            {
              float timeOnEdge = 0.0f;
              MathUtil.Pair<Vector2, Vector2> closestEdge = this.poly.GetClosestEdge(vector2, ref timeOnEdge);
              num2 = Vector2.Distance(closestEdge.First + (closestEdge.Second - closestEdge.First) * timeOnEdge, vector2);
            }
            erode = Mathf.Max(0.0f, Mathf.Min(1f, Vector2.Distance(this.poly.Centroid(), vector2) / num2));
            if (flag4)
              erode = 1f - erode;
          }
          if (flag6 | flag5)
          {
            float timeOnEdge = 0.0f;
            MathUtil.Pair<Vector2, Vector2> closestEdge = this.poly.GetClosestEdge(vector2, ref timeOnEdge);
            Vector2 a = closestEdge.First + (closestEdge.Second - closestEdge.First) * timeOnEdge;
            float num3 = 15f;
            if (flag12)
              num3 = Vector2.Distance(this.poly.Centroid(), vector2);
            Vector2 b = vector2;
            erode = Mathf.Max(0.0f, Mathf.Min(1f, Vector2.Distance(a, b) / num3));
            if (flag6)
              erode = 1f - erode;
          }
          if (flag9 | flag7)
          {
            List<ProcGen.Map.Edge> edgesWithTag = worldGen.WorldLayout.overworldGraph.GetEdgesWithTag(WorldGenTags.EdgeClosed);
            float b = float.MaxValue;
            foreach (ProcGen.Map.Edge edge in edgesWithTag)
            {
              MathUtil.Pair<Vector2, Vector2> segment = new MathUtil.Pair<Vector2, Vector2>(edge.corner0.position, edge.corner1.position);
              float num4 = 0.0f;
              Vector2 point = vector2;
              ref float local = ref num4;
              b = Mathf.Min(Mathf.Abs(MathUtil.GetClosestPointBetweenPointAndLineSegment(segment, point, ref local)), b);
            }
            float num5 = flag8 ? 7f : 20f;
            if (flag12)
              num5 = Vector2.Distance(this.poly.Centroid(), vector2);
            erode = Mathf.Max(0.0f, Mathf.Min(1f, b / num5));
            if (flag9)
              erode = 1f - erode;
          }
          if (flag10)
          {
            int y = worldGen.WorldSize.y;
            float num6 = 38f;
            float num7 = 58f;
            float num8 = (float) y - vector2.y;
            erode = (double) num8 >= (double) num6 ? ((double) num8 >= (double) num7 ? 1f : Mathf.Clamp01((float) (((double) num8 - (double) num6) / ((double) num7 - (double) num6)))) : 0.0f;
          }
          if (flag11)
          {
            int y = worldGen.WorldSize.y;
            int x = worldGen.WorldSize.x;
            float num9 = 2f;
            float num10 = 10f;
            float num11 = Mathf.Min((float) y - vector2.y, vector2.x, (float) x - vector2.x);
            erode = (double) num11 >= (double) num9 ? ((double) num11 >= (double) num10 ? 1f : Mathf.Clamp01((float) (((double) num11 - (double) num9) / ((double) num10 - (double) num9)))) : 0.0f;
          }
          Element element;
          Sim.PhysicsData pd;
          Sim.DiseaseCell dc;
          worldGen.GetElementForBiomePoint(world, elementBandForBiome1, xy, out element, out pd, out dc, erode);
          pd.mass += (float) ((double) pd.mass * 0.20000000298023224 * ((double) world.density[xy.x + world.size.x * xy.y] - 0.5));
          if (!element.IsVacuum && element.id != SimHashes.Katairite && element.id != SimHashes.Unobtanium)
          {
            float num12 = temperatureMin;
            if (element.lowTempTransition != null && (double) temperatureMin < (double) element.lowTemp)
              num12 = element.lowTemp;
            pd.temperature = num12 + world.heatOffset[availableTerrainPoint] * temperatureRange;
          }
          if (element.IsSolid && !flag1 && (double) num1 > (double) floatSetting1 && (double) num1 < 100.0)
          {
            element = !flag2 || (double) num1 <= (double) floatSetting2 ? WorldGen.vacuumElement : WorldGen.voidElement;
            pd = element.defaultValues;
          }
          simDataSetter.SetSimCell(availableTerrainPoint, element, pd, dc);
          if (this.spawnBackwall)
          {
            worldGen.GetElementForBackwallBiomePoint(world, elementBands, xy, out element, out pd, out dc);
            pd.mass += (float) ((double) pd.mass * 0.20000000298023224 * ((double) world.density[xy.x + world.size.x * xy.y] - 0.5));
            float num13 = temperatureMin;
            if (element.lowTempTransition != null && (double) temperatureMin < (double) element.lowTemp)
              num13 = element.lowTemp;
            pd.temperature = num13 + world.heatOffset[availableTerrainPoint] * temperatureRange;
            if (element.IsSolid)
              simDataSetter.SetSimBackwall(availableTerrainPoint, element, pd);
          }
        }
      }
    }
    if (this.node.tags.Contains(WorldGenTags.SprinkleOfOxyRock))
      this.HandleSprinkleOfElement(worldGen.Settings, WorldGenTags.SprinkleOfOxyRock, world, simDataSetter, temperatureMin, temperatureRange, rnd);
    if (!this.node.tags.Contains(WorldGenTags.SprinkleOfMetal))
      return;
    this.HandleSprinkleOfElement(worldGen.Settings, WorldGenTags.SprinkleOfMetal, world, simDataSetter, temperatureMin, temperatureRange, rnd);
  }

  private void GenerateActionCells(
    WorldGenSettings settings,
    Tag tag,
    HashSet<int> possiblePoints,
    SeededRandom rnd)
  {
    ProcGen.Room room = (ProcGen.Room) null;
    SettingsCache.rooms.TryGetValue(tag.Name, out room);
    SampleDescriber sampleDescriber = (SampleDescriber) room;
    if (sampleDescriber == null && settings.HasMob(tag.Name))
      sampleDescriber = (SampleDescriber) settings.GetMob(tag.Name);
    if (sampleDescriber == null)
      return;
    HashSet<int> intSet = new HashSet<int>();
    float valueWithinRange = sampleDescriber.density.GetRandomValueWithinRange(rnd);
    List<Vector2> vector2List;
    switch (sampleDescriber.selectMethod)
    {
      case SampleDescriber.PointSelectionMethod.RandomPoints:
        vector2List = PointGenerator.GetRandomPoints(this.poly, valueWithinRange, 0.0f, (List<Vector2>) null, sampleDescriber.sampleBehaviour, true, rnd);
        break;
      default:
        vector2List = new List<Vector2>();
        vector2List.Add(this.node.position);
        break;
    }
    foreach (Vector2 vector2 in vector2List)
    {
      int cell = Grid.XYToCell((int) vector2.x, (int) vector2.y);
      if (possiblePoints.Contains(cell))
        intSet.Add(cell);
    }
    if (room == null || room.mobselection != ProcGen.Room.Selection.None)
      return;
    if (this.terrainPositions == null)
      this.terrainPositions = new List<KeyValuePair<int, Tag>>();
    foreach (int num in intSet)
    {
      if (Grid.IsValidCell(num))
        this.terrainPositions.Add(new KeyValuePair<int, Tag>(num, tag));
    }
  }

  private void DoProcess(
    WorldGen worldGen,
    Chunk world,
    TerrainCell.ISimDataSetter simDataSetter,
    SeededRandom rnd)
  {
    float min = 265f;
    float range = 30f;
    this.InitializeCells(worldGen.ClaimedCells);
    this.GetTemperatureRange(worldGen, ref min, ref range);
    this.ApplyForeground(worldGen, world, simDataSetter, min, range, rnd);
    for (int i = 0; i < this.node.tags.Count; ++i)
      this.GenerateActionCells(worldGen.Settings, this.node.tags[i], this.availableTerrainPoints, rnd);
    this.ApplyBackground(worldGen, world, simDataSetter, min, range, rnd);
  }

  public void Process(
    WorldGen worldGen,
    ref WorldgenSimData simData,
    Chunk world,
    SeededRandom rnd)
  {
    TerrainCell.SimDataSetter simDataSetter = new TerrainCell.SimDataSetter(worldGen, simData);
    this.DoProcess(worldGen, world, (TerrainCell.ISimDataSetter) simDataSetter, rnd);
    simData = simDataSetter.simData;
  }

  public void Process(WorldGen worldGen, Chunk world, SeededRandom rnd)
  {
    this.DoProcess(worldGen, world, (TerrainCell.ISimDataSetter) new TerrainCell.SimUpdater(), rnd);
  }

  public int DistanceToTag(Tag tag)
  {
    int tag1;
    if (!this.distancesToTags.TryGetValue(tag, out tag1))
      DebugUtil.DevLogError($"DistanceToTag could not find tag '{tag}', did forget to include a start template?");
    return tag1;
  }

  public bool IsSafeToSpawnPOI(List<TerrainCell> allCells, bool log = true)
  {
    return this.IsSafeToSpawnPOI(allCells, TerrainCell.noPOISpawnTags, TerrainCell.noPOISpawnTagSet, log);
  }

  public bool IsSafeToSpawnPOIRelaxed(List<TerrainCell> allCells, bool log = true)
  {
    return this.IsSafeToSpawnPOI(allCells, TerrainCell.relaxedNoPOISpawnTags, TerrainCell.relaxedNoPOISpawnTagSet, log);
  }

  public bool IsSafeToSpawnPOINearStart(List<TerrainCell> allCells, bool log = true)
  {
    return this.IsSafeToSpawnPOI(allCells, TerrainCell.noPOISpawnTagsNearStart, TerrainCell.noPOISpawnNearStartTagSet, log);
  }

  public bool IsSafeToSpawnPOINearStartRelaxed(List<TerrainCell> allCells, bool log = true)
  {
    return this.IsSafeToSpawnPOI(allCells, TerrainCell.relaxedNoPOISpawnTagsAllowNearStart, TerrainCell.relaxedNoPOISpawnAllowNearStartTagSet, log);
  }

  private bool IsSafeToSpawnPOI(
    List<TerrainCell> allCells,
    Tag[] noSpawnTags,
    TagSet noSpawnTagSet,
    bool log)
  {
    return !this.node.tags.ContainsOne(noSpawnTagSet);
  }

  public interface ISimDataSetter
  {
    void SetSimCell(int cellIdx, Element element, Sim.PhysicsData pd, Sim.DiseaseCell dc);

    void SetSimBackwall(int cellIdx, Element element, Sim.PhysicsData pd);
  }

  public struct ElementOverride
  {
    public Element element;
    public Sim.PhysicsData pdelement;
    public Sim.DiseaseCell dc;
    public float mass;
    public float temperature;
    public byte diseaseIdx;
    public int diseaseAmount;
    public bool overrideMass;
    public bool overrideTemperature;
    public bool overrideDiseaseIdx;
    public bool overrideDiseaseAmount;
  }

  private class SimDataSetter : TerrainCell.ISimDataSetter
  {
    public WorldGen worldGen;
    public WorldgenSimData simData;

    public SimDataSetter(WorldGen wg, WorldgenSimData sd)
    {
      this.worldGen = wg;
      this.simData = sd;
    }

    public void SetSimBackwall(int cellIdx, Element element, Sim.PhysicsData pd)
    {
      this.simData.backwallCells[cellIdx].SetValues(element.idx, pd.mass, pd.temperature);
    }

    public void SetSimCell(int index, Element elem, Sim.PhysicsData pd, Sim.DiseaseCell dc)
    {
      if (Grid.IsValidCell(index))
      {
        if (this.worldGen.HighPriorityClaimedCells.Contains(index))
          return;
        this.simData.cells[index].SetValues(elem, pd, ElementLoader.elements);
        this.simData.diseaseCells[index] = dc;
      }
      else
        Debug.LogError((object) $"Process::SetValuesFunction Index [{index.ToString()}] is not valid. cells.Length [{this.simData.cells.Length.ToString()}]");
    }
  }

  private class SimUpdater : TerrainCell.ISimDataSetter
  {
    public void SetSimBackwall(int cellIdx, Element element, Sim.PhysicsData pd)
    {
      throw new NotImplementedException();
    }

    public void SetSimCell(int index, Element elem, Sim.PhysicsData pd, Sim.DiseaseCell dc)
    {
      SimMessages.ModifyCell(index, elem.idx, pd.temperature, pd.mass, dc.diseaseIdx, dc.elementCount, SimMessages.ReplaceType.Replace);
    }
  }
}
