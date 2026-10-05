// Decompiled with JetBrains decompiler
// Type: RustDeoxidizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class RustDeoxidizer : StateMachineComponent<RustDeoxidizer.StatesInstance>
{
  [SerializeField]
  public float maxMass = 2.5f;
  [MyCmpAdd]
  private Storage storage;
  [MyCmpGet]
  private ElementConverter emitter;
  [MyCmpReq]
  private Operational operational;
  private MeterController meter;
  private readonly Func<int, bool> overPressure;

  public RustDeoxidizer()
  {
    this.overPressure = (Func<int, bool>) (cell => (double) Grid.Mass[cell] > (double) this.maxMass);
  }

  protected override void OnSpawn()
  {
    this.smi.StartSM();
    Tutorial.Instance.oxygenGenerators.Add(this.gameObject);
  }

  protected override void OnCleanUp()
  {
    Tutorial.Instance.oxygenGenerators.Remove(this.gameObject);
    base.OnCleanUp();
  }

  private bool RoomForPressure
  {
    get
    {
      return !FloodFill.Any<FloodFill.MaxDepth>(this.overPressure, Grid.CellAbove(Grid.PosToCell(this.transform.GetPosition())), new FloodFill.MaxDepth(3), true, true);
    }
  }

  public class StatesInstance(RustDeoxidizer smi) : 
    GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.GameInstance(smi)
  {
  }

  public class States : 
    GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer>
  {
    public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State disabled;
    public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State waiting;
    public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State converting;
    public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State overpressure;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.disabled;
      this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational));
      this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.waiting.Enter("Waiting", (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State.Callback) (smi => smi.master.operational.SetActive(false))).EventTransition(GameHashes.OnStorageChange, this.converting, (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting()));
      this.converting.Enter("Ready", (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State.Callback) (smi => smi.master.operational.SetActive(true))).Transition(this.waiting, (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.Transition.ConditionCallback) (smi => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll())).Transition(this.overpressure, (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.Transition.ConditionCallback) (smi => !smi.master.RoomForPressure));
      this.overpressure.Enter("OverPressure", (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State.Callback) (smi => smi.master.operational.SetActive(false))).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk).Transition(this.converting, (StateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.Transition.ConditionCallback) (smi => smi.master.RoomForPressure));
    }
  }
}
