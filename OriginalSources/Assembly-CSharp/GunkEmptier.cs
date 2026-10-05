// Decompiled with JetBrains decompiler
// Type: GunkEmptier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class GunkEmptier : 
  GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>
{
  private static string DISEASE_ID = DUPLICANTSTATS.BIONICS.Secretions.PEE_DISEASE;
  private static int DISEASE_ON_DUPE_COUNT_PER_USE = DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE / 20;
  public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State noOperational;
  public GunkEmptier.OperationalStates operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.noOperational;
    this.noOperational.EventTransition(GameHashes.OperationalChanged, (GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State) this.operational, new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.IsOperational));
    this.operational.EventTransition(GameHashes.OperationalChanged, this.noOperational, GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Not(new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.IsOperational))).DefaultState(this.operational.noStorageSpace);
    this.operational.noStorageSpace.ToggleStatusItem(Db.Get().BuildingStatusItems.GunkEmptierFull).EventTransition(GameHashes.OnStorageChange, this.operational.ready, new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.HasSpaceToEmptyABionicGunkTank));
    this.operational.ready.EventTransition(GameHashes.OnStorageChange, this.operational.noStorageSpace, GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Not(new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.HasSpaceToEmptyABionicGunkTank))).ToggleRecurringChore(new Func<GunkEmptier.Instance, Chore>(GunkEmptier.CreateChore));
  }

  public static bool HasSpaceToEmptyABionicGunkTank(GunkEmptier.Instance smi)
  {
    return (double) smi.RemainingStorageCapacity >= (double) GunkMonitor.GUNK_CAPACITY;
  }

  public static bool IsOperational(GunkEmptier.Instance smi) => smi.IsOperational;

  private static WorkChore<GunkEmptierWorkable> CreateChore(GunkEmptier.Instance smi)
  {
    WorkChore<GunkEmptierWorkable> chore = new WorkChore<GunkEmptierWorkable>(Db.Get().ChoreTypes.ExpellGunk, smi.master, allow_in_red_alert: false, ignore_schedule_block: true, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.personalNeeds, add_to_daily_report: false);
    chore.AddPrecondition(ChorePreconditions.instance.IsPreferredAssignableOrUrgentBladder, (object) smi.master.GetComponent<Assignable>());
    return chore;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class OperationalStates : 
    GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State
  {
    public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State noStorageSpace;
    public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State ready;
  }

  public new class Instance : 
    GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.GameInstance
  {
    private Operational operational;
    private Storage storage;

    public float RemainingStorageCapacity => this.storage.RemainingCapacity();

    public bool IsOperational => this.operational.IsOperational;

    public Instance(IStateMachineTarget master, GunkEmptier.Def def)
      : base(master, def)
    {
      GunkEmptierWorkable component = this.GetComponent<GunkEmptierWorkable>();
      GunkEmptierWorkable gunkEmptierWorkable = component;
      gunkEmptierWorkable.OnWorkableEventCB = gunkEmptierWorkable.OnWorkableEventCB + new System.Action<Workable, Workable.WorkableEvent>(this.OnGunkEmptierUsed);
      Components.GunkExtractors.Add(component);
      this.storage = this.GetComponent<Storage>();
      this.operational = this.GetComponent<Operational>();
      this.gameObject.AddOrGet<Ownable>().AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.AssignablePrecondition_OnlyOnBionics));
    }

    protected override void OnCleanUp()
    {
      GunkEmptierWorkable component = this.GetComponent<GunkEmptierWorkable>();
      GunkEmptierWorkable gunkEmptierWorkable = component;
      gunkEmptierWorkable.OnWorkableEventCB = gunkEmptierWorkable.OnWorkableEventCB - new System.Action<Workable, Workable.WorkableEvent>(this.OnGunkEmptierUsed);
      Components.GunkExtractors.Remove(component);
      base.OnCleanUp();
    }

    private bool AssignablePrecondition_OnlyOnBionics(MinionAssignablesProxy worker)
    {
      return worker.GetMinionModel() == BionicMinionConfig.MODEL;
    }

    public void OnGunkEmptierUsed(Workable workable, Workable.WorkableEvent ev)
    {
      if (ev != Workable.WorkableEvent.WorkCompleted)
        return;
      this.AddDisseaseToWorker(workable.worker);
    }

    public void AddDisseaseToWorker(WorkerBase worker)
    {
      if ((UnityEngine.Object) worker != (UnityEngine.Object) null)
      {
        byte index = Db.Get().Diseases.GetIndex((HashedString) GunkEmptier.DISEASE_ID);
        worker.GetComponent<PrimaryElement>().AddDisease(index, GunkEmptier.DISEASE_ON_DUPE_COUNT_PER_USE, "GunkEmptier.Flush");
        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format((string) DUPLICANTS.DISEASES.ADDED_POPFX, (object) Db.Get().Diseases[(int) index].Name, (object) GunkEmptier.DISEASE_ON_DUPE_COUNT_PER_USE), this.transform, Vector3.up);
        Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms);
      }
      else
        DebugUtil.LogWarningArgs((object) "Tried to add disease on gunk emptier use but worker was null");
    }
  }
}
