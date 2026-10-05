// Decompiled with JetBrains decompiler
// Type: MessageDialogFrame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class MessageDialogFrame : KScreen
{
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KToggle nextMessageButton;
  [SerializeField]
  private GameObject dontShowAgainElement;
  [SerializeField]
  private MultiToggle dontShowAgainButton;
  [SerializeField]
  private LocText title;
  [SerializeField]
  private RectTransform body;
  private System.Action dontShowAgainDelegate;

  public override float GetSortKey() => 15f;

  protected override void OnActivate()
  {
    this.closeButton.onClick += new System.Action(this.OnClickClose);
    this.nextMessageButton.onClick += new System.Action(this.OnClickNextMessage);
    this.dontShowAgainButton.onClick += new System.Action(this.OnClickDontShowAgain);
    this.dontShowAgainButton.ChangeState(KPlayerPrefs.GetInt("HideTutorial_CheckState", 0) == 1 ? 0 : 1);
    this.Subscribe(Messenger.Instance.gameObject, -599791736, new Action<object>(this.OnMessagesChanged));
    this.OnMessagesChanged((object) null);
  }

  protected override void OnDeactivate()
  {
    this.Unsubscribe(Messenger.Instance.gameObject, -599791736, new Action<object>(this.OnMessagesChanged));
  }

  private void OnClickClose()
  {
    this.TryDontShowAgain();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  private void OnClickNextMessage()
  {
    this.TryDontShowAgain();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    NotificationScreen.Instance.OnClickNextMessage();
  }

  private void OnClickDontShowAgain()
  {
    this.dontShowAgainButton.NextState();
    KPlayerPrefs.SetInt("HideTutorial_CheckState", this.dontShowAgainButton.CurrentState == 0 ? 1 : 0);
  }

  private void OnMessagesChanged(object data)
  {
    this.nextMessageButton.gameObject.SetActive(Messenger.Instance.Count != 0);
  }

  public void SetMessage(MessageDialog dialog, Message message)
  {
    this.title.text = message.GetTitle().ToUpper();
    dialog.GetComponent<RectTransform>().SetParent((Transform) this.body.GetComponent<RectTransform>());
    RectTransform component = dialog.GetComponent<RectTransform>();
    component.offsetMin = Vector2.zero;
    component.offsetMax = Vector2.zero;
    dialog.transform.SetLocalPosition(Vector3.zero);
    dialog.SetMessage(message);
    dialog.OnClickAction();
    if (dialog.CanDontShowAgain)
    {
      this.dontShowAgainElement.SetActive(true);
      this.dontShowAgainDelegate = new System.Action(dialog.OnDontShowAgain);
    }
    else
    {
      this.dontShowAgainElement.SetActive(false);
      this.dontShowAgainDelegate = (System.Action) null;
    }
  }

  private void TryDontShowAgain()
  {
    if (this.dontShowAgainDelegate == null || this.dontShowAgainButton.CurrentState != 0)
      return;
    this.dontShowAgainDelegate();
  }
}
