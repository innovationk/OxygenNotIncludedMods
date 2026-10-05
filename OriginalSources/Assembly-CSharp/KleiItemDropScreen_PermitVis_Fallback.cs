// Decompiled with JetBrains decompiler
// Type: KleiItemDropScreen_PermitVis_Fallback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KleiItemDropScreen_PermitVis_Fallback : KMonoBehaviour
{
  [SerializeField]
  private Image sprite;

  public void ConfigureWith(DropScreenPresentationInfo info) => this.sprite.sprite = info.Sprite;
}
