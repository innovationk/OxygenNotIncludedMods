// Decompiled with JetBrains decompiler
// Type: ChoreGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
[DebuggerDisplay("{IdHash}")]
public class ChoreGroup : Resource
{
  public List<ChoreType> choreTypes = new List<ChoreType>();
  public Attribute attribute;
  public string description;
  public string sprite;
  private int defaultPersonalPriority;
  public bool userPrioritizable;

  public int DefaultPersonalPriority => this.defaultPersonalPriority;

  public ChoreGroup(
    string id,
    string name,
    Attribute attribute,
    string sprite,
    int default_personal_priority,
    bool user_prioritizable = true)
    : base(id, name)
  {
    this.attribute = attribute;
    this.description = Strings.Get($"STRINGS.DUPLICANTS.CHOREGROUPS.{id.ToUpper()}.DESC").String;
    this.sprite = sprite;
    this.defaultPersonalPriority = default_personal_priority;
    this.userPrioritizable = user_prioritizable;
  }
}
