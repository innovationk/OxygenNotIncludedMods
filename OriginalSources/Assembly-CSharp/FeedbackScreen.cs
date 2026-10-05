// Decompiled with JetBrains decompiler
// Type: FeedbackScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FeedbackScreen : KModalScreen
{
  public LocText title;
  public KButton dismissButton;
  public KButton closeButton;
  public KButton bugForumsButton;
  public KButton suggestionForumsButton;
  public KButton logsDirectoryButton;
  public KButton saveFilesDirectoryButton;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.title.SetText((string) STRINGS.UI.FRONTEND.FEEDBACK_SCREEN.TITLE);
    this.dismissButton.onClick += (System.Action) (() => this.Deactivate());
    this.closeButton.onClick += (System.Action) (() => this.Deactivate());
    this.bugForumsButton.onClick += (System.Action) (() => App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/"));
    this.suggestionForumsButton.onClick += (System.Action) (() => App.OpenWebURL("https://forums.kleientertainment.com/forums/forum/133-oxygen-not-included-suggestions-and-feedback/"));
    this.logsDirectoryButton.onClick += (System.Action) (() => App.OpenWebURL(Util.LogsFolder()));
    this.saveFilesDirectoryButton.onClick += (System.Action) (() => App.OpenWebURL(SaveLoader.GetSavePrefix()));
    if (!SteamUtils.IsSteamRunningOnSteamDeck())
      return;
    this.logsDirectoryButton.GetComponentInParent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 0, 0);
    this.saveFilesDirectoryButton.gameObject.SetActive(false);
    this.logsDirectoryButton.gameObject.SetActive(false);
  }
}
