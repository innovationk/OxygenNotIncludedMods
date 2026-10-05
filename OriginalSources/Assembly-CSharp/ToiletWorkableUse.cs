// Decompiled with JetBrains decompiler
// Type: ToiletWorkableUse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/ToiletWorkableUse")]
public class ToiletWorkableUse : Workable
{
  public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();
  public Dictionary<Tag, HashedString[]> workerTypePstAnims = new Dictionary<Tag, HashedString[]>();
  [Serialize]
  public int timesUsed;
  [Serialize]
  public Tag last_user_id;
  [Serialize]
  public SimHashes lastElementRemovedFromDupe = SimHashes.DirtyWater;
  [Serialize]
  public float lastAmountOfWasteMassRemovedFromDupe;

  private ToiletWorkableUse() => this.SetReportType(ReportManager.ReportType.PersonalTime);

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.showProgressBar = true;
    this.resetProgressOnStop = true;
    this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
    this.SetWorkTime(8.5f);
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    if (Sim.IsRadiationEnabled() && (double) worker.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0.0)
      worker.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads);
    Game.Instance.roomProber.GetRoomOfGameObject(this.gameObject)?.roomType.TriggerRoomEffects(this.GetComponent<KPrefabID>(), worker.GetComponent<Effects>());
    if (!((UnityEngine.Object) worker != (UnityEngine.Object) null))
      return;
    this.last_user_id = worker.gameObject.PrefabID();
  }

  public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
  {
    HashedString[] hashedStringArray = (HashedString[]) null;
    if (this.workerTypePstAnims.TryGetValue(worker.PrefabID(), out hashedStringArray))
    {
      this.workingPstComplete = hashedStringArray;
      this.workingPstFailed = hashedStringArray;
    }
    return base.GetWorkPstAnims(worker, successfully_completed);
  }

  public override Workable.AnimInfo GetAnim(WorkerBase worker)
  {
    KAnimFile[] kanimFileArray = (KAnimFile[]) null;
    if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out kanimFileArray))
      this.overrideAnims = kanimFileArray;
    return base.GetAnim(worker);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    if (Sim.IsRadiationEnabled())
      worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads);
    base.OnStopWork(worker);
  }

  protected override void OnAbortWork(WorkerBase worker)
  {
    if (Sim.IsRadiationEnabled())
      worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads);
    base.OnAbortWork(worker);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    AmountInstance amountInstance1 = Db.Get().Amounts.Bladder.Lookup((Component) worker);
    if (amountInstance1 != null)
    {
      this.lastAmountOfWasteMassRemovedFromDupe = DUPLICANTSTATS.STANDARD.Secretions.PEE_PER_TOILET_PEE;
      this.lastElementRemovedFromDupe = SimHashes.DirtyWater;
      double num = (double) amountInstance1.SetValue(0.0f);
    }
    else
    {
      GunkMonitor.Instance smi = worker.GetSMI<GunkMonitor.Instance>();
      if (smi != null)
      {
        this.lastAmountOfWasteMassRemovedFromDupe = smi.CurrentGunkMass;
        this.lastElementRemovedFromDupe = GunkMonitor.GunkElement;
        smi.SetGunkMassValue(0.0f);
        Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_GunkedToilet);
      }
    }
    if (Sim.IsRadiationEnabled())
    {
      worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads);
      AmountInstance amountInstance2 = Db.Get().Amounts.RadiationBalance.Lookup((Component) worker);
      float d = Math.Min(amountInstance2.value, 100f * worker.GetSMI<RadiationMonitor.Instance>().difficultySettingMod);
      if ((double) d >= 1.0)
        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Math.Floor((double) d).ToString() + (string) UI.UNITSUFFIXES.RADIATION.RADS, worker.transform, Vector3.up * 2f);
      double num = (double) amountInstance2.ApplyDelta(-d);
    }
    ++this.timesUsed;
    if (amountInstance1 != null)
      this.Trigger(-350347868, (object) worker);
    else
      this.Trigger(1234642927, (object) worker);
    base.OnCompleteWork(worker);
  }

  public override StatusItem GetWorkerStatusItem()
  {
    return (UnityEngine.Object) this.worker != (UnityEngine.Object) null && this.worker.gameObject.HasTag(GameTags.Minions.Models.Bionic) ? Db.Get().DuplicantStatusItems.CloggingToilet : base.GetWorkerStatusItem();
  }
}
