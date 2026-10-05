// Decompiled with JetBrains decompiler
// Type: WaterCoolerChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class WaterCoolerChore : Chore<WaterCoolerChore.StatesInstance>, IWorkerPrioritizable
{
  public int basePriority = RELAXATION.PRIORITY.TIER2;
  public string specificEffect = "Socialized";
  public string trackingEffect = "RecentlySocialized";

  public WaterCoolerChore(
    IStateMachineTarget master,
    Workable chat_workable,
    Action<Chore> on_complete = null,
    Action<Chore> on_begin = null,
    Action<Chore> on_end = null)
    : base(Db.Get().ChoreTypes.Relax, master, master.GetComponent<ChoreProvider>(), on_complete: on_complete, on_begin: on_begin, on_end: on_end, master_priority_class: PriorityScreen.PriorityClass.high, report_type: ReportManager.ReportType.PersonalTime)
  {
    this.smi = new WaterCoolerChore.StatesInstance(this);
    this.smi.sm.chitchatlocator.Set((KMonoBehaviour) chat_workable, this.smi);
    this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, (object) chat_workable);
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Recreation);
    this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, (object) this);
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.sm.drinker.Set(context.consumerState.gameObject, this.smi, false);
    base.Begin(context);
  }

  public bool GetWorkerPriority(WorkerBase worker, out int priority)
  {
    priority = this.basePriority;
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(this.trackingEffect) && component.HasEffect(this.trackingEffect))
    {
      priority = 0;
      return false;
    }
    if (!string.IsNullOrEmpty(this.specificEffect) && component.HasEffect(this.specificEffect))
      priority = RELAXATION.PRIORITY.RECENTLY_USED;
    return true;
  }

  public class States : 
    GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore>
  {
    public StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.TargetParameter drinker;
    public StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.TargetParameter chitchatlocator;
    public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.ApproachSubState<WaterCooler> drink_move;
    public WaterCoolerChore.States.DrinkStates drink;
    public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.ApproachSubState<IApproachable> chat_move;
    public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State chat;
    public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State success;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.drink_move;
      this.Target(this.drinker);
      this.drink_move.InitializeStates(this.drinker, this.masterTarget, (GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State) this.drink);
      this.drink.ToggleAnims(new Func<WaterCoolerChore.StatesInstance, KAnimFile>(WaterCoolerChore.States.GetAnimFileName)).DefaultState(this.drink.drink);
      this.drink.drink.Face(this.masterTarget, 0.5f).PlayAnim("working_pre").QueueAnim("working_loop").OnAnimQueueComplete(this.drink.post);
      this.drink.post.Enter("Drink", new StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State.Callback(this.TriggerDrink)).Enter("Mark", new StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State.Callback(this.MarkAsRecentlySocialized)).PlayAnim("working_pst").OnAnimQueueComplete((GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State) this.chat_move);
      this.chat_move.InitializeStates(this.drinker, this.chitchatlocator, this.chat);
      this.chat.ToggleWork<SocialGatheringPointWorkable>(this.chitchatlocator, this.success, (GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State) null, (Func<WaterCoolerChore.StatesInstance, bool>) null);
      this.success.ReturnSuccess();
    }

    public static KAnimFile GetAnimFileName(WaterCoolerChore.StatesInstance smi)
    {
      GameObject gameObject = smi.sm.drinker.Get(smi);
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        return Assets.GetAnim((HashedString) "anim_interacts_watercooler_kanim");
      MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
      return (UnityEngine.Object) component != (UnityEngine.Object) null && component.model == BionicMinionConfig.MODEL ? Assets.GetAnim((HashedString) "anim_bionic_interacts_watercooler_kanim") : Assets.GetAnim((HashedString) "anim_interacts_watercooler_kanim");
    }

    private void MarkAsRecentlySocialized(WaterCoolerChore.StatesInstance smi)
    {
      Effects component = this.stateTarget.Get<WorkerBase>(smi).GetComponent<Effects>();
      if (string.IsNullOrEmpty(smi.master.trackingEffect))
        return;
      component.Add(smi.master.trackingEffect, true);
    }

    private void TriggerDrink(WaterCoolerChore.StatesInstance smi)
    {
      WorkerBase workerBase = this.stateTarget.Get<WorkerBase>(smi);
      smi.master.target.gameObject.GetSMI<WaterCooler.StatesInstance>().Drink(workerBase.gameObject);
    }

    public class DrinkStates : 
      GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State
    {
      public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State drink;
      public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State post;
    }
  }

  public class StatesInstance(WaterCoolerChore master) : 
    GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.GameInstance(master)
  {
  }
}
