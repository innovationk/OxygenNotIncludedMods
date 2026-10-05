// Decompiled with JetBrains decompiler
// Type: SaveGame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using KSerialization;
using Newtonsoft.Json;
using ProcGen;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

#nullable disable
[SerializationConfig(KSerialization.MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SaveGame")]
public class SaveGame : KMonoBehaviour, ISaveLoadable
{
  [Serialize]
  private int speed;
  [Serialize]
  public List<Tag> expandedResourceTags = new List<Tag>();
  [Serialize]
  public int minGermCountForDisinfect = 10000;
  [Serialize]
  public bool enableAutoDisinfect = true;
  [Serialize]
  public bool sandboxEnabled;
  [Serialize]
  public float relativeTemperatureOverlaySliderValue = 294.15f;
  [Serialize]
  private int autoSaveCycleInterval = 1;
  [Serialize]
  private Vector2I timelapseResolution = new Vector2I(512 /*0x0200*/, 768 /*0x0300*/);
  private string baseName;
  public static SaveGame Instance;
  private ColonyAchievementTracker colonyAchievementTracker;
  public EntombedItemManager entombedItemManager;
  public WorldGenSpawner worldGenSpawner;
  [MyCmpReq]
  public MaterialSelectorSerializer materialSelectorSerializer;
  private static bool debug_SaveFileHeaderBlank_sent;

  public int AutoSaveCycleInterval
  {
    get => this.autoSaveCycleInterval;
    set => this.autoSaveCycleInterval = value;
  }

  public Vector2I TimelapseResolution
  {
    get => this.timelapseResolution;
    set => this.timelapseResolution = value;
  }

  public string BaseName => this.baseName;

  public static void DestroyInstance() => SaveGame.Instance = (SaveGame) null;

  public ColonyAchievementTracker ColonyAchievementTracker
  {
    get
    {
      if ((UnityEngine.Object) this.colonyAchievementTracker == (UnityEngine.Object) null)
        this.colonyAchievementTracker = this.GetComponent<ColonyAchievementTracker>();
      return this.colonyAchievementTracker;
    }
  }

  protected override void OnPrefabInit()
  {
    SaveGame.Instance = this;
    new ColonyRationMonitor.Instance((IStateMachineTarget) this).StartSM();
    this.entombedItemManager = this.gameObject.AddComponent<EntombedItemManager>();
    this.worldGenSpawner = this.gameObject.AddComponent<WorldGenSpawner>();
    this.gameObject.AddOrGetDef<GameplaySeasonManager.Def>();
    this.gameObject.AddOrGetDef<ClusterFogOfWarManager.Def>();
  }

  [OnSerializing]
  private void OnSerialize() => this.speed = SpeedControlScreen.Instance.GetSpeed();

  [OnDeserializing]
  private void OnDeserialize() => this.baseName = SaveLoader.Instance.GameInfo.baseName;

  public int GetSpeed() => this.speed;

  public byte[] GetSaveHeader(bool isAutoSave, bool isCompressed, out SaveGame.Header header)
  {
    string originalSaveFileName = SaveLoader.GetOriginalSaveFileName(SaveLoader.GetActiveSaveFilePath());
    byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject((object) new SaveGame.GameInfo(GameClock.Instance.GetCycle(), Components.LiveMinionIdentities.Count, this.baseName, isAutoSave, originalSaveFileName, SaveLoader.Instance.GameInfo.clusterId, SaveLoader.Instance.GameInfo.worldTraits, SaveLoader.Instance.GameInfo.colonyGuid, SaveLoader.Instance.GameInfo.dlcIds, this.sandboxEnabled)));
    header = new SaveGame.Header();
    header.buildVersion = 744825U;
    header.headerSize = bytes.Length;
    header.headerVersion = 1U;
    header.compression = isCompressed ? 1 : 0;
    return bytes;
  }

  public static string GetSaveUniqueID(SaveGame.GameInfo info)
  {
    return !(info.colonyGuid != Guid.Empty) ? $"{info.baseName}/{info.clusterId}" : info.colonyGuid.ToString();
  }

  public static Tuple<SaveGame.Header, SaveGame.GameInfo> GetFileInfo(string filename)
  {
    try
    {
      SaveGame.Header header;
      SaveGame.GameInfo b = SaveLoader.LoadHeader(filename, out header);
      if (b.saveMajorVersion >= 7)
        return new Tuple<SaveGame.Header, SaveGame.GameInfo>(header, b);
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) ("Exception while loading " + filename));
      Debug.LogWarning((object) ex);
    }
    return (Tuple<SaveGame.Header, SaveGame.GameInfo>) null;
  }

  public static SaveGame.GameInfo GetHeader(
    IReader br,
    out SaveGame.Header header,
    string debugFileName)
  {
    header = new SaveGame.Header();
    header.buildVersion = br.ReadUInt32();
    header.headerSize = br.ReadInt32();
    header.headerVersion = br.ReadUInt32();
    if (1U <= header.headerVersion)
      header.compression = br.ReadInt32();
    byte[] data = br.ReadBytes(header.headerSize);
    if (header.headerSize == 0 && !SaveGame.debug_SaveFileHeaderBlank_sent)
    {
      SaveGame.debug_SaveFileHeaderBlank_sent = true;
      Debug.LogWarning((object) ("SaveFileHeaderBlank - " + debugFileName));
    }
    SaveGame.GameInfo gameInfo = SaveGame.GetGameInfo(data);
    if (gameInfo.IsVersionOlderThan(7, 14) && gameInfo.worldTraits != null)
    {
      string[] worldTraits = gameInfo.worldTraits;
      for (int index = 0; index < worldTraits.Length; ++index)
        worldTraits[index] = worldTraits[index].Replace('\\', '/');
    }
    if (gameInfo.IsVersionOlderThan(7, 20))
      gameInfo.dlcId = "";
    if (gameInfo.IsVersionOlderThan(7, 34))
      gameInfo.dlcIds = new List<string>()
      {
        gameInfo.dlcId
      };
    return gameInfo;
  }

  public static SaveGame.GameInfo GetGameInfo(byte[] data)
  {
    return JsonConvert.DeserializeObject<SaveGame.GameInfo>(Encoding.UTF8.GetString(data));
  }

  public void SetBaseName(string newBaseName)
  {
    if (string.IsNullOrEmpty(newBaseName))
      Debug.LogWarning((object) "Cannot give the base an empty name");
    else
      this.baseName = newBaseName;
  }

  protected override void OnSpawn()
  {
    ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
    Game.Instance.Trigger(-1917495436, (object) null);
  }

  public List<Tuple<string, TextStyleSetting>> GetColonyToolTip()
  {
    List<Tuple<string, TextStyleSetting>> colonyToolTip = new List<Tuple<string, TextStyleSetting>>();
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout);
    ClusterLayout clusterLayout;
    SettingsCache.clusterLayouts.clusterCache.TryGetValue(currentQualitySetting.id, out clusterLayout);
    colonyToolTip.Add(new Tuple<string, TextStyleSetting>(this.baseName, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
    if (DlcManager.IsExpansion1Active())
    {
      StringEntry a = Strings.Get(clusterLayout.name);
      colonyToolTip.Add(new Tuple<string, TextStyleSetting>((string) a, ToolTipScreen.Instance.defaultTooltipBodyStyle));
    }
    if ((UnityEngine.Object) GameClock.Instance != (UnityEngine.Object) null)
    {
      colonyToolTip.Add(new Tuple<string, TextStyleSetting>(" ", (TextStyleSetting) null));
      colonyToolTip.Add(new Tuple<string, TextStyleSetting>(string.Format((string) UI.ASTEROIDCLOCK.CYCLES_OLD, (object) GameUtil.GetCurrentCycle()), ToolTipScreen.Instance.defaultTooltipHeaderStyle));
      colonyToolTip.Add(new Tuple<string, TextStyleSetting>(string.Format((string) UI.ASTEROIDCLOCK.TIME_PLAYED, (object) (GameClock.Instance.GetTimePlayedInSeconds() / 3600f).ToString("0.00")), ToolTipScreen.Instance.defaultTooltipBodyStyle));
    }
    int cameraActiveCluster = CameraController.Instance.cameraActiveCluster;
    WorldContainer world = ClusterManager.Instance.GetWorld(cameraActiveCluster);
    colonyToolTip.Add(new Tuple<string, TextStyleSetting>(" ", (TextStyleSetting) null));
    if (DlcManager.IsExpansion1Active())
    {
      colonyToolTip.Add(new Tuple<string, TextStyleSetting>(world.GetComponent<ClusterGridEntity>().Name, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
    }
    else
    {
      StringEntry a = Strings.Get(clusterLayout.name);
      colonyToolTip.Add(new Tuple<string, TextStyleSetting>((string) a, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
    }
    if (SaveLoader.Instance.GameInfo.worldTraits != null && SaveLoader.Instance.GameInfo.worldTraits.Length != 0)
    {
      foreach (string worldTrait in SaveLoader.Instance.GameInfo.worldTraits)
      {
        WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(worldTrait, false);
        if (cachedWorldTrait != null)
          colonyToolTip.Add(new Tuple<string, TextStyleSetting>((string) Strings.Get(cachedWorldTrait.name), ToolTipScreen.Instance.defaultTooltipBodyStyle));
        else
          colonyToolTip.Add(new Tuple<string, TextStyleSetting>((string) WORLD_TRAITS.MISSING_TRAIT, ToolTipScreen.Instance.defaultTooltipBodyStyle));
      }
    }
    else if (world.WorldTraitIds != null)
    {
      foreach (string worldTraitId in world.WorldTraitIds)
      {
        WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(worldTraitId, false);
        if (cachedWorldTrait != null)
          colonyToolTip.Add(new Tuple<string, TextStyleSetting>((string) Strings.Get(cachedWorldTrait.name), ToolTipScreen.Instance.defaultTooltipBodyStyle));
        else
          colonyToolTip.Add(new Tuple<string, TextStyleSetting>((string) WORLD_TRAITS.MISSING_TRAIT, ToolTipScreen.Instance.defaultTooltipBodyStyle));
      }
      if (world.WorldTraitIds.Count == 0)
        colonyToolTip.Add(new Tuple<string, TextStyleSetting>((string) WORLD_TRAITS.NO_TRAITS.NAME_SHORTHAND, ToolTipScreen.Instance.defaultTooltipBodyStyle));
    }
    return colonyToolTip;
  }

  public struct Header
  {
    public uint buildVersion;
    public int headerSize;
    public uint headerVersion;
    public int compression;

    public bool IsCompressed => this.compression != 0;
  }

  public struct GameInfo(
    int numberOfCycles,
    int numberOfDuplicants,
    string baseName,
    bool isAutoSave,
    string originalSaveName,
    string clusterId,
    string[] worldTraits,
    Guid colonyGuid,
    List<string> dlcIds,
    bool sandboxEnabled = false)
  {
    public int numberOfCycles = numberOfCycles;
    public int numberOfDuplicants = numberOfDuplicants;
    public string baseName = baseName;
    public bool isAutoSave = isAutoSave;
    public string originalSaveName = originalSaveName;
    public int saveMajorVersion = 7;
    public int saveMinorVersion = 38;
    public string clusterId = clusterId;
    public string[] worldTraits = worldTraits;
    public bool sandboxEnabled = sandboxEnabled;
    public Guid colonyGuid = colonyGuid;
    [Obsolete("Please use dlcIds instead.")]
    public string dlcId = (string) null;
    public List<string> dlcIds = dlcIds;

    public bool IsVersionOlderThan(int major, int minor)
    {
      if (this.saveMajorVersion < major)
        return true;
      return this.saveMajorVersion == major && this.saveMinorVersion < minor;
    }

    public bool IsVersionExactly(int major, int minor)
    {
      return this.saveMajorVersion == major && this.saveMinorVersion == minor;
    }

    public bool IsCompatableWithCurrentDlcConfiguration(
      out HashSet<string> dlcIdsToEnable,
      out HashSet<string> dlcIdToDisable)
    {
      dlcIdsToEnable = new HashSet<string>();
      foreach (string dlcId in this.dlcIds)
      {
        if (DlcManager.IsUnknownDlc(dlcId) || !DlcManager.IsContentSubscribed(dlcId))
          dlcIdsToEnable.Add(dlcId);
      }
      dlcIdToDisable = new HashSet<string>();
      if (!this.dlcIds.Contains("EXPANSION1_ID") && DlcManager.IsExpansion1Active())
        dlcIdToDisable.Add("EXPANSION1_ID");
      return dlcIdsToEnable.Count == 0 && dlcIdToDisable.Count == 0;
    }
  }
}
