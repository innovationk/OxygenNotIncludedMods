// Decompiled with JetBrains decompiler
// Type: UnderwaterBreathingLocationWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

#nullable disable
public class UnderwaterBreathingLocationWorkable : Workable
{
  [MyCmpReq]
  private Storage storage;
  private OxygenBreather breather;
  private AmountInstance breath;

  protected override void OnPrefabInit()
  {
    this.workTime = 150f;
    this.workAnims = new HashedString[2]
    {
      (HashedString) "working_pre",
      (HashedString) "working_loop"
    };
    this.workingPstComplete = new HashedString[1]
    {
      (HashedString) "working_pst"
    };
    this.workingPstFailed = new HashedString[1]
    {
      (HashedString) "working_pst"
    };
    this.resetProgressOnStop = false;
    this.showProgressBar = false;
    this.faceTargetWhenWorking = true;
    this.workLayer = Grid.SceneLayer.BuildingUse;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_underwater_breathing_station_kanim")
    };
    base.OnPrefabInit();
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    this.SetWorkTime(150f);
    worker.GetComponent<KPrefabID>().AddTag(GameTags.RecoveringBreath);
    worker.Trigger(961737054, (object) null);
    this.breather = worker.GetComponent<OxygenBreather>();
    this.breath = Db.Get().Amounts.Breath.Lookup((Component) worker);
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    if ((Object) this.breather == (Object) null || this.breath == null)
      return true;
    float amount = (float) ((double) this.breather.ConsumptionRate * (double) dt * 50.0);
    float amount_consumed;
    SimUtil.DiseaseInfo disease_info;
    float aggregate_temperature;
    SimHashes mostRelevantItemElement;
    this.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out amount_consumed, out disease_info, out aggregate_temperature, out mostRelevantItemElement);
    if ((double) amount_consumed > 0.0)
    {
      OxygenBreather.BreathableGasConsumed(this.breather, mostRelevantItemElement, amount_consumed, aggregate_temperature, disease_info.idx, disease_info.count);
      double num = (double) this.breath.ApplyDelta(amount_consumed * DUPLICANTSTATS.STANDARD.BaseStats.RECOVER_BREATH_DELTA);
    }
    return (Object) this.storage.FindFirstWithMass(GameTags.Breathable) == (Object) null;
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    worker.GetComponent<KPrefabID>().RemoveTag(GameTags.RecoveringBreath);
    worker.Trigger(-2037519664, (object) null);
    base.OnStopWork(worker);
  }
}
