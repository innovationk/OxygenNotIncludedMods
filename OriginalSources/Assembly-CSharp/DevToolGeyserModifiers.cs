// Decompiled with JetBrains decompiler
// Type: DevToolGeyserModifiers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DevToolGeyserModifiers : DevTool
{
  private const string DEV_MODIFIER_ID = "DEV MODIFIER";
  private const string NO_SELECTED_STR = "No Geyser Selected";
  private int DevModifierID;
  private const float ITERATION_BAR_HEIGHT = 10f;
  private const float YEAR_BAR_HEIGHT = 10f;
  private const float BAR_SPACING = 2f;
  private const float CURRENT_TIME_PADDING = 2f;
  private const float CURRENT_TIME_LINE_WIDTH = 2f;
  private uint YEAR_ACTIVE_COLOR = DevToolGeyserModifiers.Color((byte) 220, (byte) 15, (byte) 65, (byte) 175);
  private uint YEAR_DORMANT_COLOR = DevToolGeyserModifiers.Color(byte.MaxValue, (byte) 0, (byte) 65, (byte) 60);
  private uint ITERATION_ERUPTION_COLOR = DevToolGeyserModifiers.Color((byte) 60, (byte) 80 /*0x50*/, byte.MaxValue, (byte) 200);
  private uint ITERATION_QUIET_COLOR = DevToolGeyserModifiers.Color((byte) 60, (byte) 80 /*0x50*/, byte.MaxValue, (byte) 80 /*0x50*/);
  private uint CURRENT_TIME_COLOR = DevToolGeyserModifiers.Color(byte.MaxValue, (byte) 0, (byte) 0, byte.MaxValue);
  private Vector4 MODFIED_VALUE_TEXT_COLOR = new Vector4(0.8f, 0.7f, 0.1f, 1f);
  private Vector4 COMMENT_COLOR = new Vector4(0.1f, 0.5f, 0.1f, 1f);
  private Vector4 SUBTITLE_SLEEP_COLOR = new Vector4(0.15f, 0.35f, 0.7f, 1f);
  private Vector4 SUBTITLE_OVERPRESSURE_COLOR = new Vector4(0.7f, 0.0f, 0.0f, 1f);
  private Vector4 SUBTITLE_ERUPTING_COLOR = new Vector4(1f, 0.7f, 0.0f, 1f);
  private Vector4 ALT_COLOR = new Vector4(0.5f, 0.5f, 0.5f, 1f);
  private List<bool> modificationListUnfold = new List<bool>();
  private GameObject lastSelectedGameObject;
  private Geyser selectedGeyser;
  private Geyser.GeyserModification dev_modification;
  private string[] modifiers_FormatedList_Titles = new string[9]
  {
    "Mass per cycle",
    "Temperature",
    "Max Pressure",
    "Iteration duration",
    "Iteration percentage",
    "Year duration",
    "Year percentage",
    "Using secondary element",
    "Secondary element"
  };
  private string[] modifiers_FormatedList = new string[9];
  private string[] modifiers_FormatedList_Tooltip = new string[9];
  private string[] AllSimHashesValues;
  private int modifierSelected = -1;
  private int modifierFormatting_ValuePadding = -1;

  private float GraphHeight => 26f;

  private void DrawGeyserVariable(
    string variableTitle,
    float currentValue,
    float modifier,
    string modifierFormating = "+0.##; -0.##; +0",
    string unit = "",
    string modifierUnit = "",
    float altValue = 0.0f,
    string altUnit = "")
  {
    ImGui.BulletText($"{variableTitle}: {currentValue.ToString()}{unit}");
    if ((double) modifier != 0.0)
    {
      ImGui.SameLine();
      ImGui.TextColored(this.MODFIED_VALUE_TEXT_COLOR, $"({modifier.ToString(modifierFormating)}{modifierUnit})");
    }
    if (altUnit.IsNullOrWhiteSpace())
      return;
    ImGui.SameLine();
    ImGui.TextColored(this.ALT_COLOR, $"({altValue.ToString()}{altUnit})");
  }

  public static uint Color(byte r, byte g, byte b, byte a)
  {
    return (uint) ((int) a << 24 | (int) b << 16 /*0x10*/ | (int) g << 8) | (uint) r;
  }

  private void DrawYearAndIterationsGraph(Geyser geyser)
  {
    Vector2 contentRegionMin = ImGui.GetWindowContentRegionMin();
    Vector2 contentRegionMax = ImGui.GetWindowContentRegionMax();
    float x = contentRegionMax.x - contentRegionMin.x;
    ImGui.Dummy(new Vector2(x, this.GraphHeight));
    if (!ImGui.IsItemVisible())
      return;
    Vector2 itemRectMin = ImGui.GetItemRectMin();
    Vector2 itemRectMax = ImGui.GetItemRectMax();
    contentRegionMin.x += ImGui.GetWindowPos().x;
    contentRegionMin.y += ImGui.GetWindowPos().y;
    contentRegionMax.x += ImGui.GetWindowPos().x;
    contentRegionMax.y += ImGui.GetWindowPos().y;
    Vector2 vector2_1 = contentRegionMin;
    Vector2 vector2_2 = contentRegionMax;
    vector2_1.y = itemRectMin.y;
    vector2_2.y = itemRectMax.y;
    float iterationLength = this.selectedGeyser.configuration.GetIterationLength();
    float iterationPercent = this.selectedGeyser.configuration.GetIterationPercent();
    float yearLength = this.selectedGeyser.configuration.GetYearLength();
    float yearPercent = this.selectedGeyser.configuration.GetYearPercent();
    Vector2 p_min1 = vector2_1;
    Vector2 p_max1 = vector2_2 with
    {
      x = vector2_1.x + x * yearPercent,
      y = p_min1.y + 10f
    };
    ImGui.GetForegroundDrawList().AddRectFilled(p_min1, p_max1, this.YEAR_ACTIVE_COLOR);
    p_min1.x = p_max1.x;
    p_max1.x = vector2_2.x;
    ImGui.GetForegroundDrawList().AddRectFilled(p_min1, p_max1, this.YEAR_DORMANT_COLOR);
    double f = (double) yearLength / (double) iterationLength;
    float num1 = iterationLength / yearLength;
    p_min1.y = p_max1.y + 2f;
    p_max1.y = p_min1.y + 10f;
    float num2 = (float) Mathf.FloorToInt(geyser.GetCurrentLifeTime() / yearLength) * yearLength % iterationLength / iterationLength;
    int num3 = Mathf.CeilToInt((float) f) + 1;
    ImDrawListPtr foregroundDrawList;
    for (int index = 0; index < num3; ++index)
    {
      float num4 = (float) ((double) vector2_1.x - (double) num1 * (double) num2 * (double) x + (double) num1 * (double) index * (double) x);
      p_min1.x = num4;
      p_max1.x = p_min1.x + iterationPercent * num1 * x;
      Vector2 p_min2 = p_min1;
      Vector2 p_max2 = p_max1;
      p_min2.x = Mathf.Clamp(p_min2.x, vector2_1.x, vector2_2.x);
      p_max2.x = Mathf.Clamp(p_max2.x, vector2_1.x, vector2_2.x);
      foregroundDrawList = ImGui.GetForegroundDrawList();
      foregroundDrawList.AddRectFilled(p_min2, p_max2, this.ITERATION_ERUPTION_COLOR);
      p_min1.x = p_max1.x;
      p_max1.x += (1f - iterationPercent) * num1 * x;
      p_min2 = p_min1;
      p_max2 = p_max1;
      p_min2.x = Mathf.Clamp(p_min2.x, vector2_1.x, vector2_2.x);
      p_max2.x = Mathf.Clamp(p_max2.x, vector2_1.x, vector2_2.x);
      foregroundDrawList = ImGui.GetForegroundDrawList();
      foregroundDrawList.AddRectFilled(p_min2, p_max2, this.ITERATION_QUIET_COLOR);
    }
    float num5 = this.selectedGeyser.RemainingActiveTime();
    float num6 = this.selectedGeyser.RemainingDormantTime();
    float num7 = ((double) num6 > 0.0 ? yearLength - num6 : yearLength * yearPercent - num5) / yearLength;
    p_min1.x = (float) ((double) vector2_1.x + (double) num7 * (double) x - 1.0);
    p_max1.x = (float) ((double) vector2_1.x + (double) num7 * (double) x + 1.0);
    p_min1.y = vector2_1.y - 2f;
    p_max1.y += 2f;
    foregroundDrawList = ImGui.GetForegroundDrawList();
    foregroundDrawList.AddRectFilled(p_min1, p_max1, this.CURRENT_TIME_COLOR);
  }

  protected override void RenderTo(DevPanel panel)
  {
    this.Update();
    string fmt1 = (UnityEngine.Object) this.selectedGeyser == (UnityEngine.Object) null ? "No Geyser Selected" : UI.StripLinkFormatting(this.selectedGeyser.gameObject.GetProperName()) + " -";
    uint num1 = 0;
    ImGui.AlignTextToFramePadding();
    ImGui.Text(fmt1);
    if (!((UnityEngine.Object) this.selectedGeyser != (UnityEngine.Object) null))
      return;
    StateMachine.BaseState currentState = this.selectedGeyser.smi.GetCurrentState();
    string fmt2 = "zZ";
    string fmt3 = "Current State: " + currentState.name;
    Vector4 col = this.SUBTITLE_SLEEP_COLOR;
    if (currentState == this.selectedGeyser.smi.sm.erupt.erupting)
    {
      fmt2 = "Erupting";
      col = this.SUBTITLE_ERUPTING_COLOR;
    }
    else if (currentState == this.selectedGeyser.smi.sm.erupt.overpressure)
    {
      fmt2 = "Overpressure";
      col = this.SUBTITLE_OVERPRESSURE_COLOR;
    }
    ImGui.SameLine();
    ImGui.TextColored(col, fmt2);
    if (ImGui.IsItemHovered())
      ImGui.SetTooltip(fmt3);
    ImGui.Separator();
    ImGui.Spacing();
    Geyser.GeyserModification modifier = this.selectedGeyser.configuration.GetModifier();
    this.PrepareSummaryForModification(this.selectedGeyser.configuration.GetModifier());
    float currentLifeTime = this.selectedGeyser.GetCurrentLifeTime();
    float yearLength = this.selectedGeyser.configuration.GetYearLength();
    ImGui.Text("Time Settings: \t");
    ImGui.SameLine();
    bool flag1 = ImGui.Button("Active");
    ImGui.SameLine();
    bool flag2 = ImGui.Button("Dormant");
    ImGui.SameLine();
    bool flag3 = ImGui.Button("<");
    ImGui.SameLine();
    bool flag4 = ImGui.Button(">");
    ImGui.SameLine();
    string[] strArray = new string[5]
    {
      "\tLifetime: ",
      currentLifeTime.ToString("00.0"),
      " sec (",
      null,
      null
    };
    float num2 = currentLifeTime / yearLength;
    strArray[3] = num2.ToString("0.00");
    strArray[4] = " Years)\t";
    ImGui.Text(string.Concat(strArray));
    bool flag5 = false;
    if ((double) this.selectedGeyser.timeShift != 0.0)
    {
      ImGui.SameLine();
      flag5 = ImGui.Button("Restore");
      if (ImGui.IsItemHovered())
        ImGui.SetTooltip("Restore lifetime to match with current game time");
    }
    ImGui.SliderFloat("rateRoll", ref this.selectedGeyser.configuration.rateRoll, 0.0f, 1f);
    ImGui.SliderFloat("iterationLengthRoll", ref this.selectedGeyser.configuration.iterationLengthRoll, 0.0f, 1f);
    ImGui.SliderFloat("iterationPercentRoll", ref this.selectedGeyser.configuration.iterationPercentRoll, 0.0f, 1f);
    ImGui.SliderFloat("yearLengthRoll", ref this.selectedGeyser.configuration.yearLengthRoll, 0.0f, 1f);
    ImGui.SliderFloat("yearPercentRoll", ref this.selectedGeyser.configuration.yearPercentRoll, 0.0f, 1f);
    this.selectedGeyser.configuration.Init(true);
    if (flag1)
      this.selectedGeyser.ShiftTimeTo(Geyser.TimeShiftStep.ActiveState);
    if (flag2)
      this.selectedGeyser.ShiftTimeTo(Geyser.TimeShiftStep.DormantState);
    if (flag3)
      this.selectedGeyser.ShiftTimeTo(Geyser.TimeShiftStep.PreviousIteration);
    if (flag4)
      this.selectedGeyser.ShiftTimeTo(Geyser.TimeShiftStep.NextIteration);
    if (flag5)
      this.selectedGeyser.AlterTime(0.0f);
    this.DrawYearAndIterationsGraph(this.selectedGeyser);
    ImGui.Indent();
    bool flag6 = true;
    float num3 = flag6 ? 100f : 1f;
    string modifierUnit = flag6 ? "%%" : "";
    float convertedTemperature = GameUtil.GetConvertedTemperature(this.selectedGeyser.configuration.GetTemperature());
    string temperatureUnitSuffix = GameUtil.GetTemperatureUnitSuffix();
    Element elementByHash1 = ElementLoader.FindElementByHash(this.selectedGeyser.configuration.GetElement());
    Element elementByHash2 = ElementLoader.FindElementByHash(this.selectedGeyser.configuration.geyserType.element);
    string str1;
    if (elementByHash2.lowTempTransitionTarget != (SimHashes) 0)
    {
      num2 = GameUtil.GetConvertedTemperature(elementByHash2.lowTemp);
      str1 = $"{num2.ToString()} -> {elementByHash2.lowTempTransitionTarget.ToString()}";
    }
    else
      str1 = "";
    string str2 = str1;
    string str3;
    if (elementByHash2.highTempTransitionTarget != (SimHashes) 0)
    {
      num2 = GameUtil.GetConvertedTemperature(elementByHash2.highTemp);
      str3 = $"{num2.ToString()} -> {elementByHash2.highTempTransitionTarget.ToString()}";
    }
    else
      str3 = "";
    string str4 = str3;
    ImGui.BulletText("Element:");
    ImGui.SameLine();
    if (elementByHash2 != elementByHash1)
    {
      string str5;
      if (elementByHash1.lowTempTransitionTarget != (SimHashes) 0)
      {
        num2 = GameUtil.GetConvertedTemperature(elementByHash1.lowTemp);
        str5 = $"{num2.ToString()} {elementByHash1.lowTempTransitionTarget.ToString()}";
      }
      else
        str5 = "";
      string str6 = str5;
      string str7;
      if (elementByHash1.highTempTransitionTarget != (SimHashes) 0)
      {
        num2 = GameUtil.GetConvertedTemperature(elementByHash1.highTemp);
        str7 = $"{num2.ToString()} {elementByHash1.highTempTransitionTarget.ToString()}";
      }
      else
        str7 = "";
      string str8 = str7;
      ImGui.TextColored(this.MODFIED_VALUE_TEXT_COLOR, elementByHash1.ToString());
      ImGui.SameLine();
      ImGui.TextColored(this.MODFIED_VALUE_TEXT_COLOR, $"(Original element: {elementByHash2.id.ToString()})");
      ImGui.SameLine();
      ImGui.Text($" [original low: {str2}, {str4}, current low: {str6}, {str8}]");
    }
    else
      ImGui.Text($"{elementByHash2.id} [low: {str2}, high: {str4}]");
    float altValue = Mathf.Max(0.0f, GameUtil.GetConvertedTemperature(elementByHash2.highTemp) - convertedTemperature);
    this.DrawGeyserVariable("Emit Rate", this.selectedGeyser.configuration.GetEmitRate(), 0.0f, unit: " Kg/s");
    this.DrawGeyserVariable("Average Output", this.selectedGeyser.configuration.GetAverageEmission(), 0.0f, unit: " Kg/s");
    this.DrawGeyserVariable("Mass per cycle", this.selectedGeyser.configuration.GetMassPerCycle(), modifier.massPerCycleModifier * num3, modifierUnit: modifierUnit);
    this.DrawGeyserVariable("Temperature", convertedTemperature, modifier.temperatureModifier, unit: temperatureUnitSuffix, modifierUnit: temperatureUnitSuffix, altValue: altValue, altUnit: temperatureUnitSuffix + " before state change");
    this.DrawGeyserVariable("Max Pressure", this.selectedGeyser.configuration.GetMaxPressure(), modifier.maxPressureModifier * num3, unit: " Kg", modifierUnit: modifierUnit);
    this.DrawGeyserVariable("Iteration duration", this.selectedGeyser.configuration.GetIterationLength(), modifier.iterationDurationModifier * num3, unit: " sec", modifierUnit: modifierUnit);
    this.DrawGeyserVariable("Iteration percentage", this.selectedGeyser.configuration.GetIterationPercent(), modifier.iterationPercentageModifier * num3, modifierUnit: modifierUnit, altValue: this.selectedGeyser.configuration.GetIterationLength() * this.selectedGeyser.configuration.GetIterationPercent(), altUnit: " sec");
    this.DrawGeyserVariable("Year duration", this.selectedGeyser.configuration.GetYearLength(), modifier.yearDurationModifier * num3, unit: " sec", modifierUnit: modifierUnit, altValue: this.selectedGeyser.configuration.GetYearLength() / 600f, altUnit: " cycles");
    this.DrawGeyserVariable("Year percentage", this.selectedGeyser.configuration.GetYearPercent(), modifier.yearPercentageModifier * num3, modifierUnit: modifierUnit, altValue: (float) ((double) this.selectedGeyser.configuration.GetYearPercent() * (double) this.selectedGeyser.configuration.GetYearLength() / 600.0), altUnit: " cycles");
    ImGui.Unindent();
    ImGui.Spacing();
    ImGui.Separator();
    ImGui.Spacing();
    ImGui.Text("Create Modification");
    ImGui.SameLine();
    int num4 = ImGui.Button("Clear") ? 1 : 0;
    if (flag6)
    {
      ImGui.TextColored(this.COMMENT_COLOR, "Units specified in the inputs bellow are percentages E.g. 0.1 represents 10%%\nTemperature is measured in kelvins and percentages affect the kelvin value");
      ImGui.Spacing();
    }
    if (num4 != 0)
      this.dev_modification.Clear();
    ImGui.Indent();
    ImGui.BeginGroup();
    this.dev_modification.newElement.ToString();
    float num5 = 0.05f;
    float num6 = 0.15f;
    string str9 = "%.2f";
    ImGui.InputFloat(this.modifiers_FormatedList_Titles[0], ref this.dev_modification.massPerCycleModifier, flag6 ? num5 : 1f, flag6 ? num6 : 5f, flag6 ? str9 : "%.0f");
    ImGui.InputFloat(this.modifiers_FormatedList_Titles[1], ref this.dev_modification.temperatureModifier, flag6 ? num5 : 1f, flag6 ? num6 : 5f, flag6 ? str9 : "%.0f");
    ImGui.InputFloat(this.modifiers_FormatedList_Titles[2], ref this.dev_modification.maxPressureModifier, flag6 ? num5 : 0.1f, flag6 ? num6 : 0.5f, flag6 ? str9 : "%.1f");
    ImGui.InputFloat(this.modifiers_FormatedList_Titles[3], ref this.dev_modification.iterationDurationModifier, flag6 ? num5 : 1f, flag6 ? num6 : 5f, flag6 ? str9 : "%.0f");
    ImGui.InputFloat(this.modifiers_FormatedList_Titles[4], ref this.dev_modification.iterationPercentageModifier, flag6 ? num5 : 0.01f, flag6 ? num6 : 0.1f, flag6 ? str9 : "%.2f");
    ImGui.InputFloat(this.modifiers_FormatedList_Titles[5], ref this.dev_modification.yearDurationModifier, flag6 ? num5 : 1f, flag6 ? num6 : 5f, flag6 ? str9 : "%.0f");
    ImGui.InputFloat(this.modifiers_FormatedList_Titles[6], ref this.dev_modification.yearPercentageModifier, flag6 ? num5 : 0.01f, flag6 ? num6 : 0.1f, flag6 ? str9 : "%.2f");
    ImGui.Checkbox(this.modifiers_FormatedList_Titles[7], ref this.dev_modification.modifyElement);
    string str10 = "None";
    string preview_value = !this.dev_modification.modifyElement || this.dev_modification.newElement == (SimHashes) 0 ? str10 : this.dev_modification.newElement.ToString();
    if (ImGui.BeginCombo(this.modifiers_FormatedList_Titles[8], preview_value))
    {
      for (int index = 0; index < this.AllSimHashesValues.Length; ++index)
      {
        bool selected = this.dev_modification.newElement.ToString() == preview_value;
        if (ImGui.Selectable(this.AllSimHashesValues[index], selected))
        {
          preview_value = this.AllSimHashesValues[index];
          this.dev_modification.newElement = (SimHashes) Enum.Parse(typeof (SimHashes), preview_value);
        }
        if (selected)
          ImGui.SetItemDefaultFocus();
      }
      ImGui.EndCombo();
    }
    int num7;
    if (ImGui.Button("Add Modification"))
    {
      ref Geyser.GeyserModification local = ref this.dev_modification;
      num7 = this.DevModifierID++;
      string str11 = "DEV MODIFIER#" + num7.ToString();
      local.originID = str11;
      this.selectedGeyser.AddModification(this.dev_modification);
    }
    ImGui.SameLine();
    if (ImGui.Button("Remove Last") && this.selectedGeyser.modifications.Count > 0)
    {
      int index1 = -1;
      for (int index2 = this.selectedGeyser.modifications.Count - 1; index2 >= 0; --index2)
      {
        if (this.selectedGeyser.modifications[index2].originID.Contains("DEV MODIFIER"))
        {
          index1 = index2;
          break;
        }
      }
      if (index1 >= 0)
        this.selectedGeyser.RemoveModification(this.selectedGeyser.modifications[index1]);
    }
    ImGui.EndGroup();
    ImGui.Unindent();
    ImGui.Spacing();
    ImGui.Separator();
    ImGui.Spacing();
    while (this.modificationListUnfold.Count < this.selectedGeyser.modifications.Count)
      this.modificationListUnfold.Add(false);
    num7 = this.selectedGeyser.modifications.Count;
    ImGui.Text("Modifications: " + num7.ToString());
    ImGui.Indent();
    for (int index3 = 0; index3 < this.selectedGeyser.modifications.Count; ++index3)
    {
      bool flag7 = this.selectedGeyser.modifications[index3].originID.Contains("DEV MODIFIER");
      bool flag8 = false;
      bool flag9 = false;
      if (this.modificationListUnfold[index3] = ImGui.CollapsingHeader($"{index3.ToString()}. {this.selectedGeyser.modifications[index3].originID}", ImGuiTreeNodeFlags.SpanAvailWidth))
      {
        this.PrepareSummaryForModification(this.selectedGeyser.modifications[index3]);
        Vector2 itemRectSize = ImGui.GetItemRectSize();
        itemRectSize.y *= (float) Mathf.Max(this.modifiers_FormatedList.Length + (flag7 ? 1 : 0) + 1, 1);
        if (ImGui.BeginChild(++num1, itemRectSize, false, ImGuiWindowFlags.NoBackground))
        {
          ImGui.Indent();
          for (int index4 = 0; index4 < this.modifiers_FormatedList.Length; ++index4)
          {
            ImGui.Text(this.modifiers_FormatedList[index4]);
            if (ImGui.IsItemHovered())
            {
              this.modifierSelected = index4;
              ImGui.SetTooltip(this.modifiers_FormatedList_Tooltip[this.modifierSelected]);
            }
          }
          flag9 = ImGui.Button("Copy");
          if (flag7)
            flag8 = ImGui.Button("Remove");
          ImGui.Unindent();
        }
        ImGui.EndChild();
      }
      if (flag9)
        this.dev_modification = this.selectedGeyser.modifications[index3];
      if (flag8)
      {
        this.selectedGeyser.RemoveModification(this.selectedGeyser.modifications[index3]);
        break;
      }
    }
    ImGui.Unindent();
  }

  private void PrepareSummaryForModification(Geyser.GeyserModification modification)
  {
    float num1 = Geyser.massModificationMethod == Geyser.ModificationMethod.Percentages ? 100f : 1f;
    float num2 = Geyser.temperatureModificationMethod == Geyser.ModificationMethod.Percentages ? 100f : 1f;
    float num3 = Geyser.maxPressureModificationMethod == Geyser.ModificationMethod.Percentages ? 100f : 1f;
    float num4 = Geyser.IterationDurationModificationMethod == Geyser.ModificationMethod.Percentages ? 100f : 1f;
    float num5 = Geyser.IterationPercentageModificationMethod == Geyser.ModificationMethod.Percentages ? 100f : 1f;
    float num6 = Geyser.yearDurationModificationMethod == Geyser.ModificationMethod.Percentages ? 100f : 1f;
    float num7 = Geyser.yearPercentageModificationMethod == Geyser.ModificationMethod.Percentages ? 100f : 1f;
    string str1 = (double) num1 == 100.0 ? "%%" : "";
    string str2 = (double) num2 == 100.0 ? "%%" : "";
    string str3 = (double) num3 == 100.0 ? "%%" : "";
    string str4 = (double) num4 == 100.0 ? "%%" : "";
    string str5 = (double) num5 == 100.0 ? "%%" : "";
    string str6 = (double) num6 == 100.0 ? "%%" : "";
    string str7 = (double) num7 == 100.0 ? "%%" : "";
    this.modifiers_FormatedList[0] = $"{this.modifiers_FormatedList_Titles[0]}: {(modification.massPerCycleModifier * num1).ToString("+0.##; -0.##; +0")}{str1}";
    this.modifiers_FormatedList[1] = $"{this.modifiers_FormatedList_Titles[1]}: {(modification.temperatureModifier * num2).ToString("+0.##; -0.##; +0")}{str2}";
    string[] modifiersFormatedList1 = this.modifiers_FormatedList;
    string formatedListTitle1 = this.modifiers_FormatedList_Titles[2];
    float num8 = modification.maxPressureModifier * num3;
    string str8 = num8.ToString("+0.##; -0.##; +0");
    string str9 = (double) num3 == 100.0 ? str3 : "Kg";
    string str10 = $"{formatedListTitle1}: {str8}{str9}";
    modifiersFormatedList1[2] = str10;
    string[] modifiersFormatedList2 = this.modifiers_FormatedList;
    string formatedListTitle2 = this.modifiers_FormatedList_Titles[3];
    num8 = modification.iterationDurationModifier * num4;
    string str11 = num8.ToString("+0.##; -0.##; +0");
    string str12 = (double) num4 == 100.0 ? str4 : "s";
    string str13 = $"{formatedListTitle2}: {str11}{str12}";
    modifiersFormatedList2[3] = str13;
    string[] modifiersFormatedList3 = this.modifiers_FormatedList;
    string formatedListTitle3 = this.modifiers_FormatedList_Titles[4];
    num8 = modification.iterationPercentageModifier * num5;
    string str14 = num8.ToString("+0.##; -0.##; +0");
    string str15 = str5;
    string str16 = $"{formatedListTitle3}: {str14}{str15}";
    modifiersFormatedList3[4] = str16;
    string[] modifiersFormatedList4 = this.modifiers_FormatedList;
    string formatedListTitle4 = this.modifiers_FormatedList_Titles[5];
    num8 = modification.yearDurationModifier * num6;
    string str17 = num8.ToString("+0.##; -0.##; +0");
    string str18 = (double) num6 == 100.0 ? str6 : "s";
    string str19 = $"{formatedListTitle4}: {str17}{str18}";
    modifiersFormatedList4[5] = str19;
    string[] modifiersFormatedList5 = this.modifiers_FormatedList;
    string formatedListTitle5 = this.modifiers_FormatedList_Titles[6];
    num8 = modification.yearPercentageModifier * num7;
    string str20 = num8.ToString("+0.##; -0.##; +0");
    string str21 = str7;
    string str22 = $"{formatedListTitle5}: {str20}{str21}";
    modifiersFormatedList5[6] = str22;
    this.modifiers_FormatedList[7] = $"{this.modifiers_FormatedList_Titles[7]}: {modification.modifyElement.ToString()}";
    this.modifiers_FormatedList[8] = $"{this.modifiers_FormatedList_Titles[8]}: {(modification.IsNewElementInUse() ? modification.newElement.ToString() : "None")}";
  }

  private void Update()
  {
    this.Setup();
    GameObject gameObject = SelectTool.Instance?.selected?.gameObject;
    if ((UnityEngine.Object) this.lastSelectedGameObject != (UnityEngine.Object) gameObject && (UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      Geyser component = gameObject.GetComponent<Geyser>();
      this.selectedGeyser = (UnityEngine.Object) component == (UnityEngine.Object) null ? this.selectedGeyser : component;
    }
    this.lastSelectedGameObject = gameObject;
  }

  private void Setup()
  {
    if (this.AllSimHashesValues == null)
      this.AllSimHashesValues = Enum.GetNames(typeof (SimHashes));
    if (this.modifierFormatting_ValuePadding < 0)
    {
      for (int index = 0; index < this.modifiers_FormatedList_Titles.Length; ++index)
        this.modifierFormatting_ValuePadding = Mathf.Max(this.modifierFormatting_ValuePadding, this.modifiers_FormatedList_Titles[index].Length);
    }
    if (!string.IsNullOrEmpty(this.modifiers_FormatedList_Tooltip[0]))
      return;
    this.modifiers_FormatedList_Tooltip[0] = "Mass per cycle is not mass per iteration, mass per iteration gets calculated out of this";
    this.modifiers_FormatedList_Tooltip[1] = "Temperature modifier of the emitted element, does not refer to the temperature of the geyser itself";
    this.modifiers_FormatedList_Tooltip[2] = "Refering to the max pressure allowed in the environment surrounding the geyser before it stops emitting";
    this.modifiers_FormatedList_Tooltip[3] = "An iteration is a chunk of time that has 2 sections, one section is the erupting time while the other is the non erupting time";
    this.modifiers_FormatedList_Tooltip[4] = "Represents what percentage out of the iteration duration will be used for 'Erupting' period and the remaining will be the 'Quiet' period";
    this.modifiers_FormatedList_Tooltip[5] = "A year is a chunk of time that has 2 sections, one section is the Active section while the other is the Dormant section. While active, there could be many Iterations. While Dormant, there is no activity at all.";
    this.modifiers_FormatedList_Tooltip[6] = "Represents what percentage out of the year duration will be used for 'Active' period and the remaining will be the 'Dormant' period";
    this.modifiers_FormatedList_Tooltip[7] = "Whether to use or not to use the specified element";
    this.modifiers_FormatedList_Tooltip[8] = "Extra element to emit";
  }
}
