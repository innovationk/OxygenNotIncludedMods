// Decompiled with JetBrains decompiler
// Type: CargoBayConduit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/CargoBay")]
public class CargoBayConduit : KMonoBehaviour
{
  public static Dictionary<ConduitType, CargoBay.CargoType> ElementToCargoMap = new Dictionary<ConduitType, CargoBay.CargoType>()
  {
    {
      ConduitType.Solid,
      CargoBay.CargoType.Solids
    },
    {
      ConduitType.Liquid,
      CargoBay.CargoType.Liquids
    },
    {
      ConduitType.Gas,
      CargoBay.CargoType.Gasses
    }
  };
  private static readonly EventSystem.IntraObjectHandler<CargoBayConduit> OnLaunchDelegate = new EventSystem.IntraObjectHandler<CargoBayConduit>((Action<CargoBayConduit, object>) ((component, data) => component.OnLaunch(data)));
  private static readonly EventSystem.IntraObjectHandler<CargoBayConduit> OnLandDelegate = new EventSystem.IntraObjectHandler<CargoBayConduit>((Action<CargoBayConduit, object>) ((component, data) => component.OnLand(data)));
  private static StatusItem connectedPortStatus;
  private static StatusItem connectedWrongPortStatus;
  private static StatusItem connectedNoPortStatus;
  private CargoBay.CargoType storageType;
  private Guid connectedConduitPortStatusItem;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (CargoBayConduit.connectedPortStatus == null)
    {
      CargoBayConduit.connectedPortStatus = new StatusItem("CONNECTED_ROCKET_PORT", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID);
      CargoBayConduit.connectedWrongPortStatus = new StatusItem("CONNECTED_ROCKET_WRONG_PORT", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, true, OverlayModes.None.ID);
      CargoBayConduit.connectedNoPortStatus = new StatusItem("CONNECTED_ROCKET_NO_PORT", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.Bad, true, OverlayModes.None.ID);
    }
    if ((UnityEngine.Object) this.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad != (UnityEngine.Object) null)
    {
      this.OnLaunchpadChainChanged((object) null);
      this.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.Subscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
    }
    this.Subscribe<CargoBayConduit>(-1277991738, CargoBayConduit.OnLaunchDelegate);
    this.Subscribe<CargoBayConduit>(-887025858, CargoBayConduit.OnLandDelegate);
    this.storageType = this.GetComponent<CargoBay>().storageType;
    this.UpdateStatusItems();
  }

  protected override void OnCleanUp()
  {
    LaunchPad currentPad = this.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
    if ((UnityEngine.Object) currentPad != (UnityEngine.Object) null)
      currentPad.Unsubscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
    base.OnCleanUp();
  }

  public void OnLaunch(object data)
  {
    ConduitDispenser component = this.GetComponent<ConduitDispenser>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.conduitType = ConduitType.None;
    this.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.Unsubscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
  }

  public void OnLand(object data)
  {
    ConduitDispenser component = this.GetComponent<ConduitDispenser>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      switch (this.storageType)
      {
        case CargoBay.CargoType.Liquids:
          component.conduitType = ConduitType.Liquid;
          break;
        case CargoBay.CargoType.Gasses:
          component.conduitType = ConduitType.Gas;
          break;
        default:
          component.conduitType = ConduitType.None;
          break;
      }
    }
    this.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad.Subscribe(-1009905786, new Action<object>(this.OnLaunchpadChainChanged));
    this.UpdateStatusItems();
  }

  private void OnLaunchpadChainChanged(object data) => this.UpdateStatusItems();

  private void UpdateStatusItems()
  {
    bool hasMatch;
    bool hasAny;
    this.HasMatchingConduitPort(out hasMatch, out hasAny);
    KSelectable component = this.GetComponent<KSelectable>();
    if (hasMatch)
      this.connectedConduitPortStatusItem = component.ReplaceStatusItem(this.connectedConduitPortStatusItem, CargoBayConduit.connectedPortStatus, (object) this);
    else if (hasAny)
      this.connectedConduitPortStatusItem = component.ReplaceStatusItem(this.connectedConduitPortStatusItem, CargoBayConduit.connectedWrongPortStatus, (object) this);
    else
      this.connectedConduitPortStatusItem = component.ReplaceStatusItem(this.connectedConduitPortStatusItem, CargoBayConduit.connectedNoPortStatus, (object) this);
  }

  private void HasMatchingConduitPort(out bool hasMatch, out bool hasAny)
  {
    hasMatch = false;
    hasAny = false;
    LaunchPad currentPad = this.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
    if ((UnityEngine.Object) currentPad == (UnityEngine.Object) null)
      return;
    ChainedBuilding.StatesInstance smi = currentPad.GetSMI<ChainedBuilding.StatesInstance>();
    if (smi == null)
      return;
    HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet chain = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
    smi.GetLinkedBuildings(ref chain);
    foreach (StateMachine.Instance instance in (HashSet<ChainedBuilding.StatesInstance>) chain)
    {
      IConduitDispenser component = instance.GetComponent<IConduitDispenser>();
      if (component != null)
      {
        hasAny = true;
        if (CargoBayConduit.ElementToCargoMap[component.ConduitType] == this.storageType)
        {
          hasMatch = true;
          break;
        }
      }
    }
    chain.Recycle();
  }
}
