// Decompiled with JetBrains decompiler
// Type: StorageMeter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class StorageMeter : KMonoBehaviour
{
  [MyCmpGet]
  private Storage storage;
  private MeterController meter;
  private Func<float, int, float> interpolateFunction = new Func<float, int, float>(MeterController.MinMaxStepLerp);

  public void SetInterpolateFunction(Func<float, int, float> func)
  {
    this.interpolateFunction = func;
    if (this.meter == null)
      return;
    this.meter.interpolateFunction = this.interpolateFunction;
  }

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[3]
    {
      "meter_target",
      "meter_frame",
      "meter_level"
    });
    this.meter.interpolateFunction = this.interpolateFunction;
    this.UpdateMeter((object) null);
    this.Subscribe(-1697596308, new Action<object>(this.UpdateMeter));
  }

  private void UpdateMeter(object data)
  {
    this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
  }
}
