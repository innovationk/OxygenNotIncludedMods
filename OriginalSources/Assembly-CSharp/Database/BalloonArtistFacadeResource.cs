// Decompiled with JetBrains decompiler
// Type: Database.BalloonArtistFacadeResource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
namespace Database;

public class BalloonArtistFacadeResource : PermitResource
{
  private BalloonArtistFacadeType balloonFacadeType;
  public readonly string[] balloonOverrideSymbolIDs;
  public int nextSymbolIndex;

  public string animFilename { get; private set; }

  public KAnimFile AnimFile { get; private set; }

  public BalloonArtistFacadeResource(
    string id,
    string name,
    string desc,
    PermitRarity rarity,
    string animFile,
    BalloonArtistFacadeType balloonFacadeType,
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null)
    : base(id, name, desc, PermitCategory.JoyResponse, rarity, requiredDlcIds, forbiddenDlcIds)
  {
    this.AnimFile = Assets.GetAnim((HashedString) animFile);
    this.animFilename = animFile;
    this.balloonFacadeType = balloonFacadeType;
    Db.Get().Accessories.AddAccessories(id, this.AnimFile);
    this.balloonOverrideSymbolIDs = this.GetBalloonOverrideSymbolIDs();
    Debug.Assert(this.balloonOverrideSymbolIDs.Length != 0);
  }

  public override PermitPresentationInfo GetPermitPresentationInfo()
  {
    PermitPresentationInfo presentationInfo = new PermitPresentationInfo();
    presentationInfo.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile);
    presentationInfo.SetFacadeForText((string) UI.KLEI_INVENTORY_SCREEN.BALLOON_ARTIST_FACADE_FOR);
    return presentationInfo;
  }

  public BalloonOverrideSymbol GetNextOverride()
  {
    int nextSymbolIndex = this.nextSymbolIndex;
    this.nextSymbolIndex = (this.nextSymbolIndex + 1) % this.balloonOverrideSymbolIDs.Length;
    return new BalloonOverrideSymbol(this.animFilename, this.balloonOverrideSymbolIDs[nextSymbolIndex]);
  }

  public BalloonOverrideSymbolIter GetSymbolIter()
  {
    return new BalloonOverrideSymbolIter((Option<BalloonArtistFacadeResource>) this);
  }

  public BalloonOverrideSymbol GetOverrideAt(int index)
  {
    return new BalloonOverrideSymbol(this.animFilename, this.balloonOverrideSymbolIDs[index]);
  }

  private string[] GetBalloonOverrideSymbolIDs()
  {
    KAnim.Build build = this.AnimFile.GetData().build;
    switch (this.balloonFacadeType)
    {
      case BalloonArtistFacadeType.Single:
        return new string[1]{ "body" };
      case BalloonArtistFacadeType.ThreeSet:
        return new string[3]{ "body1", "body2", "body3" };
      default:
        throw new NotImplementedException();
    }
  }
}
