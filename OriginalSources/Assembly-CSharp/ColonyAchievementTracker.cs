// Decompiled with JetBrains decompiler
// Type: ColonyAchievementTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ColonyAchievementTracker")]
public class ColonyAchievementTracker : KMonoBehaviour, ISaveLoadableDetails, IRenderEveryTick
{
  public Dictionary<string, ColonyAchievementStatus> achievements = new Dictionary<string, ColonyAchievementStatus>();
  [KSerialization.Serialize]
  public Dictionary<int, int> fetchAutomatedChoreDeliveries = new Dictionary<int, int>();
  [KSerialization.Serialize]
  public Dictionary<int, int> fetchDupeChoreDeliveries = new Dictionary<int, int>();
  [KSerialization.Serialize]
  public Dictionary<int, List<int>> dupesCompleteChoresInSuits = new Dictionary<int, List<int>>();
  [KSerialization.Serialize]
  public HashSet<Tag> tamedCritterTypes = new HashSet<Tag>();
  [KSerialization.Serialize]
  public bool defrostedDuplicant;
  [KSerialization.Serialize]
  public HashSet<Tag> analyzedSeeds = new HashSet<Tag>();
  [KSerialization.Serialize]
  public float totalMaterialsHarvestFromPOI;
  [KSerialization.Serialize]
  public float radBoltTravelDistance;
  [KSerialization.Serialize]
  public bool harvestAHiveWithoutGettingStung;
  [KSerialization.Serialize]
  public Dictionary<int, int> cyclesRocketDupeMoraleAboveRequirement = new Dictionary<int, int>();
  [KSerialization.Serialize]
  public bool efficientlyGatheredData;
  [KSerialization.Serialize]
  public bool fullyBoostedBionic;
  [KSerialization.Serialize]
  public int deadDupeCounter;
  [KSerialization.Serialize]
  private int geothermalProgress;
  [KSerialization.Serialize]
  public ColonyAchievementTracker.LargeImpactorState largeImpactorState;
  [KSerialization.Serialize]
  public float LargeImpactorBackgroundScale = 0.6f;
  [KSerialization.Serialize]
  public int largeImpactorLandedCycle = -1;
  [KSerialization.Serialize]
  public int minnowQuestsCompleted;
  [KSerialization.Serialize]
  public bool allMinnowQuestsCompleted;
  private const int GEO_DISCOVERED_BIT = 1;
  private const int GEO_CONTROLLER_REPAIRED_BIT = 2;
  private const int GEO_CONTROLLER_VENTED_BIT = 4;
  private const int GEO_CLEARED_ENTOMBED_BIT = 8;
  private const int GEO_VICTORY_ACK_BIT = 16 /*0x10*/;
  private SchedulerHandle checkAchievementsHandle;
  private int forceCheckAchievementHandle = -1;
  [KSerialization.Serialize]
  private int updatingAchievement;
  [KSerialization.Serialize]
  private List<string> completedAchievementsToDisplay = new List<string>();
  private SchedulerHandle victorySchedulerHandle;
  public static readonly string UnlockedAchievementKey = "UnlockedAchievement";
  private Dictionary<string, object> unlockedAchievementMetric = new Dictionary<string, object>()
  {
    {
      ColonyAchievementTracker.UnlockedAchievementKey,
      (object) null
    }
  };
  private static readonly Tag[] SuitTags = new Tag[3]
  {
    GameTags.AtmoSuit,
    GameTags.JetSuit,
    GameTags.LeadSuit
  };
  private static readonly EventSystem.IntraObjectHandler<ColonyAchievementTracker> OnNewDayDelegate = new EventSystem.IntraObjectHandler<ColonyAchievementTracker>((Action<ColonyAchievementTracker, object>) ((component, data) => component.OnNewDay(data)));

  public bool HasAnyDupeDied => this.deadDupeCounter > 0;

  public bool GeothermalFacilityDiscovered
  {
    get => (this.geothermalProgress & 1) == 1;
    set
    {
      if (value)
      {
        this.geothermalProgress |= 1;
      }
      else
      {
        DebugUtil.DevAssert(value, "unsetting progress? why");
        this.geothermalProgress &= -2;
      }
    }
  }

  public bool GeothermalControllerRepaired
  {
    get => (this.geothermalProgress & 2) == 2;
    set
    {
      if (value)
      {
        this.geothermalProgress |= 2;
      }
      else
      {
        DebugUtil.DevAssert(value, "unsetting progress? why");
        this.geothermalProgress &= -3;
      }
    }
  }

  public bool GeothermalControllerHasVented
  {
    get => (this.geothermalProgress & 4) == 4;
    set
    {
      if (value)
      {
        this.geothermalProgress |= 4;
      }
      else
      {
        DebugUtil.DevAssert(value, "unsetting progress? why");
        this.geothermalProgress &= -5;
      }
    }
  }

  public bool GeothermalClearedEntombedVent
  {
    get => (this.geothermalProgress & 8) == 8;
    set
    {
      if (value)
      {
        this.geothermalProgress |= 8;
      }
      else
      {
        DebugUtil.DevAssert(value, "unsetting progress? why");
        this.geothermalProgress &= -9;
      }
    }
  }

  public bool GeothermalVictoryPopupDismissed
  {
    get => (this.geothermalProgress & 16 /*0x10*/) == 16 /*0x10*/;
    set
    {
      if (value)
      {
        this.geothermalProgress |= 16 /*0x10*/;
      }
      else
      {
        DebugUtil.DevAssert(value, "unsetting progress? why");
        this.geothermalProgress &= -17;
      }
    }
  }

  public List<string> achievementsToDisplay => this.completedAchievementsToDisplay;

  public void ClearDisplayAchievements() => this.achievementsToDisplay.Clear();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    foreach (ColonyAchievement resource in Db.Get().ColonyAchievements.resources)
    {
      if (!this.achievements.ContainsKey(resource.Id))
      {
        ColonyAchievementStatus achievementStatus = new ColonyAchievementStatus(resource.Id);
        this.achievements.Add(resource.Id, achievementStatus);
      }
    }
    this.forceCheckAchievementHandle = Game.Instance.Subscribe(395452326, new Action<object>(this.CheckAchievements));
    this.Subscribe<ColonyAchievementTracker>(631075836, ColonyAchievementTracker.OnNewDayDelegate);
    this.UpgradeTamedCritterAchievements();
  }

  private void UpgradeTamedCritterAchievements()
  {
    foreach (ColonyAchievementRequirement achievementRequirement in Db.Get().ColonyAchievements.TameAllBasicCritters.requirementChecklist)
    {
      if (achievementRequirement is CritterTypesWithTraits critterTypesWithTraits)
        critterTypesWithTraits.UpdateSavedState();
    }
    foreach (ColonyAchievementRequirement achievementRequirement in Db.Get().ColonyAchievements.TameAGassyMoo.requirementChecklist)
    {
      if (achievementRequirement is CritterTypesWithTraits critterTypesWithTraits)
        critterTypesWithTraits.UpdateSavedState();
    }
  }

  public void RenderEveryTick(float dt)
  {
    if (this.updatingAchievement >= this.achievements.Count)
      this.updatingAchievement = 0;
    KeyValuePair<string, ColonyAchievementStatus> keyValuePair = this.achievements.ElementAt<KeyValuePair<string, ColonyAchievementStatus>>(this.updatingAchievement);
    ++this.updatingAchievement;
    if (keyValuePair.Value.success || keyValuePair.Value.failed)
      return;
    keyValuePair.Value.UpdateAchievement();
    if (!keyValuePair.Value.success || keyValuePair.Value.failed)
      return;
    ColonyAchievementTracker.UnlockPlatformAchievement(keyValuePair.Key);
    this.completedAchievementsToDisplay.Add(keyValuePair.Key);
    this.TriggerNewAchievementCompleted(keyValuePair.Key);
    RetireColonyUtility.SaveColonySummaryData();
  }

  private void CheckAchievements(object data = null)
  {
    foreach (KeyValuePair<string, ColonyAchievementStatus> achievement in this.achievements)
    {
      if (!achievement.Value.success && !achievement.Value.failed)
      {
        achievement.Value.UpdateAchievement();
        if (achievement.Value.success && !achievement.Value.failed)
        {
          ColonyAchievementTracker.UnlockPlatformAchievement(achievement.Key);
          this.completedAchievementsToDisplay.Add(achievement.Key);
          this.TriggerNewAchievementCompleted(achievement.Key);
        }
      }
    }
    RetireColonyUtility.SaveColonySummaryData();
  }

  private static void UnlockPlatformAchievement(string achievement_id)
  {
    if (DebugHandler.InstantBuildMode)
      Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: instant build mode", (object) achievement_id);
    else if (SaveGame.Instance.sandboxEnabled)
      Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: sandbox mode", (object) achievement_id);
    else if (Game.Instance.debugWasUsed)
    {
      Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: debug was used.", (object) achievement_id);
    }
    else
    {
      ColonyAchievement colonyAchievement = Db.Get().ColonyAchievements.Get(achievement_id);
      if (colonyAchievement == null || string.IsNullOrEmpty(colonyAchievement.platformAchievementId))
        return;
      if ((bool) (UnityEngine.Object) SteamAchievementService.Instance)
        SteamAchievementService.Instance.Unlock(colonyAchievement.platformAchievementId);
      else
        Debug.LogWarningFormat("Steam achievement [{0}] was achieved, but achievement service was null", (object) colonyAchievement.platformAchievementId);
    }
  }

  public void DebugTriggerAchievement(string id)
  {
    this.achievements[id].failed = false;
    this.achievements[id].success = true;
  }

  private void BeginVictorySequence(string achievementID)
  {
    RootMenu.Instance.canTogglePauseScreen = false;
    CameraController.Instance.DisableUserCameraControl = true;
    if (!SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Pause(false);
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryMessageSnapshot);
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot);
    this.ToggleVictoryUI(true);
    StoryMessageScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.StoryMessageScreen.gameObject).GetComponent<StoryMessageScreen>();
    component.restoreInterfaceOnClose = false;
    component.title = (string) COLONY_ACHIEVEMENTS.PRE_VICTORY_MESSAGE_HEADER;
    component.body = string.Format((string) COLONY_ACHIEVEMENTS.PRE_VICTORY_MESSAGE_BODY, (object) $"<b>{Db.Get().ColonyAchievements.Get(achievementID).Name}</b>\n{Db.Get().ColonyAchievements.Get(achievementID).description}");
    component.Show();
    CameraController.Instance.SetWorldInteractive(false);
    component.OnClose += (System.Action) (() =>
    {
      SpeedControlScreen.Instance.SetSpeed(1);
      if (!SpeedControlScreen.Instance.IsPaused)
        SpeedControlScreen.Instance.Pause(false);
      CameraController.Instance.SetWorldInteractive(true);
      Db.Get().ColonyAchievements.Get(achievementID).victorySequence((KMonoBehaviour) this);
    });
  }

  public bool IsAchievementUnlocked(ColonyAchievement achievement)
  {
    foreach (KeyValuePair<string, ColonyAchievementStatus> achievement1 in this.achievements)
    {
      if (achievement1.Key == achievement.Id)
      {
        if (achievement1.Value.success)
          return true;
        achievement1.Value.UpdateAchievement();
        return achievement1.Value.success;
      }
    }
    return false;
  }

  protected override void OnCleanUp()
  {
    this.victorySchedulerHandle.ClearScheduler();
    Game.Instance.Unsubscribe(this.forceCheckAchievementHandle);
    this.checkAchievementsHandle.ClearScheduler();
    base.OnCleanUp();
  }

  private void TriggerNewAchievementCompleted(string achievement, GameObject cameraTarget = null)
  {
    this.unlockedAchievementMetric[ColonyAchievementTracker.UnlockedAchievementKey] = (object) achievement;
    ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.unlockedAchievementMetric, "AchievementCompleted");
    bool flag = false;
    if (Db.Get().ColonyAchievements.Get(achievement).isVictoryCondition)
    {
      flag = true;
      this.BeginVictorySequence(achievement);
    }
    if (flag)
      return;
    AchievementEarnedMessage achievementEarnedMessage = new AchievementEarnedMessage();
    Messenger.Instance.QueueMessage((Message) achievementEarnedMessage);
  }

  private void ToggleVictoryUI(bool victoryUIActive)
  {
    List<KScreen> kscreenList = new List<KScreen>();
    kscreenList.Add((KScreen) NotificationScreen.Instance);
    kscreenList.Add((KScreen) OverlayMenu.Instance);
    if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
      kscreenList.Add((KScreen) PlanScreen.Instance);
    if ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null)
      kscreenList.Add((KScreen) BuildMenu.Instance);
    kscreenList.Add((KScreen) ManagementMenu.Instance);
    kscreenList.Add((KScreen) ToolMenu.Instance);
    kscreenList.Add((KScreen) ToolMenu.Instance.PriorityScreen);
    kscreenList.Add((KScreen) ResourceCategoryScreen.Instance);
    kscreenList.Add((KScreen) TopLeftControlScreen.Instance);
    kscreenList.Add((KScreen) DateTime.Instance);
    kscreenList.Add((KScreen) BuildWatermark.Instance);
    kscreenList.Add((KScreen) HoverTextScreen.Instance);
    kscreenList.Add((KScreen) DetailsScreen.Instance);
    kscreenList.Add((KScreen) DebugPaintElementScreen.Instance);
    kscreenList.Add((KScreen) DebugBaseTemplateButton.Instance);
    kscreenList.Add((KScreen) StarmapScreen.Instance);
    foreach (KScreen kscreen in kscreenList)
    {
      if ((UnityEngine.Object) kscreen != (UnityEngine.Object) null)
        kscreen.Show(!victoryUIActive);
    }
  }

  public void Serialize(BinaryWriter writer)
  {
    writer.Write(this.achievements.Count);
    foreach (KeyValuePair<string, ColonyAchievementStatus> achievement in this.achievements)
    {
      writer.WriteKleiString(achievement.Key);
      achievement.Value.Serialize(writer);
    }
  }

  public void Deserialize(IReader reader)
  {
    if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 10))
      return;
    int num = reader.ReadInt32();
    for (int index = 0; index < num; ++index)
    {
      string str = reader.ReadKleiString();
      ColonyAchievementStatus achievementStatus = ColonyAchievementStatus.Deserialize(reader, str);
      if (Db.Get().ColonyAchievements.Exists(str))
        this.achievements.Add(str, achievementStatus);
    }
  }

  public void LogFetchChore(GameObject fetcher, ChoreType choreType)
  {
    if (choreType == Db.Get().ChoreTypes.StorageFetch || choreType == Db.Get().ChoreTypes.BuildFetch || choreType == Db.Get().ChoreTypes.RepairFetch || choreType == Db.Get().ChoreTypes.FoodFetch || choreType == Db.Get().ChoreTypes.Transport)
      return;
    Dictionary<int, int> dictionary = (Dictionary<int, int>) null;
    if ((UnityEngine.Object) fetcher.GetComponent<SolidTransferArm>() != (UnityEngine.Object) null)
      dictionary = this.fetchAutomatedChoreDeliveries;
    else if ((UnityEngine.Object) fetcher.GetComponent<MinionIdentity>() != (UnityEngine.Object) null)
      dictionary = this.fetchDupeChoreDeliveries;
    if (dictionary == null)
      return;
    int cycle = GameClock.Instance.GetCycle();
    if (!dictionary.ContainsKey(cycle))
      dictionary.Add(cycle, 0);
    dictionary[cycle]++;
  }

  public void LogCritterTamed(Tag prefabId) => this.tamedCritterTypes.Add(prefabId);

  public void LogSuitChore(ChoreDriver driver)
  {
    if ((UnityEngine.Object) driver == (UnityEngine.Object) null || (UnityEngine.Object) driver.GetComponent<MinionIdentity>() == (UnityEngine.Object) null)
      return;
    bool flag = false;
    foreach (AssignableSlotInstance slot in driver.GetComponent<MinionIdentity>().GetEquipment().Slots)
    {
      Equippable assignable = slot.assignable as Equippable;
      if ((bool) (UnityEngine.Object) assignable && assignable.GetComponent<KPrefabID>().IsAnyPrefabID(ColonyAchievementTracker.SuitTags))
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return;
    int cycle = GameClock.Instance.GetCycle();
    int instanceId = driver.GetComponent<KPrefabID>().InstanceID;
    if (!this.dupesCompleteChoresInSuits.ContainsKey(cycle))
    {
      this.dupesCompleteChoresInSuits.Add(cycle, new List<int>()
      {
        instanceId
      });
    }
    else
    {
      if (this.dupesCompleteChoresInSuits[cycle].Contains(instanceId))
        return;
      this.dupesCompleteChoresInSuits[cycle].Add(instanceId);
    }
  }

  public void LogAnalyzedSeed(Tag seed) => this.analyzedSeeds.Add(seed);

  public void OnNewDay(object data)
  {
    foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
    {
      if ((UnityEngine.Object) minionStorage.GetComponent<CommandModule>() != (UnityEngine.Object) null)
      {
        List<MinionStorage.Info> storedMinionInfo = minionStorage.GetStoredMinionInfo();
        if (storedMinionInfo.Count > 0)
        {
          int cycle = GameClock.Instance.GetCycle();
          if (!this.dupesCompleteChoresInSuits.ContainsKey(cycle))
            this.dupesCompleteChoresInSuits.Add(cycle, new List<int>());
          for (int index = 0; index < storedMinionInfo.Count; ++index)
          {
            KPrefabID kprefabId = storedMinionInfo[index].serializedMinion.Get();
            if ((UnityEngine.Object) kprefabId != (UnityEngine.Object) null)
              this.dupesCompleteChoresInSuits[cycle].Add(kprefabId.InstanceID);
          }
        }
      }
    }
    if (!DlcManager.IsExpansion1Active() || !(Db.Get().ColonyAchievements.SurviveInARocket.requirementChecklist[0] is SurviveARocketWithMinimumMorale withMinimumMorale))
      return;
    float minimumMorale = withMinimumMorale.minimumMorale;
    int numberOfCycles = withMinimumMorale.numberOfCycles;
    foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
    {
      if (worldContainer.IsModuleInterior)
      {
        if (!this.cyclesRocketDupeMoraleAboveRequirement.ContainsKey(worldContainer.id))
          this.cyclesRocketDupeMoraleAboveRequirement.Add(worldContainer.id, 0);
        if (worldContainer.GetComponent<Clustercraft>().Status != Clustercraft.CraftStatus.Grounded)
        {
          List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(worldContainer.id);
          bool flag = worldItems.Count > 0;
          foreach (MinionIdentity cmp in worldItems)
          {
            if ((double) Db.Get().Attributes.QualityOfLife.Lookup((Component) cmp).GetTotalValue() < (double) minimumMorale)
            {
              flag = false;
              break;
            }
          }
          this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] = flag ? this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] + 1 : 0;
        }
        else if (this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] < numberOfCycles)
          this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] = 0;
      }
    }
  }

  public enum LargeImpactorState
  {
    Alive,
    Defeated,
    Landed,
  }
}
