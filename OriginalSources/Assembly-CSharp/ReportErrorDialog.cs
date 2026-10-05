// Decompiled with JetBrains decompiler
// Type: ReportErrorDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ReportErrorDialog : MonoBehaviour
{
  private System.Action submitAction;
  private System.Action quitAction;
  private System.Action continueAction;
  public KInputTextField messageInputField;
  [Header("Message")]
  public GameObject referenceMessage;
  public LocText messageText;
  [Header("Upload Progress")]
  public GameObject uploadInProgress;
  public Image progressBar;
  public LocText progressText;
  private string m_stackTrace;
  private bool m_crashSubmitted;
  [SerializeField]
  private KButton submitButton;
  [SerializeField]
  private KButton moreInfoButton;
  [SerializeField]
  private KButton quitButton;
  [SerializeField]
  private KButton continueGameButton;
  [SerializeField]
  private LocText CrashLabel;
  [SerializeField]
  private GameObject CrashDescription;
  [SerializeField]
  private GameObject ModsInfo;
  [SerializeField]
  private GameObject StackTrace;
  [SerializeField]
  private GameObject modEntryPrefab;
  [SerializeField]
  private Transform modEntryParent;
  private ReportErrorDialog.Mode mode;

  private void Start()
  {
    ThreadedHttps<KleiMetrics>.Instance.EndSession(true);
    if ((bool) (UnityEngine.Object) KScreenManager.Instance)
      KScreenManager.Instance.DisableInput(true);
    this.StackTrace.SetActive(false);
    this.CrashLabel.text = (string) (this.mode == ReportErrorDialog.Mode.SubmitError ? STRINGS.UI.CRASHSCREEN.TITLE : STRINGS.UI.CRASHSCREEN.TITLE_MODS);
    this.CrashDescription.SetActive(this.mode == ReportErrorDialog.Mode.SubmitError);
    this.ModsInfo.SetActive(this.mode == ReportErrorDialog.Mode.DisableMods);
    if (this.mode == ReportErrorDialog.Mode.DisableMods)
      this.BuildModsList();
    this.submitButton.gameObject.SetActive(this.submitAction != null);
    this.submitButton.onClick += new System.Action(this.OnSelect_SUBMIT);
    this.moreInfoButton.onClick += new System.Action(this.OnSelect_MOREINFO);
    this.continueGameButton.gameObject.SetActive(this.continueAction != null);
    this.continueGameButton.onClick += new System.Action(this.OnSelect_CONTINUE);
    this.quitButton.onClick += new System.Action(this.OnSelect_QUIT);
    this.messageInputField.text = (string) STRINGS.UI.CRASHSCREEN.BODY;
    KCrashReporter.onCrashReported += new Action<bool>(this.OpenRefMessage);
    KCrashReporter.onCrashUploadProgress += new Action<float>(this.UpdateProgressBar);
  }

  private void BuildModsList()
  {
    DebugUtil.Assert((UnityEngine.Object) Global.Instance != (UnityEngine.Object) null && Global.Instance.modManager != null);
    KMod.Manager mod_mgr = Global.Instance.modManager;
    List<KMod.Mod> allCrashableMods = mod_mgr.GetAllCrashableMods();
    allCrashableMods.Sort((Comparison<KMod.Mod>) ((x, y) => y.foundInStackTrace.CompareTo(x.foundInStackTrace)));
    foreach (KMod.Mod mod in allCrashableMods)
    {
      if (mod.foundInStackTrace && mod.label.distribution_platform != KMod.Label.DistributionPlatform.Dev)
        mod_mgr.EnableMod(mod.label, false, (object) this);
      HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.modEntryPrefab, this.modEntryParent.gameObject);
      LocText reference = hierarchyReferences.GetReference<LocText>("Title");
      reference.text = mod.title;
      reference.color = mod.foundInStackTrace ? Color.red : Color.white;
      MultiToggle toggle = hierarchyReferences.GetReference<MultiToggle>("EnabledToggle");
      toggle.ChangeState(mod.IsEnabledForActiveDlc() ? 1 : 0);
      KMod.Label mod_label = mod.label;
      toggle.onClick += (System.Action) (() =>
      {
        bool enabled = !mod_mgr.IsModEnabled(mod_label);
        toggle.ChangeState(enabled ? 1 : 0);
        mod_mgr.EnableMod(mod_label, enabled, (object) this);
      });
      toggle.GetComponent<ToolTip>().OnToolTip = (Func<string>) (() => (string) (mod_mgr.IsModEnabled(mod_label) ? STRINGS.UI.FRONTEND.MODS.TOOLTIPS.ENABLED : STRINGS.UI.FRONTEND.MODS.TOOLTIPS.DISABLED));
      hierarchyReferences.gameObject.SetActive(true);
    }
  }

  private void Update() => Debug.developerConsoleVisible = false;

  private void OnDestroy()
  {
    if (KCrashReporter.terminateOnError)
      App.QuitCode(1);
    if ((bool) (UnityEngine.Object) KScreenManager.Instance)
      KScreenManager.Instance.DisableInput(false);
    KCrashReporter.onCrashReported -= new Action<bool>(this.OpenRefMessage);
    KCrashReporter.onCrashUploadProgress -= new Action<float>(this.UpdateProgressBar);
  }

  public void OnKeyDown(KButtonEvent e)
  {
    if (!e.TryConsume(Action.Escape))
      return;
    this.OnSelect_QUIT();
  }

  public void PopupSubmitErrorDialog(
    string stackTrace,
    System.Action onSubmit,
    System.Action onQuit,
    System.Action onContinue)
  {
    this.mode = ReportErrorDialog.Mode.SubmitError;
    this.m_stackTrace = stackTrace;
    this.submitAction = onSubmit;
    this.quitAction = onQuit;
    this.continueAction = onContinue;
  }

  public void PopupDisableModsDialog(string stackTrace, System.Action onQuit, System.Action onContinue)
  {
    this.mode = ReportErrorDialog.Mode.DisableMods;
    this.m_stackTrace = stackTrace;
    this.quitAction = onQuit;
    this.continueAction = onContinue;
  }

  public void OnSelect_MOREINFO()
  {
    this.StackTrace.GetComponentInChildren<LocText>().text = this.m_stackTrace;
    this.StackTrace.SetActive(true);
    this.moreInfoButton.GetComponentInChildren<LocText>().text = (string) STRINGS.UI.CRASHSCREEN.COPYTOCLIPBOARDBUTTON;
    this.moreInfoButton.ClearOnClick();
    this.moreInfoButton.onClick += new System.Action(this.OnSelect_COPYTOCLIPBOARD);
  }

  public void OnSelect_COPYTOCLIPBOARD()
  {
    TextEditor textEditor = new TextEditor();
    textEditor.text = $"{this.m_stackTrace}\nBuild: {BuildWatermark.GetBuildText()}";
    textEditor.SelectAll();
    textEditor.Copy();
  }

  public void OnSelect_SUBMIT()
  {
    this.submitButton.GetComponentInChildren<LocText>().text = (string) STRINGS.UI.CRASHSCREEN.REPORTING;
    this.submitButton.GetComponent<KButton>().isInteractable = false;
    this.Submit();
  }

  public void OnSelect_QUIT()
  {
    if (this.quitAction == null)
      return;
    this.quitAction();
  }

  public void OnSelect_CONTINUE()
  {
    if (this.continueAction == null)
      return;
    this.continueAction();
  }

  public void OpenRefMessage(bool success)
  {
    this.submitButton.gameObject.SetActive(false);
    this.uploadInProgress.SetActive(false);
    this.referenceMessage.SetActive(true);
    this.messageText.text = (string) (success ? STRINGS.UI.CRASHSCREEN.THANKYOU : STRINGS.UI.CRASHSCREEN.UPLOAD_FAILED);
    this.m_crashSubmitted = success;
  }

  public void OpenUploadingMessagee()
  {
    this.submitButton.gameObject.SetActive(false);
    this.uploadInProgress.SetActive(true);
    this.referenceMessage.SetActive(false);
    this.progressBar.fillAmount = 0.0f;
    this.progressText.text = STRINGS.UI.CRASHSCREEN.UPLOADINPROGRESS.Replace("{0}", GameUtil.GetFormattedPercent(0.0f));
  }

  public void OnSelect_MESSAGE()
  {
    if (this.m_crashSubmitted)
      return;
    Application.OpenURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
  }

  public string UserMessage() => this.messageInputField.text;

  private void Submit()
  {
    this.submitAction();
    this.OpenUploadingMessagee();
  }

  public void UpdateProgressBar(float progress)
  {
    this.progressBar.fillAmount = progress;
    this.progressText.text = STRINGS.UI.CRASHSCREEN.UPLOADINPROGRESS.Replace("{0}", GameUtil.GetFormattedPercent(progress * 100f));
  }

  private enum Mode
  {
    SubmitError,
    DisableMods,
  }
}
