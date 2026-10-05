// Decompiled with JetBrains decompiler
// Type: Pickupable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Pickupable")]
public class Pickupable : Workable, IHasSortOrder
{
  [MyCmpReq]
  private PrimaryElement primaryElement;
  public const float WorkTime = 1.5f;
  [SerializeField]
  private int _sortOrder;
  [MyCmpReq]
  [NonSerialized]
  public KPrefabID KPrefabID;
  [MyCmpAdd]
  [NonSerialized]
  public Clearable Clearable;
  [MyCmpAdd]
  [NonSerialized]
  public Prioritizable prioritizable;
  [SerializeField]
  public List<ChoreType> allowedChoreTypes;
  public bool absorbable;
  public Func<Pickupable, bool> CanAbsorb = (Func<Pickupable, bool>) (other => false);
  public Func<Pickupable, float, Pickupable> OnTake;
  public Action<Pickupable, bool, Pickupable.Reservation> OnReservationsChanged;
  public ObjectLayerListItem objectLayerListItem;
  public Workable targetWorkable;
  public KAnimFile carryAnimOverride;
  private KBatchedAnimController lastCarrier;
  public bool useGunforPickup = true;
  public static CellOffset[] displacementOffsets = new CellOffset[8]
  {
    new CellOffset(0, 1),
    new CellOffset(0, -1),
    new CellOffset(1, 0),
    new CellOffset(-1, 0),
    new CellOffset(1, 1),
    new CellOffset(1, -1),
    new CellOffset(-1, 1),
    new CellOffset(-1, -1)
  };
  private bool isReachable;
  private bool isEntombed;
  private bool cleaningUp;
  public bool trackOnPickup = true;
  private int nextTicketNumber;
  private ulong cellChangedHandlerID;
  [Serialize]
  public bool deleteOffGrid = true;
  private List<Pickupable.Reservation> reservations = new List<Pickupable.Reservation>();
  private HandleVector<int>.Handle solidPartitionerEntry;
  private HandleVector<int>.Handle worldPartitionerEntry;
  private HandleVector<int>.Handle storedPartitionerEntry;
  private FetchableMonitor.Instance fetchable_monitor;
  public bool handleFallerComponents = true;
  private LoggerFSSF log;
  private static readonly EventSystem.IntraObjectHandler<Pickupable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Pickupable>((Action<Pickupable, object>) ((component, data) => component.OnStore(data)));
  private static readonly EventSystem.IntraObjectHandler<Pickupable> OnLandedDelegate = new EventSystem.IntraObjectHandler<Pickupable>((Action<Pickupable, object>) ((component, data) => component.OnLanded(data)));
  private static readonly EventSystem.IntraObjectHandler<Pickupable> OnOreSizeChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>((Action<Pickupable, object>) ((component, data) => component.OnOreSizeChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<Pickupable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>((Action<Pickupable, object>) ((component, data) => component.OnReachableChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<Pickupable> RefreshStorageTagsDelegate = new EventSystem.IntraObjectHandler<Pickupable>((Action<Pickupable, object>) ((component, data) => component.RefreshStorageTags(data)));
  private static readonly EventSystem.IntraObjectHandler<Pickupable> OnWorkableEntombOffset = new EventSystem.IntraObjectHandler<Pickupable>((Action<Pickupable, object>) ((component, data) => component.SetWorkableOffset(data)));
  private static readonly EventSystem.IntraObjectHandler<Pickupable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>((Action<Pickupable, object>) ((component, data) => component.OnTagsChanged(data)));
  private Action<object> OnSolidChangedClosure;
  private static Action<object> OnCellChangeDispatcher = (Action<object>) (obj => Unsafe.As<Pickupable>(obj).OnCellChange());
  private int entombedCell = -1;

  public PrimaryElement PrimaryElement => this.primaryElement;

  public int sortOrder
  {
    get => this._sortOrder;
    set => this._sortOrder = value;
  }

  public Storage storage { get; set; }

  public bool MinTakeAmount { get; set; }

  public bool isChoreAllowedToPickup(ChoreType choreType)
  {
    return this.allowedChoreTypes == null || this.allowedChoreTypes.Contains(choreType);
  }

  public bool prevent_absorb_until_stored { get; set; }

  public bool isKinematic { get; set; }

  public bool wasAbsorbed { get; private set; }

  public int cachedCell { get; private set; }

  public bool IsEntombed
  {
    get => this.isEntombed;
    set
    {
      if (value == this.isEntombed)
        return;
      this.isEntombed = value;
      if (this.isEntombed)
        this.KPrefabID.AddTag(GameTags.Entombed);
      else
        this.KPrefabID.RemoveTag(GameTags.Entombed);
      this.Trigger(-1089732772, (object) BoxedBools.Box(this.isEntombed));
      this.UpdateEntombedVisualizer();
    }
  }

  [Obsolete("Use Instance ID")]
  private bool CouldBePickedUpCommon(GameObject carrier)
  {
    return this.CouldBePickedUpCommon(carrier.GetComponent<KPrefabID>().InstanceID);
  }

  private bool CouldBePickedUpCommon(int carrierID)
  {
    return (double) this.UnreservedFetchAmount > 0.0 || (double) this.FindReservedAmount(carrierID) > 0.0;
  }

  [Obsolete("Use Instance ID")]
  public bool CouldBePickedUpByMinion(GameObject carrier)
  {
    return this.CouldBePickedUpByMinion(carrier.GetComponent<KPrefabID>().InstanceID);
  }

  public bool CouldBePickedUpByMinion(int carrierID)
  {
    if (!this.CouldBePickedUpCommon(carrierID))
      return false;
    return (UnityEngine.Object) this.storage == (UnityEngine.Object) null || !(bool) (UnityEngine.Object) this.storage.automatable || !this.storage.automatable.GetAutomationOnly();
  }

  [Obsolete("Use Instance ID")]
  public bool CouldBePickedUpByTransferArm(GameObject carrier)
  {
    return this.CouldBePickedUpByTransferArm(carrier.GetComponent<KPrefabID>().InstanceID);
  }

  public bool CouldBePickedUpByTransferArm(int carrierID)
  {
    if (!this.CouldBePickedUpCommon(carrierID))
      return false;
    return this.fetchable_monitor == null || this.fetchable_monitor.IsFetchable();
  }

  [Obsolete("Use Instance ID")]
  public float FindReservedAmount(GameObject reserver)
  {
    return this.FindReservedAmount(reserver.GetComponent<KPrefabID>().InstanceID);
  }

  public float FindReservedAmount(int reserverID)
  {
    for (int index = 0; index < this.reservations.Count; ++index)
    {
      if (this.reservations[index].reserverID == reserverID)
        return this.reservations[index].amount;
    }
    return 0.0f;
  }

  public float UnreservedAmount => this.TotalAmount - this.ReservedAmount;

  public float ReservedAmount { get; private set; }

  public float FetchTotalAmount => this.primaryElement.MassPerUnit * this.primaryElement.Units;

  public float UnreservedFetchAmount => this.FetchTotalAmount - this.ReservedAmount;

  public float TotalAmount
  {
    get => this.primaryElement.Units;
    set
    {
      DebugUtil.Assert((UnityEngine.Object) this.primaryElement != (UnityEngine.Object) null);
      this.primaryElement.Units = value;
      if ((double) value < (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT && !this.primaryElement.KeepZeroMassObject)
        this.gameObject.DeleteObject();
      this.NotifyChanged(Grid.PosToCell((KMonoBehaviour) this));
    }
  }

  private void RefreshReservedAmount()
  {
    this.ReservedAmount = 0.0f;
    for (int index = 0; index < this.reservations.Count; ++index)
      this.ReservedAmount += this.reservations[index].amount;
  }

  [Conditional("UNITY_EDITOR")]
  private void Log(string evt, string param, float value)
  {
  }

  public void ClearReservations()
  {
    this.reservations.Clear();
    this.RefreshReservedAmount();
  }

  [ContextMenu("Print Reservations")]
  public void PrintReservations()
  {
    foreach (Pickupable.Reservation reservation in this.reservations)
      Debug.Log((object) reservation.ToString());
  }

  public int Reserve(string context, int reserverID, float amount)
  {
    int ticket = this.nextTicketNumber++;
    Pickupable.Reservation reservation = new Pickupable.Reservation(reserverID, amount, ticket);
    this.reservations.Add(reservation);
    this.RefreshReservedAmount();
    if (this.OnReservationsChanged != null)
      this.OnReservationsChanged(this, true, reservation);
    return ticket;
  }

  public void Unreserve(string context, int ticket)
  {
    for (int index = 0; index < this.reservations.Count; ++index)
    {
      if (this.reservations[index].ticket == ticket)
      {
        Pickupable.Reservation reservation = this.reservations[index];
        this.reservations.RemoveAt(index);
        this.RefreshReservedAmount();
        if (this.OnReservationsChanged == null)
          break;
        this.OnReservationsChanged(this, false, reservation);
        break;
      }
    }
  }

  private Pickupable()
  {
    this.showProgressBar = false;
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.shouldTransferDiseaseWithWorker = false;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.OnSolidChangedClosure = new Action<object>(this.OnSolidChanged);
    this.workingPstComplete = (HashedString[]) null;
    this.workingPstFailed = (HashedString[]) null;
    this.log = new LoggerFSSF(nameof (Pickupable));
    this.workerStatusItem = Db.Get().DuplicantStatusItems.PickingUp;
    this.SetWorkTime(1.5f);
    this.targetWorkable = (Workable) this;
    this.resetProgressOnStop = true;
    this.gameObject.layer = Game.PickupableLayer;
    this.UpdateCachedCell(Grid.PosToCell(this.transform.GetPosition()));
    this.Subscribe<Pickupable>(856640610, Pickupable.OnStoreDelegate);
    this.Subscribe<Pickupable>(1188683690, Pickupable.OnLandedDelegate);
    this.Subscribe<Pickupable>(1807976145, Pickupable.OnOreSizeChangedDelegate);
    this.Subscribe<Pickupable>(-1432940121, Pickupable.OnReachableChangedDelegate);
    this.Subscribe<Pickupable>(-778359855, Pickupable.RefreshStorageTagsDelegate);
    this.Subscribe<Pickupable>(580035959, Pickupable.OnWorkableEntombOffset);
    this.KPrefabID.AddTag(GameTags.Pickupable);
    Components.Pickupables.Add(this);
  }

  protected override void OnLoadLevel() => base.OnLoadLevel();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if (!Grid.IsValidCell(cell) && this.deleteOffGrid)
    {
      this.gameObject.DeleteObject();
    }
    else
    {
      if ((UnityEngine.Object) this.GetComponent<Health>() != (UnityEngine.Object) null)
        this.handleFallerComponents = false;
      this.UpdateCachedCell(cell);
      new ReachabilityMonitor.Instance((Workable) this).StartSM();
      this.fetchable_monitor = new FetchableMonitor.Instance(this);
      this.fetchable_monitor.StartSM();
      this.SetWorkTime(1.5f);
      this.faceTargetWhenWorking = true;
      KSelectable component1 = this.GetComponent<KSelectable>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.SetStatusIndicatorOffset(new Vector3(0.0f, -0.65f, 0.0f));
      this.OnTagsChanged((object) null);
      this.TryToOffsetIfBuried(CellOffset.none);
      DecorProvider component2 = this.GetComponent<DecorProvider>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && string.IsNullOrEmpty(component2.overrideName))
        component2.overrideName = (string) UI.OVERLAYS.DECOR.CLUTTER;
      this.UpdateEntombedVisualizer();
      this.Subscribe<Pickupable>(-1582839653, Pickupable.OnTagsChangedDelegate);
      this.NotifyChanged(cell);
    }
  }

  [OnDeserialized]
  public void OnDeserialize()
  {
    if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 28) || (double) this.transform.position.z != 0.0)
      return;
    KBatchedAnimController component = this.transform.GetComponent<KBatchedAnimController>();
    component.SetSceneLayer(component.sceneLayer);
  }

  public void UpdateListeners(bool worldSpace)
  {
    if (this.cleaningUp)
      return;
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if (worldSpace)
    {
      if (this.solidPartitionerEntry.IsValid())
        return;
      GameScenePartitioner.Instance.Free(ref this.storedPartitionerEntry);
      this.objectLayerListItem = new ObjectLayerListItem(this.gameObject, this, ObjectLayer.Pickupables, cell);
      this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterSolidListener", (object) this.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, this.OnSolidChangedClosure);
      this.worldPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterPickupable", (object) this, cell, GameScenePartitioner.Instance.pickupablesLayer, (Action<object>) null);
      this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, Pickupable.OnCellChangeDispatcher, (object) this, "Pickupable.OnCellChange");
      Singleton<CellChangeMonitor>.Instance.MarkDirty(this.transform);
      Singleton<CellChangeMonitor>.Instance.ClearLastKnownCell(this.transform);
    }
    else
    {
      if (this.storedPartitionerEntry.IsValid())
        return;
      this.storedPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterStoredPickupable", (object) this, cell, GameScenePartitioner.Instance.storedPickupablesLayer, (Action<object>) null);
      if (this.objectLayerListItem != null)
      {
        this.objectLayerListItem.Clear();
        this.objectLayerListItem = (ObjectLayerListItem) null;
      }
      GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
      GameScenePartitioner.Instance.Free(ref this.worldPartitionerEntry);
      Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
    }
  }

  public void RegisterListeners() => this.UpdateListeners(true);

  public void UnregisterListeners()
  {
    if (this.objectLayerListItem != null)
    {
      this.objectLayerListItem.Clear();
      this.objectLayerListItem = (ObjectLayerListItem) null;
    }
    GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.worldPartitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.storedPartitionerEntry);
    this.Unsubscribe<Pickupable>(856640610, Pickupable.OnStoreDelegate);
    this.Unsubscribe<Pickupable>(1188683690, Pickupable.OnLandedDelegate);
    this.Unsubscribe<Pickupable>(1807976145, Pickupable.OnOreSizeChangedDelegate);
    this.Unsubscribe<Pickupable>(-1432940121, Pickupable.OnReachableChangedDelegate);
    this.Unsubscribe<Pickupable>(-778359855, Pickupable.RefreshStorageTagsDelegate);
    this.Unsubscribe<Pickupable>(580035959, Pickupable.OnWorkableEntombOffset);
    if (this.isSpawned)
      this.Unsubscribe<Pickupable>(-1582839653, Pickupable.OnTagsChangedDelegate);
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
  }

  private void OnSolidChanged(object data) => this.TryToOffsetIfBuried(CellOffset.none);

  private void SetWorkableOffset(object data)
  {
    CellOffset offset = CellOffset.none;
    WorkerBase cmp = data as WorkerBase;
    if ((UnityEngine.Object) cmp != (UnityEngine.Object) null)
    {
      int cell1 = Grid.PosToCell((KMonoBehaviour) cmp);
      int cell2 = Grid.PosToCell((KMonoBehaviour) this);
      offset = Grid.IsValidCell(cell1) ? Grid.GetCellOffsetDirection(cell2, cell1) : CellOffset.none;
    }
    this.TryToOffsetIfBuried(offset);
  }

  private CellOffset[] GetPreferedOffsets(CellOffset preferedDirectionOffset)
  {
    if (preferedDirectionOffset == CellOffset.left || preferedDirectionOffset == CellOffset.leftup)
      return new CellOffset[3]
      {
        CellOffset.up,
        CellOffset.left,
        CellOffset.leftup
      };
    if (preferedDirectionOffset == CellOffset.right || preferedDirectionOffset == CellOffset.rightup)
      return new CellOffset[3]
      {
        CellOffset.up,
        CellOffset.right,
        CellOffset.rightup
      };
    if (preferedDirectionOffset == CellOffset.up)
      return new CellOffset[3]
      {
        CellOffset.up,
        CellOffset.rightup,
        CellOffset.leftup
      };
    if (preferedDirectionOffset == CellOffset.leftdown)
      return new CellOffset[3]
      {
        CellOffset.down,
        CellOffset.leftdown,
        CellOffset.left
      };
    if (preferedDirectionOffset == CellOffset.rightdown)
      return new CellOffset[3]
      {
        CellOffset.down,
        CellOffset.rightdown,
        CellOffset.right
      };
    if (!(preferedDirectionOffset == CellOffset.down))
      return new CellOffset[0];
    return new CellOffset[3]
    {
      CellOffset.down,
      CellOffset.leftdown,
      CellOffset.rightdown
    };
  }

  public void TryToOffsetIfBuried(CellOffset offset)
  {
    if (this.KPrefabID.HasTag(GameTags.Stored) || this.KPrefabID.HasTag(GameTags.Equipped))
      return;
    int num1 = Grid.PosToCell((KMonoBehaviour) this);
    if (!Grid.IsValidCell(num1))
      return;
    DeathMonitor.Instance smi = this.gameObject.GetSMI<DeathMonitor.Instance>();
    if ((smi == null ? 1 : (smi.IsDead() ? 1 : 0)) != 0 && (Grid.Solid[num1] && Grid.Foundation[num1] || Grid.Properties[num1] != (byte) 0))
    {
      foreach (CellOffset offset1 in this.GetPreferedOffsets(offset).Concat<CellOffset>(Pickupable.displacementOffsets))
      {
        int num2 = Grid.OffsetCell(num1, offset1);
        if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
        {
          Vector3 posCbc = Grid.CellToPosCBC(num2, Grid.SceneLayer.Move);
          KCollider2D component = this.GetComponent<KCollider2D>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            posCbc.y += this.transform.GetPosition().y - component.bounds.min.y;
          this.transform.SetPosition(posCbc);
          num1 = num2;
          this.RemoveFaller();
          this.AddFaller(Vector2.zero);
          break;
        }
      }
    }
    this.HandleSolidCell(num1);
  }

  private bool HandleSolidCell(int cell)
  {
    bool isEntombed = this.IsEntombed;
    bool flag = false;
    if (Grid.IsValidCell(cell) && Grid.Solid[cell])
    {
      DeathMonitor.Instance smi = this.gameObject.GetSMI<DeathMonitor.Instance>();
      if ((smi == null ? 1 : (smi.IsDead() ? 1 : 0)) != 0)
      {
        this.Clearable.CancelClearing();
        flag = true;
      }
    }
    if (flag != isEntombed && !this.KPrefabID.HasTag(GameTags.Stored))
    {
      this.IsEntombed = flag;
      this.GetComponent<KSelectable>().IsSelectable = !this.IsEntombed;
    }
    this.UpdateEntombedVisualizer();
    return this.IsEntombed;
  }

  private void OnCellChange()
  {
    Vector3 position = this.transform.GetPosition();
    int cell = Grid.PosToCell(position);
    if (!Grid.IsValidCell(cell))
    {
      Vector2 vector2_1 = new Vector2(-0.1f * (float) Grid.WidthInCells, 1.1f * (float) Grid.WidthInCells);
      Vector2 vector2_2 = new Vector2(-0.1f * (float) Grid.HeightInCells, 1.1f * (float) Grid.HeightInCells);
      if (!this.deleteOffGrid || (double) position.x >= (double) vector2_1.x && (double) vector2_1.y >= (double) position.x && (double) position.y >= (double) vector2_2.x && (double) vector2_2.y >= (double) position.y)
        return;
      this.DeleteObject();
    }
    else
    {
      this.ReleaseEntombedVisualizerAndAddFaller(true);
      if (this.HandleSolidCell(cell))
        return;
      this.objectLayerListItem.Update(cell);
      bool flag = false;
      if (this.absorbable && !this.KPrefabID.HasTag(GameTags.Stored))
      {
        int num = Grid.CellBelow(cell);
        if (Grid.IsValidCell(num) && Grid.Solid[num])
        {
          ObjectLayerListItem nextItem = this.objectLayerListItem.nextItem;
          while (nextItem != null)
          {
            Pickupable pickupable = nextItem.pickupable;
            nextItem = nextItem.nextItem;
            if ((UnityEngine.Object) pickupable != (UnityEngine.Object) null)
            {
              flag = pickupable.TryAbsorb(this, false);
              if (flag)
                break;
            }
          }
        }
      }
      GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, cell);
      GameScenePartitioner.Instance.UpdatePosition(this.worldPartitionerEntry, cell);
      int cachedCell = this.cachedCell;
      this.UpdateCachedCell(cell);
      if (!flag)
        this.NotifyChanged(cell);
      if (!Grid.IsValidCell(cachedCell) || cell == cachedCell)
        return;
      this.NotifyChanged(cachedCell);
    }
  }

  private void OnTagsChanged(object _)
  {
    if (!this.KPrefabID.HasTag(GameTags.Stored) && !this.KPrefabID.HasTag(GameTags.Equipped))
    {
      this.UpdateListeners(true);
      this.AddFaller(Vector2.zero);
    }
    else
    {
      this.UpdateListeners(false);
      this.RemoveFaller();
    }
  }

  private void NotifyChanged(int new_cell)
  {
    GameScenePartitioner.Instance.TriggerEvent(new_cell, GameScenePartitioner.Instance.pickupablesChangedLayer, (object) this);
  }

  public bool TryAbsorb(Pickupable other, bool hide_effects, bool allow_cross_storage = false)
  {
    if ((UnityEngine.Object) other == (UnityEngine.Object) null || other.wasAbsorbed || this.wasAbsorbed || !other.CanAbsorb(this) || this.prevent_absorb_until_stored || !allow_cross_storage && (UnityEngine.Object) this.storage == (UnityEngine.Object) null != ((UnityEngine.Object) other.storage == (UnityEngine.Object) null))
      return false;
    this.Absorb(other);
    if (!hide_effects && (UnityEngine.Object) EffectPrefabs.Instance != (UnityEngine.Object) null && !(bool) (UnityEngine.Object) this.storage)
    {
      Vector3 position = this.transform.GetPosition() with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.Front)
      };
      Util.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.OreAbsorbId), position, Quaternion.identity).SetActive(true);
    }
    return true;
  }

  protected override void OnCleanUp()
  {
    this.cleaningUp = true;
    this.ReleaseEntombedVisualizerAndAddFaller(false);
    this.RemoveFaller();
    if ((bool) (UnityEngine.Object) this.storage)
      this.storage.Remove(this.gameObject);
    this.UnregisterListeners();
    this.fetchable_monitor = (FetchableMonitor.Instance) null;
    Components.Pickupables.Remove(this);
    if (this.reservations.Count > 0)
    {
      Pickupable.Reservation[] array = this.reservations.ToArray();
      this.reservations.Clear();
      if (this.OnReservationsChanged != null)
      {
        foreach (Pickupable.Reservation reservation in array)
          this.OnReservationsChanged(this, false, reservation);
      }
    }
    if (Grid.IsValidCell(this.cachedCell))
      this.NotifyChanged(this.cachedCell);
    base.OnCleanUp();
  }

  public Pickupable TakeUnit(float units) => this.Take(units * this.primaryElement.MassPerUnit);

  public Pickupable Take(float amount)
  {
    if ((double) amount <= 0.0)
      return (Pickupable) null;
    if (this.OnTake != null)
    {
      float val1 = this.TotalAmount * this.primaryElement.MassPerUnit;
      if ((double) amount >= (double) val1 && (UnityEngine.Object) this.storage != (UnityEngine.Object) null && !this.primaryElement.KeepZeroMassObject)
        this.storage.Remove(this.gameObject);
      float num = Math.Min(val1, amount) / this.primaryElement.MassPerUnit;
      return (double) num <= 0.0 ? (Pickupable) null : this.OnTake(this, num);
    }
    if ((UnityEngine.Object) this.storage != (UnityEngine.Object) null)
      this.storage.Remove(this.gameObject);
    return this;
  }

  private void Absorb(Pickupable pickupable)
  {
    Debug.Assert(!this.wasAbsorbed);
    Debug.Assert(!pickupable.wasAbsorbed);
    this.Trigger(-2064133523, (object) pickupable);
    pickupable.Trigger(-1940207677, (object) this.gameObject);
    pickupable.wasAbsorbed = true;
    KSelectable component = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) pickupable.GetComponent<KSelectable>())
      SelectTool.Instance.Select(component);
    pickupable.gameObject.DeleteObject();
    this.NotifyChanged(Grid.PosToCell((KMonoBehaviour) this));
  }

  private void RefreshStorageTags(object data = null)
  {
    bool flag = data is Storage || data != null && ((Boxed<bool>) data).value;
    if (flag && data is Storage && (UnityEngine.Object) ((Component) data).gameObject == (UnityEngine.Object) this.gameObject)
      return;
    if (flag)
    {
      this.KPrefabID.AddTag(GameTags.Stored);
      if ((this.storage == null ? 1 : (!this.storage.allowItemRemoval ? 1 : 0)) != 0)
        this.KPrefabID.AddTag(GameTags.StoredPrivate);
      else
        this.KPrefabID.RemoveTag(GameTags.StoredPrivate);
    }
    else
    {
      this.KPrefabID.RemoveTag(GameTags.Stored);
      this.KPrefabID.RemoveTag(GameTags.StoredPrivate);
    }
  }

  public void OnStore(object data)
  {
    this.storage = data as Storage;
    bool flag = data is Storage || data != null && ((Boxed<bool>) data).value;
    SaveLoadRoot component1 = this.GetComponent<SaveLoadRoot>();
    if ((UnityEngine.Object) this.carryAnimOverride != (UnityEngine.Object) null && (UnityEngine.Object) this.lastCarrier != (UnityEngine.Object) null)
    {
      this.lastCarrier.RemoveAnimOverrides(this.carryAnimOverride);
      this.lastCarrier = (KBatchedAnimController) null;
    }
    KSelectable component2 = this.GetComponent<KSelectable>();
    if ((bool) (UnityEngine.Object) component2)
      component2.IsSelectable = !flag;
    if (flag)
    {
      int cachedCell = this.cachedCell;
      this.RefreshStorageTags(data);
      this.RemoveFaller();
      if (this.storage != null)
      {
        if ((UnityEngine.Object) this.carryAnimOverride != (UnityEngine.Object) null && (UnityEngine.Object) this.storage.GetComponent<Navigator>() != (UnityEngine.Object) null)
        {
          this.lastCarrier = this.storage.GetComponent<KBatchedAnimController>();
          if ((UnityEngine.Object) this.lastCarrier != (UnityEngine.Object) null && this.lastCarrier.HasTag(GameTags.BaseMinion))
            this.lastCarrier.AddAnimOverrides(this.carryAnimOverride);
        }
        this.UpdateCachedCell(Grid.PosToCell((KMonoBehaviour) this.storage));
      }
      this.NotifyChanged(cachedCell);
      if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null))
        return;
      component1.SetRegistered(false);
    }
    else
    {
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.SetRegistered(true);
      this.RemovedFromStorage();
    }
  }

  private void RemovedFromStorage()
  {
    this.storage = (Storage) null;
    this.UpdateCachedCell(Grid.PosToCell((KMonoBehaviour) this));
    this.RefreshStorageTags();
    this.AddFaller(Vector2.zero);
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.enabled = true;
    this.gameObject.transform.rotation = Quaternion.identity;
    this.UpdateListeners(true);
    component.GetBatchInstanceData().ClearOverrideTransformMatrix();
  }

  public void UpdateCachedCellFromStoragePosition()
  {
    Debug.Assert((UnityEngine.Object) this.storage != (UnityEngine.Object) null, (object) "Only call UpdateCachedCellFromStoragePosition on pickupables in storage!");
    this.UpdateCachedCell(Grid.PosToCell((KMonoBehaviour) this.storage));
  }

  public void UpdateCachedCell(int cell)
  {
    if (this.cachedCell != cell && this.storedPartitionerEntry.IsValid())
      GameScenePartitioner.Instance.UpdatePosition(this.storedPartitionerEntry, cell);
    this.cachedCell = cell;
    this.GetOffsets(this.cachedCell);
    if (!this.KPrefabID.HasTag(GameTags.PickupableStorage))
      return;
    this.GetComponent<Storage>().UpdateStoredItemCachedCells();
  }

  public override int GetCell() => this.cachedCell;

  public override Workable.AnimInfo GetAnim(WorkerBase worker)
  {
    if (!this.useGunforPickup || !worker.UsesMultiTool())
      return base.GetAnim(worker);
    return base.GetAnim(worker) with
    {
      smi = (StateMachine.Instance) new MultitoolController.Instance((Workable) this, worker, (HashedString) "pickup", Assets.GetPrefab((Tag) EffectConfigs.OreAbsorbId))
    };
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    Storage component = worker.GetComponent<Storage>();
    Pickupable.PickupableStartWorkInfo startWorkInfo = (Pickupable.PickupableStartWorkInfo) worker.GetStartWorkInfo();
    float amount = startWorkInfo.amount;
    Pickupable pickupable = this;
    if ((UnityEngine.Object) pickupable != (UnityEngine.Object) null)
    {
      Pickupable data = pickupable.Take(amount);
      if ((UnityEngine.Object) data != (UnityEngine.Object) null)
      {
        component.Store(data.gameObject);
        worker.SetWorkCompleteData((object) data);
        startWorkInfo.setResultCb(data.gameObject);
      }
      else
        startWorkInfo.setResultCb((GameObject) null);
    }
    else
      startWorkInfo.setResultCb((GameObject) null);
  }

  public override bool InstantlyFinish(WorkerBase worker) => false;

  public override Vector3 GetTargetPoint() => this.transform.GetPosition();

  public bool IsReachable() => this.isReachable;

  private void OnReachableChanged(object data)
  {
    this.isReachable = ((Boxed<bool>) data).value;
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.isReachable)
      component.RemoveStatusItem(Db.Get().MiscStatusItems.PickupableUnreachable);
    else
      component.AddStatusItem(Db.Get().MiscStatusItems.PickupableUnreachable, (object) this);
  }

  private void AddFaller(Vector2 initial_velocity)
  {
    if (!this.handleFallerComponents || GameComps.Fallers.Has((object) this.gameObject))
      return;
    GameComps.Fallers.Add(this.gameObject, initial_velocity);
  }

  private void RemoveFaller()
  {
    if (!this.handleFallerComponents || !GameComps.Fallers.Has((object) this.gameObject))
      return;
    GameComps.Fallers.Remove(this.gameObject);
  }

  private void OnOreSizeChanged(object data)
  {
    Vector3 initial_velocity = Vector3.zero;
    HandleVector<int>.Handle handle = GameComps.Gravities.GetHandle(this.gameObject);
    if (handle.IsValid())
      initial_velocity = (Vector3) GameComps.Gravities.GetData(handle).velocity;
    this.RemoveFaller();
    if (this.KPrefabID.HasTag(GameTags.Stored))
      return;
    this.AddFaller((Vector2) initial_velocity);
  }

  private void OnLanded(object data)
  {
    if ((UnityEngine.Object) CameraController.Instance == (UnityEngine.Object) null)
      return;
    Vector3 position = this.transform.GetPosition();
    Vector2I xy = Grid.PosToXY(position);
    if (xy.x < 0 || Grid.WidthInCells <= xy.x || xy.y < 0 || Grid.HeightInCells <= xy.y)
    {
      this.DeleteObject();
    }
    else
    {
      Vector2 vector2 = ((Boxed<Vector2>) data).value;
      if ((double) vector2.sqrMagnitude <= 0.20000000298023224 || SpeedControlScreen.Instance.IsPaused)
        return;
      Element element = this.primaryElement.Element;
      if (element.substance == null)
        return;
      string str1 = element.substance.GetOreBumpSound() ?? (!element.HasTag(GameTags.RefinedMetal) ? (!element.HasTag(GameTags.Metal) ? "Rock" : "RawMetal") : "RefinedMetal");
      string str2 = GlobalAssets.GetSound(!(element.tag.ToString() == "Creature") || this.gameObject.HasTag(GameTags.Seed) ? "Ore_bump_" + str1 : "Bodyfall_rock", true) ?? GlobalAssets.GetSound("Ore_bump_rock");
      if (!CameraController.Instance.IsAudibleSound(this.transform.GetPosition(), (HashedString) str2))
        return;
      int cell = Grid.PosToCell(position);
      int num1 = Grid.Element[cell].IsLiquid ? 1 : 0;
      float num2 = 0.0f;
      if (num1 != 0)
        num2 = SoundUtil.GetLiquidDepth(cell);
      FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(str2, CameraController.Instance.GetVerticallyScaledPosition(this.transform.GetPosition()));
      int num3 = (int) instance.setParameterByName("velocity", vector2.magnitude);
      int num4 = (int) instance.setParameterByName("liquidDepth", num2);
      KFMOD.EndOneShot(instance);
    }
  }

  private void UpdateEntombedVisualizer()
  {
    if (this.IsEntombed)
    {
      if (this.entombedCell != -1)
        return;
      int cell = Grid.PosToCell((KMonoBehaviour) this);
      if (EntombedItemManager.CanEntomb(this))
        SaveGame.Instance.entombedItemManager.Add(this);
      if (!((UnityEngine.Object) Grid.Objects[cell, 1] == (UnityEngine.Object) null))
        return;
      KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !Game.Instance.GetComponent<EntombedItemVisualizer>().AddItem(cell))
        return;
      this.entombedCell = cell;
      component.enabled = false;
      this.RemoveFaller();
    }
    else
      this.ReleaseEntombedVisualizerAndAddFaller(true);
  }

  private void ReleaseEntombedVisualizerAndAddFaller(bool add_faller_if_necessary)
  {
    if (this.entombedCell == -1)
      return;
    Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.entombedCell);
    this.entombedCell = -1;
    this.GetComponent<KBatchedAnimController>().enabled = true;
    if (!add_faller_if_necessary)
      return;
    this.AddFaller(Vector2.zero);
  }

  public struct Reservation(int reserverID, float amount, int ticket)
  {
    public int reserverID = reserverID;
    public float amount = amount;
    public int ticket = ticket;

    public override string ToString()
    {
      return $"{this.reserverID.ToString()}, {this.amount.ToString()}, {this.ticket.ToString()}";
    }
  }

  public class PickupableStartWorkInfo : WorkerBase.StartWorkInfo
  {
    public float amount { get; private set; }

    public Pickupable originalPickupable { get; private set; }

    public Action<GameObject> setResultCb { get; private set; }

    public PickupableStartWorkInfo(
      Pickupable pickupable,
      float amount,
      Action<GameObject> set_result_cb)
      : base(pickupable.targetWorkable)
    {
      this.originalPickupable = pickupable;
      this.amount = amount;
      this.setResultCb = set_result_cb;
    }
  }
}
