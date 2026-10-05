// Decompiled with JetBrains decompiler
// Type: Storage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Storage")]
public class Storage : Workable, ISaveLoadableDetails, IStorage
{
  public bool allowItemRemoval;
  public bool ignoreSourcePriority;
  public bool onlyTransferFromLowerPriority;
  public float capacityKg = 20000f;
  public bool showDescriptor;
  public bool doDiseaseTransfer = true;
  public List<Tag> storageFilters;
  public bool useGunForDelivery = true;
  public bool sendOnStoreOnSpawn;
  public bool showInUI = true;
  public bool storeDropsFromButcherables;
  public bool allowClearable;
  public bool showCapacityStatusItem;
  public bool showCapacityAsMainStatus;
  public bool showUnreachableStatus;
  public bool showSideScreenTitleBar;
  public bool useWideOffsets;
  public Action<List<GameObject>> onDestroyItemsDropped;
  public Action<GameObject> OnStorageChange;
  public Vector2 dropOffset = Vector2.zero;
  [MyCmpGet]
  private Rotatable rotatable;
  public Vector2 gunTargetOffset;
  public Storage.FetchCategory fetchCategory;
  public int storageNetworkID = -1;
  public Tag storageID = GameTags.StoragesIds.DefaultStorage;
  public float storageFullMargin;
  public Vector3 storageFXOffset = Vector3.zero;
  private static readonly EventSystem.IntraObjectHandler<Storage> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Storage>((Action<Storage, object>) ((component, data) => component.OnReachableChanged(data)));
  public Storage.FXPrefix fxPrefix;
  public List<GameObject> items = new List<GameObject>();
  [MyCmpGet]
  public Prioritizable prioritizable;
  [MyCmpGet]
  public Automatable automatable;
  [MyCmpGet]
  protected PrimaryElement primaryElement;
  public bool dropOnLoad;
  protected float maxKGPerItem = float.MaxValue;
  private bool endOfLife;
  public bool allowSettingOnlyFetchMarkedItems = true;
  [KSerialization.Serialize]
  private bool onlyFetchMarkedItems;
  [KSerialization.Serialize]
  private bool shouldSaveItems = true;
  public float storageWorkTime = 1.5f;
  private static readonly List<Storage.StoredItemModifierInfo> StoredItemModifierHandlers = new List<Storage.StoredItemModifierInfo>()
  {
    new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Hide, new Action<GameObject, bool, bool>(Storage.MakeItemInvisible)),
    new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Insulate, new Action<GameObject, bool, bool>(Storage.MakeItemTemperatureInsulated)),
    new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Seal, new Action<GameObject, bool, bool>(Storage.MakeItemSealed)),
    new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Preserve, new Action<GameObject, bool, bool>(Storage.MakeItemPreserved))
  };
  [SerializeField]
  private List<Storage.StoredItemModifier> defaultStoredItemModifers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide
  };
  public static readonly List<Storage.StoredItemModifier> StandardSealedStorage = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Seal
  };
  public static readonly List<Storage.StoredItemModifier> StandardFabricatorStorage = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Preserve
  };
  public static readonly List<Storage.StoredItemModifier> StandardInsulatedStorage = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Seal,
    Storage.StoredItemModifier.Insulate
  };
  private static StatusItem capacityStatusItem;
  private static readonly EventSystem.IntraObjectHandler<Storage> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<Storage>(GameTags.Dead, (Action<Storage, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<Storage> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<Storage>((Action<Storage, object>) ((component, data) => component.OnQueueDestroyObject(data)));
  private static readonly EventSystem.IntraObjectHandler<Storage> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Storage>((Action<Storage, object>) ((component, data) => component.OnCopySettings(data)));
  private List<GameObject> deleted_objects;

  public bool ShouldOnlyTransferFromLowerPriority
  {
    get => this.onlyTransferFromLowerPriority || this.allowItemRemoval;
  }

  public bool allowUIItemRemoval { get; set; }

  public GameObject this[int idx] => this.items[idx];

  public int Count => this.items.Count;

  public bool ShouldSaveItems
  {
    get => this.shouldSaveItems;
    set => this.shouldSaveItems = value;
  }

  public bool ShouldShowInUI() => this.showInUI;

  public List<GameObject> GetItems() => this.items;

  public void SetDefaultStoredItemModifiers(List<Storage.StoredItemModifier> modifiers)
  {
    this.defaultStoredItemModifers = modifiers;
  }

  public PrioritySetting masterPriority
  {
    get
    {
      return (bool) (UnityEngine.Object) this.prioritizable ? this.prioritizable.GetMasterPriority() : Chore.DefaultPrioritySetting;
    }
  }

  public override Workable.AnimInfo GetAnim(WorkerBase worker)
  {
    if (!this.useGunForDelivery || !worker.UsesMultiTool())
      return base.GetAnim(worker);
    return base.GetAnim(worker) with
    {
      smi = (StateMachine.Instance) new MultitoolController.Instance((Workable) this, worker, (HashedString) "store", Assets.GetPrefab((Tag) EffectConfigs.OreAbsorbId))
    };
  }

  public override Vector3 GetTargetPoint()
  {
    Vector3 targetPoint = base.GetTargetPoint();
    if (this.useGunForDelivery && this.gunTargetOffset != Vector2.zero)
    {
      if ((UnityEngine.Object) this.rotatable != (UnityEngine.Object) null)
        targetPoint += this.rotatable.GetRotatedOffset((Vector3) this.gunTargetOffset);
      else
        targetPoint += new Vector3(this.gunTargetOffset.x, this.gunTargetOffset.y, 0.0f);
    }
    return targetPoint;
  }

  public event System.Action OnStorageIncreased;

  protected override void OnPrefabInit()
  {
    if (this.useWideOffsets)
      this.SetOffsetTable(OffsetGroups.InvertedWideTable);
    else
      this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.showProgressBar = false;
    this.faceTargetWhenWorking = true;
    base.OnPrefabInit();
    GameUtil.SubscribeToTags<Storage>(this, Storage.OnDeadTagAddedDelegate, true);
    this.Subscribe<Storage>(1502190696, Storage.OnQueueDestroyObjectDelegate);
    this.Subscribe<Storage>(-905833192, Storage.OnCopySettingsDelegate);
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Storing;
    this.resetProgressOnStop = true;
    this.synchronizeAnims = false;
    this.workingPstComplete = (HashedString[]) null;
    this.workingPstFailed = (HashedString[]) null;
    this.SetupStorageStatusItems();
  }

  private void SetupStorageStatusItems()
  {
    if (Storage.capacityStatusItem == null)
    {
      Storage.capacityStatusItem = new StatusItem("StorageLocker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      Storage.capacityStatusItem.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
      {
        Storage storage = (Storage) data;
        float f = storage.MassStored();
        float b = storage.capacityKg;
        string newValue1 = Util.FormatWholeNumber((double) f <= (double) b - (double) storage.storageFullMargin || (double) f >= (double) b ? Mathf.Floor(f) : b);
        IUserControlledCapacity component = storage.GetComponent<IUserControlledCapacity>();
        if (component != null)
          b = Mathf.Min(component.UserMaxCapacity, b);
        string newValue2 = Util.FormatWholeNumber(b);
        str = str.Replace("{Stored}", newValue1);
        str = str.Replace("{Capacity}", newValue2);
        str = component == null ? str.Replace("{Units}", (string) GameUtil.GetCurrentMassUnit()) : str.Replace("{Units}", (string) component.CapacityUnits);
        return str;
      });
    }
    if (!this.showCapacityStatusItem)
      return;
    if (this.showCapacityAsMainStatus)
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Storage.capacityStatusItem, (object) this);
    else
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Stored, Storage.capacityStatusItem, (object) this);
  }

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    if (!this.allowSettingOnlyFetchMarkedItems)
      this.onlyFetchMarkedItems = false;
    this.UpdateFetchCategory();
  }

  protected override void OnSpawn()
  {
    this.SetWorkTime(this.storageWorkTime);
    foreach (GameObject go in this.items)
    {
      this.ApplyStoredItemModifiers(go, true, true);
      if (this.sendOnStoreOnSpawn)
        go.Trigger(856640610, (object) this);
    }
    KBatchedAnimController component1 = this.GetComponent<KBatchedAnimController>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.SetSymbolVisiblity((KAnimHashedString) "sweep", this.onlyFetchMarkedItems);
    Prioritizable component2 = this.GetComponent<Prioritizable>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      component2.onPriorityChanged += new Action<PrioritySetting>(this.OnPriorityChanged);
    this.UpdateFetchCategory();
    if (!this.showUnreachableStatus)
      return;
    this.Subscribe<Storage>(-1432940121, Storage.OnReachableChangedDelegate);
    new ReachabilityMonitor.Instance((Workable) this).StartSM();
  }

  public GameObject Store(
    GameObject go,
    bool hide_popups = false,
    bool block_events = false,
    bool do_disease_transfer = true,
    bool is_deserializing = false)
  {
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      return (GameObject) null;
    PrimaryElement component1 = go.GetComponent<PrimaryElement>();
    GameObject gameObject1 = go;
    if (!hide_popups && (UnityEngine.Object) PopFXManager.Instance != (UnityEngine.Object) null)
    {
      LocString format;
      Transform transform;
      if (this.fxPrefix == Storage.FXPrefix.Delivered)
      {
        format = UI.DELIVERED;
        transform = this.transform;
      }
      else
      {
        format = UI.PICKEDUP;
        transform = go.transform;
      }
      string text = Assets.IsTagCountable(go.PrefabID()) ? string.Format((string) format, (object) (int) component1.Units, (object) go.GetProperName()) : string.Format((string) format, (object) GameUtil.GetFormattedMass(component1.Units), (object) go.GetProperName());
      PopFXManager.Instance.SpawnFX(Def.GetUISprite((object) go).first, this.fxPrefix == Storage.FXPrefix.Delivered ? PopFXManager.Instance.sprite_Plus : PopFXManager.Instance.sprite_Negative, text, transform, this.storageFXOffset);
    }
    go.transform.parent = this.transform;
    Vector3 posCcc = Grid.CellToPosCCC(Grid.PosToCell((KMonoBehaviour) this), Grid.SceneLayer.Move) with
    {
      z = go.transform.GetPosition().z
    };
    go.transform.SetPosition(posCcc);
    if (!block_events & do_disease_transfer)
      this.TransferDiseaseWithObject(go);
    if (!is_deserializing)
    {
      Pickupable component2 = go.GetComponent<Pickupable>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.prevent_absorb_until_stored)
          component2.prevent_absorb_until_stored = false;
        foreach (GameObject gameObject2 in this.items)
        {
          if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null)
          {
            Pickupable component3 = gameObject2.GetComponent<Pickupable>();
            if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && component3.TryAbsorb(component2, hide_popups, true))
            {
              if (!block_events)
              {
                this.Trigger(-1697596308, (object) go);
                Action<GameObject> onStorageChange = this.OnStorageChange;
                if (onStorageChange != null)
                  onStorageChange(go);
                this.Trigger(-778359855, (object) this);
                if (this.OnStorageIncreased != null)
                  this.OnStorageIncreased();
              }
              this.ApplyStoredItemModifiers(go, true, false);
              gameObject1 = gameObject2;
              go = (GameObject) null;
              break;
            }
          }
        }
      }
    }
    if ((UnityEngine.Object) go != (UnityEngine.Object) null)
    {
      this.items.Add(go);
      if (!is_deserializing)
        this.ApplyStoredItemModifiers(go, true, false);
      if (!block_events)
      {
        go.Trigger(856640610, (object) this);
        this.Trigger(-1697596308, (object) go);
        Action<GameObject> onStorageChange = this.OnStorageChange;
        if (onStorageChange != null)
          onStorageChange(go);
        this.Trigger(-778359855, (object) this);
        if (this.OnStorageIncreased != null)
          this.OnStorageIncreased();
      }
    }
    return gameObject1;
  }

  public PrimaryElement AddElement(
    SimHashes element,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count,
    bool keep_zero_mass = false,
    bool do_disease_transfer = true)
  {
    Element elementByHash = ElementLoader.FindElementByHash(element);
    if (elementByHash.IsGas)
      return this.AddGasChunk(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
    if (elementByHash.IsLiquid)
      return this.AddLiquid(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
    return elementByHash.IsSolid ? this.AddOre(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer) : (PrimaryElement) null;
  }

  public PrimaryElement AddOre(
    SimHashes element,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count,
    bool keep_zero_mass = false,
    bool do_disease_transfer = true)
  {
    if ((double) mass <= 0.0)
      return (PrimaryElement) null;
    PrimaryElement primaryElement = this.FindPrimaryElement(element);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
    {
      float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, mass);
      primaryElement.KeepZeroMassObject = keep_zero_mass;
      primaryElement.Mass += mass;
      primaryElement.Temperature = finalTemperature;
      primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddOre");
      this.Trigger(-1697596308, (object) primaryElement.gameObject);
      Action<GameObject> onStorageChange = this.OnStorageChange;
      if (onStorageChange != null)
        onStorageChange(primaryElement.gameObject);
    }
    else
    {
      Element elementByHash = ElementLoader.FindElementByHash(element);
      GameObject go = elementByHash.substance.SpawnResource(this.transform.GetPosition(), mass, temperature, disease_idx, disease_count, true, manual_activation: true);
      go.GetComponent<Pickupable>().prevent_absorb_until_stored = true;
      elementByHash.substance.ActivateSubstanceGameObject(go, disease_idx, disease_count);
      this.Store(go, true, do_disease_transfer: do_disease_transfer);
    }
    return primaryElement;
  }

  public PrimaryElement AddLiquid(
    SimHashes element,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count,
    bool keep_zero_mass = false,
    bool do_disease_transfer = true)
  {
    if ((double) mass <= 0.0)
      return (PrimaryElement) null;
    PrimaryElement primaryElement = this.FindPrimaryElement(element);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
    {
      float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, mass);
      primaryElement.KeepZeroMassObject = keep_zero_mass;
      primaryElement.Mass += mass;
      primaryElement.Temperature = finalTemperature;
      primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddLiquid");
      this.Trigger(-1697596308, (object) primaryElement.gameObject);
      Action<GameObject> onStorageChange = this.OnStorageChange;
      if (onStorageChange != null)
        onStorageChange(primaryElement.gameObject);
    }
    else
    {
      SubstanceChunk chunk = LiquidSourceManager.Instance.CreateChunk(element, mass, temperature, disease_idx, disease_count, this.transform.GetPosition());
      primaryElement = chunk.GetComponent<PrimaryElement>();
      primaryElement.KeepZeroMassObject = keep_zero_mass;
      this.Store(chunk.gameObject, true, do_disease_transfer: do_disease_transfer);
    }
    return primaryElement;
  }

  public PrimaryElement AddGasChunk(
    SimHashes element,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count,
    bool keep_zero_mass,
    bool do_disease_transfer = true)
  {
    if ((double) mass <= 0.0)
      return (PrimaryElement) null;
    PrimaryElement primaryElement = this.FindPrimaryElement(element);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
    {
      float mass1 = primaryElement.Mass;
      float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, mass1, temperature, mass);
      primaryElement.KeepZeroMassObject = keep_zero_mass;
      primaryElement.SetMassTemperature(mass1 + mass, finalTemperature);
      primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddGasChunk");
      this.Trigger(-1697596308, (object) primaryElement.gameObject);
      Action<GameObject> onStorageChange = this.OnStorageChange;
      if (onStorageChange != null)
        onStorageChange(primaryElement.gameObject);
    }
    else
    {
      SubstanceChunk chunk = GasSourceManager.Instance.CreateChunk(element, mass, temperature, disease_idx, disease_count, this.transform.GetPosition());
      primaryElement = chunk.GetComponent<PrimaryElement>();
      primaryElement.KeepZeroMassObject = keep_zero_mass;
      this.Store(chunk.gameObject, true, do_disease_transfer: do_disease_transfer);
    }
    return primaryElement;
  }

  public void Transfer(Storage target, bool block_events = false, bool hide_popups = false)
  {
    while (this.items.Count > 0)
      this.Transfer(this.items[0], target, block_events, hide_popups);
  }

  public bool TransferMass(
    Storage dest_storage,
    Tag tag,
    float amount,
    bool flatten = false,
    bool block_events = false,
    bool hide_popups = false)
  {
    float amount1 = amount;
    while ((double) amount1 > 0.0 && (double) this.GetAmountAvailable(tag) > 0.0)
      amount1 -= this.Transfer(dest_storage, tag, amount1, block_events, hide_popups);
    if (flatten)
      dest_storage.Flatten(tag);
    return (double) amount1 <= 0.0;
  }

  public float Transfer(
    Storage dest_storage,
    Tag tag,
    float amount,
    bool block_events = false,
    bool hide_popups = false)
  {
    GameObject first = this.FindFirst(tag);
    if (!((UnityEngine.Object) first != (UnityEngine.Object) null))
      return 0.0f;
    PrimaryElement component1 = first.GetComponent<PrimaryElement>();
    if ((double) amount < (double) component1.Units)
    {
      Pickupable component2 = first.GetComponent<Pickupable>();
      Pickupable pickupable = component2.Take(amount);
      dest_storage.Store(pickupable.gameObject, hide_popups, block_events);
      if (!block_events)
      {
        this.Trigger(-1697596308, (object) component2.gameObject);
        Action<GameObject> onStorageChange = this.OnStorageChange;
        if (onStorageChange != null)
          onStorageChange(component2.gameObject);
      }
    }
    else
    {
      this.Transfer(first, dest_storage, block_events, hide_popups);
      amount = component1.Units;
    }
    return amount;
  }

  public bool Transfer(GameObject go, Storage target, bool block_events = false, bool hide_popups = false)
  {
    this.items.RemoveAll((Predicate<GameObject>) (it => (UnityEngine.Object) it == (UnityEngine.Object) null));
    int count = this.items.Count;
    for (int index = 0; index < count; ++index)
    {
      if ((UnityEngine.Object) this.items[index] == (UnityEngine.Object) go)
      {
        this.items.RemoveAt(index);
        this.ApplyStoredItemModifiers(go, false, false);
        target.Store(go, hide_popups, block_events);
        if (!block_events)
        {
          this.Trigger(-1697596308, (object) go);
          Action<GameObject> onStorageChange = this.OnStorageChange;
          if (onStorageChange != null)
            onStorageChange(go);
        }
        return true;
      }
    }
    return false;
  }

  public void TransferUnitMass(
    Storage dest_storage,
    Tag tag,
    float unitAmount,
    bool flatten = false,
    bool block_events = false,
    bool hide_popups = false)
  {
    float num = 0.0f;
    for (GameObject first = this.FindFirst(tag); (double) num < (double) unitAmount && (UnityEngine.Object) first != (UnityEngine.Object) null; first = this.FindFirst(tag))
    {
      PrimaryElement component1 = first.GetComponent<PrimaryElement>();
      if ((double) unitAmount < (double) component1.Units)
      {
        Pickupable component2 = first.GetComponent<Pickupable>();
        Pickupable unit = component2.TakeUnit(unitAmount);
        dest_storage.Store(unit.gameObject, hide_popups, block_events);
        if (block_events)
          break;
        this.Trigger(-1697596308, (object) component2.gameObject);
        Action<GameObject> onStorageChange = this.OnStorageChange;
        if (onStorageChange == null)
          break;
        onStorageChange(component2.gameObject);
        break;
      }
      this.Transfer(first, dest_storage, block_events, hide_popups);
      num += component1.Units;
    }
  }

  public bool DropSome(
    Tag tag,
    float amount,
    bool ventGas = false,
    bool dumpLiquid = false,
    Vector3 offset = default (Vector3),
    bool doDiseaseTransfer = true,
    bool showInWorldNotification = false)
  {
    bool flag1 = false;
    float amount1 = amount;
    ListPool<GameObject, Storage>.PooledList result = ListPool<GameObject, Storage>.Allocate();
    this.Find(tag, (List<GameObject>) result);
    foreach (GameObject gameObject in (List<GameObject>) result)
    {
      Pickupable component1 = gameObject.GetComponent<Pickupable>();
      if ((bool) (UnityEngine.Object) component1)
      {
        Pickupable pickupable = component1.Take(amount1);
        if ((UnityEngine.Object) pickupable != (UnityEngine.Object) null)
        {
          bool flag2 = false;
          if (ventGas | dumpLiquid)
          {
            Dumpable component2 = pickupable.GetComponent<Dumpable>();
            if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
            {
              if (ventGas && pickupable.GetComponent<PrimaryElement>().Element.IsGas)
              {
                component2.Dump(this.transform.GetPosition() + offset);
                flag2 = true;
                amount1 -= pickupable.GetComponent<PrimaryElement>().Mass;
                this.Trigger(-1697596308, (object) pickupable.gameObject);
                Action<GameObject> onStorageChange = this.OnStorageChange;
                if (onStorageChange != null)
                  onStorageChange(pickupable.gameObject);
                flag1 = true;
                if (showInWorldNotification)
                  PopFXManager.Instance.SpawnFX(Def.GetUISprite((object) component1.gameObject).first, PopFXManager.Instance.sprite_Plus, $"{pickupable.GetComponent<PrimaryElement>().Element.name} {GameUtil.GetFormattedMass(pickupable.TotalAmount)}", pickupable.transform, this.storageFXOffset);
              }
              if (dumpLiquid && pickupable.GetComponent<PrimaryElement>().Element.IsLiquid)
              {
                component2.Dump(this.transform.GetPosition() + offset);
                flag2 = true;
                amount1 -= pickupable.GetComponent<PrimaryElement>().Mass;
                this.Trigger(-1697596308, (object) pickupable.gameObject);
                Action<GameObject> onStorageChange = this.OnStorageChange;
                if (onStorageChange != null)
                  onStorageChange(pickupable.gameObject);
                flag1 = true;
                if (showInWorldNotification)
                  PopFXManager.Instance.SpawnFX(Def.GetUISprite((object) component1.gameObject).first, PopFXManager.Instance.sprite_Plus, $"{pickupable.GetComponent<PrimaryElement>().Element.name} {GameUtil.GetFormattedMass(pickupable.TotalAmount)}", pickupable.transform, this.storageFXOffset);
              }
            }
          }
          if (!flag2)
          {
            Vector3 position = Grid.CellToPosCCC(Grid.PosToCell((KMonoBehaviour) this), Grid.SceneLayer.Ore) + offset;
            pickupable.transform.SetPosition(position);
            KBatchedAnimController component3 = pickupable.GetComponent<KBatchedAnimController>();
            if ((bool) (UnityEngine.Object) component3)
              component3.SetSceneLayer(Grid.SceneLayer.Ore);
            amount1 -= pickupable.GetComponent<PrimaryElement>().Mass;
            this.MakeWorldActive(pickupable.gameObject);
            this.Trigger(-1697596308, (object) pickupable.gameObject);
            Action<GameObject> onStorageChange = this.OnStorageChange;
            if (onStorageChange != null)
              onStorageChange(pickupable.gameObject);
            flag1 = true;
            if (showInWorldNotification)
              PopFXManager.Instance.SpawnFX(Def.GetUISprite((object) component1.gameObject).first, PopFXManager.Instance.sprite_Plus, $"{pickupable.GetComponent<PrimaryElement>().Element.name} {GameUtil.GetFormattedMass(pickupable.TotalAmount)}", pickupable.transform, this.storageFXOffset);
          }
        }
      }
      if ((double) amount1 <= 0.0)
        break;
    }
    result.Recycle();
    return flag1;
  }

  public void DropAll(
    Vector3 position,
    bool vent_gas = false,
    bool dump_liquid = false,
    Vector3 offset = default (Vector3),
    bool do_disease_transfer = true,
    List<GameObject> collect_dropped_items = null)
  {
    while (this.items.Count > 0)
    {
      GameObject go = this.items[0];
      if (do_disease_transfer)
        this.TransferDiseaseWithObject(go);
      this.items.RemoveAt(0);
      if ((UnityEngine.Object) go != (UnityEngine.Object) null)
      {
        bool flag = false;
        if (vent_gas | dump_liquid)
        {
          Dumpable component = go.GetComponent<Dumpable>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            if (vent_gas && go.GetComponent<PrimaryElement>().Element.IsGas)
            {
              component.Dump(position + offset);
              flag = true;
            }
            if (dump_liquid && go.GetComponent<PrimaryElement>().Element.IsLiquid)
            {
              component.Dump(position + offset);
              flag = true;
            }
          }
        }
        if (!flag)
        {
          go.transform.SetPosition(position + offset);
          KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
          if ((bool) (UnityEngine.Object) component)
            component.SetSceneLayer(Grid.SceneLayer.Ore);
          this.MakeWorldActive(go);
          collect_dropped_items?.Add(go);
        }
      }
    }
  }

  public void DropAll(
    bool vent_gas = false,
    bool dump_liquid = false,
    Vector3 offset = default (Vector3),
    bool do_disease_transfer = true,
    List<GameObject> collect_dropped_items = null)
  {
    this.DropAll(Grid.CellToPosCCC(Grid.PosToCell((KMonoBehaviour) this), Grid.SceneLayer.Ore), vent_gas, dump_liquid, offset, do_disease_transfer, collect_dropped_items);
  }

  public void Drop(Tag t, List<GameObject> obj_list)
  {
    this.Find(t, obj_list);
    foreach (GameObject go in obj_list)
      this.Drop(go, true);
  }

  public void Drop(Tag t)
  {
    ListPool<GameObject, Storage>.PooledList result = ListPool<GameObject, Storage>.Allocate();
    this.Find(t, (List<GameObject>) result);
    foreach (GameObject go in (List<GameObject>) result)
      this.Drop(go, true);
    result.Recycle();
  }

  public void DropUnlessMatching(FetchChore chore)
  {
    for (int index = 0; index < this.items.Count; ++index)
    {
      if (!((UnityEngine.Object) this.items[index] == (UnityEngine.Object) null))
      {
        KPrefabID component = this.items[index].GetComponent<KPrefabID>();
        if (((chore.criteria != FetchChore.MatchCriteria.MatchID || !chore.tags.Contains(component.PrefabTag) ? (chore.criteria != FetchChore.MatchCriteria.MatchTags ? 0 : (component.HasTag(chore.tagsFirst) ? 1 : 0)) : 1) & (!chore.requiredTag.IsValid ? 1 : (component.HasTag(chore.requiredTag) ? 1 : 0)) & (!component.HasAnyTags(chore.forbiddenTags) ? 1 : 0)) == 0)
        {
          GameObject go = this.items[index];
          this.items.RemoveAt(index);
          --index;
          this.TransferDiseaseWithObject(go);
          this.MakeWorldActive(go);
        }
      }
    }
  }

  public GameObject[] DropUnlessHasTag(Tag tag)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    for (int index = 0; index < this.items.Count; ++index)
    {
      if (!((UnityEngine.Object) this.items[index] == (UnityEngine.Object) null) && !this.items[index].GetComponent<KPrefabID>().HasTag(tag))
      {
        GameObject go = this.items[index];
        this.items.RemoveAt(index);
        --index;
        this.TransferDiseaseWithObject(go);
        this.MakeWorldActive(go);
        Dumpable component = go.GetComponent<Dumpable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.Dump(this.transform.GetPosition());
        gameObjectList.Add(go);
      }
    }
    return gameObjectList.ToArray();
  }

  public GameObject[] DropHasTags(Tag[] tag)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    for (int index = 0; index < this.items.Count; ++index)
    {
      if (!((UnityEngine.Object) this.items[index] == (UnityEngine.Object) null) && this.items[index].GetComponent<KPrefabID>().HasAllTags(tag))
      {
        GameObject go = this.items[index];
        this.items.RemoveAt(index);
        --index;
        this.TransferDiseaseWithObject(go);
        this.MakeWorldActive(go);
        Dumpable component = go.GetComponent<Dumpable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.Dump(this.transform.GetPosition());
        gameObjectList.Add(go);
      }
    }
    return gameObjectList.ToArray();
  }

  public GameObject Drop(GameObject go, bool do_disease_transfer = true)
  {
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      return (GameObject) null;
    int count = this.items.Count;
    for (int index = 0; index < count; ++index)
    {
      if (!((UnityEngine.Object) go != (UnityEngine.Object) this.items[index]))
      {
        this.items[index] = this.items[count - 1];
        this.items.RemoveAt(count - 1);
        if (do_disease_transfer)
          this.TransferDiseaseWithObject(go);
        this.MakeWorldActive(go);
        break;
      }
    }
    return go;
  }

  public void RenotifyAll()
  {
    this.items.RemoveAll((Predicate<GameObject>) (it => (UnityEngine.Object) it == (UnityEngine.Object) null));
    foreach (GameObject go in this.items)
      go.Trigger(856640610, (object) this);
  }

  private void TransferDiseaseWithObject(GameObject obj)
  {
    if ((UnityEngine.Object) obj == (UnityEngine.Object) null || !this.doDiseaseTransfer || (UnityEngine.Object) this.primaryElement == (UnityEngine.Object) null)
      return;
    PrimaryElement component = obj.GetComponent<PrimaryElement>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    SimUtil.DiseaseInfo invalid1 = SimUtil.DiseaseInfo.Invalid with
    {
      idx = component.DiseaseIdx,
      count = (int) ((double) component.DiseaseCount * 0.05000000074505806)
    };
    SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid with
    {
      idx = this.primaryElement.DiseaseIdx,
      count = (int) ((double) this.primaryElement.DiseaseCount * 0.05000000074505806)
    };
    component.ModifyDiseaseCount(-invalid1.count, "Storage.TransferDiseaseWithObject");
    this.primaryElement.ModifyDiseaseCount(-invalid2.count, "Storage.TransferDiseaseWithObject");
    if (invalid1.count > 0)
      this.primaryElement.AddDisease(invalid1.idx, invalid1.count, "Storage.TransferDiseaseWithObject");
    if (invalid2.count <= 0)
      return;
    component.AddDisease(invalid2.idx, invalid2.count, "Storage.TransferDiseaseWithObject");
  }

  private void MakeWorldActive(GameObject go)
  {
    go.transform.parent = (Transform) null;
    if (this.dropOffset != Vector2.zero)
      go.transform.Translate((Vector3) this.dropOffset);
    go.Trigger(856640610);
    this.Trigger(-1697596308, (object) go);
    Action<GameObject> onStorageChange = this.OnStorageChange;
    if (onStorageChange != null)
      onStorageChange(go);
    this.ApplyStoredItemModifiers(go, false, false);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    PrimaryElement component = go.GetComponent<PrimaryElement>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.KeepZeroMassObject)
      return;
    component.KeepZeroMassObject = false;
    if ((double) component.Mass > 0.0)
      return;
    Util.KDestroyGameObject(go);
  }

  public List<GameObject> Find(Tag tag, List<GameObject> result)
  {
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(tag))
        result.Add(go);
    }
    return result;
  }

  public GameObject FindFirst(Tag tag)
  {
    GameObject first = (GameObject) null;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(tag))
      {
        first = go;
        break;
      }
    }
    return first;
  }

  public PrimaryElement FindFirstWithMass(Tag tag, float mass = 0.0f)
  {
    PrimaryElement firstWithMass = (PrimaryElement) null;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.HasTag(tag))
      {
        PrimaryElement component = go.GetComponent<PrimaryElement>();
        if ((double) component.Mass > 0.0 && (double) component.Mass >= (double) mass)
        {
          firstWithMass = component;
          break;
        }
      }
    }
    return firstWithMass;
  }

  private void Flatten(Tag tag_to_combine)
  {
    GameObject first = this.FindFirst(tag_to_combine);
    if ((UnityEngine.Object) first == (UnityEngine.Object) null)
      return;
    PrimaryElement component1 = first.GetComponent<PrimaryElement>();
    for (int index = this.items.Count - 1; index >= 0; --index)
    {
      GameObject gameObject = this.items[index];
      if (gameObject.HasTag(tag_to_combine) && (UnityEngine.Object) gameObject != (UnityEngine.Object) first)
      {
        PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
        component1.Mass += component2.Mass;
        this.ConsumeIgnoringDisease(gameObject);
      }
    }
  }

  public HashSet<Tag> GetAllIDsInStorage()
  {
    HashSet<Tag> allIdsInStorage = new HashSet<Tag>();
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      allIdsInStorage.Add(go.PrefabID());
    }
    return allIdsInStorage;
  }

  public GameObject Find(int ID)
  {
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if (ID == go.PrefabID().GetHashCode())
        return go;
    }
    return (GameObject) null;
  }

  public void ConsumeAllIgnoringDisease() => this.ConsumeAllIgnoringDisease(Tag.Invalid);

  public void ConsumeAllIgnoringDisease(Tag tag)
  {
    for (int index = this.items.Count - 1; index >= 0; --index)
    {
      if (!(tag != Tag.Invalid) || this.items[index].HasTag(tag))
        this.ConsumeIgnoringDisease(this.items[index]);
    }
  }

  public void ConsumeAndGetDisease(
    Tag tag,
    float amount,
    out float amount_consumed,
    out SimUtil.DiseaseInfo disease_info,
    out float aggregate_temperature)
  {
    this.ConsumeAndGetDisease(tag, amount, out amount_consumed, out disease_info, out aggregate_temperature, out SimHashes _);
  }

  public void ConsumeAndGetDisease(
    Tag tag,
    float amount,
    out float amount_consumed,
    out SimUtil.DiseaseInfo disease_info,
    out float aggregate_temperature,
    out SimHashes mostRelevantItemElement)
  {
    DebugUtil.Assert(tag.IsValid);
    amount_consumed = 0.0f;
    disease_info = SimUtil.DiseaseInfo.Invalid;
    mostRelevantItemElement = SimHashes.Vacuum;
    aggregate_temperature = 0.0f;
    bool flag = false;
    float num = 0.0f;
    for (int index = 0; index < this.items.Count && (double) amount > 0.0; ++index)
    {
      GameObject gameObject = this.items[index];
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null) && gameObject.HasTag(tag))
      {
        PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
        if ((double) component.Units > 0.0)
        {
          flag = true;
          float mass2 = Math.Min(component.Units, amount);
          Debug.Assert((double) mass2 > 0.0, (object) "Delta amount was zero, which should be impossible.");
          aggregate_temperature = SimUtil.CalculateFinalTemperature(amount_consumed, aggregate_temperature, mass2, component.Temperature);
          SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(component, mass2 / component.Units);
          disease_info = SimUtil.CalculateFinalDiseaseInfo(disease_info, percentOfDisease);
          component.Units -= mass2;
          component.ModifyDiseaseCount(-percentOfDisease.count, "Storage.ConsumeAndGetDisease");
          amount -= mass2;
          amount_consumed += mass2;
          if ((double) mass2 > (double) num)
          {
            num = mass2;
            mostRelevantItemElement = component.ElementID;
          }
        }
        if ((double) component.Units <= 0.0 && !component.KeepZeroMassObject)
        {
          if (this.deleted_objects == null)
            this.deleted_objects = new List<GameObject>();
          this.deleted_objects.Add(gameObject);
        }
        this.Trigger(-1697596308, (object) gameObject);
        Action<GameObject> onStorageChange = this.OnStorageChange;
        if (onStorageChange != null)
          onStorageChange(gameObject);
      }
    }
    if (!flag)
      aggregate_temperature = this.GetComponent<PrimaryElement>().Temperature;
    if (this.deleted_objects == null)
      return;
    for (int index = 0; index < this.deleted_objects.Count; ++index)
    {
      this.items.Remove(this.deleted_objects[index]);
      Util.KDestroyGameObject(this.deleted_objects[index]);
    }
    this.deleted_objects.Clear();
  }

  public void ConsumeAndGetDisease(
    Recipe.Ingredient ingredient,
    out SimUtil.DiseaseInfo disease_info,
    out float temperature)
  {
    this.ConsumeAndGetDisease(ingredient.tag, ingredient.amount, out float _, out disease_info, out temperature);
  }

  public void ConsumeIgnoringDisease(Tag tag, float amount)
  {
    this.ConsumeAndGetDisease(tag, amount, out float _, out SimUtil.DiseaseInfo _, out float _);
  }

  public void ConsumeIgnoringDisease(GameObject item_go)
  {
    if (!this.items.Contains(item_go))
      return;
    PrimaryElement component = item_go.GetComponent<PrimaryElement>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.KeepZeroMassObject)
    {
      component.Units = 0.0f;
      component.ModifyDiseaseCount(-component.DiseaseCount, "consume item");
      this.Trigger(-1697596308, (object) item_go);
      Action<GameObject> onStorageChange = this.OnStorageChange;
      if (onStorageChange == null)
        return;
      onStorageChange(item_go);
    }
    else
    {
      this.items.Remove(item_go);
      this.Trigger(-1697596308, (object) item_go);
      Action<GameObject> onStorageChange = this.OnStorageChange;
      if (onStorageChange != null)
        onStorageChange(item_go);
      item_go.DeleteObject();
    }
  }

  public GameObject Drop(int ID) => this.Drop(this.Find(ID), true);

  private void OnDeath(object data)
  {
    List<GameObject> collect_dropped_items = new List<GameObject>();
    this.DropAll(true, true, collect_dropped_items: collect_dropped_items);
    if (this.onDestroyItemsDropped == null)
      return;
    this.onDestroyItemsDropped(collect_dropped_items);
  }

  public bool IsFull() => (double) this.RemainingCapacity() <= 0.0;

  public bool IsEmpty() => this.items.Count == 0;

  public float Capacity() => this.capacityKg;

  public bool IsEndOfLife() => this.endOfLife;

  public float ExactMassStored()
  {
    float num = 0.0f;
    for (int index = 0; index < this.items.Count; ++index)
    {
      if (!((UnityEngine.Object) this.items[index] == (UnityEngine.Object) null))
      {
        PrimaryElement component = this.items[index].GetComponent<PrimaryElement>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          num += component.Units * component.MassPerUnit;
      }
    }
    return num;
  }

  public float MassStored() => (float) Mathf.RoundToInt(this.ExactMassStored() * 1000f) / 1000f;

  public float UnitsStored()
  {
    float num = 0.0f;
    for (int index = 0; index < this.items.Count; ++index)
    {
      if (!((UnityEngine.Object) this.items[index] == (UnityEngine.Object) null))
      {
        PrimaryElement component = this.items[index].GetComponent<PrimaryElement>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          num += component.Units;
      }
    }
    return (float) Mathf.RoundToInt(num * 1000f) / 1000f;
  }

  public bool Has(Tag tag)
  {
    bool flag = false;
    foreach (GameObject gameObject in this.items)
    {
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
      {
        PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
        if (component.HasTag(tag) && (double) component.Mass > 0.0)
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  public PrimaryElement AddToPrimaryElement(
    SimHashes element,
    float additional_mass,
    float temperature)
  {
    PrimaryElement primaryElement = this.FindPrimaryElement(element);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
    {
      float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, additional_mass);
      primaryElement.Mass += additional_mass;
      primaryElement.Temperature = finalTemperature;
    }
    return primaryElement;
  }

  public PrimaryElement FindPrimaryElement(SimHashes element)
  {
    PrimaryElement primaryElement = (PrimaryElement) null;
    foreach (GameObject gameObject in this.items)
    {
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
      {
        PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
        if (component.ElementID == element)
        {
          primaryElement = component;
          break;
        }
      }
    }
    return primaryElement;
  }

  public float RemainingCapacity() => this.capacityKg - this.MassStored();

  public bool GetOnlyFetchMarkedItems() => this.onlyFetchMarkedItems;

  public void SetOnlyFetchMarkedItems(bool is_set)
  {
    if (is_set == this.onlyFetchMarkedItems)
      return;
    this.onlyFetchMarkedItems = is_set;
    this.UpdateFetchCategory();
    this.Trigger(644822890, (object) null);
    this.GetComponent<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "sweep", is_set);
  }

  private void UpdateFetchCategory()
  {
    if (this.fetchCategory == Storage.FetchCategory.Building)
      return;
    this.fetchCategory = this.onlyFetchMarkedItems ? Storage.FetchCategory.StorageSweepOnly : Storage.FetchCategory.GeneralStorage;
  }

  protected override void OnCleanUp()
  {
    if (this.items.Count != 0)
      Debug.LogWarning((object) $"Storage for [{this.gameObject.name}] is being destroyed but it still contains items!", (UnityEngine.Object) this.gameObject);
    base.OnCleanUp();
  }

  private void OnQueueDestroyObject(object data)
  {
    this.endOfLife = true;
    List<GameObject> collect_dropped_items = new List<GameObject>();
    this.DropAll(true, collect_dropped_items: collect_dropped_items);
    if (this.onDestroyItemsDropped != null)
      this.onDestroyItemsDropped(collect_dropped_items);
    this.OnCleanUp();
  }

  public void Remove(GameObject go, bool do_disease_transfer = true)
  {
    this.items.Remove(go);
    if (do_disease_transfer)
      this.TransferDiseaseWithObject(go);
    this.Trigger(-1697596308, (object) go);
    Action<GameObject> onStorageChange = this.OnStorageChange;
    if (onStorageChange != null)
      onStorageChange(go);
    this.ApplyStoredItemModifiers(go, false, false);
  }

  public bool ForceStore(Tag tag, float amount)
  {
    Debug.Assert((double) amount < (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT);
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if ((UnityEngine.Object) go != (UnityEngine.Object) null && go.HasTag(tag))
      {
        go.GetComponent<PrimaryElement>().Mass += amount;
        return true;
      }
    }
    return false;
  }

  public float GetAmountAvailable(Tag tag)
  {
    float amountAvailable = 0.0f;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if ((UnityEngine.Object) go != (UnityEngine.Object) null && go.HasTag(tag))
        amountAvailable += go.GetComponent<PrimaryElement>().Units;
    }
    return amountAvailable;
  }

  public float GetAmountAvailable(Tag tag, Tag[] forbiddenTags = null)
  {
    if (forbiddenTags == null)
      return this.GetAmountAvailable(tag);
    float amountAvailable = 0.0f;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if ((UnityEngine.Object) go != (UnityEngine.Object) null && go.HasTag(tag) && !go.HasAnyTags(forbiddenTags))
        amountAvailable += go.GetComponent<PrimaryElement>().Units;
    }
    return amountAvailable;
  }

  public float GetUnitsAvailable(Tag tag)
  {
    float unitsAvailable = 0.0f;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if ((UnityEngine.Object) go != (UnityEngine.Object) null && go.HasTag(tag))
        unitsAvailable += go.GetComponent<PrimaryElement>().Units;
    }
    return unitsAvailable;
  }

  public float GetMassAvailable(Tag tag)
  {
    float massAvailable = 0.0f;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject go = this.items[index];
      if ((UnityEngine.Object) go != (UnityEngine.Object) null && go.HasTag(tag))
        massAvailable += go.GetComponent<PrimaryElement>().Mass;
    }
    return massAvailable;
  }

  public float GetMassAvailable(SimHashes element)
  {
    float massAvailable = 0.0f;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject gameObject = this.items[index];
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
        if (component.ElementID == element)
          massAvailable += component.Mass;
      }
    }
    return massAvailable;
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = base.GetDescriptors(go);
    if (this.showDescriptor)
      descriptors.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.STORAGECAPACITY, (object) GameUtil.GetFormattedMass(this.Capacity())), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.STORAGECAPACITY, (object) GameUtil.GetFormattedMass(this.Capacity()))));
    return descriptors;
  }

  public static void MakeItemTemperatureInsulated(
    GameObject go,
    bool is_stored,
    bool is_initializing)
  {
    SimTemperatureTransfer component = go.GetComponent<SimTemperatureTransfer>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    component.enabled = !is_stored;
  }

  public static void MakeItemInvisible(GameObject go, bool is_stored, bool is_initializing)
  {
    if (is_initializing)
      return;
    bool flag = !is_stored;
    KAnimControllerBase component1 = go.GetComponent<KAnimControllerBase>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.enabled != flag)
      component1.enabled = flag;
    KSelectable component2 = go.GetComponent<KSelectable>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null) || component2.enabled == flag)
      return;
    component2.enabled = flag;
  }

  public static void MakeItemSealed(GameObject go, bool is_stored, bool is_initializing)
  {
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    if (is_stored)
      go.GetComponent<KPrefabID>().AddTag(GameTags.Sealed);
    else
      go.GetComponent<KPrefabID>().RemoveTag(GameTags.Sealed);
  }

  public static void MakeItemPreserved(GameObject go, bool is_stored, bool is_initializing)
  {
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    if (is_stored)
      go.GetComponent<KPrefabID>().AddTag(GameTags.Preserved);
    else
      go.GetComponent<KPrefabID>().RemoveTag(GameTags.Preserved);
  }

  private void ApplyStoredItemModifiers(GameObject go, bool is_stored, bool is_initializing)
  {
    List<Storage.StoredItemModifier> storedItemModifers = this.defaultStoredItemModifers;
    for (int index1 = 0; index1 < storedItemModifers.Count; ++index1)
    {
      Storage.StoredItemModifier storedItemModifier = storedItemModifers[index1];
      for (int index2 = 0; index2 < Storage.StoredItemModifierHandlers.Count; ++index2)
      {
        Storage.StoredItemModifierInfo itemModifierHandler = Storage.StoredItemModifierHandlers[index2];
        if (itemModifierHandler.modifier == storedItemModifier)
        {
          itemModifierHandler.toggleState(go, is_stored, is_initializing);
          break;
        }
      }
    }
  }

  protected virtual void OnCopySettings(object data)
  {
    Storage component = ((GameObject) data).GetComponent<Storage>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.SetOnlyFetchMarkedItems(component.onlyFetchMarkedItems);
  }

  private void OnPriorityChanged(PrioritySetting priority)
  {
    foreach (GameObject go in this.items)
      go.Trigger(-1626373771, (object) this);
  }

  private void OnReachableChanged(object data)
  {
    int num = ((Boxed<bool>) data).value ? 1 : 0;
    KSelectable component = this.GetComponent<KSelectable>();
    if (num != 0)
      component.RemoveStatusItem(Db.Get().BuildingStatusItems.StorageUnreachable);
    else
      component.AddStatusItem(Db.Get().BuildingStatusItems.StorageUnreachable, (object) this);
  }

  public void SetContentsDeleteOffGrid(bool delete_off_grid)
  {
    for (int index = 0; index < this.items.Count; ++index)
    {
      Pickupable component1 = this.items[index].GetComponent<Pickupable>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.deleteOffGrid = delete_off_grid;
      Storage component2 = this.items[index].GetComponent<Storage>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        component2.SetContentsDeleteOffGrid(delete_off_grid);
    }
  }

  private bool ShouldSaveItem(GameObject go)
  {
    if (!this.shouldSaveItems)
      return false;
    bool flag = false;
    if ((UnityEngine.Object) go != (UnityEngine.Object) null && (UnityEngine.Object) go.GetComponent<SaveLoadRoot>() != (UnityEngine.Object) null && (double) go.GetComponent<PrimaryElement>().Mass > 0.0)
      flag = true;
    return flag;
  }

  public void Serialize(BinaryWriter writer)
  {
    int num = 0;
    int count = this.items.Count;
    for (int index = 0; index < count; ++index)
    {
      if (this.ShouldSaveItem(this.items[index]))
        ++num;
    }
    writer.Write(num);
    if (num == 0 || this.items == null || this.items.Count <= 0)
      return;
    for (int index = 0; index < this.items.Count; ++index)
    {
      GameObject gameObject = this.items[index];
      if (this.ShouldSaveItem(gameObject))
      {
        SaveLoadRoot component = gameObject.GetComponent<SaveLoadRoot>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          string name = gameObject.GetComponent<KPrefabID>().GetSaveLoadTag().Name;
          writer.WriteKleiString(name);
          component.Save(writer);
        }
        else
          Debug.Log((object) "Tried to save obj in storage but obj has no SaveLoadRoot", (UnityEngine.Object) gameObject);
      }
    }
  }

  public void Deserialize(IReader reader)
  {
    double realtimeSinceStartup1 = (double) Time.realtimeSinceStartup;
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    this.ClearItems();
    int capacity = reader.ReadInt32();
    this.items = new List<GameObject>(capacity);
    for (int index = 0; index < capacity; ++index)
    {
      float realtimeSinceStartup2 = Time.realtimeSinceStartup;
      Tag tag = TagManager.Create(reader.ReadKleiString());
      SaveLoadRoot saveLoadRoot = SaveLoadRoot.Load(tag, reader);
      num1 += Time.realtimeSinceStartup - realtimeSinceStartup2;
      if ((UnityEngine.Object) saveLoadRoot != (UnityEngine.Object) null)
      {
        KBatchedAnimController component1 = saveLoadRoot.GetComponent<KBatchedAnimController>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          component1.enabled = false;
        saveLoadRoot.SetRegistered(false);
        float realtimeSinceStartup3 = Time.realtimeSinceStartup;
        GameObject gameObject = this.Store(saveLoadRoot.gameObject, true, true, false, true);
        num2 += Time.realtimeSinceStartup - realtimeSinceStartup3;
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          Pickupable component2 = gameObject.GetComponent<Pickupable>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          {
            float realtimeSinceStartup4 = Time.realtimeSinceStartup;
            component2.OnStore((object) this);
            num3 += Time.realtimeSinceStartup - realtimeSinceStartup4;
          }
          Storable component3 = gameObject.GetComponent<Storable>();
          if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
          {
            float realtimeSinceStartup5 = Time.realtimeSinceStartup;
            component3.OnStore((object) this);
            num3 += Time.realtimeSinceStartup - realtimeSinceStartup5;
          }
          if (this.dropOnLoad)
            this.Drop(saveLoadRoot.gameObject, true);
        }
      }
      else
        Debug.LogWarning((object) $"Tried to deserialize {tag.ToString()} into storage but failed", (UnityEngine.Object) this.gameObject);
    }
  }

  private void ClearItems()
  {
    foreach (GameObject go in this.items)
      go.DeleteObject();
    this.items.Clear();
  }

  public void UpdateStoredItemCachedCells()
  {
    foreach (GameObject gameObject in this.items)
    {
      Pickupable component = gameObject.GetComponent<Pickupable>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.UpdateCachedCellFromStoragePosition();
    }
  }

  public enum StoredItemModifier
  {
    Insulate,
    Hide,
    Seal,
    Preserve,
  }

  public enum FetchCategory
  {
    Building,
    GeneralStorage,
    StorageSweepOnly,
  }

  public enum FXPrefix
  {
    Delivered,
    PickedUp,
  }

  private struct StoredItemModifierInfo(
    Storage.StoredItemModifier modifier,
    Action<GameObject, bool, bool> toggle_state)
  {
    public Storage.StoredItemModifier modifier = modifier;
    public Action<GameObject, bool, bool> toggleState = toggle_state;
  }
}
