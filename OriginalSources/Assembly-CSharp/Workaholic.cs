// Decompiled with JetBrains decompiler
// Type: Workaholic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[SkipSaveFileSerialization]
public class Workaholic : StateMachineComponent<Workaholic.StatesInstance>
{
  protected override void OnSpawn() => this.smi.StartSM();

  protected bool IsUncomfortable()
  {
    return this.smi.master.GetComponent<ChoreDriver>().GetCurrentChore() is IdleChore;
  }

  public class StatesInstance(Workaholic master) : 
    GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.GameInstance(master)
  {
  }

  public class States : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic>
  {
    public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State satisfied;
    public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State suffering;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.satisfied;
      this.root.Update("WorkaholicCheck", (Action<Workaholic.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.master.IsUncomfortable())
          smi.GoTo((StateMachine.BaseState) this.suffering);
        else
          smi.GoTo((StateMachine.BaseState) this.satisfied);
      }), UpdateRate.SIM_1000ms);
      this.suffering.AddEffect("Restless").ToggleExpression(Db.Get().Expressions.Uncomfortable);
      this.satisfied.DoNothing();
    }
  }
}
