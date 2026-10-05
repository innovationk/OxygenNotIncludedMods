// Decompiled with JetBrains decompiler
// Type: Database.ScheduleGroups
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Database;

public class ScheduleGroups : ResourceSet<ScheduleGroup>
{
  public List<ScheduleGroup> allGroups;
  public ScheduleGroup Hygene;
  public ScheduleGroup Worktime;
  public ScheduleGroup Recreation;
  public ScheduleGroup Sleep;

  public ScheduleGroup Add(
    string id,
    int defaultSegments,
    string name,
    string description,
    Color uiColor,
    string notificationTooltip,
    List<ScheduleBlockType> allowedTypes,
    bool alarm = false)
  {
    ScheduleGroup scheduleGroup = new ScheduleGroup(id, (ResourceSet) this, defaultSegments, name, description, uiColor, notificationTooltip, allowedTypes, alarm);
    this.allGroups.Add(scheduleGroup);
    return scheduleGroup;
  }

  public ScheduleGroups(ResourceSet parent)
    : base(nameof (ScheduleGroups), parent)
  {
    this.allGroups = new List<ScheduleGroup>();
    this.Hygene = this.Add(nameof (Hygene), 1, (string) UI.SCHEDULEGROUPS.HYGENE.NAME, (string) UI.SCHEDULEGROUPS.HYGENE.DESCRIPTION, Util.ColorFromHex("5A8DAF"), (string) UI.SCHEDULEGROUPS.HYGENE.NOTIFICATION_TOOLTIP, new List<ScheduleBlockType>()
    {
      Db.Get().ScheduleBlockTypes.Hygiene,
      Db.Get().ScheduleBlockTypes.Work
    });
    this.Worktime = this.Add(nameof (Worktime), 18, (string) UI.SCHEDULEGROUPS.WORKTIME.NAME, (string) UI.SCHEDULEGROUPS.WORKTIME.DESCRIPTION, Util.ColorFromHex("FFA649"), (string) UI.SCHEDULEGROUPS.WORKTIME.NOTIFICATION_TOOLTIP, new List<ScheduleBlockType>()
    {
      Db.Get().ScheduleBlockTypes.Work
    }, true);
    this.Recreation = this.Add(nameof (Recreation), 2, (string) UI.SCHEDULEGROUPS.RECREATION.NAME, (string) UI.SCHEDULEGROUPS.RECREATION.DESCRIPTION, Util.ColorFromHex("70DFAD"), (string) UI.SCHEDULEGROUPS.RECREATION.NOTIFICATION_TOOLTIP, new List<ScheduleBlockType>()
    {
      Db.Get().ScheduleBlockTypes.Hygiene,
      Db.Get().ScheduleBlockTypes.Eat,
      Db.Get().ScheduleBlockTypes.Recreation,
      Db.Get().ScheduleBlockTypes.Work
    });
    this.Sleep = this.Add(nameof (Sleep), 3, (string) UI.SCHEDULEGROUPS.SLEEP.NAME, (string) UI.SCHEDULEGROUPS.SLEEP.DESCRIPTION, Util.ColorFromHex("273469"), (string) UI.SCHEDULEGROUPS.SLEEP.NOTIFICATION_TOOLTIP, new List<ScheduleBlockType>()
    {
      Db.Get().ScheduleBlockTypes.Sleep
    });
    int num = 0;
    foreach (ScheduleGroup allGroup in this.allGroups)
      num += allGroup.defaultSegments;
    Debug.Assert(num == 24, (object) "Default schedule groups must add up to exactly 1 cycle!");
  }

  public ScheduleGroup FindGroupForScheduleTypes(List<ScheduleBlockType> types)
  {
    foreach (ScheduleGroup allGroup in this.allGroups)
    {
      if (Schedule.AreScheduleTypesIdentical(allGroup.allowedTypes, types))
        return allGroup;
    }
    return (ScheduleGroup) null;
  }
}
