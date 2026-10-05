// Decompiled with JetBrains decompiler
// Type: PlantMutationSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PlantMutationSoundEvent(
  string file_name,
  string sound_name,
  int frame,
  float min_interval) : SoundEvent(file_name, sound_name, frame, false, false, min_interval, true)
{
  public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
  {
    MutantPlant component = behaviour.controller.gameObject.GetComponent<MutantPlant>();
    Vector3 position = behaviour.position;
    if (!((Object) component != (Object) null))
      return;
    for (int index = 0; index < component.GetSoundEvents().Count; ++index)
      SoundEvent.PlayOneShot(component.GetSoundEvents()[index], position);
  }
}
