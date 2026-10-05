// Decompiled with JetBrains decompiler
// Type: CreatureVariationSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CreatureVariationSoundEvent(
  string file_name,
  string sound_name,
  int frame,
  bool do_load,
  bool is_looping,
  float min_interval,
  bool is_dynamic) : SoundEvent(file_name, sound_name, frame, do_load, is_looping, min_interval, is_dynamic)
{
  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    string sound1 = this.sound;
    CreatureBrain component = behaviour.GetComponent<CreatureBrain>();
    if ((Object) component != (Object) null && !string.IsNullOrEmpty(component.symbolPrefix))
    {
      string sound2 = GlobalAssets.GetSound(StringFormatter.Combine(component.symbolPrefix, this.name));
      if (!string.IsNullOrEmpty(sound2))
        sound1 = sound2;
    }
    this.PlaySound(behaviour, sound1);
  }

  public override void Stop(AnimEventManager.EventPlayerData behaviour)
  {
    if (!this.looping)
      return;
    LoopingSounds component1 = behaviour.GetComponent<LoopingSounds>();
    if (!((Object) component1 != (Object) null))
      return;
    string asset = this.sound;
    CreatureBrain component2 = behaviour.GetComponent<CreatureBrain>();
    if ((Object) component2 != (Object) null && !string.IsNullOrEmpty(component2.symbolPrefix))
    {
      string sound = GlobalAssets.GetSound(StringFormatter.Combine(component2.symbolPrefix, this.name));
      if (!string.IsNullOrEmpty(sound))
        asset = sound;
    }
    component1.StopSound(asset);
  }
}
