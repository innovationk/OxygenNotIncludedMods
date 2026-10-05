// Decompiled with JetBrains decompiler
// Type: Database.MiscStatusItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Database;

public class MiscStatusItems : StatusItems
{
  public StatusItem AttentionRequired;
  public StatusItem MarkedForDisinfection;
  public StatusItem MarkedForCompost;
  public StatusItem MarkedForCompostInStorage;
  public StatusItem PendingClear;
  public StatusItem PendingClearNoStorage;
  public StatusItem Edible;
  public StatusItem WaitingForDig;
  public StatusItem WaitingForMop;
  public StatusItem OreMass;
  public StatusItem OreTemp;
  public StatusItem ElementalCategory;
  public StatusItem ElementalState;
  public StatusItem ElementalTemperature;
  public StatusItem ElementalMass;
  public StatusItem ElementalDisease;
  public StatusItem TreeFilterableTags;
  public StatusItem SublimationOverpressure;
  public StatusItem SublimationEmitting;
  public StatusItem SublimationBlocked;
  public StatusItem BuriedItem;
  public StatusItem SpoutOverPressure;
  public StatusItem SpoutEmitting;
  public StatusItem SpoutPressureBuilding;
  public StatusItem SpoutIdle;
  public StatusItem SpoutDormant;
  public StatusItem SpicedFood;
  public StatusItem RehydratedFood;
  public StatusItem OrderAttack;
  public StatusItem OrderCapture;
  public StatusItem PendingHarvest;
  public StatusItem NotMarkedForHarvest;
  public StatusItem PendingUproot;
  public StatusItem PickupableUnreachable;
  public StatusItem Prioritized;
  public StatusItem Using;
  public StatusItem Operating;
  public StatusItem Cleaning;
  public StatusItem RegionIsBlocked;
  public StatusItem NoClearLocationsAvailable;
  public StatusItem AwaitingStudy;
  public StatusItem Studied;
  public StatusItem StudiedGeyserTimeRemaining;
  public StatusItem Space;
  public StatusItem HighEnergyParticleCount;
  public StatusItem Durability;
  public StatusItem StoredItemDurability;
  public StatusItem ArtifactEntombed;
  public StatusItem TearOpen;
  public StatusItem TearClosed;
  public StatusItem ImpactorStatus;
  public StatusItem ImpactorHealth;
  public StatusItem LongRangeMissileTTI;
  public StatusItem ClusterMeteorRemainingTravelTime;
  public StatusItem MarkedForMove;
  public StatusItem MoveStorageUnreachable;
  public StatusItem GrowingBranches;
  public StatusItem BionicExplorerBooster;
  public StatusItem BionicExplorerBoosterReady;
  public StatusItem UnassignedBionicBooster;
  public StatusItem ElectrobankLifetimeRemaining;
  public StatusItem ElectrobankSelfCharging;
  public StatusItem ClusterMapHarvestableResource;
  public StatusItem BackwallMass;
  public StatusItem BackwallTemperature;
  public StatusItem BubbleContents;
  public StatusItem UnderwaterVentBuildUpProgress;
  public StatusItem UnderwaterVentEmiting;
  public StatusItem UnderwaterVentBlocked;
  public StatusItem UnderwaterVentBeingDrilled;
  public StatusItem MinnowPOIDehydratedStatus;

  public MiscStatusItems(ResourceSet parent)
    : base(nameof (MiscStatusItems), parent)
  {
    this.CreateStatusItems();
  }

  private StatusItem CreateStatusItem(
    string id,
    string prefix,
    string icon,
    StatusItem.IconType icon_type,
    NotificationType notification_type,
    bool allow_multiples,
    HashedString render_overlay,
    bool showWorldIcon = true,
    int status_overlays = 129022)
  {
    return this.Add(new StatusItem(id, prefix, icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays));
  }

  private StatusItem CreateStatusItem(
    string id,
    string name,
    string tooltip,
    string icon,
    StatusItem.IconType icon_type,
    NotificationType notification_type,
    bool allow_multiples,
    HashedString render_overlay,
    int status_overlays = 129022)
  {
    return this.Add(new StatusItem(id, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays));
  }

  private void CreateStatusItems()
  {
    this.AttentionRequired = this.CreateStatusItem("AttentionRequired", "MISC", "status_item_doubleexclamation", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Edible = this.CreateStatusItem("Edible", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Edible.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      global::Edible edible = (global::Edible) data;
      str = string.Format(str, (object) GameUtil.GetFormattedCalories(edible.Calories));
      return str;
    });
    this.PendingClear = this.CreateStatusItem("PendingClear", "MISC", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.PendingClearNoStorage = this.CreateStatusItem("PendingClearNoStorage", "MISC", "status_item_pending_clear", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.MarkedForCompost = this.CreateStatusItem("MarkedForCompost", "MISC", "status_item_pending_compost", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.MarkedForCompostInStorage = this.CreateStatusItem("MarkedForCompostInStorage", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.MarkedForDisinfection = this.CreateStatusItem("MarkedForDisinfection", "MISC", "status_item_disinfect", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.Disease.ID);
    this.NoClearLocationsAvailable = this.CreateStatusItem("NoClearLocationsAvailable", "MISC", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.WaitingForDig = this.CreateStatusItem("WaitingForDig", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.WaitingForMop = this.CreateStatusItem("WaitingForMop", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.OreMass = this.CreateStatusItem("OreMass", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.OreMass.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      GameObject gameObject = (GameObject) data;
      str = str.Replace("{Mass}", GameUtil.GetFormattedMass(gameObject.GetComponent<PrimaryElement>().Mass));
      return str;
    });
    this.UnderwaterVentBlocked = this.CreateStatusItem("UnderwaterVentBlocked", "MISC", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.UnderwaterVentBeingDrilled = this.CreateStatusItem("UnderwaterVentBeingDrilled", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.UnderwaterVentEmiting = this.CreateStatusItem("UnderwaterVentEmiting", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.UnderwaterVentEmiting.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      UnderwaterVent.Instance instance = (UnderwaterVent.Instance) data;
      str = str.Replace("{ELEMENT_NAME}", GameUtil.GetElementNameByElementHash(instance.def.data.BubbleElement));
      str = str.Replace("{RATE}", GameUtil.GetFormattedMass(instance.def.data.BubbleMassRate, GameUtil.TimeSlice.PerSecond));
      str = str.Replace("{TEMP}", GameUtil.GetFormattedTemperature(instance.def.data.BubbleTemp));
      return str;
    });
    this.UnderwaterVentBuildUpProgress = this.CreateStatusItem("UnderwaterVentBuildUpProgress", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.UnderwaterVentBuildUpProgress.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      UnderwaterVent.Instance instance = (UnderwaterVent.Instance) data;
      str = str.Replace("{PERCENTAGE}", GameUtil.GetFormattedPercent(instance.BuildUpProgress * 100f));
      return str;
    });
    this.OreTemp = this.CreateStatusItem("OreTemp", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.OreTemp.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      GameObject gameObject = (GameObject) data;
      str = str.Replace("{Temp}", GameUtil.GetFormattedTemperature(gameObject.GetComponent<PrimaryElement>().Temperature));
      return str;
    });
    this.ElementalState = this.CreateStatusItem("ElementalState", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ElementalState.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      Element element = ((Func<Element>) data)();
      str = str.Replace("{State}", element.GetStateString());
      return str;
    });
    this.ElementalCategory = this.CreateStatusItem("ElementalCategory", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ElementalCategory.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      Element element = ((Func<Element>) data)();
      str = str.Replace("{Category}", element.GetMaterialCategoryTag().ProperName());
      return str;
    });
    this.ElementalTemperature = this.CreateStatusItem("ElementalTemperature", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ElementalTemperature.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
      str = str.Replace("{Temp}", GameUtil.GetFormattedTemperature(cellSelectionObject.temperature));
      return str;
    });
    this.ElementalMass = this.CreateStatusItem("ElementalMass", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ElementalMass.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
      str = str.Replace("{Mass}", GameUtil.GetFormattedMass(cellSelectionObject.Mass));
      return str;
    });
    this.ElementalDisease = this.CreateStatusItem("ElementalDisease", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ElementalDisease.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
      str = str.Replace("{Disease}", GameUtil.GetFormattedDisease(cellSelectionObject.diseaseIdx, cellSelectionObject.diseaseCount));
      return str;
    });
    this.ElementalDisease.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
      str = str.Replace("{Disease}", GameUtil.GetFormattedDisease(cellSelectionObject.diseaseIdx, cellSelectionObject.diseaseCount, true));
      return str;
    });
    this.GrowingBranches = new StatusItem("GrowingBranches", "MISC", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID);
    this.TreeFilterableTags = this.CreateStatusItem("TreeFilterableTags", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.TreeFilterableTags.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      TreeFilterable treeFilterable = (TreeFilterable) data;
      str = str.Replace("{Tags}", treeFilterable.GetTagsAsStatus());
      return str;
    });
    this.SublimationEmitting = this.CreateStatusItem("SublimationEmitting", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.SublimationEmitting.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
      if (cellSelectionObject.element.sublimateId == (SimHashes) 0)
        return str;
      str = str.Replace("{Element}", GameUtil.GetElementNameByElementHash(cellSelectionObject.element.sublimateId));
      str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(cellSelectionObject.FlowRate, GameUtil.TimeSlice.PerSecond));
      return str;
    });
    this.SublimationEmitting.resolveTooltipCallback = this.SublimationEmitting.resolveStringCallback;
    this.SublimationBlocked = this.CreateStatusItem("SublimationBlocked", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.SublimationBlocked.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
      if (cellSelectionObject.element.sublimateId == (SimHashes) 0)
        return str;
      str = str.Replace("{Element}", cellSelectionObject.element.name);
      str = str.Replace("{SubElement}", GameUtil.GetElementNameByElementHash(cellSelectionObject.element.sublimateId));
      return str;
    });
    this.SublimationBlocked.resolveTooltipCallback = this.SublimationBlocked.resolveStringCallback;
    this.SublimationOverpressure = this.CreateStatusItem("SublimationOverpressure", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.SublimationOverpressure.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
      if (cellSelectionObject.element.sublimateId == (SimHashes) 0)
        return str;
      str = str.Replace("{Element}", cellSelectionObject.element.name);
      str = str.Replace("{SubElement}", GameUtil.GetElementNameByElementHash(cellSelectionObject.element.sublimateId));
      return str;
    });
    this.Space = this.CreateStatusItem("Space", "MISC", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID);
    this.BuriedItem = this.CreateStatusItem("BuriedItem", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.BackwallMass = this.CreateStatusItem("BackwallMass", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.BackwallMass.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      BackwallSelectionObject backwallSelectionObject = (BackwallSelectionObject) data;
      str = str.Replace("{Mass}", GameUtil.GetFormattedMass(backwallSelectionObject.Mass));
      return str;
    });
    this.BackwallTemperature = this.CreateStatusItem("BackwallTemperature", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.BackwallTemperature.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      BackwallSelectionObject backwallSelectionObject = (BackwallSelectionObject) data;
      str = str.Replace("{Temp}", GameUtil.GetFormattedTemperature(backwallSelectionObject.temperature));
      return str;
    });
    this.BubbleContents = this.CreateStatusItem("BubbleContents", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID);
    this.BubbleContents.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      CellSelectionObject cellSelectionObject = (CellSelectionObject) data;
      string statusItems = "";
      foreach (BubbleManager.CellBubbleInfo bubbleInfo in cellSelectionObject.bubbleInfos)
      {
        Element elementByHash = ElementLoader.FindElementByHash(bubbleInfo.element);
        if (statusItems.Length > 0)
          statusItems += "\n";
        statusItems = $"{statusItems}{elementByHash.name} {(string) UI.TOOLS.GENERIC.BUBBLE_LABEL}: {GameUtil.GetFormattedMass(bubbleInfo.totalMass)}";
      }
      return statusItems;
    });
    this.SpoutOverPressure = this.CreateStatusItem("SpoutOverPressure", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.SpoutOverPressure.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      Geyser.StatesInstance statesInstance = (Geyser.StatesInstance) data;
      Studyable component = statesInstance.GetComponent<Studyable>();
      str = statesInstance == null || !((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Studied ? str.Replace("{StudiedDetails}", "") : str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTOVERPRESSURE.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingEruptTime())));
      return str;
    });
    this.SpoutEmitting = this.CreateStatusItem("SpoutEmitting", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.SpoutEmitting.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      Geyser.StatesInstance statesInstance = (Geyser.StatesInstance) data;
      Studyable component = statesInstance.GetComponent<Studyable>();
      str = statesInstance == null || !((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Studied ? str.Replace("{StudiedDetails}", "") : str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTEMITTING.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingEruptTime())));
      return str;
    });
    this.SpoutPressureBuilding = this.CreateStatusItem("SpoutPressureBuilding", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.SpoutPressureBuilding.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      Geyser.StatesInstance statesInstance = (Geyser.StatesInstance) data;
      Studyable component = statesInstance.GetComponent<Studyable>();
      str = statesInstance == null || !((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Studied ? str.Replace("{StudiedDetails}", "") : str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTPRESSUREBUILDING.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingNonEruptTime())));
      return str;
    });
    this.SpoutIdle = this.CreateStatusItem("SpoutIdle", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.SpoutIdle.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      Geyser.StatesInstance statesInstance = (Geyser.StatesInstance) data;
      Studyable component = statesInstance.GetComponent<Studyable>();
      str = statesInstance == null || !((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Studied ? str.Replace("{StudiedDetails}", "") : str.Replace("{StudiedDetails}", MISC.STATUSITEMS.SPOUTIDLE.STUDIED.text.Replace("{Time}", GameUtil.GetFormattedCycles(statesInstance.master.RemainingNonEruptTime())));
      return str;
    });
    this.SpoutDormant = this.CreateStatusItem("SpoutDormant", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.SpicedFood = this.CreateStatusItem("SpicedFood", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.SpicedFood.resolveTooltipCallback = (Func<string, object, string>) ((baseString, data) =>
    {
      string statusItems = baseString;
      string str1 = "\n    • ";
      foreach (SpiceInstance spiceInstance in (List<SpiceInstance>) data)
      {
        string key = $"STRINGS.ITEMS.SPICES.{spiceInstance.Id.Name.ToUpper()}.NAME";
        StringEntry result;
        Strings.TryGet(key, out result);
        string str2 = result == null ? "MISSING " + key : result.String;
        statusItems = statusItems + str1 + str2;
        string linePrefix = "\n        • ";
        if (spiceInstance.StatBonus != null)
          statusItems += Effect.CreateTooltip(spiceInstance.StatBonus, false, linePrefix, false);
      }
      return statusItems;
    });
    this.RehydratedFood = this.CreateStatusItem("RehydratedFood", "MISC", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.OrderAttack = this.CreateStatusItem("OrderAttack", "MISC", "status_item_attack", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.OrderCapture = this.CreateStatusItem("OrderCapture", "MISC", "status_item_capture", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.PendingHarvest = this.CreateStatusItem("PendingHarvest", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.NotMarkedForHarvest = this.CreateStatusItem("NotMarkedForHarvest", "MISC", "status_item_building_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.NotMarkedForHarvest.conditionalOverlayCallback = (Func<HashedString, object, bool>) ((viewMode, o) => !(viewMode != OverlayModes.None.ID));
    this.PendingUproot = this.CreateStatusItem("PendingUproot", "MISC", "status_item_pending_uproot", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.PickupableUnreachable = this.CreateStatusItem("PickupableUnreachable", "MISC", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Prioritized = this.CreateStatusItem("Prioritized", "MISC", "status_item_prioritized", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Using = this.CreateStatusItem("Using", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Using.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      Workable workable = (Workable) data;
      if ((UnityEngine.Object) workable != (UnityEngine.Object) null)
      {
        KSelectable component = workable.GetComponent<KSelectable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          str = str.Replace("{Target}", component.GetName());
      }
      return str;
    });
    this.Operating = this.CreateStatusItem("Operating", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Cleaning = this.CreateStatusItem("Cleaning", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.RegionIsBlocked = this.CreateStatusItem("RegionIsBlocked", "MISC", "status_item_solids_blocking", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.AwaitingStudy = this.CreateStatusItem("AwaitingStudy", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Studied = this.CreateStatusItem("Studied", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.HighEnergyParticleCount = this.CreateStatusItem("HighEnergyParticleCount", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.HighEnergyParticleCount.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      GameObject gameObject = (GameObject) data;
      str = GameUtil.GetFormattedHighEnergyParticles(gameObject.IsNullOrDestroyed() ? 0.0f : gameObject.GetComponent<HighEnergyParticle>().payload);
      return str;
    });
    this.Durability = this.CreateStatusItem("Durability", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Durability.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      global::Durability component = ((GameObject) data).GetComponent<global::Durability>();
      str = str.Replace("{durability}", GameUtil.GetFormattedPercent(component.GetDurability() * 100f));
      return str;
    });
    this.BionicExplorerBooster = this.CreateStatusItem("BionicExplorerBooster", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.BionicExplorerBooster.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      BionicUpgrade_ExplorerBooster.Instance instance = (BionicUpgrade_ExplorerBooster.Instance) data;
      str = string.Format(str, (object) GameUtil.GetFormattedPercent(instance.Progress * 100f));
      return str;
    });
    this.BionicExplorerBoosterReady = this.CreateStatusItem("BionicExplorerBoosterReady", "MISC", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID);
    this.UnassignedBionicBooster = this.CreateStatusItem("UnassignedBionicBooster", "MISC", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ElectrobankLifetimeRemaining = this.CreateStatusItem("ElectrobankLifetimeRemaining", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ElectrobankLifetimeRemaining.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      SelfChargingElectrobank chargingElectrobank = (SelfChargingElectrobank) data;
      str = !((UnityEngine.Object) chargingElectrobank != (UnityEngine.Object) null) ? str.Replace("{0}", GameUtil.GetFormattedCycles(0.0f)) : str.Replace("{0}", GameUtil.GetFormattedCycles(chargingElectrobank.LifetimeRemaining));
      return str;
    });
    this.ElectrobankSelfCharging = this.CreateStatusItem("ElectrobankSelfCharging", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ElectrobankSelfCharging.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      str = str.Replace("{0}", GameUtil.GetFormattedWattage((float) data));
      return str;
    });
    this.StoredItemDurability = this.CreateStatusItem("StoredItemDurability", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.StoredItemDurability.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      global::Durability component = ((GameObject) data).GetComponent<global::Durability>();
      float percent = (UnityEngine.Object) component != (UnityEngine.Object) null ? component.GetDurability() * 100f : 100f;
      str = str.Replace("{durability}", GameUtil.GetFormattedPercent(percent));
      return str;
    });
    this.ClusterMeteorRemainingTravelTime = this.CreateStatusItem("ClusterMeteorRemainingTravelTime", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ClusterMeteorRemainingTravelTime.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      float seconds = ((ClusterMapMeteorShower.Instance) data).ArrivalTime - GameUtil.GetCurrentTimeInCycles() * 600f;
      str = str.Replace("{time}", GameUtil.GetFormattedCycles(seconds));
      return str;
    });
    this.ArtifactEntombed = this.CreateStatusItem("ArtifactEntombed", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.TearOpen = this.CreateStatusItem("TearOpen", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.TearClosed = this.CreateStatusItem("TearClosed", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ImpactorStatus = this.CreateStatusItem("LargeImpactorStatus", "MISC", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID);
    this.ImpactorStatus.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      ClusterTraveler clusterTraveler = (ClusterTraveler) data;
      float seconds = 0.0f;
      if (data != null)
        seconds = clusterTraveler.TravelETA(clusterTraveler.Destination);
      return string.Format(str, (object) GameUtil.GetFormattedCycles(seconds));
    });
    this.ImpactorStatus.resolveTooltipCallback = this.ImpactorStatus.resolveStringCallback;
    this.ImpactorHealth = this.CreateStatusItem("LargeImpactorHealth", "MISC", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID);
    this.ImpactorHealth.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      LargeImpactorStatus.Instance instance = (LargeImpactorStatus.Instance) data;
      int num1 = 0;
      int num2 = 0;
      if (data != null)
      {
        num1 = instance.Health;
        num2 = instance.def.MAX_HEALTH;
      }
      return string.Format(str, (object) num1, (object) num2);
    });
    this.LongRangeMissileTTI = this.CreateStatusItem("LongRangeMissileTTI", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.LongRangeMissileTTI.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      ClusterMapLongRangeMissile.StatesInstance smi = (ClusterMapLongRangeMissile.StatesInstance) data;
      string str3 = "";
      float seconds = 0.0f;
      if (smi != null)
      {
        GameObject go = smi.sm.targetObject.Get(smi);
        if ((UnityEngine.Object) go != (UnityEngine.Object) null)
          str3 = go.GetProperName();
        seconds = smi.InterceptETA();
      }
      return string.Format(str, (object) str3, (object) GameUtil.GetFormattedCycles(seconds));
    });
    this.LongRangeMissileTTI.resolveTooltipCallback = this.LongRangeMissileTTI.resolveStringCallback;
    this.MarkedForMove = this.CreateStatusItem("MarkedForMove", "MISC", "status_item_manually_controlled", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.MoveStorageUnreachable = this.CreateStatusItem("MoveStorageUnreachable", "MISC", "status_item_manually_controlled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.ClusterMapHarvestableResource = this.CreateStatusItem("ClusterMapHarvestableResource", "MISC", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ClusterMapHarvestableResource.showInHoverCardOnly = true;
    this.ClusterMapHarvestableResource.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      List<StarmapHexCellInventory.SerializedItem> serializedItemList = data as List<StarmapHexCellInventory.SerializedItem>;
      string str4 = "";
      for (int index = 0; index < serializedItemList.Count; ++index)
      {
        StarmapHexCellInventory.SerializedItem serializedItem = serializedItemList[index];
        str4 = $"{str4}{serializedItem.ID.ProperName()}: {(serializedItem.IsEntity ? GameUtil.GetFormattedUnits(serializedItem.Mass) : GameUtil.GetFormattedMass(serializedItem.Mass))}";
        if (index < serializedItemList.Count - 1)
          str4 += "\n";
      }
      return GameUtil.SafeStringFormat(str, (object) str4);
    });
    this.ClusterMapHarvestableResource.resolveTooltipCallback = this.ClusterMapHarvestableResource.resolveStringCallback;
    this.MinnowPOIDehydratedStatus = this.CreateStatusItem("MinnowPOIDehydratedStatus", "MISC", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
  }
}
