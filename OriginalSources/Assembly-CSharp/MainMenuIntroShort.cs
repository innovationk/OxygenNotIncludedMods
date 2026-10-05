// Decompiled with JetBrains decompiler
// Type: MainMenuIntroShort
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/MainMenuIntroShort")]
public class MainMenuIntroShort : KMonoBehaviour
{
  [SerializeField]
  private bool alwaysPlay;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    string str = KPlayerPrefs.GetString("PlayShortOnLaunch", "");
    if ((string.IsNullOrEmpty(MainMenu.Instance.IntroShortName) ? 0 : (str != MainMenu.Instance.IntroShortName ? 1 : 0)) != 0)
    {
      VideoScreen component = KScreenManager.AddChild(FrontEndManager.Instance.gameObject, ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
      component.PlayVideo(Assets.GetVideo(MainMenu.Instance.IntroShortName), overrideAudioSnapshot: AudioMixerSnapshots.Get().MainMenuVideoPlayingSnapshot);
      component.OnStop += (System.Action) (() =>
      {
        KPlayerPrefs.SetString("PlayShortOnLaunch", MainMenu.Instance.IntroShortName);
        this.gameObject.SetActive(false);
      });
    }
    else
      this.gameObject.SetActive(false);
  }
}
