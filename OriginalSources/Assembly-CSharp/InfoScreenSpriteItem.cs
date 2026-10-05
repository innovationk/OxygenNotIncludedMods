// Decompiled with JetBrains decompiler
// Type: InfoScreenSpriteItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenSpriteItem")]
public class InfoScreenSpriteItem : KMonoBehaviour
{
  [SerializeField]
  private Image image;
  [SerializeField]
  private LayoutElement layout;

  public void SetSprite(Sprite sprite)
  {
    this.image.sprite = sprite;
    UnityEngine.Rect rect = sprite.rect;
    double width = (double) rect.width;
    rect = sprite.rect;
    double height = (double) rect.height;
    this.layout.preferredWidth = this.layout.preferredHeight * (float) (width / height);
  }
}
