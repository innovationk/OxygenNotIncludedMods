// Decompiled with JetBrains decompiler
// Type: Klei.AI.ModifierGroup`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Klei.AI;

public class ModifierGroup<T>(string id, string name) : Resource(id, name)
{
  public List<T> modifiers = new List<T>();

  public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this.modifiers.GetEnumerator();

  public T this[int idx] => this.modifiers[idx];

  public int Count => this.modifiers.Count;

  public void Add(T modifier) => this.modifiers.Add(modifier);
}
