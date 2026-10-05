// Decompiled with JetBrains decompiler
// Type: MinionVitalsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/MinionVitalsPanel")]
public class MinionVitalsPanel : CollapsibleDetailContentPanel
{
  public GameObject LineItemPrefab;
  public GameObject CheckboxLinePrefab;
  private GameObject lastSelectedEntity;
  public List<MinionVitalsPanel.AmountLine> amountsLines = new List<MinionVitalsPanel.AmountLine>();
  public List<MinionVitalsPanel.AttributeLine> attributesLines = new List<MinionVitalsPanel.AttributeLine>();
  public List<MinionVitalsPanel.CheckboxLine> checkboxLines = new List<MinionVitalsPanel.CheckboxLine>();
  public Transform conditionsContainerNormal;
  public Transform conditionsContainerAdditional;
  private string unpollinatedTooltip;

  public void Init()
  {
    this.AddAmountLine(Db.Get().Amounts.HitPoints);
    this.AddAmountLine(Db.Get().Amounts.BionicInternalBattery);
    this.AddAmountLine(Db.Get().Amounts.BionicOil);
    this.AddAmountLine(Db.Get().Amounts.BionicGunk);
    this.AddAttributeLine(Db.Get().CritterAttributes.Happiness);
    this.AddAmountLine(Db.Get().Amounts.Wildness);
    this.AddAmountLine(Db.Get().Amounts.Incubation);
    this.AddAmountLine(Db.Get().Amounts.Viability);
    this.AddAmountLine(Db.Get().Amounts.PowerCharge);
    this.AddAmountLine(Db.Get().Amounts.Fertility);
    this.AddAmountLine(Db.Get().Amounts.Beckoning);
    this.AddAmountLine(Db.Get().Amounts.Age);
    this.AddAmountLine(Db.Get().Amounts.Stress);
    this.AddAttributeLine(Db.Get().Attributes.QualityOfLife);
    this.AddAmountLine(Db.Get().Amounts.Bladder);
    this.AddAmountLine(Db.Get().Amounts.Breath);
    this.AddAmountLine(Db.Get().Amounts.BionicOxygenTank);
    this.AddAmountLine(Db.Get().Amounts.Stamina);
    this.AddAttributeLine(Db.Get().CritterAttributes.Metabolism);
    this.AddAmountLine(Db.Get().Amounts.Calories);
    this.AddAmountLine(Db.Get().Amounts.ScaleGrowth);
    this.AddAmountLine(Db.Get().Amounts.MilkProduction);
    this.AddAmountLine(Db.Get().Amounts.ElementGrowth);
    this.AddAmountLine(Db.Get().Amounts.Moisture);
    this.AddAmountLine(Db.Get().Amounts.Temperature);
    this.AddAmountLine(Db.Get().Amounts.CritterTemperature);
    this.AddAmountLine(Db.Get().Amounts.Decor);
    this.AddAmountLine(Db.Get().Amounts.InternalBattery);
    this.AddAmountLine(Db.Get().Amounts.InternalChemicalBattery);
    this.AddAmountLine(Db.Get().Amounts.InternalBioBattery);
    this.AddAmountLine(Db.Get().Amounts.InternalElectroBank);
    if (DlcManager.FeatureRadiationEnabled())
      this.AddAmountLine(Db.Get().Amounts.RadiationBalance);
    this.AddCheckboxLine(Db.Get().Amounts.AirPressure, this.conditionsContainerNormal, (Func<GameObject, string>) (go => this.GetAirPressureLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go => (UnityEngine.Object) go.GetComponent<PressureVulnerable>() != (UnityEngine.Object) null && go.GetComponent<PressureVulnerable>().pressure_sensitive ? MinionVitalsPanel.CheckboxLineDisplayType.Normal : MinionVitalsPanel.CheckboxLineDisplayType.Hidden), (Func<GameObject, bool>) (go => this.check_pressure(go)), (Func<GameObject, string>) (go => this.GetAirPressureTooltip(go)));
    this.AddCheckboxLine((Amount) null, this.conditionsContainerNormal, (Func<GameObject, string>) (go => this.GetAtmosphereLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go => (UnityEngine.Object) go.GetComponent<PressureVulnerable>() != (UnityEngine.Object) null && go.GetComponent<PressureVulnerable>().safe_atmospheres.Count > 0 ? MinionVitalsPanel.CheckboxLineDisplayType.Normal : MinionVitalsPanel.CheckboxLineDisplayType.Hidden), (Func<GameObject, bool>) (go => this.check_atmosphere(go)), (Func<GameObject, string>) (go => this.GetAtmosphereTooltip(go)));
    this.AddCheckboxLine(Db.Get().Amounts.Temperature, this.conditionsContainerNormal, (Func<GameObject, string>) (go => this.GetInternalTemperatureLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go => (UnityEngine.Object) go.GetComponent<TemperatureVulnerable>() != (UnityEngine.Object) null ? MinionVitalsPanel.CheckboxLineDisplayType.Normal : MinionVitalsPanel.CheckboxLineDisplayType.Hidden), (Func<GameObject, bool>) (go => this.check_temperature(go)), (Func<GameObject, string>) (go => this.GetInternalTemperatureTooltip(go)));
    this.AddCheckboxLine(Db.Get().Amounts.Fertilization, this.conditionsContainerAdditional, (Func<GameObject, string>) (go => this.GetFertilizationLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go =>
    {
      if ((UnityEngine.Object) go.GetComponent<ReceptacleMonitor>() == (UnityEngine.Object) null)
        return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
      return go.GetComponent<ReceptacleMonitor>().Replanted ? MinionVitalsPanel.CheckboxLineDisplayType.Normal : MinionVitalsPanel.CheckboxLineDisplayType.Diminished;
    }), (Func<GameObject, bool>) (go => this.check_fertilizer(go)), (Func<GameObject, string>) (go => this.GetFertilizationTooltip(go)));
    this.AddCheckboxLine(Db.Get().Amounts.Irrigation, this.conditionsContainerAdditional, (Func<GameObject, string>) (go => this.GetIrrigationLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go =>
    {
      ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
      return !((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Replanted ? MinionVitalsPanel.CheckboxLineDisplayType.Diminished : MinionVitalsPanel.CheckboxLineDisplayType.Normal;
    }), (Func<GameObject, bool>) (go => this.check_irrigation(go)), (Func<GameObject, string>) (go => this.GetIrrigationTooltip(go)));
    this.AddCheckboxLine(Db.Get().Amounts.Illumination, this.conditionsContainerNormal, (Func<GameObject, string>) (go => this.GetIlluminationLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go => MinionVitalsPanel.CheckboxLineDisplayType.Normal), (Func<GameObject, bool>) (go => this.check_illumination(go)), (Func<GameObject, string>) (go => this.GetIlluminationTooltip(go)));
    this.AddCheckboxLine((Amount) null, this.conditionsContainerNormal, (Func<GameObject, string>) (go => this.GetRadiationLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go =>
    {
      AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
      return attributeInstance != null && (double) attributeInstance.GetTotalValue() > 0.0 ? MinionVitalsPanel.CheckboxLineDisplayType.Normal : MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
    }), (Func<GameObject, bool>) (go => this.check_radiation(go)), (Func<GameObject, string>) (go => this.GetRadiationTooltip(go)));
    this.AddCheckboxLine((Amount) null, this.conditionsContainerNormal, (Func<GameObject, string>) (go => this.GetEntityConsumptionLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go => go.GetComponent<IPlantConsumeEntities>() != null ? MinionVitalsPanel.CheckboxLineDisplayType.Normal : MinionVitalsPanel.CheckboxLineDisplayType.Hidden), (Func<GameObject, bool>) (go => this.check_entity_consumed(go)), (Func<GameObject, string>) (go => this.GetEntityConsumedTooltip(go)));
    this.AddCheckboxLine((Amount) null, this.conditionsContainerNormal, (Func<GameObject, string>) (go => this.GetPollinationLabel(go)), (Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType>) (go => go.GetSMI<PollinationMonitor.StatesInstance>() == null ? MinionVitalsPanel.CheckboxLineDisplayType.Hidden : MinionVitalsPanel.CheckboxLineDisplayType.Normal), (Func<GameObject, bool>) (go => go.GetComponent<WiltCondition>().IsConditionSatisifed(WiltCondition.Condition.Pollination)), (Func<GameObject, string>) (go => this.GetPollinationTooltip(go)));
  }

  public string UnpollinatedTooltip
  {
    get
    {
      if (string.IsNullOrEmpty(this.unpollinatedTooltip))
      {
        StringBuilder sb = GlobalStringBuilderPool.Alloc();
        foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.Creatures.Pollinator))
        {
          KPrefabID component = go.GetComponent<KPrefabID>();
          if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) component))
            sb.AppendFormat("\n{0}{1}", (object) "    • ", (object) go.GetProperName());
        }
        this.unpollinatedTooltip = string.Format((string) STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_UNPOLLINATED, (object) GlobalStringBuilderPool.ReturnAndFree(sb));
      }
      return this.unpollinatedTooltip;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Init();
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    SimAndRenderScheduler.instance.Add((object) this);
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    SimAndRenderScheduler.instance.Remove((object) this);
  }

  private void AddAmountLine(Amount amount, Func<AmountInstance, string> tooltip_func = null)
  {
    GameObject gameObject = Util.KInstantiateUI(this.LineItemPrefab, this.Content.gameObject);
    gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
    gameObject.SetActive(true);
    MinionVitalsPanel.AmountLine amountLine = new MinionVitalsPanel.AmountLine();
    amountLine.amount = amount;
    amountLine.go = gameObject;
    amountLine.locText = gameObject.GetComponentInChildren<LocText>();
    amountLine.toolTip = gameObject.GetComponentInChildren<ToolTip>();
    amountLine.imageToggle = gameObject.GetComponentInChildren<ValueTrendImageToggle>();
    amountLine.toolTipFunc = tooltip_func != null ? tooltip_func : new Func<AmountInstance, string>(amount.GetTooltip);
    if (!amount.CanDisplayerDisplayCustomIcons)
      gameObject.GetComponentInChildren<Image>().sprite = Assets.GetSprite((HashedString) amount.uiSprite);
    else
      amountLine.spriteFunc = new Func<AmountInstance, Sprite>(amount.GetSprite);
    this.amountsLines.Add(amountLine);
  }

  private void AddAttributeLine(Klei.AI.Attribute attribute, Func<AttributeInstance, string> tooltip_func = null)
  {
    GameObject gameObject = Util.KInstantiateUI(this.LineItemPrefab, this.Content.gameObject);
    gameObject.GetComponentInChildren<Image>().sprite = Assets.GetSprite((HashedString) attribute.uiSprite);
    gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
    gameObject.SetActive(true);
    MinionVitalsPanel.AttributeLine attributeLine = new MinionVitalsPanel.AttributeLine();
    attributeLine.attribute = attribute;
    attributeLine.go = gameObject;
    attributeLine.locText = gameObject.GetComponentInChildren<LocText>();
    attributeLine.toolTip = gameObject.GetComponentInChildren<ToolTip>();
    gameObject.GetComponentInChildren<ValueTrendImageToggle>().gameObject.SetActive(false);
    attributeLine.toolTipFunc = tooltip_func != null ? tooltip_func : new Func<AttributeInstance, string>(attribute.GetTooltip);
    this.attributesLines.Add(attributeLine);
  }

  private void AddCheckboxLine(
    Amount amount,
    Transform parentContainer,
    Func<GameObject, string> label_text_func,
    Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType> display_condition,
    Func<GameObject, bool> checkbox_value_func,
    Func<GameObject, string> tooltip_func = null)
  {
    GameObject gameObject = Util.KInstantiateUI(this.CheckboxLinePrefab, this.Content.gameObject);
    HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
    gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
    gameObject.SetActive(true);
    MinionVitalsPanel.CheckboxLine checkboxLine = new MinionVitalsPanel.CheckboxLine();
    checkboxLine.go = gameObject;
    checkboxLine.parentContainer = parentContainer;
    checkboxLine.amount = amount;
    checkboxLine.locText = component.GetReference("Label") as LocText;
    checkboxLine.get_value = checkbox_value_func;
    checkboxLine.display_condition = display_condition;
    checkboxLine.label_text_func = label_text_func;
    checkboxLine.go.name = "Checkbox_";
    if (amount != null)
      checkboxLine.go.name += amount.Name;
    else
      checkboxLine.go.name += "Unnamed";
    if (tooltip_func != null)
    {
      checkboxLine.tooltip = tooltip_func;
      ToolTip tt = checkboxLine.go.GetComponent<ToolTip>();
      tt.refreshWhileHovering = true;
      tt.OnToolTip = (Func<string>) (() =>
      {
        tt.ClearMultiStringTooltip();
        tt.AddMultiStringTooltip(tooltip_func(this.lastSelectedEntity), (TextStyleSetting) null);
        return "";
      });
    }
    this.checkboxLines.Add(checkboxLine);
  }

  private void ShouldShowVitalsPanel(GameObject selectedEntity)
  {
  }

  public void Refresh(GameObject selectedEntity)
  {
    if ((UnityEngine.Object) selectedEntity == (UnityEngine.Object) null || (UnityEngine.Object) selectedEntity.gameObject == (UnityEngine.Object) null)
      return;
    this.lastSelectedEntity = selectedEntity;
    WiltCondition component1 = selectedEntity.GetComponent<WiltCondition>();
    MinionIdentity component2 = selectedEntity.GetComponent<MinionIdentity>();
    CreatureBrain component3 = selectedEntity.GetComponent<CreatureBrain>();
    IncubationMonitor.Instance smi = selectedEntity.GetSMI<IncubationMonitor.Instance>();
    object[] objArray = new object[4]
    {
      (object) component1,
      (object) component2,
      (object) component3,
      (object) smi
    };
    bool flag1 = false;
    for (int index = 0; index < objArray.Length; ++index)
    {
      if (objArray[index] != null)
      {
        flag1 = true;
        break;
      }
    }
    if (!flag1)
    {
      this.SetActive(false);
    }
    else
    {
      this.SetActive(true);
      this.SetTitle((string) ((UnityEngine.Object) component1 == (UnityEngine.Object) null ? STRINGS.UI.DETAILTABS.SIMPLEINFO.GROUPNAME_CONDITION : STRINGS.UI.DETAILTABS.SIMPLEINFO.GROUPNAME_REQUIREMENTS));
      Amounts amounts = selectedEntity.GetAmounts();
      Attributes attributes = selectedEntity.GetAttributes();
      if (amounts == null || attributes == null)
        return;
      if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      {
        this.conditionsContainerNormal.gameObject.SetActive(false);
        this.conditionsContainerAdditional.gameObject.SetActive(false);
        foreach (MinionVitalsPanel.AmountLine amountsLine in this.amountsLines)
        {
          bool flag2 = amountsLine.TryUpdate(amounts);
          if (amountsLine.go.activeSelf != flag2)
            amountsLine.go.SetActive(flag2);
        }
        foreach (MinionVitalsPanel.AttributeLine attributesLine in this.attributesLines)
        {
          bool flag3 = attributesLine.TryUpdate(attributes);
          if (attributesLine.go.activeSelf != flag3)
            attributesLine.go.SetActive(flag3);
        }
      }
      bool flag4 = false;
      for (int index = 0; index < this.checkboxLines.Count; ++index)
      {
        MinionVitalsPanel.CheckboxLine checkboxLine = this.checkboxLines[index];
        MinionVitalsPanel.CheckboxLineDisplayType checkboxLineDisplayType = MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
        if (this.checkboxLines[index].amount != null)
        {
          for (int idx = 0; idx < amounts.Count; ++idx)
          {
            AmountInstance amountInstance = amounts[idx];
            if (checkboxLine.amount == amountInstance.amount)
            {
              checkboxLineDisplayType = checkboxLine.display_condition(selectedEntity.gameObject);
              break;
            }
          }
        }
        else
          checkboxLineDisplayType = checkboxLine.display_condition(selectedEntity.gameObject);
        if (checkboxLineDisplayType != MinionVitalsPanel.CheckboxLineDisplayType.Hidden)
        {
          checkboxLine.locText.SetText(checkboxLine.label_text_func(selectedEntity.gameObject));
          if (!checkboxLine.go.activeSelf)
            checkboxLine.go.SetActive(true);
          GameObject gameObject = checkboxLine.go.GetComponent<HierarchyReferences>().GetReference("Check").gameObject;
          gameObject.SetActive(checkboxLine.get_value(selectedEntity.gameObject));
          if ((UnityEngine.Object) checkboxLine.go.transform.parent != (UnityEngine.Object) checkboxLine.parentContainer)
          {
            checkboxLine.go.transform.SetParent(checkboxLine.parentContainer);
            checkboxLine.go.transform.localScale = Vector3.one;
          }
          if ((UnityEngine.Object) checkboxLine.parentContainer == (UnityEngine.Object) this.conditionsContainerAdditional)
            flag4 = true;
          if (checkboxLineDisplayType == MinionVitalsPanel.CheckboxLineDisplayType.Normal)
          {
            if (checkboxLine.get_value(selectedEntity.gameObject))
            {
              checkboxLine.locText.color = Color.black;
              gameObject.transform.parent.GetComponent<Image>().color = Color.black;
            }
            else
            {
              Color color = new Color(0.992156863f, 0.0f, 0.101960786f);
              checkboxLine.locText.color = color;
              gameObject.transform.parent.GetComponent<Image>().color = color;
            }
          }
          else
          {
            checkboxLine.locText.color = Color.grey;
            gameObject.transform.parent.GetComponent<Image>().color = Color.grey;
          }
        }
        else if (checkboxLine.go.activeSelf)
          checkboxLine.go.SetActive(false);
      }
      if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null))
        return;
      IManageGrowingStates manageGrowingStates = component1.GetComponent<IManageGrowingStates>() ?? component1.GetSMI<IManageGrowingStates>();
      bool flag5 = component1.HasTag(GameTags.Decoration);
      this.conditionsContainerNormal.gameObject.SetActive(true);
      this.conditionsContainerAdditional.gameObject.SetActive(!flag5);
      if (manageGrowingStates == null)
      {
        float num = 1f;
        LocText reference1 = this.conditionsContainerNormal.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
        reference1.text = "";
        reference1.text = flag5 ? string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_DECOR.BASE) : string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_INSTANT.BASE, (object) Util.FormatTwoDecimalPlace((float) ((double) num * 0.25 * 100.0)));
        reference1.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_INSTANT.TOOLTIP));
        LocText reference2 = this.conditionsContainerAdditional.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
        ReceptacleMonitor component4 = selectedEntity.GetComponent<ReceptacleMonitor>();
        reference2.color = (UnityEngine.Object) component4 == (UnityEngine.Object) null || component4.Replanted ? Color.black : Color.grey;
        reference2.text = string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC_INSTANT.BASE, (object) Util.FormatTwoDecimalPlace(num * 100f));
        reference2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC_INSTANT.TOOLTIP));
      }
      else
      {
        LocText reference3 = this.conditionsContainerNormal.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
        reference3.text = "";
        reference3.text = string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.WILD.BASE, (object) GameUtil.GetFormattedCycles(manageGrowingStates.WildGrowthTime()));
        reference3.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.WILD.TOOLTIP, (object) GameUtil.GetFormattedCycles(manageGrowingStates.WildGrowthTime())));
        LocText reference4 = this.conditionsContainerAdditional.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
        reference4.color = manageGrowingStates.IsWildPlanted() ? Color.grey : Color.black;
        reference4.text = "";
        reference4.text = flag4 ? string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC.BASE, (object) GameUtil.GetFormattedCycles(manageGrowingStates.DomesticGrowthTime())) : string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.DOMESTIC.BASE, (object) GameUtil.GetFormattedCycles(manageGrowingStates.DomesticGrowthTime()));
        reference4.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) STRINGS.UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC.TOOLTIP, (object) GameUtil.GetFormattedCycles(manageGrowingStates.DomesticGrowthTime())));
      }
      foreach (MinionVitalsPanel.AmountLine amountsLine in this.amountsLines)
        amountsLine.go.SetActive(false);
      foreach (MinionVitalsPanel.AttributeLine attributesLine in this.attributesLines)
        attributesLine.go.SetActive(false);
    }
  }

  private string GetAirPressureTooltip(GameObject go)
  {
    PressureVulnerable component = go.GetComponent<PressureVulnerable>();
    return (UnityEngine.Object) component == (UnityEngine.Object) null ? "" : STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_PRESSURE.text.Replace("{pressure}", GameUtil.GetFormattedMass(component.GetExternalPressure()));
  }

  private string GetInternalTemperatureTooltip(GameObject go)
  {
    TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
    return (UnityEngine.Object) component == (UnityEngine.Object) null ? "" : STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_TEMPERATURE.text.Replace("{temperature}", GameUtil.GetFormattedTemperature(component.InternalTemperature));
  }

  private string GetFertilizationTooltip(GameObject go)
  {
    FertilizationMonitor.Instance smi = go.GetSMI<FertilizationMonitor.Instance>();
    return smi == null ? "" : STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_FERTILIZER.text.Replace("{mass}", GameUtil.GetFormattedMass(smi.total_fertilizer_available));
  }

  private string GetIrrigationTooltip(GameObject go)
  {
    IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
    return smi == null ? "" : STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_IRRIGATION.text.Replace("{mass}", GameUtil.GetFormattedMass(smi.total_fertilizer_available));
  }

  private string GetIlluminationTooltip(GameObject go)
  {
    IIlluminationTracker illuminationTracker = go.GetComponent<IIlluminationTracker>() ?? go.GetSMI<IIlluminationTracker>();
    return illuminationTracker == null ? "" : illuminationTracker.GetIlluminationUITooltip();
  }

  private string GetRadiationTooltip(GameObject go)
  {
    int cell = Grid.PosToCell(go);
    float rads = Grid.IsValidCell(cell) ? Grid.Radiation[cell] : 0.0f;
    AttributeInstance attributeInstance1 = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
    AttributeInstance attributeInstance2 = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
    MutantPlant component = go.GetComponent<MutantPlant>();
    int num = !((UnityEngine.Object) component != (UnityEngine.Object) null) ? 0 : (component.IsOriginal ? 1 : 0);
    string radiationTooltip = (double) attributeInstance1.GetTotalValue() != 0.0 ? STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_RADIATION.Replace("{rads}", GameUtil.GetFormattedRads(rads)).Replace("{minRads}", attributeInstance1.GetFormattedValue()).Replace("{maxRads}", attributeInstance2.GetFormattedValue()) : STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_RADIATION_NO_MIN.Replace("{rads}", GameUtil.GetFormattedRads(rads)).Replace("{maxRads}", attributeInstance2.GetFormattedValue());
    if (num != 0)
      radiationTooltip += (string) STRINGS.UI.GAMEOBJECTEFFECTS.TOOLTIPS.MUTANT_SEED_TOOLTIP;
    return radiationTooltip;
  }

  private string GetReceptacleTooltip(GameObject go)
  {
    ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return "";
    return component.HasOperationalReceptacle() ? (string) STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_RECEPTACLE_OPERATIONAL : (string) STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_RECEPTACLE_INOPERATIONAL;
  }

  private string GetEntityConsumedTooltip(GameObject go)
  {
    IPlantConsumeEntities component = go.GetComponent<IPlantConsumeEntities>();
    component.AreEntitiesConsumptionRequirementsSatisfied();
    return GameUtil.SafeStringFormat((string) STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_ENTITY_CONSUMER_REQUIREMENTS, (object) component.GetConsumableEntitiesCategoryName());
  }

  private string GetPollinationTooltip(GameObject go)
  {
    return !go.GetComponent<WiltCondition>().IsConditionSatisifed(WiltCondition.Condition.Pollination) ? this.UnpollinatedTooltip : (string) STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_POLLINATED;
  }

  private string GetAtmosphereTooltip(GameObject go)
  {
    PressureVulnerable component = go.GetComponent<PressureVulnerable>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.currentAtmoElement != null ? STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_ATMOSPHERE.text.Replace("{element}", component.currentAtmoElement.name) : (string) STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_ATMOSPHERE;
  }

  private string GetAirPressureLabel(GameObject go)
  {
    PressureVulnerable component = go.GetComponent<PressureVulnerable>();
    return $"{Db.Get().Amounts.AirPressure.Name}\n    • {GameUtil.GetFormattedMass(component.pressureWarning_Low, massFormat: GameUtil.MetricMassFormat.Gram, includeSuffix: false)} - {GameUtil.GetFormattedMass(component.pressureWarning_High, massFormat: GameUtil.MetricMassFormat.Gram)}";
  }

  private string GetInternalTemperatureLabel(GameObject go)
  {
    TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
    return $"{Db.Get().Amounts.Temperature.Name}\n    • {GameUtil.GetFormattedTemperature(component.TemperatureWarningLow, displayUnits: false)} - {GameUtil.GetFormattedTemperature(component.TemperatureWarningHigh)}";
  }

  private string GetFertilizationLabel(GameObject go)
  {
    FertilizationMonitor.Instance smi = go.GetSMI<FertilizationMonitor.Instance>();
    string fertilizationLabel = Db.Get().Amounts.Fertilization.Name;
    float totalValue = go.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
    foreach (PlantElementAbsorber.ConsumeInfo consumedElement in smi.def.consumedElements)
      fertilizationLabel = $"{fertilizationLabel}\n    • {ElementLoader.GetElement(consumedElement.tag).name} {GameUtil.GetFormattedMass(consumedElement.massConsumptionRate * totalValue, GameUtil.TimeSlice.PerCycle)}";
    return fertilizationLabel;
  }

  private string GetIrrigationLabel(GameObject go)
  {
    IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
    string irrigationLabel = Db.Get().Amounts.Irrigation.Name;
    float totalValue = go.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
    foreach (PlantElementAbsorber.ConsumeInfo consumedElement in smi.def.consumedElements)
      irrigationLabel = $"{irrigationLabel}\n    • {ElementLoader.GetElement(consumedElement.tag).name}: {GameUtil.GetFormattedMass(consumedElement.massConsumptionRate * totalValue, GameUtil.TimeSlice.PerCycle)}";
    return irrigationLabel;
  }

  private string GetIlluminationLabel(GameObject go)
  {
    return (go.GetComponent<IIlluminationTracker>() ?? go.GetSMI<IIlluminationTracker>()).GetIlluminationUILabel();
  }

  private string GetEntityConsumptionLabel(GameObject go)
  {
    IPlantConsumeEntities component = go.GetComponent<IPlantConsumeEntities>();
    string requirementText = component.GetRequirementText();
    string str;
    if (!this.check_entity_consumed(go))
      str = (string) STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_ENTITY_CONSUMER_UNSATISFIED;
    else
      str = GameUtil.SafeStringFormat((string) STRINGS.UI.TOOLTIPS.VITALS_CHECKBOX_ENTITY_CONSUMER_SATISFIED, (object) component.GetConsumedEntityName());
    return $"{requirementText}\n    • {str}";
  }

  private string GetPollinationLabel(GameObject go) => (string) STRINGS.UI.VITALSSCREEN.POLLINATION;

  private string GetAtmosphereLabel(GameObject go)
  {
    PressureVulnerable component = go.GetComponent<PressureVulnerable>();
    string atmosphereLabel = (string) STRINGS.UI.VITALSSCREEN.ATMOSPHERE_CONDITION;
    foreach (Element safeAtmosphere in component.safe_atmospheres)
      atmosphereLabel = $"{atmosphereLabel}\n    • {safeAtmosphere.name}";
    return atmosphereLabel;
  }

  private string GetRadiationLabel(GameObject go)
  {
    AttributeInstance attributeInstance1 = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
    AttributeInstance attributeInstance2 = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
    return (double) attributeInstance1.GetTotalValue() == 0.0 ? $"{(string) STRINGS.UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION}\n    • {STRINGS.UI.GAMEOBJECTEFFECTS.AMBIENT_NO_MIN_RADIATION_FMT.Replace("{maxRads}", attributeInstance2.GetFormattedValue())}" : $"{(string) STRINGS.UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION}\n    • {STRINGS.UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION_FMT.Replace("{minRads}", attributeInstance1.GetFormattedValue()).Replace("{maxRads}", attributeInstance2.GetFormattedValue())}";
  }

  private bool check_pressure(GameObject go)
  {
    PressureVulnerable component = go.GetComponent<PressureVulnerable>();
    return !((UnityEngine.Object) component != (UnityEngine.Object) null) || component.ExternalPressureState == PressureVulnerable.PressureState.Normal;
  }

  private bool check_temperature(GameObject go)
  {
    TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
    return !((UnityEngine.Object) component != (UnityEngine.Object) null) || component.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.Normal;
  }

  private bool check_irrigation(GameObject go)
  {
    IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
    if (smi == null)
      return true;
    return !smi.IsInsideState((StateMachine.BaseState) smi.sm.replanted.starved) && !smi.IsInsideState((StateMachine.BaseState) smi.sm.wild);
  }

  private bool check_illumination(GameObject go)
  {
    IIlluminationTracker illuminationTracker = go.GetComponent<IIlluminationTracker>() ?? go.GetSMI<IIlluminationTracker>();
    return illuminationTracker == null || illuminationTracker.ShouldIlluminationUICheckboxBeChecked();
  }

  private bool check_radiation(GameObject go)
  {
    AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
    if (attributeInstance == null || (double) attributeInstance.GetTotalValue() == 0.0)
      return true;
    int cell = Grid.PosToCell(go);
    return (Grid.IsValidCell(cell) ? (double) Grid.Radiation[cell] : 0.0) >= (double) attributeInstance.GetTotalValue();
  }

  private bool check_receptacle(GameObject go)
  {
    ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
    return !((UnityEngine.Object) component == (UnityEngine.Object) null) && component.HasOperationalReceptacle();
  }

  private bool check_fertilizer(GameObject go)
  {
    FertilizationMonitor.Instance smi = go.GetSMI<FertilizationMonitor.Instance>();
    return smi != null && smi.sm.isFertilized.Get(smi);
  }

  private bool check_atmosphere(GameObject go)
  {
    PressureVulnerable component = go.GetComponent<PressureVulnerable>();
    return !((UnityEngine.Object) component != (UnityEngine.Object) null) || component.testAreaElementSafe;
  }

  private bool check_entity_consumed(GameObject go)
  {
    return go.GetComponent<IPlantConsumeEntities>().AreEntitiesConsumptionRequirementsSatisfied();
  }

  [DebuggerDisplay("{amount.Name}")]
  public struct AmountLine
  {
    public Amount amount;
    public GameObject go;
    public ValueTrendImageToggle imageToggle;
    public LocText locText;
    public ToolTip toolTip;
    public Func<AmountInstance, string> toolTipFunc;
    public Func<AmountInstance, Sprite> spriteFunc;

    public bool TryUpdate(Amounts amounts)
    {
      foreach (AmountInstance modifier in amounts.ModifierList)
      {
        if (this.amount == modifier.amount && !modifier.hide)
        {
          this.locText.SetText(this.amount.GetDescription(modifier));
          this.toolTip.toolTip = this.toolTipFunc(modifier);
          this.imageToggle.SetValue(modifier);
          Sprite sprite = this.spriteFunc == null ? (Sprite) null : this.spriteFunc(modifier);
          if ((UnityEngine.Object) sprite != (UnityEngine.Object) null)
            this.go.GetComponentInChildren<Image>().sprite = sprite;
          return true;
        }
      }
      return false;
    }
  }

  [DebuggerDisplay("{attribute.Name}")]
  public struct AttributeLine
  {
    public Klei.AI.Attribute attribute;
    public GameObject go;
    public LocText locText;
    public ToolTip toolTip;
    public Func<AttributeInstance, string> toolTipFunc;

    public bool TryUpdate(Attributes attributes)
    {
      foreach (AttributeInstance attribute in attributes)
      {
        if (this.attribute == attribute.modifier && !attribute.hide)
        {
          this.locText.SetText(this.attribute.GetDescription(attribute));
          this.toolTip.toolTip = this.toolTipFunc(attribute);
          return true;
        }
      }
      return false;
    }
  }

  public struct CheckboxLine
  {
    public Amount amount;
    public GameObject go;
    public LocText locText;
    public Func<GameObject, string> tooltip;
    public Func<GameObject, bool> get_value;
    public Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType> display_condition;
    public Func<GameObject, string> label_text_func;
    public Transform parentContainer;
  }

  public enum CheckboxLineDisplayType
  {
    Normal,
    Diminished,
    Hidden,
  }
}
