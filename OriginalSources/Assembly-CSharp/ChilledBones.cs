// Decompiled with JetBrains decompiler
// Type: ChilledBones
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;

#nullable disable
public class ChilledBones : 
  GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>
{
  public const string EFFECT_NAME = "ChilledBones";
  public GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.State normal;
  public GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.State chilled;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.normal;
    this.normal.UpdateTransition(this.chilled, new Func<ChilledBones.Instance, float, bool>(this.IsChilling));
    this.chilled.ToggleEffect(nameof (ChilledBones)).UpdateTransition(this.normal, new Func<ChilledBones.Instance, float, bool>(this.IsNotChilling));
  }

  public bool IsNotChilling(ChilledBones.Instance smi, float dt) => !this.IsChilling(smi, dt);

  public bool IsChilling(ChilledBones.Instance smi, float dt) => smi.IsChilled;

  public class Def : StateMachine.BaseDef
  {
    public float THRESHOLD = -1f;
  }

  public new class Instance : 
    GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.GameInstance
  {
    [MyCmpGet]
    public MinionModifiers minionModifiers;
    public Klei.AI.Attribute bodyTemperatureTransferAttribute;

    public float TemperatureTransferAttribute
    {
      get
      {
        return this.minionModifiers.GetAttributes().GetValue(this.bodyTemperatureTransferAttribute.Id) * 600f;
      }
    }

    public bool IsChilled
    {
      get => (double) this.TemperatureTransferAttribute < (double) this.def.THRESHOLD;
    }

    public Instance(IStateMachineTarget master, ChilledBones.Def def)
      : base(master, def)
    {
      this.bodyTemperatureTransferAttribute = Db.Get().Attributes.TryGet("TemperatureDelta");
    }
  }
}
