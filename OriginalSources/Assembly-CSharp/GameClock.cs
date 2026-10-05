// Decompiled with JetBrains decompiler
// Type: GameClock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Pool;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/GameClock")]
public class GameClock : KMonoBehaviour, ISaveLoadable, ISim33ms, IRender1000ms
{
  public static GameClock Instance;
  [Serialize]
  private int frame;
  [Serialize]
  private float time;
  [Serialize]
  private float timeSinceStartOfCycle;
  [Serialize]
  private int cycle;
  [Serialize]
  private float timePlayed;
  [Serialize]
  private bool isNight;
  public static readonly string NewCycleKey = "NewCycle";

  public static void DestroyInstance() => GameClock.Instance = (GameClock) null;

  protected override void OnPrefabInit()
  {
    GameClock.Instance = this;
    this.timeSinceStartOfCycle = 50f;
  }

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    if ((double) this.time == 0.0)
      return;
    this.cycle = (int) ((double) this.time / 600.0);
    this.timeSinceStartOfCycle = Mathf.Max(this.time - (float) this.cycle * 600f, 0.0f);
    this.time = 0.0f;
  }

  public void Sim33ms(float dt) => this.AddTime(dt);

  public void Render1000ms(float dt) => this.timePlayed += dt;

  private void LateUpdate() => ++this.frame;

  private void AddTime(float dt)
  {
    this.timeSinceStartOfCycle += dt;
    bool flag = false;
    while ((double) this.timeSinceStartOfCycle >= 600.0)
    {
      ++this.cycle;
      this.timeSinceStartOfCycle -= 600f;
      this.Trigger(631075836, (object) null);
      foreach (KMonoBehaviour worldContainer in ClusterManager.Instance.WorldContainers)
        worldContainer.Trigger(631075836, (object) null);
      flag = true;
    }
    if (!this.isNight && this.IsNighttime())
    {
      this.isNight = true;
      this.Trigger(-722330267, (object) null);
    }
    if (this.isNight && !this.IsNighttime())
      this.isNight = false;
    if (flag && SaveGame.Instance.AutoSaveCycleInterval > 0 && this.cycle % SaveGame.Instance.AutoSaveCycleInterval == 0)
      this.DoAutoSave(this.cycle);
    int num1 = Mathf.FloorToInt(this.timeSinceStartOfCycle - dt / 25f);
    int num2 = Mathf.FloorToInt(this.timeSinceStartOfCycle / 25f);
    int num3 = num2;
    if (num1 == num3)
      return;
    GameClock.GameClockBlockEventData v;
    using (GameClock.GameClockBlockEventData.Pool.Get(out v))
    {
      v.block = num2;
      this.Trigger(-1215042067, (object) v);
    }
  }

  public float GetTimeSinceStartOfReport()
  {
    return this.IsNighttime() ? 525f - this.GetTimeSinceStartOfCycle() : this.GetTimeSinceStartOfCycle() + 75f;
  }

  public float GetTimeSinceStartOfCycle() => this.timeSinceStartOfCycle;

  public float GetCurrentCycleAsPercentage() => this.timeSinceStartOfCycle / 600f;

  public float GetTime() => this.timeSinceStartOfCycle + (float) this.cycle * 600f;

  public float GetTimeInCycles() => (float) this.cycle + this.GetCurrentCycleAsPercentage();

  public int GetFrame() => this.frame;

  public int GetCycle() => this.cycle;

  public bool IsNighttime() => (double) GameClock.Instance.GetCurrentCycleAsPercentage() >= 0.875;

  public float GetDaytimeDurationInPercentage() => 0.875f;

  public void SetTime(float new_time) => this.AddTime(Mathf.Max(new_time - this.GetTime(), 0.0f));

  public float GetTimePlayedInSeconds() => this.timePlayed;

  private void DoAutoSave(int day)
  {
    if (GenericGameSettings.instance.disableAutosave)
      return;
    ++day;
    OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, GameClock.NewCycleKey, (object) day);
    OniMetrics.SendEvent(OniMetrics.Event.EndOfCycle, "AutoSave");
    string path = SaveLoader.GetActiveSaveFilePath() ?? SaveLoader.GetAutosaveFilePath();
    int startIndex = path.LastIndexOf("\\");
    if (startIndex > 0)
    {
      int length = path.IndexOf(" Cycle ", startIndex);
      if (length > 0)
        path = path.Substring(0, length);
    }
    string validSaveFilename = SaveScreen.GetValidSaveFilename($"{System.IO.Path.ChangeExtension(path, (string) null)} Cycle {day.ToString()}");
    string str1 = System.IO.Path.Combine(SaveLoader.GetActiveAutoSavePath(), System.IO.Path.GetFileName(validSaveFilename));
    string str2 = str1;
    int num = 1;
    while (File.Exists(str1))
    {
      str2.Replace(".sav", "");
      str1 = SaveScreen.GetValidSaveFilename($"{str2} ({num.ToString()})");
      ++num;
    }
    Game.Instance.StartDelayedSave(str1, true, false);
  }

  private class GameClockBlockEventData
  {
    public int block = -1;
    public static ObjectPool<GameClock.GameClockBlockEventData> Pool = new ObjectPool<GameClock.GameClockBlockEventData>((Func<GameClock.GameClockBlockEventData>) (() => new GameClock.GameClockBlockEventData()), collectionCheck: false, defaultCapacity: 4, maxSize: 4);
  }
}
