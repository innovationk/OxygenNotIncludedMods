// Decompiled with JetBrains decompiler
// Type: RanchedStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class RanchedStates : 
  GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>
{
  private RanchedStates.RanchStates ranch;
  private StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.TargetParameter ranchTarget;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.ranch;
    this.root.Exit("AbandonedRanchStation", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi =>
    {
      if (smi.Monitor.TargetRanchStation != null)
      {
        if (smi.Monitor.TargetRanchStation.IsCritterInQueue(smi.Monitor))
        {
          Debug.LogWarning((object) "Why are we exiting RanchedStates while in the queue?");
          smi.Monitor.TargetRanchStation.Abandon(smi.Monitor);
        }
        smi.Monitor.TargetRanchStation = (RanchStation.Instance) null;
      }
      smi.sm.ranchTarget.Set((KMonoBehaviour) null, smi);
    }));
    this.ranch.EnterTransition((GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State) this.ranch.Cheer, (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Transition.ConditionCallback) (smi => RanchedStates.IsCrittersTurn(smi))).EventHandler(GameHashes.RanchStationNoLongerAvailable, (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.GoTo((StateMachine.BaseState) null))).BehaviourComplete(GameTags.Creatures.WantsToGetRanched, true).Update((System.Action<RanchedStates.Instance, float>) ((smi, deltaSeconds) =>
    {
      RanchStation.Instance ranchStation = smi.GetRanchStation();
      if (ranchStation.IsNullOrDestroyed())
      {
        smi.StopSM("No more target ranch station.");
      }
      else
      {
        Option<CavityInfo> option = Option.Maybe<CavityInfo>(Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell((StateMachine.Instance) smi)));
        Option<CavityInfo> cavityInfo = ranchStation.GetCavityInfo();
        if (option.IsNone() || cavityInfo.IsNone())
        {
          smi.StopSM("No longer in any cavity.");
        }
        else
        {
          if (option.Unwrap() == cavityInfo.Unwrap())
            return;
          smi.StopSM("Critter is in a different cavity");
        }
      }
    })).EventHandler(GameHashes.RancherReadyAtRanchStation, (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.UpdateWaitingState())).Exit(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.ClearLayerOverride));
    RanchedStates.CheerStates cheer = this.ranch.Cheer;
    string name1 = (string) CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.TOOLTIP;
    StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay1 = new HashedString();
    StatusItemCategory category1 = main1;
    cheer.ToggleStatusItem(name1, tooltip1, render_overlay: render_overlay1, category: category1).Enter("FaceRancher", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.GetComponent<Facing>().Face(smi.GetRanchStation().transform.GetPosition()))).PlayAnim("excited_loop").OnAnimQueueComplete(this.ranch.Cheer.Pst).ScheduleGoTo((Func<RanchedStates.Instance, float>) (smi => smi.cheerAnimLength), (StateMachine.BaseState) this.ranch.Move);
    this.ranch.Cheer.Pst.ScheduleGoTo(0.2f, (StateMachine.BaseState) this.ranch.Move);
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state1 = this.ranch.Move.DefaultState(this.ranch.Move.MoveToRanch).Enter("Speedup", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().defaultSpeed = smi.OriginalSpeed * 1.25f));
    string name2 = (string) CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.NAME;
    string tooltip2 = (string) CREATURES.STATUSITEMS.EXCITED_TO_GET_RANCHED.TOOLTIP;
    StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay2 = new HashedString();
    StatusItemCategory category2 = main2;
    state1.ToggleStatusItem(name2, tooltip2, render_overlay: render_overlay2, category: category2).Exit("RestoreSpeed", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().defaultSpeed = smi.OriginalSpeed));
    this.ranch.Move.MoveToRanch.EnterTransition(this.ranch.Wait.WaitInLine, GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Not(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Transition.ConditionCallback(RanchedStates.IsCrittersTurn))).MoveTo(new Func<RanchedStates.Instance, int>(RanchedStates.GetRanchNavTarget), this.ranch.Wait.WaitInLine).Target(this.ranchTarget).EventTransition(GameHashes.CreatureArrivedAtRanchStation, this.ranch.Wait.WaitInLine, (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Transition.ConditionCallback) (smi => !RanchedStates.IsCrittersTurn(smi)));
    this.ranch.Wait.WaitInLine.EnterTransition(this.ranch.Ranching, new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.Transition.ConditionCallback(RanchedStates.IsCrittersTurn)).Enter((StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi => smi.EnterQueue())).EventTransition(GameHashes.DestinationReached, this.ranch.Wait.Waiting);
    this.ranch.Wait.Waiting.Face(this.ranchTarget).PlayAnim((Func<RanchedStates.Instance, string>) (smi => smi.def.StartWaitingAnim)).QueueAnim((Func<RanchedStates.Instance, string>) (smi => smi.def.WaitingAnim), true);
    this.ranch.Wait.DoneWaiting.PlayAnim((Func<RanchedStates.Instance, string>) (smi => smi.def.EndWaitingAnim)).OnAnimQueueComplete(this.ranch.Move.MoveToRanch);
    this.ranch.Ranching.Enter(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.GetOnTable)).Enter("SetCreatureAtRanchingStation", (StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback) (smi =>
    {
      smi.GetRanchStation().MessageCreatureArrived(smi);
      smi.AnimController.SetSceneLayer(Grid.SceneLayer.BuildingUse);
    })).EventTransition(GameHashes.RanchingComplete, this.ranch.Wavegoodbye).ToggleMainStatusItem((Func<RanchedStates.Instance, StatusItem>) (smi =>
    {
      RanchStation.Instance ranchStation = RanchedStates.GetRanchStation(smi);
      return ranchStation != null ? ranchStation.def.CreatureRanchingStatusItem : Db.Get().CreatureStatusItems.GettingRanched;
    }));
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state2 = this.ranch.Wavegoodbye.Enter(new StateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State.Callback(RanchedStates.ClearLayerOverride)).OnAnimQueueComplete(this.ranch.Runaway);
    string name3 = (string) CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.NAME;
    string tooltip3 = (string) CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.TOOLTIP;
    StatusItemCategory main3 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay3 = new HashedString();
    StatusItemCategory category3 = main3;
    state2.ToggleStatusItem(name3, tooltip3, render_overlay: render_overlay3, category: category3);
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State state3 = this.ranch.Runaway.MoveTo(new Func<RanchedStates.Instance, int>(RanchedStates.GetRunawayCell));
    string name4 = (string) CREATURES.STATUSITEMS.IDLE.NAME;
    string tooltip4 = (string) CREATURES.STATUSITEMS.IDLE.TOOLTIP;
    StatusItemCategory main4 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay4 = new HashedString();
    StatusItemCategory category4 = main4;
    state3.ToggleStatusItem(name4, tooltip4, render_overlay: render_overlay4, category: category4);
  }

  private static void ClearLayerOverride(RanchedStates.Instance smi)
  {
    smi.AnimController.SetSceneLayer(Grid.SceneLayer.Creatures);
  }

  private static RanchStation.Instance GetRanchStation(RanchedStates.Instance smi)
  {
    return smi.GetRanchStation();
  }

  private static void GetOnTable(RanchedStates.Instance smi)
  {
    Navigator navigator = smi.Get<Navigator>();
    if (navigator.IsValidNavType(NavType.Floor))
      navigator.SetCurrentNavType(NavType.Floor);
    smi.Get<Facing>().SetFacing(false);
  }

  private static bool IsCrittersTurn(RanchedStates.Instance smi)
  {
    RanchStation.Instance ranchStation = RanchedStates.GetRanchStation(smi);
    return ranchStation != null && ranchStation.IsRancherReady && ranchStation.TryGetRanched(smi);
  }

  private static int GetRanchNavTarget(RanchedStates.Instance smi)
  {
    RanchStation.Instance ranchStation = RanchedStates.GetRanchStation(smi);
    int cell1 = smi.ModifyNavTargetForCritter(ranchStation.GetRanchNavTarget());
    if (smi.HasTag(GameTags.LargeCreature))
    {
      if (Grid.PosToXY(smi.gameObject.transform.position).x > Grid.CellToXY(cell1).x)
      {
        int cell2 = Grid.CellLeft(cell1);
        if (smi.GetComponent<Navigator>().GetNavigationCost(cell2) != -1)
          cell1 = cell2;
      }
    }
    else if (smi.HasTag(GameTags.Creatures.Swimmer))
      cell1 = Grid.CellRight(cell1);
    return cell1;
  }

  private static int GetRunawayCell(RanchedStates.Instance smi)
  {
    int cell = Grid.PosToCell(smi.transform.GetPosition());
    int i = Grid.OffsetCell(cell, 2, 0);
    if (Grid.Solid[i])
      i = Grid.OffsetCell(cell, -2, 0);
    return i;
  }

  public class Def : StateMachine.BaseDef
  {
    public string StartWaitingAnim = "queue_pre";
    public string WaitingAnim = "queue_loop";
    public string EndWaitingAnim = "queue_pst";
    public int WaitCellOffset = 1;
  }

  public new class Instance : 
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.GameInstance
  {
    public float OriginalSpeed;
    private int waitCell;
    private KBatchedAnimController animController;
    private RanchableMonitor.Instance ranchMonitor;
    public float cheerAnimLength;

    public RanchableMonitor.Instance Monitor
    {
      get
      {
        if (this.ranchMonitor == null)
          this.ranchMonitor = this.GetSMI<RanchableMonitor.Instance>();
        return this.ranchMonitor;
      }
    }

    public KBatchedAnimController AnimController => this.animController;

    public Instance(Chore<RanchedStates.Instance> chore, RanchedStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      this.animController = this.GetComponent<KBatchedAnimController>();
      this.OriginalSpeed = this.Monitor.NavComponent.defaultSpeed;
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToGetRanched);
      KAnim.Anim anim = this.animController.GetAnim(new HashedString("excited_loop"));
      this.cheerAnimLength = anim != null ? anim.totalTime + 0.2f : 1.2f;
    }

    public RanchStation.Instance GetRanchStation()
    {
      return this.Monitor != null ? this.Monitor.TargetRanchStation : (RanchStation.Instance) null;
    }

    public void EnterQueue()
    {
      if (this.GetRanchStation() == null)
        return;
      this.InitializeWaitCell();
      this.Monitor.NavComponent.GoTo(this.waitCell);
    }

    public void AbandonRanchStation()
    {
      if (this.Monitor.TargetRanchStation == null || this.status == StateMachine.Status.Failed)
        return;
      this.StopSM("Abandoned Ranch");
    }

    public void SetRanchStation(RanchStation.Instance ranch_station)
    {
      if (this.Monitor.TargetRanchStation != null && this.Monitor.TargetRanchStation != ranch_station)
        this.Monitor.TargetRanchStation.Abandon(this.smi.Monitor);
      this.smi.sm.ranchTarget.Set(ranch_station.gameObject, this.smi, false);
      this.Monitor.TargetRanchStation = ranch_station;
    }

    public int ModifyNavTargetForCritter(int navCell)
    {
      return this.smi.HasTag(GameTags.Creatures.Flyer) ? Grid.CellAbove(navCell) : navCell;
    }

    private void InitializeWaitCell()
    {
      if (this.GetRanchStation() == null)
        return;
      int cell1 = 0;
      Extents stationExtents = this.Monitor.TargetRanchStation.StationExtents;
      int cell2 = this.ModifyNavTargetForCritter(Grid.XYToCell(stationExtents.x, stationExtents.y));
      int num1 = 0;
      int hitDistance1;
      if (Grid.Raycast(cell2, new Vector2I(-1, 0), out hitDistance1, this.def.WaitCellOffset, Grid.BuildFlags.FakeFloor | Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.CritterImpassable))
      {
        num1 = 1 + this.def.WaitCellOffset - hitDistance1;
        cell1 = this.ModifyNavTargetForCritter(Grid.XYToCell(stationExtents.x + 1, stationExtents.y));
      }
      int num2 = 0;
      int hitDistance2;
      if (num1 != 0 && Grid.Raycast(cell1, new Vector2I(1, 0), out hitDistance2, this.def.WaitCellOffset, Grid.BuildFlags.FakeFloor | Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.CritterImpassable))
        num2 = this.def.WaitCellOffset - hitDistance2;
      int x = (this.def.WaitCellOffset - num1) * -1;
      if (num1 == this.def.WaitCellOffset)
        x = 1 + this.def.WaitCellOffset - num2;
      CellOffset offset = new CellOffset(x, 0);
      this.waitCell = Grid.OffsetCell(cell2, offset);
    }

    public void UpdateWaitingState()
    {
      if (RanchedStates.IsCrittersTurn(this.smi))
      {
        if (this.smi.IsInsideState((StateMachine.BaseState) this.sm.ranch.Wait.Waiting))
          this.smi.GoTo((StateMachine.BaseState) this.smi.sm.ranch.Wait.DoneWaiting);
        else
          this.smi.GoTo((StateMachine.BaseState) this.smi.sm.ranch.Cheer);
      }
      else
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.ranch.Wait.WaitInLine);
    }
  }

  public class RanchStates : 
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
  {
    public RanchedStates.CheerStates Cheer;
    public RanchedStates.MoveStates Move;
    public RanchedStates.WaitStates Wait;
    public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Ranching;
    public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Wavegoodbye;
    public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Runaway;
  }

  public class CheerStates : 
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
  {
    public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Cheer;
    public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Pst;
  }

  public class MoveStates : 
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
  {
    public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State MoveToRanch;
  }

  public class WaitStates : 
    GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State
  {
    public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State WaitInLine;
    public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State Waiting;
    public GameStateMachine<RanchedStates, RanchedStates.Instance, IStateMachineTarget, RanchedStates.Def>.State DoneWaiting;
  }
}
