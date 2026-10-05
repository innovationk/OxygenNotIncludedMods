// Decompiled with JetBrains decompiler
// Type: UIMinionOrMannequinITargetExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
public static class UIMinionOrMannequinITargetExtensions
{
  public static readonly ClothingItemResource[] EMPTY_OUTFIT = new ClothingItemResource[0];

  public static void SetOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitResource outfit)
  {
    self.SetOutfit(outfit.outfitType, ((IEnumerable<string>) outfit.itemsInOutfit).Select<string, ClothingItemResource>((Func<string, ClothingItemResource>) (itemId => Db.Get().Permits.ClothingItems.Get(itemId))));
  }

  public static void SetOutfit(
    this UIMinionOrMannequin.ITarget self,
    OutfitDesignerScreen_OutfitState outfit)
  {
    self.SetOutfit(outfit.outfitType, ((IEnumerable<string>) outfit.GetItems()).Select<string, ClothingItemResource>((Func<string, ClothingItemResource>) (itemId => Db.Get().Permits.ClothingItems.Get(itemId))));
  }

  public static void SetOutfit(this UIMinionOrMannequin.ITarget self, ClothingOutfitTarget outfit)
  {
    self.SetOutfit(outfit.OutfitType, outfit.ReadItemValues());
  }

  public static void SetOutfit(
    this UIMinionOrMannequin.ITarget self,
    ClothingOutfitUtility.OutfitType outfitType,
    Option<ClothingOutfitTarget> outfit)
  {
    if (outfit.HasValue)
      self.SetOutfit(outfit.Value);
    else
      self.ClearOutfit(outfitType);
  }

  public static void ClearOutfit(
    this UIMinionOrMannequin.ITarget self,
    ClothingOutfitUtility.OutfitType outfitType)
  {
    self.SetOutfit(outfitType, (IEnumerable<ClothingItemResource>) UIMinionOrMannequinITargetExtensions.EMPTY_OUTFIT);
  }

  public static void React(this UIMinionOrMannequin.ITarget self)
  {
    self.React(UIMinionOrMannequinReactSource.None);
  }

  public static void ReactToClothingItemChange(
    this UIMinionOrMannequin.ITarget self,
    PermitCategory clothingChangedCategory)
  {
    self.React(GetSource(clothingChangedCategory));

    static UIMinionOrMannequinReactSource GetSource(PermitCategory clothingChangedCategory)
    {
      switch (clothingChangedCategory)
      {
        case PermitCategory.DupeTops:
        case PermitCategory.AtmoSuitBody:
        case PermitCategory.AtmoSuitBelt:
        case PermitCategory.JetSuitBody:
          return UIMinionOrMannequinReactSource.OnTopChanged;
        case PermitCategory.DupeBottoms:
          return UIMinionOrMannequinReactSource.OnBottomChanged;
        case PermitCategory.DupeGloves:
        case PermitCategory.AtmoSuitGloves:
        case PermitCategory.JetSuitGloves:
          return UIMinionOrMannequinReactSource.OnGlovesChanged;
        case PermitCategory.DupeShoes:
        case PermitCategory.AtmoSuitShoes:
        case PermitCategory.JetSuitShoes:
          return UIMinionOrMannequinReactSource.OnShoesChanged;
        case PermitCategory.DupeHats:
        case PermitCategory.AtmoSuitHelmet:
        case PermitCategory.JetSuitHelmet:
          return UIMinionOrMannequinReactSource.OnHatChanged;
        default:
          DebugUtil.DevAssert(false, $"Couldn't find a reaction for \"{clothingChangedCategory}\" clothing item category being changed");
          return UIMinionOrMannequinReactSource.None;
      }
    }
  }

  public static void ReactToPersonalityChange(this UIMinionOrMannequin.ITarget self)
  {
    self.React(UIMinionOrMannequinReactSource.OnPersonalityChanged);
  }

  public static void ReactToFullOutfitChange(this UIMinionOrMannequin.ITarget self)
  {
    self.React(UIMinionOrMannequinReactSource.OnWholeOutfitChanged);
  }

  public static IEnumerable<ClothingItemResource> GetOutfitWithDefaultItems(
    ClothingOutfitUtility.OutfitType outfitType,
    IEnumerable<ClothingItemResource> outfit)
  {
    switch (outfitType)
    {
      case ClothingOutfitUtility.OutfitType.Clothing:
        return outfit;
      case ClothingOutfitUtility.OutfitType.JoyResponse:
        throw new NotSupportedException();
      case ClothingOutfitUtility.OutfitType.AtmoSuit:
        using (DictionaryPool<PermitCategory, ClothingItemResource, UIMinionOrMannequin.ITarget>.PooledDictionary pooledDictionary = PoolsFor<UIMinionOrMannequin.ITarget>.AllocateDict<PermitCategory, ClothingItemResource>())
        {
          foreach (ClothingItemResource clothingItemResource in outfit)
          {
            DebugUtil.DevAssert(!pooledDictionary.ContainsKey(clothingItemResource.Category), "Duplicate item for category");
            pooledDictionary[clothingItemResource.Category] = clothingItemResource;
          }
          if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitHelmet))
            pooledDictionary[PermitCategory.AtmoSuitHelmet] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoHelmetClear");
          if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitBody))
            pooledDictionary[PermitCategory.AtmoSuitBody] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoSuitBasicBlue");
          if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitGloves))
            pooledDictionary[PermitCategory.AtmoSuitGloves] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoGlovesBasicBlue");
          if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitBelt))
            pooledDictionary[PermitCategory.AtmoSuitBelt] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoBeltBasicBlue");
          if (!pooledDictionary.ContainsKey(PermitCategory.AtmoSuitShoes))
            pooledDictionary[PermitCategory.AtmoSuitShoes] = Db.Get().Permits.ClothingItems.Get("visonly_AtmoShoesBasicBlack");
          return (IEnumerable<ClothingItemResource>) pooledDictionary.Values.ToArray<ClothingItemResource>();
        }
      case ClothingOutfitUtility.OutfitType.JetSuit:
        using (DictionaryPool<PermitCategory, ClothingItemResource, UIMinionOrMannequin.ITarget>.PooledDictionary pooledDictionary = PoolsFor<UIMinionOrMannequin.ITarget>.AllocateDict<PermitCategory, ClothingItemResource>())
        {
          foreach (ClothingItemResource clothingItemResource in outfit)
          {
            DebugUtil.DevAssert(!pooledDictionary.ContainsKey(clothingItemResource.Category), "Duplicate item for category");
            pooledDictionary[clothingItemResource.Category] = clothingItemResource;
          }
          if (!pooledDictionary.ContainsKey(PermitCategory.JetSuitHelmet))
            pooledDictionary[PermitCategory.JetSuitHelmet] = Db.Get().Permits.ClothingItems.Get("visonly_JetHelmetClear");
          if (!pooledDictionary.ContainsKey(PermitCategory.JetSuitBody))
            pooledDictionary[PermitCategory.JetSuitBody] = Db.Get().Permits.ClothingItems.Get("visonly_JetSuitBasic");
          if (!pooledDictionary.ContainsKey(PermitCategory.JetSuitGloves))
            pooledDictionary[PermitCategory.JetSuitGloves] = Db.Get().Permits.ClothingItems.Get("visonly_JetGlovesBasic");
          if (!pooledDictionary.ContainsKey(PermitCategory.JetSuitShoes))
            pooledDictionary[PermitCategory.JetSuitShoes] = Db.Get().Permits.ClothingItems.Get("visonly_JetShoesBasic");
          return (IEnumerable<ClothingItemResource>) pooledDictionary.Values.ToArray<ClothingItemResource>();
        }
      default:
        throw new NotImplementedException();
    }
  }
}
