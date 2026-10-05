// Decompiled with JetBrains decompiler
// Type: Database.CreatureStatusItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#nullable disable
namespace Database;

public class CreatureStatusItems : StatusItems
{
  public StatusItem Dead;
  public StatusItem HealthStatus;
  public StatusItem Hot;
  public StatusItem Hot_Crop;
  public StatusItem Scalding;
  public StatusItem Scolding;
  public StatusItem Cold;
  public StatusItem Cold_Crop;
  public StatusItem Crop_Too_Dark;
  public StatusItem Crop_Too_Bright;
  public StatusItem Crop_Blighted;
  public StatusItem Hypothermia;
  public StatusItem Hyperthermia;
  public StatusItem Suffocating;
  public StatusItem AquaticCreatureSuffocating;
  public StatusItem Hatching;
  public StatusItem Incubating;
  public StatusItem Drowning;
  public StatusItem Saturated;
  public StatusItem DryingOut;
  public StatusItem Growing;
  public StatusItem GrowingFruit;
  public StatusItem GermDiet;
  public StatusItem CarnivorousPlantAwaitingVictim;
  public StatusItem ReadyForHarvest;
  public StatusItem ReadyForHarvest_Branch;
  public StatusItem EnvironmentTooWarm;
  public StatusItem EnvironmentTooCold;
  public StatusItem NotSubmerged;
  public StatusItem Entombed;
  public StatusItem Wilting;
  public StatusItem WiltingDomestic;
  public StatusItem WiltingNonGrowing;
  public StatusItem WiltingNonGrowingDomestic;
  public StatusItem WrongAtmosphere;
  public StatusItem AtmosphericPressureTooLow;
  public StatusItem AtmosphericPressureTooHigh;
  public StatusItem Barren;
  public StatusItem NeedsFertilizer;
  public StatusItem NeedsIrrigation;
  public StatusItem WrongTemperature;
  public StatusItem WrongFertilizer;
  public StatusItem WrongIrrigation;
  public StatusItem WrongFertilizerMajor;
  public StatusItem WrongIrrigationMajor;
  public StatusItem CantAcceptFertilizer;
  public StatusItem CantAcceptIrrigation;
  public StatusItem Rotting;
  public StatusItem Fresh;
  public StatusItem Stale;
  public StatusItem Spoiled;
  public StatusItem Refrigerated;
  public StatusItem RefrigeratedFrozen;
  public StatusItem Unrefrigerated;
  public StatusItem SterilizingAtmosphere;
  public StatusItem ContaminatedAtmosphere;
  public StatusItem Old;
  public StatusItem ExchangingElementOutput;
  public StatusItem ExchangingElementConsume;
  public StatusItem Hungry;
  public StatusItem HiveHungry;
  public StatusItem NoSleepSpot;
  public StatusItem ProducingSugarWater;
  public StatusItem SugarWaterProductionPaused;
  public StatusItem SugarWaterProductionWilted;
  public StatusItem SpaceTreeBranchLightStatus;
  public StatusItem OriginalPlantMutation;
  public StatusItem UnknownMutation;
  public StatusItem SpecificPlantMutation;
  public StatusItem Crop_Too_NonRadiated;
  public StatusItem Crop_Too_Radiated;
  public StatusItem ElementGrowthGrowing;
  public StatusItem ElementGrowthStunted;
  public StatusItem ElementGrowthHalted;
  public StatusItem ElementGrowthComplete;
  public StatusItem LookingForFood;
  public StatusItem LookingForGas;
  public StatusItem LookingForLiquid;
  public StatusItem Beckoning;
  public StatusItem BeckoningBlocked;
  public StatusItem MilkProducer;
  public StatusItem MilkFull;
  public StatusItem GettingRanched;
  public StatusItem GettingMilked;
  public StatusItem TemperatureHotUncomfortable;
  public StatusItem TemperatureHotDeadly;
  public StatusItem TemperatureColdUncomfortable;
  public StatusItem TemperatureColdDeadly;
  public StatusItem TravelingToPollinate;
  public StatusItem Pollinating;
  public StatusItem BubbleGasProduction;
  public StatusItem NotPollinated;
  public StatusItem Desiccation;
  public StatusItem FishFullMilk;
  public StatusItem InkFull;
  public StatusItem PunchClamApproach;
  public StatusItem PunchClamAttack;

  public CreatureStatusItems(ResourceSet parent)
    : base(nameof (CreatureStatusItems), parent)
  {
    this.CreateStatusItems();
  }

  private void CreateStatusItems()
  {
    this.Dead = new StatusItem("Dead", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.Hot = new StatusItem("Hot", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.Hot.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      TemperatureVulnerable temperatureVulnerable = (TemperatureVulnerable) data;
      return string.Format(str, (object) GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningLow), (object) GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningHigh));
    });
    this.Hot_Crop = new StatusItem("Hot_Crop", "CREATURES", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.Hot_Crop.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      TemperatureVulnerable temperatureVulnerable = (TemperatureVulnerable) data;
      str = str.Replace("{low_temperature}", GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningLow));
      str = str.Replace("{high_temperature}", GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningHigh));
      return str;
    });
    this.Scalding = new StatusItem("Scalding", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.DuplicantThreatening, true, OverlayModes.None.ID);
    this.Scalding.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      float externalTemperature = ((ScaldingMonitor.Instance) data).AverageExternalTemperature;
      float scaldingThreshold = ((ScaldingMonitor.Instance) data).GetScaldingThreshold();
      str = str.Replace("{ExternalTemperature}", GameUtil.GetFormattedTemperature(externalTemperature));
      str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(scaldingThreshold));
      return str;
    });
    this.Scalding.AddNotification();
    this.Scolding = new StatusItem("Scolding", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.DuplicantThreatening, true, OverlayModes.None.ID);
    this.Scolding.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      float externalTemperature = ((ScaldingMonitor.Instance) data).AverageExternalTemperature;
      float scoldingThreshold = ((ScaldingMonitor.Instance) data).GetScoldingThreshold();
      str = str.Replace("{ExternalTemperature}", GameUtil.GetFormattedTemperature(externalTemperature));
      str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(scoldingThreshold));
      return str;
    });
    this.Scolding.AddNotification();
    this.Cold = new StatusItem("Cold", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.Cold.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      TemperatureVulnerable temperatureVulnerable = (TemperatureVulnerable) data;
      return string.Format(str, (object) GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningLow), (object) GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningHigh));
    });
    this.Cold_Crop = new StatusItem("Cold_Crop", "CREATURES", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.Cold_Crop.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      TemperatureVulnerable temperatureVulnerable = (TemperatureVulnerable) data;
      str = str.Replace("low_temperature", GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningLow));
      str = str.Replace("high_temperature", GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningHigh));
      return str;
    });
    this.BubbleGasProduction = new StatusItem("BubbleGasProduction", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false);
    this.BubbleGasProduction.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      Tuple<SimHashes, float> tuple = (Tuple<SimHashes, float>) data;
      Element elementByHash = ElementLoader.FindElementByHash(tuple.first);
      str = str.Replace("{ELEMENT}", elementByHash.name);
      str = str.Replace("{RATE}", GameUtil.GetFormattedMass(tuple.second, GameUtil.TimeSlice.PerSecond));
      return str;
    });
    this.Crop_Too_Dark = new StatusItem("Crop_Too_Dark", "CREATURES", "status_item_plant_light", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.Crop_Too_Bright = new StatusItem("Crop_Too_Bright", "CREATURES", "status_item_plant_light", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.Crop_Blighted = new StatusItem("Crop_Blighted", "CREATURES", "status_item_plant_blighted", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.Hyperthermia = new StatusItem("Hyperthermia", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID);
    this.Hyperthermia.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      float temp = ((TemperatureMonitor.Instance) data).temperature.value;
      float hyperthermiaThreshold = ((TemperatureMonitor.Instance) data).HyperthermiaThreshold;
      str = str.Replace("{InternalTemperature}", GameUtil.GetFormattedTemperature(temp));
      str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(hyperthermiaThreshold));
      return str;
    });
    this.Hypothermia = new StatusItem("Hypothermia", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID);
    this.Hypothermia.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      float temp = ((TemperatureMonitor.Instance) data).temperature.value;
      float hypothermiaThreshold = ((TemperatureMonitor.Instance) data).HypothermiaThreshold;
      str = str.Replace("{InternalTemperature}", GameUtil.GetFormattedTemperature(temp));
      str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(hypothermiaThreshold));
      return str;
    });
    this.Suffocating = new StatusItem("Suffocating", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Hatching = new StatusItem("Hatching", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Incubating = new StatusItem("Incubating", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Drowning = new StatusItem("Drowning", "CREATURES", "status_item_flooded", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.Drowning.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
    this.AquaticCreatureSuffocating = new StatusItem("AquaticCreatureSuffocating", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Bad, false, OverlayModes.None.ID);
    this.AquaticCreatureSuffocating.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      AquaticCreatureSuffocationMonitor.Instance instance = (AquaticCreatureSuffocationMonitor.Instance) data;
      str = GameUtil.SafeStringFormat(str, (object) GameUtil.GetFormattedCycles(instance.TimeUntilDeath));
      return str;
    });
    this.ProducingSugarWater = new StatusItem("ProducingSugarWater", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID);
    this.ProducingSugarWater.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      SpaceTreePlant.Instance instance = (SpaceTreePlant.Instance) data;
      str = str.Replace("{0}", GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f));
      return str;
    });
    this.ProducingSugarWater.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      SpaceTreePlant.Instance smi1 = (SpaceTreePlant.Instance) data;
      PlantBranchGrower.Instance smi2 = smi1.GetSMI<PlantBranchGrower.Instance>();
      for (int idx = 0; idx < smi1.def.OptimalAmountOfBranches; ++idx)
      {
        string str1 = (string) CREATURES.STATUSITEMS.PRODUCINGSUGARWATER.BRANCH_LINE_MISSING;
        string newValue1 = SpaceTreeBranchConfig.BRANCH_NAMES[idx];
        GameObject branch = smi2.GetBranch(idx);
        if ((UnityEngine.Object) branch != (UnityEngine.Object) null)
        {
          SpaceTreeBranch.Instance smi3 = branch.GetSMI<SpaceTreeBranch.Instance>();
          if (smi3 != null && !smi3.isMasterNull)
          {
            if (smi3.IsBranchFullyGrown)
            {
              string formattedPercent = GameUtil.GetFormattedPercent(smi3.Productivity * 100f);
              str1 = ((string) CREATURES.STATUSITEMS.PRODUCINGSUGARWATER.BRANCH_LINE).Replace("{1}", formattedPercent);
            }
            else
            {
              string formattedPercent = GameUtil.GetFormattedPercent(smi3.GetcurrentGrowthPercentage() * 100f);
              str1 = ((string) CREATURES.STATUSITEMS.PRODUCINGSUGARWATER.BRANCH_LINE_GROWING).Replace("{1}", formattedPercent);
            }
          }
        }
        string newValue2 = str1.Replace("{0}", newValue1);
        string oldValue = $"{{BRANCH_{idx.ToString()}}}";
        str = str.Replace(oldValue, newValue2);
      }
      str = str.Replace("{0}", GameUtil.GetFormattedMass(smi1.GetProductionSpeed() * 20f / smi1.OptimalProductionDuration, GameUtil.TimeSlice.PerSecond));
      str = str.Replace("{1}", smi1.def.OptimalAmountOfBranches.ToString());
      str = str.Replace("{2}", GameUtil.GetFormattedLux(10000));
      return str;
    });
    this.SugarWaterProductionPaused = new StatusItem("SugarWaterProductionPaused", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.SugarWaterProductionPaused.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      SpaceTreePlant.Instance instance = (SpaceTreePlant.Instance) data;
      str = str.Replace("{0}", GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f));
      return str;
    });
    this.SugarWaterProductionPaused.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      SpaceTreePlant.Instance smi4 = (SpaceTreePlant.Instance) data;
      PlantBranchGrower.Instance smi5 = smi4.GetSMI<PlantBranchGrower.Instance>();
      for (int idx = 0; idx < smi4.def.OptimalAmountOfBranches; ++idx)
      {
        string str2 = (string) CREATURES.STATUSITEMS.SUGARWATERPRODUCTIONPAUSED.BRANCH_LINE_MISSING;
        string newValue3 = SpaceTreeBranchConfig.BRANCH_NAMES[idx];
        GameObject branch = smi5.GetBranch(idx);
        if ((UnityEngine.Object) branch != (UnityEngine.Object) null)
        {
          SpaceTreeBranch.Instance smi6 = branch.GetSMI<SpaceTreeBranch.Instance>();
          if (smi6 != null && !smi6.isMasterNull)
          {
            if (smi6.IsBranchFullyGrown)
            {
              string formattedPercent = GameUtil.GetFormattedPercent(smi6.Productivity * 100f);
              str2 = ((string) CREATURES.STATUSITEMS.SUGARWATERPRODUCTIONPAUSED.BRANCH_LINE).Replace("{1}", formattedPercent);
            }
            else
            {
              string formattedPercent = GameUtil.GetFormattedPercent(smi6.GetcurrentGrowthPercentage() * 100f);
              str2 = ((string) CREATURES.STATUSITEMS.SUGARWATERPRODUCTIONPAUSED.BRANCH_LINE_GROWING).Replace("{1}", formattedPercent);
            }
          }
        }
        string newValue4 = str2.Replace("{0}", newValue3);
        string oldValue = $"{{BRANCH_{idx.ToString()}}}";
        str = str.Replace(oldValue, newValue4);
      }
      str = str.Replace("{0}", smi4.def.OptimalAmountOfBranches.ToString());
      str = str.Replace("{1}", GameUtil.GetFormattedLux(10000));
      return str;
    });
    this.SugarWaterProductionWilted = new StatusItem("SugarWaterProductionWilted", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.SugarWaterProductionWilted.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      SpaceTreePlant.Instance instance = (SpaceTreePlant.Instance) data;
      str = str.Replace("{0}", GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f));
      return str;
    });
    this.SpaceTreeBranchLightStatus = new StatusItem("SpaceTreeBranchLightStatus", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID);
    this.SpaceTreeBranchLightStatus.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      SpaceTreeBranch.Instance instance = (SpaceTreeBranch.Instance) data;
      str = str.Replace("{0}", GameUtil.GetFormattedPercent(instance.Productivity * 100f));
      return str;
    });
    this.SpaceTreeBranchLightStatus.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      SpaceTreeBranch.Instance instance = (SpaceTreeBranch.Instance) data;
      str = str.Replace("{0}", GameUtil.GetFormattedLux(instance.def.OPTIMAL_LUX_LEVELS));
      str = str.Replace("{1}", GameUtil.GetFormattedLux(instance.CurrentAmountOfLux));
      return str;
    });
    this.Saturated = new StatusItem("Saturated", "CREATURES", "status_item_flooded", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.Saturated.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
    this.DryingOut = new StatusItem("DryingOut", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, status_overlays: 1026);
    this.DryingOut.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
    this.ReadyForHarvest = new StatusItem("ReadyForHarvest", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, status_overlays: 1026);
    this.ReadyForHarvest_Branch = new StatusItem("ReadyForHarvest_Branch", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, status_overlays: 1026);
    this.Growing = new StatusItem("Growing", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, status_overlays: 1026);
    this.Growing.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      IManageGrowingStates manageGrowingStates = (IManageGrowingStates) data;
      if ((UnityEngine.Object) manageGrowingStates.GetCropComponent() != (UnityEngine.Object) null)
      {
        float seconds = manageGrowingStates.TimeUntilNextHarvest();
        str = str.Replace("{TimeUntilNextHarvest}", GameUtil.GetFormattedCycles(seconds));
      }
      float val1 = 100f * manageGrowingStates.PercentGrown();
      str = str.Replace("{PercentGrow}", Math.Floor((double) Math.Max(val1, 0.0f)).ToString("F0"));
      return str;
    });
    this.GrowingFruit = new StatusItem("GrowingFruit", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, status_overlays: 1026);
    this.GrowingFruit.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      IManageGrowingStates manageGrowingStates = (IManageGrowingStates) data;
      if ((UnityEngine.Object) manageGrowingStates.GetCropComponent() != (UnityEngine.Object) null)
      {
        float seconds = manageGrowingStates.TimeUntilNextHarvest();
        str = str.Replace("{TimeUntilNextHarvest}", GameUtil.GetFormattedCycles(seconds));
      }
      float val1 = 100f * manageGrowingStates.PercentGrown();
      str = str.Replace("{PercentGrow}", Math.Floor((double) Math.Max(val1, 0.0f)).ToString("F0"));
      return str;
    });
    this.CarnivorousPlantAwaitingVictim = new StatusItem("CarnivorousPlantAwaitingVictim", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, status_overlays: 1026);
    this.CarnivorousPlantAwaitingVictim.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      string[] possiblePreyList = ((IPlantConsumeEntities) data).GetFormattedPossiblePreyList();
      string str3 = "";
      for (int index = 0; index < possiblePreyList.Length; ++index)
        str3 = $"{str3}\n{GameUtil.SafeStringFormat((string) CREATURES.STATUSITEMS.CARNIVOROUSPLANTAWAITINGVICTIM.TOOLTIP_ITEM, (object) possiblePreyList[index])}";
      str += str3;
      return str;
    });
    this.EnvironmentTooWarm = new StatusItem("EnvironmentTooWarm", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.EnvironmentTooWarm.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      float temp1 = Grid.Temperature[Grid.PosToCell(((Component) data).gameObject)];
      float temp2 = ((TemperatureVulnerable) data).TemperatureLethalHigh - 1f;
      str = str.Replace("{ExternalTemperature}", GameUtil.GetFormattedTemperature(temp1));
      str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(temp2));
      return str;
    });
    this.EnvironmentTooCold = new StatusItem("EnvironmentTooCold", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.EnvironmentTooCold.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      float temp3 = Grid.Temperature[Grid.PosToCell(((Component) data).gameObject)];
      float temp4 = ((TemperatureVulnerable) data).TemperatureLethalLow + 1f;
      str = str.Replace("{ExternalTemperature}", GameUtil.GetFormattedTemperature(temp3));
      str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(temp4));
      return str;
    });
    this.Entombed = new StatusItem("Entombed", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.Entombed.resolveStringCallback = (Func<string, object, string>) ((str, go) => str);
    this.Entombed.resolveTooltipCallback = (Func<string, object, string>) ((str, go) =>
    {
      GameObject go1 = go as GameObject;
      return string.Format(str, (object) GameUtil.GetIdentityDescriptor(go1));
    });
    this.NotSubmerged = new StatusItem("NotSubmerged", "CREATURES", "status_item_flooded", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.Wilting = new StatusItem("Wilting", "CREATURES", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 1026);
    this.Wilting.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      global::Growing growing = data as global::Growing;
      if ((UnityEngine.Object) growing != (UnityEngine.Object) null && data != null)
      {
        AmountInstance amountInstance = growing.gameObject.GetAmounts().Get(Db.Get().Amounts.Maturity);
        str = str.Replace("{TimeUntilNextHarvest}", GameUtil.GetFormattedCycles(Mathf.Min(amountInstance.GetMax(), growing.TimeUntilNextHarvest())));
      }
      str = str.Replace("{Reasons}", (data as KMonoBehaviour).GetComponent<WiltCondition>().WiltCausesString());
      return str;
    });
    this.WiltingDomestic = new StatusItem("WiltingDomestic", "CREATURES", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, status_overlays: 1026);
    this.WiltingDomestic.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      global::Growing growing = data as global::Growing;
      if ((UnityEngine.Object) growing != (UnityEngine.Object) null && data != null)
      {
        AmountInstance amountInstance = growing.gameObject.GetAmounts().Get(Db.Get().Amounts.Maturity);
        str = str.Replace("{TimeUntilNextHarvest}", GameUtil.GetFormattedCycles(Mathf.Min(amountInstance.GetMax(), growing.TimeUntilNextHarvest())));
      }
      str = str.Replace("{Reasons}", (data as KMonoBehaviour).GetComponent<WiltCondition>().WiltCausesString());
      return str;
    });
    this.WiltingNonGrowing = new StatusItem("WiltingNonGrowing", "CREATURES", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 1026);
    this.WiltingNonGrowing.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      str = (string) CREATURES.STATUSITEMS.WILTING_NON_GROWING_PLANT.NAME;
      str = str.Replace("{Reasons}", (data as WiltCondition).WiltCausesString());
      return str;
    });
    this.WiltingNonGrowingDomestic = new StatusItem("WiltingNonGrowing", "CREATURES", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, status_overlays: 1026);
    this.WiltingNonGrowingDomestic.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      str = (string) CREATURES.STATUSITEMS.WILTING_NON_GROWING_PLANT.NAME;
      str = str.Replace("{Reasons}", (data as WiltCondition).WiltCausesString());
      return str;
    });
    this.WrongAtmosphere = new StatusItem("WrongAtmosphere", "CREATURES", "status_item_plant_atmosphere", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.WrongAtmosphere.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      string newValue = "";
      foreach (Element safeAtmosphere in (data as PressureVulnerable).safe_atmospheres)
        newValue = $"{newValue}\n    •  {safeAtmosphere.name}";
      str = str.Replace("{elements}", newValue);
      return str;
    });
    this.AtmosphericPressureTooLow = new StatusItem("AtmosphericPressureTooLow", "CREATURES", "status_item_plant_atmosphere", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.AtmosphericPressureTooLow.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      PressureVulnerable pressureVulnerable = (PressureVulnerable) data;
      str = str.Replace("{low_mass}", GameUtil.GetFormattedMass(pressureVulnerable.pressureWarning_Low));
      str = str.Replace("{high_mass}", GameUtil.GetFormattedMass(pressureVulnerable.pressureWarning_High));
      return str;
    });
    this.AtmosphericPressureTooHigh = new StatusItem("AtmosphericPressureTooHigh", "CREATURES", "status_item_plant_atmosphere", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.AtmosphericPressureTooHigh.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      PressureVulnerable pressureVulnerable = (PressureVulnerable) data;
      str = str.Replace("{low_mass}", GameUtil.GetFormattedMass(pressureVulnerable.pressureWarning_Low));
      str = str.Replace("{high_mass}", GameUtil.GetFormattedMass(pressureVulnerable.pressureWarning_High));
      return str;
    });
    this.HealthStatus = new StatusItem("HealthStatus", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.HealthStatus.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      string newValue = "";
      switch ((Health.HealthState) data)
      {
        case Health.HealthState.Perfect:
          newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.PERFECT.NAME;
          break;
        case Health.HealthState.Scuffed:
          newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.SCUFFED.NAME;
          break;
        case Health.HealthState.Injured:
          newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.INJURED.NAME;
          break;
        case Health.HealthState.Critical:
          newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.CRITICAL.NAME;
          break;
        case Health.HealthState.Incapacitated:
          newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.INCAPACITATED.NAME;
          break;
        case Health.HealthState.Dead:
          newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.DEAD.NAME;
          break;
      }
      str = str.Replace("{healthState}", newValue);
      return str;
    });
    this.HealthStatus.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      string newValue = "";
      switch ((Health.HealthState) data)
      {
        case Health.HealthState.Perfect:
          newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.PERFECT.TOOLTIP;
          break;
        case Health.HealthState.Scuffed:
          newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.SCUFFED.TOOLTIP;
          break;
        case Health.HealthState.Injured:
          newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.INJURED.TOOLTIP;
          break;
        case Health.HealthState.Critical:
          newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.CRITICAL.TOOLTIP;
          break;
        case Health.HealthState.Incapacitated:
          newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.INCAPACITATED.TOOLTIP;
          break;
        case Health.HealthState.Dead:
          newValue = (string) MISC.STATUSITEMS.HEALTHSTATUS.DEAD.TOOLTIP;
          break;
      }
      str = str.Replace("{healthState}", newValue);
      return str;
    });
    this.Barren = new StatusItem("Barren", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.NeedsFertilizer = new StatusItem("NeedsFertilizer", "CREATURES", "status_item_plant_solid", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.NeedsFertilizer.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
    this.NeedsIrrigation = new StatusItem("NeedsIrrigation", "CREATURES", "status_item_plant_liquid", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.NeedsIrrigation.resolveStringCallback = (Func<string, object, string>) ((str, data) => str);
    this.WrongFertilizer = new StatusItem("WrongFertilizer", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    Func<string, object, string> func1 = (Func<string, object, string>) ((str, data) => str);
    this.WrongFertilizer.resolveStringCallback = func1;
    this.WrongFertilizerMajor = new StatusItem("WrongFertilizerMajor", "CREATURES", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.WrongFertilizerMajor.resolveStringCallback = func1;
    this.WrongIrrigation = new StatusItem("WrongIrrigation", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    Func<string, object, string> func2 = (Func<string, object, string>) ((str, data) => str);
    this.WrongIrrigation.resolveStringCallback = func2;
    this.WrongIrrigationMajor = new StatusItem("WrongIrrigationMajor", "CREATURES", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.WrongIrrigationMajor.resolveStringCallback = func2;
    this.CantAcceptFertilizer = new StatusItem("CantAcceptFertilizer", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.Rotting = new StatusItem("Rotting", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Rotting.resolveStringCallback = (Func<string, object, string>) ((str, data) => str.Replace("{RotTemperature}", GameUtil.GetFormattedTemperature(277.15f)));
    this.Fresh = new StatusItem("Fresh", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Fresh.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      Rottable.Instance instance = (Rottable.Instance) data;
      return str.Replace("{RotPercentage}", $"({Util.FormatWholeNumber(instance.RotConstitutionPercentage * 100f)}%)");
    });
    this.Fresh.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      Rottable.Instance instance = (Rottable.Instance) data;
      return str.Replace("{RotTooltip}", instance.GetToolTip());
    });
    this.Stale = new StatusItem("Stale", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.Stale.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      Rottable.Instance instance = (Rottable.Instance) data;
      return str.Replace("{RotPercentage}", $"({Util.FormatWholeNumber(instance.RotConstitutionPercentage * 100f)}%)");
    });
    this.Stale.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      Rottable.Instance instance = (Rottable.Instance) data;
      return str.Replace("{RotTooltip}", instance.GetToolTip());
    });
    this.Spoiled = new StatusItem("Spoiled", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    Func<string, object, string> func3 = (Func<string, object, string>) ((str, data) =>
    {
      IRottable rottable = (IRottable) data;
      return str.Replace("{RotTemperature}", GameUtil.GetFormattedTemperature(rottable.RotTemperature)).Replace("{PreserveTemperature}", GameUtil.GetFormattedTemperature(rottable.PreserveTemperature));
    });
    this.Refrigerated = new StatusItem("Refrigerated", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Refrigerated.resolveStringCallback = func3;
    this.RefrigeratedFrozen = new StatusItem("RefrigeratedFrozen", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.RefrigeratedFrozen.resolveStringCallback = func3;
    this.Unrefrigerated = new StatusItem("Unrefrigerated", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.Unrefrigerated.resolveStringCallback = func3;
    this.SterilizingAtmosphere = new StatusItem("SterilizingAtmosphere", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ContaminatedAtmosphere = new StatusItem("ContaminatedAtmosphere", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.Old = new StatusItem("Old", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.Old.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      AgeMonitor.Instance instance = (AgeMonitor.Instance) data;
      return str.Replace("{TimeUntilDeath}", GameUtil.GetFormattedCycles(instance.CyclesUntilDeath * 600f));
    });
    this.ExchangingElementConsume = new StatusItem("ExchangingElementConsume", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ExchangingElementConsume.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      EntityElementExchanger.StatesInstance statesInstance = (EntityElementExchanger.StatesInstance) data;
      str = str.Replace("{ConsumeElement}", ElementLoader.FindElementByHash(statesInstance.master.consumedElement).tag.ProperName());
      str = str.Replace("{ConsumeRate}", GameUtil.GetFormattedMass(statesInstance.master.consumeRate, GameUtil.TimeSlice.PerSecond));
      return str;
    });
    this.ExchangingElementConsume.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      EntityElementExchanger.StatesInstance statesInstance = (EntityElementExchanger.StatesInstance) data;
      str = str.Replace("{ConsumeElement}", ElementLoader.FindElementByHash(statesInstance.master.consumedElement).tag.ProperName());
      str = str.Replace("{ConsumeRate}", GameUtil.GetFormattedMass(statesInstance.master.consumeRate, GameUtil.TimeSlice.PerSecond));
      return str;
    });
    this.ExchangingElementOutput = new StatusItem("ExchangingElementOutput", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ExchangingElementOutput.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      EntityElementExchanger.StatesInstance statesInstance = (EntityElementExchanger.StatesInstance) data;
      str = str.Replace("{OutputElement}", ElementLoader.FindElementByHash(statesInstance.master.emittedElement).tag.ProperName());
      str = str.Replace("{OutputRate}", GameUtil.GetFormattedMass(statesInstance.master.consumeRate * statesInstance.master.exchangeRatio, GameUtil.TimeSlice.PerSecond));
      return str;
    });
    this.ExchangingElementOutput.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      EntityElementExchanger.StatesInstance statesInstance = (EntityElementExchanger.StatesInstance) data;
      str = str.Replace("{OutputElement}", ElementLoader.FindElementByHash(statesInstance.master.emittedElement).tag.ProperName());
      str = str.Replace("{OutputRate}", GameUtil.GetFormattedMass(statesInstance.master.consumeRate * statesInstance.master.exchangeRatio, GameUtil.TimeSlice.PerSecond));
      return str;
    });
    this.Hungry = new StatusItem("Hungry", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Hungry.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      Diet diet = ((CreatureCalorieMonitor.Instance) data).stomach.diet;
      if (diet.consumedTags.Count <= 0)
        return str;
      string[] strArray = diet.consumedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => t.Key.ProperName())).ToArray<string>();
      if (strArray.Length > 3)
        strArray = new string[4]
        {
          strArray[0],
          strArray[1],
          strArray[2],
          "..."
        };
      string newValue = string.Join(", ", strArray);
      return $"{str}\n{UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue)}";
    });
    this.HiveHungry = new StatusItem("HiveHungry", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.HiveHungry.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      Diet diet = ((BeehiveCalorieMonitor.Instance) data).stomach.diet;
      if (diet.consumedTags.Count <= 0)
        return str;
      string[] strArray = diet.consumedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => t.Key.ProperName())).ToArray<string>();
      if (strArray.Length > 3)
        strArray = new string[4]
        {
          strArray[0],
          strArray[1],
          strArray[2],
          "..."
        };
      string newValue = string.Join(", ", strArray);
      return $"{str}\n{UI.BUILDINGEFFECTS.DIET_STORED.text.Replace("{Foodlist}", newValue)}";
    });
    this.NoSleepSpot = new StatusItem("NoSleepSpot", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.OriginalPlantMutation = new StatusItem("OriginalPlantMutation", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.UnknownMutation = new StatusItem("UnknownMutation", "CREATURES", "status_item_unknown_mutation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.SpecificPlantMutation = new StatusItem("SpecificPlantMutation", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.SpecificPlantMutation.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      PlantMutation plantMutation = (PlantMutation) data;
      return str.Replace("{MutationName}", plantMutation.Name);
    });
    this.SpecificPlantMutation.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      PlantMutation plantMutation = (PlantMutation) data;
      str = str.Replace("{MutationName}", plantMutation.Name);
      return $"{str}\n{plantMutation.GetTooltip()}";
    });
    this.Crop_Too_NonRadiated = new StatusItem("Crop_Too_NonRadiated", "CREATURES", "status_item_plant_light", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.Crop_Too_Radiated = new StatusItem("Crop_Too_Radiated", "CREATURES", "status_item_plant_light", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false);
    this.ElementGrowthGrowing = new StatusItem("Element_Growth_Growing", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.ElementGrowthGrowing.resolveTooltipCallback = (Func<string, object, string>) ((str, data) =>
    {
      ElementGrowthMonitor.Instance instance = (ElementGrowthMonitor.Instance) data;
      StringBuilder stringBuilder = new StringBuilder(str, str.Length * 2);
      stringBuilder.Replace("{templo}", GameUtil.GetFormattedTemperature(instance.def.minTemperature));
      stringBuilder.Replace("{temphi}", GameUtil.GetFormattedTemperature(instance.def.maxTemperature));
      if ((double) instance.lastConsumedTemperature > 0.0)
      {
        stringBuilder.Append("\n\n");
        stringBuilder.Append((string) CREATURES.STATUSITEMS.ELEMENT_GROWTH_GROWING.PREFERRED_TEMP);
        stringBuilder.Replace("{element}", ElementLoader.FindElementByHash(instance.lastConsumedElement).name);
        stringBuilder.Replace("{temperature}", GameUtil.GetFormattedTemperature(instance.lastConsumedTemperature));
      }
      return stringBuilder.ToString();
    });
    this.ElementGrowthStunted = new StatusItem("Element_Growth_Stunted", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Bad, false, OverlayModes.None.ID);
    this.ElementGrowthStunted.resolveTooltipCallback = this.ElementGrowthGrowing.resolveTooltipCallback;
    this.ElementGrowthStunted.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      ElementGrowthMonitor.Instance instance = (ElementGrowthMonitor.Instance) data;
      string newValue = (string) ((double) instance.lastConsumedTemperature < (double) instance.def.minTemperature ? CREATURES.STATUSITEMS.ELEMENT_GROWTH_STUNTED.TOO_COLD : CREATURES.STATUSITEMS.ELEMENT_GROWTH_STUNTED.TOO_HOT);
      str = str.Replace("{reason}", newValue);
      return str;
    });
    this.ElementGrowthHalted = new StatusItem("Element_Growth_Halted", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Bad, false, OverlayModes.None.ID);
    this.ElementGrowthHalted.resolveTooltipCallback = this.ElementGrowthGrowing.resolveTooltipCallback;
    this.ElementGrowthComplete = new StatusItem("Element_Growth_Complete", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Bad, false, OverlayModes.None.ID);
    this.ElementGrowthComplete.resolveTooltipCallback = this.ElementGrowthGrowing.resolveTooltipCallback;
    this.LookingForFood = new StatusItem("Hungry", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.LookingForGas = new StatusItem("LookingForGas", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.LookingForLiquid = new StatusItem("LookingForLiquid", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Beckoning = new StatusItem("Beckoning", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.BeckoningBlocked = new StatusItem("BeckoningBlocked", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Bad, false, OverlayModes.None.ID);
    this.MilkProducer = new StatusItem("MilkProducer", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.MilkProducer.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      MilkProductionMonitor.Instance instance = (MilkProductionMonitor.Instance) data;
      str = str.Replace("{amount}", GameUtil.GetFormattedMass(instance.MilkPercentage));
      return str;
    });
    this.GettingRanched = new StatusItem("Getting_Ranched", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.GettingMilked = new StatusItem("Getting_Milked", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.MilkFull = new StatusItem("MilkFull", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.TemperatureHotUncomfortable = new StatusItem("TemperatureHotUncomfortable", (string) CREATURES.STATUSITEMS.TEMPERATURE_HOT_UNCOMFORTABLE.NAME, (string) CREATURES.STATUSITEMS.TEMPERATURE_HOT_UNCOMFORTABLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.TemperatureHotDeadly = new StatusItem("TemperatureHotDeadly", (string) CREATURES.STATUSITEMS.TEMPERATURE_HOT_DEADLY.NAME, (string) CREATURES.STATUSITEMS.TEMPERATURE_HOT_DEADLY.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID);
    this.TemperatureColdUncomfortable = new StatusItem("TemperatureColdUncomfortable", (string) CREATURES.STATUSITEMS.TEMPERATURE_COLD_UNCOMFORTABLE.NAME, (string) CREATURES.STATUSITEMS.TEMPERATURE_COLD_UNCOMFORTABLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    this.TemperatureColdDeadly = new StatusItem("TemperatureColdDeadly", (string) CREATURES.STATUSITEMS.TEMPERATURE_COLD_DEADLY.NAME, (string) CREATURES.STATUSITEMS.TEMPERATURE_COLD_DEADLY.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID);
    this.TemperatureHotUncomfortable.resolveStringCallback = (Func<string, object, string>) ((str, obj) =>
    {
      CritterTemperatureMonitor.Instance instance = (CritterTemperatureMonitor.Instance) obj;
      return string.Format(str, (object) GameUtil.GetFormattedTemperature(instance.GetTemperatureInternal()), (object) GameUtil.GetFormattedTemperature(instance.def.temperatureColdUncomfortable), (object) GameUtil.GetFormattedTemperature(instance.def.temperatureHotUncomfortable), (object) Effect.CreateTooltip(instance.sm.uncomfortableEffect, false));
    });
    this.TemperatureHotDeadly.resolveStringCallback = (Func<string, object, string>) ((str, obj) =>
    {
      CritterTemperatureMonitor.Instance instance = (CritterTemperatureMonitor.Instance) obj;
      return string.Format(str, (object) GameUtil.GetFormattedTemperature(instance.GetTemperatureExternal()), (object) GameUtil.GetFormattedTemperature(instance.def.temperatureColdDeadly), (object) GameUtil.GetFormattedTemperature(instance.def.temperatureHotDeadly), (object) Effect.CreateTooltip(instance.sm.deadlyEffect, false));
    });
    this.TemperatureColdUncomfortable.resolveStringCallback = this.TemperatureHotUncomfortable.resolveStringCallback;
    this.TemperatureColdDeadly.resolveStringCallback = this.TemperatureHotDeadly.resolveStringCallback;
    this.TravelingToPollinate = new StatusItem("POLLINATING.MOVINGTO", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Pollinating = new StatusItem("POLLINATING.INTERACTING", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.NotPollinated = new StatusItem("NOT_POLLINATED", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.Desiccation = new StatusItem("Desiccation", "CREATURES", string.Empty, StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID);
    this.Desiccation.SetResolveStringCallback((Func<string, object, string>) ((str, data) => !(data is DesiccationMonitor.Instance instance1) ? str : string.Format(str, (object) GameUtil.GetFormattedTime(instance1.GetEstimatedTimeUntilDeath()))));
    this.FishFullMilk = new StatusItem("FishMilkFull", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.InkFull = new StatusItem("InkFull", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.PunchClamApproach = new StatusItem("PUNCH_CLAM_APPROACH", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.PunchClamAttack = new StatusItem("PUNCH_CLAM_ATTACK", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
  }
}
