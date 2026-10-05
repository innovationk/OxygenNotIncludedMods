// Decompiled with JetBrains decompiler
// Type: StorageLocker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/StorageLocker")]
public class StorageLocker : KMonoBehaviour, IUserControlledCapacity
{
  private LoggerFS log;
  [Serialize]
  private float userMaxCapacity = float.PositiveInfinity;
  [Serialize]
  public string lockerName = "";
  protected FilteredStorage filteredStorage;
  [MyCmpGet]
  private UserNameable nameable;
  public string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;
  private static readonly EventSystem.IntraObjectHandler<StorageLocker> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<StorageLocker>((Action<StorageLocker, object>) ((component, data) => component.OnCopySettings(data)));

  protected override void OnPrefabInit() => this.Initialize(false);

  protected void Initialize(bool use_logic_meter)
  {
    base.OnPrefabInit();
    this.log = new LoggerFS(nameof (StorageLocker));
    ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
    this.filteredStorage = new FilteredStorage((KMonoBehaviour) this, (Tag[]) null, (IUserControlledCapacity) this, use_logic_meter, fetch_chore_type);
    this.Subscribe<StorageLocker>(-905833192, StorageLocker.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    this.filteredStorage.FilterChanged();
    if ((UnityEngine.Object) this.nameable != (UnityEngine.Object) null && !this.lockerName.IsNullOrWhiteSpace())
      this.nameable.SetName(this.lockerName);
    this.Trigger(-1683615038, (object) null);
  }

  protected override void OnCleanUp() => this.filteredStorage.CleanUp();

  private void OnCopySettings(object data)
  {
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return;
    StorageLocker component = gameObject.GetComponent<StorageLocker>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.UserMaxCapacity = component.UserMaxCapacity;
  }

  public void UpdateForbiddenTag(Tag game_tag, bool forbidden)
  {
    if (forbidden)
      this.filteredStorage.RemoveForbiddenTag(game_tag);
    else
      this.filteredStorage.AddForbiddenTag(game_tag);
  }

  public virtual float UserMaxCapacity
  {
    get => Mathf.Min(this.userMaxCapacity, this.GetComponent<Storage>().capacityKg);
    set
    {
      this.userMaxCapacity = value;
      this.filteredStorage.FilterChanged();
    }
  }

  public float AmountStored => this.GetComponent<Storage>().MassStored();

  public float MinCapacity => 0.0f;

  public float MaxCapacity => this.GetComponent<Storage>().capacityKg;

  public bool WholeValues => false;

  public LocString CapacityUnits => GameUtil.GetCurrentMassUnit();
}
