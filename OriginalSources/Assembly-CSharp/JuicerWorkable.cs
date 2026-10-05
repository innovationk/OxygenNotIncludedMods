// Decompiled with JetBrains decompiler
// Type: JuicerWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/JuicerWorkable")]
public class JuicerWorkable : Workable, IWorkerPrioritizable
{
  public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();
  [MyCmpReq]
  private Operational operational;
  public int basePriority;
  private Juicer juicer;

  private JuicerWorkable() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  public override Workable.AnimInfo GetAnim(WorkerBase worker)
  {
    KAnimFile[] kanimFileArray = (KAnimFile[]) null;
    if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out kanimFileArray))
      this.overrideAnims = kanimFileArray;
    return base.GetAnim(worker);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_juicer_kanim")
    };
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.synchronizeAnims = false;
    this.SetWorkTime(30f);
    this.juicer = this.GetComponent<Juicer>();
  }

  protected override void OnStartWork(WorkerBase worker) => this.operational.SetActive(true);

  protected override void OnCompleteWork(WorkerBase worker)
  {
    Storage component1 = this.GetComponent<Storage>();
    float amount_consumed;
    SimUtil.DiseaseInfo disease_info1;
    float aggregate_temperature;
    component1.ConsumeAndGetDisease(GameTags.Water, this.juicer.waterMassPerUse, out amount_consumed, out disease_info1, out aggregate_temperature);
    GermExposureMonitor.Instance smi = worker.GetSMI<GermExposureMonitor.Instance>();
    for (int index = 0; index < this.juicer.ingredientTags.Length; ++index)
    {
      SimUtil.DiseaseInfo disease_info2;
      component1.ConsumeAndGetDisease(this.juicer.ingredientTags[index], this.juicer.ingredientMassesPerUse[index], out amount_consumed, out disease_info2, out aggregate_temperature);
      smi?.TryInjectDisease(disease_info2.idx, disease_info2.count, this.juicer.ingredientTags[index], Sickness.InfectionVector.Digestion);
    }
    smi?.TryInjectDisease(disease_info1.idx, disease_info1.count, GameTags.Water, Sickness.InfectionVector.Digestion);
    Effects component2 = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(this.juicer.specificEffect))
      component2.Add(this.juicer.specificEffect, true);
    if (string.IsNullOrEmpty(this.juicer.trackingEffect))
      return;
    component2.Add(this.juicer.trackingEffect, true);
  }

  protected override void OnStopWork(WorkerBase worker) => this.operational.SetActive(false);

  public bool GetWorkerPriority(WorkerBase worker, out int priority)
  {
    priority = this.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(this.juicer.trackingEffect) && component.HasEffect(this.juicer.trackingEffect))
    {
      priority = 0;
      return false;
    }
    if (!string.IsNullOrEmpty(this.juicer.specificEffect) && component.HasEffect(this.juicer.specificEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }
}
