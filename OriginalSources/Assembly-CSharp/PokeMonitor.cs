// Decompiled with JetBrains decompiler
// Type: PokeMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class PokeMonitor : StateMachineComponent<PokeMonitor.Instance>
{
  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  private static void ClearTarget(PokeMonitor.Instance smi) => smi.AbortPoke();

  public class States : GameStateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor>
  {
    public StateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.TargetParameter target;
    public GameStateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.State noTarget;
    public GameStateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.State hasTarget;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = StateMachine.SerializeType.Never;
      default_state = (StateMachine.BaseState) this.noTarget;
      this.noTarget.ParamTransition<GameObject>((StateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.Parameter<GameObject>) this.target, this.hasTarget, GameStateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.IsNotNull);
      this.hasTarget.ParamTransition<GameObject>((StateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.Parameter<GameObject>) this.target, this.noTarget, GameStateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.IsNull).ToggleBehaviour(GameTags.Creatures.UrgeToPoke, (StateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.Transition.ConditionCallback) (smi => true), new Action<PokeMonitor.Instance>(PokeMonitor.ClearTarget));
    }
  }

  public class Instance(PokeMonitor master) : 
    GameStateMachine<PokeMonitor.States, PokeMonitor.Instance, PokeMonitor, object>.GameInstance(master)
  {
    public CellOffset[] TargetOffsets = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };

    public GameObject Target => this.sm.target.Get(this);

    public void InitiatePoke(GameObject target)
    {
      this.InitiatePoke(target, new CellOffset[1]
      {
        new CellOffset(0, 0)
      });
    }

    public void InitiatePoke(GameObject target, CellOffset[] pokeOffesets)
    {
      this.sm.target.Set(target, this, false);
      this.TargetOffsets = pokeOffesets;
    }

    public void AbortPoke()
    {
      this.sm.target.Set((KMonoBehaviour) null, this);
      this.TargetOffsets = new CellOffset[1]
      {
        new CellOffset(0, 0)
      };
    }
  }
}
