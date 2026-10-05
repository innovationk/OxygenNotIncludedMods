// Decompiled with JetBrains decompiler
// Type: FossilDigsiteLampLight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FossilDigsiteLampLight : Light2D
{
  private static readonly EventSystem.IntraObjectHandler<FossilDigsiteLampLight> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<FossilDigsiteLampLight>((Action<FossilDigsiteLampLight, object>) ((light, data) =>
  {
    if (!light.independent)
      return;
    light.enabled = ((Boxed<bool>) data).value;
  }));

  public bool independent { private set; get; }

  protected override void OnPrefabInit()
  {
    this.Subscribe<FossilDigsiteLampLight>(-592767678, FossilDigsiteLampLight.OnOperationalChangedDelegate);
    this.IntensityAnimation = 1f;
  }

  public void SetIndependentState(bool isIndependent, bool checkOperational = true)
  {
    this.independent = isIndependent;
    Operational component = this.GetComponent<Operational>();
    if (((!((UnityEngine.Object) component != (UnityEngine.Object) null) ? 0 : (this.independent ? 1 : 0)) & (checkOperational ? 1 : 0)) == 0 || this.enabled == component.IsOperational)
      return;
    this.enabled = component.IsOperational;
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    return this.independent || this.enabled ? base.GetDescriptors(go) : new List<Descriptor>();
  }
}
