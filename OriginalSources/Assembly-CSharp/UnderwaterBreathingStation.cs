// Decompiled with JetBrains decompiler
// Type: UnderwaterBreathingStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class UnderwaterBreathingStation : 
  GameStateMachine<UnderwaterBreathingStation, UnderwaterBreathingStation.Instance, IStateMachineTarget, UnderwaterBreathingStation.Def>
{
  private const string METER_TARGET_NAME = "meter_target";
  private const string METER_ANIM_NAME = "meter";
  public GameStateMachine<UnderwaterBreathingStation, UnderwaterBreathingStation.Instance, IStateMachineTarget, UnderwaterBreathingStation.Def>.State off;
  public GameStateMachine<UnderwaterBreathingStation, UnderwaterBreathingStation.Instance, IStateMachineTarget, UnderwaterBreathingStation.Def>.State on;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<UnderwaterBreathingStation, UnderwaterBreathingStation.Instance, IStateMachineTarget, UnderwaterBreathingStation.Def>.State.Callback(UnderwaterBreathingStation.RefreshMeter));
    this.off.EventTransition(GameHashes.OperationalChanged, this.on, (StateMachine<UnderwaterBreathingStation, UnderwaterBreathingStation.Instance, IStateMachineTarget, UnderwaterBreathingStation.Def>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational)).Enter(new StateMachine<UnderwaterBreathingStation, UnderwaterBreathingStation.Instance, IStateMachineTarget, UnderwaterBreathingStation.Def>.State.Callback(UnderwaterBreathingStation.RemoveCells));
    this.on.EventTransition(GameHashes.OperationalChanged, this.off, (StateMachine<UnderwaterBreathingStation, UnderwaterBreathingStation.Instance, IStateMachineTarget, UnderwaterBreathingStation.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).Enter(new StateMachine<UnderwaterBreathingStation, UnderwaterBreathingStation.Instance, IStateMachineTarget, UnderwaterBreathingStation.Def>.State.Callback(UnderwaterBreathingStation.AddCells)).Exit(new StateMachine<UnderwaterBreathingStation, UnderwaterBreathingStation.Instance, IStateMachineTarget, UnderwaterBreathingStation.Def>.State.Callback(UnderwaterBreathingStation.RemoveCells));
  }

  private static void RefreshMeter(UnderwaterBreathingStation.Instance smi) => smi.RefreshMeter();

  private static void AddCells(UnderwaterBreathingStation.Instance smi) => smi.location.MarkCells();

  private static void RemoveCells(UnderwaterBreathingStation.Instance smi)
  {
    smi.location.UnmarkCells();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<UnderwaterBreathingStation, UnderwaterBreathingStation.Instance, IStateMachineTarget, UnderwaterBreathingStation.Def>.GameInstance
  {
    public UnderwaterBreathingLocation location;
    private Storage storage;
    private MeterController meter;

    public Instance(IStateMachineTarget master, UnderwaterBreathingStation.Def def)
      : base(master, def)
    {
      this.storage = this.GetComponent<Storage>();
      this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", nameof (meter), Meter.Offset.Infront, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
    }

    public override void StartSM()
    {
      this.location = this.GetComponent<UnderwaterBreathingLocation>();
      base.StartSM();
      this.RefreshMeter();
    }

    protected override void OnCleanUp()
    {
      this.location.UnmarkCells();
      base.OnCleanUp();
    }

    public void RefreshMeter()
    {
      this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.capacityKg);
    }
  }
}
