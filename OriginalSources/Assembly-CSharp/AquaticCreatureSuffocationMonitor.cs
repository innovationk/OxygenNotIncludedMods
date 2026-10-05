// Decompiled with JetBrains decompiler
// Type: AquaticCreatureSuffocationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AquaticCreatureSuffocationMonitor : 
  GameStateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>
{
  public GameStateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.State safe;
  public GameStateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.State suffocating;
  public GameStateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.State die;
  public GameStateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.State dead;
  public StateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.FloatParameter DeathTimer;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.safe;
    this.root.TagTransition(GameTags.Dead, this.dead);
    this.safe.Transition(this.suffocating, new StateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.Transition.ConditionCallback(AquaticCreatureSuffocationMonitor.IsSuffocating), UpdateRate.SIM_1000ms).Update(new System.Action<AquaticCreatureSuffocationMonitor.Instance, float>(AquaticCreatureSuffocationMonitor.RecoveryDeathTimerUpdate));
    this.suffocating.ParamTransition<float>((StateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.Parameter<float>) this.DeathTimer, this.die, new StateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.Parameter<float>.Callback(AquaticCreatureSuffocationMonitor.CanNotHoldAnymore)).Transition(this.safe, new StateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.Transition.ConditionCallback(AquaticCreatureSuffocationMonitor.CanBreath), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.AquaticCreatureSuffocating).Update(new System.Action<AquaticCreatureSuffocationMonitor.Instance, float>(AquaticCreatureSuffocationMonitor.DeathTimerUpdate));
    this.die.Enter(new StateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.State.Callback(AquaticCreatureSuffocationMonitor.Kill));
    this.dead.DoNothing();
  }

  public static bool IsSuffocating(AquaticCreatureSuffocationMonitor.Instance smi)
  {
    return !smi.CanBreath();
  }

  public static bool CanBreath(AquaticCreatureSuffocationMonitor.Instance smi) => smi.CanBreath();

  public static bool CanNotHoldAnymore(
    AquaticCreatureSuffocationMonitor.Instance smi,
    float deathTimerValue)
  {
    return (double) deathTimerValue > (double) smi.def.DeathTimerDuration;
  }

  public static void DeathTimerUpdate(AquaticCreatureSuffocationMonitor.Instance smi, float dt)
  {
    double num = (double) smi.sm.DeathTimer.Set(smi.DeathTimerValue + dt, smi);
  }

  public static void RecoveryDeathTimerUpdate(
    AquaticCreatureSuffocationMonitor.Instance smi,
    float dt)
  {
    if ((double) smi.DeathTimerValue <= 0.0)
      return;
    double num = (double) smi.sm.DeathTimer.Set(Mathf.Max(smi.DeathTimerValue - dt * smi.def.RecoveryModifier, 0.0f), smi);
  }

  public static void Kill(AquaticCreatureSuffocationMonitor.Instance smi)
  {
    smi.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
  }

  public class Def : StateMachine.BaseDef
  {
    public float DeathTimerDuration = 2400f;
    public float RecoveryModifier = 4f;
  }

  public new class Instance : 
    GameStateMachine<AquaticCreatureSuffocationMonitor, AquaticCreatureSuffocationMonitor.Instance, IStateMachineTarget, AquaticCreatureSuffocationMonitor.Def>.GameInstance
  {
    private Pickupable pickupable;

    public float DeathTimerValue => this.sm.DeathTimer.Get(this);

    public float TimeUntilDeath
    {
      get => Mathf.Max(this.smi.def.DeathTimerDuration - this.DeathTimerValue, 0.0f);
    }

    public Instance(IStateMachineTarget master, AquaticCreatureSuffocationMonitor.Def def)
      : base(master, def)
    {
      this.pickupable = this.GetComponent<Pickupable>();
    }

    public bool CanBreath()
    {
      int cell = Grid.PosToCell((StateMachine.Instance) this);
      return (!((UnityEngine.Object) this.pickupable.storage == (UnityEngine.Object) null) ? 0 : (!Grid.IsSubstantialLiquid(cell) ? 1 : 0)) == 0;
    }
  }
}
