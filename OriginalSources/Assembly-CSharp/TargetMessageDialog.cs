// Decompiled with JetBrains decompiler
// Type: TargetMessageDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TargetMessageDialog : MessageDialog
{
  [SerializeField]
  private LocText description;
  private TargetMessage message;

  public override bool CanDisplay(Message message)
  {
    return typeof (TargetMessage).IsAssignableFrom(message.GetType());
  }

  public override void SetMessage(Message base_message)
  {
    this.message = (TargetMessage) base_message;
    this.description.text = this.message.GetMessageBody();
  }

  public override void OnClickAction()
  {
    MessageTarget target = this.message.GetTarget();
    SelectTool.Instance.SelectAndFocus(target.GetPosition(), target.GetSelectable());
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.message.OnCleanUp();
  }
}
