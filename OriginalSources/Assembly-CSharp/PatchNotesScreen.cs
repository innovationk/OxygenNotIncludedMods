// Decompiled with JetBrains decompiler
// Type: PatchNotesScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PatchNotesScreen : KModalScreen
{
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton okButton;
  [SerializeField]
  private KButton fullPatchNotes;
  [SerializeField]
  private KButton previousVersion;
  [SerializeField]
  private LocText changesLabel;
  private static string m_patchNotesUrl;
  private static string m_patchNotesText;
  private static int PatchNotesVersion = 9;
  private static PatchNotesScreen instance;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.changesLabel.text = PatchNotesScreen.m_patchNotesText;
    this.closeButton.onClick += new System.Action(this.MarkAsReadAndClose);
    this.closeButton.soundPlayer.widget_sound_events()[0].OverrideAssetName = "HUD_Click_Close";
    this.okButton.onClick += new System.Action(this.MarkAsReadAndClose);
    this.previousVersion.onClick += (System.Action) (() => App.OpenWebURL("http://support.kleientertainment.com/customer/portal/articles/2776550"));
    this.fullPatchNotes.onClick += new System.Action(this.OnPatchNotesClick);
    PatchNotesScreen.instance = this;
  }

  protected override void OnCleanUp() => PatchNotesScreen.instance = (PatchNotesScreen) null;

  public static bool ShouldShowScreen() => false;

  private void MarkAsReadAndClose()
  {
    KPlayerPrefs.SetInt("PatchNotesVersion", PatchNotesScreen.PatchNotesVersion);
    this.Deactivate();
  }

  public static void UpdatePatchNotes(string patchNotesSummary, string url)
  {
    PatchNotesScreen.m_patchNotesUrl = url;
    PatchNotesScreen.m_patchNotesText = patchNotesSummary;
    if (!((UnityEngine.Object) PatchNotesScreen.instance != (UnityEngine.Object) null))
      return;
    PatchNotesScreen.instance.changesLabel.text = PatchNotesScreen.m_patchNotesText;
  }

  private void OnPatchNotesClick() => App.OpenWebURL(PatchNotesScreen.m_patchNotesUrl);

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.MarkAsReadAndClose();
    else
      base.OnKeyDown(e);
  }
}
