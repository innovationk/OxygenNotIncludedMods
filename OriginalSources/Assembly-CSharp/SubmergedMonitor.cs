// Decompiled with JetBrains decompiler
// Type: SubmergedMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class SubmergedMonitor : 
  GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>
{
  public GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.State satisfied;
  public GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.State submerged;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.satisfied.Enter("SetNavType", (StateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Hover))).Update("SetNavType", (System.Action<SubmergedMonitor.Instance, float>) ((smi, dt) => smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Hover)), UpdateRate.SIM_1000ms).Transition(this.submerged, (StateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsSubmerged()), UpdateRate.SIM_1000ms);
    this.submerged.Enter("SetNavType", (StateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Swim))).Update("SetNavType", (System.Action<SubmergedMonitor.Instance, float>) ((smi, dt) => smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Swim)), UpdateRate.SIM_1000ms).Transition(this.satisfied, (StateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.Transition.ConditionCallback) (smi => !smi.IsSubmerged()), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Submerged);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, SubmergedMonitor.Def def) : 
    GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.GameInstance(master, def)
  {
    public bool IsSubmerged()
    {
      return Grid.IsSubstantialLiquid(Grid.PosToCell(this.transform.GetPosition()));
    }
  }
}
