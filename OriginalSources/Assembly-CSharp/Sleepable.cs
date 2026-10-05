// Decompiled with JetBrains decompiler
// Type: Sleepable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Sleepable")]
public class Sleepable : Workable
{
  private const float STRECH_CHANCE = 0.33f;
  [MyCmpGet]
  public Assignable assignable;
  public IApproachable approachable;
  [MyCmpGet]
  private Operational operational;
  public string effectName = "Sleep";
  public List<string> wakeEffects;
  public bool stretchOnWake = true;
  private float wakeTime;
  private bool isDoneSleeping;
  public bool isNormalBed = true;
  public ClinicDreamable Dreamable;
  private static readonly HashedString[] normalWorkAnims = new HashedString[2]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop"
  };
  private static readonly HashedString[] hatWorkAnims = new HashedString[2]
  {
    (HashedString) "hat_pre",
    (HashedString) "working_loop"
  };
  private static readonly HashedString[] normalWorkPstAnim = new HashedString[1]
  {
    (HashedString) "working_pst"
  };
  private static readonly HashedString[] hatWorkPstAnim = new HashedString[1]
  {
    (HashedString) "hat_pst"
  };

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetReportType(ReportManager.ReportType.PersonalTime);
    this.showProgressBar = false;
    this.workerStatusItem = (StatusItem) null;
    this.synchronizeAnims = false;
    this.triggerWorkReactions = false;
    this.lightEfficiencyBonus = false;
    this.approachable = this.GetComponent<IApproachable>();
  }

  protected override void OnSpawn()
  {
    if (this.isNormalBed)
      Components.NormalBeds.Add(this.gameObject.GetMyWorldId(), this);
    this.SetWorkTime(float.PositiveInfinity);
  }

  public override HashedString[] GetWorkAnims(WorkerBase worker)
  {
    MinionResume component = worker.GetComponent<MinionResume>();
    return (UnityEngine.Object) this.GetComponent<Building>() != (UnityEngine.Object) null && (UnityEngine.Object) component != (UnityEngine.Object) null && component.CurrentHat != null ? Sleepable.hatWorkAnims : Sleepable.normalWorkAnims;
  }

  public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
  {
    MinionResume component = worker.GetComponent<MinionResume>();
    return (UnityEngine.Object) this.GetComponent<Building>() != (UnityEngine.Object) null && (UnityEngine.Object) component != (UnityEngine.Object) null && component.CurrentHat != null ? Sleepable.hatWorkPstAnim : Sleepable.normalWorkPstAnim;
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    KAnimControllerBase animController = this.GetAnimController();
    if ((UnityEngine.Object) animController != (UnityEngine.Object) null)
    {
      animController.Play((HashedString) "working_pre");
      animController.Queue((HashedString) "working_loop", KAnim.PlayMode.Loop);
    }
    this.Subscribe(worker.gameObject, -1142962013, new Action<object>(this.PlayPstAnim));
    if ((UnityEngine.Object) this.operational != (UnityEngine.Object) null)
      this.operational.SetActive(true);
    worker.Trigger(-1283701846, (object) this);
    worker.GetComponent<Effects>().Add(this.effectName, false);
    this.isDoneSleeping = false;
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    if (this.isDoneSleeping)
      return (double) Time.time > (double) this.wakeTime;
    if ((UnityEngine.Object) this.Dreamable != (UnityEngine.Object) null && !this.Dreamable.DreamIsDisturbed)
      this.Dreamable.WorkTick(worker, dt);
    if (worker.GetSMI<StaminaMonitor.Instance>().ShouldExitSleep())
    {
      this.isDoneSleeping = true;
      this.wakeTime = Time.time + UnityEngine.Random.value * 3f;
    }
    return false;
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    base.OnStopWork(worker);
    if ((UnityEngine.Object) this.operational != (UnityEngine.Object) null)
      this.operational.SetActive(false);
    this.Unsubscribe(worker.gameObject, -1142962013, new Action<object>(this.PlayPstAnim));
    if (!((UnityEngine.Object) worker != (UnityEngine.Object) null))
      return;
    Effects component = worker.GetComponent<Effects>();
    component.Remove(this.effectName);
    if (this.wakeEffects != null)
    {
      foreach (string wakeEffect in this.wakeEffects)
        component.Add(wakeEffect, true);
    }
    if (this.stretchOnWake && (double) UnityEngine.Random.value < 0.33000001311302185)
    {
      EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, Db.Get().Emotes.Minion.MorningStretch);
    }
    if ((double) worker.GetAmounts().Get(Db.Get().Amounts.Stamina).value >= (double) worker.GetAmounts().Get(Db.Get().Amounts.Stamina).GetMax())
      return;
    worker.Trigger(1338475637, (object) this);
  }

  public override bool InstantlyFinish(WorkerBase worker) => false;

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (!this.isNormalBed)
      return;
    Components.NormalBeds.Remove(this.gameObject.GetMyWorldId(), this);
  }

  private void PlayPstAnim(object data)
  {
    WorkerBase workerBase = (WorkerBase) data;
    if (!((UnityEngine.Object) workerBase != (UnityEngine.Object) null) || !((UnityEngine.Object) workerBase.GetWorkable() != (UnityEngine.Object) null))
      return;
    KAnimControllerBase component = workerBase.GetWorkable().gameObject.GetComponent<KAnimControllerBase>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.Play((HashedString) "working_pst");
  }
}
