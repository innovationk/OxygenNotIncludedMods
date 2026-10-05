// Decompiled with JetBrains decompiler
// Type: Database.TechItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
namespace Database;

public class TechItems(ResourceSet parent) : ResourceSet<TechItem>(nameof (TechItems), parent)
{
  public const string AUTOMATION_OVERLAY_ID = "AutomationOverlay";
  public TechItem automationOverlay;
  public const string SUITS_OVERLAY_ID = "SuitsOverlay";
  public TechItem suitsOverlay;
  public const string JET_SUIT_ID = "JetSuit";
  public TechItem jetSuit;
  public const string ATMO_SUIT_ID = "AtmoSuit";
  public TechItem atmoSuit;
  public const string OXYGEN_MASK_ID = "OxygenMask";
  public TechItem oxygenMask;
  public const string LEAD_SUIT_ID = "LeadSuit";
  public TechItem leadSuit;
  public TechItem disposableElectrobankMetalOre;
  public TechItem lubricationStick;
  public TechItem gasket;
  public TechItem rubberBoots;
  public TechItem drySuit;
  public TechItem disposableElectrobankUraniumOre;
  public TechItem electrobank;
  public TechItem fetchDrone;
  public TechItem selfChargingElectrobank;
  public TechItem superLiquids;
  public const string BETA_RESEARCH_POINT_ID = "BetaResearchPoint";
  public TechItem betaResearchPoint;
  public const string GAMMA_RESEARCH_POINT_ID = "GammaResearchPoint";
  public TechItem gammaResearchPoint;
  public const string DELTA_RESEARCH_POINT_ID = "DeltaResearchPoint";
  public TechItem deltaResearchPoint;
  public const string ORBITAL_RESEARCH_POINT_ID = "OrbitalResearchPoint";
  public TechItem orbitalResearchPoint;
  public const string CONVEYOR_OVERLAY_ID = "ConveyorOverlay";
  public TechItem conveyorOverlay;

  public void Init()
  {
    this.automationOverlay = this.AddTechItem("AutomationOverlay", (string) RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_logic"));
    this.suitsOverlay = this.AddTechItem("SuitsOverlay", (string) RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_suit"));
    this.betaResearchPoint = this.AddTechItem("BetaResearchPoint", (string) RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_beta_icon"));
    this.gammaResearchPoint = this.AddTechItem("GammaResearchPoint", (string) RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_gamma_icon"));
    this.orbitalResearchPoint = this.AddTechItem("OrbitalResearchPoint", (string) RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_orbital_icon"));
    this.conveyorOverlay = this.AddTechItem("ConveyorOverlay", (string) RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_conveyor"));
    this.jetSuit = this.AddTechItem("JetSuit", (string) RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Jet_Suit".ToTag()));
    if (this.jetSuit != null)
      this.jetSuit.AddSearchTerms((string) SEARCH_TERMS.ATMOSUIT);
    this.atmoSuit = this.AddTechItem("AtmoSuit", (string) RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Atmo_Suit".ToTag()));
    if (this.atmoSuit != null)
      this.atmoSuit.AddSearchTerms((string) SEARCH_TERMS.ATMOSUIT);
    this.oxygenMask = this.AddTechItem("OxygenMask", (string) RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.DESC, this.GetPrefabSpriteFnBuilder("Oxygen_Mask".ToTag()));
    if (this.oxygenMask != null)
      this.oxygenMask.AddSearchTerms((string) SEARCH_TERMS.OXYGEN);
    this.superLiquids = this.AddTechItem("SUPER_LIQUIDS", (string) RESEARCH.OTHER_TECH_ITEMS.SUPER_LIQUIDS.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.SUPER_LIQUIDS.DESC, this.GetPrefabSpriteFnBuilder(SimHashes.ViscoGel.CreateTag()));
    this.deltaResearchPoint = this.AddTechItem("DeltaResearchPoint", (string) RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_delta_icon"), DlcManager.EXPANSION1, (string[]) null, false);
    this.leadSuit = this.AddTechItem("LeadSuit", (string) RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Lead_Suit".ToTag()), DlcManager.EXPANSION1, (string[]) null, false);
    this.disposableElectrobankMetalOre = this.AddTechItem("DisposableElectrobank_RawMetal", (string) RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_METAL_ORE.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_METAL_ORE.DESC, this.GetPrefabSpriteFnBuilder("DisposableElectrobank_RawMetal".ToTag()), DlcManager.DLC3, (string[]) null, false);
    if (this.disposableElectrobankMetalOre != null)
      this.disposableElectrobankMetalOre.AddSearchTerms((string) SEARCH_TERMS.BATTERY);
    this.lubricationStick = this.AddTechItem("LubricationStick", (string) RESEARCH.OTHER_TECH_ITEMS.LUBRICATION_STICK.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.LUBRICATION_STICK.DESC, this.GetPrefabSpriteFnBuilder("LubricationStick".ToTag()), DlcManager.DLC3, (string[]) null, false);
    if (this.lubricationStick != null)
    {
      this.lubricationStick.AddSearchTerms((string) SEARCH_TERMS.MEDICINE);
      this.lubricationStick.AddSearchTerms((string) SEARCH_TERMS.BIONIC);
    }
    this.gasket = this.AddTechItem("PlasticGasket", (string) RESEARCH.OTHER_TECH_ITEMS.GASKET.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.GASKET.DESC, this.GetPrefabSpriteFnBuilder("PlasticGasket".ToTag()), poi_unlock: true);
    this.rubberBoots = this.AddTechItem(RubberBootsConfig.ID, (string) RESEARCH.OTHER_TECH_ITEMS.RUBBER_BOOTS.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.RUBBER_BOOTS.DESC, this.GetPrefabSpriteFnBuilder(RubberBootsConfig.ID.ToTag()), DlcManager.DLC5, (string[]) null, false);
    this.drySuit = this.AddTechItem("DrySuit", (string) RESEARCH.OTHER_TECH_ITEMS.DRY_SUIT.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.DRY_SUIT.DESC, this.GetPrefabSpriteFnBuilder("DrySuit".ToTag()), DlcManager.DLC5, (string[]) null, false);
    this.disposableElectrobankUraniumOre = this.AddTechItem("DisposableElectrobank_UraniumOre", (string) RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_URANIUM_ORE.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_URANIUM_ORE.DESC, this.GetPrefabSpriteFnBuilder("DisposableElectrobank_UraniumOre".ToTag()), new string[2]
    {
      "EXPANSION1_ID",
      "DLC3_ID"
    }, (string[]) null, false);
    if (this.disposableElectrobankUraniumOre != null)
      this.disposableElectrobankUraniumOre.AddSearchTerms((string) SEARCH_TERMS.BATTERY);
    this.electrobank = this.AddTechItem("Electrobank", (string) RESEARCH.OTHER_TECH_ITEMS.ELECTROBANK.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.ELECTROBANK.DESC, this.GetPrefabSpriteFnBuilder("Electrobank".ToTag()), DlcManager.DLC3, (string[]) null, false);
    if (this.electrobank != null)
      this.electrobank.AddSearchTerms((string) SEARCH_TERMS.BATTERY);
    this.fetchDrone = this.AddTechItem("FetchDrone", (string) RESEARCH.OTHER_TECH_ITEMS.FETCHDRONE.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.FETCHDRONE.DESC, this.GetPrefabSpriteFnBuilder("FetchDrone".ToTag()), DlcManager.DLC3, (string[]) null, false);
    if (this.fetchDrone != null)
      this.fetchDrone.AddSearchTerms((string) SEARCH_TERMS.ROBOT);
    this.selfChargingElectrobank = this.AddTechItem("SelfChargingElectrobank", (string) RESEARCH.OTHER_TECH_ITEMS.SELFCHARGINGELECTROBANK.NAME, (string) RESEARCH.OTHER_TECH_ITEMS.SELFCHARGINGELECTROBANK.DESC, this.GetPrefabSpriteFnBuilder("SelfChargingElectrobank".ToTag()), new string[2]
    {
      "EXPANSION1_ID",
      "DLC3_ID"
    }, (string[]) null, false);
    if (this.selfChargingElectrobank == null)
      return;
    this.selfChargingElectrobank.AddSearchTerms((string) SEARCH_TERMS.BATTERY);
  }

  private Func<string, bool, Sprite> GetSpriteFnBuilder(string spriteName)
  {
    return (Func<string, bool, Sprite>) ((anim, centered) => Assets.GetSprite((HashedString) spriteName));
  }

  private Func<string, bool, Sprite> GetPrefabSpriteFnBuilder(Tag prefabTag)
  {
    return (Func<string, bool, Sprite>) ((anim, centered) => Def.GetUISprite((object) prefabTag).first);
  }

  [Obsolete("Used AddTechItem with requiredDlcIds and forbiddenDlcIds instead.")]
  public TechItem AddTechItem(
    string id,
    string name,
    string description,
    Func<string, bool, Sprite> getUISprite,
    string[] DLCIds,
    bool poi_unlock = false)
  {
    string[] requiredDlcIds;
    string[] forbiddenDlcIds;
    DlcManager.ConvertAvailableToRequireAndForbidden(DLCIds, out requiredDlcIds, out forbiddenDlcIds);
    return this.AddTechItem(id, name, description, getUISprite, requiredDlcIds, forbiddenDlcIds, poi_unlock);
  }

  public TechItem AddTechItem(
    string id,
    string name,
    string description,
    Func<string, bool, Sprite> getUISprite,
    string[] requiredDlcIds = null,
    string[] forbiddenDlcIds = null,
    bool poi_unlock = false)
  {
    if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
      return (TechItem) null;
    if (this.TryGet(id) != null)
    {
      DebugUtil.LogWarningArgs((object) "Tried adding a tech item called", (object) id, (object) name, (object) "but it was already added!");
      return this.Get(id);
    }
    Tech techFromItemId = this.GetTechFromItemID(id);
    if (techFromItemId == null)
      return (TechItem) null;
    TechItem techItem = new TechItem(id, (ResourceSet) this, name, description, getUISprite, techFromItemId.Id, requiredDlcIds, forbiddenDlcIds, poi_unlock);
    techFromItemId.unlockedItems.Add(techItem);
    return techItem;
  }

  public bool IsTechItemComplete(string id)
  {
    bool flag = true;
    foreach (TechItem resource in this.resources)
    {
      if (resource.Id == id)
      {
        flag = resource.IsComplete();
        break;
      }
    }
    return flag;
  }

  public Tech GetTechFromItemID(string itemId) => Db.Get().Techs?.TryGetTechForTechItem(itemId);

  public int GetTechTierForItem(string itemId)
  {
    Tech techFromItemId = this.GetTechFromItemID(itemId);
    return techFromItemId != null ? Techs.GetTier(techFromItemId) : 0;
  }
}
