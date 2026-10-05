// Decompiled with JetBrains decompiler
// Type: PlanterBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
public class PlanterBox : StateMachineComponent<PlanterBox.SMInstance>
{
  [MyCmpReq]
  private PlantablePlot plantablePlot;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public class SMInstance(PlanterBox master) : 
    GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.GameInstance(master)
  {
  }

  public class States : GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox>
  {
    public GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.State empty;
    public GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.State full;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.empty;
      this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (StateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.Transition.ConditionCallback) (smi => (Object) smi.master.plantablePlot.Occupant != (Object) null)).PlayAnim("off");
      this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (StateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.Transition.ConditionCallback) (smi => (Object) smi.master.plantablePlot.Occupant == (Object) null)).PlayAnim("on");
    }
  }
}
