// Decompiled with JetBrains decompiler
// Type: InfraredPrimaryElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class InfraredPrimaryElement : TemperatureOverlayInfraredVisualizerBase
{
  private PrimaryElement primaryElement;
  private KBatchedAnimController controller;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.primaryElement = this.GetComponent<PrimaryElement>();
    this.controller = this.GetComponent<KBatchedAnimController>();
  }

  protected override void TemperatureOverlayInfraredUpdate(
    Infrared.TemperatureOverlayInfraredData data)
  {
    if (!((Object) this.primaryElement != (Object) null) || !((Object) this.controller != (Object) null))
      return;
    float temperature = this.primaryElement.Temperature;
    this.controller.OverlayColour = (Color) (Color32) SimDebugView.Instance.NormalizedTemperature(temperature);
  }

  protected override void TemperatureOverlayInfraredClear()
  {
    if (!((Object) this.controller != (Object) null))
      return;
    this.controller.OverlayColour = Color.black;
  }
}
