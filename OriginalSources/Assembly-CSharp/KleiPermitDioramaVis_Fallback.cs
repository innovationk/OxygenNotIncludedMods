// Decompiled with JetBrains decompiler
// Type: KleiPermitDioramaVis_Fallback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KleiPermitDioramaVis_Fallback : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
  [SerializeField]
  private Image sprite;
  [SerializeField]
  private RectTransform editorOnlyErrorMessageParent;
  [SerializeField]
  private TextMeshProUGUI editorOnlyErrorMessageText;
  private Option<string> error;

  public GameObject GetGameObject() => this.gameObject;

  public void ConfigureSetup()
  {
  }

  public void ConfigureWith(PermitResource permit)
  {
    this.sprite.sprite = PermitPresentationInfo.GetUnknownSprite();
    this.editorOnlyErrorMessageParent.gameObject.SetActive(false);
  }

  public KleiPermitDioramaVis_Fallback WithError(string error)
  {
    this.error = (Option<string>) error;
    Debug.Log((object) ("[KleiInventoryScreen Error] Had to use fallback vis. " + error));
    return this;
  }
}
