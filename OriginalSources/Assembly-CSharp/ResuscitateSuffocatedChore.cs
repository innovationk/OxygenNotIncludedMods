// Decompiled with JetBrains decompiler
// Type: ResuscitateSuffocatedChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class ResuscitateSuffocatedChore : Chore<ResuscitateSuffocatedChore.StatesInstance>
{
  public static Chore.Precondition CanReachIncapacitated = new Chore.Precondition()
  {
    id = nameof (CanReachIncapacitated),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      GameObject gameObject = (GameObject) data;
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        return false;
      int navigationCost = context.consumerState.navigator.GetNavigationCost(Grid.PosToCell(gameObject.transform.GetPosition()));
      if (-1 == navigationCost)
        return false;
      context.cost += navigationCost;
      return true;
    })
  };
  public static Chore.Precondition CanReachOxygenatedArea = new Chore.Precondition()
  {
    id = nameof (CanReachOxygenatedArea),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if (context.chore.InProgress())
        return true;
      GameObject go = (GameObject) data;
      if ((UnityEngine.Object) go == (UnityEngine.Object) null)
        return false;
      Navigator navigator = context.consumerState.navigator;
      int cellToIncapacitated = ResuscitateSuffocatedChore.StatesInstance.FindClosestOxygenCellToIncapacitated(navigator, Grid.PosToCell(go));
      if (!Grid.IsValidCell(cellToIncapacitated))
        return false;
      int navigationCost = navigator.GetNavigationCost(cellToIncapacitated);
      if (-1 == navigationCost)
        return false;
      context.cost += navigationCost;
      return true;
    })
  };

  public ResuscitateSuffocatedChore(IStateMachineTarget master, GameObject incapacitatedDuplicant)
    : base(Db.Get().ChoreTypes.RescueIncapacitated, master, (ChoreProvider) null, false, master_priority_class: PriorityScreen.PriorityClass.personalNeeds)
  {
    this.smi = new ResuscitateSuffocatedChore.StatesInstance(this);
    this.runUntilComplete = true;
    this.AddPrecondition(ChorePreconditions.instance.NotChoreCreator, (object) incapacitatedDuplicant.gameObject);
    this.AddPrecondition(ChorePreconditions.instance.IsNotARobot, (object) null);
    this.AddPrecondition(ResuscitateSuffocatedChore.CanReachIncapacitated, (object) incapacitatedDuplicant);
    this.AddPrecondition(ResuscitateSuffocatedChore.CanReachOxygenatedArea, (object) incapacitatedDuplicant);
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    this.smi.sm.rescuer.Set(context.consumerState.gameObject, this.smi, false);
    this.smi.sm.rescueTarget.Set(this.gameObject, this.smi, false);
    this.smi.resucerController = context.consumerState.gameObject.GetComponent<KBatchedAnimController>();
    this.smi.rescueeController = this.gameObject.GetComponent<KBatchedAnimController>();
    this.smi.sm.deliverTarget.Set(ChoreHelpers.CreateLocator("OxygenCell", Grid.CellToPosCBC(ResuscitateSuffocatedChore.StatesInstance.FindClosestOxygenCellToIncapacitated(context.consumerState.navigator, Grid.PosToCell(this.gameObject)), Grid.SceneLayer.Move)), this.smi, false);
    base.Begin(context);
  }

  protected override void End(string reason)
  {
    this.DropIncapacitatedDuplicant();
    base.End(reason);
  }

  private void DropIncapacitatedDuplicant()
  {
    if (!((UnityEngine.Object) this.smi.sm.rescuer.Get(this.smi) != (UnityEngine.Object) null) || !((UnityEngine.Object) this.smi.sm.rescueTarget.Get(this.smi) != (UnityEngine.Object) null))
      return;
    Storage component = this.smi.sm.rescuer.Get(this.smi).GetComponent<Storage>();
    GameObject go = this.smi.sm.rescueTarget.Get(this.smi);
    if (!component.items.Contains(go))
      return;
    this.smi.sm.rescuer.Get(this.smi).GetComponent<Storage>().Drop(go, true);
  }

  public class StatesInstance(ResuscitateSuffocatedChore master) : 
    GameStateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.GameInstance(master)
  {
    public KBatchedAnimController resucerController;
    public KBatchedAnimController rescueeController;
    public static SafeResuscitateCellQuery ResuscitateCellQuery = new SafeResuscitateCellQuery();

    public static int FindClosestOxygenCellToIncapacitated(Navigator navigator, int start_cell)
    {
      if ((UnityEngine.Object) navigator == (UnityEngine.Object) null)
        return Grid.InvalidCell;
      OxygenBreather component = navigator.GetComponent<OxygenBreather>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      {
        Debug.Assert(false, (object) "How is a non- oxygen breathing attempting to resuscitate?");
        return Grid.InvalidCell;
      }
      SafeResuscitateCellQuery query = ResuscitateSuffocatedChore.StatesInstance.ResuscitateCellQuery.Reset(component);
      PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(start_cell, NavType.Floor, PathFinder.PotentialPath.Flags.None);
      PathFinder.Run(navigator.NavGrid, navigator.GetCurrentAbilities(), potential_path, (PathFinderQuery) query);
      return query.GetResultCell();
    }
  }

  public class States : 
    GameStateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore>
  {
    public GameStateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.ApproachSubState<IApproachable> approachSuffocated;
    public GameStateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State failure;
    public GameStateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State success;
    public ResuscitateSuffocatedChore.States.HoldingSuffocated holding;
    public ResuscitateSuffocatedChore.States.Resuscitate resuscitate;
    public StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.TargetParameter rescueTarget;
    public StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.TargetParameter deliverTarget;
    public StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.TargetParameter rescuer;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.approachSuffocated;
      this.root.Enter((StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State.Callback) (smi => smi.sm.rescueTarget.Get(smi).Subscribe(1623392196, (Action<object>) (d => smi.GoTo((StateMachine.BaseState) this.holding.ditch))))).Exit((StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State.Callback) (smi =>
      {
        ResuscitateSuffocatedChore.States.SyncControllers(smi, false);
        smi.sm.rescueTarget.Get(smi).Unsubscribe(1623392196);
      }));
      this.approachSuffocated.InitializeStates(this.rescuer, this.rescueTarget, this.holding.pickup, this.failure, Grid.DefaultOffset);
      this.holding.pickup.Target(this.rescuer).Enter((StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State.Callback) (smi =>
      {
        this.rescuer.Get(smi).GetComponent<Storage>().Store(this.rescueTarget.Get(smi));
        this.rescueTarget.Get(smi).transform.SetLocalPosition(Vector3.zero);
        KBatchedAnimTracker component = this.rescueTarget.Get(smi).GetComponent<KBatchedAnimTracker>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          component.symbol = new HashedString("snapTo_pivot");
          component.offset = new Vector3(0.0f, 0.0f, 1f);
        }
        ResuscitateSuffocatedChore.States.PlayResuscitateAnim(smi, "pickup");
      })).EventTransition(GameHashes.AnimQueueComplete, (GameStateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State) this.holding.delivering).EventHandler(GameHashes.BeginChore, (StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State.Callback) (smi =>
      {
        Storage storage = this.rescuer.Get<Storage>(smi);
        if (!((UnityEngine.Object) storage.Find(this.rescueTarget.Get<KPrefabID>(smi).GetHashCode()) == (UnityEngine.Object) null))
          return;
        storage.Store(this.rescueTarget.Get(smi));
        this.rescueTarget.Get(smi).transform.SetLocalPosition(Vector3.zero);
      }));
      this.holding.delivering.InitializeStates(this.rescuer, this.deliverTarget, this.resuscitate.pre, this.holding.ditch).Enter((StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State.Callback) (smi => smi.rescueeController.Play((HashedString) "carry_loop", KAnim.PlayMode.Loop))).Update((Action<ResuscitateSuffocatedChore.StatesInstance, float>) ((smi, dt) =>
      {
        if (!((UnityEngine.Object) this.deliverTarget.Get(smi) == (UnityEngine.Object) null))
          return;
        smi.GoTo((StateMachine.BaseState) this.holding.ditch);
      }));
      this.resuscitate.Enter((StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State.Callback) (smi =>
      {
        GameObject gameObject = this.rescuer.Get(smi).gameObject;
        if (gameObject.IsNullOrDestroyed())
          return;
        KAnimFile anim1 = Assets.GetAnim((HashedString) "anim_resuscitate_kanim");
        gameObject.GetComponent<KAnimControllerBase>().AddAnimOverrides(anim1);
        KAnimFile anim2 = Assets.GetAnim((HashedString) "anim_drowning_kanim");
        KAnimControllerBase component1 = smi.master.GetComponent<KAnimControllerBase>();
        component1.AddAnimOverrides(anim2);
        KBatchedAnimTracker component2 = this.rescueTarget.Get(smi).GetComponent<KBatchedAnimTracker>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          component2.symbol = new HashedString("snapTo_pivot");
          component2.offset = new Vector3(0.0f, 0.0f, 1f);
        }
        component1.gameObject.transform.SetLocalPosition(new Vector3(0.0f, 0.0f, 1f));
      })).Exit((StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State.Callback) (smi =>
      {
        GameObject gameObject = this.rescuer.Get(smi).gameObject;
        if (gameObject.IsNullOrDestroyed())
          return;
        KAnimFile anim3 = Assets.GetAnim((HashedString) "anim_resuscitate_kanim");
        gameObject.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(anim3);
        KAnimFile anim4 = Assets.GetAnim((HashedString) "anim_drowning_kanim");
        smi.master.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(anim4);
      }));
      this.resuscitate.pre.Target(this.rescuer).Enter((StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State.Callback) (smi =>
      {
        ResuscitateSuffocatedChore.States.SyncControllers(smi, true);
        ResuscitateSuffocatedChore.States.PlayResuscitateAnim(smi, "resuscitate_pre", true);
      })).OnAnimQueueComplete(this.resuscitate.loop);
      this.resuscitate.loop.Target(this.rescuer).Enter((StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State.Callback) (smi => ResuscitateSuffocatedChore.States.PlayResuscitateAnim(smi, "resuscitate_loop", true))).OnAnimQueueComplete(this.resuscitate.pst);
      this.resuscitate.pst.Target(this.rescuer).Enter((StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State.Callback) (smi => ResuscitateSuffocatedChore.States.PlayResuscitateAnim(smi, "resuscitate_pst", true))).OnAnimQueueComplete(this.success);
      this.holding.ditch.PlayAnim("place").ScheduleGoTo(0.5f, (StateMachine.BaseState) this.failure).Exit((StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State.Callback) (smi => smi.master.DropIncapacitatedDuplicant()));
      this.failure.ReturnFailure();
      this.success.Enter((StateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State.Callback) (smi =>
      {
        AmountInstance amountInstance = Db.Get().Amounts.Breath.Lookup(smi.gameObject);
        double num = (double) amountInstance.SetValue(amountInstance.GetMax());
        smi.Trigger(-1256572400);
        smi.GetSMI<IncapacitationMonitor.Instance>().ApplyRecoverEffect();
      })).ReturnSuccess();
    }

    public static void PlayResuscitateAnim(
      ResuscitateSuffocatedChore.StatesInstance smi,
      string anim_name,
      bool synced = false)
    {
      smi.resucerController.Play((HashedString) anim_name);
      if (synced)
        return;
      smi.rescueeController.Play((HashedString) anim_name);
    }

    public static void SyncControllers(ResuscitateSuffocatedChore.StatesInstance smi, bool sync)
    {
      KAnimSynchronizer synchronizer = smi.resucerController.GetSynchronizer();
      if (synchronizer == null)
        return;
      if (sync)
        synchronizer.Add((KAnimControllerBase) smi.rescueeController);
      else if (smi.rescueeController.HasTag(GameTags.Dead))
        synchronizer.RemoveWithoutIdleAnim((KAnimControllerBase) smi.rescueeController);
      else
        synchronizer.Remove((KAnimControllerBase) smi.rescueeController);
    }

    public class HoldingSuffocated : 
      GameStateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State
    {
      public GameStateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State pickup;
      public GameStateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.ApproachSubState<IApproachable> delivering;
      public GameStateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State ditch;
    }

    public class Resuscitate : 
      GameStateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State
    {
      public GameStateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State pre;
      public GameStateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State loop;
      public GameStateMachine<ResuscitateSuffocatedChore.States, ResuscitateSuffocatedChore.StatesInstance, ResuscitateSuffocatedChore, object>.State pst;
    }
  }
}
