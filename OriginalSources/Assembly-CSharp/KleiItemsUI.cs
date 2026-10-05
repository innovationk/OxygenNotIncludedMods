// Decompiled with JetBrains decompiler
// Type: KleiItemsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using STRINGS;
using UnityEngine;

#nullable disable
public static class KleiItemsUI
{
  public static readonly Color TEXT_COLOR__PERMIT_NOT_OWNED = KleiItemsUI.GetColor("#DD992F");

  public static string WrapAsToolTipTitle(string text) => $"<b><style=\"KLink\">{text}</style></b>";

  public static string WrapWithColor(string text, Color color)
  {
    return $"<color=#{color.ToHexString()}>{text}</color>";
  }

  public static Sprite GetNoneClothingItemIcon(
    PermitCategory category,
    Option<Personality> personality)
  {
    return KleiItemsUI.GetNoneIconForCategory(category, personality);
  }

  public static Sprite GetNoneBalloonArtistIcon()
  {
    return KleiItemsUI.GetNoneIconForCategory(PermitCategory.JoyResponse, (Option<Personality>) (Personality) null);
  }

  private static Sprite GetNoneIconForCategory(
    PermitCategory category,
    Option<Personality> personality)
  {
    return Assets.GetSprite((HashedString) GetIconName(category, personality));

    static string GetIconName(PermitCategory category, Option<Personality> personality)
    {
      switch (category)
      {
        case PermitCategory.DupeTops:
          return "default_no_top";
        case PermitCategory.DupeBottoms:
          return "default_no_bottom";
        case PermitCategory.DupeGloves:
          return "default_no_gloves";
        case PermitCategory.DupeShoes:
          return "default_no_footwear";
        case PermitCategory.DupeHats:
          return "icon_inventory_hats";
        case PermitCategory.DupeAccessories:
          return "icon_inventory_accessories";
        case PermitCategory.AtmoSuitHelmet:
          return "icon_inventory_atmosuit_helmet";
        case PermitCategory.AtmoSuitBody:
          return "icon_inventory_atmosuit_body";
        case PermitCategory.AtmoSuitGloves:
          return "icon_inventory_atmosuit_gloves";
        case PermitCategory.AtmoSuitBelt:
          return "icon_inventory_atmosuit_belt";
        case PermitCategory.AtmoSuitShoes:
          return "icon_inventory_atmosuit_boots";
        case PermitCategory.Building:
          return "icon_inventory_buildings";
        case PermitCategory.Critter:
          return "icon_inventory_critters";
        case PermitCategory.Sweepy:
          return "icon_inventory_sweepys";
        case PermitCategory.Duplicant:
          return "icon_inventory_duplicants";
        case PermitCategory.Artwork:
          return "icon_inventory_artworks";
        case PermitCategory.JoyResponse:
          return "icon_inventory_joyresponses";
        case PermitCategory.JetSuitHelmet:
          return "icon_inventory_jetsuit_helmet";
        case PermitCategory.JetSuitBody:
          return "icon_inventory_jetsuit_body";
        case PermitCategory.JetSuitGloves:
          return "icon_inventory_jetsuit_gloves";
        case PermitCategory.JetSuitShoes:
          return "icon_inventory_jetsuit_boots";
        default:
          return "NoTraits";
      }
    }
  }

  public static string GetNoneOutfitName(ClothingOutfitUtility.OutfitType outfitType)
  {
    switch (outfitType)
    {
      case ClothingOutfitUtility.OutfitType.Clothing:
        return (string) UI.OUTFIT_NAME.NONE;
      case ClothingOutfitUtility.OutfitType.JoyResponse:
        return (string) UI.OUTFIT_NAME.NONE_JOY_RESPONSE;
      case ClothingOutfitUtility.OutfitType.AtmoSuit:
        return (string) UI.OUTFIT_NAME.NONE_ATMO_SUIT;
      case ClothingOutfitUtility.OutfitType.JetSuit:
        return (string) UI.OUTFIT_NAME.NONE_JET_SUIT;
      default:
        DebugUtil.DevAssert(false, $"Couldn't find \"no item\" string for outfit {outfitType}");
        return "-";
    }
  }

  public static (string name, string desc) GetNoneClothingItemStrings(PermitCategory category)
  {
    switch (category)
    {
      case PermitCategory.DupeTops:
        return ((string) EQUIPMENT.PREFABS.CLOTHING_TOPS.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.DESC);
      case PermitCategory.DupeBottoms:
        return ((string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.DESC);
      case PermitCategory.DupeGloves:
        return ((string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.DESC);
      case PermitCategory.DupeShoes:
        return ((string) EQUIPMENT.PREFABS.CLOTHING_SHOES.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.DESC);
      case PermitCategory.DupeHats:
        return ((string) EQUIPMENT.PREFABS.CLOTHING_HATS.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_HATS.DESC);
      case PermitCategory.DupeAccessories:
        return ((string) EQUIPMENT.PREFABS.CLOTHING_ACCESORIES.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_ACCESORIES.DESC);
      case PermitCategory.AtmoSuitHelmet:
        return ((string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.DESC);
      case PermitCategory.AtmoSuitBody:
        return ((string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.DESC);
      case PermitCategory.AtmoSuitGloves:
        return ((string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.DESC);
      case PermitCategory.AtmoSuitBelt:
        return ((string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.DESC);
      case PermitCategory.AtmoSuitShoes:
        return ((string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.DESC);
      case PermitCategory.JoyResponse:
        return ((string) UI.OUTFIT_DESCRIPTION.NO_JOY_RESPONSE_NAME, (string) UI.OUTFIT_DESCRIPTION.NO_JOY_RESPONSE_DESC);
      case PermitCategory.JetSuitHelmet:
        return ((string) EQUIPMENT.PREFABS.JET_SUIT_HELMET.NAME, (string) EQUIPMENT.PREFABS.JET_SUIT_HELMET.DESC);
      case PermitCategory.JetSuitBody:
        return ((string) EQUIPMENT.PREFABS.JET_SUIT_BODY.NAME, (string) EQUIPMENT.PREFABS.JET_SUIT_BODY.DESC);
      case PermitCategory.JetSuitGloves:
        return ((string) EQUIPMENT.PREFABS.JET_SUIT_GLOVES.NAME, (string) EQUIPMENT.PREFABS.JET_SUIT_GLOVES.DESC);
      case PermitCategory.JetSuitShoes:
        return ((string) EQUIPMENT.PREFABS.JET_SUIT_SHOES.NAME, (string) EQUIPMENT.PREFABS.JET_SUIT_SHOES.DESC);
      default:
        DebugUtil.DevAssert(false, $"Couldn't find \"no item\" string for category {category}");
        return ("-", "-");
    }
  }

  public static void ConfigureTooltipOn(GameObject gameObject, Option<LocString> tooltipText = default (Option<LocString>))
  {
    KleiItemsUI.ConfigureTooltipOn(gameObject, tooltipText.HasValue ? Option.Some<string>((string) tooltipText.Value) : (Option<string>) Option.None);
  }

  public static void ConfigureTooltipOn(GameObject gameObject, Option<string> tooltipText = default (Option<string>))
  {
    ToolTip toolTip = gameObject.GetComponent<ToolTip>();
    if (toolTip.IsNullOrDestroyed())
    {
      toolTip = gameObject.AddComponent<ToolTip>();
      toolTip.tooltipPivot = new Vector2(0.5f, 1f);
      toolTip.tooltipPositionOffset = !(bool) (Object) gameObject.GetComponent<KButton>() ? new Vector2(0.0f, 0.0f) : new Vector2(0.0f, 22f);
      toolTip.parentPositionAnchor = new Vector2(0.5f, 0.0f);
      toolTip.toolTipPosition = ToolTip.TooltipPosition.Custom;
    }
    if (!tooltipText.HasValue)
      toolTip.ClearMultiStringTooltip();
    else
      toolTip.SetSimpleTooltip(tooltipText.Value);
  }

  public static string GetTooltipStringFor(PermitResource permit)
  {
    string tooltipStringFor = KleiItemsUI.WrapAsToolTipTitle(permit.Name);
    if (!string.IsNullOrWhiteSpace(permit.Description))
      tooltipStringFor = $"{tooltipStringFor}\n{permit.Description}";
    string dlcIdFrom = permit.GetDlcIdFrom();
    if (DlcManager.IsDlcId(dlcIdFrom))
    {
      DlcManager.DlcInfo dlcInfo;
      bool flag = DlcManager.DLC_PACKS.TryGetValue(dlcIdFrom, out dlcInfo) && dlcInfo.isCosmetic;
      tooltipStringFor = permit.Rarity != PermitRarity.UniversalLocked ? (!flag ? $"{tooltipStringFor}\n\n{UI.KLEI_INVENTORY_SCREEN.COLLECTION.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom))}" : $"{tooltipStringFor}\n\n{UI.KLEI_INVENTORY_SCREEN.COLLECTION_THE.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom))}") : (!flag ? $"{tooltipStringFor}\n\n{UI.KLEI_INVENTORY_SCREEN.COLLECTION_COMING_SOON.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom))}" : $"{tooltipStringFor}\n\n{UI.KLEI_INVENTORY_SCREEN.COLLECTION_COMING_SOON_THE.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom))}");
    }
    else
    {
      string str = UI.KLEI_INVENTORY_SCREEN.ITEM_RARITY_DETAILS.Replace("{RarityName}", permit.Rarity.GetLocStringName());
      if (!string.IsNullOrWhiteSpace(str))
        tooltipStringFor = $"{tooltipStringFor}\n\n{str}";
    }
    if (permit.IsOwnableOnServer() && PermitItems.GetOwnedCount(permit) <= 0)
      tooltipStringFor = $"{tooltipStringFor}\n\n{KleiItemsUI.WrapWithColor((string) UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE, KleiItemsUI.TEXT_COLOR__PERMIT_NOT_OWNED)}";
    return tooltipStringFor;
  }

  public static string GetNoneTooltipStringFor(PermitCategory category)
  {
    (string str, string desc) = KleiItemsUI.GetNoneClothingItemStrings(category);
    return $"{KleiItemsUI.WrapAsToolTipTitle(str)}\n{desc}";
  }

  public static Color GetColor(string input)
  {
    return input[0] == '#' ? Util.ColorFromHex(input.Substring(1)) : Util.ColorFromHex(input);
  }
}
