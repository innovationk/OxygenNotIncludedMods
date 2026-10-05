// Decompiled with JetBrains decompiler
// Type: BionicUpgradesMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class BionicUpgradesMonitor : 
  GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>
{
  public const int MAX_POSSIBLE_SLOT_COUNT = 8;
  public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State initialize;
  public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State firstSpawn;
  public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State inactive;
  public BionicUpgradesMonitor.ActiveStates active;
  private StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Signal UpgradeSlotAssignationChanged;
  private StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.BoolParameter InitialUpgradeSpawned;

  public static void CreateAssignableSlots(MinionAssignablesProxy minionAssignablesProxy)
  {
    AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
    int num = Mathf.Max(0, 7);
    for (int index = 0; index < num; ++index)
    {
      string IDSufix = (index + 2).ToString();
      BionicUpgradesMonitor.AddAssignableSlot(bionicUpgrade, IDSufix, minionAssignablesProxy);
    }
  }

  private static void AddAssignableSlot(
    AssignableSlot bionicUpgradeSlot,
    string IDSufix,
    MinionAssignablesProxy minionAssignablesProxy)
  {
    Ownables component1 = minionAssignablesProxy.GetComponent<Ownables>();
    switch (bionicUpgradeSlot)
    {
      case OwnableSlot _:
        OwnableSlotInstance slot_instance1 = new OwnableSlotInstance((Assignables) component1, (OwnableSlot) bionicUpgradeSlot);
        slot_instance1.ID += IDSufix;
        component1.Add((AssignableSlotInstance) slot_instance1);
        break;
      case EquipmentSlot _:
        Equipment component2 = component1.GetComponent<Equipment>();
        EquipmentSlotInstance slot_instance2 = new EquipmentSlotInstance((Assignables) component2, (EquipmentSlot) bionicUpgradeSlot);
        slot_instance2.ID += IDSufix;
        component2.Add((AssignableSlotInstance) slot_instance2);
        break;
    }
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.initialize;
    this.initialize.Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.InitializeSlots)).EnterTransition(this.firstSpawn, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsFirstTimeSpawningThisBionic)).EnterGoTo(this.inactive);
    this.firstSpawn.Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.SpawnAndInstallInitialUpgrade));
    this.inactive.EventTransition(GameHashes.BionicOnline, (GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State) this.active, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsBionicOnline)).Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers));
    this.active.DefaultState(this.active.idle).EventTransition(GameHashes.BionicOffline, this.inactive, GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Not(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsBionicOnline))).EventHandler(GameHashes.BionicUpgradeWattageChanged, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers)).Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers));
    this.active.idle.OnSignal(this.UpgradeSlotAssignationChanged, (GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State) this.active.seeking, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Parameter<StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.SignalParameter>.Callback(BionicUpgradesMonitor.WantsToInstallNewUpgrades));
    this.active.seeking.OnSignal(this.UpgradeSlotAssignationChanged, this.active.idle, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Parameter<StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.SignalParameter>.Callback(BionicUpgradesMonitor.DoesNotWantsToInstallNewUpgrades)).DefaultState(this.active.seeking.inProgress);
    this.active.seeking.inProgress.ToggleChore((Func<BionicUpgradesMonitor.Instance, Chore>) (smi => (Chore) new SeekAndInstallBionicUpgradeChore(smi.master)), this.active.idle, this.active.seeking.failed);
    this.active.seeking.failed.EnterTransition(this.active.idle, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.DoesNotWantsToInstallNewUpgrades)).GoTo(this.active.seeking.inProgress);
  }

  public static void InitializeSlots(BionicUpgradesMonitor.Instance smi) => smi.InitializeSlots();

  public static bool IsBionicOnline(BionicUpgradesMonitor.Instance smi) => smi.IsOnline;

  public static bool WantsToInstallNewUpgrades(
    BionicUpgradesMonitor.Instance smi,
    StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.SignalParameter param)
  {
    return smi.HasAnyUpgradeAssigned;
  }

  public static bool DoesNotWantsToInstallNewUpgrades(BionicUpgradesMonitor.Instance smi)
  {
    return BionicUpgradesMonitor.DoesNotWantsToInstallNewUpgrades(smi, (StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.SignalParameter) null);
  }

  public static bool DoesNotWantsToInstallNewUpgrades(
    BionicUpgradesMonitor.Instance smi,
    StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.SignalParameter param)
  {
    return !BionicUpgradesMonitor.WantsToInstallNewUpgrades(smi, param);
  }

  public static bool HasUpgradesInstalled(BionicUpgradesMonitor.Instance smi)
  {
    return smi.HasAnyUpgradeInstalled;
  }

  public static bool IsFirstTimeSpawningThisBionic(BionicUpgradesMonitor.Instance smi)
  {
    return !smi.sm.InitialUpgradeSpawned.Get(smi);
  }

  public static void UpdateBatteryMonitorWattageModifiers(BionicUpgradesMonitor.Instance smi)
  {
    smi.UpdateBatteryMonitorWattageModifiers();
  }

  public static void SpawnAndInstallInitialUpgrade(BionicUpgradesMonitor.Instance smi)
  {
    string traitID = smi.GetComponent<Traits>().GetTraitIds().Find((Predicate<string>) (t => DUPLICANTSTATS.BIONICUPGRADETRAITS.Find((Predicate<DUPLICANTSTATS.TraitVal>) (st => st.id == t)).id == t));
    if (traitID != null)
    {
      GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(BionicUpgradeComponentConfig.GetBionicUpgradePrefabIDWithTraitID(traitID)), smi.master.transform.position);
      gameObject.SetActive(true);
      IAssignableIdentity component1 = smi.GetComponent<IAssignableIdentity>();
      BionicUpgradeComponent component2 = gameObject.GetComponent<BionicUpgradeComponent>();
      component2.Assign(component1);
      smi.InstallUpgrade(component2);
    }
    smi.sm.InitialUpgradeSpawned.Set(true, smi);
    smi.GoTo((StateMachine.BaseState) smi.sm.inactive);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class SeekingStates : 
    GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State
  {
    public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State inProgress;
    public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State failed;
  }

  public class ActiveStates : 
    GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State
  {
    public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State idle;
    public BionicUpgradesMonitor.SeekingStates seeking;
  }

  public new class Instance : 
    GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.GameInstance
  {
    [Serialize]
    public BionicUpgradesMonitor.UpgradeComponentSlot[] upgradeComponentSlots;
    private BionicBatteryMonitor.Instance batteryMonitor;
    private Storage upgradesStorage;
    private Ownables minionOwnables;
    private MinionStorageDataHolder dataHolder;
    private Navigator navigator;

    public bool IsOnline => this.batteryMonitor != null && this.batteryMonitor.IsOnline;

    public bool HasAnyUpgradeAssigned
    {
      get => this.upgradeComponentSlots != null && this.GetAnyAssignedSlot() != null;
    }

    public bool HasAnyUpgradeInstalled
    {
      get => this.upgradeComponentSlots != null && this.GetAnyInstalledUpgradeSlot() != null;
    }

    public int UnlockedSlotCount
    {
      get
      {
        return Math.Clamp((int) this.gameObject.GetAttributes().Get(Db.Get().Attributes.BionicBoosterSlots.Id).GetTotalValue(), 0, 8);
      }
    }

    public int AssignedSlotCount
    {
      get
      {
        int assignedSlotCount = 0;
        for (int index = 0; index < this.upgradeComponentSlots.Length; ++index)
        {
          if ((UnityEngine.Object) this.upgradeComponentSlots[index].assignedUpgradeComponent != (UnityEngine.Object) null)
            ++assignedSlotCount;
        }
        return assignedSlotCount;
      }
    }

    public Instance(IStateMachineTarget master, BionicUpgradesMonitor.Def def)
      : base(master, def)
    {
      IAssignableIdentity component = this.GetComponent<IAssignableIdentity>();
      this.dataHolder = this.GetComponent<MinionStorageDataHolder>();
      this.dataHolder.OnCopyBegins += new System.Action<StoredMinionIdentity>(this.OnCopyMinionBegins);
      this.batteryMonitor = this.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
      this.navigator = this.GetComponent<Navigator>();
      this.minionOwnables = component.GetSoleOwner();
      this.upgradesStorage = this.gameObject.GetComponents<Storage>().FindFirst<Storage>((Func<Storage, bool>) (s => s.storageID == GameTags.StoragesIds.BionicUpgradeStorage));
      this.CreateUpgradeSlots();
      this.Subscribe(540773776, new System.Action<object>(this.OnSlotCountAttributeChanged));
      Game.Instance.Trigger(-1523247426, (object) this);
    }

    private void OnCopyMinionBegins(StoredMinionIdentity destination)
    {
      Tag[] tagArray = new Tag[this.upgradeComponentSlots.Length];
      for (int index = 0; index < this.upgradeComponentSlots.Length; ++index)
        tagArray[index] = this.upgradeComponentSlots[index].InstalledUpgradeID;
      this.dataHolder.UpdateData<BionicUpgradesMonitor.Instance>(new MinionStorageDataHolder.DataPackData()
      {
        Bools = new bool[1]
        {
          this.smi.sm.InitialUpgradeSpawned.Get(this.smi)
        },
        Tags = tagArray
      });
    }

    public override void PostParamsInitialized()
    {
      MinionStorageDataHolder.DataPack dataPack = this.dataHolder.GetDataPack<BionicUpgradesMonitor.Instance>();
      if (dataPack != null && dataPack.IsStoringNewData)
      {
        MinionStorageDataHolder.DataPackData dataPackData = dataPack.ReadData();
        if (dataPackData != null)
        {
          this.sm.InitialUpgradeSpawned.Set(dataPackData.Bools[0], this.smi);
          if (dataPackData.Tags != null)
          {
            for (int index = 0; index < Mathf.Min(dataPackData.Tags.Length, this.upgradeComponentSlots.Length); ++index)
            {
              Tag tag = dataPackData.Tags[index];
              this.upgradeComponentSlots[index].DeserializeAction_OverrideInstalledUpgradePrefabID(tag);
            }
          }
        }
      }
      base.PostParamsInitialized();
    }

    protected override void OnCleanUp()
    {
      if ((UnityEngine.Object) this.dataHolder != (UnityEngine.Object) null)
        this.dataHolder.OnCopyBegins -= new System.Action<StoredMinionIdentity>(this.OnCopyMinionBegins);
      base.OnCleanUp();
    }

    public void LockSlot(BionicUpgradesMonitor.UpgradeComponentSlot slot)
    {
      this.UninstallUpgrade(slot);
      if (slot.HasUpgradeComponentAssigned && slot.HasSpawned)
        slot.InternalUninstall();
      slot.InternalLock();
    }

    public void UnlockSlot(BionicUpgradesMonitor.UpgradeComponentSlot slot)
    {
      slot.InternalUnlock();
    }

    public void InstallUpgrade(BionicUpgradeComponent upgradeComponent)
    {
      BionicUpgradesMonitor.UpgradeComponentSlot forAssignedUpgrade = this.GetSlotForAssignedUpgrade(upgradeComponent);
      if (forAssignedUpgrade == null)
        return;
      forAssignedUpgrade.InternalInstall();
      Game.Instance.Trigger(-1523247426, (object) this);
    }

    public void UninstallUpgrade(BionicUpgradesMonitor.UpgradeComponentSlot slot)
    {
      if (slot == null || !slot.HasUpgradeInstalled)
        return;
      slot.InternalUninstall();
      Game.Instance.Trigger(-1523247426, (object) this);
    }

    public void UpdateBatteryMonitorWattageModifiers()
    {
      bool flag1 = true;
      bool flag2 = false;
      for (int index = 0; index < this.upgradeComponentSlots.Length; ++index)
      {
        flag1 &= this.upgradeComponentSlots[index].HasUpgradeInstalled;
        string modifierID = "UPGRADE_SLOT_" + index.ToString();
        BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[index];
        if (!upgradeComponentSlot.HasUpgradeInstalled)
        {
          flag2 |= this.batteryMonitor.RemoveModifier(modifierID, false);
        }
        else
        {
          BionicBatteryMonitor.WattageModifier modifier = new BionicBatteryMonitor.WattageModifier()
          {
            id = modifierID,
            name = upgradeComponentSlot.installedUpgradeComponent.CurrentWattageName,
            value = upgradeComponentSlot.installedUpgradeComponent.CurrentWattage,
            potentialValue = upgradeComponentSlot.installedUpgradeComponent.PotentialWattage
          };
          flag2 |= this.batteryMonitor.AddOrUpdateModifier(modifier, false);
        }
      }
      if (flag2)
        this.batteryMonitor.Trigger(1361471071);
      if (!flag1)
        return;
      SaveGame.Instance.ColonyAchievementTracker.fullyBoostedBionic = true;
    }

    private void OnSlotCountAttributeChanged(object data)
    {
      int unlockedSlotCount = this.UnlockedSlotCount;
      bool flag1 = false;
      for (int index = 0; index < this.upgradeComponentSlots.Length; ++index)
      {
        BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[index];
        bool flag2 = index >= unlockedSlotCount;
        if (upgradeComponentSlot.IsLocked != flag2)
        {
          flag1 = true;
          if (flag2)
            this.LockSlot(upgradeComponentSlot);
          else
            this.UnlockSlot(upgradeComponentSlot);
        }
      }
      this.UpdateBatteryMonitorWattageModifiers();
      if (!flag1)
        return;
      this.Trigger(1095596132);
    }

    private void CreateUpgradeSlots()
    {
      this.minionOwnables.GetSlots(Db.Get().AssignableSlots.BionicUpgrade);
      this.upgradeComponentSlots = new BionicUpgradesMonitor.UpgradeComponentSlot[8];
      for (int index = 0; index < this.upgradeComponentSlots.Length; ++index)
      {
        BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = new BionicUpgradesMonitor.UpgradeComponentSlot();
        this.upgradeComponentSlots[index] = upgradeComponentSlot;
      }
    }

    public void InitializeSlots()
    {
      AssignableSlotInstance[] slots = this.minionOwnables.GetSlots(Db.Get().AssignableSlots.BionicUpgrade);
      int unlockedSlotCount = this.UnlockedSlotCount;
      for (int index = 0; index < this.upgradeComponentSlots.Length; ++index)
        this.InitializeUpgradeSlot(this.upgradeComponentSlots[index], slots[index]);
      for (int index = 0; index < this.upgradeComponentSlots.Length; ++index)
      {
        BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[index];
        upgradeComponentSlot.OnSpawn(this);
        bool flag = index >= unlockedSlotCount;
        if (flag != upgradeComponentSlot.IsLocked)
        {
          if (flag)
            this.LockSlot(upgradeComponentSlot);
          else
            this.UnlockSlot(upgradeComponentSlot);
        }
      }
    }

    private void InitializeUpgradeSlot(
      BionicUpgradesMonitor.UpgradeComponentSlot slot,
      AssignableSlotInstance assignableSlotInstance)
    {
      slot.Initialize(assignableSlotInstance, this.upgradesStorage, this);
      slot.OnInstalledUpgradeReassigned += new System.Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity>(this.OnInstalledUpgradeComponentReassigned);
      slot.OnAssignedUpgradeChanged += new System.Action<BionicUpgradesMonitor.UpgradeComponentSlot>(this.OnSlotAssignationChanged);
    }

    private void OnSlotAssignationChanged(BionicUpgradesMonitor.UpgradeComponentSlot slot)
    {
      this.sm.UpgradeSlotAssignationChanged.Trigger(this);
    }

    private void OnInstalledUpgradeComponentReassigned(
      BionicUpgradesMonitor.UpgradeComponentSlot slot,
      IAssignableIdentity new_assignee)
    {
      if (slot.AssignedUpgradeMatchesInstalledUpgrade)
        return;
      this.UninstallUpgrade(slot);
    }

    private BionicUpgradesMonitor.UpgradeComponentSlot GetSlotForAssignedUpgrade(
      BionicUpgradeComponent upgradeComponent)
    {
      for (int index = 0; index < this.upgradeComponentSlots.Length; ++index)
      {
        BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[index];
        if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned && (UnityEngine.Object) upgradeComponentSlot.assignedUpgradeComponent == (UnityEngine.Object) upgradeComponent)
          return upgradeComponentSlot;
      }
      return (BionicUpgradesMonitor.UpgradeComponentSlot) null;
    }

    public BionicUpgradesMonitor.UpgradeComponentSlot GetAnyAssignedSlot()
    {
      for (int index = 0; index < this.upgradeComponentSlots.Length; ++index)
      {
        BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[index];
        if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned)
          return upgradeComponentSlot;
      }
      return (BionicUpgradesMonitor.UpgradeComponentSlot) null;
    }

    public BionicUpgradesMonitor.UpgradeComponentSlot GetAnyReachableAssignedSlot()
    {
      for (int index = 0; index < this.upgradeComponentSlots.Length; ++index)
      {
        BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[index];
        if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned && this.IsBionicUpgradeComponentObjectAbleToBePickedUp(upgradeComponentSlot.assignedUpgradeComponent))
          return upgradeComponentSlot;
      }
      return (BionicUpgradesMonitor.UpgradeComponentSlot) null;
    }

    public bool IsBionicUpgradeComponentObjectAbleToBePickedUp(
      BionicUpgradeComponent upgradecComponent)
    {
      Pickupable component = upgradecComponent.GetComponent<Pickupable>();
      return !((UnityEngine.Object) component == (UnityEngine.Object) null) && !component.KPrefabID.HasTag(GameTags.StoredPrivate) && component.CouldBePickedUpByMinion(this.GetComponent<KPrefabID>().InstanceID) && this.navigator.CanReach((IApproachable) component);
    }

    private BionicUpgradesMonitor.UpgradeComponentSlot GetAnyInstalledUpgradeSlot()
    {
      for (int index = 0; index < this.upgradeComponentSlots.Length; ++index)
      {
        BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[index];
        if (upgradeComponentSlot != null && upgradeComponentSlot.HasUpgradeInstalled)
          return upgradeComponentSlot;
      }
      return (BionicUpgradesMonitor.UpgradeComponentSlot) null;
    }

    public BionicUpgradesMonitor.UpgradeComponentSlot GetFirstEmptyAvailableSlot()
    {
      for (int index = 0; index < this.upgradeComponentSlots.Length; ++index)
      {
        BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[index];
        if (!upgradeComponentSlot.IsLocked && !upgradeComponentSlot.HasUpgradeInstalled && !upgradeComponentSlot.HasUpgradeComponentAssigned)
          return upgradeComponentSlot;
      }
      return (BionicUpgradesMonitor.UpgradeComponentSlot) null;
    }

    public int CountBoosterAssignments(Tag boosterID)
    {
      int num = 0;
      foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in this.upgradeComponentSlots)
      {
        if (!((UnityEngine.Object) upgradeComponentSlot.assignedUpgradeComponent == (UnityEngine.Object) null) && upgradeComponentSlot.assignedUpgradeComponent.PrefabID() == boosterID)
          ++num;
      }
      return num;
    }

    [SerializationConfig(MemberSerialization.OptIn)]
    private struct StorageDataHolderData
    {
      [Serialize]
      public bool initialUpgradesSpawned;
      [Serialize]
      public Tag[] upgradeComponentSlotsInstalledTags;
    }
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  public class UpgradeComponentSlot
  {
    private BionicUpgradeComponent _installedUpgradeComponent;
    private BionicUpgradeComponent _lastAssignedUpgradeComponent;
    [Serialize]
    private Tag installedUpgradePrefabID = Tag.Invalid;
    public System.Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity> OnInstalledUpgradeReassigned;
    public System.Action<BionicUpgradesMonitor.UpgradeComponentSlot> OnAssignedUpgradeChanged;
    private AssignableSlotInstance assignableSlotInstance;
    private Storage storage;
    private int installedUpgradeSubscribeCallbackIDX = -1;
    private StateMachine.Instance _upgradeSmi;
    private BionicUpgradesMonitor.Instance master;

    public bool HasUpgradeInstalled => this.installedUpgradePrefabID != Tag.Invalid;

    public bool HasUpgradeComponentAssigned
    {
      get
      {
        return this.assignableSlotInstance.IsAssigned() && !this.assignableSlotInstance.IsUnassigning();
      }
    }

    public bool AssignedUpgradeMatchesInstalledUpgrade
    {
      get => (UnityEngine.Object) this.assignedUpgradeComponent == (UnityEngine.Object) this.installedUpgradeComponent;
    }

    public bool HasSpawned { private set; get; }

    public bool IsLocked { private set; get; }

    public float WattageCost
    {
      get => !this.HasUpgradeInstalled ? 0.0f : this.installedUpgradeComponent.CurrentWattage;
    }

    public Func<StateMachine.Instance, StateMachine.Instance> StateMachine
    {
      get
      {
        return !this.HasUpgradeInstalled ? (Func<StateMachine.Instance, StateMachine.Instance>) null : this.installedUpgradeComponent.StateMachine;
      }
    }

    public Tag InstalledUpgradeID => this.installedUpgradePrefabID;

    public BionicUpgradeComponent assignedUpgradeComponent
    {
      get
      {
        return !this.assignableSlotInstance.IsUnassigning() ? this.assignableSlotInstance.assignable as BionicUpgradeComponent : (BionicUpgradeComponent) null;
      }
    }

    public BionicUpgradeComponent installedUpgradeComponent
    {
      get
      {
        if (this.HasUpgradeInstalled)
        {
          if ((UnityEngine.Object) this._installedUpgradeComponent == (UnityEngine.Object) null)
          {
            Debug.LogWarning((object) $"Error on BionicUpgradeMonitor. storage does not contains bionic upgrade with id {this.InstalledUpgradeID.ToString()} this could be due to loading an old save on a new version");
            this.installedUpgradePrefabID = Tag.Invalid;
          }
          return this._installedUpgradeComponent;
        }
        this._installedUpgradeComponent = (BionicUpgradeComponent) null;
        return (BionicUpgradeComponent) null;
      }
    }

    public void DeserializeAction_OverrideInstalledUpgradePrefabID(Tag installedUpgradePrefabID)
    {
      this.installedUpgradePrefabID = installedUpgradePrefabID;
    }

    public void Initialize(
      AssignableSlotInstance assignableSlotInstance,
      Storage storage,
      BionicUpgradesMonitor.Instance master)
    {
      this.assignableSlotInstance = assignableSlotInstance;
      this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().Subscribe(-1585839766, new System.Action<object>(this.OnAssignablesChanged));
      this.storage = storage;
      this.master = master;
      this._lastAssignedUpgradeComponent = this.assignedUpgradeComponent;
    }

    public AssignableSlotInstance GetAssignableSlotInstance() => this.assignableSlotInstance;

    public void OnSpawn(BionicUpgradesMonitor.Instance smi)
    {
      if (this.HasUpgradeInstalled && (UnityEngine.Object) this._installedUpgradeComponent == (UnityEngine.Object) null)
      {
        GameObject gameObject1 = (GameObject) null;
        int index = 0;
        List<GameObject> result = new List<GameObject>();
        this.storage.Find(this.InstalledUpgradeID, result);
        for (; index < result.Count && (UnityEngine.Object) this._installedUpgradeComponent == (UnityEngine.Object) null; ++index)
        {
          GameObject gameObject2 = result[index];
          bool flag = false;
          foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in smi.upgradeComponentSlots)
          {
            if (upgradeComponentSlot != this && upgradeComponentSlot.HasSpawned && !(upgradeComponentSlot.InstalledUpgradeID != this.InstalledUpgradeID) && (UnityEngine.Object) upgradeComponentSlot.installedUpgradeComponent.gameObject == (UnityEngine.Object) gameObject2)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            gameObject1 = gameObject2;
            break;
          }
        }
        if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null)
        {
          this._installedUpgradeComponent = gameObject1.GetComponent<BionicUpgradeComponent>();
          this.StartBoosterSM();
        }
      }
      if (this.HasUpgradeInstalled && (UnityEngine.Object) this.installedUpgradeComponent != (UnityEngine.Object) null)
      {
        if (!this.HasUpgradeComponentAssigned)
          this.installedUpgradeComponent.Assign((IAssignableIdentity) this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>(), this.assignableSlotInstance);
        this.SubscribeToInstallledUpgradeAssignable();
      }
      this.HasSpawned = true;
    }

    public void SubscribeToInstallledUpgradeAssignable()
    {
      this.UnsubscribeFromInstalledUpgradeAssignable();
      this.installedUpgradeSubscribeCallbackIDX = this.installedUpgradeComponent.Subscribe(684616645, new System.Action<object>(this.OnInstalledComponentReassigned));
    }

    public void UnsubscribeFromInstalledUpgradeAssignable()
    {
      if (this.installedUpgradeSubscribeCallbackIDX == -1)
        return;
      this.installedUpgradeComponent.Unsubscribe(this.installedUpgradeSubscribeCallbackIDX);
      this.installedUpgradeSubscribeCallbackIDX = -1;
    }

    private void OnInstalledComponentReassigned(object obj)
    {
      IAssignableIdentity assignableIdentity = obj == null ? (IAssignableIdentity) null : (IAssignableIdentity) obj;
      System.Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity> upgradeReassigned = this.OnInstalledUpgradeReassigned;
      if (upgradeReassigned == null)
        return;
      upgradeReassigned(this, assignableIdentity);
    }

    private void OnAssignablesChanged(object o)
    {
      if (!((UnityEngine.Object) this._lastAssignedUpgradeComponent != (UnityEngine.Object) this.assignedUpgradeComponent))
        return;
      this._lastAssignedUpgradeComponent = this.assignedUpgradeComponent;
      System.Action<BionicUpgradesMonitor.UpgradeComponentSlot> assignedUpgradeChanged = this.OnAssignedUpgradeChanged;
      if (assignedUpgradeChanged == null)
        return;
      assignedUpgradeChanged(this);
    }

    private void StartBoosterSM()
    {
      this._upgradeSmi = this.installedUpgradeComponent.StateMachine((StateMachine.Instance) this.master);
      this._upgradeSmi.StartSM();
    }

    public void InternalInstall()
    {
      if (this.HasUpgradeInstalled || !this.HasUpgradeComponentAssigned)
        return;
      this.storage.Store(this.assignedUpgradeComponent.gameObject, true);
      this.installedUpgradePrefabID = this.assignedUpgradeComponent.PrefabID();
      this._installedUpgradeComponent = this.assignedUpgradeComponent;
      this.SubscribeToInstallledUpgradeAssignable();
      this.StartBoosterSM();
      GameObject targetGameObject = this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
      if (!((UnityEngine.Object) targetGameObject != (UnityEngine.Object) null))
        return;
      targetGameObject.Trigger(2000325176);
    }

    public void InternalUninstall()
    {
      if (!this.HasUpgradeInstalled)
        return;
      this.UnsubscribeFromInstalledUpgradeAssignable();
      GameObject gameObject = this.installedUpgradeComponent.gameObject;
      this.installedUpgradeComponent.Unassign();
      this.storage.Drop(gameObject, true);
      this.installedUpgradePrefabID = Tag.Invalid;
      this._installedUpgradeComponent = (BionicUpgradeComponent) null;
      if (this._upgradeSmi != null)
      {
        this._upgradeSmi.StopSM("Uninstall");
        this._upgradeSmi = (StateMachine.Instance) null;
      }
      GameObject targetGameObject = this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
      if (!((UnityEngine.Object) targetGameObject != (UnityEngine.Object) null))
        return;
      targetGameObject.Trigger(2000325176);
    }

    public void InternalLock() => this.IsLocked = true;

    public void InternalUnlock() => this.IsLocked = false;
  }
}
