// Decompiled with JetBrains decompiler
// Type: Database.ArtableStage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
namespace Database;

public class ArtableStage : PermitResource
{
  public string id;
  public string anim;
  public string animFile;
  public string prefabId;
  public string symbolName;
  public int decor;
  public bool cheerOnComplete;
  public ArtableStatusItem statusItem;

  [Obsolete("Use ArtableStage with required/forbidden")]
  public ArtableStage(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string animFile,
    string anim,
    int decor_value,
    bool cheer_on_complete,
    ArtableStatusItem status_item,
    string prefabId,
    string symbolName,
    string[] dlcIds)
    : base(id, name, desc, PermitCategory.Artwork, rarity, (string[]) null, (string[]) null)
  {
    this.id = id;
    this.animFile = animFile;
    this.anim = anim;
    this.symbolName = symbolName;
    this.decor = decor_value;
    this.cheerOnComplete = cheer_on_complete;
    this.statusItem = status_item;
    this.prefabId = prefabId;
  }

  public ArtableStage(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string animFile,
    string anim,
    int decor_value,
    bool cheer_on_complete,
    ArtableStatusItem status_item,
    string prefabId,
    string symbolName,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds)
    : base(id, name, desc, PermitCategory.Artwork, rarity, requiredDlcIds, forbiddenDlcIds)
  {
    this.id = id;
    this.animFile = animFile;
    this.anim = anim;
    this.symbolName = symbolName;
    this.decor = decor_value;
    this.cheerOnComplete = cheer_on_complete;
    this.statusItem = status_item;
    this.prefabId = prefabId;
  }

  public override PermitPresentationInfo GetPermitPresentationInfo()
  {
    PermitPresentationInfo presentationInfo = new PermitPresentationInfo();
    presentationInfo.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim((HashedString) this.animFile));
    presentationInfo.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.ARTABLE_ITEM_FACADE_FOR.Replace("{ConfigProperName}", Assets.GetPrefab((Tag) this.prefabId).GetProperName()).Replace("{ArtableQuality}", this.statusItem.GetName((object) null)));
    return presentationInfo;
  }
}
