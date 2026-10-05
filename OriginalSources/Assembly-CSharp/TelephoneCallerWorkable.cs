// Decompiled with JetBrains decompiler
// Type: TelephoneCallerWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/TelephoneWorkable")]
public class TelephoneCallerWorkable : Workable, IWorkerPrioritizable
{
  [MyCmpReq]
  private Operational operational;
  public int basePriority;
  private Telephone telephone;

  private TelephoneCallerWorkable()
  {
    this.SetReportType(ReportManager.ReportType.PersonalTime);
    this.workingPstComplete = new HashedString[1]
    {
      (HashedString) "on_pst"
    };
    this.workAnims = new HashedString[6]
    {
      (HashedString) "on_pre",
      (HashedString) "on",
      (HashedString) "on_receiving",
      (HashedString) "on_pre_loop_receiving",
      (HashedString) "on_loop",
      (HashedString) "on_loop_pre"
    };
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_telephone_kanim")
    };
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.synchronizeAnims = true;
    this.SetWorkTime(40f);
    this.telephone = this.GetComponent<Telephone>();
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    this.operational.SetActive(true);
    this.telephone.isInUse = true;
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    Effects component = worker.GetComponent<Effects>();
    if (this.telephone.HasTag(GameTags.LongDistanceCall))
    {
      if (!string.IsNullOrEmpty(this.telephone.longDistanceEffect))
        component.Add(this.telephone.longDistanceEffect, true);
    }
    else if (this.telephone.wasAnswered)
    {
      if (!string.IsNullOrEmpty(this.telephone.chatEffect))
        component.Add(this.telephone.chatEffect, true);
    }
    else if (!string.IsNullOrEmpty(this.telephone.babbleEffect))
      component.Add(this.telephone.babbleEffect, true);
    if (string.IsNullOrEmpty(this.telephone.trackingEffect))
      return;
    component.Add(this.telephone.trackingEffect, true);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    this.operational.SetActive(false);
    this.telephone.HangUp();
  }

  public bool GetWorkerPriority(WorkerBase worker, out int priority)
  {
    priority = this.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(this.telephone.trackingEffect) && component.HasEffect(this.telephone.trackingEffect))
    {
      priority = 0;
      return false;
    }
    if (!string.IsNullOrEmpty(this.telephone.chatEffect) && component.HasEffect(this.telephone.chatEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    if (!string.IsNullOrEmpty(this.telephone.babbleEffect) && component.HasEffect(this.telephone.babbleEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }
}
