// Decompiled with JetBrains decompiler
// Type: TimeOfDay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using KSerialization;
using ProcGen;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/TimeOfDay")]
public class TimeOfDay : KMonoBehaviour, ISaveLoadable
{
  private const string MILESTONE_CYCLE_REACHED_AUDIO_NAME = "Stinger_Day_Celebrate";
  public static List<int> MILESTONE_CYCLES = new List<int>(2)
  {
    99,
    999
  };
  [Serialize]
  private float scale;
  private EventInstance nightLPEvent;
  public static TimeOfDay Instance;
  public string stingerDay;
  public string stingerNight;
  private bool isEclipse;

  public static bool IsMilestoneApproaching
  {
    get
    {
      if (!((Object) TimeOfDay.Instance != (Object) null) || !((Object) GameClock.Instance != (Object) null))
        return false;
      int currentTimeRegion = (int) TimeOfDay.Instance.GetCurrentTimeRegion();
      int cycle = GameClock.Instance.GetCycle();
      return currentTimeRegion == 2 && TimeOfDay.MILESTONE_CYCLES != null && TimeOfDay.MILESTONE_CYCLES.Contains(cycle + 1);
    }
  }

  public static bool IsMilestoneDay
  {
    get
    {
      if (!((Object) TimeOfDay.Instance != (Object) null) || !((Object) GameClock.Instance != (Object) null))
        return false;
      int currentTimeRegion = (int) TimeOfDay.Instance.GetCurrentTimeRegion();
      int cycle = GameClock.Instance.GetCycle();
      return currentTimeRegion == 1 && TimeOfDay.MILESTONE_CYCLES != null && TimeOfDay.MILESTONE_CYCLES.Contains(cycle);
    }
  }

  public TimeOfDay.TimeRegion timeRegion { private set; get; }

  public static void DestroyInstance() => TimeOfDay.Instance = (TimeOfDay) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    TimeOfDay.Instance = this;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    TimeOfDay.Instance = (TimeOfDay) null;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.timeRegion = this.GetCurrentTimeRegion();
    string clusterId = SaveLoader.Instance.GameInfo.clusterId;
    ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(clusterId);
    this.stingerDay = clusterData == null || string.IsNullOrWhiteSpace(clusterData.clusterAudio.stingerDay) ? "Stinger_Day" : clusterData.clusterAudio.stingerDay;
    this.stingerNight = clusterData == null || string.IsNullOrWhiteSpace(clusterData.clusterAudio.stingerNight) ? "Stinger_Loop_Night" : clusterData.clusterAudio.stingerNight;
    if (!MusicManager.instance.SongIsPlaying(this.stingerNight) && this.GetCurrentTimeRegion() == TimeOfDay.TimeRegion.Night)
    {
      MusicManager.instance.PlaySong(this.stingerNight);
      MusicManager.instance.SetSongParameter(this.stingerNight, "Music_PlayStinger", 0.0f);
    }
    double num = (double) this.UpdateSunlightIntensity();
  }

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized() => this.UpdateVisuals();

  public TimeOfDay.TimeRegion GetCurrentTimeRegion()
  {
    return GameClock.Instance.IsNighttime() ? TimeOfDay.TimeRegion.Night : TimeOfDay.TimeRegion.Day;
  }

  private void Update()
  {
    this.UpdateVisuals();
    TimeOfDay.TimeRegion currentTimeRegion = this.GetCurrentTimeRegion();
    Boxed<int> boxed = Boxed<int>.Get(GameClock.Instance.GetCycle());
    if (currentTimeRegion != this.timeRegion)
    {
      if (TimeOfDay.IsMilestoneApproaching)
        Game.Instance.Trigger(-720092972, (object) boxed);
      if (TimeOfDay.IsMilestoneDay)
        Game.Instance.Trigger(2070437606, (object) boxed);
      this.TriggerSoundChange(currentTimeRegion, TimeOfDay.IsMilestoneDay);
      this.timeRegion = currentTimeRegion;
      this.Trigger(1791086652, (object) null);
    }
    Boxed<int>.Release(boxed);
  }

  private void UpdateVisuals()
  {
    float num1 = 0.875f;
    float num2 = 0.2f;
    float num3 = 1f;
    float b = 0.0f;
    if ((double) GameClock.Instance.GetCurrentCycleAsPercentage() >= (double) num1)
      b = num3;
    this.scale = Mathf.Lerp(this.scale, b, Time.deltaTime * num2);
    Shader.SetGlobalVector("_TimeOfDay", new Vector4(this.scale, this.UpdateSunlightIntensity(), 0.0f, 0.0f));
  }

  public void Sim4000ms(float dt)
  {
    double num = (double) this.UpdateSunlightIntensity();
  }

  public void SetEclipse(bool eclipse) => this.isEclipse = eclipse;

  private float UpdateSunlightIntensity()
  {
    float durationInPercentage = GameClock.Instance.GetDaytimeDurationInPercentage();
    float num1 = GameClock.Instance.GetCurrentCycleAsPercentage() / durationInPercentage;
    if ((double) num1 >= 1.0 || this.isEclipse)
      num1 = 0.0f;
    float num2 = Mathf.Sin(num1 * 3.14159274f);
    Game.Instance.currentFallbackSunlightIntensity = num2 * 80000f;
    foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
    {
      worldContainer.currentSunlightIntensity = num2 * (float) worldContainer.sunlight;
      worldContainer.currentCosmicIntensity = (float) worldContainer.cosmicRadiation;
    }
    return num2;
  }

  private void TriggerSoundChange(TimeOfDay.TimeRegion new_region, bool milestoneReached)
  {
    switch (new_region)
    {
      case TimeOfDay.TimeRegion.Day:
        AudioMixer.instance.Stop(AudioMixerSnapshots.Get().NightStartedMigrated);
        if (MusicManager.instance.SongIsPlaying(this.stingerNight))
          MusicManager.instance.StopSong(this.stingerNight);
        if (milestoneReached)
          MusicManager.instance.PlaySong("Stinger_Day_Celebrate");
        else
          MusicManager.instance.PlaySong(this.stingerDay);
        MusicManager.instance.PlayDynamicMusic();
        break;
      case TimeOfDay.TimeRegion.Night:
        AudioMixer.instance.Start(AudioMixerSnapshots.Get().NightStartedMigrated);
        MusicManager.instance.PlaySong(this.stingerNight);
        break;
    }
  }

  public void SetScale(float new_scale) => this.scale = new_scale;

  public enum TimeRegion
  {
    Invalid,
    Day,
    Night,
  }
}
