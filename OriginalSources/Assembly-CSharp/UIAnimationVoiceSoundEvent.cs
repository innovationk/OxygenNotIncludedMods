// Decompiled with JetBrains decompiler
// Type: UIAnimationVoiceSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class UIAnimationVoiceSoundEvent : SoundEvent
{
  private string actualSoundName;
  private string lastPlayedLoopingSoundPath;

  public UIAnimationVoiceSoundEvent(string file_name, string sound_name, int frame, bool looping)
    : base(file_name, sound_name, frame, false, looping, (float) SoundEvent.IGNORE_INTERVAL, false)
  {
    this.actualSoundName = sound_name;
  }

  public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
  {
    this.PlaySound(behaviour);
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    string soundPath = MinionVoice.ByObject((UnityEngine.Object) behaviour.controller).UnwrapOr(MinionVoice.Random(), $"Couldn't find MinionVoice on UI {behaviour.controller}, falling back to random voice").GetSoundPath(this.actualSoundName);
    if (this.actualSoundName.Contains(":"))
    {
      float num = float.Parse(this.actualSoundName.Split(':', StringSplitOptions.None)[1]);
      if ((double) UnityEngine.Random.Range(0, 100) > (double) num)
        return;
    }
    if (this.looping)
    {
      LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        Debug.Log((object) (behaviour.name + " (UI Object) is missing LoopingSounds component."));
      else if (!component.StartSound(soundPath, false, false, false))
        DebugUtil.LogWarningArgs((object) $"SoundEvent has invalid sound [{soundPath}] on behaviour [{behaviour.name}]");
      this.lastPlayedLoopingSoundPath = soundPath;
    }
    else
    {
      try
      {
        if ((UnityEngine.Object) SoundListenerController.Instance == (UnityEngine.Object) null)
          KFMOD.PlayUISound(soundPath);
        else
          KFMOD.PlayOneShot(soundPath, SoundListenerController.Instance.transform.GetPosition());
      }
      catch
      {
        DebugUtil.LogWarningArgs((object) $"AUDIOERROR: Missing [{soundPath}]");
      }
    }
  }

  public override void Stop(AnimEventManager.EventPlayerData behaviour)
  {
    if (this.looping)
    {
      LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && this.lastPlayedLoopingSoundPath != null)
        component.StopSound(this.lastPlayedLoopingSoundPath);
    }
    this.lastPlayedLoopingSoundPath = (string) null;
  }
}
