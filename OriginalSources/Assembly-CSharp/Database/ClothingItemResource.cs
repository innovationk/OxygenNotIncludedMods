// Decompiled with JetBrains decompiler
// Type: Database.ClothingItemResource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
namespace Database;

public class ClothingItemResource : PermitResource
{
  public string animFilename { get; private set; }

  public KAnimFile AnimFile { get; private set; }

  public ClothingOutfitUtility.OutfitType outfitType { get; private set; }

  public ClothingItemResource(
    string id,
    string name,
    string desc,
    ClothingOutfitUtility.OutfitType outfitType,
    PermitCategory category,
    PermitRarity rarity,
    string animFile,
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null)
    : base(id, name, desc, category, rarity, requiredDlcIds, forbiddenDlcIds)
  {
    this.AnimFile = Assets.GetAnim((HashedString) animFile);
    this.animFilename = animFile;
    this.outfitType = outfitType;
  }

  public override PermitPresentationInfo GetPermitPresentationInfo()
  {
    PermitPresentationInfo presentationInfo = new PermitPresentationInfo();
    if ((Object) this.AnimFile == (Object) null)
      Debug.LogError((object) ("Clothing kanim is missing from bundle: " + this.animFilename));
    presentationInfo.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile);
    presentationInfo.SetFacadeForText((string) UI.KLEI_INVENTORY_SCREEN.CLOTHING_ITEM_FACADE_FOR);
    return presentationInfo;
  }
}
