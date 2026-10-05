// Decompiled with JetBrains decompiler
// Type: PlanktonCoral
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public class PlanktonCoral : 
  GameStateMachine<PlanktonCoral, PlanktonCoral.Instance, IStateMachineTarget, PlanktonCoral.Def>
{
  public const string INHALE_ANIM_NAME = "inhale";
  public const string EXHALE_ANIM_NAME = "exhale";
  public GameStateMachine<PlanktonCoral, PlanktonCoral.Instance, IStateMachineTarget, PlanktonCoral.Def>.State wilted;
  public PlanktonCoral.HealthyStates healthy;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.healthy;
    this.wilted.EventTransition(GameHashes.WiltRecover, (GameStateMachine<PlanktonCoral, PlanktonCoral.Instance, IStateMachineTarget, PlanktonCoral.Def>.State) this.healthy, GameStateMachine<PlanktonCoral, PlanktonCoral.Instance, IStateMachineTarget, PlanktonCoral.Def>.Not(new StateMachine<PlanktonCoral, PlanktonCoral.Instance, IStateMachineTarget, PlanktonCoral.Def>.Transition.ConditionCallback(PlanktonCoral.IsWilted)));
    this.healthy.DefaultState(this.healthy.breathing);
    this.healthy.breathing.EventTransition(GameHashes.Grow, this.healthy.clogged, new StateMachine<PlanktonCoral, PlanktonCoral.Instance, IStateMachineTarget, PlanktonCoral.Def>.Transition.ConditionCallback(PlanktonCoral.IsFullyGrown)).EventTransition(GameHashes.Wilt, this.wilted, new StateMachine<PlanktonCoral, PlanktonCoral.Instance, IStateMachineTarget, PlanktonCoral.Def>.Transition.ConditionCallback(PlanktonCoral.IsWilted));
    this.healthy.clogged.EventTransition(GameHashes.Harvest, this.healthy.breathing);
  }

  public static bool IsWilted(PlanktonCoral.Instance smi) => smi.IsWilted;

  public static bool IsFullyGrown(PlanktonCoral.Instance smi) => smi.IsFullyGrown;

  public class Def : StateMachine.BaseDef
  {
  }

  public class HealthyStates : 
    GameStateMachine<PlanktonCoral, PlanktonCoral.Instance, IStateMachineTarget, PlanktonCoral.Def>.State
  {
    public GameStateMachine<PlanktonCoral, PlanktonCoral.Instance, IStateMachineTarget, PlanktonCoral.Def>.State breathing;
    public GameStateMachine<PlanktonCoral, PlanktonCoral.Instance, IStateMachineTarget, PlanktonCoral.Def>.State clogged;
  }

  public new class Instance : 
    GameStateMachine<PlanktonCoral, PlanktonCoral.Instance, IStateMachineTarget, PlanktonCoral.Def>.GameInstance
  {
    private WiltCondition wiltCondition;
    private Growing growing;
    public AttributeModifier GrowModifier;

    public bool IsFullyGrown => (Object) this.growing != (Object) null && this.growing.IsGrown();

    public bool IsWilted
    {
      get => (Object) this.wiltCondition != (Object) null && this.wiltCondition.IsWilting();
    }

    public Instance(IStateMachineTarget master, PlanktonCoral.Def def)
      : base(master, def)
    {
      this.wiltCondition = this.GetComponent<WiltCondition>();
      this.growing = this.GetComponent<Growing>();
    }
  }
}
