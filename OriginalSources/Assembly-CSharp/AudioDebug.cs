// Decompiled with JetBrains decompiler
// Type: AudioDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/AudioDebug")]
public class AudioDebug : KMonoBehaviour
{
  private static AudioDebug instance;
  public bool musicEnabled;
  public bool debugSoundEvents;
  public bool debugFloorSounds;
  public bool debugGameEventSounds;
  public bool debugNotificationSounds;
  public bool debugVoiceSounds;

  public static AudioDebug Get() => AudioDebug.instance;

  protected override void OnPrefabInit() => AudioDebug.instance = this;

  public void ToggleMusic()
  {
    if ((Object) Game.Instance != (Object) null)
      Game.Instance.SetMusicEnabled(this.musicEnabled);
    this.musicEnabled = !this.musicEnabled;
  }
}
