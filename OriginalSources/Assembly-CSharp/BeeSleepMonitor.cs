// Decompiled with JetBrains decompiler
// Type: BeeSleepMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BeeSleepMonitor : 
  GameStateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.Update(new System.Action<BeeSleepMonitor.Instance, float>(this.UpdateCO2Exposure), UpdateRate.SIM_1000ms).ToggleBehaviour(GameTags.Creatures.BeeWantsToSleep, new StateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>.Transition.ConditionCallback(this.ShouldSleep));
  }

  public bool ShouldSleep(BeeSleepMonitor.Instance smi) => (double) smi.CO2Exposure >= 5.0;

  public void UpdateCO2Exposure(BeeSleepMonitor.Instance smi, float dt)
  {
    if (this.IsInCO2(smi))
      ++smi.CO2Exposure;
    else
      smi.CO2Exposure -= 0.5f;
    smi.CO2Exposure = Mathf.Clamp(smi.CO2Exposure, 0.0f, 10f);
  }

  public bool IsInCO2(BeeSleepMonitor.Instance smi)
  {
    int cell = Grid.PosToCell(smi.gameObject);
    return Grid.IsValidCell(cell) && Grid.Element[cell].id == SimHashes.CarbonDioxide;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, BeeSleepMonitor.Def def) : 
    GameStateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>.GameInstance(master, def)
  {
    public float CO2Exposure;
  }
}
