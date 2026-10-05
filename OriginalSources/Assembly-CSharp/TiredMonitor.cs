// Decompiled with JetBrains decompiler
// Type: TiredMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class TiredMonitor : GameStateMachine<TiredMonitor, TiredMonitor.Instance>
{
  public GameStateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.State tired;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.EventTransition(GameHashes.SleepFail, this.tired);
    this.tired.Enter((StateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.SetInterruptDay())).EventTransition(GameHashes.NewDay, (Func<TiredMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) GameClock.Instance), this.root, (StateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.AllowInterruptClear())).ToggleExpression(Db.Get().Expressions.Tired).ToggleAnims("anim_loco_walk_slouch_kanim").ToggleAnims("anim_idle_slouch_kanim");
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    public int disturbedDay = -1;
    public int interruptedDay = -1;

    public void SetInterruptDay() => this.interruptedDay = GameClock.Instance.GetCycle();

    public bool AllowInterruptClear()
    {
      int num = GameClock.Instance.GetCycle() > this.interruptedDay + 1 ? 1 : 0;
      if (num == 0)
        return num != 0;
      this.interruptedDay = -1;
      return num != 0;
    }
  }
}
