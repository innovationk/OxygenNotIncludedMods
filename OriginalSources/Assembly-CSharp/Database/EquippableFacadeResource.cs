// Decompiled with JetBrains decompiler
// Type: Database.EquippableFacadeResource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Database;

public class EquippableFacadeResource : PermitResource
{
  public string BuildOverride { get; private set; }

  public string DefID { get; private set; }

  public KAnimFile AnimFile { get; private set; }

  public EquippableFacadeResource(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string buildOverride,
    string defID,
    string animFile,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds)
    : base(id, name, desc, PermitCategory.Equipment, rarity, requiredDlcIds, forbiddenDlcIds)
  {
    this.DefID = defID;
    this.BuildOverride = buildOverride;
    this.AnimFile = Assets.GetAnim((HashedString) animFile);
  }

  public Tuple<Sprite, Color> GetUISprite()
  {
    if ((Object) this.AnimFile == (Object) null)
      Debug.LogError((object) ("Facade AnimFile is null: " + this.DefID));
    Sprite fromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile);
    return new Tuple<Sprite, Color>(fromMultiObjectAnim, (Object) fromMultiObjectAnim != (Object) null ? Color.white : Color.clear);
  }

  public override PermitPresentationInfo GetPermitPresentationInfo()
  {
    PermitPresentationInfo presentationInfo = new PermitPresentationInfo();
    presentationInfo.sprite = this.GetUISprite().first;
    GameObject prefab = Assets.TryGetPrefab((Tag) this.DefID);
    if ((Object) prefab == (Object) null || !(bool) (Object) prefab)
      presentationInfo.SetFacadeForPrefabID(this.DefID);
    else
      presentationInfo.SetFacadeForPrefabName(prefab.GetProperName());
    return presentationInfo;
  }
}
