// Decompiled with JetBrains decompiler
// Type: LogicClusterLocationSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicClusterLocationSensor : Switch, ISaveLoadable, ISim200ms
{
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  [Serialize]
  private List<AxialI> activeLocations = new List<AxialI>();
  [Serialize]
  private bool activeInSpace = true;
  private bool wasOn;
  private static readonly EventSystem.IntraObjectHandler<LogicClusterLocationSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicClusterLocationSensor>((Action<LogicClusterLocationSensor, object>) ((component, data) => component.OnCopySettings(data)));

  public bool ActiveInSpace => this.activeInSpace;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicClusterLocationSensor>(-905833192, LogicClusterLocationSensor.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    LogicClusterLocationSensor component = ((GameObject) data).GetComponent<LogicClusterLocationSensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.activeLocations.Clear();
    for (int index = 0; index < component.activeLocations.Count; ++index)
      this.SetLocationEnabled(component.activeLocations[index], true);
    this.activeInSpace = component.activeInSpace;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnToggle += new Action<bool>(this.OnSwitchToggled);
    this.UpdateLogicCircuit();
    this.UpdateVisualState(true);
    this.wasOn = this.switchedOn;
  }

  public void SetLocationEnabled(AxialI location, bool setting)
  {
    if (!setting)
    {
      this.activeLocations.Remove(location);
    }
    else
    {
      if (this.activeLocations.Contains(location))
        return;
      this.activeLocations.Add(location);
    }
  }

  public void SetSpaceEnabled(bool setting) => this.activeInSpace = setting;

  public void Sim200ms(float dt) => this.SetState(this.CheckCurrentLocationSelected());

  private bool CheckCurrentLocationSelected()
  {
    return this.activeLocations.Contains(this.gameObject.GetMyWorldLocation()) || this.activeInSpace && this.CheckInEmptySpace();
  }

  private bool CheckInEmptySpace()
  {
    bool flag = true;
    AxialI myWorldLocation = this.gameObject.GetMyWorldLocation();
    foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
    {
      if (!worldContainer.IsModuleInterior && worldContainer.GetMyWorldLocation() == myWorldLocation)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  public bool CheckLocationSelected(AxialI location) => this.activeLocations.Contains(location);

  private void OnSwitchToggled(bool toggled_on)
  {
    this.UpdateLogicCircuit();
    this.UpdateVisualState();
  }

  private void UpdateLogicCircuit()
  {
    this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
  }

  private void UpdateVisualState(bool force = false)
  {
    if (!(this.wasOn != this.switchedOn | force))
      return;
    this.wasOn = this.switchedOn;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    AxialI myWorldLocation = this.gameObject.GetMyWorldLocation();
    bool flag = true;
    foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
    {
      if (!worldContainer.IsModuleInterior && worldContainer.GetMyWorldLocation() == myWorldLocation)
      {
        flag = false;
        break;
      }
    }
    if (flag)
    {
      component.Play((HashedString) (this.switchedOn ? "on_space_pre" : "on_space_pst"));
      component.Queue((HashedString) (this.switchedOn ? "on_space" : "off_space"));
    }
    else
    {
      component.Play((HashedString) (this.switchedOn ? "on_asteroid_pre" : "on_asteroid_pst"));
      component.Queue((HashedString) (this.switchedOn ? "on_asteroid" : "off_asteroid"));
    }
  }

  protected override void UpdateSwitchStatus()
  {
    StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item);
  }
}
