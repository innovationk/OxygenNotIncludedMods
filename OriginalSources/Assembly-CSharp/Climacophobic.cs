// Decompiled with JetBrains decompiler
// Type: Climacophobic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
public class Climacophobic : StateMachineComponent<Climacophobic.StatesInstance>
{
  protected override void OnSpawn() => this.smi.StartSM();

  protected bool IsUncomfortable()
  {
    int num1 = 5;
    int cell1 = Grid.PosToCell(this.gameObject);
    if (!this.isCellLadder(cell1))
      return false;
    int num2 = 1;
    bool flag1 = true;
    bool flag2 = true;
    for (int y = 1; y < num1; ++y)
    {
      int cell2 = Grid.OffsetCell(cell1, 0, y);
      int cell3 = Grid.OffsetCell(cell1, 0, -y);
      if (flag1 && this.isCellLadder(cell2))
        ++num2;
      else
        flag1 = false;
      if (flag2 && this.isCellLadder(cell3))
        ++num2;
      else
        flag2 = false;
    }
    return num2 >= num1;
  }

  private bool isCellLadder(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return false;
    GameObject gameObject = Grid.Objects[cell, 1];
    return !((UnityEngine.Object) gameObject == (UnityEngine.Object) null) && !((UnityEngine.Object) gameObject.GetComponent<Ladder>() == (UnityEngine.Object) null);
  }

  public class StatesInstance(Climacophobic master) : 
    GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic>
  {
    public GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic, object>.State satisfied;
    public GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic, object>.State suffering;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.satisfied;
      this.root.Update("ClimacophobicCheck", (Action<Climacophobic.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.master.IsUncomfortable())
          smi.GoTo((StateMachine.BaseState) this.suffering);
        else
          smi.GoTo((StateMachine.BaseState) this.satisfied);
      }), UpdateRate.SIM_1000ms);
      this.suffering.AddEffect("Vertigo").ToggleExpression(Db.Get().Expressions.Uncomfortable);
      this.satisfied.DoNothing();
    }
  }
}
