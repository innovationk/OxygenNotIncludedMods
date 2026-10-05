// Decompiled with JetBrains decompiler
// Type: BlueprintProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;

#nullable disable
public abstract class BlueprintProvider : IHasDlcRestrictions
{
  public BlueprintCollection blueprintCollection;
  private string[] requiredDlcIds;
  private string[] forbiddenDlcIds;

  protected void AddBuilding(
    string prefabConfigId,
    PermitRarity rarity,
    string permitId,
    string animFile)
  {
    this.blueprintCollection.buildingFacades.Add(new BuildingFacadeInfo(permitId, (string) Strings.Get($"STRINGS.BLUEPRINTS.{permitId.ToUpper()}.NAME"), (string) Strings.Get($"STRINGS.BLUEPRINTS.{permitId.ToUpper()}.DESC"), rarity, prefabConfigId, animFile, requiredDlcIds: this.requiredDlcIds, forbiddenDlcIds: this.forbiddenDlcIds));
  }

  protected void AddBuildingWithData(
    string prefabConfigId,
    PermitRarity rarity,
    string permitId,
    string animFile,
    Dictionary<string, string> data)
  {
    this.blueprintCollection.buildingFacades.Add(new BuildingFacadeInfo(permitId, (string) Strings.Get($"STRINGS.BLUEPRINTS.{permitId.ToUpper()}.NAME"), (string) Strings.Get($"STRINGS.BLUEPRINTS.{permitId.ToUpper()}.DESC"), rarity, prefabConfigId, animFile, requiredDlcIds: this.requiredDlcIds, forbiddenDlcIds: this.forbiddenDlcIds, data: data));
  }

  protected void AddBuildingWithInteract(
    string prefabConfigId,
    PermitRarity rarity,
    string permitId,
    string animFile,
    Dictionary<string, string> interact_anim)
  {
    this.blueprintCollection.buildingFacades.Add(new BuildingFacadeInfo(permitId, (string) Strings.Get($"STRINGS.BLUEPRINTS.{permitId.ToUpper()}.NAME"), (string) Strings.Get($"STRINGS.BLUEPRINTS.{permitId.ToUpper()}.DESC"), rarity, prefabConfigId, animFile, interact_anim, this.requiredDlcIds, this.forbiddenDlcIds));
  }

  protected void AddClothing(
    BlueprintProvider.ClothingType clothingType,
    PermitRarity rarity,
    string permitId,
    string animFile)
  {
    this.blueprintCollection.clothingItems.Add(new ClothingItemInfo(permitId, (string) Strings.Get($"STRINGS.BLUEPRINTS.{permitId.ToUpper()}.NAME"), (string) Strings.Get($"STRINGS.BLUEPRINTS.{permitId.ToUpper()}.DESC"), (PermitCategory) clothingType, rarity, animFile, this.requiredDlcIds, this.forbiddenDlcIds));
  }

  protected BlueprintProvider.ArtableInfoAuthoringHelper AddArtable(
    BlueprintProvider.ArtableType artableType,
    PermitRarity rarity,
    string permitId,
    string animFile)
  {
    string prefabId;
    switch (artableType)
    {
      case BlueprintProvider.ArtableType.Painting:
        prefabId = "Canvas";
        break;
      case BlueprintProvider.ArtableType.PaintingTall:
        prefabId = "CanvasTall";
        break;
      case BlueprintProvider.ArtableType.PaintingWide:
        prefabId = "CanvasWide";
        break;
      case BlueprintProvider.ArtableType.Sculpture:
        prefabId = "Sculpture";
        break;
      case BlueprintProvider.ArtableType.SculptureSmall:
        prefabId = "SmallSculpture";
        break;
      case BlueprintProvider.ArtableType.SculptureIce:
        prefabId = "IceSculpture";
        break;
      case BlueprintProvider.ArtableType.SculptureMetal:
        prefabId = "MetalSculpture";
        break;
      case BlueprintProvider.ArtableType.SculptureMarble:
        prefabId = "MarbleSculpture";
        break;
      case BlueprintProvider.ArtableType.SculptureWood:
        prefabId = "WoodSculpture";
        break;
      case BlueprintProvider.ArtableType.FossilSculpture:
        prefabId = "FossilSculpture";
        break;
      case BlueprintProvider.ArtableType.CeilingFossilSculpture:
        prefabId = "CeilingFossilSculpture";
        break;
      default:
        prefabId = (string) null;
        break;
    }
    bool flag = true;
    if (prefabId == null)
    {
      DebugUtil.DevAssert(false, "Failed to get buildingConfigId from " + artableType.ToString());
      flag = false;
    }
    BlueprintProvider.ArtableInfoAuthoringHelper infoAuthoringHelper;
    if (flag)
    {
      KAnimFile anim;
      ArtableInfo artableInfo = new ArtableInfo(permitId, (string) Strings.Get($"STRINGS.BLUEPRINTS.{permitId.ToUpper()}.NAME"), (string) Strings.Get($"STRINGS.BLUEPRINTS.{permitId.ToUpper()}.DESC"), rarity, animFile, !Assets.TryGetAnim((HashedString) animFile, out anim) ? (string) null : anim.GetData().GetAnim(0).name, 0, false, "error", prefabId, requiredDlcIds: this.requiredDlcIds, forbiddenDlcIds: this.forbiddenDlcIds);
      infoAuthoringHelper = new BlueprintProvider.ArtableInfoAuthoringHelper(artableType, artableInfo);
      infoAuthoringHelper.Quality(BlueprintProvider.ArtableQuality.LookingGreat);
      this.blueprintCollection.artables.Add(artableInfo);
    }
    else
      infoAuthoringHelper = new BlueprintProvider.ArtableInfoAuthoringHelper();
    return infoAuthoringHelper;
  }

  protected void AddJoyResponse(
    BlueprintProvider.JoyResponseType joyResponseType,
    PermitRarity rarity,
    string permitId,
    string animFile)
  {
    if (joyResponseType != BlueprintProvider.JoyResponseType.BallonSet)
      throw new NotImplementedException("Missing case for " + joyResponseType.ToString());
    this.blueprintCollection.balloonArtistFacades.Add(new BalloonArtistFacadeInfo(permitId, (string) Strings.Get($"STRINGS.BLUEPRINTS.{permitId.ToUpper()}.NAME"), (string) Strings.Get($"STRINGS.BLUEPRINTS.{permitId.ToUpper()}.DESC"), rarity, animFile, BalloonArtistFacadeType.ThreeSet, this.requiredDlcIds, this.forbiddenDlcIds));
  }

  protected void AddOutfit(
    BlueprintProvider.OutfitType outfitType,
    string outfitId,
    string[] permitIdList)
  {
    this.blueprintCollection.outfits.Add(new ClothingOutfitResource(outfitId, permitIdList, (string) Strings.Get($"STRINGS.BLUEPRINTS.{outfitId.ToUpper()}.NAME"), (ClothingOutfitUtility.OutfitType) outfitType, this.requiredDlcIds, this.forbiddenDlcIds));
  }

  protected void AddMonumentPart(
    BlueprintProvider.MonumentPart part,
    PermitRarity rarity,
    string permitId,
    string animFile)
  {
    string symbolName = "";
    switch (part)
    {
      case BlueprintProvider.MonumentPart.Bottom:
        symbolName = "base";
        break;
      case BlueprintProvider.MonumentPart.Middle:
        symbolName = "mid";
        break;
      case BlueprintProvider.MonumentPart.Top:
        symbolName = "top";
        break;
    }
    this.blueprintCollection.monumentParts.Add(new MonumentPartInfo(permitId, (string) Strings.Get($"STRINGS.BLUEPRINTS.{permitId.ToUpper()}.NAME"), (string) Strings.Get($"STRINGS.BLUEPRINTS.{permitId.ToUpper()}.DESC"), rarity, animFile, permitId.Replace("permit_", ""), symbolName, (MonumentPartResource.Part) part, this.requiredDlcIds, this.forbiddenDlcIds));
  }

  public virtual string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public virtual string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;

  public abstract void SetupBlueprints();

  public void Internal_PreSetupBlueprints()
  {
    this.requiredDlcIds = this.GetRequiredDlcIds();
    this.forbiddenDlcIds = this.GetForbiddenDlcIds();
  }

  public enum ArtableType
  {
    Painting,
    PaintingTall,
    PaintingWide,
    Sculpture,
    SculptureSmall,
    SculptureIce,
    SculptureMetal,
    SculptureMarble,
    SculptureWood,
    FossilSculpture,
    CeilingFossilSculpture,
  }

  public enum ArtableQuality
  {
    LookingGreat,
    LookingOkay,
    LookingUgly,
  }

  public enum ClothingType
  {
    DupeTops = 1,
    DupeBottoms = 2,
    DupeGloves = 3,
    DupeShoes = 4,
    DupeHats = 5,
    DupeAccessories = 6,
    AtmoSuitHelmet = 7,
    AtmoSuitBody = 8,
    AtmoSuitGloves = 9,
    AtmoSuitBelt = 10, // 0x0000000A
    AtmoSuitShoes = 11, // 0x0000000B
    JetSuitHelmet = 18, // 0x00000012
    JetSuitBody = 19, // 0x00000013
    JetSuitGloves = 20, // 0x00000014
    JetSuitShoes = 21, // 0x00000015
  }

  public enum OutfitType
  {
    Clothing = 0,
    AtmoSuit = 2,
    JetSuit = 3,
  }

  public enum JoyResponseType
  {
    BallonSet,
  }

  public enum MonumentPart
  {
    Bottom,
    Middle,
    Top,
  }

  protected readonly ref struct ArtableInfoAuthoringHelper(
    BlueprintProvider.ArtableType artableType,
    ArtableInfo artableInfo)
  {
    private readonly BlueprintProvider.ArtableType artableType = artableType;
    private readonly ArtableInfo artableInfo = artableInfo;

    public void Quality(BlueprintProvider.ArtableQuality artableQuality)
    {
      if (this.artableInfo == null)
        return;
      int num1;
      int num2;
      int num3;
      if (this.artableType == BlueprintProvider.ArtableType.SculptureWood)
      {
        num1 = 4;
        num2 = 8;
        num3 = 12;
      }
      else
      {
        num1 = 5;
        num2 = 10;
        num3 = 15;
      }
      int num4;
      bool flag;
      string str;
      switch (artableQuality)
      {
        case BlueprintProvider.ArtableQuality.LookingGreat:
          num4 = num3;
          flag = true;
          str = "LookingGreat";
          break;
        case BlueprintProvider.ArtableQuality.LookingOkay:
          num4 = num2;
          flag = false;
          str = "LookingOkay";
          break;
        case BlueprintProvider.ArtableQuality.LookingUgly:
          num4 = num1;
          flag = false;
          str = "LookingUgly";
          break;
        default:
          throw new ArgumentException();
      }
      this.artableInfo.decor_value = num4;
      this.artableInfo.cheer_on_complete = flag;
      this.artableInfo.status_id = str;
    }
  }
}
