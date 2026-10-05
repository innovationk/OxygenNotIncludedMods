// Decompiled with JetBrains decompiler
// Type: ImageAspectRatioFitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
[ExecuteAlways]
[RequireComponent(typeof (RectTransform))]
[DisallowMultipleComponent]
public class ImageAspectRatioFitter : AspectRatioFitter
{
  [SerializeField]
  private Image targetImage;

  private void UpdateAspectRatio()
  {
    if ((Object) this.targetImage != (Object) null && (Object) this.targetImage.sprite != (Object) null)
      this.aspectRatio = this.targetImage.sprite.rect.width / this.targetImage.sprite.rect.height;
    else
      this.aspectRatio = 1f;
  }

  protected override void OnTransformParentChanged()
  {
    this.UpdateAspectRatio();
    base.OnTransformParentChanged();
  }

  protected override void OnRectTransformDimensionsChange()
  {
    this.UpdateAspectRatio();
    base.OnRectTransformDimensionsChange();
  }
}
