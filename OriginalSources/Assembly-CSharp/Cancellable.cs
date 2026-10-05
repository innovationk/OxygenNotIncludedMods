// Decompiled with JetBrains decompiler
// Type: Cancellable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Cancellable")]
public class Cancellable : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<Cancellable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Cancellable>((Action<Cancellable, object>) ((component, data) => component.OnCancel(data)));

  protected override void OnPrefabInit()
  {
    this.Subscribe<Cancellable>(2127324410, Cancellable.OnCancelDelegate);
  }

  protected virtual void OnCancel(object _) => this.DeleteObject();
}
