// Decompiled with JetBrains decompiler
// Type: ArcadeMachineWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/ArcadeMachineWorkable")]
public class ArcadeMachineWorkable : Workable, IWorkerPrioritizable
{
  public ArcadeMachine owner;
  public int basePriority = RELAXATION.PRIORITY.TIER3;
  private static string specificEffect = "PlayedArcade";
  private static string trackingEffect = "RecentlyPlayedArcade";

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetReportType(ReportManager.ReportType.PersonalTime);
    this.synchronizeAnims = false;
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.SetWorkTime(15f);
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    worker.GetComponent<Effects>().Add("ArcadePlaying", false);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    base.OnStopWork(worker);
    worker.GetComponent<Effects>().Remove("ArcadePlaying");
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(ArcadeMachineWorkable.trackingEffect))
      component.Add(ArcadeMachineWorkable.trackingEffect, true);
    if (string.IsNullOrEmpty(ArcadeMachineWorkable.specificEffect))
      return;
    component.Add(ArcadeMachineWorkable.specificEffect, true);
  }

  public bool GetWorkerPriority(WorkerBase worker, out int priority)
  {
    priority = this.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(ArcadeMachineWorkable.trackingEffect) && component.HasEffect(ArcadeMachineWorkable.trackingEffect))
    {
      priority = 0;
      return false;
    }
    if (!string.IsNullOrEmpty(ArcadeMachineWorkable.specificEffect) && component.HasEffect(ArcadeMachineWorkable.specificEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }
}
