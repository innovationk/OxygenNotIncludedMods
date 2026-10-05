// Decompiled with JetBrains decompiler
// Type: ProcGenGame.WorldgenMixing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.CustomSettings;
using ObjectCloner;
using ProcGen;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace ProcGenGame;

public class WorldgenMixing
{
  private const int NUM_WORLD_TO_TRY_SUBWORLDMIXING = 3;

  public static bool RefreshWorldMixing(
    MutatedClusterLayout mutatedLayout,
    int seed,
    bool isRunningWorldgenDebug,
    bool muteErrors)
  {
    if (mutatedLayout == null)
      return false;
    foreach (WorldPlacement worldPlacement in mutatedLayout.layout.worldPlacements)
      worldPlacement.UndoWorldMixing();
    return WorldgenMixing.DoWorldMixingInternal(mutatedLayout, seed, isRunningWorldgenDebug, muteErrors) != null;
  }

  public static MutatedClusterLayout DoWorldMixing(
    ClusterLayout layout,
    int seed,
    bool isRunningWorldgenDebug,
    bool muteErrors)
  {
    return WorldgenMixing.DoWorldMixingInternal(new MutatedClusterLayout(layout), seed, isRunningWorldgenDebug, muteErrors);
  }

  private static MutatedClusterLayout DoWorldMixingInternal(
    MutatedClusterLayout mutatedClusterLayout,
    int seed,
    bool isRunningWorldgenDebug,
    bool muteErrors)
  {
    List<WorldgenMixing.WorldMixingOption> worldMixingOptionList1 = new List<WorldgenMixing.WorldMixingOption>();
    if ((UnityEngine.Object) CustomGameSettings.Instance != (UnityEngine.Object) null && !GenericGameSettings.instance.devAutoWorldGen)
    {
      foreach (WorldMixingSettingConfig worldMixingSetting1 in CustomGameSettings.Instance.GetActiveWorldMixingSettings())
      {
        WorldMixingSettings worldMixingSetting2 = SettingsCache.TryGetCachedWorldMixingSetting(worldMixingSetting1.worldgenPath);
        if (!mutatedClusterLayout.layout.HasAnyTags(worldMixingSetting2.forbiddenClusterTags))
        {
          int num = CustomGameSettings.Instance.GetCurrentMixingSettingLevel(worldMixingSetting1.id).id == "GuranteeMixing" ? 1 : 0;
          ProcGen.World worldData = SettingsCache.worlds.GetWorldData(worldMixingSetting2.world);
          List<WorldgenMixing.WorldMixingOption> worldMixingOptionList2 = worldMixingOptionList1;
          WorldgenMixing.WorldMixingOption worldMixingOption = new WorldgenMixing.WorldMixingOption();
          worldMixingOption.worldgenPath = worldMixingSetting2.world;
          worldMixingOption.mixingSettings = worldMixingSetting2;
          worldMixingOption.minCount = num;
          worldMixingOption.maxCount = 1;
          worldMixingOption.cachedWorld = worldData;
          worldMixingOptionList2.Add(worldMixingOption);
        }
      }
    }
    else
    {
      foreach (string name in GenericGameSettings.instance.devWorldMixing)
      {
        WorldMixingSettings worldMixingSetting = SettingsCache.TryGetCachedWorldMixingSetting(name);
        ProcGen.World worldData = SettingsCache.worlds.GetWorldData(worldMixingSetting.world);
        List<WorldgenMixing.WorldMixingOption> worldMixingOptionList3 = worldMixingOptionList1;
        WorldgenMixing.WorldMixingOption worldMixingOption = new WorldgenMixing.WorldMixingOption();
        worldMixingOption.worldgenPath = worldMixingSetting.world;
        worldMixingOption.mixingSettings = worldMixingSetting;
        worldMixingOption.minCount = 1;
        worldMixingOption.maxCount = 1;
        worldMixingOption.cachedWorld = worldData;
        worldMixingOptionList3.Add(worldMixingOption);
      }
    }
    KRandom rng = new KRandom(seed);
    foreach (WorldPlacement worldPlacement in mutatedClusterLayout.layout.worldPlacements)
      worldPlacement.UndoWorldMixing();
    List<WorldPlacement> list = new List<WorldPlacement>((IEnumerable<WorldPlacement>) mutatedClusterLayout.layout.worldPlacements);
    list.ShuffleSeeded<WorldPlacement>(rng);
    foreach (WorldPlacement worldPlacement in list)
    {
      if (worldPlacement.IsMixingPlacement())
      {
        worldMixingOptionList1.ShuffleSeeded<WorldgenMixing.WorldMixingOption>(rng);
        WorldgenMixing.WorldMixingOption worldMixingOption = WorldgenMixing.FindWorldMixingOption(worldPlacement, worldMixingOptionList1);
        if (worldMixingOption != null)
        {
          Debug.Log((object) $"Mixing: Applied world substitution {worldPlacement.world} -> {worldMixingOption.worldgenPath}");
          worldPlacement.worldMixing.previousWorld = worldPlacement.world;
          worldPlacement.worldMixing.mixingWasApplied = true;
          worldPlacement.world = worldMixingOption.worldgenPath;
          worldMixingOption.Consume();
          if (worldMixingOption.IsExhausted)
            worldMixingOptionList1.Remove(worldMixingOption);
        }
      }
    }
    return !WorldgenMixing.ValidateWorldMixingOptions(worldMixingOptionList1, isRunningWorldgenDebug, muteErrors) ? (MutatedClusterLayout) null : mutatedClusterLayout;
  }

  private static WorldgenMixing.WorldMixingOption FindWorldMixingOption(
    WorldPlacement worldPlacement,
    List<WorldgenMixing.WorldMixingOption> options)
  {
    options = options.StableSort<WorldgenMixing.WorldMixingOption>().ToList<WorldgenMixing.WorldMixingOption>();
    foreach (WorldgenMixing.WorldMixingOption option in options)
    {
      if (!option.IsExhausted)
      {
        bool flag = true;
        foreach (string requiredTag in worldPlacement.worldMixing.requiredTags)
        {
          if (!option.cachedWorld.worldTags.Contains(requiredTag))
          {
            flag = false;
            break;
          }
        }
        foreach (string forbiddenTag in worldPlacement.worldMixing.forbiddenTags)
        {
          if (option.cachedWorld.worldTags.Contains(forbiddenTag))
          {
            flag = false;
            break;
          }
        }
        if (flag)
          return option;
      }
    }
    return (WorldgenMixing.WorldMixingOption) null;
  }

  private static bool ValidateWorldMixingOptions(
    List<WorldgenMixing.WorldMixingOption> options,
    bool isRunningWorldgenDebug,
    bool muteErrors)
  {
    List<string> values = new List<string>();
    foreach (WorldgenMixing.WorldMixingOption option in options)
    {
      if (!option.IsSatisfied)
        values.Add($"{option.worldgenPath} ({option.minCount})");
    }
    if (values.Count <= 0)
      return true;
    if (muteErrors)
      return false;
    string message = "WorldgenMixing: Could not guarantee these world mixings: " + string.Join("\n - ", (IEnumerable<string>) values);
    if (!isRunningWorldgenDebug)
    {
      DebugUtil.LogWarningArgs((object) message);
      throw new WorldgenException(message, (string) UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE_MIXING);
    }
    DebugUtil.LogErrorArgs((object) message);
    return false;
  }

  public static void DoSubworldMixing(
    Cluster cluster,
    int seed,
    Func<int, WorldGen, bool> ShouldSkipWorldCallback,
    bool isRunningWorldgenDebug)
  {
    List<WorldgenMixing.MixingOption<SubworldMixingSettings>> mixingOptionList = new List<WorldgenMixing.MixingOption<SubworldMixingSettings>>();
    if ((UnityEngine.Object) CustomGameSettings.Instance != (UnityEngine.Object) null && !GenericGameSettings.instance.devAutoWorldGen)
    {
      foreach (SubworldMixingSettingConfig subworldMixingSetting1 in CustomGameSettings.Instance.GetActiveSubworldMixingSettings())
      {
        SubworldMixingSettings subworldMixingSetting2 = SettingsCache.TryGetCachedSubworldMixingSetting(subworldMixingSetting1.worldgenPath);
        if (!cluster.clusterLayout.HasAnyTags(subworldMixingSetting1.forbiddenClusterTags))
        {
          int num = CustomGameSettings.Instance.GetCurrentMixingSettingLevel(subworldMixingSetting1.id).id == "GuranteeMixing" ? 1 : 0;
          mixingOptionList.Add(new WorldgenMixing.MixingOption<SubworldMixingSettings>()
          {
            worldgenPath = subworldMixingSetting1.worldgenPath,
            mixingSettings = subworldMixingSetting2,
            minCount = num,
            maxCount = 3
          });
        }
      }
    }
    else
    {
      foreach (string name in GenericGameSettings.instance.devSubworldMixing)
      {
        SubworldMixingSettings subworldMixingSetting = SettingsCache.TryGetCachedSubworldMixingSetting(name);
        mixingOptionList.Add(new WorldgenMixing.MixingOption<SubworldMixingSettings>()
        {
          worldgenPath = name,
          mixingSettings = subworldMixingSetting,
          minCount = 1,
          maxCount = 3
        });
      }
    }
    KRandom rng = new KRandom(seed);
    List<WorldGen> list = new List<WorldGen>((IEnumerable<WorldGen>) cluster.worlds);
    list.ShuffleSeeded<WorldGen>(rng);
    list.Sort((Comparison<WorldGen>) ((a, b) => WorldPlacement.GetSortOrder(a.Settings.worldType).CompareTo(WorldPlacement.GetSortOrder(b.Settings.worldType))));
    for (int index = 0; index < cluster.worlds.Count; ++index)
    {
      WorldGen worldGen = list[index];
      mixingOptionList.ShuffleSeeded<WorldgenMixing.MixingOption<SubworldMixingSettings>>(rng);
      WorldgenMixing.ApplySubworldMixingToWorld(worldGen.Settings.world, mixingOptionList);
    }
    WorldgenMixing.ValidateSubworldMixingOptions(mixingOptionList, isRunningWorldgenDebug);
  }

  private static void ValidateSubworldMixingOptions(
    List<WorldgenMixing.MixingOption<SubworldMixingSettings>> options,
    bool isRunningWorldgenDebug)
  {
    List<string> values = new List<string>();
    foreach (WorldgenMixing.MixingOption<SubworldMixingSettings> option in options)
    {
      if (!option.IsSatisfied)
        values.Add($"{option.worldgenPath} ({option.minCount})");
    }
    if (values.Count <= 0)
      return;
    string message = "WorldgenMixing: Could not guarantee these subworld mixings: " + string.Join("\n - ", (IEnumerable<string>) values);
    if (!isRunningWorldgenDebug)
    {
      DebugUtil.LogWarningArgs((object) message);
      throw new WorldgenException(message, (string) UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE_MIXING);
    }
    DebugUtil.LogErrorArgs((object) message);
  }

  private static void ApplySubworldMixingToWorld(
    ProcGen.World world,
    List<WorldgenMixing.MixingOption<SubworldMixingSettings>> availableSubworldsForMixing)
  {
    List<ProcGen.World.SubworldMixingRule> subworldMixingRuleList = new List<ProcGen.World.SubworldMixingRule>();
    foreach (ProcGen.World.SubworldMixingRule subworldMixingRule in world.subworldMixingRules)
    {
      if (availableSubworldsForMixing.Count == 0)
      {
        WorldgenMixing.CleanupUnusedMixing(world);
        return;
      }
      WorldgenMixing.MixingOption<SubworldMixingSettings> subworldMixing = WorldgenMixing.FindSubworldMixing(subworldMixingRule, availableSubworldsForMixing);
      if (subworldMixing == null)
      {
        Debug.Log((object) $"WorldgenMixing: No valid mixing for '{subworldMixingRule.name}' on World '{world.name}' from options: {string.Join(", ", availableSubworldsForMixing.Where<WorldgenMixing.MixingOption<SubworldMixingSettings>>((Func<WorldgenMixing.MixingOption<SubworldMixingSettings>, bool>) (x => !x.IsExhausted)).Select<WorldgenMixing.MixingOption<SubworldMixingSettings>, string>((Func<WorldgenMixing.MixingOption<SubworldMixingSettings>, string>) (x => x.mixingSettings.name)))}");
      }
      else
      {
        WeightedSubworldName weightedSubworldName = SerializingCloner.Copy<WeightedSubworldName>(subworldMixing.mixingSettings.subworld);
        weightedSubworldName.minCount = Math.Max(subworldMixingRule.minCount, weightedSubworldName.minCount);
        weightedSubworldName.maxCount = Math.Min(subworldMixingRule.maxCount, weightedSubworldName.maxCount);
        world.subworldFiles.Add(weightedSubworldName);
        foreach (ProcGen.World.AllowedCellsFilter cellsAllowedSubworld in world.unknownCellsAllowedSubworlds)
        {
          for (int index = 0; index < cellsAllowedSubworld.subworldNames.Count; ++index)
          {
            if (cellsAllowedSubworld.subworldNames[index] == subworldMixingRule.name)
              cellsAllowedSubworld.subworldNames[index] = weightedSubworldName.name;
          }
        }
        if (!subworldMixingRuleList.Contains(subworldMixingRule))
        {
          world.worldTemplateRules.AddRange((IEnumerable<ProcGen.World.TemplateSpawnRules>) subworldMixing.mixingSettings.additionalWorldTemplateRules);
          subworldMixingRuleList.Add(subworldMixingRule);
        }
        subworldMixing.Consume();
        if (subworldMixing.IsExhausted)
          availableSubworldsForMixing.Remove(subworldMixing);
      }
    }
    WorldgenMixing.CleanupUnusedMixing(world);
  }

  private static WorldgenMixing.MixingOption<SubworldMixingSettings> FindSubworldMixing(
    ProcGen.World.SubworldMixingRule rule,
    List<WorldgenMixing.MixingOption<SubworldMixingSettings>> options)
  {
    options = options.StableSort<WorldgenMixing.MixingOption<SubworldMixingSettings>>().ToList<WorldgenMixing.MixingOption<SubworldMixingSettings>>();
    foreach (WorldgenMixing.MixingOption<SubworldMixingSettings> option in options)
    {
      if (!option.IsExhausted)
      {
        bool flag = true;
        foreach (string forbiddenTag in rule.forbiddenTags)
        {
          if (option.mixingSettings.mixingTags.Contains(forbiddenTag))
            flag = false;
        }
        foreach (string requiredTag in rule.requiredTags)
        {
          if (!option.mixingSettings.mixingTags.Contains(requiredTag))
            flag = false;
        }
        if (Math.Max(rule.minCount, option.mixingSettings.subworld.minCount) > Math.Min(rule.maxCount, option.mixingSettings.subworld.maxCount))
          flag = false;
        if (flag)
          return option;
      }
    }
    return (WorldgenMixing.MixingOption<SubworldMixingSettings>) null;
  }

  private static void CleanupUnusedMixing(ProcGen.World world)
  {
    foreach (ProcGen.World.AllowedCellsFilter cellsAllowedSubworld in world.unknownCellsAllowedSubworlds)
      cellsAllowedSubworld.subworldNames.RemoveAll(new Predicate<string>(WorldgenMixing.IsMixingProxyName));
  }

  private static bool IsMixingProxyName(string name) => name.StartsWith("(");

  public class MixingOption<T> : IComparable<WorldgenMixing.MixingOption<T>>
  {
    public string worldgenPath;
    public T mixingSettings;
    public int minCount;
    public int maxCount;

    public bool IsExhausted => this.maxCount <= 0;

    public bool IsSatisfied => this.minCount <= 0;

    public void Consume()
    {
      --this.minCount;
      --this.maxCount;
    }

    public int CompareTo(WorldgenMixing.MixingOption<T> other)
    {
      int num = other.minCount.CompareTo(this.minCount);
      return num != 0 ? num : other.maxCount.CompareTo(this.maxCount);
    }
  }

  public class WorldMixingOption : WorldgenMixing.MixingOption<WorldMixingSettings>
  {
    public ProcGen.World cachedWorld;
  }
}
