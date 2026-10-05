// Decompiled with JetBrains decompiler
// Type: IdleChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class IdleChore : Chore<IdleChore.StatesInstance>
{
  public IdleChore(IStateMachineTarget target)
    : base(Db.Get().ChoreTypes.Idle, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.idle, report_type: ReportManager.ReportType.IdleTime)
  {
    this.showAvailabilityInHoverText = false;
    this.smi = new IdleChore.StatesInstance(this, target.gameObject);
  }

  public class StatesInstance : 
    GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.GameInstance
  {
    private IdleCellSensor idleCellSensor;

    public Navigator navigator { get; private set; }

    public KBatchedAnimController animController { get; private set; }

    public StatesInstance(IdleChore master, GameObject idler)
      : base(master)
    {
      this.sm.idler.Set(idler, this.smi, false);
      this.navigator = this.GetComponent<Navigator>();
      this.animController = this.GetComponent<KBatchedAnimController>();
      this.idleCellSensor = this.GetComponent<Sensors>().GetSensor<IdleCellSensor>();
    }

    public int GetIdleCell() => this.idleCellSensor.GetCell();

    public bool HasIdleCell() => this.idleCellSensor.GetCell() != Grid.InvalidCell;
  }

  public class States : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore>
  {
    public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnLadder;
    public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnTube;
    public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnSuitMarkerCell;
    public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isHovering;
    public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isSwimming;
    public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.TargetParameter idler;
    public IdleChore.States.IdleState idle;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.Target(this.idler);
      this.idle.DefaultState(this.idle.onfloor).Enter("UpdateNavType", (StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State.Callback) (smi => IdleChore.States.UpdateNavType(smi))).Update("UpdateNavType", (Action<IdleChore.StatesInstance, float>) ((smi, dt) => IdleChore.States.UpdateNavType(smi))).ToggleStateMachine((Func<IdleChore.StatesInstance, StateMachine.Instance>) (smi => (StateMachine.Instance) new TaskAvailabilityMonitor.Instance((IStateMachineTarget) smi.master))).ToggleTag(GameTags.Idle);
      this.idle.onfloor.PlayAnim("idle_default", KAnim.PlayMode.Loop).ParamTransition<bool>((StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.Parameter<bool>) this.isOnLadder, this.idle.onladder, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ParamTransition<bool>((StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.Parameter<bool>) this.isOnTube, this.idle.ontube, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ParamTransition<bool>((StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.Parameter<bool>) this.isOnSuitMarkerCell, this.idle.onsuitmarker, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ParamTransition<bool>((StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.Parameter<bool>) this.isHovering, this.idle.hovering, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ParamTransition<bool>((StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.Parameter<bool>) this.isSwimming, this.idle.swimming, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ToggleScheduleCallback("IdleMove", (Func<IdleChore.StatesInstance, float>) (smi => (float) UnityEngine.Random.Range(5, 15)), (Action<IdleChore.StatesInstance>) (smi => smi.GoTo((StateMachine.BaseState) this.idle.move)));
      this.idle.onladder.PlayAnim("ladder_idle", KAnim.PlayMode.Loop).ToggleScheduleCallback("IdleMove", (Func<IdleChore.StatesInstance, float>) (smi => (float) UnityEngine.Random.Range(5, 15)), (Action<IdleChore.StatesInstance>) (smi => smi.GoTo((StateMachine.BaseState) this.idle.move)));
      this.idle.ontube.PlayAnim("tube_idle_loop", KAnim.PlayMode.Loop).Update("IdleMove", (Action<IdleChore.StatesInstance, float>) ((smi, dt) =>
      {
        if (!smi.HasIdleCell())
          return;
        smi.GoTo((StateMachine.BaseState) this.idle.move);
      }), UpdateRate.SIM_1000ms);
      this.idle.hovering.PlayAnim("hover_idle", KAnim.PlayMode.Loop).Update("IdleMove", (Action<IdleChore.StatesInstance, float>) ((smi, dt) =>
      {
        if (!smi.HasIdleCell())
          return;
        smi.GoTo((StateMachine.BaseState) this.idle.move);
      }), UpdateRate.SIM_1000ms);
      this.idle.swimming.Enter("swimming", (StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State.Callback) (smi =>
      {
        string anim_name = "treading_loop";
        int cell = Grid.CellBelow(smi.navigator.cachedCell);
        if (!Grid.IsSubstantialLiquid(smi.navigator.cachedCell) && Grid.IsValidCell(cell) && Grid.IsLiquid(cell))
          anim_name = "shallow_treading_loop";
        smi.animController.Play((HashedString) anim_name, KAnim.PlayMode.Loop);
      })).Update("IdleMove", (Action<IdleChore.StatesInstance, float>) ((smi, dt) =>
      {
        if (!smi.HasIdleCell())
          return;
        smi.GoTo((StateMachine.BaseState) this.idle.move);
      }), UpdateRate.SIM_1000ms);
      this.idle.onsuitmarker.PlayAnim("idle_default", KAnim.PlayMode.Loop).Enter((StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State.Callback) (smi =>
      {
        int cell = Grid.PosToCell((StateMachine.Instance) smi);
        Grid.SuitMarker.Flags flags;
        Grid.TryGetSuitMarkerFlags(cell, out flags, out PathFinder.PotentialPath.Flags _);
        IdleSuitMarkerCellQuery query = new IdleSuitMarkerCellQuery((flags & Grid.SuitMarker.Flags.Rotated) != 0, Grid.CellToXY(cell).X);
        smi.navigator.RunQuery((PathFinderQuery) query);
        smi.navigator.GoTo(query.GetResultCell());
      })).EventTransition(GameHashes.DestinationReached, (GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State) this.idle).ToggleScheduleCallback("IdleMove", (Func<IdleChore.StatesInstance, float>) (smi => (float) UnityEngine.Random.Range(5, 15)), (Action<IdleChore.StatesInstance>) (smi => smi.GoTo((StateMachine.BaseState) this.idle.move)));
      this.idle.move.Transition((GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State) this.idle, (StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.Transition.ConditionCallback) (smi => !smi.HasIdleCell())).TriggerOnEnter(GameHashes.BeginWalk).TriggerOnExit(GameHashes.EndWalk).ToggleAnims("anim_loco_walk_kanim").MoveTo((Func<IdleChore.StatesInstance, int>) (smi => smi.GetIdleCell()), (GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State) this.idle, (GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State) this.idle).Exit("UpdateNavType", (StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State.Callback) (smi => IdleChore.States.UpdateNavType(smi))).Exit("ClearWalk", (StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State.Callback) (smi => smi.animController.Play((HashedString) "idle_default")));
    }

    public static void UpdateNavType(IdleChore.StatesInstance smi)
    {
      NavType currentNavType = smi.navigator.CurrentNavType;
      smi.sm.isOnLadder.Set(currentNavType == NavType.Ladder || currentNavType == NavType.Pole, smi);
      smi.sm.isOnTube.Set(currentNavType == NavType.Tube, smi);
      smi.sm.isHovering.Set(currentNavType == NavType.Hover, smi);
      smi.sm.isSwimming.Set(currentNavType == NavType.Swim, smi);
      int cell = Grid.PosToCell((StateMachine.Instance) smi);
      smi.sm.isOnSuitMarkerCell.Set(Grid.IsValidCell(cell) && Grid.HasSuitMarker[cell], smi);
    }

    public class IdleState : 
      GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State
    {
      public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onfloor;
      public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onladder;
      public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State ontube;
      public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onsuitmarker;
      public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State hovering;
      public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State swimming;
      public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State move;
    }
  }
}
