// Decompiled with JetBrains decompiler
// Type: ScheduleGroupInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class ScheduleGroupInstance
{
  [Serialize]
  private string scheduleGroupID;
  [Serialize]
  public int segments;

  public ScheduleGroup scheduleGroup
  {
    get => Db.Get().ScheduleGroups.Get(this.scheduleGroupID);
    set => this.scheduleGroupID = value.Id;
  }

  public ScheduleGroupInstance(ScheduleGroup scheduleGroup)
  {
    this.scheduleGroup = scheduleGroup;
    this.segments = scheduleGroup.defaultSegments;
  }
}
