// Decompiled with JetBrains decompiler
// Type: DefragmentationZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;

#nullable disable
public class DefragmentationZone : Workable
{
  private const float BEDROOM_EFFECTS_DURATION_OVERRIDE = 1800f;
  [MyCmpGet]
  public Assignable assignable;
  public IApproachable approachable;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetReportType(ReportManager.ReportType.PersonalTime);
    this.showProgressBar = false;
    this.workerStatusItem = (StatusItem) null;
    this.synchronizeAnims = false;
    this.triggerWorkReactions = false;
    this.lightEfficiencyBonus = false;
    this.approachable = this.GetComponent<IApproachable>();
    this.workAnims = new HashedString[2]
    {
      (HashedString) "microchip_bed_pre",
      (HashedString) "microchip_bed_loop"
    };
    this.workingPstComplete = new HashedString[1]
    {
      (HashedString) "microchip_bed_pst"
    };
    this.workingPstFailed = new HashedString[1]
    {
      (HashedString) "microchip_bed_pst"
    };
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetWorkTime(float.PositiveInfinity);
    this.OnWorkableEventCB = this.OnWorkableEventCB + new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent);
  }

  private void OnWorkableEvent(Workable workable, Workable.WorkableEvent workable_event)
  {
    if (workable_event != Workable.WorkableEvent.WorkStarted)
      return;
    this.AddRoomEffects();
  }

  private void AddRoomEffects()
  {
    if ((UnityEngine.Object) this.worker == (UnityEngine.Object) null)
      return;
    Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(this.gameObject);
    if (roomOfGameObject == null)
      return;
    RoomType roomType = roomOfGameObject.roomType;
    List<EffectInstance> effectInstanceList = (List<EffectInstance>) null;
    KPrefabID component1 = this.GetComponent<KPrefabID>();
    Effects component2 = this.worker.GetComponent<Effects>();
    ref List<EffectInstance> local = ref effectInstanceList;
    roomType.TriggerRoomEffects(component1, component2, out local);
    if (effectInstanceList == null)
      return;
    foreach (EffectInstance effectInstance in effectInstanceList)
      effectInstance.timeRemaining = 1800f;
  }

  public override bool InstantlyFinish(WorkerBase worker) => false;
}
