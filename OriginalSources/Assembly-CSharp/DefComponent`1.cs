// Decompiled with JetBrains decompiler
// Type: DefComponent`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class DefComponent<T> where T : Component
{
  [SerializeField]
  private T cmp;

  public DefComponent(T cmp) => this.cmp = cmp;

  public T Get(StateMachine.Instance smi)
  {
    T[] components = this.cmp.GetComponents<T>();
    int index = 0;
    while (index < components.Length && !((UnityEngine.Object) components[index] == (UnityEngine.Object) this.cmp))
      ++index;
    return smi.gameObject.GetComponents<T>()[index];
  }

  public static implicit operator DefComponent<T>(T cmp) => new DefComponent<T>(cmp);
}
