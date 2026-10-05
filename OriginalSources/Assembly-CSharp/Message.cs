// Decompiled with JetBrains decompiler
// Type: Message
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class Message : ISaveLoadable
{
  public abstract string GetTitle();

  public abstract string GetSound();

  public abstract string GetMessageBody();

  public abstract string GetTooltip();

  public virtual bool ShowDialog() => true;

  public virtual void OnCleanUp()
  {
  }

  public virtual bool IsValid() => true;

  public virtual bool PlayNotificationSound() => true;

  public virtual void OnClick()
  {
  }

  public virtual NotificationType GetMessageType() => NotificationType.Messages;

  public virtual bool ShowDismissButton() => true;
}
