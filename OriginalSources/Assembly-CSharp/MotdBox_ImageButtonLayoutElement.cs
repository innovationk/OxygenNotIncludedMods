// Decompiled with JetBrains decompiler
// Type: MotdBox_ImageButtonLayoutElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MotdBox_ImageButtonLayoutElement : LayoutElement
{
  [SerializeField]
  private float heightToWidthRatio;
  [SerializeField]
  private MotdBox_ImageButtonLayoutElement.Style style;

  private void UpdateState()
  {
    switch (this.style)
    {
      case MotdBox_ImageButtonLayoutElement.Style.WidthExpandsBasedOnHeight:
        this.flexibleHeight = 1f;
        this.preferredHeight = -1f;
        this.minHeight = -1f;
        this.flexibleWidth = 0.0f;
        this.preferredWidth = this.rectTransform().sizeDelta.y * this.heightToWidthRatio;
        this.minWidth = this.preferredWidth;
        this.ignoreLayout = false;
        break;
      case MotdBox_ImageButtonLayoutElement.Style.HeightExpandsBasedOnWidth:
        this.flexibleWidth = 1f;
        this.preferredWidth = -1f;
        this.minWidth = -1f;
        this.flexibleHeight = 0.0f;
        this.preferredHeight = this.rectTransform().sizeDelta.x / this.heightToWidthRatio;
        this.minHeight = this.preferredHeight;
        this.ignoreLayout = false;
        break;
    }
  }

  protected override void OnTransformParentChanged()
  {
    this.UpdateState();
    base.OnTransformParentChanged();
  }

  protected override void OnRectTransformDimensionsChange()
  {
    this.UpdateState();
    base.OnRectTransformDimensionsChange();
  }

  private enum Style
  {
    WidthExpandsBasedOnHeight,
    HeightExpandsBasedOnWidth,
  }
}
