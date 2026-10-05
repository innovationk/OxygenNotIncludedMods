// Decompiled with JetBrains decompiler
// Type: HotTubWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/HotTubWorkable")]
public class HotTubWorkable : Workable, IWorkerPrioritizable
{
  public HotTub hotTub;
  private bool faceLeft;

  private HotTubWorkable() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.synchronizeAnims = false;
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.faceTargetWhenWorking = true;
    this.SetWorkTime(90f);
  }

  public override Workable.AnimInfo GetAnim(WorkerBase worker)
  {
    return base.GetAnim(worker) with
    {
      smi = (StateMachine.Instance) new HotTubWorkerStateMachine.StatesInstance(worker)
    };
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    this.faceLeft = (double) Random.value > 0.5;
    worker.GetComponent<Effects>().Add("HotTubRelaxing", false);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    worker.GetComponent<Effects>().Remove("HotTubRelaxing");
  }

  public override Vector3 GetFacingTarget()
  {
    return this.transform.GetPosition() + (this.faceLeft ? Vector3.left : Vector3.right);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(this.hotTub.trackingEffect))
      component.Add(this.hotTub.trackingEffect, true);
    if (!string.IsNullOrEmpty(this.hotTub.specificEffect))
      component.Add(this.hotTub.specificEffect, true);
    component.Add("WarmTouch", true).timeRemaining = 1800f;
  }

  public bool GetWorkerPriority(WorkerBase worker, out int priority)
  {
    priority = this.hotTub.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(this.hotTub.trackingEffect) && component.HasEffect(this.hotTub.trackingEffect))
    {
      priority = 0;
      return false;
    }
    if (!string.IsNullOrEmpty(this.hotTub.specificEffect) && component.HasEffect(this.hotTub.specificEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }
}
