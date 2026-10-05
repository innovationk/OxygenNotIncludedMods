// Decompiled with JetBrains decompiler
// Type: RoleSlotUnlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class RoleSlotUnlock
{
  public string id { get; protected set; }

  public string name { get; protected set; }

  public string description { get; protected set; }

  public List<Tuple<string, int>> slots { get; protected set; }

  public Func<bool> isSatisfied { get; protected set; }

  public RoleSlotUnlock(
    string id,
    string name,
    string description,
    List<Tuple<string, int>> slots,
    Func<bool> isSatisfied)
  {
    this.id = id;
    this.name = name;
    this.description = description;
    this.slots = slots;
    this.isSatisfied = isSatisfied;
  }
}
