// Decompiled with JetBrains decompiler
// Type: GunkEmptierWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GunkEmptierWorkable : Workable
{
  private const float BATHROOM_EFFECTS_DURATION_OVERRIDE = 1800f;
  private Storage storage;
  private GunkMonitor.Instance gunkMonitor;

  private GunkEmptierWorkable() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_gunkdump_kanim")
    };
    this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
    this.storage = this.GetComponent<Storage>();
    this.SetWorkTime(8.5f);
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    this.gunkMonitor.ExpellGunk(Mathf.Min(dt / this.workTime * GunkMonitor.GUNK_CAPACITY, this.gunkMonitor.CurrentGunkMass, this.storage.RemainingCapacity()), this.storage);
    return base.OnWorkTick(worker, dt);
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    this.gunkMonitor = worker.GetSMI<GunkMonitor.Instance>();
    if (Sim.IsRadiationEnabled() && (double) worker.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0.0)
      worker.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads);
    this.TriggerRoomEffects();
  }

  private void TriggerRoomEffects()
  {
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

  protected override void OnCompleteWork(WorkerBase worker)
  {
    if (this.gunkMonitor != null)
      this.gunkMonitor.ExpellAllGunk(this.storage);
    this.gunkMonitor = (GunkMonitor.Instance) null;
    base.OnCompleteWork(worker);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    this.RemoveExpellingRadStatusItem();
    base.OnStopWork(worker);
  }

  protected override void OnAbortWork(WorkerBase worker)
  {
    this.RemoveExpellingRadStatusItem();
    base.OnAbortWork(worker);
    this.gunkMonitor = (GunkMonitor.Instance) null;
  }

  private void RemoveExpellingRadStatusItem()
  {
    if (!Sim.IsRadiationEnabled())
      return;
    this.worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads);
  }
}
