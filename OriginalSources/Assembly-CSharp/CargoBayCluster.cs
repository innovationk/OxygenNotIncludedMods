// Decompiled with JetBrains decompiler
// Type: CargoBayCluster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class CargoBayCluster : KMonoBehaviour, IUserControlledCapacity
{
  private MeterController meter;
  [SerializeField]
  public Storage storage;
  [SerializeField]
  public CargoBay.CargoType storageType;
  [Serialize]
  private float userMaxCapacity;
  private static readonly EventSystem.IntraObjectHandler<CargoBayCluster> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CargoBayCluster>((Action<CargoBayCluster, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<CargoBayCluster> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<CargoBayCluster>((Action<CargoBayCluster, object>) ((component, data) => component.OnStorageChange(data)));

  public float UserMaxCapacity
  {
    get => this.userMaxCapacity;
    set
    {
      this.userMaxCapacity = value;
      this.Trigger(-945020481, (object) this);
    }
  }

  public float MinCapacity => 0.0f;

  public float MaxCapacity => this.storage.capacityKg;

  public float AmountStored => this.storage.MassStored();

  public bool WholeValues => false;

  public LocString CapacityUnits => GameUtil.GetCurrentMassUnit();

  public float RemainingCapacity => this.userMaxCapacity - this.storage.MassStored();

  protected override void OnPrefabInit() => this.userMaxCapacity = this.storage.capacityKg;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<KBatchedAnimController>().Play((HashedString) "grounded", KAnim.PlayMode.Loop);
    this.Subscribe<CargoBayCluster>(493375141, CargoBayCluster.OnRefreshUserMenuDelegate);
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[4]
    {
      "meter_target",
      "meter_fill",
      "meter_frame",
      "meter_OL"
    });
    KBatchedAnimTracker component = this.meter.gameObject.GetComponent<KBatchedAnimTracker>();
    component.matchParentOffset = true;
    component.forceAlwaysAlive = true;
    this.OnStorageChange((object) null);
    this.Subscribe<CargoBayCluster>(-1697596308, CargoBayCluster.OnStorageChangeDelegate);
  }

  private void OnRefreshUserMenu(object data)
  {
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_empty_contents", (string) UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, (System.Action) (() => this.storage.DropAll()), tooltipText: (string) UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP));
  }

  private void OnStorageChange(object data)
  {
    this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
    this.UpdateCargoStatusItem();
  }

  private void UpdateCargoStatusItem()
  {
    RocketModuleCluster component1 = this.GetComponent<RocketModuleCluster>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      return;
    CraftModuleInterface craftInterface = component1.CraftInterface;
    if ((UnityEngine.Object) craftInterface == (UnityEngine.Object) null)
      return;
    Clustercraft component2 = craftInterface.GetComponent<Clustercraft>();
    if ((UnityEngine.Object) component2 == (UnityEngine.Object) null)
      return;
    component2.UpdateStatusItem();
  }
}
