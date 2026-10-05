// Decompiled with JetBrains decompiler
// Type: ManualCodexConversionRegistry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class ManualCodexConversionRegistry
{
  public static Dictionary<Tag, List<ManualCodexConversionRegistry.ManualConversionEntry>> conversionsByTag = new Dictionary<Tag, List<ManualCodexConversionRegistry.ManualConversionEntry>>();

  public static List<ManualCodexConversionRegistry.ManualConversionEntry> GetConversionsForGivenConverter(
    Tag converter)
  {
    if (!ManualCodexConversionRegistry.conversionsByTag.ContainsKey(converter))
      return (List<ManualCodexConversionRegistry.ManualConversionEntry>) null;
    List<ManualCodexConversionRegistry.ManualConversionEntry> forGivenConverter = new List<ManualCodexConversionRegistry.ManualConversionEntry>();
    foreach (ManualCodexConversionRegistry.ManualConversionEntry manualConversionEntry in ManualCodexConversionRegistry.conversionsByTag[converter])
    {
      if (manualConversionEntry.converter != null && manualConversionEntry.converter.first == converter)
        forGivenConverter.Add(manualConversionEntry);
    }
    return forGivenConverter;
  }

  public static List<ManualCodexConversionRegistry.ManualConversionEntry> GetProducersForGivenOutput(
    Tag output)
  {
    if (!ManualCodexConversionRegistry.conversionsByTag.ContainsKey(output))
      return (List<ManualCodexConversionRegistry.ManualConversionEntry>) null;
    List<ManualCodexConversionRegistry.ManualConversionEntry> producersForGivenOutput = new List<ManualCodexConversionRegistry.ManualConversionEntry>();
    foreach (ManualCodexConversionRegistry.ManualConversionEntry manualConversionEntry in ManualCodexConversionRegistry.conversionsByTag[output])
    {
      if (manualConversionEntry.output != null && manualConversionEntry.output.first == output)
        producersForGivenOutput.Add(manualConversionEntry);
    }
    return producersForGivenOutput;
  }

  public static List<ManualCodexConversionRegistry.ManualConversionEntry> GetConsumersForGivenInput(
    Tag input)
  {
    if (!ManualCodexConversionRegistry.conversionsByTag.ContainsKey(input))
      return (List<ManualCodexConversionRegistry.ManualConversionEntry>) null;
    List<ManualCodexConversionRegistry.ManualConversionEntry> consumersForGivenInput = new List<ManualCodexConversionRegistry.ManualConversionEntry>();
    foreach (ManualCodexConversionRegistry.ManualConversionEntry manualConversionEntry in ManualCodexConversionRegistry.conversionsByTag[input])
    {
      if (manualConversionEntry.input != null && manualConversionEntry.input.first == input)
        consumersForGivenInput.Add(manualConversionEntry);
    }
    return consumersForGivenInput;
  }

  public static void AddConversion(
    Tag inputTag,
    float inputAmount,
    Tag converterTag,
    float converterAmount,
    Tag outputTag,
    float outputAmount,
    string headerDescription,
    Func<Tag, float, bool, string> inputCustomFormating = null,
    Func<Tag, float, bool, string> converterCustomFormating = null,
    Func<Tag, float, bool, string> outputCustomFormating = null)
  {
    ManualCodexConversionRegistry.ManualConversionEntry manualConversionEntry = new ManualCodexConversionRegistry.ManualConversionEntry(new Tuple<Tag, float>(converterTag, converterAmount), new Tuple<Tag, float>(inputTag, inputAmount), new Tuple<Tag, float>(outputTag, outputAmount), headerDescription, inputCustomFormating, converterCustomFormating, outputCustomFormating);
    if (converterTag != (Tag) (string) null)
    {
      List<ManualCodexConversionRegistry.ManualConversionEntry> manualConversionEntryList;
      if (!ManualCodexConversionRegistry.conversionsByTag.TryGetValue(converterTag, out manualConversionEntryList))
      {
        manualConversionEntryList = new List<ManualCodexConversionRegistry.ManualConversionEntry>();
        ManualCodexConversionRegistry.conversionsByTag[converterTag] = manualConversionEntryList;
      }
      ManualCodexConversionRegistry.conversionsByTag[converterTag].Add(manualConversionEntry);
    }
    if (inputTag != (Tag) (string) null)
    {
      List<ManualCodexConversionRegistry.ManualConversionEntry> manualConversionEntryList;
      if (!ManualCodexConversionRegistry.conversionsByTag.TryGetValue(inputTag, out manualConversionEntryList))
      {
        manualConversionEntryList = new List<ManualCodexConversionRegistry.ManualConversionEntry>();
        ManualCodexConversionRegistry.conversionsByTag[inputTag] = manualConversionEntryList;
      }
      ManualCodexConversionRegistry.conversionsByTag[inputTag].Add(manualConversionEntry);
    }
    if (!(outputTag != (Tag) (string) null))
      return;
    List<ManualCodexConversionRegistry.ManualConversionEntry> manualConversionEntryList1;
    if (!ManualCodexConversionRegistry.conversionsByTag.TryGetValue(outputTag, out manualConversionEntryList1))
    {
      manualConversionEntryList1 = new List<ManualCodexConversionRegistry.ManualConversionEntry>();
      ManualCodexConversionRegistry.conversionsByTag[outputTag] = manualConversionEntryList1;
    }
    ManualCodexConversionRegistry.conversionsByTag[outputTag].Add(manualConversionEntry);
  }

  public class ManualConversionEntry
  {
    public string headerDescription;
    public Tuple<Tag, float> converter;
    public Tuple<Tag, float> input;
    public Tuple<Tag, float> output;
    public Func<Tag, float, bool, string> inputCustomFormating;
    public Func<Tag, float, bool, string> converterCustomFormating;
    public Func<Tag, float, bool, string> outputCustomFormating;

    public ManualConversionEntry(
      Tuple<Tag, float> converter,
      Tuple<Tag, float> input,
      Tuple<Tag, float> output,
      string headerDescription,
      Func<Tag, float, bool, string> inputCustomFormating = null,
      Func<Tag, float, bool, string> converterCustomFormating = null,
      Func<Tag, float, bool, string> outputCustomFormating = null)
    {
      this.converter = converter;
      this.input = input;
      this.output = output;
      this.headerDescription = headerDescription;
      this.inputCustomFormating = inputCustomFormating;
      this.converterCustomFormating = converterCustomFormating;
      this.outputCustomFormating = outputCustomFormating;
    }
  }
}
