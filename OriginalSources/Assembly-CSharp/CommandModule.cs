// Decompiled with JetBrains decompiler
// Type: CommandModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class CommandModule : StateMachineComponent<CommandModule.StatesInstance>
{
  public Storage storage;
  public RocketStats rocketStats;
  public CommandConditions conditions;
  private bool releasingAstronaut;
  private const Sim.Cell.Properties floorCellProperties = Sim.Cell.Properties.GasImpermeable | Sim.Cell.Properties.LiquidImpermeable | Sim.Cell.Properties.SolidImpermeable | Sim.Cell.Properties.Opaque;
  public Assignable assignable;
  public bool robotPilotControlled;
  private HandleVector<int>.Handle partitionerEntry;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.rocketStats = new RocketStats(this);
    this.conditions = this.GetComponent<CommandConditions>();
  }

  public void ReleaseAstronaut(bool fill_bladder)
  {
    if (this.releasingAstronaut || this.robotPilotControlled)
      return;
    this.releasingAstronaut = true;
    MinionStorage component = this.GetComponent<MinionStorage>();
    List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
    for (int index = storedMinionInfo.Count - 1; index >= 0; --index)
    {
      MinionStorage.Info info = storedMinionInfo[index];
      GameObject go = component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(this.smi.master.transform.GetPosition())));
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null))
      {
        if (Grid.FakeFloor[Grid.OffsetCell(Grid.PosToCell(this.smi.master.gameObject), 0, -1)])
          go.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
        if (fill_bladder)
        {
          AmountInstance amountInstance = Db.Get().Amounts.Bladder.Lookup(go);
          if (amountInstance != null)
            amountInstance.value = amountInstance.GetMax();
        }
      }
    }
    this.releasingAstronaut = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.storage = this.GetComponent<Storage>();
    if (!this.robotPilotControlled)
    {
      this.assignable = this.GetComponent<Assignable>();
      this.assignable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanAssignTo));
      this.partitionerEntry = GameScenePartitioner.Instance.Add("CommandModule.gantryChanged", (object) this.gameObject, Grid.PosToCell(this.gameObject), GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnGantryChanged));
      this.OnGantryChanged((object) null);
    }
    this.smi.StartSM();
  }

  private bool CanAssignTo(MinionAssignablesProxy worker)
  {
    if (worker.target is MinionIdentity target1)
      return target1.GetComponent<MinionResume>().HasPerk(Db.Get().SkillPerks.CanUseRockets);
    if (!(worker.target is StoredMinionIdentity target2))
      return false;
    if (target2.model == BionicMinionConfig.MODEL)
    {
      MinionStorageDataHolder component = target2.GetComponent<MinionStorageDataHolder>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        MinionStorageDataHolder.DataPack dataPack = component.GetDataPack<BionicUpgradesMonitor.Instance>();
        if (dataPack != null)
        {
          MinionStorageDataHolder.DataPackData dataPackData = dataPack.PeekData();
          if (dataPackData != null && dataPackData.Tags != null)
          {
            foreach (Tag tag in dataPackData.Tags)
            {
              if (tag == (Tag) "Booster_PilotVanilla1")
                return true;
            }
          }
        }
      }
    }
    return target2.HasPerk(Db.Get().SkillPerks.CanUseRockets);
  }

  private static bool HasValidGantry(GameObject go)
  {
    int num = Grid.OffsetCell(Grid.PosToCell(go), 0, -1);
    return Grid.IsValidCell(num) && Grid.FakeFloor[num];
  }

  private void OnGantryChanged(object data)
  {
    if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
      return;
    KSelectable component = this.GetComponent<KSelectable>();
    component.RemoveStatusItem(Db.Get().BuildingStatusItems.HasGantry);
    component.RemoveStatusItem(Db.Get().BuildingStatusItems.MissingGantry);
    if (CommandModule.HasValidGantry(this.smi.master.gameObject))
      component.AddStatusItem(Db.Get().BuildingStatusItems.HasGantry);
    else
      component.AddStatusItem(Db.Get().BuildingStatusItems.MissingGantry);
    this.smi.sm.gantryChanged.Trigger(this.smi);
  }

  private Chore CreateWorkChore()
  {
    WorkChore<CommandModuleWorkable> workChore = new WorkChore<CommandModuleWorkable>(Db.Get().ChoreTypes.Astronaut, (IStateMachineTarget) this, allow_in_red_alert: false, override_anims: Assets.GetAnim((HashedString) "anim_hat_kanim"), allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.personalNeeds);
    workChore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanUseRockets);
    workChore.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, (object) this.assignable);
    return (Chore) workChore;
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    this.partitionerEntry.Clear();
    this.ReleaseAstronaut(false);
    this.smi.StopSM("cleanup");
  }

  public class StatesInstance(CommandModule master) : 
    GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.GameInstance(master)
  {
    public void SetSuspended(bool suspended)
    {
      Storage component1 = this.GetComponent<Storage>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.allowItemRemoval = !suspended;
      ManualDeliveryKG component2 = this.GetComponent<ManualDeliveryKG>();
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
        return;
      component2.Pause(suspended, "Rocket is suspended");
    }

    public bool CheckStoredMinionIsAssignee()
    {
      if (this.smi.master.robotPilotControlled)
        return true;
      foreach (MinionStorage.Info info in this.GetComponent<MinionStorage>().GetStoredMinionInfo())
      {
        if (info.serializedMinion != null)
        {
          KPrefabID kprefabId = info.serializedMinion.Get();
          if (!((UnityEngine.Object) kprefabId == (UnityEngine.Object) null))
          {
            StoredMinionIdentity component = kprefabId.GetComponent<StoredMinionIdentity>();
            if (this.GetComponent<Assignable>().assignee == component.assignableProxy.Get())
              return true;
          }
        }
      }
      return false;
    }
  }

  public class States : 
    GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule>
  {
    public StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.Signal gantryChanged;
    public StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.BoolParameter accumulatedPee;
    public CommandModule.States.GroundedStates grounded;
    public CommandModule.States.SpaceborneStates spaceborne;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.grounded;
      this.grounded.PlayAnim("grounded", KAnim.PlayMode.Loop).DefaultState(this.grounded.awaitingAstronaut).TagTransition(GameTags.RocketNotOnGround, (GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State) this.spaceborne);
      this.grounded.refreshChore.GoTo(this.grounded.awaitingAstronaut);
      this.grounded.awaitingAstronaut.Enter((StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State.Callback) (smi =>
      {
        if (smi.CheckStoredMinionIsAssignee())
          smi.GoTo((StateMachine.BaseState) this.grounded.hasAstronaut);
        Game.Instance.userMenu.Refresh(smi.gameObject);
      })).EventHandler(GameHashes.AssigneeChanged, (StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State.Callback) (smi =>
      {
        if (smi.CheckStoredMinionIsAssignee())
          smi.GoTo((StateMachine.BaseState) this.grounded.hasAstronaut);
        else
          smi.GoTo((StateMachine.BaseState) this.grounded.refreshChore);
        Game.Instance.userMenu.Refresh(smi.gameObject);
      })).ToggleChore((Func<CommandModule.StatesInstance, Chore>) (smi => smi.master.CreateWorkChore()), this.grounded.hasAstronaut);
      this.grounded.hasAstronaut.EventHandler(GameHashes.AssigneeChanged, (StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State.Callback) (smi =>
      {
        if (smi.CheckStoredMinionIsAssignee())
          return;
        smi.GoTo((StateMachine.BaseState) this.grounded.waitingToRelease);
      }));
      this.grounded.waitingToRelease.ToggleStatusItem(Db.Get().BuildingStatusItems.DisembarkingDuplicant).OnSignal(this.gantryChanged, this.grounded.awaitingAstronaut, (StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.Parameter<StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.SignalParameter>.Callback) ((smi, param) =>
      {
        if (!CommandModule.HasValidGantry(smi.gameObject))
          return false;
        smi.master.ReleaseAstronaut(this.accumulatedPee.Get(smi));
        this.accumulatedPee.Set(false, smi);
        Game.Instance.userMenu.Refresh(smi.gameObject);
        return true;
      }));
      this.spaceborne.DefaultState(this.spaceborne.launch);
      this.spaceborne.launch.Enter((StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State.Callback) (smi => smi.SetSuspended(true))).GoTo(this.spaceborne.idle);
      this.spaceborne.idle.TagTransition(GameTags.RocketNotOnGround, this.spaceborne.land, true);
      this.spaceborne.land.Enter((StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State.Callback) (smi =>
      {
        smi.SetSuspended(false);
        Game.Instance.userMenu.Refresh(smi.gameObject);
        this.accumulatedPee.Set(true, smi);
      })).GoTo(this.grounded.waitingToRelease);
    }

    public class GroundedStates : 
      GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State
    {
      public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State refreshChore;
      public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State awaitingAstronaut;
      public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State hasAstronaut;
      public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State waitingToRelease;
    }

    public class SpaceborneStates : 
      GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State
    {
      public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State launch;
      public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State idle;
      public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State land;
    }
  }
}
