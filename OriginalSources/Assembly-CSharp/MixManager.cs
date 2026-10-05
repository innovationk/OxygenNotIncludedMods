// Decompiled with JetBrains decompiler
// Type: MixManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MixManager : MonoBehaviour
{
  private void Update()
  {
    if (AudioMixer.instance == null || !AudioMixer.instance.persistentSnapshotsActive)
      return;
    AudioMixer.instance.UpdatePersistentSnapshotParameters();
  }

  private void OnApplicationFocus(bool hasFocus)
  {
    if (AudioMixer.instance == null || (Object) AudioMixerSnapshots.Get() == (Object) null)
      return;
    if (!hasFocus && KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1)
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().GameNotFocusedSnapshot);
    else
      AudioMixer.instance.Stop(AudioMixerSnapshots.Get().GameNotFocusedSnapshot);
  }
}
