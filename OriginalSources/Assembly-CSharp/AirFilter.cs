// Decompiled with JetBrains decompiler
// Type: AirFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class AirFilter : StateMachineComponent<AirFilter.StatesInstance>, IGameObjectEffectDescriptor
{
  [MyCmpGet]
  private Operational operational;
  [MyCmpGet]
  private Storage storage;
  [MyCmpGet]
  private ElementConverter elementConverter;
  [MyCmpGet]
  private ElementConsumer elementConsumer;
  public Tag filterTag;

  public bool HasFilter() => this.elementConverter.HasEnoughMass(this.filterTag);

  public bool IsConvertable() => this.elementConverter.HasEnoughMassToStartConverting();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public List<Descriptor> GetDescriptors(GameObject go) => (List<Descriptor>) null;

  public class StatesInstance(AirFilter smi) : 
    GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.GameInstance(smi)
  {
  }

  public class States : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter>
  {
    public AirFilter.States.ReadyStates hasFilter;
    public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State waiting;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.waiting;
      this.waiting.EventTransition(GameHashes.OnStorageChange, (GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State) this.hasFilter, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => smi.master.HasFilter() && smi.master.operational.IsOperational)).EventTransition(GameHashes.OperationalChanged, (GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State) this.hasFilter, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => smi.master.HasFilter() && smi.master.operational.IsOperational));
      this.hasFilter.EventTransition(GameHashes.OperationalChanged, this.waiting, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).Enter("EnableConsumption", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(true))).Exit("DisableConsumption", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.elementConsumer.EnableConsumption(false))).DefaultState(this.hasFilter.idle);
      this.hasFilter.idle.EventTransition(GameHashes.OnStorageChange, this.hasFilter.converting, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => smi.master.IsConvertable()));
      this.hasFilter.converting.Enter("SetActive(true)", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.operational.SetActive(true))).Exit("SetActive(false)", (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State.Callback) (smi => smi.master.operational.SetActive(false))).EventTransition(GameHashes.OnStorageChange, this.hasFilter.idle, (StateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.Transition.ConditionCallback) (smi => !smi.master.IsConvertable()));
    }

    public class ReadyStates : 
      GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State
    {
      public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State idle;
      public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State converting;
    }
  }
}
