// Decompiled with JetBrains decompiler
// Type: HealthyGameMessageScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/HealthyGameMessageScreen")]
public class HealthyGameMessageScreen : KMonoBehaviour
{
  public KButton confirmButton;
  public CanvasGroup canvasGroup;
  private float spawnTime;
  private float totalTime = 10f;
  private float fadeTime = 1.5f;
  private bool isFirstUpdate = true;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.confirmButton.onClick += (System.Action) (() => this.PlayIntroShort());
    this.confirmButton.gameObject.SetActive(false);
  }

  private void PlayIntroShort()
  {
    string str = KPlayerPrefs.GetString("PlayShortOnLaunch", "");
    if (!string.IsNullOrEmpty(MainMenu.Instance.IntroShortName) && str != MainMenu.Instance.IntroShortName)
    {
      VideoScreen component = KScreenManager.AddChild(FrontEndManager.Instance.gameObject, ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
      component.PlayVideo(Assets.GetVideo(MainMenu.Instance.IntroShortName), overrideAudioSnapshot: AudioMixerSnapshots.Get().MainMenuVideoPlayingSnapshot);
      component.OnStop += (System.Action) (() =>
      {
        KPlayerPrefs.SetString("PlayShortOnLaunch", MainMenu.Instance.IntroShortName);
        if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
          return;
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      });
    }
    else
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  private void Update()
  {
    if (!DistributionPlatform.Inst.IsDLCStatusReady())
      return;
    if (this.isFirstUpdate)
    {
      this.isFirstUpdate = false;
      this.spawnTime = Time.unscaledTime;
    }
    else
    {
      float num1 = Mathf.Min(Time.unscaledDeltaTime, 0.0333333351f);
      float num2 = Time.unscaledTime - this.spawnTime;
      if ((double) num2 < (double) this.totalTime - (double) this.fadeTime)
        this.canvasGroup.alpha += num1 * (1f / this.fadeTime);
      else if ((double) num2 >= (double) this.totalTime + 0.75)
      {
        this.canvasGroup.alpha = 1f;
        this.confirmButton.gameObject.SetActive(true);
      }
      else
      {
        if ((double) num2 < (double) this.totalTime - (double) this.fadeTime)
          return;
        this.canvasGroup.alpha -= num1 * (1f / this.fadeTime);
      }
    }
  }
}
