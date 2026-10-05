// Decompiled with JetBrains decompiler
// Type: VitalsTableScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class VitalsTableScreen : TableScreen
{
  protected override void OnActivate()
  {
    this.has_default_duplicant_row = false;
    this.title = (string) UI.VITALS;
    base.OnActivate();
    this.AddPortraitColumn("Portrait", new Action<IAssignableIdentity, GameObject>(((TableScreen) this).on_load_portrait), (Comparison<IAssignableIdentity>) null);
    this.AddButtonLabelColumn("Names", new Action<IAssignableIdentity, GameObject>(((TableScreen) this).on_load_name_label), new Func<IAssignableIdentity, GameObject, string>(((TableScreen) this).get_value_name_label), (Action<GameObject>) (widget_go => this.GetWidgetRow(widget_go).SelectMinion()), (Action<GameObject>) (widget_go => this.GetWidgetRow(widget_go).SelectAndFocusMinion()), new Comparison<IAssignableIdentity>(((TableScreen) this).compare_rows_alphabetical), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_name), new Action<IAssignableIdentity, GameObject, ToolTip>(((TableScreen) this).on_tooltip_sort_alphabetically));
    this.AddLabelColumn("Stress", new Action<IAssignableIdentity, GameObject>(this.on_load_stress), new Func<IAssignableIdentity, GameObject, string>(this.get_value_stress_label), new Comparison<IAssignableIdentity>(this.compare_rows_stress), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_stress), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_stress), 64 /*0x40*/, true);
    this.AddLabelColumn("QOLExpectations", new Action<IAssignableIdentity, GameObject>(this.on_load_qualityoflife_expectations), new Func<IAssignableIdentity, GameObject, string>(this.get_value_qualityoflife_expectations_label), new Comparison<IAssignableIdentity>(this.compare_rows_qualityoflife_expectations), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_qualityoflife_expectations), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_qualityoflife_expectations), 64 /*0x40*/, true);
    if (Game.IsDlcActiveForCurrentSave("DLC3_ID"))
      this.AddLabelColumn("PowerBanks", new Action<IAssignableIdentity, GameObject>(this.on_load_power_banks), new Func<IAssignableIdentity, GameObject, string>(this.get_value_power_banks_label), new Comparison<IAssignableIdentity>(this.compare_rows_power_banks), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_power_banks), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_power_banks), 64 /*0x40*/, true);
    this.AddLabelColumn("Fullness", new Action<IAssignableIdentity, GameObject>(this.on_load_fullness), new Func<IAssignableIdentity, GameObject, string>(this.get_value_fullness_label), new Comparison<IAssignableIdentity>(this.compare_rows_fullness), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_fullness), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_fullness), 96 /*0x60*/, true);
    this.AddLabelColumn("Health", new Action<IAssignableIdentity, GameObject>(this.on_load_health), new Func<IAssignableIdentity, GameObject, string>(this.get_value_health_label), new Comparison<IAssignableIdentity>(this.compare_rows_health), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_health), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_health), 64 /*0x40*/, true);
    this.AddLabelColumn("Immunity", new Action<IAssignableIdentity, GameObject>(this.on_load_sickness), new Func<IAssignableIdentity, GameObject, string>(this.get_value_sickness_label), new Comparison<IAssignableIdentity>(this.compare_rows_sicknesses), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sicknesses), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_sicknesses), 192 /*0xC0*/, true);
  }

  private void on_load_stress(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
    if (minion != null)
      componentInChildren.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
    else
      componentInChildren.text = widgetRow.isDefault ? "" : UI.VITALSSCREEN.STRESS.ToString();
  }

  private string get_value_stress_label(IAssignableIdentity identity, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if (widgetRow.rowType == TableRow.RowType.Minion)
    {
      MinionIdentity cmp = identity as MinionIdentity;
      if ((UnityEngine.Object) cmp != (UnityEngine.Object) null)
        return Db.Get().Amounts.Stress.Lookup((Component) cmp).GetValueString();
    }
    else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
      return (string) UI.TABLESCREENS.NA;
    return "";
  }

  private int compare_rows_stress(IAssignableIdentity a, IAssignableIdentity b)
  {
    MinionIdentity cmp1 = a as MinionIdentity;
    MinionIdentity cmp2 = b as MinionIdentity;
    if ((UnityEngine.Object) cmp1 == (UnityEngine.Object) null && (UnityEngine.Object) cmp2 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) cmp1 == (UnityEngine.Object) null)
      return -1;
    if ((UnityEngine.Object) cmp2 == (UnityEngine.Object) null)
      return 1;
    float num = Db.Get().Amounts.Stress.Lookup((Component) cmp1).value;
    return Db.Get().Amounts.Stress.Lookup((Component) cmp2).value.CompareTo(num);
  }

  protected void on_tooltip_stress(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        MinionIdentity cmp = minion as MinionIdentity;
        if (!((UnityEngine.Object) cmp != (UnityEngine.Object) null))
          break;
        tooltip.AddMultiStringTooltip(Db.Get().Amounts.Stress.Lookup((Component) cmp).GetTooltip(), (TextStyleSetting) null);
        break;
      case TableRow.RowType.StoredMinon:
        this.StoredMinionTooltip(minion, tooltip);
        break;
    }
  }

  protected void on_tooltip_sort_stress(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) UI.TABLESCREENS.COLUMN_SORT_BY_STRESS, (TextStyleSetting) null);
        break;
    }
  }

  private void on_load_qualityoflife_expectations(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
    if (minion != null)
      componentInChildren.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
    else
      componentInChildren.text = widgetRow.isDefault ? "" : UI.VITALSSCREEN.QUALITYOFLIFE_EXPECTATIONS.ToString();
  }

  private string get_value_qualityoflife_expectations_label(
    IAssignableIdentity identity,
    GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if (widgetRow.rowType == TableRow.RowType.Minion)
    {
      MinionIdentity cmp = identity as MinionIdentity;
      if ((UnityEngine.Object) cmp != (UnityEngine.Object) null)
        return Db.Get().Attributes.QualityOfLife.Lookup((Component) cmp).GetFormattedValue();
    }
    else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
      return (string) UI.TABLESCREENS.NA;
    return "";
  }

  private int compare_rows_qualityoflife_expectations(IAssignableIdentity a, IAssignableIdentity b)
  {
    MinionIdentity cmp1 = a as MinionIdentity;
    MinionIdentity cmp2 = b as MinionIdentity;
    if ((UnityEngine.Object) cmp1 == (UnityEngine.Object) null && (UnityEngine.Object) cmp2 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) cmp1 == (UnityEngine.Object) null)
      return -1;
    return (UnityEngine.Object) cmp2 == (UnityEngine.Object) null ? 1 : Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) cmp1).GetTotalValue().CompareTo(Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) cmp2).GetTotalValue());
  }

  protected void on_tooltip_qualityoflife_expectations(
    IAssignableIdentity identity,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        MinionIdentity cmp = identity as MinionIdentity;
        if (!((UnityEngine.Object) cmp != (UnityEngine.Object) null))
          break;
        tooltip.AddMultiStringTooltip(Db.Get().Attributes.QualityOfLife.Lookup((Component) cmp).GetAttributeValueTooltip(), (TextStyleSetting) null);
        break;
      case TableRow.RowType.StoredMinon:
        this.StoredMinionTooltip(identity, tooltip);
        break;
    }
  }

  protected void on_tooltip_sort_qualityoflife_expectations(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) UI.TABLESCREENS.COLUMN_SORT_BY_EXPECTATIONS, (TextStyleSetting) null);
        break;
    }
  }

  private void on_load_health(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
    if (minion != null)
      componentInChildren.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
    else
      componentInChildren.text = widgetRow.isDefault ? "" : (componentInChildren.text = UI.VITALSSCREEN_HEALTH.ToString());
  }

  private string get_value_health_label(IAssignableIdentity minion, GameObject widget_go)
  {
    if (minion != null)
    {
      TableRow widgetRow = this.GetWidgetRow(widget_go);
      if (widgetRow.rowType == TableRow.RowType.Minion && (UnityEngine.Object) (minion as MinionIdentity) != (UnityEngine.Object) null)
        return Db.Get().Amounts.HitPoints.Lookup((Component) (minion as MinionIdentity)).GetValueString();
      if (widgetRow.rowType == TableRow.RowType.StoredMinon)
        return (string) UI.TABLESCREENS.NA;
    }
    return "";
  }

  private int compare_rows_health(IAssignableIdentity a, IAssignableIdentity b)
  {
    MinionIdentity cmp1 = a as MinionIdentity;
    MinionIdentity cmp2 = b as MinionIdentity;
    if ((UnityEngine.Object) cmp1 == (UnityEngine.Object) null && (UnityEngine.Object) cmp2 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) cmp1 == (UnityEngine.Object) null)
      return -1;
    if ((UnityEngine.Object) cmp2 == (UnityEngine.Object) null)
      return 1;
    float num = Db.Get().Amounts.HitPoints.Lookup((Component) cmp1).value;
    return Db.Get().Amounts.HitPoints.Lookup((Component) cmp2).value.CompareTo(num);
  }

  protected void on_tooltip_health(
    IAssignableIdentity identity,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        MinionIdentity cmp = identity as MinionIdentity;
        if (!((UnityEngine.Object) cmp != (UnityEngine.Object) null))
          break;
        tooltip.AddMultiStringTooltip(Db.Get().Amounts.HitPoints.Lookup((Component) cmp).GetTooltip(), (TextStyleSetting) null);
        break;
      case TableRow.RowType.StoredMinon:
        this.StoredMinionTooltip(identity, tooltip);
        break;
    }
  }

  protected void on_tooltip_sort_health(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) UI.TABLESCREENS.COLUMN_SORT_BY_HITPOINTS, (TextStyleSetting) null);
        break;
    }
  }

  private void on_load_sickness(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
    if (minion != null)
      componentInChildren.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
    else
      componentInChildren.text = widgetRow.isDefault ? "" : UI.VITALSSCREEN_SICKNESS.ToString();
  }

  private string get_value_sickness_label(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if (widgetRow.rowType == TableRow.RowType.Minion)
    {
      MinionIdentity cmp = minion as MinionIdentity;
      if ((UnityEngine.Object) cmp != (UnityEngine.Object) null)
      {
        List<KeyValuePair<string, float>> keyValuePairList = new List<KeyValuePair<string, float>>();
        foreach (SicknessInstance modifier in cmp.GetComponent<MinionModifiers>().sicknesses.ModifierList)
          keyValuePairList.Add(new KeyValuePair<string, float>(modifier.modifier.Name, modifier.GetInfectedTimeRemaining()));
        if (DlcManager.FeatureRadiationEnabled())
        {
          RadiationMonitor.Instance smi = cmp.GetSMI<RadiationMonitor.Instance>();
          if (smi != null && smi.sm.isSick.Get(smi))
          {
            Effects component = cmp.GetComponent<Effects>();
            string key = component.HasEffect(RadiationMonitor.minorSicknessEffect) || component.HasEffect(RadiationMonitor.bionic_minorSicknessEffect) ? Db.Get().effects.Get(RadiationMonitor.minorSicknessEffect).Name : (component.HasEffect(RadiationMonitor.majorSicknessEffect) || component.HasEffect(RadiationMonitor.bionic_majorSicknessEffect) ? Db.Get().effects.Get(RadiationMonitor.majorSicknessEffect).Name : (component.HasEffect(RadiationMonitor.extremeSicknessEffect) || component.HasEffect(RadiationMonitor.bionic_extremeSicknessEffect) ? Db.Get().effects.Get(RadiationMonitor.extremeSicknessEffect).Name : (string) DUPLICANTS.MODIFIERS.RADIATIONEXPOSUREDEADLY.NAME));
            keyValuePairList.Add(new KeyValuePair<string, float>(key, smi.SicknessSecondsRemaining()));
          }
        }
        if (keyValuePairList.Count <= 0)
          return (string) UI.VITALSSCREEN.NO_SICKNESSES;
        string valueSicknessLabel = "";
        if (keyValuePairList.Count > 1)
        {
          float seconds = 0.0f;
          foreach (KeyValuePair<string, float> keyValuePair in keyValuePairList)
            seconds = Mathf.Min(keyValuePair.Value);
          valueSicknessLabel += string.Format((string) UI.VITALSSCREEN.MULTIPLE_SICKNESSES, (object) GameUtil.GetFormattedCycles(seconds));
        }
        else
        {
          foreach (KeyValuePair<string, float> keyValuePair in keyValuePairList)
          {
            if (!string.IsNullOrEmpty(valueSicknessLabel))
              valueSicknessLabel += "\n";
            valueSicknessLabel += string.Format((string) UI.VITALSSCREEN.SICKNESS_REMAINING, (object) keyValuePair.Key, (object) GameUtil.GetFormattedCycles(keyValuePair.Value));
          }
        }
        return valueSicknessLabel;
      }
    }
    else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
      return (string) UI.TABLESCREENS.NA;
    return "";
  }

  private int compare_rows_sicknesses(IAssignableIdentity a, IAssignableIdentity b)
  {
    return 0.0f.CompareTo(0.0f);
  }

  protected void on_tooltip_sicknesses(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        MinionIdentity cmp = minion as MinionIdentity;
        if (!((UnityEngine.Object) cmp != (UnityEngine.Object) null))
          break;
        bool flag = false;
        List<KeyValuePair<string, float>> keyValuePairList = new List<KeyValuePair<string, float>>();
        if (DlcManager.FeatureRadiationEnabled())
        {
          RadiationMonitor.Instance smi = cmp.GetSMI<RadiationMonitor.Instance>();
          if (smi != null && smi.sm.isSick.Get(smi))
          {
            tooltip.AddMultiStringTooltip(smi.GetEffectStatusTooltip(), (TextStyleSetting) null);
            flag = true;
          }
        }
        Sicknesses sicknesses = cmp.GetComponent<MinionModifiers>().sicknesses;
        if (sicknesses.IsInfected())
        {
          flag = true;
          foreach (SicknessInstance modifier in sicknesses.ModifierList)
          {
            tooltip.AddMultiStringTooltip(UI.HORIZONTAL_RULE, (TextStyleSetting) null);
            tooltip.AddMultiStringTooltip(modifier.modifier.Name, (TextStyleSetting) null);
            StatusItem statusItem = modifier.GetStatusItem();
            tooltip.AddMultiStringTooltip(statusItem.GetTooltip((object) modifier.ExposureInfo), (TextStyleSetting) null);
          }
        }
        if (flag)
          break;
        tooltip.AddMultiStringTooltip((string) UI.VITALSSCREEN.NO_SICKNESSES, (TextStyleSetting) null);
        break;
      case TableRow.RowType.StoredMinon:
        this.StoredMinionTooltip(minion, tooltip);
        break;
    }
  }

  protected void on_tooltip_sort_sicknesses(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) UI.TABLESCREENS.COLUMN_SORT_BY_SICKNESSES, (TextStyleSetting) null);
        break;
    }
  }

  private void on_load_fullness(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
    if (minion != null)
      componentInChildren.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
    else
      componentInChildren.text = widgetRow.isDefault ? "" : UI.VITALSSCREEN_CALORIES.ToString();
  }

  private string get_value_fullness_label(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if (widgetRow.rowType == TableRow.RowType.Minion && (UnityEngine.Object) (minion as MinionIdentity) != (UnityEngine.Object) null)
    {
      AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup((Component) (minion as MinionIdentity));
      return amountInstance != null ? amountInstance.GetValueString() : (string) UI.TABLESCREENS.NA;
    }
    return widgetRow.rowType == TableRow.RowType.StoredMinon ? (string) UI.TABLESCREENS.NA : "";
  }

  private int compare_rows_fullness(IAssignableIdentity a, IAssignableIdentity b)
  {
    MinionIdentity cmp1 = a as MinionIdentity;
    MinionIdentity cmp2 = b as MinionIdentity;
    if ((UnityEngine.Object) cmp1 == (UnityEngine.Object) null && (UnityEngine.Object) cmp2 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) cmp1 == (UnityEngine.Object) null)
      return -1;
    if ((UnityEngine.Object) cmp2 == (UnityEngine.Object) null)
      return 1;
    AmountInstance amountInstance1 = Db.Get().Amounts.Calories.Lookup((Component) cmp1);
    AmountInstance amountInstance2 = Db.Get().Amounts.Calories.Lookup((Component) cmp2);
    if (amountInstance1 == null && amountInstance2 == null)
      return 0;
    if (amountInstance1 == null)
      return -1;
    if (amountInstance2 == null)
      return 1;
    float num = amountInstance1.value;
    return amountInstance2.value.CompareTo(num);
  }

  protected void on_tooltip_fullness(
    IAssignableIdentity identity,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        MinionIdentity minionIdentity = identity as MinionIdentity;
        if (!((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null))
          break;
        AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup((Component) minionIdentity);
        if (amountInstance == null)
          break;
        tooltip.AddMultiStringTooltip(amountInstance.GetTooltip(), (TextStyleSetting) null);
        tooltip.AddMultiStringTooltip("\n" + string.Format((string) UI.VITALSSCREEN.EATEN_TODAY_TOOLTIP, (object) GameUtil.GetFormattedCalories(VitalsTableScreen.RationsEatenToday(minionIdentity))), (TextStyleSetting) null);
        break;
      case TableRow.RowType.StoredMinon:
        this.StoredMinionTooltip(identity, tooltip);
        break;
    }
  }

  protected void on_tooltip_sort_fullness(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) UI.TABLESCREENS.COLUMN_SORT_BY_FULLNESS, (TextStyleSetting) null);
        break;
    }
  }

  protected void on_tooltip_name(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        if (minion == null)
          break;
        tooltip.AddMultiStringTooltip(string.Format((string) UI.TABLESCREENS.GOTO_DUPLICANT_BUTTON, (object) minion.GetProperName()), (TextStyleSetting) null);
        break;
    }
  }

  private void on_load_eaten_today(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
    if (minion != null)
      componentInChildren.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
    else
      componentInChildren.text = widgetRow.isDefault ? "" : UI.VITALSSCREEN_EATENTODAY.ToString();
  }

  private static float RationsEatenToday(MinionIdentity minion)
  {
    float num = 0.0f;
    if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
    {
      RationMonitor.Instance smi = minion.GetSMI<RationMonitor.Instance>();
      if (smi != null)
        num = smi.GetRationsAteToday();
    }
    return num;
  }

  private string get_value_eaten_today_label(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if (widgetRow.rowType == TableRow.RowType.Minion)
      return GameUtil.GetFormattedCalories(VitalsTableScreen.RationsEatenToday(minion as MinionIdentity));
    return widgetRow.rowType == TableRow.RowType.StoredMinon ? (string) UI.TABLESCREENS.NA : "";
  }

  private int compare_rows_eaten_today(IAssignableIdentity a, IAssignableIdentity b)
  {
    MinionIdentity minion1 = a as MinionIdentity;
    MinionIdentity minion2 = b as MinionIdentity;
    if ((UnityEngine.Object) minion1 == (UnityEngine.Object) null && (UnityEngine.Object) minion2 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) minion1 == (UnityEngine.Object) null)
      return -1;
    if ((UnityEngine.Object) minion2 == (UnityEngine.Object) null)
      return 1;
    float num = VitalsTableScreen.RationsEatenToday(minion1);
    return VitalsTableScreen.RationsEatenToday(minion2).CompareTo(num);
  }

  protected void on_tooltip_eaten_today(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        if (minion == null)
          break;
        float calories = VitalsTableScreen.RationsEatenToday(minion as MinionIdentity);
        tooltip.AddMultiStringTooltip(string.Format((string) UI.VITALSSCREEN.EATEN_TODAY_TOOLTIP, (object) GameUtil.GetFormattedCalories(calories)), (TextStyleSetting) null);
        break;
      case TableRow.RowType.StoredMinon:
        this.StoredMinionTooltip(minion, tooltip);
        break;
    }
  }

  protected void on_tooltip_sort_eaten_today(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) UI.TABLESCREENS.COLUMN_SORT_BY_EATEN_TODAY, (TextStyleSetting) null);
        break;
    }
  }

  private void on_load_power_banks(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
    if (minion != null)
      componentInChildren.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
    else
      componentInChildren.text = widgetRow.isDefault ? "" : UI.VITALSSCREEN_POWERBANKS.ToString();
  }

  private string get_value_power_banks_label(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if (widgetRow.rowType == TableRow.RowType.Minion)
    {
      MinionIdentity cmp = minion as MinionIdentity;
      return (UnityEngine.Object) cmp != (UnityEngine.Object) null && cmp.HasTag(GameTags.Minions.Models.Bionic) ? GameUtil.GetFormattedJoules(cmp.GetAmounts().Get(Db.Get().Amounts.BionicInternalBattery).value) : (string) UI.TABLESCREENS.NA;
    }
    return widgetRow.rowType == TableRow.RowType.StoredMinon ? (string) UI.TABLESCREENS.NA : "";
  }

  private int compare_rows_power_banks(IAssignableIdentity a, IAssignableIdentity b)
  {
    MinionIdentity cmp1 = a as MinionIdentity;
    MinionIdentity cmp2 = b as MinionIdentity;
    float num = !((UnityEngine.Object) cmp1 != (UnityEngine.Object) null) || !cmp1.HasTag(GameTags.Minions.Models.Bionic) ? -1f : cmp1.GetAmounts().Get(Db.Get().Amounts.BionicInternalBattery).value;
    return (!((UnityEngine.Object) cmp2 != (UnityEngine.Object) null) || !cmp2.HasTag(GameTags.Minions.Models.Bionic) ? -1f : cmp2.GetAmounts().Get(Db.Get().Amounts.BionicInternalBattery).value).CompareTo(num);
  }

  protected void on_tooltip_power_banks(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        MinionIdentity cmp = minion as MinionIdentity;
        if (!((UnityEngine.Object) cmp != (UnityEngine.Object) null) || !((UnityEngine.Object) cmp != (UnityEngine.Object) null) || !cmp.HasTag(GameTags.Minions.Models.Bionic))
          break;
        tooltip.SetSimpleTooltip(cmp.GetAmounts().Get(Db.Get().Amounts.BionicInternalBattery).GetDescription());
        break;
      case TableRow.RowType.StoredMinon:
        this.StoredMinionTooltip(minion, tooltip);
        break;
    }
  }

  protected void on_tooltip_sort_power_banks(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) UI.TABLESCREENS.COLUMN_SORT_BY_POWERBANKS, (TextStyleSetting) null);
        break;
    }
  }

  private void StoredMinionTooltip(IAssignableIdentity minion, ToolTip tooltip)
  {
    if (minion == null || !((UnityEngine.Object) (minion as StoredMinionIdentity) != (UnityEngine.Object) null))
      return;
    tooltip.AddMultiStringTooltip(string.Format((string) UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, (object) (minion as StoredMinionIdentity).GetStorageReason(), (object) minion.GetProperName()), (TextStyleSetting) null);
  }
}
