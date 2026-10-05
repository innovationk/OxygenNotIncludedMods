// Decompiled with JetBrains decompiler
// Type: MingleMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class MingleMonitor : GameStateMachine<MingleMonitor, MingleMonitor.Instance>
{
  public GameStateMachine<MingleMonitor, MingleMonitor.Instance, IStateMachineTarget, object>.State mingle;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.mingle;
    this.serializable = StateMachine.SerializeType.Never;
    this.mingle.ToggleRecurringChore(new Func<MingleMonitor.Instance, Chore>(this.CreateMingleChore));
  }

  private Chore CreateMingleChore(MingleMonitor.Instance smi)
  {
    return (Chore) new MingleChore(smi.master);
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<MingleMonitor, MingleMonitor.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
  }
}
