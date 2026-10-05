// Decompiled with JetBrains decompiler
// Type: RobotAi
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class RobotAi : GameStateMachine<RobotAi, RobotAi.Instance>
{
  public RobotAi.AliveStates alive;
  public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State dead;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleStateMachine((Func<RobotAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new DeathMonitor.Instance(smi.master, new DeathMonitor.Def()))).Enter((StateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if (smi.HasTag(GameTags.Dead))
        smi.GoTo((StateMachine.BaseState) this.dead);
      else
        smi.GoTo((StateMachine.BaseState) this.alive);
    }));
    this.alive.DefaultState(this.alive.normal).TagTransition(GameTags.Dead, this.dead).Toggle("Toggle Component Registration", (StateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State.Callback) (smi => RobotAi.ToggleRegistration(smi, true)), (StateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State.Callback) (smi => RobotAi.ToggleRegistration(smi, false)));
    this.alive.normal.TagTransition(GameTags.Stored, this.alive.stored).Enter((StateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if (smi.HasTag(GameTags.Robots.Models.FetchDrone))
        return;
      smi.fallMonitor = new FallMonitor.Instance(smi.master, false);
      smi.fallMonitor.StartSM();
    })).Exit((StateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if (smi.fallMonitor == null)
        return;
      smi.fallMonitor.StopSM("StoredRobotAI");
    }));
    this.alive.stored.PlayAnim("in_storage").TagTransition(GameTags.Stored, this.alive.normal, true).ToggleBrain("stored").Enter((StateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.GetComponent<Navigator>().Pause("stored"))).Exit((StateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.GetComponent<Navigator>().Unpause("unstored")));
    this.dead.ToggleBrain("dead").ToggleComponentIfFound<Deconstructable>().ToggleStateMachine((Func<RobotAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new FallWhenDeadMonitor.Instance(smi.master))).Enter("RefreshUserMenu", (StateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.RefreshUserMenu())).Enter("DropStorage", (StateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.GetComponent<Storage>().DropAll())).Enter("Delete", new StateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State.Callback(RobotAi.DeleteOnDeath));
  }

  public static void DeleteOnDeath(RobotAi.Instance smi)
  {
    if (!((RobotAi.Def) smi.def).DeleteOnDead)
      return;
    smi.gameObject.DeleteObject();
  }

  private static void ToggleRegistration(RobotAi.Instance smi, bool register)
  {
    if (register)
      Components.LiveRobotsIdentities.Add(smi);
    else
      Components.LiveRobotsIdentities.Remove(smi);
  }

  public class Def : StateMachine.BaseDef
  {
    public bool DeleteOnDead;
  }

  public class AliveStates : 
    GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State normal;
    public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State stored;
  }

  public new class Instance : 
    GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.GameInstance
  {
    public FallMonitor.Instance fallMonitor;
    private int onBeginChoreHandlerID;

    public Instance(IStateMachineTarget master, RobotAi.Def def)
      : base(master, (object) def)
    {
      ChoreConsumer component = this.GetComponent<ChoreConsumer>();
      component.AddUrge(Db.Get().Urges.EmoteHighPriority);
      component.AddUrge(Db.Get().Urges.EmoteIdle);
      this.onBeginChoreHandlerID = this.Subscribe(-1988963660, new System.Action<object>(this.OnBeginChore));
    }

    private void OnBeginChore(object data)
    {
      Storage component = this.GetComponent<Storage>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.DropAll();
    }

    protected override void OnCleanUp()
    {
      this.Unsubscribe(ref this.onBeginChoreHandlerID);
      base.OnCleanUp();
    }

    public void RefreshUserMenu() => Game.Instance.userMenu.Refresh(this.master.gameObject);
  }
}
