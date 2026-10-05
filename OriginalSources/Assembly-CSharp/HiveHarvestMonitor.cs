// Decompiled with JetBrains decompiler
// Type: HiveHarvestMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class HiveHarvestMonitor : 
  GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>
{
  public StateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.BoolParameter shouldHarvest;
  public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State do_not_harvest;
  public HiveHarvestMonitor.HarvestStates harvest;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.do_not_harvest;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.root.EventHandler(GameHashes.RefreshUserMenu, (StateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State.Callback) (smi => smi.OnRefreshUserMenu()));
    this.do_not_harvest.ParamTransition<bool>((StateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.Parameter<bool>) this.shouldHarvest, (GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State) this.harvest, (StateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.Parameter<bool>.Callback) ((smi, bShouldHarvest) => bShouldHarvest));
    this.harvest.ParamTransition<bool>((StateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.Parameter<bool>) this.shouldHarvest, this.do_not_harvest, (StateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.Parameter<bool>.Callback) ((smi, bShouldHarvest) => !bShouldHarvest)).DefaultState(this.harvest.not_ready);
    this.harvest.not_ready.EventTransition(GameHashes.OnStorageChange, this.harvest.ready, (StateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.Transition.ConditionCallback) (smi => (double) smi.storage.GetMassAvailable(smi.def.producedOre) >= (double) smi.def.harvestThreshold));
    this.harvest.ready.ToggleChore((Func<HiveHarvestMonitor.Instance, Chore>) (smi => smi.CreateHarvestChore()), new System.Action<HiveHarvestMonitor.Instance, Chore>(HiveHarvestMonitor.SetRemoteChore), this.harvest.not_ready).EventTransition(GameHashes.OnStorageChange, this.harvest.not_ready, (StateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.Transition.ConditionCallback) (smi => (double) smi.storage.GetMassAvailable(smi.def.producedOre) < (double) smi.def.harvestThreshold));
  }

  private static void SetRemoteChore(HiveHarvestMonitor.Instance smi, Chore chore)
  {
    smi.remoteChore.SetChore(chore);
  }

  public class Def : StateMachine.BaseDef
  {
    public Tag producedOre;
    public float harvestThreshold;
  }

  public class HarvestStates : 
    GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State
  {
    public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State not_ready;
    public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State ready;
  }

  public new class Instance(IStateMachineTarget master, HiveHarvestMonitor.Def def) : 
    GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.GameInstance(master, def)
  {
    [MyCmpReq]
    public Storage storage;
    [MyCmpAdd]
    public ManuallySetRemoteWorkTargetComponent remoteChore;

    public void OnRefreshUserMenu()
    {
      if (this.sm.shouldHarvest.Get(this))
        Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_building_disabled", (string) UI.USERMENUACTIONS.CANCELEMPTYBEEHIVE.NAME, (System.Action) (() => this.sm.shouldHarvest.Set(false, this)), tooltipText: (string) UI.USERMENUACTIONS.CANCELEMPTYBEEHIVE.TOOLTIP));
      else
        Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_empty_contents", (string) UI.USERMENUACTIONS.EMPTYBEEHIVE.NAME, (System.Action) (() => this.sm.shouldHarvest.Set(true, this)), tooltipText: (string) UI.USERMENUACTIONS.EMPTYBEEHIVE.TOOLTIP));
    }

    public Chore CreateHarvestChore()
    {
      return (Chore) new WorkChore<HiveWorkableEmpty>(Db.Get().ChoreTypes.Ranch, (IStateMachineTarget) this.master.GetComponent<HiveWorkableEmpty>(), on_complete: new System.Action<Chore>(this.smi.OnEmptyComplete));
    }

    public void OnEmptyComplete(Chore chore) => this.smi.storage.Drop(this.smi.def.producedOre);
  }
}
