// Decompiled with JetBrains decompiler
// Type: PhonoboxSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using System;
using UnityEngine;

#nullable disable
public class PhonoboxSoundEvent(
  string file_name,
  string sound_name,
  int frame,
  float min_interval) : SoundEvent(file_name, sound_name, frame, true, true, min_interval, false)
{
  private const string SOUND_PARAM_SONG = "jukeboxSong";
  private const string SOUND_PARAM_PITCH = "jukeboxPitch";
  private int song;
  private int pitch;

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    Vector3 position = behaviour.position with { z = 0.0f };
    AudioDebug audioDebug = AudioDebug.Get();
    if ((UnityEngine.Object) audioDebug != (UnityEngine.Object) null && audioDebug.debugSoundEvents)
      Debug.Log((object) $"{behaviour.name}, {this.sound}, {this.frame.ToString()}, {position.ToString()}");
    try
    {
      LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      {
        Debug.Log((object) (behaviour.name + " is missing LoopingSounds component. "));
      }
      else
      {
        if (component.IsSoundPlaying(this.sound))
          return;
        if (component.StartSound(this.sound, behaviour, this.noiseValues, this.ignorePause))
        {
          EventDescription eventDescription = RuntimeManager.GetEventDescription(this.sound);
          PARAMETER_DESCRIPTION parameter1;
          int descriptionByName1 = (int) eventDescription.getParameterDescriptionByName("jukeboxSong", out parameter1);
          int maximum1 = (int) parameter1.maximum;
          PARAMETER_DESCRIPTION parameter2;
          int descriptionByName2 = (int) eventDescription.getParameterDescriptionByName("jukeboxPitch", out parameter2);
          int maximum2 = (int) parameter2.maximum;
          this.song = UnityEngine.Random.Range(0, maximum1 + 1);
          this.pitch = UnityEngine.Random.Range(0, maximum2 + 1);
          component.UpdateFirstParameter(this.sound, (HashedString) "jukeboxSong", (float) this.song);
          component.UpdateSecondParameter(this.sound, (HashedString) "jukeboxPitch", (float) this.pitch);
        }
        else
          DebugUtil.LogWarningArgs((object) $"SoundEvent has invalid sound [{this.sound}] on behaviour [{behaviour.name}]");
      }
    }
    catch (Exception ex)
    {
      string message = string.Format("Error trying to trigger sound [{0}] in behaviour [{1}] [{2}]\n{3}" + this.sound != null ? this.sound.ToString() : "null", (object) behaviour.GetType().ToString(), (object) ex.Message, (object) ex.StackTrace);
      Debug.LogError((object) message);
      throw new ArgumentException(message, ex);
    }
  }
}
