// Decompiled with JetBrains decompiler
// Type: FarmTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FarmTile : StateMachineComponent<FarmTile.SMInstance>
{
  [MyCmpReq]
  private PlantablePlot plantablePlot;
  [MyCmpReq]
  private Storage storage;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public class SMInstance(FarmTile master) : 
    GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.GameInstance(master)
  {
    public bool HasWater()
    {
      PrimaryElement primaryElement = this.master.storage.FindPrimaryElement(SimHashes.Water);
      return (Object) primaryElement != (Object) null && (double) primaryElement.Mass > 0.0;
    }
  }

  public class States : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile>
  {
    public FarmTile.States.FarmStates empty;
    public FarmTile.States.FarmStates full;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.empty;
      this.empty.EventTransition(GameHashes.OccupantChanged, (GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State) this.full, (StateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.Transition.ConditionCallback) (smi => (Object) smi.master.plantablePlot.Occupant != (Object) null));
      this.empty.wet.EventTransition(GameHashes.OnStorageChange, this.empty.dry, (StateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.Transition.ConditionCallback) (smi => !smi.HasWater()));
      this.empty.dry.EventTransition(GameHashes.OnStorageChange, this.empty.wet, (StateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.Transition.ConditionCallback) (smi => !smi.HasWater()));
      this.full.EventTransition(GameHashes.OccupantChanged, (GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State) this.empty, (StateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.Transition.ConditionCallback) (smi => (Object) smi.master.plantablePlot.Occupant == (Object) null));
      this.full.wet.EventTransition(GameHashes.OnStorageChange, this.full.dry, (StateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.Transition.ConditionCallback) (smi => !smi.HasWater()));
      this.full.dry.EventTransition(GameHashes.OnStorageChange, this.full.wet, (StateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.Transition.ConditionCallback) (smi => !smi.HasWater()));
    }

    public class FarmStates : 
      GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State
    {
      public GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State wet;
      public GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State dry;
    }
  }
}
