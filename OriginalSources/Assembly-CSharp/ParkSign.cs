// Decompiled with JetBrains decompiler
// Type: ParkSign
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ParkSign")]
public class ParkSign : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<ParkSign> TriggerRoomEffectsDelegate = new EventSystem.IntraObjectHandler<ParkSign>((Action<ParkSign, object>) ((component, data) => component.TriggerRoomEffects(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<ParkSign>(-832141045, ParkSign.TriggerRoomEffectsDelegate);
  }

  private void TriggerRoomEffects(object data)
  {
    GameObject gameObject = (GameObject) data;
    Game.Instance.roomProber.GetRoomOfGameObject(this.gameObject)?.roomType.TriggerRoomEffects(this.gameObject.GetComponent<KPrefabID>(), gameObject.GetComponent<Effects>());
  }
}
