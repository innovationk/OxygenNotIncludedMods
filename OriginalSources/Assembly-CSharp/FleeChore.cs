// Decompiled with JetBrains decompiler
// Type: FleeChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class FleeChore : Chore<FleeChore.StatesInstance>
{
  private Navigator nav;

  public FleeChore(IStateMachineTarget target, GameObject enemy)
    : base(Db.Get().ChoreTypes.Flee, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.compulsory)
  {
    this.smi = new FleeChore.StatesInstance(this);
    this.smi.sm.self.Set(this.gameObject, this.smi, false);
    this.nav = this.gameObject.GetComponent<Navigator>();
    this.smi.sm.fleeFromTarget.Set(enemy, this.smi, false);
  }

  private bool isInFavoredDirection(int cell, int fleeFromCell)
  {
    return ((double) Grid.CellToPos(fleeFromCell).x < (double) this.gameObject.transform.GetPosition().x ? 1 : 0) == ((double) Grid.CellToPos(fleeFromCell).x < (double) Grid.CellToPos(cell).x ? (true ? 1 : 0) : (false ? 1 : 0));
  }

  private bool CanFleeTo(int cell)
  {
    return this.nav.CanReach(cell) || this.nav.CanReach(Grid.OffsetCell(cell, -1, -1)) || this.nav.CanReach(Grid.OffsetCell(cell, 1, -1)) || this.nav.CanReach(Grid.OffsetCell(cell, -1, 1)) || this.nav.CanReach(Grid.OffsetCell(cell, 1, 1));
  }

  public GameObject CreateLocator(Vector3 pos) => ChoreHelpers.CreateLocator("GoToLocator", pos);

  protected override void OnStateMachineStop(string reason, StateMachine.Status status)
  {
    if ((UnityEngine.Object) this.smi.sm.fleeToTarget.Get(this.smi) != (UnityEngine.Object) null)
      ChoreHelpers.DestroyLocator(this.smi.sm.fleeToTarget.Get(this.smi));
    base.OnStateMachineStop(reason, status);
  }

  public class StatesInstance(FleeChore master) : 
    GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.GameInstance(master)
  {
  }

  public class States : GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore>
  {
    public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter fleeFromTarget;
    public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter fleeToTarget;
    public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter self;
    public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State planFleeRoute;
    public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.ApproachSubState<IApproachable> flee;
    public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State cower;
    public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State end;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.planFleeRoute;
      this.root.ToggleStatusItem(Db.Get().DuplicantStatusItems.Fleeing, (Func<FleeChore.StatesInstance, object>) null).ToggleNotification((Func<FleeChore.StatesInstance, Notification>) (smi => new Notification(Db.Get().DuplicantStatusItems.Fleeing.notificationText, Db.Get().DuplicantStatusItems.Fleeing.notificationType, click_focus: smi.master.gameObject.transform)));
      this.planFleeRoute.Enter((StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State.Callback) (smi =>
      {
        int fleeFromCell = Grid.PosToCell(this.fleeFromTarget.Get(smi));
        int best = FloodFill.FindBest(new Func<int, float>(RateCell), new Func<int, FloodFill.BoundaryCheckResult>(BoundaryCondition), Grid.PosToCell(smi.master.gameObject), 300);
        if (best != -1)
        {
          smi.sm.fleeToTarget.Set(smi.master.CreateLocator(Grid.CellToPos(best)), smi, false);
          smi.sm.fleeToTarget.Get(smi).name = "FleeLocator";
          if (best == fleeFromCell)
            smi.GoTo((StateMachine.BaseState) this.cower);
          else
            smi.GoTo((StateMachine.BaseState) this.flee);
        }
        else
          smi.GoTo((StateMachine.BaseState) this.cower);

        float RateCell(int cell)
        {
          int num1 = -1;
          if (!smi.master.nav.CanReach(cell))
            return (float) num1;
          int num2 = num1 + Grid.GetCellDistance(cell, fleeFromCell);
          if (smi.master.isInFavoredDirection(cell, fleeFromCell))
            num2 += 8;
          return (float) num2;
        }

        FloodFill.BoundaryCheckResult BoundaryCondition(int cell)
        {
          return !smi.master.CanFleeTo(cell) ? FloodFill.BoundaryCheckResult.Halt : FloodFill.BoundaryCheckResult.Continue;
        }
      }));
      this.flee.InitializeStates(this.self, this.fleeToTarget, this.cower, this.cower, tactic: NavigationTactics.ReduceTravelDistance).ToggleAnims("anim_loco_run_insane_kanim", 2f);
      this.cower.ToggleAnims("anim_cringe_kanim", 4f).PlayAnim("cringe_pre").QueueAnim("cringe_loop").QueueAnim("cringe_pst").OnAnimQueueComplete(this.end);
      this.end.Enter((StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State.Callback) (smi => smi.StopSM("stopped")));
    }
  }
}
