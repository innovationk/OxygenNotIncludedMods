// Decompiled with JetBrains decompiler
// Type: SolidBooster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SolidBooster : RocketEngine
{
  public Storage fuelStorage;
  private static readonly EventSystem.IntraObjectHandler<SolidBooster> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<SolidBooster>((Action<SolidBooster, object>) ((component, data) => component.OnRocketLanded(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<SolidBooster>(-887025858, SolidBooster.OnRocketLandedDelegate);
  }

  [ContextMenu("Fill Tank")]
  public void FillTank()
  {
    Element element1 = ElementLoader.GetElement(this.fuelTag);
    this.fuelStorage.Store(element1.substance.SpawnResource(this.gameObject.transform.GetPosition(), this.fuelStorage.capacityKg / 2f, element1.defaultValues.temperature, byte.MaxValue, 0));
    Element element2 = ElementLoader.GetElement(GameTags.OxyRock);
    this.fuelStorage.Store(element2.substance.SpawnResource(this.gameObject.transform.GetPosition(), this.fuelStorage.capacityKg / 2f, element2.defaultValues.temperature, byte.MaxValue, 0));
  }

  private void OnRocketLanded(object data)
  {
    if (!((UnityEngine.Object) this.fuelStorage != (UnityEngine.Object) null) || this.fuelStorage.items == null)
      return;
    for (int index = this.fuelStorage.items.Count - 1; index >= 0; --index)
      Util.KDestroyGameObject(this.fuelStorage.items[index]);
    this.fuelStorage.items.Clear();
  }
}
