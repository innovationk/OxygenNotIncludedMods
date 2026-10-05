// Decompiled with JetBrains decompiler
// Type: BlueprintCollection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BlueprintCollection
{
  public List<ArtableInfo> artables = new List<ArtableInfo>();
  public List<BuildingFacadeInfo> buildingFacades = new List<BuildingFacadeInfo>();
  public List<ClothingItemInfo> clothingItems = new List<ClothingItemInfo>();
  public List<BalloonArtistFacadeInfo> balloonArtistFacades = new List<BalloonArtistFacadeInfo>();
  public List<StickerBombFacadeInfo> stickerBombFacades = new List<StickerBombFacadeInfo>();
  public List<EquippableFacadeInfo> equippableFacades = new List<EquippableFacadeInfo>();
  public List<MonumentPartInfo> monumentParts = new List<MonumentPartInfo>();
  public List<ClothingOutfitResource> outfits = new List<ClothingOutfitResource>();

  public void AddBlueprintsFrom<T>(T provider) where T : BlueprintProvider
  {
    provider.blueprintCollection = this;
    provider.Internal_PreSetupBlueprints();
    provider.SetupBlueprints();
  }

  public void AddBlueprintsFrom(BlueprintCollection collection)
  {
    this.artables.AddRange((IEnumerable<ArtableInfo>) collection.artables);
    this.buildingFacades.AddRange((IEnumerable<BuildingFacadeInfo>) collection.buildingFacades);
    this.clothingItems.AddRange((IEnumerable<ClothingItemInfo>) collection.clothingItems);
    this.balloonArtistFacades.AddRange((IEnumerable<BalloonArtistFacadeInfo>) collection.balloonArtistFacades);
    this.stickerBombFacades.AddRange((IEnumerable<StickerBombFacadeInfo>) collection.stickerBombFacades);
    this.equippableFacades.AddRange((IEnumerable<EquippableFacadeInfo>) collection.equippableFacades);
    this.monumentParts.AddRange((IEnumerable<MonumentPartInfo>) collection.monumentParts);
    this.outfits.AddRange((IEnumerable<ClothingOutfitResource>) collection.outfits);
  }

  public void PostProcess()
  {
    if (!Application.isPlaying)
      return;
    this.artables.RemoveAll(new Predicate<ArtableInfo>(ShouldExcludeBlueprint));
    this.buildingFacades.RemoveAll(new Predicate<BuildingFacadeInfo>(ShouldExcludeBlueprint));
    this.clothingItems.RemoveAll(new Predicate<ClothingItemInfo>(ShouldExcludeBlueprint));
    this.balloonArtistFacades.RemoveAll(new Predicate<BalloonArtistFacadeInfo>(ShouldExcludeBlueprint));
    this.stickerBombFacades.RemoveAll(new Predicate<StickerBombFacadeInfo>(ShouldExcludeBlueprint));
    this.equippableFacades.RemoveAll(new Predicate<EquippableFacadeInfo>(ShouldExcludeBlueprint));
    this.monumentParts.RemoveAll(new Predicate<MonumentPartInfo>(ShouldExcludeBlueprint));
    this.outfits.RemoveAll(new Predicate<ClothingOutfitResource>(ShouldExcludeBlueprint));

    static bool ShouldExcludeBlueprint(IHasDlcRestrictions blueprintDlcInfo)
    {
      if (!DlcManager.IsCorrectDlcSubscribed(blueprintDlcInfo))
        return true;
      if (blueprintDlcInfo is IBlueprintInfo blueprintInfo && !Assets.TryGetAnim((HashedString) blueprintInfo.animFile, out KAnimFile _))
        DebugUtil.DevAssert(false, $"Couldnt find anim \"{blueprintInfo.animFile}\" for blueprint \"{blueprintInfo.id}\"");
      return false;
    }
  }
}
