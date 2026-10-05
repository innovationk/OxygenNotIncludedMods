// Decompiled with JetBrains decompiler
// Type: Database.BalloonArtistFacades
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Database;

public class BalloonArtistFacades : ResourceSet<BalloonArtistFacadeResource>
{
  public BalloonArtistFacades(ResourceSet parent)
    : base(nameof (BalloonArtistFacades), parent)
  {
    foreach (BalloonArtistFacadeInfo balloonArtistFacade in Blueprints.Get().all.balloonArtistFacades)
      this.Add(balloonArtistFacade.id, balloonArtistFacade.name, balloonArtistFacade.desc, balloonArtistFacade.rarity, balloonArtistFacade.animFile, balloonArtistFacade.balloonFacadeType, balloonArtistFacade.GetRequiredDlcIds(), balloonArtistFacade.GetForbiddenDlcIds());
  }

  [Obsolete("Please use Add(...) with required/forbidden")]
  public void Add(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string animFile,
    BalloonArtistFacadeType balloonFacadeType)
  {
    this.Add(id, name, desc, rarity, animFile, balloonFacadeType, (string[]) null, (string[]) null);
  }

  [Obsolete("Please use Add(...) with required/forbidden")]
  public void Add(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string animFile,
    BalloonArtistFacadeType balloonFacadeType,
    string[] dlcIds)
  {
    this.Add(id, name, desc, rarity, animFile, balloonFacadeType, (string[]) null, (string[]) null);
  }

  public void Add(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string animFile,
    BalloonArtistFacadeType balloonFacadeType,
    string[] requiredDlcIds,
    string[] forbiddenDlcIds)
  {
    this.resources.Add(new BalloonArtistFacadeResource(id, name, desc, rarity, animFile, balloonFacadeType, requiredDlcIds, forbiddenDlcIds));
  }
}
