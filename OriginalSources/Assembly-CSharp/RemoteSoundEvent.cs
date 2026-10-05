// Decompiled with JetBrains decompiler
// Type: RemoteSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using UnityEngine;

#nullable disable
[Serializable]
public class RemoteSoundEvent(string file_name, string sound_name, int frame, float min_interval) : 
  SoundEvent(file_name, sound_name, frame, true, false, min_interval, false)
{
  private const string STATE_PARAMETER = "State";

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    Vector3 vector3 = behaviour.position with { z = 0.0f };
    if (SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject))
      vector3 = SoundEvent.AudioHighlightListenerPosition(vector3);
    Workable workable = behaviour.GetComponent<WorkerBase>().GetWorkable();
    if (!((UnityEngine.Object) workable != (UnityEngine.Object) null))
      return;
    Toggleable component = workable.GetComponent<Toggleable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    IToggleHandler handlerForWorker = component.GetToggleHandlerForWorker(behaviour.GetComponent<WorkerBase>());
    float num1 = 1f;
    if (handlerForWorker != null && handlerForWorker.IsHandlerOn())
      num1 = 0.0f;
    if (!this.objectIsSelectedAndVisible && !SoundEvent.ShouldPlaySound(behaviour.controller, this.sound, this.soundHash, this.looping, this.isDynamic))
      return;
    EventInstance instance = SoundEvent.BeginOneShot(this.sound, vector3, SoundEvent.GetVolume(this.objectIsSelectedAndVisible));
    int num2 = (int) instance.setParameterByName("State", num1);
    SoundEvent.EndOneShot(instance);
  }
}
