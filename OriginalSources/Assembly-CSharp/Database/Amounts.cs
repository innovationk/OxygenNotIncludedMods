// Decompiled with JetBrains decompiler
// Type: Database.Amounts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
namespace Database;

public class Amounts : ResourceSet<Amount>
{
  public Amount Stamina;
  public Amount Calories;
  public Amount ImmuneLevel;
  public Amount Breath;
  public Amount Stress;
  public Amount Toxicity;
  public Amount Bladder;
  public Amount Decor;
  public Amount RadiationBalance;
  public Amount BionicOxygenTank;
  public Amount BionicOil;
  public Amount BionicGunk;
  public Amount BionicInternalBattery;
  public Amount Temperature;
  public Amount CritterTemperature;
  public Amount HitPoints;
  public Amount AirPressure;
  public Amount Maturity;
  public Amount Maturity2;
  public Amount OldAge;
  public Amount Age;
  public Amount Fertilization;
  public Amount Illumination;
  public Amount Irrigation;
  public Amount Fertility;
  public Amount Viability;
  public Amount PowerCharge;
  public Amount Wildness;
  public Amount Incubation;
  public Amount ScaleGrowth;
  public Amount ElementGrowth;
  public Amount Beckoning;
  public Amount MilkProduction;
  public Amount Moisture;
  public Amount Mucus;
  public Amount InternalBattery;
  public Amount InternalChemicalBattery;
  public Amount InternalBioBattery;
  public Amount InternalElectroBank;
  public Amount Rot;

  public void Load()
  {
    this.Stamina = this.CreateAmount("Stamina", 0.0f, 100f, false, Units.Flat, 0.35f, true, "STRINGS.DUPLICANTS", "ui_icon_stamina", "attribute_stamina", "mod_stamina");
    this.Stamina.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle));
    this.Calories = this.CreateAmount("Calories", 0.0f, 0.0f, false, Units.Flat, 4000f, true, "STRINGS.DUPLICANTS", "ui_icon_calories", "attribute_calories", "mod_calories");
    this.Calories.SetDisplayer((IAmountDisplayer) new CaloriesDisplayer());
    this.Breath = this.CreateAmount("Breath", 0.0f, 100f, true, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_breath", uiFullColourSprite: "mod_breath");
    StandardAmountDisplayer displayer = new StandardAmountDisplayer(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.PerSecond);
    displayer.SetDeltaFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerSecond));
    this.Breath.SetDisplayer((IAmountDisplayer) displayer);
    this.Breath.maxAttribute.SetFormatter((IAttributeFormatter) new StandardAttributeFormatter(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None));
    this.Stress = this.CreateAmount("Stress", 0.0f, 100f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_stress", "attribute_stress", "mod_stress");
    this.Stress.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
    this.Toxicity = this.CreateAmount("Toxicity", 0.0f, 100f, true, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS");
    this.Toxicity.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle));
    this.Bladder = this.CreateAmount("Bladder", 0.0f, 100f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_bladder", uiFullColourSprite: "mod_bladder");
    this.Bladder.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle));
    this.Decor = this.CreateAmount("Decor", -1000f, 1000f, false, Units.Flat, 0.0166666675f, true, "STRINGS.DUPLICANTS", "ui_icon_decor", uiFullColourSprite: "mod_decor");
    this.Decor.SetDisplayer((IAmountDisplayer) new DecorDisplayer());
    this.RadiationBalance = this.CreateAmount("RadiationBalance", 0.0f, 10000f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_radiation", uiFullColourSprite: "mod_health");
    this.RadiationBalance.SetDisplayer((IAmountDisplayer) new RadiationBalanceDisplayer());
    this.Temperature = this.CreateAmount("Temperature", 0.0f, 10000f, false, Units.Kelvin, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_temperature");
    this.Temperature.SetDisplayer((IAmountDisplayer) new DuplicantTemperatureDeltaAsEnergyAmountDisplayer(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.PerSecond));
    this.CritterTemperature = this.CreateAmount("CritterTemperature", 0.0f, 10000f, false, Units.Kelvin, 0.5f, true, "STRINGS.CREATURES", "ui_icon_temperature");
    this.CritterTemperature.SetDisplayer((IAmountDisplayer) new CritterTemperatureDeltaAsEnergyAmountDisplayer(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.PerSecond));
    this.HitPoints = this.CreateAmount("HitPoints", 0.0f, 0.0f, true, Units.Flat, 0.1675f, true, "STRINGS.DUPLICANTS", "ui_icon_hitpoints", "attribute_hitpoints", "mod_health");
    this.HitPoints.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.PerCycle, tense: GameUtil.IdentityDescriptorTense.Possessive));
    this.AirPressure = this.CreateAmount("AirPressure", 0.0f, 1E+09f, false, Units.Flat, 0.0f, true, "STRINGS.CREATURES");
    this.AirPressure.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerSecond));
    this.Maturity = this.CreateAmount("Maturity", 0.0f, 0.0f, true, Units.Flat, 0.0009166667f, true, "STRINGS.CREATURES", "ui_icon_maturity");
    this.Maturity.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Cycles, GameUtil.TimeSlice.None));
    this.Maturity2 = this.CreateAmount("Maturity2", 0.0f, 0.0f, true, Units.Flat, 0.0009166667f, true, false, "STRINGS.CREATURES", "ui_icon_maturity");
    this.Maturity2.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Cycles, GameUtil.TimeSlice.None));
    this.OldAge = this.CreateAmount("OldAge", 0.0f, 0.0f, false, Units.Flat, 0.0f, false, "STRINGS.CREATURES");
    this.Fertilization = this.CreateAmount("Fertilization", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES");
    this.Fertilization.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerSecond));
    this.Fertility = this.CreateAmount("Fertility", 0.0f, 100f, true, Units.Flat, 0.008375f, true, "STRINGS.CREATURES", "ui_icon_fertility");
    this.Fertility.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
    this.Wildness = this.CreateAmount("Wildness", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_wildness");
    this.Wildness.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
    this.Incubation = this.CreateAmount("Incubation", 0.0f, 100f, true, Units.Flat, 0.01675f, true, "STRINGS.CREATURES", "ui_icon_incubation");
    this.Incubation.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
    this.Viability = this.CreateAmount("Viability", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_viability");
    this.Viability.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
    this.PowerCharge = this.CreateAmount("PowerCharge", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES");
    this.PowerCharge.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
    this.Age = this.CreateAmount("Age", 0.0f, 0.0f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_age");
    this.Age.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.PerCycle));
    this.Irrigation = this.CreateAmount("Irrigation", 0.0f, 1f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES");
    this.Irrigation.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerSecond));
    this.ImmuneLevel = this.CreateAmount("ImmuneLevel", 0.0f, DUPLICANTSTATS.STANDARD.BaseStats.IMMUNE_LEVEL_MAX, true, Units.Flat, 0.1675f, true, "STRINGS.DUPLICANTS", "ui_icon_immunelevel", "attribute_immunelevel");
    this.ImmuneLevel.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
    this.Rot = this.CreateAmount("Rot", 0.0f, 0.0f, false, Units.Flat, 0.0f, true, "STRINGS.CREATURES");
    this.Rot.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
    this.Illumination = this.CreateAmount("Illumination", 0.0f, 1f, false, Units.Flat, 0.0f, true, "STRINGS.CREATURES");
    this.Illumination.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None));
    this.ScaleGrowth = this.CreateAmount("ScaleGrowth", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_scale_growth");
    this.ScaleGrowth.SetDisplayer((IAmountDisplayer) new ScaleGrowthDisplayer(GameUtil.TimeSlice.PerCycle));
    this.MilkProduction = this.CreateAmount("MilkProduction", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_milk_production");
    this.MilkProduction.SetDisplayer((IAmountDisplayer) new MilkProductionDisplayer(GameUtil.TimeSlice.PerCycle, new Dictionary<Tag, string>()
    {
      {
        SimHashes.Ink.CreateTag(),
        "ui_icon_gunk"
      }
    }));
    this.ElementGrowth = this.CreateAmount("ElementGrowth", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES", "ui_icon_scale_growth");
    this.ElementGrowth.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
    this.Beckoning = this.CreateAmount("Beckoning", 0.0f, 100f, true, Units.Flat, 100.5f, true, "STRINGS.CREATURES", "ui_icon_moo");
    this.Beckoning.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
    this.BionicOxygenTank = this.CreateAmount("BionicOxygenTank", 0.0f, BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG, true, Units.Flat, 60f, true, "STRINGS.DUPLICANTS", "ui_icon_oxygentank");
    this.BionicOxygenTank.SetDisplayer((IAmountDisplayer) new BionicOxygenTankDisplayer(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerSecond));
    this.BionicOxygenTank.debugSetValue = (Action<AmountInstance, float>) ((instance, val) =>
    {
      BionicOxygenTankMonitor.Instance smi = instance.gameObject.GetSMI<BionicOxygenTankMonitor.Instance>();
      if (smi != null)
      {
        float availableOxygen = smi.AvailableOxygen;
        if ((double) val >= (double) availableOxygen)
        {
          float mass = val - availableOxygen;
          double num = (double) smi.AddGas(SimHashes.Oxygen, mass, 6282.44971f);
        }
        else
        {
          float amount = Mathf.Min(availableOxygen - val, availableOxygen);
          smi.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out float _, out SimUtil.DiseaseInfo _, out float _);
        }
      }
      else
      {
        double num1 = (double) instance.SetValue(val);
      }
    });
    this.BionicInternalBattery = this.CreateAmount("BionicInternalBattery", 0.0f, 480000f, true, Units.Flat, 4000f, true, "STRINGS.DUPLICANTS", "ui_icon_battery");
    this.BionicInternalBattery.SetDisplayer((IAmountDisplayer) new BionicBatteryDisplayer());
    this.BionicInternalBattery.debugSetValue = (Action<AmountInstance, float>) ((instance, val) =>
    {
      BionicBatteryMonitor.Instance smi = instance.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
      if (smi != null)
      {
        float currentCharge = smi.CurrentCharge;
        if ((double) val >= (double) currentCharge)
        {
          float joules = val - currentCharge;
          smi.DebugAddCharge(joules);
        }
        else
        {
          float joules = currentCharge - val;
          smi.ConsumePower(joules);
        }
      }
      else
      {
        double num = (double) instance.SetValue(val);
      }
    });
    this.BionicOil = this.CreateAmount("BionicOil", 0.0f, 200f, true, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_liquid");
    this.BionicOil.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerCycle));
    this.BionicGunk = this.CreateAmount("BionicGunk", 0.0f, GunkMonitor.GUNK_CAPACITY, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS", "ui_icon_gunk");
    this.BionicGunk.SetDisplayer((IAmountDisplayer) new BionicGunkDisplayer(GameUtil.TimeSlice.PerCycle));
    this.InternalBattery = this.CreateAmount("InternalBattery", 0.0f, 0.0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery");
    this.InternalBattery.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond));
    this.InternalChemicalBattery = this.CreateAmount("InternalChemicalBattery", 0.0f, 0.0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery");
    this.InternalChemicalBattery.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond));
    this.InternalBioBattery = this.CreateAmount("InternalBioBattery", 0.0f, 0.0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery");
    this.InternalBioBattery.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond));
    this.InternalElectroBank = this.CreateAmount("InternalElectroBank", 0.0f, 0.0f, true, Units.Flat, 4000f, true, "STRINGS.ROBOTS", "ui_icon_battery");
    this.InternalElectroBank.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond));
    this.Moisture = this.CreateAmount("Moisture", 0.0f, 100f, false, Units.Flat, 0.35f, true, "STRINGS.CREATURES", "ui_icon_wet");
    this.Moisture.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle));
    this.Mucus = this.CreateAmount("Mucus", 0.0f, 10f, false, Units.Flat, 0.35f, true, "STRINGS.CREATURES", "ui_icon_stamina", "attribute_stamina", "mod_stamina");
    this.Mucus.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerCycle));
  }

  public Amount CreateAmount(
    string id,
    float min,
    float max,
    bool show_max,
    Units units,
    float delta_threshold,
    bool show_in_ui,
    string string_root,
    string uiSprite = null,
    string thoughtSprite = null,
    string uiFullColourSprite = null)
  {
    return this.CreateAmount(id, min, max, show_max, units, delta_threshold, show_in_ui, true, string_root, uiSprite, thoughtSprite, uiFullColourSprite);
  }

  public Amount CreateAmount(
    string id,
    float min,
    float max,
    bool show_max,
    Units units,
    float delta_threshold,
    bool show_in_ui,
    bool show_delta_in_ui,
    string string_root,
    string uiSprite = null,
    string thoughtSprite = null,
    string uiFullColourSprite = null)
  {
    string name1 = (string) Strings.Get(string.Format("{1}.STATS.{0}.NAME", (object) id.ToUpper(), (object) string_root.ToUpper()));
    string description = (string) Strings.Get(string.Format("{1}.STATS.{0}.TOOLTIP", (object) id.ToUpper(), (object) string_root.ToUpper()));
    Klei.AI.Attribute.Display show_in_ui1 = show_in_ui ? Klei.AI.Attribute.Display.Normal : Klei.AI.Attribute.Display.Never;
    Klei.AI.Attribute.Display show_in_ui2 = show_delta_in_ui ? Klei.AI.Attribute.Display.Normal : Klei.AI.Attribute.Display.Never;
    string str1 = id + "Min";
    StringEntry result1;
    string name2 = Strings.TryGet(new StringKey(string.Format("{1}.ATTRIBUTES.{0}.NAME", (object) str1.ToUpper(), (object) string_root)), out result1) ? result1.String : "Minimum" + name1;
    StringEntry result2;
    string attribute_description1 = Strings.TryGet(new StringKey(string.Format("{1}.ATTRIBUTES.{0}.DESC", (object) str1.ToUpper(), (object) string_root)), out result2) ? result2.String : "Minimum" + name1;
    Klei.AI.Attribute attribute1 = new Klei.AI.Attribute(id + "Min", name2, "", attribute_description1, min, show_in_ui1, false, uiFullColourSprite: uiFullColourSprite);
    string str2 = id + "Max";
    StringEntry result3;
    string name3 = Strings.TryGet(new StringKey(string.Format("{1}.ATTRIBUTES.{0}.NAME", (object) str2.ToUpper(), (object) string_root)), out result3) ? result3.String : "Maximum" + name1;
    StringEntry result4;
    string attribute_description2 = Strings.TryGet(new StringKey(string.Format("{1}.ATTRIBUTES.{0}.DESC", (object) str2.ToUpper(), (object) string_root)), out result4) ? result4.String : "Maximum" + name1;
    Klei.AI.Attribute attribute2 = new Klei.AI.Attribute(id + "Max", name3, "", attribute_description2, max, show_in_ui1, false, uiFullColourSprite: uiFullColourSprite);
    string id1 = id + "Delta";
    string name4 = (string) Strings.Get(string.Format("{1}.ATTRIBUTES.{0}.NAME", (object) id1.ToUpper(), (object) string_root));
    string attribute_description3 = (string) Strings.Get(string.Format("{1}.ATTRIBUTES.{0}.DESC", (object) id1.ToUpper(), (object) string_root));
    Klei.AI.Attribute attribute3 = new Klei.AI.Attribute(id1, name4, "", attribute_description3, 0.0f, show_in_ui2, false, uiFullColourSprite: uiFullColourSprite);
    Amount resource = new Amount(id, name1, description, attribute1, attribute2, attribute3, show_max, units, delta_threshold, show_in_ui, uiSprite, thoughtSprite);
    Db.Get().Attributes.Add(attribute1);
    Db.Get().Attributes.Add(attribute2);
    Db.Get().Attributes.Add(attribute3);
    this.Add(resource);
    return resource;
  }
}
