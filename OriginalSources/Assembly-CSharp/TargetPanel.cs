// Decompiled with JetBrains decompiler
// Type: TargetPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public abstract class TargetPanel : KMonoBehaviour
{
  protected GameObject selectedTarget;

  public abstract bool IsValidForTarget(GameObject target);

  public virtual void SetTarget(GameObject target)
  {
    if (!((UnityEngine.Object) this.selectedTarget != (UnityEngine.Object) target))
      return;
    if ((UnityEngine.Object) this.selectedTarget != (UnityEngine.Object) null)
      this.OnDeselectTarget(this.selectedTarget);
    this.selectedTarget = target;
    if (!((UnityEngine.Object) this.selectedTarget != (UnityEngine.Object) null))
      return;
    this.OnSelectTarget(this.selectedTarget);
  }

  protected virtual void OnSelectTarget(GameObject target)
  {
    target.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
  }

  public virtual void OnDeselectTarget(GameObject target)
  {
    target.Unsubscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
  }

  private void OnTargetDestroyed(object data)
  {
    DetailsScreen.Instance.Show(false);
    this.SetTarget((GameObject) null);
  }
}
