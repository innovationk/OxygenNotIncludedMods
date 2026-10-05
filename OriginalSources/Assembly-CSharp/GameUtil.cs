// Decompiled with JetBrains decompiler
// Type: GameUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TUNING;
using UnityEngine;
using UnityEngine.Pool;

#nullable disable
public static class GameUtil
{
  public static GameUtil.TemperatureUnit temperatureUnit;
  public static GameUtil.MassUnit massUnit;
  private static string[] adjectives;
  public static TagSet foodTags = new TagSet(new string[10]
  {
    "BasicPlantFood",
    "MushBar",
    "ColdWheatSeed",
    "ColdWheatSeed",
    "SpiceNut",
    "PrickleFruit",
    "Meat",
    "Mushroom",
    "ColdWheat",
    GameTags.Compostable.Name
  });
  public static TagSet solidTags = new TagSet(new string[5]
  {
    "Filter",
    "Coal",
    "BasicFabric",
    "SwampLilyFlower",
    "RefinedMetal"
  });

  public static CellOffset[] Expand(this CellOffset[] original)
  {
    List<CellOffset> cellOffsetList = new List<CellOffset>((IEnumerable<CellOffset>) original);
    Vector4 vector4_1 = (Vector4) new Vector2(float.MaxValue, float.MinValue);
    Vector4 vector4_2 = (Vector4) new Vector2(float.MaxValue, float.MinValue);
    for (int index = 0; index < original.Length; ++index)
    {
      CellOffset cellOffset = original[index];
      if ((double) cellOffset.x < (double) vector4_1.x)
        vector4_1.x = (float) cellOffset.x;
      if ((double) cellOffset.x > (double) vector4_1.y)
        vector4_1.y = (float) cellOffset.x;
      if ((double) cellOffset.y < (double) vector4_2.x)
        vector4_2.x = (float) cellOffset.y;
      if ((double) cellOffset.y > (double) vector4_2.y)
        vector4_2.y = (float) cellOffset.y;
    }
    for (int index = 0; index < original.Length; ++index)
    {
      CellOffset cellOffset = original[index];
      Vector2Int zero = Vector2Int.zero;
      if ((double) cellOffset.x == (double) vector4_1.x)
      {
        cellOffsetList.Add(new CellOffset(cellOffset.x - 1, cellOffset.y));
        zero.x = -1;
      }
      if ((double) cellOffset.x == (double) vector4_1.y)
      {
        cellOffsetList.Add(new CellOffset(cellOffset.x + 1, cellOffset.y));
        zero.x = 1;
      }
      if ((double) cellOffset.y == (double) vector4_2.x)
      {
        cellOffsetList.Add(new CellOffset(cellOffset.x, cellOffset.y - 1));
        zero.y = -1;
      }
      if ((double) cellOffset.y == (double) vector4_2.y)
      {
        cellOffsetList.Add(new CellOffset(cellOffset.x, cellOffset.y + 1));
        zero.y = 1;
      }
      if (zero.x != 0 && zero.y != 0)
        cellOffsetList.Add(new CellOffset((zero.x < 0 ? (int) vector4_1.x : (int) vector4_1.y) + zero.x, (zero.y < 0 ? (int) vector4_2.x : (int) vector4_2.y) + zero.y));
    }
    return cellOffsetList.ToArray();
  }

  public static void TintLiquidSymbolOnBuilding(
    string symbolName,
    KBatchedAnimController controller,
    Element element)
  {
    Color color = (element.IsMoltenMetal ? WaterCubes.MOLTEN_METAL_COLOR : (Color) element.substance.colour) with
    {
      a = 1f
    };
    string name1 = symbolName;
    string name2 = name1 + "_bloom";
    string name3 = element.substance.Glows ? name2 : name1;
    controller.SetSymbolVisiblity(new KAnimHashedString(name1), !element.substance.Glows);
    controller.SetSymbolVisiblity(new KAnimHashedString(name2), element.substance.Glows);
    controller.SetSymbolTint(new KAnimHashedString(name3), color);
  }

  public static string GetTemperatureUnitSuffix()
  {
    string temperatureUnitSuffix;
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        temperatureUnitSuffix = (string) UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
        break;
      case GameUtil.TemperatureUnit.Fahrenheit:
        temperatureUnitSuffix = (string) UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
        break;
      default:
        temperatureUnitSuffix = (string) UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
        break;
    }
    return temperatureUnitSuffix;
  }

  private static string AddTemperatureUnitSuffix(string text)
  {
    return text + GameUtil.GetTemperatureUnitSuffix();
  }

  public static float GetTemperatureConvertedFromKelvin(
    float temperature,
    GameUtil.TemperatureUnit targetUnit)
  {
    if (targetUnit == GameUtil.TemperatureUnit.Celsius)
      return temperature - 273.15f;
    return targetUnit == GameUtil.TemperatureUnit.Fahrenheit ? (float) ((double) temperature * 1.7999999523162842 - 459.67001342773438) : temperature;
  }

  public static float GetConvertedTemperature(float temperature, bool roundOutput = false)
  {
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        float f1 = temperature - 273.15f;
        return !roundOutput ? f1 : Mathf.Round(f1);
      case GameUtil.TemperatureUnit.Fahrenheit:
        float f2 = (float) ((double) temperature * 1.7999999523162842 - 459.67001342773438);
        return !roundOutput ? f2 : Mathf.Round(f2);
      default:
        return !roundOutput ? temperature : Mathf.Round(temperature);
    }
  }

  public static float GetTemperatureConvertedToKelvin(
    float temperature,
    GameUtil.TemperatureUnit fromUnit)
  {
    if (fromUnit == GameUtil.TemperatureUnit.Celsius)
      return temperature + 273.15f;
    return fromUnit == GameUtil.TemperatureUnit.Fahrenheit ? (float) (((double) temperature + 459.67001342773438) * 5.0 / 9.0) : temperature;
  }

  public static float GetTemperatureConvertedToKelvin(float temperature)
  {
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        return temperature + 273.15f;
      case GameUtil.TemperatureUnit.Fahrenheit:
        return (float) (((double) temperature + 459.67001342773438) * 5.0 / 9.0);
      default:
        return temperature;
    }
  }

  private static float GetConvertedTemperatureDelta(float kelvin_delta)
  {
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        return kelvin_delta;
      case GameUtil.TemperatureUnit.Fahrenheit:
        return kelvin_delta * 1.8f;
      case GameUtil.TemperatureUnit.Kelvin:
        return kelvin_delta;
      default:
        return kelvin_delta;
    }
  }

  public static float ApplyTimeSlice(float val, GameUtil.TimeSlice timeSlice)
  {
    return timeSlice == GameUtil.TimeSlice.PerCycle ? val * 600f : val;
  }

  public static float ApplyTimeSlice(int val, GameUtil.TimeSlice timeSlice)
  {
    return timeSlice == GameUtil.TimeSlice.PerCycle ? (float) val * 600f : (float) val;
  }

  public static string AddTimeSliceText(string text, GameUtil.TimeSlice timeSlice)
  {
    switch (timeSlice)
    {
      case GameUtil.TimeSlice.PerSecond:
        return text + (string) UI.UNITSUFFIXES.PERSECOND;
      case GameUtil.TimeSlice.PerCycle:
        return text + (string) UI.UNITSUFFIXES.PERCYCLE;
      default:
        return text;
    }
  }

  public static void AddTimeSliceText(StringBuilder builder, GameUtil.TimeSlice timeSlice)
  {
    switch (timeSlice)
    {
      case GameUtil.TimeSlice.PerSecond:
        builder.Append((string) UI.UNITSUFFIXES.PERSECOND);
        break;
      case GameUtil.TimeSlice.PerCycle:
        builder.Append((string) UI.UNITSUFFIXES.PERCYCLE);
        break;
    }
  }

  public static string AddPositiveSign(string text, bool positive)
  {
    return positive ? string.Format((string) UI.POSITIVE_FORMAT, (object) text) : text;
  }

  public static float AttributeSkillToAlpha(AttributeInstance attributeInstance)
  {
    return Mathf.Min(attributeInstance.GetTotalValue() / 10f, 1f);
  }

  public static float AttributeSkillToAlpha(float attributeSkill)
  {
    return Mathf.Min(attributeSkill / 10f, 1f);
  }

  public static float AptitudeToAlpha(float aptitude) => Mathf.Min(aptitude / 10f, 1f);

  public static float GetThermalEnergy(PrimaryElement pe)
  {
    return pe.Temperature * pe.Mass * pe.Element.specificHeatCapacity;
  }

  public static float CalculateTemperatureChange(float shc, float mass, float kilowatts)
  {
    return kilowatts / (shc * mass);
  }

  public static void DeltaThermalEnergy(
    PrimaryElement pe,
    float kilowatts,
    float targetTemperature)
  {
    float temperatureChange = GameUtil.CalculateTemperatureChange(pe.Element.specificHeatCapacity, pe.Mass, kilowatts);
    float num1 = pe.Temperature + temperatureChange;
    float num2 = (double) targetTemperature <= (double) pe.Temperature ? Mathf.Clamp(num1, targetTemperature, pe.Temperature) : Mathf.Clamp(num1, pe.Temperature, targetTemperature);
    pe.Temperature = num2;
  }

  public static BindingEntry ActionToBinding(Action action)
  {
    foreach (BindingEntry keyBinding in GameInputMapping.KeyBindings)
    {
      if (keyBinding.mAction == action)
        return keyBinding;
    }
    throw new ArgumentException(action.ToString() + " is not bound in GameInputBindings");
  }

  public static string GetIdentityDescriptor(GameObject go, GameUtil.IdentityDescriptorTense tense = GameUtil.IdentityDescriptorTense.Normal)
  {
    if ((bool) (UnityEngine.Object) go.GetComponent<MinionIdentity>())
    {
      switch (tense)
      {
        case GameUtil.IdentityDescriptorTense.Normal:
          return (string) DUPLICANTS.STATS.SUBJECTS.DUPLICANT;
        case GameUtil.IdentityDescriptorTense.Possessive:
          return (string) DUPLICANTS.STATS.SUBJECTS.DUPLICANT_POSSESSIVE;
        case GameUtil.IdentityDescriptorTense.Plural:
          return (string) DUPLICANTS.STATS.SUBJECTS.DUPLICANT_PLURAL;
      }
    }
    else if ((bool) (UnityEngine.Object) go.GetComponent<CreatureBrain>())
    {
      switch (tense)
      {
        case GameUtil.IdentityDescriptorTense.Normal:
          return (string) DUPLICANTS.STATS.SUBJECTS.CREATURE;
        case GameUtil.IdentityDescriptorTense.Possessive:
          return (string) DUPLICANTS.STATS.SUBJECTS.CREATURE_POSSESSIVE;
        case GameUtil.IdentityDescriptorTense.Plural:
          return (string) DUPLICANTS.STATS.SUBJECTS.CREATURE_PLURAL;
      }
    }
    else
    {
      switch (tense)
      {
        case GameUtil.IdentityDescriptorTense.Normal:
          return (string) DUPLICANTS.STATS.SUBJECTS.PLANT;
        case GameUtil.IdentityDescriptorTense.Possessive:
          return (string) DUPLICANTS.STATS.SUBJECTS.PLANT_POSESSIVE;
        case GameUtil.IdentityDescriptorTense.Plural:
          return (string) DUPLICANTS.STATS.SUBJECTS.PLANT_PLURAL;
      }
    }
    return "";
  }

  public static float GetEnergyInPrimaryElement(PrimaryElement element)
  {
    return (float) (1.0 / 1000.0 * ((double) element.Temperature * ((double) element.Mass * 1000.0 * (double) element.Element.specificHeatCapacity)));
  }

  public static float EnergyToTemperatureDelta(float kilojoules, PrimaryElement element)
  {
    Debug.Assert((double) element.Mass > 0.0);
    double num1 = (double) Mathf.Max(GameUtil.GetEnergyInPrimaryElement(element) - kilojoules, 1f);
    float temperature = element.Temperature;
    double num2 = 1.0 / 1000.0 * ((double) element.Mass * ((double) element.Element.specificHeatCapacity * 1000.0));
    return (float) (num1 / num2) - temperature;
  }

  public static float CalculateEnergyDeltaForElement(
    PrimaryElement element,
    float startTemp,
    float endTemp)
  {
    return GameUtil.CalculateEnergyDeltaForElementChange(element.Mass, element.Element.specificHeatCapacity, startTemp, endTemp);
  }

  public static float CalculateEnergyDeltaForElementChange(
    float mass,
    float shc,
    float startTemp,
    float endTemp)
  {
    return (endTemp - startTemp) * mass * shc;
  }

  public static float GetFinalTemperature(float t1, float m1, float t2, float m2)
  {
    float num1 = m1 + m2;
    float num2 = (float) ((double) t1 * (double) m1 + (double) t2 * (double) m2) / num1;
    float min = Mathf.Min(t1, t2);
    float max = Mathf.Max(t1, t2);
    float f = Mathf.Clamp(num2, min, max);
    if (float.IsNaN(f) || float.IsInfinity(f))
      Debug.LogError((object) $"Calculated an invalid temperature: t1={t1}, m1={m1}, t2={t2}, m2={m2}, min_temp={min}, max_temp={max}");
    return f;
  }

  public static void ForceConduction(PrimaryElement a, PrimaryElement b, float dt)
  {
    float num1 = a.Temperature * a.Element.specificHeatCapacity * a.Mass;
    float num2 = b.Temperature * b.Element.specificHeatCapacity * b.Mass;
    float num3 = Math.Min(a.Element.thermalConductivity, b.Element.thermalConductivity);
    float num4 = Math.Min(a.Mass, b.Mass);
    float val1 = (float) (((double) b.Temperature - (double) a.Temperature) * ((double) num3 * (double) num4)) * dt;
    double num5 = ((double) num1 + (double) num2) / ((double) a.Element.specificHeatCapacity * (double) a.Mass + (double) b.Element.specificHeatCapacity * (double) b.Mass);
    float val2 = Math.Min(Math.Abs(((float) num5 - a.Temperature) * a.Element.specificHeatCapacity * a.Mass), Math.Abs(((float) num5 - b.Temperature) * b.Element.specificHeatCapacity * b.Mass));
    float num6 = Math.Max(Math.Min(val1, val2), -val2);
    a.Temperature = (num1 + num6) / a.Element.specificHeatCapacity / a.Mass;
    b.Temperature = (num2 - num6) / b.Element.specificHeatCapacity / b.Mass;
  }

  public static string FloatToString(float f, string format = null)
  {
    if (float.IsPositiveInfinity(f))
      return (string) UI.POS_INFINITY;
    return float.IsNegativeInfinity(f) ? (string) UI.NEG_INFINITY : f.ToString(format);
  }

  public static void AppendFloatToString(StringBuilder builder, float f, string format = null)
  {
    if (float.IsPositiveInfinity(f))
      builder.Append((string) UI.POS_INFINITY);
    else if (float.IsNegativeInfinity(f))
      builder.Append((string) UI.NEG_INFINITY);
    else if (format != null)
    {
      Span<char> span = stackalloc char[64 /*0x40*/];
      int num;
      f.TryFormat(span, ref num, string.op_Implicit(format), (IFormatProvider) null);
      builder.Append(Span<char>.op_Implicit(span.Slice(0, num)));
    }
    else
      builder.Append(f);
  }

  public static string GetFloatWithDecimalPoint(float f)
  {
    string format = (double) f != 0.0 ? ((double) Mathf.Abs(f) >= 1.0 ? "#,###.#" : "#,##0.#") : "0";
    return GameUtil.FloatToString(f, format);
  }

  public static void AppendFloatWithDecimalPoint(StringBuilder builder, float f)
  {
    if ((double) f == 0.0)
      builder.AppendFormat("{0:0}", (object) f);
    else if ((double) Mathf.Abs(f) < 1.0)
      builder.AppendFormat("{0:#,##0.#}", (object) f);
    else
      builder.AppendFormat("{0:#,###.#}", (object) f);
  }

  public static string GetStandardFloat(float f)
  {
    string format = (double) f != 0.0 ? ((double) Mathf.Abs(f) >= 1.0 ? ((double) Mathf.Abs(f) >= 10.0 ? "#,###" : "#,###.#") : "#,##0.#") : "0";
    return GameUtil.FloatToString(f, format);
  }

  public static void AppendStandardFloat(StringBuilder builder, float f)
  {
    if (float.IsPositiveInfinity(f))
      builder.Append((string) UI.POS_INFINITY);
    else if (float.IsNegativeInfinity(f))
      builder.Append((string) UI.NEG_INFINITY);
    else if ((double) f == 0.0)
      builder.AppendFormat("{0:0}", (object) f);
    else if ((double) Math.Abs(f) < 1.0)
      builder.AppendFormat("{0:#,##0.##}", (object) f);
    else if ((double) Math.Abs(f) < 10.0)
      builder.AppendFormat("{0:#,##0.##}", (object) f);
    else
      builder.AppendFormat("{0:#,###}", (object) f);
  }

  public static string GetStandardPercentageFloat(float f, bool allowHundredths = false)
  {
    string format = (double) Mathf.Abs(f) != 0.0 ? (!((double) Mathf.Abs(f) < 0.10000000149011612 & allowHundredths) ? ((double) Mathf.Abs(f) >= 1.0 ? "##0" : "##0.#") : "##0.##") : "0";
    return GameUtil.FloatToString(f, format);
  }

  public static void AppendStandardPercentageFloat(
    StringBuilder builder,
    float f,
    bool allowHundredths = false)
  {
    if ((double) Mathf.Abs(f) == 0.0)
      builder.AppendFormat("{0:0}", (object) f);
    else if ((double) Mathf.Abs(f) < 0.10000000149011612 & allowHundredths)
      builder.AppendFormat("{0:##0.##}", (object) f);
    else if ((double) Mathf.Abs(f) < 1.0)
    {
      builder.AppendFormat("{0:##0.#}", (object) f);
    }
    else
    {
      if ((double) f < 100.0 && (double) f >= 99.5)
        f = 99f;
      builder.AppendFormat("{0:##0}", (object) f);
    }
  }

  public static string GetUnitFormattedName(GameObject go, bool upperName = false)
  {
    KPrefabID component1 = go.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && Assets.IsTagCountable(component1.PrefabTag))
    {
      PrimaryElement component2 = go.GetComponent<PrimaryElement>();
      return GameUtil.GetUnitFormattedName(go.GetProperName(), component2.Units, upperName);
    }
    return !upperName ? go.GetProperName() : StringFormatter.ToUpper(go.GetProperName());
  }

  public static string GetUnitFormattedName(string name, float count, bool upperName = false)
  {
    if (upperName)
      name = name.ToUpper();
    return StringFormatter.Replace((string) UI.NAME_WITH_UNITS, "{0}", name).Replace("{1}", $"{count:0.##}");
  }

  public static void AppendFormattedUnits(
    StringBuilder builder,
    float units,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool displaySuffix = true,
    string floatFormatOverride = "")
  {
    units = GameUtil.ApplyTimeSlice(units, timeSlice);
    if (!floatFormatOverride.IsNullOrWhiteSpace())
      builder.AppendFormat(floatFormatOverride, (object) units);
    else
      GameUtil.AppendStandardFloat(builder, units);
    if (displaySuffix)
      builder.Append((string) ((double) units == 1.0 ? UI.UNITSUFFIXES.UNIT : UI.UNITSUFFIXES.UNITS));
    GameUtil.AddTimeSliceText(builder, timeSlice);
  }

  public static string GetFormattedUnits(
    float units,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool displaySuffix = true,
    string floatFormatOverride = "")
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedUnits(stringBuilder, units, timeSlice, displaySuffix, floatFormatOverride);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static void AppendFormattedRocketRangePerCycle(
    StringBuilder builder,
    float range,
    bool displaySuffix = true)
  {
    if (displaySuffix)
      builder.AppendFormat("{0:N1} {1}", (object) range, (object) UI.CLUSTERMAP.TILES_PER_CYCLE);
    else
      builder.AppendFormat("{0:N1}", (object) range);
  }

  public static string GetFormattedRocketRangePerCycle(float range, bool displaySuffix = true)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedRocketRangePerCycle(stringBuilder, range, displaySuffix);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static void AppendFormattedRocketRange(
    StringBuilder builder,
    int rangeInTiles,
    bool displaySuffix = true)
  {
    builder.Append(rangeInTiles);
    if (!displaySuffix)
      return;
    builder.Append(" ");
    builder.Append((string) UI.CLUSTERMAP.TILES);
  }

  public static string GetFormattedRocketRange(int rangeInTiles, bool displaySuffix = true)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedRocketRange(stringBuilder, rangeInTiles, displaySuffix);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static string ApplyBoldString(string source) => $"<b>{source}</b>";

  public static void AppendBoldString(StringBuilder builder, string source)
  {
    builder.AppendFormat("<b>{0}</b>", (object) source);
  }

  public static float GetRoundedTemperatureInKelvin(float kelvin)
  {
    float temperatureInKelvin = 0.0f;
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        temperatureInKelvin = GameUtil.GetTemperatureConvertedToKelvin(Mathf.Round(GameUtil.GetConvertedTemperature(Mathf.Round(kelvin), true)));
        break;
      case GameUtil.TemperatureUnit.Fahrenheit:
        temperatureInKelvin = GameUtil.GetTemperatureConvertedToKelvin((float) Mathf.RoundToInt(GameUtil.GetTemperatureConvertedFromKelvin(kelvin, GameUtil.TemperatureUnit.Fahrenheit)), GameUtil.TemperatureUnit.Fahrenheit);
        break;
      case GameUtil.TemperatureUnit.Kelvin:
        temperatureInKelvin = (float) Mathf.RoundToInt(kelvin);
        break;
    }
    return temperatureInKelvin;
  }

  public static void AppendFormattedTemperature(
    StringBuilder builder,
    float temp,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    GameUtil.TemperatureInterpretation interpretation = GameUtil.TemperatureInterpretation.Absolute,
    bool displayUnits = true,
    bool roundInDestinationFormat = false)
  {
    if (interpretation != GameUtil.TemperatureInterpretation.Absolute)
    {
      if (interpretation == GameUtil.TemperatureInterpretation.Relative)
        ;
      temp = GameUtil.GetConvertedTemperatureDelta(temp);
    }
    else
      temp = GameUtil.GetConvertedTemperature(temp, roundInDestinationFormat);
    temp = GameUtil.ApplyTimeSlice(temp, timeSlice);
    if ((double) Mathf.Abs(temp) < 0.10000000149011612)
      builder.AppendFormat("{0:##0.####}", (object) temp);
    else
      builder.AppendFormat("{0:##0.#}", (object) temp);
    if (displayUnits)
      builder.Append(GameUtil.GetTemperatureUnitSuffix());
    GameUtil.AddTimeSliceText(builder, timeSlice);
  }

  public static string GetFormattedTemperature(
    float temp,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    GameUtil.TemperatureInterpretation interpretation = GameUtil.TemperatureInterpretation.Absolute,
    bool displayUnits = true,
    bool roundInDestinationFormat = false)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedTemperature(stringBuilder, temp, timeSlice, interpretation, displayUnits, roundInDestinationFormat);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static void AppendFormattedCaloriesForItem(
    StringBuilder builder,
    Tag tag,
    float amount,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool forceKcal = true)
  {
    EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(tag.Name);
    GameUtil.AppendFormattedCalories(builder, foodInfo != null ? foodInfo.CaloriesPerUnit * amount : -1f, timeSlice, forceKcal);
  }

  public static string GetFormattedCaloriesForItem(
    Tag tag,
    float amount,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool forceKcal = true)
  {
    return GameUtil.GetFormattedCaloriesForItem(tag, amount, true, timeSlice, forceKcal);
  }

  public static string GetFormattedCaloriesForItem(
    Tag tag,
    float amount,
    bool showSuffix,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool forceKcal = true)
  {
    EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(tag.Name);
    return GameUtil.GetFormattedCalories(foodInfo != null ? foodInfo.CaloriesPerUnit * amount : -1f, showSuffix, timeSlice, forceKcal);
  }

  public static void AppendFormattedCalories(
    StringBuilder builder,
    float calories,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool forceKcal = true)
  {
    GameUtil.AppendFormattedCalories(builder, calories, true, timeSlice, forceKcal);
  }

  public static void AppendFormattedCalories(
    StringBuilder builder,
    float calories,
    bool showSuffix,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool forceKcal = true)
  {
    string str = (string) UI.UNITSUFFIXES.CALORIES.CALORIE;
    if ((double) Mathf.Abs(calories) >= 1000.0 | forceKcal)
    {
      calories /= 1000f;
      str = (string) UI.UNITSUFFIXES.CALORIES.KILOCALORIE;
    }
    calories = GameUtil.ApplyTimeSlice(calories, timeSlice);
    GameUtil.AppendStandardFloat(builder, calories);
    if (!showSuffix)
      return;
    builder.Append(str);
    GameUtil.AddTimeSliceText(builder, timeSlice);
  }

  public static string GetFormattedCalories(
    float calories,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool forceKcal = true)
  {
    return GameUtil.GetFormattedCalories(calories, true, timeSlice, forceKcal);
  }

  public static string GetFormattedCalories(
    float calories,
    bool showSuffix,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool forceKcal = true)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedCalories(stringBuilder, calories, showSuffix, timeSlice, forceKcal);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static string GetFormattedPreyConsumptionValuePerCycle(
    Tag preyTag,
    float crittersPerSecond,
    bool perCycle = true)
  {
    Assets.GetPrefab(preyTag).GetComponent<PrimaryElement>();
    return GameUtil.GetFormattedUnits(crittersPerSecond, GameUtil.TimeSlice.PerCycle);
  }

  public static string GetFormattedDirectPlantConsumptionValuePerCycle(
    Tag plantTag,
    float consumer_caloriesLossPerCaloriesPerKG,
    bool perCycle = true)
  {
    IPlantConsumptionInstructions[] consumptionInstructions1 = GameUtil.GetPlantConsumptionInstructions(Assets.GetPrefab(plantTag));
    if (consumptionInstructions1 == null || consumptionInstructions1.Length == 0)
      return "Error";
    foreach (IPlantConsumptionInstructions consumptionInstructions2 in consumptionInstructions1)
    {
      if (consumptionInstructions2.GetDietFoodType() == Diet.Info.FoodType.EatPlantDirectly)
        return consumptionInstructions2.GetFormattedConsumptionPerCycle(consumer_caloriesLossPerCaloriesPerKG);
    }
    return "Error";
  }

  public static string GetFormattedBranchGrowerPlantProductionValuePerCycle(
    Tag productTag,
    float outputAmountPerBranch,
    int branchCount,
    bool perCycle = true)
  {
    return GameUtil.SafeStringFormat((string) UI.BUILDINGEFFECTS.TOOLTIPS.BRANCH_GROWER_PLANT_POTENTIAL_OUTPUT, (object) GameUtil.GetFormattedByTag(productTag, outputAmountPerBranch, false, GameUtil.TimeSlice.PerCycle), (object) GameUtil.GetFormattedByTag(productTag, outputAmountPerBranch * (float) branchCount, GameUtil.TimeSlice.PerCycle));
  }

  public static string GetFormattedBranchGrowerPlantPlantFiberProductionValuePerCycle(
    Tag productTag,
    float outputAmountPerBranch,
    int branchCount,
    bool perCycle = true)
  {
    return GameUtil.SafeStringFormat((string) UI.BUILDINGEFFECTS.TOOLTIPS.BRANCH_GROWER_PLANT_POTENTIAL_OUTPUT, (object) GameUtil.GetFormattedMass(GameUtil.ApplyTimeSlice(outputAmountPerBranch, GameUtil.TimeSlice.PerCycle)), (object) GameUtil.GetFormattedMass(outputAmountPerBranch * (float) branchCount, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram));
  }

  public static string GetFormattedPlantStorageConsumptionValuePerCycle(
    Tag plantTag,
    float consumer_caloriesLossPerCaloriesPerKG,
    bool perCycle = true)
  {
    IPlantConsumptionInstructions[] consumptionInstructions1 = GameUtil.GetPlantConsumptionInstructions(Assets.GetPrefab(plantTag));
    if (consumptionInstructions1 == null || consumptionInstructions1.Length == 0)
      return "Error";
    foreach (IPlantConsumptionInstructions consumptionInstructions2 in consumptionInstructions1)
    {
      if (consumptionInstructions2.GetDietFoodType() == Diet.Info.FoodType.EatPlantStorage)
        return consumptionInstructions2.GetFormattedConsumptionPerCycle(consumer_caloriesLossPerCaloriesPerKG);
    }
    return "Error";
  }

  public static IPlantConsumptionInstructions[] GetPlantConsumptionInstructions(GameObject prefab)
  {
    IPlantConsumptionInstructions[] components = prefab.GetComponents<IPlantConsumptionInstructions>();
    List<IPlantConsumptionInstructions> allSmi = prefab.GetAllSMI<IPlantConsumptionInstructions>();
    List<IPlantConsumptionInstructions> consumptionInstructionsList = new List<IPlantConsumptionInstructions>();
    if (components != null)
      consumptionInstructionsList.AddRange((IEnumerable<IPlantConsumptionInstructions>) components);
    if (allSmi != null)
      consumptionInstructionsList.AddRange((IEnumerable<IPlantConsumptionInstructions>) allSmi);
    return consumptionInstructionsList.ToArray();
  }

  public static void AppendFormattedPlantGrowth(
    StringBuilder builder,
    float percent,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    percent = GameUtil.ApplyTimeSlice(percent, timeSlice);
    GameUtil.AppendStandardPercentageFloat(builder, percent, true);
    builder.Append((string) UI.UNITSUFFIXES.PERCENT);
    builder.Append(" ");
    builder.Append((string) UI.UNITSUFFIXES.GROWTH);
    GameUtil.AddTimeSliceText(builder, timeSlice);
  }

  public static string GetFormattedPlantGrowth(float percent, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedPlantGrowth(stringBuilder, percent, timeSlice);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static void AppendFormattedPercent(
    StringBuilder builder,
    float percent,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    GameUtil.AppendStandardPercentageFloat(builder, GameUtil.ApplyTimeSlice(percent, timeSlice));
    builder.Append((string) UI.UNITSUFFIXES.PERCENT);
    GameUtil.AddTimeSliceText(builder, timeSlice);
  }

  public static string GetFormattedPercent(float percent, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedPercent(stringBuilder, percent, timeSlice);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static void AppendFormattedRoundedJoules(StringBuilder builder, float joules)
  {
    if ((double) Mathf.Abs(joules) > 1000.0)
    {
      builder.AppendFormat("{0:F1}", (object) (float) ((double) joules / 1000.0));
      builder.Append((string) UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE);
    }
    else
    {
      builder.AppendFormat("{0:F1}", (object) joules);
      builder.Append((string) UI.UNITSUFFIXES.ELECTRICAL.JOULE);
    }
  }

  public static string GetFormattedRoundedJoules(float joules)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedRoundedJoules(stringBuilder, joules);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static string GetFormattedJoules(
    float joules,
    string floatFormat = "F1",
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    if (timeSlice == GameUtil.TimeSlice.PerSecond)
      return GameUtil.GetFormattedWattage(joules);
    joules = GameUtil.ApplyTimeSlice(joules, timeSlice);
    return GameUtil.AddTimeSliceText((double) Math.Abs(joules) <= 1000000.0 ? ((double) Mathf.Abs(joules) <= 1000.0 ? GameUtil.FloatToString(joules, floatFormat) + (string) UI.UNITSUFFIXES.ELECTRICAL.JOULE : GameUtil.FloatToString(joules / 1000f, floatFormat) + (string) UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE) : GameUtil.FloatToString(joules / 1000000f, floatFormat) + (string) UI.UNITSUFFIXES.ELECTRICAL.MEGAJOULE, timeSlice);
  }

  public static void AppendFormattedRads(
    StringBuilder builder,
    float rads,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    rads = GameUtil.ApplyTimeSlice(rads, timeSlice);
    GameUtil.AppendStandardFloat(builder, rads);
    builder.Append((string) UI.UNITSUFFIXES.RADIATION.RADS);
    GameUtil.AddTimeSliceText(builder, timeSlice);
  }

  public static string GetFormattedRads(float rads, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedRads(stringBuilder, rads, timeSlice);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static void AppendFormattedHighEnergyParticles(
    StringBuilder builder,
    float units,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool displayUnits = true)
  {
    GameUtil.AppendFloatWithDecimalPoint(builder, units);
    if (displayUnits)
      builder.Append((string) ((double) units == 1.0 ? UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLE : UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES));
    GameUtil.AddTimeSliceText(builder, timeSlice);
  }

  public static string GetFormattedHighEnergyParticles(
    float units,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool displayUnits = true)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedHighEnergyParticles(stringBuilder, units, timeSlice, displayUnits);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static void AppendFormattedWattage(
    StringBuilder builder,
    float watts,
    GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Automatic,
    bool displayUnits = true)
  {
    string str = (string) null;
    switch (unit)
    {
      case GameUtil.WattageFormatterUnit.Watts:
        str = (string) UI.UNITSUFFIXES.ELECTRICAL.WATT;
        break;
      case GameUtil.WattageFormatterUnit.Kilowatts:
        watts /= 1000f;
        str = (string) UI.UNITSUFFIXES.ELECTRICAL.KILOWATT;
        break;
      case GameUtil.WattageFormatterUnit.Automatic:
        if ((double) Mathf.Abs(watts) > 1000.0)
        {
          watts /= 1000f;
          str = (string) UI.UNITSUFFIXES.ELECTRICAL.KILOWATT;
          break;
        }
        str = (string) UI.UNITSUFFIXES.ELECTRICAL.WATT;
        break;
    }
    GameUtil.AppendFloatToString(builder, watts, "###0.##");
    if (!displayUnits || str == null)
      return;
    builder.Append(str);
  }

  public static string GetFormattedWattage(
    float watts,
    GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Automatic,
    bool displayUnits = true)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedWattage(stringBuilder, watts, unit, displayUnits);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static void AppendFormattedHeatEnergy(
    StringBuilder builder,
    float dtu,
    GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
  {
    string str;
    string format;
    switch (unit)
    {
      case GameUtil.HeatEnergyFormatterUnit.DTU_S:
        str = (string) UI.UNITSUFFIXES.HEAT.DTU;
        format = "###0.";
        break;
      case GameUtil.HeatEnergyFormatterUnit.KDTU_S:
        dtu /= 1000f;
        str = (string) UI.UNITSUFFIXES.HEAT.KDTU;
        format = "###0.##";
        break;
      default:
        if ((double) Mathf.Abs(dtu) > 1000.0)
        {
          dtu /= 1000f;
          str = (string) UI.UNITSUFFIXES.HEAT.KDTU;
          format = "###0.##";
          break;
        }
        str = (string) UI.UNITSUFFIXES.HEAT.DTU;
        format = "###0.";
        break;
    }
    GameUtil.AppendFloatToString(builder, dtu, format);
    builder.Append(str);
  }

  public static string GetFormattedHeatEnergy(float dtu, GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedHeatEnergy(stringBuilder, dtu, unit);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static void AppendFormattedHeatEnergyRate(
    StringBuilder builder,
    float dtu_s,
    GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
  {
    string str = (string) null;
    switch (unit)
    {
      case GameUtil.HeatEnergyFormatterUnit.DTU_S:
        str = (string) UI.UNITSUFFIXES.HEAT.DTU_S;
        break;
      case GameUtil.HeatEnergyFormatterUnit.KDTU_S:
        dtu_s /= 1000f;
        str = (string) UI.UNITSUFFIXES.HEAT.KDTU_S;
        break;
      case GameUtil.HeatEnergyFormatterUnit.Automatic:
        if ((double) Mathf.Abs(dtu_s) > 1000.0)
        {
          dtu_s /= 1000f;
          str = (string) UI.UNITSUFFIXES.HEAT.KDTU_S;
          break;
        }
        str = (string) UI.UNITSUFFIXES.HEAT.DTU_S;
        break;
    }
    GameUtil.AppendFloatToString(builder, dtu_s);
    if (str == null)
      return;
    builder.Append(str);
  }

  public static string GetFormattedHeatEnergyRate(
    float dtu_s,
    GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedHeatEnergyRate(stringBuilder, dtu_s, unit);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static string GetFormattedInt(float num, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    num = GameUtil.ApplyTimeSlice(num, timeSlice);
    return GameUtil.AddTimeSliceText(GameUtil.FloatToString(num, "F0"), timeSlice);
  }

  public static string GetSpeciesNameFromGameObject(GameObject critterGameObject)
  {
    CreatureBrain component = critterGameObject.GetComponent<CreatureBrain>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null ? GameUtil.GetNameForSpecies(component.species) : "UNKNOWN SPECIES";
  }

  public static string GetNameForSpecies(Tag species)
  {
    Option<string> none = (Option<string>) Option.None;
    return (!(species == GameTags.Creatures.Species.HatchSpecies) ? (!(species == GameTags.Creatures.Species.LightBugSpecies) ? (!(species == GameTags.Creatures.Species.OilFloaterSpecies) ? (!(species == GameTags.Creatures.Species.DreckoSpecies) ? (!(species == GameTags.Creatures.Species.GlomSpecies) ? (!(species == GameTags.Creatures.Species.PuftSpecies) ? (!(species == GameTags.Creatures.Species.PacuSpecies) ? (!(species == GameTags.Creatures.Species.MooSpecies) ? (!(species == GameTags.Creatures.Species.MoleSpecies) ? (!(species == GameTags.Creatures.Species.SquirrelSpecies) ? (!(species == GameTags.Creatures.Species.CrabSpecies) ? (!(species == GameTags.Creatures.Species.DivergentSpecies) ? (!(species == GameTags.Creatures.Species.StaterpillarSpecies) ? (!(species == GameTags.Creatures.Species.BeetaSpecies) ? (!(species == GameTags.Creatures.Species.BellySpecies) ? (!(species == GameTags.Creatures.Species.SealSpecies) ? (!(species == GameTags.Creatures.Species.DeerSpecies) ? (!(species == GameTags.Creatures.Species.RaptorSpecies) ? (!(species == GameTags.Creatures.Species.ChameleonSpecies) ? (!(species == GameTags.Creatures.Species.PrehistoricPacuSpecies) ? (!(species == GameTags.Creatures.Species.StegoSpecies) ? (!(species == GameTags.Creatures.Species.ButterflySpecies) ? (!(species == GameTags.Creatures.Species.ParrotFishSpecies) ? (!(species == GameTags.Creatures.Species.SnailSpecies) ? (!(species == GameTags.Creatures.Species.SquidSpecies) ? (!(species == GameTags.Creatures.Species.PufferFishSpecies) ? (!(species == GameTags.Creatures.Species.SeaFairySpecies) ? (!(species == GameTags.Creatures.Species.SeaTurtleSpecies) ? (!(species == GameTags.Creatures.Species.SeaHorseSpecies) ? (Option<string>) Option.None : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.SEAHORSESPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.SEATURTLESPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.SEAFAIRYSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.PUFFERFISHSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.SQUIDSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.SNAILSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.PARROTFISHSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.BUTTERFLYSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.STEGOSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.PREHISTORICPACUSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.CHAMELEONSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.RAPTORSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.DEERSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.SEALSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.BELLYSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.BEETASPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.STATERPILLARSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.DIVERGENTSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.CRABSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.SQUIRRELSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.MOLESPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.MOOSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.PACUSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.PUFTSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.GLOMSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.DRECKOSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.OILFLOATERSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.LIGHTBUGSPECIES)) : Option.Some<string>((string) STRINGS.CREATURES.FAMILY_PLURAL.HATCHSPECIES)).Value;
  }

  public static void AppendFormattedSimple(
    StringBuilder builder,
    float num,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    string formatString = null)
  {
    num = GameUtil.ApplyTimeSlice(num, timeSlice);
    if (formatString != null)
      GameUtil.AppendFloatToString(builder, num, formatString);
    else if ((double) num == 0.0)
      builder.Append("0");
    else if ((double) Mathf.Abs(num) < 1.0)
      GameUtil.AppendFloatToString(builder, num, "#,##0.##");
    else if ((double) Mathf.Abs(num) < 10.0)
      GameUtil.AppendFloatToString(builder, num, "#,###.##");
    else
      GameUtil.AppendFloatToString(builder, num, "#,###.##");
    GameUtil.AddTimeSliceText(builder, timeSlice);
  }

  public static string GetFormattedSimple(
    float num,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    string formatString = null)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedSimple(stringBuilder, num, timeSlice, formatString);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static void AppendFormattedLux(StringBuilder builder, int lux)
  {
    builder.Append(lux);
    builder.Append((string) UI.UNITSUFFIXES.LIGHT.LUX);
  }

  public static string GetFormattedLux(int lux)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedLux(stringBuilder, lux);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static string GetLightDescription(int lux)
  {
    if (lux == 0)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.NO_LIGHT;
    if (lux < DUPLICANTSTATS.STANDARD.Light.LOW_LIGHT)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.VERY_LOW_LIGHT;
    if (lux < DUPLICANTSTATS.STANDARD.Light.MEDIUM_LIGHT)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.LOW_LIGHT;
    if (lux < DUPLICANTSTATS.STANDARD.Light.HIGH_LIGHT)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.MEDIUM_LIGHT;
    if (lux < DUPLICANTSTATS.STANDARD.Light.VERY_HIGH_LIGHT)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.HIGH_LIGHT;
    return lux < DUPLICANTSTATS.STANDARD.Light.MAX_LIGHT ? (string) UI.OVERLAYS.LIGHTING.RANGES.VERY_HIGH_LIGHT : (string) UI.OVERLAYS.LIGHTING.RANGES.MAX_LIGHT;
  }

  public static string GetRadiationDescription(float radsPerCycle)
  {
    if ((double) radsPerCycle == 0.0)
      return (string) UI.OVERLAYS.RADIATION.RANGES.NONE;
    if ((double) radsPerCycle < 100.0)
      return (string) UI.OVERLAYS.RADIATION.RANGES.VERY_LOW;
    if ((double) radsPerCycle < 200.0)
      return (string) UI.OVERLAYS.RADIATION.RANGES.LOW;
    if ((double) radsPerCycle < 400.0)
      return (string) UI.OVERLAYS.RADIATION.RANGES.MEDIUM;
    if ((double) radsPerCycle < 2000.0)
      return (string) UI.OVERLAYS.RADIATION.RANGES.HIGH;
    return (double) radsPerCycle < 4000.0 ? (string) UI.OVERLAYS.RADIATION.RANGES.VERY_HIGH : (string) UI.OVERLAYS.RADIATION.RANGES.MAX;
  }

  public static void AppendFormattedByTag(
    StringBuilder builder,
    Tag tag,
    float amount,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    if (GameTags.DisplayAsCalories.Contains(tag))
      GameUtil.AppendFormattedCaloriesForItem(builder, tag, amount, timeSlice);
    else if (GameTags.DisplayAsUnits.Contains(tag))
      GameUtil.AppendFormattedUnits(builder, amount, timeSlice);
    else
      GameUtil.AppendFormattedMass(builder, amount, timeSlice);
  }

  public static string GetFormattedByTag(Tag tag, float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    return GameUtil.GetFormattedByTag(tag, amount, true, timeSlice);
  }

  public static string GetFormattedByTag(
    Tag tag,
    float amount,
    bool showSuffix,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    if (GameTags.DisplayAsCalories.Contains(tag))
      return GameUtil.GetFormattedCaloriesForItem(tag, amount, showSuffix, timeSlice);
    return GameTags.DisplayAsUnits.Contains(tag) ? GameUtil.GetFormattedUnits(amount, timeSlice, showSuffix) : GameUtil.GetFormattedMass(amount, timeSlice, includeSuffix: showSuffix);
  }

  public static string GetFormattedFoodQuality(int quality)
  {
    if (GameUtil.adjectives == null)
      GameUtil.adjectives = LocString.GetStrings(typeof (DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVES));
    LocString format = quality >= 0 ? DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_FORMAT_POSITIVE : DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_FORMAT_NEGATIVE;
    int index = Mathf.Clamp(quality - DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_INDEX_OFFSET, 0, GameUtil.adjectives.Length);
    return string.Format((string) format, (object) GameUtil.adjectives[index], (object) GameUtil.AddPositiveSign(quality.ToString(), quality > 0));
  }

  public static string GetFormattedBytes(ulong amount)
  {
    string[] strArray = new string[5]
    {
      (string) UI.UNITSUFFIXES.INFORMATION.BYTE,
      (string) UI.UNITSUFFIXES.INFORMATION.KILOBYTE,
      (string) UI.UNITSUFFIXES.INFORMATION.MEGABYTE,
      (string) UI.UNITSUFFIXES.INFORMATION.GIGABYTE,
      (string) UI.UNITSUFFIXES.INFORMATION.TERABYTE
    };
    int y = amount == 0UL ? 0 : (int) Math.Floor(Math.Floor(Math.Log((double) amount)) / Math.Log(1024.0));
    double num = (double) amount / Math.Pow(1024.0, (double) y);
    Debug.Assert(y >= 0 && y < strArray.Length);
    return $"{num:F} {strArray[y]}";
  }

  public static string GetFormattedInfomation(float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    amount = GameUtil.ApplyTimeSlice(amount, timeSlice);
    string str = "";
    if ((double) amount < 1024.0)
      str = (string) UI.UNITSUFFIXES.INFORMATION.KILOBYTE;
    else if ((double) amount < 1048576.0)
    {
      amount /= 1000f;
      str = (string) UI.UNITSUFFIXES.INFORMATION.MEGABYTE;
    }
    else if ((double) amount < 1073741824.0)
    {
      amount /= 1048576f;
      str = (string) UI.UNITSUFFIXES.INFORMATION.GIGABYTE;
    }
    return GameUtil.AddTimeSliceText(amount.ToString() + str, timeSlice);
  }

  public static LocString GetCurrentMassUnit(bool useSmallUnit = false)
  {
    LocString currentMassUnit = (LocString) null;
    switch (GameUtil.massUnit)
    {
      case GameUtil.MassUnit.Kilograms:
        currentMassUnit = !useSmallUnit ? UI.UNITSUFFIXES.MASS.KILOGRAM : UI.UNITSUFFIXES.MASS.GRAM;
        break;
      case GameUtil.MassUnit.Pounds:
        currentMassUnit = UI.UNITSUFFIXES.MASS.POUND;
        break;
    }
    return currentMassUnit;
  }

  public static void AppendFormattedMass(
    StringBuilder builder,
    float mass,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    GameUtil.MetricMassFormat massFormat = GameUtil.MetricMassFormat.UseThreshold,
    bool includeSuffix = true,
    string floatFormat = "{0:0.#}")
  {
    if ((double) mass == -3.4028234663852886E+38)
      builder.Append((string) UI.CALCULATING);
    else if (float.IsPositiveInfinity(mass))
    {
      builder.Append((string) UI.POS_INFINITY);
      builder.Append((string) UI.UNITSUFFIXES.MASS.TONNE);
    }
    else if (float.IsNegativeInfinity(mass))
    {
      builder.Append((string) UI.NEG_INFINITY);
      builder.Append((string) UI.UNITSUFFIXES.MASS.TONNE);
    }
    else
    {
      mass = GameUtil.ApplyTimeSlice(mass, timeSlice);
      string str;
      if (GameUtil.massUnit == GameUtil.MassUnit.Kilograms)
      {
        str = (string) UI.UNITSUFFIXES.MASS.TONNE;
        switch (massFormat)
        {
          case GameUtil.MetricMassFormat.UseThreshold:
            float num = Mathf.Abs(mass);
            if (0.0 < (double) num)
            {
              if ((double) num < 4.9999998736893758E-06)
              {
                str = (string) UI.UNITSUFFIXES.MASS.MICROGRAM;
                mass = Mathf.Floor(mass * 1E+09f);
                break;
              }
              if ((double) num < 0.004999999888241291)
              {
                mass *= 1000000f;
                str = (string) UI.UNITSUFFIXES.MASS.MILLIGRAM;
                break;
              }
              if ((double) Mathf.Abs(mass) < 5.0)
              {
                mass *= 1000f;
                str = (string) UI.UNITSUFFIXES.MASS.GRAM;
                break;
              }
              if ((double) Mathf.Abs(mass) < 5000.0)
              {
                str = (string) UI.UNITSUFFIXES.MASS.KILOGRAM;
                break;
              }
              mass /= 1000f;
              str = (string) UI.UNITSUFFIXES.MASS.TONNE;
              break;
            }
            str = (string) UI.UNITSUFFIXES.MASS.KILOGRAM;
            break;
          case GameUtil.MetricMassFormat.Kilogram:
            str = (string) UI.UNITSUFFIXES.MASS.KILOGRAM;
            break;
          case GameUtil.MetricMassFormat.Gram:
            mass *= 1000f;
            str = (string) UI.UNITSUFFIXES.MASS.GRAM;
            break;
          case GameUtil.MetricMassFormat.Tonne:
            mass /= 1000f;
            str = (string) UI.UNITSUFFIXES.MASS.TONNE;
            break;
        }
      }
      else
      {
        mass /= 2.2f;
        str = (string) UI.UNITSUFFIXES.MASS.POUND;
        if (massFormat == GameUtil.MetricMassFormat.UseThreshold)
        {
          float num = Mathf.Abs(mass);
          if ((double) num < 5.0 && (double) num > 1.0 / 1000.0)
          {
            mass *= 256f;
            str = (string) UI.UNITSUFFIXES.MASS.DRACHMA;
          }
          else
          {
            mass *= 7000f;
            str = (string) UI.UNITSUFFIXES.MASS.GRAIN;
          }
        }
      }
      builder.AppendFormat(floatFormat, (object) mass);
      if (!includeSuffix)
        return;
      builder.Append(str);
      GameUtil.AddTimeSliceText(builder, timeSlice);
    }
  }

  public static string GetFormattedMass(
    float mass,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    GameUtil.MetricMassFormat massFormat = GameUtil.MetricMassFormat.UseThreshold,
    bool includeSuffix = true,
    string floatFormat = "{0:0.#}")
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedMass(stringBuilder, mass, timeSlice, massFormat, includeSuffix, floatFormat);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static void AppendFormattedTime(StringBuilder builder, float seconds)
  {
    builder.AppendFormat((string) UI.FORMATSECONDS, (object) (int) seconds);
  }

  public static string GetFormattedTime(float seconds, string floatFormat = "F0")
  {
    return string.Format((string) UI.FORMATSECONDS, (object) seconds.ToString(floatFormat));
  }

  public static void AppendFormattedEngineEfficiency(StringBuilder builder, float amount)
  {
    builder.Append(amount);
    builder.Append(" km /");
    builder.Append((string) UI.UNITSUFFIXES.MASS.KILOGRAM);
  }

  public static string GetFormattedEngineEfficiency(float amount)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedEngineEfficiency(stringBuilder, amount);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static void AppendFormattedDistance(StringBuilder builder, float meters)
  {
    if ((double) Mathf.Abs(meters) < 1.0)
      builder.AppendFormat("{0:0.0} cm", (object) Math.Abs(meters * 100f));
    else if ((double) meters < 1000.0)
    {
      builder.Append(meters);
      builder.Append(" m");
    }
    else
      builder.AppendFormat("{0:0.0} km", (object) (float) ((double) meters / 1000.0));
  }

  public static string GetFormattedDistance(float meters)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedDistance(stringBuilder, meters);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static void AppendFormattedCycles(StringBuilder builder, float seconds, bool forceCycles = false)
  {
    if (forceCycles || (double) Math.Abs(seconds) > 100.0)
      builder.AppendFormat((string) UI.FORMATDAY, (object) (float) ((double) seconds / 600.0));
    else
      GameUtil.AppendFormattedTime(builder, seconds);
  }

  public static string GetFormattedCycles(float seconds, string formatString = "F1", bool forceCycles = false)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendFormattedCycles(stringBuilder, seconds, forceCycles);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static float GetDisplaySHC(float shc)
  {
    if (GameUtil.temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
      shc /= 1.8f;
    return shc;
  }

  public static string GetSHCSuffix() => $"(DTU/g)/{GameUtil.GetTemperatureUnitSuffix()}";

  public static string GetFormattedSHC(float shc)
  {
    shc = GameUtil.GetDisplaySHC(shc);
    return $"{shc.ToString("0.000")} (DTU/g)/{GameUtil.GetTemperatureUnitSuffix()}";
  }

  public static float GetDisplayThermalConductivity(float tc)
  {
    if (GameUtil.temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
      tc /= 1.8f;
    return tc;
  }

  public static string GetThermalConductivitySuffix()
  {
    return $"(DTU/(m*s))/{GameUtil.GetTemperatureUnitSuffix()}";
  }

  public static string GetFormattedThermalConductivity(float tc)
  {
    tc = GameUtil.GetDisplayThermalConductivity(tc);
    return $"{tc.ToString("0.000")} (DTU/(m*s))/{GameUtil.GetTemperatureUnitSuffix()}";
  }

  public static string GetElementNameByElementHash(SimHashes elementHash)
  {
    return ElementLoader.FindElementByHash(elementHash).tag.ProperName();
  }

  public static string SafeStringFormat(string source, params object[] args)
  {
    for (int index = 0; index < args.Length; ++index)
    {
      string oldValue = $"{{{index.ToString()}}}";
      if (!source.Contains(oldValue))
        KCrashReporter.ReportDevNotification($"Format error in string: \"{source}\". Source is missing the {{{index}}} format marker for argument \"{args[index]}\" insertion.", Environment.StackTrace);
      else
        source = source.Replace(oldValue, args[index].ToString());
    }
    return source;
  }

  public static bool HasTrait(GameObject go, string traitName)
  {
    Traits component = go.GetComponent<Traits>();
    return !((UnityEngine.Object) component == (UnityEngine.Object) null) && component.HasTrait(traitName);
  }

  public static float GetRadiationAbsorptionPercentage(int cell)
  {
    return Grid.IsValidCell(cell) ? GameUtil.GetRadiationAbsorptionPercentage(Grid.Element[cell], Grid.Mass[cell], Grid.IsSolidCell(cell) && ((int) Grid.Properties[cell] & 128 /*0x80*/) == 128 /*0x80*/) : 0.0f;
  }

  public static float GetRadiationAbsorptionPercentage(
    Element elem,
    float mass,
    bool isConstructed)
  {
    float num1 = 2000f;
    float num2 = 0.3f;
    float num3 = 0.7f;
    float num4 = 0.8f;
    return Mathf.Clamp(!isConstructed ? (float) ((double) elem.radiationAbsorptionFactor * (double) num2 + (double) mass / (double) num1 * (double) elem.radiationAbsorptionFactor * (double) num3) : elem.radiationAbsorptionFactor * num4, 0.0f, 1f);
  }

  public static void AppendHardnessString(StringBuilder builder, Element element, bool addColor = true)
  {
    if (!element.IsSolid)
    {
      builder.Append((string) ELEMENTS.HARDNESS.NA);
    }
    else
    {
      Color firmColor = GameUtil.Hardness.firmColor;
      Color c;
      string format;
      if (element.hardness >= byte.MaxValue)
      {
        c = GameUtil.Hardness.ImpenetrableColor;
        format = (string) ELEMENTS.HARDNESS.IMPENETRABLE;
      }
      else if (element.hardness >= (byte) 150)
      {
        c = GameUtil.Hardness.nearlyImpenetrableColor;
        format = (string) ELEMENTS.HARDNESS.NEARLYIMPENETRABLE;
      }
      else if (element.hardness >= (byte) 50)
      {
        c = GameUtil.Hardness.veryFirmColor;
        format = (string) ELEMENTS.HARDNESS.VERYFIRM;
      }
      else if (element.hardness >= (byte) 25)
      {
        c = GameUtil.Hardness.firmColor;
        format = (string) ELEMENTS.HARDNESS.FIRM;
      }
      else if (element.hardness >= (byte) 10)
      {
        c = GameUtil.Hardness.softColor;
        format = (string) ELEMENTS.HARDNESS.SOFT;
      }
      else
      {
        c = GameUtil.Hardness.verySoftColor;
        format = (string) ELEMENTS.HARDNESS.VERYSOFT;
      }
      if (addColor)
        builder.AppendFormat("<color=#{0}>", (object) c.ToHexString());
      builder.AppendFormat(format, (object) element.hardness);
      if (!addColor)
        return;
      builder.Append("</color>");
    }
  }

  public static string GetHardnessString(Element element, bool addColor = true)
  {
    StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
    GameUtil.AppendHardnessString(stringBuilder, element, addColor);
    return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
  }

  public static string GetGermResistanceModifierString(float modifier, bool addColor = true)
  {
    Color c = Color.black;
    string resistanceModifierString = "";
    if ((double) modifier > 0.0)
    {
      if ((double) modifier >= 5.0)
      {
        c = GameUtil.GermResistanceValues.PositiveLargeColor;
        resistanceModifierString = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_LARGE, (object) modifier);
      }
      else if ((double) modifier >= 2.0)
      {
        c = GameUtil.GermResistanceValues.PositiveMediumColor;
        resistanceModifierString = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_MEDIUM, (object) modifier);
      }
      else if ((double) modifier > 0.0)
      {
        c = GameUtil.GermResistanceValues.PositiveSmallColor;
        resistanceModifierString = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_SMALL, (object) modifier);
      }
    }
    else if ((double) modifier < 0.0)
    {
      if ((double) modifier <= -5.0)
      {
        c = GameUtil.GermResistanceValues.NegativeLargeColor;
        resistanceModifierString = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_LARGE, (object) modifier);
      }
      else if ((double) modifier <= -2.0)
      {
        c = GameUtil.GermResistanceValues.NegativeMediumColor;
        resistanceModifierString = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_MEDIUM, (object) modifier);
      }
      else if ((double) modifier < 0.0)
      {
        c = GameUtil.GermResistanceValues.NegativeSmallColor;
        resistanceModifierString = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_SMALL, (object) modifier);
      }
    }
    else
    {
      addColor = false;
      resistanceModifierString = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NONE, (object) modifier);
    }
    if (addColor)
      resistanceModifierString = $"<color=#{c.ToHexString()}>{resistanceModifierString}</color>";
    return resistanceModifierString;
  }

  public static string GetThermalConductivityString(Element element, bool addColor = true, bool addValue = true)
  {
    Color conductivityColor1 = GameUtil.ThermalConductivityValues.mediumConductivityColor;
    Color conductivityColor2;
    string conductivityString;
    if ((double) element.thermalConductivity >= 50.0)
    {
      conductivityColor2 = GameUtil.ThermalConductivityValues.veryHighConductivityColor;
      conductivityString = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VERY_HIGH_CONDUCTIVITY;
    }
    else if ((double) element.thermalConductivity >= 10.0)
    {
      conductivityColor2 = GameUtil.ThermalConductivityValues.highConductivityColor;
      conductivityString = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.HIGH_CONDUCTIVITY;
    }
    else if ((double) element.thermalConductivity >= 2.0)
    {
      conductivityColor2 = GameUtil.ThermalConductivityValues.mediumConductivityColor;
      conductivityString = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.MEDIUM_CONDUCTIVITY;
    }
    else if ((double) element.thermalConductivity >= 1.0)
    {
      conductivityColor2 = GameUtil.ThermalConductivityValues.lowConductivityColor;
      conductivityString = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.LOW_CONDUCTIVITY;
    }
    else
    {
      conductivityColor2 = GameUtil.ThermalConductivityValues.veryLowConductivityColor;
      conductivityString = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VERY_LOW_CONDUCTIVITY;
    }
    if (addColor)
      conductivityString = $"<color=#{conductivityColor2.ToHexString()}>{conductivityString}</color>";
    if (addValue)
      conductivityString = string.Format((string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VALUE_WITH_ADJECTIVE, (object) element.thermalConductivity.ToString(), (object) conductivityString);
    return conductivityString;
  }

  public static string GetBreathableString(Element element, float Mass)
  {
    if (!element.IsGas && !element.IsVacuum)
      return "";
    Color positiveColor = GameUtil.BreathableValues.positiveColor;
    Color c;
    LocString locString;
    switch (element.id)
    {
      case SimHashes.Oxygen:
        if ((double) Mass >= (double) SimDebugView.optimallyBreathable)
        {
          c = GameUtil.BreathableValues.positiveColor;
          locString = UI.OVERLAYS.OXYGEN.LEGEND1;
          break;
        }
        if ((double) Mass >= (double) SimDebugView.minimumBreathable + ((double) SimDebugView.optimallyBreathable - (double) SimDebugView.minimumBreathable) / 2.0)
        {
          c = GameUtil.BreathableValues.positiveColor;
          locString = UI.OVERLAYS.OXYGEN.LEGEND2;
          break;
        }
        if ((double) Mass >= (double) SimDebugView.minimumBreathable)
        {
          c = GameUtil.BreathableValues.warningColor;
          locString = UI.OVERLAYS.OXYGEN.LEGEND3;
          break;
        }
        c = GameUtil.BreathableValues.negativeColor;
        locString = UI.OVERLAYS.OXYGEN.LEGEND4;
        break;
      case SimHashes.ContaminatedOxygen:
        if ((double) Mass >= (double) SimDebugView.optimallyBreathable)
        {
          c = GameUtil.BreathableValues.positiveColor;
          locString = UI.OVERLAYS.OXYGEN.LEGEND1;
          break;
        }
        if ((double) Mass >= (double) SimDebugView.minimumBreathable + ((double) SimDebugView.optimallyBreathable - (double) SimDebugView.minimumBreathable) / 2.0)
        {
          c = GameUtil.BreathableValues.positiveColor;
          locString = UI.OVERLAYS.OXYGEN.LEGEND2;
          break;
        }
        if ((double) Mass >= (double) SimDebugView.minimumBreathable)
        {
          c = GameUtil.BreathableValues.warningColor;
          locString = UI.OVERLAYS.OXYGEN.LEGEND3;
          break;
        }
        c = GameUtil.BreathableValues.negativeColor;
        locString = UI.OVERLAYS.OXYGEN.LEGEND4;
        break;
      default:
        c = GameUtil.BreathableValues.negativeColor;
        locString = UI.OVERLAYS.OXYGEN.LEGEND4;
        break;
    }
    return string.Format((string) ELEMENTS.BREATHABLEDESC, (object) c.ToHexString(), (object) locString);
  }

  public static string GetWireLoadColor(float load, float maxLoad, float potentialLoad)
  {
    return ((double) load <= (double) maxLoad + (double) TUNING.POWER.FLOAT_FUDGE_FACTOR ? ((double) potentialLoad <= (double) maxLoad || (double) load / (double) maxLoad < 0.75 ? Color.white : GameUtil.WireLoadValues.warningColor) : GameUtil.WireLoadValues.negativeColor).ToHexString();
  }

  public static string GetHotkeyString(Action action)
  {
    return KInputManager.currentControllerIsGamepad ? UI.FormatAsHotkey(GameUtil.GetActionString(action)) : UI.FormatAsHotkey($"[{GameUtil.GetActionString(action)}]");
  }

  public static string ReplaceHotkeyString(string template, Action action)
  {
    return template.Replace("{Hotkey}", GameUtil.GetHotkeyString(action));
  }

  public static string ReplaceHotkeyString(string template, Action action1, Action action2)
  {
    return template.Replace("{Hotkey}", GameUtil.GetHotkeyString(action1) + GameUtil.GetHotkeyString(action2));
  }

  public static string GetKeycodeLocalized(KKeyCode key_code)
  {
    string keycodeLocalized = key_code.ToString();
    switch (key_code)
    {
      case KKeyCode.None:
        return keycodeLocalized;
      case KKeyCode.Backspace:
        keycodeLocalized = (string) INPUT.BACKSPACE;
        goto case KKeyCode.None;
      case KKeyCode.Tab:
        keycodeLocalized = (string) INPUT.TAB;
        goto case KKeyCode.None;
      case KKeyCode.Return:
        keycodeLocalized = (string) INPUT.ENTER;
        goto case KKeyCode.None;
      case KKeyCode.Escape:
        keycodeLocalized = (string) INPUT.ESCAPE;
        goto case KKeyCode.None;
      case KKeyCode.Space:
        keycodeLocalized = (string) INPUT.SPACE;
        goto case KKeyCode.None;
      case KKeyCode.Plus:
        keycodeLocalized = "+";
        goto case KKeyCode.None;
      case KKeyCode.Comma:
        keycodeLocalized = ",";
        goto case KKeyCode.None;
      case KKeyCode.Minus:
        keycodeLocalized = "-";
        goto case KKeyCode.None;
      case KKeyCode.Period:
        keycodeLocalized = (string) INPUT.PERIOD;
        goto case KKeyCode.None;
      case KKeyCode.Slash:
        keycodeLocalized = "/";
        goto case KKeyCode.None;
      case KKeyCode.Colon:
        keycodeLocalized = ":";
        goto case KKeyCode.None;
      case KKeyCode.Semicolon:
        keycodeLocalized = ";";
        goto case KKeyCode.None;
      case KKeyCode.Equals:
        keycodeLocalized = "=";
        goto case KKeyCode.None;
      case KKeyCode.LeftBracket:
        keycodeLocalized = "[";
        goto case KKeyCode.None;
      case KKeyCode.Backslash:
        keycodeLocalized = "\\";
        goto case KKeyCode.None;
      case KKeyCode.RightBracket:
        keycodeLocalized = "]";
        goto case KKeyCode.None;
      case KKeyCode.BackQuote:
        keycodeLocalized = (string) INPUT.BACKQUOTE;
        goto case KKeyCode.None;
      case KKeyCode.Keypad0:
        keycodeLocalized = (string) INPUT.NUM + " 0";
        goto case KKeyCode.None;
      case KKeyCode.Keypad1:
        keycodeLocalized = (string) INPUT.NUM + " 1";
        goto case KKeyCode.None;
      case KKeyCode.Keypad2:
        keycodeLocalized = (string) INPUT.NUM + " 2";
        goto case KKeyCode.None;
      case KKeyCode.Keypad3:
        keycodeLocalized = (string) INPUT.NUM + " 3";
        goto case KKeyCode.None;
      case KKeyCode.Keypad4:
        keycodeLocalized = (string) INPUT.NUM + " 4";
        goto case KKeyCode.None;
      case KKeyCode.Keypad5:
        keycodeLocalized = (string) INPUT.NUM + " 5";
        goto case KKeyCode.None;
      case KKeyCode.Keypad6:
        keycodeLocalized = (string) INPUT.NUM + " 6";
        goto case KKeyCode.None;
      case KKeyCode.Keypad7:
        keycodeLocalized = (string) INPUT.NUM + " 7";
        goto case KKeyCode.None;
      case KKeyCode.Keypad8:
        keycodeLocalized = (string) INPUT.NUM + " 8";
        goto case KKeyCode.None;
      case KKeyCode.Keypad9:
        keycodeLocalized = (string) INPUT.NUM + " 9";
        goto case KKeyCode.None;
      case KKeyCode.KeypadPeriod:
        keycodeLocalized = $"{(string) INPUT.NUM} {(string) INPUT.PERIOD}";
        goto case KKeyCode.None;
      case KKeyCode.KeypadDivide:
        keycodeLocalized = (string) INPUT.NUM + " /";
        goto case KKeyCode.None;
      case KKeyCode.KeypadMultiply:
        keycodeLocalized = (string) INPUT.NUM + " *";
        goto case KKeyCode.None;
      case KKeyCode.KeypadMinus:
        keycodeLocalized = (string) INPUT.NUM + " -";
        goto case KKeyCode.None;
      case KKeyCode.KeypadPlus:
        keycodeLocalized = (string) INPUT.NUM + " +";
        goto case KKeyCode.None;
      case KKeyCode.KeypadEnter:
        keycodeLocalized = $"{(string) INPUT.NUM} {(string) INPUT.ENTER}";
        goto case KKeyCode.None;
      case KKeyCode.Insert:
        keycodeLocalized = (string) INPUT.INSERT;
        goto case KKeyCode.None;
      case KKeyCode.RightShift:
        keycodeLocalized = (string) INPUT.RIGHT_SHIFT;
        goto case KKeyCode.None;
      case KKeyCode.LeftShift:
        keycodeLocalized = (string) INPUT.LEFT_SHIFT;
        goto case KKeyCode.None;
      case KKeyCode.RightControl:
        keycodeLocalized = (string) INPUT.RIGHT_CTRL;
        goto case KKeyCode.None;
      case KKeyCode.LeftControl:
        keycodeLocalized = (string) INPUT.LEFT_CTRL;
        goto case KKeyCode.None;
      case KKeyCode.RightAlt:
        keycodeLocalized = (string) INPUT.RIGHT_ALT;
        goto case KKeyCode.None;
      case KKeyCode.LeftAlt:
        keycodeLocalized = (string) INPUT.LEFT_ALT;
        goto case KKeyCode.None;
      case KKeyCode.Mouse0:
        keycodeLocalized = (string) INPUT.MOUSE + " 0";
        goto case KKeyCode.None;
      case KKeyCode.Mouse1:
        keycodeLocalized = (string) INPUT.MOUSE + " 1";
        goto case KKeyCode.None;
      case KKeyCode.Mouse2:
        keycodeLocalized = (string) INPUT.MOUSE + " 2";
        goto case KKeyCode.None;
      case KKeyCode.Mouse3:
        keycodeLocalized = (string) INPUT.MOUSE + " 3";
        goto case KKeyCode.None;
      case KKeyCode.Mouse4:
        keycodeLocalized = (string) INPUT.MOUSE + " 4";
        goto case KKeyCode.None;
      case KKeyCode.Mouse5:
        keycodeLocalized = (string) INPUT.MOUSE + " 5";
        goto case KKeyCode.None;
      case KKeyCode.Mouse6:
        keycodeLocalized = (string) INPUT.MOUSE + " 6";
        goto case KKeyCode.None;
      case KKeyCode.MouseScrollDown:
        keycodeLocalized = (string) INPUT.MOUSE_SCROLL_DOWN;
        goto case KKeyCode.None;
      case KKeyCode.MouseScrollUp:
        keycodeLocalized = (string) INPUT.MOUSE_SCROLL_UP;
        goto case KKeyCode.None;
      default:
        if (KKeyCode.A <= key_code && key_code <= KKeyCode.Z)
        {
          keycodeLocalized = ((char) (65 + (key_code - 97))).ToString();
          goto case KKeyCode.None;
        }
        if (KKeyCode.Alpha0 <= key_code && key_code <= KKeyCode.Alpha9)
        {
          keycodeLocalized = ((char) (48 /*0x30*/ + (key_code - 48 /*0x30*/))).ToString();
          goto case KKeyCode.None;
        }
        if (KKeyCode.F1 <= key_code && key_code <= KKeyCode.F12)
        {
          keycodeLocalized = "F" + ((int) (key_code - 282 + 1)).ToString();
          goto case KKeyCode.None;
        }
        Debug.LogWarning((object) $"Unable to find proper string for KKeyCode: {key_code.ToString()} using key_code.ToString()");
        goto case KKeyCode.None;
    }
  }

  public static string GetActionString(Action action)
  {
    string actionString = "";
    if (action == Action.NumActions)
      return actionString;
    BindingEntry binding = GameUtil.ActionToBinding(action);
    KKeyCode mKeyCode = binding.mKeyCode;
    if (KInputManager.currentControllerIsGamepad)
      return KInputManager.steamInputInterpreter.GetActionGlyph(action);
    if (binding.mModifier == Modifier.None)
      return GameUtil.GetKeycodeLocalized(mKeyCode).ToUpper();
    string str = "";
    switch (binding.mModifier)
    {
      case Modifier.Alt:
        str = INPUT.ALT.ToString();
        break;
      case Modifier.Ctrl:
        str = INPUT.CTRL.ToString();
        break;
      case Modifier.Shift:
        str = INPUT.SHIFT.ToString();
        break;
      case Modifier.CapsLock:
        str = GameUtil.GetKeycodeLocalized(KKeyCode.CapsLock);
        break;
      case Modifier.Backtick:
        str = GameUtil.GetKeycodeLocalized(KKeyCode.BackQuote);
        break;
    }
    return $"{str} + {GameUtil.GetKeycodeLocalized(mKeyCode)}".ToUpper();
  }

  public static void CreateExplosion(Vector3 explosion_pos)
  {
    Vector2 vector2 = new Vector2(explosion_pos.x, explosion_pos.y);
    double num1 = 5.0;
    float num2 = (float) (num1 * num1);
    foreach (Health health in Components.Health.Items)
    {
      Vector3 position = health.transform.GetPosition();
      float sqrMagnitude = (new Vector2(position.x, position.y) - vector2).sqrMagnitude;
      if ((double) num2 >= (double) sqrMagnitude && (UnityEngine.Object) health != (UnityEngine.Object) null)
        health.Damage(health.maxHitPoints);
    }
  }

  private static void GetNonSolidCells(
    int x,
    int y,
    List<int> cells,
    int min_x,
    int min_y,
    int max_x,
    int max_y)
  {
    int cell = Grid.XYToCell(x, y);
    if (!Grid.IsValidCell(cell) || Grid.Solid[cell] || Grid.DupePassable[cell] || x < min_x || x > max_x || y < min_y || y > max_y || cells.Contains(cell))
      return;
    cells.Add(cell);
    GameUtil.GetNonSolidCells(x + 1, y, cells, min_x, min_y, max_x, max_y);
    GameUtil.GetNonSolidCells(x - 1, y, cells, min_x, min_y, max_x, max_y);
    GameUtil.GetNonSolidCells(x, y + 1, cells, min_x, min_y, max_x, max_y);
    GameUtil.GetNonSolidCells(x, y - 1, cells, min_x, min_y, max_x, max_y);
  }

  public static void GetNonSolidCells(int cell, int radius, List<int> cells)
  {
    int x = 0;
    int y = 0;
    Grid.CellToXY(cell, out x, out y);
    GameUtil.GetNonSolidCells(x, y, cells, x - radius, y - radius, x + radius, y + radius);
  }

  public static float GetMaxStressInActiveWorld()
  {
    if (Components.LiveMinionIdentities.Count <= 0)
      return 0.0f;
    float a = 0.0f;
    foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
    {
      if (!minionIdentity.IsNullOrDestroyed() && minionIdentity.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
      {
        AmountInstance amountInstance = Db.Get().Amounts.Stress.Lookup((Component) minionIdentity);
        if (amountInstance != null)
          a = Mathf.Max(a, amountInstance.value);
      }
    }
    return a;
  }

  public static float GetAverageStressInActiveWorld()
  {
    if (Components.LiveMinionIdentities.Count <= 0)
      return 0.0f;
    float num1 = 0.0f;
    int num2 = 0;
    foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
    {
      if (!minionIdentity.IsNullOrDestroyed() && minionIdentity.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
      {
        num1 += Db.Get().Amounts.Stress.Lookup((Component) minionIdentity).value;
        ++num2;
      }
    }
    return num1 / (float) num2;
  }

  public static string MigrateFMOD(FMODAsset asset)
  {
    if ((UnityEngine.Object) asset == (UnityEngine.Object) null)
      return (string) null;
    return asset.path == null ? asset.name : asset.path;
  }

  private static void SortGameObjectDescriptors(List<IGameObjectEffectDescriptor> descriptorList)
  {
    descriptorList.Sort((Comparison<IGameObjectEffectDescriptor>) ((e1, e2) => TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.IndexOf(e1.GetType()).CompareTo(TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.IndexOf(e2.GetType()))));
  }

  public static void IndentListOfDescriptors(List<Descriptor> list, int indentCount = 1)
  {
    for (int index1 = 0; index1 < list.Count; ++index1)
    {
      Descriptor descriptor = list[index1];
      for (int index2 = 0; index2 < indentCount; ++index2)
        descriptor.IncreaseIndent();
      list[index1] = descriptor;
    }
  }

  public static List<Descriptor> GetAllDescriptors(GameObject go, bool simpleInfoScreen = false)
  {
    List<Descriptor> allDescriptors = new List<Descriptor>();
    List<IGameObjectEffectDescriptor> descriptorList = new List<IGameObjectEffectDescriptor>((IEnumerable<IGameObjectEffectDescriptor>) go.GetComponents<IGameObjectEffectDescriptor>());
    StateMachineController component1 = go.GetComponent<StateMachineController>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      descriptorList.AddRange((IEnumerable<IGameObjectEffectDescriptor>) component1.GetDescriptors());
    GameUtil.SortGameObjectDescriptors(descriptorList);
    foreach (IGameObjectEffectDescriptor effectDescriptor in descriptorList)
    {
      List<Descriptor> descriptors = effectDescriptor.GetDescriptors(go);
      if (descriptors != null)
      {
        foreach (Descriptor descriptor in descriptors)
        {
          if (!descriptor.onlyForSimpleInfoScreen || simpleInfoScreen)
            allDescriptors.Add(descriptor);
        }
      }
    }
    KPrefabID component2 = go.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.AdditionalRequirements != null)
    {
      foreach (Descriptor additionalRequirement in component2.AdditionalRequirements)
      {
        if (!additionalRequirement.onlyForSimpleInfoScreen || simpleInfoScreen)
          allDescriptors.Add(additionalRequirement);
      }
    }
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.AdditionalEffects != null)
    {
      foreach (Descriptor additionalEffect in component2.AdditionalEffects)
      {
        if (!additionalEffect.onlyForSimpleInfoScreen || simpleInfoScreen)
          allDescriptors.Add(additionalEffect);
      }
    }
    return allDescriptors;
  }

  public static List<Descriptor> GetDetailDescriptors(List<Descriptor> descriptors)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor in descriptors)
    {
      if (descriptor.type == Descriptor.DescriptorType.Detail)
        list.Add(descriptor);
    }
    GameUtil.IndentListOfDescriptors(list);
    return list;
  }

  public static List<Descriptor> GetRequirementDescriptors(List<Descriptor> descriptors)
  {
    return GameUtil.GetRequirementDescriptors(descriptors, true);
  }

  public static List<Descriptor> GetRequirementDescriptors(
    List<Descriptor> descriptors,
    bool indent)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor in descriptors)
    {
      if (descriptor.type == Descriptor.DescriptorType.Requirement)
        list.Add(descriptor);
    }
    if (indent)
      GameUtil.IndentListOfDescriptors(list);
    return list;
  }

  public static List<Descriptor> GetEffectDescriptors(List<Descriptor> descriptors)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor in descriptors)
    {
      if (descriptor.type == Descriptor.DescriptorType.Effect || descriptor.type == Descriptor.DescriptorType.DiseaseSource)
        list.Add(descriptor);
    }
    GameUtil.IndentListOfDescriptors(list);
    return list;
  }

  public static List<Descriptor> GetInformationDescriptors(List<Descriptor> descriptors)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor in descriptors)
    {
      if (descriptor.type == Descriptor.DescriptorType.Lifecycle)
        list.Add(descriptor);
    }
    GameUtil.IndentListOfDescriptors(list);
    return list;
  }

  public static List<Descriptor> GetCropOptimumConditionDescriptors(List<Descriptor> descriptors)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor1 in descriptors)
    {
      if (descriptor1.type == Descriptor.DescriptorType.Lifecycle)
      {
        Descriptor descriptor2 = descriptor1;
        descriptor2.text = "• " + descriptor2.text;
        list.Add(descriptor2);
      }
    }
    GameUtil.IndentListOfDescriptors(list);
    return list;
  }

  public static List<Descriptor> GetGameObjectRequirements(GameObject go)
  {
    List<Descriptor> objectRequirements = new List<Descriptor>();
    List<IGameObjectEffectDescriptor> descriptorList = new List<IGameObjectEffectDescriptor>((IEnumerable<IGameObjectEffectDescriptor>) go.GetComponents<IGameObjectEffectDescriptor>());
    StateMachineController component1 = go.GetComponent<StateMachineController>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      descriptorList.AddRange((IEnumerable<IGameObjectEffectDescriptor>) component1.GetDescriptors());
    GameUtil.SortGameObjectDescriptors(descriptorList);
    foreach (IGameObjectEffectDescriptor effectDescriptor in descriptorList)
    {
      List<Descriptor> descriptors = effectDescriptor.GetDescriptors(go);
      if (descriptors != null)
      {
        foreach (Descriptor descriptor in descriptors)
        {
          if (descriptor.type == Descriptor.DescriptorType.Requirement)
            objectRequirements.Add(descriptor);
        }
      }
    }
    KPrefabID component2 = go.GetComponent<KPrefabID>();
    if (component2.AdditionalRequirements != null)
      objectRequirements.AddRange((IEnumerable<Descriptor>) component2.AdditionalRequirements);
    return objectRequirements;
  }

  public static List<Descriptor> GetGameObjectEffects(GameObject go, bool simpleInfoScreen = false)
  {
    List<Descriptor> gameObjectEffects = new List<Descriptor>();
    List<IGameObjectEffectDescriptor> descriptorList = new List<IGameObjectEffectDescriptor>((IEnumerable<IGameObjectEffectDescriptor>) go.GetComponents<IGameObjectEffectDescriptor>());
    StateMachineController component1 = go.GetComponent<StateMachineController>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      descriptorList.AddRange((IEnumerable<IGameObjectEffectDescriptor>) component1.GetDescriptors());
    GameUtil.SortGameObjectDescriptors(descriptorList);
    foreach (IGameObjectEffectDescriptor effectDescriptor in descriptorList)
    {
      List<Descriptor> descriptors = effectDescriptor.GetDescriptors(go);
      if (descriptors != null)
      {
        foreach (Descriptor descriptor in descriptors)
        {
          if ((!descriptor.onlyForSimpleInfoScreen || simpleInfoScreen) && (descriptor.type == Descriptor.DescriptorType.Effect || descriptor.type == Descriptor.DescriptorType.DiseaseSource))
            gameObjectEffects.Add(descriptor);
        }
      }
    }
    KPrefabID component2 = go.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.AdditionalEffects != null)
    {
      foreach (Descriptor additionalEffect in component2.AdditionalEffects)
      {
        if (!additionalEffect.onlyForSimpleInfoScreen || simpleInfoScreen)
          gameObjectEffects.Add(additionalEffect);
      }
    }
    return gameObjectEffects;
  }

  public static void PartitionBuildingDescriptors(
    GameObject buildingComplete,
    bool simpleInfoScreen,
    out List<Descriptor> allDescs,
    List<(ElementConverter converter, List<Descriptor> descriptors)> converterDescCache,
    List<Descriptor> nonConverterReqs,
    List<Descriptor> nonConverterEffects,
    out bool hasConverterReqs)
  {
    ElementConverter[] components = buildingComplete.GetComponents<ElementConverter>();
    allDescs = GameUtil.GetAllDescriptors(buildingComplete, simpleInfoScreen);
    hasConverterReqs = false;
    HashSetPool<string, BuildingDef>.PooledHashSet pooledHashSet = HashSetPool<string, BuildingDef>.Allocate();
    foreach (ElementConverter elementConverter in components)
    {
      if (elementConverter.consumedElements != null && elementConverter.consumedElements.Length != 0)
      {
        List<Descriptor> descriptors = elementConverter.GetDescriptors(buildingComplete);
        converterDescCache.Add((elementConverter, descriptors));
        if (descriptors != null)
        {
          foreach (Descriptor descriptor in descriptors)
          {
            pooledHashSet.Add(descriptor.text);
            if (descriptor.type == Descriptor.DescriptorType.Requirement)
              hasConverterReqs = true;
          }
        }
      }
    }
    IConverterByproduct implementingInterface = buildingComplete.GetDefImplementingInterface<IConverterByproduct>();
    if (implementingInterface != null && (double) implementingInterface.ByproductRate > 0.0)
    {
      foreach ((ElementConverter converter, List<Descriptor> descriptors) in converterDescCache)
      {
        bool flag = false;
        foreach (ElementConverter.ConsumedElement consumedElement in converter.consumedElements)
        {
          if (consumedElement.Tag == implementingInterface.ByproductAssociatedInputTag)
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          implementingInterface.GetByproductDescriptors(buildingComplete, descriptors);
          break;
        }
      }
    }
    foreach (Descriptor descriptor in allDescs)
    {
      if (!pooledHashSet.Contains(descriptor.text))
      {
        switch (descriptor.type)
        {
          case Descriptor.DescriptorType.Requirement:
            nonConverterReqs.Add(descriptor);
            continue;
          case Descriptor.DescriptorType.Effect:
          case Descriptor.DescriptorType.DiseaseSource:
            nonConverterEffects.Add(descriptor);
            continue;
          default:
            continue;
        }
      }
    }
    pooledHashSet.Recycle();
  }

  public static void BuildPartitionedRequirements(
    List<Descriptor> result,
    List<Descriptor> nonConverterReqs,
    List<(ElementConverter converter, List<Descriptor> descriptors)> converterDescCache,
    bool hasConverterReqs)
  {
    foreach (Descriptor nonConverterReq in nonConverterReqs)
    {
      nonConverterReq.IncreaseIndent();
      result.Add(nonConverterReq);
    }
    if (hasConverterReqs)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.OPERATIONINPUTS, "", Descriptor.DescriptorType.Requirement);
      descriptor.IncreaseIndent();
      result.Add(descriptor);
    }
    foreach ((ElementConverter converter, List<Descriptor> descriptors) tuple in converterDescCache)
    {
      foreach (Descriptor descriptor1 in tuple.descriptors)
      {
        if (descriptor1.type == Descriptor.DescriptorType.Requirement)
        {
          Descriptor descriptor2 = descriptor1;
          descriptor2.text = "• " + descriptor2.text;
          descriptor2.IncreaseIndent();
          descriptor2.IncreaseIndent();
          result.Add(descriptor2);
        }
      }
    }
  }

  public static void BuildPartitionedEffects(
    List<Descriptor> result,
    List<Descriptor> nonConverterEffects,
    List<(ElementConverter converter, List<Descriptor> descriptors)> converterDescCache)
  {
    foreach (Descriptor nonConverterEffect in nonConverterEffects)
    {
      nonConverterEffect.IncreaseIndent();
      result.Add(nonConverterEffect);
    }
    foreach ((ElementConverter converter, List<Descriptor> descriptors) in converterDescCache)
    {
      string str = converter.consumedElements[0].Name;
      for (int index = 1; index < converter.consumedElements.Length; ++index)
        str = $"{str}, {converter.consumedElements[index].Name}";
      Descriptor descriptor1 = new Descriptor(str + ":", "");
      descriptor1.IncreaseIndent();
      result.Add(descriptor1);
      foreach (Descriptor descriptor2 in descriptors)
      {
        if (descriptor2.type != Descriptor.DescriptorType.Requirement)
        {
          Descriptor descriptor3 = descriptor2;
          descriptor3.IncreaseIndent();
          descriptor3.IncreaseIndent();
          result.Add(descriptor3);
        }
      }
    }
  }

  public static List<Descriptor> GetPlantRequirementDescriptors(GameObject go)
  {
    List<Descriptor> requirementDescriptors1 = new List<Descriptor>();
    List<Descriptor> requirementDescriptors2 = GameUtil.GetRequirementDescriptors(GameUtil.GetAllDescriptors(go));
    if (requirementDescriptors2.Count > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.PLANTREQUIREMENTS, (string) UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTREQUIREMENTS, Descriptor.DescriptorType.Requirement);
      requirementDescriptors1.Add(descriptor);
      requirementDescriptors1.AddRange((IEnumerable<Descriptor>) requirementDescriptors2);
    }
    return requirementDescriptors1;
  }

  public static List<Descriptor> GetPlantLifeCycleDescriptors(GameObject go)
  {
    List<Descriptor> cycleDescriptors = new List<Descriptor>();
    List<Descriptor> informationDescriptors = GameUtil.GetInformationDescriptors(GameUtil.GetAllDescriptors(go));
    if (informationDescriptors.Count > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.LIFECYCLE, (string) UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTLIFECYCLE, Descriptor.DescriptorType.Lifecycle);
      cycleDescriptors.Add(descriptor);
      cycleDescriptors.AddRange((IEnumerable<Descriptor>) informationDescriptors);
    }
    return cycleDescriptors;
  }

  public static List<Descriptor> GetPlantEffectDescriptors(GameObject go)
  {
    List<Descriptor> effectDescriptors = new List<Descriptor>();
    List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(go);
    List<Descriptor> collection = new List<Descriptor>();
    collection.AddRange((IEnumerable<Descriptor>) GameUtil.GetEffectDescriptors(allDescriptors));
    if (collection.Count > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.PLANTEFFECTS, (string) UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTEFFECTS);
      effectDescriptors.Add(descriptor);
      effectDescriptors.AddRange((IEnumerable<Descriptor>) collection);
    }
    return effectDescriptors;
  }

  public static string GetGameObjectEffectsTooltipString(GameObject go)
  {
    string effectsTooltipString = "";
    List<Descriptor> gameObjectEffects = GameUtil.GetGameObjectEffects(go);
    if (gameObjectEffects.Count > 0)
      effectsTooltipString = $"{effectsTooltipString}{(string) UI.BUILDINGEFFECTS.OPERATIONEFFECTS}\n";
    foreach (Descriptor descriptor in gameObjectEffects)
      effectsTooltipString = $"{effectsTooltipString}{descriptor.IndentedText()}\n";
    return effectsTooltipString;
  }

  public static List<Descriptor> GetEquipmentEffects(EquipmentDef def)
  {
    Debug.Assert((UnityEngine.Object) def != (UnityEngine.Object) null);
    List<Descriptor> equipmentEffects = new List<Descriptor>();
    List<AttributeModifier> attributeModifiers = def.AttributeModifiers;
    if (attributeModifiers != null)
    {
      foreach (AttributeModifier attributeModifier in attributeModifiers)
      {
        string name = Db.Get().Attributes.Get(attributeModifier.AttributeId).Name;
        string formattedString = attributeModifier.GetFormattedString();
        string newValue = (double) attributeModifier.Value >= 0.0 ? "produced" : "consumed";
        string str = UI.GAMEOBJECTEFFECTS.EQUIPMENT_MODS.text.Replace("{Attribute}", name).Replace("{Style}", newValue).Replace("{Value}", formattedString);
        equipmentEffects.Add(new Descriptor(str, str));
      }
    }
    return equipmentEffects;
  }

  public static string GetRecipeDescription(Recipe recipe)
  {
    string recipeDescription = (string) null;
    if (recipe != null)
      recipeDescription = recipe.recipeDescription;
    if (recipeDescription == null)
    {
      recipeDescription = (string) RESEARCH.TYPES.MISSINGRECIPEDESC;
      Debug.LogWarning((object) "Missing recipeDescription");
    }
    return recipeDescription;
  }

  public static int GetCurrentCycle() => GameClock.Instance.GetCycle() + 1;

  public static float GetCurrentTimeInCycles() => GameClock.Instance.GetTimeInCycles() + 1f;

  public static GameObject GetActiveTelepad()
  {
    GameObject telepad = GameUtil.GetTelepad(ClusterManager.Instance.activeWorldId);
    if ((UnityEngine.Object) telepad == (UnityEngine.Object) null)
      telepad = GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id);
    return telepad;
  }

  public static GameObject GetTelepad(int worldId)
  {
    if (Components.Telepads.Count > 0)
    {
      for (int idx = 0; idx < Components.Telepads.Count; ++idx)
      {
        if (Components.Telepads[idx].GetMyWorldId() == worldId)
          return Components.Telepads[idx].gameObject;
      }
    }
    return (GameObject) null;
  }

  public static GameObject KInstantiate(
    GameObject original,
    Vector3 position,
    Grid.SceneLayer sceneLayer,
    string name = null,
    int gameLayer = 0)
  {
    return GameUtil.KInstantiate(original, position, sceneLayer, (GameObject) null, name, gameLayer);
  }

  public static GameObject KInstantiate(
    GameObject original,
    Vector3 position,
    Grid.SceneLayer sceneLayer,
    GameObject parent,
    string name = null,
    int gameLayer = 0)
  {
    position.z = Grid.GetLayerZ(sceneLayer);
    return Util.KInstantiate(original, position, Quaternion.identity, parent, name, gameLayer: gameLayer);
  }

  public static GameObject KInstantiate(
    GameObject original,
    Grid.SceneLayer sceneLayer,
    string name = null,
    int gameLayer = 0)
  {
    return GameUtil.KInstantiate(original, Vector3.zero, sceneLayer, name, gameLayer);
  }

  public static GameObject KInstantiate(
    Component original,
    Grid.SceneLayer sceneLayer,
    string name = null,
    int gameLayer = 0)
  {
    return GameUtil.KInstantiate(original.gameObject, Vector3.zero, sceneLayer, name, gameLayer);
  }

  public static unsafe void IsEmissionBlocked(
    int cell,
    out bool all_not_gaseous,
    out bool all_over_pressure)
  {
    int* numPtr = stackalloc int[4]
    {
      Grid.CellBelow(cell),
      Grid.CellLeft(cell),
      Grid.CellRight(cell),
      Grid.CellAbove(cell)
    };
    all_not_gaseous = true;
    all_over_pressure = true;
    for (int index1 = 0; index1 < 4; ++index1)
    {
      int index2 = numPtr[index1];
      if (Grid.IsValidCell(index2))
      {
        Element element = Grid.Element[index2];
        all_not_gaseous = all_not_gaseous && !element.IsGas && !element.IsVacuum;
        all_over_pressure = all_over_pressure && (!element.IsGas && !element.IsVacuum || (double) Grid.Mass[index2] >= 1.7999999523162842);
      }
    }
  }

  public static float GetDecorAtCell(int cell) => GameUtil.GetDecorAtCell(cell, true);

  public static float GetDecorAtCell(int cell, bool includeLightDecor)
  {
    float decorAtCell = 0.0f;
    if (!Grid.Solid[cell])
    {
      decorAtCell = Grid.Decor[cell];
      if (includeLightDecor)
        decorAtCell += (float) DecorProvider.GetLightDecorBonus(cell);
    }
    return decorAtCell;
  }

  public static string GetUnitTypeMassOrUnit(GameObject go)
  {
    string unitTypeMassOrUnit = (string) UI.UNITSUFFIXES.UNITS;
    KPrefabID component = go.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      unitTypeMassOrUnit = (string) (component.Tags.Contains(GameTags.Seed) ? UI.UNITSUFFIXES.UNITS : UI.UNITSUFFIXES.MASS.KILOGRAM);
    return unitTypeMassOrUnit;
  }

  public static string GetKeywordStyle(Tag tag)
  {
    Element element = ElementLoader.GetElement(tag);
    return element == null ? (!GameUtil.foodTags.Contains(tag) ? (!GameUtil.solidTags.Contains(tag) ? (string) null : "solid") : "food") : GameUtil.GetKeywordStyle(element);
  }

  public static string GetKeywordStyle(SimHashes hash)
  {
    Element elementByHash = ElementLoader.FindElementByHash(hash);
    return elementByHash != null ? GameUtil.GetKeywordStyle(elementByHash) : (string) null;
  }

  public static string GetKeywordStyle(Element element)
  {
    if (element.id == SimHashes.Oxygen)
      return "oxygen";
    if (element.IsSolid)
      return "solid";
    if (element.IsLiquid)
      return "liquid";
    if (element.IsGas)
      return "gas";
    return element.IsVacuum ? "vacuum" : (string) null;
  }

  public static string GetKeywordStyle(GameObject go)
  {
    string keywordStyle = "";
    Edible component1 = go.GetComponent<Edible>();
    Equippable component2 = go.GetComponent<Equippable>();
    MedicinalPill component3 = go.GetComponent<MedicinalPill>();
    ResearchPointObject component4 = go.GetComponent<ResearchPointObject>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      keywordStyle = "food";
    else if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      keywordStyle = "equipment";
    else if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      keywordStyle = "medicine";
    else if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
      keywordStyle = "research";
    return keywordStyle;
  }

  public static Sprite GetBiomeSprite(string id)
  {
    string name = $"biomeIcon{char.ToUpper(id[0]).ToString()}{id.Substring(1).ToLower()}";
    Sprite sprite = Assets.GetSprite((HashedString) name);
    if ((UnityEngine.Object) sprite != (UnityEngine.Object) null)
      return new Tuple<Sprite, Color>(sprite, Color.white).first;
    Debug.LogWarning((object) ("Missing codex biome icon: " + name));
    return (Sprite) null;
  }

  public static string GenerateRandomDuplicantName()
  {
    string str1 = "";
    string str2 = "";
    bool flag = (double) UnityEngine.Random.Range(0.0f, 1f) >= 0.5;
    List<string> tList1 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.NAME.NB)));
    tList1.AddRange(flag ? (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.NAME.MALE)) : (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.NAME.FEMALE)));
    string random = tList1.GetRandom<string>();
    if ((double) UnityEngine.Random.Range(0.0f, 1f) > 0.699999988079071)
    {
      List<string> tList2 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.PREFIX.NB)));
      tList2.AddRange(flag ? (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.PREFIX.MALE)) : (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.PREFIX.FEMALE)));
      str1 = tList2.GetRandom<string>();
    }
    if (!string.IsNullOrEmpty(str1))
      str1 += " ";
    if ((double) UnityEngine.Random.Range(0.0f, 1f) >= 0.89999997615814209)
    {
      List<string> tList3 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.SUFFIX.NB)));
      tList3.AddRange(flag ? (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.SUFFIX.MALE)) : (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.SUFFIX.FEMALE)));
      str2 = tList3.GetRandom<string>();
    }
    if (!string.IsNullOrEmpty(str2))
      str2 = " " + str2;
    return str1 + random + str2;
  }

  public static string GenerateRandomLaunchPadName()
  {
    return NAMEGEN.LAUNCHPAD.FORMAT.Replace("{Name}", UnityEngine.Random.Range(1, 1000).ToString());
  }

  public static string GenerateRandomRocketName()
  {
    string newValue1 = "";
    string newValue2 = "";
    string newValue3 = "";
    int num1 = 1;
    int num2 = 2;
    int num3 = 4;
    string random = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.ROCKET.NOUN))).GetRandom<string>();
    int num4 = 0;
    if ((double) UnityEngine.Random.value > 0.699999988079071)
    {
      newValue1 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.ROCKET.PREFIX))).GetRandom<string>();
      num4 |= num1;
    }
    if ((double) UnityEngine.Random.value > 0.5)
    {
      newValue2 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.ROCKET.ADJECTIVE))).GetRandom<string>();
      num4 |= num2;
    }
    if ((double) UnityEngine.Random.value > 0.10000000149011612)
    {
      newValue3 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.ROCKET.SUFFIX))).GetRandom<string>();
      num4 |= num3;
    }
    string str = num4 != (num1 | num2 | num3) ? (num4 != (num2 | num3) ? (num4 != (num1 | num3) ? (num4 != num3 ? (num4 != (num1 | num2) ? (num4 != num1 ? (num4 != num2 ? (string) NAMEGEN.ROCKET.FMT_NOUN : (string) NAMEGEN.ROCKET.FMT_ADJECTIVE_NOUN) : (string) NAMEGEN.ROCKET.FMT_PREFIX_NOUN) : (string) NAMEGEN.ROCKET.FMT_PREFIX_ADJECTIVE_NOUN) : (string) NAMEGEN.ROCKET.FMT_NOUN_SUFFIX) : (string) NAMEGEN.ROCKET.FMT_PREFIX_NOUN_SUFFIX) : (string) NAMEGEN.ROCKET.FMT_ADJECTIVE_NOUN_SUFFIX) : (string) NAMEGEN.ROCKET.FMT_PREFIX_ADJECTIVE_NOUN_SUFFIX;
    DebugUtil.LogArgs((object) "Rocket name bits:", (object) Convert.ToString(num4, 2));
    return str.Replace("{Prefix}", newValue1).Replace("{Adjective}", newValue2).Replace("{Noun}", random).Replace("{Suffix}", newValue3);
  }

  public static string GenerateRandomWorldName(string[] nameTables)
  {
    if (nameTables == null)
    {
      Debug.LogWarning((object) "No name tables provided to generate world name. Using GENERIC");
      nameTables = new string[1]{ "GENERIC" };
    }
    string source = "";
    foreach (string nameTable in nameTables)
      source += (string) Strings.Get("STRINGS.NAMEGEN.WORLD.ROOTS." + nameTable.ToUpper());
    string str1 = GameUtil.RandomValueFromSeparatedString(source);
    if (string.IsNullOrEmpty(str1))
      str1 = GameUtil.RandomValueFromSeparatedString((string) Strings.Get((string) NAMEGEN.WORLD.ROOTS.GENERIC));
    string str2 = GameUtil.RandomValueFromSeparatedString((string) NAMEGEN.WORLD.SUFFIXES.GENERICLIST);
    return str1 + str2;
  }

  public static float GetThermalComfort(Tag duplicantType, int cell, float tolerance)
  {
    DUPLICANTSTATS statsFor = DUPLICANTSTATS.GetStatsFor(duplicantType);
    float num1 = tolerance;
    float num2 = 0.0f;
    Element elementByHash = ElementLoader.FindElementByHash(SimHashes.Creature);
    if ((double) Grid.Element[cell].thermalConductivity != 0.0)
      num2 = SimUtil.CalculateEnergyFlowCreatures(cell, statsFor.Temperature.Internal.IDEAL, elementByHash.specificHeatCapacity, elementByHash.thermalConductivity, statsFor.Temperature.SURFACE_AREA, statsFor.Temperature.SKIN_THICKNESS + 1f / 400f);
    return (num2 - num1) * 1000f;
  }

  public static void FocusCamera(Transform target, bool select = true, bool show_back_button = true)
  {
    GameUtil.FocusCamera(target.GetPosition(), show_back_button: show_back_button);
    if (!select)
      return;
    KSelectable component = target.GetComponent<KSelectable>();
    SelectTool.Instance.Select(component);
  }

  public static void FocusCameraOnWorld(
    int worldID,
    Vector3 pos,
    float forceOrthgraphicSize = 10f,
    System.Action callback = null,
    bool show_back_button = true)
  {
    CameraController.Instance.ActiveWorldStarWipe(worldID, pos, forceOrthgraphicSize, callback);
    if (!show_back_button || !((UnityEngine.Object) NotificationScreen_TemporaryActions.Instance != (UnityEngine.Object) null))
      return;
    NotificationScreen_TemporaryActions.Instance.CreateCameraReturnActionButton(CameraController.Instance.transform.position);
  }

  public static void FocusCamera(int cell, bool show_back_button = true)
  {
    GameUtil.FocusCamera(Grid.CellToPos(cell), show_back_button: show_back_button);
  }

  public static void FocusCamera(
    Vector3 position,
    float speed = 2f,
    bool playSound = true,
    bool show_back_button = true)
  {
    CameraController.Instance.CameraGoTo(position, speed, playSound);
    if (!show_back_button || !((UnityEngine.Object) NotificationScreen_TemporaryActions.Instance != (UnityEngine.Object) null))
      return;
    NotificationScreen_TemporaryActions.Instance.CreateCameraReturnActionButton(CameraController.Instance.transform.position);
  }

  public static string RandomValueFromSeparatedString(string source, string separator = "\n")
  {
    int startIndex1 = 0;
    int maxExclusive = 0;
    while (true)
    {
      int num = source.IndexOf(separator, startIndex1);
      if (num != -1)
      {
        startIndex1 = num + separator.Length;
        ++maxExclusive;
      }
      else
        break;
    }
    if (maxExclusive == 0)
      return "";
    int num1 = UnityEngine.Random.Range(0, maxExclusive);
    int startIndex2 = 0;
    for (int index = 0; index < num1; ++index)
      startIndex2 = source.IndexOf(separator, startIndex2) + separator.Length;
    int num2 = source.IndexOf(separator, startIndex2);
    return source.Substring(startIndex2, num2 == -1 ? source.Length - startIndex2 : num2 - startIndex2);
  }

  public static string GetFormattedDiseaseName(byte idx, bool color = false)
  {
    Klei.AI.Disease disease = Db.Get().Diseases[(int) idx];
    return color ? string.Format((string) UI.OVERLAYS.DISEASE.DISEASE_NAME_FORMAT, (object) disease.Name, (object) GameUtil.ColourToHex(GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName))) : string.Format((string) UI.OVERLAYS.DISEASE.DISEASE_NAME_FORMAT_NO_COLOR, (object) disease.Name);
  }

  public static string GetFormattedDisease(byte idx, int units, bool color = false)
  {
    if (idx == byte.MaxValue || units <= 0)
      return (string) UI.OVERLAYS.DISEASE.NO_DISEASE;
    Klei.AI.Disease disease = Db.Get().Diseases[(int) idx];
    return color ? string.Format((string) UI.OVERLAYS.DISEASE.DISEASE_FORMAT, (object) disease.Name, (object) GameUtil.GetFormattedDiseaseAmount(units), (object) GameUtil.ColourToHex(GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName))) : string.Format((string) UI.OVERLAYS.DISEASE.DISEASE_FORMAT_NO_COLOR, (object) disease.Name, (object) GameUtil.GetFormattedDiseaseAmount(units));
  }

  public static string GetFormattedDiseaseAmount(int units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    double num = (double) GameUtil.ApplyTimeSlice(units, timeSlice);
    return GameUtil.AddTimeSliceText(units.ToString("#,##0") + (string) UI.UNITSUFFIXES.DISEASE.UNITS, timeSlice);
  }

  public static string GetFormattedDiseaseAmount(long units, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    double num = (double) GameUtil.ApplyTimeSlice((float) units, timeSlice);
    return GameUtil.AddTimeSliceText(units.ToString("#,##0") + (string) UI.UNITSUFFIXES.DISEASE.UNITS, timeSlice);
  }

  public static string ColourizeString(Color32 colour, string str)
  {
    return $"<color=#{GameUtil.ColourToHex(colour)}>{str}</color>";
  }

  public static string ColourToHex(Color32 colour)
  {
    return $"{colour.r:X2}{colour.g:X2}{colour.b:X2}{colour.a:X2}";
  }

  public static string GetFormattedDecor(float value, bool enforce_max = false)
  {
    string str = "";
    LocString format = (double) value > (double) DecorMonitor.MAXIMUM_DECOR_VALUE & enforce_max ? UI.OVERLAYS.DECOR.MAXIMUM_DECOR : UI.OVERLAYS.DECOR.VALUE;
    if (enforce_max)
      value = Math.Min(value, DecorMonitor.MAXIMUM_DECOR_VALUE);
    if ((double) value > 0.0)
      str = "+";
    else if ((double) value >= 0.0)
      format = UI.OVERLAYS.DECOR.VALUE_ZERO;
    return string.Format((string) format, (object) str, (object) value);
  }

  public static Color GetDecorColourFromValue(int decor)
  {
    Color black = Color.black;
    float f = (float) decor / 100f;
    return (double) f <= 0.0 ? Color.Lerp(new Color(0.15f, 0.0f, 0.0f), new Color(1f, 0.0f, 0.0f), Mathf.Abs(f)) : Color.Lerp(new Color(0.15f, 0.0f, 0.0f), new Color(0.0f, 1f, 0.0f), Mathf.Abs(f));
  }

  public static List<Descriptor> GetMaterialDescriptors(Element element)
  {
    List<Descriptor> materialDescriptors = new List<Descriptor>();
    if (element.attributeModifiers.Count > 0)
    {
      foreach (AttributeModifier attributeModifier in element.attributeModifiers)
      {
        string txt = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier.AttributeId.ToUpper())), (object) attributeModifier.GetFormattedString());
        string tooltip = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier.AttributeId.ToUpper())), (object) attributeModifier.GetFormattedString());
        Descriptor descriptor = new Descriptor();
        descriptor.SetupDescriptor(txt, tooltip);
        descriptor.IncreaseIndent();
        materialDescriptors.Add(descriptor);
      }
    }
    materialDescriptors.AddRange((IEnumerable<Descriptor>) GameUtil.GetSignificantMaterialPropertyDescriptors(element));
    return materialDescriptors;
  }

  public static string GetMaterialTooltips(Element element)
  {
    string str = element.tag.ProperName();
    foreach (AttributeModifier attributeModifier in element.attributeModifiers)
    {
      string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
      string formattedString = attributeModifier.GetFormattedString();
      str = $"{str}\n    • {string.Format((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) name, (object) formattedString)}";
    }
    return str + GameUtil.GetSignificantMaterialPropertyTooltips(element);
  }

  public static string GetSignificantMaterialPropertyTooltips(Element element)
  {
    string propertyTooltips = "";
    List<Descriptor> propertyDescriptors = GameUtil.GetSignificantMaterialPropertyDescriptors(element);
    if (propertyDescriptors.Count > 0)
    {
      propertyTooltips += "\n";
      for (int index = 0; index < propertyDescriptors.Count; ++index)
        propertyTooltips = $"{propertyTooltips}    • {Util.StripTextFormatting(propertyDescriptors[index].text)}\n";
    }
    return propertyTooltips;
  }

  public static List<Descriptor> GetSignificantMaterialPropertyDescriptors(Element element)
  {
    List<Descriptor> propertyDescriptors = new List<Descriptor>();
    if ((double) element.thermalConductivity > 10.0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.HIGH_THERMAL_CONDUCTIVITY, (object) GameUtil.GetThermalConductivityString(element, false, false)), string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.HIGH_THERMAL_CONDUCTIVITY, (object) element.name, (object) element.thermalConductivity.ToString("0.#####")));
      descriptor.IncreaseIndent();
      propertyDescriptors.Add(descriptor);
    }
    if ((double) element.thermalConductivity < 1.0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.LOW_THERMAL_CONDUCTIVITY, (object) GameUtil.GetThermalConductivityString(element, false, false)), string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.LOW_THERMAL_CONDUCTIVITY, (object) element.name, (object) element.thermalConductivity.ToString("0.#####")));
      descriptor.IncreaseIndent();
      propertyDescriptors.Add(descriptor);
    }
    if ((double) element.specificHeatCapacity <= 0.20000000298023224)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) ELEMENTS.MATERIAL_MODIFIERS.LOW_SPECIFIC_HEAT_CAPACITY, string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.LOW_SPECIFIC_HEAT_CAPACITY, (object) element.name, (object) (float) ((double) element.specificHeatCapacity * 1.0)));
      descriptor.IncreaseIndent();
      propertyDescriptors.Add(descriptor);
    }
    if ((double) element.specificHeatCapacity >= 1.0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) ELEMENTS.MATERIAL_MODIFIERS.HIGH_SPECIFIC_HEAT_CAPACITY, string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.HIGH_SPECIFIC_HEAT_CAPACITY, (object) element.name, (object) (float) ((double) element.specificHeatCapacity * 1.0)));
      descriptor.IncreaseIndent();
      propertyDescriptors.Add(descriptor);
    }
    if (Sim.IsRadiationEnabled() && (double) element.radiationAbsorptionFactor >= 0.800000011920929)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) ELEMENTS.MATERIAL_MODIFIERS.EXCELLENT_RADIATION_SHIELD, string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EXCELLENT_RADIATION_SHIELD, (object) element.name, (object) element.radiationAbsorptionFactor));
      descriptor.IncreaseIndent();
      propertyDescriptors.Add(descriptor);
    }
    return propertyDescriptors;
  }

  public static int NaturalBuildingCell(this KMonoBehaviour cmp)
  {
    return Grid.PosToCell(cmp.transform.GetPosition());
  }

  public static List<Descriptor> GetMaterialDescriptors(Tag tag)
  {
    List<Descriptor> materialDescriptors = new List<Descriptor>();
    Element element = ElementLoader.GetElement(tag);
    if (element != null)
    {
      if (element.attributeModifiers.Count > 0)
      {
        foreach (AttributeModifier attributeModifier in element.attributeModifiers)
        {
          string txt = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier.AttributeId.ToUpper())), (object) attributeModifier.GetFormattedString());
          string tooltip = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier.AttributeId.ToUpper())), (object) attributeModifier.GetFormattedString());
          Descriptor descriptor = new Descriptor();
          descriptor.SetupDescriptor(txt, tooltip);
          descriptor.IncreaseIndent();
          materialDescriptors.Add(descriptor);
        }
      }
      materialDescriptors.AddRange((IEnumerable<Descriptor>) GameUtil.GetSignificantMaterialPropertyDescriptors(element));
    }
    else
    {
      GameObject prefab = Assets.TryGetPrefab(tag);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        PrefabAttributeModifiers component = prefab.GetComponent<PrefabAttributeModifiers>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          foreach (AttributeModifier descriptor1 in component.descriptors)
          {
            string txt = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + descriptor1.AttributeId.ToUpper())), (object) descriptor1.GetFormattedString());
            string tooltip = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + descriptor1.AttributeId.ToUpper())), (object) descriptor1.GetFormattedString());
            Descriptor descriptor2 = new Descriptor();
            descriptor2.SetupDescriptor(txt, tooltip);
            descriptor2.IncreaseIndent();
            materialDescriptors.Add(descriptor2);
          }
        }
      }
    }
    return materialDescriptors;
  }

  public static string GetMaterialTooltips(Tag tag)
  {
    string materialTooltips = tag.ProperName();
    Element element = ElementLoader.GetElement(tag);
    if (element != null)
    {
      foreach (AttributeModifier attributeModifier in element.attributeModifiers)
      {
        string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
        string formattedString = attributeModifier.GetFormattedString();
        materialTooltips = $"{materialTooltips}\n    • {string.Format((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) name, (object) formattedString)}";
      }
      materialTooltips += GameUtil.GetSignificantMaterialPropertyTooltips(element);
    }
    else
    {
      GameObject prefab = Assets.TryGetPrefab(tag);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        PrefabAttributeModifiers component = prefab.GetComponent<PrefabAttributeModifiers>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          foreach (AttributeModifier descriptor in component.descriptors)
          {
            string name = Db.Get().BuildingAttributes.Get(descriptor.AttributeId).Name;
            string formattedString = descriptor.GetFormattedString();
            materialTooltips = $"{materialTooltips}\n    • {string.Format((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) name, (object) formattedString)}";
          }
        }
      }
    }
    return materialTooltips;
  }

  public static bool AreChoresUIMergeable(
    Chore.Precondition.Context choreA,
    Chore.Precondition.Context choreB)
  {
    if (choreA.chore.target.isNull || choreB.chore.target.isNull)
      return false;
    ChoreType choreType1 = choreB.chore.choreType;
    ChoreType choreType2 = choreA.chore.choreType;
    return choreA.chore.choreType == choreB.chore.choreType && choreA.chore.target.GetComponent<KPrefabID>().PrefabTag == choreB.chore.target.GetComponent<KPrefabID>().PrefabTag || choreA.chore.choreType == Db.Get().ChoreTypes.Dig && choreB.chore.choreType == Db.Get().ChoreTypes.Dig || choreA.chore.choreType == Db.Get().ChoreTypes.Relax && choreB.chore.choreType == Db.Get().ChoreTypes.Relax || (choreType2 == Db.Get().ChoreTypes.ReturnSuitIdle || choreType2 == Db.Get().ChoreTypes.ReturnSuitUrgent) && (choreType1 == Db.Get().ChoreTypes.ReturnSuitIdle || choreType1 == Db.Get().ChoreTypes.ReturnSuitUrgent) || (UnityEngine.Object) choreA.chore.target.gameObject == (UnityEngine.Object) choreB.chore.target.gameObject && choreA.chore.choreType == choreB.chore.choreType;
  }

  public static string GetChoreName(Chore chore, object choreData)
  {
    string choreName = "";
    if (chore.choreType == Db.Get().ChoreTypes.Fetch || chore.choreType == Db.Get().ChoreTypes.MachineFetch || chore.choreType == Db.Get().ChoreTypes.FabricateFetch || chore.choreType == Db.Get().ChoreTypes.FetchCritical || chore.choreType == Db.Get().ChoreTypes.PowerFetch)
      choreName = chore.GetReportName(chore.gameObject.GetProperName());
    else if (chore.choreType == Db.Get().ChoreTypes.StorageFetch || chore.choreType == Db.Get().ChoreTypes.FoodFetch)
    {
      FetchChore fetchChore = chore as FetchChore;
      if (chore is FetchAreaChore fetchAreaChore)
      {
        GameObject getFetchTarget = fetchAreaChore.GetFetchTarget;
        KMonoBehaviour cmp = choreData as KMonoBehaviour;
        choreName = !((UnityEngine.Object) getFetchTarget != (UnityEngine.Object) null) ? (!((UnityEngine.Object) cmp != (UnityEngine.Object) null) ? chore.GetReportName() : chore.GetReportName(cmp.GetProperName())) : chore.GetReportName(getFetchTarget.GetProperName());
      }
      else if (fetchChore != null)
      {
        Pickupable fetchTarget = fetchChore.fetchTarget;
        KMonoBehaviour cmp = choreData as KMonoBehaviour;
        choreName = !((UnityEngine.Object) fetchTarget != (UnityEngine.Object) null) ? (!((UnityEngine.Object) cmp != (UnityEngine.Object) null) ? chore.GetReportName() : chore.GetReportName(cmp.GetProperName())) : chore.GetReportName(fetchTarget.GetProperName());
      }
    }
    else
      choreName = chore.GetReportName();
    return choreName;
  }

  public static string ChoreGroupsForChoreType(ChoreType choreType)
  {
    if (choreType.groups == null || choreType.groups.Length == 0)
      return (string) null;
    string str = "";
    for (int index = 0; index < choreType.groups.Length; ++index)
    {
      if (index != 0)
        str += (string) UI.UISIDESCREENS.MINIONTODOSIDESCREEN.CHORE_GROUP_SEPARATOR;
      str += choreType.groups[index].Name;
    }
    return str;
  }

  public static List<BuildingDef> GetBuildingsRequiringSkillPerk(string perkID)
  {
    return Assets.BuildingDefs.Where<BuildingDef>((Func<BuildingDef, bool>) (building => building.RequiredSkillPerkID == perkID)).ToList<BuildingDef>();
  }

  public static string NamesOfBuildingsRequiringSkillPerk(string perkID)
  {
    List<string> list = GameUtil.GetBuildingsRequiringSkillPerk(perkID).Select<BuildingDef, string>((Func<BuildingDef, string>) (building => GameUtil.SafeStringFormat((string) UI.ROLES_SCREEN.PERKS.CAN_USE_BUILDING.DESCRIPTION, (object) building.Name))).ToList<string>();
    return list == null || list.Count == 0 ? (string) null : string.Join("\n", (IEnumerable<string>) list);
  }

  public static string NamesOfBoostersWithSkillPerk(string perkID)
  {
    return string.Join("\n", (IEnumerable<string>) ((IEnumerable<Tag>) BionicUpgradeComponentConfig.GetBoostersWithSkillPerk(perkID)).Select<Tag, string>((Func<Tag, string>) (tag => Strings.Get($"STRINGS.ITEMS.BIONIC_BOOSTERS.{tag.ToString().ToUpper()}.NAME").String)).ToList<string>());
  }

  public static string NamesOfSkillsWithSkillPerk(string perkID)
  {
    return string.Join("\n", Db.Get().Skills.resources.Where<Skill>((Func<Skill, bool>) (match => !match.deprecated && match.GivesPerk((HashedString) perkID))).Select<Skill, string>((Func<Skill, string>) (match => match.Name)).ToList<string>().ToArray());
  }

  public static bool IsCapturingTimeLapse()
  {
    return (UnityEngine.Object) Game.Instance != (UnityEngine.Object) null && (UnityEngine.Object) Game.Instance.timelapser != (UnityEngine.Object) null && Game.Instance.timelapser.CapturingTimelapseScreenshot;
  }

  public static ExposureType GetExposureTypeForDisease(Klei.AI.Disease disease)
  {
    for (int index = 0; index < GERM_EXPOSURE.TYPES.Length; ++index)
    {
      if (disease.id == (HashedString) GERM_EXPOSURE.TYPES[index].germ_id)
        return GERM_EXPOSURE.TYPES[index];
    }
    return (ExposureType) null;
  }

  public static Sickness GetSicknessForDisease(Klei.AI.Disease disease)
  {
    for (int index = 0; index < GERM_EXPOSURE.TYPES.Length; ++index)
    {
      if (disease.id == (HashedString) GERM_EXPOSURE.TYPES[index].germ_id)
        return GERM_EXPOSURE.TYPES[index].sickness_id == null ? (Sickness) null : Db.Get().Sicknesses.Get(GERM_EXPOSURE.TYPES[index].sickness_id);
    }
    return (Sickness) null;
  }

  public static void SubscribeToTags<T>(
    T target,
    EventSystem.IntraObjectHandler<T> handler,
    bool triggerImmediately)
    where T : KMonoBehaviour
  {
    if (triggerImmediately)
    {
      Boxed<TagChangedEventData> boxed = Boxed<TagChangedEventData>.Get(new TagChangedEventData(Tag.Invalid, false));
      handler.Trigger(target.gameObject, (object) boxed);
      Boxed<TagChangedEventData>.Release(boxed);
    }
    target.Subscribe<T>(-1582839653, handler);
  }

  public static void UnsubscribeToTags<T>(T target, EventSystem.IntraObjectHandler<T> handler) where T : KMonoBehaviour
  {
    target.Unsubscribe<T>(-1582839653, handler, false);
  }

  public static EventSystem.IntraObjectHandler<T> CreateHasTagHandler<T>(
    Tag tag,
    Action<T, object> callback)
    where T : KMonoBehaviour
  {
    return new EventSystem.IntraObjectHandler<T>((Action<T, object>) ((component, data) =>
    {
      TagChangedEventData changedEventData = ((Boxed<TagChangedEventData>) data).value;
      if (changedEventData.tag == Tag.Invalid)
        changedEventData = new TagChangedEventData(tag, component.GetComponent<KPrefabID>().HasTag(tag));
      if (!(changedEventData.tag == tag) || !changedEventData.added)
        return;
      callback(component, data);
    }));
  }

  public static void DestroyCell(int cell, CellElementEvent eventSource, bool deleteMinions = true)
  {
    List<GameObject> gameObjectList;
    using (CollectionPool<List<GameObject>, GameObject>.Get(out gameObjectList))
    {
      gameObjectList.Add(Grid.Objects[cell, 2]);
      gameObjectList.Add(Grid.Objects[cell, 1]);
      gameObjectList.Add(Grid.Objects[cell, 9]);
      gameObjectList.Add(Grid.Objects[cell, 5]);
      gameObjectList.Add(Grid.Objects[cell, 12]);
      gameObjectList.Add(Grid.Objects[cell, 15]);
      gameObjectList.Add(Grid.Objects[cell, 16 /*0x10*/]);
      gameObjectList.Add(Grid.Objects[cell, 19]);
      gameObjectList.Add(Grid.Objects[cell, 20]);
      gameObjectList.Add(Grid.Objects[cell, 23]);
      gameObjectList.Add(Grid.Objects[cell, 26]);
      gameObjectList.Add(Grid.Objects[cell, 29]);
      gameObjectList.Add(Grid.Objects[cell, 27]);
      gameObjectList.Add(Grid.Objects[cell, 31 /*0x1F*/]);
      gameObjectList.Add(Grid.Objects[cell, 30]);
      foreach (Comet cmp in Components.Meteors.GetItems((int) Grid.WorldIdx[cell]))
      {
        if (!cmp.IsNullOrDestroyed() && Grid.PosToCell((KMonoBehaviour) cmp) == cell)
          gameObjectList.Add(cmp.gameObject);
      }
      foreach (GameObject original in gameObjectList)
      {
        if ((UnityEngine.Object) original != (UnityEngine.Object) null)
          Util.KDestroyGameObject(original);
      }
      GameUtil.ClearCell(cell, deleteMinions);
      FallingWater.instance.ClearParticles(cell);
      if (ElementLoader.elements[(int) Grid.ElementIdx[cell]].id == SimHashes.Void)
        SimMessages.ReplaceElement(cell, SimHashes.Void, eventSource, 0.0f, 0.0f);
      else
        SimMessages.ReplaceElement(cell, SimHashes.Vacuum, eventSource, 0.0f, 0.0f);
      if (!BackwallManager.HasBackwall(cell))
        return;
      SimMessages.Dig(cell, skipEvent: true, backwall: true);
    }
  }

  public static void ClearCell(int cell, bool deleteMinions = true)
  {
    Vector2I xy = Grid.CellToXY(cell);
    List<ScenePartitionerEntry> gathered_entries;
    using (CollectionPool<List<ScenePartitionerEntry>, ScenePartitionerEntry>.Get(out gathered_entries))
    {
      GameScenePartitioner.Instance.GatherEntries(xy.x, xy.y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, gathered_entries);
      for (int index = 0; index < gathered_entries.Count; ++index)
      {
        Pickupable pickupable = gathered_entries[index].obj as Pickupable;
        if (!((UnityEngine.Object) pickupable == (UnityEngine.Object) null))
        {
          bool flag = pickupable.KPrefabID.HasTag(GameTags.BaseMinion);
          if (deleteMinions || !flag)
            Util.KDestroyGameObject(pickupable.gameObject);
        }
      }
    }
  }

  public enum UnitClass
  {
    SimpleFloat,
    SimpleInteger,
    Temperature,
    Mass,
    Calories,
    Percent,
    Distance,
    Disease,
    Radiation,
    Energy,
    Power,
    Lux,
    Time,
    Seconds,
    Cycles,
  }

  public enum TemperatureUnit
  {
    Celsius,
    Fahrenheit,
    Kelvin,
  }

  public enum MassUnit
  {
    Kilograms,
    Pounds,
  }

  public enum MetricMassFormat
  {
    UseThreshold,
    Kilogram,
    Gram,
    Tonne,
  }

  public enum TemperatureInterpretation
  {
    Absolute,
    Relative,
  }

  public enum TimeSlice
  {
    None,
    ModifyOnly,
    PerSecond,
    PerCycle,
  }

  public enum MeasureUnit
  {
    mass,
    kcal,
    quantity,
  }

  public enum IdentityDescriptorTense
  {
    Normal,
    Possessive,
    Plural,
  }

  public enum WattageFormatterUnit
  {
    Watts,
    Kilowatts,
    Automatic,
  }

  public enum HeatEnergyFormatterUnit
  {
    DTU_S,
    KDTU_S,
    Automatic,
  }

  public static class Hardness
  {
    public const int VERY_SOFT = 0;
    public const int SOFT = 10;
    public const int FIRM = 25;
    public const int VERY_FIRM = 50;
    public const int NEARLY_IMPENETRABLE = 150;
    public const int SUPER_DUPER_HARD = 200;
    public const int RADIOACTIVE_MATERIALS = 251;
    public const int IMPENETRABLE = 255 /*0xFF*/;
    public static Color ImpenetrableColor = new Color(0.831372559f, 0.286274523f, 0.282352954f);
    public static Color nearlyImpenetrableColor = new Color(0.7411765f, 0.349019617f, 0.498039216f);
    public static Color veryFirmColor = new Color(0.6392157f, 0.392156869f, 0.6039216f);
    public static Color firmColor = new Color(0.5254902f, 0.419607848f, 0.647058845f);
    public static Color softColor = new Color(0.427450985f, 0.482352942f, 0.75686276f);
    public static Color verySoftColor = new Color(0.443137258f, 0.670588255f, 0.8117647f);
  }

  public static class GermResistanceValues
  {
    public const float MEDIUM = 2f;
    public const float LARGE = 5f;
    public static Color NegativeLargeColor = new Color(0.831372559f, 0.286274523f, 0.282352954f);
    public static Color NegativeMediumColor = new Color(0.7411765f, 0.349019617f, 0.498039216f);
    public static Color NegativeSmallColor = new Color(0.6392157f, 0.392156869f, 0.6039216f);
    public static Color PositiveSmallColor = new Color(0.5254902f, 0.419607848f, 0.647058845f);
    public static Color PositiveMediumColor = new Color(0.427450985f, 0.482352942f, 0.75686276f);
    public static Color PositiveLargeColor = new Color(0.443137258f, 0.670588255f, 0.8117647f);
  }

  public static class ThermalConductivityValues
  {
    public const float VERY_HIGH = 50f;
    public const float HIGH = 10f;
    public const float MEDIUM = 2f;
    public const float LOW = 1f;
    public static Color veryLowConductivityColor = new Color(0.831372559f, 0.286274523f, 0.282352954f);
    public static Color lowConductivityColor = new Color(0.7411765f, 0.349019617f, 0.498039216f);
    public static Color mediumConductivityColor = new Color(0.6392157f, 0.392156869f, 0.6039216f);
    public static Color highConductivityColor = new Color(0.5254902f, 0.419607848f, 0.647058845f);
    public static Color veryHighConductivityColor = new Color(0.427450985f, 0.482352942f, 0.75686276f);
  }

  public static class BreathableValues
  {
    public static Color positiveColor = new Color(0.443137258f, 0.670588255f, 0.8117647f);
    public static Color warningColor = new Color(0.6392157f, 0.392156869f, 0.6039216f);
    public static Color negativeColor = new Color(0.831372559f, 0.286274523f, 0.282352954f);
  }

  public static class WireLoadValues
  {
    public static Color warningColor = new Color(0.9843137f, 0.6901961f, 0.23137255f);
    public static Color negativeColor = new Color(1f, 0.192156866f, 0.192156866f);
  }
}
