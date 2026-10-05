// Decompiled with JetBrains decompiler
// Type: ClusterMapSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

#nullable disable
public class ClusterMapSoundEvent(string file_name, string sound_name, int frame, bool looping) : 
  SoundEvent(file_name, sound_name, frame, true, looping, (float) SoundEvent.IGNORE_INTERVAL, false)
{
  private static string X_POSITION_PARAMETER = "Starmap_Position_X";
  private static string Y_POSITION_PARAMETER = "Starmap_Position_Y";
  private static string ZOOM_PARAMETER = "Starmap_Zoom_Percentage";

  public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
  {
    if (!((Object) ClusterMapScreen.Instance != (Object) null) || !ClusterMapScreen.Instance.IsActive())
      return;
    this.PlaySound(behaviour);
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    if (this.looping)
    {
      LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
      if ((Object) component == (Object) null)
      {
        Debug.Log((object) (behaviour.name + " (Cluster Map Object) is missing LoopingSounds component."));
      }
      else
      {
        if (component.StartSound(this.sound, enable_culling: false, enable_camera_scaled_position: false))
          return;
        DebugUtil.LogWarningArgs((object) $"SoundEvent has invalid sound [{this.sound}] on behaviour [{behaviour.name}]");
      }
    }
    else
    {
      EventInstance instance = KFMOD.BeginOneShot(this.sound, Vector3.zero);
      int num1 = (int) instance.setParameterByName(ClusterMapSoundEvent.X_POSITION_PARAMETER, behaviour.controller.transform.GetPosition().x / (float) Screen.width);
      int num2 = (int) instance.setParameterByName(ClusterMapSoundEvent.Y_POSITION_PARAMETER, behaviour.controller.transform.GetPosition().y / (float) Screen.height);
      int num3 = (int) instance.setParameterByName(ClusterMapSoundEvent.ZOOM_PARAMETER, ClusterMapScreen.Instance.CurrentZoomPercentage());
      KFMOD.EndOneShot(instance);
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
