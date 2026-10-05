// Decompiled with JetBrains decompiler
// Type: Database.BalloonOverrideSymbolIter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Database;

public class BalloonOverrideSymbolIter
{
  public readonly Option<BalloonArtistFacadeResource> facade;
  private BalloonOverrideSymbol current;
  private int index;

  public BalloonOverrideSymbolIter(Option<BalloonArtistFacadeResource> facade)
  {
    Debug.Assert(facade.IsNone() || facade.Unwrap().balloonOverrideSymbolIDs.Length != 0);
    this.facade = facade;
    if (facade.IsSome())
      this.index = Random.Range(0, facade.Unwrap().balloonOverrideSymbolIDs.Length);
    this.Next();
  }

  public BalloonOverrideSymbol Current() => this.current;

  public BalloonOverrideSymbol Next()
  {
    if (!this.facade.IsSome())
      return new BalloonOverrideSymbol();
    BalloonArtistFacadeResource artistFacadeResource = this.facade.Unwrap();
    this.current = new BalloonOverrideSymbol(artistFacadeResource.animFilename, artistFacadeResource.balloonOverrideSymbolIDs[this.index]);
    this.index = (this.index + 1) % artistFacadeResource.balloonOverrideSymbolIDs.Length;
    return this.current;
  }
}
