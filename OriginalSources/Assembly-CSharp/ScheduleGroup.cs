// Decompiled with JetBrains decompiler
// Type: ScheduleGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
[DebuggerDisplay("{Id}")]
public class ScheduleGroup : Resource
{
  public int defaultSegments { get; private set; }

  public string description { get; private set; }

  public string notificationTooltip { get; private set; }

  public List<ScheduleBlockType> allowedTypes { get; private set; }

  public bool alarm { get; private set; }

  public Color uiColor { get; private set; }

  public ScheduleGroup(
    string id,
    ResourceSet parent,
    int defaultSegments,
    string name,
    string description,
    Color uiColor,
    string notificationTooltip,
    List<ScheduleBlockType> allowedTypes,
    bool alarm = false)
    : base(id, parent, name)
  {
    this.defaultSegments = defaultSegments;
    this.description = description;
    this.notificationTooltip = notificationTooltip;
    this.allowedTypes = allowedTypes;
    this.alarm = alarm;
    this.uiColor = uiColor;
  }

  public bool Allowed(ScheduleBlockType type) => this.allowedTypes.Contains(type);

  public string GetTooltip()
  {
    return string.Format((string) UI.SCHEDULEGROUPS.TOOLTIP_FORMAT, (object) this.Name, (object) this.description);
  }
}
