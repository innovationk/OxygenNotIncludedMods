// Decompiled with JetBrains decompiler
// Type: RemoteWorkerSM
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using System;
using UnityEngine;

#nullable disable
public class RemoteWorkerSM : StateMachineComponent<RemoteWorkerSM.StatesInstance>
{
  [MyCmpAdd]
  private RemoteWorkerCapacitor power;
  [MyCmpAdd]
  private RemoteWorkerGunkMonitor gunk;
  [MyCmpAdd]
  private RemoteWorkerOilMonitor oil;
  [MyCmpAdd]
  private ChoreDriver driver;
  [MyCmpGet]
  private ChoreConsumer consumer;
  [MyCmpGet]
  private Storage storage;
  public bool playNewWorker;
  [Serialize]
  private bool docked = true;
  private Chore.Precondition.Context? nextChore;
  private const string LostAnim_pre = "sos_pre";
  private const string LostAnim_loop = "sos_loop";
  private const string LostAnim_pst = "sos_pst";
  private const string DeathAnim = "explode";
  [Serialize]
  private Ref<RemoteWorkerDock> homeDepot;

  public bool Docked
  {
    get => this.docked;
    set => this.docked = value;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public void SetNextChore(Chore.Precondition.Context next)
  {
    if (this.nextChore.HasValue)
      this.nextChore.Value.chore.Reserve((ChoreDriver) null);
    this.nextChore = new Chore.Precondition.Context?(next);
    next.chore.Reserve(this.driver);
  }

  public void StartNextChore()
  {
    if (!this.nextChore.HasValue)
      return;
    this.driver.SetChore(this.nextChore.Value);
    this.nextChore = new Chore.Precondition.Context?();
  }

  public bool HasChoreQueued() => this.nextChore.HasValue;

  public RemoteWorkerDock HomeDepot
  {
    get => this.homeDepot?.Get();
    set => this.homeDepot = new Ref<RemoteWorkerDock>(value);
  }

  public ChoreConsumerState ConsumerState => this.consumer.consumerState;

  public bool ActivelyControlled { get; set; }

  public bool ActivelyWorking { get; set; }

  public bool Available { get; set; }

  public bool RequiresMaintnence => this.power.IsLowPower;

  public void TickResources(float dt)
  {
    double num = (double) this.power.ApplyDeltaEnergy(-0.1f * dt);
    float amount_consumed;
    SimUtil.DiseaseInfo disease_info;
    float aggregate_temperature;
    this.storage.ConsumeAndGetDisease(GameTags.LubricatingOil, 0.0333333351f * dt, out amount_consumed, out disease_info, out aggregate_temperature);
    if ((double) amount_consumed <= 0.0)
      return;
    this.storage.AddElement(SimHashes.LiquidGunk, amount_consumed, aggregate_temperature, disease_info.idx, disease_info.count, true);
  }

  public GameObject FindStation()
  {
    return Components.ComplexFabricators.Count == 0 ? (GameObject) null : Components.ComplexFabricators[0].gameObject;
  }

  public bool HasHomeDepot() => !this.HomeDepot.IsNullOrDestroyed();

  public class StatesInstance : 
    GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.GameInstance
  {
    public StatesInstance(RemoteWorkerSM master)
      : base(master)
    {
      this.sm.homedock.Set((KMonoBehaviour) this.smi.master.HomeDepot, this.smi);
    }
  }

  public class States : 
    GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM>
  {
    public RemoteWorkerSM.States.ControlledStates controlled;
    public RemoteWorkerSM.States.UncontrolledStates uncontrolled;
    public RemoteWorkerSM.States.IncapacitatedStates incapacitated;
    public StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.TargetParameter homedock;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.uncontrolled;
      this.root.Update((Action<RemoteWorkerSM.StatesInstance, float>) ((smi, dt) => smi.GetComponent<Navigator>().UpdateProbe()), UpdateRate.SIM_4000ms);
      this.controlled.Enter((StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback) (smi => smi.master.Available = false)).EnterTransition(this.controlled.exit_dock, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock)).EnterTransition((GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State) this.controlled.working, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock))).Transition((GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State) this.uncontrolled, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasRemoteOperator))).Transition(this.incapacitated.lost, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot))).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot))).Update(new Action<RemoteWorkerSM.StatesInstance, float>(RemoteWorkerSM.States.TickResources));
      this.controlled.exit_dock.ToggleWork<RemoteWorkerDock.ExitableDock>(this.homedock, (GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State) this.controlled.working, (GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State) this.controlled.working, (Func<RemoteWorkerSM.StatesInstance, bool>) (_ => true));
      this.controlled.working.Enter((StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback) (smi => smi.master.ActivelyWorking = true)).Exit((StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback) (smi => smi.master.ActivelyWorking = false)).DefaultState(this.controlled.working.find_work);
      this.controlled.working.find_work.Enter((StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback) (smi =>
      {
        if (RemoteWorkerSM.States.HasChore(smi))
        {
          smi.GoTo((StateMachine.BaseState) this.controlled.working.do_work);
        }
        else
        {
          RemoteWorkerSM.States.SetNextChore(smi);
          smi.GoTo(RemoteWorkerSM.States.HasChore(smi) ? (StateMachine.BaseState) this.controlled.working.do_work : (StateMachine.BaseState) this.controlled.no_work);
        }
      }));
      this.controlled.working.do_work.Exit(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.ClearChore)).Transition(this.controlled.working.find_work, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChore)));
      this.controlled.no_work.Transition(this.controlled.working.do_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChore)).Transition(this.controlled.working.find_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasChoreQueued));
      this.uncontrolled.EnterTransition(this.uncontrolled.working.new_worker, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsNewWorker)).EnterTransition(this.uncontrolled.idle, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.And(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock), GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsNewWorker)))).EnterTransition(this.uncontrolled.approach_dock, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.And(GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsInsideDock)), GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.IsNewWorker)))).Transition(this.controlled.working.find_work, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasRemoteOperator)).Transition(this.incapacitated.lost, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot))).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot)));
      this.uncontrolled.approach_dock.Enter((StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback) (smi => smi.master.Available = true)).MoveTo<IApproachable>(this.homedock, this.uncontrolled.working.enter, this.incapacitated.lost);
      this.uncontrolled.working.Enter((StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback) (smi => smi.master.Available = false));
      this.uncontrolled.working.new_worker.Enter((StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback) (smi => smi.master.playNewWorker = false)).ToggleWork<RemoteWorkerDock.NewWorker>(this.homedock, this.uncontrolled.working.recharge, this.uncontrolled.working.recharge, (Func<RemoteWorkerSM.StatesInstance, bool>) (_ => true));
      this.uncontrolled.working.enter.ToggleWork<RemoteWorkerDock.EnterableDock>(this.homedock, this.uncontrolled.working.recharge, this.uncontrolled.idle, (Func<RemoteWorkerSM.StatesInstance, bool>) (_ => true));
      this.uncontrolled.working.recharge.ToggleWork<RemoteWorkerDock.WorkerRecharger>(this.homedock, this.uncontrolled.working.recharge_pst, this.uncontrolled.idle, (Func<RemoteWorkerSM.StatesInstance, bool>) (_ => true));
      this.uncontrolled.working.recharge_pst.OnAnimQueueComplete(this.uncontrolled.working.drain_gunk).ScheduleGoTo(1f, (StateMachine.BaseState) this.uncontrolled.working.drain_gunk);
      this.uncontrolled.working.drain_gunk.ToggleWork<RemoteWorkerDock.WorkerGunkRemover>(this.homedock, this.uncontrolled.working.drain_gunk_pst, this.uncontrolled.idle, (Func<RemoteWorkerSM.StatesInstance, bool>) (_ => true));
      this.uncontrolled.working.drain_gunk_pst.OnAnimQueueComplete(this.uncontrolled.working.fill_oil).ScheduleGoTo(1f, (StateMachine.BaseState) this.uncontrolled.working.fill_oil);
      this.uncontrolled.working.fill_oil.ToggleWork<RemoteWorkerDock.WorkerOilRefiller>(this.homedock, this.uncontrolled.working.fill_oil_pst, this.uncontrolled.idle, (Func<RemoteWorkerSM.StatesInstance, bool>) (_ => true));
      this.uncontrolled.working.fill_oil_pst.OnAnimQueueComplete(this.uncontrolled.idle).ScheduleGoTo(1f, (StateMachine.BaseState) this.uncontrolled.idle);
      this.uncontrolled.idle.Enter((StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback) (smi => smi.master.Available = true)).PlayAnim(RemoteWorkerConfig.IDLE_IN_DOCK_ANIM, KAnim.PlayMode.Loop).Transition(this.uncontrolled.working.recharge, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.And(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.RequiresMaintnence), new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.DockIsOperational)), UpdateRate.SIM_1000ms);
      this.incapacitated.lost.Enter((StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback) (smi =>
      {
        smi.Play("sos_pre");
        smi.Queue("sos_loop", KAnim.PlayMode.Loop);
        RemoteWorkerSM.States.ClearChore(smi);
      })).ToggleStatusItem(Db.Get().DuplicantStatusItems.UnreachableDock).Transition(this.incapacitated.lost_recovery, new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.CanReachDepot)).Transition(this.incapacitated.die, GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Not(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.Transition.ConditionCallback(RemoteWorkerSM.States.HasHomeDepot)));
      this.incapacitated.lost_recovery.PlayAnim("sos_pst").OnAnimQueueComplete((GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State) this.controlled);
      this.incapacitated.die.Enter(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.ClearChore)).PlayAnim("explode").OnAnimQueueComplete(this.incapacitated.explode).ToggleStatusItem(Db.Get().DuplicantStatusItems.NoHomeDock);
      this.incapacitated.explode.Enter(new StateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State.Callback(RemoteWorkerSM.States.Explode));
    }

    public static bool IsNewWorker(RemoteWorkerSM.StatesInstance smi) => smi.master.playNewWorker;

    public static void SetNextChore(RemoteWorkerSM.StatesInstance smi)
    {
      smi.master.StartNextChore();
    }

    public static void ClearChore(RemoteWorkerSM.StatesInstance smi)
    {
      smi.master.driver.StopChore();
    }

    public static bool HasChore(RemoteWorkerSM.StatesInstance smi) => smi.master.driver.HasChore();

    public static bool HasChoreQueued(RemoteWorkerSM.StatesInstance smi)
    {
      return smi.master.HasChoreQueued();
    }

    public static bool CanReachDepot(RemoteWorkerSM.StatesInstance smi)
    {
      int depotCell = RemoteWorkerSM.States.GetDepotCell(smi);
      return depotCell != Grid.InvalidCell && smi.master.GetComponent<Navigator>().CanReach(depotCell);
    }

    public static int GetDepotCell(RemoteWorkerSM.StatesInstance smi)
    {
      RemoteWorkerDock homeDepot = smi.master.HomeDepot;
      return (UnityEngine.Object) homeDepot == (UnityEngine.Object) null ? Grid.InvalidCell : Grid.PosToCell((KMonoBehaviour) homeDepot);
    }

    public static bool HasRemoteOperator(RemoteWorkerSM.StatesInstance smi)
    {
      return smi.master.ActivelyControlled;
    }

    public static bool RequiresMaintnence(RemoteWorkerSM.StatesInstance smi)
    {
      return smi.master.RequiresMaintnence;
    }

    public static bool DockIsOperational(RemoteWorkerSM.StatesInstance smi)
    {
      return (UnityEngine.Object) smi.master.HomeDepot != (UnityEngine.Object) null && smi.master.HomeDepot.IsOperational;
    }

    public static bool HasHomeDepot(RemoteWorkerSM.StatesInstance smi)
    {
      return RemoteWorkerSM.States.GetDepotCell(smi) != Grid.InvalidCell;
    }

    public static void StopWork(RemoteWorkerSM.StatesInstance smi)
    {
      if (!smi.master.driver.HasChore())
        return;
      smi.master.driver.StopChore();
    }

    public static bool IsInsideDock(RemoteWorkerSM.StatesInstance smi) => smi.master.Docked;

    public static void Explode(RemoteWorkerSM.StatesInstance smi)
    {
      Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, smi.master.transform.position, 0.0f);
      PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
      component.Element.substance.SpawnResource(Grid.CellToPosCCC(Grid.PosToCell(smi.master.gameObject), Grid.SceneLayer.Ore), 42f, component.Temperature, component.DiseaseIdx, component.DiseaseCount);
      Util.KDestroyGameObject(smi.master.gameObject);
    }

    public static void TickResources(RemoteWorkerSM.StatesInstance smi, float dt)
    {
      if ((double) dt <= 0.0)
        return;
      smi.master.TickResources(dt);
    }

    public class ControlledStates : 
      GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
    {
      public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State exit_dock;
      public RemoteWorkerSM.States.ControlledStates.WorkingStates working;
      public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State no_work;

      public class WorkingStates : 
        GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
      {
        public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State find_work;
        public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State do_work;
      }
    }

    public class UncontrolledStates : 
      GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
    {
      public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State approach_dock;
      public RemoteWorkerSM.States.UncontrolledStates.WorkingDockStates working;
      public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State idle;

      public class WorkingDockStates : 
        GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
      {
        public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State new_worker;
        public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State enter;
        public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State recharge;
        public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State recharge_pst;
        public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State drain_gunk;
        public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State drain_gunk_pst;
        public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State fill_oil;
        public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State fill_oil_pst;
      }
    }

    public class IncapacitatedStates : 
      GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State
    {
      public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State lost;
      public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State lost_recovery;
      public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State die;
      public GameStateMachine<RemoteWorkerSM.States, RemoteWorkerSM.StatesInstance, RemoteWorkerSM, object>.State explode;
    }
  }
}
