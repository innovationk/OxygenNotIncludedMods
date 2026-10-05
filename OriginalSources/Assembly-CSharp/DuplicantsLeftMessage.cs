// Decompiled with JetBrains decompiler
// Type: DuplicantsLeftMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
public class DuplicantsLeftMessage : Message
{
  public override string GetSound() => "";

  public override string GetTitle() => (string) MISC.NOTIFICATIONS.DUPLICANTABSORBED.NAME;

  public override string GetMessageBody()
  {
    return (string) MISC.NOTIFICATIONS.DUPLICANTABSORBED.MESSAGEBODY;
  }

  public override string GetTooltip() => (string) MISC.NOTIFICATIONS.DUPLICANTABSORBED.TOOLTIP;
}
