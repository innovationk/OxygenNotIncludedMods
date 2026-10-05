// Decompiled with JetBrains decompiler
// Type: CellDigEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;

#nullable disable
public class CellDigEvent(bool enable_logging = true) : CellEvent("Dig", "Dig", true, enable_logging)
{
  [Conditional("ENABLE_CELL_EVENT_LOGGER")]
  public void Log(int cell, int callback_id)
  {
    if (!this.enableLogging)
      return;
    CellEventInstance ev = new CellEventInstance(cell, 0, 0, (CellEvent) this);
    CellEventLogger.Instance.Add(ev);
  }

  public override string GetDescription(EventInstanceBase ev)
  {
    return this.GetMessagePrefix() + "Dig=true";
  }
}
