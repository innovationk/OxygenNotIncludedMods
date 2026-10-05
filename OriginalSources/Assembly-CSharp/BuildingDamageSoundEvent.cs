// Decompiled with JetBrains decompiler
// Type: BuildingDamageSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class BuildingDamageSoundEvent(string file_name, string sound_name, int frame) : SoundEvent(file_name, sound_name, frame, false, false, (float) SoundEvent.IGNORE_INTERVAL, false)
{
  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    Vector3 sound_pos = behaviour.position with { z = 0.0f };
    this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject);
    if (this.objectIsSelectedAndVisible)
      sound_pos = SoundEvent.AudioHighlightListenerPosition(sound_pos);
    WorkerBase component1 = behaviour.GetComponent<WorkerBase>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
    {
      string sound = GlobalAssets.GetSound("Building_Dmg_Metal");
      if (this.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, sound, this.looping, this.isDynamic))
      {
        SoundEvent.PlayOneShot(this.sound, sound_pos, SoundEvent.GetVolume(this.objectIsSelectedAndVisible));
        return;
      }
    }
    Workable workable = component1.GetWorkable();
    if (!((UnityEngine.Object) workable != (UnityEngine.Object) null))
      return;
    Building component2 = workable.GetComponent<Building>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return;
    string sound1 = GlobalAssets.GetSound(StringFormatter.Combine(this.name, "_", component2.Def.AudioCategory)) ?? GlobalAssets.GetSound("Building_Dmg_Metal");
    if (sound1 == null || !this.objectIsSelectedAndVisible && !SoundEvent.ShouldPlaySound(behaviour.controller, sound1, this.looping, this.isDynamic))
      return;
    SoundEvent.PlayOneShot(sound1, sound_pos, SoundEvent.GetVolume(this.objectIsSelectedAndVisible));
  }
}
