// Decompiled with JetBrains decompiler
// Type: KleiPermitDioramaVis_WiresAndAutomation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KleiPermitDioramaVis_WiresAndAutomation : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
  [SerializeField]
  private Image itemSprite;
  private bool itemSpriteDidInit;
  private Vector2 itemSpritePosStart;
  private Vector2 itemSpritePosEnd;

  public GameObject GetGameObject() => this.gameObject;

  public void ConfigureSetup()
  {
  }

  public void ConfigureWith(PermitResource permit)
  {
    this.itemSprite.sprite = permit.GetPermitPresentationInfo().sprite;
    if (!this.itemSpriteDidInit)
    {
      this.itemSpriteDidInit = true;
      this.itemSpritePosStart = this.itemSprite.rectTransform.anchoredPosition + new Vector2(0.0f, 16f);
      this.itemSpritePosEnd = this.itemSprite.rectTransform.anchoredPosition;
    }
    this.itemSprite.StartCoroutine((IEnumerator) Updater.Parallel(Updater.Ease((Action<float>) (alpha => this.itemSprite.color = new Color(1f, 1f, 1f, alpha)), 0.0f, 1f, 0.2f, Easing.SmoothStep, 0.1f), Updater.Ease((Action<Vector2>) (position => this.itemSprite.rectTransform.anchoredPosition = position), this.itemSpritePosStart, this.itemSpritePosEnd, 0.2f, Easing.SmoothStep, 0.1f)));
  }
}
