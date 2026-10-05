// Decompiled with JetBrains decompiler
// Type: HiveGrowthMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class HiveGrowthMonitor : 
  GameStateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.GrowUpBehaviour, new StateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>.Transition.ConditionCallback(HiveGrowthMonitor.IsGrowing));
  }

  public static bool IsGrowing(HiveGrowthMonitor.Instance smi)
  {
    return !smi.GetSMI<BeeHive.StatesInstance>().IsFullyGrown();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, HiveGrowthMonitor.Def def) : 
    GameStateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>.GameInstance(master, def)
  {
  }
}
