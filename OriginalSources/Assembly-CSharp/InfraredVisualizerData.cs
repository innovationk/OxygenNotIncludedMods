// Decompiled with JetBrains decompiler
// Type: InfraredVisualizerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public struct InfraredVisualizerData(GameObject go)
{
  public KAnimControllerBase controller = (KAnimControllerBase) null;
  public AmountInstance temperatureAmount = (AmountInstance) null;
  public HandleVector<int>.Handle structureTemperature = HandleVector<int>.InvalidHandle;
  public PrimaryElement primaryElement = (PrimaryElement) null;
  public TemperatureVulnerable temperatureVulnerable = (TemperatureVulnerable) null;
  public CritterTemperatureMonitor.Instance critterTemperatureMonitorInstance = (CritterTemperatureMonitor.Instance) null;

  public void Update()
  {
  }
}
