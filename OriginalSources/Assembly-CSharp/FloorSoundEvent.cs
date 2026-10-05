// Decompiled with JetBrains decompiler
// Type: FloorSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;
using UnityEngine;

#nullable disable
[DebuggerDisplay("{Name}")]
public class FloorSoundEvent : SoundEvent
{
  public static float IDLE_WALKING_VOLUME_REDUCTION = 0.55f;

  public FloorSoundEvent(string file_name, string sound_name, int frame)
    : base(file_name, sound_name, frame, false, false, (float) SoundEvent.IGNORE_INTERVAL, true)
  {
    this.noiseValues = SoundEventVolumeCache.instance.GetVolume(nameof (FloorSoundEvent), sound_name);
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    Vector3 pos = behaviour.position;
    KBatchedAnimController controller = behaviour.controller;
    if ((Object) controller != (Object) null)
      pos = controller.GetPivotSymbolPosition();
    int cell1 = Grid.PosToCell(pos);
    int cell2 = Grid.CellBelow(cell1);
    if (!Grid.IsValidCell(cell2))
      return;
    string str = GlobalAssets.GetSound(StringFormatter.Combine(FloorSoundEvent.GetAudioCategory(cell2), "_", this.name), true) ?? GlobalAssets.GetSound(StringFormatter.Combine("Rock_", this.name), true) ?? GlobalAssets.GetSound(this.name, true);
    GameObject gameObject = behaviour.controller.gameObject;
    MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
    this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
    if (SoundEvent.IsLowPrioritySound(str) && !this.objectIsSelectedAndVisible)
      return;
    Vector3 vector3 = SoundEvent.GetCameraScaledPosition(pos) with
    {
      z = 0.0f
    };
    if (this.objectIsSelectedAndVisible)
      vector3 = SoundEvent.AudioHighlightListenerPosition(vector3);
    if (Grid.Element == null)
      return;
    int num1 = Grid.Element[cell1].IsLiquid ? 1 : 0;
    float num2 = 0.0f;
    bool flag = Grid.IsSubstantialLiquid(Grid.CellAbove(cell1));
    if (num1 != 0)
    {
      num2 = SoundUtil.GetLiquidDepth(cell1);
      string sound = GlobalAssets.GetSound(flag ? "uw_footstep" : "Liquid_footstep");
      if (sound != null && (this.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, sound, this.looping, this.isDynamic)))
      {
        FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(sound, vector3, SoundEvent.GetVolume(this.objectIsSelectedAndVisible));
        if ((double) num2 > 0.0 && !flag)
        {
          int num3 = (int) instance.setParameterByName("liquidDepth", num2);
        }
        SoundEvent.EndOneShot(instance);
      }
    }
    if ((Object) component != (Object) null && component.model == BionicMinionConfig.MODEL)
    {
      string sound = GlobalAssets.GetSound("Bionic_move", true);
      if (sound != null && (this.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, sound, this.looping, this.isDynamic)))
        SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, vector3, SoundEvent.GetVolume(this.objectIsSelectedAndVisible)));
    }
    if (str == null || !this.objectIsSelectedAndVisible && !SoundEvent.ShouldPlaySound(behaviour.controller, str, this.looping, this.isDynamic))
      return;
    FMOD.Studio.EventInstance instance1 = SoundEvent.BeginOneShot(str, vector3);
    if (!instance1.isValid())
      return;
    if ((double) num2 > 0.0)
    {
      int num4 = (int) instance1.setParameterByName("liquidDepth", num2);
    }
    if (behaviour.controller.HasAnimationFile((KAnimHashedString) "anim_loco_walk_kanim"))
    {
      int num5 = (int) instance1.setVolume(FloorSoundEvent.IDLE_WALKING_VOLUME_REDUCTION);
    }
    SoundEvent.EndOneShot(instance1);
  }

  private static string GetAudioCategory(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return "Rock";
    Element element = Grid.Element[cell];
    if (Grid.Foundation[cell])
    {
      BuildingDef buildingDef = (BuildingDef) null;
      GameObject gameObject = Grid.Objects[cell, 1];
      if ((Object) gameObject != (Object) null)
      {
        Building component = (Building) gameObject.GetComponent<BuildingComplete>();
        if ((Object) component != (Object) null)
          buildingDef = component.Def;
      }
      string audioCategory = "";
      if ((Object) buildingDef != (Object) null)
      {
        switch (buildingDef.PrefabID)
        {
          case "PlasticTile":
            audioCategory = "TilePlastic";
            break;
          case "GlassTile":
            audioCategory = "TileGlass";
            break;
          case "BunkerTile":
            audioCategory = "TileBunker";
            break;
          case "MetalTile":
            audioCategory = "TileMetal";
            break;
          case "CarpetTile":
            audioCategory = "Carpet";
            break;
          case "SnowTile":
            audioCategory = "TileSnow";
            break;
          case "WoodTile":
            audioCategory = "TileWood";
            break;
          case "RubberTile":
            audioCategory = "TileRubber";
            break;
          default:
            audioCategory = "Tile";
            break;
        }
      }
      return audioCategory;
    }
    string eventAudioCategory = element.substance.GetFloorEventAudioCategory();
    if (eventAudioCategory != null)
      return eventAudioCategory;
    if (element.HasTag(GameTags.RefinedMetal))
      return "RefinedMetal";
    return element.HasTag(GameTags.Metal) ? "RawMetal" : "Rock";
  }
}
