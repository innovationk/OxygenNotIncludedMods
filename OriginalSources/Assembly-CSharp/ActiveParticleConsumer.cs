// Decompiled with JetBrains decompiler
// Type: ActiveParticleConsumer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ActiveParticleConsumer : 
  GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>
{
  public static readonly Operational.Flag canConsumeParticlesFlag = new Operational.Flag("canConsumeParticles", Operational.Flag.Type.Requirement);
  public GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State inoperational;
  public ActiveParticleConsumer.OperationalStates operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.inoperational;
    this.root.Enter((StateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State.Callback) (smi => smi.GetComponent<Operational>().SetFlag(ActiveParticleConsumer.canConsumeParticlesFlag, false)));
    this.inoperational.EventTransition(GameHashes.OnParticleStorageChanged, (GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State) this.operational, new StateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.Transition.ConditionCallback(this.IsReady)).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForHighEnergyParticles);
    this.operational.DefaultState(this.operational.waiting).EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational, GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.Not(new StateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.Transition.ConditionCallback(this.IsReady))).ToggleOperationalFlag(ActiveParticleConsumer.canConsumeParticlesFlag);
    this.operational.waiting.EventTransition(GameHashes.ActiveChanged, this.operational.consuming, (StateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsActive));
    this.operational.consuming.EventTransition(GameHashes.ActiveChanged, this.operational.waiting, (StateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive)).Update((System.Action<ActiveParticleConsumer.Instance, float>) ((smi, dt) => smi.Update(dt)), UpdateRate.SIM_1000ms);
  }

  public bool IsReady(ActiveParticleConsumer.Instance smi)
  {
    return (double) smi.storage.Particles >= (double) smi.def.minParticlesForOperational;
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public float activeConsumptionRate = 1f;
    public float minParticlesForOperational = 1f;
    public string meterSymbolName;

    public List<Descriptor> GetDescriptors(GameObject go)
    {
      return new List<Descriptor>()
      {
        new Descriptor(UI.BUILDINGEFFECTS.ACTIVE_PARTICLE_CONSUMPTION.Replace("{Rate}", GameUtil.GetFormattedHighEnergyParticles(this.activeConsumptionRate, GameUtil.TimeSlice.PerSecond)), UI.BUILDINGEFFECTS.TOOLTIPS.ACTIVE_PARTICLE_CONSUMPTION.Replace("{Rate}", GameUtil.GetFormattedHighEnergyParticles(this.activeConsumptionRate, GameUtil.TimeSlice.PerSecond)), Descriptor.DescriptorType.Requirement)
      };
    }
  }

  public class OperationalStates : 
    GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State
  {
    public GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State waiting;
    public GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State consuming;
  }

  public new class Instance : 
    GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.GameInstance
  {
    public bool ShowWorkingStatus;
    public HighEnergyParticleStorage storage;

    public Instance(IStateMachineTarget master, ActiveParticleConsumer.Def def)
      : base(master, def)
    {
      this.storage = master.GetComponent<HighEnergyParticleStorage>();
    }

    public void Update(float dt)
    {
      double num = (double) this.storage.ConsumeAndGet(dt * this.def.activeConsumptionRate);
    }
  }
}
