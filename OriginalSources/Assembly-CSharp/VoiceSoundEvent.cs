// Decompiled with JetBrains decompiler
// Type: VoiceSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Klei.AI;
using System;
using UnityEngine;

#nullable disable
public class VoiceSoundEvent : SoundEvent
{
  public static float locomotionSoundProb = 50f;
  public float timeLastSpoke;
  public float intervalBetweenSpeaking = 10f;

  public VoiceSoundEvent(string file_name, string sound_name, int frame, bool is_looping)
    : base(file_name, sound_name, frame, false, is_looping, (float) SoundEvent.IGNORE_INTERVAL, true)
  {
    this.noiseValues = SoundEventVolumeCache.instance.GetVolume(nameof (VoiceSoundEvent), sound_name);
  }

  public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
  {
    VoiceSoundEvent.PlayVoice(this.name, behaviour.controller, this.intervalBetweenSpeaking, this.looping);
  }

  public static EventInstance PlayVoice(
    string name,
    KBatchedAnimController controller,
    float interval_between_speaking,
    bool looping,
    bool objectIsSelectedAndVisible = false)
  {
    EventInstance instance = new EventInstance();
    MinionIdentity component1 = controller.GetComponent<MinionIdentity>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null || name.Contains("state") && (double) Time.time - (double) component1.timeLastSpoke < (double) interval_between_speaking)
      return instance;
    bool flag = component1.model == BionicMinionConfig.MODEL;
    if (name.Contains(":"))
    {
      float num = float.Parse(name.Split(':', StringSplitOptions.None)[1]);
      if ((double) UnityEngine.Random.Range(0, 100) > (double) num)
        return instance;
    }
    WorkerBase component2 = controller.GetComponent<WorkerBase>();
    string assetName = VoiceSoundEvent.GetAssetName(name, (Component) component2);
    StaminaMonitor.Instance smi = component2.GetSMI<StaminaMonitor.Instance>();
    if (!name.Contains("sleep_") && smi != null && smi.IsSleeping())
      return instance;
    Vector3 vector3 = component2.transform.GetPosition() with
    {
      z = 0.0f
    };
    if (SoundEvent.ObjectIsSelectedAndVisible(controller.gameObject))
      vector3 = SoundEvent.AudioHighlightListenerPosition(vector3);
    string sound = GlobalAssets.GetSound(assetName, true);
    if (!SoundEvent.ShouldPlaySound(controller, sound, looping, false))
      return instance;
    if (sound != null)
    {
      if (looping)
      {
        LoopingSounds component3 = controller.GetComponent<LoopingSounds>();
        if ((UnityEngine.Object) component3 == (UnityEngine.Object) null)
          Debug.Log((object) (controller.name + " is missing LoopingSounds component. "));
        else if (!component3.StartSound(sound))
          DebugUtil.LogWarningArgs((object) $"SoundEvent has invalid sound [{sound}] on behaviour [{controller.name}]");
        else
          component3.UpdateFirstParameter(sound, (HashedString) "isBionic", flag ? 1f : 0.0f);
      }
      else
      {
        instance = SoundEvent.BeginOneShot(sound, vector3);
        int num1 = (int) instance.setParameterByName("isBionic", flag ? 1f : 0.0f);
        if (sound.Contains("sleep_") && controller.GetComponent<Traits>().HasTrait("Snorer"))
        {
          int num2 = (int) instance.setParameterByName("snoring", 1f);
        }
        SoundEvent.EndOneShot(instance);
        component1.timeLastSpoke = Time.time;
      }
    }
    else if (AudioDebug.Get().debugVoiceSounds)
      Debug.LogWarning((object) ("Missing voice sound: " + assetName));
    return instance;
  }

  private static string GetAssetName(string name, Component cmp)
  {
    string b = "F01";
    if ((UnityEngine.Object) cmp != (UnityEngine.Object) null)
    {
      MinionIdentity component = cmp.GetComponent<MinionIdentity>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        b = component.GetVoiceId();
    }
    string d = name;
    if (name.Contains(":"))
      d = name.Split(':', StringSplitOptions.None)[0];
    return StringFormatter.Combine("DupVoc_", b, "_", d);
  }

  public override void Stop(AnimEventManager.EventPlayerData behaviour)
  {
    if (!this.looping)
      return;
    LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    string sound = GlobalAssets.GetSound(VoiceSoundEvent.GetAssetName(this.name, (Component) component), true);
    component.StopSound(sound);
  }
}
