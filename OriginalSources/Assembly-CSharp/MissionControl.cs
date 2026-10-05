// Decompiled with JetBrains decompiler
// Type: MissionControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class MissionControl : 
  GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>
{
  public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State Inoperational;
  public MissionControl.OperationalState Operational;
  public StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.BoolParameter WorkableRocketsAreInRange;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.Inoperational;
    this.Inoperational.EventTransition(GameHashes.OperationalChanged, (GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State) this.Operational, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, (GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State) this.Operational, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition));
    this.Operational.EventTransition(GameHashes.OperationalChanged, this.Inoperational, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational.WrongRoom, GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Not(new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.IsInLabRoom))).Enter(new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State.Callback(this.OnEnterOperational)).DefaultState(this.Operational.NoRockets).Update((System.Action<MissionControl.Instance, float>) ((smi, dt) => smi.UpdateWorkableRockets((object) null)), UpdateRate.SIM_1000ms);
    this.Operational.WrongRoom.EventTransition(GameHashes.UpdateRoom, this.Operational.NoRockets, new StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Transition.ConditionCallback(this.IsInLabRoom));
    this.Operational.NoRockets.ToggleStatusItem(Db.Get().BuildingStatusItems.NoRocketsToMissionControlBoost).ParamTransition<bool>((StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Parameter<bool>) this.WorkableRocketsAreInRange, this.Operational.HasRockets, (StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Parameter<bool>.Callback) ((smi, inRange) => this.WorkableRocketsAreInRange.Get(smi)));
    this.Operational.HasRockets.ParamTransition<bool>((StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Parameter<bool>) this.WorkableRocketsAreInRange, this.Operational.NoRockets, (StateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.Parameter<bool>.Callback) ((smi, inRange) => !this.WorkableRocketsAreInRange.Get(smi))).ToggleChore(new Func<MissionControl.Instance, Chore>(this.CreateChore), (GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State) this.Operational);
  }

  private Chore CreateChore(MissionControl.Instance smi)
  {
    MissionControlWorkable component = smi.master.gameObject.GetComponent<MissionControlWorkable>();
    WorkChore<MissionControlWorkable> chore = new WorkChore<MissionControlWorkable>(Db.Get().ChoreTypes.Research, (IStateMachineTarget) component);
    Spacecraft boostableSpacecraft = smi.GetRandomBoostableSpacecraft();
    component.TargetSpacecraft = boostableSpacecraft;
    return (Chore) chore;
  }

  private void OnEnterOperational(MissionControl.Instance smi)
  {
    smi.UpdateWorkableRockets((object) null);
    if (this.WorkableRocketsAreInRange.Get(smi))
      smi.GoTo((StateMachine.BaseState) this.Operational.HasRockets);
    else
      smi.GoTo((StateMachine.BaseState) this.Operational.NoRockets);
  }

  private bool ValidateOperationalTransition(MissionControl.Instance smi)
  {
    global::Operational component = smi.GetComponent<global::Operational>();
    bool flag = smi.IsInsideState((StateMachine.BaseState) smi.sm.Operational);
    return (UnityEngine.Object) component != (UnityEngine.Object) null && flag != component.IsOperational;
  }

  private bool IsInLabRoom(MissionControl.Instance smi) => smi.roomTracker.IsInCorrectRoom();

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, MissionControl.Def def) : 
    GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.GameInstance(master, def)
  {
    private List<Spacecraft> boostableSpacecraft = new List<Spacecraft>();
    [MyCmpReq]
    public RoomTracker roomTracker;

    public void UpdateWorkableRockets(object data)
    {
      this.boostableSpacecraft.Clear();
      for (int index = 0; index < SpacecraftManager.instance.GetSpacecraft().Count; ++index)
      {
        if (this.CanBeBoosted(SpacecraftManager.instance.GetSpacecraft()[index]))
        {
          bool flag = false;
          foreach (MissionControlWorkable missionControlWorkable in Components.MissionControlWorkables)
          {
            if (!((UnityEngine.Object) missionControlWorkable.gameObject == (UnityEngine.Object) this.gameObject) && missionControlWorkable.TargetSpacecraft == SpacecraftManager.instance.GetSpacecraft()[index])
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            this.boostableSpacecraft.Add(SpacecraftManager.instance.GetSpacecraft()[index]);
        }
      }
      this.sm.WorkableRocketsAreInRange.Set(this.boostableSpacecraft.Count > 0, this.smi);
    }

    public Spacecraft GetRandomBoostableSpacecraft()
    {
      return this.boostableSpacecraft.GetRandom<Spacecraft>();
    }

    private bool CanBeBoosted(Spacecraft spacecraft)
    {
      return (double) spacecraft.controlStationBuffTimeRemaining == 0.0 && spacecraft.state == Spacecraft.MissionState.Underway;
    }

    public void ApplyEffect(Spacecraft spacecraft)
    {
      spacecraft.controlStationBuffTimeRemaining = 600f;
    }
  }

  public class OperationalState : 
    GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State
  {
    public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State WrongRoom;
    public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State NoRockets;
    public GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def>.State HasRockets;
  }
}
