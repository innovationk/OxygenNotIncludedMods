// Decompiled with JetBrains decompiler
// Type: CustomGameSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.CustomSettings;
using KSerialization;
using ProcGen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/CustomGameSettings")]
public class CustomGameSettings : KMonoBehaviour
{
  private static CustomGameSettings instance;
  public const long NO_COORDINATE_RANGE = -1;
  private const int NUM_STORY_LEVELS = 3;
  public const string STORY_DISABLED_LEVEL = "Disabled";
  public const string STORY_GUARANTEED_LEVEL = "Guaranteed";
  [Serialize]
  public bool is_custom_game;
  [Serialize]
  public CustomGameSettings.CustomGameMode customGameMode;
  [Serialize]
  private Dictionary<string, string> CurrentQualityLevelsBySetting = new Dictionary<string, string>();
  [Serialize]
  private Dictionary<string, string> CurrentMixingLevelsBySetting = new Dictionary<string, string>();
  private Dictionary<string, string> currentStoryLevelsBySetting = new Dictionary<string, string>();
  public List<string> CoordinatedQualitySettings = new List<string>();
  public Dictionary<string, SettingConfig> QualitySettings = new Dictionary<string, SettingConfig>();
  public List<string> CoordinatedStorySettings = new List<string>();
  public Dictionary<string, SettingConfig> StorySettings = new Dictionary<string, SettingConfig>();
  public List<string> CoordinatedMixingSettings = new List<string>();
  public Dictionary<string, SettingConfig> MixingSettings = new Dictionary<string, SettingConfig>();
  private const string coordinatePatern = "(.*)-(\\d*)-(.*)-(.*)-(.*)";
  private string hexChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

  public static CustomGameSettings Instance => CustomGameSettings.instance;

  public IReadOnlyDictionary<string, string> CurrentStoryLevelsBySetting
  {
    get => (IReadOnlyDictionary<string, string>) this.currentStoryLevelsBySetting;
  }

  public event Action<SettingConfig, SettingLevel> OnQualitySettingChanged;

  public event Action<SettingConfig, SettingLevel> OnStorySettingChanged;

  public event Action<SettingConfig, SettingLevel> OnMixingSettingChanged;

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 6))
      this.customGameMode = this.is_custom_game ? CustomGameSettings.CustomGameMode.Custom : CustomGameSettings.CustomGameMode.Survival;
    if (this.CurrentQualityLevelsBySetting.ContainsKey("CarePackages "))
    {
      if (!this.CurrentQualityLevelsBySetting.ContainsKey(CustomGameSettingConfigs.CarePackages.id))
        this.CurrentQualityLevelsBySetting.Add(CustomGameSettingConfigs.CarePackages.id, this.CurrentQualityLevelsBySetting["CarePackages "]);
      this.CurrentQualityLevelsBySetting.Remove("CarePackages ");
    }
    this.CurrentQualityLevelsBySetting.Remove("Expansion1Active");
    string clusterDefaultName;
    this.CurrentQualityLevelsBySetting.TryGetValue(CustomGameSettingConfigs.ClusterLayout.id, out clusterDefaultName);
    if (clusterDefaultName.IsNullOrWhiteSpace())
    {
      if (!DlcManager.IsExpansion1Active())
        DebugUtil.LogWarningArgs((object) "Deserializing CustomGameSettings.ClusterLayout: ClusterLayout is blank, using default cluster instead");
      clusterDefaultName = WorldGenSettings.ClusterDefaultName;
      this.SetQualitySetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout, clusterDefaultName);
    }
    if (!SettingsCache.clusterLayouts.clusterCache.ContainsKey(clusterDefaultName))
    {
      Debug.Log((object) $"Deserializing CustomGameSettings.ClusterLayout: '{clusterDefaultName}' doesn't exist in the clusterCache, trying to rewrite path to scoped path.");
      string key = SettingsCache.GetScope("EXPANSION1_ID") + clusterDefaultName;
      if (SettingsCache.clusterLayouts.clusterCache.ContainsKey(key))
      {
        Debug.Log((object) $"Deserializing CustomGameSettings.ClusterLayout: Success in rewriting ClusterLayout '{clusterDefaultName}' to '{key}'");
        this.SetQualitySetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout, key);
      }
      else
      {
        Debug.LogWarning((object) $"Deserializing CustomGameSettings.ClusterLayout: Failed to find cluster '{clusterDefaultName}' including the scoped path, setting to default cluster name.");
        Debug.Log((object) ("ClusterCache: " + string.Join(",", (IEnumerable<string>) SettingsCache.clusterLayouts.clusterCache.Keys)));
        this.SetQualitySetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout, WorldGenSettings.ClusterDefaultName);
      }
    }
    this.CheckCustomGameMode();
  }

  private void AddMissingQualitySettings()
  {
    foreach (KeyValuePair<string, SettingConfig> qualitySetting in this.QualitySettings)
    {
      SettingConfig settingConfig = qualitySetting.Value;
      if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) settingConfig) && !this.CurrentQualityLevelsBySetting.ContainsKey(settingConfig.id))
      {
        if (settingConfig.missing_content_default != "")
        {
          DebugUtil.LogArgs((object) $"QualitySetting '{settingConfig.id}' is missing, setting it to missing_content_default '{settingConfig.missing_content_default}'.");
          this.SetQualitySetting(settingConfig, settingConfig.missing_content_default);
        }
        else
          DebugUtil.DevLogError($"QualitySetting '{settingConfig.id}' is missing in this save. Either provide a missing_content_default or handle it in OnDeserialized.");
      }
    }
  }

  protected override void OnPrefabInit()
  {
    DlcManager.IsExpansion1Active();
    Action<SettingConfig> action1 = (Action<SettingConfig>) (setting =>
    {
      this.AddQualitySettingConfig(setting);
      if (setting.coordinate_range < 0L)
        return;
      this.CoordinatedQualitySettings.Add(setting.id);
    });
    Action<SettingConfig> action2 = (Action<SettingConfig>) (setting =>
    {
      this.AddStorySettingConfig(setting);
      if (setting.coordinate_range < 0L)
        return;
      this.CoordinatedStorySettings.Add(setting.id);
    });
    Action<SettingConfig> action3 = (Action<SettingConfig>) (setting =>
    {
      this.AddMixingSettingsConfig(setting);
      if (setting.coordinate_range < 0L)
        return;
      this.CoordinatedMixingSettings.Add(setting.id);
    });
    CustomGameSettings.instance = this;
    action1((SettingConfig) CustomGameSettingConfigs.ClusterLayout);
    action1((SettingConfig) CustomGameSettingConfigs.WorldgenSeed);
    action1(CustomGameSettingConfigs.ImmuneSystem);
    action1(CustomGameSettingConfigs.CalorieBurn);
    action1(CustomGameSettingConfigs.Morale);
    action1(CustomGameSettingConfigs.Durability);
    action1(CustomGameSettingConfigs.MeteorShowers);
    action1(CustomGameSettingConfigs.Radiation);
    action1(CustomGameSettingConfigs.Stress);
    action1(CustomGameSettingConfigs.StressBreaks);
    action1(CustomGameSettingConfigs.CarePackages);
    action1(CustomGameSettingConfigs.SandboxMode);
    action1(CustomGameSettingConfigs.FastWorkersMode);
    action1(CustomGameSettingConfigs.SaveToCloud);
    action1(CustomGameSettingConfigs.Teleporters);
    action1(CustomGameSettingConfigs.BionicWattage);
    action1(CustomGameSettingConfigs.DemoliorDifficulty);
    action3(CustomMixingSettingsConfigs.DLC2Mixing);
    action3(CustomMixingSettingsConfigs.IceCavesMixing);
    action3(CustomMixingSettingsConfigs.CarrotQuarryMixing);
    action3(CustomMixingSettingsConfigs.SugarWoodsMixing);
    action3(CustomMixingSettingsConfigs.CeresAsteroidMixing);
    action3(CustomMixingSettingsConfigs.DLC3Mixing);
    action3(CustomMixingSettingsConfigs.DLC4Mixing);
    action3(CustomMixingSettingsConfigs.GardenMixing);
    action3(CustomMixingSettingsConfigs.RaptorMixing);
    action3(CustomMixingSettingsConfigs.WetlandsMixing);
    action3(CustomMixingSettingsConfigs.PrehistoricAsteroidMixing);
    action3(CustomMixingSettingsConfigs.DLC5Mixing);
    action3(CustomMixingSettingsConfigs.BeachMixing);
    action3(CustomMixingSettingsConfigs.ReefMixing);
    action3(CustomMixingSettingsConfigs.KelpForestMixing);
    action3(CustomMixingSettingsConfigs.AbyssMixing);
    action3(CustomMixingSettingsConfigs.AquaticAsteroidMixing);
    foreach (Story story in Db.Get().Stories.GetStoriesSortedByCoordinateOrder())
    {
      int num = story.kleiUseOnlyCoordinateOrder == -1 ? -1 : 3;
      string id = story.Id;
      List<SettingLevel> levels = new List<SettingLevel>();
      levels.Add(new SettingLevel("Disabled", "", ""));
      levels.Add(new SettingLevel("Guaranteed", "", "", 1L));
      long coordinate_range = (long) num;
      SettingConfig settingConfig = (SettingConfig) new ListSettingConfig(id, "", "", levels, "Disabled", "Disabled", coordinate_range, triggers_custom_game: false);
      action2(settingConfig);
    }
    foreach (KeyValuePair<string, SettingConfig> mixingSetting in this.MixingSettings)
    {
      if (mixingSetting.Value is DlcMixingSettingConfig config && DlcManager.IsContentSubscribed(config.id))
        this.SetMixingSetting((SettingConfig) config, "Enabled");
    }
    this.VerifySettingCoordinates();
  }

  public void DisableAllStories()
  {
    foreach (KeyValuePair<string, SettingConfig> storySetting in this.StorySettings)
      this.SetStorySetting(storySetting.Value, false);
  }

  public void SetSurvivalDefaults()
  {
    this.customGameMode = CustomGameSettings.CustomGameMode.Survival;
    foreach (KeyValuePair<string, SettingConfig> qualitySetting in this.QualitySettings)
      this.SetQualitySetting(qualitySetting.Value, qualitySetting.Value.GetDefaultLevelId());
  }

  public void SetNosweatDefaults()
  {
    this.customGameMode = CustomGameSettings.CustomGameMode.Nosweat;
    foreach (KeyValuePair<string, SettingConfig> qualitySetting in this.QualitySettings)
      this.SetQualitySetting(qualitySetting.Value, qualitySetting.Value.GetNoSweatDefaultLevelId());
  }

  public SettingLevel CycleQualitySettingLevel(ListSettingConfig config, int direction)
  {
    this.SetQualitySetting((SettingConfig) config, config.CycleSettingLevelID(this.CurrentQualityLevelsBySetting[config.id], direction));
    return config.GetLevel(this.CurrentQualityLevelsBySetting[config.id]);
  }

  public SettingLevel ToggleQualitySettingLevel(ToggleSettingConfig config)
  {
    this.SetQualitySetting((SettingConfig) config, config.ToggleSettingLevelID(this.CurrentQualityLevelsBySetting[config.id]));
    return config.GetLevel(this.CurrentQualityLevelsBySetting[config.id]);
  }

  private void CheckCustomGameMode()
  {
    bool flag1 = true;
    bool flag2 = true;
    foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
    {
      if (!this.QualitySettings.ContainsKey(keyValuePair.Key))
        DebugUtil.LogWarningArgs((object) ("Quality settings missing " + keyValuePair.Key));
      else if (this.QualitySettings[keyValuePair.Key].triggers_custom_game)
      {
        if (keyValuePair.Value != this.QualitySettings[keyValuePair.Key].GetDefaultLevelId())
          flag1 = false;
        if (keyValuePair.Value != this.QualitySettings[keyValuePair.Key].GetNoSweatDefaultLevelId())
          flag2 = false;
        if (!flag1)
        {
          if (!flag2)
            break;
        }
      }
    }
    CustomGameSettings.CustomGameMode customGameMode = !flag1 ? (!flag2 ? CustomGameSettings.CustomGameMode.Custom : CustomGameSettings.CustomGameMode.Nosweat) : CustomGameSettings.CustomGameMode.Survival;
    if (customGameMode == this.customGameMode)
      return;
    DebugUtil.LogArgs((object) "Game mode changed from", (object) this.customGameMode, (object) "to", (object) customGameMode);
    this.customGameMode = customGameMode;
  }

  public void SetQualitySetting(SettingConfig config, string value)
  {
    this.SetQualitySetting(config, value, true);
  }

  public void SetQualitySetting(SettingConfig config, string value, bool notify)
  {
    this.CurrentQualityLevelsBySetting[config.id] = value;
    this.CheckCustomGameMode();
    if (!notify || this.OnQualitySettingChanged == null)
      return;
    this.OnQualitySettingChanged(config, this.GetCurrentQualitySetting(config));
  }

  public SettingLevel GetCurrentQualitySetting(SettingConfig setting)
  {
    return this.GetCurrentQualitySetting(setting.id);
  }

  public SettingLevel GetCurrentQualitySetting(string setting_id)
  {
    SettingConfig qualitySetting = this.QualitySettings[setting_id];
    if (this.customGameMode == CustomGameSettings.CustomGameMode.Survival && qualitySetting.triggers_custom_game)
      return qualitySetting.GetLevel(qualitySetting.GetDefaultLevelId());
    if (this.customGameMode == CustomGameSettings.CustomGameMode.Nosweat && qualitySetting.triggers_custom_game)
      return qualitySetting.GetLevel(qualitySetting.GetNoSweatDefaultLevelId());
    if (!this.CurrentQualityLevelsBySetting.ContainsKey(setting_id))
      this.CurrentQualityLevelsBySetting[setting_id] = this.QualitySettings[setting_id].GetDefaultLevelId();
    string level_id = DlcManager.IsAllContentSubscribed(qualitySetting.required_content) ? this.CurrentQualityLevelsBySetting[setting_id] : qualitySetting.GetDefaultLevelId();
    return this.QualitySettings[setting_id].GetLevel(level_id);
  }

  public string GetCurrentQualitySettingLevelId(SettingConfig config)
  {
    return this.CurrentQualityLevelsBySetting[config.id];
  }

  public string GetSettingLevelLabel(string setting_id, string level_id)
  {
    SettingConfig qualitySetting = this.QualitySettings[setting_id];
    if (qualitySetting != null)
    {
      SettingLevel level = qualitySetting.GetLevel(level_id);
      if (level != null)
        return level.label;
    }
    Debug.LogWarning((object) $"No label string for setting: {setting_id} level: {level_id}");
    return "";
  }

  public string GetQualitySettingLevelTooltip(string setting_id, string level_id)
  {
    SettingConfig qualitySetting = this.QualitySettings[setting_id];
    if (qualitySetting != null)
    {
      SettingLevel level = qualitySetting.GetLevel(level_id);
      if (level != null)
        return level.tooltip;
    }
    Debug.LogWarning((object) $"No tooltip string for setting: {setting_id} level: {level_id}");
    return "";
  }

  public void AddQualitySettingConfig(SettingConfig config)
  {
    this.QualitySettings.Add(config.id, config);
    if (this.CurrentQualityLevelsBySetting.ContainsKey(config.id) && !string.IsNullOrEmpty(this.CurrentQualityLevelsBySetting[config.id]))
      return;
    this.CurrentQualityLevelsBySetting[config.id] = config.GetDefaultLevelId();
  }

  public void AddStorySettingConfig(SettingConfig config)
  {
    this.StorySettings.Add(config.id, config);
    if (this.currentStoryLevelsBySetting.ContainsKey(config.id) && !string.IsNullOrEmpty(this.currentStoryLevelsBySetting[config.id]))
      return;
    this.currentStoryLevelsBySetting[config.id] = config.GetDefaultLevelId();
  }

  public void SetStorySetting(SettingConfig config, string value)
  {
    this.SetStorySetting(config, value == "Guaranteed");
  }

  public void SetStorySetting(SettingConfig config, bool value)
  {
    this.currentStoryLevelsBySetting[config.id] = value ? "Guaranteed" : "Disabled";
    if (this.OnStorySettingChanged == null)
      return;
    this.OnStorySettingChanged(config, this.GetCurrentStoryTraitSetting(config));
  }

  public void ParseAndApplyStoryTraitSettingsCode(string code)
  {
    BigInteger bigInteger = this.Base36toBinary(code);
    Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
    foreach (string key in Util.Reverse((IList) this.CoordinatedStorySettings))
    {
      SettingConfig storySetting = this.StorySettings[key];
      if (storySetting.coordinate_range != -1L)
      {
        long num = (long) (bigInteger % (BigInteger) storySetting.coordinate_range);
        bigInteger /= (BigInteger) storySetting.coordinate_range;
        foreach (SettingLevel level in storySetting.GetLevels())
        {
          if (level.coordinate_value == num)
          {
            dictionary[storySetting] = level.id;
            break;
          }
        }
      }
    }
    foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
      this.SetStorySetting(keyValuePair.Key, keyValuePair.Value);
  }

  private string GetStoryTraitSettingsCode()
  {
    BigInteger input = (BigInteger) 0;
    foreach (string coordinatedStorySetting in this.CoordinatedStorySettings)
    {
      SettingConfig storySetting = this.StorySettings[coordinatedStorySetting];
      input *= (BigInteger) storySetting.coordinate_range;
      input += (BigInteger) storySetting.GetLevel(this.currentStoryLevelsBySetting[coordinatedStorySetting]).coordinate_value;
    }
    return this.BinarytoBase36(input);
  }

  public SettingLevel GetCurrentStoryTraitSetting(SettingConfig setting)
  {
    return this.GetCurrentStoryTraitSetting(setting.id);
  }

  public SettingLevel GetCurrentStoryTraitSetting(string settingId)
  {
    SettingConfig storySetting = this.StorySettings[settingId];
    if (this.customGameMode == CustomGameSettings.CustomGameMode.Survival && storySetting.triggers_custom_game)
      return storySetting.GetLevel(storySetting.GetDefaultLevelId());
    if (this.customGameMode == CustomGameSettings.CustomGameMode.Nosweat && storySetting.triggers_custom_game)
      return storySetting.GetLevel(storySetting.GetNoSweatDefaultLevelId());
    if (!this.currentStoryLevelsBySetting.ContainsKey(settingId))
      this.currentStoryLevelsBySetting[settingId] = this.StorySettings[settingId].GetDefaultLevelId();
    string level_id = DlcManager.IsAllContentSubscribed(storySetting.required_content) ? this.currentStoryLevelsBySetting[settingId] : storySetting.GetDefaultLevelId();
    return this.StorySettings[settingId].GetLevel(level_id);
  }

  public List<string> GetCurrentStories()
  {
    List<string> currentStories = new List<string>();
    foreach (KeyValuePair<string, string> keyValuePair in this.currentStoryLevelsBySetting)
    {
      if (this.IsStoryActive(keyValuePair.Key, keyValuePair.Value))
        currentStories.Add(keyValuePair.Key);
    }
    return currentStories;
  }

  public bool IsStoryActive(string id, string level)
  {
    SettingConfig settingConfig;
    return this.StorySettings.TryGetValue(id, out settingConfig) && settingConfig != null && level == "Guaranteed";
  }

  public void SetMixingSetting(SettingConfig config, string value)
  {
    this.SetMixingSetting(config, value, true);
  }

  public void SetMixingSetting(SettingConfig config, string value, bool notify)
  {
    this.CurrentMixingLevelsBySetting[config.id] = value;
    if (!notify || this.OnMixingSettingChanged == null)
      return;
    this.OnMixingSettingChanged(config, this.GetCurrentMixingSettingLevel(config));
  }

  public void AddMixingSettingsConfig(SettingConfig config)
  {
    this.MixingSettings.Add(config.id, config);
    if (this.CurrentMixingLevelsBySetting.ContainsKey(config.id) && !string.IsNullOrEmpty(this.CurrentMixingLevelsBySetting[config.id]))
      return;
    this.CurrentMixingLevelsBySetting[config.id] = config.GetDefaultLevelId();
  }

  public SettingLevel GetCurrentMixingSettingLevel(SettingConfig setting)
  {
    return this.GetCurrentMixingSettingLevel(setting.id);
  }

  public SettingConfig GetWorldMixingSettingForWorldgenFile(string file)
  {
    foreach (KeyValuePair<string, SettingConfig> mixingSetting in this.MixingSettings)
    {
      if (mixingSetting.Value is WorldMixingSettingConfig mixingSettingConfig && mixingSettingConfig.worldgenPath == file)
        return mixingSetting.Value;
    }
    return (SettingConfig) null;
  }

  public SettingConfig GetSubworldMixingSettingForWorldgenFile(string file)
  {
    foreach (KeyValuePair<string, SettingConfig> mixingSetting in this.MixingSettings)
    {
      if (mixingSetting.Value is SubworldMixingSettingConfig mixingSettingConfig && mixingSettingConfig.worldgenPath == file)
        return mixingSetting.Value;
    }
    return (SettingConfig) null;
  }

  public void DisableAllMixing()
  {
    foreach (SettingConfig config in this.MixingSettings.Values)
      this.SetMixingSetting(config, config.GetDefaultLevelId());
  }

  public List<SubworldMixingSettingConfig> GetActiveSubworldMixingSettings()
  {
    List<SubworldMixingSettingConfig> subworldMixingSettings = new List<SubworldMixingSettingConfig>();
    foreach (SettingConfig setting in this.MixingSettings.Values)
    {
      if (setting is SubworldMixingSettingConfig mixingSettingConfig && this.GetCurrentMixingSettingLevel(setting).id != "Disabled")
        subworldMixingSettings.Add(mixingSettingConfig);
    }
    return subworldMixingSettings;
  }

  public List<WorldMixingSettingConfig> GetActiveWorldMixingSettings()
  {
    List<WorldMixingSettingConfig> worldMixingSettings = new List<WorldMixingSettingConfig>();
    foreach (SettingConfig setting in this.MixingSettings.Values)
    {
      if (setting is WorldMixingSettingConfig mixingSettingConfig && this.GetCurrentMixingSettingLevel(setting).id != "Disabled")
        worldMixingSettings.Add(mixingSettingConfig);
    }
    return worldMixingSettings;
  }

  public SettingLevel CycleMixingSettingLevel(ListSettingConfig config, int direction)
  {
    this.SetMixingSetting((SettingConfig) config, config.CycleSettingLevelID(this.CurrentMixingLevelsBySetting[config.id], direction));
    return config.GetLevel(this.CurrentMixingLevelsBySetting[config.id]);
  }

  public SettingLevel ToggleMixingSettingLevel(ToggleSettingConfig config)
  {
    this.SetMixingSetting((SettingConfig) config, config.ToggleSettingLevelID(this.CurrentMixingLevelsBySetting[config.id]));
    return config.GetLevel(this.CurrentMixingLevelsBySetting[config.id]);
  }

  public SettingLevel GetCurrentMixingSettingLevel(string settingId)
  {
    SettingConfig mixingSetting = this.MixingSettings[settingId];
    if (!this.CurrentMixingLevelsBySetting.ContainsKey(settingId))
      this.CurrentMixingLevelsBySetting[settingId] = this.MixingSettings[settingId].GetDefaultLevelId();
    string level_id = DlcManager.IsAllContentSubscribed(mixingSetting.required_content) ? this.CurrentMixingLevelsBySetting[settingId] : mixingSetting.GetDefaultLevelId();
    return this.MixingSettings[settingId].GetLevel(level_id);
  }

  public List<string> GetCurrentDlcMixingIds()
  {
    List<string> currentDlcMixingIds = new List<string>();
    foreach (KeyValuePair<string, SettingConfig> mixingSetting in this.MixingSettings)
    {
      if (mixingSetting.Value is DlcMixingSettingConfig mixingSettingConfig && mixingSettingConfig.IsOnLevel(this.GetCurrentMixingSettingLevel(mixingSettingConfig.id).id))
        currentDlcMixingIds.Add(mixingSettingConfig.id);
    }
    return currentDlcMixingIds;
  }

  public void ParseAndApplyMixingSettingsCode(string code)
  {
    BigInteger bigInteger = this.Base36toBinary(code);
    Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
    foreach (string key in Util.Reverse((IList) this.CoordinatedMixingSettings))
    {
      SettingConfig mixingSetting = this.MixingSettings[key];
      if (mixingSetting.coordinate_range != -1L)
      {
        long num = (long) (bigInteger % (BigInteger) mixingSetting.coordinate_range);
        bigInteger /= (BigInteger) mixingSetting.coordinate_range;
        foreach (SettingLevel level in mixingSetting.GetLevels())
        {
          if (level.coordinate_value == num)
          {
            dictionary[mixingSetting] = level.id;
            break;
          }
        }
      }
    }
    foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
      this.SetMixingSetting(keyValuePair.Key, keyValuePair.Value);
  }

  private string GetMixingSettingsCode()
  {
    BigInteger input = (BigInteger) 0;
    foreach (string coordinatedMixingSetting in this.CoordinatedMixingSettings)
    {
      SettingConfig mixingSetting = this.MixingSettings[coordinatedMixingSetting];
      input *= (BigInteger) mixingSetting.coordinate_range;
      input += (BigInteger) mixingSetting.GetLevel(this.GetCurrentMixingSettingLevel(mixingSetting).id).coordinate_value;
    }
    return this.BinarytoBase36(input);
  }

  public void RemoveInvalidMixingSettings()
  {
    ClusterLayout currentClusterLayout = this.GetCurrentClusterLayout();
    foreach (KeyValuePair<string, SettingConfig> mixingSetting in this.MixingSettings)
    {
      if (mixingSetting.Value is DlcMixingSettingConfig mixingSettingConfig && ((IEnumerable<string>) currentClusterLayout.requiredDlcIds).Contains<string>(mixingSettingConfig.id))
        this.SetMixingSetting(mixingSetting.Value, "Disabled");
    }
    List<string> availableDlcs = this.GetCurrentDlcMixingIds();
    availableDlcs.AddRange((IEnumerable<string>) currentClusterLayout.requiredDlcIds);
    foreach (KeyValuePair<string, SettingConfig> mixingSetting in this.MixingSettings)
    {
      SettingConfig settingConfig = mixingSetting.Value;
      if (!(settingConfig is WorldMixingSettingConfig mixingSettingConfig2))
      {
        if (settingConfig is SubworldMixingSettingConfig mixingSettingConfig1 && (!HasRequiredContent(mixingSettingConfig1.required_content) || currentClusterLayout.HasAnyTags(mixingSettingConfig1.forbiddenClusterTags)))
          this.SetMixingSetting(mixingSetting.Value, "Disabled");
      }
      else if (!HasRequiredContent(mixingSettingConfig2.required_content) || currentClusterLayout.HasAnyTags(mixingSettingConfig2.forbiddenClusterTags))
        this.SetMixingSetting(mixingSetting.Value, "Disabled");
    }

    bool HasRequiredContent(string[] requiredContent)
    {
      foreach (string str in requiredContent)
      {
        if (!(str == "") && !availableDlcs.Contains(str))
          return false;
      }
      return true;
    }
  }

  public ClusterLayout GetCurrentClusterLayout()
  {
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout);
    return currentQualitySetting == null ? (ClusterLayout) null : SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
  }

  public int GetCurrentWorldgenSeed()
  {
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.WorldgenSeed);
    return currentQualitySetting == null ? 0 : int.Parse(currentQualitySetting.id);
  }

  public void LoadClusters()
  {
    Dictionary<string, ClusterLayout> clusterCache = SettingsCache.clusterLayouts.clusterCache;
    List<SettingLevel> levels = new List<SettingLevel>(clusterCache.Count);
    foreach (KeyValuePair<string, ClusterLayout> keyValuePair in clusterCache)
    {
      StringEntry result;
      string label = Strings.TryGet(new StringKey(keyValuePair.Value.name), out result) ? result.ToString() : keyValuePair.Value.name;
      string tooltip = Strings.TryGet(new StringKey(keyValuePair.Value.description), out result) ? result.ToString() : keyValuePair.Value.description;
      levels.Add(new SettingLevel(keyValuePair.Key, label, tooltip));
    }
    CustomGameSettingConfigs.ClusterLayout.StompLevels(levels, WorldGenSettings.ClusterDefaultName, WorldGenSettings.ClusterDefaultName);
  }

  public void Print()
  {
    string str1 = "Custom Settings: ";
    foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
      str1 = $"{str1}{keyValuePair.Key}={keyValuePair.Value},";
    Debug.Log((object) str1);
    string str2 = "Story Settings: ";
    foreach (KeyValuePair<string, string> keyValuePair in this.currentStoryLevelsBySetting)
      str2 = $"{str2}{keyValuePair.Key}={keyValuePair.Value},";
    Debug.Log((object) str2);
    string str3 = "Mixing Settings: ";
    foreach (KeyValuePair<string, string> keyValuePair in this.CurrentMixingLevelsBySetting)
      str3 = $"{str3}{keyValuePair.Key}={keyValuePair.Value},";
    Debug.Log((object) str3);
  }

  private bool AllValuesMatch(
    Dictionary<string, string> data,
    CustomGameSettings.CustomGameMode mode)
  {
    bool flag = true;
    foreach (KeyValuePair<string, SettingConfig> qualitySetting in this.QualitySettings)
    {
      if (!(qualitySetting.Key == CustomGameSettingConfigs.WorldgenSeed.id))
      {
        string str = (string) null;
        switch (mode)
        {
          case CustomGameSettings.CustomGameMode.Survival:
            str = qualitySetting.Value.GetDefaultLevelId();
            break;
          case CustomGameSettings.CustomGameMode.Nosweat:
            str = qualitySetting.Value.GetNoSweatDefaultLevelId();
            break;
        }
        if (data.ContainsKey(qualitySetting.Key) && data[qualitySetting.Key] != str)
        {
          flag = false;
          break;
        }
      }
    }
    return flag;
  }

  public List<CustomGameSettings.MetricSettingsData> GetSettingsForMetrics()
  {
    List<CustomGameSettings.MetricSettingsData> settingsForMetrics = new List<CustomGameSettings.MetricSettingsData>();
    settingsForMetrics.Add(new CustomGameSettings.MetricSettingsData()
    {
      Name = "CustomGameMode",
      Value = this.customGameMode.ToString()
    });
    foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
      settingsForMetrics.Add(new CustomGameSettings.MetricSettingsData()
      {
        Name = keyValuePair.Key,
        Value = keyValuePair.Value
      });
    CustomGameSettings.MetricSettingsData metricSettingsData = new CustomGameSettings.MetricSettingsData()
    {
      Name = "CustomGameModeActual",
      Value = CustomGameSettings.CustomGameMode.Custom.ToString()
    };
    foreach (CustomGameSettings.CustomGameMode mode in Enum.GetValues(typeof (CustomGameSettings.CustomGameMode)))
    {
      if (mode != CustomGameSettings.CustomGameMode.Custom && this.AllValuesMatch(this.CurrentQualityLevelsBySetting, mode))
      {
        metricSettingsData.Value = mode.ToString();
        break;
      }
    }
    settingsForMetrics.Add(metricSettingsData);
    return settingsForMetrics;
  }

  public List<CustomGameSettings.MetricSettingsData> GetSettingsForMixingMetrics()
  {
    List<CustomGameSettings.MetricSettingsData> forMixingMetrics = new List<CustomGameSettings.MetricSettingsData>();
    foreach (KeyValuePair<string, string> keyValuePair in this.CurrentMixingLevelsBySetting)
    {
      SettingConfig settingConfig;
      if (this.MixingSettings.TryGetValue(keyValuePair.Key, out settingConfig) && DlcManager.IsAllContentSubscribed(settingConfig.required_content))
        forMixingMetrics.Add(new CustomGameSettings.MetricSettingsData()
        {
          Name = keyValuePair.Key,
          Value = keyValuePair.Value
        });
    }
    return forMixingMetrics;
  }

  public bool VerifySettingCoordinates()
  {
    return this.VerifySettingsDictionary(this.QualitySettings) | this.VerifySettingsDictionary(this.StorySettings);
  }

  private bool VerifySettingsDictionary(Dictionary<string, SettingConfig> configs)
  {
    bool flag1 = false;
    foreach (KeyValuePair<string, SettingConfig> config in configs)
    {
      if (config.Value.coordinate_range >= 0L)
      {
        List<SettingLevel> levels = config.Value.GetLevels();
        if (config.Value.coordinate_range < (long) levels.Count)
        {
          flag1 = true;
          Debug.Assert(false, (object) $"{config.Value.id}: Range between coordinate min and max insufficient for all levels ({config.Value.coordinate_range.ToString()}<{levels.Count.ToString()})");
        }
        foreach (SettingLevel settingLevel in levels)
        {
          Dictionary<long, string> dictionary = new Dictionary<long, string>();
          string str1 = $"{config.Value.id} > {settingLevel.id}";
          if (config.Value.coordinate_range <= settingLevel.coordinate_value)
          {
            flag1 = true;
            Debug.Assert(false, (object) string.Format("%s: Level coordinate value (%u) exceedes range (%u)", (object) str1, (object) settingLevel.coordinate_value, (object) config.Value.coordinate_range));
          }
          if (settingLevel.coordinate_value < 0L)
          {
            flag1 = true;
            Debug.Assert(false, (object) (str1 + ": Level coordinate value must be >= 0"));
          }
          else if (settingLevel.coordinate_value == 0L)
          {
            if (settingLevel.id != config.Value.GetDefaultLevelId())
            {
              flag1 = true;
              Debug.Assert(false, (object) (str1 + ": Only the default level should have a coordinate value of 0"));
            }
          }
          else
          {
            string str2;
            bool flag2 = !dictionary.TryGetValue(settingLevel.coordinate_value, out str2);
            dictionary[settingLevel.coordinate_value] = str1;
            if (settingLevel.id == config.Value.GetDefaultLevelId())
            {
              flag1 = true;
              Debug.Assert(false, (object) (str1 + ": Default level must be a coordinate value of 0"));
            }
            if (!flag2)
            {
              flag1 = true;
              Debug.Assert(false, (object) $"{str1}: Combined coordinate conflicts with another coordinate ({str2}). Ensure this SettingConfig's min and max don't overlap with another SettingConfig's");
            }
          }
        }
      }
    }
    return flag1;
  }

  public static string[] ParseSettingCoordinate(string coord)
  {
    System.Text.RegularExpressions.Match match = new Regex("(.*)-(\\d*)-(.*)-(.*)-(.*)").Match(coord);
    for (int index = 1; index <= 2; ++index)
    {
      if (match.Groups.Count == 1)
        match = new Regex("(.*)-(\\d*)-(.*)-(.*)-(.*)".Remove("(.*)-(\\d*)-(.*)-(.*)-(.*)".Length - index * 5)).Match(coord);
    }
    string[] settingCoordinate = new string[match.Groups.Count];
    for (int groupnum = 0; groupnum < match.Groups.Count; ++groupnum)
      settingCoordinate[groupnum] = match.Groups[groupnum].Value;
    return settingCoordinate;
  }

  public string GetSettingsCoordinate()
  {
    SettingLevel currentQualitySetting1 = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout);
    if (currentQualitySetting1 == null)
    {
      DebugUtil.DevLogError("GetSettingsCoordinate: clusterLayoutSetting is null, returning '0' coordinate");
      CustomGameSettings.Instance.Print();
      Debug.Log((object) ("ClusterCache: " + string.Join(",", (IEnumerable<string>) SettingsCache.clusterLayouts.clusterCache.Keys)));
      return "0-0-0-0-0";
    }
    ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting1.id);
    SettingLevel currentQualitySetting2 = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.WorldgenSeed);
    string otherSettingsCode = this.GetOtherSettingsCode();
    string traitSettingsCode = this.GetStoryTraitSettingsCode();
    string mixingSettingsCode = this.GetMixingSettingsCode();
    return $"{clusterData.GetCoordinatePrefix()}-{currentQualitySetting2.id}-{otherSettingsCode}-{traitSettingsCode}-{mixingSettingsCode}";
  }

  public void ParseAndApplySettingsCode(string code)
  {
    BigInteger bigInteger = this.Base36toBinary(code);
    Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
    foreach (string key in Util.Reverse((IList) this.CoordinatedQualitySettings))
    {
      if (this.QualitySettings.ContainsKey(key))
      {
        SettingConfig qualitySetting = this.QualitySettings[key];
        if (qualitySetting.coordinate_range != -1L)
        {
          long num = (long) (bigInteger % (BigInteger) qualitySetting.coordinate_range);
          bigInteger /= (BigInteger) qualitySetting.coordinate_range;
          foreach (SettingLevel level in qualitySetting.GetLevels())
          {
            if (level.coordinate_value == num)
            {
              dictionary[qualitySetting] = level.id;
              break;
            }
          }
        }
      }
    }
    foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
      this.SetQualitySetting(keyValuePair.Key, keyValuePair.Value);
  }

  private string GetOtherSettingsCode()
  {
    BigInteger input = (BigInteger) 0;
    foreach (string coordinatedQualitySetting in this.CoordinatedQualitySettings)
    {
      SettingConfig qualitySetting = this.QualitySettings[coordinatedQualitySetting];
      input *= (BigInteger) qualitySetting.coordinate_range;
      input += (BigInteger) qualitySetting.GetLevel(this.GetCurrentQualitySetting(coordinatedQualitySetting).id).coordinate_value;
    }
    return this.BinarytoBase36(input);
  }

  private BigInteger Base36toBinary(string input)
  {
    if (input == "0")
      return (BigInteger) 0;
    BigInteger input1 = (BigInteger) 0;
    for (int index = input.Length - 1; index >= 0; --index)
      input1 = input1 * (BigInteger) 36 + (BigInteger) (long) this.hexChars.IndexOf(input[index]);
    DebugUtil.LogArgs((object) "tried converting", (object) input, (object) ", got", (object) input1, (object) "and returns to", (object) this.BinarytoBase36(input1));
    return input1;
  }

  private string BinarytoBase36(BigInteger input)
  {
    if (input == 0L)
      return "0";
    BigInteger bigInteger = input;
    string str = "";
    for (; bigInteger > 0L; bigInteger /= (BigInteger) 36)
      str += this.hexChars[(int) (bigInteger % (BigInteger) 36)].ToString();
    return str;
  }

  public enum CustomGameMode
  {
    Survival = 0,
    Nosweat = 1,
    Custom = 255, // 0x000000FF
  }

  public struct MetricSettingsData
  {
    public string Name;
    public string Value;
  }
}
