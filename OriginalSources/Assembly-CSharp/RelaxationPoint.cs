// Decompiled with JetBrains decompiler
// Type: RelaxationPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/RelaxationPoint")]
public class RelaxationPoint : Workable
{
  [MyCmpGet]
  private RoomTracker roomTracker;
  [Serialize]
  protected float stopStressingValue;
  public float stressModificationValue;
  public float roomStressModificationValue;
  private RelaxationPoint.RelaxationPointSM.Instance smi;
  private static Effect stressReductionEffect;
  private static Effect roomStressReductionEffect;

  public RelaxationPoint()
  {
    this.SetReportType(ReportManager.ReportType.PersonalTime);
    this.showProgressBar = false;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.lightEfficiencyBonus = false;
    this.GetComponent<KPrefabID>().AddTag(TagManager.Create(nameof (RelaxationPoint), (string) MISC.TAGS.RELAXATION_POINT));
    if (RelaxationPoint.stressReductionEffect != null)
      return;
    RelaxationPoint.stressReductionEffect = this.CreateEffect();
    RelaxationPoint.roomStressReductionEffect = this.CreateRoomEffect();
  }

  public Effect CreateEffect()
  {
    Effect effect = new Effect("StressReduction", (string) DUPLICANTS.MODIFIERS.STRESSREDUCTION.NAME, (string) DUPLICANTS.MODIFIERS.STRESSREDUCTION.TOOLTIP, 0.0f, true, false, false);
    effect.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, this.stressModificationValue / 600f, (string) DUPLICANTS.MODIFIERS.STRESSREDUCTION.NAME));
    return effect;
  }

  public Effect CreateRoomEffect()
  {
    Effect roomEffect = new Effect("RoomRelaxationEffect", (string) DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.NAME, (string) DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.TOOLTIP, 0.0f, true, false, false);
    roomEffect.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, this.roomStressModificationValue / 600f, (string) DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.NAME));
    return roomEffect;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi = new RelaxationPoint.RelaxationPointSM.Instance(this);
    this.smi.StartSM();
    this.SetWorkTime(float.PositiveInfinity);
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    if ((UnityEngine.Object) this.roomTracker != (UnityEngine.Object) null && this.roomTracker.room != null && this.roomTracker.room.roomType == Db.Get().RoomTypes.MassageClinic)
      worker.GetComponent<Effects>().Add(RelaxationPoint.roomStressReductionEffect, false);
    else
      worker.GetComponent<Effects>().Add(RelaxationPoint.stressReductionEffect, false);
    this.GetComponent<Operational>().SetActive(true);
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    if ((double) Db.Get().Amounts.Stress.Lookup(worker.gameObject).value <= (double) this.stopStressingValue)
      return true;
    base.OnWorkTick(worker, dt);
    return false;
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    worker.GetComponent<Effects>().Remove(RelaxationPoint.stressReductionEffect);
    worker.GetComponent<Effects>().Remove(RelaxationPoint.roomStressReductionEffect);
    this.GetComponent<Operational>().SetActive(false);
    base.OnStopWork(worker);
  }

  protected override void OnCompleteWork(WorkerBase worker) => base.OnCompleteWork(worker);

  public override bool InstantlyFinish(WorkerBase worker) => false;

  protected virtual WorkChore<RelaxationPoint> CreateWorkChore()
  {
    return new WorkChore<RelaxationPoint>(Db.Get().ChoreTypes.Relax, (IStateMachineTarget) this, run_until_complete: false, allow_in_red_alert: false);
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = base.GetDescriptors(go);
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.STRESSREDUCEDPERMINUTE, (object) GameUtil.GetFormattedPercent((float) ((double) this.stressModificationValue / 600.0 * 60.0))), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.STRESSREDUCEDPERMINUTE, (object) GameUtil.GetFormattedPercent((float) ((double) this.stressModificationValue / 600.0 * 60.0))));
    descriptors.Add(descriptor);
    return descriptors;
  }

  public class RelaxationPointSM : 
    GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint>
  {
    public GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.State unoperational;
    public GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.State operational;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.unoperational;
      this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (StateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational)).PlayAnim("off");
      this.operational.ToggleChore((Func<RelaxationPoint.RelaxationPointSM.Instance, Chore>) (smi => (Chore) smi.master.CreateWorkChore()), this.unoperational);
    }

    public new class Instance(RelaxationPoint master) : 
      GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.GameInstance(master)
    {
    }
  }
}
