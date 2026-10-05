// Decompiled with JetBrains decompiler
// Type: ProcGenGame.TemplateSpawning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ProcGenGame;

public class TemplateSpawning
{
  private static float s_minProgressPercent;
  private static float s_maxProgressPercent;
  private static int s_poiPadding;
  private const int TEMPERATURE_PADDING = 3;
  private const float EXTREME_POI_OVERLAP_TEMPERATURE_RANGE = 100f;

  public static List<TemplateSpawning.TemplateSpawner> DetermineTemplatesForWorld(
    WorldGenSettings settings,
    List<TerrainCell> terrainCells,
    SeededRandom myRandom,
    ref List<RectInt> placedPOIBounds,
    bool isRunningDebugGen,
    ref List<WorldTrait> placedStoryTraits,
    WorldGen.OfflineCallbackFunction successCallbackFn)
  {
    int num1 = successCallbackFn(UI.WORLDGEN.PLACINGTEMPLATES.key, 0.0f, WorldGenProgressStages.Stages.PlaceTemplates) ? 1 : 0;
    List<TemplateSpawning.TemplateSpawner> templateSpawnTargets = new List<TemplateSpawning.TemplateSpawner>();
    TemplateSpawning.s_poiPadding = settings.GetIntSetting("POIPadding");
    TemplateSpawning.ValidateTemplateRules(settings);
    TemplateSpawning.s_minProgressPercent = 0.0f;
    TemplateSpawning.s_maxProgressPercent = 0.33f;
    TemplateSpawning.SpawnStartingTemplate(settings, terrainCells, ref templateSpawnTargets, ref placedPOIBounds, isRunningDebugGen, successCallbackFn);
    TemplateSpawning.s_minProgressPercent = TemplateSpawning.s_maxProgressPercent;
    TemplateSpawning.s_maxProgressPercent = 0.66f;
    TemplateSpawning.SpawnTemplatesFromTemplateRules(settings, terrainCells, myRandom, ref templateSpawnTargets, ref placedPOIBounds, isRunningDebugGen, successCallbackFn);
    TemplateSpawning.s_minProgressPercent = TemplateSpawning.s_maxProgressPercent;
    TemplateSpawning.s_maxProgressPercent = 1f;
    TemplateSpawning.SpawnStoryTraitTemplates(settings, terrainCells, myRandom, ref templateSpawnTargets, ref placedPOIBounds, ref placedStoryTraits, isRunningDebugGen, successCallbackFn);
    int num2 = successCallbackFn(UI.WORLDGEN.PLACINGTEMPLATES.key, 1f, WorldGenProgressStages.Stages.PlaceTemplates) ? 1 : 0;
    return templateSpawnTargets;
  }

  private static float ProgressPercent(float stagePercent)
  {
    return MathUtil.ReRange(stagePercent, 0.0f, 1f, TemplateSpawning.s_minProgressPercent, TemplateSpawning.s_maxProgressPercent);
  }

  private static void SpawnStartingTemplate(
    WorldGenSettings settings,
    List<TerrainCell> terrainCells,
    ref List<TemplateSpawning.TemplateSpawner> templateSpawnTargets,
    ref List<RectInt> placedPOIBounds,
    bool isRunningDebugGen,
    WorldGen.OfflineCallbackFunction successCallbackFn)
  {
    TerrainCell terrainCell = terrainCells.Find((Predicate<TerrainCell>) (tc => tc.node.tags.Contains(WorldGenTags.StartLocation)));
    if (settings.world.startingBaseTemplate.IsNullOrWhiteSpace())
      return;
    TemplateContainer template = TemplateCache.GetTemplate(settings.world.startingBaseTemplate);
    Vector2I position = new Vector2I((int) terrainCell.poly.Centroid().x, (int) terrainCell.poly.Centroid().y);
    RectInt templateBounds = template.GetTemplateBounds(position, TemplateSpawning.s_poiPadding);
    TemplateSpawning.TemplateSpawner templateSpawner = new TemplateSpawning.TemplateSpawner(position, templateBounds, template, terrainCell);
    if (TemplateSpawning.IsPOIOverlappingBounds(placedPOIBounds, templateBounds))
    {
      string str = $"TemplateSpawning: Starting template overlaps world boundaries in world '{settings.world.filePath}'";
      DebugUtil.DevLogError(str);
      if (!isRunningDebugGen)
        throw new Exception(str);
    }
    int num = successCallbackFn(UI.WORLDGEN.PLACINGTEMPLATES.key, TemplateSpawning.ProgressPercent(1f), WorldGenProgressStages.Stages.PlaceTemplates) ? 1 : 0;
    templateSpawnTargets.Add(templateSpawner);
    placedPOIBounds.Add(templateBounds);
  }

  private static void SpawnTemplatesFromTemplateRules(
    WorldGenSettings settings,
    List<TerrainCell> terrainCells,
    SeededRandom myRandom,
    ref List<TemplateSpawning.TemplateSpawner> templateSpawnTargets,
    ref List<RectInt> placedPOIBounds,
    bool isRunningDebugGen,
    WorldGen.OfflineCallbackFunction successCallbackFn)
  {
    List<ProcGen.World.TemplateSpawnRules> templateSpawnRulesList = new List<ProcGen.World.TemplateSpawnRules>();
    if (settings.world.worldTemplateRules != null)
      templateSpawnRulesList.AddRange((IEnumerable<ProcGen.World.TemplateSpawnRules>) settings.world.worldTemplateRules);
    foreach (WeightedSubworldName subworldFile in settings.world.subworldFiles)
    {
      SubWorld subWorld = settings.GetSubWorld(subworldFile.name);
      if (subWorld.subworldTemplateRules != null)
        templateSpawnRulesList.AddRange((IEnumerable<ProcGen.World.TemplateSpawnRules>) subWorld.subworldTemplateRules);
    }
    if (templateSpawnRulesList.Count == 0)
      return;
    int num1 = 0;
    float count = (float) templateSpawnRulesList.Count;
    templateSpawnRulesList.Sort((Comparison<ProcGen.World.TemplateSpawnRules>) ((a, b) => b.priority.CompareTo(a.priority)));
    List<TemplateSpawning.TemplateSpawner> newTemplateSpawnTargets = new List<TemplateSpawning.TemplateSpawner>();
    HashSet<string> usedTemplates = new HashSet<string>();
    foreach (ProcGen.World.TemplateSpawnRules rule in templateSpawnRulesList)
    {
      int num2 = successCallbackFn(UI.WORLDGEN.PLACINGTEMPLATES.key, TemplateSpawning.ProgressPercent((float) num1++ / count), WorldGenProgressStages.Stages.PlaceTemplates) ? 1 : 0;
      string errorMessage;
      if (!TemplateSpawning.ApplyTemplateRule(settings, terrainCells, myRandom, ref templateSpawnTargets, ref placedPOIBounds, rule, ref usedTemplates, out errorMessage, ref newTemplateSpawnTargets))
      {
        DebugUtil.LogErrorArgs((object) errorMessage);
        if (!isRunningDebugGen)
          throw new WorldgenException(errorMessage, (string) UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE);
      }
    }
  }

  private static void SpawnStoryTraitTemplates(
    WorldGenSettings settings,
    List<TerrainCell> terrainCells,
    SeededRandom myRandom,
    ref List<TemplateSpawning.TemplateSpawner> templateSpawnTargets,
    ref List<RectInt> placedPOIBounds,
    ref List<WorldTrait> placedStoryTraits,
    bool isRunningDebugGen,
    WorldGen.OfflineCallbackFunction successCallbackFn)
  {
    Queue<WorldTrait> worldTraitQueue = new Queue<WorldTrait>((IEnumerable<WorldTrait>) settings.GetStoryTraitCandiates());
    int count = worldTraitQueue.Count;
    List<WorldTrait> worldTraitList = new List<WorldTrait>();
    HashSet<string> usedTemplates = new HashSet<string>();
    while (worldTraitQueue.Count > 0 && worldTraitList.Count < count)
    {
      WorldTrait storyTrait = worldTraitQueue.Dequeue();
      bool flag = false;
      List<TemplateSpawning.TemplateSpawner> newTemplateSpawnTargets = new List<TemplateSpawning.TemplateSpawner>();
      string errorMessage = "";
      List<ProcGen.World.TemplateSpawnRules> templateSpawnRulesList = new List<ProcGen.World.TemplateSpawnRules>();
      templateSpawnRulesList.AddRange((IEnumerable<ProcGen.World.TemplateSpawnRules>) storyTrait.additionalWorldTemplateRules);
      templateSpawnRulesList.Sort((Comparison<ProcGen.World.TemplateSpawnRules>) ((a, b) => b.priority.CompareTo(a.priority)));
      foreach (ProcGen.World.TemplateSpawnRules rule in templateSpawnRulesList)
      {
        flag = TemplateSpawning.ApplyTemplateRule(settings, terrainCells, myRandom, ref templateSpawnTargets, ref placedPOIBounds, rule, ref usedTemplates, out errorMessage, ref newTemplateSpawnTargets);
        if (!flag)
        {
          flag = false;
          break;
        }
      }
      if (flag)
      {
        placedStoryTraits.Add(storyTrait);
        worldTraitList.Add(storyTrait);
        settings.ApplyStoryTrait(storyTrait);
        DebugUtil.LogArgs((object) $"Applied story trait '{storyTrait.filePath}'");
      }
      else
      {
        foreach (TemplateSpawning.TemplateSpawner toRemove in newTemplateSpawnTargets)
        {
          TemplateSpawning.RemoveTemplate(toRemove, settings, terrainCells, ref templateSpawnTargets, ref placedPOIBounds);
          usedTemplates.Remove(toRemove.container.name);
        }
        if (DlcManager.FeatureClusterSpaceEnabled())
          DebugUtil.LogArgs((object) $"Cannot place story trait on '{storyTrait.filePath}' and will try another world. error='{errorMessage}'.");
        else
          DebugUtil.LogArgs((object) $"Cannot place story trait '{storyTrait.filePath}' error='{errorMessage}'");
      }
    }
  }

  private static void RemoveTemplate(
    TemplateSpawning.TemplateSpawner toRemove,
    WorldGenSettings settings,
    List<TerrainCell> terrainCells,
    ref List<TemplateSpawning.TemplateSpawner> templateSpawnTargets,
    ref List<RectInt> placedPOIBounds)
  {
    TemplateSpawning.UpdateNodeTags((ProcGen.Node) toRemove.terrainCell.node, toRemove.container.name, true);
    templateSpawnTargets.Remove(toRemove);
    placedPOIBounds.RemoveAll((Predicate<RectInt>) (bound => bound.center == (Vector2) toRemove.position));
  }

  private static void ValidateTemplateRules(List<ProcGen.World.TemplateSpawnRules> rules, string context)
  {
    foreach (ProcGen.World.TemplateSpawnRules rule in rules)
    {
      foreach (string name in rule.names)
        DebugUtil.DevAssert((TemplateCache.TemplateExists(name) ? 1 : 0) != 0, string.Format($"TemplateSpawning: Missing template '{name}' in '{context}'"));
    }
  }

  private static void ValidateTemplateRules(WorldGenSettings settings)
  {
    TemplateSpawning.ValidateTemplateRules(settings.world.worldTemplateRules, settings.world.filePath);
    foreach (WeightedSubworldName subworldFile in settings.world.subworldFiles)
    {
      SubWorld subWorld = settings.GetSubWorld(subworldFile.name);
      if (subWorld.subworldTemplateRules != null)
        TemplateSpawning.ValidateTemplateRules(subWorld.subworldTemplateRules, subWorld.name);
    }
    foreach (WorldTrait storyTraitCandiate in settings.GetStoryTraitCandiates())
      TemplateSpawning.ValidateTemplateRules(storyTraitCandiate.additionalWorldTemplateRules, "Story Trait Candidates");
  }

  private static bool ApplyTemplateRule(
    WorldGenSettings settings,
    List<TerrainCell> terrainCells,
    SeededRandom myRandom,
    ref List<TemplateSpawning.TemplateSpawner> templateSpawnTargets,
    ref List<RectInt> placedPOIBounds,
    ProcGen.World.TemplateSpawnRules rule,
    ref HashSet<string> usedTemplates,
    out string errorMessage,
    ref List<TemplateSpawning.TemplateSpawner> newTemplateSpawnTargets)
  {
    ListPool<string, TemplateSpawning>.PooledList candidates = ListPool<string, TemplateSpawning>.Allocate();
    for (int index = 0; index < rule.times; ++index)
    {
      candidates.Clear();
      foreach (string name in rule.names)
      {
        if ((rule.allowDuplicates || !usedTemplates.Contains(name)) && TemplateCache.TemplateExists(name))
          candidates.Add(name);
      }
      candidates.ShuffleSeeded<string>(myRandom.RandomSource());
      if (candidates.Count != 0)
      {
        int num1 = 0;
        if (rule.listRule == ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeRange || rule.listRule == ProcGen.World.TemplateSpawnRules.ListRule.TryRange)
          num1 = myRandom.RandomRange(rule.range.x, rule.range.y);
        int num2 = 0;
        int num3 = 0;
        switch (rule.listRule)
        {
          case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeOne:
            num2 = 1;
            num3 = 1;
            break;
          case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeSome:
            num2 = rule.someCount;
            num3 = rule.someCount;
            break;
          case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeSomeTryMore:
            num2 = rule.someCount;
            num3 = rule.someCount + rule.moreCount;
            break;
          case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeAll:
            num2 = candidates.Count;
            num3 = candidates.Count;
            break;
          case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeRange:
            num2 = num1;
            num3 = num1;
            break;
          case ProcGen.World.TemplateSpawnRules.ListRule.TryOne:
            num3 = 1;
            break;
          case ProcGen.World.TemplateSpawnRules.ListRule.TrySome:
            num3 = rule.someCount;
            break;
          case ProcGen.World.TemplateSpawnRules.ListRule.TryRange:
            num3 = num1;
            break;
          case ProcGen.World.TemplateSpawnRules.ListRule.TryAll:
            num3 = candidates.Count;
            break;
        }
        string str1 = "";
        foreach (string str2 in (List<string>) candidates)
        {
          if (num3 > 0)
          {
            TemplateContainer template = TemplateCache.GetTemplate(str2);
            if (template != null)
            {
              bool guarantee = num2 > 0;
              Vector2I position = Vector2I.zero;
              TerrainCell targetForTemplate;
              if (rule.overridePlacement != Vector2I.minusone)
              {
                position = rule.overridePlacement;
                targetForTemplate = terrainCells.Find((Predicate<TerrainCell>) (x => x.poly.Contains((Vector2) rule.overridePlacement)));
                if (num2 > 0 && targetForTemplate.node.templateTag != Tag.Invalid)
                {
                  errorMessage = $"Tried to place '{str2}' at ({position.x},{position.y}) using overridePlacement but '{targetForTemplate.node.templateTag}' is already there.";
                  return ExitFn(false);
                }
              }
              else
              {
                targetForTemplate = TemplateSpawning.FindTargetForTemplate(template, rule, terrainCells, myRandom, ref templateSpawnTargets, ref placedPOIBounds, guarantee, settings);
                if (targetForTemplate != null)
                  position = new Vector2I((int) targetForTemplate.poly.Centroid().x + rule.overrideOffset.x, (int) targetForTemplate.poly.Centroid().y + rule.overrideOffset.y);
              }
              if (targetForTemplate != null)
              {
                RectInt templateBounds = template.GetTemplateBounds(position, TemplateSpawning.s_poiPadding);
                TemplateSpawning.TemplateSpawner templateSpawner = new TemplateSpawning.TemplateSpawner(position, templateBounds, template, targetForTemplate);
                templateSpawnTargets.Add(templateSpawner);
                newTemplateSpawnTargets.Add(templateSpawner);
                placedPOIBounds.Add(templateBounds);
                TemplateSpawning.UpdateNodeTags((ProcGen.Node) targetForTemplate.node, str2);
                usedTemplates.Add(str2);
                --num3;
                --num2;
              }
              else
                str1 = $"{str1}\n    - {str2}";
            }
          }
          else
            break;
        }
        if (num2 > 0)
        {
          string str3 = string.Join(", ", settings.GetWorldTraitIDs());
          string str4 = string.Join(", ", settings.GetStoryTraitIDs());
          errorMessage = $"TemplateSpawning: Guaranteed placement failure on {settings.world.filePath}\n{$"    listRule={rule.listRule} someCount={rule.someCount} moreCount={rule.moreCount} count={candidates.Count}\n"}    Could not place templates:{str1}\n    world traits={str3}\n    story traits={str4}";
          return ExitFn(false);
        }
      }
    }
    errorMessage = "";
    return ExitFn(true);

    bool ExitFn(bool result)
    {
      candidates.Recycle();
      return result;
    }
  }

  private static void UpdateNodeTags(ProcGen.Node node, string template, bool remove = false)
  {
    Tag tag = template.ToTag();
    if (remove)
    {
      node.templateTag = Tag.Invalid;
      node.tags.Remove(tag);
      node.tags.Remove(WorldGenTags.POI);
    }
    else
    {
      node.templateTag = tag;
      node.tags.Add(template.ToTag());
      node.tags.Add(WorldGenTags.POI);
    }
  }

  private static TerrainCell FindTargetForTemplate(
    TemplateContainer template,
    ProcGen.World.TemplateSpawnRules rule,
    List<TerrainCell> terrainCells,
    SeededRandom myRandom,
    ref List<TemplateSpawning.TemplateSpawner> templateSpawnTargets,
    ref List<RectInt> placedPOIBounds,
    bool guarantee,
    WorldGenSettings settings)
  {
    List<TerrainCell> filteredTerrainCells = !rule.allowNearStart ? (rule.useRelaxedFiltering ? terrainCells.FindAll((Predicate<TerrainCell>) (tc =>
    {
      tc.LogInfo("Filtering Relaxed (replace features)", template.name, 0.0f);
      return tc.IsSafeToSpawnPOIRelaxed(terrainCells) && TemplateSpawning.DoesCellMatchFilters(tc, rule.allowedCellsFilter);
    })) : terrainCells.FindAll((Predicate<TerrainCell>) (tc =>
    {
      tc.LogInfo("Filtering", template.name, 0.0f);
      return tc.IsSafeToSpawnPOI(terrainCells) && TemplateSpawning.DoesCellMatchFilters(tc, rule.allowedCellsFilter);
    }))) : terrainCells.FindAll((Predicate<TerrainCell>) (tc =>
    {
      tc.LogInfo("Filtering Near Start", template.name, 0.0f);
      return tc.IsSafeToSpawnPOINearStart(terrainCells) && TemplateSpawning.DoesCellMatchFilters(tc, rule.allowedCellsFilter);
    }));
    TemplateSpawning.RemoveOverlappingPOIs(ref filteredTerrainCells, ref terrainCells, ref placedPOIBounds, template, settings, rule.allowExtremeTemperatureOverlap, (Vector2) rule.overrideOffset);
    if (filteredTerrainCells.Count == 0 & guarantee)
    {
      if (rule.allowNearStart && rule.useRelaxedFiltering)
      {
        DebugUtil.LogWarningArgs((object) $"Could not place {template.name} using normal rules, trying relaxed near start");
        filteredTerrainCells = terrainCells.FindAll((Predicate<TerrainCell>) (tc =>
        {
          tc.LogInfo("Filtering Near Start Relaxed", template.name, 0.0f);
          return tc.IsSafeToSpawnPOINearStartRelaxed(terrainCells) && TemplateSpawning.DoesCellMatchFilters(tc, rule.allowedCellsFilter);
        }));
        TemplateSpawning.RemoveOverlappingPOIs(ref filteredTerrainCells, ref terrainCells, ref placedPOIBounds, template, settings, rule.allowExtremeTemperatureOverlap, (Vector2) rule.overrideOffset);
      }
      else if (!rule.useRelaxedFiltering)
      {
        DebugUtil.LogWarningArgs((object) $"Could not place {template.name} using normal rules, trying relaxed");
        filteredTerrainCells = terrainCells.FindAll((Predicate<TerrainCell>) (tc =>
        {
          tc.LogInfo("Filtering Relaxed", template.name, 0.0f);
          return tc.IsSafeToSpawnPOIRelaxed(terrainCells) && TemplateSpawning.DoesCellMatchFilters(tc, rule.allowedCellsFilter);
        }));
        TemplateSpawning.RemoveOverlappingPOIs(ref filteredTerrainCells, ref terrainCells, ref placedPOIBounds, template, settings, rule.allowExtremeTemperatureOverlap, (Vector2) rule.overrideOffset);
      }
    }
    if (filteredTerrainCells.Count == 0)
      return (TerrainCell) null;
    filteredTerrainCells.ShuffleSeeded<TerrainCell>(myRandom.RandomSource());
    return filteredTerrainCells[filteredTerrainCells.Count - 1];
  }

  private static bool IsPOIOverlappingBounds(List<RectInt> placedPOIBounds, RectInt templateBounds)
  {
    foreach (RectInt placedPoiBound in placedPOIBounds)
    {
      if (templateBounds.Overlaps(placedPoiBound))
        return true;
    }
    return false;
  }

  private static bool IsPOIOverlappingHighTemperatureDelta(
    RectInt paddedTemplateBounds,
    SubWorld subworld,
    ref List<TerrainCell> allCells,
    WorldGenSettings settings)
  {
    Vector2 vector2_1 = 2f * Vector2.one * (float) TemplateSpawning.s_poiPadding;
    Vector2 vector2_2 = 2f * Vector2.one * 3f;
    UnityEngine.Rect rect = new UnityEngine.Rect((Vector2) paddedTemplateBounds.position, (Vector2) paddedTemplateBounds.size - vector2_1 + vector2_2);
    ProcGen.Temperature temperature1 = SettingsCache.temperatures[subworld.temperatureRange];
    foreach (TerrainCell terrainCell in allCells)
    {
      SubWorld subWorld = settings.GetSubWorld(terrainCell.node.GetSubworld());
      ProcGen.Temperature temperature2 = SettingsCache.temperatures[subWorld.temperatureRange];
      if (subWorld.temperatureRange != subworld.temperatureRange)
      {
        float num1 = Mathf.Min(temperature1.min, temperature2.min);
        float num2 = Mathf.Max(temperature1.max, temperature2.max) - num1;
        if (rect.Overlaps(terrainCell.poly.bounds) & (double) num2 > 100.0)
          return true;
      }
    }
    return false;
  }

  private static void RemoveOverlappingPOIs(
    ref List<TerrainCell> filteredTerrainCells,
    ref List<TerrainCell> allCells,
    ref List<RectInt> placedPOIBounds,
    TemplateContainer container,
    WorldGenSettings settings,
    bool allowExtremeTemperatureOverlap,
    Vector2 poiOffset)
  {
    for (int index1 = filteredTerrainCells.Count - 1; index1 >= 0; --index1)
    {
      TerrainCell terrainCell = filteredTerrainCells[index1];
      int index2 = index1;
      SubWorld subWorld = settings.GetSubWorld(terrainCell.node.GetSubworld());
      RectInt templateBounds = container.GetTemplateBounds(terrainCell.poly.Centroid() + poiOffset, TemplateSpawning.s_poiPadding);
      bool flag = false;
      if (TemplateSpawning.IsPOIOverlappingBounds(placedPOIBounds, templateBounds))
      {
        terrainCell.LogInfo("-> Removed due to overlapping POIs", "", 0.0f);
        flag = true;
      }
      else if (!allowExtremeTemperatureOverlap && TemplateSpawning.IsPOIOverlappingHighTemperatureDelta(templateBounds, subWorld, ref allCells, settings))
      {
        terrainCell.LogInfo("-> Removed due to overlapping extreme temperature delta", "", 0.0f);
        flag = true;
      }
      if (flag)
        filteredTerrainCells.RemoveAt(index2);
    }
  }

  public static bool DoesCellMatchFilters(TerrainCell cell, List<ProcGen.World.AllowedCellsFilter> filters)
  {
    bool flag1 = false;
    foreach (ProcGen.World.AllowedCellsFilter filter in filters)
    {
      bool applied;
      bool flag2 = TemplateSpawning.DoesCellMatchFilter(cell, filter, out applied);
      if (applied)
      {
        ProcGen.World.AllowedCellsFilter.Command command = filter.command;
        switch (command)
        {
          case ProcGen.World.AllowedCellsFilter.Command.Clear:
            flag1 = false;
            break;
          case ProcGen.World.AllowedCellsFilter.Command.Replace:
            flag1 = flag2;
            break;
          case ProcGen.World.AllowedCellsFilter.Command.UnionWith:
            flag1 = flag2 | flag1;
            break;
          case ProcGen.World.AllowedCellsFilter.Command.IntersectWith:
            flag1 = flag2 & flag1;
            break;
          case ProcGen.World.AllowedCellsFilter.Command.ExceptWith:
          case ProcGen.World.AllowedCellsFilter.Command.SymmetricExceptWith:
            if (flag2)
            {
              flag1 = false;
              break;
            }
            break;
          case ProcGen.World.AllowedCellsFilter.Command.All:
            flag1 = true;
            break;
        }
        TerrainCell terrainCell = cell;
        command = filter.command;
        string evt = "-> DoesCellMatchFilter " + command.ToString();
        string str = flag2 ? "1" : "0";
        double num = flag1 ? 1.0 : 0.0;
        terrainCell.LogInfo(evt, str, (float) num);
      }
    }
    cell.LogInfo("> Final match", flag1 ? "true" : "false", 0.0f);
    return flag1;
  }

  private static bool DoesCellMatchFilter(
    TerrainCell cell,
    ProcGen.World.AllowedCellsFilter filter,
    out bool applied)
  {
    applied = true;
    if (!TemplateSpawning.ValidateFilter(filter))
      return false;
    if (filter.tagcommand != ProcGen.World.AllowedCellsFilter.TagCommand.Default)
    {
      switch (filter.tagcommand)
      {
        case ProcGen.World.AllowedCellsFilter.TagCommand.Default:
          return true;
        case ProcGen.World.AllowedCellsFilter.TagCommand.AtTag:
          return cell.node.tags.Contains((Tag) filter.tag);
        case ProcGen.World.AllowedCellsFilter.TagCommand.NotAtTag:
          return !cell.node.tags.Contains((Tag) filter.tag);
        case ProcGen.World.AllowedCellsFilter.TagCommand.DistanceFromTag:
          Tag tag1 = filter.tag.ToTag();
          bool flag = cell.distancesToTags.ContainsKey(tag1);
          if (!flag && tag1 == WorldGenTags.AtStart && !filter.ignoreIfMissingTag)
            DebugUtil.DevLogError("DistanceFromTag was used on a world without an AtStart tag, use ignoreIfMissingTag to skip it.");
          else
            Debug.Assert(flag || filter.ignoreIfMissingTag, (object) $"DistanceFromTag is missing tag {filter.tag}, use ignoreIfMissingTag to skip it.");
          if (flag)
          {
            int tag2 = cell.DistanceToTag(tag1);
            return tag2 >= filter.minDistance && tag2 <= filter.maxDistance;
          }
          applied = false;
          return true;
      }
    }
    else
    {
      if (filter.subworldNames != null && filter.subworldNames.Count > 0)
      {
        foreach (string subworldName in filter.subworldNames)
        {
          if (cell.node.tags.Contains((Tag) subworldName))
            return true;
        }
        return false;
      }
      if (filter.zoneTypes != null && filter.zoneTypes.Count > 0)
      {
        foreach (SubWorld.ZoneType zoneType in filter.zoneTypes)
        {
          if (cell.node.tags.Contains((Tag) zoneType.ToString()))
            return true;
        }
        return false;
      }
      if (filter.temperatureRanges != null && filter.temperatureRanges.Count > 0)
      {
        foreach (ProcGen.Temperature.Range temperatureRange in filter.temperatureRanges)
        {
          if (cell.node.tags.Contains((Tag) temperatureRange.ToString()))
            return true;
        }
        return false;
      }
    }
    return true;
  }

  private static bool ValidateFilter(ProcGen.World.AllowedCellsFilter filter)
  {
    if (filter.command == ProcGen.World.AllowedCellsFilter.Command.All)
      return true;
    int num = 0;
    if (filter.tagcommand != ProcGen.World.AllowedCellsFilter.TagCommand.Default)
      ++num;
    if (filter.subworldNames != null && filter.subworldNames.Count > 0)
      ++num;
    if (filter.zoneTypes != null && filter.zoneTypes.Count > 0)
      ++num;
    if (filter.temperatureRanges != null && filter.temperatureRanges.Count > 0)
      ++num;
    if (num == 1)
      return true;
    string str = "BAD ALLOWED CELLS FILTER in FEATURE RULES!" + "\nA filter can only specify one of `tagcommand`, `subworldNames`, `zoneTypes`, or `temperatureRanges`." + "\nFound a filter with the following:";
    if (filter.tagcommand != ProcGen.World.AllowedCellsFilter.TagCommand.Default)
      str = str + "\ntagcommand:\n\t" + filter.tagcommand.ToString() + "\ntag:\n\t" + filter.tag;
    if (filter.subworldNames != null && filter.subworldNames.Count > 0)
      str = str + "\nsubworldNames:\n\t" + string.Join(", ", (IEnumerable<string>) filter.subworldNames);
    if (filter.zoneTypes != null && filter.zoneTypes.Count > 0)
      str = str + "\nzoneTypes:\n" + string.Join<SubWorld.ZoneType>(", ", (IEnumerable<SubWorld.ZoneType>) filter.zoneTypes);
    if (filter.temperatureRanges != null && filter.temperatureRanges.Count > 0)
      str = str + "\ntemperatureRanges:\n" + string.Join<ProcGen.Temperature.Range>(", ", (IEnumerable<ProcGen.Temperature.Range>) filter.temperatureRanges);
    Debug.LogError((object) str);
    return false;
  }

  public class TemplateSpawner
  {
    public Vector2I position;
    public TemplateContainer container;
    public TerrainCell terrainCell;
    public RectInt bounds;

    public TemplateSpawner(
      Vector2I position,
      RectInt bounds,
      TemplateContainer container,
      TerrainCell terrainCell)
    {
      this.position = position;
      this.container = container;
      this.terrainCell = terrainCell;
      this.bounds = bounds;
    }
  }
}
