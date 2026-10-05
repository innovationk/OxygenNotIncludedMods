// Decompiled with JetBrains decompiler
// Type: CodexUnlockedMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
public class CodexUnlockedMessage : Message
{
  private string unlockMessage;
  private string lockId;

  public CodexUnlockedMessage()
  {
  }

  public CodexUnlockedMessage(string lock_id, string unlock_message)
  {
    this.lockId = lock_id;
    this.unlockMessage = unlock_message;
  }

  public string GetLockId() => this.lockId;

  public override string GetSound() => "AI_Notification_ResearchComplete";

  public override string GetMessageBody()
  {
    return UI.CODEX.CODEX_DISCOVERED_MESSAGE.BODY.Replace("{codex}", this.unlockMessage);
  }

  public override string GetTitle() => (string) UI.CODEX.CODEX_DISCOVERED_MESSAGE.TITLE;

  public override string GetTooltip() => this.GetMessageBody();

  public override bool IsValid() => true;
}
