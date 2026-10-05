// Decompiled with JetBrains decompiler
// Type: UIAnimationSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class UIAnimationSoundEvent(string file_name, string sound_name, int frame, bool looping) : 
  SoundEvent(file_name, sound_name, frame, true, looping, (float) SoundEvent.IGNORE_INTERVAL, false)
{
  public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
  {
    this.PlaySound(behaviour);
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    if (this.looping)
    {
      LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
      if ((Object) component == (Object) null)
      {
        Debug.Log((object) (behaviour.name + " (UI Object) is missing LoopingSounds component."));
      }
      else
      {
        if (component.StartSound(this.sound, false, false, false))
          return;
        DebugUtil.LogWarningArgs((object) $"SoundEvent has invalid sound [{this.sound}] on behaviour [{behaviour.name}]");
      }
    }
    else
    {
      try
      {
        if ((Object) SoundListenerController.Instance == (Object) null)
          KFMOD.PlayUISound(this.sound);
        else
          KFMOD.PlayOneShot(this.sound, SoundListenerController.Instance.transform.GetPosition());
      }
      catch
      {
        DebugUtil.LogWarningArgs((object) $"AUDIOERROR: Missing [{this.sound}]");
      }
    }
  }

  public override void Stop(AnimEventManager.EventPlayerData behaviour)
  {
    if (!this.looping)
      return;
    LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
    if (!((Object) component != (Object) null))
      return;
    component.StopSound(this.sound);
  }
}
