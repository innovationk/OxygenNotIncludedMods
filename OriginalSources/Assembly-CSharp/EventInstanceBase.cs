// Decompiled with JetBrains decompiler
// Type: EventInstanceBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class EventInstanceBase : ISaveLoadable
{
  [Serialize]
  public int frame;
  [Serialize]
  public int eventHash;
  public EventBase ev;

  public EventInstanceBase(EventBase ev)
  {
    this.frame = GameClock.Instance.GetFrame();
    this.eventHash = ev.hash;
    this.ev = ev;
  }

  public override string ToString()
  {
    string str = $"[{this.frame.ToString()}] ";
    return this.ev != null ? str + this.ev.GetDescription(this) : str + "Unknown event";
  }
}
