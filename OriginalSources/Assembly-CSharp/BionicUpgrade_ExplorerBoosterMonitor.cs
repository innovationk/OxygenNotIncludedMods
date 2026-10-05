// Decompiled with JetBrains decompiler
// Type: BionicUpgrade_ExplorerBoosterMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;

#nullable disable
public class BionicUpgrade_ExplorerBoosterMonitor : 
  BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>
{
  public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State attachToBooster;
  public BionicUpgrade_ExplorerBoosterMonitor.ActiveStates Active;
  public StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Signal ReadyToDiscoverSignal;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.attachToBooster;
    this.attachToBooster.Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.FindAndAttachToInstalledBooster)).GoTo(this.Inactive);
    this.Inactive.EventTransition(GameHashes.ScheduleBlocksChanged, (GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State) this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.ScheduleChanged, (GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State) this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.BionicOnline, (GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State) this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.MinionMigration, (Func<BionicUpgrade_ExplorerBoosterMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), (GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State) this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged);
    this.Active.EventTransition(GameHashes.ScheduleBlocksChanged, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBedTimeChore))).EventTransition(GameHashes.ScheduleChanged, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBedTimeChore))).EventTransition(GameHashes.BionicOffline, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsOnline))).EventTransition(GameHashes.MinionMigration, (Func<BionicUpgrade_ExplorerBoosterMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive))).DefaultState(this.Active.gatheringData);
    this.Active.gatheringData.OnSignal(this.ReadyToDiscoverSignal, this.Active.discover, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Parameter<StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.SignalParameter>.Callback(BionicUpgrade_ExplorerBoosterMonitor.IsReadyToDiscoverAndThereIsSomethingToDiscover)).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicExplorerBooster).Update(new System.Action<BionicUpgrade_ExplorerBoosterMonitor.Instance, float>(BionicUpgrade_ExplorerBoosterMonitor.DataGatheringUpdate));
    this.Active.discover.Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.ConsumeAllData)).Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.RevealUndiscoveredGeyser)).EnterTransition(this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover))).GoTo(this.Active.gatheringData);
  }

  public static bool ShouldBeActive(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
  {
    return BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsOnline((BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.BaseInstance) smi) && BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBedTimeChore((BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.BaseInstance) smi) && BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover(smi);
  }

  public static bool IsReadyToDiscoverAndThereIsSomethingToDiscover(
    BionicUpgrade_ExplorerBoosterMonitor.Instance smi,
    StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.SignalParameter param)
  {
    return smi.IsReadyToDiscover && BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover(smi);
  }

  public static void ConsumeAllData(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
  {
    smi.ConsumeAllData();
  }

  public static void FindAndAttachToInstalledBooster(
    BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
  {
    smi.Initialize();
  }

  public static void DataGatheringUpdate(
    BionicUpgrade_ExplorerBoosterMonitor.Instance smi,
    float dt)
  {
    smi.GatheringDataUpdate(dt);
  }

  public static bool IsThereGeysersToDiscover(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
  {
    WorldContainer myWorld = smi.GetMyWorld();
    if (myWorld.id == (int) byte.MaxValue)
      return false;
    List<WorldGenSpawner.Spawnable> spawnableList = new List<WorldGenSpawner.Spawnable>();
    spawnableList.AddRange((IEnumerable<WorldGenSpawner.Spawnable>) SaveGame.Instance.worldGenSpawner.GeInfoOfUnspawnedWithType<Geyser>(myWorld.id));
    spawnableList.AddRange((IEnumerable<WorldGenSpawner.Spawnable>) SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag((Tag) "GeyserGeneric", myWorld.id));
    spawnableList.AddRange((IEnumerable<WorldGenSpawner.Spawnable>) SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag((Tag) "OilWell", myWorld.id));
    return spawnableList.Count > 0;
  }

  public static void RevealUndiscoveredGeyser(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
  {
    WorldContainer myWorld = smi.GetMyWorld();
    if (myWorld.id == (int) byte.MaxValue)
      return;
    List<WorldGenSpawner.Spawnable> tList = new List<WorldGenSpawner.Spawnable>();
    tList.AddRange((IEnumerable<WorldGenSpawner.Spawnable>) SaveGame.Instance.worldGenSpawner.GeInfoOfUnspawnedWithType<Geyser>(myWorld.id));
    tList.AddRange((IEnumerable<WorldGenSpawner.Spawnable>) SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag((Tag) "GeyserGeneric", myWorld.id));
    tList.AddRange((IEnumerable<WorldGenSpawner.Spawnable>) SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag((Tag) "OilWell", myWorld.id));
    if (tList.Count <= 0)
      return;
    WorldGenSpawner.Spawnable random = tList.GetRandom<WorldGenSpawner.Spawnable>();
    int x;
    int y;
    Grid.CellToXY(random.cell, out x, out y);
    GridVisibility.Reveal(x, y, 4, 4f);
    Notifier notifier = smi.gameObject.AddOrGet<Notifier>();
    Notification discoveredNotification = smi.GetGeyserDiscoveredNotification();
    int cell = random.cell;
    discoveredNotification.customClickCallback = (Notification.ClickCallback) (obj => GameUtil.FocusCamera(cell));
    Notification notification = discoveredNotification;
    notifier.Add(notification);
  }

  public new class Def(string upgradeID) : 
    BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def(upgradeID)
  {
    public override string GetDescription()
    {
      return "BionicUpgrade_ExplorerBoosterMonitor.Def description not implemented";
    }
  }

  public class ActiveStates : 
    GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State
  {
    public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State gatheringData;
    public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State discover;
  }

  public new class Instance(
    IStateMachineTarget master,
    BionicUpgrade_ExplorerBoosterMonitor.Def def) : 
    BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.BaseInstance(master, (BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def) def)
  {
    private BionicUpgrade_ExplorerBooster.Instance explorerBooster;

    public bool IsReadyToDiscover => this.explorerBooster != null && this.explorerBooster.IsReady;

    public float CurrentProgress
    {
      get => this.explorerBooster != null ? this.explorerBooster.Progress : 0.0f;
    }

    public void Initialize()
    {
      foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in this.gameObject.GetSMI<BionicUpgradesMonitor.Instance>().upgradeComponentSlots)
      {
        if (upgradeComponentSlot.HasUpgradeInstalled)
        {
          BionicUpgrade_ExplorerBooster.Instance smi = upgradeComponentSlot.installedUpgradeComponent.GetSMI<BionicUpgrade_ExplorerBooster.Instance>();
          if (smi != null && !smi.IsBeingMonitored)
          {
            this.explorerBooster = smi;
            smi.SetMonitor(this);
            break;
          }
        }
      }
    }

    protected override void OnCleanUp()
    {
      if (this.explorerBooster != null)
        this.explorerBooster.SetMonitor((BionicUpgrade_ExplorerBoosterMonitor.Instance) null);
      base.OnCleanUp();
    }

    public void GatheringDataUpdate(float dt)
    {
      bool isReadyToDiscover = this.IsReadyToDiscover;
      this.explorerBooster.AddData((double) dt == 0.0 ? 0.0f : dt / 600f);
      if (!this.IsReadyToDiscover || isReadyToDiscover)
        return;
      this.sm.ReadyToDiscoverSignal.Trigger(this);
    }

    public void ConsumeAllData() => this.explorerBooster.SetDataProgress(0.0f);

    public Notification GetGeyserDiscoveredNotification()
    {
      return new Notification((string) DUPLICANTS.STATUSITEMS.BIONICEXPLORERBOOSTER.NOTIFICATION_NAME, NotificationType.MessageImportant, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) DUPLICANTS.STATUSITEMS.BIONICEXPLORERBOOSTER.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)));
    }

    public override float GetCurrentWattageCost()
    {
      return this.IsInsideState((StateMachine.BaseState) this.sm.Active) ? this.Data.WattageCost : 0.0f;
    }

    public override string GetCurrentWattageCostName()
    {
      float currentWattageCost = this.GetCurrentWattageCost();
      return this.IsInsideState((StateMachine.BaseState) this.sm.Active) ? string.Format((string) DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_ACTIVE_TEMPLATE, (object) this.upgradeComponent.GetProperName(), (object) GameUtil.GetFormattedWattage(currentWattageCost)) : string.Format((string) DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_INACTIVE_TEMPLATE, (object) this.upgradeComponent.GetProperName(), (object) GameUtil.GetFormattedWattage(this.upgradeComponent.PotentialWattage));
    }
  }
}
