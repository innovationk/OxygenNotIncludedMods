// Decompiled with JetBrains decompiler
// Type: CellSolidEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;

#nullable disable
public class CellSolidEvent(string id, string reason, bool is_send, bool enable_logging = true) : 
  CellEvent(id, reason, is_send, enable_logging)
{
  [Conditional("ENABLE_CELL_EVENT_LOGGER")]
  public void Log(int cell, bool solid)
  {
    if (!this.enableLogging)
      return;
    CellEventInstance ev = new CellEventInstance(cell, solid ? 1 : 0, 0, (CellEvent) this);
    CellEventLogger.Instance.Add(ev);
  }

  public override string GetDescription(EventInstanceBase ev)
  {
    return (ev as CellEventInstance).data == 1 ? $"{this.GetMessagePrefix()}Solid=true ({this.reason})" : $"{this.GetMessagePrefix()}Solid=false ({this.reason})";
  }
}
