// Decompiled with JetBrains decompiler
// Type: CellEventInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class CellEventInstance : EventInstanceBase, ISaveLoadable
{
  [Serialize]
  public int cell;
  [Serialize]
  public int data;
  [Serialize]
  public int data2;

  public CellEventInstance(int cell, int data, int data2, CellEvent ev)
    : base((EventBase) ev)
  {
    this.cell = cell;
    this.data = data;
    this.data2 = data2;
  }
}
