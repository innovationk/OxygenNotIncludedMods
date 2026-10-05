// Decompiled with JetBrains decompiler
// Type: RoboDancerChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class RoboDancerChore : Chore<RoboDancerChore.StatesInstance>, IWorkerPrioritizable
{
  private int basePriority = RELAXATION.PRIORITY.TIER1;

  public RoboDancerChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.JoyReaction, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.high, report_type: ReportManager.ReportType.PersonalTime)
  {
    this.showAvailabilityInHoverText = false;
    this.smi = new RoboDancerChore.StatesInstance(this, target.gameObject);
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Recreation);
    this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, (object) this);
  }

  public bool GetWorkerPriority(WorkerBase worker, out int priority)
  {
    priority = this.basePriority;
    return true;
  }

  public class States : 
    GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore>
  {
    public StateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.TargetParameter roboDancer;
    public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State idle;
    public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State goToStand;
    public RoboDancerChore.States.DancingStates dancing;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.goToStand;
      this.Target(this.roboDancer);
      this.idle.EventTransition(GameHashes.ScheduleBlocksTick, this.goToStand, (StateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.Transition.ConditionCallback) (smi => !smi.IsRecTime()));
      this.goToStand.MoveTo((Func<RoboDancerChore.StatesInstance, int>) (smi => smi.GetTargetCell()), (GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State) this.dancing, this.idle);
      this.dancing.ToggleEffect("Dancing").ToggleAnims("anim_bionic_joy_kanim").DefaultState(this.dancing.pre).Update((Action<RoboDancerChore.StatesInstance, float>) ((smi, dt) =>
      {
        RoboDancer.Instance smi1 = this.roboDancer.Get(smi).GetSMI<RoboDancer.Instance>();
        RoboDancer sm = smi1.sm;
        sm.hasAudience.Set(smi.HasAudience(), smi1);
        double num = (double) sm.timeSpentDancing.Set(sm.timeSpentDancing.Get(smi1) + dt, smi1);
      }), UpdateRate.SIM_33ms).Exit((StateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State.Callback) (smi => smi.ClearAudienceWorkables()));
      this.dancing.pre.QueueAnim("robotdance_pre").OnAnimQueueComplete(this.dancing.variation_1).Enter((StateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State.Callback) (smi =>
      {
        smi.ClearAudienceWorkables();
        smi.CreateAudienceWorkables();
      }));
      this.dancing.variation_1.QueueAnim("robotdance_loop").OnAnimQueueComplete(this.dancing.variation_2);
      this.dancing.variation_2.QueueAnim("robotdance_2_loop").OnAnimQueueComplete(this.dancing.pst);
      this.dancing.pst.QueueAnim("robotdance_pst").OnAnimQueueComplete(this.dancing.pre);
    }

    public class DancingStates : 
      GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State
    {
      public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State pre;
      public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State variation_1;
      public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State variation_2;
      public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State pst;
    }
  }

  public class StatesInstance : 
    GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.GameInstance
  {
    private GameObject roboDancer;
    private GameObject[] audienceWorkables = new GameObject[4];
    private WatchRoboDancerWorkable[] watchWorkables;
    private static Chore.Precondition IsNotRoboHyped = new Chore.Precondition()
    {
      id = nameof (IsNotRoboHyped),
      description = "__ Duplicant hasn't watched the dance yet",
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => !((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null) && !context.consumerState.gameObject.GetComponent<Effects>().HasEffect(WatchRoboDancerWorkable.TRACKING_EFFECT))
    };

    public StatesInstance(RoboDancerChore master, GameObject roboDancer)
      : base(master)
    {
      this.roboDancer = roboDancer;
      this.sm.roboDancer.Set(roboDancer, this.smi, false);
    }

    public bool IsRecTime()
    {
      return this.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
    }

    public int GetTargetCell()
    {
      Navigator component = this.GetComponent<Navigator>();
      float num = float.MaxValue;
      SocialGatheringPoint cmp1 = (SocialGatheringPoint) null;
      foreach (SocialGatheringPoint cmp2 in Components.SocialGatheringPoints.GetItems((int) Grid.WorldIdx[Grid.PosToCell((StateMachine.Instance) this)]))
      {
        float navigationCost = (float) component.GetNavigationCost(Grid.PosToCell((KMonoBehaviour) cmp2));
        if ((double) navigationCost != -1.0 && (double) navigationCost < (double) num)
        {
          num = navigationCost;
          cmp1 = cmp2;
        }
      }
      return (UnityEngine.Object) cmp1 != (UnityEngine.Object) null ? Grid.PosToCell((KMonoBehaviour) cmp1) : Grid.PosToCell(this.master.gameObject);
    }

    public bool HasAudience()
    {
      if (this.smi.watchWorkables == null)
        return false;
      foreach (Workable watchWorkable in this.smi.watchWorkables)
      {
        if ((bool) (UnityEngine.Object) watchWorkable.worker)
          return true;
      }
      return false;
    }

    public void CreateAudienceWorkables()
    {
      int cell1 = Grid.PosToCell(this.gameObject);
      Vector3Int[] vector3IntArray = new Vector3Int[6]
      {
        Vector3Int.left * 3,
        Vector3Int.left * 2,
        Vector3Int.left,
        Vector3Int.right,
        Vector3Int.right * 2,
        Vector3Int.right * 3
      };
      int length = 0;
      for (int index = 0; index < this.audienceWorkables.Length; ++index)
      {
        int cell2 = Grid.OffsetCell(cell1, vector3IntArray[index].x, vector3IntArray[index].y);
        if (Grid.IsValidCellInWorld(cell2, (int) Grid.WorldIdx[cell1]))
        {
          GameObject locator = ChoreHelpers.CreateLocator("WatchRoboDancerWorkable", Grid.CellToPos(cell2));
          this.audienceWorkables[index] = locator;
          KSelectable kselectable = locator.AddOrGet<KSelectable>();
          kselectable.SetName("WatchRoboDancerWorkable");
          kselectable.IsSelectable = false;
          WatchRoboDancerWorkable target = locator.AddOrGet<WatchRoboDancerWorkable>();
          target.owner = this.roboDancer;
          WorkChore<WatchRoboDancerWorkable> data = new WorkChore<WatchRoboDancerWorkable>(Db.Get().ChoreTypes.JoyReaction, (IStateMachineTarget) target, schedule_block: Db.Get().ScheduleBlockTypes.Recreation, priority_class: PriorityScreen.PriorityClass.high);
          data.AddPrecondition(ChorePreconditions.instance.IsNotARobot, (object) null);
          data.AddPrecondition(RoboDancerChore.StatesInstance.IsNotRoboHyped, (object) data);
          ++length;
        }
      }
      this.watchWorkables = new WatchRoboDancerWorkable[length];
      for (int index = 0; index < length; ++index)
        this.watchWorkables[index] = this.audienceWorkables[index].GetComponent<WatchRoboDancerWorkable>();
    }

    public void ClearAudienceWorkables()
    {
      for (int index = 0; index < this.audienceWorkables.Length; ++index)
      {
        if (!((UnityEngine.Object) this.audienceWorkables[index] == (UnityEngine.Object) null))
        {
          WorkerBase worker = this.audienceWorkables[index].GetComponent<WatchRoboDancerWorkable>().worker;
          if ((UnityEngine.Object) worker != (UnityEngine.Object) null)
            this.audienceWorkables[index].GetComponent<WatchRoboDancerWorkable>().CompleteWork(worker);
          ChoreHelpers.DestroyLocator(this.audienceWorkables[index]);
        }
      }
      this.watchWorkables = (WatchRoboDancerWorkable[]) null;
    }
  }
}
