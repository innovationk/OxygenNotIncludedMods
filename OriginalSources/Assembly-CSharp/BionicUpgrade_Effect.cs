// Decompiled with JetBrains decompiler
// Type: BionicUpgrade_Effect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

#nullable disable
public class BionicUpgrade_Effect : 
  GameStateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.root;
    this.root.Enter(new StateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.State.Callback(BionicUpgrade_Effect.EnableEffect)).Exit(new StateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.State.Callback(BionicUpgrade_Effect.DisableEffect));
  }

  public static void EnableEffect(BionicUpgrade_Effect.Instance smi) => smi.ApplyEffect();

  public static void DisableEffect(BionicUpgrade_Effect.Instance smi) => smi.RemoveEffect();

  public class Def : StateMachine.BaseDef
  {
    public string EFFECT_NAME;
  }

  public new class Instance : 
    GameStateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.GameInstance
  {
    private Effects effects;

    public Instance(IStateMachineTarget master, BionicUpgrade_Effect.Def def)
      : base(master, def)
    {
      this.effects = this.GetComponent<Effects>();
    }

    public void ApplyEffect()
    {
      this.effects.Add(Db.Get().effects.Get(this.def.EFFECT_NAME), false);
    }

    public void RemoveEffect() => this.effects.Remove(Db.Get().effects.Get(this.def.EFFECT_NAME));
  }
}
