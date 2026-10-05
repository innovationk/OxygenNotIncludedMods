// Decompiled with JetBrains decompiler
// Type: ScheduleBlockType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;
using UnityEngine;

#nullable disable
[DebuggerDisplay("{Id}")]
public class ScheduleBlockType : Resource
{
  public Color color { get; private set; }

  public string description { get; private set; }

  public ScheduleBlockType(
    string id,
    ResourceSet parent,
    string name,
    string description,
    Color color)
    : base(id, parent, name)
  {
    this.color = color;
    this.description = description;
  }
}
