// Decompiled with JetBrains decompiler
// Type: GameOptionsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class GameOptionsScreen : KModalButtonMenu
{
  [SerializeField]
  private SaveConfigurationScreen saveConfiguration;
  [SerializeField]
  private UnitConfigurationScreen unitConfiguration;
  [SerializeField]
  private KButton resetTutorialButton;
  [SerializeField]
  private KButton controlsButton;
  [SerializeField]
  private KButton sandboxButton;
  [SerializeField]
  private ConfirmDialogScreen confirmPrefab;
  [SerializeField]
  private KButton doneButton;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private GameObject cloudSavesPanel;
  [SerializeField]
  private GameObject defaultToCloudSaveToggle;
  [SerializeField]
  private GameObject savePanel;
  [SerializeField]
  private InputBindingsScreen inputBindingsScreenPrefab;
  [SerializeField]
  private KSlider cameraSpeedSlider;
  [SerializeField]
  private LocText cameraSpeedSliderLabel;
  private const int cameraSliderNotchScale = 10;
  public const string PREFS_KEY_CAMERA_SPEED = "CameraSpeed";

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.unitConfiguration.Init();
    if ((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null)
    {
      this.saveConfiguration.ToggleDisabledContent(true);
      this.saveConfiguration.Init();
      this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
    }
    else
      this.saveConfiguration.ToggleDisabledContent(false);
    this.resetTutorialButton.onClick += new System.Action(this.OnTutorialReset);
    if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
      this.controlsButton.gameObject.SetActive(false);
    else
      this.controlsButton.onClick += new System.Action(this.OnKeyBindings);
    this.sandboxButton.onClick += new System.Action(this.OnUnlockSandboxMode);
    this.doneButton.onClick += new System.Action(((KScreen) this).Deactivate);
    this.closeButton.onClick += new System.Action(((KScreen) this).Deactivate);
    if ((UnityEngine.Object) this.defaultToCloudSaveToggle != (UnityEngine.Object) null)
    {
      this.RefreshCloudSaveToggle();
      this.defaultToCloudSaveToggle.GetComponentInChildren<KButton>().onClick += new System.Action(this.OnDefaultToCloudSaveToggle);
    }
    if ((UnityEngine.Object) this.cloudSavesPanel != (UnityEngine.Object) null)
      this.cloudSavesPanel.SetActive(SaveLoader.GetCloudSavesAvailable());
    this.cameraSpeedSlider.minValue = 1f;
    this.cameraSpeedSlider.maxValue = 20f;
    this.cameraSpeedSlider.onValueChanged.AddListener((UnityAction<float>) (val => this.OnCameraSpeedValueChanged(Mathf.FloorToInt(val))));
    this.cameraSpeedSlider.value = this.CameraSpeedToSlider(KPlayerPrefs.GetFloat("CameraSpeed"));
    this.RefreshCameraSliderLabel();
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if ((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null)
    {
      this.savePanel.SetActive(true);
      this.saveConfiguration.Show(show);
      this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
    }
    else
      this.savePanel.SetActive(false);
    if (KPlayerPrefs.HasKey("CameraSpeed"))
      return;
    CameraController.SetDefaultCameraSpeed();
  }

  private float CameraSpeedToSlider(float prefsValue) => prefsValue * 10f;

  private void OnCameraSpeedValueChanged(int sliderValue)
  {
    KPlayerPrefs.SetFloat("CameraSpeed", (float) sliderValue / 10f);
    this.RefreshCameraSliderLabel();
    if (!((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null))
      return;
    Game.Instance.Trigger(75424175, (object) null);
  }

  private void RefreshCameraSliderLabel()
  {
    this.cameraSpeedSliderLabel.text = string.Format((string) UI.FRONTEND.GAME_OPTIONS_SCREEN.CAMERA_SPEED_LABEL, (object) ((float) ((double) KPlayerPrefs.GetFloat("CameraSpeed") * 10.0 * 10.0)).ToString());
  }

  private void OnDefaultToCloudSaveToggle()
  {
    SaveLoader.SetCloudSavesDefault(!SaveLoader.GetCloudSavesDefault());
    this.RefreshCloudSaveToggle();
  }

  private void RefreshCloudSaveToggle()
  {
    bool cloudSavesDefault = SaveLoader.GetCloudSavesDefault();
    this.defaultToCloudSaveToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(cloudSavesDefault);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.Deactivate();
    else
      base.OnKeyDown(e);
  }

  private void OnTutorialReset()
  {
    ConfirmDialogScreen component = this.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
    component.PopupConfirmDialog((string) UI.FRONTEND.OPTIONS_SCREEN.RESET_TUTORIAL_WARNING, (System.Action) (() => Tutorial.ResetHiddenTutorialMessages()), (System.Action) (() => { }));
    component.Activate();
  }

  private void OnUnlockSandboxMode()
  {
    ConfirmDialogScreen component = this.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
    string unlockSandboxWarning = (string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.UNLOCK_SANDBOX_WARNING;
    System.Action on_confirm = (System.Action) (() =>
    {
      SaveGame.Instance.sandboxEnabled = true;
      this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
      TopLeftControlScreen.Instance.UpdateSandboxToggleState();
      this.Deactivate();
    });
    System.Action on_cancel = (System.Action) (() =>
    {
      SaveLoader.Instance.Save(System.IO.Path.Combine(SaveLoader.GetSavePrefixAndCreateFolder(), $"{SaveGame.Instance.BaseName}{(string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.BACKUP_SAVE_GAME_APPEND}.sav"), updateSavePointer: false);
      this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
      TopLeftControlScreen.Instance.UpdateSandboxToggleState();
      this.Deactivate();
    });
    string confirm = (string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM;
    string confirmSaveBackup = (string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM_SAVE_BACKUP;
    string cancel = (string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CANCEL;
    string confirm_text = confirm;
    string cancel_text = confirmSaveBackup;
    component.PopupConfirmDialog(unlockSandboxWarning, on_confirm, on_cancel, cancel, (System.Action) (() => { }), confirm_text: confirm_text, cancel_text: cancel_text);
    component.Activate();
  }

  private void OnKeyBindings()
  {
    this.ActivateChildScreen(this.inputBindingsScreenPrefab.gameObject);
  }

  private void SetSandboxModeActive(bool active)
  {
    this.sandboxButton.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(active);
    this.sandboxButton.isInteractable = !active;
    this.sandboxButton.gameObject.GetComponentInParent<CanvasGroup>().alpha = active ? 0.5f : 1f;
  }
}
