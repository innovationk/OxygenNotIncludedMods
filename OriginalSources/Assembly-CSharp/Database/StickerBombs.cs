// Decompiled with JetBrains decompiler
// Type: Database.StickerBombs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Database;

public class StickerBombs : ResourceSet<DbStickerBomb>
{
  public StickerBombs(ResourceSet parent)
    : base(nameof (StickerBombs), parent)
  {
    foreach (StickerBombFacadeInfo stickerBombFacade in Blueprints.Get().all.stickerBombFacades)
      this.Add(stickerBombFacade.id, stickerBombFacade.name, stickerBombFacade.desc, stickerBombFacade.rarity, stickerBombFacade.animFile, stickerBombFacade.sticker, stickerBombFacade.requiredDlcIds, stickerBombFacade.GetForbiddenDlcIds());
  }

  private DbStickerBomb Add(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string animfilename,
    string symbolName,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds)
  {
    DbStickerBomb dbStickerBomb = new DbStickerBomb(id, name, desc, rarity, animfilename, symbolName, requiredDlcIds, forbiddenDlcIds);
    this.resources.Add(dbStickerBomb);
    return dbStickerBomb;
  }

  public DbStickerBomb GetRandomSticker() => this.resources.GetRandom<DbStickerBomb>();
}
