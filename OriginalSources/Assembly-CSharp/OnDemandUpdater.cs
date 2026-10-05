// Decompiled with JetBrains decompiler
// Type: OnDemandUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OnDemandUpdater : MonoBehaviour
{
  private List<IUpdateOnDemand> Updaters = new List<IUpdateOnDemand>();
  public static OnDemandUpdater Instance;

  public static void DestroyInstance() => OnDemandUpdater.Instance = (OnDemandUpdater) null;

  private void Awake() => OnDemandUpdater.Instance = this;

  public void Register(IUpdateOnDemand updater)
  {
    if (this.Updaters.Contains(updater))
      return;
    this.Updaters.Add(updater);
  }

  public void Unregister(IUpdateOnDemand updater)
  {
    if (!this.Updaters.Contains(updater))
      return;
    this.Updaters.Remove(updater);
  }

  private void Update()
  {
    for (int index = 0; index < this.Updaters.Count; ++index)
    {
      if (this.Updaters[index] != null)
        this.Updaters[index].UpdateOnDemand();
    }
  }
}
