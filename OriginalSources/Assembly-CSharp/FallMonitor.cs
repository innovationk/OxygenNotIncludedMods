// Decompiled with JetBrains decompiler
// Type: FallMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FallMonitor : GameStateMachine<FallMonitor, FallMonitor.Instance>
{
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State standing;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State falling_pre;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State falling;
  public FallMonitor.EntombedStates entombed;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverladder;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverpole;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverswim;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverinitialfall;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State landfloor;
  public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State instorage;
  public StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.BoolParameter isEntombed;
  public StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.BoolParameter isFalling;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.standing;
    this.root.TagTransition(GameTags.Stored, this.instorage).Update("CheckLanded", (System.Action<FallMonitor.Instance, float>) ((smi, dt) => smi.UpdateFalling()), UpdateRate.SIM_33ms, true);
    this.standing.ParamTransition<bool>((StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isEntombed, (GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State) this.entombed, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue).ParamTransition<bool>((StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isFalling, this.falling_pre, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue);
    this.falling_pre.Enter("StopNavigator", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.GetComponent<Navigator>().Stop())).Enter("AttemptInitialRecovery", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.AttemptInitialRecovery())).GoTo(this.falling).ToggleBrain("falling_pre");
    this.falling.ToggleBrain("falling").PlayAnim("fall_pre").QueueAnim("fall_loop", true).ParamTransition<bool>((StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isEntombed, (GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State) this.entombed, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue).Transition(this.recoverladder, (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.CanRecoverToLadder()), UpdateRate.SIM_33ms).Transition(this.recoverpole, (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.CanRecoverToPole()), UpdateRate.SIM_33ms).Transition(this.recoverswim, (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.CanRecoverToSwim()), UpdateRate.SIM_33ms).ToggleGravity(this.landfloor);
    this.recoverinitialfall.ToggleBrain("recoverinitialfall").Enter("Recover", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.Recover())).EventTransition(GameHashes.DestinationReached, this.standing).EventTransition(GameHashes.NavigationFailed, this.standing).Exit((StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.RecoverEmote()));
    this.landfloor.Enter("Land", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.LandFloor())).GoTo(this.standing);
    this.recoverladder.ToggleBrain("recoverladder").PlayAnim("floor_ladder_0_0").Enter("MountLadder", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.MountLadder())).OnAnimQueueComplete(this.standing);
    this.recoverpole.ToggleBrain("recoverpole").PlayAnim("floor_pole_0_0").Enter("MountPole", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.MountPole())).OnAnimQueueComplete(this.standing);
    this.recoverswim.ToggleBrain("recoverswim").PlayAnim("treading_loop").Enter("Swim", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.Swim())).OnAnimQueueComplete(this.standing);
    this.instorage.TagTransition(GameTags.Stored, this.standing, true);
    this.entombed.DefaultState(this.entombed.recovering);
    this.entombed.recovering.Enter("TryEntombedEscape", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.TryEntombedEscape()));
    this.entombed.stuck.Enter("StopNavigator", (StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.GetComponent<Navigator>().Stop())).ToggleChore((Func<FallMonitor.Instance, Chore>) (smi => (Chore) new EntombedChore(smi.master, smi.entombedAnimOverride)), this.standing).ParamTransition<bool>((StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>) this.isEntombed, this.standing, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsFalse);
  }

  public class EntombedStates : 
    GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recovering;
    public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State stuck;
  }

  public new class Instance : 
    GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    private CellOffset[] entombedEscapeOffsets = new CellOffset[7]
    {
      new CellOffset(0, 1),
      new CellOffset(1, 0),
      new CellOffset(-1, 0),
      new CellOffset(1, 1),
      new CellOffset(-1, 1),
      new CellOffset(1, -1),
      new CellOffset(-1, -1)
    };
    private Navigator navigator;
    private bool shouldPlayEmotes;
    public string entombedAnimOverride;
    private List<int> safeCells = new List<int>();
    private int MAX_CELLS_TRACKED = 3;
    private static System.Action<object, object> OnDestinationReachedDispatcher = (System.Action<object, object>) ((context, data) => Unsafe.As<FallMonitor.Instance>(context).OnDestinationReached(data));
    private static System.Action<object, object> OnMovementStateChangedDispatcher = (System.Action<object, object>) ((context, data) => Unsafe.As<FallMonitor.Instance>(context).OnMovementStateChanged(data));
    private static System.Action<object, object> OnCellChangedDispatcher = (System.Action<object, object>) ((context, data) => Unsafe.As<FallMonitor.Instance>(context).OnCellChanged(data));
    private bool flipRecoverEmote;

    public Instance(IStateMachineTarget master, bool shouldPlayEmotes, string entombedAnimOverride = null)
      : base(master)
    {
      this.navigator = this.GetComponent<Navigator>();
      this.shouldPlayEmotes = shouldPlayEmotes;
      this.entombedAnimOverride = entombedAnimOverride;
      Pathfinding.Instance.FlushNavGridsOnLoad();
      this.Subscribe(915392638, FallMonitor.Instance.OnCellChangedDispatcher, (object) this);
      this.Subscribe(1027377649, FallMonitor.Instance.OnMovementStateChangedDispatcher, (object) this);
      this.Subscribe(387220196, FallMonitor.Instance.OnDestinationReachedDispatcher, (object) this);
    }

    private void OnDestinationReached(object data)
    {
      int cell = Grid.PosToCell(this.transform.GetPosition());
      if (this.safeCells.Contains(cell))
        return;
      this.safeCells.Add(cell);
      if (this.safeCells.Count <= this.MAX_CELLS_TRACKED)
        return;
      this.safeCells.RemoveAt(0);
    }

    private void OnMovementStateChanged(object data)
    {
      if (((Boxed<GameHashes>) data).value != GameHashes.ObjectMovementWakeUp)
        return;
      int cell = Grid.PosToCell(this.transform.GetPosition());
      if (this.safeCells.Contains(cell))
        return;
      this.safeCells.Add(cell);
      if (this.safeCells.Count <= this.MAX_CELLS_TRACKED)
        return;
      this.safeCells.RemoveAt(0);
    }

    private void OnCellChanged(object data)
    {
      int num = ((Boxed<int>) data).value;
      if (this.safeCells.Contains(num))
        return;
      this.safeCells.Add(num);
      if (this.safeCells.Count <= this.MAX_CELLS_TRACKED)
        return;
      this.safeCells.RemoveAt(0);
    }

    public void Recover()
    {
      int cell1 = Grid.PosToCell((KMonoBehaviour) this.navigator);
      foreach (NavGrid.Transition transition in this.navigator.NavGrid.transitions)
      {
        if (transition.isEscape && this.navigator.CurrentNavType == transition.start)
        {
          int cell2 = transition.IsValid(cell1, this.navigator.NavGrid.NavTable);
          if (Grid.InvalidCell != cell2)
          {
            Vector2I xy = Grid.CellToXY(cell1);
            this.flipRecoverEmote = Grid.CellToXY(cell2).x < xy.x;
            this.navigator.BeginTransition(transition);
            break;
          }
        }
      }
    }

    public void RecoverEmote()
    {
      if (!this.shouldPlayEmotes || UnityEngine.Random.Range(0, 9) != 8)
        return;
      EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) this.master.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, Db.Get().Emotes.Minion.CloseCall_Fall, KAnim.PlayMode.Once, flip_x: this.flipRecoverEmote);
    }

    public void LandFloor()
    {
      this.navigator.SetCurrentNavType(NavType.Floor);
      this.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(this.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
    }

    public void AttemptInitialRecovery()
    {
      if (this.IsIncapacitated())
        return;
      int cell = Grid.PosToCell((KMonoBehaviour) this.navigator);
      foreach (NavGrid.Transition transition in this.navigator.NavGrid.transitions)
      {
        if (transition.isEscape && this.navigator.CurrentNavType == transition.start)
        {
          int num = transition.IsValid(cell, this.navigator.NavGrid.NavTable);
          if (Grid.InvalidCell != num)
          {
            this.smi.GoTo((StateMachine.BaseState) this.smi.sm.recoverinitialfall);
            break;
          }
        }
      }
    }

    public bool CanRecoverToLadder()
    {
      return this.navigator.NavGrid.NavTable.IsValid(Grid.PosToCell(this.master.transform.GetPosition()), NavType.Ladder) && !this.IsIncapacitated();
    }

    public void MountLadder()
    {
      this.navigator.SetCurrentNavType(NavType.Ladder);
      this.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(this.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
    }

    public bool CanRecoverToPole()
    {
      return this.navigator.NavGrid.NavTable.IsValid(Grid.PosToCell(this.master.transform.GetPosition()), NavType.Pole) && !this.IsIncapacitated();
    }

    public void MountPole()
    {
      this.navigator.SetCurrentNavType(NavType.Pole);
      this.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(this.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
    }

    public bool CanRecoverToSwim()
    {
      int cell = Grid.PosToCell(this.master.transform.GetPosition());
      SwimMonitor.Instance smi = this.navigator.GetSMI<SwimMonitor.Instance>();
      return smi != null && smi.CanSwim() && !this.IsIncapacitated() && this.navigator.NavGrid.NavTable.IsValid(cell, NavType.Swim);
    }

    public void Swim()
    {
      this.navigator.SetCurrentNavType(NavType.Swim);
      this.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(this.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
    }

    private bool IsIncapacitated()
    {
      return this.gameObject.HasTag(GameTags.Incapacitated) || this.gameObject.HasTag(GameTags.SuffocatingIncapacitated);
    }

    public void UpdateFalling()
    {
      bool flag1 = false;
      bool flag2 = false;
      if (!this.navigator.IsMoving() && this.navigator.CurrentNavType != NavType.Tube)
      {
        int cell = Grid.PosToCell(this.transform.GetPosition());
        int index = Grid.CellAbove(cell);
        bool flag3 = Grid.IsValidCell(cell);
        bool flag4 = Grid.IsValidCell(index);
        bool flag5 = this.IsValidNavCell(cell) && (!this.IsIncapacitated() || this.navigator.CurrentNavType != NavType.Ladder && this.navigator.CurrentNavType != NavType.Pole && this.navigator.CurrentNavType != NavType.Swim);
        flag2 = !flag5 && flag3 && Grid.Solid[cell] && !Grid.DupePassable[cell] || flag4 && Grid.Solid[index] && !Grid.DupePassable[index] || flag3 && Grid.DupeImpassable[cell] || flag4 && Grid.DupeImpassable[index];
        flag1 = !flag5 && !flag2;
        if (!flag3 & flag4 || flag4 && (int) Grid.WorldIdx[cell] != (int) Grid.WorldIdx[index] && Grid.IsWorldValidCell(index))
          this.TeleportInWorld(cell);
      }
      this.sm.isFalling.Set(flag1, this.smi);
      this.sm.isEntombed.Set(flag2, this.smi);
    }

    private void TeleportInWorld(int cell)
    {
      int index = Grid.CellAbove(cell);
      WorldContainer world = ClusterManager.Instance.GetWorld((int) Grid.WorldIdx[index]);
      if ((UnityEngine.Object) world != (UnityEngine.Object) null)
      {
        int safeCell = world.GetSafeCell();
        Debug.Log((object) $"Teleporting {this.navigator.name} to {safeCell}");
        this.MoveToCell(safeCell);
      }
      else
        Debug.LogError((object) $"Unable to teleport {this.navigator.name} stuck on {cell}");
    }

    private bool IsValidNavCell(int cell)
    {
      return this.navigator.NavGrid.NavTable.IsValid(cell, this.navigator.CurrentNavType) && !Grid.DupeImpassable[cell];
    }

    public void TryEntombedEscape()
    {
      int cell1 = Grid.PosToCell(this.transform.GetPosition());
      int backCell = this.GetComponent<Facing>().GetBackCell();
      int num1 = Grid.CellAbove(backCell);
      int num2 = Grid.CellBelow(backCell);
      int[] numArray = new int[3]{ backCell, num1, num2 };
      foreach (int num3 in numArray)
      {
        if (this.IsValidNavCell(num3) && !Grid.HasDoor[num3])
        {
          this.MoveToCell(num3);
          return;
        }
      }
      int cell2 = Grid.PosToCell(this.transform.GetPosition());
      foreach (CellOffset entombedEscapeOffset in this.entombedEscapeOffsets)
      {
        if (Grid.IsCellOffsetValid(cell2, entombedEscapeOffset))
        {
          int num4 = Grid.OffsetCell(cell2, entombedEscapeOffset);
          if (this.IsValidNavCell(num4) && !Grid.HasDoor[num4])
          {
            this.MoveToCell(num4);
            return;
          }
        }
      }
      for (int index = this.safeCells.Count - 1; index >= 0; --index)
      {
        int safeCell = this.safeCells[index];
        if (safeCell != cell1 && this.IsValidNavCell(safeCell) && !Grid.HasDoor[safeCell])
        {
          this.MoveToCell(safeCell);
          return;
        }
      }
      foreach (CellOffset entombedEscapeOffset in this.entombedEscapeOffsets)
      {
        if (Grid.IsCellOffsetValid(cell2, entombedEscapeOffset))
        {
          int num5 = Grid.OffsetCell(cell2, entombedEscapeOffset);
          int num6 = Grid.CellAbove(num5);
          if (Grid.IsValidCell(num6) && !Grid.Solid[num5] && !Grid.Solid[num6] && !Grid.DupeImpassable[num5] && !Grid.DupeImpassable[num6] && !Grid.HasDoor[num5] && !Grid.HasDoor[num6])
          {
            this.MoveToCell(num5, true);
            return;
          }
        }
      }
      this.GoTo((StateMachine.BaseState) this.sm.entombed.stuck);
    }

    private void MoveToCell(int cell, bool forceFloorNav = false)
    {
      this.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
      this.transform.GetComponent<Navigator>().Stop();
      if (this.IsIncapacitated() | forceFloorNav)
        this.transform.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
      this.UpdateFalling();
      if (this.sm.isEntombed.Get(this.smi))
        this.GoTo((StateMachine.BaseState) this.sm.entombed.stuck);
      else
        this.GoTo((StateMachine.BaseState) this.sm.standing);
    }
  }
}
