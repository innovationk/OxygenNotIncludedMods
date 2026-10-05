// Decompiled with JetBrains decompiler
// Type: BionicBatteryMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BionicBatteryMonitor : 
  GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>
{
  public const int DEFAULT_ELECTROBANK_COUNT = 4;
  public const int BIONIC_SKILL_EXTRA_BATTERY_COUNT = 2;
  public const int MAX_ELECTROBANK_COUNT = 6;
  public const float DEFAULT_WATTS = 200f;
  public const string INITIAL_ELECTROBANK_TYPE_ID = "DisposableElectrobank_RawMetal";
  public static readonly string ChargedBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_charged_electrobank\">";
  public static readonly string DischargedBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_discharged_electrobank\">";
  public static readonly string CriticalBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_critical_electrobank\">";
  public static readonly string SavingBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_saving_electrobank\">";
  public static readonly string EmptySlotBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_empty_slot_electrobank\">";
  private const string ANIM_NAME_REBOOT = "power_up";
  public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State firstSpawn;
  public BionicBatteryMonitor.OnlineStates online;
  public BionicBatteryMonitor.OfflineStates offline;
  public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Signal OnClosestAvailableElectrobankChangedSignal;
  public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IntParameter ChargedElectrobankCount;
  public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IntParameter DepletedElectrobankCount;
  private StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.BoolParameter InitialElectrobanksSpawned;
  private StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.BoolParameter IsOnline;
  private StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Signal OnElectrobankStorageChanged;
  private static readonly Dictionary<string, BionicBatteryMonitor.WattageModifier> difficultyWattages = new Dictionary<string, BionicBatteryMonitor.WattageModifier>()
  {
    {
      "VeryHard",
      BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.VERYHARD.NAME, 200f)
    },
    {
      "Hard",
      BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.HARD.NAME, 100f)
    },
    {
      "Default",
      BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.DEFAULT.NAME, 0.0f)
    },
    {
      "Easy",
      BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.EASY.NAME, -100f)
    },
    {
      "VeryEasy",
      BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.VERYEASY.NAME, -150f)
    }
  };

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.firstSpawn;
    this.firstSpawn.ParamTransition<bool>((StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Parameter<bool>) this.InitialElectrobanksSpawned, (GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State) this.online, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsTrue).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SpawnAndInstallInitialElectrobanks));
    this.online.TriggerOnEnter(GameHashes.BionicOnline).Transition((GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State) this.offline, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.DoesNotHaveCharge)).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.ReorganizeElectrobankStorage)).Update(new System.Action<BionicBatteryMonitor.Instance, float>(BionicBatteryMonitor.DischargeUpdate)).DefaultState(this.online.idle);
    this.online.idle.ParamTransition<int>((StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Parameter<int>) this.ChargedElectrobankCount, (GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State) this.online.critical, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsLTEOne_Int).OnSignal(this.OnElectrobankStorageChanged, (GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State) this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Parameter<StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.SignalParameter>.Callback(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleBlocksChanged, (GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State) this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleChanged, (GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State) this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleBlocksTick, (GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State) this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep));
    this.online.upkeep.ParamTransition<int>((StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Parameter<int>) this.ChargedElectrobankCount, (GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State) this.online.critical, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsLTEOne_Int).EventTransition(GameHashes.ScheduleBlocksChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).EventTransition(GameHashes.ScheduleChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).EventTransition(GameHashes.ScheduleBlocksTick, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).OnSignal(this.OnElectrobankStorageChanged, this.online.idle, (StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Parameter<StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.SignalParameter>.Callback) ((smi, param) => !BionicBatteryMonitor.WantsToUpkeep(smi))).DefaultState(this.online.upkeep.seekElectrobank);
    this.online.upkeep.seekElectrobank.ToggleUrge(Db.Get().Urges.ReloadElectrobank).ToggleChore((Func<BionicBatteryMonitor.Instance, Chore>) (smi => (Chore) new ReloadElectrobankChore(smi.master)), this.online.idle);
    this.online.critical.DefaultState(this.online.critical.seekElectrobank).ParamTransition<int>((StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Parameter<int>) this.ChargedElectrobankCount, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsGTOne_Int).DoTutorial(Tutorial.TutorialMessages.TM_BionicBattery);
    this.online.critical.seekElectrobank.ToggleUrge(Db.Get().Urges.ReloadElectrobank).ToggleRecurringChore((Func<BionicBatteryMonitor.Instance, Chore>) (smi => (Chore) new ReloadElectrobankChore(smi.master)));
    this.offline.DefaultState(this.offline.waitingForBatteryDelivery).ToggleTag(GameTags.Incapacitated).ToggleRecurringChore((Func<BionicBatteryMonitor.Instance, Chore>) (smi => (Chore) new BeOfflineChore(smi.master))).ToggleUrge(Db.Get().Urges.BeOffline).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SetOffline)).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.DropAllDischargedElectrobanks)).TriggerOnEnter(GameHashes.BionicOffline);
    this.offline.waitingForBatteryDelivery.ParamTransition<int>((StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Parameter<int>) this.ChargedElectrobankCount, (GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State) this.offline.waitingForBatteryInstallation, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsGTZero_Int).Toggle("Enable Delivery of new Electrobanks", new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.EnableManualDelivery), new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.DisableManualDelivery)).Toggle("Enable User Prioritization", new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.EnablePrioritizationComponent), new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.DisablePrioritizationComponent));
    this.offline.waitingForBatteryInstallation.Toggle("Enable User Prioritization", new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.EnablePrioritizationComponent), new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.DisablePrioritizationComponent)).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.StartReanimateWorkChore)).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.CancelReanimateWorkChore)).WorkableCompleteTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.reboot).DefaultState(this.offline.waitingForBatteryInstallation.waiting);
    this.offline.waitingForBatteryInstallation.waiting.ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicWaitingForReboot).WorkableStartTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.waitingForBatteryInstallation.working);
    this.offline.waitingForBatteryInstallation.working.WorkableStopTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.waitingForBatteryInstallation.waiting);
    this.offline.reboot.PlayAnim("power_up").OnAnimQueueComplete((GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State) this.online).ScheduleGoTo(10f, (StateMachine.BaseState) this.online).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.AutomaticallyDropAllDepletedElectrobanks)).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SetOnline));
  }

  public static ReanimateBionicWorkable GetReanimateWorkable(BionicBatteryMonitor.Instance smi)
  {
    return smi.reanimateWorkable;
  }

  public static bool DoesNotHaveCharge(BionicBatteryMonitor.Instance smi)
  {
    return (double) smi.CurrentCharge <= 0.0;
  }

  public static bool IsCriticallyLow(BionicBatteryMonitor.Instance smi)
  {
    return smi.ChargedElectrobankCount <= 1;
  }

  public static bool ChargeIsBelowNotificationThreshold(BionicBatteryMonitor.Instance smi)
  {
    return (double) smi.CurrentCharge <= 30000.0;
  }

  public static bool IsAnyElectrobankAvailableToBeFetched(BionicBatteryMonitor.Instance smi)
  {
    return (UnityEngine.Object) smi.GetClosestElectrobank() != (UnityEngine.Object) null;
  }

  public static bool WantsToInstallNewBattery(BionicBatteryMonitor.Instance smi)
  {
    if (BionicBatteryMonitor.IsCriticallyLow(smi))
      return true;
    return smi.InUpkeepTime && smi.ChargedElectrobankCount < smi.ElectrobankCountCapacity;
  }

  public static bool WantsToUpkeep(BionicBatteryMonitor.Instance smi)
  {
    return BionicBatteryMonitor.WantsToInstallNewBattery(smi);
  }

  public static bool WantsToUpkeep(
    BionicBatteryMonitor.Instance smi,
    StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.SignalParameter param)
  {
    return BionicBatteryMonitor.WantsToInstallNewBattery(smi);
  }

  public static void SpawnAndInstallInitialElectrobanks(BionicBatteryMonitor.Instance smi)
  {
    smi.SpawnAndInstallInitialElectrobanks();
  }

  public static void RefreshCharge(BionicBatteryMonitor.Instance smi) => smi.RefreshCharge();

  public static void EnableManualDelivery(BionicBatteryMonitor.Instance smi)
  {
    smi.SetManualDeliveryEnableState(true);
  }

  public static void DisableManualDelivery(BionicBatteryMonitor.Instance smi)
  {
    smi.SetManualDeliveryEnableState(false);
  }

  public static void StartReanimateWorkChore(BionicBatteryMonitor.Instance smi)
  {
    smi.CreateWorkableChore();
  }

  public static void CancelReanimateWorkChore(BionicBatteryMonitor.Instance smi)
  {
    smi.CancelWorkChore();
  }

  public static void SetOffline(BionicBatteryMonitor.Instance smi) => smi.SetOnlineState(false);

  public static void SetOnline(BionicBatteryMonitor.Instance smi) => smi.SetOnlineState(true);

  public static void AutomaticallyDropAllDepletedElectrobanks(BionicBatteryMonitor.Instance smi)
  {
    smi.AutomaticallyDropAllDepletedElectrobanks();
  }

  public static void ReorganizeElectrobankStorage(BionicBatteryMonitor.Instance smi)
  {
    smi.ReorganizeElectrobanks();
  }

  public static void DropAllDischargedElectrobanks(BionicBatteryMonitor.Instance smi)
  {
    smi.DropAllDischargedElectrobanks();
  }

  public static void EnablePrioritizationComponent(BionicBatteryMonitor.Instance smi)
  {
    Prioritizable.AddRef(smi.gameObject);
    smi.gameObject.Trigger(1980521255);
  }

  public static void DisablePrioritizationComponent(BionicBatteryMonitor.Instance smi)
  {
    Prioritizable.RemoveRef(smi.gameObject);
    smi.gameObject.Trigger(1980521255);
  }

  public static void DischargeUpdate(BionicBatteryMonitor.Instance smi, float dt)
  {
    float joules = Mathf.Min(dt * smi.Wattage, smi.CurrentCharge);
    smi.ConsumePower(joules);
  }

  private static BionicBatteryMonitor.WattageModifier MakeDifficultyModifier(
    string id,
    string desc,
    float watts)
  {
    return new BionicBatteryMonitor.WattageModifier(id, $"{desc}: <b>{((double) watts >= 0.0 ? "+</b>" : "-</b>")}{GameUtil.GetFormattedWattage(Mathf.Abs(watts))}", watts, watts);
  }

  public static BionicBatteryMonitor.WattageModifier GetDifficultyModifier()
  {
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.BionicWattage);
    BionicBatteryMonitor.WattageModifier wattageModifier;
    return BionicBatteryMonitor.difficultyWattages.TryGetValue(currentQualitySetting.id, out wattageModifier) ? wattageModifier : BionicBatteryMonitor.difficultyWattages["Default"];
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public struct WattageModifier(string id, string name, float value, float potentialValue)
  {
    public float potentialValue = potentialValue;
    public float value = value;
    public string name = name;
    public string id = id;
  }

  public class OnlineStates : 
    GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
  {
    public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State idle;
    public BionicBatteryMonitor.UpkeepStates upkeep;
    public BionicBatteryMonitor.UpkeepStates critical;
  }

  public class UpkeepStates : 
    GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
  {
    public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State seekElectrobank;
  }

  public class OfflineStates : 
    GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
  {
    public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State waitingForBatteryDelivery;
    public BionicBatteryMonitor.RebootWorkableState waitingForBatteryInstallation;
    public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State reboot;
  }

  public class RebootWorkableState : 
    GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
  {
    public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State waiting;
    public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State working;
  }

  public new class Instance : 
    GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.GameInstance
  {
    public Storage storage;
    public KPrefabID prefabID;
    private Schedulable schedulable;
    private AmountInstance BionicBattery;
    private ManualDeliveryKG manualDelivery;
    private ClosestElectrobankSensor closestElectrobankSensor;
    private KSelectable selectable;
    private MinionStorageDataHolder dataHolder;
    private Guid criticalBatteryStatusItemGuid;
    private Chore reanimateChore;

    public float Wattage => this.GetBaseWattage() + this.GetModifiersWattage();

    public bool IsOnline => this.sm.IsOnline.Get(this);

    public bool InUpkeepTime => this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Eat);

    public bool HaveInitialElectrobanksBeenSpawned => this.sm.InitialElectrobanksSpawned.Get(this);

    public bool HasSpaceForNewElectrobank => this.ElectrobankCount < this.ElectrobankCountCapacity;

    public int ElectrobankCount => this.ChargedElectrobankCount + this.DepletedElectrobankCount;

    public int ChargedElectrobankCount => this.sm.ChargedElectrobankCount.Get(this);

    public int DepletedElectrobankCount => this.sm.DepletedElectrobankCount.Get(this);

    public float CurrentCharge => this.BionicBattery.value;

    public int ElectrobankCountCapacity
    {
      get
      {
        return (int) this.gameObject.GetAttributes().Get(Db.Get().Attributes.BionicBatteryCountCapacity.Id).GetTotalValue();
      }
    }

    public ReanimateBionicWorkable reanimateWorkable { private set; get; }

    public List<BionicBatteryMonitor.WattageModifier> Modifiers { set; get; } = new List<BionicBatteryMonitor.WattageModifier>();

    public Instance(IStateMachineTarget master, BionicBatteryMonitor.Def def)
      : base(master, def)
    {
      this.storage = this.gameObject.GetComponents<Storage>().FindFirst<Storage>((Func<Storage, bool>) (s => s.storageID == GameTags.StoragesIds.BionicBatteryStorage));
      this.reanimateWorkable = this.GetComponent<ReanimateBionicWorkable>();
      this.schedulable = this.GetComponent<Schedulable>();
      this.manualDelivery = this.GetComponent<ManualDeliveryKG>();
      this.selectable = this.GetComponent<KSelectable>();
      this.prefabID = this.GetComponent<KPrefabID>();
      this.dataHolder = this.GetComponent<MinionStorageDataHolder>();
      this.dataHolder.OnCopyBegins += new System.Action<StoredMinionIdentity>(this.OnCopyMinionBegins);
      this.BionicBattery = Db.Get().Amounts.BionicInternalBattery.Lookup(this.gameObject);
      this.storage.onDestroyItemsDropped += new System.Action<List<GameObject>>(this.OnBatteriesDroppedFromDeath);
      this.storage.OnStorageChange += new System.Action<GameObject>(this.OnElectrobankStorageChanged);
      this.Subscribe(540773776, new System.Action<object>(this.OnSkillsChanged));
      this.UpdateCapacityAmount();
      this.ApplyDifficultyModifiers();
    }

    public override void StartSM()
    {
      this.closestElectrobankSensor = this.GetComponent<Sensors>().GetSensor<ClosestElectrobankSensor>();
      ClosestElectrobankSensor electrobankSensor = this.closestElectrobankSensor;
      electrobankSensor.OnItemChanged = electrobankSensor.OnItemChanged + new System.Action<Electrobank>(this.OnClosestElectrobankChanged);
      base.StartSM();
    }

    private void OnCopyMinionBegins(StoredMinionIdentity destination)
    {
      this.dataHolder.UpdateData<BionicBatteryMonitor.Instance>(new MinionStorageDataHolder.DataPackData()
      {
        Bools = new bool[2]
        {
          this.HaveInitialElectrobanksBeenSpawned,
          this.IsOnline
        }
      });
    }

    public override void PostParamsInitialized()
    {
      MinionStorageDataHolder.DataPack dataPack = this.dataHolder.GetDataPack<BionicBatteryMonitor.Instance>();
      if (dataPack != null && dataPack.IsStoringNewData)
      {
        MinionStorageDataHolder.DataPackData dataPackData = dataPack.ReadData();
        if (dataPackData != null)
        {
          bool flag1 = dataPackData.Bools == null || dataPackData.Bools.Length == 0 ? this.HasSpaceForNewElectrobank : dataPackData.Bools[0];
          bool flag2 = dataPackData.Bools == null || dataPackData.Bools.Length <= 1 ? this.IsOnline : dataPackData.Bools[1];
          this.sm.InitialElectrobanksSpawned.Set(flag1, this);
          this.sm.IsOnline.Set(flag2, this);
        }
      }
      this.RefreshCharge();
      base.PostParamsInitialized();
    }

    public void DropAllDischargedElectrobanks()
    {
      List<GameObject> result = new List<GameObject>();
      this.storage.Find(GameTags.EmptyPortableBattery, result);
      foreach (GameObject go in result)
        this.storage.Drop(go, true);
    }

    protected override void OnCleanUp()
    {
      if ((UnityEngine.Object) this.dataHolder != (UnityEngine.Object) null)
        this.dataHolder.OnCopyBegins -= new System.Action<StoredMinionIdentity>(this.OnCopyMinionBegins);
      this.UpdateNotifications();
      base.OnCleanUp();
    }

    private void OnSkillsChanged(object o)
    {
      if ((double) this.storage.capacityKg == (double) this.ElectrobankCountCapacity)
        return;
      this.OnBatteryCapacityChanged();
    }

    private void ApplyDifficultyModifiers()
    {
      SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.BionicWattage);
      BionicBatteryMonitor.WattageModifier wattageModifier;
      if (!BionicBatteryMonitor.difficultyWattages.TryGetValue(currentQualitySetting.id, out wattageModifier))
        return;
      this.Modifiers.Add(wattageModifier);
    }

    private void UpdateCapacityAmount()
    {
      int num = this.ElectrobankCountCapacity - 4;
      this.BionicBattery.maxAttribute.ClearModifiers();
      this.BionicBattery.maxAttribute.Add(new AttributeModifier(Db.Get().Amounts.BionicInternalBattery.maxAttribute.Id, 120000f * (float) num));
    }

    private void OnBatteryCapacityChanged()
    {
      this.UpdateCapacityAmount();
      for (int index = this.storage.Count - 1; index >= 0; --index)
      {
        if (this.storage.Count > this.ElectrobankCountCapacity)
        {
          GameObject go = this.storage.items[index];
          Electrobank component = go.GetComponent<Electrobank>();
          this.storage.Drop(go, true);
          Vector3 position = go.transform.position with
          {
            z = Grid.GetLayerZ(Grid.SceneLayer.Ore)
          };
          go.transform.position = position;
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
          {
            double num = (double) component.RemovePower(component.Charge, true);
          }
        }
      }
      this.smi.storage.capacityKg = (float) this.ElectrobankCountCapacity;
    }

    private void OnClosestElectrobankChanged(Electrobank newItem)
    {
      this.sm.OnClosestAvailableElectrobankChangedSignal.Trigger(this);
    }

    public float GetBaseWattage() => 200f;

    public float GetModifiersWattage()
    {
      float modifiersWattage = 0.0f;
      foreach (BionicBatteryMonitor.WattageModifier modifier in this.Modifiers)
        modifiersWattage += modifier.value;
      return modifiersWattage;
    }

    private void OnElectrobankStorageChanged(object o)
    {
      this.ReorganizeElectrobanks();
      this.RefreshCharge();
      this.smi.sm.OnElectrobankStorageChanged.Trigger(this);
    }

    public void ReorganizeElectrobanks()
    {
      this.storage.items.Sort((Comparison<GameObject>) ((b1, b2) =>
      {
        Electrobank component1 = b1.GetComponent<Electrobank>();
        Electrobank component2 = b2.GetComponent<Electrobank>();
        if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
          return -1;
        return (UnityEngine.Object) component2 == (UnityEngine.Object) null ? 1 : component1.Charge.CompareTo(component2.Charge);
      }));
    }

    public void CreateWorkableChore()
    {
      if (this.reanimateChore != null)
        return;
      this.reanimateChore = (Chore) new WorkChore<ReanimateBionicWorkable>(Db.Get().ChoreTypes.RescueIncapacitated, (IStateMachineTarget) this.reanimateWorkable, only_when_operational: false, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.personalNeeds);
      this.reanimateChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot);
    }

    public void CancelWorkChore()
    {
      if (this.reanimateChore == null)
        return;
      this.reanimateChore.Cancel("BionicBatteryMonitor.CancelChore");
      this.reanimateChore = (Chore) null;
    }

    public void SetOnlineState(bool online)
    {
      this.sm.IsOnline.Set(online, this);
      this.RefreshCharge();
    }

    public void SetManualDeliveryEnableState(bool enable)
    {
      if (!enable)
      {
        this.manualDelivery.capacity = 0.0f;
        this.manualDelivery.refillMass = 0.0f;
        this.manualDelivery.RequestedItemTag = (Tag) (string) null;
        this.manualDelivery.AbortDelivery("Manual delivery disabled");
      }
      else
      {
        Tag[] array = new Tag[GameTags.BionicIncompatibleBatteries.Count];
        GameTags.BionicIncompatibleBatteries.CopyTo(array, 0);
        this.smi.storage.capacityKg = (float) this.ElectrobankCountCapacity;
        this.smi.manualDelivery.capacity = 1f;
        this.smi.manualDelivery.refillMass = 1f;
        this.smi.manualDelivery.MinimumMass = 1f;
        this.manualDelivery.ForbiddenTags = array;
        this.manualDelivery.RequestedItemTag = GameTags.ChargedPortableBattery;
      }
    }

    public GameObject GetFirstDischargedElectrobankInInventory()
    {
      return this.storage.FindFirst(GameTags.EmptyPortableBattery);
    }

    public Electrobank GetClosestElectrobank() => this.closestElectrobankSensor.GetItem();

    public void RefreshCharge()
    {
      ListPool<GameObject, BionicBatteryMonitor.Instance>.PooledList result1 = ListPool<GameObject, BionicBatteryMonitor.Instance>.Allocate();
      ListPool<GameObject, BionicBatteryMonitor.Instance>.PooledList result2 = ListPool<GameObject, BionicBatteryMonitor.Instance>.Allocate();
      this.storage.Find(GameTags.ChargedPortableBattery, (List<GameObject>) result1);
      this.storage.Find(GameTags.EmptyPortableBattery, (List<GameObject>) result2);
      float num1 = 0.0f;
      if (this.IsOnline)
      {
        for (int index = 0; index < result1.Count; ++index)
        {
          Electrobank component = result1[index].GetComponent<Electrobank>();
          num1 += component.Charge;
        }
      }
      double num2 = (double) this.BionicBattery.SetValue(num1);
      this.sm.ChargedElectrobankCount.Set(result1.Count, this);
      result1.Recycle();
      this.sm.DepletedElectrobankCount.Set(result2.Count, this);
      result2.Recycle();
      this.UpdateNotifications();
    }

    public void ConsumePower(float joules)
    {
      ListPool<GameObject, BionicBatteryMonitor.Instance>.PooledList result = ListPool<GameObject, BionicBatteryMonitor.Instance>.Allocate();
      this.storage.Find(GameTags.ChargedPortableBattery, (List<GameObject>) result);
      float b = joules;
      for (int index = 0; index < result.Count; ++index)
      {
        Electrobank component = result[index].GetComponent<Electrobank>();
        float joules1 = Mathf.Min(component.Charge, b);
        float valueConsumed = component.RemovePower(joules1, false);
        b -= valueConsumed;
        WorldResourceAmountTracker<ElectrobankTracker>.Get().RegisterAmountConsumed(component.ID, valueConsumed);
      }
      this.RefreshCharge();
      result.Recycle();
    }

    public void DebugAddCharge(float joules)
    {
      float b = MathF.Min(joules, (float) this.ElectrobankCountCapacity * 120000f - this.CurrentCharge);
      ListPool<GameObject, BionicBatteryMonitor.Instance>.PooledList result = ListPool<GameObject, BionicBatteryMonitor.Instance>.Allocate();
      this.storage.Find(GameTags.ChargedPortableBattery, (List<GameObject>) result);
      for (int index = 0; (double) b > 0.0 && index < result.Count; ++index)
      {
        Electrobank component = result[index].GetComponent<Electrobank>();
        float joules1 = Mathf.Min(120000f - component.Charge, b);
        double num = (double) component.AddPower(joules1);
        b -= joules1;
      }
      if ((double) b > 0.0 && result.Count < this.ElectrobankCountCapacity)
      {
        for (int index = this.storage.items.Count - 1; (double) b > 0.0 && index >= 0; --index)
        {
          GameObject go1 = this.storage.items[index];
          if (!((UnityEngine.Object) go1 == (UnityEngine.Object) null) && (UnityEngine.Object) go1.GetComponent<Electrobank>() == (UnityEngine.Object) null && go1.HasTag(GameTags.EmptyPortableBattery))
          {
            this.storage.Drop(go1, true);
            GameObject go2 = Util.KInstantiate(Assets.GetPrefab((Tag) "DisposableElectrobank_RawMetal"), this.transform.position);
            go2.SetActive(true);
            Electrobank component = go2.GetComponent<Electrobank>();
            float joules2 = Mathf.Clamp(component.Charge - b, 0.0f, float.MaxValue);
            double num = (double) component.RemovePower(joules2, true);
            b -= component.Charge;
            this.storage.Store(go2);
          }
        }
      }
      if ((double) b > 0.0 && this.storage.items.Count < this.ElectrobankCountCapacity)
      {
        do
        {
          GameObject go = Util.KInstantiate(Assets.GetPrefab((Tag) "DisposableElectrobank_RawMetal"), this.transform.position);
          go.SetActive(true);
          Electrobank component = go.GetComponent<Electrobank>();
          float joules3 = Mathf.Clamp(component.Charge - b, 0.0f, float.MaxValue);
          double num = (double) component.RemovePower(joules3, true);
          b -= component.Charge;
          this.storage.Store(go);
        }
        while ((double) b > 0.0 && this.storage.items.Count < this.ElectrobankCountCapacity && (double) b > 0.0);
      }
      this.RefreshCharge();
      result.Recycle();
    }

    private void UpdateNotifications()
    {
      this.criticalBatteryStatusItemGuid = this.selectable.ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicCriticalBattery, this.criticalBatteryStatusItemGuid, BionicBatteryMonitor.ChargeIsBelowNotificationThreshold(this.smi) && !this.prefabID.HasTag(GameTags.Incapacitated) && !this.prefabID.HasTag(GameTags.Dead), (object) this.gameObject);
    }

    public bool AddOrUpdateModifier(
      BionicBatteryMonitor.WattageModifier modifier,
      bool triggerCallbacks = true)
    {
      int index = this.Modifiers.FindIndex((Predicate<BionicBatteryMonitor.WattageModifier>) (mod => mod.id == modifier.id));
      bool flag;
      if (index >= 0)
      {
        flag = this.Modifiers[index].name != modifier.name || (double) this.Modifiers[index].value != (double) modifier.value || (double) this.Modifiers[index].potentialValue != (double) modifier.potentialValue;
        this.Modifiers[index] = modifier;
      }
      else
      {
        this.Modifiers.Add(modifier);
        flag = true;
      }
      if (flag)
        this.Modifiers.Sort((Comparison<BionicBatteryMonitor.WattageModifier>) ((a, b) => b.value.CompareTo(a.value)));
      if (triggerCallbacks)
        this.BoxingTrigger<float>(1361471071, this.Wattage);
      return flag;
    }

    public bool RemoveModifier(string modifierID, bool triggerCallbacks = true)
    {
      int index = this.Modifiers.FindIndex((Predicate<BionicBatteryMonitor.WattageModifier>) (mod => mod.id == modifierID));
      if (index < 0)
        return false;
      this.Modifiers.RemoveAt(index);
      if (triggerCallbacks)
        this.BoxingTrigger<float>(1361471071, this.Wattage);
      this.Modifiers.Sort((Comparison<BionicBatteryMonitor.WattageModifier>) ((a, b) => b.value.CompareTo(a.value)));
      return true;
    }

    private void OnBatteriesDroppedFromDeath(List<GameObject> items)
    {
      if (items == null)
        return;
      for (int index = 0; index < items.Count; ++index)
      {
        Electrobank component = items[index].GetComponent<Electrobank>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
        {
          double num = (double) component.RemovePower(component.Charge, true);
        }
      }
    }

    public void SpawnAndInstallInitialElectrobanks()
    {
      for (int index = 0; index < this.ElectrobankCountCapacity; ++index)
      {
        GameObject go = Util.KInstantiate(Assets.GetPrefab((Tag) "DisposableElectrobank_RawMetal"), this.transform.position);
        go.SetActive(true);
        this.storage.Store(go);
      }
      this.RefreshCharge();
      this.SetOnlineState(true);
      this.sm.InitialElectrobanksSpawned.Set(true, this);
    }

    public void AutomaticallyDropAllDepletedElectrobanks()
    {
      List<GameObject> result = new List<GameObject>();
      this.storage.Find(GameTags.EmptyPortableBattery, result);
      for (int index = 0; index < result.Count; ++index)
        this.storage.Drop(result[index], true);
    }
  }
}
