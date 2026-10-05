// Decompiled with JetBrains decompiler
// Type: Database.PermitResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Database;

public class PermitResources : ResourceSet<PermitResource>
{
  public ResourceSet Root;
  public BuildingFacades BuildingFacades;
  public EquippableFacades EquippableFacades;
  public ArtableStages ArtableStages;
  public StickerBombs StickerBombs;
  public ClothingItems ClothingItems;
  public ClothingOutfits ClothingOutfits;
  public MonumentParts MonumentParts;
  public BalloonArtistFacades BalloonArtistFacades;
  public Dictionary<string, IEnumerable<PermitResource>> Permits;

  public PermitResources(ResourceSet parent)
    : base(nameof (PermitResources), parent)
  {
    this.Root = (ResourceSet) new ResourceSet<Resource>(nameof (Root), (ResourceSet) null);
    this.Permits = new Dictionary<string, IEnumerable<PermitResource>>();
    this.BuildingFacades = new BuildingFacades(this.Root);
    this.Permits.Add(this.BuildingFacades.Id, (IEnumerable<PermitResource>) this.BuildingFacades.resources);
    this.EquippableFacades = new EquippableFacades(this.Root);
    this.Permits.Add(this.EquippableFacades.Id, (IEnumerable<PermitResource>) this.EquippableFacades.resources);
    this.ArtableStages = new ArtableStages(this.Root);
    this.Permits.Add(this.ArtableStages.Id, (IEnumerable<PermitResource>) this.ArtableStages.resources);
    this.StickerBombs = new StickerBombs(this.Root);
    this.Permits.Add(this.StickerBombs.Id, (IEnumerable<PermitResource>) this.StickerBombs.resources);
    this.ClothingItems = new ClothingItems(this.Root);
    this.ClothingOutfits = new ClothingOutfits(this.Root, this.ClothingItems);
    this.Permits.Add(this.ClothingItems.Id, (IEnumerable<PermitResource>) this.ClothingItems.resources);
    this.BalloonArtistFacades = new BalloonArtistFacades(this.Root);
    this.Permits.Add(this.BalloonArtistFacades.Id, (IEnumerable<PermitResource>) this.BalloonArtistFacades.resources);
    this.MonumentParts = new MonumentParts(this.Root);
    this.Permits.Add(this.MonumentParts.Id, (IEnumerable<PermitResource>) this.MonumentParts.resources);
    foreach (IEnumerable<PermitResource> collection in this.Permits.Values)
      this.resources.AddRange(collection);
  }

  public void PostProcess() => this.BuildingFacades.PostProcess();
}
