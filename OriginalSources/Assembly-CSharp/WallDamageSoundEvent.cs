// Decompiled with JetBrains decompiler
// Type: WallDamageSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

#nullable disable
public class WallDamageSoundEvent(
  string file_name,
  string sound_name,
  int frame,
  float min_interval) : SoundEvent(file_name, sound_name, frame, true, false, min_interval, false)
{
  public int tile;

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    Vector3 vector3_1 = new Vector3();
    AggressiveChore.StatesInstance smi = behaviour.controller.gameObject.GetSMI<AggressiveChore.StatesInstance>();
    if (smi == null)
      return;
    this.tile = smi.sm.wallCellToBreak;
    int audioCategory = WallDamageSoundEvent.GetAudioCategory(this.tile);
    Vector3 vector3_2 = Grid.CellToPos(this.tile) with
    {
      z = 0.0f
    };
    GameObject gameObject = behaviour.controller.gameObject;
    if (this.objectIsSelectedAndVisible)
      vector3_2 = SoundEvent.AudioHighlightListenerPosition(vector3_2);
    if (!this.objectIsSelectedAndVisible && !SoundEvent.ShouldPlaySound(behaviour.controller, this.sound, this.soundHash, this.looping, this.isDynamic))
      return;
    EventInstance instance = SoundEvent.BeginOneShot(this.sound, vector3_2, SoundEvent.GetVolume(this.objectIsSelectedAndVisible));
    int num = (int) instance.setParameterByName("material_ID", (float) audioCategory);
    SoundEvent.EndOneShot(instance);
  }

  private static int GetAudioCategory(int tile)
  {
    Element element = Grid.Element[tile];
    if (Grid.Foundation[tile])
      return 12;
    if (element.id == SimHashes.Dirt)
      return 0;
    if (element.id == SimHashes.CrushedIce || element.id == SimHashes.Ice || element.id == SimHashes.DirtyIce)
      return 1;
    if (element.id == SimHashes.OxyRock)
      return 3;
    if (element.HasTag(GameTags.Metal))
      return 5;
    if (element.HasTag(GameTags.RefinedMetal))
      return 6;
    if (element.id == SimHashes.Sand)
      return 8;
    return element.id == SimHashes.Algae ? 10 : 7;
  }
}
