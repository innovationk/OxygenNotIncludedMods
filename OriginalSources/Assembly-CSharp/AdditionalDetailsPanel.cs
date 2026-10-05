// Decompiled with JetBrains decompiler
// Type: AdditionalDetailsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AdditionalDetailsPanel : DetailScreenTab
{
  public GameObject attributesLabelTemplate;
  private CollapsibleDetailContentPanel detailsPanel;
  private DetailsPanelDrawer drawer;
  private CollapsibleDetailContentPanel immuneSystemPanel;
  private CollapsibleDetailContentPanel diseaseSourcePanel;
  private CollapsibleDetailContentPanel currentGermsPanel;
  private CollapsibleDetailContentPanel overviewPanel;
  private CollapsibleDetailContentPanel generatorsPanel;
  private CollapsibleDetailContentPanel consumersPanel;
  private CollapsibleDetailContentPanel batteriesPanel;
  private static readonly EventSystem.IntraObjectHandler<AdditionalDetailsPanel> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<AdditionalDetailsPanel>((Action<AdditionalDetailsPanel, object>) ((component, data) => component.OnRefreshData(data)));

  public override bool IsValidForTarget(GameObject target) => true;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.detailsPanel = this.CreateCollapsableSection((string) UI.DETAILTABS.DETAILS.GROUPNAME_DETAILS);
    this.drawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.detailsPanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
    this.immuneSystemPanel = this.CreateCollapsableSection((string) UI.DETAILTABS.DISEASE.CONTRACTION_RATES);
    this.diseaseSourcePanel = this.CreateCollapsableSection((string) UI.DETAILTABS.DISEASE.DISEASE_SOURCE);
    this.currentGermsPanel = this.CreateCollapsableSection((string) UI.DETAILTABS.DISEASE.CURRENT_GERMS);
    this.overviewPanel = this.CreateCollapsableSection((string) UI.DETAILTABS.ENERGYGENERATOR.CIRCUITOVERVIEW);
    this.generatorsPanel = this.CreateCollapsableSection((string) UI.DETAILTABS.ENERGYGENERATOR.GENERATORS);
    this.consumersPanel = this.CreateCollapsableSection((string) UI.DETAILTABS.ENERGYGENERATOR.CONSUMERS);
    this.batteriesPanel = this.CreateCollapsableSection((string) UI.DETAILTABS.ENERGYGENERATOR.BATTERIES);
    this.Subscribe<AdditionalDetailsPanel>(-1514841199, AdditionalDetailsPanel.OnRefreshDataDelegate);
  }

  private void OnRefreshData(object obj) => this.Refresh();

  private void Update() => this.Refresh();

  protected override void OnSelectTarget(GameObject target)
  {
    base.OnSelectTarget(target);
    this.Refresh();
  }

  private void Refresh()
  {
    AdditionalDetailsPanel.RefreshDetailsPanel(this.detailsPanel, this.selectedTarget);
    AdditionalDetailsPanel.RefreshImuneSystemPanel(this.immuneSystemPanel, this.selectedTarget);
    AdditionalDetailsPanel.RefreshCurrentGermsPanel(this.currentGermsPanel, this.selectedTarget);
    AdditionalDetailsPanel.RefreshDiseaseSourcePanel(this.diseaseSourcePanel, this.selectedTarget);
    AdditionalDetailsPanel.RefreshEnergyOverviewPanel(this.overviewPanel, this.selectedTarget);
    AdditionalDetailsPanel.RefreshEnergyGeneratorPanel(this.generatorsPanel, this.selectedTarget);
    AdditionalDetailsPanel.RefreshEnergyConsumerPanel(this.consumersPanel, this.selectedTarget);
    AdditionalDetailsPanel.RefreshEnergyBatteriesPanel(this.batteriesPanel, this.selectedTarget);
  }

  private static void RefreshDetailsPanel(
    CollapsibleDetailContentPanel targetPanel,
    GameObject targetEntity)
  {
    PrimaryElement component1 = targetEntity.GetComponent<PrimaryElement>();
    CellSelectionObject component2 = targetEntity.GetComponent<CellSelectionObject>();
    float mass;
    float temperature;
    Element element;
    byte idx;
    int units;
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      mass = component1.Mass;
      temperature = component1.Temperature;
      element = component1.Element;
      idx = component1.DiseaseIdx;
      units = component1.DiseaseCount;
    }
    else if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      mass = component2.Mass;
      temperature = component2.temperature;
      element = component2.element;
      idx = component2.diseaseIdx;
      units = component2.diseaseCount;
    }
    else
    {
      BackwallSelectionObject component3 = targetEntity.GetComponent<BackwallSelectionObject>();
      if (!((UnityEngine.Object) component3 != (UnityEngine.Object) null) || component3.element == null)
        return;
      mass = component3.Mass;
      temperature = component3.temperature;
      element = component3.element;
      idx = byte.MaxValue;
      units = 0;
    }
    bool flag1 = element.id == SimHashes.Vacuum || element.id == SimHashes.Void;
    float specificHeatCapacity = element.specificHeatCapacity;
    float highTemp = element.highTemp;
    float lowTemp = element.lowTemp;
    BuildingComplete component4 = targetEntity.GetComponent<BuildingComplete>();
    float num1 = !((UnityEngine.Object) component4 != (UnityEngine.Object) null) ? -1f : component4.creationTime;
    LogicPorts component5 = targetEntity.GetComponent<LogicPorts>();
    EnergyConsumer component6 = targetEntity.GetComponent<EnergyConsumer>();
    Operational component7 = targetEntity.GetComponent<Operational>();
    Battery component8 = targetEntity.GetComponent<Battery>();
    targetPanel.SetLabel("element_name", string.Format((string) UI.ELEMENTAL.PRIMARYELEMENT.NAME, (object) element.name), string.Format((string) UI.ELEMENTAL.PRIMARYELEMENT.TOOLTIP, (object) element.name));
    targetPanel.SetLabel("element_mass", string.Format((string) UI.ELEMENTAL.MASS.NAME, (object) GameUtil.GetFormattedMass(mass)), string.Format((string) UI.ELEMENTAL.MASS.TOOLTIP, (object) GameUtil.GetFormattedMass(mass)));
    if ((double) num1 > 0.0)
      targetPanel.SetLabel("element_age", string.Format((string) UI.ELEMENTAL.AGE.NAME, (object) Util.FormatTwoDecimalPlace((float) (((double) GameClock.Instance.GetTime() - (double) num1) / 600.0))), string.Format((string) UI.ELEMENTAL.AGE.TOOLTIP, (object) Util.FormatTwoDecimalPlace((float) (((double) GameClock.Instance.GetTime() - (double) num1) / 600.0))));
    int num_cycles = 5;
    float num2;
    float num3;
    float num4;
    if ((UnityEngine.Object) component7 != (UnityEngine.Object) null && ((UnityEngine.Object) component5 != (UnityEngine.Object) null || (UnityEngine.Object) component6 != (UnityEngine.Object) null || (UnityEngine.Object) component8 != (UnityEngine.Object) null))
    {
      num2 = component7.GetCurrentCycleUptime();
      num3 = component7.GetLastCycleUptime();
      num4 = component7.GetUptimeOverCycles(num_cycles);
    }
    else
    {
      num2 = -1f;
      num3 = -1f;
      num4 = -1f;
    }
    if ((double) num2 >= 0.0)
    {
      string text = ((string) UI.ELEMENTAL.UPTIME.NAME).Replace("{0}", "    • ").Replace("{1}", (string) UI.ELEMENTAL.UPTIME.THIS_CYCLE).Replace("{2}", GameUtil.GetFormattedPercent(num2 * 100f)).Replace("{3}", (string) UI.ELEMENTAL.UPTIME.LAST_CYCLE).Replace("{4}", GameUtil.GetFormattedPercent(num3 * 100f)).Replace("{5}", UI.ELEMENTAL.UPTIME.LAST_X_CYCLES.Replace("{0}", num_cycles.ToString())).Replace("{6}", GameUtil.GetFormattedPercent(num4 * 100f));
      targetPanel.SetLabel("uptime_name", text, "");
    }
    if (!flag1)
    {
      bool flag2 = false;
      float thermalConductivity = element.thermalConductivity;
      Building component9 = targetEntity.GetComponent<Building>();
      if ((UnityEngine.Object) component9 != (UnityEngine.Object) null)
      {
        thermalConductivity *= component9.Def.ThermalConductivity;
        flag2 = (double) component9.Def.ThermalConductivity < 1.0;
      }
      string temperatureUnitSuffix = GameUtil.GetTemperatureUnitSuffix();
      float shc = specificHeatCapacity * 1f;
      string text1 = string.Format((string) UI.ELEMENTAL.SHC.NAME, (object) GameUtil.GetDisplaySHC(shc).ToString("0.000"));
      string tooltip1 = ((string) UI.ELEMENTAL.SHC.TOOLTIP).Replace("{SPECIFIC_HEAT_CAPACITY}", text1 + GameUtil.GetSHCSuffix()).Replace("{TEMPERATURE_UNIT}", temperatureUnitSuffix);
      string text2 = string.Format((string) UI.ELEMENTAL.THERMALCONDUCTIVITY.NAME, (object) GameUtil.GetDisplayThermalConductivity(thermalConductivity).ToString("0.000"));
      string tooltip2 = ((string) UI.ELEMENTAL.THERMALCONDUCTIVITY.TOOLTIP).Replace("{THERMAL_CONDUCTIVITY}", text2 + GameUtil.GetThermalConductivitySuffix()).Replace("{TEMPERATURE_UNIT}", temperatureUnitSuffix);
      targetPanel.SetLabel("temperature", string.Format((string) UI.ELEMENTAL.TEMPERATURE.NAME, (object) GameUtil.GetFormattedTemperature(temperature)), string.Format((string) UI.ELEMENTAL.TEMPERATURE.TOOLTIP, (object) GameUtil.GetFormattedTemperature(temperature)));
      targetPanel.SetLabel("disease", string.Format((string) UI.ELEMENTAL.DISEASE.NAME, (object) GameUtil.GetFormattedDisease(idx, units)), string.Format((string) UI.ELEMENTAL.DISEASE.TOOLTIP, (object) GameUtil.GetFormattedDisease(idx, units, true)));
      targetPanel.SetLabel("shc", text1, tooltip1);
      targetPanel.SetLabel("tc", text2, tooltip2);
      if (flag2)
        targetPanel.SetLabel("insulated", (string) UI.GAMEOBJECTEFFECTS.INSULATED.NAME, (string) UI.GAMEOBJECTEFFECTS.INSULATED.TOOLTIP);
    }
    if (element.IsSolid)
    {
      targetPanel.SetLabel("melting_point", string.Format((string) UI.ELEMENTAL.MELTINGPOINT.NAME, (object) GameUtil.GetFormattedTemperature(highTemp)), string.Format((string) UI.ELEMENTAL.MELTINGPOINT.TOOLTIP, (object) GameUtil.GetFormattedTemperature(highTemp)));
      targetPanel.SetLabel("melting_point", string.Format((string) UI.ELEMENTAL.MELTINGPOINT.NAME, (object) GameUtil.GetFormattedTemperature(highTemp)), string.Format((string) UI.ELEMENTAL.MELTINGPOINT.TOOLTIP, (object) GameUtil.GetFormattedTemperature(highTemp)));
      if ((UnityEngine.Object) targetEntity.GetComponent<ElementChunk>() != (UnityEngine.Object) null)
      {
        AttributeModifier attributeModifier = component1.Element.attributeModifiers.Find((Predicate<AttributeModifier>) (m => m.AttributeId == Db.Get().BuildingAttributes.OverheatTemperature.Id));
        if (attributeModifier != null)
          targetPanel.SetLabel("overheat", string.Format((string) UI.ELEMENTAL.OVERHEATPOINT.NAME, (object) attributeModifier.GetFormattedString()), string.Format((string) UI.ELEMENTAL.OVERHEATPOINT.TOOLTIP, (object) attributeModifier.GetFormattedString()));
      }
    }
    else if (element.IsLiquid)
    {
      targetPanel.SetLabel("freezepoint", string.Format((string) UI.ELEMENTAL.FREEZEPOINT.NAME, (object) GameUtil.GetFormattedTemperature(lowTemp)), string.Format((string) UI.ELEMENTAL.FREEZEPOINT.TOOLTIP, (object) GameUtil.GetFormattedTemperature(lowTemp)));
      targetPanel.SetLabel("vapourizationpoint", string.Format((string) UI.ELEMENTAL.VAPOURIZATIONPOINT.NAME, (object) GameUtil.GetFormattedTemperature(highTemp)), string.Format((string) UI.ELEMENTAL.VAPOURIZATIONPOINT.TOOLTIP, (object) GameUtil.GetFormattedTemperature(highTemp)));
    }
    else if (!flag1)
      targetPanel.SetLabel("dewpoint", string.Format((string) UI.ELEMENTAL.DEWPOINT.NAME, (object) GameUtil.GetFormattedTemperature(lowTemp)), string.Format((string) UI.ELEMENTAL.DEWPOINT.TOOLTIP, (object) GameUtil.GetFormattedTemperature(lowTemp)));
    if (DlcManager.FeatureRadiationEnabled())
    {
      string formattedPercent = GameUtil.GetFormattedPercent(GameUtil.GetRadiationAbsorptionPercentage(Grid.PosToCell(targetEntity)) * 100f);
      targetPanel.SetLabel("radiationabsorption", string.Format((string) UI.DETAILTABS.DETAILS.RADIATIONABSORPTIONFACTOR.NAME, (object) formattedPercent), string.Format((string) UI.DETAILTABS.DETAILS.RADIATIONABSORPTIONFACTOR.TOOLTIP, (object) formattedPercent));
    }
    Attributes attributes = targetEntity.GetAttributes();
    if (attributes != null)
    {
      for (int index = 0; index < attributes.Count; ++index)
      {
        AttributeInstance attributeInstance = attributes.AttributeTable[index];
        if (DlcManager.IsCorrectDlcSubscribed((IHasDlcRestrictions) attributeInstance.Attribute) && (attributeInstance.Attribute.ShowInUI == Klei.AI.Attribute.Display.Details || attributeInstance.Attribute.ShowInUI == Klei.AI.Attribute.Display.Expectation))
          targetPanel.SetLabel(attributeInstance.modifier.Id, $"{attributeInstance.modifier.Name}: {attributeInstance.GetFormattedValue()}", attributeInstance.GetAttributeValueTooltip());
      }
    }
    List<Descriptor> detailDescriptors = GameUtil.GetDetailDescriptors(GameUtil.GetAllDescriptors(targetEntity));
    for (int index = 0; index < detailDescriptors.Count; ++index)
    {
      Descriptor descriptor = detailDescriptors[index];
      targetPanel.SetLabel("descriptor_" + index.ToString(), descriptor.text, descriptor.tooltipText);
    }
    targetPanel.Commit();
  }

  private static void RefreshDiseaseSourcePanel(
    CollapsibleDetailContentPanel targetPanel,
    GameObject targetEntity)
  {
    List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(targetEntity, true);
    Sicknesses sicknesses = targetEntity.GetSicknesses();
    if (sicknesses != null)
    {
      for (int idx = 0; idx < sicknesses.Count; ++idx)
        allDescriptors.AddRange((IEnumerable<Descriptor>) sicknesses[idx].GetDescriptors());
    }
    List<Descriptor> all = allDescriptors.FindAll((Predicate<Descriptor>) (e => e.type == Descriptor.DescriptorType.DiseaseSource));
    if (all.Count > 0)
    {
      for (int index = 0; index < all.Count; ++index)
        targetPanel.SetLabel("source_" + index.ToString(), all[index].text, all[index].tooltipText);
    }
    targetPanel.Commit();
  }

  private static void RefreshCurrentGermsPanel(
    CollapsibleDetailContentPanel targetPanel,
    GameObject targetEntity)
  {
    if ((UnityEngine.Object) targetEntity != (UnityEngine.Object) null)
    {
      CellSelectionObject component1 = targetEntity.GetComponent<CellSelectionObject>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        if (component1.diseaseIdx != byte.MaxValue && component1.diseaseCount > 0)
        {
          Klei.AI.Disease disease = Db.Get().Diseases[(int) component1.diseaseIdx];
          AdditionalDetailsPanel.BuildFactorsStrings(targetPanel, component1.diseaseCount, component1.element.idx, component1.SelectedCell, component1.Mass, component1.temperature, (HashSet<Tag>) null, disease, true);
        }
        else
          targetPanel.SetLabel("currentgerms", (string) UI.DETAILTABS.DISEASE.DETAILS.NODISEASE, (string) UI.DETAILTABS.DISEASE.DETAILS.NODISEASE_TOOLTIP);
      }
      else
      {
        PrimaryElement component2 = targetEntity.GetComponent<PrimaryElement>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          if (component2.DiseaseIdx != byte.MaxValue && component2.DiseaseCount > 0)
          {
            Klei.AI.Disease disease = Db.Get().Diseases[(int) component2.DiseaseIdx];
            int cell = Grid.PosToCell(component2.transform.GetPosition());
            KPrefabID component3 = component2.GetComponent<KPrefabID>();
            AdditionalDetailsPanel.BuildFactorsStrings(targetPanel, component2.DiseaseCount, component2.Element.idx, cell, component2.Mass, component2.Temperature, component3.Tags, disease);
          }
          else
            targetPanel.SetLabel("currentgerms", (string) UI.DETAILTABS.DISEASE.DETAILS.NODISEASE, (string) UI.DETAILTABS.DISEASE.DETAILS.NODISEASE_TOOLTIP);
        }
      }
    }
    targetPanel.Commit();
  }

  private static void RefreshImuneSystemPanel(
    CollapsibleDetailContentPanel targetPanel,
    GameObject targetEntity)
  {
    GermExposureMonitor.Instance smi = targetEntity.GetSMI<GermExposureMonitor.Instance>();
    if (smi != null)
    {
      targetPanel.SetLabel("germ_resistance", $"{Db.Get().Attributes.GermResistance.Name}: {smi.GetGermResistance().ToString()}", (string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.DESC);
      for (int idx = 0; idx < Db.Get().Diseases.Count; ++idx)
      {
        Klei.AI.Disease disease = Db.Get().Diseases[idx];
        ExposureType exposureTypeForDisease = GameUtil.GetExposureTypeForDisease(disease);
        Sickness sicknessForDisease = GameUtil.GetSicknessForDisease(disease);
        if (sicknessForDisease != null)
        {
          bool flag1 = true;
          List<string> stringList1 = new List<string>();
          if (exposureTypeForDisease.required_traits != null && exposureTypeForDisease.required_traits.Count > 0)
          {
            for (int index = 0; index < exposureTypeForDisease.required_traits.Count; ++index)
            {
              if (!targetEntity.GetComponent<Traits>().HasTrait(exposureTypeForDisease.required_traits[index]))
                stringList1.Add(exposureTypeForDisease.required_traits[index]);
            }
            if (stringList1.Count > 0)
              flag1 = false;
          }
          bool flag2 = false;
          List<string> stringList2 = new List<string>();
          if (exposureTypeForDisease.excluded_effects != null && exposureTypeForDisease.excluded_effects.Count > 0)
          {
            for (int index = 0; index < exposureTypeForDisease.excluded_effects.Count; ++index)
            {
              if (targetEntity.GetComponent<Effects>().HasEffect(exposureTypeForDisease.excluded_effects[index]))
                stringList2.Add(exposureTypeForDisease.excluded_effects[index]);
            }
            if (stringList2.Count > 0)
              flag2 = true;
          }
          bool flag3 = false;
          List<string> stringList3 = new List<string>();
          if (exposureTypeForDisease.excluded_traits != null && exposureTypeForDisease.excluded_traits.Count > 0)
          {
            for (int index = 0; index < exposureTypeForDisease.excluded_traits.Count; ++index)
            {
              if (targetEntity.GetComponent<Traits>().HasTrait(exposureTypeForDisease.excluded_traits[index]))
                stringList3.Add(exposureTypeForDisease.excluded_traits[index]);
            }
            if (stringList3.Count > 0)
              flag3 = true;
          }
          string str1 = "";
          float num;
          if (!flag1)
          {
            num = 0.0f;
            string str2 = "";
            for (int index = 0; index < stringList1.Count; ++index)
            {
              if (str2 != "")
                str2 += ", ";
              str2 += Db.Get().traits.Get(stringList1[index]).Name;
            }
            str1 += string.Format((string) DUPLICANTS.DISEASES.IMMUNE_FROM_MISSING_REQUIRED_TRAIT, (object) str2);
          }
          else if (flag3)
          {
            num = 0.0f;
            string str3 = "";
            for (int index = 0; index < stringList3.Count; ++index)
            {
              if (str3 != "")
                str3 += ", ";
              str3 += Db.Get().traits.Get(stringList3[index]).Name;
            }
            if (str1 != "")
              str1 += "\n";
            str1 += string.Format((string) DUPLICANTS.DISEASES.IMMUNE_FROM_HAVING_EXLCLUDED_TRAIT, (object) str3);
          }
          else if (flag2)
          {
            num = 0.0f;
            string str4 = "";
            for (int index = 0; index < stringList2.Count; ++index)
            {
              if (str4 != "")
                str4 += ", ";
              str4 += Db.Get().effects.Get(stringList2[index]).Name;
            }
            if (str1 != "")
              str1 += "\n";
            str1 += string.Format((string) DUPLICANTS.DISEASES.IMMUNE_FROM_HAVING_EXCLUDED_EFFECT, (object) str4);
          }
          else
            num = !exposureTypeForDisease.infect_immediately ? GermExposureMonitor.GetContractionChance(smi.GetResistanceToExposureType(exposureTypeForDisease, 3f)) : 1f;
          string str5 = str1 != "" ? str1 : string.Format((string) DUPLICANTS.DISEASES.CONTRACTION_PROBABILITY, (object) GameUtil.GetFormattedPercent(num * 100f), (object) targetEntity.GetProperName(), (object) sicknessForDisease.Name);
          targetPanel.SetLabel("disease_" + disease.Id, $"    • {disease.Name}: {GameUtil.GetFormattedPercent(num * 100f)}", string.Format((string) DUPLICANTS.DISEASES.RESISTANCES_PANEL_TOOLTIP, (object) str5, (object) sicknessForDisease.Name));
        }
      }
    }
    targetPanel.Commit();
  }

  private static string GetFormattedHalfLife(float hl)
  {
    return AdditionalDetailsPanel.GetFormattedGrowthRate(Klei.AI.Disease.HalfLifeToGrowthRate(hl, 600f));
  }

  private static string GetFormattedGrowthRate(float rate)
  {
    if ((double) rate < 1.0)
      return string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.DEATH_FORMAT, (object) GameUtil.GetFormattedPercent((float) (100.0 * (1.0 - (double) rate))), (object) UI.DETAILTABS.DISEASE.DETAILS.DEATH_FORMAT_TOOLTIP);
    return (double) rate > 1.0 ? string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FORMAT, (object) GameUtil.GetFormattedPercent((float) (100.0 * ((double) rate - 1.0))), (object) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FORMAT_TOOLTIP) : string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.NEUTRAL_FORMAT, (object) UI.DETAILTABS.DISEASE.DETAILS.NEUTRAL_FORMAT_TOOLTIP);
  }

  private static string GetFormattedGrowthEntry(
    string name,
    float halfLife,
    string dyingFormat,
    string growingFormat,
    string neutralFormat)
  {
    return string.Format((double) halfLife != double.PositiveInfinity ? ((double) halfLife <= 0.0 ? growingFormat : dyingFormat) : neutralFormat, (object) name, (object) AdditionalDetailsPanel.GetFormattedHalfLife(halfLife));
  }

  private static void BuildFactorsStrings(
    CollapsibleDetailContentPanel targetPanel,
    int diseaseCount,
    ushort elementIdx,
    int environmentCell,
    float environmentMass,
    float temperature,
    HashSet<Tag> tags,
    Klei.AI.Disease disease,
    bool isCell = false)
  {
    targetPanel.SetTitle(string.Format((string) UI.DETAILTABS.DISEASE.CURRENT_GERMS, (object) disease.Name.ToUpper()));
    targetPanel.SetLabel("currentgerms", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.DISEASE_AMOUNT, (object) disease.Name, (object) GameUtil.GetFormattedDiseaseAmount(diseaseCount)), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.DISEASE_AMOUNT_TOOLTIP, (object) GameUtil.GetFormattedDiseaseAmount(diseaseCount)));
    Element element = ElementLoader.elements[(int) elementIdx];
    CompositeGrowthRule growthRuleForElement = disease.GetGrowthRuleForElement(element);
    float tags_multiplier_base = 1f;
    if (tags != null && tags.Count > 0)
      tags_multiplier_base = disease.GetGrowthRateForTags(tags, (double) diseaseCount > (double) growthRuleForElement.maxCountPerKG * (double) environmentMass);
    float delta = DiseaseContainers.CalculateDelta(diseaseCount, elementIdx, environmentMass, environmentCell, temperature, tags_multiplier_base, disease, 1f, Sim.IsRadiationEnabled());
    targetPanel.SetLabel("finaldelta", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RATE_OF_CHANGE, (object) GameUtil.GetFormattedSimple(delta, GameUtil.TimeSlice.PerSecond, "F0")), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RATE_OF_CHANGE_TOOLTIP, (object) GameUtil.GetFormattedSimple(delta, GameUtil.TimeSlice.PerSecond, "F0")));
    float halfLife = Klei.AI.Disease.GrowthRateToHalfLife((float) (1.0 - (double) delta / (double) diseaseCount));
    if ((double) halfLife > 0.0)
      targetPanel.SetLabel("finalhalflife", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEG, (object) GameUtil.GetFormattedCycles(halfLife)), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEG_TOOLTIP, (object) GameUtil.GetFormattedCycles(halfLife)));
    else if ((double) halfLife < 0.0)
      targetPanel.SetLabel("finalhalflife", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_POS, (object) GameUtil.GetFormattedCycles(-halfLife)), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_POS_TOOLTIP, (object) GameUtil.GetFormattedCycles(halfLife)));
    else
      targetPanel.SetLabel("finalhalflife", (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEUTRAL, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEUTRAL_TOOLTIP);
    targetPanel.SetLabel("factors", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TITLE), (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TOOLTIP);
    bool flag = false;
    if ((double) diseaseCount < (double) growthRuleForElement.minCountPerKG * (double) environmentMass)
    {
      targetPanel.SetLabel("critical_status", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.DYING_OFF.TITLE, (object) AdditionalDetailsPanel.GetFormattedGrowthRate(-growthRuleForElement.underPopulationDeathRate)), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.DYING_OFF.TOOLTIP, (object) GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(growthRuleForElement.minCountPerKG * environmentMass)), (object) GameUtil.GetFormattedMass(environmentMass), (object) growthRuleForElement.minCountPerKG));
      flag = true;
    }
    else if ((double) diseaseCount > (double) growthRuleForElement.maxCountPerKG * (double) environmentMass)
    {
      targetPanel.SetLabel("critical_status", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.OVERPOPULATED.TITLE, (object) AdditionalDetailsPanel.GetFormattedHalfLife(growthRuleForElement.overPopulationHalfLife)), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.OVERPOPULATED.TOOLTIP, (object) GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(growthRuleForElement.maxCountPerKG * environmentMass)), (object) GameUtil.GetFormattedMass(environmentMass), (object) growthRuleForElement.maxCountPerKG));
      flag = true;
    }
    if (!flag)
      targetPanel.SetLabel("substrate", AdditionalDetailsPanel.GetFormattedGrowthEntry(growthRuleForElement.Name(), growthRuleForElement.populationHalfLife, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL), AdditionalDetailsPanel.GetFormattedGrowthEntry(growthRuleForElement.Name(), growthRuleForElement.populationHalfLife, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE_TOOLTIP, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW_TOOLTIP, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL_TOOLTIP));
    int num1 = 0;
    if (tags != null)
    {
      foreach (Tag tag in tags)
      {
        TagGrowthRule growthRuleForTag = disease.GetGrowthRuleForTag(tag);
        if (growthRuleForTag != null)
          targetPanel.SetLabel("tag_" + num1.ToString(), AdditionalDetailsPanel.GetFormattedGrowthEntry(growthRuleForTag.Name(), growthRuleForTag.populationHalfLife.Value, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL), AdditionalDetailsPanel.GetFormattedGrowthEntry(growthRuleForTag.Name(), growthRuleForTag.populationHalfLife.Value, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE_TOOLTIP, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW_TOOLTIP, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL_TOOLTIP));
        ++num1;
      }
    }
    if (Grid.IsValidCell(environmentCell))
    {
      if (!isCell)
      {
        CompositeExposureRule exposureRuleForElement = disease.GetExposureRuleForElement(Grid.Element[environmentCell]);
        if (exposureRuleForElement != null && (double) exposureRuleForElement.populationHalfLife != double.PositiveInfinity)
        {
          if ((double) exposureRuleForElement.GetHalfLifeForCount(diseaseCount) > 0.0)
            targetPanel.SetLabel("environment", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.TITLE, (object) exposureRuleForElement.Name(), (object) AdditionalDetailsPanel.GetFormattedHalfLife(exposureRuleForElement.GetHalfLifeForCount(diseaseCount))), (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.DIE_TOOLTIP);
          else
            targetPanel.SetLabel("environment", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.TITLE, (object) exposureRuleForElement.Name(), (object) AdditionalDetailsPanel.GetFormattedHalfLife(exposureRuleForElement.GetHalfLifeForCount(diseaseCount))), (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.GROW_TOOLTIP);
        }
      }
      if (Sim.IsRadiationEnabled())
      {
        float f = Grid.Radiation[environmentCell];
        if ((double) f > 0.0)
        {
          float num2 = disease.radiationKillRate * f;
          float hl = (float) diseaseCount * 0.5f / num2;
          targetPanel.SetLabel("radiation", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RADIATION.TITLE, (object) Mathf.RoundToInt(f), (object) AdditionalDetailsPanel.GetFormattedHalfLife(hl)), (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RADIATION.DIE_TOOLTIP);
        }
      }
    }
    float temperatureHalfLife = disease.CalculateTemperatureHalfLife(temperature);
    if ((double) temperatureHalfLife == double.PositiveInfinity)
      return;
    if ((double) temperatureHalfLife > 0.0)
      targetPanel.SetLabel(nameof (temperature), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.TITLE, (object) GameUtil.GetFormattedTemperature(temperature), (object) AdditionalDetailsPanel.GetFormattedHalfLife(temperatureHalfLife)), (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.DIE_TOOLTIP);
    else
      targetPanel.SetLabel(nameof (temperature), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.TITLE, (object) GameUtil.GetFormattedTemperature(temperature), (object) AdditionalDetailsPanel.GetFormattedHalfLife(temperatureHalfLife)), (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.GROW_TOOLTIP);
  }

  private static void RefreshEnergyOverviewPanel(
    CollapsibleDetailContentPanel targetPanel,
    GameObject targetEntity)
  {
    if ((UnityEngine.Object) targetEntity == (UnityEngine.Object) null)
      return;
    if (targetEntity.GetComponent<ICircuitConnected>() != null || (UnityEngine.Object) targetEntity.GetComponent<Wire>() != (UnityEngine.Object) null)
    {
      ushort selectedTargetCircuitId = AdditionalDetailsPanel.GetSelectedTargetCircuitID(targetEntity);
      if (selectedTargetCircuitId == ushort.MaxValue)
      {
        targetPanel.SetLabel("nocircuit", (string) UI.DETAILTABS.ENERGYGENERATOR.DISCONNECTED, (string) UI.DETAILTABS.ENERGYGENERATOR.DISCONNECTED);
      }
      else
      {
        float availableOnCircuit = Game.Instance.circuitManager.GetJoulesAvailableOnCircuit(selectedTargetCircuitId);
        targetPanel.SetLabel("joulesAvailable", string.Format((string) UI.DETAILTABS.ENERGYGENERATOR.AVAILABLE_JOULES, (object) GameUtil.GetFormattedJoules(availableOnCircuit)), (string) UI.DETAILTABS.ENERGYGENERATOR.AVAILABLE_JOULES_TOOLTIP);
        float generatedByCircuit1 = Game.Instance.circuitManager.GetWattsGeneratedByCircuit(selectedTargetCircuitId);
        float generatedByCircuit2 = Game.Instance.circuitManager.GetPotentialWattsGeneratedByCircuit(selectedTargetCircuitId);
        string str = (double) generatedByCircuit1 != (double) generatedByCircuit2 ? $"{GameUtil.GetFormattedWattage(generatedByCircuit1)} / {GameUtil.GetFormattedWattage(generatedByCircuit2)}" : GameUtil.GetFormattedWattage(generatedByCircuit1);
        targetPanel.SetLabel("wattageGenerated", string.Format((string) UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_GENERATED, (object) str), (string) UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_GENERATED_TOOLTIP);
        targetPanel.SetLabel("wattageConsumed", string.Format((string) UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_CONSUMED, (object) GameUtil.GetFormattedWattage(Game.Instance.circuitManager.GetWattsUsedByCircuit(selectedTargetCircuitId))), (string) UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_CONSUMED_TOOLTIP);
        targetPanel.SetLabel("potentialWattageConsumed", string.Format((string) UI.DETAILTABS.ENERGYGENERATOR.POTENTIAL_WATTAGE_CONSUMED, (object) GameUtil.GetFormattedWattage(Game.Instance.circuitManager.GetWattsNeededWhenActive(selectedTargetCircuitId))), (string) UI.DETAILTABS.ENERGYGENERATOR.POTENTIAL_WATTAGE_CONSUMED_TOOLTIP);
        targetPanel.SetLabel("maxSafeWattage", string.Format((string) UI.DETAILTABS.ENERGYGENERATOR.MAX_SAFE_WATTAGE, (object) GameUtil.GetFormattedWattage(Game.Instance.circuitManager.GetMaxSafeWattageForCircuit(selectedTargetCircuitId))), (string) UI.DETAILTABS.ENERGYGENERATOR.MAX_SAFE_WATTAGE_TOOLTIP);
      }
    }
    targetPanel.Commit();
  }

  private static void RefreshEnergyGeneratorPanel(
    CollapsibleDetailContentPanel targetPanel,
    GameObject targetEntity)
  {
    if ((UnityEngine.Object) targetEntity == (UnityEngine.Object) null)
      return;
    ushort selectedTargetCircuitId = AdditionalDetailsPanel.GetSelectedTargetCircuitID(targetEntity);
    if (selectedTargetCircuitId == ushort.MaxValue)
    {
      targetPanel.SetActive(false);
    }
    else
    {
      targetPanel.SetActive(true);
      List<Generator> generatorsOnCircuit = Game.Instance.circuitManager.GetGeneratorsOnCircuit(selectedTargetCircuitId);
      if (generatorsOnCircuit.Count > 0)
      {
        foreach (Generator generator in generatorsOnCircuit)
        {
          if ((UnityEngine.Object) generator != (UnityEngine.Object) null && (UnityEngine.Object) generator.GetComponent<Battery>() == (UnityEngine.Object) null)
          {
            string str = !generator.IsProducingPower() ? $"{generator.GetComponent<KSelectable>().entityName}: {GameUtil.GetFormattedWattage(0.0f)} / {GameUtil.GetFormattedWattage(generator.WattageRating)}" : $"{generator.GetComponent<KSelectable>().entityName}: {GameUtil.GetFormattedWattage(generator.WattageRating)}";
            string text = (UnityEngine.Object) generator.gameObject == (UnityEngine.Object) targetEntity ? $"<b>{str}</b>" : str;
            targetPanel.SetLabel(generator.gameObject.GetInstanceID().ToString(), text, "");
          }
        }
      }
      else
        targetPanel.SetLabel("nogenerators", (string) UI.DETAILTABS.ENERGYGENERATOR.NOGENERATORS, "");
      targetPanel.Commit();
    }
  }

  private static void RefreshEnergyConsumerPanel(
    CollapsibleDetailContentPanel targetPanel,
    GameObject targetEntity)
  {
    if ((UnityEngine.Object) targetEntity == (UnityEngine.Object) null)
      return;
    ushort selectedTargetCircuitId = AdditionalDetailsPanel.GetSelectedTargetCircuitID(targetEntity);
    if (selectedTargetCircuitId == ushort.MaxValue)
    {
      targetPanel.SetActive(false);
    }
    else
    {
      targetPanel.SetActive(true);
      List<IEnergyConsumer> consumersOnCircuit = Game.Instance.circuitManager.GetConsumersOnCircuit(selectedTargetCircuitId);
      List<Battery> transformersOnCircuit = Game.Instance.circuitManager.GetTransformersOnCircuit(selectedTargetCircuitId);
      if (consumersOnCircuit.Count > 0 || transformersOnCircuit.Count > 0)
      {
        foreach (IEnergyConsumer consumer in consumersOnCircuit)
          AddConsumerInfo(consumer);
        foreach (IEnergyConsumer consumer in transformersOnCircuit)
          AddConsumerInfo(consumer);
      }
      else
        targetPanel.SetLabel("noconsumers", (string) UI.DETAILTABS.ENERGYGENERATOR.NOCONSUMERS, "");
      targetPanel.Commit();
    }

    void AddConsumerInfo(IEnergyConsumer consumer)
    {
      KMonoBehaviour kmonoBehaviour = consumer as KMonoBehaviour;
      if (!((UnityEngine.Object) kmonoBehaviour != (UnityEngine.Object) null))
        return;
      float wattsUsed = consumer.WattsUsed;
      float neededWhenActive = consumer.WattsNeededWhenActive;
      string str1 = (double) wattsUsed != (double) neededWhenActive ? $"{GameUtil.GetFormattedWattage(wattsUsed)} / {GameUtil.GetFormattedWattage(neededWhenActive)}" : GameUtil.GetFormattedWattage(wattsUsed);
      string str2 = $"{consumer.Name}: {str1}";
      string text = (UnityEngine.Object) kmonoBehaviour.gameObject == (UnityEngine.Object) targetEntity ? $"<b>{str2}</b>" : str2;
      targetPanel.SetLabel(kmonoBehaviour.gameObject.GetInstanceID().ToString(), text, "");
    }
  }

  private static void RefreshEnergyBatteriesPanel(
    CollapsibleDetailContentPanel targetPanel,
    GameObject targetEntity)
  {
    if ((UnityEngine.Object) targetEntity == (UnityEngine.Object) null)
      return;
    ushort selectedTargetCircuitId = AdditionalDetailsPanel.GetSelectedTargetCircuitID(targetEntity);
    if (selectedTargetCircuitId == ushort.MaxValue)
    {
      targetPanel.SetActive(false);
    }
    else
    {
      targetPanel.SetActive(true);
      List<Battery> batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(selectedTargetCircuitId);
      if (batteriesOnCircuit.Count > 0)
      {
        foreach (Battery battery in batteriesOnCircuit)
        {
          if ((UnityEngine.Object) battery != (UnityEngine.Object) null)
          {
            string str = $"{battery.GetComponent<KSelectable>().entityName}: {GameUtil.GetFormattedJoules(battery.JoulesAvailable)}";
            string text = (UnityEngine.Object) battery.gameObject == (UnityEngine.Object) targetEntity ? $"<b>{str}</b>" : str;
            targetPanel.SetLabel(battery.gameObject.GetInstanceID().ToString(), text, "");
          }
        }
      }
      else
        targetPanel.SetLabel("nobatteries", (string) UI.DETAILTABS.ENERGYGENERATOR.NOBATTERIES, "");
      targetPanel.Commit();
    }
  }

  private static ushort GetSelectedTargetCircuitID(GameObject targetEntity)
  {
    CircuitManager circuitManager = Game.Instance.circuitManager;
    ICircuitConnected component = targetEntity.GetComponent<ICircuitConnected>();
    ushort selectedTargetCircuitId = ushort.MaxValue;
    if (component != null)
      selectedTargetCircuitId = Game.Instance.circuitManager.GetCircuitID(component);
    else if ((UnityEngine.Object) targetEntity.GetComponent<Wire>() != (UnityEngine.Object) null)
      selectedTargetCircuitId = Game.Instance.circuitManager.GetCircuitID(Grid.PosToCell(targetEntity.transform.GetPosition()));
    return selectedTargetCircuitId;
  }
}
