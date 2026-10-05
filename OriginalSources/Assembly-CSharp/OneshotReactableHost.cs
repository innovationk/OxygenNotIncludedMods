// Decompiled with JetBrains decompiler
// Type: OneshotReactableHost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/OneshotReactableHost")]
public class OneshotReactableHost : KMonoBehaviour
{
  private Reactable reactable;
  public float lifetime = 1f;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    GameScheduler.Instance.Schedule("CleanupOneshotReactable", this.lifetime, new Action<object>(this.OnExpire), (object) null, (SchedulerGroup) null);
  }

  public void SetReactable(Reactable reactable) => this.reactable = reactable;

  private void OnExpire(object obj)
  {
    if (!this.reactable.IsReacting)
    {
      this.reactable.Cleanup();
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
      GameScheduler.Instance.Schedule("CleanupOneshotReactable", 0.5f, new Action<object>(this.OnExpire), (object) null, (SchedulerGroup) null);
  }
}
