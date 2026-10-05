// Decompiled with JetBrains decompiler
// Type: RationalAi
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class RationalAi : GameStateMachine<RationalAi, RationalAi.Instance>
{
  public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State alive;
  public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State dead;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new DeathMonitor.Instance(smi.master, new DeathMonitor.Def()))).Enter((StateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if (smi.HasTag(GameTags.Dead))
        smi.GoTo((StateMachine.BaseState) this.dead);
      else
        smi.GoTo((StateMachine.BaseState) this.alive);
    }));
    this.alive.TagTransition(GameTags.Dead, this.dead).Exit(new StateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State.Callback(RationalAi.IncreaseDeathCounterIfDying)).ToggleStateMachineList(new Func<RationalAi.Instance, Func<RationalAi.Instance, StateMachine.Instance>[]>(RationalAi.GetStateMachinesToRunWhenAlive));
    this.dead.ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new FallWhenDeadMonitor.Instance(smi.master))).ToggleBrain("dead").Enter("RefreshUserMenu", (StateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.RefreshUserMenu())).Enter("DropStorage", (StateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.GetComponent<Storage>().DropAll()));
  }

  public static Func<RationalAi.Instance, StateMachine.Instance>[] GetStateMachinesToRunWhenAlive(
    RationalAi.Instance smi)
  {
    return smi.stateMachinesToRunWhenAlive;
  }

  private static void IncreaseDeathCounterIfDying(RationalAi.Instance smi)
  {
    if (!smi.HasTag(GameTags.Dead))
      return;
    ++SaveGame.Instance.ColonyAchievementTracker.deadDupeCounter;
  }

  public new class Instance : 
    GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Tag MinionModel;
    public Func<RationalAi.Instance, StateMachine.Instance>[] stateMachinesToRunWhenAlive;

    public Instance(IStateMachineTarget master, Tag minionModel)
      : base(master)
    {
      this.MinionModel = minionModel;
      ChoreConsumer component = this.GetComponent<ChoreConsumer>();
      component.AddUrge(Db.Get().Urges.EmoteHighPriority);
      component.AddUrge(Db.Get().Urges.EmoteIdle);
      component.prioritizeBrainIfNoChore = true;
    }

    public void RefreshUserMenu() => Game.Instance.userMenu.Refresh(this.master.gameObject);
  }
}
