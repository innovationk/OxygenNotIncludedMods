// Decompiled with JetBrains decompiler
// Type: SuitLocker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SuitLocker : StateMachineComponent<SuitLocker.StatesInstance>
{
  [MyCmpGet]
  private Building building;
  public Tag[] OutfitTags;
  private float OutfitMass;
  private FetchChore fetchChore;
  [MyCmpAdd]
  public SuitLocker.ReturnSuitWorkable returnSuitWorkable;
  private MeterController meter;
  private SuitLocker.SuitMarkerState suitMarkerState;

  public float OxygenAvailable
  {
    get
    {
      KPrefabID storedOutfit = this.GetStoredOutfit();
      return (UnityEngine.Object) storedOutfit == (UnityEngine.Object) null ? 0.0f : storedOutfit.GetComponent<SuitTank>().PercentFull();
    }
  }

  public float BatteryAvailable
  {
    get
    {
      KPrefabID storedOutfit = this.GetStoredOutfit();
      return (UnityEngine.Object) storedOutfit == (UnityEngine.Object) null ? 0.0f : storedOutfit.GetComponent<LeadSuitTank>().batteryCharge;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[3]
    {
      "meter_target",
      "meter_arrow",
      "meter_scale"
    });
    DebugUtil.DevAssert(this.OutfitTags.Length == 1, $"Suit Locker {this.name} requesting more than one suit type, this will break the fetch chore");
    if (this.OutfitTags.Length == 1)
    {
      GameObject prefab = Assets.GetPrefab(this.OutfitTags[0]);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
        this.OutfitMass = prefab.GetComponent<PrimaryElement>().MassPerUnit;
    }
    else
      this.OutfitMass = (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_MASS;
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), this.gameObject);
    this.smi.StartSM();
    Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Suits);
  }

  public KPrefabID GetStoredOutfit()
  {
    foreach (GameObject gameObject in this.GetComponent<Storage>().items)
    {
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
      {
        KPrefabID component = gameObject.GetComponent<KPrefabID>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.IsAnyPrefabID(this.OutfitTags))
          return component;
      }
    }
    return (KPrefabID) null;
  }

  public float GetSuitScore()
  {
    float suitScore = -1f;
    KPrefabID partiallyChargedOutfit = this.GetPartiallyChargedOutfit();
    if ((bool) (UnityEngine.Object) partiallyChargedOutfit)
    {
      suitScore = partiallyChargedOutfit.GetComponent<SuitTank>().PercentFull();
      JetSuitTank component = partiallyChargedOutfit.GetComponent<JetSuitTank>();
      if ((bool) (UnityEngine.Object) component && (double) component.PercentFull() < (double) suitScore)
        suitScore = component.PercentFull();
    }
    return suitScore;
  }

  public KPrefabID GetPartiallyChargedOutfit()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!(bool) (UnityEngine.Object) storedOutfit)
      return (KPrefabID) null;
    if ((double) storedOutfit.GetComponent<SuitTank>().PercentFull() < (double) TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE)
      return (KPrefabID) null;
    JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
    return (bool) (UnityEngine.Object) component && (double) component.PercentFull() < (double) TUNING.EQUIPMENT.SUITS.MINIMUM_USABLE_SUIT_CHARGE ? (KPrefabID) null : storedOutfit;
  }

  public KPrefabID GetFullyChargedOutfit()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!(bool) (UnityEngine.Object) storedOutfit)
      return (KPrefabID) null;
    if (!storedOutfit.GetComponent<SuitTank>().IsFull())
      return (KPrefabID) null;
    JetSuitTank component = storedOutfit.GetComponent<JetSuitTank>();
    return (bool) (UnityEngine.Object) component && !component.IsFull() ? (KPrefabID) null : storedOutfit;
  }

  private void CreateFetchChore()
  {
    this.fetchChore = new FetchChore(Db.Get().ChoreTypes.EquipmentFetch, this.GetComponent<Storage>(), this.OutfitMass, new HashSet<Tag>((IEnumerable<Tag>) this.OutfitTags), FetchChore.MatchCriteria.MatchID, Tag.Invalid, new Tag[1]
    {
      GameTags.Assigned
    }, operational_requirement: Operational.State.None);
    this.fetchChore.allowMultifetch = false;
  }

  private void CancelFetchChore()
  {
    if (this.fetchChore == null)
      return;
    this.fetchChore.Cancel("SuitLocker.CancelFetchChore");
    this.fetchChore = (FetchChore) null;
  }

  public bool HasOxygen()
  {
    GameObject oxygen = this.GetOxygen();
    return (UnityEngine.Object) oxygen != (UnityEngine.Object) null && (double) oxygen.GetComponent<PrimaryElement>().Mass > 0.0;
  }

  private void RefreshMeter()
  {
    GameObject oxygen = this.GetOxygen();
    float percent_full = 0.0f;
    if ((UnityEngine.Object) oxygen != (UnityEngine.Object) null)
      percent_full = Math.Min(oxygen.GetComponent<PrimaryElement>().Mass / this.GetComponent<ConduitConsumer>().capacityKG, 1f);
    this.meter.SetPositionPercent(percent_full);
  }

  public bool IsSuitFullyCharged()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!((UnityEngine.Object) storedOutfit != (UnityEngine.Object) null))
      return false;
    SuitTank component1 = storedOutfit.GetComponent<SuitTank>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (double) component1.PercentFull() < 1.0)
      return false;
    JetSuitTank component2 = storedOutfit.GetComponent<JetSuitTank>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (double) component2.PercentFull() < 1.0)
      return false;
    LeadSuitTank component3 = (UnityEngine.Object) storedOutfit != (UnityEngine.Object) null ? storedOutfit.GetComponent<LeadSuitTank>() : (LeadSuitTank) null;
    return !((UnityEngine.Object) component3 != (UnityEngine.Object) null) || (double) component3.PercentFull() >= 1.0;
  }

  public bool IsOxygenTankFull()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if (!((UnityEngine.Object) storedOutfit != (UnityEngine.Object) null))
      return false;
    SuitTank component = storedOutfit.GetComponent<SuitTank>();
    return (UnityEngine.Object) component == (UnityEngine.Object) null || (double) component.PercentFull() >= 1.0;
  }

  private void OnRequestOutfit() => this.smi.sm.isWaitingForSuit.Set(true, this.smi);

  private void OnCancelRequest() => this.smi.sm.isWaitingForSuit.Set(false, this.smi);

  public void DropSuit()
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if ((UnityEngine.Object) storedOutfit == (UnityEngine.Object) null)
      return;
    this.GetComponent<Storage>().Drop(storedOutfit.gameObject, true);
  }

  public void EquipTo(Equipment equipment)
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if ((UnityEngine.Object) storedOutfit == (UnityEngine.Object) null)
      return;
    this.GetComponent<Storage>().Drop(storedOutfit.gameObject, true);
    Prioritizable component = storedOutfit.GetComponent<Prioritizable>();
    PrioritySetting masterPriority = component.GetMasterPriority();
    PrioritySetting priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.GetMasterPriority().priority_class == PriorityScreen.PriorityClass.topPriority)
      component.SetMasterPriority(priority);
    storedOutfit.GetComponent<Equippable>().Assign(equipment.GetComponent<IAssignableIdentity>());
    storedOutfit.GetComponent<EquippableWorkable>().CancelChore("Manual equip");
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.GetMasterPriority() != masterPriority)
      component.SetMasterPriority(masterPriority);
    equipment.Equip(storedOutfit.GetComponent<Equippable>());
    this.returnSuitWorkable.CreateChore();
  }

  public void UnequipFrom(Equipment equipment)
  {
    Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
    assignable.Unassign();
    Durability component = assignable.GetComponent<Durability>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.IsWornOut())
      this.ConfigRequestSuit();
    else
      this.GetComponent<Storage>().Store(assignable.gameObject);
  }

  public void ConfigRequestSuit()
  {
    this.smi.sm.isConfigured.Set(true, this.smi);
    this.smi.sm.isWaitingForSuit.Set(true, this.smi);
  }

  public void ConfigNoSuit()
  {
    this.smi.sm.isConfigured.Set(true, this.smi);
    this.smi.sm.isWaitingForSuit.Set(false, this.smi);
  }

  public bool CanDropOffSuit()
  {
    return this.smi.sm.isConfigured.Get(this.smi) && !this.smi.sm.isWaitingForSuit.Get(this.smi) && (UnityEngine.Object) this.GetStoredOutfit() == (UnityEngine.Object) null;
  }

  private GameObject GetOxygen() => this.GetComponent<Storage>().FindFirst(GameTags.Oxygen);

  private void ChargeSuit(float dt)
  {
    KPrefabID storedOutfit = this.GetStoredOutfit();
    if ((UnityEngine.Object) storedOutfit == (UnityEngine.Object) null)
      return;
    GameObject oxygen = this.GetOxygen();
    if ((UnityEngine.Object) oxygen == (UnityEngine.Object) null)
      return;
    SuitTank component = storedOutfit.GetComponent<SuitTank>();
    float b = Mathf.Min((float) ((double) component.capacity * 15.0 * (double) dt / 600.0), component.capacity - component.GetTankAmount());
    float amount = Mathf.Min(oxygen.GetComponent<PrimaryElement>().Mass, b);
    if ((double) amount <= 0.0)
      return;
    double num = (double) this.GetComponent<Storage>().Transfer(component.storage, component.elementTag, amount, hide_popups: true);
  }

  public void SetSuitMarker(SuitMarker suit_marker)
  {
    SuitLocker.SuitMarkerState suitMarkerState = SuitLocker.SuitMarkerState.HasMarker;
    if ((UnityEngine.Object) suit_marker == (UnityEngine.Object) null)
      suitMarkerState = SuitLocker.SuitMarkerState.NoMarker;
    else if ((double) suit_marker.transform.GetPosition().x > (double) this.transform.GetPosition().x && suit_marker.GetComponent<Rotatable>().IsRotated)
      suitMarkerState = SuitLocker.SuitMarkerState.WrongSide;
    else if ((double) suit_marker.transform.GetPosition().x < (double) this.transform.GetPosition().x && !suit_marker.GetComponent<Rotatable>().IsRotated)
      suitMarkerState = SuitLocker.SuitMarkerState.WrongSide;
    else if (!suit_marker.GetComponent<Operational>().IsOperational)
      suitMarkerState = SuitLocker.SuitMarkerState.NotOperational;
    if (suitMarkerState == this.suitMarkerState)
      return;
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoSuitMarker);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SuitMarkerWrongSide);
    switch (suitMarkerState)
    {
      case SuitLocker.SuitMarkerState.NoMarker:
        this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSuitMarker);
        break;
      case SuitLocker.SuitMarkerState.WrongSide:
        this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SuitMarkerWrongSide);
        break;
    }
    this.suitMarkerState = suitMarkerState;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    SuitLocker.UpdateSuitMarkerStates(Grid.PosToCell(this.transform.position), (GameObject) null);
  }

  private static void GatherSuitBuildings(
    int cell,
    int dir,
    List<SuitLocker.SuitLockerEntry> suit_lockers,
    List<SuitLocker.SuitMarkerEntry> suit_markers)
  {
    int x = dir;
    while (true)
    {
      int cell1 = Grid.OffsetCell(cell, x, 0);
      if (!Grid.IsValidCell(cell1) || SuitLocker.GatherSuitBuildingsOnCell(cell1, suit_lockers, suit_markers))
        x += dir;
      else
        break;
    }
  }

  private static bool GatherSuitBuildingsOnCell(
    int cell,
    List<SuitLocker.SuitLockerEntry> suit_lockers,
    List<SuitLocker.SuitMarkerEntry> suit_markers)
  {
    GameObject gameObject = Grid.Objects[cell, 1];
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return false;
    SuitMarker component1 = gameObject.GetComponent<SuitMarker>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      suit_markers.Add(new SuitLocker.SuitMarkerEntry()
      {
        suitMarker = component1,
        cell = cell
      });
      return true;
    }
    SuitLocker component2 = gameObject.GetComponent<SuitLocker>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return false;
    suit_lockers.Add(new SuitLocker.SuitLockerEntry()
    {
      suitLocker = component2,
      cell = cell
    });
    return true;
  }

  private static SuitMarker FindSuitMarker(int cell, List<SuitLocker.SuitMarkerEntry> suit_markers)
  {
    if (!Grid.IsValidCell(cell))
      return (SuitMarker) null;
    foreach (SuitLocker.SuitMarkerEntry suitMarker in suit_markers)
    {
      if (suitMarker.cell == cell)
        return suitMarker.suitMarker;
    }
    return (SuitMarker) null;
  }

  public static void UpdateSuitMarkerStates(int cell, GameObject self)
  {
    ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.PooledList suit_lockers = ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.Allocate();
    ListPool<SuitLocker.SuitMarkerEntry, SuitLocker>.PooledList suit_markers1 = ListPool<SuitLocker.SuitMarkerEntry, SuitLocker>.Allocate();
    if ((UnityEngine.Object) self != (UnityEngine.Object) null)
    {
      SuitLocker component1 = self.GetComponent<SuitLocker>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        suit_lockers.Add(new SuitLocker.SuitLockerEntry()
        {
          suitLocker = component1,
          cell = cell
        });
      SuitMarker component2 = self.GetComponent<SuitMarker>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        suit_markers1.Add(new SuitLocker.SuitMarkerEntry()
        {
          suitMarker = component2,
          cell = cell
        });
    }
    SuitLocker.GatherSuitBuildings(cell, 1, (List<SuitLocker.SuitLockerEntry>) suit_lockers, (List<SuitLocker.SuitMarkerEntry>) suit_markers1);
    SuitLocker.GatherSuitBuildings(cell, -1, (List<SuitLocker.SuitLockerEntry>) suit_lockers, (List<SuitLocker.SuitMarkerEntry>) suit_markers1);
    suit_lockers.Sort((IComparer<SuitLocker.SuitLockerEntry>) SuitLocker.SuitLockerEntry.comparer);
    for (int index1 = 0; index1 < suit_lockers.Count; ++index1)
    {
      SuitLocker.SuitLockerEntry suitLockerEntry1 = suit_lockers[index1];
      SuitLocker.SuitLockerEntry suitLockerEntry2 = suitLockerEntry1;
      ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.PooledList pooledList = ListPool<SuitLocker.SuitLockerEntry, SuitLocker>.Allocate();
      pooledList.Add(suitLockerEntry1);
      for (int index2 = index1 + 1; index2 < suit_lockers.Count; ++index2)
      {
        SuitLocker.SuitLockerEntry suitLockerEntry3 = suit_lockers[index2];
        if (Grid.CellRight(suitLockerEntry2.cell) == suitLockerEntry3.cell)
        {
          ++index1;
          suitLockerEntry2 = suitLockerEntry3;
          pooledList.Add(suitLockerEntry3);
        }
        else
          break;
      }
      int cell1 = Grid.CellLeft(suitLockerEntry1.cell);
      int cell2 = Grid.CellRight(suitLockerEntry2.cell);
      ListPool<SuitLocker.SuitMarkerEntry, SuitLocker>.PooledList suit_markers2 = suit_markers1;
      SuitMarker suitMarker = SuitLocker.FindSuitMarker(cell1, (List<SuitLocker.SuitMarkerEntry>) suit_markers2);
      if ((UnityEngine.Object) suitMarker == (UnityEngine.Object) null)
        suitMarker = SuitLocker.FindSuitMarker(cell2, (List<SuitLocker.SuitMarkerEntry>) suit_markers1);
      foreach (SuitLocker.SuitLockerEntry suitLockerEntry4 in (List<SuitLocker.SuitLockerEntry>) pooledList)
        suitLockerEntry4.suitLocker.SetSuitMarker(suitMarker);
      pooledList.Recycle();
    }
    suit_lockers.Recycle();
    suit_markers1.Recycle();
  }

  [AddComponentMenu("KMonoBehaviour/Workable/ReturnSuitWorkable")]
  public class ReturnSuitWorkable : Workable
  {
    public static readonly Chore.Precondition DoesSuitNeedRechargingUrgent = new Chore.Precondition()
    {
      id = nameof (DoesSuitNeedRechargingUrgent),
      description = (string) DUPLICANTS.CHORES.PRECONDITIONS.DOES_SUIT_NEED_RECHARGING_URGENT,
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
      {
        Equipment equipment = context.consumerState.equipment;
        if ((UnityEngine.Object) equipment == (UnityEngine.Object) null)
          return false;
        AssignableSlotInstance slot = equipment.GetSlot(Db.Get().AssignableSlots.Suit);
        if ((UnityEngine.Object) slot.assignable == (UnityEngine.Object) null)
          return false;
        Equippable component1 = slot.assignable.GetComponent<Equippable>();
        if ((UnityEngine.Object) component1 == (UnityEngine.Object) null || !component1.isEquipped)
          return false;
        SuitTank component2 = slot.assignable.GetComponent<SuitTank>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.NeedsRecharging())
          return true;
        JetSuitTank component3 = slot.assignable.GetComponent<JetSuitTank>();
        if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && component3.NeedsRecharging())
          return true;
        LeadSuitTank component4 = slot.assignable.GetComponent<LeadSuitTank>();
        return (UnityEngine.Object) component4 != (UnityEngine.Object) null && component4.NeedsRecharging();
      })
    };
    public static readonly Chore.Precondition DoesSuitNeedRechargingIdle = new Chore.Precondition()
    {
      id = nameof (DoesSuitNeedRechargingIdle),
      description = (string) DUPLICANTS.CHORES.PRECONDITIONS.DOES_SUIT_NEED_RECHARGING_IDLE,
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
      {
        Equipment equipment = context.consumerState.equipment;
        if ((UnityEngine.Object) equipment == (UnityEngine.Object) null)
          return false;
        AssignableSlotInstance slot = equipment.GetSlot(Db.Get().AssignableSlots.Suit);
        if ((UnityEngine.Object) slot.assignable == (UnityEngine.Object) null)
          return false;
        Equippable component = slot.assignable.GetComponent<Equippable>();
        return !((UnityEngine.Object) component == (UnityEngine.Object) null) && component.isEquipped && ((UnityEngine.Object) slot.assignable.GetComponent<SuitTank>() != (UnityEngine.Object) null || (UnityEngine.Object) slot.assignable.GetComponent<JetSuitTank>() != (UnityEngine.Object) null || (UnityEngine.Object) slot.assignable.GetComponent<LeadSuitTank>() != (UnityEngine.Object) null);
      })
    };
    public static Chore.Precondition HasSuitMarker = new Chore.Precondition()
    {
      id = "IsValid",
      description = (string) DUPLICANTS.CHORES.PRECONDITIONS.HAS_SUIT_MARKER,
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => ((SuitLocker) data).suitMarkerState == SuitLocker.SuitMarkerState.HasMarker)
    };
    public static Chore.Precondition SuitTypeMatchesLocker = new Chore.Precondition()
    {
      id = "IsValid",
      description = (string) DUPLICANTS.CHORES.PRECONDITIONS.HAS_SUIT_MARKER,
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
      {
        SuitLocker suitLocker = (SuitLocker) data;
        Equipment equipment = context.consumerState.equipment;
        if ((UnityEngine.Object) equipment == (UnityEngine.Object) null)
          return false;
        AssignableSlotInstance slot = equipment.GetSlot(Db.Get().AssignableSlots.Suit);
        return !((UnityEngine.Object) slot.assignable == (UnityEngine.Object) null) && slot.assignable.GetComponent<KPrefabID>().IsAnyPrefabID(suitLocker.OutfitTags);
      })
    };
    private WorkChore<SuitLocker.ReturnSuitWorkable> urgentChore;
    private WorkChore<SuitLocker.ReturnSuitWorkable> idleChore;

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.resetProgressOnStop = true;
      this.workTime = 0.25f;
      this.synchronizeAnims = false;
    }

    public void CreateChore()
    {
      if (this.urgentChore != null)
        return;
      SuitLocker component = this.GetComponent<SuitLocker>();
      this.urgentChore = new WorkChore<SuitLocker.ReturnSuitWorkable>(Db.Get().ChoreTypes.ReturnSuitUrgent, (IStateMachineTarget) this, only_when_operational: false, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.personalNeeds, add_to_daily_report: false);
      this.urgentChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.DoesSuitNeedRechargingUrgent, (object) null);
      this.urgentChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.HasSuitMarker, (object) component);
      this.urgentChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.SuitTypeMatchesLocker, (object) component);
      this.idleChore = new WorkChore<SuitLocker.ReturnSuitWorkable>(Db.Get().ChoreTypes.ReturnSuitIdle, (IStateMachineTarget) this, only_when_operational: false, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.idle, add_to_daily_report: false);
      this.idleChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.DoesSuitNeedRechargingIdle, (object) null);
      this.idleChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.HasSuitMarker, (object) component);
      this.idleChore.AddPrecondition(SuitLocker.ReturnSuitWorkable.SuitTypeMatchesLocker, (object) component);
    }

    public void CancelChore()
    {
      if (this.urgentChore != null)
      {
        this.urgentChore.Cancel("ReturnSuitWorkable.CancelChore");
        this.urgentChore = (WorkChore<SuitLocker.ReturnSuitWorkable>) null;
      }
      if (this.idleChore == null)
        return;
      this.idleChore.Cancel("ReturnSuitWorkable.CancelChore");
      this.idleChore = (WorkChore<SuitLocker.ReturnSuitWorkable>) null;
    }

    protected override void OnStartWork(WorkerBase worker) => this.ShowProgressBar(false);

    protected override bool OnWorkTick(WorkerBase worker, float dt) => true;

    protected override void OnCompleteWork(WorkerBase worker)
    {
      Equipment equipment = worker.GetComponent<MinionIdentity>().GetEquipment();
      if (equipment.IsSlotOccupied(Db.Get().AssignableSlots.Suit))
      {
        if (this.GetComponent<SuitLocker>().CanDropOffSuit())
          this.GetComponent<SuitLocker>().UnequipFrom(equipment);
        else
          equipment.GetAssignable(Db.Get().AssignableSlots.Suit).Unassign();
      }
      if (this.urgentChore == null)
        return;
      this.CancelChore();
      this.CreateChore();
    }

    public override HashedString[] GetWorkAnims(WorkerBase worker)
    {
      return new HashedString[1]{ new HashedString("none") };
    }
  }

  public class StatesInstance(SuitLocker suit_locker) : 
    GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.GameInstance(suit_locker)
  {
  }

  public class States : GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker>
  {
    public SuitLocker.States.EmptyStates empty;
    public SuitLocker.States.ChargingStates charging;
    public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State waitingforsuit;
    public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State suitfullycharged;
    public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter isWaitingForSuit;
    public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter isConfigured;
    public StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.BoolParameter hasSuitMarker;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.empty;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.root.Update("RefreshMeter", (Action<SuitLocker.StatesInstance, float>) ((smi, dt) => smi.master.RefreshMeter()), UpdateRate.RENDER_200ms);
      this.empty.DefaultState(this.empty.notconfigured).EventTransition(GameHashes.OnStorageChange, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.charging, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() != (UnityEngine.Object) null)).ParamTransition<bool>((StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Parameter<bool>) this.isWaitingForSuit, this.waitingforsuit, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsTrue).Enter("CreateReturnSuitChore", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.returnSuitWorkable.CreateChore())).RefreshUserMenuOnEnter().Exit("CancelReturnSuitChore", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.returnSuitWorkable.CancelChore())).PlayAnim("no_suit_pre").QueueAnim("no_suit");
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state1 = this.empty.notconfigured.ParamTransition<bool>((StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Parameter<bool>) this.isConfigured, this.empty.configured, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsTrue);
      string name1 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER_NEEDS_CONFIGURATION.NAME;
      string tooltip1 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER_NEEDS_CONFIGURATION.TOOLTIP;
      StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
      HashedString render_overlay1 = new HashedString();
      StatusItemCategory category1 = main1;
      state1.ToggleStatusItem(name1, tooltip1, "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, render_overlay: render_overlay1, category: category1);
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state2 = this.empty.configured.RefreshUserMenuOnEnter();
      string name2 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.READY.NAME;
      string tooltip2 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.READY.TOOLTIP;
      StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
      HashedString render_overlay2 = new HashedString();
      StatusItemCategory category2 = main2;
      state2.ToggleStatusItem(name2, tooltip2, render_overlay: render_overlay2, category: category2);
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state3 = this.waitingforsuit.EventTransition(GameHashes.OnStorageChange, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.charging, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() != (UnityEngine.Object) null)).Enter("CreateFetchChore", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.CreateFetchChore())).ParamTransition<bool>((StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Parameter<bool>) this.isWaitingForSuit, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.empty, GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.IsFalse).RefreshUserMenuOnEnter().PlayAnim("no_suit_pst").QueueAnim("awaiting_suit").Exit("ClearIsWaitingForSuit", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => this.isWaitingForSuit.Set(false, smi))).Exit("CancelFetchChore", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.CancelFetchChore()));
      string name3 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.SUIT_REQUESTED.NAME;
      string tooltip3 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.SUIT_REQUESTED.TOOLTIP;
      StatusItemCategory main3 = Db.Get().StatusItemCategories.Main;
      HashedString render_overlay3 = new HashedString();
      StatusItemCategory category3 = main3;
      state3.ToggleStatusItem(name3, tooltip3, render_overlay: render_overlay3, category: category3);
      this.charging.DefaultState(this.charging.pre).RefreshUserMenuOnEnter().EventTransition(GameHashes.OnStorageChange, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.empty, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() == (UnityEngine.Object) null)).ToggleStatusItem(Db.Get().MiscStatusItems.StoredItemDurability, (Func<SuitLocker.StatesInstance, object>) (smi => (object) smi.master.GetStoredOutfit().gameObject)).Enter((StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi =>
      {
        KAnim.Build.Symbol symbol = smi.master.GetStoredOutfit().GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol((KAnimHashedString) "suit");
        SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
        component.TryRemoveSymbolOverride((HashedString) "suit_swap");
        if (symbol == null)
          return;
        component.AddSymbolOverride((HashedString) "suit_swap", symbol);
      }));
      this.charging.pre.Enter((StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi =>
      {
        if (smi.master.IsSuitFullyCharged())
        {
          smi.GoTo((StateMachine.BaseState) this.suitfullycharged);
        }
        else
        {
          smi.GetComponent<KBatchedAnimController>().Play((HashedString) "no_suit_pst");
          smi.GetComponent<KBatchedAnimController>().Queue((HashedString) "charging_pre");
        }
      })).OnAnimQueueComplete(this.charging.operational);
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state4 = this.charging.operational.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.nooxygen, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => !smi.master.HasOxygen())).PlayAnim("charging_loop", KAnim.PlayMode.Loop).Enter("SetActive", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.GetComponent<Operational>().SetActive(true))).Transition(this.charging.pst, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.IsSuitFullyCharged())).Update("ChargeSuit", (Action<SuitLocker.StatesInstance, float>) ((smi, dt) => smi.master.ChargeSuit(dt))).Exit("ClearActive", (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State.Callback) (smi => smi.master.GetComponent<Operational>().SetActive(false)));
      string name4 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.CHARGING.NAME;
      string tooltip4 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.CHARGING.TOOLTIP;
      StatusItemCategory main4 = Db.Get().StatusItemCategories.Main;
      HashedString render_overlay4 = new HashedString();
      StatusItemCategory category4 = main4;
      state4.ToggleStatusItem(name4, tooltip4, render_overlay: render_overlay4, category: category4);
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state5 = this.charging.nooxygen.TagTransition(GameTags.Operational, this.charging.notoperational, true).Transition(this.charging.operational, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.HasOxygen())).Transition(this.charging.pst, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.IsSuitFullyCharged())).PlayAnim("no_o2_loop", KAnim.PlayMode.Loop);
      string name5 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.NO_OXYGEN.NAME;
      string tooltip5 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.NO_OXYGEN.TOOLTIP;
      StatusItemCategory main5 = Db.Get().StatusItemCategories.Main;
      HashedString render_overlay5 = new HashedString();
      StatusItemCategory category5 = main5;
      state5.ToggleStatusItem(name5, tooltip5, "status_item_suit_locker_no_oxygen", StatusItem.IconType.Custom, NotificationType.BadMinor, render_overlay: render_overlay5, category: category5);
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state6 = this.charging.notoperational.TagTransition(GameTags.Operational, this.charging.operational).PlayAnim("not_charging_loop", KAnim.PlayMode.Loop).Transition(this.charging.pst, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => smi.master.IsSuitFullyCharged()));
      string name6 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.NOT_OPERATIONAL.NAME;
      string tooltip6 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.NOT_OPERATIONAL.TOOLTIP;
      StatusItemCategory main6 = Db.Get().StatusItemCategories.Main;
      HashedString render_overlay6 = new HashedString();
      StatusItemCategory category6 = main6;
      state6.ToggleStatusItem(name6, tooltip6, render_overlay: render_overlay6, category: category6);
      this.charging.pst.PlayAnim("charging_pst").OnAnimQueueComplete(this.suitfullycharged);
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State state7 = this.suitfullycharged.EventTransition(GameHashes.OnStorageChange, (GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State) this.empty, (StateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.master.GetStoredOutfit() == (UnityEngine.Object) null)).PlayAnim("has_suit").RefreshUserMenuOnEnter().ToggleStatusItem(Db.Get().MiscStatusItems.StoredItemDurability, (Func<SuitLocker.StatesInstance, object>) (smi => (object) smi.master.GetStoredOutfit().gameObject));
      string name7 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.FULLY_CHARGED.NAME;
      string tooltip7 = (string) BUILDING.STATUSITEMS.SUIT_LOCKER.FULLY_CHARGED.TOOLTIP;
      StatusItemCategory main7 = Db.Get().StatusItemCategories.Main;
      HashedString render_overlay7 = new HashedString();
      StatusItemCategory category7 = main7;
      state7.ToggleStatusItem(name7, tooltip7, render_overlay: render_overlay7, category: category7);
    }

    public class ChargingStates : 
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State
    {
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State pre;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State pst;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State operational;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State nooxygen;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State notoperational;
    }

    public class EmptyStates : 
      GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State
    {
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State configured;
      public GameStateMachine<SuitLocker.States, SuitLocker.StatesInstance, SuitLocker, object>.State notconfigured;
    }
  }

  private enum SuitMarkerState
  {
    HasMarker,
    NoMarker,
    WrongSide,
    NotOperational,
  }

  private struct SuitLockerEntry
  {
    public SuitLocker suitLocker;
    public int cell;
    public static SuitLocker.SuitLockerEntry.Comparer comparer = new SuitLocker.SuitLockerEntry.Comparer();

    public class Comparer : IComparer<SuitLocker.SuitLockerEntry>
    {
      public int Compare(SuitLocker.SuitLockerEntry a, SuitLocker.SuitLockerEntry b)
      {
        return a.cell - b.cell;
      }
    }
  }

  private struct SuitMarkerEntry
  {
    public SuitMarker suitMarker;
    public int cell;
  }
}
