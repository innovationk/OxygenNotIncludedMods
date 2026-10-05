// Decompiled with JetBrains decompiler
// Type: OilChangerWorkableUse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using UnityEngine;

#nullable disable
public class OilChangerWorkableUse : Workable
{
  private Operational operational;

  private OilChangerWorkableUse() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.operational = this.GetComponent<Operational>();
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
    this.SetWorkTime(8.5f);
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    if ((Object) worker != (Object) null)
    {
      Vector3 position = worker.transform.GetPosition() with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse)
      };
      worker.transform.SetPosition(position);
    }
    Game.Instance.roomProber.GetRoomOfGameObject(this.gameObject)?.roomType.TriggerRoomEffects(this.GetComponent<KPrefabID>(), worker.GetComponent<Effects>());
    this.operational.SetActive(true);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    if ((Object) worker != (Object) null)
    {
      Vector3 position = worker.transform.GetPosition() with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.Move)
      };
      worker.transform.SetPosition(position);
    }
    this.operational.SetActive(false);
    base.OnStopWork(worker);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    Storage component1 = this.GetComponent<Storage>();
    BionicOilMonitor.Instance smi = worker.GetSMI<BionicOilMonitor.Instance>();
    if (smi != null)
    {
      float b = 200f - smi.CurrentOilMass;
      float amount1 = Mathf.Min(component1.GetMassAvailable(GameTags.LubricatingOil), b);
      float amount2 = amount1;
      float num = 0.0f;
      Storage component2 = this.GetComponent<Storage>();
      SimHashes lubricant = SimHashes.CrudeOil;
      foreach (SimHashes key in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Keys)
      {
        float amount_consumed;
        component2.ConsumeAndGetDisease(key.CreateTag(), amount2, out amount_consumed, out SimUtil.DiseaseInfo _, out float _);
        if ((double) amount_consumed > (double) num)
        {
          lubricant = key;
          num = amount_consumed;
        }
        amount2 -= amount_consumed;
      }
      this.GetComponent<Storage>().ConsumeIgnoringDisease(GameTags.LubricatingOil, amount2);
      smi.RefillOil(amount1);
      BionicOilMonitor.ApplyLubricationEffects(worker.GetComponent<Effects>(), lubricant);
    }
    base.OnCompleteWork(worker);
  }
}
