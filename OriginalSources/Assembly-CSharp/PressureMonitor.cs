// Decompiled with JetBrains decompiler
// Type: PressureMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

#nullable disable
public class PressureMonitor : 
  GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>
{
  public const string OVER_PRESSURE_EFFECT_NAME = "PoppedEarDrums";
  public const float TIME_IN_PRESSURE_BEFORE_EAR_POPS = 3f;
  private static CellOffset[] PRESSURE_TEST_OFFSET = new CellOffset[2]
  {
    new CellOffset(0, 0),
    new CellOffset(0, 1)
  };
  public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State safe;
  public PressureMonitor.PressureStates inPressure;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.safe;
    this.safe.Transition((GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State) this.inPressure, new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsInPressureGas));
    this.inPressure.Transition(this.safe, GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Not(new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsInPressureGas))).DefaultState(this.inPressure.idle);
    this.inPressure.idle.EventTransition(GameHashes.EffectImmunityAdded, this.inPressure.immune, new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsImmuneToPressure)).Update(new System.Action<PressureMonitor.Instance, float>(PressureMonitor.HighPressureUpdate));
    this.inPressure.immune.EventTransition(GameHashes.EffectImmunityRemoved, this.inPressure.idle, GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Not(new StateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.Transition.ConditionCallback(PressureMonitor.IsImmuneToPressure)));
  }

  public static bool IsInPressureGas(PressureMonitor.Instance smi) => smi.IsInHighPressure();

  public static bool IsImmuneToPressure(PressureMonitor.Instance smi)
  {
    return smi.IsImmuneToHighPressure();
  }

  public static void RemoveOverpressureEffect(PressureMonitor.Instance smi) => smi.RemoveEffect();

  public static void HighPressureUpdate(PressureMonitor.Instance smi, float dt)
  {
    if ((double) smi.timeinstate <= 3.0)
      return;
    smi.AddEffect();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class PressureStates : 
    GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State
  {
    public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State idle;
    public GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.State immune;
  }

  public new class Instance : 
    GameStateMachine<PressureMonitor, PressureMonitor.Instance, IStateMachineTarget, PressureMonitor.Def>.GameInstance
  {
    private Effects effects;

    public Instance(IStateMachineTarget master, PressureMonitor.Def def)
      : base(master, def)
    {
      this.effects = this.GetComponent<Effects>();
    }

    public bool IsImmuneToHighPressure()
    {
      return this.effects.HasImmunityTo(Db.Get().effects.Get("PoppedEarDrums"));
    }

    public bool IsInHighPressure()
    {
      int cell = Grid.PosToCell(this.gameObject);
      for (int index1 = 0; index1 < PressureMonitor.PRESSURE_TEST_OFFSET.Length; ++index1)
      {
        int index2 = Grid.OffsetCell(cell, PressureMonitor.PRESSURE_TEST_OFFSET[index1]);
        if (Grid.IsValidCell(index2) && Grid.Element[index2].IsGas && (double) Grid.Mass[index2] > 4.0)
          return true;
      }
      return false;
    }

    public void RemoveEffect() => this.effects.Remove("PoppedEarDrums");

    public void AddEffect() => this.effects.Add("PoppedEarDrums", true);
  }
}
