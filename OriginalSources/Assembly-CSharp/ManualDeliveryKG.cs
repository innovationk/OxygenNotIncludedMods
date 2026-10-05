// Decompiled with JetBrains decompiler
// Type: ManualDeliveryKG
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ManualDeliveryKG")]
public class ManualDeliveryKG : KMonoBehaviour, ISim1000ms
{
  [MyCmpGet]
  private Operational operational;
  [SerializeField]
  private Storage storage;
  [SerializeField]
  private Tag requestedItemTag;
  private Tag[] forbiddenTags;
  [SerializeField]
  public float capacity = 100f;
  [SerializeField]
  public float refillMass = 10f;
  [SerializeField]
  public float MinimumMass = 10f;
  [SerializeField]
  public bool RoundFetchAmountToInt;
  [SerializeField]
  public bool FillToCapacity;
  [SerializeField]
  public Operational.State operationalRequirement;
  [SerializeField]
  public bool allowPause;
  [SerializeField]
  private bool paused;
  [SerializeField]
  public HashedString choreTypeIDHash;
  [Serialize]
  private bool userPaused;
  public bool handlePrioritizable = true;
  public bool ShowStatusItem = true;
  public bool FillToMinimumMass;
  private FetchList2 fetchList;
  private int onStorageChangeSubscription = -1;
  private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>((Action<ManualDeliveryKG, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>((Action<ManualDeliveryKG, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>((Action<ManualDeliveryKG, object>) ((component, data) => component.OnStorageChanged(data)));

  public bool IsPaused => this.paused;

  public float Capacity => this.capacity;

  public Tag RequestedItemTag
  {
    get => this.requestedItemTag;
    set
    {
      this.requestedItemTag = value;
      this.AbortDelivery("Requested Item Tag Changed");
    }
  }

  public Tag[] ForbiddenTags
  {
    get => this.forbiddenTags;
    set
    {
      this.forbiddenTags = value;
      this.AbortDelivery("Forbidden Tags Changed");
    }
  }

  public Storage DebugStorage => this.storage;

  public FetchList2 DebugFetchList => this.fetchList;

  private float MassStored => this.storage.GetMassAvailable(this.requestedItemTag);

  protected override void OnSpawn()
  {
    base.OnSpawn();
    DebugUtil.Assert(this.choreTypeIDHash.IsValid, "ManualDeliveryKG Must have a valid chore type specified!", this.name);
    if (this.allowPause)
    {
      this.Subscribe<ManualDeliveryKG>(493375141, ManualDeliveryKG.OnRefreshUserMenuDelegate);
      this.Subscribe<ManualDeliveryKG>(-111137758, ManualDeliveryKG.OnRefreshUserMenuDelegate);
    }
    this.Subscribe<ManualDeliveryKG>(-592767678, ManualDeliveryKG.OnOperationalChangedDelegate);
    if ((UnityEngine.Object) this.storage != (UnityEngine.Object) null)
      this.SetStorage(this.storage);
    if (this.handlePrioritizable)
      Prioritizable.AddRef(this.gameObject);
    if (!this.userPaused || !this.allowPause)
      return;
    this.OnPause();
  }

  protected override void OnCleanUp()
  {
    this.AbortDelivery("ManualDeliverKG destroyed");
    if (this.handlePrioritizable && this.isSpawned)
      Prioritizable.RemoveRef(this.gameObject);
    base.OnCleanUp();
  }

  public void SetStorage(Storage storage)
  {
    if ((UnityEngine.Object) this.storage != (UnityEngine.Object) null)
    {
      this.storage.Unsubscribe(this.onStorageChangeSubscription);
      this.onStorageChangeSubscription = -1;
    }
    this.AbortDelivery("storage pointer changed");
    this.storage = storage;
    if (!((UnityEngine.Object) this.storage != (UnityEngine.Object) null) || !this.isSpawned)
      return;
    Debug.Assert(this.onStorageChangeSubscription == -1);
    this.onStorageChangeSubscription = this.storage.Subscribe<ManualDeliveryKG>(-1697596308, ManualDeliveryKG.OnStorageChangedDelegate);
  }

  public void Pause(bool pause, string reason)
  {
    if (this.paused == pause)
      return;
    this.paused = pause;
    if (!pause)
      return;
    this.AbortDelivery(reason);
  }

  public void Sim1000ms(float dt) => this.UpdateDeliveryState();

  [ContextMenu("UpdateDeliveryState")]
  public void UpdateDeliveryState()
  {
    if (!this.requestedItemTag.IsValid || (UnityEngine.Object) this.storage == (UnityEngine.Object) null)
      return;
    this.UpdateFetchList();
  }

  public void RequestDelivery()
  {
    if (this.fetchList != null)
      return;
    float massStored = this.MassStored;
    if ((double) massStored >= (double) this.capacity)
      return;
    this.CreateFetchChore(massStored);
  }

  private void CreateFetchChore(float stored_mass)
  {
    float b = this.capacity - stored_mass;
    float num1 = Mathf.Max(PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT, b);
    if (this.RoundFetchAmountToInt)
    {
      num1 = (float) (int) num1;
      if ((double) num1 < 0.10000000149011612)
        return;
    }
    this.fetchList = new FetchList2(this.storage, Db.Get().ChoreTypes.GetByHash(this.choreTypeIDHash));
    this.fetchList.ShowStatusItem = this.ShowStatusItem;
    this.fetchList.MinimumAmount[this.requestedItemTag] = Mathf.Max(PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT, this.MinimumMass);
    FetchList2 fetchList = this.fetchList;
    Tag requestedItemTag = this.requestedItemTag;
    float num2 = num1;
    Tag[] forbiddenTags = this.forbiddenTags;
    double amount = (double) num2;
    fetchList.Add(requestedItemTag, forbiddenTags, (float) amount);
    this.fetchList.Submit(new System.Action(this.OnFetchComplete), false);
  }

  private void OnFetchComplete()
  {
    if (!this.FillToCapacity || !((UnityEngine.Object) this.storage != (UnityEngine.Object) null))
      return;
    float amountAvailable = this.storage.GetAmountAvailable(this.requestedItemTag);
    if ((double) amountAvailable >= (double) this.capacity)
      return;
    this.CreateFetchChore(amountAvailable);
  }

  private void UpdateFetchList()
  {
    if (this.paused)
      return;
    if (this.fetchList != null && this.fetchList.IsComplete)
      this.fetchList = (FetchList2) null;
    if (!((UnityEngine.Object) this.operational == (UnityEngine.Object) null) && !this.operational.MeetsRequirements(this.operationalRequirement))
    {
      if (this.fetchList == null)
        return;
      this.fetchList.Cancel("Operational requirements");
      this.fetchList = (FetchList2) null;
    }
    else if (this.fetchList == null)
    {
      if ((double) this.MassStored >= (double) this.refillMass)
        return;
      this.RequestDelivery();
    }
    else
    {
      if (!this.FillToMinimumMass)
        return;
      Dictionary<Tag, float> remaining = this.fetchList.GetRemaining();
      if (!remaining.ContainsKey(this.requestedItemTag) || (double) remaining[this.requestedItemTag] >= (double) this.MinimumMass)
        return;
      this.AbortDelivery("Invalid Mass");
    }
  }

  public void AbortDelivery(string reason)
  {
    if (this.fetchList == null)
      return;
    FetchList2 fetchList = this.fetchList;
    this.fetchList = (FetchList2) null;
    string reason1 = reason;
    fetchList.Cancel(reason1);
  }

  protected void OnStorageChanged(object data) => this.UpdateDeliveryState();

  private void OnPause()
  {
    this.userPaused = true;
    this.Pause(true, "Forbid manual delivery");
  }

  private void OnResume()
  {
    this.userPaused = false;
    this.Pause(false, "Allow manual delivery");
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.allowPause)
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, !this.paused ? new KIconButtonMenu.ButtonInfo("action_move_to_storage", (string) UI.USERMENUACTIONS.MANUAL_DELIVERY.NAME, new System.Action(this.OnPause), tooltipText: (string) UI.USERMENUACTIONS.MANUAL_DELIVERY.TOOLTIP) : new KIconButtonMenu.ButtonInfo("action_move_to_storage", (string) UI.USERMENUACTIONS.MANUAL_DELIVERY.NAME_OFF, new System.Action(this.OnResume), tooltipText: (string) UI.USERMENUACTIONS.MANUAL_DELIVERY.TOOLTIP_OFF));
  }

  private void OnOperationalChanged(object _) => this.UpdateDeliveryState();
}
