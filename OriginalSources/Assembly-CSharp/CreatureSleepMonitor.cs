// Decompiled with JetBrains decompiler
// Type: CreatureSleepMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class CreatureSleepMonitor : 
  GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.SleepBehaviour, new StateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.Transition.ConditionCallback(CreatureSleepMonitor.ShouldSleep));
  }

  public static bool ShouldSleep(CreatureSleepMonitor.Instance smi)
  {
    return GameClock.Instance.IsNighttime();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, CreatureSleepMonitor.Def def) : 
    GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.GameInstance(master, def)
  {
  }
}
