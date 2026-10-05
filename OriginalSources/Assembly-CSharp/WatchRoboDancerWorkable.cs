// Decompiled with JetBrains decompiler
// Type: WatchRoboDancerWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

#nullable disable
public class WatchRoboDancerWorkable : Workable, IWorkerPrioritizable
{
  public GameObject owner;
  public int basePriority = RELAXATION.PRIORITY.TIER3;
  public static string SPECIFIC_EFFECT = "SawRoboDancer";
  public static string TRACKING_EFFECT = "RecentlySawRoboDancer";
  public KAnimFile[][] workerOverrideAnims = new KAnimFile[2][]
  {
    new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_robotdance_kanim")
    },
    new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_robotdance1_kanim")
    }
  };

  private WatchRoboDancerWorkable() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.synchronizeAnims = false;
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.WatchRoboDancerWorkable;
    this.SetWorkTime(30f);
    this.showProgressBar = false;
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.TRACKING_EFFECT))
      component.Add(WatchRoboDancerWorkable.TRACKING_EFFECT, true);
    if (string.IsNullOrEmpty(WatchRoboDancerWorkable.SPECIFIC_EFFECT))
      return;
    component.Add(WatchRoboDancerWorkable.SPECIFIC_EFFECT, true);
  }

  public bool GetWorkerPriority(WorkerBase worker, out int priority)
  {
    priority = this.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.TRACKING_EFFECT) && component.HasEffect(WatchRoboDancerWorkable.TRACKING_EFFECT))
    {
      priority = 0;
      return false;
    }
    if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.SPECIFIC_EFFECT) && component.HasEffect(WatchRoboDancerWorkable.SPECIFIC_EFFECT))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    worker.GetComponent<Effects>().Add("Dancing", false);
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    worker.GetComponent<Facing>().Face(this.owner.transform.position.x);
    return base.OnWorkTick(worker, dt);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    worker.GetComponent<Effects>().Remove("Dancing");
    ChoreHelpers.DestroyLocator(this.gameObject);
  }

  public override Workable.AnimInfo GetAnim(WorkerBase worker)
  {
    this.overrideAnims = this.workerOverrideAnims[Random.Range(0, this.workerOverrideAnims.Length)];
    return base.GetAnim(worker);
  }
}
