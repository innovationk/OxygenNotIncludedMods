// Decompiled with JetBrains decompiler
// Type: TutorialMessageDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

#nullable disable
public class TutorialMessageDialog : MessageDialog
{
  [SerializeField]
  private LocText description;
  private TutorialMessage message;
  [SerializeField]
  private GameObject videoWidgetPrefab;
  private VideoWidget videoWidget;

  public override bool CanDontShowAgain => true;

  public override bool CanDisplay(Message message)
  {
    return typeof (TutorialMessage).IsAssignableFrom(message.GetType());
  }

  public override void SetMessage(Message base_message)
  {
    this.message = base_message as TutorialMessage;
    this.description.text = this.message.GetMessageBody();
    if (string.IsNullOrEmpty(this.message.videoClipId))
      return;
    this.SetVideo(Assets.GetVideo(this.message.videoClipId), this.message.videoOverlayName, this.message.videoTitleText);
  }

  public void SetVideo(VideoClip clip, string overlayName, string titleText)
  {
    if ((Object) this.videoWidget == (Object) null)
    {
      this.videoWidget = Util.KInstantiateUI(this.videoWidgetPrefab, this.transform.gameObject, true).GetComponent<VideoWidget>();
      this.videoWidget.transform.SetAsFirstSibling();
    }
    this.videoWidget.SetClip(clip, overlayName, new List<string>()
    {
      titleText,
      (string) VIDEOS.TUTORIAL_HEADER
    });
  }

  public override void OnClickAction()
  {
  }

  public override void OnDontShowAgain()
  {
    Tutorial.Instance.HideTutorialMessage(this.message.messageId);
  }
}
