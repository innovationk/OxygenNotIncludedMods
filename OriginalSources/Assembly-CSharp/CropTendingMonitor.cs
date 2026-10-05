// Decompiled with JetBrains decompiler
// Type: CropTendingMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class CropTendingMonitor : 
  GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>
{
  private StateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.FloatParameter cooldownTimer;
  private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State cooldown;
  private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State lookingForCrop;
  private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State reset;

  private bool InterestedInTendingCrops(CropTendingMonitor.Instance smi)
  {
    return !smi.HasTag(GameTags.Creatures.Hungry) || (double) UnityEngine.Random.value <= (double) smi.def.unsatisfiedTendChance;
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.cooldown;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    double num1;
    this.cooldown.ParamTransition<float>((StateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.Parameter<float>) this.cooldownTimer, this.lookingForCrop, (StateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.Parameter<float>.Callback) ((smi, p) => (double) this.cooldownTimer.Get(smi) <= 0.0 && this.InterestedInTendingCrops(smi))).ParamTransition<float>((StateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.Parameter<float>) this.cooldownTimer, this.reset, (StateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.Parameter<float>.Callback) ((smi, p) => (double) this.cooldownTimer.Get(smi) <= 0.0 && !this.InterestedInTendingCrops(smi))).Update((System.Action<CropTendingMonitor.Instance, float>) ((smi, dt) => num1 = (double) this.cooldownTimer.Delta(-dt, smi)), UpdateRate.SIM_1000ms);
    this.lookingForCrop.ToggleBehaviour(GameTags.Creatures.WantsToTendCrops, (StateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.Transition.ConditionCallback) (smi => true), (System.Action<CropTendingMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.reset)));
    double num2;
    this.reset.Exit((StateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State.Callback) (smi => num2 = (double) this.cooldownTimer.Set(600f / smi.def.numCropsTendedPerCycle, smi))).GoTo(this.cooldown);
  }

  public class Def : StateMachine.BaseDef
  {
    public float numCropsTendedPerCycle = 8f;
    public float unsatisfiedTendChance = 0.5f;
  }

  public new class Instance(IStateMachineTarget master, CropTendingMonitor.Def def) : 
    GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.GameInstance(master, def)
  {
  }
}
