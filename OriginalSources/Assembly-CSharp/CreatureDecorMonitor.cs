// Decompiled with JetBrains decompiler
// Type: CreatureDecorMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class CreatureDecorMonitor : 
  GameStateMachine<CreatureDecorMonitor, CreatureDecorMonitor.Instance, IStateMachineTarget, CreatureDecorMonitor.Def>
{
  private const UpdateRate UPDATE_RATE = UpdateRate.SIM_4000ms;
  private GameStateMachine<CreatureDecorMonitor, CreatureDecorMonitor.Instance, IStateMachineTarget, CreatureDecorMonitor.Def>.State lowDecor;
  private GameStateMachine<CreatureDecorMonitor, CreatureDecorMonitor.Instance, IStateMachineTarget, CreatureDecorMonitor.Def>.State highDecor;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.lowDecor;
    this.lowDecor.UpdateTransition(this.highDecor, new Func<CreatureDecorMonitor.Instance, float, bool>(CreatureDecorMonitor.IsInHighDecor), UpdateRate.SIM_4000ms).Update(new System.Action<CreatureDecorMonitor.Instance, float>(CreatureDecorMonitor.TriggerLowDecorUpdate), UpdateRate.SIM_4000ms).TriggerOnEnter(GameHashes.CreatureLowDecor);
    this.highDecor.UpdateTransition(this.lowDecor, new Func<CreatureDecorMonitor.Instance, float, bool>(CreatureDecorMonitor.IsInLowDecor), UpdateRate.SIM_4000ms).Update(new System.Action<CreatureDecorMonitor.Instance, float>(CreatureDecorMonitor.TriggerHighDecorUpdate), UpdateRate.SIM_4000ms).TriggerOnEnter(GameHashes.CreatureHighDecor);
  }

  private static void TriggerHighDecorUpdate(CreatureDecorMonitor.Instance smi, float dt)
  {
    System.Action<float> onHighDecorUpdate = smi.OnHighDecorUpdate;
    if (onHighDecorUpdate == null)
      return;
    onHighDecorUpdate(dt);
  }

  private static void TriggerLowDecorUpdate(CreatureDecorMonitor.Instance smi, float dt)
  {
    System.Action<float> onLowDecorUpdate = smi.OnLowDecorUpdate;
    if (onLowDecorUpdate == null)
      return;
    onLowDecorUpdate(dt);
  }

  private static bool IsInHighDecor(CreatureDecorMonitor.Instance smi, float dt)
  {
    return (double) Grid.Decor[Grid.PosToCell((StateMachine.Instance) smi)] >= (double) smi.def.DecorValueTreshold;
  }

  private static bool IsInLowDecor(CreatureDecorMonitor.Instance smi, float dt)
  {
    return !CreatureDecorMonitor.IsInHighDecor(smi, dt);
  }

  public class Def : StateMachine.BaseDef
  {
    public float DecorValueTreshold;
  }

  public new class Instance(IStateMachineTarget master, CreatureDecorMonitor.Def def) : 
    GameStateMachine<CreatureDecorMonitor, CreatureDecorMonitor.Instance, IStateMachineTarget, CreatureDecorMonitor.Def>.GameInstance(master, def)
  {
    public System.Action<float> OnHighDecorUpdate;
    public System.Action<float> OnLowDecorUpdate;
  }
}
