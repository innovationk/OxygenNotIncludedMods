// Decompiled with JetBrains decompiler
// Type: CleaningMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class CleaningMonitor : 
  GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>
{
  public GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.State cooldown;
  public GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.State clean;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.clean;
    this.clean.ToggleBehaviour(GameTags.Creatures.Cleaning, (StateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.Transition.ConditionCallback) (smi => smi.CanCleanElementState()), (System.Action<CleaningMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.cooldown)));
    this.cooldown.ScheduleGoTo((Func<CleaningMonitor.Instance, float>) (smi => smi.def.coolDown), (StateMachine.BaseState) this.clean);
  }

  public class Def : StateMachine.BaseDef
  {
    public Element.State elementState = Element.State.Liquid;
    public CellOffset[] cellOffsets;
    public float coolDown = 30f;
  }

  public new class Instance(IStateMachineTarget master, CleaningMonitor.Def def) : 
    GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.GameInstance(master, def)
  {
    public bool CanCleanElementState()
    {
      int cell = Grid.PosToCell(this.smi.transform.GetPosition());
      if (!Grid.IsValidCell(cell) || !Grid.IsLiquid(cell) && this.smi.def.elementState == Element.State.Liquid)
        return false;
      if (Grid.DiseaseCount[cell] > 0)
        return true;
      if (this.smi.def.cellOffsets != null)
      {
        foreach (CellOffset cellOffset in this.smi.def.cellOffsets)
        {
          int num = Grid.OffsetCell(cell, cellOffset);
          if (Grid.IsValidCell(num) && Grid.DiseaseCount[num] > 0)
            return true;
        }
      }
      return false;
    }
  }
}
