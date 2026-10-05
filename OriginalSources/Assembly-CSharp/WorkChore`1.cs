// Decompiled with JetBrains decompiler
// Type: WorkChore`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WorkChore<WorkableType> : Chore<WorkChore<WorkableType>.StatesInstance> where WorkableType : Workable
{
  public Func<Chore.Precondition.Context, bool> preemption_cb;

  public bool onlyWhenOperational { get; private set; }

  public override string ToString() => $"WorkChore<{typeof (WorkableType).ToString()}>";

  public WorkChore(
    ChoreType chore_type,
    IStateMachineTarget target,
    ChoreProvider chore_provider = null,
    bool run_until_complete = true,
    Action<Chore> on_complete = null,
    Action<Chore> on_begin = null,
    Action<Chore> on_end = null,
    bool allow_in_red_alert = true,
    ScheduleBlockType schedule_block = null,
    bool ignore_schedule_block = false,
    bool only_when_operational = true,
    KAnimFile override_anims = null,
    bool is_preemptable = false,
    bool allow_in_context_menu = true,
    bool allow_prioritization = true,
    PriorityScreen.PriorityClass priority_class = PriorityScreen.PriorityClass.basic,
    int priority_class_value = 5,
    bool ignore_building_assignment = false,
    bool add_to_daily_report = true)
  {
    ChoreType chore_type1 = chore_type;
    IStateMachineTarget target1 = target;
    ChoreProvider chore_provider1 = chore_provider;
    int num1 = run_until_complete ? 1 : 0;
    Action<Chore> on_complete1 = on_complete;
    Action<Chore> on_begin1 = on_begin;
    Action<Chore> on_end1 = on_end;
    bool flag1 = is_preemptable;
    bool flag2 = allow_in_context_menu;
    int master_priority_class = (int) priority_class;
    int master_priority_value = priority_class_value;
    int num2 = flag1 ? 1 : 0;
    int num3 = flag2 ? 1 : 0;
    int num4 = add_to_daily_report ? 1 : 0;
    // ISSUE: explicit constructor call
    base.\u002Ector(chore_type1, target1, chore_provider1, num1 != 0, on_complete1, on_begin1, on_end1, (PriorityScreen.PriorityClass) master_priority_class, master_priority_value, num2 != 0, num3 != 0, add_to_daily_report: num4 != 0);
    this.smi = new WorkChore<WorkableType>.StatesInstance(this, target.gameObject, override_anims);
    this.onlyWhenOperational = only_when_operational;
    if (allow_prioritization)
      this.SetPrioritizable(target.GetComponent<Prioritizable>());
    this.AddPrecondition(ChorePreconditions.instance.IsNotTransferArm, (object) null);
    if (!allow_in_red_alert)
      this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    if (schedule_block != null)
      this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) schedule_block);
    else if (!ignore_schedule_block)
      this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Work);
    this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, (object) this.smi.sm.workable.Get<WorkableType>(this.smi));
    Operational component1 = target.GetComponent<Operational>();
    if (only_when_operational && (UnityEngine.Object) component1 != (UnityEngine.Object) null)
      this.AddPrecondition(ChorePreconditions.instance.IsOperational, (object) component1);
    if (only_when_operational)
    {
      Deconstructable component2 = target.GetComponent<Deconstructable>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, (object) component2);
      BuildingEnabledButton component3 = target.GetComponent<BuildingEnabledButton>();
      if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
        this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, (object) component3);
    }
    if (!ignore_building_assignment && (UnityEngine.Object) this.smi.sm.workable.Get(this.smi).GetComponent<Assignable>() != (UnityEngine.Object) null)
      this.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, (object) this.smi.sm.workable.Get<Assignable>(this.smi));
    if ((UnityEngine.Object) override_anims != (UnityEngine.Object) null)
      this.AddPrecondition(ChorePreconditions.instance.IsNotARobot, (object) null);
    WorkableType workableType = target as WorkableType;
    if (!((UnityEngine.Object) workableType != (UnityEngine.Object) null))
      return;
    if (!string.IsNullOrEmpty(workableType.requiredSkillPerk))
      this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) (HashedString) workableType.requiredSkillPerk);
    if (!workableType.requireMinionToWork)
      return;
    this.AddPrecondition(ChorePreconditions.instance.IsMinion, (object) null);
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.sm.worker.Set(context.consumerState.gameObject, this.smi, false);
    base.Begin(context);
  }

  public override bool IsValid()
  {
    WorkableType target = this.target as WorkableType;
    if (!((UnityEngine.Object) target != (UnityEngine.Object) null))
      return base.IsValid();
    return (UnityEngine.Object) this.provider != (UnityEngine.Object) null && Grid.IsWorldValidCell(target.GetCell());
  }

  public bool IsOperationalValid()
  {
    if (this.onlyWhenOperational)
    {
      Operational component = this.smi.master.GetComponent<Operational>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && !component.IsOperational)
        return false;
    }
    return true;
  }

  public override bool CanPreempt(Chore.Precondition.Context context)
  {
    if (!base.CanPreempt(context) || (UnityEngine.Object) context.chore.driver == (UnityEngine.Object) null || (UnityEngine.Object) context.chore.driver == (UnityEngine.Object) context.consumerState.choreDriver)
      return false;
    Workable workable = (Workable) this.smi.sm.workable.Get<WorkableType>(this.smi);
    if ((UnityEngine.Object) workable == (UnityEngine.Object) null || (UnityEngine.Object) workable.worker != (UnityEngine.Object) null && (workable.worker.GetState() == WorkerBase.State.PendingCompletion || workable.worker.GetState() == WorkerBase.State.Completing))
      return false;
    if (this.preemption_cb != null)
    {
      if (!this.preemption_cb(context))
        return false;
    }
    else
    {
      int num = 4;
      int navigationCost = context.chore.driver.GetComponent<Navigator>().GetNavigationCost((IApproachable) workable);
      if (navigationCost == -1 || navigationCost < num || context.consumerState.navigator.GetNavigationCost((IApproachable) workable) * 2 > navigationCost)
        return false;
    }
    return true;
  }

  public class StatesInstance : 
    GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.GameInstance
  {
    private KAnimFile overrideAnims;

    public StatesInstance(
      WorkChore<WorkableType> master,
      GameObject workable,
      KAnimFile override_anims)
      : base(master)
    {
      this.overrideAnims = override_anims;
      this.sm.workable.Set(workable, this.smi, false);
    }

    public void EnableAnimOverrides()
    {
      if (!((UnityEngine.Object) this.overrideAnims != (UnityEngine.Object) null))
        return;
      this.sm.worker.Get<KAnimControllerBase>(this.smi).AddAnimOverrides(this.overrideAnims);
    }

    public void DisableAnimOverrides()
    {
      if (!((UnityEngine.Object) this.overrideAnims != (UnityEngine.Object) null))
        return;
      this.sm.worker.Get<KAnimControllerBase>(this.smi).RemoveAnimOverrides(this.overrideAnims);
    }
  }

  public class States : 
    GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>>
  {
    public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.ApproachSubState<WorkableType> approach;
    public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State work;
    public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State success;
    public StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.TargetParameter workable;
    public StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.TargetParameter worker;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.approach;
      this.Target(this.worker);
      this.approach.InitializeStates(this.worker, this.workable, this.work).Update("CheckOperational", (Action<WorkChore<WorkableType>.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.master.IsOperationalValid())
          return;
        smi.StopSM("Building not operational");
      }));
      this.work.Enter((StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State.Callback) (smi => smi.EnableAnimOverrides())).ToggleWork<WorkableType>(this.workable, this.success, (GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State) null, (Func<WorkChore<WorkableType>.StatesInstance, bool>) (smi => smi.master.IsOperationalValid())).Exit((StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State.Callback) (smi => smi.DisableAnimOverrides()));
      this.success.ReturnSuccess();
    }
  }
}
