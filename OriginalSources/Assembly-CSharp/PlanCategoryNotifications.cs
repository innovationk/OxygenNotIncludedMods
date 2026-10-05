// Decompiled with JetBrains decompiler
// Type: PlanCategoryNotifications
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlanCategoryNotifications : MonoBehaviour
{
  public Image AttentionImage;

  public void ToggleAttention(bool active)
  {
    if (!(bool) (Object) this.AttentionImage)
      return;
    this.AttentionImage.gameObject.SetActive(active);
  }
}
