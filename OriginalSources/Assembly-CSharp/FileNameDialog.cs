// Decompiled with JetBrains decompiler
// Type: FileNameDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class FileNameDialog : KModalScreen
{
  public Action<string> onConfirm;
  public System.Action onCancel;
  [SerializeField]
  private KInputTextField inputField;
  [SerializeField]
  private KButton confirmButton;
  [SerializeField]
  private KButton cancelButton;
  [SerializeField]
  private KButton closeButton;

  public override float GetSortKey() => 150f;

  public void SetTextAndSelect(string text)
  {
    if ((UnityEngine.Object) this.inputField == (UnityEngine.Object) null)
      return;
    this.inputField.text = text;
    this.inputField.Select();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.confirmButton.onClick += new System.Action(this.OnConfirm);
    this.cancelButton.onClick += new System.Action(this.OnCancel);
    this.closeButton.onClick += new System.Action(this.OnCancel);
    this.inputField.onValueChanged.AddListener((UnityAction<string>) (_param1 => Util.ScrubInputField(this.inputField)));
    this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    this.inputField.Select();
    this.inputField.ActivateInputField();
    CameraController.Instance.DisableUserCameraControl = true;
  }

  protected override void OnDeactivate()
  {
    CameraController.Instance.DisableUserCameraControl = false;
    base.OnDeactivate();
  }

  public void OnConfirm()
  {
    if (this.onConfirm == null || string.IsNullOrEmpty(this.inputField.text))
      return;
    string text = this.inputField.text;
    if (!text.EndsWith(".sav"))
      text += ".sav";
    this.onConfirm(text);
    this.Deactivate();
  }

  private void OnEndEdit(string str)
  {
    if (!Localization.HasDirtyWords(str))
      return;
    this.inputField.text = "";
  }

  public void OnCancel()
  {
    if (this.onCancel != null)
      this.onCancel();
    this.Deactivate();
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape))
      this.Deactivate();
    else if (e.TryConsume(Action.DialogSubmit))
      this.OnConfirm();
    e.Consumed = true;
  }

  public override void OnKeyDown(KButtonEvent e) => e.Consumed = true;
}
