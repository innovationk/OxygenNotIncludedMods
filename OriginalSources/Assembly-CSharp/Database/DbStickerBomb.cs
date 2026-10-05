// Decompiled with JetBrains decompiler
// Type: Database.DbStickerBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Database;

public class DbStickerBomb : PermitResource
{
  public string id;
  public string sticker;
  public KAnimFile animFile;
  private const string stickerAnimPrefix = "idle_sticker";
  private const string stickerSymbolPrefix = "sticker";

  public DbStickerBomb(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string animfilename,
    string sticker,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds)
    : base(id, name, desc, PermitCategory.Artwork, rarity, requiredDlcIds, forbiddenDlcIds)
  {
    this.id = id;
    this.sticker = sticker;
    this.animFile = Assets.GetAnim((HashedString) animfilename);
  }

  public override PermitPresentationInfo GetPermitPresentationInfo()
  {
    return new PermitPresentationInfo()
    {
      sprite = Def.GetUISpriteFromMultiObjectAnim(this.animFile, $"{"idle_sticker"}_{this.sticker}", symbolName: $"{"sticker"}_{this.sticker}")
    };
  }
}
