// Decompiled with JetBrains decompiler
// Type: RobotElectroBankDeadStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class RobotElectroBankDeadStates : 
  GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>
{
  public RobotElectroBankDeadStates.PowerDown powerdown;
  public RobotElectroBankDeadStates.PowerUp powerup;
  public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.powerdown;
    this.powerdown.DefaultState(this.powerdown.pre).ToggleStatusItem(Db.Get().RobotStatusItems.DeadBatteryFlydo, (Func<RobotElectroBankDeadStates.Instance, object>) (smi => (object) smi.gameObject), Db.Get().StatusItemCategories.Main).EventTransition(GameHashes.OnStorageChange, this.powerup.grounded, (StateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.Transition.ConditionCallback) (smi => RobotElectroBankDeadStates.ElectrobankDelivered(smi))).Exit((StateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State.Callback) (smi =>
    {
      if (!GameComps.Fallers.Has((object) smi.gameObject))
        return;
      GameComps.Fallers.Remove(smi.gameObject);
    }));
    this.powerdown.pre.PlayAnim("power_down_pre").OnAnimQueueComplete(this.powerdown.fall);
    this.powerdown.fall.PlayAnim("power_down_loop", KAnim.PlayMode.Loop).Enter((StateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State.Callback) (smi =>
    {
      if (GameComps.Fallers.Has((object) smi.gameObject))
        return;
      GameComps.Fallers.Add(smi.gameObject, Vector2.zero);
    })).Update((System.Action<RobotElectroBankDeadStates.Instance, float>) ((smi, dt) =>
    {
      if (GameComps.Gravities.Has((object) smi.gameObject))
        return;
      smi.GoTo((StateMachine.BaseState) this.powerdown.landed);
    })).EventTransition(GameHashes.Landed, this.powerdown.landed);
    this.powerdown.landed.PlayAnim("power_down_pst").Enter((StateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State.Callback) (smi => smi.GetComponent<LoopingSounds>().PauseSound(GlobalAssets.GetSound("Flydo_flying_LP"), true))).OnAnimQueueComplete(this.powerdown.dead);
    this.powerdown.dead.PlayAnim("dead_battery").EventTransition(GameHashes.OnStorageChange, this.powerup.grounded, (StateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.Transition.ConditionCallback) (smi => RobotElectroBankDeadStates.ElectrobankDelivered(smi)));
    this.powerup.Exit((StateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State.Callback) (smi =>
    {
      smi.GetComponent<LoopingSounds>().PauseSound(GlobalAssets.GetSound("Flydo_flying_LP"), false);
      smi.Get<Brain>().Resume("power up");
    }));
    this.powerup.grounded.PlayAnim("battery_change_dead").OnAnimQueueComplete(this.powerup.takeoff);
    this.powerup.takeoff.PlayAnim("power_up").OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.NoElectroBank);
  }

  private static bool ElectrobankDelivered(RobotElectroBankDeadStates.Instance smi)
  {
    foreach (Storage component in smi.gameObject.GetComponents<Storage>())
    {
      if (component.storageID == GameTags.ChargedPortableBattery)
        return component.Has(GameTags.ChargedPortableBattery);
    }
    return false;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class PowerDown : 
    GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State
  {
    public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State pre;
    public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State fall;
    public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State landed;
    public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State dead;
  }

  public class PowerUp : 
    GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State
  {
    public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State grounded;
    public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State takeoff;
  }

  public new class Instance : 
    GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.GameInstance
  {
    public Instance(
      Chore<RobotElectroBankDeadStates.Instance> chore,
      RobotElectroBankDeadStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.choreType.interruptPriority = Db.Get().ChoreTypes.Die.interruptPriority;
      chore.masterPriority.priority_class = PriorityScreen.PriorityClass.compulsory;
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Robots.Behaviours.NoElectroBank);
    }
  }
}
