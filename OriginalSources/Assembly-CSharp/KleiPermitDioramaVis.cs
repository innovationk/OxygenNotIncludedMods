// Decompiled with JetBrains decompiler
// Type: KleiPermitDioramaVis
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KleiPermitDioramaVis : KMonoBehaviour
{
  [SerializeField]
  private Image dlcImage;
  [SerializeField]
  private KleiPermitDioramaVis_Fallback fallbackVis;
  [SerializeField]
  private KleiPermitDioramaVis_DupeEquipment equipmentVis;
  [SerializeField]
  private KleiPermitDioramaVis_BuildingOnFloor buildingOnFloorVis;
  [SerializeField]
  private KleiPermitDioramaVis_BuildingOnFloorBig buildingOnFloorBigVis;
  [SerializeField]
  private KleiPermitDioramaVis_BuildingPresentationStand buildingOnWallVis;
  [SerializeField]
  private KleiPermitDioramaVis_BuildingPresentationStand buildingOnCeilingVis;
  [SerializeField]
  private KleiPermitDioramaVis_BuildingPresentationStand buildingInCeilingCornerVis;
  [SerializeField]
  private KleiPermitDioramaVis_BuildingRocket buildingRocketVis;
  [SerializeField]
  private KleiPermitDioramaVis_BuildingOnFloor buildingOnFloorBotanicalVis;
  [SerializeField]
  private KleiPermitDioramaVis_BuildingHangingHook buildingHangingHookBotanicalVis;
  [SerializeField]
  private KleiPermitDioramaVis_WiresAndAutomation buildingWiresAndAutomationVis;
  [SerializeField]
  private KleiPermitDioramaVis_AutomationGates buildingAutomationGatesVis;
  [SerializeField]
  private KleiPermitDioramaVis_Wallpaper wallpaperVis;
  [SerializeField]
  private KleiPermitDioramaVis_ArtablePainting artablePaintingVis;
  [SerializeField]
  private KleiPermitDioramaVis_ArtableSculpture artableSculptureVis;
  [SerializeField]
  private KleiPermitDioramaVis_JoyResponseBalloon joyResponseBalloonVis;
  [SerializeField]
  private KleiPermitDioramaVis_MonumentPart monumentPartVis;
  private bool initComplete;
  private IReadOnlyList<IKleiPermitDioramaVisTarget> allVisList;
  public static PermitResource lastRenderedPermit;

  protected override void OnPrefabInit() => this.Init();

  private void Init()
  {
    if (this.initComplete)
      return;
    this.allVisList = (IReadOnlyList<IKleiPermitDioramaVisTarget>) ReflectionUtil.For<KleiPermitDioramaVis>(this).CollectValuesForFieldsThatInheritOrImplement<IKleiPermitDioramaVisTarget>();
    foreach (IKleiPermitDioramaVisTarget allVis in (IEnumerable<IKleiPermitDioramaVisTarget>) this.allVisList)
      allVis.ConfigureSetup();
    this.initComplete = true;
  }

  public void ConfigureWith(PermitResource permit)
  {
    if (!this.initComplete)
      this.Init();
    foreach (IKleiPermitDioramaVisTarget allVis in (IEnumerable<IKleiPermitDioramaVisTarget>) this.allVisList)
      allVis.GetGameObject().SetActive(false);
    KleiPermitVisUtil.ClearAnimation();
    IKleiPermitDioramaVisTarget permitVisTarget = this.GetPermitVisTarget(permit);
    permitVisTarget.GetGameObject().SetActive(true);
    permitVisTarget.ConfigureWith(permit);
    string dlcIdFrom = permit.GetDlcIdFrom();
    if (DlcManager.IsDlcId(dlcIdFrom))
    {
      this.dlcImage.gameObject.SetActive(true);
      this.dlcImage.sprite = Assets.GetSprite((HashedString) DlcManager.GetDlcLargeLogo(dlcIdFrom));
    }
    else
      this.dlcImage.gameObject.SetActive(false);
  }

  private IKleiPermitDioramaVisTarget GetPermitVisTarget(PermitResource permit)
  {
    KleiPermitDioramaVis.lastRenderedPermit = permit;
    if (permit == null)
      return (IKleiPermitDioramaVisTarget) this.fallbackVis.WithError($"Given invalid permit: {permit}");
    if (permit.Category == PermitCategory.Equipment || permit.Category == PermitCategory.DupeTops || permit.Category == PermitCategory.DupeBottoms || permit.Category == PermitCategory.DupeGloves || permit.Category == PermitCategory.DupeShoes || permit.Category == PermitCategory.DupeHats || permit.Category == PermitCategory.DupeAccessories || permit.Category == PermitCategory.AtmoSuitHelmet || permit.Category == PermitCategory.AtmoSuitBody || permit.Category == PermitCategory.AtmoSuitGloves || permit.Category == PermitCategory.AtmoSuitBelt || permit.Category == PermitCategory.AtmoSuitShoes || permit.Category == PermitCategory.JetSuitHelmet || permit.Category == PermitCategory.JetSuitBody || permit.Category == PermitCategory.JetSuitGloves || permit.Category == PermitCategory.JetSuitShoes)
      return (IKleiPermitDioramaVisTarget) this.equipmentVis;
    if (permit.Category == PermitCategory.Building)
    {
      BuildLocationRule? buildLocationRule = KleiPermitVisUtil.GetBuildLocationRule(permit);
      BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
      if ((Object) buildingDef == (Object) null || !buildingDef.BuildingComplete.GetComponent<Bed>().IsNullOrDestroyed())
        return (IKleiPermitDioramaVisTarget) this.buildingOnFloorVis;
      if (permit is BuildingFacadeResource buildingFacadeResource)
      {
        if (buildingFacadeResource.PrefabID.Contains("Wire") || buildingFacadeResource.PrefabID.Contains("Ribbon"))
          return (IKleiPermitDioramaVisTarget) this.buildingWiresAndAutomationVis;
        if (buildingFacadeResource.PrefabID.Contains("Logic"))
          return (IKleiPermitDioramaVisTarget) this.buildingAutomationGatesVis;
      }
      if (buildingDef.PrefabID == "RockCrusher" || buildingDef.PrefabID == "GasReservoir" || buildingDef.PrefabID == "ArcadeMachine" || buildingDef.PrefabID == "MicrobeMusher" || buildingDef.PrefabID == "FlushToilet" || buildingDef.PrefabID == "WashSink" || buildingDef.PrefabID == "Headquarters" || buildingDef.PrefabID == "GourmetCookingStation" || buildingDef.PrefabID == "ExobaseHeadquarters" || buildingDef.PrefabID == "SteamTurbine2" || buildingDef.PrefabID == "Generator" || buildingDef.PrefabID == "ResetSkillsStation" || buildingDef.PrefabID == "MetalRefinery" || buildingDef.PrefabID == "WaterPurifier")
        return (IKleiPermitDioramaVisTarget) this.buildingOnFloorBigVis;
      if (!buildingDef.BuildingComplete.GetComponent<RocketModule>().IsNullOrDestroyed() || !buildingDef.BuildingComplete.GetComponent<RocketEngine>().IsNullOrDestroyed())
        return (IKleiPermitDioramaVisTarget) this.buildingRocketVis;
      if (buildingDef.PrefabID == "PlanterBox" || buildingDef.PrefabID == "FlowerVase")
        return (IKleiPermitDioramaVisTarget) this.buildingOnFloorBotanicalVis;
      if (buildingDef.PrefabID == "ExteriorWall")
        return (IKleiPermitDioramaVisTarget) this.wallpaperVis;
      if (buildingDef.PrefabID == "FlowerVaseHanging" || buildingDef.PrefabID == "FlowerVaseHangingFancy")
        return (IKleiPermitDioramaVisTarget) this.buildingHangingHookBotanicalVis;
      if (buildLocationRule.HasValue)
      {
        switch (buildLocationRule.GetValueOrDefault())
        {
          case BuildLocationRule.Anywhere:
          case BuildLocationRule.OnFloor:
          case BuildLocationRule.OnFoundationRotatable:
            return (IKleiPermitDioramaVisTarget) this.buildingOnFloorVis;
          case BuildLocationRule.OnCeiling:
            return (IKleiPermitDioramaVisTarget) this.buildingOnCeilingVis.WithAlignment(Alignment.Top());
          case BuildLocationRule.OnWall:
            return (IKleiPermitDioramaVisTarget) this.buildingOnWallVis.WithAlignment(Alignment.Left());
          case BuildLocationRule.InCorner:
            return (IKleiPermitDioramaVisTarget) this.buildingInCeilingCornerVis.WithAlignment(Alignment.TopLeft());
        }
      }
      return (IKleiPermitDioramaVisTarget) this.fallbackVis.WithError($"No visualization available for building with BuildLocationRule of {buildLocationRule}");
    }
    if (permit.Category == PermitCategory.Artwork)
    {
      BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
      if (buildingDef.IsNullOrDestroyed())
        return (IKleiPermitDioramaVisTarget) this.fallbackVis.WithError("Couldn't find building def for Artable " + permit.Id);
      if (Has<Sculpture>(buildingDef))
        return buildingDef.PrefabID == "WoodSculpture" ? (IKleiPermitDioramaVisTarget) this.artablePaintingVis : (IKleiPermitDioramaVisTarget) this.artableSculptureVis;
      if (Has<Painting>(buildingDef))
        return (IKleiPermitDioramaVisTarget) this.artablePaintingVis;
      return Has<MonumentPart>(buildingDef) ? (IKleiPermitDioramaVisTarget) this.monumentPartVis : (IKleiPermitDioramaVisTarget) this.fallbackVis.WithError("No visualization available for Artable " + permit.Id);
    }
    if (permit.Category != PermitCategory.JoyResponse)
      return (IKleiPermitDioramaVisTarget) this.fallbackVis.WithError($"No visualization has been defined for permit with id \"{permit.Id}\"");
    return permit is BalloonArtistFacadeResource ? (IKleiPermitDioramaVisTarget) this.joyResponseBalloonVis : (IKleiPermitDioramaVisTarget) this.fallbackVis.WithError("No visualization available for JoyResponse " + permit.Id);

    static bool Has<T>(BuildingDef buildingDef) where T : Component
    {
      return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
    }
  }

  public static Sprite GetDioramaBackground(PermitCategory permitCategory)
  {
    switch (permitCategory)
    {
      case PermitCategory.DupeTops:
      case PermitCategory.DupeBottoms:
      case PermitCategory.DupeGloves:
      case PermitCategory.DupeShoes:
      case PermitCategory.DupeHats:
      case PermitCategory.DupeAccessories:
        return Assets.GetSprite((HashedString) "screen_bg_clothing");
      case PermitCategory.AtmoSuitHelmet:
      case PermitCategory.AtmoSuitBody:
      case PermitCategory.AtmoSuitGloves:
      case PermitCategory.AtmoSuitBelt:
      case PermitCategory.AtmoSuitShoes:
      case PermitCategory.JetSuitHelmet:
      case PermitCategory.JetSuitBody:
      case PermitCategory.JetSuitGloves:
      case PermitCategory.JetSuitShoes:
        return Assets.GetSprite((HashedString) "screen_bg_atmosuit");
      case PermitCategory.Building:
        return Assets.GetSprite((HashedString) "screen_bg_buildings");
      case PermitCategory.Artwork:
        return Assets.GetSprite((HashedString) "screen_bg_art");
      case PermitCategory.JoyResponse:
        return Assets.GetSprite((HashedString) "screen_bg_joyresponse");
      default:
        return (Sprite) null;
    }
  }

  public static Sprite GetDioramaBackground(ClothingOutfitUtility.OutfitType outfitType)
  {
    switch (outfitType)
    {
      case ClothingOutfitUtility.OutfitType.Clothing:
        return Assets.GetSprite((HashedString) "screen_bg_clothing");
      case ClothingOutfitUtility.OutfitType.JoyResponse:
        return Assets.GetSprite((HashedString) "screen_bg_joyresponse");
      case ClothingOutfitUtility.OutfitType.AtmoSuit:
      case ClothingOutfitUtility.OutfitType.JetSuit:
        return Assets.GetSprite((HashedString) "screen_bg_atmosuit");
      default:
        return (Sprite) null;
    }
  }
}
