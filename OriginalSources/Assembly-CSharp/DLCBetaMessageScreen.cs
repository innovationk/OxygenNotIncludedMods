// Decompiled with JetBrains decompiler
// Type: DLCBetaMessageScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DLCBetaMessageScreen : KModalScreen
{
  public RectTransform logo;
  public KButton confirmButton;
  public KButton quitButton;
  public LocText bodyText;
  public RectTransform messageContainer;
  private bool betaIsLive;
  private bool skipInEditor;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.confirmButton.onClick += (System.Action) (() =>
    {
      this.gameObject.SetActive(false);
      AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
    });
    this.quitButton.onClick += (System.Action) (() => App.Quit());
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!this.betaIsLive || Application.isEditor && this.skipInEditor || !DlcManager.IsContentSubscribed("DLC5_ID"))
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    else
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
  }

  private void Update()
  {
    this.logo.rectTransform().localPosition = new Vector3(0.0f, Mathf.Sin(Time.realtimeSinceStartup) * 7.5f);
  }
}
