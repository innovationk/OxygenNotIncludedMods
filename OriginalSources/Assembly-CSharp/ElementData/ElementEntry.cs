// Decompiled with JetBrains decompiler
// Type: ElementData.ElementEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using VYaml.Annotations;
using VYaml.Emitter;
using VYaml.Parser;
using VYaml.Serialization;

#nullable enable
namespace ElementData;

[YamlObject(NamingConvention.LowerCamelCase)]
public class ElementEntry
{
  private 
  #nullable disable
  string description_backing;

  public string elementId { get; set; }

  public float specificHeatCapacity { get; set; }

  public float thermalConductivity { get; set; }

  public float solidSurfaceAreaMultiplier { get; set; }

  public float liquidSurfaceAreaMultiplier { get; set; }

  public float gasSurfaceAreaMultiplier { get; set; }

  public float defaultMass { get; set; }

  public float defaultTemperature { get; set; }

  public float defaultPressure { get; set; }

  public float molarMass { get; set; }

  public float lightAbsorptionFactor { get; set; }

  public float radiationAbsorptionFactor { get; set; }

  public float radiationPer1000Mass { get; set; }

  public string lowTempTransitionTarget { get; set; }

  public float? lowTemp { get; set; }

  public string highTempTransitionTarget { get; set; }

  public float? highTemp { get; set; }

  public string lowTempTransitionOreId { get; set; }

  public float lowTempTransitionOreMassConversion { get; set; }

  public string highTempTransitionOreId { get; set; }

  public float highTempTransitionOreMassConversion { get; set; }

  public string sublimateId { get; set; }

  public string sublimateFx { get; set; }

  public float sublimateRate { get; set; }

  public float sublimateEfficiency { get; set; }

  public float sublimateProbability { get; set; }

  public float offGasPercentage { get; set; }

  public string materialCategory { get; set; }

  public string[] tags { get; set; }

  public bool isDisabled { get; set; }

  public float strength { get; set; }

  public float maxMass { get; set; }

  public byte hardness { get; set; }

  public float toxicity { get; set; }

  public float liquidCompression { get; set; }

  public float speed { get; set; }

  public float minHorizontalFlow { get; set; }

  public float minVerticalFlow { get; set; }

  public string convertId { get; set; }

  public float flow { get; set; }

  public int buildMenuSort { get; set; }

  public Element.State state { get; set; }

  public string localizationID { get; set; }

  public string dlcId { get; set; }

  public string refinedMetalTarget { get; set; }

  public ElementComposition[] composition { get; set; }

  public string description
  {
    get
    {
      return this.description_backing ?? $"STRINGS.ELEMENTS.{this.elementId.ToString().ToUpper()}.DESC";
    }
    set => this.description_backing = value;
  }

  [Preserve]
  public static void __RegisterVYamlFormatter()
  {
    GeneratedResolver.Register<ElementEntry>((IYamlFormatter<ElementEntry>) new ElementEntry.ElementEntryGeneratedFormatter());
  }

  [Preserve]
  public class ElementEntryGeneratedFormatter : IYamlFormatter<
  #nullable enable
  ElementEntry?>, IYamlFormatter
  {
    private static readonly byte[] elementIdKeyUtf8Bytes = new byte[9]
    {
      (byte) 101,
      (byte) 108,
      (byte) 101,
      (byte) 109,
      (byte) 101,
      (byte) 110,
      (byte) 116,
      (byte) 73,
      (byte) 100
    };
    private static readonly byte[] specificHeatCapacityKeyUtf8Bytes = new byte[20]
    {
      (byte) 115,
      (byte) 112 /*0x70*/,
      (byte) 101,
      (byte) 99,
      (byte) 105,
      (byte) 102,
      (byte) 105,
      (byte) 99,
      (byte) 72,
      (byte) 101,
      (byte) 97,
      (byte) 116,
      (byte) 67,
      (byte) 97,
      (byte) 112 /*0x70*/,
      (byte) 97,
      (byte) 99,
      (byte) 105,
      (byte) 116,
      (byte) 121
    };
    private static readonly byte[] thermalConductivityKeyUtf8Bytes = new byte[19]
    {
      (byte) 116,
      (byte) 104,
      (byte) 101,
      (byte) 114,
      (byte) 109,
      (byte) 97,
      (byte) 108,
      (byte) 67,
      (byte) 111,
      (byte) 110,
      (byte) 100,
      (byte) 117,
      (byte) 99,
      (byte) 116,
      (byte) 105,
      (byte) 118,
      (byte) 105,
      (byte) 116,
      (byte) 121
    };
    private static readonly byte[] solidSurfaceAreaMultiplierKeyUtf8Bytes = new byte[26]
    {
      (byte) 115,
      (byte) 111,
      (byte) 108,
      (byte) 105,
      (byte) 100,
      (byte) 83,
      (byte) 117,
      (byte) 114,
      (byte) 102,
      (byte) 97,
      (byte) 99,
      (byte) 101,
      (byte) 65,
      (byte) 114,
      (byte) 101,
      (byte) 97,
      (byte) 77,
      (byte) 117,
      (byte) 108,
      (byte) 116,
      (byte) 105,
      (byte) 112 /*0x70*/,
      (byte) 108,
      (byte) 105,
      (byte) 101,
      (byte) 114
    };
    private static readonly byte[] liquidSurfaceAreaMultiplierKeyUtf8Bytes = new byte[27]
    {
      (byte) 108,
      (byte) 105,
      (byte) 113,
      (byte) 117,
      (byte) 105,
      (byte) 100,
      (byte) 83,
      (byte) 117,
      (byte) 114,
      (byte) 102,
      (byte) 97,
      (byte) 99,
      (byte) 101,
      (byte) 65,
      (byte) 114,
      (byte) 101,
      (byte) 97,
      (byte) 77,
      (byte) 117,
      (byte) 108,
      (byte) 116,
      (byte) 105,
      (byte) 112 /*0x70*/,
      (byte) 108,
      (byte) 105,
      (byte) 101,
      (byte) 114
    };
    private static readonly byte[] gasSurfaceAreaMultiplierKeyUtf8Bytes = new byte[24]
    {
      (byte) 103,
      (byte) 97,
      (byte) 115,
      (byte) 83,
      (byte) 117,
      (byte) 114,
      (byte) 102,
      (byte) 97,
      (byte) 99,
      (byte) 101,
      (byte) 65,
      (byte) 114,
      (byte) 101,
      (byte) 97,
      (byte) 77,
      (byte) 117,
      (byte) 108,
      (byte) 116,
      (byte) 105,
      (byte) 112 /*0x70*/,
      (byte) 108,
      (byte) 105,
      (byte) 101,
      (byte) 114
    };
    private static readonly byte[] defaultMassKeyUtf8Bytes = new byte[11]
    {
      (byte) 100,
      (byte) 101,
      (byte) 102,
      (byte) 97,
      (byte) 117,
      (byte) 108,
      (byte) 116,
      (byte) 77,
      (byte) 97,
      (byte) 115,
      (byte) 115
    };
    private static readonly byte[] defaultTemperatureKeyUtf8Bytes = new byte[18]
    {
      (byte) 100,
      (byte) 101,
      (byte) 102,
      (byte) 97,
      (byte) 117,
      (byte) 108,
      (byte) 116,
      (byte) 84,
      (byte) 101,
      (byte) 109,
      (byte) 112 /*0x70*/,
      (byte) 101,
      (byte) 114,
      (byte) 97,
      (byte) 116,
      (byte) 117,
      (byte) 114,
      (byte) 101
    };
    private static readonly byte[] defaultPressureKeyUtf8Bytes = new byte[15]
    {
      (byte) 100,
      (byte) 101,
      (byte) 102,
      (byte) 97,
      (byte) 117,
      (byte) 108,
      (byte) 116,
      (byte) 80 /*0x50*/,
      (byte) 114,
      (byte) 101,
      (byte) 115,
      (byte) 115,
      (byte) 117,
      (byte) 114,
      (byte) 101
    };
    private static readonly byte[] molarMassKeyUtf8Bytes = new byte[9]
    {
      (byte) 109,
      (byte) 111,
      (byte) 108,
      (byte) 97,
      (byte) 114,
      (byte) 77,
      (byte) 97,
      (byte) 115,
      (byte) 115
    };
    private static readonly byte[] lightAbsorptionFactorKeyUtf8Bytes = new byte[21]
    {
      (byte) 108,
      (byte) 105,
      (byte) 103,
      (byte) 104,
      (byte) 116,
      (byte) 65,
      (byte) 98,
      (byte) 115,
      (byte) 111,
      (byte) 114,
      (byte) 112 /*0x70*/,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110,
      (byte) 70,
      (byte) 97,
      (byte) 99,
      (byte) 116,
      (byte) 111,
      (byte) 114
    };
    private static readonly byte[] radiationAbsorptionFactorKeyUtf8Bytes = new byte[25]
    {
      (byte) 114,
      (byte) 97,
      (byte) 100,
      (byte) 105,
      (byte) 97,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110,
      (byte) 65,
      (byte) 98,
      (byte) 115,
      (byte) 111,
      (byte) 114,
      (byte) 112 /*0x70*/,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110,
      (byte) 70,
      (byte) 97,
      (byte) 99,
      (byte) 116,
      (byte) 111,
      (byte) 114
    };
    private static readonly byte[] radiationPer1000MassKeyUtf8Bytes = new byte[20]
    {
      (byte) 114,
      (byte) 97,
      (byte) 100,
      (byte) 105,
      (byte) 97,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110,
      (byte) 80 /*0x50*/,
      (byte) 101,
      (byte) 114,
      (byte) 49,
      (byte) 48 /*0x30*/,
      (byte) 48 /*0x30*/,
      (byte) 48 /*0x30*/,
      (byte) 77,
      (byte) 97,
      (byte) 115,
      (byte) 115
    };
    private static readonly byte[] lowTempTransitionTargetKeyUtf8Bytes = new byte[23]
    {
      (byte) 108,
      (byte) 111,
      (byte) 119,
      (byte) 84,
      (byte) 101,
      (byte) 109,
      (byte) 112 /*0x70*/,
      (byte) 84,
      (byte) 114,
      (byte) 97,
      (byte) 110,
      (byte) 115,
      (byte) 105,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110,
      (byte) 84,
      (byte) 97,
      (byte) 114,
      (byte) 103,
      (byte) 101,
      (byte) 116
    };
    private static readonly byte[] lowTempKeyUtf8Bytes = new byte[7]
    {
      (byte) 108,
      (byte) 111,
      (byte) 119,
      (byte) 84,
      (byte) 101,
      (byte) 109,
      (byte) 112 /*0x70*/
    };
    private static readonly byte[] highTempTransitionTargetKeyUtf8Bytes = new byte[24]
    {
      (byte) 104,
      (byte) 105,
      (byte) 103,
      (byte) 104,
      (byte) 84,
      (byte) 101,
      (byte) 109,
      (byte) 112 /*0x70*/,
      (byte) 84,
      (byte) 114,
      (byte) 97,
      (byte) 110,
      (byte) 115,
      (byte) 105,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110,
      (byte) 84,
      (byte) 97,
      (byte) 114,
      (byte) 103,
      (byte) 101,
      (byte) 116
    };
    private static readonly byte[] highTempKeyUtf8Bytes = new byte[8]
    {
      (byte) 104,
      (byte) 105,
      (byte) 103,
      (byte) 104,
      (byte) 84,
      (byte) 101,
      (byte) 109,
      (byte) 112 /*0x70*/
    };
    private static readonly byte[] lowTempTransitionOreIdKeyUtf8Bytes = new byte[22]
    {
      (byte) 108,
      (byte) 111,
      (byte) 119,
      (byte) 84,
      (byte) 101,
      (byte) 109,
      (byte) 112 /*0x70*/,
      (byte) 84,
      (byte) 114,
      (byte) 97,
      (byte) 110,
      (byte) 115,
      (byte) 105,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110,
      (byte) 79,
      (byte) 114,
      (byte) 101,
      (byte) 73,
      (byte) 100
    };
    private static readonly byte[] lowTempTransitionOreMassConversionKeyUtf8Bytes = new byte[34]
    {
      (byte) 108,
      (byte) 111,
      (byte) 119,
      (byte) 84,
      (byte) 101,
      (byte) 109,
      (byte) 112 /*0x70*/,
      (byte) 84,
      (byte) 114,
      (byte) 97,
      (byte) 110,
      (byte) 115,
      (byte) 105,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110,
      (byte) 79,
      (byte) 114,
      (byte) 101,
      (byte) 77,
      (byte) 97,
      (byte) 115,
      (byte) 115,
      (byte) 67,
      (byte) 111,
      (byte) 110,
      (byte) 118,
      (byte) 101,
      (byte) 114,
      (byte) 115,
      (byte) 105,
      (byte) 111,
      (byte) 110
    };
    private static readonly byte[] highTempTransitionOreIdKeyUtf8Bytes = new byte[23]
    {
      (byte) 104,
      (byte) 105,
      (byte) 103,
      (byte) 104,
      (byte) 84,
      (byte) 101,
      (byte) 109,
      (byte) 112 /*0x70*/,
      (byte) 84,
      (byte) 114,
      (byte) 97,
      (byte) 110,
      (byte) 115,
      (byte) 105,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110,
      (byte) 79,
      (byte) 114,
      (byte) 101,
      (byte) 73,
      (byte) 100
    };
    private static readonly byte[] highTempTransitionOreMassConversionKeyUtf8Bytes = new byte[35]
    {
      (byte) 104,
      (byte) 105,
      (byte) 103,
      (byte) 104,
      (byte) 84,
      (byte) 101,
      (byte) 109,
      (byte) 112 /*0x70*/,
      (byte) 84,
      (byte) 114,
      (byte) 97,
      (byte) 110,
      (byte) 115,
      (byte) 105,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110,
      (byte) 79,
      (byte) 114,
      (byte) 101,
      (byte) 77,
      (byte) 97,
      (byte) 115,
      (byte) 115,
      (byte) 67,
      (byte) 111,
      (byte) 110,
      (byte) 118,
      (byte) 101,
      (byte) 114,
      (byte) 115,
      (byte) 105,
      (byte) 111,
      (byte) 110
    };
    private static readonly byte[] sublimateIdKeyUtf8Bytes = new byte[11]
    {
      (byte) 115,
      (byte) 117,
      (byte) 98,
      (byte) 108,
      (byte) 105,
      (byte) 109,
      (byte) 97,
      (byte) 116,
      (byte) 101,
      (byte) 73,
      (byte) 100
    };
    private static readonly byte[] sublimateFxKeyUtf8Bytes = new byte[11]
    {
      (byte) 115,
      (byte) 117,
      (byte) 98,
      (byte) 108,
      (byte) 105,
      (byte) 109,
      (byte) 97,
      (byte) 116,
      (byte) 101,
      (byte) 70,
      (byte) 120
    };
    private static readonly byte[] sublimateRateKeyUtf8Bytes = new byte[13]
    {
      (byte) 115,
      (byte) 117,
      (byte) 98,
      (byte) 108,
      (byte) 105,
      (byte) 109,
      (byte) 97,
      (byte) 116,
      (byte) 101,
      (byte) 82,
      (byte) 97,
      (byte) 116,
      (byte) 101
    };
    private static readonly byte[] sublimateEfficiencyKeyUtf8Bytes = new byte[19]
    {
      (byte) 115,
      (byte) 117,
      (byte) 98,
      (byte) 108,
      (byte) 105,
      (byte) 109,
      (byte) 97,
      (byte) 116,
      (byte) 101,
      (byte) 69,
      (byte) 102,
      (byte) 102,
      (byte) 105,
      (byte) 99,
      (byte) 105,
      (byte) 101,
      (byte) 110,
      (byte) 99,
      (byte) 121
    };
    private static readonly byte[] sublimateProbabilityKeyUtf8Bytes = new byte[20]
    {
      (byte) 115,
      (byte) 117,
      (byte) 98,
      (byte) 108,
      (byte) 105,
      (byte) 109,
      (byte) 97,
      (byte) 116,
      (byte) 101,
      (byte) 80 /*0x50*/,
      (byte) 114,
      (byte) 111,
      (byte) 98,
      (byte) 97,
      (byte) 98,
      (byte) 105,
      (byte) 108,
      (byte) 105,
      (byte) 116,
      (byte) 121
    };
    private static readonly byte[] offGasPercentageKeyUtf8Bytes = new byte[16 /*0x10*/]
    {
      (byte) 111,
      (byte) 102,
      (byte) 102,
      (byte) 71,
      (byte) 97,
      (byte) 115,
      (byte) 80 /*0x50*/,
      (byte) 101,
      (byte) 114,
      (byte) 99,
      (byte) 101,
      (byte) 110,
      (byte) 116,
      (byte) 97,
      (byte) 103,
      (byte) 101
    };
    private static readonly byte[] materialCategoryKeyUtf8Bytes = new byte[16 /*0x10*/]
    {
      (byte) 109,
      (byte) 97,
      (byte) 116,
      (byte) 101,
      (byte) 114,
      (byte) 105,
      (byte) 97,
      (byte) 108,
      (byte) 67,
      (byte) 97,
      (byte) 116,
      (byte) 101,
      (byte) 103,
      (byte) 111,
      (byte) 114,
      (byte) 121
    };
    private static readonly byte[] tagsKeyUtf8Bytes = new byte[4]
    {
      (byte) 116,
      (byte) 97,
      (byte) 103,
      (byte) 115
    };
    private static readonly byte[] isDisabledKeyUtf8Bytes = new byte[10]
    {
      (byte) 105,
      (byte) 115,
      (byte) 68,
      (byte) 105,
      (byte) 115,
      (byte) 97,
      (byte) 98,
      (byte) 108,
      (byte) 101,
      (byte) 100
    };
    private static readonly byte[] strengthKeyUtf8Bytes = new byte[8]
    {
      (byte) 115,
      (byte) 116,
      (byte) 114,
      (byte) 101,
      (byte) 110,
      (byte) 103,
      (byte) 116,
      (byte) 104
    };
    private static readonly byte[] maxMassKeyUtf8Bytes = new byte[7]
    {
      (byte) 109,
      (byte) 97,
      (byte) 120,
      (byte) 77,
      (byte) 97,
      (byte) 115,
      (byte) 115
    };
    private static readonly byte[] hardnessKeyUtf8Bytes = new byte[8]
    {
      (byte) 104,
      (byte) 97,
      (byte) 114,
      (byte) 100,
      (byte) 110,
      (byte) 101,
      (byte) 115,
      (byte) 115
    };
    private static readonly byte[] toxicityKeyUtf8Bytes = new byte[8]
    {
      (byte) 116,
      (byte) 111,
      (byte) 120,
      (byte) 105,
      (byte) 99,
      (byte) 105,
      (byte) 116,
      (byte) 121
    };
    private static readonly byte[] liquidCompressionKeyUtf8Bytes = new byte[17]
    {
      (byte) 108,
      (byte) 105,
      (byte) 113,
      (byte) 117,
      (byte) 105,
      (byte) 100,
      (byte) 67,
      (byte) 111,
      (byte) 109,
      (byte) 112 /*0x70*/,
      (byte) 114,
      (byte) 101,
      (byte) 115,
      (byte) 115,
      (byte) 105,
      (byte) 111,
      (byte) 110
    };
    private static readonly byte[] speedKeyUtf8Bytes = new byte[5]
    {
      (byte) 115,
      (byte) 112 /*0x70*/,
      (byte) 101,
      (byte) 101,
      (byte) 100
    };
    private static readonly byte[] minHorizontalFlowKeyUtf8Bytes = new byte[17]
    {
      (byte) 109,
      (byte) 105,
      (byte) 110,
      (byte) 72,
      (byte) 111,
      (byte) 114,
      (byte) 105,
      (byte) 122,
      (byte) 111,
      (byte) 110,
      (byte) 116,
      (byte) 97,
      (byte) 108,
      (byte) 70,
      (byte) 108,
      (byte) 111,
      (byte) 119
    };
    private static readonly byte[] minVerticalFlowKeyUtf8Bytes = new byte[15]
    {
      (byte) 109,
      (byte) 105,
      (byte) 110,
      (byte) 86,
      (byte) 101,
      (byte) 114,
      (byte) 116,
      (byte) 105,
      (byte) 99,
      (byte) 97,
      (byte) 108,
      (byte) 70,
      (byte) 108,
      (byte) 111,
      (byte) 119
    };
    private static readonly byte[] convertIdKeyUtf8Bytes = new byte[9]
    {
      (byte) 99,
      (byte) 111,
      (byte) 110,
      (byte) 118,
      (byte) 101,
      (byte) 114,
      (byte) 116,
      (byte) 73,
      (byte) 100
    };
    private static readonly byte[] flowKeyUtf8Bytes = new byte[4]
    {
      (byte) 102,
      (byte) 108,
      (byte) 111,
      (byte) 119
    };
    private static readonly byte[] buildMenuSortKeyUtf8Bytes = new byte[13]
    {
      (byte) 98,
      (byte) 117,
      (byte) 105,
      (byte) 108,
      (byte) 100,
      (byte) 77,
      (byte) 101,
      (byte) 110,
      (byte) 117,
      (byte) 83,
      (byte) 111,
      (byte) 114,
      (byte) 116
    };
    private static readonly byte[] stateKeyUtf8Bytes = new byte[5]
    {
      (byte) 115,
      (byte) 116,
      (byte) 97,
      (byte) 116,
      (byte) 101
    };
    private static readonly byte[] localizationIDKeyUtf8Bytes = new byte[14]
    {
      (byte) 108,
      (byte) 111,
      (byte) 99,
      (byte) 97,
      (byte) 108,
      (byte) 105,
      (byte) 122,
      (byte) 97,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110,
      (byte) 73,
      (byte) 68
    };
    private static readonly byte[] dlcIdKeyUtf8Bytes = new byte[5]
    {
      (byte) 100,
      (byte) 108,
      (byte) 99,
      (byte) 73,
      (byte) 100
    };
    private static readonly byte[] refinedMetalTargetKeyUtf8Bytes = new byte[18]
    {
      (byte) 114,
      (byte) 101,
      (byte) 102,
      (byte) 105,
      (byte) 110,
      (byte) 101,
      (byte) 100,
      (byte) 77,
      (byte) 101,
      (byte) 116,
      (byte) 97,
      (byte) 108,
      (byte) 84,
      (byte) 97,
      (byte) 114,
      (byte) 103,
      (byte) 101,
      (byte) 116
    };
    private static readonly byte[] compositionKeyUtf8Bytes = new byte[11]
    {
      (byte) 99,
      (byte) 111,
      (byte) 109,
      (byte) 112 /*0x70*/,
      (byte) 111,
      (byte) 115,
      (byte) 105,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110
    };
    private static readonly byte[] descriptionKeyUtf8Bytes = new byte[11]
    {
      (byte) 100,
      (byte) 101,
      (byte) 115,
      (byte) 99,
      (byte) 114,
      (byte) 105,
      (byte) 112 /*0x70*/,
      (byte) 116,
      (byte) 105,
      (byte) 111,
      (byte) 110
    };

    [Preserve]
    public void Serialize(
      ref Utf8YamlEmitter emitter,
      ElementEntry? value,
      YamlSerializationContext context)
    {
      if (value == null)
      {
        emitter.WriteNull();
      }
      else
      {
        emitter.BeginMapping();
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.elementIdKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.elementIdKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string>(ref emitter, value.elementId);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.specificHeatCapacityKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.specificHeatCapacityKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.specificHeatCapacity);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.thermalConductivityKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.thermalConductivityKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.thermalConductivity);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.solidSurfaceAreaMultiplierKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.solidSurfaceAreaMultiplierKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.solidSurfaceAreaMultiplier);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.liquidSurfaceAreaMultiplierKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.liquidSurfaceAreaMultiplierKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.liquidSurfaceAreaMultiplier);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.gasSurfaceAreaMultiplierKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.gasSurfaceAreaMultiplierKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.gasSurfaceAreaMultiplier);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.defaultMassKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.defaultMassKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.defaultMass);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.defaultTemperatureKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.defaultTemperatureKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.defaultTemperature);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.defaultPressureKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.defaultPressureKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.defaultPressure);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.molarMassKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.molarMassKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.molarMass);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lightAbsorptionFactorKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lightAbsorptionFactorKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.lightAbsorptionFactor);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.radiationAbsorptionFactorKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.radiationAbsorptionFactorKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.radiationAbsorptionFactor);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.radiationPer1000MassKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.radiationPer1000MassKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.radiationPer1000Mass);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lowTempTransitionTargetKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lowTempTransitionTargetKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string>(ref emitter, value.lowTempTransitionTarget);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lowTempKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lowTempKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float?>(ref emitter, value.lowTemp);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.highTempTransitionTargetKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.highTempTransitionTargetKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string>(ref emitter, value.highTempTransitionTarget);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.highTempKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.highTempKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float?>(ref emitter, value.highTemp);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lowTempTransitionOreIdKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lowTempTransitionOreIdKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string>(ref emitter, value.lowTempTransitionOreId);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lowTempTransitionOreMassConversionKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lowTempTransitionOreMassConversionKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.lowTempTransitionOreMassConversion);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.highTempTransitionOreIdKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.highTempTransitionOreIdKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string>(ref emitter, value.highTempTransitionOreId);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.highTempTransitionOreMassConversionKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.highTempTransitionOreMassConversionKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.highTempTransitionOreMassConversion);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateIdKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateIdKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string>(ref emitter, value.sublimateId);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateFxKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateFxKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string>(ref emitter, value.sublimateFx);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateRateKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateRateKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.sublimateRate);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateEfficiencyKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateEfficiencyKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.sublimateEfficiency);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateProbabilityKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateProbabilityKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.sublimateProbability);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.offGasPercentageKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.offGasPercentageKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.offGasPercentage);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.materialCategoryKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.materialCategoryKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string>(ref emitter, value.materialCategory);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.tagsKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.tagsKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string[]>(ref emitter, value.tags);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.isDisabledKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.isDisabledKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<bool>(ref emitter, value.isDisabled);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.strengthKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.strengthKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.strength);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.maxMassKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.maxMassKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.maxMass);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.hardnessKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.hardnessKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<byte>(ref emitter, value.hardness);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.toxicityKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.toxicityKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.toxicity);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.liquidCompressionKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.liquidCompressionKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.liquidCompression);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.speedKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.speedKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.speed);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.minHorizontalFlowKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.minHorizontalFlowKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.minHorizontalFlow);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.minVerticalFlowKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.minVerticalFlowKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.minVerticalFlow);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.convertIdKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.convertIdKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string>(ref emitter, value.convertId);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.flowKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.flowKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<float>(ref emitter, value.flow);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.buildMenuSortKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.buildMenuSortKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<int>(ref emitter, value.buildMenuSort);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.stateKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.stateKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<Element.State>(ref emitter, value.state);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.localizationIDKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.localizationIDKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string>(ref emitter, value.localizationID);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.dlcIdKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.dlcIdKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string>(ref emitter, value.dlcId);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.refinedMetalTargetKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.refinedMetalTargetKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string>(ref emitter, value.refinedMetalTarget);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.compositionKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.compositionKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<ElementComposition[]>(ref emitter, value.composition);
        if (context.Options.NamingConvention == NamingConvention.LowerCamelCase)
        {
          emitter.WriteScalar(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.descriptionKeyUtf8Bytes));
        }
        else
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.descriptionKeyUtf8Bytes), context.Options.NamingConvention, out threadStaticBuffer, out written);
          emitter.WriteScalar(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written)));
        }
        context.Serialize<string>(ref emitter, value.description);
        emitter.EndMapping();
      }
    }

    [Preserve]
    public ElementEntry? Deserialize(ref YamlParser parser, YamlDeserializationContext context)
    {
      if (parser.IsNullScalar())
      {
        parser.Read();
        return (ElementEntry) null;
      }
      parser.ReadWithVerify(ParseEventType.MappingStart);
      string str1 = (string) null;
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = 0.0f;
      float num4 = 0.0f;
      float num5 = 0.0f;
      float num6 = 0.0f;
      float num7 = 0.0f;
      float num8 = 0.0f;
      float num9 = 0.0f;
      float num10 = 0.0f;
      float num11 = 0.0f;
      float num12 = 0.0f;
      string str2 = (string) null;
      float? nullable1 = new float?();
      string str3 = (string) null;
      float? nullable2 = new float?();
      string str4 = (string) null;
      float num13 = 0.0f;
      string str5 = (string) null;
      float num14 = 0.0f;
      string str6 = (string) null;
      string str7 = (string) null;
      float num15 = 0.0f;
      float num16 = 0.0f;
      float num17 = 0.0f;
      float num18 = 0.0f;
      string str8 = (string) null;
      string[] strArray = (string[]) null;
      bool flag = false;
      float num19 = 0.0f;
      float num20 = 0.0f;
      byte num21 = 0;
      float num22 = 0.0f;
      float num23 = 0.0f;
      float num24 = 0.0f;
      float num25 = 0.0f;
      float num26 = 0.0f;
      string str9 = (string) null;
      float num27 = 0.0f;
      int num28 = 0;
      Element.State state = Element.State.Vacuum;
      string str10 = (string) null;
      string str11 = (string) null;
      string str12 = (string) null;
      ElementComposition[] elementCompositionArray = (ElementComposition[]) null;
      string str13 = (string) null;
      while (!parser.End && parser.CurrentEventType != ParseEventType.MappingEnd)
      {
        if (parser.CurrentEventType != ParseEventType.Scalar)
          throw new YamlSerializerException(parser.CurrentMark, "Custom type deserialization supports only string key");
        ReadOnlySpan<byte> span;
        if (!parser.TryGetScalarAsSpan(out span))
          throw new YamlSerializerException(parser.CurrentMark, "Custom type deserialization supports only string key");
        if (context.Options.NamingConvention != NamingConvention.LowerCamelCase)
        {
          byte[] threadStaticBuffer;
          int written;
          NamingConventionMutator.MutateToThreadStaticBufferUtf8(span, NamingConvention.LowerCamelCase, out threadStaticBuffer, out written);
          span = Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(threadStaticBuffer, 0, written));
        }
        switch (span.Length)
        {
          case 4:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.tagsKeyUtf8Bytes)))
            {
              parser.Read();
              strArray = context.DeserializeWithAlias<string[]>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.flowKeyUtf8Bytes)))
            {
              parser.Read();
              num27 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            break;
          case 5:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.speedKeyUtf8Bytes)))
            {
              parser.Read();
              num24 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.stateKeyUtf8Bytes)))
            {
              parser.Read();
              state = context.DeserializeWithAlias<Element.State>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.dlcIdKeyUtf8Bytes)))
            {
              parser.Read();
              str11 = context.DeserializeWithAlias<string>(ref parser);
              continue;
            }
            break;
          case 7:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lowTempKeyUtf8Bytes)))
            {
              parser.Read();
              nullable1 = context.DeserializeWithAlias<float?>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.maxMassKeyUtf8Bytes)))
            {
              parser.Read();
              num20 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            break;
          case 8:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.highTempKeyUtf8Bytes)))
            {
              parser.Read();
              nullable2 = context.DeserializeWithAlias<float?>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.strengthKeyUtf8Bytes)))
            {
              parser.Read();
              num19 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.hardnessKeyUtf8Bytes)))
            {
              parser.Read();
              num21 = context.DeserializeWithAlias<byte>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.toxicityKeyUtf8Bytes)))
            {
              parser.Read();
              num22 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            break;
          case 9:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.elementIdKeyUtf8Bytes)))
            {
              parser.Read();
              str1 = context.DeserializeWithAlias<string>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.molarMassKeyUtf8Bytes)))
            {
              parser.Read();
              num9 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.convertIdKeyUtf8Bytes)))
            {
              parser.Read();
              str9 = context.DeserializeWithAlias<string>(ref parser);
              continue;
            }
            break;
          case 10:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.isDisabledKeyUtf8Bytes)))
            {
              parser.Read();
              flag = context.DeserializeWithAlias<bool>(ref parser);
              continue;
            }
            break;
          case 11:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.defaultMassKeyUtf8Bytes)))
            {
              parser.Read();
              num6 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateIdKeyUtf8Bytes)))
            {
              parser.Read();
              str6 = context.DeserializeWithAlias<string>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateFxKeyUtf8Bytes)))
            {
              parser.Read();
              str7 = context.DeserializeWithAlias<string>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.compositionKeyUtf8Bytes)))
            {
              parser.Read();
              elementCompositionArray = context.DeserializeWithAlias<ElementComposition[]>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.descriptionKeyUtf8Bytes)))
            {
              parser.Read();
              str13 = context.DeserializeWithAlias<string>(ref parser);
              continue;
            }
            break;
          case 13:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateRateKeyUtf8Bytes)))
            {
              parser.Read();
              num15 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.buildMenuSortKeyUtf8Bytes)))
            {
              parser.Read();
              num28 = context.DeserializeWithAlias<int>(ref parser);
              continue;
            }
            break;
          case 14:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.localizationIDKeyUtf8Bytes)))
            {
              parser.Read();
              str10 = context.DeserializeWithAlias<string>(ref parser);
              continue;
            }
            break;
          case 15:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.defaultPressureKeyUtf8Bytes)))
            {
              parser.Read();
              num8 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.minVerticalFlowKeyUtf8Bytes)))
            {
              parser.Read();
              num26 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            break;
          case 16 /*0x10*/:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.offGasPercentageKeyUtf8Bytes)))
            {
              parser.Read();
              num18 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.materialCategoryKeyUtf8Bytes)))
            {
              parser.Read();
              str8 = context.DeserializeWithAlias<string>(ref parser);
              continue;
            }
            break;
          case 17:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.liquidCompressionKeyUtf8Bytes)))
            {
              parser.Read();
              num23 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.minHorizontalFlowKeyUtf8Bytes)))
            {
              parser.Read();
              num25 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            break;
          case 18:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.defaultTemperatureKeyUtf8Bytes)))
            {
              parser.Read();
              num7 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.refinedMetalTargetKeyUtf8Bytes)))
            {
              parser.Read();
              str12 = context.DeserializeWithAlias<string>(ref parser);
              continue;
            }
            break;
          case 19:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.thermalConductivityKeyUtf8Bytes)))
            {
              parser.Read();
              num2 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateEfficiencyKeyUtf8Bytes)))
            {
              parser.Read();
              num16 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            break;
          case 20:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.specificHeatCapacityKeyUtf8Bytes)))
            {
              parser.Read();
              num1 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.radiationPer1000MassKeyUtf8Bytes)))
            {
              parser.Read();
              num12 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.sublimateProbabilityKeyUtf8Bytes)))
            {
              parser.Read();
              num17 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            break;
          case 21:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lightAbsorptionFactorKeyUtf8Bytes)))
            {
              parser.Read();
              num10 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            break;
          case 22:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lowTempTransitionOreIdKeyUtf8Bytes)))
            {
              parser.Read();
              str4 = context.DeserializeWithAlias<string>(ref parser);
              continue;
            }
            break;
          case 23:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lowTempTransitionTargetKeyUtf8Bytes)))
            {
              parser.Read();
              str2 = context.DeserializeWithAlias<string>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.highTempTransitionOreIdKeyUtf8Bytes)))
            {
              parser.Read();
              str5 = context.DeserializeWithAlias<string>(ref parser);
              continue;
            }
            break;
          case 24:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.gasSurfaceAreaMultiplierKeyUtf8Bytes)))
            {
              parser.Read();
              num5 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.highTempTransitionTargetKeyUtf8Bytes)))
            {
              parser.Read();
              str3 = context.DeserializeWithAlias<string>(ref parser);
              continue;
            }
            break;
          case 25:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.radiationAbsorptionFactorKeyUtf8Bytes)))
            {
              parser.Read();
              num11 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            break;
          case 26:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.solidSurfaceAreaMultiplierKeyUtf8Bytes)))
            {
              parser.Read();
              num3 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            break;
          case 27:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.liquidSurfaceAreaMultiplierKeyUtf8Bytes)))
            {
              parser.Read();
              num4 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            break;
          case 34:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.lowTempTransitionOreMassConversionKeyUtf8Bytes)))
            {
              parser.Read();
              num13 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            break;
          case 35:
            if (MemoryExtensions.SequenceEqual<byte>(span, ReadOnlySpan<byte>.op_Implicit(ElementEntry.ElementEntryGeneratedFormatter.highTempTransitionOreMassConversionKeyUtf8Bytes)))
            {
              parser.Read();
              num14 = context.DeserializeWithAlias<float>(ref parser);
              continue;
            }
            break;
        }
        parser.Read();
        parser.SkipCurrentNode();
      }
      parser.ReadWithVerify(ParseEventType.MappingEnd);
      return new ElementEntry()
      {
        elementId = str1,
        specificHeatCapacity = num1,
        thermalConductivity = num2,
        solidSurfaceAreaMultiplier = num3,
        liquidSurfaceAreaMultiplier = num4,
        gasSurfaceAreaMultiplier = num5,
        defaultMass = num6,
        defaultTemperature = num7,
        defaultPressure = num8,
        molarMass = num9,
        lightAbsorptionFactor = num10,
        radiationAbsorptionFactor = num11,
        radiationPer1000Mass = num12,
        lowTempTransitionTarget = str2,
        lowTemp = nullable1,
        highTempTransitionTarget = str3,
        highTemp = nullable2,
        lowTempTransitionOreId = str4,
        lowTempTransitionOreMassConversion = num13,
        highTempTransitionOreId = str5,
        highTempTransitionOreMassConversion = num14,
        sublimateId = str6,
        sublimateFx = str7,
        sublimateRate = num15,
        sublimateEfficiency = num16,
        sublimateProbability = num17,
        offGasPercentage = num18,
        materialCategory = str8,
        tags = strArray,
        isDisabled = flag,
        strength = num19,
        maxMass = num20,
        hardness = num21,
        toxicity = num22,
        liquidCompression = num23,
        speed = num24,
        minHorizontalFlow = num25,
        minVerticalFlow = num26,
        convertId = str9,
        flow = num27,
        buildMenuSort = num28,
        state = state,
        localizationID = str10,
        dlcId = str11,
        refinedMetalTarget = str12,
        composition = elementCompositionArray,
        description = str13
      };
    }
  }
}
