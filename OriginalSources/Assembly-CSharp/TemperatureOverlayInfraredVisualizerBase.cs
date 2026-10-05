// Decompiled with JetBrains decompiler
// Type: TemperatureOverlayInfraredVisualizerBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public abstract class TemperatureOverlayInfraredVisualizerBase : KMonoBehaviour
{
  private int updateCBHandle = -1;
  private int clearCBHandle = -1;
  private static Action<object, object> OverlayInfraredUpdateDispatcher = (Action<object, object>) ((context, data) => Unsafe.As<TemperatureOverlayInfraredVisualizerBase>(context).OnTemperatureOverlayInfraredUpdate(data));
  private static Action<object, object> OverlayIfraredClearDispatcher = (Action<object, object>) ((context, data) => Unsafe.As<TemperatureOverlayInfraredVisualizerBase>(context).TemperatureOverlayInfraredClear());

  protected override void OnPrefabInit()
  {
    this.updateCBHandle = Game.Instance.Subscribe(-880408538, TemperatureOverlayInfraredVisualizerBase.OverlayInfraredUpdateDispatcher, (object) this);
    this.clearCBHandle = Game.Instance.Subscribe(972756592, TemperatureOverlayInfraredVisualizerBase.OverlayIfraredClearDispatcher, (object) this);
    base.OnPrefabInit();
  }

  protected override void OnCleanUp()
  {
    Game.Instance.Unsubscribe(ref this.updateCBHandle);
    Game.Instance.Unsubscribe(ref this.clearCBHandle);
    base.OnCleanUp();
  }

  private void OnTemperatureOverlayInfraredUpdate(object obj)
  {
    Infrared.TemperatureOverlayInfraredData data = (Infrared.TemperatureOverlayInfraredData) obj;
    Vector3 position = this.transform.GetPosition();
    if (!(data.bounds.Min <= (Vector2) position) || !((Vector2) position <= data.bounds.Max))
      return;
    this.TemperatureOverlayInfraredUpdate(data);
  }

  protected abstract void TemperatureOverlayInfraredUpdate(
    Infrared.TemperatureOverlayInfraredData data);

  protected abstract void TemperatureOverlayInfraredClear();
}
