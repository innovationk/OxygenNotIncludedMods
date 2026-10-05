// Decompiled with JetBrains decompiler
// Type: DebugGoToStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class DebugGoToStates : 
  GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>
{
  public GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State moving;
  public GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.moving;
    GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State state = this.moving.MoveTo(new Func<DebugGoToStates.Instance, int>(DebugGoToStates.GetTargetCell), this.behaviourcomplete, this.behaviourcomplete, true);
    string name = (string) CREATURES.STATUSITEMS.DEBUGGOTO.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.DEBUGGOTO.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    state.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category);
    this.behaviourcomplete.BehaviourComplete(GameTags.HasDebugDestination);
  }

  private static int GetTargetCell(DebugGoToStates.Instance smi)
  {
    return smi.GetSMI<CreatureDebugGoToMonitor.Instance>().targetCell;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.GameInstance
  {
    public Instance(Chore<DebugGoToStates.Instance> chore, DebugGoToStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.HasDebugDestination);
    }
  }
}
