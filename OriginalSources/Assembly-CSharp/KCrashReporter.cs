// Decompiled with JetBrains decompiler
// Type: KCrashReporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Newtonsoft.Json;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

#nullable disable
public class KCrashReporter : MonoBehaviour
{
  public static string MOST_RECENT_SAVEFILE = (string) null;
  public const string CRASH_REPORTER_SERVER = "https://games-feedback.klei.com";
  public const uint MAX_LOGS = 10000000;
  public static bool ignoreAll = false;
  public static bool debugWasUsed = false;
  public static bool haveActiveMods = false;
  public static uint logCount = 0;
  public static string error_canvas_name = "ErrorCanvas";
  public static bool disableDeduping = false;
  public static bool hasCrash = false;
  private static readonly Regex failedToLoadModuleRegEx = new Regex("^Failed to load '(.*?)' with error (.*)", RegexOptions.Multiline);
  [SerializeField]
  private LoadScreen loadScreenPrefab;
  [SerializeField]
  private GameObject reportErrorPrefab;
  [SerializeField]
  private ConfirmDialogScreen confirmDialogPrefab;
  private GameObject errorScreen;
  public static bool terminateOnError = true;
  private static string dataRoot;
  private static readonly string[] IgnoreStrings = new string[5]
  {
    "Releasing render texture whose render buffer is set as Camera's target buffer with Camera.SetTargetBuffers!",
    "The profiler has run out of samples for this frame. This frame will be skipped. Increase the sample limit using Profiler.maxNumberOfSamplesPerFrame",
    "Trying to add Text (LocText) for graphic rebuild while we are already inside a graphic rebuild loop. This is not supported.",
    "Texture has out of range width / height",
    "<I> Failed to get cursor position:\r\nSuccess.\r\n"
  };
  private static HashSet<int> previouslyReportedDevNotifications;
  private static KCrashReporter.PendingReport pendingReport;
  private static KCrashReporter.PendingCrash pendingCrash;

  public static event Action<bool> onCrashReported;

  public static event Action<float> onCrashUploadProgress;

  public static bool hasReportedError { get; private set; }

  private void OnEnable()
  {
    KCrashReporter.dataRoot = Application.dataPath;
    Application.logMessageReceived += new Application.LogCallback(this.HandleLog);
    KCrashReporter.ignoreAll = true;
    string path = System.IO.Path.Combine(KCrashReporter.dataRoot, "hashes.json");
    if (File.Exists(path))
    {
      StringBuilder stringBuilder = new StringBuilder();
      MD5 md5 = MD5.Create();
      Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
      if (dictionary.Count > 0)
      {
        bool flag = true;
        foreach (KeyValuePair<string, string> keyValuePair in dictionary)
        {
          string key = keyValuePair.Key;
          string str = keyValuePair.Value;
          stringBuilder.Length = 0;
          using (FileStream inputStream = new FileStream(System.IO.Path.Combine(KCrashReporter.dataRoot, key), FileMode.Open, FileAccess.Read))
          {
            foreach (byte num in md5.ComputeHash((Stream) inputStream))
              stringBuilder.AppendFormat("{0:x2}", (object) num);
            if (stringBuilder.ToString() != str)
            {
              flag = false;
              break;
            }
          }
        }
        if (flag)
          KCrashReporter.ignoreAll = false;
      }
      else
        KCrashReporter.ignoreAll = false;
    }
    else
      KCrashReporter.ignoreAll = false;
    if (KCrashReporter.ignoreAll)
      Debug.Log((object) "Ignoring crash due to mismatched hashes.json entries.");
    if (File.Exists("ignorekcrashreporter.txt"))
    {
      KCrashReporter.ignoreAll = true;
      Debug.Log((object) "Ignoring crash due to ignorekcrashreporter.txt");
    }
    if (!Application.isEditor || GenericGameSettings.instance.enableEditorCrashReporting)
      return;
    KCrashReporter.terminateOnError = false;
  }

  private void OnDisable()
  {
    Application.logMessageReceived -= new Application.LogCallback(this.HandleLog);
  }

  private void HandleLog(string msg, string stack_trace, LogType type)
  {
    if (++KCrashReporter.logCount == 10000000U)
    {
      DebugUtil.DevLogError("Turning off logging to avoid increasing the file to an unreasonable size, please review the logs as they probably contain spam");
      Debug.DisableLogging();
    }
    if (KCrashReporter.ignoreAll)
      return;
    if (msg != null && msg.StartsWith(DebugUtil.START_CALLSTACK))
    {
      string str = msg;
      msg = str.Substring(str.IndexOf(DebugUtil.END_CALLSTACK, StringComparison.Ordinal) + DebugUtil.END_CALLSTACK.Length);
      stack_trace = str.Substring(DebugUtil.START_CALLSTACK.Length, str.IndexOf(DebugUtil.END_CALLSTACK, StringComparison.Ordinal) - DebugUtil.START_CALLSTACK.Length);
    }
    if (Array.IndexOf<string>(KCrashReporter.IgnoreStrings, msg) != -1 || msg != null && msg.StartsWith("<RI.Hid>") || msg != null && msg.StartsWith("Failed to load cursor") || msg != null && msg.StartsWith("Failed to save a temporary cursor"))
      return;
    if (type == LogType.Exception)
      RestartWarning.ShouldWarn = true;
    if (!((UnityEngine.Object) this.errorScreen == (UnityEngine.Object) null) || type != LogType.Exception && type != LogType.Error || KCrashReporter.terminateOnError && KCrashReporter.hasCrash)
      return;
    if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null)
      SpeedControlScreen.Instance.Pause(isCrashed: true);
    string error = msg;
    string stack_trace1 = stack_trace;
    if (string.IsNullOrEmpty(stack_trace1))
      stack_trace1 = new StackTrace(5, true).ToString();
    if (App.isLoading)
    {
      if (SceneInitializerLoader.deferred_error.IsValid)
        return;
      SceneInitializerLoader.deferred_error = new SceneInitializerLoader.DeferredError()
      {
        msg = error,
        stack_trace = stack_trace1
      };
    }
    else
      this.ShowDialog(error, stack_trace1);
  }

  public bool ShowDialog(
    string error,
    string stack_trace,
    bool includeSaveFile = true,
    string[] extraCategories = null,
    string[] extraFiles = null)
  {
    GenericGameSettings instance = GenericGameSettings.instance;
    if ((instance.devBootSmoke ? 1 : (!string.IsNullOrEmpty(instance.scriptedProfile.saveGame) ? 1 : 0)) != 0)
    {
      KCrashReporter.hasCrash = true;
      Debug.Log((object) "Automated test resulted in a crash. Return exit code 1.");
      App.QuitCode(1);
      return false;
    }
    if ((UnityEngine.Object) this.errorScreen != (UnityEngine.Object) null)
      return false;
    GameObject gameObject = GameObject.Find(KCrashReporter.error_canvas_name);
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
    {
      gameObject = new GameObject();
      gameObject.name = KCrashReporter.error_canvas_name;
      Canvas canvas = gameObject.AddComponent<Canvas>();
      canvas.renderMode = RenderMode.ScreenSpaceOverlay;
      canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
      canvas.sortingOrder = (int) short.MaxValue;
      gameObject.AddComponent<GraphicRaycaster>();
    }
    this.errorScreen = UnityEngine.Object.Instantiate<GameObject>(this.reportErrorPrefab, Vector3.zero, Quaternion.identity);
    this.errorScreen.transform.SetParent(gameObject.transform, false);
    ReportErrorDialog errorDialog = this.errorScreen.GetComponentInChildren<ReportErrorDialog>();
    string stackTrace = $"{error}\n\n{stack_trace}";
    if ((UnityEngine.Object) Global.Instance != (UnityEngine.Object) null && Global.Instance.modManager != null && Global.Instance.modManager.HasCrashableMods())
    {
      Exception e = DebugUtil.RetrieveLastExceptionLogged();
      Global.Instance.modManager.SearchForModsInStackTrace(e != null ? new StackTrace(e) : new StackTrace(5, true));
      Global.Instance.modManager.SearchForModsInStackTrace(stack_trace);
      errorDialog.PopupDisableModsDialog(stackTrace, new System.Action(this.OnQuitToDesktopCrashed), Global.Instance.modManager.IsInDevMode() || !KCrashReporter.terminateOnError ? new System.Action(this.OnCloseErrorDialog) : (System.Action) null);
    }
    else
      errorDialog.PopupSubmitErrorDialog(stackTrace, (System.Action) (() => KCrashReporter.ReportError(error, stack_trace, this.confirmDialogPrefab, this.errorScreen, errorDialog.UserMessage(), includeSaveFile, extraCategories, extraFiles)), new System.Action(this.OnQuitToDesktopCrashed), KCrashReporter.terminateOnError ? (System.Action) null : new System.Action(this.OnCloseErrorDialog));
    return true;
  }

  private void OnCloseErrorDialog()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.errorScreen);
    this.errorScreen = (GameObject) null;
    KCrashReporter.hasCrash = false;
    if (!((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null))
      return;
    SpeedControlScreen.Instance.Unpause();
  }

  private void OnQuitToDesktopCrashed() => App.QuitCode(1);

  private static string GetUserID()
  {
    if (!DistributionPlatform.Initialized)
      return "LocalUser_" + Environment.UserName;
    return $"{DistributionPlatform.Inst.Name}ID_{DistributionPlatform.Inst.LocalUser.Name}_{DistributionPlatform.Inst.LocalUser.Id?.ToString()}";
  }

  private static string GetLogContents()
  {
    string path = Util.LogFilePath();
    if (!File.Exists(path))
      return "";
    using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    {
      using (StreamReader streamReader = new StreamReader((Stream) fileStream))
        return streamReader.ReadToEnd();
    }
  }

  public static void ReportDevNotification(
    string notification_name,
    string stack_trace,
    string details = "",
    bool includeSaveFile = false,
    string[] extraCategories = null)
  {
    if (KCrashReporter.previouslyReportedDevNotifications == null)
      KCrashReporter.previouslyReportedDevNotifications = new HashSet<int>();
    details = $"{notification_name} - {details}";
    Debug.Log((object) details);
    int hashValue = new HashedString(notification_name).HashValue;
    bool hasReportedError = KCrashReporter.hasReportedError;
    if (!KCrashReporter.previouslyReportedDevNotifications.Contains(hashValue))
    {
      KCrashReporter.previouslyReportedDevNotifications.Add(hashValue);
      if (extraCategories != null)
      {
        Array.Resize<string>(ref extraCategories, extraCategories.Length + 1);
        extraCategories[extraCategories.Length - 1] = KCrashReporter.CRASH_CATEGORY.DEVNOTIFICATION;
      }
      else
        extraCategories = new string[1]
        {
          KCrashReporter.CRASH_CATEGORY.DEVNOTIFICATION
        };
      KCrashReporter.ReportError("DevNotification: " + notification_name, stack_trace, (ConfirmDialogScreen) null, (GameObject) null, details, includeSaveFile, extraCategories);
    }
    KCrashReporter.hasReportedError = hasReportedError;
  }

  public static void ReportError(
    string msg,
    string stack_trace,
    ConfirmDialogScreen confirm_prefab,
    GameObject confirm_parent,
    string userMessage = "",
    bool includeSaveFile = true,
    string[] extraCategories = null,
    string[] extraFiles = null)
  {
    if (KPrivacyPrefs.instance.disableDataCollection || KCrashReporter.ignoreAll)
      return;
    Debug.Log((object) "Reporting error.\n");
    if (msg != null)
      Debug.Log((object) msg);
    if (stack_trace != null)
      Debug.Log((object) stack_trace);
    KCrashReporter.hasReportedError = true;
    if (string.IsNullOrEmpty(msg))
      msg = "No message";
    System.Text.RegularExpressions.Match match = KCrashReporter.failedToLoadModuleRegEx.Match(msg);
    if (match.Success)
    {
      string path = match.Groups[1].ToString();
      string str = match.Groups[2].ToString();
      msg = $"Failed to load '{System.IO.Path.GetFileName(path)}' with error '{str}'.";
    }
    if (string.IsNullOrEmpty(stack_trace))
      stack_trace = $"No stack trace {BuildWatermark.GetBuildText()}\n\n{msg}";
    List<string> stringList = new List<string>();
    if (KCrashReporter.debugWasUsed)
      stringList.Add("(Debug Used)");
    if (KCrashReporter.haveActiveMods)
      stringList.Add("(Mods Active)");
    stringList.Add(msg);
    string[] strArray = new string[8]
    {
      "Debug:LogError",
      "UnityEngine.Debug",
      "Output:LogError",
      "DebugUtil:Assert",
      "System.Array",
      "System.Collections",
      "KCrashReporter.Assert",
      "No stack trace."
    };
    foreach (string str1 in stack_trace.Split('\n', StringSplitOptions.None))
    {
      if (stringList.Count < 5)
      {
        if (!string.IsNullOrEmpty(str1))
        {
          bool flag = false;
          foreach (string str2 in strArray)
          {
            if (str1.StartsWith(str2))
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            stringList.Add(str1);
        }
      }
      else
        break;
    }
    userMessage = userMessage == STRINGS.UI.CRASHSCREEN.BODY.text || userMessage.IsNullOrWhiteSpace() ? "" : $"[{BuildWatermark.GetBuildText()}] {userMessage}";
    userMessage = userMessage.Replace(stack_trace, "");
    KCrashReporter.Error error = new KCrashReporter.Error();
    if (extraCategories != null)
      error.categories.AddRange((IEnumerable<string>) extraCategories);
    error.callstack = stack_trace;
    if (KCrashReporter.disableDeduping)
      error.callstack = $"{error.callstack}\n{Guid.NewGuid().ToString()}";
    error.fullstack = $"{msg}\n\n{stack_trace}";
    error.summaryline = string.Join("\n", stringList.ToArray());
    error.userMessage = userMessage;
    List<string> files = new List<string>();
    if (includeSaveFile && KCrashReporter.MOST_RECENT_SAVEFILE != null)
    {
      files.Add(KCrashReporter.MOST_RECENT_SAVEFILE);
      error.saveFilename = System.IO.Path.GetFileName(KCrashReporter.MOST_RECENT_SAVEFILE);
    }
    if (extraFiles != null)
    {
      foreach (string extraFile in extraFiles)
      {
        files.Add(extraFile);
        error.extraFilenames.Add(System.IO.Path.GetFileName(extraFile));
      }
    }
    string str3 = JsonConvert.SerializeObject((object) error);
    byte[] archiveZip = KCrashReporter.CreateArchiveZip(KCrashReporter.GetLogContents(), files);
    System.Action action1 = (System.Action) (() =>
    {
      if (!((UnityEngine.Object) confirm_prefab != (UnityEngine.Object) null) || !((UnityEngine.Object) confirm_parent != (UnityEngine.Object) null))
        return;
      ((ConfirmDialogScreen) KScreenManager.Instance.StartScreen(confirm_prefab.gameObject, confirm_parent)).PopupConfirmDialog((string) STRINGS.UI.CRASHSCREEN.REPORTEDERROR_SUCCESS, (System.Action) null, (System.Action) null);
    });
    Action<long> action2 = (Action<long>) (errorCode =>
    {
      if (!((UnityEngine.Object) confirm_prefab != (UnityEngine.Object) null) || !((UnityEngine.Object) confirm_parent != (UnityEngine.Object) null))
        return;
      string text = (string) (errorCode == 413L ? STRINGS.UI.CRASHSCREEN.REPORTEDERROR_FAILURE_TOO_LARGE : STRINGS.UI.CRASHSCREEN.REPORTEDERROR_FAILURE);
      ((ConfirmDialogScreen) KScreenManager.Instance.StartScreen(confirm_prefab.gameObject, confirm_parent)).PopupConfirmDialog(text, (System.Action) null, (System.Action) null);
    });
    KCrashReporter.pendingCrash = new KCrashReporter.PendingCrash()
    {
      jsonString = str3,
      archiveData = archiveZip,
      successCallback = action1,
      failureCallback = action2
    };
  }

  private static IEnumerator SubmitCrashAsync(
    string jsonString,
    byte[] archiveData,
    System.Action successCallback,
    Action<long> failureCallback)
  {
    bool success = false;
    Uri uri = new Uri("https://games-feedback.klei.com/submit");
    List<IMultipartFormSection> multipartFormSections = new List<IMultipartFormSection>()
    {
      (IMultipartFormSection) new MultipartFormDataSection("metadata", jsonString),
      (IMultipartFormSection) new MultipartFormFileSection("archiveFile", archiveData, "Archive.zip", "application/octet-stream")
    };
    if (KleiAccount.KleiToken != null)
      multipartFormSections.Add((IMultipartFormSection) new MultipartFormDataSection("loginToken", KleiAccount.KleiToken));
    using (UnityWebRequest w = UnityWebRequest.Post(uri, multipartFormSections))
    {
      w.SendWebRequest();
      while (!w.isDone)
      {
        yield return (object) null;
        if (KCrashReporter.onCrashUploadProgress != null)
          KCrashReporter.onCrashUploadProgress(w.uploadProgress);
      }
      if (w.result == UnityWebRequest.Result.Success)
      {
        UnityEngine.Debug.Log((object) "Submitted crash!");
        if (successCallback != null)
          successCallback();
        success = true;
      }
      else
      {
        UnityEngine.Debug.Log((object) ("CrashReporter: Could not submit crash " + w.result.ToString()));
        if (failureCallback != null)
          failureCallback(w.responseCode);
      }
    }
    if (KCrashReporter.onCrashReported != null)
      KCrashReporter.onCrashReported(success);
  }

  public static void ReportBug(string msg, GameObject confirmParent)
  {
    string stack_trace = $"Bug Report From: {KCrashReporter.GetUserID()} at {System.DateTime.Now.ToString()}";
    KCrashReporter.ReportError(msg, stack_trace, ScreenPrefabs.Instance.ConfirmDialogScreen, confirmParent);
  }

  public static void Assert(bool condition, string message, string[] extraCategories = null)
  {
    if (condition || KCrashReporter.hasReportedError)
      return;
    StackTrace stackTrace = new StackTrace(1, true);
    KCrashReporter.ReportError("ASSERT: " + message, stackTrace.ToString(), (ConfirmDialogScreen) null, (GameObject) null, (string) null, extraCategories: extraCategories);
  }

  public static void ReportSimDLLCrash(string msg, string stack_trace, string dmp_filename)
  {
    if (KCrashReporter.hasReportedError)
      return;
    KCrashReporter.pendingReport = new KCrashReporter.PendingReport(msg, stack_trace, dmp_filename);
  }

  private static byte[] CreateArchiveZip(string log, List<string> files)
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      using (ZipArchive zipArchive = new ZipArchive((Stream) memoryStream, ZipArchiveMode.Create, true))
      {
        if (files != null)
        {
          foreach (string file in files)
          {
            try
            {
              if (!File.Exists(file))
              {
                UnityEngine.Debug.Log((object) ("CrashReporter: file does not exist to include: " + file));
              }
              else
              {
                using (Stream stream = zipArchive.CreateEntry(System.IO.Path.GetFileName(file), System.IO.Compression.CompressionLevel.Fastest).Open())
                {
                  byte[] buffer = File.ReadAllBytes(file);
                  stream.Write(buffer, 0, buffer.Length);
                }
              }
            }
            catch (Exception ex)
            {
              UnityEngine.Debug.Log((object) $"CrashReporter: Could not add file '{file}' to archive: {ex?.ToString()}");
            }
          }
          using (Stream stream = zipArchive.CreateEntry("Player.log", System.IO.Compression.CompressionLevel.Fastest).Open())
          {
            byte[] bytes = Encoding.UTF8.GetBytes(log);
            stream.Write(bytes, 0, bytes.Length);
          }
        }
      }
      return memoryStream.ToArray();
    }
  }

  private void Update()
  {
    if (KCrashReporter.pendingReport != null)
    {
      KCrashReporter.PendingReport pendingReport = KCrashReporter.pendingReport;
      KCrashReporter.pendingReport = (KCrashReporter.PendingReport) null;
      if (KCrashReporter.hasReportedError)
        return;
      KCrashReporter component = Global.Instance.GetComponent<KCrashReporter>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.ShowDialog(pendingReport.message, pendingReport.stack_trace, extraCategories: new string[1]
        {
          KCrashReporter.CRASH_CATEGORY.SIM
        }, extraFiles: new string[1]
        {
          pendingReport.additional_filename
        });
      else
        KCrashReporter.ReportError(pendingReport.message, pendingReport.stack_trace, (ConfirmDialogScreen) null, (GameObject) null, extraCategories: new string[1]
        {
          KCrashReporter.CRASH_CATEGORY.SIM
        }, extraFiles: new string[1]
        {
          pendingReport.additional_filename
        });
    }
    if (KCrashReporter.pendingCrash == null)
      return;
    KCrashReporter.PendingCrash pendingCrash = KCrashReporter.pendingCrash;
    KCrashReporter.pendingCrash = (KCrashReporter.PendingCrash) null;
    Debug.Log((object) "Submitting crash...");
    this.StartCoroutine(KCrashReporter.SubmitCrashAsync(pendingCrash.jsonString, pendingCrash.archiveData, pendingCrash.successCallback, pendingCrash.failureCallback));
  }

  public class CRASH_CATEGORY
  {
    public static string DEVNOTIFICATION = "DevNotification";
    public static string VANILLA = "Vanilla";
    public static string SPACEDOUT = "SpacedOut";
    public static string MODDED = "Modded";
    public static string DEBUGUSED = "DebugUsed";
    public static string SANDBOX = "Sandbox";
    public static string STEAMDECK = "SteamDeck";
    public static string SIM = "SimDll";
    public static string FILEIO = "FileIO";
    public static string MODSYSTEM = "ModSystem";
    public static string WORLDGENFAILURE = "WorldgenFailure";
  }

  private class Error
  {
    public string game = "ONI";
    public string userName;
    public string platform;
    public string version = LaunchInitializer.BuildPrefix();
    public string branch = "default";
    public string sku = "";
    public int build = 744825;
    public string callstack = "";
    public string fullstack = "";
    public string summaryline = "";
    public string userMessage = "";
    public List<string> categories = new List<string>();
    public string slackSummary;
    public string logFilename = "Player.log";
    public string saveFilename = "";
    public string screenshotFilename = "";
    public List<string> extraFilenames = new List<string>();
    public string title = "";
    public bool isServer;
    public bool isDedicated;
    public bool isError = true;
    public string emote = "";

    public Error()
    {
      this.userName = KCrashReporter.GetUserID();
      this.platform = Util.GetOperatingSystem();
      this.InitDefaultCategories();
      this.InitSku();
      this.InitSlackSummary();
      if (!DistributionPlatform.Inst.Initialized)
        return;
      string pchName;
      int num = !SteamApps.GetCurrentBetaName(out pchName, 100) ? 1 : 0;
      this.branch = pchName;
      if (pchName == "public_playtest")
        this.branch = "public_testing";
      if (num == 0 && (!(pchName == "public_testing") || UnityEngine.Debug.isDebugBuild))
        return;
      this.branch = "default";
    }

    private void InitDefaultCategories()
    {
      if (DlcManager.IsPureVanilla())
        this.categories.Add(KCrashReporter.CRASH_CATEGORY.VANILLA);
      if (DlcManager.IsExpansion1Active())
        this.categories.Add(KCrashReporter.CRASH_CATEGORY.SPACEDOUT);
      foreach (string activeDlcId in DlcManager.GetActiveDLCIds())
      {
        if (!(activeDlcId == "EXPANSION1_ID"))
          this.categories.Add(activeDlcId);
      }
      if (KCrashReporter.debugWasUsed)
        this.categories.Add(KCrashReporter.CRASH_CATEGORY.DEBUGUSED);
      if (KCrashReporter.haveActiveMods)
        this.categories.Add(KCrashReporter.CRASH_CATEGORY.MODDED);
      if ((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null && SaveGame.Instance.sandboxEnabled)
        this.categories.Add(KCrashReporter.CRASH_CATEGORY.SANDBOX);
      if (!DistributionPlatform.Inst.Initialized || !SteamUtils.IsSteamRunningOnSteamDeck())
        return;
      this.categories.Add(KCrashReporter.CRASH_CATEGORY.STEAMDECK);
    }

    private void InitSku()
    {
      this.sku = "steam";
      if (!DistributionPlatform.Inst.Initialized)
        return;
      string pchName;
      int num = !SteamApps.GetCurrentBetaName(out pchName, 100) ? 1 : 0;
      if (pchName == "public_testing" || pchName == "preview" || pchName == "public_playtest" || pchName == "playtest")
        this.sku = !UnityEngine.Debug.isDebugBuild ? "steam-release" : "steam-public-testing";
      if (num == 0 && !(pchName == "release"))
        return;
      this.sku = "steam-release";
    }

    private void InitSlackSummary()
    {
      string buildText = BuildWatermark.GetBuildText();
      string str1 = (UnityEngine.Object) GameClock.Instance != (UnityEngine.Object) null ? $" - Cycle {GameClock.Instance.GetCycle() + 1}" : "";
      int num = !((UnityEngine.Object) Global.Instance != (UnityEngine.Object) null) || Global.Instance.modManager == null ? 0 : Global.Instance.modManager.mods.Count<KMod.Mod>((Func<KMod.Mod, bool>) (x => x.IsEnabledForActiveDlc()));
      string str2 = num > 0 ? $" - {num} active mods" : "";
      this.slackSummary = $"{buildText} {this.platform}{str1}{str2}";
    }
  }

  public class PendingCrash
  {
    public string jsonString;
    public byte[] archiveData;
    public System.Action successCallback;
    public Action<long> failureCallback;
  }

  public class PendingReport
  {
    public string message;
    public string stack_trace;
    public string additional_filename;

    public PendingReport(string msg, string stack_trace, string filename)
    {
      this.message = msg;
      this.stack_trace = stack_trace;
      this.additional_filename = filename;
    }
  }
}
