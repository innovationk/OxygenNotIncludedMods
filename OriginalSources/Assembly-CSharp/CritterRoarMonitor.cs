// Decompiled with JetBrains decompiler
// Type: CritterRoarMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class CritterRoarMonitor : 
  GameStateMachine<CritterRoarMonitor, CritterRoarMonitor.Instance, IStateMachineTarget, CritterRoarMonitor.Def>
{
  public static Tag TAG = GameTags.Creatures.Behaviours.CritterRoarBehaviour;
  private readonly GameStateMachine<CritterRoarMonitor, CritterRoarMonitor.Instance, IStateMachineTarget, CritterRoarMonitor.Def>.State wait;
  private readonly GameStateMachine<CritterRoarMonitor, CritterRoarMonitor.Instance, IStateMachineTarget, CritterRoarMonitor.Def>.State roar;
  private readonly GameStateMachine<CritterRoarMonitor, CritterRoarMonitor.Instance, IStateMachineTarget, CritterRoarMonitor.Def>.State cooldown;
  private static readonly StateMachine<CritterRoarMonitor, CritterRoarMonitor.Instance, IStateMachineTarget, CritterRoarMonitor.Def>.Transition.ConditionCallback ALWAYS_TRUE = (StateMachine<CritterRoarMonitor, CritterRoarMonitor.Instance, IStateMachineTarget, CritterRoarMonitor.Def>.Transition.ConditionCallback) (smi => true);

  public override void InitializeStates(out StateMachine.BaseState defaultState)
  {
    defaultState = (StateMachine.BaseState) this.wait;
    this.wait.ScheduleGoTo((Func<CritterRoarMonitor.Instance, float>) (smi => smi.NextWaitDuration()), (StateMachine.BaseState) this.roar);
    this.roar.ToggleBehaviour(CritterRoarMonitor.TAG, CritterRoarMonitor.ALWAYS_TRUE, (System.Action<CritterRoarMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.cooldown)));
    this.cooldown.ScheduleGoTo((Func<CritterRoarMonitor.Instance, float>) (smi => smi.Def.Cooldown), (StateMachine.BaseState) this.wait);
  }

  public class Def : StateMachine.BaseDef
  {
    public float SecondsPerRoarMax { get; private set; }

    public float Cooldown { get; private set; }

    public void Initialize(int roarsPerCycle, float cooldown)
    {
      this.SecondsPerRoarMax = 600f / (float) roarsPerCycle;
      this.Cooldown = cooldown;
    }
  }

  public new class Instance : 
    GameStateMachine<CritterRoarMonitor, CritterRoarMonitor.Instance, IStateMachineTarget, CritterRoarMonitor.Def>.GameInstance
  {
    private readonly float maxWait;
    private float wait;

    public CritterRoarMonitor.Def Def { get; private set; }

    public Instance(IStateMachineTarget master, CritterRoarMonitor.Def def)
      : base(master, def)
    {
      this.Def = def;
      this.wait = this.Def.SecondsPerRoarMax;
      DebugUtil.DevAssert((double) this.Def.SecondsPerRoarMax >= (double) this.Def.Cooldown, "Cooldown is so long so as to prevent us from achieving desired roars per cycle.");
      this.maxWait = this.Def.SecondsPerRoarMax - this.Def.Cooldown;
    }

    public float NextWaitDuration()
    {
      float minInclusive = this.Def.SecondsPerRoarMax - this.wait;
      this.wait = UnityEngine.Random.Range(minInclusive, minInclusive + this.maxWait);
      return this.wait;
    }
  }
}
