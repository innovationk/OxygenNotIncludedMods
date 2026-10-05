// Decompiled with JetBrains decompiler
// Type: MusicManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using ProcGen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/MusicManager")]
public class MusicManager : KMonoBehaviour, ISerializationCallbackReceiver
{
  private const string VARIATION_ID = "variation";
  private const string INTERRUPTED_DIMMED_ID = "interrupted_dimmed";
  private const string MUSIC_KEY = "MusicInKey";
  private const float DYNAMIC_MUSIC_SCHEDULE_DELAY = 16000f;
  private const float DYNAMIC_MUSIC_SCHEDULE_LOOKAHEAD = 48000f;
  [Header("Song Lists")]
  [Tooltip("Play during the daytime. The mix of the song is affected by the player's input, like pausing the sim, activating an overlay, or zooming in and out.")]
  [SerializeField]
  private MusicManager.DynamicSong[] fullSongs;
  [Tooltip("Simple dynamic songs which are more ambient in nature, which play quietly during \"non-music\" days. These are affected by Pause and OverlayActive.")]
  [SerializeField]
  private MusicManager.Minisong[] miniSongs;
  [Tooltip("Triggered by in-game events, such as completing research or night-time falling. They will temporarily interrupt a dynamicSong, fading the dynamicSong back in after the stinger is complete.")]
  [SerializeField]
  private MusicManager.Stinger[] stingers;
  [Tooltip("Generally songs that don't play during gameplay, while a menu is open. For example, the ESC menu or the Starmap.")]
  [SerializeField]
  private MusicManager.MenuSong[] menuSongs;
  private Dictionary<string, MusicManager.SongInfo> songMap = new Dictionary<string, MusicManager.SongInfo>();
  public Dictionary<string, MusicManager.SongInfo> activeSongs = new Dictionary<string, MusicManager.SongInfo>();
  [Space]
  [Header("Tuning Values")]
  [Tooltip("Just before night-time (88%), dynamic music fades out. At which point of the day should the music fade?")]
  [SerializeField]
  private float duskTimePercentage = 85f;
  [Tooltip("If we load into a save and the day is almost over, we shouldn't play music because it will stop soon anyway. At what point of the day should we not play music?")]
  [SerializeField]
  private float loadGameCutoffPercentage = 50f;
  [Tooltip("When dynamic music is active, we play a snapshot which attenuates the ambience and SFX. What intensity should that snapshot be applied?")]
  [SerializeField]
  private float dynamicMusicSFXAttenuationPercentage = 65f;
  [Tooltip("When mini songs are active, we play a snapshot which attenuates the ambience and SFX. What intensity should that snapshot be applied?")]
  [SerializeField]
  private float miniSongSFXAttenuationPercentage;
  [SerializeField]
  private MusicManager.TypeOfMusic[] musicStyleOrder;
  [NonSerialized]
  public bool alwaysPlayMusic;
  private MusicManager.DynamicSongPlaylist fullSongPlaylist = new MusicManager.DynamicSongPlaylist();
  private MusicManager.DynamicSongPlaylist miniSongPlaylist = new MusicManager.DynamicSongPlaylist();
  [NonSerialized]
  public MusicManager.SongInfo activeDynamicSong;
  [NonSerialized]
  public MusicManager.DynamicSongPlaylist activePlaylist;
  private MusicManager.TypeOfMusic nextMusicType;
  private int musicTypeIterator;
  private float time;
  private float timeOfDayUpdateRate = 2f;
  private static MusicManager _instance;
  [NonSerialized]
  public List<string> MusicDebugLog = new List<string>();

  public Dictionary<string, MusicManager.SongInfo> SongMap => this.songMap;

  public void PlaySong(string song_name, bool canWait = false)
  {
    this.Log("Play: " + song_name);
    if (!AudioDebug.Get().musicEnabled)
      return;
    MusicManager.SongInfo songInfo = (MusicManager.SongInfo) null;
    if (!this.songMap.TryGetValue(song_name, out songInfo))
      DebugUtil.LogErrorArgs((object) "Unknown song:", (object) song_name);
    else if (this.activeSongs.ContainsKey(song_name))
      DebugUtil.LogWarningArgs((object) "Trying to play duplicate song:", (object) song_name);
    else if (this.activeSongs.Count == 0)
    {
      songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
      if (!songInfo.ev.isValid())
        DebugUtil.LogWarningArgs((object) $"Failed to find FMOD event [{songInfo.fmodEvent.ToString()}]");
      int num1 = songInfo.numberOfVariations > 0 ? UnityEngine.Random.Range(1, songInfo.numberOfVariations + 1) : -1;
      if (num1 != -1)
      {
        int num2 = (int) songInfo.ev.setParameterByName("variation", (float) num1);
      }
      if (songInfo.dynamic)
      {
        int num3 = (int) songInfo.ev.setProperty(EVENT_PROPERTY.SCHEDULE_DELAY, 16000f);
        int num4 = (int) songInfo.ev.setProperty(EVENT_PROPERTY.SCHEDULE_LOOKAHEAD, 48000f);
        this.activeDynamicSong = songInfo;
      }
      int num5 = (int) songInfo.ev.start();
      this.activeSongs[song_name] = songInfo;
    }
    else
    {
      List<string> stringList = new List<string>((IEnumerable<string>) this.activeSongs.Keys);
      if (songInfo.interruptsActiveMusic)
      {
        for (int index = 0; index < stringList.Count; ++index)
        {
          if (!this.activeSongs[stringList[index]].interruptsActiveMusic)
          {
            MusicManager.SongInfo activeSong = this.activeSongs[stringList[index]];
            int num = (int) activeSong.ev.setParameterByName("interrupted_dimmed", 1f);
            this.Log("Dimming: " + Assets.GetSimpleSoundEventName(activeSong.fmodEvent));
            songInfo.songsOnHold.Add(stringList[index]);
          }
        }
        songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
        if (!songInfo.ev.isValid())
          DebugUtil.LogWarningArgs((object) $"Failed to find FMOD event [{songInfo.fmodEvent.ToString()}]");
        int num6 = (int) songInfo.ev.start();
        int num7 = (int) songInfo.ev.release();
        this.activeSongs[song_name] = songInfo;
      }
      else
      {
        int num8 = 0;
        foreach (string key in this.activeSongs.Keys)
        {
          MusicManager.SongInfo activeSong = this.activeSongs[key];
          if (!activeSong.interruptsActiveMusic && activeSong.priority > num8)
            num8 = activeSong.priority;
        }
        if (songInfo.priority < num8)
          return;
        for (int index = 0; index < stringList.Count; ++index)
        {
          MusicManager.SongInfo activeSong = this.activeSongs[stringList[index]];
          FMOD.Studio.EventInstance ev = activeSong.ev;
          if (!activeSong.interruptsActiveMusic)
          {
            int num9 = (int) ev.setParameterByName("interrupted_dimmed", 1f);
            int num10 = (int) ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            this.activeSongs.Remove(stringList[index]);
            stringList.Remove(stringList[index]);
          }
        }
        songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
        if (!songInfo.ev.isValid())
          DebugUtil.LogWarningArgs((object) $"Failed to find FMOD event [{songInfo.fmodEvent.ToString()}]");
        int num11 = songInfo.numberOfVariations > 0 ? UnityEngine.Random.Range(1, songInfo.numberOfVariations + 1) : -1;
        if (num11 != -1)
        {
          int num12 = (int) songInfo.ev.setParameterByName("variation", (float) num11);
        }
        int num13 = (int) songInfo.ev.start();
        this.activeSongs[song_name] = songInfo;
      }
    }
  }

  public void StopSong(string song_name, bool shouldLog = true, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
  {
    if (shouldLog)
      this.Log("Stop: " + song_name);
    MusicManager.SongInfo songInfo1 = (MusicManager.SongInfo) null;
    if (!this.songMap.TryGetValue(song_name, out songInfo1))
      DebugUtil.LogErrorArgs((object) "Unknown song:", (object) song_name);
    else if (!this.activeSongs.ContainsKey(song_name))
    {
      DebugUtil.LogWarningArgs((object) "Trying to stop a song that isn't playing:", (object) song_name);
    }
    else
    {
      FMOD.Studio.EventInstance ev1 = songInfo1.ev;
      int num1 = (int) ev1.stop(stopMode);
      int num2 = (int) ev1.release();
      if (songInfo1.dynamic)
        this.activeDynamicSong = (MusicManager.SongInfo) null;
      if (songInfo1.songsOnHold.Count > 0)
      {
        for (int index = 0; index < songInfo1.songsOnHold.Count; ++index)
        {
          MusicManager.SongInfo songInfo2;
          if (this.activeSongs.TryGetValue(songInfo1.songsOnHold[index], out songInfo2) && songInfo2.ev.isValid())
          {
            FMOD.Studio.EventInstance ev2 = songInfo2.ev;
            this.Log("Undimming: " + Assets.GetSimpleSoundEventName(songInfo2.fmodEvent));
            int num3 = (int) ev2.setParameterByName("interrupted_dimmed", 0.0f);
            songInfo1.songsOnHold.Remove(songInfo1.songsOnHold[index]);
          }
          else
            songInfo1.songsOnHold.Remove(songInfo1.songsOnHold[index]);
        }
      }
      this.activeSongs.Remove(song_name);
    }
  }

  public void KillAllSongs(FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
  {
    this.Log("Kill All Songs");
    if (this.DynamicMusicIsActive())
      this.StopDynamicMusic(true);
    List<string> stringList = new List<string>((IEnumerable<string>) this.activeSongs.Keys);
    for (int index = 0; index < stringList.Count; ++index)
      this.StopSong(stringList[index]);
  }

  public void SetSongParameter(
    string song_name,
    string parameter_name,
    float parameter_value,
    bool shouldLog = true)
  {
    if (shouldLog)
      this.Log($"Set Param {song_name}: {parameter_name}, {parameter_value}");
    MusicManager.SongInfo songInfo = (MusicManager.SongInfo) null;
    if (!this.activeSongs.TryGetValue(song_name, out songInfo))
      return;
    FMOD.Studio.EventInstance ev = songInfo.ev;
    if (!ev.isValid())
      return;
    int num = (int) ev.setParameterByName(parameter_name, parameter_value);
  }

  public void SetSongParameter(
    string song_name,
    string parameter_name,
    string parameter_lable,
    bool shouldLog = true)
  {
    if (shouldLog)
      this.Log($"Set Param {song_name}: {parameter_name}, {parameter_lable}");
    MusicManager.SongInfo songInfo = (MusicManager.SongInfo) null;
    if (!this.activeSongs.TryGetValue(song_name, out songInfo))
      return;
    FMOD.Studio.EventInstance ev = songInfo.ev;
    if (!ev.isValid())
      return;
    int num = (int) ev.setParameterByNameWithLabel(parameter_name, parameter_lable);
  }

  public bool SongIsPlaying(string song_name)
  {
    MusicManager.SongInfo songInfo = (MusicManager.SongInfo) null;
    return this.activeSongs.TryGetValue(song_name, out songInfo) && songInfo.musicPlaybackState != PLAYBACK_STATE.STOPPED;
  }

  private void Update()
  {
    this.ClearFinishedSongs();
    if (!this.DynamicMusicIsActive())
      return;
    this.SetDynamicMusicZoomLevel();
    this.SetDynamicMusicTimeSinceLastJob();
    if (this.activeDynamicSong.useTimeOfDay)
      this.SetDynamicMusicTimeOfDay();
    if (!((UnityEngine.Object) GameClock.Instance != (UnityEngine.Object) null) || (double) GameClock.Instance.GetCurrentCycleAsPercentage() < (double) this.duskTimePercentage / 100.0)
      return;
    this.StopDynamicMusic();
  }

  private void ClearFinishedSongs()
  {
    if (this.activeSongs.Count <= 0)
      return;
    ListPool<string, MusicManager>.PooledList pooledList = ListPool<string, MusicManager>.Allocate();
    foreach (KeyValuePair<string, MusicManager.SongInfo> activeSong in this.activeSongs)
    {
      MusicManager.SongInfo songInfo = activeSong.Value;
      int playbackState = (int) songInfo.ev.getPlaybackState(out songInfo.musicPlaybackState);
      if (songInfo.musicPlaybackState == PLAYBACK_STATE.STOPPED || songInfo.musicPlaybackState == PLAYBACK_STATE.STOPPING)
      {
        pooledList.Add(activeSong.Key);
        foreach (string song_name in songInfo.songsOnHold)
          this.SetSongParameter(song_name, "interrupted_dimmed", 0.0f);
        songInfo.songsOnHold.Clear();
      }
    }
    foreach (string key in (List<string>) pooledList)
      this.activeSongs.Remove(key);
    pooledList.Recycle();
  }

  public void OnEscapeMenu(bool paused)
  {
    foreach (KeyValuePair<string, MusicManager.SongInfo> activeSong in this.activeSongs)
    {
      if (activeSong.Value != null)
        this.StartFadeToPause(activeSong.Value.ev, paused);
    }
  }

  public void OnSupplyClosetMenu(bool paused, float fadeTime)
  {
    bool flag = !paused;
    if (((PauseScreen.Instance.IsNullOrDestroyed() ? 0 : (PauseScreen.Instance.IsActive() ? 1 : 0)) & (flag ? 1 : 0)) != 0 && MusicManager.instance.SongIsPlaying("Music_ESC_Menu"))
    {
      MusicManager.SongInfo song = this.songMap["Music_ESC_Menu"];
      foreach (KeyValuePair<string, MusicManager.SongInfo> activeSong in this.activeSongs)
      {
        if (activeSong.Value != null && activeSong.Value != song)
          this.StartFadeToPause(activeSong.Value.ev, paused);
      }
      this.StartFadeToPause(song.ev, false);
    }
    else
    {
      foreach (KeyValuePair<string, MusicManager.SongInfo> activeSong in this.activeSongs)
      {
        if (activeSong.Value != null)
          this.StartFadeToPause(activeSong.Value.ev, paused, fadeTime);
      }
    }
  }

  public void StartFadeToPause(FMOD.Studio.EventInstance inst, bool paused, float fadeTime = 0.25f)
  {
    if (paused)
      this.StartCoroutine(this.FadeToPause(inst, fadeTime));
    else
      this.StartCoroutine(this.FadeToUnpause(inst, fadeTime));
  }

  private IEnumerator FadeToPause(FMOD.Studio.EventInstance inst, float fadeTime)
  {
    float startVolume;
    float targetVolume;
    int volume = (int) inst.getVolume(out startVolume, out targetVolume);
    targetVolume = 0.0f;
    float lerpTime = 0.0f;
    while ((double) lerpTime < 1.0)
    {
      lerpTime += Time.unscaledDeltaTime / fadeTime;
      int num = (int) inst.setVolume(Mathf.Lerp(startVolume, targetVolume, lerpTime));
      yield return (object) null;
    }
    int num1 = (int) inst.setPaused(true);
  }

  private IEnumerator FadeToUnpause(FMOD.Studio.EventInstance inst, float fadeTime)
  {
    float startVolume;
    float targetVolume;
    int volume = (int) inst.getVolume(out startVolume, out targetVolume);
    targetVolume = 1f;
    float lerpTime = 0.0f;
    int num1 = (int) inst.setPaused(false);
    while ((double) lerpTime < 1.0)
    {
      lerpTime += Time.unscaledDeltaTime / fadeTime;
      int num2 = (int) inst.setVolume(Mathf.Lerp(startVolume, targetVolume, lerpTime));
      yield return (object) null;
    }
  }

  public void WattsonStartDynamicMusic()
  {
    ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
    if (currentClusterLayout != null && currentClusterLayout.clusterAudio != null)
    {
      if (currentClusterLayout.clusterAudio.musicPlaylist != null && currentClusterLayout.clusterAudio.musicPlaylist.Count > 0)
      {
        List<string> songs = new List<string>((IEnumerable<string>) currentClusterLayout.clusterAudio.musicPlaylist);
        if (!currentClusterLayout.clusterAudio.musicFirst.IsNullOrWhiteSpace() && !songs.Contains(currentClusterLayout.clusterAudio.musicFirst))
          songs.Insert(0, currentClusterLayout.clusterAudio.musicFirst);
        this.fullSongPlaylist.SetPrioritySongs(songs);
      }
      if (!currentClusterLayout.clusterAudio.musicFirst.IsNullOrWhiteSpace())
      {
        this.activePlaylist = this.fullSongPlaylist;
        this.fullSongPlaylist.MarkSongPlayed(currentClusterLayout.clusterAudio.musicFirst);
        this.PlayDynamicMusic(currentClusterLayout.clusterAudio.musicFirst);
      }
      else
        this.PlayDynamicMusic();
    }
    else
      this.PlayDynamicMusic();
  }

  public void PlayDynamicMusic()
  {
    if (this.DynamicMusicIsActive())
      this.Log("Trying to play DynamicMusic when it is already playing.");
    else
      this.PlayDynamicMusic(this.GetNextDynamicSong());
  }

  private void PlayDynamicMusic(string song_name)
  {
    if (song_name == "NONE")
      return;
    this.PlaySong(song_name);
    MusicManager.SongInfo songInfo;
    if (this.activeSongs.TryGetValue(song_name, out songInfo))
    {
      this.activeDynamicSong = songInfo;
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot);
      if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null && SpeedControlScreen.Instance.IsPaused)
        this.SetDynamicMusicPaused();
      if ((UnityEngine.Object) OverlayScreen.Instance != (UnityEngine.Object) null && OverlayScreen.Instance.mode != OverlayModes.None.ID)
        this.SetDynamicMusicOverlayActive();
      this.SetDynamicMusicPlayHook();
      this.SetDynamicMusicKeySigniture();
      string key = "Volume_Music";
      if (KPlayerPrefs.HasKey(key))
      {
        float parameter_value = KPlayerPrefs.GetFloat(key);
        AudioMixer.instance.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "userVolume_Music", parameter_value);
      }
      AudioMixer.instance.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "intensity", songInfo.sfxAttenuationPercentage / 100f);
    }
    else
    {
      this.Log($"DynamicMusic song {song_name} did not start.");
      string str = "";
      foreach (KeyValuePair<string, MusicManager.SongInfo> activeSong in this.activeSongs)
      {
        str = $"{str}{activeSong.Key}, ";
        Debug.Log((object) str);
      }
      DebugUtil.DevAssert(false, "Song failed to play: " + song_name);
    }
  }

  public void StopDynamicMusic(bool stopImmediate = false)
  {
    if (this.activeDynamicSong == null)
      return;
    FMOD.Studio.STOP_MODE stopMode = stopImmediate ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT;
    this.Log("Stop DynamicMusic: " + Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent));
    this.StopSong(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), stopMode: stopMode);
    this.activeDynamicSong = (MusicManager.SongInfo) null;
    AudioMixer.instance.Stop(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot);
  }

  public string GetNextDynamicSong()
  {
    string nextDynamicSong = "";
    if (this.alwaysPlayMusic && this.nextMusicType == MusicManager.TypeOfMusic.None)
    {
      while (this.nextMusicType == MusicManager.TypeOfMusic.None)
        this.CycleToNextMusicType();
    }
    switch (this.nextMusicType)
    {
      case MusicManager.TypeOfMusic.DynamicSong:
        nextDynamicSong = this.fullSongPlaylist.GetNextSong();
        this.activePlaylist = this.fullSongPlaylist;
        break;
      case MusicManager.TypeOfMusic.MiniSong:
        nextDynamicSong = this.miniSongPlaylist.GetNextSong();
        this.activePlaylist = this.miniSongPlaylist;
        break;
      case MusicManager.TypeOfMusic.None:
        nextDynamicSong = "NONE";
        this.activePlaylist = (MusicManager.DynamicSongPlaylist) null;
        break;
    }
    this.CycleToNextMusicType();
    return nextDynamicSong;
  }

  private void CycleToNextMusicType()
  {
    this.musicTypeIterator = ++this.musicTypeIterator % this.musicStyleOrder.Length;
    this.nextMusicType = this.musicStyleOrder[this.musicTypeIterator];
  }

  public bool DynamicMusicIsActive() => this.activeDynamicSong != null;

  public void SetDynamicMusicPaused()
  {
    if (!this.DynamicMusicIsActive())
      return;
    this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "Paused", 1f);
  }

  public void SetDynamicMusicUnpaused()
  {
    if (!this.DynamicMusicIsActive())
      return;
    this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "Paused", 0.0f);
  }

  public void SetDynamicMusicZoomLevel()
  {
    if (!((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null))
      return;
    float parameter_value = (float) (100.0 - (double) Camera.main.orthographicSize / 20.0 * 100.0);
    this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "zoomPercentage", parameter_value, false);
  }

  public void SetDynamicMusicTimeSinceLastJob()
  {
    this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "secsSinceNewJob", Time.time - Game.Instance.LastTimeWorkStarted, false);
  }

  public void SetDynamicMusicTimeOfDay()
  {
    if ((double) this.time >= (double) this.timeOfDayUpdateRate)
    {
      this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "timeOfDay", GameClock.Instance.GetCurrentCycleAsPercentage(), false);
      this.time = 0.0f;
    }
    this.time += Time.deltaTime;
  }

  public void SetDynamicMusicOverlayActive()
  {
    if (!this.DynamicMusicIsActive())
      return;
    this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "overlayActive", 1f);
  }

  public void SetDynamicMusicOverlayInactive()
  {
    if (!this.DynamicMusicIsActive())
      return;
    this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "overlayActive", 0.0f);
  }

  public void SetDynamicMusicPlayHook()
  {
    if (!this.DynamicMusicIsActive())
      return;
    string simpleSoundEventName = Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent);
    this.SetSongParameter(simpleSoundEventName, "playHook", this.activeDynamicSong.playHook ? 1f : 0.0f);
    this.activePlaylist.songMap[simpleSoundEventName].playHook = !this.activePlaylist.songMap[simpleSoundEventName].playHook;
  }

  public bool ShouldPlayDynamicMusicLoadedGame()
  {
    return (double) GameClock.Instance.GetCurrentCycleAsPercentage() <= (double) this.loadGameCutoffPercentage / 100.0;
  }

  public void SetDynamicMusicKeySigniture()
  {
    if (!this.DynamicMusicIsActive())
      return;
    float num1;
    switch (this.activePlaylist.songMap[Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent)].musicKeySigniture)
    {
      case "Ab":
        num1 = 0.0f;
        break;
      case "Bb":
        num1 = 1f;
        break;
      case "C":
        num1 = 2f;
        break;
      case "D":
        num1 = 3f;
        break;
      default:
        num1 = 2f;
        break;
    }
    int num2 = (int) RuntimeManager.StudioSystem.setParameterByName("MusicInKey", num1);
  }

  public static MusicManager instance => MusicManager._instance;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!RuntimeManager.IsInitialized)
    {
      this.enabled = false;
    }
    else
    {
      if (!KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayMusicKey))
        return;
      this.alwaysPlayMusic = KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayMusicKey) == 1;
    }
  }

  protected override void OnPrefabInit()
  {
    MusicManager._instance = this;
    this.ConfigureSongs();
    this.nextMusicType = this.musicStyleOrder[this.musicTypeIterator];
  }

  protected override void OnCleanUp() => MusicManager._instance = (MusicManager) null;

  private static bool IsValidForDLCContext(string dlcid)
  {
    if (dlcid == "")
      return true;
    return (UnityEngine.Object) SaveLoader.Instance != (UnityEngine.Object) null ? Game.IsDlcActiveForCurrentSave(dlcid) : DlcManager.IsContentSubscribed(dlcid);
  }

  [ContextMenu("Reload")]
  public void ConfigureSongs()
  {
    this.songMap.Clear();
    this.fullSongPlaylist.Clear();
    this.miniSongPlaylist.Clear();
    foreach (MusicManager.DynamicSong fullSong in this.fullSongs)
    {
      if (MusicManager.IsValidForDLCContext(fullSong.requiredDlcId))
      {
        string simpleSoundEventName = Assets.GetSimpleSoundEventName(fullSong.fmodEvent);
        MusicManager.SongInfo songInfo = new MusicManager.SongInfo();
        songInfo.fmodEvent = fullSong.fmodEvent;
        songInfo.requiredDlcId = fullSong.requiredDlcId;
        songInfo.priority = 100;
        songInfo.interruptsActiveMusic = false;
        songInfo.dynamic = true;
        songInfo.useTimeOfDay = fullSong.useTimeOfDay;
        songInfo.numberOfVariations = fullSong.numberOfVariations;
        songInfo.musicKeySigniture = fullSong.musicKeySigniture;
        songInfo.sfxAttenuationPercentage = this.dynamicMusicSFXAttenuationPercentage;
        this.songMap[simpleSoundEventName] = songInfo;
        this.fullSongPlaylist.songMap[simpleSoundEventName] = songInfo;
      }
    }
    foreach (MusicManager.Minisong miniSong in this.miniSongs)
    {
      if (MusicManager.IsValidForDLCContext(miniSong.requiredDlcId))
      {
        string simpleSoundEventName = Assets.GetSimpleSoundEventName(miniSong.fmodEvent);
        MusicManager.SongInfo songInfo = new MusicManager.SongInfo();
        songInfo.fmodEvent = miniSong.fmodEvent;
        songInfo.requiredDlcId = miniSong.requiredDlcId;
        songInfo.priority = 100;
        songInfo.interruptsActiveMusic = false;
        songInfo.dynamic = true;
        songInfo.useTimeOfDay = false;
        songInfo.numberOfVariations = 5;
        songInfo.musicKeySigniture = miniSong.musicKeySigniture;
        songInfo.sfxAttenuationPercentage = this.miniSongSFXAttenuationPercentage;
        this.songMap[simpleSoundEventName] = songInfo;
        this.miniSongPlaylist.songMap[simpleSoundEventName] = songInfo;
      }
    }
    foreach (MusicManager.Stinger stinger in this.stingers)
    {
      if (MusicManager.IsValidForDLCContext(stinger.requiredDlcId))
        this.songMap[Assets.GetSimpleSoundEventName(stinger.fmodEvent)] = new MusicManager.SongInfo()
        {
          fmodEvent = stinger.fmodEvent,
          priority = 100,
          interruptsActiveMusic = true,
          dynamic = false,
          useTimeOfDay = false,
          numberOfVariations = 0,
          requiredDlcId = stinger.requiredDlcId
        };
    }
    foreach (MusicManager.MenuSong menuSong in this.menuSongs)
    {
      if (MusicManager.IsValidForDLCContext(menuSong.requiredDlcId))
        this.songMap[Assets.GetSimpleSoundEventName(menuSong.fmodEvent)] = new MusicManager.SongInfo()
        {
          fmodEvent = menuSong.fmodEvent,
          priority = 100,
          interruptsActiveMusic = true,
          dynamic = false,
          useTimeOfDay = false,
          numberOfVariations = 0,
          requiredDlcId = menuSong.requiredDlcId
        };
    }
    this.fullSongPlaylist.ResetUnplayedSongs();
    this.miniSongPlaylist.ResetUnplayedSongs();
  }

  public void OnBeforeSerialize()
  {
  }

  public void OnAfterDeserialize()
  {
  }

  private void Log(string s)
  {
  }

  [DebuggerDisplay("{fmodEvent}")]
  [Serializable]
  public class SongInfo
  {
    public EventReference fmodEvent;
    [NonSerialized]
    public int priority;
    [NonSerialized]
    public bool interruptsActiveMusic;
    [NonSerialized]
    public bool dynamic;
    [NonSerialized]
    public string requiredDlcId;
    [NonSerialized]
    public bool useTimeOfDay;
    [NonSerialized]
    public int numberOfVariations;
    [NonSerialized]
    public string musicKeySigniture = "C";
    [NonSerialized]
    public FMOD.Studio.EventInstance ev;
    [NonSerialized]
    public List<string> songsOnHold = new List<string>();
    [NonSerialized]
    public PLAYBACK_STATE musicPlaybackState;
    [NonSerialized]
    public bool playHook = true;
    [NonSerialized]
    public float sfxAttenuationPercentage = 65f;
  }

  [DebuggerDisplay("{fmodEvent}")]
  [Serializable]
  public class DynamicSong
  {
    public EventReference fmodEvent;
    [Tooltip("Some songs are set up to have Morning, Daytime, Hook, and Intro sections. Toggle this ON if this song has those sections.")]
    [SerializeField]
    public bool useTimeOfDay;
    [Tooltip("Some songs have different possible start locations. Enter how many start locations this song is set up to support.")]
    [SerializeField]
    public int numberOfVariations;
    [Tooltip("Some songs have different key signitures. Enter the key this music is in.")]
    [SerializeField]
    public string musicKeySigniture = "";
    [Tooltip("Should playback of this song be limited to an active DLC?")]
    [SerializeField]
    public string requiredDlcId;
  }

  [DebuggerDisplay("{fmodEvent}")]
  [Serializable]
  public class Stinger
  {
    public EventReference fmodEvent;
    [Tooltip("Should playback of this song be limited to an active DLC?")]
    [SerializeField]
    public string requiredDlcId;
  }

  [DebuggerDisplay("{fmodEvent}")]
  [Serializable]
  public class MenuSong
  {
    public EventReference fmodEvent;
    [Tooltip("Should playback of this song be limited to an active DLC?")]
    [SerializeField]
    public string requiredDlcId;
  }

  [DebuggerDisplay("{fmodEvent}")]
  [Serializable]
  public class Minisong
  {
    public EventReference fmodEvent;
    [Tooltip("Some songs have different key signitures. Enter the key this music is in.")]
    [SerializeField]
    public string musicKeySigniture = "";
    [Tooltip("Should playback of this song be limited to an active DLC?")]
    [SerializeField]
    public string requiredDlcId;
  }

  public enum TypeOfMusic
  {
    DynamicSong,
    MiniSong,
    None,
  }

  public class DynamicSongPlaylist
  {
    public Dictionary<string, MusicManager.SongInfo> songMap = new Dictionary<string, MusicManager.SongInfo>();
    public List<string> unplayedSongs = new List<string>();
    private string lastSongPlayed = "";

    public void Clear()
    {
      this.songMap.Clear();
      this.unplayedSongs.Clear();
      this.lastSongPlayed = "";
    }

    public void SetPrioritySongs(List<string> songs)
    {
      this.unplayedSongs.Clear();
      foreach (string song in songs)
      {
        if (this.songMap.ContainsKey(song))
          this.unplayedSongs.Add(song);
        else
          DebugUtil.DevLogError("Priority song not found in playlist: " + song);
      }
    }

    public void MarkSongPlayed(string song)
    {
      this.unplayedSongs.Remove(song);
      this.lastSongPlayed = song;
    }

    public string GetNextSong()
    {
      string unplayedSong;
      if (this.unplayedSongs.Count > 0)
      {
        int index = UnityEngine.Random.Range(0, this.unplayedSongs.Count);
        unplayedSong = this.unplayedSongs[index];
        this.unplayedSongs.RemoveAt(index);
      }
      else
      {
        this.ResetUnplayedSongs();
        bool flag = this.unplayedSongs.Count > 1;
        if (flag)
        {
          for (int index = 0; index < this.unplayedSongs.Count; ++index)
          {
            if (this.unplayedSongs[index] == this.lastSongPlayed)
            {
              this.unplayedSongs.Remove(this.unplayedSongs[index]);
              break;
            }
          }
        }
        int index1 = UnityEngine.Random.Range(0, this.unplayedSongs.Count);
        unplayedSong = this.unplayedSongs[index1];
        this.unplayedSongs.RemoveAt(index1);
        if (flag)
          this.unplayedSongs.Add(this.lastSongPlayed);
      }
      this.lastSongPlayed = unplayedSong;
      Debug.Assert(this.songMap.ContainsKey(unplayedSong), (object) ("Missing song " + unplayedSong));
      return Assets.GetSimpleSoundEventName(this.songMap[unplayedSong].fmodEvent);
    }

    public void ResetUnplayedSongs()
    {
      this.unplayedSongs.Clear();
      foreach (KeyValuePair<string, MusicManager.SongInfo> song in this.songMap)
      {
        if (MusicManager.IsValidForDLCContext(song.Value.requiredDlcId))
          this.unplayedSongs.Add(song.Key);
      }
    }
  }
}
