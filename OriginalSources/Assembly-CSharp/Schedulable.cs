// Decompiled with JetBrains decompiler
// Type: Schedulable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Schedulable")]
public class Schedulable : KMonoBehaviour
{
  public Schedule GetSchedule() => ScheduleManager.Instance.GetSchedule(this);

  public bool IsAllowed(ScheduleBlockType schedule_block_type)
  {
    WorldContainer myWorld = this.gameObject.GetMyWorld();
    if ((Object) myWorld == (Object) null)
    {
      DebugUtil.LogWarningArgs((object) $"Trying to schedule {schedule_block_type.Id} but {this.gameObject.name} is not on a valid world. Grid cell: {Grid.PosToCell((KMonoBehaviour) this.gameObject.GetComponent<KPrefabID>())}");
      return false;
    }
    return myWorld.AlertManager.IsRedAlert() || ScheduleManager.Instance.IsAllowed(this, schedule_block_type);
  }

  public void OnScheduleChanged(Schedule schedule) => this.Trigger(467134493, (object) schedule);

  public void OnScheduleBlocksTick(Schedule schedule)
  {
    this.Trigger(1714332666, (object) schedule);
  }

  public void OnScheduleBlocksChanged(Schedule schedule)
  {
    this.Trigger(-894023145, (object) schedule);
  }
}
