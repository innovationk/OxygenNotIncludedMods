// Decompiled with JetBrains decompiler
// Type: AudioSheets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class AudioSheets : ScriptableObject
{
  public List<AudioSheet> sheets = new List<AudioSheet>();
  public Dictionary<HashedString, HashedString> skinToBaseAnim = new Dictionary<HashedString, HashedString>();
  public Dictionary<HashedString, List<AnimEvent>> events = new Dictionary<HashedString, List<AnimEvent>>();

  public virtual void Initialize()
  {
    foreach (AudioSheet sheet in this.sheets)
    {
      foreach (AudioSheet.SoundInfo soundInfo in sheet.soundInfos)
      {
        if (DlcManager.IsContentSubscribed(soundInfo.RequiredDlcId))
        {
          string type = soundInfo.Type;
          if (type == null || type == "")
            type = sheet.defaultType;
          this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name0, soundInfo.Frame0, soundInfo.RequiredDlcId);
          this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name1, soundInfo.Frame1, soundInfo.RequiredDlcId);
          this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name2, soundInfo.Frame2, soundInfo.RequiredDlcId);
          this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name3, soundInfo.Frame3, soundInfo.RequiredDlcId);
          this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name4, soundInfo.Frame4, soundInfo.RequiredDlcId);
          this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name5, soundInfo.Frame5, soundInfo.RequiredDlcId);
          this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name6, soundInfo.Frame6, soundInfo.RequiredDlcId);
          this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name7, soundInfo.Frame7, soundInfo.RequiredDlcId);
          this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name8, soundInfo.Frame8, soundInfo.RequiredDlcId);
          this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name9, soundInfo.Frame9, soundInfo.RequiredDlcId);
          this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name10, soundInfo.Frame10, soundInfo.RequiredDlcId);
          this.CreateSound(soundInfo.File, soundInfo.Anim, type, soundInfo.MinInterval, soundInfo.Name11, soundInfo.Frame11, soundInfo.RequiredDlcId);
        }
      }
    }
  }

  private void CreateSound(
    string file_name,
    string anim_name,
    string type,
    float min_interval,
    string sound_name,
    int frame,
    string dlcId)
  {
    if (string.IsNullOrEmpty(sound_name))
      return;
    HashedString key = (HashedString) $"{file_name}.{anim_name}";
    AnimEvent soundOfType = this.CreateSoundOfType(type, file_name, sound_name, frame, min_interval, dlcId);
    if (soundOfType == null)
    {
      Debug.LogError((object) ("Unknown sound type: " + type));
    }
    else
    {
      List<AnimEvent> animEventList = (List<AnimEvent>) null;
      if (!this.events.TryGetValue(key, out animEventList))
      {
        animEventList = new List<AnimEvent>();
        this.events[key] = animEventList;
      }
      animEventList.Add(soundOfType);
    }
  }

  protected abstract AnimEvent CreateSoundOfType(
    string type,
    string file_name,
    string sound_name,
    int frame,
    float min_interval,
    string dlcId);

  public List<AnimEvent> GetEvents(HashedString anim_id)
  {
    List<AnimEvent> events = (List<AnimEvent>) null;
    HashedString hashedString;
    if (this.skinToBaseAnim.TryGetValue(anim_id, out hashedString))
      anim_id = hashedString;
    this.events.TryGetValue(anim_id, out events);
    return events;
  }
}
