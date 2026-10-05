// Decompiled with JetBrains decompiler
// Type: AudioMixer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD;
using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AudioMixer
{
  private static AudioMixer _instance = (AudioMixer) null;
  private const string DUPLICANT_COUNT_ID = "duplicantCount";
  private const string PULSE_ID = "Pulse";
  private const string SNAPSHOT_ACTIVE_ID = "snapshotActive";
  private const string SPACE_VISIBLE_ID = "spaceVisible";
  private const string FACILITY_VISIBLE_ID = "facilityVisible";
  private const string FOCUS_BUS_PATH = "bus:/SFX/Focus";
  public Dictionary<GUID, EventInstance> activeSnapshots = new Dictionary<GUID, EventInstance>();
  public List<HashedString> SnapshotDebugLog = new List<HashedString>();
  public bool activeNIS;
  public static float LOW_PRIORITY_CUTOFF_DISTANCE = 10f;
  public static float PULSE_SNAPSHOT_BPM = 120f;
  public static int VISIBLE_DUPLICANTS_BEFORE_ATTENUATION = 2;
  private EventInstance duplicantCountInst;
  private EventInstance pulseInst;
  private EventInstance duplicantCountMovingInst;
  private EventInstance duplicantCountSleepingInst;
  private EventInstance spaceVisibleInst;
  private EventInstance facilityVisibleInst;
  private static readonly HashedString UserVolumeSettingsHash = new HashedString("event:/Snapshots/Mixing/Snapshot_UserVolumeSettings");
  public bool persistentSnapshotsActive;
  private Dictionary<string, int> visibleDupes = new Dictionary<string, int>();
  public Dictionary<string, AudioMixer.UserVolumeBus> userVolumeSettings = new Dictionary<string, AudioMixer.UserVolumeBus>();

  public static AudioMixer instance => AudioMixer._instance;

  public static AudioMixer Create()
  {
    AudioMixer._instance = new AudioMixer();
    AudioMixerSnapshots audioMixerSnapshots = AudioMixerSnapshots.Get();
    if ((UnityEngine.Object) audioMixerSnapshots != (UnityEngine.Object) null)
      audioMixerSnapshots.ReloadSnapshots();
    return AudioMixer._instance;
  }

  public static void Destroy()
  {
    AudioMixer._instance.StopAll();
    AudioMixer._instance = (AudioMixer) null;
  }

  public EventInstance Start(EventReference event_ref)
  {
    RuntimeManager.GetEventDescription(event_ref.Guid);
    EventInstance eventInstance;
    if (!this.activeSnapshots.TryGetValue(event_ref.Guid, out eventInstance))
    {
      if (RuntimeManager.IsInitialized)
      {
        eventInstance = KFMOD.CreateInstance(event_ref);
        this.activeSnapshots[event_ref.Guid] = eventInstance;
        int num1 = (int) eventInstance.start();
        int num2 = (int) eventInstance.setParameterByName("snapshotActive", 1f);
      }
      else
        eventInstance = new EventInstance();
    }
    return eventInstance;
  }

  public bool Stop(EventReference event_ref, FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
  {
    return this.Stop(event_ref.Guid, stop_mode);
  }

  public bool Stop(GUID event_guid, FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
  {
    bool flag = false;
    EventInstance eventInstance;
    if (this.activeSnapshots.TryGetValue(event_guid, out eventInstance))
    {
      int num1 = (int) eventInstance.setParameterByName("snapshotActive", 0.0f);
      int num2 = (int) eventInstance.stop(stop_mode);
      int num3 = (int) eventInstance.release();
      this.activeSnapshots.Remove(event_guid);
      flag = true;
    }
    return flag;
  }

  public void Reset() => this.StopAll();

  public void StopAll(FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
  {
    List<GUID> guidList = new List<GUID>();
    GUID guid = AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot.Guid;
    foreach (KeyValuePair<GUID, EventInstance> activeSnapshot in this.activeSnapshots)
    {
      if (activeSnapshot.Key != guid)
        guidList.Add(activeSnapshot.Key);
    }
    for (int index = 0; index < guidList.Count; ++index)
      this.Stop(guidList[index], stop_mode);
  }

  public bool SnapshotIsActive(EventReference event_ref) => this.SnapshotIsActive(event_ref.Guid);

  public bool SnapshotIsActive(GUID guid) => this.activeSnapshots.ContainsKey(guid);

  public void SetSnapshotParameter(
    EventReference event_ref,
    string parameter_name,
    float parameter_value,
    bool shouldLog = true)
  {
    shouldLog = false;
    if (shouldLog)
      this.Log($"Set Param {this.GetSnapshotName(event_ref)}: {parameter_name}, {parameter_value}");
    if (!(!this.SetSnapshotParameter(event_ref.Guid, parameter_name, parameter_value) & shouldLog))
      return;
    this.Log($"Tried to set [{parameter_name}] to [{parameter_value.ToString()}] but [{this.GetSnapshotName(event_ref)}] is not active.");
  }

  private bool SetSnapshotParameter(GUID guid, string parameter_name, float parameter_value)
  {
    EventInstance eventInstance;
    if (!this.activeSnapshots.TryGetValue(guid, out eventInstance))
      return false;
    int num = (int) eventInstance.setParameterByName(parameter_name, parameter_value);
    return true;
  }

  public void StartPersistentSnapshots()
  {
    this.persistentSnapshotsActive = true;
    this.Start(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated);
    this.Start(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot);
    this.Start(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot);
    this.spaceVisibleInst = this.Start(AudioMixerSnapshots.Get().SpaceVisibleSnapshot);
    this.facilityVisibleInst = this.Start(AudioMixerSnapshots.Get().FacilityVisibleSnapshot);
    this.Start(AudioMixerSnapshots.Get().PulseSnapshot);
  }

  public void StopPersistentSnapshots()
  {
    this.persistentSnapshotsActive = false;
    this.Stop(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated);
    this.Stop(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot);
    this.Stop(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot);
    this.Stop(AudioMixerSnapshots.Get().SpaceVisibleSnapshot);
    this.Stop(AudioMixerSnapshots.Get().FacilityVisibleSnapshot);
    this.Stop(AudioMixerSnapshots.Get().PulseSnapshot);
  }

  private string GetSnapshotName(EventReference event_ref)
  {
    string path1;
    int path2 = (int) RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out path1);
    return path1;
  }

  public void UpdatePersistentSnapshotParameters()
  {
    this.SetVisibleDuplicants();
    if (this.activeSnapshots.TryGetValue(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot.Guid, out this.duplicantCountMovingInst))
    {
      int num1 = (int) this.duplicantCountMovingInst.setParameterByName("duplicantCount", (float) Mathf.Max(0, this.visibleDupes["moving"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION));
    }
    if (this.activeSnapshots.TryGetValue(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot.Guid, out this.duplicantCountSleepingInst))
    {
      int num2 = (int) this.duplicantCountSleepingInst.setParameterByName("duplicantCount", (float) Mathf.Max(0, this.visibleDupes["sleeping"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION));
    }
    if (this.activeSnapshots.TryGetValue(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated.Guid, out this.duplicantCountInst))
    {
      int num3 = (int) this.duplicantCountInst.setParameterByName("duplicantCount", (float) Mathf.Max(0, this.visibleDupes["visible"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION));
    }
    if (!this.activeSnapshots.TryGetValue(AudioMixerSnapshots.Get().PulseSnapshot.Guid, out this.pulseInst))
      return;
    float num4 = AudioMixer.PULSE_SNAPSHOT_BPM / 60f;
    switch (SpeedControlScreen.Instance.GetSpeed())
    {
      case 1:
        num4 /= 2f;
        break;
      case 2:
        num4 /= 3f;
        break;
    }
    int num5 = (int) this.pulseInst.setParameterByName("Pulse", Mathf.Abs(Mathf.Sin(Time.time * 3.14159274f * num4)));
  }

  public void UpdateSpaceVisibleSnapshot(float percent)
  {
    int num = (int) this.spaceVisibleInst.setParameterByName("spaceVisible", percent);
  }

  public void PauseSpaceVisibleSnapshot(bool pause)
  {
    int num1 = (int) this.spaceVisibleInst.setParameterByName("spaceVisible", 0.0f, true);
    int num2 = (int) this.spaceVisibleInst.setPaused(pause);
  }

  public void UpdateFacilityVisibleSnapshot(float percent)
  {
    int num = (int) this.facilityVisibleInst.setParameterByName("facilityVisible", percent);
  }

  private void SetVisibleDuplicants()
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    for (int idx = 0; idx < Components.LiveMinionIdentities.Count; ++idx)
    {
      if (CameraController.Instance.IsVisiblePos(Components.LiveMinionIdentities[idx].transform.GetPosition()))
      {
        ++num1;
        Navigator component = Components.LiveMinionIdentities[idx].GetComponent<Navigator>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.IsMoving())
        {
          ++num2;
        }
        else
        {
          StaminaMonitor.Instance smi = Components.LiveMinionIdentities[idx].GetComponent<WorkerBase>().GetSMI<StaminaMonitor.Instance>();
          if (smi != null && smi.IsSleeping())
            ++num3;
        }
      }
    }
    this.visibleDupes["visible"] = num1;
    this.visibleDupes["moving"] = num2;
    this.visibleDupes["sleeping"] = num3;
  }

  public void StartUserVolumesSnapshot()
  {
    this.Start(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
    EventInstance eventInstance;
    if (!this.activeSnapshots.TryGetValue(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot.Guid, out eventInstance))
      return;
    EventDescription description1;
    int description2 = (int) eventInstance.getDescription(out description1);
    USER_PROPERTY property;
    int userProperty = (int) description1.getUserProperty("buses", out property);
    string[] strArray = property.stringValue().Split('-', StringSplitOptions.None);
    for (int index = 0; index < strArray.Length; ++index)
    {
      float num = 1f;
      string key = "Volume_" + strArray[index];
      if (KPlayerPrefs.HasKey(key))
        num = KPlayerPrefs.GetFloat(key);
      AudioMixer.UserVolumeBus userVolumeBus = new AudioMixer.UserVolumeBus();
      userVolumeBus.busLevel = num;
      userVolumeBus.labelString = (string) Strings.Get("STRINGS.UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUDIO_BUS_" + strArray[index].ToUpper());
      this.userVolumeSettings.Add(strArray[index], userVolumeBus);
      this.SetUserVolume(strArray[index], userVolumeBus.busLevel);
    }
  }

  public void SetUserVolume(string bus, float value)
  {
    if (!this.userVolumeSettings.ContainsKey(bus))
    {
      Debug.LogError((object) "The provided bus doesn't exist. Check yo'self fool!");
    }
    else
    {
      if ((double) value > 1.0)
        value = 1f;
      else if ((double) value < 0.0)
        value = 0.0f;
      this.userVolumeSettings[bus].busLevel = value;
      KPlayerPrefs.SetFloat("Volume_" + bus, value);
      EventInstance eventInstance;
      if (this.activeSnapshots.TryGetValue(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot.Guid, out eventInstance))
      {
        int num = (int) eventInstance.setParameterByName("userVolume_" + bus, this.userVolumeSettings[bus].busLevel);
      }
      if (!(bus == "Music"))
        return;
      this.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "userVolume_Music", value);
    }
  }

  private void Log(string s)
  {
  }

  public class UserVolumeBus
  {
    public string labelString;
    public float busLevel;
  }
}
