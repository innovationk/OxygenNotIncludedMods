// Decompiled with JetBrains decompiler
// Type: DebugGoToMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class DebugGoToMonitor : 
  GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>
{
  public GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.State satisfied;
  public GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.State hastarget;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.satisfied.DoNothing();
    this.hastarget.ToggleChore((Func<DebugGoToMonitor.Instance, Chore>) (smi => (Chore) new MoveChore(smi.master, Db.Get().ChoreTypes.DebugGoTo, (Func<MoveChore.StatesInstance, int>) (smii => smi.targetCellIndex))), this.satisfied);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget target, DebugGoToMonitor.Def def) : 
    GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.GameInstance(target, def)
  {
    public int targetCellIndex = Grid.InvalidCell;

    public void GoToCursor()
    {
      this.targetCellIndex = DebugHandler.GetMouseCell();
      if (this.smi.GetCurrentState() != this.smi.sm.satisfied)
        return;
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.hastarget);
    }

    public void GoToCell(int cellIndex)
    {
      this.targetCellIndex = cellIndex;
      if (this.smi.GetCurrentState() != this.smi.sm.satisfied)
        return;
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.hastarget);
    }
  }
}
