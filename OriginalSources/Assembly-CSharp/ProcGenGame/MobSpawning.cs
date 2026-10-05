// Decompiled with JetBrains decompiler
// Type: ProcGenGame.MobSpawning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

#nullable disable
namespace ProcGenGame;

public static class MobSpawning
{
  public static Dictionary<int, string> PlaceFeatureAmbientMobs(
    WorldGenSettings settings,
    TerrainCell tc,
    SeededRandom rnd,
    ref WorldgenSimData simData,
    HashSet<int> cavityCells,
    HashSet<int> avoidCells,
    bool isDebug,
    ref HashSet<int> alreadyOccupiedCells)
  {
    Dictionary<int, string> spawnedMobs = new Dictionary<int, string>();
    ProcGen.Map.Cell node = tc.node;
    FeatureSettings featureSettings = (FeatureSettings) null;
    foreach (Tag featureSpecificTag in node.featureSpecificTags)
    {
      if (settings.HasFeature(featureSpecificTag.Name))
      {
        featureSettings = settings.GetFeature(featureSpecificTag.Name);
        break;
      }
    }
    if (featureSettings == null || featureSettings.internalMobs == null || featureSettings.internalMobs.Count == 0)
      return spawnedMobs;
    List<int> spawnCellsFeature = tc.GetAvailableSpawnCellsFeature();
    tc.LogInfo(nameof (PlaceFeatureAmbientMobs), "possibleSpawnPoints", (float) spawnCellsFeature.Count);
    for (int index1 = spawnCellsFeature.Count - 1; index1 > 0; --index1)
    {
      int index2 = spawnCellsFeature[index1];
      if (ElementLoader.elements[(int) simData.cells[index2].elementIdx].id == SimHashes.Katairite || ElementLoader.elements[(int) simData.cells[index2].elementIdx].id == SimHashes.Unobtanium || avoidCells.Contains(index2))
        spawnCellsFeature.RemoveAt(index1);
    }
    TerrainCell terrainCell = tc;
    long nodeId = node.NodeId;
    string str = $"Id:{nodeId.ToString()} possible cells";
    double count1 = (double) spawnCellsFeature.Count;
    terrainCell.LogInfo("mob spawns", str, (float) count1);
    if (spawnCellsFeature.Count == 0)
    {
      if (isDebug)
      {
        nodeId = tc.node.NodeId;
        Debug.LogWarning((object) $"No where to put mobs possibleSpawnPoints [{nodeId.ToString()}]");
      }
      return (Dictionary<int, string>) null;
    }
    foreach (MobReference internalMob in featureSettings.internalMobs)
    {
      Mob mob = settings.GetMob(internalMob.type);
      if (mob == null)
      {
        Debug.LogError((object) $"Missing mob description for internal mob [{internalMob.type}]");
      }
      else
      {
        List<int> possibleSpawnPoints = MobSpawning.GetMobPossibleSpawnPoints(mob, spawnCellsFeature, ref simData, tc, cavityCells, alreadyOccupiedCells, rnd);
        if (possibleSpawnPoints.Count == 0)
        {
          if (!isDebug)
            ;
        }
        else
        {
          tc.LogInfo("\t\tpossible", $"{internalMob.type} mps: {possibleSpawnPoints.Count.ToString()} ps:", (float) spawnCellsFeature.Count);
          int count2 = Mathf.RoundToInt(internalMob.count.GetRandomValueWithinRange(rnd));
          tc.LogInfo("\t\tcount", internalMob.type, (float) count2);
          Tag mobPrefab = mob.prefabName == null ? new Tag(internalMob.type) : new Tag(mob.prefabName);
          MobSpawning.SpawnCountMobs(mob, mobPrefab, count2, possibleSpawnPoints, tc, ref spawnedMobs, ref alreadyOccupiedCells);
        }
      }
    }
    return spawnedMobs;
  }

  public static Dictionary<int, string> PlaceBiomeAmbientMobs(
    WorldGenSettings settings,
    TerrainCell tc,
    SeededRandom rnd,
    ref WorldgenSimData simData,
    HashSet<int> cavityCells,
    HashSet<int> avoidCells,
    bool isDebug,
    ref HashSet<int> alreadyOccupiedCells)
  {
    Dictionary<int, string> spawnedMobs = new Dictionary<int, string>();
    ProcGen.Map.Cell node = tc.node;
    List<Tag> list = new List<Tag>();
    if (node.biomeSpecificTags == null)
    {
      tc.LogInfo(nameof (PlaceBiomeAmbientMobs), "No tags", (float) node.NodeId);
      return (Dictionary<int, string>) null;
    }
    foreach (Tag biomeSpecificTag in node.biomeSpecificTags)
    {
      if (settings.HasMob(biomeSpecificTag.Name) && settings.GetMob(biomeSpecificTag.Name) != null)
        list.Add(biomeSpecificTag);
    }
    if (list.Count <= 0)
    {
      tc.LogInfo(nameof (PlaceBiomeAmbientMobs), "No biome MOBS", (float) node.NodeId);
      return (Dictionary<int, string>) null;
    }
    List<int> possibleSpawnPoints1 = node.tags.Contains(WorldGenTags.PreventAmbientMobsInFeature) ? tc.GetAvailableSpawnCellsBiome() : tc.GetAvailableSpawnCellsAll();
    tc.LogInfo("PlaceBiomAmbientMobs", "possibleSpawnPoints", (float) possibleSpawnPoints1.Count);
    for (int index1 = possibleSpawnPoints1.Count - 1; index1 > 0; --index1)
    {
      int index2 = possibleSpawnPoints1[index1];
      if (ElementLoader.elements[(int) simData.cells[index2].elementIdx].id == SimHashes.Katairite || ElementLoader.elements[(int) simData.cells[index2].elementIdx].id == SimHashes.Unobtanium || avoidCells.Contains(index2))
        possibleSpawnPoints1.RemoveAt(index1);
    }
    TerrainCell terrainCell1 = tc;
    long nodeId = node.NodeId;
    string str1 = $"Id:{nodeId.ToString()} possible cells";
    double count1 = (double) possibleSpawnPoints1.Count;
    terrainCell1.LogInfo("mob spawns", str1, (float) count1);
    if (possibleSpawnPoints1.Count == 0)
    {
      if (isDebug)
      {
        nodeId = tc.node.NodeId;
        Debug.LogWarning((object) $"No where to put mobs possibleSpawnPoints [{nodeId.ToString()}]");
      }
      return (Dictionary<int, string>) null;
    }
    list.ShuffleSeeded<Tag>(rnd.RandomSource());
    for (int index = 0; index < list.Count; ++index)
    {
      WorldGenSettings worldGenSettings = settings;
      Tag tag = list[index];
      string name = tag.Name;
      Mob mob = worldGenSettings.GetMob(name);
      if (mob == null)
      {
        tag = list[index];
        Debug.LogError((object) $"Missing sample description for tag [{tag.Name}]");
      }
      else
      {
        List<int> possibleSpawnPoints2 = MobSpawning.GetMobPossibleSpawnPoints(mob, possibleSpawnPoints1, ref simData, tc, cavityCells, alreadyOccupiedCells, rnd);
        if (possibleSpawnPoints2.Count == 0)
        {
          if (!isDebug)
            ;
        }
        else
        {
          TerrainCell terrainCell2 = tc;
          tag = list[index];
          string str2 = $"{tag.ToString()} mps: {possibleSpawnPoints2.Count.ToString()} ps:";
          double count2 = (double) possibleSpawnPoints1.Count;
          terrainCell2.LogInfo("\t\tpossible", str2, (float) count2);
          float num1 = mob.density.GetRandomValueWithinRange(rnd) * MobSettings.AmbientMobDensity;
          if ((double) num1 > 1.0)
          {
            if (isDebug)
            {
              tag = list[index];
              Debug.LogWarning((object) $"Got a mob density greater than 1.0 for {tag.Name}. Probably using density as spacing!");
            }
            num1 = 1f;
          }
          tc.LogInfo("\t\tdensity:", "", num1);
          int count3 = Mathf.RoundToInt((float) possibleSpawnPoints2.Count * num1);
          TerrainCell terrainCell3 = tc;
          tag = list[index];
          string str3 = tag.ToString();
          double num2 = (double) count3;
          terrainCell3.LogInfo("\t\tcount", str3, (float) num2);
          Tag mobPrefab = mob.prefabName == null ? list[index] : new Tag(mob.prefabName);
          MobSpawning.SpawnCountMobs(mob, mobPrefab, count3, possibleSpawnPoints2, tc, ref spawnedMobs, ref alreadyOccupiedCells);
        }
      }
    }
    return spawnedMobs;
  }

  private static List<int> GetMobPossibleSpawnPoints(
    Mob mob,
    List<int> possibleSpawnPoints,
    ref WorldgenSimData simData,
    TerrainCell tc,
    HashSet<int> cavityCells,
    HashSet<int> alreadyOccupiedCells,
    SeededRandom rnd)
  {
    List<int> list = new List<int>();
    foreach (int possibleSpawnPoint in possibleSpawnPoints)
    {
      if (MobSpawning.IsSuitableMobSpawnPoint(possibleSpawnPoint, mob, ref simData, cavityCells, ref alreadyOccupiedCells, tc))
        list.Add(possibleSpawnPoint);
    }
    list.ShuffleSeeded<int>(rnd.RandomSource());
    return list;
  }

  public static void SpawnCountMobs(
    Mob mobData,
    Tag mobPrefab,
    int count,
    List<int> mobPossibleSpawnPoints,
    TerrainCell tc,
    ref Dictionary<int, string> spawnedMobs,
    ref HashSet<int> alreadyOccupiedCells)
  {
    int num1 = 0;
    for (int index1 = 0; index1 - num1 < count && index1 < mobPossibleSpawnPoints.Count; ++index1)
    {
      int possibleSpawnPoint = mobPossibleSpawnPoints[index1];
      int num2 = mobData.location == Mob.Location.Ceiling ? -1 : 1;
      bool flag = false;
      for (int widthIterator = 0; widthIterator < mobData.width + mobData.paddingX * 2; ++widthIterator)
      {
        for (int index2 = 0; index2 < mobData.height; ++index2)
        {
          int num3 = MobSpawning.MobWidthOffset(Grid.OffsetCell(possibleSpawnPoint, 0, index2 * num2), widthIterator);
          if (alreadyOccupiedCells.Contains(num3))
          {
            flag = true;
            break;
          }
        }
        if (flag)
          break;
      }
      if (flag)
      {
        ++num1;
      }
      else
      {
        for (int widthIterator = 0; widthIterator < mobData.width + mobData.paddingX * 2; ++widthIterator)
        {
          for (int index3 = 0; index3 < mobData.height; ++index3)
          {
            int num4 = MobSpawning.MobWidthOffset(Grid.OffsetCell(possibleSpawnPoint, 0, index3 * num2), widthIterator);
            alreadyOccupiedCells.Add(num4);
          }
        }
        tc.AddMob(new KeyValuePair<int, Tag>(possibleSpawnPoint, mobPrefab));
        spawnedMobs.Add(possibleSpawnPoint, mobPrefab.Name);
      }
    }
  }

  public static int MobWidthOffset(int occupiedCell, int widthIterator)
  {
    return Grid.OffsetCell(occupiedCell, widthIterator % 2 == 0 ? -(widthIterator / 2) : widthIterator / 2 + widthIterator % 2, 0);
  }

  private static bool IsSuitableMobSpawnPoint(
    int cell,
    Mob mob,
    ref WorldgenSimData simData,
    HashSet<int> cavityCells,
    ref HashSet<int> alreadyOccupiedCells,
    TerrainCell tc)
  {
    int num1 = mob.location == Mob.Location.Ceiling || mob.location == Mob.Location.LiquidCeiling ? -1 : 1;
    if (!Grid.IsValidCell(cell))
      return false;
    int num2 = mob.width + mob.paddingX * 2;
    CellOffset offset1 = new CellOffset(num2 / 2 - mob.width - mob.paddingX + 1 - 1, num1 < 0 ? 1 : mob.height);
    CellOffset offset2 = offset1 + new CellOffset(mob.width + 1, -(mob.height + 1));
    if (!Grid.IsCellOffsetValid(cell, offset1) || !Grid.IsCellOffsetValid(cell, offset2))
      return false;
    for (int widthIterator = 0; widthIterator < num2; ++widthIterator)
    {
      for (int index = 0; index < mob.height; ++index)
      {
        int cell1 = MobSpawning.MobWidthOffset(Grid.OffsetCell(cell, 0, index * num1), widthIterator);
        if (!Grid.IsValidCell(cell1) || alreadyOccupiedCells.Contains(cell1))
          return false;
      }
    }
    Element element1 = ElementLoader.elements[(int) simData.cells[cell].elementIdx];
    Element element2 = ElementLoader.elements[(int) simData.cells[Grid.CellAbove(cell)].elementIdx];
    switch (mob.location)
    {
      case Mob.Location.Floor:
        bool flag1 = true;
        for (int y = 0; y < mob.height; ++y)
        {
          for (int widthIterator = 0; widthIterator < num2; ++widthIterator)
          {
            int cell2 = MobSpawning.MobWidthOffset(Grid.OffsetCell(cell, 0, y), widthIterator);
            Element element3 = ElementLoader.elements[(int) simData.cells[cell2].elementIdx];
            Element element4 = ElementLoader.elements[(int) simData.cells[Grid.CellAbove(cell2)].elementIdx];
            Element element5 = ElementLoader.elements[(int) simData.cells[Grid.CellBelow(cell2)].elementIdx];
            flag1 = flag1 && cavityCells.Contains(cell2) && !element3.IsSolid && !element3.IsLiquid;
            if (y == 0 && widthIterator < mob.width)
              flag1 = flag1 && element5.IsSolid;
            if (!flag1)
              break;
          }
          if (!flag1)
            break;
        }
        return flag1;
      case Mob.Location.Ceiling:
        Debug.Assert(mob.paddingX == 0, (object) ("Mob paddingX not implemented yet for rule 'Ceiling' and is used for mob " + mob.name));
        bool flag2 = true;
        for (int index = 0; index < mob.height; ++index)
        {
          for (int widthIterator = 0; widthIterator < mob.width; ++widthIterator)
          {
            int cell3 = MobSpawning.MobWidthOffset(Grid.OffsetCell(cell, 0, -index), widthIterator);
            Element element6 = ElementLoader.elements[(int) simData.cells[cell3].elementIdx];
            Element element7 = ElementLoader.elements[(int) simData.cells[Grid.CellAbove(cell3)].elementIdx];
            if (index == 0)
              flag2 = flag2 && element7.IsSolid;
            flag2 = flag2 && cavityCells.Contains(cell3) && !element6.IsSolid && !element6.IsLiquid;
            if (!flag2)
              break;
          }
          if (!flag2)
            break;
        }
        return flag2;
      case Mob.Location.Air:
        Debug.Assert(mob.paddingX == 0, (object) ("Mob paddingX not implemented yet for rule 'Air' and is used for mob " + mob.name));
        return !element1.IsSolid && !element2.IsSolid && !element1.IsLiquid;
      case Mob.Location.Solid:
        Debug.Assert(mob.paddingX == 0, (object) ("Mob paddingX not implemented yet for rule 'Solid' and is used for mob " + mob.name));
        for (int widthIterator = 0; widthIterator < mob.width; ++widthIterator)
        {
          for (int index1 = 0; index1 < mob.height; ++index1)
          {
            int index2 = MobSpawning.MobWidthOffset(Grid.OffsetCell(cell, 0, index1 * num1), widthIterator);
            if (cavityCells.Contains(index2) || !ElementLoader.elements[(int) simData.cells[index2].elementIdx].IsSolid)
              return false;
          }
        }
        return true;
      case Mob.Location.Water:
        DebugUtil.DevLogError("Mob location rule 'Water' is deprecated, use 'Liquid' instead. Mob: " + mob.name);
        goto case Mob.Location.Liquid;
      case Mob.Location.Surface:
        bool flag3 = true;
        Debug.Assert(mob.paddingX == 0, (object) ("Mob paddingX not implemented yet for rule 'Surface' and is used for mob " + mob.name));
        for (int widthIterator = 0; widthIterator < mob.width; ++widthIterator)
        {
          int cell4 = MobSpawning.MobWidthOffset(cell, widthIterator);
          Element element8 = ElementLoader.elements[(int) simData.cells[cell4].elementIdx];
          Element element9 = ElementLoader.elements[(int) simData.cells[Grid.CellBelow(cell4)].elementIdx];
          flag3 = flag3 && element8.id == SimHashes.Vacuum && element9.IsSolid;
        }
        return flag3;
      case Mob.Location.LiquidFloor:
      case Mob.Location.LiquidFloorCavityNoRequired:
        bool flag4 = true;
        Debug.Assert(mob.paddingX == 0, (object) ("Mob paddingX not implemented yet for rule 'LiquidFloor' and is used for mob " + mob.name));
        for (int y = 0; y < mob.height; ++y)
        {
          for (int widthIterator = 0; widthIterator < mob.width; ++widthIterator)
          {
            int cell5 = MobSpawning.MobWidthOffset(Grid.OffsetCell(cell, 0, y), widthIterator);
            Element element10 = ElementLoader.elements[(int) simData.cells[cell5].elementIdx];
            Element element11 = ElementLoader.elements[(int) simData.cells[Grid.CellBelow(cell5)].elementIdx];
            flag4 = flag4 && (mob.location == Mob.Location.LiquidFloorCavityNoRequired || cavityCells.Contains(cell)) && !element10.IsSolid;
            if (y == 0)
              flag4 = flag4 && element10.IsLiquid && element11.IsSolid;
            if (!flag4)
              break;
          }
          if (!flag4)
            break;
        }
        return flag4;
      case Mob.Location.AnyFloor:
        bool flag5 = true;
        Debug.Assert(mob.paddingX == 0, (object) ("Mob paddingX not implemented yet for rule 'AnyFloor' and is used for mob " + mob.name));
        for (int y = 0; y < mob.height; ++y)
        {
          for (int widthIterator = 0; widthIterator < mob.width; ++widthIterator)
          {
            int cell6 = MobSpawning.MobWidthOffset(Grid.OffsetCell(cell, 0, y), widthIterator);
            Element element12 = ElementLoader.elements[(int) simData.cells[cell6].elementIdx];
            Element element13 = ElementLoader.elements[(int) simData.cells[Grid.CellAbove(cell6)].elementIdx];
            Element element14 = ElementLoader.elements[(int) simData.cells[Grid.CellBelow(cell6)].elementIdx];
            flag5 = flag5 && cavityCells.Contains(cell) && !element12.IsSolid;
            if (y == 0)
              flag5 = flag5 && element14.IsSolid;
            if (!flag5)
              break;
          }
          if (!flag5)
            break;
        }
        return flag5;
      case Mob.Location.LiquidCeiling:
        bool flag6 = true;
        Debug.Assert(mob.paddingX == 0, (object) ("Mob paddingX not implemented yet for rule 'LiquidCeiling' and is used for mob " + mob.name));
        for (int index = 0; index < mob.height; ++index)
        {
          for (int widthIterator = 0; widthIterator < mob.width; ++widthIterator)
          {
            int cell7 = MobSpawning.MobWidthOffset(Grid.OffsetCell(cell, 0, -index), widthIterator);
            Element element15 = ElementLoader.elements[(int) simData.cells[cell7].elementIdx];
            Element element16 = ElementLoader.elements[(int) simData.cells[Grid.CellAbove(cell7)].elementIdx];
            if (index == 0)
              flag6 = flag6 && element16.IsSolid;
            flag6 = flag6 && cavityCells.Contains(cell7) && element15.IsLiquid && !element15.IsSolid;
            if (!flag6)
              break;
          }
          if (!flag6)
            break;
        }
        return flag6;
      case Mob.Location.Liquid:
        bool flag7 = true;
        Debug.Assert(mob.paddingX == 0, (object) ("Mob paddingX not implemented yet for rule 'Liquid' and is used for mob " + mob.name));
        for (int y = 0; y < mob.height; ++y)
        {
          for (int widthIterator = 0; widthIterator < mob.width; ++widthIterator)
          {
            int cell8 = MobSpawning.MobWidthOffset(Grid.OffsetCell(cell, 0, y), widthIterator);
            Element element17 = ElementLoader.elements[(int) simData.cells[cell8].elementIdx];
            flag7 = flag7 && element17.IsLiquid;
            if (y == mob.height - 1)
            {
              Element element18 = ElementLoader.elements[(int) simData.cells[Grid.CellAbove(cell8)].elementIdx];
              flag7 = flag7 && element18.IsLiquid;
            }
          }
        }
        return flag7;
      case Mob.Location.EntombedFloorPeek:
        bool flag8 = false;
        bool flag9 = true;
        Debug.Assert(mob.paddingX == 0, (object) ("Mob paddingX not implemented yet for rule 'EntombedFloorPeek' and is used for mob " + mob.name));
        for (int y = 0; y < mob.height; ++y)
        {
          for (int widthIterator = 0; widthIterator < mob.width; ++widthIterator)
          {
            int cell9 = MobSpawning.MobWidthOffset(Grid.OffsetCell(cell, 0, y), widthIterator);
            Element element19 = ElementLoader.elements[(int) simData.cells[cell9].elementIdx];
            Element element20 = ElementLoader.elements[(int) simData.cells[Grid.CellBelow(cell9)].elementIdx];
            flag8 = flag8 || !element19.IsSolid;
            if (y == 0)
              flag9 = flag9 && element20.IsSolid;
            if (!flag9)
              break;
          }
          if (!flag9)
            break;
        }
        return flag9 & flag8;
      case Mob.Location.AnchoredToBackWall:
        bool flag10 = true;
        Debug.Assert(mob.paddingX == 0, (object) ("Mob paddingX not implemented yet for rule 'AnchoredToBackWall' and is used for mob " + mob.name));
        for (int y = 0; y < mob.height; ++y)
        {
          for (int widthIterator = 0; widthIterator < mob.width; ++widthIterator)
          {
            int index = MobSpawning.MobWidthOffset(Grid.OffsetCell(cell, 0, y), widthIterator);
            Element element21 = ElementLoader.elements[(int) simData.cells[index].elementIdx];
            bool isSolid = ElementLoader.elements[(int) simData.backwallCells[cell].elementIdx].IsSolid;
            flag10 = flag10 & isSolid && !element21.IsSolid;
            if (!flag10)
              break;
          }
          if (!flag10)
            break;
        }
        return flag10;
      default:
        return cavityCells.Contains(cell) && !element1.IsSolid;
    }
  }

  public static void DetectNaturalCavities(
    int world,
    Sim.Cell[] cells,
    HashSet<int> cavityCells,
    WorldGen.OfflineCallbackFunction updateProgressFn)
  {
    int num1 = updateProgressFn(UI.WORLDGEN.ANALYZINGWORLD.key, 0.0f, WorldGenProgressStages.Stages.DetectNaturalCavities) ? 1 : 0;
    Func<int, FloodFill.BoundaryCheckResult> boundaryCondition = new Func<int, FloodFill.BoundaryCheckResult>(BoundaryCondition);
    HashSetPool<int, MobSpawning.AllocatorTag>.PooledHashSet visitedCells = HashSetPool<int, MobSpawning.AllocatorTag>.Allocate();
    ListPool<int, MobSpawning.AllocatorTag>.PooledList pooledList = ListPool<int, MobSpawning.AllocatorTag>.Allocate();
    for (int startCell = 0; startCell < cells.Length; ++startCell)
    {
      float completePercent = (float) visitedCells.Count / (float) cells.Length;
      int num2 = updateProgressFn(UI.WORLDGEN.ANALYZINGWORLD.key, completePercent, WorldGenProgressStages.Stages.DetectNaturalCavities) ? 1 : 0;
      pooledList.Clear();
      FloodFill.DepthCollect(startCell, boundaryCondition, (HashSet<int>) visitedCells, (List<int>) pooledList);
      if (pooledList.Count != 0 && pooledList.Count <= 300)
        cavityCells.UnionWith((IEnumerable<int>) pooledList);
    }
    pooledList.Recycle();
    int num3 = updateProgressFn(UI.WORLDGEN.ANALYZINGWORLDCOMPLETE.key, 1f, WorldGenProgressStages.Stages.DetectNaturalCavities) ? 1 : 0;
    visitedCells.Recycle();

    FloodFill.BoundaryCheckResult BoundaryCondition(int cell)
    {
      return !ElementLoader.elements[(int) cells[cell].elementIdx].IsSolid ? FloodFill.BoundaryCheckResult.Continue : FloodFill.BoundaryCheckResult.Halt;
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  private struct AllocatorTag
  {
  }
}
