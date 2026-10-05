// Decompiled with JetBrains decompiler
// Type: Blueprints_Default
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using STRINGS;
using System.Collections.Generic;

#nullable disable
public class Blueprints_Default : BlueprintProvider
{
  public override void SetupBlueprints()
  {
    this.SetupBuildingFacades();
    this.SetupArtables();
    this.SetupClothingItems();
    this.SetupClothingOutfits();
    this.SetupBalloonArtistFacades();
    this.SetupStickerBombFacades();
    this.SetupEquippableFacades();
    this.SetupMonumentParts();
  }

  public void SetupBuildingFacades()
  {
  }

  private void SetupArtables()
  {
    this.blueprintCollection.artables.AddRange((IEnumerable<ArtableInfo>) new ArtableInfo[42]
    {
      new ArtableInfo("Canvas_Bad", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_A.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_A.DESC, PermitRarity.Universal, "painting_art_a_kanim", "art_a", 5, false, "LookingUgly", "Canvas", "canvas"),
      new ArtableInfo("Canvas_Average", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_B.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_B.DESC, PermitRarity.Universal, "painting_art_b_kanim", "art_b", 10, false, "LookingOkay", "Canvas", "canvas"),
      new ArtableInfo("Canvas_Good", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_C.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_C.DESC, PermitRarity.Universal, "painting_art_c_kanim", "art_c", 15, true, "LookingGreat", "Canvas", "canvas"),
      new ArtableInfo("Canvas_Good2", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_D.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_D.DESC, PermitRarity.Universal, "painting_art_d_kanim", "art_d", 15, true, "LookingGreat", "Canvas", "canvas"),
      new ArtableInfo("Canvas_Good3", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_E.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_E.DESC, PermitRarity.Universal, "painting_art_e_kanim", "art_e", 15, true, "LookingGreat", "Canvas", "canvas"),
      new ArtableInfo("Canvas_Good4", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_F.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_F.DESC, PermitRarity.Universal, "painting_art_f_kanim", "art_f", 15, true, "LookingGreat", "Canvas", "canvas"),
      new ArtableInfo("Canvas_Good5", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_G.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_G.DESC, PermitRarity.Universal, "painting_art_g_kanim", "art_g", 15, true, "LookingGreat", "Canvas", "canvas"),
      new ArtableInfo("Canvas_Good6", (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_H.NAME, (string) BUILDINGS.PREFABS.CANVAS.FACADES.ART_H.DESC, PermitRarity.Universal, "painting_art_h_kanim", "art_h", 15, true, "LookingGreat", "Canvas", "canvas"),
      new ArtableInfo("CanvasTall_Bad", (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_A.NAME, (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_A.DESC, PermitRarity.Universal, "painting_tall_art_a_kanim", "art_a", 5, false, "LookingUgly", "CanvasTall", "canvas"),
      new ArtableInfo("CanvasTall_Average", (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_B.NAME, (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_B.DESC, PermitRarity.Universal, "painting_tall_art_b_kanim", "art_b", 10, false, "LookingOkay", "CanvasTall", "canvas"),
      new ArtableInfo("CanvasTall_Good", (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_C.NAME, (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_C.DESC, PermitRarity.Universal, "painting_tall_art_c_kanim", "art_c", 15, true, "LookingGreat", "CanvasTall", "canvas"),
      new ArtableInfo("CanvasTall_Good2", (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_D.NAME, (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_D.DESC, PermitRarity.Universal, "painting_tall_art_d_kanim", "art_d", 15, true, "LookingGreat", "CanvasTall", "canvas"),
      new ArtableInfo("CanvasTall_Good3", (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_E.NAME, (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_E.DESC, PermitRarity.Universal, "painting_tall_art_e_kanim", "art_e", 15, true, "LookingGreat", "CanvasTall", "canvas"),
      new ArtableInfo("CanvasTall_Good4", (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_F.NAME, (string) BUILDINGS.PREFABS.CANVASTALL.FACADES.ART_TALL_F.DESC, PermitRarity.Universal, "painting_tall_art_f_kanim", "art_f", 15, true, "LookingGreat", "CanvasTall", "canvas"),
      new ArtableInfo("CanvasWide_Bad", (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_A.NAME, (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_A.DESC, PermitRarity.Universal, "painting_wide_art_a_kanim", "art_a", 5, false, "LookingUgly", "CanvasWide", "canvas"),
      new ArtableInfo("CanvasWide_Average", (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_B.NAME, (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_B.DESC, PermitRarity.Universal, "painting_wide_art_b_kanim", "art_b", 10, false, "LookingOkay", "CanvasWide", "canvas"),
      new ArtableInfo("CanvasWide_Good", (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_C.NAME, (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_C.DESC, PermitRarity.Universal, "painting_wide_art_c_kanim", "art_c", 15, true, "LookingGreat", "CanvasWide", "canvas"),
      new ArtableInfo("CanvasWide_Good2", (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_D.NAME, (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_D.DESC, PermitRarity.Universal, "painting_wide_art_d_kanim", "art_d", 15, true, "LookingGreat", "CanvasWide", "canvas"),
      new ArtableInfo("CanvasWide_Good3", (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_E.NAME, (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_E.DESC, PermitRarity.Universal, "painting_wide_art_e_kanim", "art_e", 15, true, "LookingGreat", "CanvasWide", "canvas"),
      new ArtableInfo("CanvasWide_Good4", (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_F.NAME, (string) BUILDINGS.PREFABS.CANVASWIDE.FACADES.ART_WIDE_F.DESC, PermitRarity.Universal, "painting_wide_art_f_kanim", "art_f", 15, true, "LookingGreat", "CanvasWide", "canvas"),
      new ArtableInfo("Sculpture_Bad", (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_CRAP_1.NAME, (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_CRAP_1.DESC, PermitRarity.Universal, "sculpture_crap_1_kanim", "crap_1", 5, false, "LookingUgly", "Sculpture"),
      new ArtableInfo("Sculpture_Average", (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_GOOD_1.NAME, (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_GOOD_1.DESC, PermitRarity.Universal, "sculpture_good_1_kanim", "good_1", 10, false, "LookingOkay", "Sculpture"),
      new ArtableInfo("Sculpture_Good1", (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_1.NAME, (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_1.DESC, PermitRarity.Universal, "sculpture_amazing_1_kanim", "amazing_1", 15, true, "LookingGreat", "Sculpture"),
      new ArtableInfo("Sculpture_Good2", (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_2.NAME, (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_2.DESC, PermitRarity.Universal, "sculpture_amazing_2_kanim", "amazing_2", 15, true, "LookingGreat", "Sculpture"),
      new ArtableInfo("Sculpture_Good3", (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_3.NAME, (string) BUILDINGS.PREFABS.SCULPTURE.FACADES.SCULPTURE_AMAZING_3.DESC, PermitRarity.Universal, "sculpture_amazing_3_kanim", "amazing_3", 15, true, "LookingGreat", "Sculpture"),
      new ArtableInfo("SmallSculpture_Bad", (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_CRAP.NAME, (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_CRAP.DESC, PermitRarity.Universal, "sculpture_1x2_crap_1_kanim", "crap_1", 5, false, "LookingUgly", "SmallSculpture"),
      new ArtableInfo("SmallSculpture_Average", (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_GOOD.NAME, (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_GOOD.DESC, PermitRarity.Universal, "sculpture_1x2_good_1_kanim", "good_1", 10, false, "LookingOkay", "SmallSculpture"),
      new ArtableInfo("SmallSculpture_Good", (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_1.NAME, (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_1.DESC, PermitRarity.Universal, "sculpture_1x2_amazing_1_kanim", "amazing_1", 15, true, "LookingGreat", "SmallSculpture"),
      new ArtableInfo("SmallSculpture_Good2", (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_2.NAME, (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_2.DESC, PermitRarity.Universal, "sculpture_1x2_amazing_2_kanim", "amazing_2", 15, true, "LookingGreat", "SmallSculpture"),
      new ArtableInfo("SmallSculpture_Good3", (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_3.NAME, (string) BUILDINGS.PREFABS.SMALLSCULPTURE.FACADES.SCULPTURE_1x2_AMAZING_3.DESC, PermitRarity.Universal, "sculpture_1x2_amazing_3_kanim", "amazing_3", 15, true, "LookingGreat", "SmallSculpture"),
      new ArtableInfo("IceSculpture_Bad", (string) BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_CRAP.NAME, (string) BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_CRAP.DESC, PermitRarity.Universal, "icesculpture_crap_kanim", "crap", 5, false, "LookingUgly", "IceSculpture"),
      new ArtableInfo("IceSculpture_Average", (string) BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_AMAZING_1.NAME, (string) BUILDINGS.PREFABS.ICESCULPTURE.FACADES.ICESCULPTURE_AMAZING_1.DESC, PermitRarity.Universal, "icesculpture_idle_kanim", "idle", 10, false, "LookingOkay", "IceSculpture", "good"),
      new ArtableInfo("MarbleSculpture_Bad", (string) BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_CRAP_1.NAME, (string) BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_CRAP_1.DESC, PermitRarity.Universal, "sculpture_marble_crap_1_kanim", "crap_1", 5, false, "LookingUgly", "MarbleSculpture"),
      new ArtableInfo("MarbleSculpture_Average", (string) BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_GOOD_1.NAME, (string) BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_GOOD_1.DESC, PermitRarity.Universal, "sculpture_marble_good_1_kanim", "good_1", 10, false, "LookingOkay", "MarbleSculpture"),
      new ArtableInfo("MarbleSculpture_Good1", (string) BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_1.NAME, (string) BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_1.DESC, PermitRarity.Universal, "sculpture_marble_amazing_1_kanim", "amazing_1", 15, true, "LookingGreat", "MarbleSculpture"),
      new ArtableInfo("MarbleSculpture_Good2", (string) BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_2.NAME, (string) BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_2.DESC, PermitRarity.Universal, "sculpture_marble_amazing_2_kanim", "amazing_2", 15, true, "LookingGreat", "MarbleSculpture"),
      new ArtableInfo("MarbleSculpture_Good3", (string) BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_3.NAME, (string) BUILDINGS.PREFABS.MARBLESCULPTURE.FACADES.SCULPTURE_MARBLE_AMAZING_3.DESC, PermitRarity.Universal, "sculpture_marble_amazing_3_kanim", "amazing_3", 15, true, "LookingGreat", "MarbleSculpture"),
      new ArtableInfo("MetalSculpture_Bad", (string) BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_CRAP_1.NAME, (string) BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_CRAP_1.DESC, PermitRarity.Universal, "sculpture_metal_crap_1_kanim", "crap_1", 5, false, "LookingUgly", "MetalSculpture"),
      new ArtableInfo("MetalSculpture_Average", (string) BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_GOOD_1.NAME, (string) BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_GOOD_1.DESC, PermitRarity.Universal, "sculpture_metal_good_1_kanim", "good_1", 10, false, "LookingOkay", "MetalSculpture"),
      new ArtableInfo("MetalSculpture_Good1", (string) BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_1.NAME, (string) BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_1.DESC, PermitRarity.Universal, "sculpture_metal_amazing_1_kanim", "amazing_1", 15, true, "LookingGreat", "MetalSculpture"),
      new ArtableInfo("MetalSculpture_Good2", (string) BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_2.NAME, (string) BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_2.DESC, PermitRarity.Universal, "sculpture_metal_amazing_2_kanim", "amazing_2", 15, true, "LookingGreat", "MetalSculpture"),
      new ArtableInfo("MetalSculpture_Good3", (string) BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_3.NAME, (string) BUILDINGS.PREFABS.METALSCULPTURE.FACADES.SCULPTURE_METAL_AMAZING_3.DESC, PermitRarity.Universal, "sculpture_metal_amazing_3_kanim", "amazing_3", 15, true, "LookingGreat", "MetalSculpture")
    });
  }

  private void SetupClothingItems()
  {
  }

  private void SetupClothingOutfits()
  {
  }

  private void SetupBalloonArtistFacades()
  {
  }

  private void SetupStickerBombFacades()
  {
    this.blueprintCollection.stickerBombFacades.AddRange((IEnumerable<StickerBombFacadeInfo>) new StickerBombFacadeInfo[20]
    {
      new StickerBombFacadeInfo("a", (string) STICKERNAMES.STICKER_A, "TODO:DbStickers", PermitRarity.Unknown, "sticker_a_kanim", "a"),
      new StickerBombFacadeInfo("b", (string) STICKERNAMES.STICKER_B, "TODO:DbStickers", PermitRarity.Unknown, "sticker_b_kanim", "b"),
      new StickerBombFacadeInfo("c", (string) STICKERNAMES.STICKER_C, "TODO:DbStickers", PermitRarity.Unknown, "sticker_c_kanim", "c"),
      new StickerBombFacadeInfo("d", (string) STICKERNAMES.STICKER_D, "TODO:DbStickers", PermitRarity.Unknown, "sticker_d_kanim", "d"),
      new StickerBombFacadeInfo("e", (string) STICKERNAMES.STICKER_E, "TODO:DbStickers", PermitRarity.Unknown, "sticker_e_kanim", "e"),
      new StickerBombFacadeInfo("f", (string) STICKERNAMES.STICKER_F, "TODO:DbStickers", PermitRarity.Unknown, "sticker_f_kanim", "f"),
      new StickerBombFacadeInfo("g", (string) STICKERNAMES.STICKER_G, "TODO:DbStickers", PermitRarity.Unknown, "sticker_g_kanim", "g"),
      new StickerBombFacadeInfo("h", (string) STICKERNAMES.STICKER_H, "TODO:DbStickers", PermitRarity.Unknown, "sticker_h_kanim", "h"),
      new StickerBombFacadeInfo("rocket", (string) STICKERNAMES.STICKER_ROCKET, "TODO:DbStickers", PermitRarity.Unknown, "sticker_rocket_kanim", "rocket"),
      new StickerBombFacadeInfo("paperplane", (string) STICKERNAMES.STICKER_PAPERPLANE, "TODO:DbStickers", PermitRarity.Unknown, "sticker_paperplane_kanim", "paperplane"),
      new StickerBombFacadeInfo("plant", (string) STICKERNAMES.STICKER_PLANT, "TODO:DbStickers", PermitRarity.Unknown, "sticker_plant_kanim", "plant"),
      new StickerBombFacadeInfo("plantpot", (string) STICKERNAMES.STICKER_PLANTPOT, "TODO:DbStickers", PermitRarity.Unknown, "sticker_plantpot_kanim", "plantpot"),
      new StickerBombFacadeInfo("mushroom", (string) STICKERNAMES.STICKER_MUSHROOM, "TODO:DbStickers", PermitRarity.Unknown, "sticker_mushroom_kanim", "mushroom"),
      new StickerBombFacadeInfo("mermaid", (string) STICKERNAMES.STICKER_MERMAID, "TODO:DbStickers", PermitRarity.Unknown, "sticker_mermaid_kanim", "mermaid"),
      new StickerBombFacadeInfo("spacepet", (string) STICKERNAMES.STICKER_SPACEPET, "TODO:DbStickers", PermitRarity.Unknown, "sticker_spacepet_kanim", "spacepet"),
      new StickerBombFacadeInfo("spacepet2", (string) STICKERNAMES.STICKER_SPACEPET2, "TODO:DbStickers", PermitRarity.Unknown, "sticker_spacepet2_kanim", "spacepet2"),
      new StickerBombFacadeInfo("spacepet3", (string) STICKERNAMES.STICKER_SPACEPET3, "TODO:DbStickers", PermitRarity.Unknown, "sticker_spacepet3_kanim", "spacepet3"),
      new StickerBombFacadeInfo("spacepet4", (string) STICKERNAMES.STICKER_SPACEPET4, "TODO:DbStickers", PermitRarity.Unknown, "sticker_spacepet4_kanim", "spacepet4"),
      new StickerBombFacadeInfo("spacepet5", (string) STICKERNAMES.STICKER_SPACEPET5, "TODO:DbStickers", PermitRarity.Unknown, "sticker_spacepet5_kanim", "spacepet5"),
      new StickerBombFacadeInfo("unicorn", (string) STICKERNAMES.STICKER_UNICORN, "TODO:DbStickers", PermitRarity.Unknown, "sticker_unicorn_kanim", "unicorn")
    });
  }

  private void SetupEquippableFacades()
  {
    this.blueprintCollection.equippableFacades.AddRange((IEnumerable<EquippableFacadeInfo>) new EquippableFacadeInfo[12]
    {
      new EquippableFacadeInfo("clubshirt", (string) EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.CLUBSHIRT, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_clubshirt_kanim", "shirt_clubshirt_kanim"),
      new EquippableFacadeInfo("cummerbund", (string) EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.CUMMERBUND, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_cummerbund_kanim", "shirt_cummerbund_kanim"),
      new EquippableFacadeInfo("decor_02", (string) EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.DECOR_02, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_decor02_kanim", "shirt_decor02_kanim"),
      new EquippableFacadeInfo("decor_03", (string) EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.DECOR_03, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_decor03_kanim", "shirt_decor03_kanim"),
      new EquippableFacadeInfo("decor_04", (string) EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.DECOR_04, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_decor04_kanim", "shirt_decor04_kanim"),
      new EquippableFacadeInfo("decor_05", (string) EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.DECOR_05, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_decor05_kanim", "shirt_decor05_kanim"),
      new EquippableFacadeInfo("gaudysweater", (string) EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.GAUDYSWEATER, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_gaudysweater_kanim", "shirt_gaudysweater_kanim"),
      new EquippableFacadeInfo("limone", (string) EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.LIMONE, "n/a", PermitRarity.Unknown, "CustomClothing", "body_suit_limone_kanim", "suit_limone_kanim"),
      new EquippableFacadeInfo("mondrian", (string) EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.MONDRIAN, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_mondrian_kanim", "shirt_mondrian_kanim"),
      new EquippableFacadeInfo("overalls", (string) EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.OVERALLS, "n/a", PermitRarity.Unknown, "CustomClothing", "body_suit_overalls_kanim", "suit_overalls_kanim"),
      new EquippableFacadeInfo("triangles", (string) EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.TRIANGLES, "n/a", PermitRarity.Unknown, "CustomClothing", "body_shirt_triangles_kanim", "shirt_triangles_kanim"),
      new EquippableFacadeInfo("workout", (string) EQUIPMENT.PREFABS.CUSTOMCLOTHING.FACADES.WORKOUT, "n/a", PermitRarity.Unknown, "CustomClothing", "body_suit_workout_kanim", "suit_workout_kanim")
    });
  }

  private void SetupMonumentParts()
  {
    this.blueprintCollection.monumentParts.AddRange((IEnumerable<MonumentPartInfo>) new MonumentPartInfo[40]
    {
      new MonumentPartInfo("bottom_option_a", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_A.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_A.DESC, PermitRarity.Universal, "monument_base_a_kanim", "option_a", "straight_legs", MonumentPartResource.Part.Bottom),
      new MonumentPartInfo("bottom_option_b", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_B.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_B.DESC, PermitRarity.Universal, "monument_base_b_kanim", "option_b", "wide_stance", MonumentPartResource.Part.Bottom),
      new MonumentPartInfo("bottom_option_c", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_C.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_C.DESC, PermitRarity.Universal, "monument_base_c_kanim", "option_c", "hmmm_legs", MonumentPartResource.Part.Bottom),
      new MonumentPartInfo("bottom_option_d", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_D.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_D.DESC, PermitRarity.Universal, "monument_base_d_kanim", "option_d", "sitting_stool", MonumentPartResource.Part.Bottom),
      new MonumentPartInfo("bottom_option_e", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_E.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_E.DESC, PermitRarity.Universal, "monument_base_e_kanim", "option_e", "wide_stance2", MonumentPartResource.Part.Bottom),
      new MonumentPartInfo("bottom_option_f", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_F.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_F.DESC, PermitRarity.Universal, "monument_base_f_kanim", "option_f", "posing1", MonumentPartResource.Part.Bottom),
      new MonumentPartInfo("bottom_option_g", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_G.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_G.DESC, PermitRarity.Universal, "monument_base_g_kanim", "option_g", "knee_kick", MonumentPartResource.Part.Bottom),
      new MonumentPartInfo("bottom_option_h", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_H.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_H.DESC, PermitRarity.Universal, "monument_base_h_kanim", "option_h", "step_on_hatches", MonumentPartResource.Part.Bottom),
      new MonumentPartInfo("bottom_option_i", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_I.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_I.DESC, PermitRarity.Universal, "monument_base_i_kanim", "option_i", "sit_on_tools", MonumentPartResource.Part.Bottom),
      new MonumentPartInfo("bottom_option_j", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_J.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_J.DESC, PermitRarity.Universal, "monument_base_j_kanim", "option_j", "water_pacu", MonumentPartResource.Part.Bottom),
      new MonumentPartInfo("bottom_option_k", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_K.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_K.DESC, PermitRarity.Universal, "monument_base_k_kanim", "option_k", "sit_on_eggs", MonumentPartResource.Part.Bottom),
      new MonumentPartInfo("mid_option_a", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_A.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_A.DESC, PermitRarity.Universal, "monument_mid_a_kanim", "option_a", "thumbs_up", MonumentPartResource.Part.Middle),
      new MonumentPartInfo("mid_option_b", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_B.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_B.DESC, PermitRarity.Universal, "monument_mid_b_kanim", "option_b", "wrench", MonumentPartResource.Part.Middle),
      new MonumentPartInfo("mid_option_c", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_C.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_C.DESC, PermitRarity.Universal, "monument_mid_c_kanim", "option_c", "hmmm", MonumentPartResource.Part.Middle),
      new MonumentPartInfo("mid_option_d", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_D.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_D.DESC, PermitRarity.Universal, "monument_mid_d_kanim", "option_d", "hips_hands", MonumentPartResource.Part.Middle),
      new MonumentPartInfo("mid_option_e", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_E.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_E.DESC, PermitRarity.Universal, "monument_mid_e_kanim", "option_e", "hold_face", MonumentPartResource.Part.Middle),
      new MonumentPartInfo("mid_option_f", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_F.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_F.DESC, PermitRarity.Universal, "monument_mid_f_kanim", "option_f", "finger_gun", MonumentPartResource.Part.Middle),
      new MonumentPartInfo("mid_option_g", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_G.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_G.DESC, PermitRarity.Universal, "monument_mid_g_kanim", "option_g", "model_pose", MonumentPartResource.Part.Middle),
      new MonumentPartInfo("mid_option_h", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_H.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_H.DESC, PermitRarity.Universal, "monument_mid_h_kanim", "option_h", "punch", MonumentPartResource.Part.Middle),
      new MonumentPartInfo("mid_option_i", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_I.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_I.DESC, PermitRarity.Universal, "monument_mid_i_kanim", "option_i", "holding_hatch", MonumentPartResource.Part.Middle),
      new MonumentPartInfo("mid_option_j", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_J.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_J.DESC, PermitRarity.Universal, "monument_mid_j_kanim", "option_j", "model_pose2", MonumentPartResource.Part.Middle),
      new MonumentPartInfo("mid_option_k", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_K.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_K.DESC, PermitRarity.Universal, "monument_mid_k_kanim", "option_k", "balancing", MonumentPartResource.Part.Middle),
      new MonumentPartInfo("mid_option_l", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_L.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_L.DESC, PermitRarity.Universal, "monument_mid_l_kanim", "option_l", "holding_babies", MonumentPartResource.Part.Middle),
      new MonumentPartInfo("top_option_a", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_A.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_A.DESC, PermitRarity.Universal, "monument_upper_a_kanim", "option_a", "leira", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_b", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_B.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_B.DESC, PermitRarity.Universal, "monument_upper_b_kanim", "option_b", "mae", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_c", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_C.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_C.DESC, PermitRarity.Universal, "monument_upper_c_kanim", "option_c", "puft", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_d", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_D.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_D.DESC, PermitRarity.Universal, "monument_upper_d_kanim", "option_d", "nikola", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_e", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_E.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_E.DESC, PermitRarity.Universal, "monument_upper_e_kanim", "option_e", "burt", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_f", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_F.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_F.DESC, PermitRarity.Universal, "monument_upper_f_kanim", "option_f", "rowan", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_g", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_G.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_G.DESC, PermitRarity.Universal, "monument_upper_g_kanim", "option_g", "nisbet", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_h", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_H.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_H.DESC, PermitRarity.Universal, "monument_upper_h_kanim", "option_h", "joshua", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_i", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_I.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_I.DESC, PermitRarity.Universal, "monument_upper_i_kanim", "option_i", "ren", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_j", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_J.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_J.DESC, PermitRarity.Universal, "monument_upper_j_kanim", "option_j", "hatch", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_k", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_K.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_K.DESC, PermitRarity.Universal, "monument_upper_k_kanim", "option_k", "drecko", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_l", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_L.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_L.DESC, PermitRarity.Universal, "monument_upper_l_kanim", "option_l", "driller", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_m", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_M.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_M.DESC, PermitRarity.Universal, "monument_upper_m_kanim", "option_m", "gassymoo", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_n", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_N.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_N.DESC, PermitRarity.Universal, "monument_upper_n_kanim", "option_n", "glom", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_o", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_O.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_O.DESC, PermitRarity.Universal, "monument_upper_o_kanim", "option_o", "lightbug", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_p", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_P.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_P.DESC, PermitRarity.Universal, "monument_upper_p_kanim", "option_p", "slickster", MonumentPartResource.Part.Top),
      new MonumentPartInfo("top_option_q", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_Q.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_Q.DESC, PermitRarity.Universal, "monument_upper_q_kanim", "option_q", "pacu", MonumentPartResource.Part.Top)
    });
    this.blueprintCollection.monumentParts.AddRange((IEnumerable<MonumentPartInfo>) new MonumentPartInfo[21]
    {
      new MonumentPartInfo("bottom_option_l", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_L.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_L.DESC, PermitRarity.Universal, "monument_base_l_kanim", "option_l", "rocketnosecone", MonumentPartResource.Part.Bottom, DlcManager.EXPANSION1),
      new MonumentPartInfo("bottom_option_m", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_M.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_M.DESC, PermitRarity.Universal, "monument_base_m_kanim", "option_m", "rocketsugarengine", MonumentPartResource.Part.Bottom, DlcManager.EXPANSION1),
      new MonumentPartInfo("bottom_option_n", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_N.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_N.DESC, PermitRarity.Universal, "monument_base_n_kanim", "option_n", "rocketnCO2", MonumentPartResource.Part.Bottom, DlcManager.EXPANSION1),
      new MonumentPartInfo("bottom_option_o", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_O.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_O.DESC, PermitRarity.Universal, "monument_base_o_kanim", "option_o", "rocketpetro", MonumentPartResource.Part.Bottom, DlcManager.EXPANSION1),
      new MonumentPartInfo("bottom_option_p", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_P.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_P.DESC, PermitRarity.Universal, "monument_base_p_kanim", "option_p", "rocketnoseconesmall", MonumentPartResource.Part.Bottom, DlcManager.EXPANSION1),
      new MonumentPartInfo("bottom_option_q", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_Q.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_Q.DESC, PermitRarity.Universal, "monument_base_q_kanim", "option_q", "rocketradengine", MonumentPartResource.Part.Bottom, DlcManager.EXPANSION1),
      new MonumentPartInfo("bottom_option_r", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_R.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_R.DESC, PermitRarity.Universal, "monument_base_r_kanim", "option_r", "sweepyoff", MonumentPartResource.Part.Bottom, DlcManager.EXPANSION1),
      new MonumentPartInfo("bottom_option_s", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_S.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_S.DESC, PermitRarity.Universal, "monument_base_s_kanim", "option_s", "sweepypeek", MonumentPartResource.Part.Bottom, DlcManager.EXPANSION1),
      new MonumentPartInfo("bottom_option_t", (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_T.NAME, (string) BUILDINGS.PREFABS.MONUMENTBOTTOM.FACADES.OPTION_T.DESC, PermitRarity.Universal, "monument_base_t_kanim", "option_t", "sweepy", MonumentPartResource.Part.Bottom, DlcManager.EXPANSION1),
      new MonumentPartInfo("mid_option_m", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_M.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_M.DESC, PermitRarity.Universal, "monument_mid_m_kanim", "option_m", "rocket", MonumentPartResource.Part.Middle, DlcManager.EXPANSION1),
      new MonumentPartInfo("mid_option_n", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_N.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_N.DESC, PermitRarity.Universal, "monument_mid_n_kanim", "option_n", "holding_baby_worm", MonumentPartResource.Part.Middle, DlcManager.EXPANSION1),
      new MonumentPartInfo("mid_option_o", (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_O.NAME, (string) BUILDINGS.PREFABS.MONUMENTMIDDLE.FACADES.OPTION_O.DESC, PermitRarity.Universal, "monument_mid_o_kanim", "option_o", "holding_baby_blarva_critter", MonumentPartResource.Part.Middle, DlcManager.EXPANSION1),
      new MonumentPartInfo("top_option_r", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_R.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_R.DESC, PermitRarity.Universal, "monument_upper_r_kanim", "option_r", "bee", MonumentPartResource.Part.Top, DlcManager.EXPANSION1),
      new MonumentPartInfo("top_option_s", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_S.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_S.DESC, PermitRarity.Universal, "monument_upper_s_kanim", "option_s", "critter", MonumentPartResource.Part.Top, DlcManager.EXPANSION1),
      new MonumentPartInfo("top_option_t", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_T.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_T.DESC, PermitRarity.Universal, "monument_upper_t_kanim", "option_t", "caterpillar", MonumentPartResource.Part.Top, DlcManager.EXPANSION1),
      new MonumentPartInfo("top_option_u", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_U.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_U.DESC, PermitRarity.Universal, "monument_upper_u_kanim", "option_u", "worm", MonumentPartResource.Part.Top, DlcManager.EXPANSION1),
      new MonumentPartInfo("top_option_v", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_V.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_V.DESC, PermitRarity.Universal, "monument_upper_v_kanim", "option_v", "scout_bot", MonumentPartResource.Part.Top, DlcManager.EXPANSION1),
      new MonumentPartInfo("top_option_w", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_W.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_W.DESC, PermitRarity.Universal, "monument_upper_w_kanim", "option_w", "MiMa", MonumentPartResource.Part.Top, DlcManager.EXPANSION1),
      new MonumentPartInfo("top_option_x", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_X.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_X.DESC, PermitRarity.Universal, "monument_upper_x_kanim", "option_x", "Stinky", MonumentPartResource.Part.Top, DlcManager.EXPANSION1),
      new MonumentPartInfo("top_option_y", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_Y.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_Y.DESC, PermitRarity.Universal, "monument_upper_y_kanim", "option_y", "Harold", MonumentPartResource.Part.Top, DlcManager.EXPANSION1),
      new MonumentPartInfo("top_option_z", (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_Z.NAME, (string) BUILDINGS.PREFABS.MONUMENTTOP.FACADES.OPTION_Z.DESC, PermitRarity.Universal, "monument_upper_z_kanim", "option_z", "Nails", MonumentPartResource.Part.Top, DlcManager.EXPANSION1)
    });
  }
}
