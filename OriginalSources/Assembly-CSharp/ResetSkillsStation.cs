// Decompiled with JetBrains decompiler
// Type: ResetSkillsStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/ResetSkillsStation")]
public class ResetSkillsStation : Workable
{
  [MyCmpReq]
  public Assignable assignable;
  private Notification notification;
  private Chore chore;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.lightEfficiencyBonus = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnAssign(this.assignable.assignee);
    this.assignable.OnAssign += new Action<IAssignableIdentity>(this.OnAssign);
  }

  private void OnAssign(IAssignableIdentity obj)
  {
    if (obj != null)
    {
      this.CreateChore();
    }
    else
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("Unassigned");
      this.chore = (Chore) null;
    }
  }

  private void CreateChore()
  {
    this.chore = (Chore) new WorkChore<ResetSkillsStation>(Db.Get().ChoreTypes.UnlearnSkill, (IStateMachineTarget) this, allow_in_red_alert: false, ignore_schedule_block: true, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.high);
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    this.GetComponent<Operational>().SetActive(true);
    this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorTraining, (object) this);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    base.OnCompleteWork(worker);
    this.assignable.Unassign();
    MinionResume component = worker.GetComponent<MinionResume>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.ResetSkillLevels();
    component.SetHats(component.CurrentHat, (string) null);
    component.ApplyTargetHat();
    this.notification = new Notification((string) MISC.NOTIFICATIONS.RESETSKILL.NAME, NotificationType.Good, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.RESETSKILL.TOOLTIP + notificationList.ReduceMessages(false)));
    worker.GetComponent<Notifier>().Add(this.notification);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    base.OnStopWork(worker);
    this.GetComponent<Operational>().SetActive(false);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorTraining, (bool) (UnityEngine.Object) this);
    this.chore = (Chore) null;
  }
}
