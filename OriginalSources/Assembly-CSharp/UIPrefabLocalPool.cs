// Decompiled with JetBrains decompiler
// Type: UIPrefabLocalPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class UIPrefabLocalPool
{
  public readonly GameObject sourcePrefab;
  public readonly GameObject parent;
  private Dictionary<int, GameObject> checkedOutInstances = new Dictionary<int, GameObject>();
  private Dictionary<int, GameObject> availableInstances = new Dictionary<int, GameObject>();

  public UIPrefabLocalPool(GameObject sourcePrefab, GameObject parent)
  {
    this.sourcePrefab = sourcePrefab;
    this.parent = parent;
  }

  public GameObject Borrow()
  {
    GameObject gameObject;
    if (this.availableInstances.Count == 0)
    {
      gameObject = Util.KInstantiateUI(this.sourcePrefab, this.parent, true);
    }
    else
    {
      gameObject = this.availableInstances.First<KeyValuePair<int, GameObject>>().Value;
      this.availableInstances.Remove(gameObject.GetInstanceID());
    }
    this.checkedOutInstances.Add(gameObject.GetInstanceID(), gameObject);
    gameObject.SetActive(true);
    gameObject.transform.SetAsLastSibling();
    return gameObject;
  }

  public void Return(GameObject instance)
  {
    this.checkedOutInstances.Remove(instance.GetInstanceID());
    this.availableInstances.Add(instance.GetInstanceID(), instance);
    instance.SetActive(false);
  }

  public void ReturnAll()
  {
    foreach (KeyValuePair<int, GameObject> checkedOutInstance in this.checkedOutInstances)
    {
      int num;
      GameObject gameObject1;
      checkedOutInstance.Deconstruct(ref num, ref gameObject1);
      int key = num;
      GameObject gameObject2 = gameObject1;
      this.availableInstances.Add(key, gameObject2);
      gameObject2.SetActive(false);
    }
    this.checkedOutInstances.Clear();
  }

  public IEnumerable<GameObject> GetBorrowedObjects()
  {
    return (IEnumerable<GameObject>) this.checkedOutInstances.Values;
  }
}
