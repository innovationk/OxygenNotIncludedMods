// Decompiled with JetBrains decompiler
// Type: RemoteWorkerCapacitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
public class RemoteWorkerCapacitor : StateMachineComponent<RemoteWorkerCapacitor.StatesInstance>
{
  [Serialize]
  private float charge;
  public const float LOW_LEVEL = 12f;
  public const float POWER_USE_RATE_J_PER_S = -0.1f;
  public const float POWER_CHARGE_RATE_J_PER_S = 7.5f;
  public const float CAPACITY_J = 60f;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public float ApplyDeltaEnergy(float delta)
  {
    float charge = this.charge;
    this.charge = Mathf.Clamp(this.charge + delta, 0.0f, 60f);
    return this.charge - charge;
  }

  public float ChargeRatio => this.charge / 60f;

  public float Charge => this.charge;

  public bool IsLowPower => (double) this.charge < 12.0;

  public bool IsOutOfPower => (double) this.charge < 1.4012984643248171E-45;

  public class StatesInstance(RemoteWorkerCapacitor master) : 
    GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor>
  {
    private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State ok;
    private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State low_power;
    private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State out_of_power;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      base.InitializeStates(out default_state);
      default_state = (StateMachine.BaseState) this.ok;
      this.root.ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerCapacitorStatus, (Func<RemoteWorkerCapacitor.StatesInstance, object>) (smi => (object) smi.master));
      this.ok.Transition(this.out_of_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOutOfPower)).Transition(this.low_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsLowPower));
      this.low_power.Transition(this.out_of_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOutOfPower)).Transition(this.ok, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOkForPower)).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerLowPower);
      this.out_of_power.Transition(this.low_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsLowPower)).Transition(this.ok, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOkForPower)).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerOutOfPower);
    }

    public static bool IsOkForPower(RemoteWorkerCapacitor.StatesInstance smi)
    {
      return !smi.master.IsLowPower;
    }

    public static bool IsLowPower(RemoteWorkerCapacitor.StatesInstance smi)
    {
      return smi.master.IsLowPower && !smi.master.IsOutOfPower;
    }

    public static bool IsOutOfPower(RemoteWorkerCapacitor.StatesInstance smi)
    {
      return smi.master.IsOutOfPower;
    }
  }
}
