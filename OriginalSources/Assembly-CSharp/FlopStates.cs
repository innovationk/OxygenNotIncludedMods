// Decompiled with JetBrains decompiler
// Type: FlopStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class FlopStates : 
  GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>
{
  private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State flop_pre;
  private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State flop_cycle;
  private GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State pst;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.flop_pre;
    GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State root = this.root;
    string name = (string) CREATURES.STATUSITEMS.FLOPPING.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.FLOPPING.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name, tooltip, icon_type: StatusItem.IconType.Exclamation, notification_type: NotificationType.Bad, render_overlay: render_overlay, category: category);
    this.flop_pre.Enter(new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.State.Callback(FlopStates.ChooseDirection)).Transition(this.flop_cycle, new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.ShouldFlop)).Transition(this.pst, GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Not(new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.ShouldFlop)));
    this.flop_cycle.PlayAnim("flop_loop", KAnim.PlayMode.Once).Transition(this.pst, new StateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.Transition.ConditionCallback(FlopStates.IsSubstantialLiquid)).Update("Flop", new System.Action<FlopStates.Instance, float>(FlopStates.FlopForward), UpdateRate.SIM_33ms).OnAnimQueueComplete(this.flop_pre);
    this.pst.QueueAnim("flop_loop", true).BehaviourComplete(GameTags.Creatures.Flopping);
  }

  public static bool ShouldFlop(FlopStates.Instance smi)
  {
    int num = Grid.CellBelow(Grid.PosToCell(smi.transform.GetPosition()));
    return Grid.IsValidCell(num) && Grid.Solid[num];
  }

  public static void ChooseDirection(FlopStates.Instance smi)
  {
    int cell = Grid.PosToCell(smi.transform.GetPosition());
    if (FlopStates.SearchForLiquid(cell, 1))
      smi.currentDir = 1f;
    else if (FlopStates.SearchForLiquid(cell, -1))
      smi.currentDir = -1f;
    else if ((double) UnityEngine.Random.value > 0.5)
      smi.currentDir = 1f;
    else
      smi.currentDir = -1f;
  }

  private static bool SearchForLiquid(int cell, int delta_x)
  {
    while (Grid.IsValidCell(cell))
    {
      if (Grid.IsSubstantialLiquid(cell))
        return true;
      if (Grid.Solid[cell] || Grid.CritterImpassable[cell])
        return false;
      int num = Grid.CellBelow(cell);
      if (Grid.IsValidCell(num) && Grid.Solid[num])
        cell += delta_x;
      else
        cell = num;
    }
    return false;
  }

  public static void FlopForward(FlopStates.Instance smi, float dt)
  {
    if (smi.HasTag(GameTags.Creatures.StunnedForCapture))
      return;
    KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
    int currentFrame = component.currentFrame;
    if (component.IsVisible() && (currentFrame < smi.def.frameToFlopStart || currentFrame > smi.def.frameToFlopEnd))
      return;
    Vector3 position = smi.transform.GetPosition();
    Vector3 vector3 = position with
    {
      x = position.x + (float) ((double) smi.currentDir * (double) dt * 1.0)
    };
    int cell = Grid.PosToCell(vector3);
    if (FlopStates.CanFlopForward(smi, cell))
      smi.transform.SetPosition(vector3);
    else
      smi.currentDir = -smi.currentDir;
  }

  private static bool CanFlopForward(FlopStates.Instance smi, int new_cell)
  {
    if (!Grid.IsValidCell(new_cell) || Grid.Solid[new_cell] || Grid.CritterImpassable[new_cell])
      return false;
    foreach (CellOffset occupiedCellsOffset in smi.GetComponent<OccupyArea>().OccupiedCellsOffsets)
    {
      int num = Grid.OffsetCell(new_cell, occupiedCellsOffset);
      if (!Grid.IsValidCell(num) || Grid.Solid[num] || Grid.CritterImpassable[num])
        return false;
    }
    return true;
  }

  public static bool IsSubstantialLiquid(FlopStates.Instance smi)
  {
    return Grid.IsSubstantialLiquid(Grid.PosToCell(smi.transform.GetPosition()));
  }

  public class Def : StateMachine.BaseDef
  {
    public bool flipFacing;
    public int frameToFlopStart = 23;
    public int frameToFlopEnd = 36;
  }

  public new class Instance : 
    GameStateMachine<FlopStates, FlopStates.Instance, IStateMachineTarget, FlopStates.Def>.GameInstance
  {
    private float currentDirection;

    public float currentDir
    {
      set
      {
        this.currentDirection = value;
        if (!this.def.flipFacing)
          return;
        this.GetComponent<Facing>().SetFacing((double) this.currentDir < 0.0);
      }
      get => this.currentDirection;
    }

    public Instance(Chore<FlopStates.Instance> chore, FlopStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Flopping);
    }
  }
}
