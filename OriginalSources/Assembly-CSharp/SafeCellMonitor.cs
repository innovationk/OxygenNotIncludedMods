// Decompiled with JetBrains decompiler
// Type: SafeCellMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class SafeCellMonitor : 
  GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, SafeCellMonitor.Def>
{
  public GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, SafeCellMonitor.Def>.State safe;
  public GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, SafeCellMonitor.Def>.State danger;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.safe;
    this.root.ToggleUrge(Db.Get().Urges.MoveToSafety);
    this.safe.EventTransition(GameHashes.SafeCellDetected, this.danger, (StateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, SafeCellMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsAreaUnsafe()));
    this.danger.EventTransition(GameHashes.SafeCellLost, this.safe, (StateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, SafeCellMonitor.Def>.Transition.ConditionCallback) (smi => !smi.IsAreaUnsafe())).ToggleChore((Func<SafeCellMonitor.Instance, Chore>) (smi => (Chore) new MoveToSafetyChore(smi.master)), this.safe);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, SafeCellMonitor.Def>.GameInstance
  {
    private SafeCellSensor safeCellSensor;

    public Instance(IStateMachineTarget master, SafeCellMonitor.Def def)
      : base(master, def)
    {
      this.safeCellSensor = this.GetComponent<Sensors>().GetSensor<SafeCellSensor>();
    }

    public bool IsAreaUnsafe() => this.safeCellSensor.HasSafeCell();
  }
}
