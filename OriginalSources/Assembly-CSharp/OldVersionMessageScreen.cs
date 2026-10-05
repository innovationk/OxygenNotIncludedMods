// Decompiled with JetBrains decompiler
// Type: OldVersionMessageScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/SplashMessageScreen")]
public class OldVersionMessageScreen : KModalScreen
{
  public KButton forumButton;
  public KButton confirmButton;
  public KButton quitButton;
  public LocText bodyText;
  public bool previewInEditor;
  public RectTransform messageContainer;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.forumButton.onClick += (System.Action) (() => App.OpenWebURL("https://forums.kleientertainment.com/forums/topic/140474-previous-update-steam-branch-access/"));
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
    this.messageContainer.sizeDelta = new Vector2(Mathf.Max(384f, (float) Screen.width * 0.25f), this.messageContainer.sizeDelta.y);
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
  }
}
