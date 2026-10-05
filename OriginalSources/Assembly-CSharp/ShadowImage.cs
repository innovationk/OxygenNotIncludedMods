// Decompiled with JetBrains decompiler
// Type: ShadowImage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ShadowImage : ShadowRect
{
  private Image shadowImage;
  private Image mainImage;

  protected override void MatchRect()
  {
    base.MatchRect();
    if ((Object) this.RectMain == (Object) null || (Object) this.RectShadow == (Object) null)
      return;
    if ((Object) this.shadowImage == (Object) null)
      this.shadowImage = this.RectShadow.GetComponent<Image>();
    if ((Object) this.mainImage == (Object) null)
      this.mainImage = this.RectMain.GetComponent<Image>();
    if ((Object) this.mainImage == (Object) null)
    {
      if (!((Object) this.shadowImage != (Object) null))
        return;
      this.shadowImage.color = Color.clear;
    }
    else
    {
      if ((Object) this.shadowImage == (Object) null)
        return;
      if ((Object) this.shadowImage.sprite != (Object) this.mainImage.sprite)
        this.shadowImage.sprite = this.mainImage.sprite;
      if (!(this.shadowImage.color != this.shadowColor))
        return;
      if ((Object) this.shadowImage.sprite != (Object) null)
        this.shadowImage.color = this.shadowColor;
      else
        this.shadowImage.color = Color.clear;
    }
  }
}
