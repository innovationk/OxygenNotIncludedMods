// Decompiled with JetBrains decompiler
// Type: LubricatedMovementMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

#nullable disable
public class LubricatedMovementMonitor : 
  GameStateMachine<LubricatedMovementMonitor, LubricatedMovementMonitor.Instance, IStateMachineTarget, LubricatedMovementMonitor.Def>
{
  public GameStateMachine<LubricatedMovementMonitor, LubricatedMovementMonitor.Instance, IStateMachineTarget, LubricatedMovementMonitor.Def>.State idle;
  public GameStateMachine<LubricatedMovementMonitor, LubricatedMovementMonitor.Instance, IStateMachineTarget, LubricatedMovementMonitor.Def>.State moving;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.EnterTransition(this.moving, (StateMachine<LubricatedMovementMonitor, LubricatedMovementMonitor.Instance, IStateMachineTarget, LubricatedMovementMonitor.Def>.Transition.ConditionCallback) (smi => smi.GetComponent<Navigator>().IsMoving())).EventHandlerTransition(GameHashes.ObjectMovementStateChanged, this.moving, new Func<LubricatedMovementMonitor.Instance, object, bool>(this.IsMoving));
    this.moving.ToggleAttributeModifier("DryingOutVeryFast", (Func<LubricatedMovementMonitor.Instance, AttributeModifier>) (smi => smi.movementMoistureModifier)).EventHandlerTransition(GameHashes.ObjectMovementStateChanged, this.idle, (Func<LubricatedMovementMonitor.Instance, object, bool>) ((smi, data) => !this.IsMoving(smi, data)));
  }

  private bool IsMoving(LubricatedMovementMonitor.Instance smi, object data)
  {
    return data is GameHashes.ObjectMovementWakeUp;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<LubricatedMovementMonitor, LubricatedMovementMonitor.Instance, IStateMachineTarget, LubricatedMovementMonitor.Def>.GameInstance
  {
    public AmountInstance moisture;
    public AttributeModifier movementMoistureModifier;
    public float movingDryRate = -0.8333333f;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.moisture = Db.Get().Amounts.Moisture.Lookup(this.gameObject);
      this.movementMoistureModifier = new AttributeModifier(this.moisture.amount.deltaAttribute.Id, this.movingDryRate, (string) CREATURES.MODIFIERS.MOVEMENT_MOISTURE_LOSS.NAME);
    }
  }
}
