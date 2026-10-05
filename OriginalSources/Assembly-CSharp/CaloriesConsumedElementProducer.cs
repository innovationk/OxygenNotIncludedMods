// Decompiled with JetBrains decompiler
// Type: CaloriesConsumedElementProducer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CaloriesConsumedElementProducer : KMonoBehaviour, IGameObjectEffectDescriptor
{
  public SimHashes producedElement;
  public float kgProducedPerKcalConsumed = 1f;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    CaloriesConsumedSecondaryExcretionMonitor.Instance instance = new CaloriesConsumedSecondaryExcretionMonitor.Instance((IStateMachineTarget) this.gameObject.GetComponent<StateMachineController>());
    instance.sm.producedElement = this.producedElement;
    instance.sm.kgProducedPerKcalConsumed = this.kgProducedPerKcalConsumed;
    instance.StartSM();
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor(UI.BUILDINGEFFECTS.DIET_ADDITIONAL_PRODUCED.Replace("{Items}", ElementLoader.GetElement(this.producedElement.CreateTag()).name), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_ADDITIONAL_PRODUCED.Replace("{Items}", ElementLoader.GetElement(this.producedElement.CreateTag()).name))
    };
  }
}
