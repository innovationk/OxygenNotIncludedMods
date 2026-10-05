// Decompiled with JetBrains decompiler
// Type: Blueprints_DlcPack2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;

#nullable disable
public class Blueprints_DlcPack2 : BlueprintProvider
{
  public override string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public override void SetupBlueprints()
  {
    this.AddBuilding("Headquarters", PermitRarity.Universal, "permit_headquarters_ceres", "hqbase_ice_kanim");
    this.AddBuilding("Bed", PermitRarity.Universal, "permit_bed_jorge", "bed_jorge_kanim");
    this.AddBuilding("StorageLockerSmart", PermitRarity.Universal, "permit_smartstoragelocker_gravitas", "smartstoragelocker_gravitas_kanim");
    this.AddArtable(BlueprintProvider.ArtableType.SculptureIce, PermitRarity.Universal, "permit_icesculpture_amazing_idle_bammoth", "icesculpture_idle_bammoth_kanim").Quality(BlueprintProvider.ArtableQuality.LookingOkay);
    this.AddArtable(BlueprintProvider.ArtableType.SculptureIce, PermitRarity.Universal, "permit_icesculpture_amazing_idle_wood_deer", "icesculpture_idle_floof_kanim").Quality(BlueprintProvider.ArtableQuality.LookingOkay);
    this.AddArtable(BlueprintProvider.ArtableType.SculptureIce, PermitRarity.Universal, "permit_icesculpture_amazing_idle_seal", "icesculpture_idle_seal_kanim").Quality(BlueprintProvider.ArtableQuality.LookingOkay);
    this.AddArtable(BlueprintProvider.ArtableType.SculptureWood, PermitRarity.Universal, "permit_sculpture_wood_crap_low_one", "sculpture_wood_low_one_kanim").Quality(BlueprintProvider.ArtableQuality.LookingUgly);
    this.AddArtable(BlueprintProvider.ArtableType.SculptureWood, PermitRarity.Universal, "permit_sculpture_wood_okay_mid_one", "sculpture_wood_mid_one_kanim").Quality(BlueprintProvider.ArtableQuality.LookingOkay);
    this.AddArtable(BlueprintProvider.ArtableType.SculptureWood, PermitRarity.Universal, "permit_sculpture_wood_amazing_action_wood_deer", "sculpture_wood_action_floof_kanim");
    this.AddArtable(BlueprintProvider.ArtableType.SculptureWood, PermitRarity.Universal, "permit_sculpture_wood_amazing_action_gulp", "sculpture_wood_action_gulp_kanim");
    this.AddArtable(BlueprintProvider.ArtableType.SculptureWood, PermitRarity.Universal, "permit_sculpture_wood_amazing_action_pacu", "sculpture_wood_action_pacu_kanim");
    this.AddArtable(BlueprintProvider.ArtableType.SculptureWood, PermitRarity.Universal, "permit_sculpture_wood_amazing_action_puft", "sculpture_wood_action_puft_kanim");
    this.AddArtable(BlueprintProvider.ArtableType.SculptureWood, PermitRarity.Universal, "permit_sculpture_wood_amazing_rear_cuddlepip", "sculpture_wood_rear_cuddlepip_kanim");
    this.AddArtable(BlueprintProvider.ArtableType.SculptureWood, PermitRarity.Universal, "permit_sculpture_wood_amazing_rear_drecko", "sculpture_wood_rear_drecko_kanim");
    this.AddArtable(BlueprintProvider.ArtableType.SculptureWood, PermitRarity.Universal, "permit_sculpture_wood_amazing_rear_puft", "sculpture_wood_rear_puft_kanim");
    this.AddArtable(BlueprintProvider.ArtableType.SculptureWood, PermitRarity.Universal, "permit_sculpture_wood_amazing_rear_shovevole", "sculpture_wood_rear_shovevole_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_flannel_red", "top_flannel_red_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_flannel_orange", "top_flannel_orange_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_flannel_yellow", "top_flannel_yellow_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_flannel_green", "top_flannel_green_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_flannel_blue_middle", "top_flannel_blue_middle_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_flannel_purple", "top_flannel_purple_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_flannel_pink_orchid", "top_flannel_pink_orchid_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_flannel_white", "top_flannel_white_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_flannel_black", "top_flannel_black_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.AtmoSuitBelt, PermitRarity.Universal, "permit_atmo_belt_80s", "atmo_belt_80s_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.AtmoSuitBody, PermitRarity.Universal, "permit_atmosuit_80s", "atmosuit_80s_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.AtmoSuitShoes, PermitRarity.Universal, "permit_atmo_shoes_80s", "atmo_shoes_80s_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.AtmoSuitGloves, PermitRarity.Universal, "permit_atmo_gloves_80s", "atmo_gloves_80s_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.AtmoSuitHelmet, PermitRarity.Universal, "permit_atmo_helmet_80s", "atmo_helmet_80s_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_gloves_hockey_01", "gloves_hockey_01_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_gloves_hockey_02", "gloves_hockey_02_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_gloves_hockey_03", "gloves_hockey_03_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_gloves_hockey_04", "gloves_hockey_04_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_gloves_hockey_05", "gloves_hockey_05_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_gloves_hockey_07", "gloves_hockey_07_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_gloves_hockey_08", "gloves_hockey_08_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_gloves_hockey_09", "gloves_hockey_09_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_gloves_hockey_10", "gloves_hockey_10_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_gloves_hockey_11", "gloves_hockey_11_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_gloves_hockey_12", "gloves_hockey_12_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_mittens_knit_black_smog", "mittens_knit_black_smog_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_mittens_knit_white", "mittens_knit_white_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_mittens_knit_yellowcake", "mittens_knit_yellowcake_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_mittens_knit_orange_tectonic", "mittens_knit_orange_tectonic_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_mittens_knit_green_enzyme", "mittens_knit_green_enzyme_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_mittens_knit_blue_azulene", "mittens_knit_blue_azulene_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_mittens_knit_purple_astral", "mittens_knit_purple_astral_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeGloves, PermitRarity.Universal, "permit_mittens_knit_pink_cosmic", "mittens_knit_pink_cosmic_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_jersey_01", "top_jersey_01_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_jersey_02", "top_jersey_02_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_jersey_03", "top_jersey_03_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_jersey_04", "top_jersey_04_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_jersey_05", "top_jersey_05_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_jersey_07", "top_jersey_07_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_jersey_08", "top_jersey_08_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_jersey_09", "top_jersey_09_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_jersey_10", "top_jersey_10_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_jersey_11", "top_jersey_11_kanim");
    this.AddClothing(BlueprintProvider.ClothingType.DupeTops, PermitRarity.Universal, "permit_top_jersey_12", "top_jersey_12_kanim");
    this.AddBuilding("Bed", PermitRarity.Universal, "permit_bed_cottage", "bed_cottage_kanim");
    this.AddBuilding("FloorLamp", PermitRarity.Universal, "permit_floorlamp_cottage", "floorlamp_cottage_kanim");
    this.AddBuilding("CookingStation", PermitRarity.Universal, "permit_cookingstation_cottage", "cookstation_cottage_kanim");
    this.AddBuilding("GourmetCookingStation", PermitRarity.Universal, "permit_cookingstation_gourmet_cottage", "cookstation_gourmet_cottage_kanim");
    this.AddBuilding("RanchStation", PermitRarity.Universal, "permit_rancherstation_cottage", "rancherstation_cottage_kanim");
    this.AddBuilding("ExteriorWall", PermitRarity.Universal, "permit_walls_wood_panel", "walls_wood_panel_kanim");
    this.AddBuilding("ExteriorWall", PermitRarity.Universal, "permit_walls_igloo", "walls_igloo_kanim");
    this.AddBuilding("ExteriorWall", PermitRarity.Universal, "permit_walls_forest", "walls_forest_kanim");
    this.AddBuilding("ExteriorWall", PermitRarity.Universal, "permit_walls_southwest", "walls_southwest_kanim");
    this.AddBuilding("ItemPedestal", PermitRarity.Universal, "permit_pedestal_cottage", "pedestal_cottage_kanim");
    this.AddBuilding("ExobaseHeadquarters", PermitRarity.Universal, "permit_porta_pod_y_ice", "porta_pod_y_ice_kanim");
    this.AddArtable(BlueprintProvider.ArtableType.Painting, PermitRarity.Universal, "permit_painting_art_ceres_a", "painting_art_ceres_a_kanim");
    this.AddArtable(BlueprintProvider.ArtableType.PaintingWide, PermitRarity.Universal, "permit_painting_wide_art_ceres_a", "painting_wide_art_ceres_a_kanim");
    this.AddArtable(BlueprintProvider.ArtableType.PaintingTall, PermitRarity.Universal, "permit_painting_tall_art_ceres_a", "painting_tall_art_ceres_a_kanim");
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_flannel_red", new string[1]
    {
      "permit_top_flannel_red"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_flannel_orange", new string[1]
    {
      "permit_top_flannel_orange"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_flannel_yellow", new string[1]
    {
      "permit_top_flannel_yellow"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_flannel_green", new string[1]
    {
      "permit_top_flannel_green"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_flannel_blue_middle", new string[1]
    {
      "permit_top_flannel_blue_middle"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_flannel_purple", new string[1]
    {
      "permit_top_flannel_purple"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_flannel_pink_orchid", new string[1]
    {
      "permit_top_flannel_pink_orchid"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_flannel_white", new string[1]
    {
      "permit_top_flannel_white"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_flannel_black", new string[1]
    {
      "permit_top_flannel_black"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_hockey_01", new string[2]
    {
      "permit_gloves_hockey_01",
      "permit_top_jersey_01"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_hockey_02", new string[2]
    {
      "permit_gloves_hockey_02",
      "permit_top_jersey_02"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_hockey_03", new string[2]
    {
      "permit_gloves_hockey_03",
      "permit_top_jersey_03"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_hockey_04", new string[2]
    {
      "permit_gloves_hockey_04",
      "permit_top_jersey_04"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_hockey_05", new string[2]
    {
      "permit_gloves_hockey_05",
      "permit_top_jersey_05"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_hockey_07", new string[2]
    {
      "permit_gloves_hockey_07",
      "permit_top_jersey_07"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_hockey_08", new string[2]
    {
      "permit_gloves_hockey_08",
      "permit_top_jersey_08"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_hockey_09", new string[2]
    {
      "permit_gloves_hockey_09",
      "permit_top_jersey_09"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_hockey_10", new string[2]
    {
      "permit_gloves_hockey_10",
      "permit_top_jersey_10"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_hockey_11", new string[2]
    {
      "permit_gloves_hockey_11",
      "permit_top_jersey_11"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.Clothing, "outfit_top_hockey_12", new string[2]
    {
      "permit_gloves_hockey_12",
      "permit_top_jersey_12"
    });
    this.AddOutfit(BlueprintProvider.OutfitType.AtmoSuit, "outfit_atmo_suit_80s", new string[5]
    {
      "permit_atmo_helmet_80s",
      "permit_atmo_gloves_80s",
      "permit_atmo_shoes_80s",
      "permit_atmosuit_80s",
      "permit_atmo_belt_80s"
    });
    this.AddMonumentPart(BlueprintProvider.MonumentPart.Bottom, PermitRarity.Universal, "permit_monument_base_a_frosty", "monument_base_a_frosty_kanim");
    this.AddMonumentPart(BlueprintProvider.MonumentPart.Bottom, PermitRarity.Universal, "permit_monument_base_b_frosty", "monument_base_b_frosty_kanim");
    this.AddMonumentPart(BlueprintProvider.MonumentPart.Bottom, PermitRarity.Universal, "permit_monument_base_c_frosty", "monument_base_c_frosty_kanim");
    this.AddMonumentPart(BlueprintProvider.MonumentPart.Middle, PermitRarity.Universal, "permit_monument_mid_a_frosty", "monument_mid_a_frosty_kanim");
    this.AddMonumentPart(BlueprintProvider.MonumentPart.Middle, PermitRarity.Universal, "permit_monument_mid_b_frosty", "monument_mid_b_frosty_kanim");
    this.AddMonumentPart(BlueprintProvider.MonumentPart.Middle, PermitRarity.Universal, "permit_monument_mid_c_frosty", "monument_mid_c_frosty_kanim");
    this.AddMonumentPart(BlueprintProvider.MonumentPart.Top, PermitRarity.Universal, "permit_monument_upper_a_frosty", "monument_upper_a_frosty_kanim");
    this.AddMonumentPart(BlueprintProvider.MonumentPart.Top, PermitRarity.Universal, "permit_monument_upper_b_frosty", "monument_upper_b_frosty_kanim");
    this.AddMonumentPart(BlueprintProvider.MonumentPart.Top, PermitRarity.Universal, "permit_monument_upper_c_frosty", "monument_upper_c_frosty_kanim");
  }
}
