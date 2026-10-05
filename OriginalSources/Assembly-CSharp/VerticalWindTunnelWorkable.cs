// Decompiled with JetBrains decompiler
// Type: VerticalWindTunnelWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/VerticalWindTunnelWorkable")]
public class VerticalWindTunnelWorkable : Workable, IWorkerPrioritizable
{
  public VerticalWindTunnel windTunnel;
  public HashedString overrideAnim;
  public string[] preAnims;
  public string loopAnim;
  public string[] pstAnims;

  private VerticalWindTunnelWorkable() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  public override Workable.AnimInfo GetAnim(WorkerBase worker)
  {
    return base.GetAnim(worker) with
    {
      smi = (StateMachine.Instance) new WindTunnelWorkerStateMachine.StatesInstance(worker, this)
    };
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.synchronizeAnims = false;
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.SetWorkTime(90f);
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    worker.GetComponent<Effects>().Add("VerticalWindTunnelFlying", false);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    base.OnStopWork(worker);
    worker.GetComponent<Effects>().Remove("VerticalWindTunnelFlying");
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    Effects component = worker.GetComponent<Effects>();
    component.Add(this.windTunnel.trackingEffect, true);
    component.Add(this.windTunnel.specificEffect, true);
  }

  public bool GetWorkerPriority(WorkerBase worker, out int priority)
  {
    priority = this.windTunnel.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (component.HasEffect(this.windTunnel.trackingEffect))
    {
      priority = 0;
      return false;
    }
    if (component.HasEffect(this.windTunnel.specificEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }
}
