// Decompiled with JetBrains decompiler
// Type: ElementLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ElementData;
using Klei;
using ProcGenGame;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class ElementLoader
{
  public static List<Element> elements;
  public static Dictionary<int, Element> elementTable;
  public static Dictionary<Tag, Element> elementTagTable;
  private static string path = Application.streamingAssetsPath + "/elements/";
  private static readonly Color noColour = new Color(0.0f, 0.0f, 0.0f, 0.0f);

  public static float GetMinMeltingPointAmongElements(IList<Tag> elements)
  {
    float a = float.MaxValue;
    for (int index = 0; index < elements.Count; ++index)
    {
      Element element = ElementLoader.GetElement(elements[index]);
      if (element != null)
        a = Mathf.Min(a, element.highTemp);
    }
    return a;
  }

  public static List<ElementEntry> CollectElementsFromYAML()
  {
    List<ElementEntry> elementEntryList = new List<ElementEntry>();
    ListPool<FileHandle, ElementLoader>.PooledList result1 = ListPool<FileHandle, ElementLoader>.Allocate();
    FileSystem.GetFiles(FileSystem.Normalize(ElementLoader.path), "*.yaml", (ICollection<FileHandle>) result1);
    ListPool<YamlIO.Error, ElementLoader>.PooledList errors = ListPool<YamlIO.Error, ElementLoader>.Allocate();
    foreach (FileHandle fileHandle in (List<FileHandle>) result1)
    {
      FileHandle file = fileHandle;
      ElementEntryCollection result2;
      if (!System.IO.Path.GetFileName(file.full_path).StartsWith(".") && KYaml.LoadFile<ElementEntryCollection>(file, out result2, (KYaml.ErrorHandler) ((path, exception) => errors.Add(new YamlIO.Error()
      {
        file = file,
        message = exception.Message,
        inner_exception = exception.InnerException,
        severity = YamlIO.Error.Severity.Fatal
      }))))
        elementEntryList.AddRange((IEnumerable<ElementEntry>) result2.elements);
    }
    result1.Recycle();
    if ((UnityEngine.Object) Global.Instance != (UnityEngine.Object) null && Global.Instance.modManager != null)
      Global.Instance.modManager.HandleErrors((List<YamlIO.Error>) errors);
    errors.Recycle();
    return elementEntryList;
  }

  public static void Load(
    ref Hashtable substanceList,
    Dictionary<string, SubstanceTable> substanceTablesByDlc)
  {
    ElementLoader.elements = new List<Element>();
    ElementLoader.elementTable = new Dictionary<int, Element>();
    ElementLoader.elementTagTable = new Dictionary<Tag, Element>();
    foreach (ElementEntry entry in ElementLoader.CollectElementsFromYAML())
    {
      int key = Hash.SDBMLower(entry.elementId);
      if (!ElementLoader.elementTable.ContainsKey(key) && substanceTablesByDlc.ContainsKey(entry.dlcId))
      {
        Element elem = new Element()
        {
          id = (SimHashes) key,
          name = (string) Strings.Get(entry.localizationID)
        };
        elem.nameUpperCase = elem.name.ToUpper();
        elem.description = (string) Strings.Get(entry.description);
        elem.tag = TagManager.Create(entry.elementId, elem.name);
        ElementLoader.CopyEntryToElement(entry, elem);
        ElementLoader.elements.Add(elem);
        ElementLoader.elementTable[key] = elem;
        ElementLoader.elementTagTable[elem.tag] = elem;
        if (!ElementLoader.ManifestSubstanceForElement(elem, ref substanceList, substanceTablesByDlc[entry.dlcId]))
          Debug.LogWarning((object) ("Missing substance for element: " + elem.id.ToString()));
      }
    }
    ElementLoader.FinaliseElementsTable(ref substanceList);
    WorldGen.SetupDefaultElements();
  }

  private static void CopyEntryToElement(ElementEntry entry, Element elem)
  {
    Hash.SDBMLower(entry.elementId);
    elem.tag = TagManager.Create(entry.elementId.ToString());
    elem.specificHeatCapacity = entry.specificHeatCapacity;
    elem.thermalConductivity = entry.thermalConductivity;
    elem.molarMass = entry.molarMass;
    elem.strength = entry.strength;
    elem.disabled = entry.isDisabled;
    elem.dlcId = entry.dlcId;
    elem.flow = entry.flow;
    elem.maxMass = entry.maxMass;
    elem.maxCompression = entry.liquidCompression;
    elem.viscosity = entry.speed;
    elem.minHorizontalFlow = entry.minHorizontalFlow;
    elem.minVerticalFlow = entry.minVerticalFlow;
    elem.solidSurfaceAreaMultiplier = entry.solidSurfaceAreaMultiplier;
    elem.liquidSurfaceAreaMultiplier = entry.liquidSurfaceAreaMultiplier;
    elem.gasSurfaceAreaMultiplier = entry.gasSurfaceAreaMultiplier;
    elem.state = entry.state;
    elem.hardness = entry.hardness;
    Element element1 = elem;
    float? nullable;
    double num1;
    if (!entry.lowTemp.HasValue)
    {
      num1 = 0.0;
    }
    else
    {
      nullable = entry.lowTemp;
      num1 = (double) nullable.Value;
    }
    element1.lowTemp = (float) num1;
    elem.lowTempTransitionTarget = (SimHashes) Hash.SDBMLower(entry.lowTempTransitionTarget);
    Element element2 = elem;
    nullable = entry.highTemp;
    double num2;
    if (!nullable.HasValue)
    {
      num2 = 10000.0;
    }
    else
    {
      nullable = entry.highTemp;
      num2 = (double) nullable.Value;
    }
    element2.highTemp = (float) num2;
    elem.highTempTransitionTarget = (SimHashes) Hash.SDBMLower(entry.highTempTransitionTarget);
    elem.highTempTransitionOreID = (SimHashes) Hash.SDBMLower(entry.highTempTransitionOreId);
    elem.highTempTransitionOreMassConversion = entry.highTempTransitionOreMassConversion;
    elem.lowTempTransitionOreID = (SimHashes) Hash.SDBMLower(entry.lowTempTransitionOreId);
    elem.lowTempTransitionOreMassConversion = entry.lowTempTransitionOreMassConversion;
    elem.refinedMetalTarget = (SimHashes) Hash.SDBMLower(entry.refinedMetalTarget);
    elem.sublimateId = (SimHashes) Hash.SDBMLower(entry.sublimateId);
    elem.convertId = (SimHashes) Hash.SDBMLower(entry.convertId);
    elem.sublimateFX = (SpawnFXHashes) Hash.SDBMLower(entry.sublimateFx);
    elem.sublimateRate = entry.sublimateRate;
    elem.sublimateEfficiency = entry.sublimateEfficiency;
    elem.sublimateProbability = entry.sublimateProbability;
    elem.offGasPercentage = entry.offGasPercentage;
    elem.lightAbsorptionFactor = entry.lightAbsorptionFactor;
    elem.radiationAbsorptionFactor = entry.radiationAbsorptionFactor;
    elem.radiationPer1000Mass = entry.radiationPer1000Mass;
    elem.toxicity = entry.toxicity;
    elem.elementComposition = entry.composition;
    Tag phaseTag = TagManager.Create(entry.state.ToString());
    elem.materialCategory = ElementLoader.CreateMaterialCategoryTag(elem.id, phaseTag, entry.materialCategory);
    elem.oreTags = ElementLoader.CreateOreTags(elem.materialCategory, phaseTag, entry.tags);
    elem.buildMenuSort = entry.buildMenuSort;
    Sim.PhysicsData physicsData = new Sim.PhysicsData();
    physicsData.temperature = entry.defaultTemperature;
    physicsData.mass = entry.defaultMass;
    physicsData.pressure = entry.defaultPressure;
    switch (entry.state)
    {
      case Element.State.Gas:
        GameTags.GasElements.Add(elem.tag);
        physicsData.mass = 1f;
        elem.maxMass = 1.8f;
        break;
      case Element.State.Liquid:
        GameTags.LiquidElements.Add(elem.tag);
        break;
      case Element.State.Solid:
        GameTags.SolidElements.Add(elem.tag);
        break;
    }
    elem.defaultValues = physicsData;
  }

  private static bool ManifestSubstanceForElement(
    Element elem,
    ref Hashtable substanceList,
    SubstanceTable substanceTable)
  {
    elem.substance = (Substance) null;
    if (substanceList.ContainsKey((object) elem.id))
    {
      elem.substance = substanceList[(object) elem.id] as Substance;
      return false;
    }
    if ((UnityEngine.Object) substanceTable != (UnityEngine.Object) null)
      elem.substance = substanceTable.GetSubstance(elem.id);
    if (elem.substance == null)
    {
      elem.substance = new Substance();
      substanceTable.GetList().Add(elem.substance);
    }
    elem.substance.elementID = elem.id;
    elem.substance.renderedByWorld = elem.IsSolid;
    elem.substance.idx = substanceList.Count;
    if ((Color) elem.substance.uiColour == ElementLoader.noColour)
    {
      int count = ElementLoader.elements.Count;
      int idx = elem.substance.idx;
      elem.substance.uiColour = (Color32) Color.HSVToRGB((float) idx / (float) count, 1f, 1f);
    }
    string str = UI.StripLinkFormatting(elem.name);
    elem.substance.name = str;
    elem.substance.nameTag = elem.tag;
    elem.substance.audioConfig = ElementsAudio.Instance.GetConfigForElement(elem.id);
    substanceList.Add((object) elem.id, (object) elem.substance);
    return true;
  }

  public static Element FindElementByName(string name)
  {
    return ElementLoader.FindElementByHash((SimHashes) Hash.SDBMLower(name));
  }

  public static Element FindElementByTag(Tag tag) => ElementLoader.GetElement(tag);

  public static List<Element> FindElements(Func<Element, bool> filter)
  {
    List<Element> elements = new List<Element>();
    foreach (int key in ElementLoader.elementTable.Keys)
    {
      Element element = ElementLoader.elementTable[key];
      if (filter(element))
        elements.Add(element);
    }
    return elements;
  }

  public static Element FindElementByHash(SimHashes hash)
  {
    Element elementByHash = (Element) null;
    ElementLoader.elementTable.TryGetValue((int) hash, out elementByHash);
    return elementByHash;
  }

  public static ushort GetElementIndex(SimHashes hash)
  {
    Element element = (Element) null;
    ElementLoader.elementTable.TryGetValue((int) hash, out element);
    return element != null ? element.idx : ushort.MaxValue;
  }

  public static Element GetElement(Tag tag)
  {
    Element element;
    ElementLoader.elementTagTable.TryGetValue(tag, out element);
    return element;
  }

  public static SimHashes GetElementID(Tag tag)
  {
    Element element;
    ElementLoader.elementTagTable.TryGetValue(tag, out element);
    return element != null ? element.id : SimHashes.Vacuum;
  }

  private static SimHashes GetID(int column, int row, string[,] grid, SimHashes defaultValue = SimHashes.Vacuum)
  {
    if (column >= grid.GetLength(0) || row > grid.GetLength(1))
    {
      Debug.LogError((object) $"Could not find element at loc [{column},{row}] grid is only [{grid.GetLength(0)},{grid.GetLength(1)}]");
      return defaultValue;
    }
    string str = grid[column, row];
    if (str == null || str == "")
      return defaultValue;
    object id;
    try
    {
      id = Enum.Parse(typeof (SimHashes), str);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) $"Could not find element {str}: {ex.ToString()}");
      return defaultValue;
    }
    return (SimHashes) id;
  }

  private static SpawnFXHashes GetSpawnFX(int column, int row, string[,] grid)
  {
    if (column >= grid.GetLength(0) || row > grid.GetLength(1))
    {
      Debug.LogError((object) $"Could not find SpawnFXHashes at loc [{column},{row}] grid is only [{grid.GetLength(0)},{grid.GetLength(1)}]");
      return SpawnFXHashes.None;
    }
    string str = grid[column, row];
    if (str == null || str == "")
      return SpawnFXHashes.None;
    object spawnFx;
    try
    {
      spawnFx = Enum.Parse(typeof (SpawnFXHashes), str);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) $"Could not find FX {str}: {ex.ToString()}");
      return SpawnFXHashes.None;
    }
    return (SpawnFXHashes) spawnFx;
  }

  private static Tag CreateMaterialCategoryTag(
    SimHashes element_id,
    Tag phaseTag,
    string materialCategoryField)
  {
    if (string.IsNullOrEmpty(materialCategoryField))
      return phaseTag;
    Tag materialCategoryTag = TagManager.Create(materialCategoryField);
    if (!GameTags.MaterialCategories.Contains(materialCategoryTag) && !GameTags.IgnoredMaterialCategories.Contains(materialCategoryTag))
      Debug.LogWarningFormat("Element {0} has category {1}, but that isn't in GameTags.MaterialCategores!", (object) element_id, (object) materialCategoryField);
    return materialCategoryTag;
  }

  private static Tag[] CreateOreTags(Tag materialCategory, Tag phaseTag, string[] ore_tags_split)
  {
    List<Tag> tagList = new List<Tag>();
    if (ore_tags_split != null)
    {
      foreach (string tag_string in ore_tags_split)
      {
        if (!string.IsNullOrEmpty(tag_string))
          tagList.Add(TagManager.Create(tag_string));
      }
    }
    tagList.Add(phaseTag);
    if (materialCategory.IsValid && !tagList.Contains(materialCategory))
      tagList.Add(materialCategory);
    return tagList.ToArray();
  }

  private static void FinaliseElementsTable(ref Hashtable substanceList)
  {
    foreach (Element element in ElementLoader.elements)
    {
      if (element != null)
      {
        if (element.substance == null)
        {
          Debug.LogWarning((object) ("Skipping finalise for missing element: " + element.id.ToString()));
        }
        else
        {
          Debug.Assert(element.substance.nameTag.IsValid);
          if ((double) element.thermalConductivity == 0.0)
            element.state |= Element.State.TemperatureInsulated;
          if ((double) element.strength == 0.0)
            element.state |= Element.State.Unbreakable;
          if (element.IsSolid || element.IsLiquid || element.IsGas)
          {
            Element elementByHash1 = ElementLoader.FindElementByHash(element.highTempTransitionTarget);
            if (elementByHash1 != null)
              element.highTempTransition = elementByHash1;
            Element elementByHash2 = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);
            if (elementByHash2 != null)
              element.lowTempTransition = elementByHash2;
          }
        }
      }
    }
    ElementLoader.elements = ElementLoader.elements.OrderByDescending<Element, int>((Func<Element, int>) (e => (int) (e.state & Element.State.Solid))).ThenBy<Element, SimHashes>((Func<Element, SimHashes>) (e => e.id)).ToList<Element>();
    for (int index = 0; index < ElementLoader.elements.Count; ++index)
    {
      if (ElementLoader.elements[index].substance != null)
        ElementLoader.elements[index].substance.idx = index;
      ElementLoader.elements[index].idx = (ushort) index;
    }
  }

  private static void ValidateElements()
  {
    Debug.Log((object) "------ Start Validating Elements ------");
    foreach (Element element in ElementLoader.elements)
    {
      string str = $"{element.tag.ProperNameStripLink()} ({element.state})";
      if (element.IsLiquid && element.sublimateId != (SimHashes) 0)
      {
        Debug.Assert((double) element.sublimateRate == 0.0, (object) (str + ": Liquids don't use sublimateRate, use offGasPercentage instead."));
        Debug.Assert((double) element.offGasPercentage > 0.0, (object) (str + ": Missing offGasPercentage"));
      }
      if (element.IsSolid && element.sublimateId != (SimHashes) 0)
      {
        Debug.Assert((double) element.offGasPercentage == 0.0, (object) (str + ": Solids don't use offGasPercentage, use sublimateRate instead."));
        Debug.Assert((double) element.sublimateRate > 0.0, (object) (str + ": Missing sublimationRate"));
        Debug.Assert((double) element.sublimateRate * (double) element.sublimateEfficiency > 1.0 / 1000.0, (object) (str + ": Sublimation rate and efficiency will result in gas that will be obliterated because its less than 1g. Increase these values and use sublimateProbability if you want a low amount of sublimation"));
      }
      if (element.highTempTransition != null && element.highTempTransition.lowTempTransition == element)
        Debug.Assert((double) element.highTemp >= (double) element.highTempTransition.lowTemp, (object) $"{str}: highTemp is higher than transition element's ({element.highTempTransition.tag.ProperNameStripLink()}) lowTemp");
      Debug.Assert((double) element.defaultValues.mass <= (double) element.maxMass, (object) (str + ": Default mass should be less than max mass"));
      if (false)
      {
        if (element.IsSolid && element.highTempTransition != null && element.highTempTransition.IsLiquid && (double) element.defaultValues.mass > (double) element.highTempTransition.maxMass)
          Debug.LogWarning((object) $"{str} defaultMass {element.defaultValues.mass} > {element.highTempTransition.tag.ProperNameStripLink()}: maxMass {element.highTempTransition.maxMass}");
        if ((double) element.defaultValues.mass < (double) element.maxMass && element.IsLiquid)
          Debug.LogWarning((object) $"{element.tag.ProperNameStripLink()} has defaultMass: {element.defaultValues.mass} and maxMass {element.maxMass}");
      }
    }
    Debug.Log((object) "------ End Validating Elements ------");
  }
}
