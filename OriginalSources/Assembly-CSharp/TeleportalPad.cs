// Decompiled with JetBrains decompiler
// Type: TeleportalPad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class TeleportalPad : StateMachineComponent<TeleportalPad.StatesInstance>
{
  [MyCmpReq]
  private Operational operational;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public class StatesInstance(TeleportalPad master) : 
    GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad>
  {
    public StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.Signal targetTeleporter;
    public StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.Signal doTeleport;
    public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State inactive;
    public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State no_target;
    public TeleportalPad.States.PortalOnStates portal_on;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.inactive;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.root.EventTransition(GameHashes.OperationalChanged, this.inactive, (StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational));
      this.inactive.PlayAnim("idle").EventTransition(GameHashes.OperationalChanged, this.no_target, (StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
      this.no_target.Enter((StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State.Callback) (smi =>
      {
        if (!smi.master.GetComponent<Teleporter>().HasTeleporterTarget())
          return;
        smi.GoTo((StateMachine.BaseState) this.portal_on.turn_on);
      })).PlayAnim("idle").EventTransition(GameHashes.TeleporterIDsChanged, this.portal_on.turn_on, (StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<Teleporter>().HasTeleporterTarget()));
      this.portal_on.EventTransition(GameHashes.TeleporterIDsChanged, this.portal_on.turn_off, (StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.Transition.ConditionCallback) (smi => !smi.master.GetComponent<Teleporter>().HasTeleporterTarget()));
      this.portal_on.turn_on.PlayAnim("working_pre").OnAnimQueueComplete(this.portal_on.loop);
      this.portal_on.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Update((Action<TeleportalPad.StatesInstance, float>) ((smi, dt) =>
      {
        Teleporter component = smi.master.GetComponent<Teleporter>();
        Teleporter teleportTarget = component.FindTeleportTarget();
        component.SetTeleportTarget(teleportTarget);
        if (!((UnityEngine.Object) teleportTarget != (UnityEngine.Object) null))
          return;
        component.TeleportObjects();
      }));
      this.portal_on.turn_off.PlayAnim("working_pst").OnAnimQueueComplete(this.no_target);
    }

    public class PortalOnStates : 
      GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State
    {
      public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State turn_on;
      public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State loop;
      public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State turn_off;
    }
  }
}
