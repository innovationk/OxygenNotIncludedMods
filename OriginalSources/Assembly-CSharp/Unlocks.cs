// Decompiled with JetBrains decompiler
// Type: Unlocks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json;
using ProcGen;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Unlocks")]
public class Unlocks : KMonoBehaviour
{
  private const int FILE_IO_RETRY_ATTEMPTS = 5;
  private List<string> unlocked = new List<string>();
  private List<Unlocks.MetaUnlockCategory> MetaUnlockCategories = new List<Unlocks.MetaUnlockCategory>()
  {
    new Unlocks.MetaUnlockCategory("dimensionalloreMeta", "dimensionallore", 4)
  };
  public Dictionary<string, string[]> lockCollections = new Dictionary<string, string[]>()
  {
    {
      "emails",
      new string[25]
      {
        "email_thermodynamiclaws",
        "email_security2",
        "email_pens2",
        "email_atomiconrecruitment",
        "email_devonsblog",
        "email_researchgiant",
        "email_thejanitor",
        "email_newemployee",
        "email_timeoffapproved",
        "email_security3",
        "email_preliminarycalculations",
        "email_hollandsdog",
        "email_temporalbowupdate",
        "email_retemporalbowupdate",
        "email_memorychip",
        "email_arthistoryrequest",
        "email_AIcontrol",
        "email_AIcontrol2",
        "email_friendlyemail",
        "email_AIcontrol3",
        "email_AIcontrol4",
        "email_engineeringcandidate",
        "email_missingnotes",
        "email_journalistrequest",
        "email_journalistrequest2"
      }
    },
    {
      "dlc2emails",
      new string[5]
      {
        "email_newbaby",
        "email_cerestourism1",
        "email_cerestourism2",
        "email_voicemail",
        "email_expelled"
      }
    },
    {
      "dlc3emails",
      new string[1]{ "email_ulti" }
    },
    {
      "dlc4emails",
      new string[2]{ "notices_foreword", "notes_HigbySong" }
    },
    {
      "dlc5archivebuilding",
      new string[1]{ "notes_marinea" }
    },
    {
      "dlc5emails",
      new string[2]{ "email_ceasedesist", "email_flood" }
    },
    {
      "journals",
      new string[35]
      {
        "journal_timesarrowthoughts",
        "journal_A046_1",
        "journal_B835_1",
        "journal_sunflowerseeds",
        "journal_B327_1",
        "journal_B556_1",
        "journal_employeeprocessing",
        "journal_B327_2",
        "journal_A046_2",
        "journal_elliesbirthday1",
        "journal_B835_2",
        "journal_ants",
        "journal_pipedream",
        "journal_B556_2",
        "journal_movedrats",
        "journal_B835_3",
        "journal_A046_3",
        "journal_B556_3",
        "journal_B327_3",
        "journal_B835_4",
        "journal_cleanup",
        "journal_A046_4",
        "journal_B327_4",
        "journal_revisitednumbers",
        "journal_B556_4",
        "journal_B835_5",
        "journal_elliesbirthday2",
        "journal_B111_1",
        "journal_revisitednumbers2",
        "journal_timemusings",
        "journal_evil",
        "journal_timesorder",
        "journal_inspace",
        "journal_mysteryaward",
        "journal_courier"
      }
    },
    {
      "dlc3journals",
      new string[3]
      {
        "journal_potatobattery1",
        "journal_potatobattery2",
        "journal_potatobattery3"
      }
    },
    {
      "dlc4journals",
      new string[5]
      {
        "journal_expedition1",
        "journal_expedition2",
        "journal_expedition3",
        "journal_B824",
        "journal_incoming"
      }
    },
    {
      "dlc5journals",
      new string[4]
      {
        "journal_cakeandtea",
        "journal_hoax",
        "journal_printanomaly",
        "journal_fridgenote"
      }
    },
    {
      "researchnotes",
      new string[25]
      {
        "notes_clonedrats",
        "misc_dishbot",
        "notes_agriculture1",
        "notes_husbandry1",
        "notes_hibiscus3",
        "misc_newsecurity",
        "notes_husbandry2",
        "notes_agriculture2",
        "notes_geneticooze",
        "notes_agriculture3",
        "notes_husbandry3",
        "misc_casualfriday",
        "notes_memoryimplantation",
        "notes_husbandry4",
        "notes_agriculture4",
        "notes_neutronium",
        "misc_mailroometiquette",
        "notes_firstsuccess",
        "misc_reminder",
        "notes_neutroniumapplications",
        "notes_teleportation",
        "notes_AI",
        "misc_politerequest",
        "cryotank_warning",
        "misc_unattendedcultures"
      }
    },
    {
      "dlc2researchnotes",
      new string[1]{ "notes_cleanup" }
    },
    {
      "dlc3researchnotes",
      new string[2]{ "notes_talkshow", "notes_remoteworkstation" }
    },
    {
      "dlc4researchnotes",
      new string[1]{ "notes_seepage" }
    },
    {
      "dimensionallore",
      new string[6]
      {
        "notes_clonedrabbits",
        "notes_clonedraccoons",
        "journal_movedrabbits",
        "journal_movedraccoons",
        "journal_strawberries",
        "journal_shrimp"
      }
    },
    {
      "dimensionalloreMeta",
      new string[1]{ "log9" }
    },
    {
      "dlc2dimensionallore",
      new string[3]
      {
        "notes_tragicnews",
        "notes_tragicnews2",
        "notes_tragicnews3"
      }
    },
    {
      "dlc2archivebuilding",
      new string[1]{ "notes_welcometoceres" }
    },
    {
      "dlc2geoplantinput",
      new string[1]{ "notes_geoinputs" }
    },
    {
      "dlc2geoplantcomplete",
      new string[1]{ "notes_earthquake" }
    },
    {
      "dlc4surfacepoi",
      new string[1]{ "notice_surfacepoi" }
    },
    {
      "space",
      new string[4]
      {
        "display_spaceprop1",
        "notice_pilot",
        "journal_inspace",
        "notes_firstcolony"
      }
    },
    {
      "storytraits",
      new string[20]
      {
        "story_trait_critter_manipulator_initial",
        "story_trait_critter_manipulator_complete",
        "storytrait_crittermanipulator_workiversary",
        "story_trait_mega_brain_tank_initial",
        "story_trait_mega_brain_tank_competed",
        "story_trait_fossilhunt_initial",
        "story_trait_fossilhunt_poi1",
        "story_trait_fossilhunt_poi2",
        "story_trait_fossilhunt_poi3",
        "story_trait_fossilhunt_complete",
        "story_trait_morbrover_initial",
        "story_trait_morbrover_reveal",
        "story_trait_morbrover_reveal_lore",
        "story_trait_morbrover_complete",
        "story_trait_morbrover_complete_lore",
        "story_trait_morbrover_biobot",
        "story_trait_morbrover_locker",
        "story_trait_hijackheadquarters_mirror",
        "story_trait_hijackheadquarters_complete",
        "story_trait_hijackheadquarters_initial"
      }
    }
  };
  public Dictionary<int, string> cycleLocked = new Dictionary<int, string>()
  {
    {
      0,
      "log1"
    },
    {
      3,
      "log2"
    },
    {
      15,
      "log3"
    },
    {
      1000,
      "log4"
    },
    {
      1500,
      "log4b"
    },
    {
      2000,
      "log5"
    },
    {
      2500,
      "log5b"
    },
    {
      3000,
      "log6"
    },
    {
      3500,
      "log6b"
    },
    {
      4000,
      "log7"
    },
    {
      4001,
      "log8"
    }
  };
  private static readonly EventSystem.IntraObjectHandler<Unlocks> OnLaunchRocketDelegate = new EventSystem.IntraObjectHandler<Unlocks>((Action<Unlocks, object>) ((component, data) => component.OnLaunchRocket(data)));
  private static readonly EventSystem.IntraObjectHandler<Unlocks> OnDuplicantDiedDelegate = new EventSystem.IntraObjectHandler<Unlocks>((Action<Unlocks, object>) ((component, data) => component.OnDuplicantDied(data)));
  private static readonly EventSystem.IntraObjectHandler<Unlocks> OnDiscoveredSpaceDelegate = new EventSystem.IntraObjectHandler<Unlocks>((Action<Unlocks, object>) ((component, data) => component.OnDiscoveredSpace(data)));

  private static string UnlocksFilename => System.IO.Path.Combine(Util.RootFolder(), "unlocks.json");

  protected override void OnPrefabInit() => this.LoadUnlocks();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.UnlockCycleCodexes();
    GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDay));
    this.Subscribe<Unlocks>(-1277991738, Unlocks.OnLaunchRocketDelegate);
    this.Subscribe<Unlocks>(282337316, Unlocks.OnDuplicantDiedDelegate);
    this.Subscribe<Unlocks>(-818188514, Unlocks.OnDiscoveredSpaceDelegate);
    Components.LiveMinionIdentities.OnAdd += new Action<MinionIdentity>(this.OnNewDupe);
  }

  public bool IsUnlocked(string unlockID)
  {
    if (string.IsNullOrEmpty(unlockID))
      return false;
    return DebugHandler.InstantBuildMode || this.unlocked.Contains(unlockID);
  }

  public IReadOnlyList<string> GetAllUnlockedIds() => (IReadOnlyList<string>) this.unlocked;

  public void Lock(string unlockID)
  {
    if (!this.unlocked.Contains(unlockID))
      return;
    this.unlocked.Remove(unlockID);
    this.SaveUnlocks();
    Game.Instance.Trigger(1594320620, (object) unlockID);
  }

  public void Unlock(string unlockID, bool shouldTryShowCodexNotification = true)
  {
    if (string.IsNullOrEmpty(unlockID))
    {
      DebugUtil.DevAssert(false, "Unlock called with null or empty string");
    }
    else
    {
      if (!this.unlocked.Contains(unlockID))
      {
        this.unlocked.Add(unlockID);
        this.SaveUnlocks();
        Game.Instance.Trigger(1594320620, (object) unlockID);
        if (shouldTryShowCodexNotification)
        {
          MessageNotification unlockNotification = this.GenerateCodexUnlockNotification(unlockID);
          if (unlockNotification != null)
            this.GetComponent<Notifier>().Add((Notification) unlockNotification);
        }
      }
      this.EvalMetaCategories();
    }
  }

  private void EvalMetaCategories()
  {
    foreach (Unlocks.MetaUnlockCategory metaUnlockCategory in this.MetaUnlockCategories)
    {
      string metaCollectionId = metaUnlockCategory.metaCollectionID;
      string mesaCollectionID = metaUnlockCategory.mesaCollectionID;
      int mesaUnlockCount = metaUnlockCategory.mesaUnlockCount;
      int count = 0;
      bool isCollectionReplaced = false;
      if ((UnityEngine.Object) SaveLoader.Instance != (UnityEngine.Object) null)
      {
        foreach (LoreCollectionOverride clusterUnlock in SaveLoader.Instance.ClusterLayout.clusterUnlocks)
        {
          if (EvaluateCollection(clusterUnlock))
            break;
        }
        foreach (string currentDlcMixingId in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
        {
          DlcMixingSettings dlcMixingSettings = SettingsCache.GetCachedDlcMixingSettings(currentDlcMixingId);
          if (dlcMixingSettings != null)
          {
            foreach (LoreCollectionOverride globalLoreUnlock in dlcMixingSettings.globalLoreUnlocks)
            {
              if (EvaluateCollection(globalLoreUnlock))
                break;
            }
          }
        }
      }
      if (!isCollectionReplaced)
      {
        foreach (string unlockID in this.lockCollections[mesaCollectionID])
        {
          if (this.IsUnlocked(unlockID))
            count++;
        }
      }
      if (count >= mesaUnlockCount)
        this.UnlockNext(metaCollectionId);

      bool EvaluateCollection(LoreCollectionOverride loreUnlock)
      {
        if (loreUnlock.id == mesaCollectionID)
        {
          foreach (string unlockID in this.lockCollections[loreUnlock.collection])
          {
            if (this.IsUnlocked(unlockID))
              count++;
          }
          if (loreUnlock.orderRule == LoreCollectionOverride.OrderRule.Replace)
          {
            isCollectionReplaced = true;
            return true;
          }
        }
        return false;
      }
    }
  }

  private void SaveUnlocks()
  {
    if (!Directory.Exists(Util.RootFolder()))
      Directory.CreateDirectory(Util.RootFolder());
    string s = JsonConvert.SerializeObject((object) this.unlocked);
    bool flag = false;
    int num = 0;
    while (!flag)
    {
      if (num >= 5)
        break;
      try
      {
        Thread.Sleep(num * 100);
        using (FileStream fileStream = File.Open(Unlocks.UnlocksFilename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
        {
          flag = true;
          byte[] bytes = new ASCIIEncoding().GetBytes(s);
          fileStream.Write(bytes, 0, bytes.Length);
        }
      }
      catch (Exception ex)
      {
        Debug.LogWarningFormat("Failed to save Unlocks attempt {0}: {1}", (object) (num + 1), (object) ex.ToString());
      }
      ++num;
    }
  }

  public void LoadUnlocks()
  {
    this.unlocked.Clear();
    if (!File.Exists(Unlocks.UnlocksFilename))
      return;
    string str1 = "";
    bool flag = false;
    int num = 0;
    while (!flag)
    {
      if (num < 5)
      {
        try
        {
          Thread.Sleep(num * 100);
          using (FileStream fileStream = File.Open(Unlocks.UnlocksFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
          {
            flag = true;
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            byte[] numArray = new byte[fileStream.Length];
            if ((long) fileStream.Read(numArray, 0, numArray.Length) == fileStream.Length)
              str1 += asciiEncoding.GetString(numArray);
          }
        }
        catch (Exception ex)
        {
          Debug.LogWarningFormat("Failed to load Unlocks attempt {0}: {1}", (object) (num + 1), (object) ex.ToString());
        }
        ++num;
      }
      else
        break;
    }
    if (string.IsNullOrEmpty(str1))
      return;
    try
    {
      foreach (string str2 in JsonConvert.DeserializeObject<string[]>(str1))
      {
        if (!string.IsNullOrEmpty(str2) && !this.unlocked.Contains(str2))
          this.unlocked.Add(str2);
      }
    }
    catch (Exception ex)
    {
      Debug.LogErrorFormat("Error parsing unlocks file [{0}]: {1}", (object) Unlocks.UnlocksFilename, (object) ex.ToString());
    }
  }

  private string GetNextClusterUnlock(
    string collectionID,
    out LoreCollectionOverride.OrderRule orderRule,
    bool randomize)
  {
    foreach (LoreCollectionOverride clusterUnlock in SaveLoader.Instance.ClusterLayout.clusterUnlocks)
    {
      if (!(clusterUnlock.id != collectionID))
      {
        if (!this.lockCollections.ContainsKey(collectionID))
        {
          DebugUtil.DevLogError($"Lore collection '{collectionID}' is missing");
          orderRule = LoreCollectionOverride.OrderRule.Invalid;
          return (string) null;
        }
        if (!this.lockCollections.ContainsKey(clusterUnlock.collection))
        {
          DebugUtil.DevLogError($"Lore collection '{clusterUnlock.collection}' is missing but defined in the cluster file.");
        }
        else
        {
          string[] lockCollection = this.lockCollections[clusterUnlock.collection];
          if (randomize)
            ((IList<string>) lockCollection).Shuffle<string>();
          foreach (string unlockID in lockCollection)
          {
            if (!this.IsUnlocked(unlockID))
            {
              orderRule = clusterUnlock.orderRule;
              return unlockID;
            }
          }
          if (clusterUnlock.orderRule == LoreCollectionOverride.OrderRule.Replace)
          {
            orderRule = clusterUnlock.orderRule;
            return (string) null;
          }
        }
      }
    }
    orderRule = LoreCollectionOverride.OrderRule.Invalid;
    return (string) null;
  }

  private string GetNextGlobalDlcUnlock(
    string collectionID,
    out LoreCollectionOverride.OrderRule orderRule,
    bool randomize)
  {
    foreach (string currentDlcMixingId in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
    {
      DlcMixingSettings dlcMixingSettings = SettingsCache.GetCachedDlcMixingSettings(currentDlcMixingId);
      if (dlcMixingSettings != null)
      {
        foreach (LoreCollectionOverride globalLoreUnlock in dlcMixingSettings.globalLoreUnlocks)
        {
          if (!(globalLoreUnlock.id != collectionID))
          {
            if (!this.lockCollections.ContainsKey(collectionID))
            {
              DebugUtil.DevLogError($"Lore collection '{collectionID}' is missing");
              orderRule = LoreCollectionOverride.OrderRule.Invalid;
              return (string) null;
            }
            string[] lockCollection = this.lockCollections[globalLoreUnlock.collection];
            if (randomize)
              ((IList<string>) lockCollection).Shuffle<string>();
            foreach (string unlockID in lockCollection)
            {
              if (!this.IsUnlocked(unlockID))
              {
                orderRule = globalLoreUnlock.orderRule;
                return unlockID;
              }
            }
            if (globalLoreUnlock.orderRule == LoreCollectionOverride.OrderRule.Replace)
            {
              orderRule = globalLoreUnlock.orderRule;
              return (string) null;
            }
          }
        }
      }
    }
    orderRule = LoreCollectionOverride.OrderRule.Invalid;
    return (string) null;
  }

  public string UnlockNext(string collectionID, bool randomize = false)
  {
    if ((UnityEngine.Object) SaveLoader.Instance != (UnityEngine.Object) null)
    {
      LoreCollectionOverride.OrderRule orderRule1;
      string nextClusterUnlock = this.GetNextClusterUnlock(collectionID, out orderRule1, randomize);
      if (nextClusterUnlock != null && (orderRule1 == LoreCollectionOverride.OrderRule.Prepend || orderRule1 == LoreCollectionOverride.OrderRule.Replace))
      {
        this.Unlock(nextClusterUnlock);
        return nextClusterUnlock;
      }
      LoreCollectionOverride.OrderRule orderRule2;
      string nextGlobalDlcUnlock = this.GetNextGlobalDlcUnlock(collectionID, out orderRule2, randomize);
      if (nextGlobalDlcUnlock != null && (orderRule2 == LoreCollectionOverride.OrderRule.Prepend || orderRule2 == LoreCollectionOverride.OrderRule.Replace))
      {
        this.Unlock(nextGlobalDlcUnlock);
        return nextGlobalDlcUnlock;
      }
      if (orderRule1 == LoreCollectionOverride.OrderRule.Replace || orderRule2 == LoreCollectionOverride.OrderRule.Replace)
        return (string) null;
    }
    string[] lockCollection = this.lockCollections[collectionID];
    if (randomize)
      ((IList<string>) lockCollection).Shuffle<string>();
    foreach (string unlockID in lockCollection)
    {
      if (string.IsNullOrEmpty(unlockID))
        DebugUtil.DevAssertArgs(false, (object) "Found null/empty string in Unlocks collection: ", (object) collectionID);
      else if (!this.IsUnlocked(unlockID))
      {
        this.Unlock(unlockID);
        return unlockID;
      }
    }
    if ((UnityEngine.Object) SaveLoader.Instance != (UnityEngine.Object) null)
    {
      LoreCollectionOverride.OrderRule orderRule;
      string nextClusterUnlock = this.GetNextClusterUnlock(collectionID, out orderRule, randomize);
      if (nextClusterUnlock != null && orderRule == LoreCollectionOverride.OrderRule.Append)
      {
        this.Unlock(nextClusterUnlock);
        return nextClusterUnlock;
      }
      string nextGlobalDlcUnlock = this.GetNextGlobalDlcUnlock(collectionID, out orderRule, randomize);
      if (nextGlobalDlcUnlock != null && orderRule == LoreCollectionOverride.OrderRule.Append)
      {
        this.Unlock(nextGlobalDlcUnlock);
        return nextGlobalDlcUnlock;
      }
    }
    return (string) null;
  }

  private MessageNotification GenerateCodexUnlockNotification(string lockID)
  {
    string entryForLock = CodexCache.GetEntryForLock(lockID);
    if (string.IsNullOrEmpty(entryForLock))
      return (MessageNotification) null;
    string key = (string) null;
    if (CodexCache.FindSubEntry(lockID) != null)
      key = CodexCache.FindSubEntry(lockID).title;
    else if (CodexCache.FindSubEntry(entryForLock) != null)
      key = CodexCache.FindSubEntry(entryForLock).title;
    else if (CodexCache.FindEntry(entryForLock) != null)
      key = CodexCache.FindEntry(entryForLock).title;
    string unlock_message = UI.FormatAsLink((string) Strings.Get(key), entryForLock);
    if (string.IsNullOrEmpty(key))
      return (MessageNotification) null;
    ContentContainer contentContainer = CodexCache.FindEntry(entryForLock).contentContainers.Find((Predicate<ContentContainer>) (match => match.lockID == lockID));
    if (contentContainer != null)
    {
      foreach (ICodexWidget codexWidget in contentContainer.content)
      {
        if (codexWidget is CodexText codexText)
          unlock_message = $"{unlock_message}\n\n{codexText.text}";
      }
    }
    return new MessageNotification((Message) new CodexUnlockedMessage(lockID, unlock_message));
  }

  private void UnlockCycleCodexes()
  {
    foreach (KeyValuePair<int, string> keyValuePair in this.cycleLocked)
    {
      if (GameClock.Instance.GetCycle() + 1 >= keyValuePair.Key)
        this.Unlock(keyValuePair.Value);
    }
  }

  private void OnNewDay(object data) => this.UnlockCycleCodexes();

  private void OnLaunchRocket(object data)
  {
    this.Unlock("surfacebreach");
    this.Unlock("firstrocketlaunch");
  }

  private void OnDuplicantDied(object data)
  {
    this.Unlock("duplicantdeath");
    if (Components.LiveMinionIdentities.Count != 1)
      return;
    this.Unlock("onedupeleft");
  }

  private void OnNewDupe(MinionIdentity minion_identity)
  {
    if (Components.LiveMinionIdentities.Count < Db.Get().Personalities.GetAll(true, false).Count)
      return;
    this.Unlock("fulldupecolony");
  }

  private void OnDiscoveredSpace(object data) => this.Unlock("surfacebreach");

  private class MetaUnlockCategory
  {
    public string metaCollectionID;
    public string mesaCollectionID;
    public int mesaUnlockCount;

    public MetaUnlockCategory(
      string metaCollectionID,
      string mesaCollectionID,
      int mesaUnlockCount)
    {
      this.metaCollectionID = metaCollectionID;
      this.mesaCollectionID = mesaCollectionID;
      this.mesaUnlockCount = mesaUnlockCount;
    }
  }
}
