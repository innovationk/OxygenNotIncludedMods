// Decompiled with JetBrains decompiler
// Type: SimulatedTemperatureAdjuster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SimulatedTemperatureAdjuster
{
  private float temperature;
  private float heatCapacity;
  private float thermalConductivity;
  private bool active;
  private Storage storage;

  public SimulatedTemperatureAdjuster(
    float simulated_temperature,
    float heat_capacity,
    float thermal_conductivity,
    Storage storage)
  {
    this.temperature = simulated_temperature;
    this.heatCapacity = heat_capacity;
    this.thermalConductivity = thermal_conductivity;
    this.storage = storage;
    storage.gameObject.Subscribe(824508782, new Action<object>(this.OnActivechanged));
    storage.gameObject.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
    this.OnActivechanged((object) storage.gameObject.GetComponent<Operational>());
  }

  public List<Descriptor> GetDescriptors()
  {
    return SimulatedTemperatureAdjuster.GetDescriptors(this.temperature);
  }

  public static List<Descriptor> GetDescriptors(float temperature)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    string formattedTemperature = GameUtil.GetFormattedTemperature(temperature);
    descriptors.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ITEM_TEMPERATURE_ADJUST, (object) formattedTemperature), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ITEM_TEMPERATURE_ADJUST, (object) formattedTemperature)));
    return descriptors;
  }

  private void Register(SimTemperatureTransfer stt)
  {
    stt.onSimRegistered -= new Action<SimTemperatureTransfer>(this.OnItemSimRegistered);
    stt.onSimRegistered += new Action<SimTemperatureTransfer>(this.OnItemSimRegistered);
    if (!Sim.IsValidHandle(stt.SimHandle))
      return;
    this.OnItemSimRegistered(stt);
  }

  private void Unregister(SimTemperatureTransfer stt)
  {
    stt.onSimRegistered -= new Action<SimTemperatureTransfer>(this.OnItemSimRegistered);
    if (!Sim.IsValidHandle(stt.SimHandle))
      return;
    SimMessages.ModifyElementChunkTemperatureAdjuster(stt.SimHandle, 0.0f, 0.0f, 0.0f);
  }

  private void OnItemSimRegistered(SimTemperatureTransfer stt)
  {
    if ((UnityEngine.Object) stt == (UnityEngine.Object) null || !Sim.IsValidHandle(stt.SimHandle))
      return;
    float temperature = this.temperature;
    float heat_capacity = this.heatCapacity;
    float thermal_conductivity = this.thermalConductivity;
    if (!this.active)
    {
      temperature = 0.0f;
      heat_capacity = 0.0f;
      thermal_conductivity = 0.0f;
    }
    SimMessages.ModifyElementChunkTemperatureAdjuster(stt.SimHandle, temperature, heat_capacity, thermal_conductivity);
  }

  private void OnActivechanged(object data)
  {
    this.active = ((Operational) data).IsActive;
    if (this.active)
    {
      foreach (GameObject gameObject in this.storage.items)
      {
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          this.OnItemSimRegistered(gameObject.GetComponent<SimTemperatureTransfer>());
      }
    }
    else
    {
      foreach (GameObject gameObject in this.storage.items)
      {
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          this.Unregister(gameObject.GetComponent<SimTemperatureTransfer>());
      }
    }
  }

  public void CleanUp()
  {
    this.storage.gameObject.Unsubscribe(-1697596308, new Action<object>(this.OnStorageChanged));
    foreach (GameObject gameObject in this.storage.items)
    {
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        this.Unregister(gameObject.GetComponent<SimTemperatureTransfer>());
    }
  }

  private void OnStorageChanged(object data)
  {
    GameObject gameObject = (GameObject) data;
    SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || (UnityEngine.Object) gameObject.GetComponent<Pickupable>() == (UnityEngine.Object) null)
      return;
    if ((!this.active ? 0 : (this.storage.items.Contains(gameObject) ? 1 : 0)) != 0)
      this.Register(component);
    else
      this.Unregister(component);
  }
}
