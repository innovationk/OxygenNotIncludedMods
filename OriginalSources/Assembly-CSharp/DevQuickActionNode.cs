// Decompiled with JetBrains decompiler
// Type: DevQuickActionNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

#nullable disable
public class DevQuickActionNode : MonoBehaviour
{
  public TextMeshProUGUI label;
  protected DevQuickActionNode parentNode;
  public Action<DevQuickActionNode> OnRecycle;
  protected System.Action OnNodeInteractedWith;
  protected float space = 100f;

  public RectTransform transform => base.transform as RectTransform;

  public void SetChildrenSeparationSpace(float space) => this.space = space;

  public virtual void Recycle()
  {
    this.parentNode = (DevQuickActionNode) null;
    this.OnNodeInteractedWith = (System.Action) null;
    this.gameObject.SetActive(false);
    Action<DevQuickActionNode> onRecycle = this.OnRecycle;
    if (onRecycle == null)
      return;
    onRecycle(this);
  }
}
