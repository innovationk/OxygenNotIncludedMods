// Decompiled with JetBrains decompiler
// Type: InfraredTemperatureAmount
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public class InfraredTemperatureAmount : TemperatureOverlayInfraredVisualizerBase
{
  private AmountInstance temperatureAmount;
  private KBatchedAnimController controller;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.temperatureAmount = Db.Get().Amounts.Temperature.Lookup(this.gameObject);
    this.controller = this.GetComponent<KBatchedAnimController>();
  }

  protected override void TemperatureOverlayInfraredUpdate(
    Infrared.TemperatureOverlayInfraredData data)
  {
    if (this.temperatureAmount == null || !((Object) this.controller != (Object) null))
      return;
    float actualTemperature = this.temperatureAmount.value;
    this.controller.OverlayColour = (Color) (Color32) SimDebugView.Instance.NormalizedTemperature(actualTemperature);
  }

  protected override void TemperatureOverlayInfraredClear()
  {
    if (!((Object) this.controller != (Object) null))
      return;
    this.controller.OverlayColour = Color.black;
  }
}
