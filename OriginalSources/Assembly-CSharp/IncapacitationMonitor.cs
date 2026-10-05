// Decompiled with JetBrains decompiler
// Type: IncapacitationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
public class IncapacitationMonitor : 
  GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance>
{
  public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State healthy;
  public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State recovered;
  public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State incapacitated;
  public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State die;
  private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter bleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);
  private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseBleedOutSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);
  private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseStaminaRecoverSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);
  private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter maxBleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.healthy;
    this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
    this.healthy.Update((System.Action<IncapacitationMonitor.Instance, float>) ((smi, dt) => smi.RecoverBleedOutStamina(dt, smi))).EventTransition(GameHashes.BecameIncapacitated, this.incapacitated);
    this.incapacitated.EventTransition(GameHashes.IncapacitationRecovery, this.recovered).ToggleTag(GameTags.Incapacitated).ToggleRecurringChore((Func<IncapacitationMonitor.Instance, Chore>) (smi => (Chore) new BeIncapacitatedChore(smi.master))).ParamTransition<float>((StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.Parameter<float>) this.bleedOutStamina, this.die, GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.IsLTEZero).ToggleUrge(Db.Get().Urges.BeIncapacitated).Update((System.Action<IncapacitationMonitor.Instance, float>) ((smi, dt) => smi.Bleed(dt, smi)));
    this.recovered.Enter((StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.ApplyRecoverEffect())).GoTo(this.healthy);
    this.die.Enter((StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.master.gameObject.GetSMI<DeathMonitor.Instance>().Kill(smi.GetCauseOfIncapacitation())));
  }

  public new class Instance : 
    GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
      Health component = master.GetComponent<Health>();
      if (!(bool) (UnityEngine.Object) component)
        return;
      component.canBeIncapacitated = true;
    }

    public void Bleed(float dt, IncapacitationMonitor.Instance smi)
    {
      double num = (double) smi.sm.bleedOutStamina.Delta(dt * -smi.sm.baseBleedOutSpeed.Get(smi), smi);
    }

    public void RecoverBleedOutStamina(float dt, IncapacitationMonitor.Instance smi)
    {
      double num = (double) smi.sm.bleedOutStamina.Delta(Mathf.Min(dt * smi.sm.baseStaminaRecoverSpeed.Get(smi), smi.sm.maxBleedOutStamina.Get(smi) - smi.sm.bleedOutStamina.Get(smi)), smi);
    }

    public float GetBleedLifeTime(IncapacitationMonitor.Instance smi)
    {
      return Mathf.Floor(smi.sm.bleedOutStamina.Get(smi) / smi.sm.baseBleedOutSpeed.Get(smi));
    }

    public Death GetCauseOfIncapacitation()
    {
      Health component = this.GetComponent<Health>();
      if (component.CauseOfIncapacitation == GameTags.RadiationSicknessIncapacitation)
        return Db.Get().Deaths.Radiation;
      return component.CauseOfIncapacitation == GameTags.HitPointsDepleted ? Db.Get().Deaths.Slain : Db.Get().Deaths.Generic;
    }

    public void ApplyRecoverEffect() => this.smi.Get<Effects>().Add("NearDeathExperience", true);
  }
}
