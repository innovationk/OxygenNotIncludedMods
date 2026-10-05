// Decompiled with JetBrains decompiler
// Type: LadderSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LadderSoundEvent(string file_name, string sound_name, int frame) : SoundEvent(file_name, sound_name, frame, false, false, (float) SoundEvent.IGNORE_INTERVAL, true)
{
  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject);
    if (!this.objectIsSelectedAndVisible && !SoundEvent.ShouldPlaySound(behaviour.controller, this.sound, this.looping, this.isDynamic))
      return;
    Vector3 vector3 = behaviour.position with { z = 0.0f };
    float volume = 1f;
    if (this.objectIsSelectedAndVisible)
    {
      vector3 = SoundEvent.AudioHighlightListenerPosition(vector3);
      volume = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
    }
    int cell = Grid.PosToCell(vector3);
    BuildingDef buildingDef = (BuildingDef) null;
    if (Grid.IsValidCell(cell))
    {
      GameObject gameObject = Grid.Objects[cell, 1];
      if ((Object) gameObject != (Object) null && (Object) gameObject.GetComponent<Ladder>() != (Object) null)
      {
        Building component = (Building) gameObject.GetComponent<BuildingComplete>();
        if ((Object) component != (Object) null)
          buildingDef = component.Def;
      }
    }
    if (!((Object) buildingDef != (Object) null))
      return;
    string sound = GlobalAssets.GetSound(buildingDef.PrefabID == "LadderFast" ? StringFormatter.Combine(this.name, "_Plastic") : this.name);
    if (sound == null)
      return;
    SoundEvent.PlayOneShot(sound, vector3, volume);
  }
}
