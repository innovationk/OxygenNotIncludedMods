// Decompiled with JetBrains decompiler
// Type: CellEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class CellEvent : EventBase
{
  public string reason;
  public bool isSend;
  public bool enableLogging;

  public CellEvent(string id, string reason, bool is_send, bool enable_logging = true)
    : base(id)
  {
    this.reason = reason;
    this.isSend = is_send;
    this.enableLogging = enable_logging;
  }

  public string GetMessagePrefix() => this.isSend ? ">>>: " : "<<<: ";
}
