// Decompiled with JetBrains decompiler
// Type: DialogPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class DialogPanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
  public bool destroyOnDeselect = true;

  public void OnDeselect(BaseEventData eventData)
  {
    if (this.destroyOnDeselect)
    {
      foreach (Component component in this.transform)
        Util.KDestroyGameObject(component.gameObject);
    }
    this.gameObject.SetActive(false);
  }
}
