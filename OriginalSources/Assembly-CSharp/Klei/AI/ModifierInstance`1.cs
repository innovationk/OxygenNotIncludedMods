// Decompiled with JetBrains decompiler
// Type: Klei.AI.ModifierInstance`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Klei.AI;

public class ModifierInstance<ModifierType> : IStateMachineTarget
{
  public ModifierType modifier;

  public GameObject gameObject { get; private set; }

  public ModifierInstance(GameObject game_object, ModifierType modifier)
  {
    this.gameObject = game_object;
    this.modifier = modifier;
  }

  public ComponentType GetComponent<ComponentType>()
  {
    return this.gameObject.GetComponent<ComponentType>();
  }

  public int Subscribe(int hash, Action<object> handler)
  {
    return this.gameObject.GetComponent<KMonoBehaviour>().Subscribe(hash, handler);
  }

  public int Subscribe(int hash, Action<object, object> handler, object context)
  {
    return this.gameObject.GetComponent<KMonoBehaviour>().Subscribe(hash, handler, context);
  }

  public void Unsubscribe(int hash, Action<object> handler)
  {
    this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(hash, handler);
  }

  public void Unsubscribe(int id) => this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(id);

  public void Trigger(int hash, object data = null)
  {
    this.gameObject.GetComponent<KPrefabID>().Trigger(hash, data);
  }

  public Transform transform => this.gameObject.transform;

  public bool isNull => (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null;

  public string name => this.gameObject.name;

  public virtual void OnCleanUp()
  {
  }
}
