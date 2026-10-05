// Decompiled with JetBrains decompiler
// Type: PauseScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using ProcGen;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class PauseScreen : KModalButtonMenu
{
  [SerializeField]
  private OptionsMenuScreen optionsScreen;
  [SerializeField]
  private SaveScreen saveScreenPrefab;
  [SerializeField]
  private LoadScreen loadScreenPrefab;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private LocText title;
  [SerializeField]
  private LocText worldSeed;
  [SerializeField]
  private CopyTextFieldToClipboard clipboard;
  [SerializeField]
  private MultiToggle dlc1ActivationButton;
  [SerializeField]
  private GameObject dlcActivationButtonPrefab;
  private Dictionary<string, GameObject> dlcActivationButtons = new Dictionary<string, GameObject>();
  private float originalTimeScale;
  private bool recentlySaved;
  private static PauseScreen instance;

  public static PauseScreen Instance => PauseScreen.instance;

  public static void DestroyInstance() => PauseScreen.instance = (PauseScreen) null;

  protected override void OnPrefabInit()
  {
    this.keepMenuOpen = true;
    base.OnPrefabInit();
    this.ConfigureButtonInfos();
    this.closeButton.onClick += new System.Action(this.OnResume);
    PauseScreen.instance = this;
    this.Show(false);
  }

  private void ConfigureButtonInfos()
  {
    if (!GenericGameSettings.instance.demoMode)
      this.buttons = (IList<KButtonMenu.ButtonInfo>) new KButtonMenu.ButtonInfo[9]
      {
        new KButtonMenu.ButtonInfo((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.RESUME, Action.NumActions, new UnityAction(this.OnResume)),
        new KButtonMenu.ButtonInfo((string) (this.recentlySaved ? STRINGS.UI.FRONTEND.PAUSE_SCREEN.ALREADY_SAVED : STRINGS.UI.FRONTEND.PAUSE_SCREEN.SAVE), Action.NumActions, new UnityAction(this.OnSave)),
        new KButtonMenu.ButtonInfo((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.SAVEAS, Action.NumActions, new UnityAction(this.OnSaveAs)),
        new KButtonMenu.ButtonInfo((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.LOAD, Action.NumActions, new UnityAction(this.OnLoad)),
        new KButtonMenu.ButtonInfo((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.OPTIONS, Action.NumActions, new UnityAction(this.OnOptions)),
        new KButtonMenu.ButtonInfo((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.COLONY_SUMMARY, Action.NumActions, new UnityAction(this.OnColonySummary)),
        new KButtonMenu.ButtonInfo((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.LOCKERMENU, Action.NumActions, new UnityAction(this.OnLockerMenu)),
        new KButtonMenu.ButtonInfo((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.QUIT, Action.NumActions, new UnityAction(this.OnQuit)),
        new KButtonMenu.ButtonInfo((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, Action.NumActions, new UnityAction(this.OnDesktopQuit))
      };
    else
      this.buttons = (IList<KButtonMenu.ButtonInfo>) new KButtonMenu.ButtonInfo[4]
      {
        new KButtonMenu.ButtonInfo((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.RESUME, Action.NumActions, new UnityAction(this.OnResume)),
        new KButtonMenu.ButtonInfo((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.OPTIONS, Action.NumActions, new UnityAction(this.OnOptions)),
        new KButtonMenu.ButtonInfo((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.QUIT, Action.NumActions, new UnityAction(this.OnQuit)),
        new KButtonMenu.ButtonInfo((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, Action.NumActions, new UnityAction(this.OnDesktopQuit))
      };
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.clipboard.GetText = new Func<string>(this.GetClipboardText);
    this.title.SetText((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.TITLE);
    try
    {
      string settingsCoordinate = CustomGameSettings.Instance.GetSettingsCoordinate();
      string[] settingCoordinate = CustomGameSettings.ParseSettingCoordinate(settingsCoordinate);
      this.worldSeed.SetText(string.Format((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, (object) settingsCoordinate));
      this.worldSeed.GetComponent<ToolTip>().toolTip = string.Format((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED_TOOLTIP, (object) settingCoordinate[1], (object) settingCoordinate[2], (object) settingCoordinate[3], (object) settingCoordinate[4], (object) settingCoordinate[5]);
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) $"Failed to load Coordinates on ClusterLayout {ex}, please report this error on the forums");
      CustomGameSettings.Instance.Print();
      Debug.Log((object) ("ClusterCache: " + string.Join(",", (IEnumerable<string>) SettingsCache.clusterLayouts.clusterCache.Keys)));
      this.worldSeed.SetText(string.Format((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, (object) "0"));
    }
  }

  public override float GetSortKey() => 30f;

  private string GetClipboardText()
  {
    try
    {
      return CustomGameSettings.Instance.GetSettingsCoordinate();
    }
    catch
    {
      return "";
    }
  }

  private void OnResume() => this.Show(false);

  protected override void OnShow(bool show)
  {
    this.recentlySaved = false;
    this.ConfigureButtonInfos();
    base.OnShow(show);
    if (show)
    {
      this.RefreshButtons();
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().ESCPauseSnapshot);
      MusicManager.instance.OnEscapeMenu(true);
      MusicManager.instance.PlaySong("Music_ESC_Menu");
      this.RefreshDLCActivationButtons();
    }
    else
    {
      ToolTipScreen.Instance.ClearToolTip(this.closeButton.GetComponent<ToolTip>());
      AudioMixer.instance.Stop(AudioMixerSnapshots.Get().ESCPauseSnapshot);
      MusicManager.instance.OnEscapeMenu(false);
      if (!MusicManager.instance.SongIsPlaying("Music_ESC_Menu"))
        return;
      MusicManager.instance.StopSong("Music_ESC_Menu");
    }
  }

  private void OnOptions() => this.ActivateChildScreen(this.optionsScreen.gameObject);

  private void OnSaveAs() => this.ActivateChildScreen(this.saveScreenPrefab.gameObject);

  private void OnSave()
  {
    string filename = SaveLoader.GetActiveSaveFilePath();
    if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
    {
      this.gameObject.SetActive(false);
      ((ConfirmDialogScreen) GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.transform.parent.gameObject)).PopupConfirmDialog(string.Format((string) STRINGS.UI.FRONTEND.SAVESCREEN.OVERWRITEMESSAGE, (object) System.IO.Path.GetFileNameWithoutExtension(filename)), (System.Action) (() =>
      {
        this.DoSave(filename);
        this.gameObject.SetActive(true);
      }), new System.Action(this.OnCancelPopup));
    }
    else
      this.OnSaveAs();
  }

  public void OnSaveComplete()
  {
    this.recentlySaved = true;
    this.ConfigureButtonInfos();
    this.RefreshButtons();
  }

  private void DoSave(string filename)
  {
    try
    {
      SaveLoader.Instance.Save(filename);
      this.OnSaveComplete();
    }
    catch (IOException ex)
    {
      Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.transform.parent.gameObject, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format((string) STRINGS.UI.FRONTEND.SAVESCREEN.IO_ERROR, (object) ex.ToString()), (System.Action) (() => this.Deactivate()), (System.Action) null, (string) STRINGS.UI.FRONTEND.SAVESCREEN.REPORT_BUG, (System.Action) (() => KCrashReporter.ReportError(ex.Message, ex.StackTrace.ToString(), (ConfirmDialogScreen) null, (GameObject) null, (string) null, extraCategories: new string[1]
      {
        KCrashReporter.CRASH_CATEGORY.FILEIO
      })));
    }
  }

  private void ConfirmDecision(
    string questionText,
    string primaryButtonText,
    System.Action primaryButtonAction,
    string alternateButtonText = null,
    System.Action alternateButtonAction = null)
  {
    this.gameObject.SetActive(false);
    ((ConfirmDialogScreen) GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, this.transform.parent.gameObject)).PopupConfirmDialog(questionText, primaryButtonAction, new System.Action(this.OnCancelPopup), alternateButtonText, alternateButtonAction, confirm_text: primaryButtonText);
  }

  private void OnLoad() => this.ActivateChildScreen(this.loadScreenPrefab.gameObject);

  private void OnColonySummary()
  {
    MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
  }

  private void OnLockerMenu() => LockerMenuScreen.Instance.Show(true);

  private void OnQuit()
  {
    this.ConfirmDecision((string) STRINGS.UI.FRONTEND.MAINMENU.QUITCONFIRM, (string) STRINGS.UI.FRONTEND.MAINMENU.SAVEANDQUITTITLE, (System.Action) (() => this.OnQuitConfirm(true)), (string) STRINGS.UI.FRONTEND.MAINMENU.QUIT, (System.Action) (() => this.OnQuitConfirm(false)));
  }

  private void OnDesktopQuit()
  {
    this.ConfirmDecision((string) STRINGS.UI.FRONTEND.MAINMENU.DESKTOPQUITCONFIRM, (string) STRINGS.UI.FRONTEND.MAINMENU.SAVEANDQUITDESKTOP, (System.Action) (() => this.OnDesktopQuitConfirm(true)), (string) STRINGS.UI.FRONTEND.MAINMENU.QUIT, (System.Action) (() => this.OnDesktopQuitConfirm(false)));
  }

  private void OnCancelPopup() => this.gameObject.SetActive(true);

  private void OnLoadConfirm()
  {
    LoadingOverlay.Load((System.Action) (() =>
    {
      LoadScreen.ForceStopGame();
      this.Deactivate();
      App.LoadScene("frontend");
    }));
  }

  private void OnRetireConfirm() => RetireColonyUtility.SaveColonySummaryData();

  private void OnQuitConfirm(bool saveFirst)
  {
    if (saveFirst)
    {
      string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
      if (!string.IsNullOrEmpty(activeSaveFilePath) && File.Exists(activeSaveFilePath))
        this.DoSave(activeSaveFilePath);
      else
        this.OnSaveAs();
    }
    LoadingOverlay.Load((System.Action) (() =>
    {
      this.Deactivate();
      PauseScreen.TriggerQuitGame();
    }));
  }

  private void OnDesktopQuitConfirm(bool saveFirst)
  {
    if (saveFirst)
    {
      string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
      if (!string.IsNullOrEmpty(activeSaveFilePath) && File.Exists(activeSaveFilePath))
        this.DoSave(activeSaveFilePath);
      else
        this.OnSaveAs();
    }
    App.Quit();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.Show(false);
    else
      base.OnKeyDown(e);
  }

  public static void TriggerQuitGame()
  {
    ThreadedHttps<KleiMetrics>.Instance.EndGame();
    LoadScreen.ForceStopGame();
    App.LoadScene("frontend");
  }

  private void RefreshDLCActivationButtons()
  {
    foreach (KeyValuePair<string, DlcManager.DlcInfo> keyValuePair in DlcManager.DLC_PACKS)
    {
      if (!keyValuePair.Value.isCosmetic && !this.dlcActivationButtons.ContainsKey(keyValuePair.Key))
      {
        GameObject gameObject = Util.KInstantiateUI(this.dlcActivationButtonPrefab, this.dlcActivationButtonPrefab.transform.parent.gameObject, true);
        Sprite sprite = Assets.GetSprite((HashedString) DlcManager.GetDlcSmallLogo(keyValuePair.Key));
        gameObject.GetComponent<Image>().sprite = sprite;
        gameObject.GetComponent<MultiToggle>().states[0].sprite = sprite;
        gameObject.GetComponent<MultiToggle>().states[1].sprite = sprite;
        this.dlcActivationButtons.Add(keyValuePair.Key, gameObject);
      }
    }
    this.RefreshDLCButton("EXPANSION1_ID", this.dlc1ActivationButton, false);
    foreach (KeyValuePair<string, GameObject> activationButton in this.dlcActivationButtons)
      this.RefreshDLCButton(activationButton.Key, activationButton.Value.GetComponent<MultiToggle>(), true);
  }

  private void RefreshDLCButton(string DLCID, MultiToggle button, bool userEditable)
  {
    button.GetComponent<MultiToggle>().states[0].sprite = Assets.GetSprite((HashedString) DlcManager.GetDlcSmallLogo(DLCID));
    button.GetComponent<MultiToggle>().states[1].sprite = Assets.GetSprite((HashedString) DlcManager.GetDlcSmallLogo(DLCID));
    button.ChangeState(Game.IsDlcActiveForCurrentSave(DLCID) ? 1 : 0);
    button.GetComponent<Image>().material = Game.IsDlcActiveForCurrentSave(DLCID) ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated;
    ToolTip component = button.GetComponent<ToolTip>();
    string dlcTitle = DlcManager.GetDlcTitle(DLCID);
    if (DlcManager.IsContentSubscribed(DLCID))
    {
      if (userEditable)
      {
        component.SetSimpleTooltip(Game.IsDlcActiveForCurrentSave(DLCID) ? string.Format((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_ENABLED_TOOLTIP, (object) dlcTitle) : string.Format((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_TOOLTIP, (object) dlcTitle));
        button.onClick = (System.Action) (() => this.OnClickAddDLCButton(DLCID));
      }
      else
      {
        component.SetSimpleTooltip(Game.IsDlcActiveForCurrentSave(DLCID) ? string.Format((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_ENABLED_TOOLTIP, (object) dlcTitle) : string.Format((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_NOT_EDITABLE_TOOLTIP, (object) dlcTitle));
        button.onClick = (System.Action) null;
      }
    }
    else
    {
      component.SetSimpleTooltip(string.Format((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_NOT_EDITABLE_TOOLTIP, (object) dlcTitle));
      button.onClick = (System.Action) null;
    }
  }

  private void OnClickAddDLCButton(string dlcID)
  {
    if (Game.IsDlcActiveForCurrentSave(dlcID))
      return;
    this.ConfirmDecision((string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.ENABLE_QUESTION, (string) STRINGS.UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.CONFIRM, (System.Action) (() => this.OnConfirmAddDLC(dlcID)));
  }

  private void OnConfirmAddDLC(string dlcId)
  {
    SaveLoader.Instance.UpgradeActiveSaveDLCInfo(dlcId, true);
  }
}
