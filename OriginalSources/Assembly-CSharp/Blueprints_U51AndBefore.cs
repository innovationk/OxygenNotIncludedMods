// Decompiled with JetBrains decompiler
// Type: Blueprints_U51AndBefore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using STRINGS;
using System.Collections.Generic;

#nullable disable
public class Blueprints_U51AndBefore : BlueprintProvider
{
  public override void SetupBlueprints()
  {
    this.SetupBuildingFacades();
    this.SetupArtables();
    this.SetupClothingItems();
    this.SetupClothingOutfits();
    this.SetupBalloonArtistFacades();
  }

  public void SetupBuildingFacades()
  {
    this.blueprintCollection.buildingFacades.AddRange((IEnumerable<BuildingFacadeInfo>) new BuildingFacadeInfo[203]
    {
      new BuildingFacadeInfo("ExteriorWall_basic_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_WHITE.DESC, PermitRarity.Universal, "ExteriorWall", "walls_basic_white_kanim"),
      new BuildingFacadeInfo("FlowerVase_retro", (string) BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_SUNNY.NAME, (string) BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_SUNNY.DESC, PermitRarity.Common, "FlowerVase", "flowervase_retro_yellow_kanim"),
      new BuildingFacadeInfo("FlowerVase_retro_red", (string) BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_BOLD.NAME, (string) BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_BOLD.DESC, PermitRarity.Common, "FlowerVase", "flowervase_retro_red_kanim"),
      new BuildingFacadeInfo("FlowerVase_retro_white", (string) BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_ELEGANT.NAME, (string) BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_ELEGANT.DESC, PermitRarity.Common, "FlowerVase", "flowervase_retro_white_kanim"),
      new BuildingFacadeInfo("FlowerVase_retro_green", (string) BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_BRIGHT.NAME, (string) BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_BRIGHT.DESC, PermitRarity.Common, "FlowerVase", "flowervase_retro_green_kanim"),
      new BuildingFacadeInfo("FlowerVase_retro_blue", (string) BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_DREAMY.NAME, (string) BUILDINGS.PREFABS.FLOWERVASE.FACADES.RETRO_DREAMY.DESC, PermitRarity.Common, "FlowerVase", "flowervase_retro_blue_kanim"),
      new BuildingFacadeInfo("LuxuryBed_boat", (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.BOAT.NAME, (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.BOAT.DESC, PermitRarity.Splendid, "LuxuryBed", "elegantbed_boat_kanim"),
      new BuildingFacadeInfo("LuxuryBed_bouncy", (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.BOUNCY_BED.NAME, (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.BOUNCY_BED.DESC, PermitRarity.Splendid, "LuxuryBed", "elegantbed_bouncy_kanim"),
      new BuildingFacadeInfo("LuxuryBed_grandprix", (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.GRANDPRIX.NAME, (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.GRANDPRIX.DESC, PermitRarity.Splendid, "LuxuryBed", "elegantbed_grandprix_kanim"),
      new BuildingFacadeInfo("LuxuryBed_rocket", (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.ROCKET_BED.NAME, (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.ROCKET_BED.DESC, PermitRarity.Splendid, "LuxuryBed", "elegantbed_rocket_kanim"),
      new BuildingFacadeInfo("LuxuryBed_puft", (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.PUFT_BED.NAME, (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.PUFT_BED.DESC, PermitRarity.Loyalty, "LuxuryBed", "elegantbed_puft_kanim"),
      new BuildingFacadeInfo("ExteriorWall_pastel_pink", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELPINK.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELPINK.DESC, PermitRarity.Common, "ExteriorWall", "walls_pastel_pink_kanim"),
      new BuildingFacadeInfo("ExteriorWall_pastel_yellow", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELYELLOW.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELYELLOW.DESC, PermitRarity.Common, "ExteriorWall", "walls_pastel_yellow_kanim"),
      new BuildingFacadeInfo("ExteriorWall_pastel_green", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELGREEN.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELGREEN.DESC, PermitRarity.Common, "ExteriorWall", "walls_pastel_green_kanim"),
      new BuildingFacadeInfo("ExteriorWall_pastel_blue", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELBLUE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELBLUE.DESC, PermitRarity.Common, "ExteriorWall", "walls_pastel_blue_kanim"),
      new BuildingFacadeInfo("ExteriorWall_pastel_purple", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELPURPLE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELPURPLE.DESC, PermitRarity.Common, "ExteriorWall", "walls_pastel_purple_kanim"),
      new BuildingFacadeInfo("ExteriorWall_balm_lily", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BALM_LILY.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BALM_LILY.DESC, PermitRarity.Decent, "ExteriorWall", "walls_balm_lily_kanim"),
      new BuildingFacadeInfo("ExteriorWall_clouds", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CLOUDS.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CLOUDS.DESC, PermitRarity.Decent, "ExteriorWall", "walls_clouds_kanim"),
      new BuildingFacadeInfo("ExteriorWall_coffee", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.COFFEE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.COFFEE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_coffee_kanim"),
      new BuildingFacadeInfo("ExteriorWall_mosaic", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.AQUATICMOSAIC.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.AQUATICMOSAIC.DESC, PermitRarity.Decent, "ExteriorWall", "walls_mosaic_kanim"),
      new BuildingFacadeInfo("ExteriorWall_mushbar", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.MUSHBAR.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.MUSHBAR.DESC, PermitRarity.Decent, "ExteriorWall", "walls_mushbar_kanim"),
      new BuildingFacadeInfo("ExteriorWall_plaid", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLAID.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLAID.DESC, PermitRarity.Decent, "ExteriorWall", "walls_plaid_kanim"),
      new BuildingFacadeInfo("ExteriorWall_rain", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.RAIN.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.RAIN.DESC, PermitRarity.Decent, "ExteriorWall", "walls_rain_kanim"),
      new BuildingFacadeInfo("ExteriorWall_rainbow", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.RAINBOW.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.RAINBOW.DESC, PermitRarity.Decent, "ExteriorWall", "walls_rainbow_kanim"),
      new BuildingFacadeInfo("ExteriorWall_snow", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SNOW.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SNOW.DESC, PermitRarity.Decent, "ExteriorWall", "walls_snow_kanim"),
      new BuildingFacadeInfo("ExteriorWall_sun", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SUN.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SUN.DESC, PermitRarity.Decent, "ExteriorWall", "walls_sun_kanim"),
      new BuildingFacadeInfo("ExteriorWall_polka", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELPOLKA.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PASTELPOLKA.DESC, PermitRarity.Decent, "ExteriorWall", "walls_polka_kanim"),
      new BuildingFacadeInfo("ExteriorWall_diagonal_red_deep_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_RED_DEEP_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_RED_DEEP_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_diagonal_red_deep_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_diagonal_orange_satsuma_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_ORANGE_SATSUMA_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_ORANGE_SATSUMA_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_diagonal_orange_satsuma_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_diagonal_yellow_lemon_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_YELLOW_LEMON_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_YELLOW_LEMON_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_diagonal_yellow_lemon_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_diagonal_green_kelly_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_GREEN_KELLY_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_GREEN_KELLY_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_diagonal_green_kelly_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_diagonal_blue_cobalt_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_BLUE_COBALT_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_BLUE_COBALT_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_diagonal_blue_cobalt_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_diagonal_pink_flamingo_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_PINK_FLAMINGO_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_PINK_FLAMINGO_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_diagonal_pink_flamingo_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_diagonal_grey_charcoal_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_GREY_CHARCOAL_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DIAGONAL_GREY_CHARCOAL_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_diagonal_grey_charcoal_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_circle_red_deep_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_RED_DEEP_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_RED_DEEP_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_circle_red_deep_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_circle_orange_satsuma_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_ORANGE_SATSUMA_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_ORANGE_SATSUMA_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_circle_orange_satsuma_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_circle_yellow_lemon_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_YELLOW_LEMON_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_YELLOW_LEMON_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_circle_yellow_lemon_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_circle_green_kelly_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_GREEN_KELLY_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_GREEN_KELLY_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_circle_green_kelly_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_circle_blue_cobalt_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_BLUE_COBALT_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_BLUE_COBALT_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_circle_blue_cobalt_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_circle_pink_flamingo_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_PINK_FLAMINGO_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_PINK_FLAMINGO_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_circle_pink_flamingo_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_circle_grey_charcoal_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_GREY_CHARCOAL_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.CIRCLE_GREY_CHARCOAL_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_circle_grey_charcoal_white_kanim"),
      new BuildingFacadeInfo("Bed_star_curtain", (string) BUILDINGS.PREFABS.BED.FACADES.STARCURTAIN.NAME, (string) BUILDINGS.PREFABS.BED.FACADES.STARCURTAIN.DESC, PermitRarity.Nifty, "Bed", "bed_star_curtain_kanim"),
      new BuildingFacadeInfo("Bed_canopy", (string) BUILDINGS.PREFABS.BED.FACADES.CREAKY.NAME, (string) BUILDINGS.PREFABS.BED.FACADES.CREAKY.DESC, PermitRarity.Nifty, "Bed", "bed_canopy_kanim"),
      new BuildingFacadeInfo("Bed_rowan_tropical", (string) BUILDINGS.PREFABS.BED.FACADES.STAYCATION.NAME, (string) BUILDINGS.PREFABS.BED.FACADES.STAYCATION.DESC, PermitRarity.Nifty, "Bed", "bed_rowan_tropical_kanim"),
      new BuildingFacadeInfo("Bed_ada_science_lab", (string) BUILDINGS.PREFABS.BED.FACADES.SCIENCELAB.NAME, (string) BUILDINGS.PREFABS.BED.FACADES.SCIENCELAB.DESC, PermitRarity.Nifty, "Bed", "bed_ada_science_lab_kanim"),
      new BuildingFacadeInfo("CeilingLight_mining", (string) BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.MINING.NAME, (string) BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.MINING.DESC, PermitRarity.Common, "CeilingLight", "ceilinglight_mining_kanim"),
      new BuildingFacadeInfo("CeilingLight_flower", (string) BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.BLOSSOM.NAME, (string) BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.BLOSSOM.DESC, PermitRarity.Common, "CeilingLight", "ceilinglight_flower_kanim"),
      new BuildingFacadeInfo("CeilingLight_polka_lamp_shade", (string) BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.POLKADOT.NAME, (string) BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.POLKADOT.DESC, PermitRarity.Common, "CeilingLight", "ceilinglight_polka_lamp_shade_kanim"),
      new BuildingFacadeInfo("CeilingLight_burt_shower", (string) BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.FAUXPIPE.NAME, (string) BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.FAUXPIPE.DESC, PermitRarity.Common, "CeilingLight", "ceilinglight_burt_shower_kanim"),
      new BuildingFacadeInfo("CeilingLight_ada_flask_round", (string) BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.LABFLASK.NAME, (string) BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.LABFLASK.DESC, PermitRarity.Common, "CeilingLight", "ceilinglight_ada_flask_round_kanim"),
      new BuildingFacadeInfo("FlowerVaseWall_retro_green", (string) BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_GREEN.NAME, (string) BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_GREEN.DESC, PermitRarity.Common, "FlowerVaseWall", "flowervase_wall_retro_green_kanim"),
      new BuildingFacadeInfo("FlowerVaseWall_retro_yellow", (string) BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_YELLOW.NAME, (string) BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_YELLOW.DESC, PermitRarity.Common, "FlowerVaseWall", "flowervase_wall_retro_yellow_kanim"),
      new BuildingFacadeInfo("FlowerVaseWall_retro_red", (string) BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_RED.NAME, (string) BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_RED.DESC, PermitRarity.Common, "FlowerVaseWall", "flowervase_wall_retro_red_kanim"),
      new BuildingFacadeInfo("FlowerVaseWall_retro_blue", (string) BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_BLUE.NAME, (string) BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_BLUE.DESC, PermitRarity.Common, "FlowerVaseWall", "flowervase_wall_retro_blue_kanim"),
      new BuildingFacadeInfo("FlowerVaseWall_retro_white", (string) BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_WHITE.NAME, (string) BUILDINGS.PREFABS.FLOWERVASEWALL.FACADES.RETRO_WHITE.DESC, PermitRarity.Common, "FlowerVaseWall", "flowervase_wall_retro_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_basic_blue_cobalt", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_BLUE_COBALT.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_BLUE_COBALT.DESC, PermitRarity.Common, "ExteriorWall", "walls_basic_blue_cobalt_kanim"),
      new BuildingFacadeInfo("ExteriorWall_basic_green_kelly", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_GREEN_KELLY.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_GREEN_KELLY.DESC, PermitRarity.Common, "ExteriorWall", "walls_basic_green_kelly_kanim"),
      new BuildingFacadeInfo("ExteriorWall_basic_grey_charcoal", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_GREY_CHARCOAL.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_GREY_CHARCOAL.DESC, PermitRarity.Common, "ExteriorWall", "walls_basic_grey_charcoal_kanim"),
      new BuildingFacadeInfo("ExteriorWall_basic_orange_satsuma", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_ORANGE_SATSUMA.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_ORANGE_SATSUMA.DESC, PermitRarity.Common, "ExteriorWall", "walls_basic_orange_satsuma_kanim"),
      new BuildingFacadeInfo("ExteriorWall_basic_pink_flamingo", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_PINK_FLAMINGO.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_PINK_FLAMINGO.DESC, PermitRarity.Common, "ExteriorWall", "walls_basic_pink_flamingo_kanim"),
      new BuildingFacadeInfo("ExteriorWall_basic_red_deep", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_RED_DEEP.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_RED_DEEP.DESC, PermitRarity.Common, "ExteriorWall", "walls_basic_red_deep_kanim"),
      new BuildingFacadeInfo("ExteriorWall_basic_yellow_lemon", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_YELLOW_LEMON.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BASIC_YELLOW_LEMON.DESC, PermitRarity.Common, "ExteriorWall", "walls_basic_yellow_lemon_kanim"),
      new BuildingFacadeInfo("ExteriorWall_blueberries", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BLUEBERRIES.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.BLUEBERRIES.DESC, PermitRarity.Decent, "ExteriorWall", "walls_blueberries_kanim"),
      new BuildingFacadeInfo("ExteriorWall_grapes", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.GRAPES.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.GRAPES.DESC, PermitRarity.Decent, "ExteriorWall", "walls_grapes_kanim"),
      new BuildingFacadeInfo("ExteriorWall_lemon", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.LEMON.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.LEMON.DESC, PermitRarity.Decent, "ExteriorWall", "walls_lemon_kanim"),
      new BuildingFacadeInfo("ExteriorWall_lime", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.LIME.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.LIME.DESC, PermitRarity.Decent, "ExteriorWall", "walls_lime_kanim"),
      new BuildingFacadeInfo("ExteriorWall_satsuma", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SATSUMA.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SATSUMA.DESC, PermitRarity.Decent, "ExteriorWall", "walls_satsuma_kanim"),
      new BuildingFacadeInfo("ExteriorWall_strawberry", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRAWBERRY.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRAWBERRY.DESC, PermitRarity.Decent, "ExteriorWall", "walls_strawberry_kanim"),
      new BuildingFacadeInfo("ExteriorWall_watermelon", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.WATERMELON.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.WATERMELON.DESC, PermitRarity.Decent, "ExteriorWall", "walls_watermelon_kanim"),
      new BuildingFacadeInfo("FlowerVaseHanging_retro_red", (string) BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_RED.NAME, (string) BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_RED.DESC, PermitRarity.Common, "FlowerVaseHanging", "flowervase_hanging_retro_red_kanim"),
      new BuildingFacadeInfo("FlowerVaseHanging_retro_green", (string) BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_GREEN.NAME, (string) BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_GREEN.DESC, PermitRarity.Common, "FlowerVaseHanging", "flowervase_hanging_retro_green_kanim"),
      new BuildingFacadeInfo("FlowerVaseHanging_retro_blue", (string) BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_BLUE.NAME, (string) BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_BLUE.DESC, PermitRarity.Common, "FlowerVaseHanging", "flowervase_hanging_retro_blue_kanim"),
      new BuildingFacadeInfo("FlowerVaseHanging_retro_yellow", (string) BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_YELLOW.NAME, (string) BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_YELLOW.DESC, PermitRarity.Common, "FlowerVaseHanging", "flowervase_hanging_retro_yellow_kanim"),
      new BuildingFacadeInfo("FlowerVaseHanging_retro_white", (string) BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_WHITE.NAME, (string) BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RETRO_WHITE.DESC, PermitRarity.Common, "FlowerVaseHanging", "flowervase_hanging_retro_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_toiletpaper", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TOILETPAPER.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TOILETPAPER.DESC, PermitRarity.Decent, "ExteriorWall", "walls_toiletpaper_kanim"),
      new BuildingFacadeInfo("ExteriorWall_plunger", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUNGER.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUNGER.DESC, PermitRarity.Decent, "ExteriorWall", "walls_plunger_kanim"),
      new BuildingFacadeInfo("ExteriorWall_tropical", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TROPICAL.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TROPICAL.DESC, PermitRarity.Decent, "ExteriorWall", "walls_tropical_kanim"),
      new BuildingFacadeInfo("ItemPedestal_hand", (string) BUILDINGS.PREFABS.ITEMPEDESTAL.FACADES.HAND.NAME, (string) BUILDINGS.PREFABS.ITEMPEDESTAL.FACADES.HAND.DESC, PermitRarity.Decent, "ItemPedestal", "pedestal_hand_kanim"),
      new BuildingFacadeInfo("MassageTable_shiatsu", (string) BUILDINGS.PREFABS.MASSAGETABLE.FACADES.SHIATSU.NAME, (string) BUILDINGS.PREFABS.MASSAGETABLE.FACADES.SHIATSU.DESC, PermitRarity.Splendid, "MassageTable", "masseur_shiatsu_kanim"),
      new BuildingFacadeInfo("RockCrusher_hands", (string) BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.HANDS.NAME, (string) BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.HANDS.DESC, PermitRarity.Splendid, "RockCrusher", "rockrefinery_hands_kanim"),
      new BuildingFacadeInfo("RockCrusher_teeth", (string) BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.TEETH.NAME, (string) BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.TEETH.DESC, PermitRarity.Splendid, "RockCrusher", "rockrefinery_teeth_kanim"),
      new BuildingFacadeInfo("WaterCooler_round_body", (string) BUILDINGS.PREFABS.WATERCOOLER.FACADES.ROUND_BODY.NAME, (string) BUILDINGS.PREFABS.WATERCOOLER.FACADES.ROUND_BODY.DESC, PermitRarity.Splendid, "WaterCooler", "watercooler_round_body_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_blue", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_BLUE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_BLUE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_blue_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_diagonal_blue", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_BLUE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_BLUE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_diagonal_blue_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_circle_blue", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_BLUE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_BLUE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_circle_blue_kanim"),
      new BuildingFacadeInfo("ExteriorWall_squares_red_deep_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_RED_DEEP_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_RED_DEEP_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_squares_red_deep_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_squares_orange_satsuma_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_ORANGE_SATSUMA_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_ORANGE_SATSUMA_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_squares_orange_satsuma_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_squares_yellow_lemon_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_YELLOW_LEMON_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_YELLOW_LEMON_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_squares_yellow_lemon_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_squares_green_kelly_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_GREEN_KELLY_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_GREEN_KELLY_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_squares_green_kelly_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_squares_blue_cobalt_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_BLUE_COBALT_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_BLUE_COBALT_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_squares_blue_cobalt_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_squares_pink_flamingo_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_PINK_FLAMINGO_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_PINK_FLAMINGO_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_squares_pink_flamingo_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_squares_grey_charcoal_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_GREY_CHARCOAL_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.SQUARES_GREY_CHARCOAL_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_squares_grey_charcoal_white_kanim"),
      new BuildingFacadeInfo("EggCracker_beaker", (string) BUILDINGS.PREFABS.EGGCRACKER.FACADES.BEAKER.NAME, (string) BUILDINGS.PREFABS.EGGCRACKER.FACADES.BEAKER.DESC, PermitRarity.Nifty, "EggCracker", "egg_cracker_beaker_kanim"),
      new BuildingFacadeInfo("EggCracker_flower", (string) BUILDINGS.PREFABS.EGGCRACKER.FACADES.FLOWER.NAME, (string) BUILDINGS.PREFABS.EGGCRACKER.FACADES.FLOWER.DESC, PermitRarity.Nifty, "EggCracker", "egg_cracker_flower_kanim"),
      new BuildingFacadeInfo("EggCracker_hands", (string) BUILDINGS.PREFABS.EGGCRACKER.FACADES.HANDS.NAME, (string) BUILDINGS.PREFABS.EGGCRACKER.FACADES.HANDS.DESC, PermitRarity.Nifty, "EggCracker", "egg_cracker_hands_kanim"),
      new BuildingFacadeInfo("CeilingLight_rubiks", (string) BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.RUBIKS.NAME, (string) BUILDINGS.PREFABS.CEILINGLIGHT.FACADES.RUBIKS.DESC, PermitRarity.Common, "CeilingLight", "ceilinglight_rubiks_kanim"),
      new BuildingFacadeInfo("FlowerVaseHanging_beaker", (string) BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.BEAKER.NAME, (string) BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.BEAKER.DESC, PermitRarity.Common, "FlowerVaseHanging", "flowervase_hanging_beaker_kanim"),
      new BuildingFacadeInfo("FlowerVaseHanging_rubiks", (string) BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RUBIKS.NAME, (string) BUILDINGS.PREFABS.FLOWERVASEHANGING.FACADES.RUBIKS.DESC, PermitRarity.Common, "FlowerVaseHanging", "flowervase_hanging_rubiks_kanim"),
      new BuildingFacadeInfo("LuxuryBed_hand", (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.HAND.NAME, (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.HAND.DESC, PermitRarity.Splendid, "LuxuryBed", "elegantbed_hand_kanim"),
      new BuildingFacadeInfo("LuxuryBed_rubiks", (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.RUBIKS.NAME, (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.RUBIKS.DESC, PermitRarity.Splendid, "LuxuryBed", "elegantbed_rubiks_kanim"),
      new BuildingFacadeInfo("RockCrusher_roundstamp", (string) BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.ROUNDSTAMP.NAME, (string) BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.ROUNDSTAMP.DESC, PermitRarity.Splendid, "RockCrusher", "rockrefinery_roundstamp_kanim"),
      new BuildingFacadeInfo("RockCrusher_spikebeds", (string) BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.SPIKEBEDS.NAME, (string) BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.SPIKEBEDS.DESC, PermitRarity.Splendid, "RockCrusher", "rockrefinery_spikebeds_kanim"),
      new BuildingFacadeInfo("StorageLocker_green_mush", (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.GREEN_MUSH.NAME, (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "StorageLocker", "storagelocker_green_mush_kanim"),
      new BuildingFacadeInfo("StorageLocker_red_rose", (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.RED_ROSE.NAME, (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "StorageLocker", "storagelocker_red_rose_kanim"),
      new BuildingFacadeInfo("StorageLocker_blue_babytears", (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.BLUE_BABYTEARS.NAME, (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.BLUE_BABYTEARS.DESC, PermitRarity.Nifty, "StorageLocker", "storagelocker_blue_babytears_kanim"),
      new BuildingFacadeInfo("StorageLocker_purple_brainfat", (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.PURPLE_BRAINFAT.NAME, (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "StorageLocker", "storagelocker_purple_brainfat_kanim"),
      new BuildingFacadeInfo("StorageLocker_yellow_tartar", (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.YELLOW_TARTAR.NAME, (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "StorageLocker", "storagelocker_yellow_tartar_kanim"),
      new BuildingFacadeInfo("PlanterBox_mealwood", (string) BUILDINGS.PREFABS.PLANTERBOX.FACADES.MEALWOOD.NAME, (string) BUILDINGS.PREFABS.PLANTERBOX.FACADES.MEALWOOD.DESC, PermitRarity.Common, "PlanterBox", "planterbox_skin_mealwood_kanim"),
      new BuildingFacadeInfo("PlanterBox_bristleblossom", (string) BUILDINGS.PREFABS.PLANTERBOX.FACADES.BRISTLEBLOSSOM.NAME, (string) BUILDINGS.PREFABS.PLANTERBOX.FACADES.BRISTLEBLOSSOM.DESC, PermitRarity.Common, "PlanterBox", "planterbox_skin_bristleblossom_kanim"),
      new BuildingFacadeInfo("PlanterBox_wheezewort", (string) BUILDINGS.PREFABS.PLANTERBOX.FACADES.WHEEZEWORT.NAME, (string) BUILDINGS.PREFABS.PLANTERBOX.FACADES.WHEEZEWORT.DESC, PermitRarity.Decent, "PlanterBox", "planterbox_skin_wheezewort_kanim"),
      new BuildingFacadeInfo("PlanterBox_sleetwheat", (string) BUILDINGS.PREFABS.PLANTERBOX.FACADES.SLEETWHEAT.NAME, (string) BUILDINGS.PREFABS.PLANTERBOX.FACADES.SLEETWHEAT.DESC, PermitRarity.Common, "PlanterBox", "planterbox_skin_sleetwheat_kanim"),
      new BuildingFacadeInfo("PlanterBox_salmon_pink", (string) BUILDINGS.PREFABS.PLANTERBOX.FACADES.SALMON_PINK.NAME, (string) BUILDINGS.PREFABS.PLANTERBOX.FACADES.SALMON_PINK.DESC, PermitRarity.Common, "PlanterBox", "planterbox_skin_salmon_pink_kanim"),
      new BuildingFacadeInfo("GasReservoir_lightgold", (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.LIGHTGOLD.NAME, (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.LIGHTGOLD.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_lightgold_kanim"),
      new BuildingFacadeInfo("GasReservoir_peagreen", (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.PEAGREEN.NAME, (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.PEAGREEN.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_peagreen_kanim"),
      new BuildingFacadeInfo("GasReservoir_lightcobalt", (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.LIGHTCOBALT.NAME, (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.LIGHTCOBALT.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_lightcobalt_kanim"),
      new BuildingFacadeInfo("GasReservoir_polka_darkpurpleresin", (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.POLKA_DARKPURPLERESIN.NAME, (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.POLKA_DARKPURPLERESIN.DESC, PermitRarity.Splendid, "GasReservoir", "gasstorage_polka_darkpurpleresin_kanim"),
      new BuildingFacadeInfo("GasReservoir_polka_darknavynookgreen", (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.POLKA_DARKNAVYNOOKGREEN.NAME, (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.POLKA_DARKNAVYNOOKGREEN.DESC, PermitRarity.Splendid, "GasReservoir", "gasstorage_polka_darknavynookgreen_kanim"),
      new BuildingFacadeInfo("ExteriorWall_kitchen_retro1", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.KITCHEN_RETRO1.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.KITCHEN_RETRO1.DESC, PermitRarity.Decent, "ExteriorWall", "walls_kitchen_retro1_kanim"),
      new BuildingFacadeInfo("ExteriorWall_plus_red_deep_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_RED_DEEP_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_RED_DEEP_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_plus_red_deep_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_plus_orange_satsuma_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_ORANGE_SATSUMA_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_ORANGE_SATSUMA_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_plus_orange_satsuma_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_plus_yellow_lemon_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_YELLOW_LEMON_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_YELLOW_LEMON_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_plus_yellow_lemon_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_plus_green_kelly_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_GREEN_KELLY_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_GREEN_KELLY_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_plus_green_kelly_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_plus_blue_cobalt_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_BLUE_COBALT_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_BLUE_COBALT_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_plus_blue_cobalt_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_plus_pink_flamingo_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_PINK_FLAMINGO_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_PINK_FLAMINGO_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_plus_pink_flamingo_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_plus_grey_charcoal_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_GREY_CHARCOAL_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PLUS_GREY_CHARCOAL_WHITE.DESC, PermitRarity.Common, "ExteriorWall", "walls_plus_grey_charcoal_white_kanim"),
      new BuildingFacadeInfo("RockCrusher_chomp", (string) BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.CHOMP.NAME, (string) BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.CHOMP.DESC, PermitRarity.Splendid, "RockCrusher", "rockrefinery_chomp_kanim"),
      new BuildingFacadeInfo("RockCrusher_gears", (string) BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.GEARS.NAME, (string) BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.GEARS.DESC, PermitRarity.Splendid, "RockCrusher", "rockrefinery_gears_kanim"),
      new BuildingFacadeInfo("RockCrusher_balloon", (string) BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.BALLOON.NAME, (string) BUILDINGS.PREFABS.ROCKCRUSHER.FACADES.BALLOON.DESC, PermitRarity.Splendid, "RockCrusher", "rockrefinery_balloon_kanim"),
      new BuildingFacadeInfo("StorageLocker_polka_darknavynookgreen", (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.POLKA_DARKNAVYNOOKGREEN.NAME, (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.POLKA_DARKNAVYNOOKGREEN.DESC, PermitRarity.Splendid, "StorageLocker", "storagelocker_polka_darknavynookgreen_kanim"),
      new BuildingFacadeInfo("StorageLocker_polka_darkpurpleresin", (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.POLKA_DARKPURPLERESIN.NAME, (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.POLKA_DARKPURPLERESIN.DESC, PermitRarity.Splendid, "StorageLocker", "storagelocker_polka_darkpurpleresin_kanim"),
      new BuildingFacadeInfo("GasReservoir_blue_babytears", (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.BLUE_BABYTEARS.NAME, (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.BLUE_BABYTEARS.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_blue_babytears_kanim"),
      new BuildingFacadeInfo("GasReservoir_yellow_tartar", (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.YELLOW_TARTAR.NAME, (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_yellow_tartar_kanim"),
      new BuildingFacadeInfo("GasReservoir_green_mush", (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.GREEN_MUSH.NAME, (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_green_mush_kanim"),
      new BuildingFacadeInfo("GasReservoir_red_rose", (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.RED_ROSE.NAME, (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_red_rose_kanim"),
      new BuildingFacadeInfo("GasReservoir_purple_brainfat", (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.PURPLE_BRAINFAT.NAME, (string) BUILDINGS.PREFABS.GASRESERVOIR.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "GasReservoir", "gasstorage_purple_brainfat_kanim"),
      new BuildingFacadeInfo("MassageTable_balloon", (string) BUILDINGS.PREFABS.MASSAGETABLE.FACADES.MASSEUR_BALLOON.NAME, (string) BUILDINGS.PREFABS.MASSAGETABLE.FACADES.MASSEUR_BALLOON.DESC, PermitRarity.Splendid, "MassageTable", "masseur_balloon_kanim", new Dictionary<string, string>()
      {
        {
          "MassageTableComplete",
          "anim_interacts_masseur_balloon_kanim"
        }
      }),
      new BuildingFacadeInfo("WaterCooler_balloon", (string) BUILDINGS.PREFABS.WATERCOOLER.FACADES.BALLOON.NAME, (string) BUILDINGS.PREFABS.WATERCOOLER.FACADES.BALLOON.DESC, PermitRarity.Splendid, "WaterCooler", "watercooler_balloon_kanim"),
      new BuildingFacadeInfo("Bed_stringlights", (string) BUILDINGS.PREFABS.BED.FACADES.STRINGLIGHTS.NAME, (string) BUILDINGS.PREFABS.BED.FACADES.STRINGLIGHTS.DESC, PermitRarity.Nifty, "Bed", "bed_stringlights_kanim"),
      new BuildingFacadeInfo("CornerMoulding_shineornaments", (string) BUILDINGS.PREFABS.CORNERMOULDING.FACADES.SHINEORNAMENTS.NAME, (string) BUILDINGS.PREFABS.CORNERMOULDING.FACADES.SHINEORNAMENTS.DESC, PermitRarity.Decent, "CornerMoulding", "corner_tile_shineornaments_kanim"),
      new BuildingFacadeInfo("CrownMoulding_shineornaments", (string) BUILDINGS.PREFABS.CROWNMOULDING.FACADES.SHINEORNAMENTS.NAME, (string) BUILDINGS.PREFABS.CROWNMOULDING.FACADES.SHINEORNAMENTS.DESC, PermitRarity.Decent, "CrownMoulding", "crown_moulding_shineornaments_kanim"),
      new BuildingFacadeInfo("FloorLamp_leg", (string) BUILDINGS.PREFABS.FLOORLAMP.FACADES.LEG.NAME, (string) BUILDINGS.PREFABS.FLOORLAMP.FACADES.LEG.DESC, PermitRarity.Decent, "FloorLamp", "floorlamp_leg_kanim"),
      new BuildingFacadeInfo("FloorLamp_bristle_blossom", (string) BUILDINGS.PREFABS.FLOORLAMP.FACADES.BRISTLEBLOSSOM.NAME, (string) BUILDINGS.PREFABS.FLOORLAMP.FACADES.BRISTLEBLOSSOM.DESC, PermitRarity.Decent, "FloorLamp", "floorlamp_bristle_blossom_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_rose", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_ROSE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_ROSE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_rose_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_diagonal_rose", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_ROSE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_ROSE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_diagonal_rose_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_circle_rose", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_ROSE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_ROSE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_circle_rose_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_mush", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_MUSH.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_MUSH.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_mush_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_diagonal_mush", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_MUSH.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_MUSH.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_diagonal_mush_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_circle_mush", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_MUSH.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_MUSH.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_circle_mush_kanim"),
      new BuildingFacadeInfo("StorageLocker_stripes_red_white", (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.STRIPES_RED_WHITE.NAME, (string) BUILDINGS.PREFABS.STORAGELOCKER.FACADES.STRIPES_RED_WHITE.DESC, PermitRarity.Splendid, "StorageLocker", "storagelocker_stripes_red_white_kanim"),
      new BuildingFacadeInfo("Refrigerator_stripes_red_white", (string) BUILDINGS.PREFABS.REFRIGERATOR.FACADES.STRIPES_RED_WHITE.NAME, (string) BUILDINGS.PREFABS.REFRIGERATOR.FACADES.STRIPES_RED_WHITE.DESC, PermitRarity.Splendid, "Refrigerator", "fridge_stripes_red_white_kanim"),
      new BuildingFacadeInfo("Refrigerator_blue_babytears", (string) BUILDINGS.PREFABS.REFRIGERATOR.FACADES.BLUE_BABYTEARS.NAME, (string) BUILDINGS.PREFABS.REFRIGERATOR.FACADES.BLUE_BABYTEARS.DESC, PermitRarity.Nifty, "Refrigerator", "fridge_blue_babytears_kanim"),
      new BuildingFacadeInfo("Refrigerator_green_mush", (string) BUILDINGS.PREFABS.REFRIGERATOR.FACADES.GREEN_MUSH.NAME, (string) BUILDINGS.PREFABS.REFRIGERATOR.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "Refrigerator", "fridge_green_mush_kanim"),
      new BuildingFacadeInfo("Refrigerator_red_rose", (string) BUILDINGS.PREFABS.REFRIGERATOR.FACADES.RED_ROSE.NAME, (string) BUILDINGS.PREFABS.REFRIGERATOR.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "Refrigerator", "fridge_red_rose_kanim"),
      new BuildingFacadeInfo("Refrigerator_yellow_tartar", (string) BUILDINGS.PREFABS.REFRIGERATOR.FACADES.YELLOW_TARTAR.NAME, (string) BUILDINGS.PREFABS.REFRIGERATOR.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "Refrigerator", "fridge_yellow_tartar_kanim"),
      new BuildingFacadeInfo("Refrigerator_purple_brainfat", (string) BUILDINGS.PREFABS.REFRIGERATOR.FACADES.PURPLE_BRAINFAT.NAME, (string) BUILDINGS.PREFABS.REFRIGERATOR.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "Refrigerator", "fridge_purple_brainfat_kanim"),
      new BuildingFacadeInfo("MicrobeMusher_purple_brainfat", (string) BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.PURPLE_BRAINFAT.NAME, (string) BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "MicrobeMusher", "microbemusher_purple_brainfat_kanim"),
      new BuildingFacadeInfo("MicrobeMusher_yellow_tartar", (string) BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.YELLOW_TARTAR.NAME, (string) BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "MicrobeMusher", "microbemusher_yellow_tartar_kanim"),
      new BuildingFacadeInfo("MicrobeMusher_red_rose", (string) BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.RED_ROSE.NAME, (string) BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "MicrobeMusher", "microbemusher_red_rose_kanim"),
      new BuildingFacadeInfo("MicrobeMusher_green_mush", (string) BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.GREEN_MUSH.NAME, (string) BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "MicrobeMusher", "microbemusher_green_mush_kanim"),
      new BuildingFacadeInfo("MicrobeMusher_blue_babytears", (string) BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.BLUE_BABYTEARS.NAME, (string) BUILDINGS.PREFABS.MICROBEMUSHER.FACADES.BLUE_BABYTEARS.DESC, PermitRarity.Nifty, "MicrobeMusher", "microbemusher_blue_babytears_kanim"),
      new BuildingFacadeInfo("WashSink_purple_brainfat", (string) BUILDINGS.PREFABS.WASHSINK.FACADES.PURPLE_BRAINFAT.NAME, (string) BUILDINGS.PREFABS.WASHSINK.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "WashSink", "wash_sink_purple_brainfat_kanim"),
      new BuildingFacadeInfo("WashSink_blue_babytears", (string) BUILDINGS.PREFABS.WASHSINK.FACADES.BLUE_BABYTEARS.NAME, (string) BUILDINGS.PREFABS.WASHSINK.FACADES.BLUE_BABYTEARS.DESC, PermitRarity.Nifty, "WashSink", "wash_sink_blue_babytears_kanim"),
      new BuildingFacadeInfo("WashSink_green_mush", (string) BUILDINGS.PREFABS.WASHSINK.FACADES.GREEN_MUSH.NAME, (string) BUILDINGS.PREFABS.WASHSINK.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "WashSink", "wash_sink_green_mush_kanim"),
      new BuildingFacadeInfo("WashSink_yellow_tartar", (string) BUILDINGS.PREFABS.WASHSINK.FACADES.YELLOW_TARTAR.NAME, (string) BUILDINGS.PREFABS.WASHSINK.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "WashSink", "wash_sink_yellow_tartar_kanim"),
      new BuildingFacadeInfo("WashSink_red_rose", (string) BUILDINGS.PREFABS.WASHSINK.FACADES.RED_ROSE.NAME, (string) BUILDINGS.PREFABS.WASHSINK.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "WashSink", "wash_sink_red_rose_kanim"),
      new BuildingFacadeInfo("FlushToilet_polka_darkpurpleresin", (string) BUILDINGS.PREFABS.FLUSHTOILET.FACADES.POLKA_DARKPURPLERESIN.NAME, (string) BUILDINGS.PREFABS.FLUSHTOILET.FACADES.POLKA_DARKPURPLERESIN.DESC, PermitRarity.Splendid, "FlushToilet", "toiletflush_polka_darkpurpleresin_kanim"),
      new BuildingFacadeInfo("FlushToilet_polka_darknavynookgreen", (string) BUILDINGS.PREFABS.FLUSHTOILET.FACADES.POLKA_DARKNAVYNOOKGREEN.NAME, (string) BUILDINGS.PREFABS.FLUSHTOILET.FACADES.POLKA_DARKNAVYNOOKGREEN.DESC, PermitRarity.Splendid, "FlushToilet", "toiletflush_polka_darknavynookgreen_kanim"),
      new BuildingFacadeInfo("FlushToilet_purple_brainfat", (string) BUILDINGS.PREFABS.FLUSHTOILET.FACADES.PURPLE_BRAINFAT.NAME, (string) BUILDINGS.PREFABS.FLUSHTOILET.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "FlushToilet", "toiletflush_purple_brainfat_kanim"),
      new BuildingFacadeInfo("FlushToilet_yellow_tartar", (string) BUILDINGS.PREFABS.FLUSHTOILET.FACADES.YELLOW_TARTAR.NAME, (string) BUILDINGS.PREFABS.FLUSHTOILET.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "FlushToilet", "toiletflush_yellow_tartar_kanim"),
      new BuildingFacadeInfo("FlushToilet_red_rose", (string) BUILDINGS.PREFABS.FLUSHTOILET.FACADES.RED_ROSE.NAME, (string) BUILDINGS.PREFABS.FLUSHTOILET.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "FlushToilet", "toiletflush_red_rose_kanim"),
      new BuildingFacadeInfo("FlushToilet_green_mush", (string) BUILDINGS.PREFABS.FLUSHTOILET.FACADES.GREEN_MUSH.NAME, (string) BUILDINGS.PREFABS.FLUSHTOILET.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "FlushToilet", "toiletflush_green_mush_kanim"),
      new BuildingFacadeInfo("FlushToilet_blue_babytears", (string) BUILDINGS.PREFABS.FLUSHTOILET.FACADES.BLUE_BABYTEARS.NAME, (string) BUILDINGS.PREFABS.FLUSHTOILET.FACADES.BLUE_BABYTEARS.DESC, PermitRarity.Nifty, "FlushToilet", "toiletflush_blue_babytears_kanim"),
      new BuildingFacadeInfo("LuxuryBed_red_rose", (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.RED_ROSE.NAME, (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "LuxuryBed", "elegantbed_red_rose_kanim"),
      new BuildingFacadeInfo("LuxuryBed_green_mush", (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.GREEN_MUSH.NAME, (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "LuxuryBed", "elegantbed_green_mush_kanim"),
      new BuildingFacadeInfo("LuxuryBed_yellow_tartar", (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.YELLOW_TARTAR.NAME, (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "LuxuryBed", "elegantbed_yellow_tartar_kanim"),
      new BuildingFacadeInfo("LuxuryBed_purple_brainfat", (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.PURPLE_BRAINFAT.NAME, (string) BUILDINGS.PREFABS.LUXURYBED.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "LuxuryBed", "elegantbed_purple_brainfat_kanim"),
      new BuildingFacadeInfo("WaterCooler_yellow_tartar", (string) BUILDINGS.PREFABS.WATERCOOLER.FACADES.YELLOW_TARTAR.NAME, (string) BUILDINGS.PREFABS.WATERCOOLER.FACADES.YELLOW_TARTAR.DESC, PermitRarity.Nifty, "WaterCooler", "watercooler_yellow_tartar_kanim"),
      new BuildingFacadeInfo("WaterCooler_red_rose", (string) BUILDINGS.PREFABS.WATERCOOLER.FACADES.RED_ROSE.NAME, (string) BUILDINGS.PREFABS.WATERCOOLER.FACADES.RED_ROSE.DESC, PermitRarity.Nifty, "WaterCooler", "watercooler_red_rose_kanim"),
      new BuildingFacadeInfo("WaterCooler_green_mush", (string) BUILDINGS.PREFABS.WATERCOOLER.FACADES.GREEN_MUSH.NAME, (string) BUILDINGS.PREFABS.WATERCOOLER.FACADES.GREEN_MUSH.DESC, PermitRarity.Nifty, "WaterCooler", "watercooler_green_mush_kanim"),
      new BuildingFacadeInfo("WaterCooler_purple_brainfat", (string) BUILDINGS.PREFABS.WATERCOOLER.FACADES.PURPLE_BRAINFAT.NAME, (string) BUILDINGS.PREFABS.WATERCOOLER.FACADES.PURPLE_BRAINFAT.DESC, PermitRarity.Nifty, "WaterCooler", "watercooler_purple_brainfat_kanim"),
      new BuildingFacadeInfo("WaterCooler_blue_babytears", (string) BUILDINGS.PREFABS.WATERCOOLER.FACADES.BLUE_BABYTEARS.NAME, (string) BUILDINGS.PREFABS.WATERCOOLER.FACADES.BLUE_BABYTEARS.DESC, PermitRarity.Nifty, "WaterCooler", "watercooler_blue_babytears_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_yellow_tartar", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_YELLOW_TARTAR.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_YELLOW_TARTAR.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_yellow_tartar_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_diagonal_yellow_tartar", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_YELLOW_TARTAR.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_YELLOW_TARTAR.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_diagonal_yellow_tartar_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_circle_yellow_tartar", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_YELLOW_TARTAR.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_YELLOW_TARTAR.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_circle_yellow_tartar_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_purple_brainfat", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_PURPLE_BRAINFAT.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_PURPLE_BRAINFAT.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_purple_brainfat_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_diagonal_purple_brainfat", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_PURPLE_BRAINFAT.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_DIAGONAL_PURPLE_BRAINFAT.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_diagonal_purple_brainfat_kanim"),
      new BuildingFacadeInfo("ExteriorWall_stripes_circle_purple_brainfat", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_PURPLE_BRAINFAT.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.STRIPES_CIRCLE_PURPLE_BRAINFAT.DESC, PermitRarity.Decent, "ExteriorWall", "walls_stripes_circle_purple_brainfat_kanim"),
      new BuildingFacadeInfo("ExteriorWall_floppy_azulene_vitro", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_AZULENE_VITRO.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_AZULENE_VITRO.DESC, PermitRarity.Decent, "ExteriorWall", "walls_floppy_azulene_vitro_kanim"),
      new BuildingFacadeInfo("ExteriorWall_floppy_black_white", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_BLACK_WHITE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_BLACK_WHITE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_floppy_black_white_kanim"),
      new BuildingFacadeInfo("ExteriorWall_floppy_peagreen_balmy", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_PEAGREEN_BALMY.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_PEAGREEN_BALMY.DESC, PermitRarity.Decent, "ExteriorWall", "walls_floppy_peagreen_balmy_kanim"),
      new BuildingFacadeInfo("ExteriorWall_floppy_satsuma_yellowcake", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_SATSUMA_YELLOWCAKE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_SATSUMA_YELLOWCAKE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_floppy_satsuma_yellowcake_kanim"),
      new BuildingFacadeInfo("ExteriorWall_floppy_magma_amino", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_MAGMA_AMINO.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.FLOPPY_MAGMA_AMINO.DESC, PermitRarity.Decent, "ExteriorWall", "walls_floppy_magma_amino_kanim"),
      new BuildingFacadeInfo("ExteriorWall_orange_juice", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.ORANGE_JUICE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.ORANGE_JUICE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_orange_juice_kanim"),
      new BuildingFacadeInfo("ExteriorWall_paint_blots", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PAINT_BLOTS.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.PAINT_BLOTS.DESC, PermitRarity.Decent, "ExteriorWall", "walls_paint_blots_kanim"),
      new BuildingFacadeInfo("ExteriorWall_telescope", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TELESCOPE.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TELESCOPE.DESC, PermitRarity.Decent, "ExteriorWall", "walls_telescope_kanim"),
      new BuildingFacadeInfo("ExteriorWall_tictactoe_o", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TICTACTOE_O.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TICTACTOE_O.DESC, PermitRarity.Decent, "ExteriorWall", "walls_tictactoe_o_kanim"),
      new BuildingFacadeInfo("ExteriorWall_tictactoe_x", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TICTACTOE_X.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.TICTACTOE_X.DESC, PermitRarity.Decent, "ExteriorWall", "walls_tictactoe_x_kanim"),
      new BuildingFacadeInfo("ExteriorWall_dice_1", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_1.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_1.DESC, PermitRarity.Decent, "ExteriorWall", "walls_dice_1_kanim"),
      new BuildingFacadeInfo("ExteriorWall_dice_2", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_2.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_2.DESC, PermitRarity.Decent, "ExteriorWall", "walls_dice_2_kanim"),
      new BuildingFacadeInfo("ExteriorWall_dice_3", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_3.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_3.DESC, PermitRarity.Decent, "ExteriorWall", "walls_dice_3_kanim"),
      new BuildingFacadeInfo("ExteriorWall_dice_4", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_4.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_4.DESC, PermitRarity.Decent, "ExteriorWall", "walls_dice_4_kanim"),
      new BuildingFacadeInfo("ExteriorWall_dice_5", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_5.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_5.DESC, PermitRarity.Decent, "ExteriorWall", "walls_dice_5_kanim"),
      new BuildingFacadeInfo("ExteriorWall_dice_6", (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_6.NAME, (string) BUILDINGS.PREFABS.EXTERIORWALL.FACADES.DICE_6.DESC, PermitRarity.Decent, "ExteriorWall", "walls_dice_6_kanim")
    });
  }

  private void SetupArtables()
  {
    this.blueprintCollection.artables.AddRange((IEnumerable<ArtableInfo>) new ArtableInfo[38]
    {
      new ArtableInfo("Canvas_Good7", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_I.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_I.DESC, PermitRarity.Decent, "painting_art_i_kanim", "art_i", 15, true, "LookingGreat", "Canvas", "canvas"),
      new ArtableInfo("Canvas_Good8", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_J.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_J.DESC, PermitRarity.Decent, "painting_art_j_kanim", "art_j", 15, true, "LookingGreat", "Canvas", "canvas"),
      new ArtableInfo("Canvas_Good9", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_K.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_K.DESC, PermitRarity.Decent, "painting_art_k_kanim", "art_k", 15, true, "LookingGreat", "Canvas", "canvas"),
      new ArtableInfo("CanvasTall_Good5", (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_G.NAME, (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_G.DESC, PermitRarity.Decent, "painting_tall_art_g_kanim", "art_g", 15, true, "LookingGreat", "CanvasTall", "canvas"),
      new ArtableInfo("CanvasTall_Good6", (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_H.NAME, (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_H.DESC, PermitRarity.Decent, "painting_tall_art_h_kanim", "art_h", 15, true, "LookingGreat", "CanvasTall", "canvas"),
      new ArtableInfo("CanvasTall_Good7", (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_I.NAME, (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_I.DESC, PermitRarity.Decent, "painting_tall_art_i_kanim", "art_i", 15, true, "LookingGreat", "CanvasTall", "canvas"),
      new ArtableInfo("CanvasWide_Good5", (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_G.NAME, (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_G.DESC, PermitRarity.Decent, "painting_wide_art_g_kanim", "art_g", 15, true, "LookingGreat", "CanvasWide", "canvas"),
      new ArtableInfo("CanvasWide_Good6", (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_H.NAME, (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_H.DESC, PermitRarity.Decent, "painting_wide_art_h_kanim", "art_h", 15, true, "LookingGreat", "CanvasWide", "canvas"),
      new ArtableInfo("CanvasWide_Good7", (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_I.NAME, (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_I.DESC, PermitRarity.Decent, "painting_wide_art_i_kanim", "art_i", 15, true, "LookingGreat", "CanvasWide", "canvas"),
      new ArtableInfo("Sculpture_Good4", (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_4.NAME, (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_4.DESC, PermitRarity.Decent, "sculpture_amazing_4_kanim", "amazing_4", 15, true, "LookingGreat", "Sculpture"),
      new ArtableInfo("SmallSculpture_Good4", (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_4.NAME, (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_4.DESC, PermitRarity.Decent, "sculpture_1x2_amazing_4_kanim", "amazing_4", 15, true, "LookingGreat", "SmallSculpture"),
      new ArtableInfo("MetalSculpture_Good4", (string) BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_4.NAME, (string) BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_4.DESC, PermitRarity.Decent, "sculpture_metal_amazing_4_kanim", "amazing_4", 15, true, "LookingGreat", "MetalSculpture"),
      new ArtableInfo("MarbleSculpture_Good4", (string) BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_4.NAME, (string) BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_4.DESC, PermitRarity.Decent, "sculpture_marble_amazing_4_kanim", "amazing_4", 15, true, "LookingGreat", "MarbleSculpture"),
      new ArtableInfo("MarbleSculpture_Good5", (string) BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_5.NAME, (string) BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_5.DESC, PermitRarity.Decent, "sculpture_marble_amazing_5_kanim", "amazing_5", 15, true, "LookingGreat", "MarbleSculpture"),
      new ArtableInfo("IceSculpture_Average2", (string) BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_AMAZING_2.NAME, (string) BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_AMAZING_2.DESC, PermitRarity.Decent, "icesculpture_idle_2_kanim", "idle_2", 10, false, "LookingOkay", "IceSculpture"),
      new ArtableInfo("Canvas_Good10", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_L.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_L.DESC, PermitRarity.Decent, "painting_art_l_kanim", "art_l", 15, true, "LookingGreat", "Canvas", "canvas"),
      new ArtableInfo("Canvas_Good11", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_M.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_M.DESC, PermitRarity.Decent, "painting_art_m_kanim", "art_m", 15, true, "LookingGreat", "Canvas", "canvas"),
      new ArtableInfo("CanvasTall_Good8", (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_J.NAME, (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_J.DESC, PermitRarity.Decent, "painting_tall_art_j_kanim", "art_j", 15, true, "LookingGreat", "CanvasTall", "canvas"),
      new ArtableInfo("CanvasTall_Good9", (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_K.NAME, (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_K.DESC, PermitRarity.Decent, "painting_tall_art_k_kanim", "art_k", 15, true, "LookingGreat", "CanvasTall", "canvas"),
      new ArtableInfo("CanvasWide_Good8", (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_J.NAME, (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_J.DESC, PermitRarity.Decent, "painting_wide_art_j_kanim", "art_j", 15, true, "LookingGreat", "CanvasWide", "canvas"),
      new ArtableInfo("CanvasWide_Good9", (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_K.NAME, (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_K.DESC, PermitRarity.Decent, "painting_wide_art_k_kanim", "art_k", 15, true, "LookingGreat", "CanvasWide", "canvas"),
      new ArtableInfo("Canvas_Good13", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_O.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_O.DESC, PermitRarity.Decent, "painting_art_o_kanim", "art_o", 15, true, "LookingGreat", "Canvas"),
      new ArtableInfo("CanvasWide_Good10", (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_L.NAME, (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_L.DESC, PermitRarity.Decent, "painting_wide_art_l_kanim", "art_l", 15, true, "LookingGreat", "CanvasWide"),
      new ArtableInfo("CanvasTall_Good11", (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_M.NAME, (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_M.DESC, PermitRarity.Decent, "painting_tall_art_m_kanim", "art_m", 15, true, "LookingGreat", "CanvasTall"),
      new ArtableInfo("Sculpture_Good5", (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_5.NAME, (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_5.DESC, PermitRarity.Decent, "sculpture_amazing_5_kanim", "amazing_5", 15, true, "LookingGreat", "Sculpture"),
      new ArtableInfo("SmallSculpture_Good5", (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_5.NAME, (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_5.DESC, PermitRarity.Decent, "sculpture_1x2_amazing_5_kanim", "amazing_5", 15, true, "LookingGreat", "SmallSculpture"),
      new ArtableInfo("SmallSculpture_Good6", (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_6.NAME, (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_6.DESC, PermitRarity.Decent, "sculpture_1x2_amazing_6_kanim", "amazing_6", 15, true, "LookingGreat", "SmallSculpture"),
      new ArtableInfo("MetalSculpture_Good5", (string) BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_5.NAME, (string) BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_5.DESC, PermitRarity.Decent, "sculpture_metal_amazing_5_kanim", "amazing_5", 15, true, "LookingGreat", "MetalSculpture"),
      new ArtableInfo("IceSculpture_Average3", (string) BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_AMAZING_3.NAME, (string) BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_AMAZING_3.DESC, PermitRarity.Decent, "icesculpture_idle_3_kanim", "idle_3", 10, true, "LookingOkay", "IceSculpture"),
      new ArtableInfo("Canvas_Good12", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_N.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_N.DESC, PermitRarity.Decent, "painting_art_n_kanim", "art_n", 15, true, "LookingGreat", "Canvas"),
      new ArtableInfo("Canvas_Good14", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_P.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_P.DESC, PermitRarity.Decent, "painting_art_p_kanim", "art_p", 15, true, "LookingGreat", "Canvas"),
      new ArtableInfo("CanvasWide_Good11", (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_M.NAME, (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_M.DESC, PermitRarity.Decent, "painting_wide_art_m_kanim", "art_m", 15, true, "LookingGreat", "CanvasWide"),
      new ArtableInfo("CanvasTall_Good10", (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_L.NAME, (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_L.DESC, PermitRarity.Decent, "painting_tall_art_l_kanim", "art_l", 15, true, "LookingGreat", "CanvasTall"),
      new ArtableInfo("Sculpture_Good6", (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_6.NAME, (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_6.DESC, PermitRarity.Decent, "sculpture_amazing_6_kanim", "amazing_6", 15, true, "LookingGreat", "Sculpture"),
      new ArtableInfo("Canvas_Good15", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_Q.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_Q.DESC, PermitRarity.Decent, "painting_art_q_kanim", "art_q", 15, true, "LookingGreat", "Canvas"),
      new ArtableInfo("CanvasTall_Good14", (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_P.NAME, (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_P.DESC, PermitRarity.Decent, "painting_tall_art_p_kanim", "art_p", 15, true, "LookingGreat", "CanvasTall"),
      new ArtableInfo("Canvas_Good16", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_R.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_R.DESC, PermitRarity.Decent, "painting_art_r_kanim", "art_r", 15, true, "LookingGreat", "Canvas"),
      new ArtableInfo("CanvasWide_Good13", (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_O.NAME, (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_O.DESC, PermitRarity.Decent, "painting_wide_art_o_kanim", "art_o", 15, true, "LookingGreat", "CanvasWide")
    });
  }

  private void SetupClothingItems()
  {
    this.blueprintCollection.clothingItems.AddRange((IEnumerable<ClothingItemInfo>) new ClothingItemInfo[294]
    {
      new ClothingItemInfo("TopStandardYellow", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.STANDARD_YELLOW_TOP.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.STANDARD_YELLOW_TOP.DESC, PermitCategory.DupeTops, PermitRarity.Universal, "top_standard_yellow_kanim"),
      new ClothingItemInfo("TopStandardRed", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.STANDARD_RED_TOP.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.STANDARD_RED_TOP.DESC, PermitCategory.DupeTops, PermitRarity.Universal, "top_standard_red_kanim"),
      new ClothingItemInfo("TopStandardBlue", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.STANDARD_BLUE_TOP.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.STANDARD_BLUE_TOP.DESC, PermitCategory.DupeTops, PermitRarity.Universal, "top_standard_blue_kanim"),
      new ClothingItemInfo("TopStandardGreen", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.STANDARD_GREEN_TOP.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.STANDARD_GREEN_TOP.DESC, PermitCategory.DupeTops, PermitRarity.Universal, "top_standard_green_kanim"),
      new ClothingItemInfo("GlovesStandardGold", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.STANDARD_GOLD.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.STANDARD_GOLD.DESC, PermitCategory.DupeGloves, PermitRarity.Universal, "gloves_basic_gold_kanim"),
      new ClothingItemInfo("TopBasicBlack", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_BLACK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_BLACK.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_black_kanim"),
      new ClothingItemInfo("TopBasicWhite", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_WHITE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_WHITE.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_white_kanim"),
      new ClothingItemInfo("TopBasicRed", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_RED_BURNT.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_RED_BURNT.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_red_kanim"),
      new ClothingItemInfo("TopBasicOrange", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_ORANGE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_ORANGE.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_orange_kanim"),
      new ClothingItemInfo("TopBasicYellow", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_YELLOW.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_YELLOW.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_yellow_kanim"),
      new ClothingItemInfo("TopBasicGreen", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_GREEN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_GREEN.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_green_kanim"),
      new ClothingItemInfo("TopBasicAqua", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_BLUE_MIDDLE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_BLUE_MIDDLE.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_blue_middle_kanim"),
      new ClothingItemInfo("TopBasicPurple", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_PURPLE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_PURPLE.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_purple_kanim"),
      new ClothingItemInfo("TopBasicPinkOrchid", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_PINK_ORCHID.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BASIC_PINK_ORCHID.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_basic_pink_orchid_kanim"),
      new ClothingItemInfo("BottomBasicBlack", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_BLACK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_BLACK.DESC, PermitCategory.DupeBottoms, PermitRarity.Universal, "pants_basic_black_kanim"),
      new ClothingItemInfo("BottomBasicWhite", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_WHITE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_WHITE.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_white_kanim"),
      new ClothingItemInfo("BottomBasicRed", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_RED.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_RED.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_red_kanim"),
      new ClothingItemInfo("BottomBasicOrange", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_ORANGE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_ORANGE.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_orange_kanim"),
      new ClothingItemInfo("BottomBasicYellow", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_YELLOW.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_YELLOW.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_yellow_kanim"),
      new ClothingItemInfo("BottomBasicGreen", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_GREEN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_GREEN.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_green_kanim"),
      new ClothingItemInfo("BottomBasicAqua", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_BLUE_MIDDLE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_BLUE_MIDDLE.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_blue_middle_kanim"),
      new ClothingItemInfo("BottomBasicPurple", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_PURPLE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_PURPLE.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_purple_kanim"),
      new ClothingItemInfo("BottomBasicPinkOrchid", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_PINK_ORCHID.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_PINK_ORCHID.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_pink_orchid_kanim"),
      new ClothingItemInfo("GlovesBasicBlack", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BLACK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BLACK.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_black_kanim"),
      new ClothingItemInfo("GlovesBasicWhite", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_WHITE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_WHITE.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_white_kanim"),
      new ClothingItemInfo("GlovesBasicRed", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_RED.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_RED.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_red_kanim"),
      new ClothingItemInfo("GlovesBasicOrange", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_ORANGE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_ORANGE.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_orange_kanim"),
      new ClothingItemInfo("GlovesBasicYellow", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_YELLOW.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_YELLOW.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_yellow_kanim"),
      new ClothingItemInfo("GlovesBasicGreen", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_GREEN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_GREEN.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_green_kanim"),
      new ClothingItemInfo("GlovesBasicAqua", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BLUE_MIDDLE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BLUE_MIDDLE.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_blue_middle_kanim"),
      new ClothingItemInfo("GlovesBasicPurple", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_PURPLE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_PURPLE.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_purple_kanim"),
      new ClothingItemInfo("GlovesBasicPinkOrchid", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_PINK_ORCHID.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_PINK_ORCHID.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_pink_orchid_kanim"),
      new ClothingItemInfo("ShoesBasicBlack", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_BLACK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_BLACK.DESC, PermitCategory.DupeShoes, PermitRarity.Universal, "shoes_basic_black_kanim"),
      new ClothingItemInfo("ShoesBasicWhite", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_WHITE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_WHITE.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_white_kanim"),
      new ClothingItemInfo("ShoesBasicRed", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_RED.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_RED.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_red_kanim"),
      new ClothingItemInfo("ShoesBasicOrange", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_ORANGE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_ORANGE.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_orange_kanim"),
      new ClothingItemInfo("ShoesBasicYellow", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_YELLOW.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_YELLOW.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_yellow_kanim"),
      new ClothingItemInfo("ShoesBasicGreen", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_GREEN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_GREEN.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_green_kanim"),
      new ClothingItemInfo("ShoesBasicAqua", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_BLUE_MIDDLE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_BLUE_MIDDLE.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_blue_middle_kanim"),
      new ClothingItemInfo("ShoesBasicPurple", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_PURPLE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_PURPLE.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_purple_kanim"),
      new ClothingItemInfo("ShoesBasicPinkOrchid", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_PINK_ORCHID.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_PINK_ORCHID.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_pink_orchid_kanim"),
      new ClothingItemInfo("TopRaglanDeepRed", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_DEEPRED.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_DEEPRED.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_raglan_deepred_kanim"),
      new ClothingItemInfo("TopRaglanCobalt", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_COBALT.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_COBALT.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_raglan_cobalt_kanim"),
      new ClothingItemInfo("TopRaglanFlamingo", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_FLAMINGO.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_FLAMINGO.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_raglan_flamingo_kanim"),
      new ClothingItemInfo("TopRaglanKellyGreen", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_KELLYGREEN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_KELLYGREEN.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_raglan_kellygreen_kanim"),
      new ClothingItemInfo("TopRaglanCharcoal", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_CHARCOAL.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_CHARCOAL.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_raglan_charcoal_kanim"),
      new ClothingItemInfo("TopRaglanLemon", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_LEMON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_LEMON.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_raglan_lemon_kanim"),
      new ClothingItemInfo("TopRaglanSatsuma", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_SATSUMA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.RAGLANTOP_SATSUMA.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_raglan_satsuma_kanim"),
      new ClothingItemInfo("ShortsBasicDeepRed", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_DEEPRED.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_DEEPRED.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "shorts_basic_deepred_kanim"),
      new ClothingItemInfo("ShortsBasicSatsuma", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_SATSUMA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_SATSUMA.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "shorts_basic_satsuma_kanim"),
      new ClothingItemInfo("ShortsBasicYellowcake", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_YELLOWCAKE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_YELLOWCAKE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "shorts_basic_yellowcake_kanim"),
      new ClothingItemInfo("ShortsBasicKellyGreen", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_KELLYGREEN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_KELLYGREEN.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "shorts_basic_kellygreen_kanim"),
      new ClothingItemInfo("ShortsBasicBlueCobalt", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_BLUE_COBALT.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_BLUE_COBALT.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "shorts_basic_blue_cobalt_kanim"),
      new ClothingItemInfo("ShortsBasicPinkFlamingo", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_PINK_FLAMINGO.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_PINK_FLAMINGO.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "shorts_basic_pink_flamingo_kanim"),
      new ClothingItemInfo("ShortsBasicCharcoal", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_CHARCOAL.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SHORTS_BASIC_CHARCOAL.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "shorts_basic_charcoal_kanim"),
      new ClothingItemInfo("SocksAthleticDeepRed", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_DEEPRED.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_DEEPRED.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_athletic_red_deep_kanim"),
      new ClothingItemInfo("SocksAthleticOrangeSatsuma", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_SATSUMA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_SATSUMA.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_athletic_orange_satsuma_kanim"),
      new ClothingItemInfo("SocksAthleticYellowLemon", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_LEMON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_LEMON.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_athletic_yellow_lemon_kanim"),
      new ClothingItemInfo("SocksAthleticGreenKelly", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_KELLYGREEN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_KELLYGREEN.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_athletic_green_kelly_kanim"),
      new ClothingItemInfo("SocksAthleticBlueCobalt", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_COBALT.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_COBALT.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_athletic_blue_cobalt_kanim"),
      new ClothingItemInfo("SocksAthleticPinkFlamingo", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_FLAMINGO.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_FLAMINGO.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_athletic_pink_flamingo_kanim"),
      new ClothingItemInfo("SocksAthleticGreyCharcoal", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_CHARCOAL.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.SOCKS_ATHLETIC_CHARCOAL.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_athletic_grey_charcoal_kanim"),
      new ClothingItemInfo("GlovesAthleticRedDeep", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_DEEPRED.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_DEEPRED.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_athletic_red_deep_kanim"),
      new ClothingItemInfo("GlovesAthleticOrangeSatsuma", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_SATSUMA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_SATSUMA.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_athletic_orange_satsuma_kanim"),
      new ClothingItemInfo("GlovesAthleticYellowLemon", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_LEMON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_LEMON.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_athletic_yellow_lemon_kanim"),
      new ClothingItemInfo("GlovesAthleticGreenKelly", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_KELLYGREEN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_KELLYGREEN.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_athletic_green_kelly_kanim"),
      new ClothingItemInfo("GlovesAthleticBlueCobalt", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_COBALT.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_COBALT.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_athletic_blue_cobalt_kanim"),
      new ClothingItemInfo("GlovesAthleticPinkFlamingo", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_FLAMINGO.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_FLAMINGO.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_athletic_pink_flamingo_kanim"),
      new ClothingItemInfo("GlovesAthleticGreyCharcoal", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_CHARCOAL.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GLOVES_ATHLETIC_CHARCOAL.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_athletic_grey_charcoal_kanim"),
      new ClothingItemInfo("TopJellypuffJacketBlueberry", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_BLUEBERRY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_BLUEBERRY.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jellypuffjacket_blueberry_kanim"),
      new ClothingItemInfo("TopJellypuffJacketGrape", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_GRAPE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_GRAPE.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jellypuffjacket_grape_kanim"),
      new ClothingItemInfo("TopJellypuffJacketLemon", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_LEMON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_LEMON.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jellypuffjacket_lemon_kanim"),
      new ClothingItemInfo("TopJellypuffJacketLime", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_LIME.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_LIME.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jellypuffjacket_lime_kanim"),
      new ClothingItemInfo("TopJellypuffJacketSatsuma", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_SATSUMA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_SATSUMA.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jellypuffjacket_satsuma_kanim"),
      new ClothingItemInfo("TopJellypuffJacketStrawberry", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_STRAWBERRY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_STRAWBERRY.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jellypuffjacket_strawberry_kanim"),
      new ClothingItemInfo("TopJellypuffJacketWatermelon", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_WATERMELON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JELLYPUFFJACKET_WATERMELON.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jellypuffjacket_watermelon_kanim"),
      new ClothingItemInfo("GlovesCufflessBlueberry", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_BLUEBERRY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_BLUEBERRY.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_blueberry_kanim"),
      new ClothingItemInfo("GlovesCufflessGrape", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_GRAPE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_GRAPE.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_grape_kanim"),
      new ClothingItemInfo("GlovesCufflessLemon", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_LEMON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_LEMON.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_lemon_kanim"),
      new ClothingItemInfo("GlovesCufflessLime", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_LIME.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_LIME.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_lime_kanim"),
      new ClothingItemInfo("GlovesCufflessSatsuma", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_SATSUMA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_SATSUMA.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_satsuma_kanim"),
      new ClothingItemInfo("GlovesCufflessStrawberry", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_STRAWBERRY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_STRAWBERRY.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_strawberry_kanim"),
      new ClothingItemInfo("GlovesCufflessWatermelon", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_WATERMELON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_WATERMELON.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_watermelon_kanim"),
      new ClothingItemInfo("visonly_AtmoHelmetClear", (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Universal, "atmo_helmet_clear_kanim"),
      new ClothingItemInfo("visonly_AtmoSuitBasicBlue", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Universal, "atmosuit_basic_blue_kanim"),
      new ClothingItemInfo("visonly_AtmoGlovesBasicBlue", (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Universal, "atmo_gloves_blue_kanim"),
      new ClothingItemInfo("visonly_AtmoBeltBasicBlue", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Universal, "atmo_belt_basic_blue_kanim"),
      new ClothingItemInfo("visonly_AtmoShoesBasicBlack", (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Universal, "atmo_shoes_basic_black_kanim"),
      new ClothingItemInfo("visonly_JetHelmetClear", "_STRINGS.EQUIPMENT.PREFABS.JET_SUIT_HELMET.NAME", "_STRINGS.EQUIPMENT.PREFABS.JET_SUIT_HELMET.DESC", PermitCategory.JetSuitHelmet, PermitRarity.Universal, "jet_helmet_00_default_kanim"),
      new ClothingItemInfo("visonly_JetSuitBasic", "_STRINGS.EQUIPMENT.PREFABS.JET_SUIT_BODY.NAME", "_STRINGS.EQUIPMENT.PREFABS.JET_SUIT_BODY.DESC", PermitCategory.JetSuitBody, PermitRarity.Universal, "jetsuit_black_default_kanim"),
      new ClothingItemInfo("visonly_JetGlovesBasic", "_STRINGS.EQUIPMENT.PREFABS.JET_SUIT_GLOVES.NAME", "_STRINGS.EQUIPMENT.PREFABS.JET_SUIT_GLOVES.DESC", PermitCategory.JetSuitGloves, PermitRarity.Universal, "jet_gloves_default_kanim"),
      new ClothingItemInfo("visonly_JetShoesBasic", "_STRINGS.EQUIPMENT.PREFABS.JET_SUIT_SHOES.NAME", "_STRINGS.EQUIPMENT.PREFABS.JET_SUIT_SHOES.DESC", PermitCategory.JetSuitShoes, PermitRarity.Universal, "jet_shoes_basic_black_kanim"),
      new ClothingItemInfo("AtmoHelmetLimone", (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.LIMONE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.LIMONE.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Universal, "atmo_helmet_limone_lime_kanim"),
      new ClothingItemInfo("AtmoSuitBasicYellow", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.LIMONE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.LIMONE.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Universal, "atmosuit_basic_yellow_kanim"),
      new ClothingItemInfo("AtmoGlovesLime", (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.LIMONE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.LIMONE.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Universal, "atmo_gloves_lime_kanim"),
      new ClothingItemInfo("AtmoBeltBasicLime", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.LIMONE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.LIMONE.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Universal, "atmo_belt_basic_lime_kanim"),
      new ClothingItemInfo("AtmoShoesBasicYellow", (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.LIMONE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.LIMONE.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Universal, "atmo_shoes_basic_yellow_kanim"),
      new ClothingItemInfo("AtmoHelmetPuft", (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.PUFT.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.PUFT.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Loyalty, "atmo_helmet_puft_kanim"),
      new ClothingItemInfo("AtmoSuitPuft", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.PUFT.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.PUFT.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Loyalty, "atmosuit_puft_kanim"),
      new ClothingItemInfo("AtmoGlovesPuft", (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.PUFT.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.PUFT.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Loyalty, "atmo_gloves_puft_kanim"),
      new ClothingItemInfo("AtmoBeltPuft", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.PUFT.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.PUFT.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Loyalty, "atmo_belt_puft_kanim"),
      new ClothingItemInfo("AtmoShoesPuft", (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.PUFT.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.PUFT.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Loyalty, "atmo_shoes_puft_kanim"),
      new ClothingItemInfo("TopTShirtWhite", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.TSHIRT_WHITE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.TSHIRT_WHITE.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_tshirt_white_kanim"),
      new ClothingItemInfo("TopTShirtMagenta", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.TSHIRT_MAGENTA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.TSHIRT_MAGENTA.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_tshirt_magenta_kanim"),
      new ClothingItemInfo("TopAthlete", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.ATHLETE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.ATHLETE.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_athlete_kanim"),
      new ClothingItemInfo("TopCircuitGreen", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.CIRCUIT_GREEN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.CIRCUIT_GREEN.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_circuit_green_kanim"),
      new ClothingItemInfo("GlovesBasicBlueGrey", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BLUEGREY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BLUEGREY.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_bluegrey_kanim"),
      new ClothingItemInfo("GlovesBasicBrownKhaki", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BROWN_KHAKI.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_BROWN_KHAKI.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_brown_khaki_kanim"),
      new ClothingItemInfo("GlovesAthlete", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.ATHLETE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.ATHLETE.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_athlete_kanim"),
      new ClothingItemInfo("GlovesCircuitGreen", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CIRCUIT_GREEN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CIRCUIT_GREEN.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_circuit_green_kanim"),
      new ClothingItemInfo("PantsBasicRedOrange", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_REDORANGE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_REDORANGE.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_redorange_kanim"),
      new ClothingItemInfo("PantsBasicLightBrown", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_LIGHTBROWN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_LIGHTBROWN.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_lightbrown_kanim"),
      new ClothingItemInfo("PantsAthlete", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.ATHLETE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.ATHLETE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "pants_athlete_kanim"),
      new ClothingItemInfo("PantsCircuitGreen", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.CIRCUIT_GREEN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.CIRCUIT_GREEN.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "pants_circuit_green_kanim"),
      new ClothingItemInfo("ShoesBasicBlueGrey", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_BLUEGREY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_BLUEGREY.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_bluegrey_kanim"),
      new ClothingItemInfo("ShoesBasicTan", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_TAN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_TAN.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_tan_kanim"),
      new ClothingItemInfo("AtmoHelmetSparkleRed", (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_RED.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_RED.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_sparkle_red_kanim"),
      new ClothingItemInfo("AtmoHelmetSparkleGreen", (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_GREEN.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_GREEN.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_sparkle_green_kanim"),
      new ClothingItemInfo("AtmoHelmetSparkleBlue", (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_BLUE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_BLUE.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_sparkle_blue_kanim"),
      new ClothingItemInfo("AtmoHelmetSparklePurple", (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_PURPLE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.SPARKLE_PURPLE.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_sparkle_purple_kanim"),
      new ClothingItemInfo("AtmoSuitSparkleRed", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_RED.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_RED.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_sparkle_red_kanim"),
      new ClothingItemInfo("AtmoSuitSparkleGreen", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_GREEN.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_GREEN.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_sparkle_green_kanim"),
      new ClothingItemInfo("AtmoSuitSparkleBlue", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_BLUE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_BLUE.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_sparkle_blue_kanim"),
      new ClothingItemInfo("AtmoSuitSparkleLavender", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_LAVENDER.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.SPARKLE_LAVENDER.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_sparkle_lavender_kanim"),
      new ClothingItemInfo("AtmoGlovesSparkleRed", (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_RED.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_RED.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_sparkle_red_kanim"),
      new ClothingItemInfo("AtmoGlovesSparkleGreen", (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_GREEN.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_GREEN.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_sparkle_green_kanim"),
      new ClothingItemInfo("AtmoGlovesSparkleBlue", (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_BLUE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_BLUE.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_sparkle_blue_kanim"),
      new ClothingItemInfo("AtmoGlovesSparkleLavender", (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_LAVENDER.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.SPARKLE_LAVENDER.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_sparkle_lavender_kanim"),
      new ClothingItemInfo("AtmoBeltSparkleRed", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_RED.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_RED.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Splendid, "atmo_belt_sparkle_red_kanim"),
      new ClothingItemInfo("AtmoBeltSparkleGreen", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_GREEN.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_GREEN.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Splendid, "atmo_belt_sparkle_green_kanim"),
      new ClothingItemInfo("AtmoBeltSparkleBlue", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_BLUE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_BLUE.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Splendid, "atmo_belt_sparkle_blue_kanim"),
      new ClothingItemInfo("AtmoBeltSparkleLavender", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_LAVENDER.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.SPARKLE_LAVENDER.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Splendid, "atmo_belt_sparkle_lavender_kanim"),
      new ClothingItemInfo("AtmoShoesSparkleBlack", (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.SPARKLE_BLACK.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.SPARKLE_BLACK.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Common, "atmo_shoes_sparkle_black_kanim"),
      new ClothingItemInfo("TopDenimBlue", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.DENIM_BLUE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.DENIM_BLUE.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_denim_blue_kanim"),
      new ClothingItemInfo("TopUndershirtExecutive", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_STRAWBERRY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_STRAWBERRY.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_gonch_strawberry_kanim"),
      new ClothingItemInfo("TopUndershirtUnderling", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_SATSUMA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_SATSUMA.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_gonch_satsuma_kanim"),
      new ClothingItemInfo("TopUndershirtGroupthink", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_LEMON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_LEMON.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_gonch_lemon_kanim"),
      new ClothingItemInfo("TopUndershirtStakeholder", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_LIME.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_LIME.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_gonch_lime_kanim"),
      new ClothingItemInfo("TopUndershirtAdmin", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_BLUEBERRY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_BLUEBERRY.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_gonch_blueberry_kanim"),
      new ClothingItemInfo("TopUndershirtBuzzword", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_GRAPE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_GRAPE.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_gonch_grape_kanim"),
      new ClothingItemInfo("TopUndershirtSynergy", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_WATERMELON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GONCH_WATERMELON.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_gonch_watermelon_kanim"),
      new ClothingItemInfo("TopResearcher", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.NERD_BROWN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.NERD_BROWN.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_nerd_white_cream_kanim"),
      new ClothingItemInfo("TopRebelGi", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GI_WHITE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GI_WHITE.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_gi_white_kanim"),
      new ClothingItemInfo("BottomBriefsExecutive", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_STRAWBERRY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_STRAWBERRY.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_gonch_strawberry_kanim"),
      new ClothingItemInfo("BottomBriefsUnderling", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_SATSUMA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_SATSUMA.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_gonch_satsuma_kanim"),
      new ClothingItemInfo("BottomBriefsGroupthink", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_LEMON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_LEMON.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_gonch_lemon_kanim"),
      new ClothingItemInfo("BottomBriefsStakeholder", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_LIME.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_LIME.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_gonch_lime_kanim"),
      new ClothingItemInfo("BottomBriefsAdmin", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_BLUEBERRY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_BLUEBERRY.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_gonch_blueberry_kanim"),
      new ClothingItemInfo("BottomBriefsBuzzword", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_GRAPE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_GRAPE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_gonch_grape_kanim"),
      new ClothingItemInfo("BottomBriefsSynergy", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_WATERMELON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GONCH_WATERMELON.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_gonch_watermelon_kanim"),
      new ClothingItemInfo("PantsJeans", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.DENIM_BLUE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.DENIM_BLUE.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "pants_denim_blue_kanim"),
      new ClothingItemInfo("PantsRebelGi", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GI_WHITE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GI_WHITE.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "pants_gi_white_kanim"),
      new ClothingItemInfo("PantsResearch", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.NERD_BROWN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.NERD_BROWN.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "pants_nerd_brown_kanim"),
      new ClothingItemInfo("ShoesBasicGray", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_GREY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BASIC_GREY.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "shoes_basic_grey_kanim"),
      new ClothingItemInfo("ShoesDenimBlue", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.DENIM_BLUE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.DENIM_BLUE.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_denim_blue_kanim"),
      new ClothingItemInfo("SocksLegwarmersBlueberry", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_BLUEBERRY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_BLUEBERRY.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "socks_legwarmers_blueberry_kanim"),
      new ClothingItemInfo("SocksLegwarmersGrape", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_GRAPE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_GRAPE.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "socks_legwarmers_grape_kanim"),
      new ClothingItemInfo("SocksLegwarmersLemon", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_LEMON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_LEMON.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "socks_legwarmers_lemon_kanim"),
      new ClothingItemInfo("SocksLegwarmersLime", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_LIME.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_LIME.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "socks_legwarmers_lime_kanim"),
      new ClothingItemInfo("SocksLegwarmersSatsuma", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_SATSUMA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_SATSUMA.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "socks_legwarmers_satsuma_kanim"),
      new ClothingItemInfo("SocksLegwarmersStrawberry", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_STRAWBERRY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_STRAWBERRY.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "socks_legwarmers_strawberry_kanim"),
      new ClothingItemInfo("SocksLegwarmersWatermelon", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_WATERMELON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.LEGWARMERS_WATERMELON.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "socks_legwarmers_watermelon_kanim"),
      new ClothingItemInfo("GlovesCufflessBlack", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_BLACK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.CUFFLESS_BLACK.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_cuffless_black_kanim"),
      new ClothingItemInfo("GlovesDenimBlue", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.DENIM_BLUE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.DENIM_BLUE.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_denim_blue_kanim"),
      new ClothingItemInfo("AtmoGlovesGold", (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.GOLD.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.GOLD.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_gold_kanim"),
      new ClothingItemInfo("AtmoGlovesEggplant", (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.PURPLE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.PURPLE.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_purple_kanim"),
      new ClothingItemInfo("AtmoHelmetEggplant", (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.CLUBSHIRT_PURPLE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.CLUBSHIRT_PURPLE.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_clubshirt_purple_kanim"),
      new ClothingItemInfo("AtmoHelmetConfetti", (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.TRIANGLES_TURQ.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.TRIANGLES_TURQ.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_triangles_turq_kanim"),
      new ClothingItemInfo("AtmoShoesStealth", (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.BASIC_BLACK.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.BASIC_BLACK.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Common, "atmo_shoes_basic_black_kanim"),
      new ClothingItemInfo("AtmoShoesEggplant", (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.BASIC_PURPLE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.BASIC_PURPLE.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Common, "atmo_shoes_basic_purple_kanim"),
      new ClothingItemInfo("AtmoSuitCrispEggplant", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.BASIC_PURPLE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.BASIC_PURPLE.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Nifty, "atmosuit_basic_purple_kanim"),
      new ClothingItemInfo("AtmoSuitConfetti", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.PRINT_TRIANGLES_TURQ.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.PRINT_TRIANGLES_TURQ.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_print_triangles_turq_kanim"),
      new ClothingItemInfo("AtmoBeltBasicGold", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.BASIC_GOLD.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.BASIC_GOLD.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Nifty, "atmo_belt_basic_gold_kanim"),
      new ClothingItemInfo("AtmoBeltEggplant", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.TWOTONE_PURPLE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.TWOTONE_PURPLE.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Nifty, "atmo_belt_2tone_purple_kanim"),
      new ClothingItemInfo("SkirtBasicBlueMiddle", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_BLUE_MIDDLE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_BLUE_MIDDLE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "skirt_basic_blue_middle_kanim"),
      new ClothingItemInfo("SkirtBasicPurple", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_PURPLE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_PURPLE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "skirt_basic_purple_kanim"),
      new ClothingItemInfo("SkirtBasicGreen", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_GREEN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_GREEN.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "skirt_basic_green_kanim"),
      new ClothingItemInfo("SkirtBasicOrange", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_ORANGE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_ORANGE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "skirt_basic_orange_kanim"),
      new ClothingItemInfo("SkirtBasicPinkOrchid", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_PINK_ORCHID.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_PINK_ORCHID.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "skirt_basic_pink_orchid_kanim"),
      new ClothingItemInfo("SkirtBasicRed", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_RED.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_RED.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "skirt_basic_red_kanim"),
      new ClothingItemInfo("SkirtBasicYellow", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_YELLOW.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_YELLOW.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "skirt_basic_yellow_kanim"),
      new ClothingItemInfo("SkirtBasicPolkadot", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_POLKADOT.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_POLKADOT.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "skirt_basic_polkadot_kanim"),
      new ClothingItemInfo("SkirtBasicWatermelon", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_WATERMELON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BASIC_WATERMELON.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "skirt_basic_watermelon_kanim"),
      new ClothingItemInfo("SkirtDenimBlue", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_DENIM_BLUE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_DENIM_BLUE.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "skirt_denim_blue_kanim"),
      new ClothingItemInfo("SkirtLeopardPrintBluePink", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_LEOPARD_PRINT_BLUE_PINK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_LEOPARD_PRINT_BLUE_PINK.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "skirt_leopard_print_blue_pink_kanim"),
      new ClothingItemInfo("SkirtSparkleBlue", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_SPARKLE_BLUE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_SPARKLE_BLUE.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "skirt_sparkle_blue_kanim"),
      new ClothingItemInfo("AtmoBeltBasicGrey", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.BASIC_GREY.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.BASIC_GREY.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Nifty, "atmo_belt_basic_grey_kanim"),
      new ClothingItemInfo("AtmoBeltBasicNeonPink", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.BASIC_NEON_PINK.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.BASIC_NEON_PINK.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Nifty, "atmo_belt_basic_neon_pink_kanim"),
      new ClothingItemInfo("AtmoGlovesWhite", (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.WHITE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.WHITE.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_white_kanim"),
      new ClothingItemInfo("AtmoGlovesStripesLavender", (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.STRIPES_LAVENDER.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.STRIPES_LAVENDER.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_stripes_lavender_kanim"),
      new ClothingItemInfo("AtmoHelmetCummerbundRed", (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.CUMMERBUND_RED.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.CUMMERBUND_RED.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_cummerbund_red_kanim"),
      new ClothingItemInfo("AtmoHelmetWorkoutLavender", (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.WORKOUT_LAVENDER.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.WORKOUT_LAVENDER.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_workout_lavender_kanim"),
      new ClothingItemInfo("AtmoShoesBasicLavender", (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.BASIC_LAVENDER.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.BASIC_LAVENDER.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Common, "atmo_shoes_basic_lavender_kanim"),
      new ClothingItemInfo("AtmoSuitBasicNeonPink", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.BASIC_NEON_PINK.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.BASIC_NEON_PINK.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Nifty, "atmosuit_basic_neon_pink_kanim"),
      new ClothingItemInfo("AtmoSuitMultiRedBlack", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.MULTI_RED_BLACK.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.MULTI_RED_BLACK.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_multi_red_black_kanim"),
      new ClothingItemInfo("TopJacketSmokingBurgundy", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JACKET_SMOKING_BURGUNDY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.JACKET_SMOKING_BURGUNDY.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_jacket_smoking_burgundy_kanim"),
      new ClothingItemInfo("TopMechanic", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.MECHANIC.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.MECHANIC.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_mechanic_kanim"),
      new ClothingItemInfo("TopVelourBlack", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.VELOUR_BLACK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.VELOUR_BLACK.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_velour_black_kanim"),
      new ClothingItemInfo("TopVelourBlue", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.VELOUR_BLUE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.VELOUR_BLUE.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_velour_blue_kanim"),
      new ClothingItemInfo("TopVelourPink", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.VELOUR_PINK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.VELOUR_PINK.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_velour_pink_kanim"),
      new ClothingItemInfo("TopWaistcoatPinstripeSlate", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.WAISTCOAT_PINSTRIPE_SLATE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.WAISTCOAT_PINSTRIPE_SLATE.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_waistcoat_pinstripe_slate_kanim"),
      new ClothingItemInfo("TopWater", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.WATER.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.WATER.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_water_kanim"),
      new ClothingItemInfo("TopTweedPinkOrchid", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.TWEED_PINK_ORCHID.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.TWEED_PINK_ORCHID.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_tweed_pink_orchid_kanim"),
      new ClothingItemInfo("DressSleevelessBowBw", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.DRESS_SLEEVELESS_BOW_BW.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.DRESS_SLEEVELESS_BOW_BW.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "dress_sleeveless_bow_bw_kanim"),
      new ClothingItemInfo("BodysuitBallerinaPink", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BODYSUIT_BALLERINA_PINK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BODYSUIT_BALLERINA_PINK.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "bodysuit_ballerina_pink_kanim"),
      new ClothingItemInfo("PantsBasicOrangeSatsuma", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_ORANGE_SATSUMA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BASIC_ORANGE_SATSUMA.DESC, PermitCategory.DupeBottoms, PermitRarity.Common, "pants_basic_orange_satsuma_kanim"),
      new ClothingItemInfo("PantsPinstripeSlate", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.PINSTRIPE_SLATE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.PINSTRIPE_SLATE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "pants_pinstripe_slate_kanim"),
      new ClothingItemInfo("PantsVelourBlack", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.VELOUR_BLACK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.VELOUR_BLACK.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "pants_velour_black_kanim"),
      new ClothingItemInfo("PantsVelourBlue", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.VELOUR_BLUE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.VELOUR_BLUE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "pants_velour_blue_kanim"),
      new ClothingItemInfo("PantsVelourPink", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.VELOUR_PINK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.VELOUR_PINK.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "pants_velour_pink_kanim"),
      new ClothingItemInfo("SkirtBallerinaPink", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BALLERINA_PINK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_BALLERINA_PINK.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "skirt_ballerina_pink_kanim"),
      new ClothingItemInfo("SkirtTweedPinkOrchid", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_TWEED_PINK_ORCHID.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.SKIRT_TWEED_PINK_ORCHID.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "skirt_tweed_pink_orchid_kanim"),
      new ClothingItemInfo("ShoesBallerinaPink", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BALLERINA_PINK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.BALLERINA_PINK.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_ballerina_pink_kanim"),
      new ClothingItemInfo("ShoesMaryjaneSocksBw", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.MARYJANE_SOCKS_BW.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.MARYJANE_SOCKS_BW.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_maryjane_socks_bw_kanim"),
      new ClothingItemInfo("ShoesClassicFlatsCreamCharcoal", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.CLASSICFLATS_CREAM_CHARCOAL.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.CLASSICFLATS_CREAM_CHARCOAL.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_classicflats_cream_charcoal_kanim"),
      new ClothingItemInfo("ShoesVelourBlue", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.VELOUR_BLUE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.VELOUR_BLUE.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_velour_blue_kanim"),
      new ClothingItemInfo("ShoesVelourPink", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.VELOUR_PINK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.VELOUR_PINK.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_velour_pink_kanim"),
      new ClothingItemInfo("ShoesVelourBlack", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.VELOUR_BLACK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.VELOUR_BLACK.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_velour_black_kanim"),
      new ClothingItemInfo("GlovesBasicGrey", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_GREY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_GREY.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_grey_kanim"),
      new ClothingItemInfo("GlovesBasicPinksalmon", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_PINKSALMON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_PINKSALMON.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_pinksalmon_kanim"),
      new ClothingItemInfo("GlovesBasicTan", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_TAN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_TAN.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_tan_kanim"),
      new ClothingItemInfo("GlovesBallerinaPink", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BALLERINA_PINK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BALLERINA_PINK.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_ballerina_pink_kanim"),
      new ClothingItemInfo("GlovesFormalWhite", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.FORMAL_WHITE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.FORMAL_WHITE.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_formal_white_kanim"),
      new ClothingItemInfo("GlovesLongWhite", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.LONG_WHITE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.LONG_WHITE.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_long_white_kanim"),
      new ClothingItemInfo("Gloves2ToneCreamCharcoal", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.TWOTONE_CREAM_CHARCOAL.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.TWOTONE_CREAM_CHARCOAL.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_2tone_cream_charcoal_kanim"),
      new ClothingItemInfo("AtmoHelmetRocketmelon", (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.CANTALOUPE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.CANTALOUPE.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_cantaloupe_kanim"),
      new ClothingItemInfo("AtmoSuitRocketmelon", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.CANTALOUPE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.CANTALOUPE.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_cantaloupe_kanim"),
      new ClothingItemInfo("AtmoBeltRocketmelon", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.CANTALOUPE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.CANTALOUPE.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Nifty, "atmo_belt_cantaloupe_kanim"),
      new ClothingItemInfo("AtmoGlovesRocketmelon", (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.CANTALOUPE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.CANTALOUPE.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_cantaloupe_kanim"),
      new ClothingItemInfo("AtmoBootsRocketmelon", (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.CANTALOUPE.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.FACADES.CANTALOUPE.DESC, PermitCategory.AtmoSuitShoes, PermitRarity.Common, "atmo_shoes_cantaloupe_kanim"),
      new ClothingItemInfo("TopXSporchid", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.X_SPORCHID.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.X_SPORCHID.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_x_sporchid_kanim"),
      new ClothingItemInfo("TopX1Pinchapeppernutbells", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.X1_PINCHAPEPPERNUTBELLS.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.X1_PINCHAPEPPERNUTBELLS.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_x1_pinchapeppernutbells_kanim"),
      new ClothingItemInfo("TopPompomShinebugsPinkPeppernut", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.POMPOM_SHINEBUGS_PINK_PEPPERNUT.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.POMPOM_SHINEBUGS_PINK_PEPPERNUT.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_pompom_shinebugs_pink_peppernut_kanim"),
      new ClothingItemInfo("TopSnowflakeBlue", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.SNOWFLAKE_BLUE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.SNOWFLAKE_BLUE.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_snowflake_blue_kanim"),
      new ClothingItemInfo("AtmoBeltTwoToneBrown", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.TWOTONE_BROWN.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BELT.FACADES.TWOTONE_BROWN.DESC, PermitCategory.AtmoSuitBelt, PermitRarity.Nifty, "atmo_belt_2tone_brown_kanim"),
      new ClothingItemInfo("AtmoSuitMultiBlueGreyBlack", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.MULTI_BLUE_GREY_BLACK.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.MULTI_BLUE_GREY_BLACK.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_multi_blue_grey_black_kanim"),
      new ClothingItemInfo("AtmoSuitMultiBlueYellowRed", (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.MULTI_BLUE_YELLOW_RED.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_BODY.FACADES.MULTI_BLUE_YELLOW_RED.DESC, PermitCategory.AtmoSuitBody, PermitRarity.Splendid, "atmosuit_multi_blue_yellow_red_kanim"),
      new ClothingItemInfo("AtmoGlovesBrown", (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.BROWN.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.FACADES.BROWN.DESC, PermitCategory.AtmoSuitGloves, PermitRarity.Common, "atmo_gloves_brown_kanim"),
      new ClothingItemInfo("AtmoHelmetMondrianBlueRedYellow", (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.MONDRIAN_BLUE_RED_YELLOW.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.MONDRIAN_BLUE_RED_YELLOW.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_mondrian_blue_red_yellow_kanim"),
      new ClothingItemInfo("AtmoHelmetOverallsRed", (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.OVERALLS_RED.NAME, (string) EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.FACADES.OVERALLS_RED.DESC, PermitCategory.AtmoSuitHelmet, PermitRarity.Splendid, "atmo_helmet_overalls_red_kanim"),
      new ClothingItemInfo("PjCloversGlitchKelly", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.PJ_CLOVERS_GLITCH_KELLY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.PJ_CLOVERS_GLITCH_KELLY.DESC, PermitCategory.DupeTops, PermitRarity.Splendid, "pj_clovers_glitch_kelly_kanim"),
      new ClothingItemInfo("PjHeartsChilliStrawberry", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.PJ_HEARTS_CHILLI_STRAWBERRY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.PJ_HEARTS_CHILLI_STRAWBERRY.DESC, PermitCategory.DupeTops, PermitRarity.Splendid, "pj_hearts_chilli_strawberry_kanim"),
      new ClothingItemInfo("BottomGinchPinkGluon", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_PINK_GLUON.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_PINK_GLUON.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_pink_gluon_kanim"),
      new ClothingItemInfo("BottomGinchPurpleCortex", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_PURPLE_CORTEX.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_PURPLE_CORTEX.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_purple_cortex_kanim"),
      new ClothingItemInfo("BottomGinchBlueFrosty", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_BLUE_FROSTY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_BLUE_FROSTY.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_blue_frosty_kanim"),
      new ClothingItemInfo("BottomGinchTealLocus", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_TEAL_LOCUS.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_TEAL_LOCUS.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_teal_locus_kanim"),
      new ClothingItemInfo("BottomGinchGreenGoop", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_GREEN_GOOP.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_GREEN_GOOP.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_green_goop_kanim"),
      new ClothingItemInfo("BottomGinchYellowBile", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_YELLOW_BILE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_YELLOW_BILE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_yellow_bile_kanim"),
      new ClothingItemInfo("BottomGinchOrangeNybble", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_ORANGE_NYBBLE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_ORANGE_NYBBLE.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_orange_nybble_kanim"),
      new ClothingItemInfo("BottomGinchRedIronbow", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_RED_IRONBOW.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_RED_IRONBOW.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_red_ironbow_kanim"),
      new ClothingItemInfo("BottomGinchGreyPhlegm", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_GREY_PHLEGM.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_GREY_PHLEGM.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_grey_phlegm_kanim"),
      new ClothingItemInfo("BottomGinchGreyObelus", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_GREY_OBELUS.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GINCH_GREY_OBELUS.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "bottom_ginch_grey_obelus_kanim"),
      new ClothingItemInfo("PantsKnitPolkadotTurq", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.KNIT_POLKADOT_TURQ.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.KNIT_POLKADOT_TURQ.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "pants_knit_polkadot_turq_kanim"),
      new ClothingItemInfo("PantsGiBeltWhiteBlack", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GI_BELT_WHITE_BLACK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.GI_BELT_WHITE_BLACK.DESC, PermitCategory.DupeBottoms, PermitRarity.Nifty, "pants_gi_belt_white_black_kanim"),
      new ClothingItemInfo("PantsBeltKhakiTan", (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BELT_KHAKI_TAN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.FACADES.BELT_KHAKI_TAN.DESC, PermitCategory.DupeBottoms, PermitRarity.Decent, "pants_belt_khaki_tan_kanim"),
      new ClothingItemInfo("ShoesFlashy", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.FLASHY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.FLASHY.DESC, PermitCategory.DupeShoes, PermitRarity.Decent, "shoes_flashy_kanim"),
      new ClothingItemInfo("SocksGinchPinkSaltrock", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_PINK_SALTROCK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_PINK_SALTROCK.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_pink_saltrock_kanim"),
      new ClothingItemInfo("SocksGinchPurpleDusky", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_PURPLE_DUSKY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_PURPLE_DUSKY.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_purple_dusky_kanim"),
      new ClothingItemInfo("SocksGinchBlueBasin", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_BLUE_BASIN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_BLUE_BASIN.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_blue_basin_kanim"),
      new ClothingItemInfo("SocksGinchTealBalmy", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_TEAL_BALMY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_TEAL_BALMY.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_teal_balmy_kanim"),
      new ClothingItemInfo("SocksGinchGreenLime", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_GREEN_LIME.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_GREEN_LIME.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_green_lime_kanim"),
      new ClothingItemInfo("SocksGinchYellowYellowcake", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_YELLOW_YELLOWCAKE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_YELLOW_YELLOWCAKE.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_yellow_yellowcake_kanim"),
      new ClothingItemInfo("SocksGinchOrangeAtomic", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_ORANGE_ATOMIC.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_ORANGE_ATOMIC.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_orange_atomic_kanim"),
      new ClothingItemInfo("SocksGinchRedMagma", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_RED_MAGMA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_RED_MAGMA.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_red_magma_kanim"),
      new ClothingItemInfo("SocksGinchGreyGrey", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_GREY_GREY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_GREY_GREY.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_grey_grey_kanim"),
      new ClothingItemInfo("SocksGinchGreyCharcoal", (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_GREY_CHARCOAL.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_SHOES.FACADES.GINCH_GREY_CHARCOAL.DESC, PermitCategory.DupeShoes, PermitRarity.Common, "socks_ginch_grey_charcoal_kanim"),
      new ClothingItemInfo("GlovesBasicSlate", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_SLATE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.BASIC_SLATE.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_basic_slate_kanim"),
      new ClothingItemInfo("GlovesKnitGold", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.KNIT_GOLD.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.KNIT_GOLD.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_knit_gold_kanim"),
      new ClothingItemInfo("GlovesKnitMagenta", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.KNIT_MAGENTA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.KNIT_MAGENTA.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_knit_magenta_kanim"),
      new ClothingItemInfo("GlovesSparkleWhite", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.SPARKLE_WHITE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.SPARKLE_WHITE.DESC, PermitCategory.DupeGloves, PermitRarity.Decent, "gloves_sparkle_white_kanim"),
      new ClothingItemInfo("GlovesGinchPinkSaltrock", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_PINK_SALTROCK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_PINK_SALTROCK.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_pink_saltrock_kanim"),
      new ClothingItemInfo("GlovesGinchPurpleDusky", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_PURPLE_DUSKY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_PURPLE_DUSKY.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_purple_dusky_kanim"),
      new ClothingItemInfo("GlovesGinchBlueBasin", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_BLUE_BASIN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_BLUE_BASIN.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_blue_basin_kanim"),
      new ClothingItemInfo("GlovesGinchTealBalmy", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_TEAL_BALMY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_TEAL_BALMY.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_teal_balmy_kanim"),
      new ClothingItemInfo("GlovesGinchGreenLime", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_GREEN_LIME.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_GREEN_LIME.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_green_lime_kanim"),
      new ClothingItemInfo("GlovesGinchYellowYellowcake", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_YELLOW_YELLOWCAKE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_YELLOW_YELLOWCAKE.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_yellow_yellowcake_kanim"),
      new ClothingItemInfo("GlovesGinchOrangeAtomic", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_ORANGE_ATOMIC.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_ORANGE_ATOMIC.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_orange_atomic_kanim"),
      new ClothingItemInfo("GlovesGinchRedMagma", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_RED_MAGMA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_RED_MAGMA.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_red_magma_kanim"),
      new ClothingItemInfo("GlovesGinchGreyGrey", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_GREY_GREY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_GREY_GREY.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_grey_grey_kanim"),
      new ClothingItemInfo("GlovesGinchGreyCharcoal", (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_GREY_CHARCOAL.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_GLOVES.FACADES.GINCH_GREY_CHARCOAL.DESC, PermitCategory.DupeGloves, PermitRarity.Common, "gloves_ginch_grey_charcoal_kanim"),
      new ClothingItemInfo("TopBuilder", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BUILDER.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.BUILDER.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_builder_kanim"),
      new ClothingItemInfo("TopFloralPink", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.FLORAL_PINK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.FLORAL_PINK.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_floral_pink_kanim"),
      new ClothingItemInfo("TopGinchPinkSaltrock", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_PINK_SALTROCK.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_PINK_SALTROCK.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_pink_saltrock_kanim"),
      new ClothingItemInfo("TopGinchPurpleDusky", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_PURPLE_DUSKY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_PURPLE_DUSKY.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_purple_dusky_kanim"),
      new ClothingItemInfo("TopGinchBlueBasin", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_BLUE_BASIN.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_BLUE_BASIN.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_blue_basin_kanim"),
      new ClothingItemInfo("TopGinchTealBalmy", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_TEAL_BALMY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_TEAL_BALMY.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_teal_balmy_kanim"),
      new ClothingItemInfo("TopGinchGreenLime", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_GREEN_LIME.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_GREEN_LIME.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_green_lime_kanim"),
      new ClothingItemInfo("TopGinchYellowYellowcake", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_YELLOW_YELLOWCAKE.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_YELLOW_YELLOWCAKE.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_yellow_yellowcake_kanim"),
      new ClothingItemInfo("TopGinchOrangeAtomic", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_ORANGE_ATOMIC.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_ORANGE_ATOMIC.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_orange_atomic_kanim"),
      new ClothingItemInfo("TopGinchRedMagma", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_RED_MAGMA.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_RED_MAGMA.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_red_magma_kanim"),
      new ClothingItemInfo("TopGinchGreyGrey", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_GREY_GREY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_GREY_GREY.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_grey_grey_kanim"),
      new ClothingItemInfo("TopGinchGreyCharcoal", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_GREY_CHARCOAL.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.GINCH_GREY_CHARCOAL.DESC, PermitCategory.DupeTops, PermitRarity.Decent, "top_ginch_grey_charcoal_kanim"),
      new ClothingItemInfo("TopKnitPolkadotTurq", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.KNIT_POLKADOT_TURQ.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.KNIT_POLKADOT_TURQ.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_knit_polkadot_turq_kanim"),
      new ClothingItemInfo("TopFlashy", (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.FLASHY.NAME, (string) EQUIPMENT.PREFABS.CLOTHING_TOPS.FACADES.FLASHY.DESC, PermitCategory.DupeTops, PermitRarity.Nifty, "top_flashy_kanim")
    });
  }

  private void SetupClothingOutfits()
  {
    Add("StandardYellow", new string[4]
    {
      "TopStandardYellow",
      "BottomBasicBlack",
      "GlovesStandardGold",
      "ShoesBasicBlack"
    }, (string) UI.OUTFITS.STANDARD_YELLOW.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("StandardRed", new string[4]
    {
      "TopStandardRed",
      "BottomBasicBlack",
      "GlovesStandardGold",
      "ShoesBasicBlack"
    }, (string) UI.OUTFITS.STANDARD_RED.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("StandardGreen", new string[4]
    {
      "TopStandardGreen",
      "BottomBasicBlack",
      "GlovesStandardGold",
      "ShoesBasicBlack"
    }, (string) UI.OUTFITS.STANDARD_GREEN.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("StandardBlue", new string[4]
    {
      "TopStandardBlue",
      "BottomBasicBlack",
      "GlovesStandardGold",
      "ShoesBasicBlack"
    }, (string) UI.OUTFITS.STANDARD_BLUE.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicBlack", new string[4]
    {
      "TopBasicBlack",
      "BottomBasicBlack",
      "GlovesBasicBlack",
      "ShoesBasicBlack"
    }, (string) UI.OUTFITS.BASIC_BLACK.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicWhite", new string[4]
    {
      "TopBasicWhite",
      "BottomBasicWhite",
      "GlovesBasicWhite",
      "ShoesBasicWhite"
    }, (string) UI.OUTFITS.BASIC_WHITE.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicRed", new string[4]
    {
      "TopBasicRed",
      "BottomBasicRed",
      "GlovesBasicRed",
      "ShoesBasicRed"
    }, (string) UI.OUTFITS.BASIC_RED.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicOrange", new string[4]
    {
      "TopBasicOrange",
      "BottomBasicOrange",
      "GlovesBasicOrange",
      "ShoesBasicOrange"
    }, (string) UI.OUTFITS.BASIC_ORANGE.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicYellow", new string[4]
    {
      "TopBasicYellow",
      "BottomBasicYellow",
      "GlovesBasicYellow",
      "ShoesBasicYellow"
    }, (string) UI.OUTFITS.BASIC_YELLOW.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicGreen", new string[4]
    {
      "TopBasicGreen",
      "BottomBasicGreen",
      "GlovesBasicGreen",
      "ShoesBasicGreen"
    }, (string) UI.OUTFITS.BASIC_GREEN.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicAqua", new string[4]
    {
      "TopBasicAqua",
      "BottomBasicAqua",
      "GlovesBasicAqua",
      "ShoesBasicAqua"
    }, (string) UI.OUTFITS.BASIC_AQUA.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicPurple", new string[4]
    {
      "TopBasicPurple",
      "BottomBasicPurple",
      "GlovesBasicPurple",
      "ShoesBasicPurple"
    }, (string) UI.OUTFITS.BASIC_PURPLE.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicPinkOrchid", new string[4]
    {
      "TopBasicPinkOrchid",
      "BottomBasicPinkOrchid",
      "GlovesBasicPinkOrchid",
      "ShoesBasicPinkOrchid"
    }, (string) UI.OUTFITS.BASIC_PINK_ORCHID.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicDeepRed", new string[4]
    {
      "TopRaglanDeepRed",
      "ShortsBasicDeepRed",
      "GlovesAthleticRedDeep",
      "SocksAthleticDeepRed"
    }, (string) UI.OUTFITS.BASIC_DEEPRED.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicOrangeSatsuma", new string[4]
    {
      "TopRaglanSatsuma",
      "ShortsBasicSatsuma",
      "GlovesAthleticOrangeSatsuma",
      "SocksAthleticOrangeSatsuma"
    }, (string) UI.OUTFITS.BASIC_SATSUMA.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicLemon", new string[4]
    {
      "TopRaglanLemon",
      "ShortsBasicYellowcake",
      "GlovesAthleticYellowLemon",
      "SocksAthleticYellowLemon"
    }, (string) UI.OUTFITS.BASIC_LEMON.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicBlueCobalt", new string[4]
    {
      "TopRaglanCobalt",
      "ShortsBasicBlueCobalt",
      "GlovesAthleticBlueCobalt",
      "SocksAthleticBlueCobalt"
    }, (string) UI.OUTFITS.BASIC_BLUE_COBALT.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicGreenKelly", new string[4]
    {
      "TopRaglanKellyGreen",
      "ShortsBasicKellyGreen",
      "GlovesAthleticGreenKelly",
      "SocksAthleticGreenKelly"
    }, (string) UI.OUTFITS.BASIC_GREEN_KELLY.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicPinkFlamingo", new string[4]
    {
      "TopRaglanFlamingo",
      "ShortsBasicPinkFlamingo",
      "GlovesAthleticPinkFlamingo",
      "SocksAthleticPinkFlamingo"
    }, (string) UI.OUTFITS.BASIC_PINK_FLAMINGO.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("BasicGreyCharcoal", new string[4]
    {
      "TopRaglanCharcoal",
      "ShortsBasicCharcoal",
      "GlovesAthleticGreyCharcoal",
      "SocksAthleticGreyCharcoal"
    }, (string) UI.OUTFITS.BASIC_GREY_CHARCOAL.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("JellypuffBlueberry", new string[2]
    {
      "TopJellypuffJacketBlueberry",
      "GlovesCufflessBlueberry"
    }, (string) UI.OUTFITS.JELLYPUFF_BLUEBERRY.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("JellypuffGrape", new string[2]
    {
      "TopJellypuffJacketGrape",
      "GlovesCufflessGrape"
    }, (string) UI.OUTFITS.JELLYPUFF_GRAPE.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("JellypuffLemon", new string[2]
    {
      "TopJellypuffJacketLemon",
      "GlovesCufflessLemon"
    }, (string) UI.OUTFITS.JELLYPUFF_LEMON.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("JellypuffLime", new string[2]
    {
      "TopJellypuffJacketLime",
      "GlovesCufflessLime"
    }, (string) UI.OUTFITS.JELLYPUFF_LIME.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("JellypuffSatsuma", new string[2]
    {
      "TopJellypuffJacketSatsuma",
      "GlovesCufflessSatsuma"
    }, (string) UI.OUTFITS.JELLYPUFF_SATSUMA.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("JellypuffStrawberry", new string[2]
    {
      "TopJellypuffJacketStrawberry",
      "GlovesCufflessStrawberry"
    }, (string) UI.OUTFITS.JELLYPUFF_STRAWBERRY.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("JellypuffWatermelon", new string[2]
    {
      "TopJellypuffJacketWatermelon",
      "GlovesCufflessWatermelon"
    }, (string) UI.OUTFITS.JELLYPUFF_WATERMELON.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("Athlete", new string[4]
    {
      "TopAthlete",
      "PantsAthlete",
      "GlovesAthlete",
      "ShoesBasicBlack"
    }, (string) UI.OUTFITS.ATHLETE.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("Circuit", new string[3]
    {
      "TopCircuitGreen",
      "PantsCircuitGreen",
      "GlovesCircuitGreen"
    }, (string) UI.OUTFITS.CIRCUIT.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("AtmoLimone", new string[5]
    {
      "AtmoHelmetLimone",
      "AtmoSuitBasicYellow",
      "AtmoGlovesLime",
      "AtmoBeltBasicLime",
      "AtmoShoesBasicYellow"
    }, (string) UI.OUTFITS.ATMOSUIT_LIMONE.NAME, BlueprintProvider.OutfitType.AtmoSuit);
    Add("AtmoPuft", new string[5]
    {
      "AtmoHelmetPuft",
      "AtmoSuitPuft",
      "AtmoGlovesPuft",
      "AtmoBeltPuft",
      "AtmoShoesPuft"
    }, (string) UI.OUTFITS.ATMOSUIT_PUFT.NAME, BlueprintProvider.OutfitType.AtmoSuit);
    Add("AtmoSparkleRed", new string[5]
    {
      "AtmoHelmetSparkleRed",
      "AtmoSuitSparkleRed",
      "AtmoGlovesSparkleRed",
      "AtmoBeltSparkleRed",
      "AtmoShoesSparkleBlack"
    }, (string) UI.OUTFITS.ATMOSUIT_SPARKLE_RED.NAME, BlueprintProvider.OutfitType.AtmoSuit);
    Add("AtmoSparkleBlue", new string[5]
    {
      "AtmoHelmetSparkleBlue",
      "AtmoSuitSparkleBlue",
      "AtmoGlovesSparkleBlue",
      "AtmoBeltSparkleBlue",
      "AtmoShoesSparkleBlack"
    }, (string) UI.OUTFITS.ATMOSUIT_SPARKLE_BLUE.NAME, BlueprintProvider.OutfitType.AtmoSuit);
    Add("AtmoSparkleGreen", new string[5]
    {
      "AtmoHelmetSparkleGreen",
      "AtmoSuitSparkleGreen",
      "AtmoGlovesSparkleGreen",
      "AtmoBeltSparkleGreen",
      "AtmoShoesSparkleBlack"
    }, (string) UI.OUTFITS.ATMOSUIT_SPARKLE_GREEN.NAME, BlueprintProvider.OutfitType.AtmoSuit);
    Add("AtmoSparkleLavender", new string[5]
    {
      "AtmoHelmetSparklePurple",
      "AtmoSuitSparkleLavender",
      "AtmoGlovesSparkleLavender",
      "AtmoBeltSparkleLavender",
      "AtmoShoesSparkleBlack"
    }, (string) UI.OUTFITS.ATMOSUIT_SPARKLE_LAVENDER.NAME, BlueprintProvider.OutfitType.AtmoSuit);
    Add("AtmoConfetti", new string[5]
    {
      "AtmoHelmetConfetti",
      "AtmoSuitConfetti",
      "AtmoGlovesGold",
      "AtmoBeltBasicGold",
      "AtmoShoesStealth"
    }, (string) UI.OUTFITS.ATMOSUIT_CONFETTI.NAME, BlueprintProvider.OutfitType.AtmoSuit);
    Add("AtmoEggplant", new string[5]
    {
      "AtmoHelmetEggplant",
      "AtmoSuitCrispEggplant",
      "AtmoGlovesEggplant",
      "AtmoBeltEggplant",
      "AtmoShoesEggplant"
    }, (string) UI.OUTFITS.ATMOSUIT_BASIC_PURPLE.NAME, BlueprintProvider.OutfitType.AtmoSuit);
    Add("CanadianTuxedo", new string[4]
    {
      "TopDenimBlue",
      "PantsJeans",
      "GlovesDenimBlue",
      "ShoesDenimBlue"
    }, (string) UI.OUTFITS.CANUXTUX.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("Researcher", new string[4]
    {
      "TopResearcher",
      "PantsResearch",
      "GlovesBasicBrownKhaki",
      "ShoesBasicGray"
    }, (string) UI.OUTFITS.NERD.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("UndiesExec", new string[2]
    {
      "TopUndershirtExecutive",
      "BottomBriefsExecutive"
    }, (string) UI.OUTFITS.GONCHIES_STRAWBERRY.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("UndiesUnderling", new string[2]
    {
      "TopUndershirtUnderling",
      "BottomBriefsUnderling"
    }, (string) UI.OUTFITS.GONCHIES_SATSUMA.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("UndiesGroupthink", new string[2]
    {
      "TopUndershirtGroupthink",
      "BottomBriefsGroupthink"
    }, (string) UI.OUTFITS.GONCHIES_LEMON.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("UndiesStakeholder", new string[2]
    {
      "TopUndershirtStakeholder",
      "BottomBriefsStakeholder"
    }, (string) UI.OUTFITS.GONCHIES_LIME.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("UndiesAdmin", new string[2]
    {
      "TopUndershirtAdmin",
      "BottomBriefsAdmin"
    }, (string) UI.OUTFITS.GONCHIES_BLUEBERRY.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("UndiesBuzzword", new string[2]
    {
      "TopUndershirtBuzzword",
      "BottomBriefsBuzzword"
    }, (string) UI.OUTFITS.GONCHIES_GRAPE.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("UndiesSynergy", new string[2]
    {
      "TopUndershirtSynergy",
      "BottomBriefsSynergy"
    }, (string) UI.OUTFITS.GONCHIES_WATERMELON.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("RebelGiOutfit", new string[3]
    {
      "TopRebelGi",
      "PantsGiBeltWhiteBlack",
      "GlovesCufflessBlack"
    }, (string) UI.OUTFITS.REBELGI.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("AtmoPinkPurple", new string[5]
    {
      "AtmoBeltBasicNeonPink",
      "AtmoGlovesStripesLavender",
      "AtmoHelmetWorkoutLavender",
      "AtmoSuitBasicNeonPink",
      "AtmoShoesBasicLavender"
    }, (string) UI.OUTFITS.ATMOSUIT_PINK_PURPLE.NAME, BlueprintProvider.OutfitType.AtmoSuit);
    Add("AtmoRedGrey", new string[4]
    {
      "AtmoBeltBasicGrey",
      "AtmoGlovesWhite",
      "AtmoHelmetCummerbundRed",
      "AtmoSuitMultiRedBlack"
    }, (string) UI.OUTFITS.ATMOSUIT_RED_GREY.NAME, BlueprintProvider.OutfitType.AtmoSuit);
    Add("Donor", new string[3]
    {
      "TopJacketSmokingBurgundy",
      "BottomBasicBlack",
      "GlovesBasicBlack"
    }, (string) UI.OUTFITS.DONOR.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("EngineerCoveralls", new string[4]
    {
      "TopMechanic",
      "PantsBasicRedOrange",
      "GlovesBasicGrey",
      "ShoesBasicBlack"
    }, (string) UI.OUTFITS.MECHANIC.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("PhdVelour", new string[4]
    {
      "TopVelourBlack",
      "PantsVelourBlack",
      "GlovesBasicWhite",
      "ShoesVelourBlack"
    }, (string) UI.OUTFITS.VELOUR_BLACK.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("PhdDress", new string[3]
    {
      "DressSleevelessBowBw",
      "GlovesLongWhite",
      "ShoesMaryjaneSocksBw"
    }, (string) UI.OUTFITS.SLEEVELESS_BOW_BW.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("ShortwaveVelour", new string[4]
    {
      "TopVelourBlue",
      "PantsVelourBlue",
      "GlovesBasicWhite",
      "ShoesVelourBlue"
    }, (string) UI.OUTFITS.VELOUR_BLUE.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("GammaVelour", new string[4]
    {
      "TopVelourPink",
      "PantsVelourPink",
      "GlovesBasicPinksalmon",
      "ShoesVelourPink"
    }, (string) UI.OUTFITS.VELOUR_PINK.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("HvacCoveralls", new string[4]
    {
      "TopWater",
      "PantsBeltKhakiTan",
      "GlovesBasicTan",
      "ShoesBasicTan"
    }, (string) UI.OUTFITS.WATER.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("NobelPinstripe", new string[3]
    {
      "TopWaistcoatPinstripeSlate",
      "PantsPinstripeSlate",
      "GlovesBasicSlate"
    }, (string) UI.OUTFITS.WAISTCOAT_PINSTRIPE_SLATE.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("PowerBrunch", new string[4]
    {
      "TopTweedPinkOrchid",
      "SkirtTweedPinkOrchid",
      "Gloves2ToneCreamCharcoal",
      "ShoesClassicFlatsCreamCharcoal"
    }, (string) UI.OUTFITS.TWEED_PINK_ORCHID.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("Ballet", new string[4]
    {
      "BodysuitBallerinaPink",
      "SkirtBallerinaPink",
      "GlovesBallerinaPink",
      "ShoesBallerinaPink"
    }, (string) UI.OUTFITS.BALLET.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("AtmoRocketmelon", new string[5]
    {
      "AtmoHelmetRocketmelon",
      "AtmoSuitRocketmelon",
      "AtmoGlovesRocketmelon",
      "AtmoBeltRocketmelon",
      "AtmoBootsRocketmelon"
    }, (string) UI.OUTFITS.ATMOSUIT_CANTALOUPE.NAME, BlueprintProvider.OutfitType.AtmoSuit);
    Add("TopXSporchid", new string[1]{ "TopXSporchid" }, (string) UI.OUTFITS.X_SPORCHID.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("TopX1Pinchapeppernutbells", new string[1]
    {
      "TopX1Pinchapeppernutbells"
    }, (string) UI.OUTFITS.X1_PINCHAPEPPERNUTBELLS.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("TopPompomShinebugsPinkPeppernut", new string[1]
    {
      "TopPompomShinebugsPinkPeppernut"
    }, (string) UI.OUTFITS.POMPOM_SHINEBUGS_PINK_PEPPERNUT.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("TopSnowflakeBlue", new string[1]
    {
      "TopSnowflakeBlue"
    }, (string) UI.OUTFITS.SNOWFLAKE_BLUE.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("PolkaDotTracksuit", new string[3]
    {
      "TopKnitPolkadotTurq",
      "PantsKnitPolkadotTurq",
      "GlovesKnitMagenta"
    }, (string) UI.OUTFITS.POLKADOT_TRACKSUIT.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("Superstar", new string[4]
    {
      "TopFlashy",
      "ShoesFlashy",
      "GlovesSparkleWhite",
      "BottomBasicBlack"
    }, (string) UI.OUTFITS.SUPERSTAR.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("Spiffy", new string[4]
    {
      "AtmoHelmetOverallsRed",
      "AtmoSuitMultiBlueGreyBlack",
      "AtmoGlovesBrown",
      "AtmoBeltTwoToneBrown"
    }, (string) UI.OUTFITS.ATMOSUIT_SPIFFY.NAME, BlueprintProvider.OutfitType.AtmoSuit);
    Add("Cubist", new string[4]
    {
      "AtmoHelmetMondrianBlueRedYellow",
      "AtmoSuitMultiBlueYellowRed",
      "AtmoGlovesGold",
      "AtmoBeltBasicGold"
    }, (string) UI.OUTFITS.ATMOSUIT_CUBIST.NAME, BlueprintProvider.OutfitType.AtmoSuit);
    Add("Lucky", new string[1]{ "PjCloversGlitchKelly" }, (string) UI.OUTFITS.LUCKY.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("Sweetheart", new string[1]
    {
      "PjHeartsChilliStrawberry"
    }, (string) UI.OUTFITS.SWEETHEART.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("GinchGluon", new string[4]
    {
      "TopGinchPinkSaltrock",
      "BottomGinchPinkGluon",
      "GlovesGinchPinkSaltrock",
      "SocksGinchPinkSaltrock"
    }, (string) UI.OUTFITS.GINCH_GLUON.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("GinchCortex", new string[4]
    {
      "TopGinchPurpleDusky",
      "BottomGinchPurpleCortex",
      "GlovesGinchPurpleDusky",
      "SocksGinchPurpleDusky"
    }, (string) UI.OUTFITS.GINCH_CORTEX.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("GinchFrosty", new string[4]
    {
      "TopGinchBlueBasin",
      "BottomGinchBlueFrosty",
      "GlovesGinchBlueBasin",
      "SocksGinchBlueBasin"
    }, (string) UI.OUTFITS.GINCH_FROSTY.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("GinchLocus", new string[4]
    {
      "TopGinchTealBalmy",
      "BottomGinchTealLocus",
      "GlovesGinchTealBalmy",
      "SocksGinchTealBalmy"
    }, (string) UI.OUTFITS.GINCH_LOCUS.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("GinchGoop", new string[4]
    {
      "TopGinchGreenLime",
      "BottomGinchGreenGoop",
      "GlovesGinchGreenLime",
      "SocksGinchGreenLime"
    }, (string) UI.OUTFITS.GINCH_GOOP.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("GinchBile", new string[4]
    {
      "TopGinchYellowYellowcake",
      "BottomGinchYellowBile",
      "GlovesGinchYellowYellowcake",
      "SocksGinchYellowYellowcake"
    }, (string) UI.OUTFITS.GINCH_BILE.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("GinchNybble", new string[4]
    {
      "TopGinchOrangeAtomic",
      "BottomGinchOrangeNybble",
      "GlovesGinchOrangeAtomic",
      "SocksGinchOrangeAtomic"
    }, (string) UI.OUTFITS.GINCH_NYBBLE.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("GinchIronbow", new string[4]
    {
      "TopGinchRedMagma",
      "BottomGinchRedIronbow",
      "GlovesGinchRedMagma",
      "SocksGinchRedMagma"
    }, (string) UI.OUTFITS.GINCH_IRONBOW.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("GinchPhlegm", new string[4]
    {
      "TopGinchGreyGrey",
      "BottomGinchGreyPhlegm",
      "GlovesGinchGreyGrey",
      "SocksGinchGreyGrey"
    }, (string) UI.OUTFITS.GINCH_PHLEGM.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("GinchObelus", new string[4]
    {
      "TopGinchGreyCharcoal",
      "BottomGinchGreyObelus",
      "GlovesGinchGreyCharcoal",
      "SocksGinchGreyCharcoal"
    }, (string) UI.OUTFITS.GINCH_OBELUS.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("HiVis", new string[4]
    {
      "TopBuilder",
      "PantsBasicOrangeSatsuma",
      "GlovesBasicYellow",
      "ShoesBasicBlack"
    }, (string) UI.OUTFITS.HIVIS.NAME, BlueprintProvider.OutfitType.Clothing);
    Add("Downtime", new string[2]
    {
      "TopFloralPink",
      "GlovesKnitGold"
    }, (string) UI.OUTFITS.DOWNTIME.NAME, BlueprintProvider.OutfitType.Clothing);

    void Add(
      string outfitId,
      string[] itemIds,
      string name,
      BlueprintProvider.OutfitType outfitType)
    {
      this.blueprintCollection.outfits.Add(new ClothingOutfitResource(outfitId, itemIds, name, (ClothingOutfitUtility.OutfitType) outfitType));
    }
  }

  private void SetupBalloonArtistFacades()
  {
    this.blueprintCollection.balloonArtistFacades.AddRange((IEnumerable<BalloonArtistFacadeInfo>) new BalloonArtistFacadeInfo[21]
    {
      new BalloonArtistFacadeInfo("BalloonRedFireEngineLongSparkles", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_FIREENGINE_LONG_SPARKLES.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_FIREENGINE_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_red_fireengine_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonYellowLongSparkles", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_YELLOW_LONG_SPARKLES.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_YELLOW_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_yellow_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonBlueLongSparkles", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BLUE_LONG_SPARKLES.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BLUE_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_blue_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonGreenLongSparkles", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_GREEN_LONG_SPARKLES.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_GREEN_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_green_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonPinkLongSparkles", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_PINK_LONG_SPARKLES.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_PINK_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_pink_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonPurpleLongSparkles", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_PURPLE_LONG_SPARKLES.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_PURPLE_LONG_SPARKLES.DESC, PermitRarity.Common, "balloon_purple_long_sparkles_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonBabyPacuEgg", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PACU_EGG.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PACU_EGG.DESC, PermitRarity.Splendid, "balloon_babypacu_egg_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonBabyGlossyDreckoEgg", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_GLOSSY_DRECKO_EGG.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_GLOSSY_DRECKO_EGG.DESC, PermitRarity.Splendid, "balloon_babyglossydrecko_egg_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonBabyHatchEgg", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_HATCH_EGG.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_HATCH_EGG.DESC, PermitRarity.Splendid, "balloon_babyhatch_egg_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonBabyPokeshellEgg", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_POKESHELL_EGG.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_POKESHELL_EGG.DESC, PermitRarity.Splendid, "balloon_babypokeshell_egg_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonBabyPuftEgg", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PUFT_EGG.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PUFT_EGG.DESC, PermitRarity.Splendid, "balloon_babypuft_egg_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonBabyShovoleEgg", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_SHOVOLE_EGG.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_SHOVOLE_EGG.DESC, PermitRarity.Splendid, "balloon_babyshovole_egg_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonBabyPipEgg", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PIP_EGG.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.BALLOON_BABY_PIP_EGG.DESC, PermitRarity.Splendid, "balloon_babypip_egg_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonCandyBlueberry", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_BLUEBERRY.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_BLUEBERRY.DESC, PermitRarity.Decent, "balloon_candy_blueberry_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonCandyGrape", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_GRAPE.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_GRAPE.DESC, PermitRarity.Decent, "balloon_candy_grape_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonCandyLemon", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_LEMON.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_LEMON.DESC, PermitRarity.Decent, "balloon_candy_lemon_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonCandyLime", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_LIME.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_LIME.DESC, PermitRarity.Decent, "balloon_candy_lime_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonCandyOrange", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_ORANGE.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_ORANGE.DESC, PermitRarity.Decent, "balloon_candy_orange_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonCandyStrawberry", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_STRAWBERRY.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_STRAWBERRY.DESC, PermitRarity.Decent, "balloon_candy_strawberry_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonCandyWatermelon", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_WATERMELON.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.CANDY_WATERMELON.DESC, PermitRarity.Decent, "balloon_candy_watermelon_kanim", BalloonArtistFacadeType.ThreeSet),
      new BalloonArtistFacadeInfo("BalloonHandGold", (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.HAND_GOLD.NAME, (string) EQUIPMENT.PREFABS.EQUIPPABLEBALLOON.FACADES.HAND_GOLD.DESC, PermitRarity.Decent, "balloon_hand_gold_kanim", BalloonArtistFacadeType.ThreeSet)
    });
  }
}
