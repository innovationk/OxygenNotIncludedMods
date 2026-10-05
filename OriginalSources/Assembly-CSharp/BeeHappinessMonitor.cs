// Decompiled with JetBrains decompiler
// Type: BeeHappinessMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

#nullable disable
public class BeeHappinessMonitor : 
  GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>
{
  private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State satisfied;
  private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State happy;
  private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State unhappy;
  private Effect happyEffect;
  private Effect neutralEffect;
  private Effect unhappyEffect;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.satisfied.TriggerOnEnter(GameHashes.Satisfied).Transition(this.happy, new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsHappy), UpdateRate.SIM_1000ms).Transition(this.unhappy, new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsUnhappy), UpdateRate.SIM_1000ms).ToggleEffect((Func<BeeHappinessMonitor.Instance, Effect>) (smi => this.neutralEffect));
    this.happy.TriggerOnEnter(GameHashes.Happy).ToggleEffect((Func<BeeHappinessMonitor.Instance, Effect>) (smi => this.happyEffect)).Transition(this.satisfied, GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Not(new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsHappy)), UpdateRate.SIM_1000ms);
    this.unhappy.TriggerOnEnter(GameHashes.Unhappy).Transition(this.satisfied, GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Not(new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsUnhappy)), UpdateRate.SIM_1000ms).ToggleEffect((Func<BeeHappinessMonitor.Instance, Effect>) (smi => this.unhappyEffect));
    this.happyEffect = new Effect("Happy", (string) CREATURES.MODIFIERS.HAPPY_WILD.NAME, (string) CREATURES.MODIFIERS.HAPPY_WILD.TOOLTIP, 0.0f, true, false, false);
    this.neutralEffect = new Effect("Neutral", (string) CREATURES.MODIFIERS.NEUTRAL.NAME, (string) CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0.0f, true, false, false);
    this.unhappyEffect = new Effect("Unhappy", (string) CREATURES.MODIFIERS.GLUM.NAME, (string) CREATURES.MODIFIERS.GLUM.TOOLTIP, 0.0f, true, false, true);
  }

  private static bool IsHappy(BeeHappinessMonitor.Instance smi)
  {
    return (double) smi.happiness.GetTotalValue() >= (double) smi.def.happyThreshold;
  }

  private static bool IsUnhappy(BeeHappinessMonitor.Instance smi)
  {
    return (double) smi.happiness.GetTotalValue() <= (double) smi.def.unhappyThreshold;
  }

  public class Def : StateMachine.BaseDef
  {
    public float happyThreshold = 4f;
    public float unhappyThreshold = -1f;
  }

  public new class Instance : 
    GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.GameInstance
  {
    public AttributeInstance happiness;

    public Instance(IStateMachineTarget master, BeeHappinessMonitor.Def def)
      : base(master, def)
    {
      this.happiness = this.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Happiness);
    }
  }
}
