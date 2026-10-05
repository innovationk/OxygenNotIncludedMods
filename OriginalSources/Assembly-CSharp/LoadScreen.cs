// Decompiled with JetBrains decompiler
// Type: LoadScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using ProcGenGame;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LoadScreen : KModalScreen
{
  private const int MAX_CLOUD_TUTORIALS = 5;
  private const string CLOUD_TUTORIAL_KEY = "LoadScreenCloudTutorialTimes";
  private const int ITEMS_PER_PAGE = 20;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private GameObject saveButtonRoot;
  [SerializeField]
  private GameObject colonyListRoot;
  [SerializeField]
  private GameObject colonyViewRoot;
  [SerializeField]
  private HierarchyReferences migrationPanelRefs;
  [SerializeField]
  private HierarchyReferences saveButtonPrefab;
  [SerializeField]
  private KButton loadMoreButton;
  [Space]
  [SerializeField]
  private KButton colonyCloudButton;
  [SerializeField]
  private KButton colonyLocalButton;
  [SerializeField]
  private KButton colonyInfoButton;
  [SerializeField]
  private Sprite localToCloudSprite;
  [SerializeField]
  private Sprite cloudToLocalSprite;
  [SerializeField]
  private Sprite errorSprite;
  [SerializeField]
  private Sprite infoSprite;
  [SerializeField]
  private Bouncer cloudTutorialBouncer;
  public bool requireConfirmation = true;
  private LoadScreen.SelectedSave selectedSave;
  private List<LoadScreen.SaveGameFileDetails> currentColony;
  private UIPool<HierarchyReferences> colonyListPool;
  private ConfirmDialogScreen confirmScreen;
  private InfoDialogScreen infoScreen;
  private InfoDialogScreen errorInfoScreen;
  private ConfirmDialogScreen errorScreen;
  private InspectSaveScreen inspectScreenInstance;
  private int displayedPageCount = 1;

  public static LoadScreen Instance { get; private set; }

  public static void DestroyInstance() => LoadScreen.Instance = (LoadScreen) null;

  protected override void OnPrefabInit()
  {
    Debug.Assert((UnityEngine.Object) LoadScreen.Instance == (UnityEngine.Object) null);
    LoadScreen.Instance = this;
    base.OnPrefabInit();
    this.saveButtonPrefab.gameObject.SetActive(false);
    this.colonyListPool = new UIPool<HierarchyReferences>(this.saveButtonPrefab);
    if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null)
      SpeedControlScreen.Instance.Pause(false);
    if ((UnityEngine.Object) this.closeButton != (UnityEngine.Object) null)
      this.closeButton.onClick += (System.Action) (() => this.Deactivate());
    if ((UnityEngine.Object) this.colonyCloudButton != (UnityEngine.Object) null)
      this.colonyCloudButton.onClick += (System.Action) (() => this.ConvertAllToCloud());
    if ((UnityEngine.Object) this.colonyLocalButton != (UnityEngine.Object) null)
      this.colonyLocalButton.onClick += (System.Action) (() => this.ConvertAllToLocal());
    if ((UnityEngine.Object) this.colonyInfoButton != (UnityEngine.Object) null)
      this.colonyInfoButton.onClick += (System.Action) (() => this.ShowSaveInfo());
    if (!((UnityEngine.Object) this.loadMoreButton != (UnityEngine.Object) null))
      return;
    this.loadMoreButton.onClick += (System.Action) (() =>
    {
      ++this.displayedPageCount;
      this.RefreshColonyList();
      this.ShowColonyList();
    });
  }

  private bool IsInMenu() => App.GetCurrentSceneName() == "frontend";

  private bool CloudSavesVisible() => SaveLoader.GetCloudSavesAvailable() && this.IsInMenu();

  protected override void OnActivate()
  {
    base.OnActivate();
    WorldGen.LoadSettings();
    this.SetCloudSaveInfoActive(this.CloudSavesVisible());
    this.displayedPageCount = 1;
    this.RefreshColonyList();
    this.ShowColonyList();
    bool cloudSavesAvailable = SaveLoader.GetCloudSavesAvailable();
    this.cloudTutorialBouncer.gameObject.SetActive(cloudSavesAvailable);
    if (cloudSavesAvailable && !this.cloudTutorialBouncer.IsBouncing())
    {
      int num = KPlayerPrefs.GetInt("LoadScreenCloudTutorialTimes", 0);
      if (num < 5)
      {
        this.cloudTutorialBouncer.Bounce();
        KPlayerPrefs.SetInt("LoadScreenCloudTutorialTimes", num + 1);
        KPlayerPrefs.GetInt("LoadScreenCloudTutorialTimes", 0);
      }
      else
        this.cloudTutorialBouncer.gameObject.SetActive(false);
    }
    if (!DistributionPlatform.Initialized || !SteamUtils.IsSteamRunningOnSteamDeck())
      return;
    this.colonyInfoButton.gameObject.SetActive(false);
  }

  private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetColoniesDetails(
    List<SaveLoader.SaveFileEntry> files)
  {
    Dictionary<string, List<LoadScreen.SaveGameFileDetails>> coloniesDetails = new Dictionary<string, List<LoadScreen.SaveGameFileDetails>>();
    if (files.Count <= 0)
      return coloniesDetails;
    for (int index = 0; index < files.Count; ++index)
    {
      if (this.IsFileValid(files[index].path))
      {
        Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(files[index].path);
        SaveGame.Header first = fileInfo.first;
        SaveGame.GameInfo second = fileInfo.second;
        System.DateTime timeStamp = files[index].timeStamp;
        long num = 0;
        try
        {
          num = new FileInfo(files[index].path).Length;
        }
        catch (Exception ex)
        {
          Debug.LogWarning((object) $"Failed to get size for file: {files[index].ToString()}\n{ex.ToString()}");
        }
        LoadScreen.SaveGameFileDetails saveGameFileDetails = new LoadScreen.SaveGameFileDetails();
        saveGameFileDetails.BaseName = second.baseName;
        saveGameFileDetails.FileName = files[index].path;
        saveGameFileDetails.FileDate = timeStamp;
        saveGameFileDetails.FileHeader = first;
        saveGameFileDetails.FileInfo = second;
        saveGameFileDetails.Size = num;
        saveGameFileDetails.UniqueID = SaveGame.GetSaveUniqueID(second);
        if (!coloniesDetails.ContainsKey(saveGameFileDetails.UniqueID))
          coloniesDetails.Add(saveGameFileDetails.UniqueID, new List<LoadScreen.SaveGameFileDetails>());
        coloniesDetails[saveGameFileDetails.UniqueID].Add(saveGameFileDetails);
      }
    }
    return coloniesDetails;
  }

  private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetColonies(bool sort)
  {
    return this.GetColoniesDetails(SaveLoader.GetAllFiles(sort));
  }

  private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetLocalColonies(bool sort)
  {
    return this.GetColoniesDetails(SaveLoader.GetAllFiles(sort, SaveLoader.SaveType.local));
  }

  private Dictionary<string, List<LoadScreen.SaveGameFileDetails>> GetCloudColonies(bool sort)
  {
    return this.GetColoniesDetails(SaveLoader.GetAllFiles(sort, SaveLoader.SaveType.cloud));
  }

  private bool IsFileValid(string filename)
  {
    bool flag = false;
    try
    {
      flag = SaveLoader.LoadHeader(filename, out SaveGame.Header _).saveMajorVersion >= 7;
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) $"Corrupted save file: {filename}\n{ex.ToString()}");
    }
    return flag;
  }

  private void CheckCloudLocalOverlap()
  {
    if (!SaveLoader.GetCloudSavesAvailable())
      return;
    string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
    if (cloudSavePrefix == null)
      return;
    foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> colony in this.GetColonies(false))
    {
      bool flag = false;
      List<LoadScreen.SaveGameFileDetails> saveGameFileDetailsList = new List<LoadScreen.SaveGameFileDetails>();
      foreach (LoadScreen.SaveGameFileDetails saveGameFileDetails in colony.Value)
      {
        if (SaveLoader.IsSaveCloud(saveGameFileDetails.FileName))
          flag = true;
        else
          saveGameFileDetailsList.Add(saveGameFileDetails);
      }
      if (flag && saveGameFileDetailsList.Count != 0)
      {
        string baseName = saveGameFileDetailsList[0].BaseName;
        string path1 = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), baseName);
        string path2 = System.IO.Path.Combine(cloudSavePrefix, baseName);
        if (!Directory.Exists(path2))
          Directory.CreateDirectory(path2);
        Debug.Log((object) $"Saves / Found overlapped cloud/local saves for colony '{baseName}', moving to cloud...");
        foreach (LoadScreen.SaveGameFileDetails saveGameFileDetails in saveGameFileDetailsList)
        {
          string fileName = saveGameFileDetails.FileName;
          string source = System.IO.Path.ChangeExtension(fileName, "png");
          string path1_1 = path2;
          if (SaveLoader.IsSaveAuto(fileName))
          {
            string path3 = System.IO.Path.Combine(path1_1, "auto_save");
            if (!Directory.Exists(path3))
              Directory.CreateDirectory(path3);
            path1_1 = path3;
          }
          string str = System.IO.Path.Combine(path1_1, System.IO.Path.GetFileName(fileName));
          if (this.FileMatch(fileName, str, out Tuple<bool, bool> _))
          {
            Debug.Log((object) $"Saves / file match found for `{fileName}`...");
            this.MigrateFile(fileName, str);
            string dest = System.IO.Path.ChangeExtension(str, "png");
            this.MigrateFile(source, dest, true);
          }
          else
          {
            Debug.Log((object) $"Saves / no file match found for `{fileName}`... move as copy");
            string nextUsableSavePath = SaveLoader.GetNextUsableSavePath(str);
            this.MigrateFile(fileName, nextUsableSavePath);
            string dest = System.IO.Path.ChangeExtension(nextUsableSavePath, "png");
            this.MigrateFile(source, dest, true);
          }
        }
        this.RemoveEmptyFolder(path1);
      }
    }
  }

  private void DeleteFileAndEmptyFolder(string file)
  {
    if (File.Exists(file))
      File.Delete(file);
    this.RemoveEmptyFolder(System.IO.Path.GetDirectoryName(file));
  }

  private void RemoveEmptyFolder(string path)
  {
    if (!Directory.Exists(path) || !File.GetAttributes(path).HasFlag((Enum) FileAttributes.Directory))
      return;
    if (Directory.EnumerateFileSystemEntries(path).Any<string>())
      return;
    try
    {
      Directory.Delete(path);
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) $"Failed to remove empty directory `{path}`...");
      Debug.LogWarning((object) ex);
    }
  }

  private void RefreshColonyList()
  {
    if (this.colonyListPool != null)
      this.colonyListPool.ClearAll();
    this.CheckCloudLocalOverlap();
    Dictionary<string, List<LoadScreen.SaveGameFileDetails>> colonies = this.GetColonies(true);
    if (colonies.Count <= 0)
      return;
    int num = 0;
    foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in colonies)
    {
      if (num < this.displayedPageCount * 20)
      {
        this.AddColonyToList(keyValuePair.Value);
        ++num;
      }
      else
        break;
    }
    this.loadMoreButton.gameObject.SetActive(colonies.Count != num);
    this.loadMoreButton.gameObject.transform.SetAsLastSibling();
  }

  private string GetFileHash(string path)
  {
    using (MD5 md5 = MD5.Create())
    {
      using (FileStream inputStream = File.OpenRead(path))
        return BitConverter.ToString(md5.ComputeHash((Stream) inputStream)).Replace("-", "").ToLowerInvariant();
    }
  }

  private bool FileMatch(string file, string other_file, out Tuple<bool, bool> matches)
  {
    matches = new Tuple<bool, bool>(false, false);
    if (!File.Exists(file) || !File.Exists(other_file))
      return false;
    bool flag1;
    bool flag2;
    try
    {
      string fileHash1 = this.GetFileHash(file);
      string fileHash2 = this.GetFileHash(other_file);
      flag1 = new FileInfo(file).Length == new FileInfo(other_file).Length;
      string str = fileHash2;
      flag2 = fileHash1 == str;
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) $"FileMatch / file match failed for `{file}` vs `{other_file}`!");
      Debug.LogWarning((object) ex);
      return false;
    }
    matches.first = flag1;
    matches.second = flag2;
    return flag1 & flag2;
  }

  private bool MigrateFile(string source, string dest, bool ignoreMissing = false)
  {
    Debug.Log((object) $"Migration / moving `{source}` to `{dest}` ...");
    if (dest == source)
    {
      Debug.Log((object) $"Migration / ignored `{source}` to `{dest}` ... same location");
      return true;
    }
    if (this.FileMatch(source, dest, out Tuple<bool, bool> _))
    {
      Debug.Log((object) "Migration / dest and source are identical size + hash ... removing original");
      try
      {
        this.DeleteFileAndEmptyFolder(source);
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) $"Migration / removing original failed for `{source}`!");
        Debug.LogWarning((object) ex);
        throw ex;
      }
      return true;
    }
    try
    {
      Debug.Log((object) "Migration / copying...");
      File.Copy(source, dest, false);
    }
    catch (FileNotFoundException ex) when (ignoreMissing)
    {
      Debug.Log((object) $"Migration / File `{source}` wasn't found but we're ignoring that.");
      return true;
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) $"Migration / copy failed for `{source}`! Leaving it alone");
      Debug.LogWarning((object) ex);
      Debug.LogWarning((object) ("failed to convert colony: " + ex.ToString()));
      throw ex;
    }
    Debug.Log((object) "Migration / copy ok ...");
    Tuple<bool, bool> matches;
    if (!this.FileMatch(source, dest, out matches))
    {
      Debug.LogWarning((object) $"Migration / failed to match dest file for `{source}`!");
      Debug.LogWarning((object) $"Migration / did hash match? {matches.second} did size match? {matches.first}");
      throw new Exception("Hash/Size didn't match for source and destination");
    }
    Debug.Log((object) "Migration / hash validation ok ... removing original");
    try
    {
      this.DeleteFileAndEmptyFolder(source);
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) $"Migration / removing original failed for `{source}`!");
      Debug.LogWarning((object) ex);
      throw ex;
    }
    Debug.Log((object) $"Migration / moved ok for `{source}`!");
    return true;
  }

  private bool MigrateSave(string dest_root, string file, bool is_auto_save, out string saveError)
  {
    saveError = (string) null;
    Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(file);
    SaveGame.Header first = fileInfo.first;
    string path2 = fileInfo.second.baseName.TrimEnd(' ');
    string fileName = System.IO.Path.GetFileName(file);
    string str1 = System.IO.Path.Combine(dest_root, path2);
    if (!Directory.Exists(str1))
      str1 = Directory.CreateDirectory(str1).FullName;
    string path1 = str1;
    if (is_auto_save)
    {
      string path = System.IO.Path.Combine(str1, "auto_save");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      path1 = path;
    }
    string str2 = System.IO.Path.Combine(path1, fileName);
    string source = System.IO.Path.ChangeExtension(file, "png");
    string dest = System.IO.Path.ChangeExtension(str2, "png");
    try
    {
      this.MigrateFile(file, str2);
      this.MigrateFile(source, dest, true);
    }
    catch (Exception ex)
    {
      saveError = ex.Message;
      return false;
    }
    return true;
  }

  private (int, int, ulong) GetSavesSizeAndCounts(List<LoadScreen.SaveGameFileDetails> list)
  {
    ulong num1 = 0;
    int num2 = 0;
    int num3 = 0;
    for (int index = 0; index < list.Count; ++index)
    {
      LoadScreen.SaveGameFileDetails saveGameFileDetails = list[index];
      num1 += (ulong) saveGameFileDetails.Size;
      if (saveGameFileDetails.FileInfo.isAutoSave)
        ++num3;
      else
        ++num2;
    }
    return (num2, num3, num1);
  }

  private int CountValidSaves(string path, SearchOption searchType = SearchOption.AllDirectories)
  {
    int num = 0;
    List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(path, false, searchType);
    for (int index = 0; index < saveFiles.Count; ++index)
    {
      if (this.IsFileValid(saveFiles[index].path))
        ++num;
    }
    return num;
  }

  private (int, int) GetMigrationSaveCounts()
  {
    return (this.CountValidSaves(SaveLoader.GetSavePrefixAndCreateFolder(), SearchOption.TopDirectoryOnly), this.CountValidSaves(SaveLoader.GetAutoSavePrefix()));
  }

  private (int, int) MigrateSaves(out string errorColony, out string errorMessage)
  {
    errorColony = (string) null;
    errorMessage = (string) null;
    int num1 = 0;
    string prefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
    List<SaveLoader.SaveFileEntry> saveFiles1 = SaveLoader.GetSaveFiles(prefixAndCreateFolder, false, SearchOption.TopDirectoryOnly);
    for (int index = 0; index < saveFiles1.Count; ++index)
    {
      SaveLoader.SaveFileEntry saveFileEntry = saveFiles1[index];
      if (this.IsFileValid(saveFileEntry.path))
      {
        string saveError;
        if (this.MigrateSave(prefixAndCreateFolder, saveFileEntry.path, false, out saveError))
          ++num1;
        else if (errorColony == null)
        {
          errorColony = saveFileEntry.path;
          errorMessage = saveError;
        }
      }
    }
    int num2 = 0;
    List<SaveLoader.SaveFileEntry> saveFiles2 = SaveLoader.GetSaveFiles(SaveLoader.GetAutoSavePrefix(), false);
    for (int index = 0; index < saveFiles2.Count; ++index)
    {
      SaveLoader.SaveFileEntry saveFileEntry = saveFiles2[index];
      if (this.IsFileValid(saveFileEntry.path))
      {
        string saveError;
        if (this.MigrateSave(prefixAndCreateFolder, saveFileEntry.path, true, out saveError))
          ++num2;
        else if (errorColony == null)
        {
          errorColony = saveFileEntry.path;
          errorMessage = saveError;
        }
      }
    }
    return (num1, num2);
  }

  public void ShowMigrationIfNecessary(bool fromMainMenu)
  {
    (int num1, int num2) = this.GetMigrationSaveCounts();
    if (num1 == 0 && num2 == 0)
    {
      if (!fromMainMenu)
        return;
      this.Deactivate();
    }
    else
    {
      this.Activate();
      this.migrationPanelRefs.gameObject.SetActive(true);
      KButton migrateButton = this.migrationPanelRefs.GetReference<RectTransform>("MigrateSaves").GetComponent<KButton>();
      KButton continueButton = this.migrationPanelRefs.GetReference<RectTransform>("Continue").GetComponent<KButton>();
      KButton moreInfoButton = this.migrationPanelRefs.GetReference<RectTransform>("MoreInfo").GetComponent<KButton>();
      KButton component = this.migrationPanelRefs.GetReference<RectTransform>("OpenSaves").GetComponent<KButton>();
      LocText statsText = this.migrationPanelRefs.GetReference<RectTransform>("CountText").GetComponent<LocText>();
      LocText infoText = this.migrationPanelRefs.GetReference<RectTransform>("InfoText").GetComponent<LocText>();
      migrateButton.gameObject.SetActive(true);
      continueButton.gameObject.SetActive(false);
      moreInfoButton.gameObject.SetActive(false);
      statsText.text = string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_COUNT, (object) num1, (object) num2);
      component.ClearOnClick();
      component.onClick += (System.Action) (() => App.OpenWebURL(SaveLoader.GetSavePrefixAndCreateFolder()));
      migrateButton.ClearOnClick();
      migrateButton.onClick += (System.Action) (() =>
      {
        migrateButton.gameObject.SetActive(false);
        string errorColony;
        string errorMessage;
        (int num5, int num6) = this.MigrateSaves(out errorColony, out errorMessage);
        bool flag = errorColony == null;
        string format = flag ? STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT.text : STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES.Replace("{ErrorColony}", errorColony).Replace("{ErrorMessage}", errorMessage);
        statsText.text = string.Format(format, (object) num5, (object) num1, (object) num6, (object) num2);
        infoText.gameObject.SetActive(false);
        if (flag)
          continueButton.gameObject.SetActive(true);
        else
          moreInfoButton.gameObject.SetActive(true);
        MainMenu.Instance.RefreshResumeButton();
        this.RefreshColonyList();
      });
      continueButton.ClearOnClick();
      continueButton.onClick += (System.Action) (() =>
      {
        this.migrationPanelRefs.gameObject.SetActive(false);
        this.cloudTutorialBouncer.Bounce();
      });
      moreInfoButton.ClearOnClick();
      moreInfoButton.onClick += (System.Action) (() =>
      {
        if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
          Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.gameObject).SetHeader((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_TITLE).AddPlainText((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_PRE).AddLineItem((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM1, "").AddLineItem((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM2, "").AddLineItem((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM3, "").AddPlainText((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_POST).AddOption((string) STRINGS.UI.CONFIRMDIALOG.OK, (Action<InfoDialogScreen>) (d =>
          {
            this.migrationPanelRefs.gameObject.SetActive(false);
            this.cloudTutorialBouncer.Bounce();
            d.Deactivate();
          }), true).Activate();
        else
          Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.gameObject).SetHeader((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_TITLE).AddPlainText((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_PRE).AddLineItem((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM1, "").AddLineItem((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM2, "").AddLineItem((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM3, "").AddPlainText((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_RESULT_FAILURES_MORE_INFO_POST).AddOption((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_FAILURES_FORUM_BUTTON, (Action<InfoDialogScreen>) (d => App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/"))).AddOption((string) STRINGS.UI.CONFIRMDIALOG.OK, (Action<InfoDialogScreen>) (d =>
          {
            this.migrationPanelRefs.gameObject.SetActive(false);
            this.cloudTutorialBouncer.Bounce();
            d.Deactivate();
          }), true).Activate();
      });
    }
  }

  private void SetCloudSaveInfoActive(bool active)
  {
    this.colonyCloudButton.gameObject.SetActive(active);
    this.colonyLocalButton.gameObject.SetActive(active);
  }

  private bool ConvertToLocalOrCloud(string fromRoot, string destRoot, string colonyName)
  {
    string sourceDirName = System.IO.Path.Combine(fromRoot, colonyName);
    string destDirName = System.IO.Path.Combine(destRoot, colonyName);
    Debug.Log((object) $"Convert / Colony '{colonyName}' from `{sourceDirName}` => `{destDirName}`");
    try
    {
      Directory.Move(sourceDirName, destDirName);
      return true;
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) ("failed to convert colony: " + ex.ToString()));
      this.ShowConvertError(STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_ERROR.Replace("{Colony}", colonyName).Replace("{Error}", ex.Message));
    }
    return false;
  }

  private bool ConvertColonyToCloud(string colonyName)
  {
    string savePrefix = SaveLoader.GetSavePrefix();
    string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
    if (cloudSavePrefix != null)
      return this.ConvertToLocalOrCloud(savePrefix, cloudSavePrefix, colonyName);
    Debug.LogWarning((object) "Failed to move colony to cloud, no cloud save prefix found (usually a userID is missing, not logged in?)");
    return false;
  }

  private bool ConvertColonyToLocal(string colonyName)
  {
    string savePrefix = SaveLoader.GetSavePrefix();
    string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
    if (cloudSavePrefix != null)
      return this.ConvertToLocalOrCloud(cloudSavePrefix, savePrefix, colonyName);
    Debug.LogWarning((object) "Failed to move colony from cloud, no cloud save prefix found (usually a userID is missing, not logged in?)");
    return false;
  }

  private void DoConvertAllToLocal()
  {
    Dictionary<string, List<LoadScreen.SaveGameFileDetails>> cloudColonies = this.GetCloudColonies(false);
    if (cloudColonies.Count == 0)
      return;
    bool flag = true;
    foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in cloudColonies)
      flag &= this.ConvertColonyToLocal(keyValuePair.Value[0].BaseName);
    if (flag)
    {
      string steam = (string) STRINGS.UI.PLATFORMS.STEAM;
      this.ShowSimpleDialog((string) STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL, STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_ALL_TO_LOCAL_SUCCESS.Replace("{Client}", steam));
    }
    this.RefreshColonyList();
    MainMenu.Instance.RefreshResumeButton();
    SaveLoader.SetCloudSavesDefault(false);
  }

  private void DoConvertAllToCloud()
  {
    Dictionary<string, List<LoadScreen.SaveGameFileDetails>> localColonies = this.GetLocalColonies(false);
    if (localColonies.Count == 0)
      return;
    List<string> stringList = new List<string>();
    foreach (KeyValuePair<string, List<LoadScreen.SaveGameFileDetails>> keyValuePair in localColonies)
    {
      string baseName = keyValuePair.Value[0].BaseName;
      if (!stringList.Contains(baseName))
        stringList.Add(baseName);
    }
    bool flag = true;
    foreach (string colonyName in stringList)
      flag &= this.ConvertColonyToCloud(colonyName);
    if (flag)
    {
      string steam = (string) STRINGS.UI.PLATFORMS.STEAM;
      this.ShowSimpleDialog((string) STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD, STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_ALL_TO_CLOUD_SUCCESS.Replace("{Client}", steam));
    }
    this.RefreshColonyList();
    MainMenu.Instance.RefreshResumeButton();
    SaveLoader.SetCloudSavesDefault(true);
  }

  private void ConvertAllToCloud()
  {
    string message = $"{STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD_DETAILS}\n{STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_ALL_WARNING}\n";
    KPlayerPrefs.SetInt("LoadScreenCloudTutorialTimes", 5);
    this.ConfirmCloudSaveMigrations(message, (string) STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD, (string) STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_ALL_COLONIES, (string) STRINGS.UI.FRONTEND.LOADSCREEN.OPEN_SAVE_FOLDER, (System.Action) (() => this.DoConvertAllToCloud()), (System.Action) (() => App.OpenWebURL(SaveLoader.GetSavePrefix())), this.localToCloudSprite);
  }

  private void ConvertAllToLocal()
  {
    string message = $"{STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL_DETAILS}\n{STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_ALL_WARNING}\n";
    KPlayerPrefs.SetInt("LoadScreenCloudTutorialTimes", 5);
    this.ConfirmCloudSaveMigrations(message, (string) STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL, (string) STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_ALL_COLONIES, (string) STRINGS.UI.FRONTEND.LOADSCREEN.OPEN_SAVE_FOLDER, (System.Action) (() => this.DoConvertAllToLocal()), (System.Action) (() => App.OpenWebURL(SaveLoader.GetCloudSavePrefix())), this.cloudToLocalSprite);
  }

  private void ShowSaveInfo()
  {
    if (!((UnityEngine.Object) this.infoScreen == (UnityEngine.Object) null))
      return;
    this.infoScreen = Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.gameObject).SetHeader((string) STRINGS.UI.FRONTEND.LOADSCREEN.SAVE_INFO_DIALOG_TITLE).AddSprite(this.infoSprite).AddPlainText((string) STRINGS.UI.FRONTEND.LOADSCREEN.SAVE_INFO_DIALOG_TEXT).AddOption((string) STRINGS.UI.FRONTEND.LOADSCREEN.OPEN_SAVE_FOLDER, (Action<InfoDialogScreen>) (d => App.OpenWebURL(SaveLoader.GetSavePrefix())), true).AddDefaultCancel();
    string cloudRoot = SaveLoader.GetCloudSavePrefix();
    if (cloudRoot != null && this.CloudSavesVisible())
      this.infoScreen.AddOption((string) STRINGS.UI.FRONTEND.LOADSCREEN.OPEN_CLOUDSAVE_FOLDER, (Action<InfoDialogScreen>) (d => App.OpenWebURL(cloudRoot)), true);
    this.infoScreen.gameObject.SetActive(true);
  }

  protected override void OnDeactivate()
  {
    if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null)
      SpeedControlScreen.Instance.Unpause(false);
    this.selectedSave = (LoadScreen.SelectedSave) null;
    base.OnDeactivate();
  }

  private void ShowColonyList()
  {
    this.colonyListRoot.SetActive(true);
    this.colonyViewRoot.SetActive(false);
    this.currentColony = (List<LoadScreen.SaveGameFileDetails>) null;
    this.selectedSave = (LoadScreen.SelectedSave) null;
  }

  private bool CheckSaveVersion(LoadScreen.SaveGameFileDetails save, LocText display)
  {
    if (LoadScreen.IsSaveFileFromUnsupportedFutureBuild(save.FileHeader, save.FileInfo))
    {
      if ((UnityEngine.Object) display != (UnityEngine.Object) null)
        display.text = string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.SAVE_TOO_NEW, (object) save.FileName, (object) save.FileHeader.buildVersion, (object) save.FileInfo.saveMinorVersion, (object) 744825U, (object) 38);
      return false;
    }
    if (save.FileInfo.saveMajorVersion >= 7)
      return true;
    if ((UnityEngine.Object) display != (UnityEngine.Object) null)
      display.text = string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.UNSUPPORTED_SAVE_VERSION, (object) save.FileName, (object) save.FileInfo.saveMajorVersion, (object) save.FileInfo.saveMinorVersion, (object) 7, (object) 38);
    return false;
  }

  private bool CheckSaveDLCsCompatable(LoadScreen.SaveGameFileDetails save)
  {
    return save.FileInfo.IsCompatableWithCurrentDlcConfiguration(out HashSet<string> _, out HashSet<string> _);
  }

  private string GetSaveDLCIncompatabilityTooltip(LoadScreen.SaveGameFileDetails save)
  {
    HashSet<string> dlcIdsToEnable;
    HashSet<string> dlcIdToDisable;
    string incompatabilityTooltip;
    if (save.FileInfo.IsCompatableWithCurrentDlcConfiguration(out dlcIdsToEnable, out dlcIdToDisable))
    {
      incompatabilityTooltip = (string) null;
    }
    else
    {
      incompatabilityTooltip = (string) STRINGS.UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION;
      foreach (string dlcId in dlcIdsToEnable)
        incompatabilityTooltip = $"{incompatabilityTooltip}\n{string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_ENABLE, (object) DlcManager.GetDlcTitle(dlcId))}";
      foreach (string dlcId in dlcIdToDisable)
        incompatabilityTooltip = $"{incompatabilityTooltip}\n{string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_DISABLE, (object) DlcManager.GetDlcTitle(dlcId))}";
    }
    return incompatabilityTooltip;
  }

  private void ShowColonySave(LoadScreen.SaveGameFileDetails save)
  {
    HierarchyReferences component1 = this.colonyViewRoot.GetComponent<HierarchyReferences>();
    component1.GetReference<RectTransform>("Title").GetComponent<LocText>().text = save.BaseName;
    component1.GetReference<RectTransform>("Date").GetComponent<LocText>().text = string.Format("{0:H:mm:ss} - " + Localization.GetFileDateFormat(0), (object) save.FileDate.ToLocalTime());
    string str1 = save.FileInfo.clusterId;
    if (str1 != null && !SettingsCache.clusterLayouts.clusterCache.ContainsKey(str1))
    {
      string key = SettingsCache.GetScope("EXPANSION1_ID") + str1;
      if (SettingsCache.clusterLayouts.clusterCache.ContainsKey(key))
      {
        str1 = key;
      }
      else
      {
        DebugUtil.LogWarningArgs((object) $"Failed to find cluster {str1} including the scoped path, setting to default cluster name.");
        Debug.Log((object) ("ClusterCache: " + string.Join(",", (IEnumerable<string>) SettingsCache.clusterLayouts.clusterCache.Keys)));
        str1 = WorldGenSettings.ClusterDefaultName;
      }
    }
    ProcGen.World worldData = str1 != null ? SettingsCache.clusterLayouts.GetWorldData(str1, 0) : (ProcGen.World) null;
    string str2 = worldData != null ? (string) Strings.Get(worldData.name) : " - ";
    component1.GetReference<LocText>("InfoWorld").text = string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.COLONY_INFO_FMT, (object) STRINGS.UI.FRONTEND.LOADSCREEN.WORLD_NAME, (object) str2);
    component1.GetReference<LocText>("InfoCycles").text = string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.COLONY_INFO_FMT, (object) STRINGS.UI.FRONTEND.LOADSCREEN.CYCLES_SURVIVED, (object) save.FileInfo.numberOfCycles);
    component1.GetReference<LocText>("InfoDupes").text = string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.COLONY_INFO_FMT, (object) STRINGS.UI.FRONTEND.LOADSCREEN.DUPLICANTS_ALIVE, (object) save.FileInfo.numberOfDuplicants);
    LocText component2 = component1.GetReference<RectTransform>("FileSize").GetComponent<LocText>();
    string formattedBytes = GameUtil.GetFormattedBytes((ulong) save.Size);
    string str3 = string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.COLONY_FILE_SIZE, (object) formattedBytes);
    component2.text = str3;
    component1.GetReference<RectTransform>("Filename").GetComponent<LocText>().text = string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.COLONY_FILE_NAME, (object) System.IO.Path.GetFileName(save.FileName));
    LocText component3 = component1.GetReference<RectTransform>("AutoInfo").GetComponent<LocText>();
    component3.gameObject.SetActive(!this.CheckSaveVersion(save, component3));
    Image component4 = component1.GetReference<RectTransform>("Preview").GetComponent<Image>();
    this.SetPreview(save.FileName, save.BaseName, component4);
    KButton component5 = component1.GetReference<RectTransform>("DeleteButton").GetComponent<KButton>();
    component5.ClearOnClick();
    component5.onClick += (System.Action) (() => this.Delete((System.Action) (() =>
    {
      int num = this.currentColony.IndexOf(save);
      this.currentColony.Remove(save);
      this.ShowColony(this.currentColony, num - 1);
    })));
  }

  private void ShowColony(List<LoadScreen.SaveGameFileDetails> saves, int selectIndex = -1)
  {
    if (saves.Count <= 0)
    {
      this.RefreshColonyList();
      this.ShowColonyList();
    }
    else
    {
      this.currentColony = saves;
      this.colonyListRoot.SetActive(false);
      this.colonyViewRoot.SetActive(true);
      string baseName = saves[0].BaseName;
      HierarchyReferences component1 = this.colonyViewRoot.GetComponent<HierarchyReferences>();
      KButton component2 = component1.GetReference<RectTransform>("Back").GetComponent<KButton>();
      component2.ClearOnClick();
      component2.onClick += (System.Action) (() => this.ShowColonyList());
      component1.GetReference<RectTransform>("ColonyTitle").GetComponent<LocText>().text = string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.COLONY_TITLE, (object) baseName);
      GameObject gameObject1 = component1.GetReference<RectTransform>("Content").gameObject;
      RectTransform reference1 = component1.GetReference<RectTransform>("SaveTemplate");
      for (int index = 0; index < gameObject1.transform.childCount; ++index)
      {
        GameObject gameObject2 = gameObject1.transform.GetChild(index).gameObject;
        if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null && gameObject2.name.Contains("Clone"))
          UnityEngine.Object.Destroy((UnityEngine.Object) gameObject2);
      }
      if (selectIndex < 0)
        selectIndex = 0;
      if (selectIndex > saves.Count - 1)
        selectIndex = saves.Count - 1;
      for (int index1 = 0; index1 < saves.Count; ++index1)
      {
        LoadScreen.SaveGameFileDetails save = saves[index1];
        RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(reference1, gameObject1.transform);
        HierarchyReferences component3 = rectTransform.GetComponent<HierarchyReferences>();
        rectTransform.gameObject.SetActive(true);
        component3.GetReference<RectTransform>("AutoLabel").gameObject.SetActive(save.FileInfo.isAutoSave);
        component3.GetReference<RectTransform>("SaveText").GetComponent<LocText>().text = System.IO.Path.GetFileNameWithoutExtension(save.FileName);
        component3.GetReference<RectTransform>("DateText").GetComponent<LocText>().text = string.Format("{0:H:mm:ss} - " + Localization.GetFileDateFormat(0), (object) save.FileDate.ToLocalTime());
        component3.GetReference<RectTransform>("NewestLabel").gameObject.SetActive(index1 == 0);
        RectTransform reference2 = component3.GetReference<RectTransform>("DLCIconPrefab");
        foreach (string dlcId in save.FileInfo.dlcIds)
        {
          if (DlcManager.IsUnknownDlc(dlcId))
          {
            GameObject gameObject3 = Util.KInstantiateUI(reference2.gameObject, reference2.transform.parent.gameObject, true);
            gameObject3.GetComponent<Image>().sprite = Assets.GetSprite((HashedString) DlcManager.GetDlcSmallLogo(dlcId));
            gameObject3.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_USES_DLC, (object) STRINGS.UI.UNKNOWN_DLC.NAME));
          }
        }
        for (int index2 = DlcManager.RELEASED_VERSIONS.Count - 1; index2 >= 0; --index2)
        {
          string dlcId = DlcManager.RELEASED_VERSIONS[index2];
          if (!DlcManager.IsVanillaId(dlcId) && save.FileInfo.dlcIds.Contains(dlcId))
          {
            GameObject gameObject4 = Util.KInstantiateUI(reference2.gameObject, reference2.transform.parent.gameObject, true);
            gameObject4.GetComponent<Image>().sprite = Assets.GetSprite((HashedString) DlcManager.GetDlcSmallLogo(dlcId));
            gameObject4.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_USES_DLC, (object) DlcManager.GetDlcTitle(dlcId)));
          }
        }
        int num = !this.CheckSaveVersion(save, (LocText) null) ? 0 : (this.CheckSaveDLCsCompatable(save) ? 1 : 0);
        KButton button = rectTransform.GetComponent<KButton>();
        button.ClearOnClick();
        button.onClick += (System.Action) (() =>
        {
          this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
          this.ShowColonySave(save);
        });
        if (num != 0)
          button.onDoubleClick += (System.Action) (() =>
          {
            this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
            this.Load();
          });
        KButton component4 = component3.GetReference<RectTransform>("LoadButton").GetComponent<KButton>();
        component4.ClearOnClick();
        if (num == 0)
        {
          component4.isInteractable = false;
          component4.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Disabled);
          component4.GetComponent<ToolTip>().SetSimpleTooltip(this.GetSaveDLCIncompatabilityTooltip(save));
        }
        else
          component4.onClick += (System.Action) (() =>
          {
            this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
            this.Load();
          });
        if (index1 == selectIndex)
        {
          this.UpdateSelected(button, save.FileName, save.FileInfo.dlcIds);
          this.ShowColonySave(save);
        }
      }
    }
  }

  private void AddColonyToList(List<LoadScreen.SaveGameFileDetails> saves)
  {
    if (saves.Count == 0)
      return;
    HierarchyReferences freeElement = this.colonyListPool.GetFreeElement(this.saveButtonRoot, true);
    saves.Sort((Comparison<LoadScreen.SaveGameFileDetails>) ((x, y) => y.FileDate.CompareTo(x.FileDate)));
    LoadScreen.SaveGameFileDetails save = saves[0];
    string colonyName = save.BaseName;
    (int, int, ulong) savesSizeAndCounts = this.GetSavesSizeAndCounts(saves);
    int num1 = savesSizeAndCounts.Item1;
    int num2 = savesSizeAndCounts.Item2;
    string formattedBytes = GameUtil.GetFormattedBytes(savesSizeAndCounts.Item3);
    freeElement.GetReference<RectTransform>("HeaderTitle").GetComponent<LocText>().text = colonyName;
    freeElement.GetReference<RectTransform>("HeaderDate").GetComponent<LocText>().text = string.Format("{0:H:mm:ss} - " + Localization.GetFileDateFormat(0), (object) save.FileDate.ToLocalTime());
    freeElement.GetReference<RectTransform>("SaveTitle").GetComponent<LocText>().text = string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.SAVE_INFO, (object) num1, (object) num2, (object) formattedBytes);
    Image component1 = freeElement.GetReference<RectTransform>("Preview").GetComponent<Image>();
    this.SetPreview(save.FileName, colonyName, component1, true);
    List<(Sprite, string)> valueTupleList = new List<(Sprite, string)>();
    foreach (string dlcId in DlcManager.RELEASED_VERSIONS)
    {
      if (!DlcManager.IsVanillaId(dlcId) && save.FileInfo.dlcIds.Contains(dlcId))
        valueTupleList.Add((Assets.GetSprite((HashedString) DlcManager.GetDlcSmallLogo(dlcId)), string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_USES_DLC, (object) DlcManager.GetDlcTitle(dlcId))));
    }
    foreach (string dlcId in save.FileInfo.dlcIds)
    {
      if (DlcManager.IsUnknownDlc(dlcId))
        valueTupleList.Add((Assets.GetSprite((HashedString) DlcManager.GetDlcSmallLogo("unknown")), string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.TOOLTIP_SAVE_USES_DLC, (object) STRINGS.UI.UNKNOWN_DLC.NAME)));
    }
    GameObject gameObject1 = freeElement.transform.Find("Header").Find("DlcIcons").Find("Prefab_DlcIcon").gameObject;
    gameObject1.SetActive(false);
    for (int index = 0; index < gameObject1.transform.parent.childCount; ++index)
    {
      GameObject gameObject2 = gameObject1.transform.parent.GetChild(index).gameObject;
      if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) gameObject1)
        UnityEngine.Object.Destroy((UnityEngine.Object) gameObject2);
    }
    foreach ((Sprite, string) valueTuple in valueTupleList)
    {
      GameObject gameObject3 = Util.KInstantiateUI(gameObject1, gameObject1.transform.parent.gameObject, true);
      Image component2 = gameObject3.GetComponent<Image>();
      ToolTip component3 = gameObject3.GetComponent<ToolTip>();
      component2.sprite = valueTuple.Item1;
      string message = valueTuple.Item2;
      component3.SetSimpleTooltip(message);
    }
    RectTransform reference = freeElement.GetReference<RectTransform>("LocationIcons");
    bool flag = this.CloudSavesVisible();
    reference.gameObject.SetActive(flag);
    if (flag)
    {
      LocText locationText = freeElement.GetReference<RectTransform>("LocationText").GetComponent<LocText>();
      bool isLocal = SaveLoader.IsSaveLocal(save.FileName);
      locationText.text = (string) (isLocal ? STRINGS.UI.FRONTEND.LOADSCREEN.LOCAL_SAVE : STRINGS.UI.FRONTEND.LOADSCREEN.CLOUD_SAVE);
      KButton cloudButton = freeElement.GetReference<RectTransform>("CloudButton").GetComponent<KButton>();
      KButton localButton = freeElement.GetReference<RectTransform>("LocalButton").GetComponent<KButton>();
      cloudButton.gameObject.SetActive(!isLocal);
      cloudButton.ClearOnClick();
      cloudButton.onClick += (System.Action) (() => this.ConfirmCloudSaveMigrations($"{STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL_DETAILS}\n", (string) STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_TO_LOCAL, (string) STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_COLONY, (string) null, (System.Action) (() =>
      {
        cloudButton.gameObject.SetActive(false);
        isLocal = true;
        locationText.text = (string) (isLocal ? STRINGS.UI.FRONTEND.LOADSCREEN.LOCAL_SAVE : STRINGS.UI.FRONTEND.LOADSCREEN.CLOUD_SAVE);
        this.ConvertColonyToLocal(colonyName);
        this.RefreshColonyList();
        MainMenu.Instance.RefreshResumeButton();
      }), (System.Action) null, this.cloudToLocalSprite));
      localButton.gameObject.SetActive(isLocal);
      localButton.ClearOnClick();
      localButton.onClick += (System.Action) (() => this.ConfirmCloudSaveMigrations($"{STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD_DETAILS}\n", (string) STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_TO_CLOUD, (string) STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_COLONY, (string) null, (System.Action) (() =>
      {
        localButton.gameObject.SetActive(false);
        isLocal = false;
        locationText.text = (string) (isLocal ? STRINGS.UI.FRONTEND.LOADSCREEN.LOCAL_SAVE : STRINGS.UI.FRONTEND.LOADSCREEN.CLOUD_SAVE);
        this.ConvertColonyToCloud(colonyName);
        this.RefreshColonyList();
        MainMenu.Instance.RefreshResumeButton();
      }), (System.Action) null, this.localToCloudSprite));
    }
    this.GetSaveDLCIncompatabilityTooltip(save);
    freeElement.GetReference<RectTransform>("Button").GetComponent<KButton>().onClick += (System.Action) (() => this.ShowColony(saves));
    freeElement.transform.SetAsLastSibling();
  }

  private void SetPreview(
    string filename,
    string basename,
    Image preview,
    bool fallbackToTimelapse = false)
  {
    preview.color = Color.black;
    preview.gameObject.SetActive(false);
    try
    {
      Sprite sprite = RetireColonyUtility.LoadColonyPreview(filename, basename, fallbackToTimelapse);
      if ((UnityEngine.Object) sprite == (UnityEngine.Object) null)
        return;
      UnityEngine.Rect rect = preview.rectTransform.parent.rectTransform().rect;
      preview.sprite = sprite;
      preview.color = (bool) (UnityEngine.Object) sprite ? Color.white : Color.black;
      float num = sprite.bounds.size.x / sprite.bounds.size.y;
      if ((double) num >= 16.0 / 9.0)
        preview.rectTransform.sizeDelta = new Vector2(rect.height * num, rect.height);
      else
        preview.rectTransform.sizeDelta = new Vector2(rect.width, rect.width / num);
      preview.gameObject.SetActive(true);
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void ForceStopGame()
  {
    ThreadedHttps<KleiMetrics>.Instance.ClearGameFields();
    ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
    Game.Instance.SetIsLoading();
    Grid.CellCount = 0;
    Sim.Shutdown();
  }

  private static bool IsSaveFileFromUnsupportedFutureBuild(
    SaveGame.Header header,
    SaveGame.GameInfo gameInfo)
  {
    return gameInfo.saveMajorVersion > 7 || gameInfo.saveMajorVersion == 7 && gameInfo.saveMinorVersion > 38 || header.buildVersion > 744825U;
  }

  private void UpdateSelected(KButton button, string filename, List<string> dlcIds)
  {
    if (this.selectedSave != null && (UnityEngine.Object) this.selectedSave.button != (UnityEngine.Object) null)
      this.selectedSave.button.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Inactive);
    if (this.selectedSave == null)
      this.selectedSave = new LoadScreen.SelectedSave();
    this.selectedSave.button = button;
    this.selectedSave.filename = filename;
    this.selectedSave.dlcIds = dlcIds;
    if (!((UnityEngine.Object) this.selectedSave.button != (UnityEngine.Object) null))
      return;
    this.selectedSave.button.GetComponent<ImageToggleState>().SetState(ImageToggleState.State.Active);
  }

  private void Load()
  {
    if (!DlcManager.IsAllContentSubscribed(this.selectedSave.dlcIds))
      this.ConfirmDoAction((string) (this.selectedSave.dlcIds.Contains("") ? STRINGS.UI.FRONTEND.LOADSCREEN.VANILLA_RESTART : STRINGS.UI.FRONTEND.LOADSCREEN.EXPANSION1_RESTART), (System.Action) (() =>
      {
        KPlayerPrefs.SetString("AutoResumeSaveFile", this.selectedSave.filename);
        DlcManager.ToggleDLC("EXPANSION1_ID");
      }));
    else
      LoadingOverlay.Load(new System.Action(this.DoLoad));
  }

  private void DoLoad()
  {
    if (this.selectedSave == null)
      return;
    LoadScreen.DoLoad(this.selectedSave.filename);
    this.Deactivate();
  }

  public static void DoLoad(string filename)
  {
    KCrashReporter.MOST_RECENT_SAVEFILE = filename;
    SaveGame.Header header;
    SaveGame.GameInfo gameInfo = SaveLoader.LoadHeader(filename, out header);
    string str1 = (string) null;
    string str2 = (string) null;
    if (header.buildVersion > 744825U)
    {
      str1 = header.buildVersion.ToString();
      str2 = 744825U.ToString();
    }
    else if (gameInfo.saveMajorVersion < 7)
    {
      str1 = $"v{gameInfo.saveMajorVersion}.{gameInfo.saveMinorVersion}";
      str2 = $"v{7}.{38}";
    }
    if (false)
    {
      Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, (UnityEngine.Object) FrontEndManager.Instance == (UnityEngine.Object) null ? GameScreenManager.Instance.ssOverlayCanvas : FrontEndManager.Instance.gameObject, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format((string) STRINGS.UI.CRASHSCREEN.LOADFAILED, (object) "Version Mismatch", (object) str1, (object) str2), (System.Action) null, (System.Action) null);
    }
    else
    {
      if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
        LoadScreen.ForceStopGame();
      SaveLoader.SetActiveSaveFilePath(filename);
      Time.timeScale = 0.0f;
      SaveLoader.LoadScene();
    }
  }

  private void MoreInfo()
  {
    App.OpenWebURL("http://support.kleientertainment.com/customer/portal/articles/2776550");
  }

  private void Delete(System.Action onDelete)
  {
    if (this.selectedSave == null || string.IsNullOrEmpty(this.selectedSave.filename))
      Debug.LogError((object) "The path provided is not valid and cannot be deleted.");
    else
      this.ConfirmDoAction(string.Format((string) STRINGS.UI.FRONTEND.LOADSCREEN.CONFIRMDELETE, (object) System.IO.Path.GetFileName(this.selectedSave.filename)), (System.Action) (() =>
      {
        try
        {
          this.DeleteFileAndEmptyFolder(this.selectedSave.filename);
          this.DeleteFileAndEmptyFolder(System.IO.Path.ChangeExtension(this.selectedSave.filename, "png"));
          if (onDelete != null)
            onDelete();
          MainMenu.Instance.RefreshResumeButton();
        }
        catch (SystemException ex)
        {
          Debug.LogError((object) ex.ToString());
        }
      }));
  }

  private void ShowSimpleDialog(string title, string message)
  {
    Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.gameObject).SetHeader(title).AddPlainText(message).AddDefaultOK().Activate();
  }

  private void ConfirmCloudSaveMigrations(
    string message,
    string title,
    string confirmText,
    string backupText,
    System.Action commitAction,
    System.Action backupAction,
    Sprite sprite)
  {
    Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.gameObject).SetHeader(title).AddSprite(sprite).AddPlainText(message).AddDefaultCancel().AddOption(confirmText, (Action<InfoDialogScreen>) (d =>
    {
      d.Deactivate();
      commitAction();
    }), true).Activate();
  }

  private void ShowConvertError(string message)
  {
    if (!((UnityEngine.Object) this.errorInfoScreen == (UnityEngine.Object) null))
      return;
    if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
    {
      this.errorInfoScreen = Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.gameObject).SetHeader((string) STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_ERROR_TITLE).AddSprite(this.errorSprite).AddPlainText(message).AddDefaultOK();
      this.errorInfoScreen.Activate();
    }
    else
    {
      this.errorInfoScreen = Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.gameObject).SetHeader((string) STRINGS.UI.FRONTEND.LOADSCREEN.CONVERT_ERROR_TITLE).AddSprite(this.errorSprite).AddPlainText(message).AddOption((string) STRINGS.UI.FRONTEND.LOADSCREEN.MIGRATE_FAILURES_FORUM_BUTTON, (Action<InfoDialogScreen>) (d => App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/"))).AddDefaultOK();
      this.errorInfoScreen.Activate();
    }
  }

  private void ConfirmDoAction(string message, System.Action action)
  {
    if (!((UnityEngine.Object) this.confirmScreen == (UnityEngine.Object) null))
      return;
    this.confirmScreen = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.gameObject);
    this.confirmScreen.PopupConfirmDialog(message, action, (System.Action) (() => { }));
    this.confirmScreen.gameObject.SetActive(true);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.currentColony != null && e.TryConsume(Action.Escape))
      this.ShowColonyList();
    base.OnKeyDown(e);
  }

  private struct SaveGameFileDetails
  {
    public string BaseName;
    public string FileName;
    public string UniqueID;
    public System.DateTime FileDate;
    public SaveGame.Header FileHeader;
    public SaveGame.GameInfo FileInfo;
    public long Size;
  }

  private class SelectedSave
  {
    public string filename;
    public List<string> dlcIds;
    public KButton button;
  }
}
