// Decompiled with JetBrains decompiler
// Type: MoltStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using UnityEngine;

#nullable disable
public class MoltStates : 
  GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>
{
  public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State moltpre;
  public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State moltpst;
  public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.moltpre;
    GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State root = this.root;
    string name = (string) CREATURES.STATUSITEMS.MOLTING.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.MOLTING.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category);
    this.moltpre.Enter(new StateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State.Callback(MoltStates.Molt)).QueueAnim("lay_egg_pre").OnAnimQueueComplete(this.moltpst);
    this.moltpst.QueueAnim("lay_egg_pst").OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.ScalesGrown);
  }

  private static void Molt(MoltStates.Instance smi)
  {
    smi.eggPos = smi.transform.GetPosition();
    smi.GetSMI<ScaleGrowthMonitor.Instance>().Shear();
  }

  private static int GetMoveAsideCell(MoltStates.Instance smi)
  {
    int x = 1;
    if (GenericGameSettings.instance.acceleratedLifecycle)
      x = 8;
    int cell1 = Grid.PosToCell((StateMachine.Instance) smi);
    if (Grid.IsValidCell(cell1))
    {
      int moveAsideCell = Grid.OffsetCell(cell1, x, 0);
      if (Grid.IsValidCell(moveAsideCell) && !Grid.Solid[moveAsideCell])
        return moveAsideCell;
      int cell2 = Grid.OffsetCell(cell1, -x, 0);
      if (Grid.IsValidCell(cell2))
        return cell2;
    }
    return Grid.InvalidCell;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.GameInstance
  {
    public Vector3 eggPos;

    public Instance(Chore<MoltStates.Instance> chore, MoltStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.ScalesGrown);
    }
  }
}
