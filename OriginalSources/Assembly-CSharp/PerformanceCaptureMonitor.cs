// Decompiled with JetBrains decompiler
// Type: PerformanceCaptureMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Unity.Profiling;
using UnityEngine;

#nullable disable
public class PerformanceCaptureMonitor
{
  public static PerformanceCaptureMonitor.PerformanceCaptureData Data = new PerformanceCaptureMonitor.PerformanceCaptureData();
  private static ProfilerRecorder systemUsedMemory;
  private static Stopwatch loadTimer = new Stopwatch();
  private static Stopwatch captureTimer = new Stopwatch();

  public static void WritePerformanceCaptureData()
  {
    PerformanceCaptureMonitor.Data.SWAverageFrameTimeMs = (float) PerformanceCaptureMonitor.captureTimer.Elapsed.TotalMilliseconds / (float) GenericGameSettings.instance.scriptedProfile.frameCount;
    PerformanceCaptureMonitor.Data.Revision = 744825U;
    PerformanceCaptureMonitor.Data.Branch = "release";
    PerformanceCaptureMonitor.Data.IsBaseGame = !DlcManager.IsExpansion1Active();
    PerformanceCaptureMonitor.Data.LoadedDlcs = DlcManager.GetActiveDLCIds();
    if ((UnityEngine.Object) SaveLoader.Instance != (UnityEngine.Object) null)
    {
      PerformanceCaptureMonitor.Data.ActiveDlcsInSave = SaveLoader.Instance.GameInfo.dlcIds.ToArray();
      PerformanceCaptureMonitor.Data.PerfMonAverageFrameTimeMs = SaveLoader.Instance.GetFrameTime() * 1000f;
    }
    if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
    {
      PerformanceCaptureMonitor.Data.Cycle = GameUtil.GetCurrentCycle();
      PerformanceCaptureMonitor.Data.Brains = new List<PerformanceCaptureMonitor.PerformanceCaptureData.BrainInfo>();
      foreach (BrainScheduler.BrainGroup brainGroup in Game.BrainScheduler.debugGetBrainGroups())
        PerformanceCaptureMonitor.Data.Brains.Add(new PerformanceCaptureMonitor.PerformanceCaptureData.BrainInfo()
        {
          name = brainGroup.tag.ToString(),
          count = brainGroup.BrainCount
        });
    }
    PerformanceCaptureMonitor.Data.Patch = "";
    PerformanceCaptureMonitor.Data.BuildTags = new List<string>();
    PerformanceCaptureMonitor.Data.BuildTags.Add("release");
    PerformanceCaptureMonitor.Data.BuildConfig = string.Join("_", (IEnumerable<string>) PerformanceCaptureMonitor.Data.BuildTags);
    if (!PerformanceCaptureMonitor.Data.Patch.IsNullOrWhiteSpace())
      PerformanceCaptureMonitor.Data.BuildTags.Add("PatchedBuild");
    if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null)
      PerformanceCaptureMonitor.Data.GameSpeed = SpeedControlScreen.Instance.GetSpeed() + 1;
    PerformanceCaptureMonitor.Data.EndMemoryMegs = PerformanceCaptureMonitor.GetMemoryUsed();
    File.WriteAllText("PerformanceCaptureData.json", JsonUtility.ToJson((object) PerformanceCaptureMonitor.Data));
    DebugUtil.LogArgs((object) "Written PerformanceCaptureData.json");
  }

  public static bool IsCapturingPerformance()
  {
    return !GenericGameSettings.instance.scriptedProfile.saveGame.IsNullOrWhiteSpace();
  }

  public static void Initialize()
  {
    if (!PerformanceCaptureMonitor.IsCapturingPerformance())
      return;
    Application.targetFrameRate = -1;
    QualitySettings.vSyncCount = 0;
    KProfilerBegin.OnStopCapture += new System.Action(PerformanceCaptureMonitor.WritePerformanceCaptureData);
    KProfilerBegin.OnStartCapture += new System.Action(PerformanceCaptureMonitor.StartCapture);
    PerformanceCaptureMonitor.systemUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
  }

  public static void StartCapture() => PerformanceCaptureMonitor.captureTimer.Restart();

  public static void StartLoadingSave() => PerformanceCaptureMonitor.loadTimer.Restart();

  public static IEnumerator FinishedLoadingSave()
  {
    yield return (object) null;
    PerformanceCaptureMonitor.Data.SaveLoadMemoryMegs = PerformanceCaptureMonitor.GetMemoryUsed();
    PerformanceCaptureMonitor.loadTimer.Stop();
    PerformanceCaptureMonitor.Data.SaveLoadTimeSec = (float) PerformanceCaptureMonitor.loadTimer.Elapsed.TotalSeconds;
    if (GenericGameSettings.instance.devQuitAfterLoadingSave)
      App.QuitCode(KCrashReporter.hasCrash ? 1 : 0);
  }

  public static void TryRecordMainMenuStats()
  {
    if ((double) PerformanceCaptureMonitor.Data.MainMenuMemoryMegs != 0.0)
      return;
    PerformanceCaptureMonitor.Data.MainMenuMemoryMegs = PerformanceCaptureMonitor.GetMemoryUsed();
    PerformanceCaptureMonitor.Data.MainMenuLoadTimeSec = Time.realtimeSinceStartup;
  }

  public static float GetMemoryUsed()
  {
    return !PerformanceCaptureMonitor.systemUsedMemory.Valid ? 0.0f : (float) PerformanceCaptureMonitor.systemUsedMemory.CurrentValue / 1048576f;
  }

  public class PerformanceCaptureData
  {
    public uint Revision;
    public string Patch;
    public string Branch;
    public bool IsDevelopmentBuild;
    public bool IsBaseGame;
    public string[] ActiveDlcsInSave;
    public List<string> LoadedDlcs;
    public List<string> BuildTags;
    public List<PerformanceCaptureMonitor.PerformanceCaptureData.BrainInfo> Brains;
    public int Cycle;
    public float MainMenuLoadTimeSec;
    public float MainMenuMemoryMegs;
    public float SaveLoadTimeSec;
    public float SaveLoadMemoryMegs;
    public float EndMemoryMegs;
    public float PerfMonAverageFrameTimeMs;
    public float SWAverageFrameTimeMs;
    public string BuildConfig;
    public int GameSpeed;

    [Serializable]
    public class BrainInfo
    {
      public string name;
      public int count;
    }
  }
}
