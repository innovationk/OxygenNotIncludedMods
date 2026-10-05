// Decompiled with JetBrains decompiler
// Type: Reservoir
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Reservoir")]
public class Reservoir : KMonoBehaviour
{
  private MeterController meter;
  [MyCmpGet]
  private Storage storage;
  private static readonly EventSystem.IntraObjectHandler<Reservoir> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Reservoir>((Action<Reservoir, object>) ((component, data) => component.OnStorageChange(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[2]
    {
      "meter_fill",
      "meter_OL"
    });
    this.Subscribe<Reservoir>(-1697596308, Reservoir.OnStorageChangeDelegate);
    this.OnStorageChange((object) null);
  }

  private void OnStorageChange(object data)
  {
    this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
  }
}
