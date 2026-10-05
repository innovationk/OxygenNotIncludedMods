// Decompiled with JetBrains decompiler
// Type: Expectation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class Expectation
{
  public string id { get; protected set; }

  public string name { get; protected set; }

  public string description { get; protected set; }

  public Action<MinionResume> OnApply { get; protected set; }

  public Action<MinionResume> OnRemove { get; protected set; }

  public Expectation(
    string id,
    string name,
    string description,
    Action<MinionResume> OnApply,
    Action<MinionResume> OnRemove)
  {
    this.id = id;
    this.name = name;
    this.description = description;
    this.OnApply = OnApply;
    this.OnRemove = OnRemove;
  }
}
