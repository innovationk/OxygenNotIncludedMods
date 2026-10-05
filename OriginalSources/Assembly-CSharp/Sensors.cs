// Decompiled with JetBrains decompiler
// Type: Sensors
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Sensors")]
public class Sensors : KMonoBehaviour
{
  public List<Sensor> sensors = new List<Sensor>();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<Brain>().onPreUpdate += new System.Action(this.OnBrainPreUpdate);
  }

  public SensorType GetSensor<SensorType>() where SensorType : Sensor
  {
    foreach (Sensor sensor in this.sensors)
    {
      if (typeof (SensorType).IsAssignableFrom(sensor.GetType()))
        return (SensorType) sensor;
    }
    Debug.LogError((object) ("Missing sensor of type: " + typeof (SensorType).Name));
    return default (SensorType);
  }

  public void Add(Sensor sensor)
  {
    this.sensors.Add(sensor);
    if (!sensor.IsEnabled)
      return;
    sensor.Update();
  }

  public void UpdateSensors()
  {
    foreach (Sensor sensor in this.sensors)
    {
      if (sensor.IsEnabled)
        sensor.Update();
    }
  }

  private void OnBrainPreUpdate() => this.UpdateSensors();

  public void ShowEditor()
  {
    foreach (Sensor sensor in this.sensors)
      sensor.ShowEditor();
  }
}
