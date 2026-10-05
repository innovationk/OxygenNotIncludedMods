// Decompiled with JetBrains decompiler
// Type: Database.PermitCategories
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;

#nullable disable
namespace Database;

public static class PermitCategories
{
  private static Dictionary<PermitCategory, PermitCategories.CategoryInfo> CategoryInfos = new Dictionary<PermitCategory, PermitCategories.CategoryInfo>()
  {
    {
      PermitCategory.Equipment,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.EQUIPMENT, "icon_inventory_equipment", (Option<ClothingOutfitUtility.OutfitType>) Option.None)
    },
    {
      PermitCategory.DupeTops,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_TOPS, "icon_inventory_tops", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.Clothing)
    },
    {
      PermitCategory.DupeBottoms,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_BOTTOMS, "icon_inventory_bottoms", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.Clothing)
    },
    {
      PermitCategory.DupeGloves,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_GLOVES, "icon_inventory_gloves", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.Clothing)
    },
    {
      PermitCategory.DupeShoes,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_SHOES, "icon_inventory_shoes", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.Clothing)
    },
    {
      PermitCategory.DupeHats,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_HATS, "icon_inventory_hats", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.Clothing)
    },
    {
      PermitCategory.DupeAccessories,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_ACCESSORIES, "icon_inventory_accessories", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.Clothing)
    },
    {
      PermitCategory.AtmoSuitHelmet,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_HELMET, "icon_inventory_atmosuit_helmet", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.AtmoSuit)
    },
    {
      PermitCategory.AtmoSuitBody,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_BODY, "icon_inventory_atmosuit_body", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.AtmoSuit)
    },
    {
      PermitCategory.AtmoSuitGloves,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_GLOVES, "icon_inventory_atmosuit_gloves", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.AtmoSuit)
    },
    {
      PermitCategory.AtmoSuitBelt,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_BELT, "icon_inventory_atmosuit_belt", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.AtmoSuit)
    },
    {
      PermitCategory.AtmoSuitShoes,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_SHOES, "icon_inventory_atmosuit_boots", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.AtmoSuit)
    },
    {
      PermitCategory.Building,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.BUILDINGS, "icon_inventory_buildings", (Option<ClothingOutfitUtility.OutfitType>) Option.None)
    },
    {
      PermitCategory.Critter,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.CRITTERS, "icon_inventory_critters", (Option<ClothingOutfitUtility.OutfitType>) Option.None)
    },
    {
      PermitCategory.Sweepy,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.SWEEPYS, "icon_inventory_sweepys", (Option<ClothingOutfitUtility.OutfitType>) Option.None)
    },
    {
      PermitCategory.Duplicant,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPLICANTS, "icon_inventory_duplicants", (Option<ClothingOutfitUtility.OutfitType>) Option.None)
    },
    {
      PermitCategory.Artwork,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ARTWORKS, "icon_inventory_artworks", (Option<ClothingOutfitUtility.OutfitType>) Option.None)
    },
    {
      PermitCategory.JoyResponse,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSE, "icon_inventory_joyresponses", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.JoyResponse)
    },
    {
      PermitCategory.JetSuitHelmet,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JET_SUIT_HELMET, "icon_inventory_jetsuit_helmet", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.JetSuit)
    },
    {
      PermitCategory.JetSuitBody,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JET_SUIT_BODY, "icon_inventory_jetsuit_body", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.JetSuit)
    },
    {
      PermitCategory.JetSuitGloves,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JET_SUIT_GLOVES, "icon_inventory_jetsuit_gloves", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.JetSuit)
    },
    {
      PermitCategory.JetSuitShoes,
      new PermitCategories.CategoryInfo((string) UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JET_SUIT_SHOES, "icon_inventory_jetsuit_boots", (Option<ClothingOutfitUtility.OutfitType>) ClothingOutfitUtility.OutfitType.JetSuit)
    }
  };

  public static string GetDisplayName(PermitCategory category)
  {
    return PermitCategories.CategoryInfos[category].displayName;
  }

  public static string GetUppercaseDisplayName(PermitCategory category)
  {
    return PermitCategories.CategoryInfos[category].displayName.ToUpper();
  }

  public static string GetIconName(PermitCategory category)
  {
    return PermitCategories.CategoryInfos[category].iconName;
  }

  public static PermitCategory GetCategoryForId(string id)
  {
    try
    {
      return (PermitCategory) Enum.Parse(typeof (PermitCategory), id);
    }
    catch (ArgumentException ex)
    {
      Debug.LogError((object) (id + " is not a valid PermitCategory."));
    }
    return PermitCategory.Equipment;
  }

  public static Option<ClothingOutfitUtility.OutfitType> GetOutfitTypeFor(
    PermitCategory permitCategory)
  {
    return PermitCategories.CategoryInfos[permitCategory].outfitType;
  }

  private class CategoryInfo
  {
    public string displayName;
    public string iconName;
    public Option<ClothingOutfitUtility.OutfitType> outfitType;

    public CategoryInfo(
      string displayName,
      string iconName,
      Option<ClothingOutfitUtility.OutfitType> outfitType)
    {
      this.displayName = displayName;
      this.iconName = iconName;
      this.outfitType = outfitType;
    }
  }
}
