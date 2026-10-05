// Decompiled with JetBrains decompiler
// Type: UIPool`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UIPool<T> where T : MonoBehaviour
{
  private T prefab;
  private Stack<T> freeElements;
  private List<T> activeElements;
  public Transform disabledElementParent;

  public int ActiveElementsCount => this.activeElements.Count;

  public int FreeElementsCount => this.freeElements.Count;

  public int TotalElementsCount => this.ActiveElementsCount + this.FreeElementsCount;

  public UIPool(T prefab)
  {
    this.prefab = prefab;
    this.freeElements = new Stack<T>();
    this.activeElements = new List<T>();
  }

  public T GetFreeElement(GameObject instantiateParent = null, bool forceActive = false)
  {
    T freeElement;
    if (this.freeElements.Count == 0)
    {
      freeElement = Util.KInstantiateUI<T>(this.prefab.gameObject, instantiateParent);
    }
    else
    {
      freeElement = this.freeElements.Pop();
      if ((UnityEngine.Object) freeElement.transform.parent != (UnityEngine.Object) instantiateParent)
        freeElement.transform.SetParent(instantiateParent?.transform);
    }
    if (freeElement.gameObject.activeInHierarchy != forceActive)
      freeElement.gameObject.SetActive(forceActive);
    this.activeElements.Add(freeElement);
    return freeElement;
  }

  public void ClearElement(T element)
  {
    if (!this.activeElements.Contains(element))
    {
      Debug.LogError(this.freeElements.Contains(element) ? (object) "The element provided is already inactive" : (object) "The element provided does not belong to this pool");
    }
    else
    {
      if ((UnityEngine.Object) this.disabledElementParent != (UnityEngine.Object) null)
        element.transform.SetParent(this.disabledElementParent);
      element.gameObject.SetActive(false);
      this.freeElements.Push(element);
      this.activeElements.Remove(element);
    }
  }

  public void ClearAll()
  {
    for (int index = this.activeElements.Count - 1; index >= 0; --index)
    {
      T activeElement = this.activeElements[index];
      activeElement.gameObject.SetActive(false);
      if ((UnityEngine.Object) this.disabledElementParent != (UnityEngine.Object) null)
        activeElement.transform.SetParent(this.disabledElementParent);
      this.freeElements.Push(activeElement);
    }
    this.activeElements.Clear();
  }

  public void DestroyAll()
  {
    this.DestroyAllActive();
    this.DestroyAllFree();
  }

  public void DestroyAllActive()
  {
    foreach (T activeElement in this.activeElements)
      UnityEngine.Object.Destroy((UnityEngine.Object) activeElement.gameObject);
    this.activeElements.Clear();
  }

  public void DestroyAllFree()
  {
    foreach (T freeElement in this.freeElements)
      UnityEngine.Object.Destroy((UnityEngine.Object) freeElement.gameObject);
    this.freeElements.Clear();
  }

  public void ForEachActiveElement(Action<T> predicate)
  {
    for (int index = 0; index < this.activeElements.Count; ++index)
      predicate(this.activeElements[index]);
  }

  public void ForEachFreeElement(Action<T> predicate)
  {
    foreach (T freeElement in this.freeElements)
      predicate(freeElement);
  }
}
